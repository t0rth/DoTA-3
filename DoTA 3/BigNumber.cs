using System;
using System.Text;

namespace DoTA_3
{
    public class BigNumber
    {
        private const int Base = 1000;
        private int[] number;

        public BigNumber(string str)
        {
            // Убираем возможные пробелы на всякий случай
            str = str.Trim();

            // Сколько блоков получится? Округляем вверх: (длина + 2) / 3
            int count = (str.Length + 2) / 3;
            number = new int[count];

            // Идём с конца строки, заполняем массив с number[0]
            int pos = str.Length;   // позиция "после последнего символа"
            int blockIndex = 0;

            while (pos > 0)
            {
                // Берём 3 символа слева от pos, но не выходим за начало строки
                int start = Math.Max(0, pos - 3);
                int len = pos - start;

                string blockStr = str.Substring(start, len);
                number[blockIndex] = int.Parse(blockStr);

                pos -= 3;
                blockIndex++;
            }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            // Идём с последнего индекса (старший блок) к нулевому
            for (int i = number.Length - 1; i >= 0; i--)
            {
                if (i == number.Length - 1)
                    sb.Append(number[i].ToString());      // старший — как есть
                else
                    sb.Append(number[i].ToString("D3"));  // остальные — с нулями
            }

            return sb.ToString();
        }
        private BigNumber Add(BigNumber other)
        {
            // Максимальная длина: у большего из двух + 1 на случай переноса
            int maxLen = Math.Max(this.number.Length, other.number.Length) + 1;
            int[] result = new int[maxLen];

            int carry = 0;   // перенос

            for (int i = 0; i < maxLen; i++)
            {
                // Берём блок из первого числа, если он есть, иначе 0
                int a = (i < this.number.Length) ? this.number[i] : 0;
                // То же для второго
                int b = (i < other.number.Length) ? other.number[i] : 0;

                int sum = a + b + carry;
                result[i] = sum % Base;   // что записываем в этот разряд
                carry = sum / Base;       // что переносим в следующий
            }

            // Создаём BigNumber из массива result и убираем лишние нули
            return new BigNumber(result);
        }
        private BigNumber(int[] blocks)
        {
            number = TrimLeadingZeros(blocks);
        }
        private static int[] TrimLeadingZeros(int[] arr)
        {
            int lastNonZero = arr.Length - 1;

            // Идём с конца, пока не найдём ненулевой блок
            while (lastNonZero > 0 && arr[lastNonZero] == 0)
                lastNonZero--;

            // Копируем только значимую часть
            int[] trimmed = new int[lastNonZero + 1];
            Array.Copy(arr, trimmed, lastNonZero + 1);

            return trimmed;
        }
        public static BigNumber operator +(BigNumber a, BigNumber b)
        {
            return a.Add(b);
        }
        private BigNumber Subtract(BigNumber other)
        {
            // Проверка: a < b — вычитание "в минус" не поддерживаем
            if (this.CompareTo(other) < 0)
                throw new InvalidOperationException("Результат вычитания отрицателен");

            int maxLen = this.number.Length;   // результат не длиннее a
            int[] result = new int[maxLen];

            int borrow = 0;   // заём

            for (int i = 0; i < maxLen; i++)
            {
                int a = this.number[i];
                int b = (i < other.number.Length) ? other.number[i] : 0;

                int diff = a - b - borrow;

                if (diff < 0)
                {
                    diff += Base;   // добавляем 1000 "взятых" у старшего
                    borrow = 1;
                }
                else
                {
                    borrow = 0;
                }

                result[i] = diff;
            }

            return new BigNumber(result);
        }
        public int CompareTo(BigNumber other)
        {
            // Сначала — по длине
            if (this.number.Length != other.number.Length)
                return this.number.Length.CompareTo(other.number.Length);

            // Одинаковая длина — сравниваем с конца (от старшего блока)
            for (int i = this.number.Length - 1; i >= 0; i--)
            {
                if (this.number[i] != other.number[i])
                    return this.number[i].CompareTo(other.number[i]);
            }

            return 0;   // равны
        }
        public static BigNumber operator -(BigNumber a, BigNumber b)
        {
            return a.Subtract(b);
        }

        public static bool operator >(BigNumber a, BigNumber b)
        {
            return a.CompareTo(b) > 0;
        }

        public static bool operator <(BigNumber a, BigNumber b)
        {
            return a.CompareTo(b) < 0;
        }
        private BigNumber Multiply(double multiplier)
        {
            // Сначала оценим, сколько блоков понадобится.
            // Обычно хватает длины исходного числа + 1 (перенос может добавить блок).
            // Но при multiplier > 1000 может понадобиться больше — поэтому возьмём с запасом.
            int maxLen = this.number.Length + 2;
            int[] result = new int[maxLen];

            double carry = 0;

            for (int i = 0; i < this.number.Length; i++)
            {
                double product = this.number[i] * multiplier + carry;

                // Округляем вниз до целого — так избегаем накопления ошибок
                product = Math.Floor(product);

                result[i] = (int)(product % Base);
                carry = Math.Floor(product / Base);
            }

            // Дописываем оставшийся перенос
            int pos = this.number.Length;
            while (carry > 0 && pos < maxLen)
            {
                result[pos] = (int)(carry % Base);
                carry = Math.Floor(carry / Base);
                pos++;
            }

            return new BigNumber(result);
        }
        public static BigNumber operator *(BigNumber a, double b)
        {
            return a.Multiply(b);
        }
        private BigNumber Divide(double divisor)
        {
            if (divisor == 0)
                throw new DivideByZeroException("Деление на ноль");

            int[] result = new int[this.number.Length];

            double rest = 0;

            // Идём с конца (от старшего блока) к началу
            for (int i = this.number.Length - 1; i >= 0; i--)
            {
                // "Сносим" следующий блок: остаток * 1000 + текущий блок
                double current = rest * Base + this.number[i];

                // Целая часть от деления — в результат
                result[i] = (int)Math.Floor(current / divisor);

                // Остаток — на следующий шаг
                rest = current - result[i] * divisor;
            }

            // Отбрасываем финальный остаток (он не нужен)
            return new BigNumber(result);
        }
        public static BigNumber operator /(BigNumber a, double b)
        {
            return a.Divide(b);
        }
    }
}
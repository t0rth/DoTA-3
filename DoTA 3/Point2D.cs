using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoTA_3
{
    public class Point2D
    {
        //Свойства класса — чтение снаружи, изменение только изнутри класса
        public int X { get; private set; }
        public int Y { get; private set; }
        //Конструктор класса
        public Point2D(int x, int y)
        {
            X = x;
            Y = y;
        }
        //Методы для изменения координат (смещение на заданную величину,
        //а не просто присваивание — поэтому это методы, а не сеттер свойства)
        public void AddX(int x)
        {
            X += x;
        }
        public void AddY(int y)
        {
            Y += y;
        }
    }
}
namespace DoTA_3
{
    public class Player
    {
        // ---------- ПОЛЯ ----------
        private int lvl;
        private BigNumber gold;
        private BigNumber damage;
        private double damageModifier;
        private BigNumber upgradeCost;
        private double upgradeModifier;

        // ---------- СВОЙСТВА (только для чтения) ----------
        public int Lvl => lvl;
        public BigNumber Gold => gold;
        public BigNumber Damage => damage;
        public double DamageModifier => damageModifier;
        public BigNumber UpgradeCost => upgradeCost;
        public double UpgradeModifier => upgradeModifier;

        // ---------- КОНСТРУКТОР ----------
        public Player()
        {
            lvl = 1;
            gold = new BigNumber("0");
            damage = new BigNumber("5");          // стартовый урон — 5
            damageModifier = 1.2;
            upgradeCost = new BigNumber("10");
            upgradeModifier = 1.2;
        }

        // ---------- ПУБЛИЧНЫЕ МЕТОДЫ ----------

        // Добавить золото (например, за победу над врагом)
        public void AddGold(BigNumber amount)
        {
            gold = gold + amount;
        }

        // Попытка улучшить урон за золото
        public bool TryUpgrade()
        {
            if (!TrySpendGold(upgradeCost))
                return false;

            lvl++;
            damage = damage * damageModifier;
            upgradeCost = CalculateNextUpgradeCost();

            return true;
        }

        // Урон, который игрок наносит за клик
        public BigNumber DealDamage()
        {
            return damage;
        }

        // ---------- ПРИВАТНЫЕ МЕТОДЫ ----------

        // Попытка списать золото. true — если хватило
        private bool TrySpendGold(BigNumber amount)
        {
            if (gold.CompareTo(amount) < 0)
                return false;

            gold = gold - amount;
            return true;
        }

        // Стоимость следующего апгрейда
        private BigNumber CalculateNextUpgradeCost()
        {
            double factor = upgradeModifier * lvl;
            return upgradeCost * factor;
        }

        // Пересчёт характеристик (по UML — заготовка)
        private void RecalculateStats()
        {
            // Пока не используется — можно оставить пустым
        }

        // Общий урон (по UML — заготовка; пока совпадает с damage)
        private BigNumber CalculateTotalDamage()
        {
            return damage;
        }
    }
}

using System; // если нужно

namespace DoTA_3
{
    public class Player
    {
        private int lvl;
        private BigNumber gold;
        private BigNumber damage;
        private double damageModifier;
        private BigNumber upgradeCost;
        private double upgradeModifier;

        // НОВОЕ: Таймер перезарядки
        private CCountdownTimer cooldownTimer;

        public int Lvl => lvl;
        public BigNumber Gold => gold;
        public BigNumber Damage => damage;
        public double DamageModifier => damageModifier;
        public BigNumber UpgradeCost => upgradeCost;
        public double UpgradeModifier => upgradeModifier;
        public bool CanClick => cooldownTimer.IsReady; // НОВОЕ

        public Player()
        {
            lvl = 1;
            gold = new BigNumber("0");
            damage = new BigNumber("5");
            damageModifier = 1.2;
            upgradeCost = new BigNumber("10");
            upgradeModifier = 1.2;

            cooldownTimer = new CCountdownTimer(1.0); // Стартовый КД — 1 сек
        }

        public void Update(double delta)
        {
            cooldownTimer.Update(delta); // НОВОЕ
        }

        public void AddGold(BigNumber amount)
        {
            gold = gold + amount;
        }

        public bool TryUpgrade()
        {
            if (!TrySpendGold(upgradeCost)) return false;
            lvl++;
            damage = damage * damageModifier;
            upgradeCost = CalculateNextUpgradeCost();
            return true;
        }

        public BigNumber DealDamage()
        {
            return damage;
        }

        // НОВОЕ: Вызывается при клике
        public void onMouseClick()
        {
            if (CanClick)
            {
                cooldownTimer.Reset();
            }
        }

        // НОВОЕ: Уменьшение перезарядки
        public void DecreaseCooldown(double amount)
        {
            cooldownTimer.DecreaseTargetTime(amount);
        }

        // НОВОЕ: Увеличение урона от бонуса
        public void IncreaseDamage(double multiplier)
        {
            damage = damage * multiplier;
        }

        private bool TrySpendGold(BigNumber amount)
        {
            if (gold.CompareTo(amount) < 0) return false;
            gold = gold - amount;
            return true;
        }

        private BigNumber CalculateNextUpgradeCost()
        {
            double factor = upgradeModifier * lvl;
            return upgradeCost * factor;
        }
    }
}
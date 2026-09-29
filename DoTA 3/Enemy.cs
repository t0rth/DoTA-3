using System;

namespace DoTA_3
{
    public class Enemy
    {
        private string name;
        private BigNumber maxHitPoints;
        private BigNumber currentHitPoints;
        private BigNumber goldReward;
        private bool isDead;
        private EnemyIcon icon;

        public string Name => name;
        public BigNumber MaxHitPoints => maxHitPoints;
        public BigNumber CurrentHitPoints => currentHitPoints;
        public BigNumber GoldReward => goldReward;
        public bool IsDead => isDead;
        public EnemyIcon Icon => icon;

        public Enemy(CEnemyTemplate template, int level, EnemyIcon iconItem)
        {
            name = template.Name;
            icon = iconItem;

            // Масштабирование HP: BaseLife * (LifeModifier ^ level)
            BigNumber baseHP = new BigNumber(template.BaseLife.ToString());
            maxHitPoints = baseHP;
            for (int i = 0; i < level; i++)
                maxHitPoints = maxHitPoints * template.LifeModifier;

            currentHitPoints = maxHitPoints;
            isDead = false;

            // Масштабирование золота
            BigNumber baseGold = new BigNumber(template.BaseGold.ToString());
            goldReward = baseGold;
            for (int i = 0; i < level; i++)
                goldReward = goldReward * template.GoldModifier;
        }

        public bool TakeDamage(BigNumber dmg, out BigNumber reward)
        {
            reward = null;

            if (isDead) return false;

            // Если урон >= текущего HP — победа одним ударом
            if (dmg.CompareTo(currentHitPoints) >= 0)
            {
                currentHitPoints = new BigNumber("0");
                Die();
                reward = goldReward;
                return true;
            }

            // Иначе — вычитаем, HP точно останется > 0
            currentHitPoints = currentHitPoints - dmg;
            return false;
        }

        private void Die()
        {
            isDead = true;
        }
    }
}
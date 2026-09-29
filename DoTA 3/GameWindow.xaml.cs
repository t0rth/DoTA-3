using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace DoTA_3
{
    public partial class GameWindow : Window
    {
        Player player;
        Enemy currentEnemy;
        CEnemyTemplateList enemyList;
        List<EnemyIcon> enemyIcons = new List<EnemyIcon>();
        Random rnd = new Random();

        public GameWindow()
        {
            InitializeComponent();

            player = new Player();
            enemyList = new CEnemyTemplateList();

            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string jsonPath = Path.Combine(baseDir, "enemies.json");

            if (!File.Exists(jsonPath))
            {
                MessageBox.Show("Файл enemies.json не найден!");
                return;
            }

            enemyList.LoadFromJson(jsonPath);
            enemyList.NormalizeChances();

            // Загрузка иконок из папки Icons
            string iconFolder = Path.Combine(baseDir, "Icons");
            if (Directory.Exists(iconFolder))
            {
                foreach (string file in Directory.GetFiles(iconFolder, "*.png"))
                {
                    enemyIcons.Add(new EnemyIcon
                    {
                        Name = Path.GetFileName(file),
                        ImagePath = file
                    });
                }
            }

            SpawnNextEnemy();
            UpdateUI();
        }

        private void SpawnNextEnemy()
        {
            CEnemyTemplate template = enemyList.FindByChance(rnd.NextDouble());
            if (template == null) return;

            EnemyIcon icon = FindIconByName(template.IconName);
            currentEnemy = new Enemy(template, player.Lvl, icon);

            EnemyIconImage.Source = icon != null
                ? new BitmapImage(new Uri(icon.ImagePath))
                : null;
        }

        private EnemyIcon FindIconByName(string iconName)
        {
            foreach (EnemyIcon icon in enemyIcons)
                if (icon.Name == iconName) return icon;
            return null;
        }

        private void UpdateUI()
        {
            if (currentEnemy != null)
            {
                EnemyNameText.Text = currentEnemy.Name;
                EnemyHPText.Text = currentEnemy.CurrentHitPoints.ToString();
                EnemyGoldText.Text = currentEnemy.GoldReward.ToString();
            }

            PlayerGoldText.Text = player.Gold.ToString();
            PlayerDamageText.Text = player.Damage.ToString();
            LevelText.Text = player.Lvl.ToString();
            UpgradeCostText.Text = player.UpgradeCost.ToString();
        }

        private void EnemyIcon_Click(object sender, MouseButtonEventArgs e)
        {
            if (currentEnemy == null || currentEnemy.IsDead) return;

            BigNumber reward;
            bool killed = currentEnemy.TakeDamage(player.DealDamage(), out reward);

            if (killed)
            {
                player.AddGold(reward);
                SpawnNextEnemy();
            }

            UpdateUI();
        }

        private void BtnUpgrade_Click(object sender, RoutedEventArgs e)
        {
            if (player.TryUpgrade())
                UpdateUI();
            else
                MessageBox.Show("Недостаточно золота");
        }
    }
}
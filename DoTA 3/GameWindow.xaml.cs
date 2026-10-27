using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace DoTA_3
{
    public partial class GameWindow : Window
    {
        public Player Player { get; private set; }
        private Enemy currentEnemy;
        private CEnemyTemplateList enemyList;
        private List<EnemyIcon> enemyIcons = new List<EnemyIcon>();
        private Random rnd = new Random();

        // НОВОЕ: Для сфер и таймера
        private DispatcherTimer timer;
        private List<CCollectable> activeSpheres = new List<CCollectable>();
        private double spawnSphereTime = 0;

        public GameWindow()
        {
            InitializeComponent();

            Player = new Player();
            enemyList = new CEnemyTemplateList();

            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string jsonPath = Path.Combine(baseDir, "enemies.json");

            if (File.Exists(jsonPath))
            {
                enemyList.LoadFromJson(jsonPath);
                enemyList.NormalizeChances();
            }

            string iconFolder = Path.Combine(baseDir, "Icons");
            if (Directory.Exists(iconFolder))
            {
                foreach (string file in Directory.GetFiles(iconFolder, "*.png"))
                {
                    enemyIcons.Add(new EnemyIcon { Name = Path.GetFileName(file), ImagePath = file });
                }
            }

            SpawnNextEnemy();
            UpdateUI();

            // НОВОЕ: Запуск таймера игры (каждые 100 мс)
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += GameTimer_Tick;
            timer.Start();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            double delta = 0.1;

            // 1. Обновляем перезарядку игрока
            Player.Update(delta);

            // 2. Обновляем время жизни сфер
            for (int i = activeSpheres.Count - 1; i >= 0; i--)
            {
                if (activeSpheres[i].updateLifetime(delta))
                    activeSpheres.RemoveAt(i);
            }

            // 3. Спавн сфер (раз в 3 секунды)
            spawnSphereTime += delta;
            if (spawnSphereTime >= 3.0)
            {
                SpawnSphere();
                spawnSphereTime = 0;
            }

            UpdateUI();
        }

        private void SpawnSphere()
        {
            double x = rnd.NextDouble() * (SphereCanvas.ActualWidth - 30);
            double y = rnd.NextDouble() * (SphereCanvas.ActualHeight - 30);
            double size = 30;
            double r = rnd.NextDouble();

            CCollectable sphere;
            if (r < 0.33) sphere = new SphereDamage(new Point(x, y), size, 4.0, this);
            else if (r < 0.66) sphere = new SphereCooldown(new Point(x, y), size, 4.0, this);
            else sphere = new SphereGold(new Point(x, y), size, 4.0, this);

            activeSpheres.Add(sphere);
            SphereCanvas.Children.Add(sphere.getSprite());
        }

        public void RemoveSphere(CCollectable sphere)
        {
            SphereCanvas.Children.Remove(sphere.getSprite());
        }

        private void EnemyIcon_Click(object sender, MouseButtonEventArgs e)
        {
            if (currentEnemy == null || currentEnemy.IsDead) return;

            // НОВОЕ: Проверка перезарядки
            if (!Player.CanClick) return;

            Player.onMouseClick(); // Сбрасываем КД

            BigNumber reward;
            bool killed = currentEnemy.TakeDamage(Player.DealDamage(), out reward);

            if (killed)
            {
                Player.AddGold(reward);
                SpawnNextEnemy();
            }

            UpdateUI();
        }

        private void BtnUpgrade_Click(object sender, RoutedEventArgs e)
        {
            if (Player.TryUpgrade())
                UpdateUI();
            else
                MessageBox.Show("Недостаточно золота");
        }

        private void SpawnNextEnemy()
        {
            CEnemyTemplate template = enemyList.FindByChance(rnd.NextDouble());
            if (template == null) return;

            EnemyIcon icon = FindIconByName(template.IconName);
            currentEnemy = new Enemy(template, Player.Lvl, icon);

            EnemyIconImage.Source = icon != null ? new BitmapImage(new Uri(icon.ImagePath)) : null;
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

            PlayerGoldText.Text = Player.Gold.ToString();
            PlayerDamageText.Text = Player.Damage.ToString();
            LevelText.Text = Player.Lvl.ToString();
            UpgradeCostText.Text = Player.UpgradeCost.ToString();

            // Визуальный индикатор перезарядки
            if (Player.CanClick)
                EnemyIconImage.Opacity = 1.0; // Можно кликать
            else
                EnemyIconImage.Opacity = 0.5; // Идет перезарядка
        }
    }
}
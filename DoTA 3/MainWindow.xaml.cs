using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Formats.Asn1.AsnWriter;

namespace DoTA_3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window

    {
        string selectedIconName = "";

        List<EnemyIcon> enemyIcons = new List<EnemyIcon>();
        CEnemyTemplateList enemyList = new CEnemyTemplateList();// ← вот это
        public void LoadIconsFromFolder(string path)
        {
            //фильтр расширения изображения
            string filter = "*.png";
            //получение массива строк содержащих пути до изображений
            string[] files = Directory.GetFiles(path, filter);
            //перебор всех полученных путей
            //в file содержится путь до изображения с расширением .png
            foreach (string file in files)
            {
                enemyIcons.Add(
                new EnemyIcon
                {
                    // получение имени файла с расширением
                    Name = System.IO.Path.GetFileName(file),
                    // получение полного пути до файла
                    ImagePath = file
                }
                );
            }
        }
        public void ShowIcons()
        {
            IconsListBox.Items.Clear();  // очищаем старые иконки

            foreach (EnemyIcon icon in enemyIcons)
            {
                Image image = new Image()
                {
                    Source = new BitmapImage(new Uri(icon.ImagePath)),
                    Height = 64
                };

                IconsListBox.Items.Add(image);
            }
        }
        private void EnemiesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EnemiesListBox.SelectedItem == null) return;

            string selectedName = EnemiesListBox.SelectedItem.ToString();

            CEnemyTemplate enemy = enemyList.GetEnemyByName(selectedName);
            if (enemy == null) return;

            // Заполняем поля
            DetailName.Text = enemy.Name;
            DetailIconName.Text = enemy.IconName;
            DetailBaseLife.Text = enemy.BaseLife.ToString();
            DetailLifeMod.Text = enemy.LifeModifier.ToString();
            DetailBaseGold.Text = enemy.BaseGold.ToString();
            DetailGoldMod.Text = enemy.GoldModifier.ToString();
            DetailSpawnChance.Text = enemy.SpawnChance.ToString();

            // Загружаем иконку
            string iconPath = FindIconPath(enemy.IconName);
            if (iconPath != null)
            {
                MainEnemyIcon.Source = new BitmapImage(new Uri(iconPath));
            }
        }
        private string FindIconPath(string iconName)
        {
            foreach (EnemyIcon icon in enemyIcons)
            {
                if (icon.Name == iconName)
                {
                    return icon.ImagePath;
                }
            }
            return null;
        }
        private void BtnLoadIcons_Click(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog dlg = new OpenFolderDialog();
            dlg.Title = "Выберите папку с иконками";

            if (dlg.ShowDialog() == true)
            {
                enemyIcons.Clear();   // очищаем старый список
                LoadIconsFromFolder(dlg.FolderName);
                ShowIcons();   // ← вот здесь
                MessageBox.Show($"Загружено иконок: {enemyIcons.Count}");
            }
        }
        private void IconsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // приводим sender к типу ListBox
            ListBox iconHolder = sender as ListBox;

            // проверяем, что выбран именно Image (и что выбор вообще есть)
            if (iconHolder.SelectedItem is Image selectedImage && iconHolder.SelectedItem != null)
            {
                // Source — это Uri, поэтому получаем имя файла через ToString + Path.GetFileName
                string iconName = System.IO.Path.GetFileName(selectedImage.Source.ToString());
                selectedIconName = iconName;
                IconNameTextBox.Text = iconName;
                // сохраняем имя иконки в шаблон противника
            }
        }
        public void RefreshEnemiesList()
        {
            EnemiesListBox.Items.Clear();
            foreach (string name in enemyList.GetListOfEnemyNames())
            {
                EnemiesListBox.Items.Add(name);
            }
        }
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                MessageBox.Show("Введите имя противника");
                return;
            }

            try
            {
                enemyList.AddEnemy(
                    NameTextBox.Text,
                    selectedIconName,
                    int.Parse(BaseLifeTextBox.Text),
                    double.Parse(LifeModTextBox.Text),
                    int.Parse(BaseGoldTextBox.Text),
                    double.Parse(GoldModTextBox.Text),
                    double.Parse(SpawnChanceTextBox.Text)
                );

                RefreshEnemiesList();

                // очистка полей
                NameTextBox.Clear();
                IconNameTextBox.Clear();
                BaseLifeTextBox.Clear();
                BaseGoldTextBox.Clear();
                LifeModTextBox.Clear();
                GoldModTextBox.Clear();
                SpawnChanceTextBox.Clear();
                selectedIconName = "";
            }
            catch (FormatException)
            {
                MessageBox.Show("Проверьте числовые поля");
            }
        }
        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (EnemiesListBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите противника для удаления");
                return;
            }

            string name = EnemiesListBox.SelectedItem.ToString();
            enemyList.DeleteEnemyByName(name);
            RefreshEnemiesList();
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "JSON files (*.json)|*.json";

            if (dlg.ShowDialog() == true)
            {
                enemyList.SaveToJson(dlg.FileName);
                MessageBox.Show("Сохранено");
            }
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "JSON files (*.json)|*.json";

            if (dlg.ShowDialog() == true)
            {
                enemyList.LoadFromJson(dlg.FileName);
                RefreshEnemiesList();
                MessageBox.Show("Загружено");
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            CEnemyTemplateList enemyList = new CEnemyTemplateList();
            string content = File.ReadAllText("enemies.json");
            MessageBox.Show(content);

        }

    }
}
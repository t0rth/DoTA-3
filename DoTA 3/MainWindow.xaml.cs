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

        List<EnemyIcon> enemyIcons = new List<EnemyIcon>();  // ← вот это
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
                MessageBox.Show($"Выбрана иконка: {selectedIconName}");
                // сохраняем имя иконки в шаблон противника
            }
        }

        public MainWindow()
        {
            InitializeComponent();

        }

    }
}
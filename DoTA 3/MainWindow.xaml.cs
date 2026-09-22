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
        public MainWindow()
        {
            InitializeComponent();

            CEnemyTemplateList list = new CEnemyTemplateList();
            list.AddEnemy("Axe", "Axe.png", 100, 1.0, 10, 1.0, 0.5);
            list.AddEnemy("Ork", "ork.png", 200, 1.2, 20, 1.1, 0.3);

            list.SaveToJson("enemies.json");
            list.LoadFromJson("enemies.json");
            MessageBox.Show($"Загружено противников: {list.GetListOfEnemyNames().Count}");
            EnemyIcon icon = new EnemyIcon();
            icon.Name = "Axe.png";
            icon.ImagePath = @"C:\Users\Владимир\source\repos\DoTA 3\DoTA 3\Icons\EnemyIcons\Axe.png";

            MessageBox.Show($"Иконка: {icon.Name}, путь: {icon.ImagePath}");
        }

    }
}
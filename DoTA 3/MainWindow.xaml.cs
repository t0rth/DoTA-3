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
        //функция в основном теле программы
        public void DrawLine(Point2D p1, Point2D p2)
        {
            //Создание новой линии
            Line line = new Line();
            //Цвет и толщина линии
            line.Stroke = Brushes.Red;
            line.StrokeThickness = 3;
            //Установка координат линии из координат точек Point2D
            line.X1 = p1.X;
            line.Y1 = p1.Y;
            line.X2 = p2.X;
            line.Y2 = p2.Y;
            //Добавление линии в Canvas
            Scene.Children.Add(line);
        }
        Triangle tr;
        Rectangle rc;
        Random rnd = new Random();
        public MainWindow()
        {
            InitializeComponent();
            //Создание треугольника со случайными координатами
            Point2D p1 = new Point2D(rnd.Next(0, (int)Scene.Width), rnd.Next(0, (int)Scene.Height));
            Point2D p2 = new Point2D(rnd.Next(0, (int)Scene.Width), rnd.Next(0, (int)Scene.Height));
            Point2D p3 = new Point2D(rnd.Next(0, (int)Scene.Width), rnd.Next(0, (int)Scene.Height));
            Point2D p4 = new Point2D(rnd.Next(0, (int)Scene.Width), rnd.Next(0, (int)Scene.Height));
            tr = new Triangle(p1, p2, p3);
            int width = rnd.Next(50, 200);
            int height = rnd.Next(50, 200);
            Point2D start = new Point2D(
                rnd.Next(0, (int)Scene.Width - width),
                rnd.Next(0, (int)Scene.Height - height)
            );
            rc = new Rectangle(start, width, height);
        }
        public void DrawTriangle(Triangle tr)
        {
            //Отрисовка треугольника с помощью функции отрисовки линии
            DrawLine(tr.P1, tr.P2);
            DrawLine(tr.P2, tr.P3);
            DrawLine(tr.P3, tr.P1);
        }
        public void DrawRectangle(Rectangle rc)
        {
            //Отрисовка треугольника с помощью функции отрисовки линии
            DrawLine(rc.P1, rc.P2);
            DrawLine(rc.P2, rc.P3);
            DrawLine(rc.P3, rc.P4);
            DrawLine(rc.P4, rc.P1);
        }
        public void ClearScene()
        {
            //Очистка Canvas от всех объектов
            Scene.Children.Clear();
        }
        private void BtnTriangle_Click(object sender, RoutedEventArgs e)
        {
            ClearScene();   // стираем старое

            Random rnd = new Random();
            Point2D p1 = new Point2D(rnd.Next(0, (int)Scene.Width), rnd.Next(0, (int)Scene.Height));
            Point2D p2 = new Point2D(rnd.Next(0, (int)Scene.Width), rnd.Next(0, (int)Scene.Height));
            Point2D p3 = new Point2D(rnd.Next(0, (int)Scene.Width), rnd.Next(0, (int)Scene.Height));

            Triangle tr = new Triangle(p1, p2, p3);
            DrawTriangle(tr);
        }
        private void BtnRectangle_Click(object sender, RoutedEventArgs e)
        {
            ClearScene();

            int width = rnd.Next(50, 200);
            int height = rnd.Next(50, 200);
            Point2D start = new Point2D(
                rnd.Next(0, (int)Scene.Width - width),
                rnd.Next(0, (int)Scene.Height - height)
            );

            Rectangle rc = new Rectangle(start, width, height);
            DrawRectangle(rc);
        }
    }
}
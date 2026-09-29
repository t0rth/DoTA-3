using System.Windows;

namespace DoTA_3
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnEditor_Click(object sender, RoutedEventArgs e)
        {
            EnemyEditorWindow editor = new EnemyEditorWindow();
            editor.Show();
        }

        private void BtnGame_Click(object sender, RoutedEventArgs e)
        {
            GameWindow game = new GameWindow();
            game.Show();
        }
    }
}
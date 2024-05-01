using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Supermarket_Simulation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DoubleAnimation hide_menu = new();
        DoubleAnimation show_menu = new();

        public MainWindow()
        {
            InitializeComponent();

            hide_menu.Duration  = TimeSpan.FromSeconds(0.5);
            hide_menu.From      = 1.0;
            hide_menu.To        = 0.0;
            show_menu.Duration  = TimeSpan.FromSeconds(0.5);
            show_menu.From      = 0.0;
            show_menu.To        = 1.0;
        }

        private void quit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void menu_grid_MouseEnter(object sender, MouseEventArgs e)
        {
            menu_grid.BeginAnimation(OpacityProperty, show_menu);
        }

        private void menu_grid_MouseLeave(object sender, MouseEventArgs e)
        {
            menu_grid.BeginAnimation(OpacityProperty, hide_menu);
        }
    }
}
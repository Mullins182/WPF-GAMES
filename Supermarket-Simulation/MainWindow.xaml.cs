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
using System.Windows.Threading;

namespace Supermarket_Simulation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DoubleAnimation hide_menu      = new();
        private readonly DoubleAnimation show_menu      = new();

        private readonly DispatcherTimer gameroutine    = new();

        private int loopCounter = 0;

        private void Set_MenuAnimationProperties()
        {
            hide_menu.Duration  = TimeSpan.FromSeconds(0.5);
            hide_menu.From      = 1.0;
            hide_menu.To        = 0.0;
            show_menu.Duration  = TimeSpan.FromSeconds(0.5);
            show_menu.From      = 0.0;
            show_menu.To        = 1.0;
        }

        private void Set_DispatcherTimerProperties()
        {
            gameroutine.Interval    = TimeSpan.FromMilliseconds(50);
            gameroutine.Tick        += Gameroutine;
        }

        public MainWindow()
        {
            InitializeComponent();

            Set_MenuAnimationProperties();
            Set_DispatcherTimerProperties();
        }

        // GAMEROUTINE SECTION
        private void Gameroutine(object? sender, EventArgs e)
        {
            loopCounter += 1;
            GameTime.Content = loopCounter;
        }

            // GAMEROUTINE SECTION END

        // UI-Controls-Events
        private void Quit_Click(object sender, RoutedEventArgs e)
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

        private void StartStop_Click(object sender, RoutedEventArgs e)
        {
            if (gameroutine.IsEnabled)
            {
                gameroutine.Stop();
            }
            else
            {
                GameTime.Visibility = Visibility.Visible;
                GameTime.Content = "";
                gameroutine.Start();
            }
        }

        // UI-Controls-Events END
    }
}
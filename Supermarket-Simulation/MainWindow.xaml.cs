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
        private readonly DoubleAnimation gameActivity   = new();

        private readonly DispatcherTimer gameroutine    = new();

        private bool gameIsActive                       = false;

        private void Set_DoubleAnimationProperties()
        {
            hide_menu.Duration          = TimeSpan.FromSeconds(0.5);
            hide_menu.From              = 1.0;
            hide_menu.To                = 0.0;
            show_menu.Duration          = TimeSpan.FromSeconds(0.5);
            show_menu.From              = 0.0;
            show_menu.To                = 1.0;
            gameActivity.Duration       = TimeSpan.FromSeconds(0.5);
            gameActivity.From           = 0.0;
            gameActivity.To             = 1.0;
            gameActivity.AutoReverse    = true;
            gameActivity.Completed      += GameStatusAnimLoop;
        }

        private void GameStatusAnimLoop(object? sender, EventArgs e)
        {
            if (gameIsActive)
            {
                GameStatus.BeginAnimation(OpacityProperty, gameActivity);
            }
            gameIsActive = false;
        }

        private void Set_DispatcherTimerProperties()
        {
            gameroutine.Interval    = TimeSpan.FromMilliseconds(50);
            gameroutine.Tick        += Gameroutine;
        }

        private void Set_GameStatusEllipsePos()
        {
            Canvas.SetTop(GameStatus, 0.0);
            Canvas.SetLeft(GameStatus, this.Width - GameStatus.Width);
        }

        public MainWindow()
        {
            InitializeComponent();

            Set_DoubleAnimationProperties();
            Set_DispatcherTimerProperties();
            Set_GameStatusEllipsePos();

        }

        // GAMEROUTINE SECTION
        private void Gameroutine(object? sender, EventArgs e)
        {
            gameIsActive = true;
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
                gameIsActive = false;
            }
            else
            {
                gameroutine.Start();
                GameStatus.BeginAnimation(OpacityProperty, gameActivity);
            }
        }
                                                            // UI-Controls-Events END
    }
}
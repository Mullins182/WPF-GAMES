using JetPack_2.Models;
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
using System.Windows.Threading;

namespace JetPack_2
{
    public partial class MainWindow : Window
    {
        private readonly DispatcherTimer GameRoutine = new();

        private Player player   = new();

        private bool moveLeft   = false;
        private bool moveRight  = false;
        private bool jump       = false;

        public MainWindow()
        {
            InitializeComponent();
            Game_ini();
        }

        private void Game_ini()
        {
            GameRoutine.Interval = TimeSpan.FromMilliseconds(30);
            GameRoutine.Tick += GameRoutine_Tick;

            player.SetPlayerProps();
            SetPlayerPos();
            GenerateLevel1();

            GameRoutine.Start();
        }

        private void GameRoutine_Tick(object? sender, EventArgs e)
        {
            jump = player.PosY <= 0 ? false : jump;
            player.PosY = jump ? player.PosY -= 5 : player.PosY >= 880 ? player.PosY : player.PosY += 5;
            Canvas.SetTop(player.PlayerModel, player.PosY);

            moveLeft = player.PosX <= 0 ? false : moveLeft;
            moveRight = player.PosX + player.PlayerModel.Width >= GameCanvas.ActualWidth ? false : moveRight;

            player.PosX = moveLeft ? player.PosX -= 5 : moveRight ? player.PosX += 5 
                : player.PosX;
            Canvas.SetLeft(player.PlayerModel, player.PosX);
        }

        private void GenerateLevel1()
        {
            Platforms Ground = new(0, 940, Brushes.Green, 20, 1500);
            Rectangle ground = Ground.GeneratePlatform();

            GameCanvas.Children.Add(ground);

            Canvas.SetTop(ground, Ground.PosY);
            Canvas.SetLeft(ground, Ground.PosX);
        }

        private void SetPlayerPos()
        {
            GameCanvas.Children.Add(player.PlayerModel);

            Canvas.SetTop(player.PlayerModel, player.PosY);
            Canvas.SetLeft(player.PlayerModel, player.PosX);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            moveLeft    = e.Key == Key.Left ? true : moveLeft;
            moveRight   = e.Key == Key.Right ? true : moveRight;
            jump        = e.Key == Key.Space ? true : jump;
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            moveLeft    = e.Key == Key.Left ? false : moveLeft;
            moveRight   = e.Key == Key.Right ? false : moveRight;
            jump        = e.Key == Key.Space ? false : jump;
        }
    }
}
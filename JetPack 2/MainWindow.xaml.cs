using JetPack_2.Models;
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

        private async void Game_ini()
        {
            GameRoutine.Interval = TimeSpan.FromMilliseconds(30);
            GameRoutine.Tick += GameRoutine_Tick;

            DoubleAnimation GameLabelBlend = new();
            GameLabelBlend.From     = 0;
            GameLabelBlend.To       = 0.95;
            GameLabelBlend.Duration = TimeSpan.FromSeconds(2.666);

            Label GameLogo = new();
            ImageBrush GameLogoPicture = new();
            GameLogoPicture.ImageSource = new BitmapImage(new Uri("pack://application:,,,/PNG/Jetpack3Logo.png"));

            GameLogo.Width = 800;
            GameLogo.Height = 170;
            GameLogo.Background = GameLogoPicture;
            GameLogo.Opacity = 1;
            GameCanvas.Children.Add(GameLogo);
            Canvas.SetTop(GameLogo, 60);
            Canvas.SetLeft(GameLogo, (this.Width / 2) - 400);

            Label GameLabel = new();
            ImageBrush GamePicture = new();
            GamePicture.ImageSource = new BitmapImage(new Uri("pack://application:,,,/PNG/Jetpack2Label.png"));

            GameLabel.Width = 500;
            GameLabel.Height = 500;
            GameLabel.Background = GamePicture;
            GameLabel.Opacity = 0;
            GameCanvas.Children.Add(GameLabel);
            Canvas.SetTop(GameLabel, (this.Height / 2) - 250);
            Canvas.SetLeft(GameLabel, (this.Width / 2) - 250);

            player.SetPlayerProps();
            SetPlayerPos();
            GenerateLevel1();

            await Task.Delay(1500);

            GameLabel.BeginAnimation(OpacityProperty, GameLabelBlend);

            await Task.Delay(4000);

            GameLabelBlend.From     = 0.95;
            GameLabelBlend.To       = 0;

            GameLabel.BeginAnimation(OpacityProperty, GameLabelBlend);

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
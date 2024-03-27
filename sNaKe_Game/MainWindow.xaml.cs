using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SnakeGame
{
    public partial class MainWindow : Window
    {
        private const int CellSize                  = 50;

        private bool iniFin                         = true;

        private readonly DispatcherTimer Game_Timer = new();

        private readonly MediaPlayer SnakeSound1    = new();
        private readonly MediaPlayer SnakeSound2    = new();

        private readonly DoubleAnimation FadeInOut  = new();

        private readonly SolidColorBrush SnakeColor = Brushes.Green;
        private readonly SolidColorBrush FoodColor  = Brushes.Red;

        private readonly List<Ellipse> snake        = [];
        private Point food;
        private Direction direction                 = Direction.Left;
        private readonly Ellipse foodPiece          = new()
        {
            Fill = Brushes.Red,
            Width = CellSize,
            Height = CellSize
        };

        public MainWindow()
        {
            InitializeComponent();

            this.KeyDown            += MainWindow_KeyDown; // Event for keyboard inputs

            Game_Timer.Interval     = TimeSpan.FromMilliseconds(100);

            Game_Timer.Tick         += GameRoutine;

            SnakeSound1.Open(new Uri("sound_effects/snake_rattle1.mp3", UriKind.RelativeOrAbsolute));
            SnakeSound2.Open(new Uri("sound_effects/snake_hiss.mp3", UriKind.RelativeOrAbsolute));

            SnakeSound1.IsMuted     = true;
            SnakeSound2.IsMuted     = true;
            infobox.Visibility      = Visibility.Collapsed;
            infobox.Content         = "PRESS\n[SPACE] => Start Game\n[R]          => Reset Game\n[Q]          => Quit Game";

            HelloSnake();
            InitializeGame(7333);
        }

        private async void HelloSnake()
        {
            play.IsEnabled  = false;
            reset.IsEnabled = false;

            await Task.Delay(1101);

            SnakeSound1.IsMuted = false;
            SnakeSound2.IsMuted = false;
            SnakeSound1.Play();

            FadeInOut.AutoReverse = true;
            FadeInOut.Duration = TimeSpan.FromMilliseconds(3333);
            FadeInOut.From = 0;
            FadeInOut.To = 1;
            Start_Snake.BeginAnimation(OpacityProperty, FadeInOut);

            await Task.Delay(2000);

            SnakeSound2.Play();
        }
        private async void InitializeGame(int delay)
        {
            await Task.Delay(delay);

            play.IsTabStop  = false;
            reset.IsTabStop = false;
            quit.IsTabStop  = false;
            play.IsEnabled  = true;
            reset.IsEnabled = true;

            snake.Clear();
            direction = Direction.Left;
            play_area.Children.Clear();

            await Task.Delay(555);

            DrawSnakePiece(1000, 350);
            DrawSnakePiece(1050, 350);
            DrawSnakePiece(1100, 350);

            PlaceFood();

            ReadyForPlay();
        }

        private void GameRoutine(object? sender, EventArgs e)
        {
            MoveSnake();
            CheckCollision();
        }

        private void DrawSnakePiece(int x, int y)
        {
            var piece = new Ellipse
            {
                Fill = SnakeColor,
                Width = CellSize,
                Height = CellSize
            };
            Canvas.SetLeft(piece, x);
            Canvas.SetTop(piece, y);

            play_area.Children.Add(piece);
            snake.Insert(0, piece);
        }

        private void MoveSnake()
        {
            var head = snake.First();
            var newX = Canvas.GetLeft(head);
            var newY = Canvas.GetTop(head);

            switch (direction)
            {
                case Direction.Up:
                    newY -= CellSize;
                    break;
                case Direction.Down:
                    newY += CellSize;
                    break;
                case Direction.Left:
                    newX -= CellSize;
                    break;
                case Direction.Right:
                    newX += CellSize;
                    break;
            }

            // Remove tail
            var tail = snake.Last();
            snake.Remove(tail);
            play_area.Children.Remove(tail);

            // Add new head
            var newHead = new Ellipse
            {
                Fill = SnakeColor,
                Width = CellSize,
                Height = CellSize
            };
            Canvas.SetLeft(newHead, newX);
            Canvas.SetTop(newHead, newY);
            play_area.Children.Add(newHead);
            snake.Insert(0, newHead);
        }

        private void CheckCollision()
        {
            var head    = snake.First();
            var headX   = Canvas.GetLeft(head);
            var headY   = Canvas.GetTop(head);

            if (headX < 0 || headX >= play_area.ActualWidth - head.ActualWidth ||
                headY < 0 || headY >= play_area.ActualHeight - 50)
            {
                GameOver();
            }

            //if (headX < 0 || headX >= play_area.ActualWidth ||
            //    headY < 0 || headY >= play_area.ActualHeight ||
            //    snake.Any(s => s != head && Canvas.GetLeft(s) == headX && Canvas.GetTop(s) == headY))
            //{
            //    GameOver();
            //}


            // Check if food is eaten
            if (headX == food.X && headY == food.Y)
            {
                EatFood();
            }
        }

        private void PlaceFood()
        {
            var random = new Random();
            int x, y;
            //do
            //{
            //    x = random.Next(0, (int)play_area.ActualWidth);
            //    y = random.Next(0, (int)play_area.ActualHeight);
            //} while (snake.Any(piece => Canvas.GetLeft(piece) == x * CellSize && Canvas.GetTop(piece) == y * CellSize));

            do
            {
                x = random.Next(0, (int)play_area.ActualWidth);
                y = random.Next(0, (int)play_area.ActualHeight - 50);
            } while (x % 50 != 0 || y % 50 != 0);

            food = new Point(x, y);

            play_area.Children.Add(foodPiece);

            Canvas.SetLeft(foodPiece, food.X);
            Canvas.SetTop(foodPiece, food.Y);
        }

        private void EatFood()
        {
            play_area.Children.Remove(foodPiece);
            DrawSnakePiece((int)food.X, (int)food.Y);
            PlaceFood();
        }

        private void GameOver()
        {
            Game_Timer.Stop();
            play_area.Background = Brushes.DarkRed;
            quit.IsEnabled      = true;
            reset.IsEnabled     = true;
            play.IsEnabled      = false;
            quit.Foreground     = Brushes.DarkOrange;
            reset.Foreground    = Brushes.DarkOrange;
            play.Foreground     = Brushes.DarkRed;
            reset.Focus();
        }

        // Event-Handler for Keyboard Inputs
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    if (direction != Direction.Down)
                        direction = Direction.Up;
                    break;
                case Key.Down:
                    if (direction != Direction.Up)
                        direction = Direction.Down;
                    break;
                case Key.Left:
                    if (direction != Direction.Right)
                        direction = Direction.Left;
                    break;
                case Key.Right:
                    if (direction != Direction.Left)
                        direction = Direction.Right;
                    break;
            }
        }

        private void ReadyForPlay()
        {
            play_area.Background    = new SolidColorBrush(Color.FromArgb(255,34,47,00));
            quit.IsEnabled          = true;
            reset.IsEnabled         = true;
            play.IsEnabled          = true;
            quit.Foreground         = Brushes.DarkOrange;
            reset.Foreground        = Brushes.DarkOrange;
            play.Foreground         = Brushes.DarkOrange;
            infobox.Visibility      = Visibility.Visible;
            play.Focus();
            iniFin = true;
        }
        private void play_Click(object sender, RoutedEventArgs e)
        {
            if (Game_Timer.IsEnabled)
            {
                Game_Timer.Stop();
                quit.IsEnabled      = true;
                reset.IsEnabled     = true;
                quit.Foreground     = Brushes.DarkOrange;
                reset.Foreground    = Brushes.DarkOrange;
                play.Foreground     = Brushes.OrangeRed;
                infobox.Visibility  = Visibility.Visible;
            }
            else
            {
                infobox.Visibility  = Visibility.Collapsed;
                quit.IsEnabled      = false;
                reset.IsEnabled     = false;
                quit.Foreground     = Brushes.OrangeRed;
                reset.Foreground    = Brushes.OrangeRed;
                play.Foreground     = Brushes.YellowGreen;

                Game_Timer.Start();
            }
        }

        private void quit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void reset_Click(object sender, RoutedEventArgs e)
        {
            if (iniFin)
            {
                iniFin = false;

                InitializeGame(0);

                play.Foreground = Brushes.OrangeRed;
            }
        }

        private void menu_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) 
            {
                if (play.IsEnabled) 
                {
                    play_Click(sender, e);                
                }
            }
            else if (e.Key == Key.R)
            {
                if (reset.IsEnabled) 
                {
                    reset_Click(sender, e);                
                }
            }
            else if (e.Key == Key.Q) 
            {
                if (quit.IsEnabled) 
                {
                    quit_Click(sender, e);                
                }
            }
        }
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SnakeGame
{
    public partial class MainWindow : Window
    {
        //private const int BoardWidth                = 100;
        //private const int BoardHeight               = 100;
        private const int CellSize                  = 50;

        private readonly SolidColorBrush SnakeColor = Brushes.Green;
        private readonly SolidColorBrush FoodColor  = Brushes.Red;

        private readonly List<Ellipse> snake        = [];
        private Point food;
        private Direction direction                 = Direction.Left;
        private readonly Ellipse foodPiece = new Ellipse
        {
            Fill = Brushes.Red,
            Width = CellSize,
            Height = CellSize
        };
        private readonly DispatcherTimer timer      = new();

        private int canvasFoodi                     = 0;

        public MainWindow()
        {
            InitializeComponent();
            this.KeyDown += MainWindow_KeyDown; // Event for keyboard inputs
        }

        private void InitializeGame()
        {
            snake.Clear();
            direction = Direction.Left;
            play_area.Children.Clear();

            DrawSnakePiece(1000, 350);
            DrawSnakePiece(1050, 350);
            DrawSnakePiece(1100, 350);

            PlaceFood();

            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += GameRoutine;
            timer.Start();
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

            //play_area.Children.Add(piece);
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

            if (headX < 0 || headX >= play_area.ActualWidth ||
                headY < 0 || headY >= play_area.ActualHeight)
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
                y = random.Next(0, (int)play_area.ActualHeight);
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
            timer.Stop();
            //play_area.Background = Brushes.DarkRed;
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
        private void play_Click(object sender, RoutedEventArgs e)
        {
            InitializeGame();
        }

        private void quit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
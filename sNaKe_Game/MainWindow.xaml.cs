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
        private const int BoardWidth = 30;
        private const int BoardHeight = 20;
        private const int CellSize = 20;
        private readonly SolidColorBrush SnakeColor = Brushes.Green;
        private readonly SolidColorBrush FoodColor = Brushes.Red;

        private readonly List<Rectangle> snake = new List<Rectangle>();
        private Point food;
        private Direction direction = Direction.Right;
        private DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
            InitializeGame();
            this.KeyDown += MainWindow_KeyDown; // Event für Tastatureingaben hinzufügen
        }

        private void InitializeGame()
        {
            DrawSnakePiece(5, 5);
            DrawSnakePiece(5, 6);
            DrawSnakePiece(5, 7);

            PlaceFood();

            timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            MoveSnake();
            CheckCollision();
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
            var newHead = new Rectangle
            {
                Fill = SnakeColor,
                Width = CellSize,
                Height = CellSize
            };
            Canvas.SetLeft(newHead, newX);
            Canvas.SetTop(newHead, newY);
            play_area.Children.Add(newHead);
            snake.Insert(0, newHead);

            // Check if food is eaten
            if (newX == food.X && newY == food.Y)
            {
                EatFood();
            }
        }

        private void CheckCollision()
        {
            var head = snake.First();
            var headX = Canvas.GetLeft(head);
            var headY = Canvas.GetTop(head);

            if (headX < 0 || headX >= BoardWidth * CellSize ||
                headY < 0 || headY >= BoardHeight * CellSize ||
                snake.Any(s => s != head && Canvas.GetLeft(s) == headX && Canvas.GetTop(s) == headY))
            {
                GameOver();
            }
        }

        private void GameOver()
        {
            timer.Stop();
            InitializeGame();
        }

        private void EatFood()
        {
            DrawSnakePiece((int)food.X / CellSize, (int)food.Y / CellSize);
            PlaceFood();
        }

        private void DrawSnakePiece(int x, int y)
        {
            var piece = new Rectangle
            {
                Fill = SnakeColor,
                Width = CellSize,
                Height = CellSize
            };
            Canvas.SetLeft(piece, x * CellSize);
            Canvas.SetTop(piece, y * CellSize);

            play_area.Children.Add(piece);
            snake.Insert(0, piece);
        }

        private void PlaceFood()
        {
            var rand = new Random();
            int x, y;
            do
            {
                x = rand.Next(BoardWidth);
                y = rand.Next(BoardHeight);
            } while (snake.Any(piece => Canvas.GetLeft(piece) == x * CellSize && Canvas.GetTop(piece) == y * CellSize));

            food = new Point(x * CellSize, y * CellSize);

            var foodPiece = new Rectangle
            {
                Fill = FoodColor,
                Width = CellSize,
                Height = CellSize
            };
            Canvas.SetLeft(foodPiece, food.X);
            Canvas.SetTop(foodPiece, food.Y);

            play_area.Children.Add(foodPiece);
        }

        // Event-Handler für Tastatureingaben
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
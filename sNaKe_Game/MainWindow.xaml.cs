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
        private int BoardWidth = 0;
        private int BoardHeight = 0;
        private const int CellSize = 1;
        private readonly SolidColorBrush SnakeColor = Brushes.Green;
        private readonly SolidColorBrush FoodColor = Brushes.Red;

        private readonly List<Ellipse> snake = [];
        private Point food;
        private Direction direction = Direction.Right;
        private DispatcherTimer timer = new();

        public MainWindow()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            BoardHeight = (int)play_area.ActualHeight;
            BoardWidth = (int)play_area.ActualWidth;

            DrawSnakePiece(5, 5);
            DrawSnakePiece(5, 6);
            DrawSnakePiece(5, 7);

            PlaceFood();

            timer.Interval = TimeSpan.FromMilliseconds(30);
            timer.Tick += GameRoutine;
            timer.Start();
        }

        private void GameRoutine(object? sender, EventArgs e)
        {
            MoveSnake();
            CheckCollision();
        }

        private void MoveSnake()
        {
            for (int i = snake.Count - 1; i > 0; i--)
            {
                var currentPiece = snake[i];
                var nextPiece = snake[i - 1];
                Canvas.SetLeft(currentPiece, Canvas.GetLeft(nextPiece));
                Canvas.SetTop(currentPiece, Canvas.GetTop(nextPiece));
            }

            var head = snake[0];
            switch (direction)
            {
                case Direction.Up:
                    Canvas.SetTop(head, Canvas.GetTop(head) - CellSize);
                    break;
                case Direction.Down:
                    Canvas.SetTop(head, Canvas.GetTop(head) + CellSize);
                    break;
                case Direction.Left:
                    Canvas.SetLeft(head, Canvas.GetLeft(head) - CellSize);
                    break;
                case Direction.Right:
                    Canvas.SetLeft(head, Canvas.GetLeft(head) + CellSize);
                    break;
            }
        }

        private void CheckCollision()
        {
            //var head = snake[0];
            //if (Canvas.GetLeft(head) < 0 || Canvas.GetLeft(head) >= BoardWidth * CellSize ||
            //    Canvas.GetTop(head) < 0 || Canvas.GetTop(head) >= BoardHeight * CellSize)
            //{
            //    GameOver();
            //}

            //if (Canvas.GetLeft(head) == Canvas.GetLeft((UIElement)play_area.FindName("food")) && Canvas.GetTop(head) == Canvas.GetTop((UIElement)play_area.FindName("food")))
            //{
            //    EatFood();
            //}
        }

        private void GameOver()
        {
            timer.Stop();
            InitializeGame();
        }

        private void EatFood()
        {
            var tail = snake.Last();
            DrawSnakePiece((int)Canvas.GetLeft(tail) / CellSize, (int)Canvas.GetTop(tail) / CellSize);
            PlaceFood();
        }

        private void DrawSnakePiece(int x, int y)
        {
            var piece = new Ellipse
            {
                Fill = SnakeColor,
                Width = 50,
                Height = 30
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

            var foodPiece = new Ellipse
            {
                Fill = FoodColor,
                Width = CellSize,
                Height = CellSize,
                Name = "food"
            };
            Canvas.SetLeft(foodPiece, food.X);
            Canvas.SetTop(foodPiece, food.Y);

            play_area.Children.Add(foodPiece);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
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

        private void quit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void play_Click(object sender, RoutedEventArgs e)
        {

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

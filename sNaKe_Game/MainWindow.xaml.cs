using System;
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
        private const int SnakeSizeH = 30;
        private const int SnakeSizeW = 50;
        private const int SnakeSpeed = 100;
        private Point currentPosition = new(1000, 350);
        private DispatcherTimer timer = new();
        private Direction direction = Direction.Left;
        private UIElement? snakeFood = null;
        private SolidColorBrush foodBrush = Brushes.Red;

        public MainWindow()
        {
            InitializeComponent();

            this.KeyDown += new KeyEventHandler(OnButtonKeyDown);
            paintSnake(currentPosition);
            timer.Interval = TimeSpan.FromMilliseconds(SnakeSpeed);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void paintSnake(Point currentPosition)
        {
            Ellipse Segment = new();
            Segment.Fill = Brushes.Red;
            Segment.Width = SnakeSizeW;
            Segment.Height = SnakeSizeH;
            Canvas.SetTop(Segment, currentPosition.Y);
            Canvas.SetLeft(Segment, currentPosition.X);
            int count = play_area.Children.Count;
            play_area.Children.Add(Segment);
            //currentPosition = new Point(currentPosition.X, currentPosition.Y);
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            // Update the position of the snake and repaint it

            if (direction == Direction.Left)
            {
                currentPosition.X -= 5;
                Canvas.SetLeft(play_area.Children[0], currentPosition.X);
            }
            else if (direction == Direction.Right) 
            {
                currentPosition.X += 5;
                Canvas.SetLeft(play_area.Children[0], currentPosition.X);
            }
            else if (direction == Direction.Up) 
            {
                currentPosition.Y -= 5;
                Canvas.SetTop(play_area.Children[0], currentPosition.Y);
            }
            else if (direction == Direction.Down) 
            {
                currentPosition.Y += 5;
                Canvas.SetTop(play_area.Children[0], currentPosition.Y);
            }
        }

        private void OnButtonKeyDown(object sender, KeyEventArgs e)
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
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PAIN_lab5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isGameStarted = false;
        private bool isGameOver = false;
        private int dx = -1;
        private int dy = -1;
        private System.Windows.Threading.DispatcherTimer dispatcherTimer;
        private List<Rectangle> rectangles = new List<Rectangle>();
        private int points = 0;
        public MainWindow()
        {
            InitializeComponent();
            for (int j = 2; j < 7; j = j + 2)
            {
                for (int i = 3; i < 24; i = i + 3)
                {
                    Rectangle rectangle = new Rectangle();
                    rectangle.Fill = new SolidColorBrush(System.Windows.Media.Colors.Black);
                    Grid.SetColumn(rectangle, i);
                    Grid.SetColumnSpan(rectangle, 1);
                    Grid.SetRow(rectangle, j);
                    Grid.SetRowSpan(rectangle, 1);
                    MyGrid.Children.Add(rectangle);
                    rectangles.Add(rectangle);
                }
            }
        }

        private void UserBlock_KeyDown(object sender, KeyEventArgs e)
        {
            Console.Out.WriteLine("Wojtek");
            if (e.Key == Key.D)
            {
                int column = Grid.GetColumn(UserBlock);
                if (column < 21)
                {
                    Grid.SetColumn(UserBlock, column + 1);
                    if (!isGameStarted)
                    {
                        int ballColumn = Grid.GetColumn(Ball);
                        Grid.SetColumn(Ball, ballColumn + 1);
                    }
                }
            }
            else if (e.Key == Key.A)
            {
                int column = Grid.GetColumn(UserBlock);
                if (column > 0)
                {
                    Grid.SetColumn(UserBlock, column - 1);
                    if (!isGameStarted)
                    {
                        int ballColumn = Grid.GetColumn(Ball);
                        Grid.SetColumn(Ball, ballColumn - 1);
                    }
                }
            }
            else if (!isGameStarted && e.Key == Key.Space)
            {
                isGameStarted = true;
                dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
                dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 200);
                dispatcherTimer.Start();
            }

        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (isGameOver)
            {
                dispatcherTimer.Stop();
                return;
            }
            else if (MyGrid.Children.Count == 2)
            {
                dispatcherTimer.Stop();
                MessageBox.Show("You win !");
                return;
            }
            checkCollisions();
            int ballColumn = Grid.GetColumn(Ball);
            Grid.SetColumn(Ball, ballColumn + dx);
            int ballRow = Grid.GetRow(Ball);
            Grid.SetRow(Ball, ballRow + dy);
        }

        private void checkCollisions()
        {
            int userBlockColumn = Grid.GetColumn(UserBlock);
            int ballColumn = Grid.GetColumn(Ball);
            int ballRow = Grid.GetRow(Ball);
            if (ballRow == 21 && dy > 0)
            {
                if (userBlockColumn >= (ballColumn - 2) && userBlockColumn <= ballColumn)
                {
                    dy = -dy;
                }
                else if ((userBlockColumn == (ballColumn - 3) && dx < 0) || (userBlockColumn == (ballColumn + 1) && dx > 0))
                {
                    dy = -dy;
                    dx = -dx;
                }
                else
                {
                    isGameOver = true;
                    MessageBox.Show("Game over");
                    return;
                }
            }
            if ((ballColumn == 0 && dx < 0) || (ballColumn == 23 && dx > 0))
            {
                dx = -dx;
            }
            if (ballRow == 0 && dy < 0)
            {
                dy = -dy;
            }
            UIElement upDownBlock = MyGrid.Children.Cast<Rectangle>().Where(i => (Grid.GetRow(i) == ballRow + dy) && (Grid.GetColumn(i) == ballColumn)).FirstOrDefault();
            if (upDownBlock != null)
            {
                dy = -dy;
                MyGrid.Children.Remove(upDownBlock);
            }
            UIElement leftRightBlock = MyGrid.Children.Cast<Rectangle>().Where(i => (Grid.GetColumn(i) == ballColumn + dx) && (Grid.GetRow(i) == ballRow)).FirstOrDefault();
            if (leftRightBlock != null)
            {
                dx = -dx;
                MyGrid.Children.Remove(leftRightBlock);
            }
            UIElement skos = MyGrid.Children.Cast<Rectangle>().Where(i => (Grid.GetColumn(i) == ballColumn + dx) && (Grid.GetRow(i) == ballRow + dy)).FirstOrDefault();
            if (skos != null)
            {
                dx = -dx;
                dy = -dy;
                MyGrid.Children.Remove(skos);
            }

        }
    }
}

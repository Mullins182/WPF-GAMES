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

namespace JetPack_2
{
    public partial class MainWindow : Window
    {
        Player player = new();

        public MainWindow()
        {
            InitializeComponent();
            GameInitializing();
        }

        private void GameInitializing()
        {
            player.SetPlayerProps();
            SetPlayerPos();
            GenerateLevel1();
        }

        private void GenerateLevel1()
        {

        }

        private void SetPlayerPos()
        {
            GameCanvas.Children.Add(player.PlayerModel);
            Canvas.SetTop(player.PlayerModel, player.PosY);
            Canvas.SetLeft(player.PlayerModel, player.PosX);
        }
    }
}
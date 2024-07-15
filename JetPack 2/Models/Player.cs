using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace JetPack_2.Models
{
    public class Player
    {

        public Player() {}

        public Rectangle PlayerModel = new();

        private string? Name { get; set; }

        private SolidColorBrush PlayerColor     = Brushes.DarkRed;
        private readonly int Width              = 30;
        private readonly int Height             = 65;
        public int PosX = 600;
        public int PosY = 880;

        public void SetPlayerProps()
        {

            PlayerModel.Name      = "Jack";
            PlayerModel.Width     = this.Width;
            PlayerModel.Height    = this.Height;
            PlayerModel.Fill      = this.PlayerColor;
        }
    }
}

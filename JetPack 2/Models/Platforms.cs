using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace JetPack_2.Models
{
    public class Platforms
    {

        public Rectangle Platform = new();


        public Platforms(int posX, int posY, SolidColorBrush color, int height, int width)
        {
            this.PosX           = posX;
            this.PosY           = posY;
            PlatformColor       = color;
            Platform.Height     = height;
            Platform.Width      = width;
            Platform.Fill       = PlatformColor;
        }
        public SolidColorBrush? PlatformColor { get; set; }

        public int PosX { get; set; }
        public int PosY { get; set; }

        public Rectangle GeneratePlatform()
        {
            return Platform!;
        }
    }
}

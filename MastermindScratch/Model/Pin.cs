using System.Windows.Media;
using System.Windows.Shapes;

namespace MastermindScratch.Model
{
    public class Pin
    {
        public Ellipse Ellipse { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public bool Filled { get; set; }

        public Pin(int i, int j, Brush fill)
        {
            Ellipse = new Ellipse()
            {
                Fill = fill
            };                   
            Filled = false;
            Row = i;
            Column = j;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
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

        public Pin(int i, int j, Style style, MouseButtonEventHandler color_revert)
        {
            Ellipse = new Ellipse();
            Ellipse.Style = style;
            Ellipse.Fill = Brushes.White;
            Ellipse.MouseLeftButtonDown += color_revert;
            Filled = false;
            Row = i;
            Column = j;
        }




    }
}

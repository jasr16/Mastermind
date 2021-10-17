using Mastermind.Settings;
using System;
using System.IO;
using System.Windows.Media;

namespace Mastermind.Model
{
    public class HintPins
    {
        public Pin[,] Array;


        public HintPins(int trials, int pinsToGuess)
        {
            Array = new Pin[trials, pinsToGuess];
        }
        

        public void DisplayHints(int rowNumber, Hits hits)
        {
            for (int i = 0; i < hits.FullHits; i++)
            {
                Pin pin = Array[rowNumber, i];
                pin.Ellipse.Fill = Brushes.Black;
            }

            for (int j = hits.FullHits; j < hits.FullHits + hits.ColorHits; j++)
            {
                Pin pin = Array[rowNumber, j];
                pin.Ellipse.Fill = Brushes.Gray;
            }
        }

    }

}

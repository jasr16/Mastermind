using System;
using System.IO;
using System.Windows.Media;

namespace MastermindScratch.Model
{
    public class HintPins
    {
        public static Pin[,] Array = new Pin[Constants.NumberOfTrials, Constants.NumberOfPinsToGuess];

        public static void DisplayHints(int rowNumber, Hits hits)
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

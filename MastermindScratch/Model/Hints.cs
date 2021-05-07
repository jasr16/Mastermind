using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MastermindScratch.Model
{
    public class Hints
    {
        public static Ellipse[,] hintsArray = new Ellipse[Constants.NumberOfTrials, Constants.NumberOfPinsToGuess];

        public static void DisplayHints(int rowNumber, int[] hits)
        {
            for (int i = 0; i < hits[0]; i++)
            {
                Ellipse el = hintsArray[rowNumber, i];
                el.Fill = Brushes.Black;
            }

            for (int j = hits[0]; j < hits[0] + hits[1]; j++)
            {
                Ellipse el = hintsArray[rowNumber, j];
                el.Fill = Brushes.Gray;
            }
        }


    }



}

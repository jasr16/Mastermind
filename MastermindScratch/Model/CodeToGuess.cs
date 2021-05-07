using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MastermindScratch.Model
{
    public static class CodeToGuess
    {
        public static readonly Brush[] BrushesArray = new Brush[6] { Brushes.Yellow, Brushes.RoyalBlue, Brushes.Red, Brushes.LightGreen, Brushes.Brown, Brushes.Orange };

        public static Ellipse[] codePinsArray = new Ellipse[Constants.NumberOfPinsToGuess];

        public static Brush[] GenerateCodeToGuess(int numberOfPinsToGuess)
        {
            Brush[] codeArray = new Brush[numberOfPinsToGuess];
            Random rnd = new Random();
            for (int i = 0; i < numberOfPinsToGuess; i++)
            {
                int randomNumber = rnd.Next(1, 6);
                codeArray[i] = BrushesArray[randomNumber];
            }
            return codeArray;
        }

        public static void RevealCodeToGuess(Brush[] codeArray)
        {
            for (int i = 0; i < Constants.NumberOfPinsToGuess; i++)
            {
                codePinsArray[i].Fill = codeArray[i]; 
            }
        }
    }
}

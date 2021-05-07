using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MastermindScratch.Model
{
    public class Pins
    {
        public static Pin[,] pinsArray = new Pin[Constants.NumberOfTrials, Constants.NumberOfPinsToGuess];

        public static Pin GetCurrentPin()
        {
            for (int i = 0; i < Constants.NumberOfTrials; i++)
            {
                for (int j = 0; j < Constants.NumberOfPinsToGuess; j++)
                {
                    Pin pin = pinsArray[i, j];
                    if (!pin.Filled)
                    {
                        return pin;
                    }
                }
            }
            return default(Pin);
        }

        public static Pin[] GetRow(int rowNumber)
        {
            Pin[] row = new Pin[pinsArray.GetLength(1)];
            for (int i = 0; i < pinsArray.GetLength(1); i++)
            {
                row[i] = pinsArray[rowNumber, i];
            }
            return row;
        }

        public static Brush[] GetBrushRow(int rowNumber)
        {
            Brush[] brushRow = new Brush[pinsArray.GetLength(1)];
            for (int i = 0; i < pinsArray.GetLength(1); i++)
            {
                brushRow[i] = pinsArray[rowNumber, i].Ellipse.Fill;
            }
            return brushRow;

        }

        public static bool isRowCompleted(int rowNumber)
        {
            Pin[] row = GetRow(rowNumber);
            foreach(Pin pin in row)
            {
                if (!pin.Filled)
                {
                    return false;
                }
            }
            return true;
        }

        public static int[] CompareGuessAndCode(Brush[] brushesRow, Brush[] code)
        {
            int colorHits = 0;
            int fullHits = 0;
            for (int i = 0; i < code.Length; i++)
            {
                if (brushesRow[i] == code[i])
                {
                    fullHits++;
                }    
            }

            foreach (Brush br in CodeToGuess.BrushesArray)
            {
                int occurencesInCode = code.Where(x => x == br).Count();
                int occurencesInBrushesRow = brushesRow.Where(x => x == br).Count();
                colorHits += Math.Min(occurencesInCode, occurencesInBrushesRow);
            }
            colorHits -= fullHits;

            return new int[2] {fullHits, colorHits};    
        }
    }

}

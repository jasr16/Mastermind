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
        public static Ellipse[,] pinsArray = new Ellipse[Constants.NumberOfTrials, Constants.NumberOfPinsToGuess];


        public static Brush[] GetRow(int rowNumber)
        {
            Brush[] row = new Brush[pinsArray.GetLength(1)];
            for (int i = 0; i < pinsArray.GetLength(1); i++)
            {
                row[i] = pinsArray[rowNumber, i].Fill;
            }
            return row;
        }

        //public static Brush[] GetBrushesRow(Ellipse[] row)
        //{
        //    Brush[] brushesRow = new Brush[row.Length];
        //    for (int i = 0; i < row.Length; i++)
        //    {
        //        brushesRow[i] = row[i].Fill;
        //    }
        //    return brushesRow;
        //}
            

        public static bool isRowCompleted(int rowNumber)
        {
            Brush[] row = GetRow(rowNumber);
            foreach(Brush br in row)
            {
                if (br == Brushes.White)
                {
                    return false;
                }
            }
            return true;
        }

        public static int[] GetCurrentPinPosition()
        {
            int[] positions = new int[2];
            for (int i = 0; i < pinsArray.GetLength(0); i++)
            {
                for (int j = 0; j < pinsArray.GetLength(1); j++)
                {
                    Ellipse el = pinsArray[i, j];
                    if (el.Fill == Brushes.White)
                    {
                        positions[0] = i;
                        positions[1] = j;
                        return positions;
                    }
                }

            }
            return default(int[]);
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

using System;
using System.IO;
using System.Linq;
using System.Windows.Media;

namespace MastermindScratch.Model
{
    public class GuessPins
    {
        public static Pin[,] Array = new Pin[Constants.NumberOfTrials, Constants.NumberOfPinsToGuess];

        public static Pin GetCurrentPin()
        {
            for (int i = 0; i < Constants.NumberOfTrials; i++)
            {
                for (int j = 0; j < Constants.NumberOfPinsToGuess; j++)
                {
                    Pin pin = Array[i, j];
                    if (!pin.Filled)
                    {
                        return pin;
                    }
                }
            }
            return default;
        }

        public static Pin[] GetRow(int rowNumber)
        {
            Pin[] row = new Pin[Array.GetLength(1)];
            for (int i = 0; i < Array.GetLength(1); i++)
            {
                row[i] = Array[rowNumber, i];
            }
            return row;
        }

        public static Brush[] GetBrushRow(int rowNumber)
        {
            Pin[] row = GetRow(rowNumber);
            Brush[]  brushRow = row.Select(x => x.Ellipse.Fill).ToArray();
            return brushRow;

        }

        public static bool IsRowCompleted(int rowNumber)
        {
            Pin[] row = GetRow(rowNumber);
            foreach (Pin pin in row)
            {
                if (!pin.Filled)
                {
                    return false;
                }
            }
            return true;
        }


        public static Hits CompareGuessAndCode(Brush[] brushRow, Brush[] code)
        {
            //Conversions to SolidColorBrush allow comparison of the colors
            SolidColorBrush[] solidBrushRow = System.Array.ConvertAll(brushRow, item => (SolidColorBrush)item);
            SolidColorBrush[] solidBrushCode = System.Array.ConvertAll(code, item => (SolidColorBrush)item);

            int colorHits = 0;
            int fullHits = 0;
            for (int i = 0; i < code.Length; i++)
            {
                if (solidBrushRow[i].Color == solidBrushCode[i].Color)
                {
                    fullHits++;
                }
            }

            foreach (Brush br in CodeToGuess.AvailableBrushes)
            {
                SolidColorBrush solidBrush = (SolidColorBrush)br;
                int occurencesInBrushesRow = solidBrushRow.Where(x => x.Color == solidBrush.Color).Count();
                int occurencesInCode = solidBrushCode.Where(x => x.Color == solidBrush.Color).Count();              
                colorHits += Math.Min(occurencesInCode, occurencesInBrushesRow);
            }
            colorHits -= fullHits;

            return new Hits(fullHits, colorHits);
        }

        public static string EvaluateRow(int rowNumber, CodeToGuess code)
        {
            string stateOfGame = "";

            if (IsRowCompleted(rowNumber))
            {
                Brush[] currentBrushRow = GetBrushRow(rowNumber);
                Hits hits = CompareGuessAndCode(currentBrushRow, code.Colors);
                HintPins.DisplayHints(rowNumber, hits);

                if (hits.FullHits == 4)
                {
                    code.Reveal(CodePins.Array);
                    stateOfGame = "win";
                }
                else if (rowNumber == Constants.NumberOfTrials - 1)
                {
                    code.Reveal(CodePins.Array);
                    stateOfGame = "lost";
                }
            }
            return stateOfGame;

        }

        public static void SavePins(string filename)
        {
            using (StreamWriter swPins = new StreamWriter(filename))
            {
                foreach (Pin pin in Array)
                {
                    string[] pinToSave = { pin.Ellipse.Fill.ToString(), pin.Row.ToString(), pin.Column.ToString() , pin.Filled.ToString()};
                    string lineToSave = String.Join(";", pinToSave);
                    swPins.WriteLine(lineToSave);
                }
            }

        }

        public static void LoadPins(string filename)
        {
            using (StreamReader sr = new StreamReader(filename))
            {
                string line;
                while (((line = sr.ReadLine()) != null))
                {
                    string[] items = line.Split(';');
                    int i = Convert.ToInt32(items[1]);
                    int j = Convert.ToInt32(items[2]);
                    bool filled = Convert.ToBoolean(items[3]);
                    var converter = new BrushConverter();
                    Brush brush = (Brush)converter.ConvertFromString(items[0]);
                    Array[i, j].Ellipse.Fill = brush;
                    Array[i, j].Filled = filled;
                }
            }
        }
    }

}

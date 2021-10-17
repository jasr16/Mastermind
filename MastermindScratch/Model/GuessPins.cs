using Mastermind.Settings;
using System;
using System.IO;
using System.Linq;
using System.Windows.Media;

namespace Mastermind.Model
{
    public class GuessPins
    {

        public Pin[,] Array;

        public GuessPins(int trials, int pinsToGuess)
        {
            Array = new Pin[trials, pinsToGuess];
        }
        public Pin GetCurrentPin()
        {
            for (int i = 0; i < Array.GetLength(0); i++)
            {
                for (int j = 0; j < Array.GetLength(1); j++)
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

        public Pin[] GetRow(int rowNumber)
        {
            Pin[] row = new Pin[Array.GetLength(1)];
            for (int i = 0; i < Array.GetLength(1); i++)
            {
                row[i] = Array[rowNumber, i];
            }
            return row;
        }

        public Brush[] GetBrushRow(int rowNumber)
        {
            Pin[] row = GetRow(rowNumber);
            Brush[]  brushRow = row.Select(x => x.Ellipse.Fill).ToArray();
            return brushRow;

        }

        public bool IsRowCompleted(int rowNumber)
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

        public string EvaluateRow(int rowNumber, CodeToGuess code, HintPins hintPins, CodePins codePins)
        {
            string stateOfGame = "";

            if (IsRowCompleted(rowNumber))
            {
                Brush[] currentBrushRow = GetBrushRow(rowNumber);
                Hits hits = CompareGuessAndCode(currentBrushRow, code.Colors);
                hintPins.DisplayHints(rowNumber, hits);

                if (hits.FullHits == Array.GetLength(1))
                {
                    code.Reveal(codePins.Array);
                    stateOfGame = "win";
                }
                else if (rowNumber == Array.GetLength(0) - 1)
                {
                    code.Reveal(codePins.Array);
                    stateOfGame = "lost";
                }
            }
            return stateOfGame;

        }

        public void SavePins(string filename)
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

        public void LoadPins(string filename)
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

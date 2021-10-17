using Mastermind.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;

namespace Mastermind.Model
{
    public class CodeToGuess
    {
        public static readonly Brush[] AvailableBrushes = new Brush[6] { Brushes.Yellow, Brushes.RoyalBlue, 
            Brushes.Red, Brushes.LightGreen, Brushes.Brown, Brushes.Orange };    

        public Brush[] Colors;

        public CodeToGuess(int pinsToGuess, int colors)
        {
            Colors = GenerateCodeToGuess(pinsToGuess, colors);
        }

        public CodeToGuess(Brush[] colorsArray)
        {
            Colors = colorsArray;
        }

        public static Brush[] GenerateCodeToGuess(int pinsToGuess, int colors)
        {
            Brush[] codeArray = new Brush[pinsToGuess];
            Random rnd = new Random();
            for (int i = 0; i < pinsToGuess; i++)
            {
                int randomNumber = rnd.Next(0, colors);
                codeArray[i] = AvailableBrushes[randomNumber];
            }
            return codeArray;
        }


        public void Reveal(Pin[] codePins)
        {
            for (int i = 0; i < Colors.Length; i++)
            {
                codePins[i].Ellipse.Fill = Colors[i]; 
            }
        }

        public void Save(string filename)
        {
            using (StreamWriter swCode = new StreamWriter(filename))
            {
                for (int i = 0; i < Colors.Length; i++)
                {
                    string[] colorToSave = { Colors[i].ToString(), i.ToString() };
                    string lineToSave = String.Join(";", colorToSave);
                    swCode.WriteLine(lineToSave);
                }
            }

        }

        public static CodeToGuess Load(string filename)
        {
            using (StreamReader sr = new StreamReader(filename))
            {
                string line;
                List<Brush> colors = new List<Brush>(); // Brush[Colors.Length];
                while (((line = sr.ReadLine()) != null))
                {
                    string[] items = line.Split(';');
                    var converter = new BrushConverter();
                    Brush color = (Brush)converter.ConvertFromString(items[0]);
                    colors.Add(color);                   
                }
                return new CodeToGuess(colors.ToArray());
            }
        }
    }
}

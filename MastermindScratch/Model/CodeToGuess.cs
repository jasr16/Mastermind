using MastermindScratch.Settings;
using System;
using System.IO;
using System.Windows.Media;

namespace MastermindScratch.Model
{
    public class CodeToGuess
    {
        public static readonly Brush[] AvailableBrushes = new Brush[6] { Brushes.Yellow, Brushes.RoyalBlue, Brushes.Red, Brushes.LightGreen, Brushes.Brown, Brushes.Orange };    

        public Brush[] Colors = new Brush[GameSettings.NumberOfPinsToGuess];

        public CodeToGuess()
        {
            Colors = GenerateCodeToGuess(GameSettings.NumberOfPinsToGuess, GameSettings.NumberOfColors);
        }

        public CodeToGuess(Brush[] colors)
        {
            Colors = colors;
        }

        public static Brush[] GenerateCodeToGuess(int numberOfPinsToGuess, int numberOfColors)
        {
            Brush[] codeArray = new Brush[numberOfPinsToGuess];
            Random rnd = new Random();
            for (int i = 0; i < numberOfPinsToGuess; i++)
            {
                int randomNumber = rnd.Next(0, numberOfColors);
                codeArray[i] = AvailableBrushes[randomNumber];
            }
            return codeArray;
        }


        public void Reveal(Pin[] codePins)
        {
            for (int i = 0; i < GameSettings.NumberOfPinsToGuess; i++)
            {
                codePins[i].Ellipse.Fill = Colors[i]; 
            }
        }

        public void Save(string filename)
        {
            using (StreamWriter swCode = new StreamWriter(filename))
            {
                for (int i = 0; i < GameSettings.NumberOfPinsToGuess; i++)
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
                Brush[] colors = new Brush[GameSettings.NumberOfPinsToGuess];
                while (((line = sr.ReadLine()) != null))
                {
                    string[] items = line.Split(';');
                    int i = Convert.ToInt32(items[1]);
                    var converter = new BrushConverter();
                    Brush color = (Brush)converter.ConvertFromString(items[0]);
                    colors[i] = color;
                    
                }
                return new CodeToGuess(colors);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace MastermindScratch.Settings
{
    public class GameSettings : INotifyPropertyChanged
    {
        public static int NumberOfTrials { get; set; }

        public static int NumberOfPinsToGuess {get; set;}

        public static int NumberOfColors { get; set; }

        // public bool ColorsRepetition { get; set; }

        public static string Filename = Path.Combine(Directory.GetCurrentDirectory(), @"Settings", "settings.xml");

        public event PropertyChangedEventHandler PropertyChanged;

        public static void Save()
        {

            if (!Directory.Exists(Path.GetDirectoryName(Filename)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Filename));
            }

            using (StreamWriter sw = new StreamWriter(Filename))
            {             
                sw.WriteLine(NumberOfTrials.ToString());
                sw.WriteLine(NumberOfPinsToGuess.ToString());
                sw.WriteLine(NumberOfColors.ToString());
            }
        }

        public static void Load()
        {
            if (File.Exists(Filename))
            {
                using (StreamReader sr = new StreamReader(Filename))
                {
                    NumberOfTrials = Convert.ToInt32(sr.ReadLine());
                    NumberOfPinsToGuess = Convert.ToInt32(sr.ReadLine());
                    NumberOfColors = Convert.ToInt32(sr.ReadLine());
                }
            }

            else 
            {
                NumberOfTrials = 10;
                NumberOfPinsToGuess = 4;
                NumberOfColors = 6;
            }
        }

    }
}

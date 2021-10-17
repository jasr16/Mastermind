using Mastermind;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Mastermind.Settings
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void ApplySettings_Click(object sender, RoutedEventArgs e)
        {
            GameSettings gameSettings = new GameSettings((int)NumberofTrialsSlider.Value,
                (int)NumberOfPinsToGuessSlider.Value, (int)NumberOfColorsSlider.Value);
            gameSettings.Save();

            foreach (Window window in Application.Current.Windows)
            {
                if (window as MainWindow != null)
                {
                    ((MainWindow)window).GenerateNewGame();
                }
            }

        }
    }
}

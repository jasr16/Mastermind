using MastermindScratch.Model;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MastermindScratch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// 
    /// </summary>
    public partial class MainWindow : Window
    {
        private CodeToGuess Code;

        private bool PlayingAllowed;

        public MainWindow()
        {
            InitializeComponent();
            GenerateNewGame();
        }

        public void GenerateNewGame()
        {
            Code = new CodeToGuess();
            PlayingAllowed = true;
            GenerateGameLayout();
        }
        public void GenerateGameLayout()
        {
            GenerateGuessPins();
            GenerateHintPins();
            GenerateCodePins();
        }

        public void GenerateGuessPins()
        {
            for (int i = 0; i < Constants.NumberOfTrials; i++)
            {
                for (int j = 0; j < Constants.NumberOfPinsToGuess; j++)
                {
                    Pin pin = new Pin(i, j, Brushes.White);
                    pin.Ellipse.Style = (Style)this.FindResource("Pin");
                    pin.Ellipse.MouseLeftButtonDown += Revert_Color_MouseLeftButtonDown;

                    theGrid.Children.Add(pin.Ellipse);
                    Grid.SetRow(pin.Ellipse, Constants.RowOffset + Constants.NumberOfTrials - 1 - i);
                    Grid.SetColumn(pin.Ellipse, Constants.ColumnOffset + j);

                    GuessPins.Array[i, j] = pin;
                }
            }
        }

        public void GenerateHintPins()
        {
            for (int i = 0; i < Constants.NumberOfTrials; i++)
            {
                StackPanel hStack = new StackPanel()
                {
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                

                for (int j = 0; j < 2; j++)
                {
                    StackPanel vStack = new StackPanel()
                    {
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    
                    for (int k = 0; k < 2; k++)
                    {
                        Pin hintPin = new Pin(i, k + (j * 2), Brushes.White);
                        hintPin.Ellipse.Style = (Style)this.FindResource("Hint");
                                                

                        vStack.Children.Add(hintPin.Ellipse);

                        HintPins.Array[i, k + (j * 2)] = hintPin;
                    }

                    hStack.Children.Add(vStack);
                }

                theGrid.Children.Add(hStack);
                Grid.SetRow(hStack, Constants.RowOffset + Constants.NumberOfTrials - 1 - i);
                Grid.SetColumn(hStack, Constants.ColumnOffset + Constants.NumberOfColors);
            }
        }

        public void GenerateCodePins()
        {

            for (int i = 0; i < Constants.NumberOfPinsToGuess; i++)
            {
                Pin codePin = new Pin(i, 0, Brushes.LightGray);
                codePin.Ellipse.Style = (Style)this.FindResource("Pin"); ;                

                theGrid.Children.Add(codePin.Ellipse);
                Grid.SetRow(codePin.Ellipse, Constants.RowOffset - 1);
                Grid.SetColumn(codePin.Ellipse, Constants.ColumnOffset + i);

                CodePins.Array[i] = codePin;
            }

        }

        public void Ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (PlayingAllowed)
            {
                Ellipse colorEllipse = (Ellipse)sender;
                Brush ellipseColor = colorEllipse.Fill;

                Pin currentPin = GuessPins.GetCurrentPin();
                if (currentPin != default)
                {
                    currentPin.Ellipse.Fill = ellipseColor;
                    currentPin.Filled = true;

                    string stateOfGame = GuessPins.EvaluateRow(currentPin.Row, Code);

                    EndGameDialog(stateOfGame);
                   
                }

            }

        }

        private void New_Game_Click(object sender, RoutedEventArgs e)
        {
            GenerateNewGame();
        }

        private void Revert_Color_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Ellipse ellipseToRevert = (Ellipse)sender;
            Pin pinToRevert = default;
            foreach (Pin pin in GuessPins.Array)
            {
                if (pin.Ellipse == ellipseToRevert)
                {
                    pinToRevert = pin;
                    break;
                }
                else 
                { pinToRevert = default; }

            }
            if (pinToRevert != default && !GuessPins.IsRowCompleted(pinToRevert.Row))
            {
                pinToRevert.Ellipse.Fill = Brushes.White;
                pinToRevert.Filled = false;
            }

        }

        private void EndGameDialog(string stateOfGame)
        {
            if (stateOfGame != "")
            {
                string message;
                switch (stateOfGame)
                {
                    case "win":
                        message = "You revealed the code! ";
                        break;
                    case "lost":
                        message = "You lost. ";
                        break;
                    default:
                        message = "";
                        break;
                }

                MessageBoxResult result = MessageBox.Show(message + "Do you like to play new game?", "End of Game", MessageBoxButton.YesNo);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        GenerateNewGame();
                        break;
                    case MessageBoxResult.No:
                        DisablePins();
                        break;
                }
            }
            
        }

        public void DisablePins()
        {
            PlayingAllowed = false;
        }

        private void Load_Game_Click(object sender, RoutedEventArgs e)
        {
            string path = "";

            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Multiselect = true,
                Filter = "Csv files (*.csv)|*.csv|All files (*.*)|*.*",
                InitialDirectory = System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"Backups\")
            };

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filename in openFileDialog.FileNames)
                    path = System.IO.Path.GetDirectoryName(filename);
            }
            if (path == "" || Directory.GetFiles(path).Length != 2)
            {
                MessageBox.Show("Backup is not complete", "Invalid backup", MessageBoxButton.OK);
            }
            else 
            {
                string codeFileName = System.IO.Path.Combine(path, "code.csv");
                Code = CodeToGuess.Load(codeFileName);

                PlayingAllowed = true;
                GenerateGameLayout();

                string pinsFileName = System.IO.Path.Combine(path, "pins.csv");
                GuessPins.LoadPins(pinsFileName);

                int rowsToEvaulate = GuessPins.GetCurrentPin() == default(Pin) ? Constants.NumberOfTrials - 1 : GuessPins.GetCurrentPin().Row;

                for (int i = 0; i <= rowsToEvaulate; i++)
                {
                    string stateOfGame = GuessPins.EvaluateRow(i, Code);
                    EndGameDialog(stateOfGame);
                }
            }
        }

        private void Save_Game_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"Backups\");
            if (saveFileDialog.ShowDialog() == true)
            {
                Directory.CreateDirectory(saveFileDialog.FileName);

                string pinsFileName = System.IO.Path.Combine(saveFileDialog.FileName, "pins.csv");
                GuessPins.SavePins(pinsFileName);

                string codeFileName = System.IO.Path.Combine(saveFileDialog.FileName, "code.csv");
                Code.Save(codeFileName);

                MessageBox.Show("Game saved", "Game saved", MessageBoxButton.OK);
            }
            

        }
    }
}

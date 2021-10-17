using Mastermind.Model;
using Mastermind.Settings;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Mastermind
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// 
    /// </summary>
    public partial class MainWindow : Window
    {
        public CodeToGuess Code;

        public Grid DynamicGrid;

        public Menu Menu;

        public bool PlayingAllowed;

        public GameSettings GameSettings;

        public GuessPins GuessPins;

        public HintPins HintPins;

        public CodePins CodePins;

        public MainWindow()
        {
            InitializeComponent();
            GenerateNewGame();
        }

        public void GenerateNewGame()
        {
            GameSettings = new GameSettings();
            Code = new CodeToGuess(GameSettings.NumberOfPinsToGuess, GameSettings.NumberOfColors);
            PlayingAllowed = true;
            GenerateGameLayout();
        }

        public void GenerateGameLayout()
        {
            GenerateGrid();
            GenerateMenu();
            GenerateColorPins();
            GenerateGuessPins();
            GenerateHintPins();
            GenerateCodePins();
        }

        public void GenerateGrid()
        {
            if (!(DynamicGrid == default(Grid)))
            {
                DynamicGrid.Children.Remove(Menu);
            }

            DynamicGrid = new Grid();

            ColumnDefinition firstGridCol = new ColumnDefinition();
            firstGridCol.Width = new GridLength(50);
            DynamicGrid.ColumnDefinitions.Add(firstGridCol);

            for (int i = 0; i < System.Math.Max(GameSettings.NumberOfColors, GameSettings.NumberOfPinsToGuess); i++)
            {
                ColumnDefinition gridCol = new ColumnDefinition();
                gridCol.Width = new GridLength(70);
                DynamicGrid.ColumnDefinitions.Add(gridCol);
            }

            for (int i = 0; i < 2; i++)
            {
                ColumnDefinition gridCol = new ColumnDefinition();
                DynamicGrid.ColumnDefinitions.Add(gridCol);
            }

            RowDefinition firstGridRow = new RowDefinition();
            firstGridRow.Height = new GridLength(20);
            DynamicGrid.RowDefinitions.Add(firstGridRow);

            for (int i = 0; i < GameSettings.NumberOfTrials + 1; i++)
            {
                RowDefinition gridRow = new RowDefinition();
                gridRow.Height = new GridLength(70);
                DynamicGrid.RowDefinitions.Add(gridRow);
            }
            
            RowDefinition lastGridRow = new RowDefinition();
            DynamicGrid.RowDefinitions.Add(lastGridRow);

            this.Content = DynamicGrid;
        }

        public void GenerateMenu()
        {
            Menu = (Menu)this.FindResource("Menu");
            DynamicGrid.Children.Add(Menu);
            Grid.SetRow(Menu, 0);
            Grid.SetColumn(Menu, 0);
            Grid.SetColumnSpan(Menu, DynamicGrid.ColumnDefinitions.Count);
        }

        public void GenerateColorPins()
        {
            for (int i = 0; i < GameSettings.NumberOfColors; i++)
            {
                Ellipse colorEllipse = new Ellipse()
                {
                    Style = (Style)this.FindResource("Pin"),
                    Fill = CodeToGuess.AvailableBrushes[i],

                };
                colorEllipse.MouseLeftButtonDown += Ellipse_MouseLeftButtonDown;
                DynamicGrid.Children.Add(colorEllipse);
                Grid.SetRow(colorEllipse, GameSettings.NumberOfTrials + Constants.RowOffset);
                Grid.SetColumn(colorEllipse, i + Constants.ColumnOffset);
            }

        }

        public void GenerateGuessPins()
        {
            GuessPins = new GuessPins(GameSettings.NumberOfTrials, GameSettings.NumberOfPinsToGuess);

            for (int i = 0; i < GameSettings.NumberOfTrials; i++)
            {
                for (int j = 0; j < GameSettings.NumberOfPinsToGuess; j++)
                {
                    Pin pin = new Pin(i, j, Brushes.White);
                    pin.Ellipse.Style = (Style)this.FindResource("Pin");
                    pin.Ellipse.MouseLeftButtonDown += Revert_Color_MouseLeftButtonDown;

                    DynamicGrid.Children.Add(pin.Ellipse);
                    Grid.SetRow(pin.Ellipse, Constants.RowOffset + GameSettings.NumberOfTrials - 1 - i);
                    Grid.SetColumn(pin.Ellipse, Constants.ColumnOffset + j);

                    GuessPins.Array[i, j] = pin;
                }
            }
        }

        public void GenerateHintPins()
        {
            HintPins = new HintPins(GameSettings.NumberOfTrials, GameSettings.NumberOfPinsToGuess);

            for (int i = 0; i < GameSettings.NumberOfTrials; i++)
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

                DynamicGrid.Children.Add(hStack);
                Grid.SetRow(hStack, Constants.RowOffset + GameSettings.NumberOfTrials - 1 - i);
                Grid.SetColumn(hStack, Constants.ColumnOffset + System.Math.Max(GameSettings.NumberOfColors, GameSettings.NumberOfPinsToGuess));
            }
        }

        public void GenerateCodePins()
        {
            CodePins = new CodePins(GameSettings.NumberOfPinsToGuess);

            for (int i = 0; i < GameSettings.NumberOfPinsToGuess; i++)
            {
                Pin codePin = new Pin(i, 0, Brushes.LightGray);
                codePin.Ellipse.Style = (Style)this.FindResource("Pin"); ;                

                DynamicGrid.Children.Add(codePin.Ellipse);
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

                    string stateOfGame = GuessPins.EvaluateRow(currentPin.Row, Code, HintPins, CodePins);

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

                int rowsToEvaulate = GuessPins.GetCurrentPin() == default(Pin) ? GameSettings.NumberOfTrials - 1 : GuessPins.GetCurrentPin().Row;

                for (int i = 0; i <= rowsToEvaulate; i++)
                {
                    string stateOfGame = GuessPins.EvaluateRow(i, Code, HintPins, CodePins);
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

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.Show();
        }
    }
}

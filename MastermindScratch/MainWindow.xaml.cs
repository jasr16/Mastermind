using MastermindScratch.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MastermindScratch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// 
    /// </summary>
    public partial class MainWindow : Window
    {
        private Brush[] Code;

        public MainWindow()
        {
            InitializeComponent();
            Code = CodeToGuess.GenerateCodeToGuess(Constants.NumberOfPinsToGuess);
            GenerateGameLayout();
        }

        public void Ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Ellipse colorEllipse = (Ellipse)sender;
            Brush ellipseColor = colorEllipse.Fill;

            Pin currentPin = Pins.GetCurrentPin();
            if (currentPin != default(Pin))
            {
                currentPin.Ellipse.Fill = ellipseColor;
                currentPin.Filled = true;

                if (Pins.isRowCompleted(currentPin.Row))
                {
                    int[] hits = Pins.CompareGuessAndCode(Pins.GetBrushRow(currentPin.Row), this.Code);
                    Hints.DisplayHints(currentPin.Row, hits);

                    if (hits[0] == 4)
                    {
                        CodeToGuess.RevealCodeToGuess(this.Code);
                        RaiseDialog(true);
                    }
                }
            }
            else
            {
                RaiseDialog(false);
            }
        }

        public void GeneratePins()
        {
            for (int i = 0; i < Constants.NumberOfTrials; i++)
            {
                for (int j = 0; j < Constants.NumberOfPinsToGuess; j++)
                {
                    Pin pin = new Pin(i, j, (Style)this.FindResource("Pin"), Revert_Color_MouseLeftButtonDown);

                    theGrid.Children.Add(pin.Ellipse);
                    Grid.SetRow(pin.Ellipse, Constants.RowOffset + Constants.NumberOfTrials - 1 - i);
                    Grid.SetColumn(pin.Ellipse, Constants.ColumnOffset + j);

                    Pins.pinsArray[i, j] = pin;
                }
            }
        }

        public void GenerateHints()
        {
            for (int i = 0; i < Constants.NumberOfTrials; i++)
            {
                StackPanel hStack = new StackPanel();
                hStack.Orientation = Orientation.Horizontal;

                for (int j = 0; j < 2; j++)
                {
                    StackPanel vStack = new StackPanel();
                    vStack.VerticalAlignment = VerticalAlignment.Center;
                    for (int k = 0; k < 2; k++)
                    {
                        Ellipse hint = new Ellipse();
                        hint.Style = (Style)this.FindResource("Hint");
                        hint.Fill = Brushes.White;

                        vStack.Children.Add(hint);

                        Hints.hintsArray[i, k + (j * 2)] = hint;
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
                Ellipse pin = new Ellipse();
                pin.Style = (Style)this.FindResource("Pin");
                pin.Fill = Brushes.LightGray;

                theGrid.Children.Add(pin);
                Grid.SetRow(pin, Constants.RowOffset - 1);
                Grid.SetColumn(pin, Constants.ColumnOffset + i);

                CodeToGuess.codePinsArray[i] = pin;
            }

        }

        public void GenerateGameLayout()
        {
            GeneratePins();
            GenerateHints();
            GenerateCodePins();
        }

        private void New_Game_Click(object sender, RoutedEventArgs e)
        {
            Code = CodeToGuess.GenerateCodeToGuess(Constants.NumberOfPinsToGuess);
            GenerateGameLayout();
        }

        private void Revert_Color_Click(object sender, RoutedEventArgs e)
        {
            Code = CodeToGuess.GenerateCodeToGuess(Constants.NumberOfPinsToGuess);
            GenerateGameLayout();
        }

        private void Revert_Color_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Ellipse ellipseToRevert = (Ellipse)sender;
            Pin pinToRevert = default(Pin);
            foreach (Pin pin in Pins.pinsArray)
            {
                if (pin.Ellipse == ellipseToRevert)
                {
                    pinToRevert = pin;
                    break;
                }
                else 
                { pinToRevert = default(Pin); }

            }
            if (pinToRevert != default(Pin) && !Pins.isRowCompleted(pinToRevert.Row))
            {
                pinToRevert.Ellipse.Fill = Brushes.White;
                pinToRevert.Filled = false;
            }

        }

        private void RaiseDialog(bool win)
        {
            string message = win ? "You revealed the code!" : "You lost.";

            MessageBoxResult result = MessageBox.Show(message + "Do you like to play new game?", "End of Game", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    Code = CodeToGuess.GenerateCodeToGuess(Constants.NumberOfPinsToGuess);
                    GenerateGameLayout();
                    break;
                case MessageBoxResult.No:
                    break;
                
            }
        }

    }
}

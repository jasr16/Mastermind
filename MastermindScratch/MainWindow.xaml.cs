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
            int[] position = Pins.GetCurrentPinPosition();
            if (position != default(int[]))
            {
                Ellipse currentEllipse = Pins.pinsArray[position[0], position[1]];
                currentEllipse.Fill = ellipseColor;

                if (Pins.isRowCompleted(position[0]))
                {
                    int[] hits = Pins.CompareGuessAndCode(Pins.GetRow(position[0]), this.Code);
                    Hints.DisplayHints(position[0], hits);

                    if (hits[0] == 4)
                    {
                        CodeToGuess.RevealCodeToGuess(this.Code);
                    }
                }
            }
        }

        public void GeneratePins()
        {
            for (int i = 0; i < Constants.NumberOfTrials; i++)
            {
                for (int j = 0; j < Constants.NumberOfPinsToGuess; j++)
                {
                    Ellipse pin = new Ellipse();
                    pin.Style = (Style)this.FindResource("Pin");
                    pin.Fill = Brushes.White;
                    pin.MouseLeftButtonDown += new MouseButtonEventHandler(Revert_Color_MouseLeftButtonDown);

                    theGrid.Children.Add(pin);
                    Grid.SetRow(pin, Constants.RowOffset + Constants.NumberOfTrials - 1 - i);
                    Grid.SetColumn(pin, Constants.ColumnOffset + j);

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
            Ellipse colorEllipse = (Ellipse)sender;
            colorEllipse.Fill = Brushes.White;

        }
    }
}

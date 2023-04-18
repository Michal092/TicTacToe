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

namespace TicTacToe
{
    public partial class MainWindow : Window
    {
        public bool SwitchPlayer { get; set; } = true;
        public int counter { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            NewGame();
        }
        public void NewGame()
        {
            SwitchPlayer = false;
            counter = 0;
            Button_0_0.Content = string.Empty;
            Button_0_1.Content = string.Empty;
            Button_0_2.Content = string.Empty;
            Button_1_0.Content = string.Empty;
            Button_1_1.Content = string.Empty;
            Button_1_2.Content = string.Empty;
            Button_2_0.Content = string.Empty;
            Button_2_1.Content = string.Empty;
            Button_2_2.Content = string.Empty;

            Button_0_0.Background = Brushes.CornflowerBlue;
            Button_0_1.Background = Brushes.CornflowerBlue;
            Button_0_2.Background = Brushes.CornflowerBlue;
            Button_1_0.Background = Brushes.CornflowerBlue;
            Button_1_1.Background = Brushes.CornflowerBlue;
            Button_1_2.Background = Brushes.CornflowerBlue;
            Button_2_0.Background = Brushes.CornflowerBlue;
            Button_2_1.Background = Brushes.CornflowerBlue;
            Button_2_2.Background = Brushes.CornflowerBlue;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SwitchPlayer ^= true;
            counter++;
            if (counter > 9)
            {
                NewGame();
                return;
            }
            var button = sender as Button;

            button.Content = SwitchPlayer ? "X" : "O";

            if (CheckIfSomePlayerWon())
            {
                counter = 9;

            }
        }
        private bool CheckIfSomePlayerWon()
        {
            if (Button_0_0.Content.ToString() != string.Empty &&
                Button_0_0.Content == Button_0_1.Content && Button_0_1.Content == Button_0_2.Content)
            {
                Button_0_0.Background = Brushes.Green;
                Button_0_1.Background = Brushes.Green;
                Button_0_2.Background = Brushes.Green;
                return true;
            }
            if (Button_1_0.Content.ToString() != string.Empty &&
                Button_1_0.Content == Button_1_1.Content && Button_1_1.Content == Button_1_2.Content)
            {
                Button_1_0.Background = Brushes.Green;
                Button_1_1.Background = Brushes.Green;
                Button_1_2.Background = Brushes.Green;
                return true;
            }
            if (Button_2_0.Content.ToString() != string.Empty &&
                Button_2_0.Content == Button_2_1.Content && Button_2_1.Content == Button_2_2.Content)
            {
                Button_2_0.Background = Brushes.Green;
                Button_2_1.Background = Brushes.Green;
                Button_2_2.Background = Brushes.Green;
                return true;
            }
            if (Button_0_0.Content.ToString() != string.Empty &&
                Button_0_0.Content == Button_1_0.Content && Button_1_0.Content == Button_2_0.Content)
            {
                Button_0_0.Background = Brushes.Green;
                Button_1_0.Background = Brushes.Green;
                Button_2_0.Background = Brushes.Green;
                return true;
            }
            if (Button_0_1.Content.ToString() != string.Empty &&
                Button_0_1.Content == Button_1_1.Content && Button_1_1.Content == Button_2_1.Content)
            {
                Button_0_1.Background = Brushes.Green;
                Button_1_1.Background = Brushes.Green;
                Button_2_1.Background = Brushes.Green;
                return true;
            }
            if (Button_0_2.Content.ToString() != string.Empty &&
                Button_0_2.Content == Button_1_2.Content && Button_1_2.Content == Button_2_2.Content)
            {
                Button_0_2.Background = Brushes.Green;
                Button_1_2.Background = Brushes.Green;
                Button_2_2.Background = Brushes.Green;
                return true;
            }
            if (Button_0_0.Content.ToString() != string.Empty &&
                Button_0_0.Content == Button_1_1.Content && Button_1_1.Content == Button_2_2.Content)
            {
                Button_0_0.Background = Brushes.Green;
                Button_1_1.Background = Brushes.Green;
                Button_2_2.Background = Brushes.Green;
                return true;
            }
            if (Button_0_2.Content.ToString() != string.Empty &&
                Button_0_2.Content == Button_1_1.Content && Button_1_1.Content == Button_2_0.Content)
            {
                Button_0_2.Background = Brushes.Green;
                Button_1_1.Background = Brushes.Green;
                Button_2_0.Background = Brushes.Green;
                return true;
            }


            return false;
        }

    }
}

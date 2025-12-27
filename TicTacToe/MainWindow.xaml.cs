using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TicTacToe
{
    public partial class MainWindow : Window
    {
        int counter = 0;
        bool switchPlayer { get; set; } = true;
        bool isOpponentPC = false;
        Button[,] gameButtons = new Button[3, 3];

        public MainWindow()
        {
            InitializeComponent();
            NewGame();
        }

        private void NewGame()
        {
            myGrid.Children.Clear();
            myGrid.RowDefinitions.Clear();
            myGrid.ColumnDefinitions.Clear();

            for (int i = 0; i < 4; i++) myGrid.RowDefinitions.Add(new RowDefinition());

            Button CreateButton(string content, Brush background, int row)
            {
                Button button = new Button
                {
                    Content = content,
                    Background = background,
                    BorderBrush = Brushes.Black,
                    FontFamily = new FontFamily("Arial Black"),
                    FontWeight = FontWeights.Bold,
                    FontSize = 22,
                    Width = 180,
                    Height = 60
                };
                Grid.SetRow(button, row);
                button.Click += ChoiceGamemode;
                return button;
            }

            myGrid.Children.Add(CreateButton("One Player", Brushes.Green, 1));
            myGrid.Children.Add(CreateButton("Two Players", Brushes.Orange, 2));
        }

        private void ChoiceGamemode(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                switch (button.Content)
                {
                    case "Two Players":
                        DrawGameboard();
                        break;
                    case "One Player":
                        isOpponentPC = true;
                        DrawGameboard();
                        break;
                }
            }
        }
        private async void DrawGameboard()
        {
            myGrid.Children.Clear();
            myGrid.RowDefinitions.Clear();
            myGrid.ColumnDefinitions.Clear();
            counter = 0;

            for (int i = 0; i < 3; i++)
            {
                myGrid.RowDefinitions.Add(new RowDefinition());
                myGrid.ColumnDefinitions.Add(new ColumnDefinition());
                for (int j = 0; j < 3; j++)
                {
                    Button button = new Button
                    {
                        FontSize = 72,
                        Background = Brushes.CornflowerBlue,
                        BorderBrush = Brushes.Black,
                    };
                    Grid.SetRow(button, i);
                    Grid.SetColumn(button, j);
                    myGrid.Children.Add(button);
                    gameButtons[i, j] = button;
                    button.Click += ButtonClick;
                }
            }
            Random rand = new Random();
            int x = rand.Next(0, 123); 
            switchPlayer = (x % 2 == 0) ? true : false;

            if (isOpponentPC && !switchPlayer)
            {
                ComputerMove();
                switchPlayer ^= true;
            }
        }

        private async void ButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button.Content != null)
                return; 

            if (!isOpponentPC)
            {
                // Tryb dwóch graczy
                button.Content = switchPlayer ? "X" : "O";
                button.Foreground = switchPlayer ? Brushes.Blue : Brushes.Red;
                switchPlayer ^= true;
                counter++;
                CheckIfSomePlayerWin();
            }
            else
            {
                if (switchPlayer)
                {
                    button.Content = "X";
                    button.Foreground = Brushes.Blue;
                    switchPlayer ^= true;
                    counter++;
                    CheckIfSomePlayerWin();
                }
                if (!switchPlayer)
                {
                    await Task.Delay(600); 
                    ComputerMove();
                    switchPlayer ^= true;
                    CheckIfSomePlayerWin();
                }      
            }
        }

        private void ComputerMove()
        {
            if (CanWinRows()) return;
            else if (CanWinColumns()) return;
            else if (CanWinDiagonals()) return;
            else if (CanBlockRows()) return;
            else if (CanBlockColumns()) return;
            else if (CanBlockDiagonals()) return;
            else if (CanBuildRows()) return;       
            else if (CanBuildColumns()) return;
            else if (CanBuildDiagonals()) return;
            else
                MakeRandomMove();
        }

        private bool CanWinRows()
        {
            List<(int row, int col)> allO = FindAllO();

            foreach (var o in allO)
            {
                int row = o.row;
                int counterO = 0;
                int emptyFields = 0;
                int emptyCol = -1;

                for (int c = 0; c < 3; c++)
                {
                    var content = gameButtons[row, c].Content as string;

                    if (content == "O")
                        counterO++;
                    else if (content == null)
                    {
                        emptyFields++;
                        emptyCol = c;
                    }
                }

                if (counterO == 2 && emptyFields == 1)
                {
                    gameButtons[row, emptyCol].Content = "O";
                    gameButtons[row, emptyCol].Foreground = Brushes.Red;
                    counter++;
                    return true;
                }
            }
            return false;
        }

        private bool CanWinColumns()
        {
            List<(int row, int col)> allO = FindAllO();

            foreach (var o in allO)
            {
                int col = o.col;
                int counterO = 0;
                int emptyFields = 0;
                int emptyRow = -1;

                for (int r = 0; r < 3; r++)
                {
                    var content = gameButtons[r, col].Content as string;

                    if (content == "O")
                        counterO++;
                    else if (content == null)
                    {
                        emptyFields++;
                        emptyRow = r;
                    }
                }

                if (counterO == 2 && emptyFields == 1)
                {
                    gameButtons[emptyRow, col].Content = "O";
                    gameButtons[emptyRow, col].Foreground = Brushes.Red;
                    counter++;
                    return true;
                }
            }
            return false;
        }
        private bool CanWinDiagonals()
        {
            int counterO = 0;
            int emptyFields = 0;
            int emptyPos = -1;

            for (int i = 0; i < 3; i++)
            {
                var content = gameButtons[i, i].Content as string;
                if (content == "O") counterO++;
                else if (content == null) { emptyFields++; emptyPos = i; }
            }

            if (counterO == 2 && emptyFields == 1)
            {
                gameButtons[emptyPos, emptyPos].Content = "O";
                gameButtons[emptyPos, emptyPos].Foreground = Brushes.Red;
                counter++;
                return true;
            }

            counterO = 0;
            emptyFields = 0;
            emptyPos = -1;

            for (int i = 0; i < 3; i++)
            {
                var content = gameButtons[i, 2 - i].Content as string;
                if (content == "O") counterO++;
                else if (content == null) { emptyFields++; emptyPos = i; }
            }

            if (counterO == 2 && emptyFields == 1)
            {
                gameButtons[emptyPos, 2 - emptyPos].Content = "O";
                gameButtons[emptyPos, 2 - emptyPos].Foreground = Brushes.Red;
                counter++;
                return true;
            }
            return false;
        }

        private bool CanBlockRows()
        {
            List<(int row, int col)> allX = FindAllX();

            foreach (var x in allX)
            {
                int row = x.row;
                int counterX = 0;
                int emptyFields = 0;
                int emptyCol = -1;

                for (int c = 0; c < 3; c++)
                {
                    var content = gameButtons[row, c].Content as string;

                    if (content == "X")
                        counterX++;
                    else if (content == null)
                    {
                        emptyFields++;
                        emptyCol = c;
                    }
                }

                if (counterX == 2 && emptyFields == 1)
                {
                    gameButtons[row, emptyCol].Content = "O";
                    gameButtons[row, emptyCol].Foreground = Brushes.Red;
                    counter++;
                    return true;
                }
            }
            return false;
        }

        private bool CanBlockColumns()
        {
            List<(int row, int col)> allX = FindAllX();

            foreach (var x in allX)
            {
                int col = x.col;
                int counterX = 0;
                int emptyFields = 0;
                int emptyRow = -1;

                for (int r = 0; r < 3; r++)
                {
                    var content = gameButtons[r, col].Content as string;

                    if (content == "X")
                        counterX++;
                    else if (content == null)
                    {
                        emptyFields++;
                        emptyRow = r;
                    }
                }

                if (counterX == 2 && emptyFields == 1)
                {
                    gameButtons[emptyRow, col].Content = "O";
                    gameButtons[emptyRow, col].Foreground = Brushes.Red;
                    counter++;
                    return true;
                }
            }

            return false;
        }

        private bool CanBlockDiagonals()
        {
            int counterX = 0;
            int emptyFields = 0;
            int emptyPos = -1;

            for (int i = 0; i < 3; i++)
            {
                var content = gameButtons[i, i].Content as string;
                if (content == "X") counterX++;
                else if (content == null) { emptyFields++; emptyPos = i; }
            }

            if (counterX == 2 && emptyFields == 1)
            {
                gameButtons[emptyPos, emptyPos].Content = "O";
                gameButtons[emptyPos, emptyPos].Foreground = Brushes.Red;
                counter++;
                return true;
            }

            counterX = 0;
            emptyFields = 0;
            emptyPos = -1;

            for (int i = 0; i < 3; i++)
            {
                var content = gameButtons[i, 2 - i].Content as string;
                if (content == "X") counterX++;
                else if (content == null) { emptyFields++; emptyPos = i; }
            }

            if (counterX == 2 && emptyFields == 1)
            {
                gameButtons[emptyPos, 2 - emptyPos].Content = "O";
                gameButtons[emptyPos, 2 - emptyPos].Foreground = Brushes.Red;
                counter++;
                return true;
            }
            return false;
        }

        private bool CanBuildRows()
        {
            for (int r = 0; r < 3; r++)
            {
                int counterO = 0;
                int emptyFields = 0;
                int emptyCol = -1;

                for (int c = 0; c < 3; c++)
                {
                    var content = gameButtons[r, c].Content as string;

                    if (content == "O") counterO++;
                    else if (content == null)
                    {
                        emptyFields++;
                        emptyCol = c;
                    }
                }

                if (counterO == 1 && emptyFields == 2)
                {
                    gameButtons[r, emptyCol].Content = "O";
                    gameButtons[r, emptyCol].Foreground = Brushes.Red;
                    counter++;
                    return true;
                }
            }
            return false;
        }

        private bool CanBuildColumns()
        {
            for (int c = 0; c < 3; c++)
            {
                int counterO = 0;
                int emptyFields = 0;
                int emptyRow = -1;

                for (int r = 0; r < 3; r++)
                {
                    var content = gameButtons[r, c].Content as string;

                    if (content == "O") counterO++;
                    else if (content == null)
                    {
                        emptyFields++;
                        emptyRow = r;
                    }
                }

                if (counterO == 1 && emptyFields == 2)
                {
                    gameButtons[emptyRow, c].Content = "O";
                    gameButtons[emptyRow, c].Foreground = Brushes.Red;
                    counter++;
                    return true;
                }
            }
            return false;
        }

        private bool CanBuildDiagonals()
        {
            int counterO = 0, empty = 0, pos = -1;
            for (int i = 0; i < 3; i++)
            {
                var c = gameButtons[i, i].Content as string;
                if (c == "O") counterO++;
                else if (c == null) { empty++; pos = i; }
            }
            if (counterO == 1 && empty == 2)
            {
                gameButtons[pos, pos].Content = "O";
                gameButtons[pos, pos].Foreground = Brushes.Red;
                counter++;
                return true;
            }

            counterO = 0; empty = 0; pos = -1;
            for (int i = 0; i < 3; i++)
            {
                var c = gameButtons[i, 2 - i].Content as string;
                if (c == "O") counterO++;
                else if (c == null) { empty++; pos = i; }
            }
            if (counterO == 1 && empty == 2)
            {
                gameButtons[pos, 2 - pos].Content = "O";
                gameButtons[pos, 2 - pos].Foreground = Brushes.Red;
                counter++;
                return true;
            }

            return false;
        }

        private void MakeRandomMove()
        {
            var empty = new List<(int r, int c)>();

            for (int r = 0; r < 3; r++)
                for (int c = 0; c < 3; c++)
                    if (gameButtons[r, c].Content == null)
                        empty.Add((r, c));

            if (empty.Count == 0) return;

            if (gameButtons[1, 1].Content == null)
            {
                gameButtons[1, 1].Content = "O";
                gameButtons[1, 1].Foreground = Brushes.Red;
                counter++;
                return;
            }
            else if (gameButtons[0, 0].Content == null)
            {
                gameButtons[0, 0].Content = "O";
                gameButtons[0, 0].Foreground = Brushes.Red;
                counter++;
                return;
            }
            else if (gameButtons[0, 2].Content == null)
            {
                gameButtons[0, 2].Content = "O";
                gameButtons[0, 2].Foreground = Brushes.Red;
                counter++;
                return;
            }
            else if (gameButtons[2, 0].Content == null)
            {
                gameButtons[2, 0].Content = "O";
                gameButtons[2, 0].Foreground = Brushes.Red;
                counter++;
                return;
            }
            else if (gameButtons[2, 2].Content == null)
            {
                gameButtons[2, 2].Content = "O";
                gameButtons[2, 2].Foreground = Brushes.Red;
                counter++;
                return;
            }
            else
            {
                var rand = new Random();
                var (row, col) = empty[rand.Next(empty.Count)];

                gameButtons[row, col].Content = "O";
                gameButtons[row, col].Foreground = Brushes.Red;
                counter++;
            }
        }

        private void CheckIfSomePlayerWin()
        {
            for (int i = 0; i < 3; i++)
            {
                if (gameButtons[i, 0].Content != null &&
                    gameButtons[i, 0].Content == gameButtons[i, 1].Content &&
                    gameButtons[i, 1].Content == gameButtons[i, 2].Content)
                {
                    gameButtons[i, 0].Background = Brushes.LightGreen;
                    gameButtons[i, 1].Background = Brushes.LightGreen;
                    gameButtons[i, 2].Background = Brushes.LightGreen;
                    MessageBox.Show($"Player {gameButtons[i, 0].Content} wins!");
                    DrawGameboard();
                    return;
                }
                if (gameButtons[0, i].Content != null &&
                    gameButtons[0, i].Content == gameButtons[1, i].Content &&
                    gameButtons[1, i].Content == gameButtons[2, i].Content)
                {
                    gameButtons[0, i].Background = Brushes.LightGreen;
                    gameButtons[1, i].Background = Brushes.LightGreen;
                    gameButtons[2, i].Background = Brushes.LightGreen;
                    MessageBox.Show($"Player {gameButtons[0, i].Content} wins!");
                    DrawGameboard();
                    return;
                }
            }

            if (gameButtons[0, 0].Content != null &&
                gameButtons[0, 0].Content == gameButtons[1, 1].Content &&
                gameButtons[1, 1].Content == gameButtons[2, 2].Content)
            {
                gameButtons[0, 0].Background = Brushes.LightGreen;
                gameButtons[1, 1].Background = Brushes.LightGreen;
                gameButtons[2, 2].Background = Brushes.LightGreen;
                MessageBox.Show($"Player {gameButtons[0, 0].Content} wins!");
                DrawGameboard();
                return;
            }
            if (gameButtons[0, 2].Content != null &&
                gameButtons[0, 2].Content == gameButtons[1, 1].Content &&
                gameButtons[1, 1].Content == gameButtons[2, 0].Content)
            {
                gameButtons[0, 2].Background = Brushes.LightGreen;
                gameButtons[1, 1].Background = Brushes.LightGreen;
                gameButtons[2, 0].Background = Brushes.LightGreen;
                MessageBox.Show($"Player {gameButtons[0, 2].Content} wins!");
                DrawGameboard();
                return;
            }

            if (counter == 9)
            {
                MessageBox.Show("It's a draw!");
                DrawGameboard();
            }
        }

        private List<(int row, int col)> FindAllX()
        {
            List<(int, int)> result = new();

            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    if (gameButtons[r, c].Content as string == "X")
                    {
                        result.Add((r, c));
                    }
                }
            }

            return result;
        }

        private List<(int row, int col)> FindAllO()
        {
            List<(int, int)> result = new();

            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    if (gameButtons[r, c].Content as string == "O")
                    {
                        result.Add((r, c));
                    }
                }
            }
            return result;
        }
    }
}
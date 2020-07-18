namespace Green_vs_Red_Assignment
{
    using Green_vs_Red_Assignment.Contracts;
    using Green_vs_Red_Assignment.Enums;
    using Green_vs_Red_Assignment.Implementations;

    using System;
    using System.Linq;
    using System.Collections.Generic;
    public class Grid
    {
        // i was thinking to make class Cell and make Cell[][], instead of char[][]
        // but in this particular case i dont think its neccessary
        private char[][] grid;
        private IReader reader;
        private IWriter writer;

        // the key is coordinates, value is change that needs to be applied on the next turn
        static Dictionary<string, Color> ChangesTracker = new Dictionary<string, Color>();

        // if a green cell is surrounded by some ot these number of green cells
        // we are allowed to change the cell
        static int[] availableSurroundingsCountGreenCells = new int[]
           {
                0, 1, 4, 5, 7, 8
           };

        static int[] availableSurroundingsCountRedCells = new int[]
            {
                3, 6
            };

        // X and Y are coordinates of the cell,
        // that we are tracking for changes
        private int X;
        private int Y;

        // how many times we gonna track changes on desired cell
        private int Turns;

        // count changes on the desired cell
        private int ChangesCount;

        private Grid(ConsoleWorker console)
        {
            this.reader = console;
            this.writer = console;

            this.InitializeGrid();
        }

        // make instance of the Grid only once
        // because this constructor is gonna be called only one time
        // i am guaranteed that this ConsoleWorker will be instantiated only once
        public static Grid Instance { get; } = new Grid(new ConsoleWorker());

        public void Calculate()
        {
            for (int i = 0; i < this.Turns; i++)
            {
                // on every turn we're checking if all elements are equal to zero
                bool result = CheckIfAllElementsAreZeros();

                // if result is true, then desired cell wont be changing anymore and we exit the program
                if (result)
                {
                    this.PrintResult();
                    return;
                }

                CheckCells();
                ApplyChanges();
                CountChanges();
            }

            this.PrintResult();
        }

        private bool CheckIfAllElementsAreZeros()
        {
            char firstElement = grid[0][0];

            if (firstElement != '0')
            {
                return false;
            }

            bool areEqual = true;

            for (int i = 0; i < grid.Length; i++)
            {
                areEqual = Array.TrueForAll<char>(grid[i], x => x == firstElement);

                if(!areEqual)
                {
                    return false;
                }
            }

            return true;
        }

        // this method is iterating through all elements in the matrix
        private void CheckCells()
        {
            for (int i = 0; i < grid.Length; i++)
            {
                for (int j = 0; j < grid[i].Length; j++)
                {
                    if (grid[i][j] == '0')
                    {
                        // if the current cell is red, this method is called
                        CheckRedCellSurroundings(i, j);
                    }
                    else if(grid[i][j] == '1')
                    {
                        // if the current cell is green, this method is called
                        CheckGreenCellSurroundings(i, j);
                    }
                }
            }
        }

        private void CheckRedCellSurroundings(int row, int col)
        {
            int currentCount = 0;

            if (row - 1 >= 0)
            {
                if (grid[row - 1][col] == '1')
                {
                    currentCount++;
                }

                if (col - 1 >= 0)
                {
                    if (grid[row - 1][col - 1] == '1')
                    {
                        currentCount++;
                    }
                }

                if (col + 1 < grid[row].Length)
                {
                    if (grid[row - 1][col + 1] == '1')
                    {
                        currentCount++;
                    }
                }


            }

            if (col - 1 >= 0)
            {
                if (grid[row][col - 1] == '1')
                {
                    currentCount++;
                }
            }

            if (col + 1 < grid[row].Length)
            {
                if (grid[row][col + 1] == '1')
                {
                    currentCount++;
                }
            }

            if (row + 1 < grid.Length)
            {
                if (grid[row + 1][col] == '1')
                {
                    currentCount++;
                }

                if (col - 1 >= 0)
                {
                    if (grid[row + 1][col - 1] == '1')
                    {
                        currentCount++;
                    }
                }

                if (col + 1 < grid[row + 1].Length)
                {
                    if (grid[row + 1][col + 1] == '1')
                    {
                        currentCount++;
                    }
                }
            }

            if (availableSurroundingsCountRedCells.Contains(currentCount))
            {
                this.AddChanges(row, col, Color.Green);
            }
        }

        private void CheckGreenCellSurroundings(int row, int col)
        {
            int currentCount = 0;

            // checking previous row
            if (row - 1 >= 0)
            {
                if (grid[row - 1][col] == '1')
                {
                    currentCount++;
                }

                if (col - 1 >= 0)
                {

                    if (grid[row - 1][col - 1] == '1')
                    {
                        currentCount++;
                    }
                }

                if (col + 1 < grid[row].Length)
                {
                    if (grid[row - 1][col + 1] == '1')
                    {
                        currentCount++;
                    }
                }

            }

            // checking current row
            if (col - 1 >= 0)
            {
                if (grid[row][col - 1] == '1')
                {
                    currentCount++;
                }
            }

            if (col + 1 < grid[row].Length)
            {
                if (grid[row][col + 1] == '1')
                {
                    currentCount++;
                }
            }

            //checking next row
            if (row + 1 < grid.Length)
            {
                if (grid[row + 1][col] == '1')
                {
                    currentCount++;
                }

                if (col - 1 >= 0)
                {
                    if (grid[row + 1][col - 1] == '1')
                    {
                        currentCount++;
                    }
                }

                if (col + 1 < grid[row].Length)
                {
                    if (grid[row + 1][col + 1] == '1')
                    {
                        currentCount++;
                    }
                }
            }


            if (availableSurroundingsCountGreenCells.Contains(currentCount))
            {
                this.AddChanges(row, col, Color.Red);
            }
        }

        private void AddChanges(int row, int col, Color color)
        {
            string coordinates = row.ToString() + ", " + col.ToString();
            ChangesTracker.Add(coordinates, color);
        }

        private void ApplyChanges()
        {
            foreach (var kvp in ChangesTracker)
            {
                var key = kvp.Key.Split(", ").ToArray();

                var row = int.Parse(key[0]);
                var col = int.Parse(key[1]);
                var color = kvp.Value;

                grid[row][col] = color == Color.Green ? '1' : '0';
            }


            // after applying changes we reset the ChangesTracker,
            // so on the next turn will be empty and ready to add new changes
            ChangesTracker.Clear();
        }

        private void CountChanges()
        {
            if (this.grid[this.X][this.Y] == '1')
            {
                ChangesCount++;
            }
        }

        private void InitializeGrid()
        {
            var gridSize = this.reader.ReadSize();
            var rows = gridSize[0];

            this.grid = new char[rows][];

            for (int i = 0; i < rows; i++)
            {
                grid[i] = this.reader.ReadRow();
            }

            var outputCondition = this.reader.ReadOutputCondition();
            this.X = outputCondition[0];
            this.Y = outputCondition[1];
            this.Turns = outputCondition[2];
        }

        private void PrintResult()
        {
            this.writer.Write(this.ChangesCount);
        }

    }
}

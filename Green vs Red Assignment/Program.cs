using System;
using System.Collections.Generic;
using System.Linq;

namespace Green_vs_Red_Assignment
{
    class Program
    {
        // the key is coordinates, value is change that needs to be applied on the next turn;
        static Dictionary<string, Color> GridChangesForNextTurn = new Dictionary<string, Color>();
        static int[] avalaibleSurroundingsCount = new int[]
            {
                0, 1, 4, 5, 7, 8
            };


        private static int ChangesCount = 0;

        static void Main(string[] args)
        {
            var gridSize = Console.ReadLine().Split(", ").Select(int.Parse).ToArray();

            var rows = gridSize[0];
            var cols = gridSize[1];

            char[][] grid = new char[rows][];

            // initialize the grid
            for (int i = 0; i < rows; i++)
            {
                grid[i] = Console.ReadLine().ToCharArray();
            }

            ;

            var outputCondition = Console.ReadLine().Split(", ").Select(int.Parse).ToArray();


            // row and col are coordinates in the grid
            var row = outputCondition[0]; var col = outputCondition[1];
            var turns = outputCondition[2];



            for (int i = 0; i < turns; i++)
            {
                // getting color of needed cell
                Color color = grid[row][col] == '1' ? Color.Green : Color.Red;

                //// applying the first rule
                //if (grid[0][0] == '0')
                //{
                //    // if red cell is surrounded by exactly 3 green cells
                //    if (grid[0][1] == '1' && grid[1][0] == '1' && grid[1][1] == '1')
                //    {
                //        // these are coordinates because 0, 0 is current position
                //        string coordinates = "0, 0";
                //        GridChangesForNextTurn.Add(coordinates, Color.Green);
                //    }
                //}

                //if (grid[0][cols - 1] == '0')
                //{
                //    if (grid[0][cols - 2] == '1' && grid[1][cols - 2] == '1' && grid[1][cols - 1] == '1')
                //    {
                //        string coordinates = "0" + ", " + (cols - 1).ToString();
                //        GridChangesForNextTurn.Add(coordinates, Color.Green);
                //    }
                //}

                //if (grid[rows - 1][0] == '0')
                //{
                //    if (grid[rows - 2][0] == '1' && grid[rows - 2][1] == '1' && grid[rows - 1][1] == '1')
                //    {
                //        string coordinates = (rows - 1).ToString() + ", " + "0";
                //        GridChangesForNextTurn.Add(coordinates, Color.Green);
                //    }
                //}

                //if (grid[rows - 1][cols - 1] == '0')
                //{
                //    if (grid[rows - 1][cols - 1] == '1' && grid[rows - 2][cols - 2] == '1' && grid[rows - 2][cols - 1] == '1')
                //    {
                //        string coordinates = (rows - 1).ToString() + ", " + (cols - 1).ToString();
                //        GridChangesForNextTurn.Add(coordinates, Color.Green);
                //    }
                //}

                CheckIfThereAreExactly6GreenCells(grid);

                // check red cells
                CheckGreenCells(grid);

                ApplyGridChanges(grid);

                if (grid[row][col] == '1')
                {
                    ChangesCount++;
                }
            }

            Console.WriteLine(ChangesCount);
        }

        private static void ApplyGridChanges(char[][] grid)
        {
            foreach (var kvp in GridChangesForNextTurn)
            {
                var key = kvp.Key.Split(", ").ToArray();

                var row = int.Parse(key[0]);
                var col = int.Parse(key[1]);
                var color = kvp.Value;

                grid[row][col] = color == Color.Green ? '1' : '0';
            }

            GridChangesForNextTurn.Clear();
        }

        private static void CheckGreenCells(char[][] grid)
        {
            // for every single cells checks if its surround be exactly 0, 1, 4, 5, 7, 8

            for (int i = 0; i < grid.Length; i++)
            {
                for (int j = 0; j < grid[i].Length; j++)
                {
                    if (grid[i][j] == '1')
                    {
                        CheckGreenCellSurroundings(grid, i, j);
                    }
                }
            }
        }

        private static void CheckGreenCellSurroundings(char[][] grid, int row, int col)
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


            if (avalaibleSurroundingsCount.Contains(currentCount))
            {
                string coordinates = row.ToString() + ", " + col.ToString();
                GridChangesForNextTurn.Add(coordinates, Color.Red);

            }

        }

        private static void CheckIfThereAreExactly6GreenCells(char[][] grid)
        {
            // for example: if original matrix is 5 x 5,
            // the cells where is possible to be surrounded by 6 cells
            // are going to be all cells inside inner matrix with size 3 x 3

            // if original matrix is 3 x 3,
            // the inner will be 1 x 1 etc we have to check only one cell if it surrounded by 6 cells;
            //var sizeOfInnerMatrix = grid[0].Length - 2;


            //for (int i = 1; i <= sizeOfInnerMatrix; i++)
            //{
            //    for (int j = 1; j <= sizeOfInnerMatrix; j++)
            //    {
            //        if (grid[i][j] == '0')
            //        {
            //            //check every cell in the inner matrix
            //            CheckRedCellSurroundings(grid, i, j);
            //        }
            //    }
            //}

            for (int i = 0; i < grid.Length; i++)
            {
                for (int j = 0; j < grid[i].Length; j++)
                {
                    if (grid[i][j] == '0')
                    {
                        CheckRedCellSurroundings(grid, i, j);
                    }
                }
            }

        }

        private static void CheckRedCellSurroundings(char[][] grid, int row, int col)
        {
            //int greenCells = 0;

            //if (grid[row - 1][col - 1] == '1')
            //{
            //    greenCells++;
            //}

            //if (grid[row - 1][col] == '1')
            //{
            //    greenCells++;
            //}

            //if (grid[row - 1][col + 1] == '1')
            //{
            //    greenCells++;
            //}

            //if (grid[row][col - 1] == '1')
            //{
            //    greenCells++;
            //}

            //if (grid[row][col + 1] == '1')
            //{
            //    greenCells++;
            //}

            //if (grid[row + 1][col - 1] == '1')
            //{
            //    greenCells++;
            //}

            //if (grid[row + 1][col] == '1')
            //{
            //    greenCells++;
            //}

            //if (grid[row + 1][col + 1] == '1')
            //{
            //    greenCells++;
            //}


            //if (greenCells == 6)
            //{
            //    string coordinates = row.ToString() + ", " + col.ToString();
            //    GridChangesForNextTurn.Add(coordinates, Color.Green);
            //}

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

                if(col - 1 >= 0)
                {
                    if(grid[row + 1][col - 1] == '1')
                    {
                        currentCount++;
                    }
                }

                if(col + 1 < grid[row + 1].Length)
                {
                    if(grid[row + 1][col + 1] == '1')
                    {
                        currentCount++;
                    }
                }
            }

            if(currentCount == 3 || currentCount == 6)
            {
                string coordinates = row.ToString() + ", " + col.ToString();
                GridChangesForNextTurn.Add(coordinates, Color.Green);
            }


        }
    }

    enum Color
    {
        Green,
        Red
    }
}

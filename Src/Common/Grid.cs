using System;

namespace Common;

public class Grid
{
    // Define the size of the grid
    private int rows;
    private int cols;
    private string[,] grid;

    public Grid(int rows, int cols)
    {
        this.rows = rows;
        this.cols = cols;
        grid = new string[rows, cols];
    }

    public void SetRows(int rows)
    {
        this.rows = rows;
        grid = new string[rows, cols];
    }

    public void SetCols(int cols)
    {
        this.cols = cols;
        grid = new string[rows, cols];
    }

    // Indexer to allow setting and getting values in the grid
    public string this[int row, int col]
    {
        get => grid[row, col];
        set => grid[row, col] = value;
    }

    // Method to print the grid
    public void PrintGrid()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Console.Write(grid[i, j] + "\t");
            }
            Console.WriteLine();
        }
    }
}
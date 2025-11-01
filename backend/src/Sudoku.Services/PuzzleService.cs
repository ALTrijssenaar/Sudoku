using Sudoku.Models;

namespace Sudoku.Services;

/// <summary>
/// Service for generating and validating Sudoku puzzles.
/// </summary>
public class PuzzleService : IPuzzleService
{
    private readonly Random _random = new();

    /// <summary>
    /// Generates a new Sudoku puzzle with the specified difficulty.
    /// </summary>
    public SudokuPuzzle GeneratePuzzle(DifficultyLevel difficulty)
    {
        var puzzle = new SudokuPuzzle
        {
            Difficulty = difficulty
        };

        // Generate a complete, valid solution
        puzzle.Solution = GenerateCompleteSolution();

        // Create the initial board by removing cells based on difficulty
        puzzle.InitialBoard = CreatePuzzleFromSolution(puzzle.Solution, difficulty);

        return puzzle;
    }

    /// <summary>
    /// Validates if a move is valid for the given board state.
    /// </summary>
    public bool IsValidMove(int[,] board, int row, int col, int value)
    {
        if (value < 1 || value > 9)
            return false;

        if (row < 0 || row >= 9 || col < 0 || col >= 9)
            return false;

        // Check row
        for (int j = 0; j < 9; j++)
        {
            if (j != col && board[row, j] == value)
                return false;
        }

        // Check column
        for (int i = 0; i < 9; i++)
        {
            if (i != row && board[i, col] == value)
                return false;
        }

        // Check 3x3 box
        int boxRow = (row / 3) * 3;
        int boxCol = (col / 3) * 3;
        for (int i = boxRow; i < boxRow + 3; i++)
        {
            for (int j = boxCol; j < boxCol + 3; j++)
            {
                if (i != row && j != col && board[i, j] == value)
                    return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Validates if a puzzle is complete and correct.
    /// </summary>
    public bool IsPuzzleComplete(int[,] board, int[,] solution)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (board[i, j] != solution[i, j])
                    return false;
            }
        }
        return true;
    }

    private int[,] GenerateCompleteSolution()
    {
        var board = new int[9, 9];
        FillBoard(board);
        return board;
    }

    private bool FillBoard(int[,] board)
    {
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                if (board[row, col] == 0)
                {
                    var numbers = Enumerable.Range(1, 9).OrderBy(_ => _random.Next()).ToArray();
                    foreach (var num in numbers)
                    {
                        if (IsValidMove(board, row, col, num))
                        {
                            board[row, col] = num;
                            if (FillBoard(board))
                                return true;
                            board[row, col] = 0;
                        }
                    }
                    return false;
                }
            }
        }
        return true;
    }

    private int[,] CreatePuzzleFromSolution(int[,] solution, DifficultyLevel difficulty)
    {
        var board = (int[,])solution.Clone();
        
        // Determine how many cells to remove based on difficulty
        int cellsToRemove = difficulty switch
        {
            DifficultyLevel.Easy => 30,
            DifficultyLevel.Medium => 40,
            DifficultyLevel.Hard => 50,
            _ => 30
        };

        var cells = new List<(int, int)>();
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                cells.Add((i, j));
            }
        }

        // Randomly remove cells
        cells = cells.OrderBy(_ => _random.Next()).Take(cellsToRemove).ToList();
        foreach (var (row, col) in cells)
        {
            board[row, col] = 0;
        }

        return board;
    }
}

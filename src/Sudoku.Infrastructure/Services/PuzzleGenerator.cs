using Sudoku.Core.Services;

namespace Sudoku.Infrastructure.Services;

public class PuzzleGenerator : IPuzzleGenerator
{
    private readonly Random _random = new();

    public (int[] initialCells, int[] solution) GeneratePuzzle(string difficulty)
    {
        // Generate a complete valid Sudoku solution
        int[] solution = GenerateCompleteSolution();
        
        // Create puzzle by removing cells based on difficulty
        int[] puzzle = (int[])solution.Clone();
        int cellsToRemove = GetCellsToRemove(difficulty);
        
        RemoveCells(puzzle, cellsToRemove);
        
        return (puzzle, solution);
    }

    private int[] GenerateCompleteSolution()
    {
        int[] board = new int[81];
        FillBoard(board, 0);
        return board;
    }

    private bool FillBoard(int[] board, int position)
    {
        if (position == 81)
            return true; // Board is complete

        int row = position / 9;
        int col = position % 9;

        // Create randomized list of numbers 1-9
        var numbers = Enumerable.Range(1, 9).OrderBy(_ => _random.Next()).ToList();

        foreach (int num in numbers)
        {
            if (IsValidPlacement(board, row, col, num))
            {
                board[position] = num;
                if (FillBoard(board, position + 1))
                    return true;
                board[position] = 0; // Backtrack
            }
        }

        return false;
    }

    private bool IsValidPlacement(int[] board, int row, int col, int value)
    {
        // Check row
        for (int c = 0; c < 9; c++)
        {
            if (board[row * 9 + c] == value)
                return false;
        }

        // Check column
        for (int r = 0; r < 9; r++)
        {
            if (board[r * 9 + col] == value)
                return false;
        }

        // Check 3x3 box
        int boxRow = (row / 3) * 3;
        int boxCol = (col / 3) * 3;
        for (int r = boxRow; r < boxRow + 3; r++)
        {
            for (int c = boxCol; c < boxCol + 3; c++)
            {
                if (board[r * 9 + c] == value)
                    return false;
            }
        }

        return true;
    }

    private int GetCellsToRemove(string difficulty)
    {
        return difficulty?.ToLower() switch
        {
            "easy" => 30,    // Remove 30 cells (51 given)
            "medium" => 45,  // Remove 45 cells (36 given)
            "hard" => 55,    // Remove 55 cells (26 given)
            _ => 40          // Default to medium-ish
        };
    }

    private void RemoveCells(int[] board, int count)
    {
        var positions = Enumerable.Range(0, 81).OrderBy(_ => _random.Next()).ToList();
        
        for (int i = 0; i < count && i < positions.Count; i++)
        {
            board[positions[i]] = 0;
        }
    }
}

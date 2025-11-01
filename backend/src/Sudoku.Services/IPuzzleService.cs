using Sudoku.Models;

namespace Sudoku.Services;

/// <summary>
/// Interface for puzzle generation and validation service.
/// </summary>
public interface IPuzzleService
{
    /// <summary>
    /// Generates a new Sudoku puzzle with the specified difficulty.
    /// </summary>
    SudokuPuzzle GeneratePuzzle(DifficultyLevel difficulty);

    /// <summary>
    /// Validates if a move is valid for the given board state.
    /// </summary>
    bool IsValidMove(int[,] board, int row, int col, int value);

    /// <summary>
    /// Validates if a puzzle is complete and correct.
    /// </summary>
    bool IsPuzzleComplete(int[,] board, int[,] solution);
}

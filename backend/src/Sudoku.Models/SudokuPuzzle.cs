namespace Sudoku.Models;

/// <summary>
/// Represents a Sudoku puzzle with its initial state, solution, and difficulty level.
/// </summary>
public class SudokuPuzzle
{
    /// <summary>
    /// Unique identifier for the puzzle.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// The initial board state (9x9 grid). Empty cells are represented by 0.
    /// </summary>
    public int[,] InitialBoard { get; set; } = new int[9, 9];

    /// <summary>
    /// The solution to the puzzle (9x9 grid).
    /// </summary>
    public int[,] Solution { get; set; } = new int[9, 9];

    /// <summary>
    /// Difficulty level of the puzzle.
    /// </summary>
    public DifficultyLevel Difficulty { get; set; } = DifficultyLevel.Easy;

    /// <summary>
    /// When this puzzle was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Difficulty levels for Sudoku puzzles.
/// </summary>
public enum DifficultyLevel
{
    Easy = 1,
    Medium = 2,
    Hard = 3
}

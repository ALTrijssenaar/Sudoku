namespace Sudoku.Core.Entities;

public class GameSession
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid PuzzleId { get; set; }
    public int[] CurrentState { get; set; } = new int[81];
    public DateTime StartedAt { get; set; }
    public DateTime LastSavedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string Status { get; set; } = "in-progress";

    // Navigation properties
    public User User { get; set; } = null!;
    public SudokuPuzzle Puzzle { get; set; } = null!;
}

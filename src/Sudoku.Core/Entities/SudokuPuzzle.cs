namespace Sudoku.Core.Entities;

public class SudokuPuzzle
{
    public Guid Id { get; set; }
    public string Difficulty { get; set; } = string.Empty;
    public int[] InitialCells { get; set; } = new int[81];
    public int[] Solution { get; set; } = new int[81];
    public DateTime GeneratedAt { get; set; }

    // Navigation properties
    public ICollection<GameSession> GameSessions { get; set; } = new List<GameSession>();
}

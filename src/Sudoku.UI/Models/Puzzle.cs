namespace Sudoku.UI.Models;

public class Puzzle
{
    public Guid Id { get; set; }
    public string Difficulty { get; set; } = string.Empty;
    public int[] InitialCells { get; set; } = new int[81];
    public DateTime GeneratedAt { get; set; }
}

namespace Sudoku.Core.Services;

public interface IPuzzleGenerator
{
    /// <summary>
    /// Generates a new Sudoku puzzle at the specified difficulty level.
    /// </summary>
    /// <param name="difficulty">The difficulty level (Easy, Medium, Hard)</param>
    /// <returns>A tuple containing the initial puzzle state and its solution</returns>
    (int[] initialCells, int[] solution) GeneratePuzzle(string difficulty);
}

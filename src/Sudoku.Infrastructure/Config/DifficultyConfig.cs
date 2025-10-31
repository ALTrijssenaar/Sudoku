namespace Sudoku.Infrastructure.Config;

/// <summary>
/// Configuration for Sudoku difficulty levels
/// </summary>
public static class DifficultyConfig
{
    /// <summary>
    /// Valid difficulty levels
    /// </summary>
    public static readonly string[] ValidDifficulties = { "Easy", "Medium", "Hard" };

    /// <summary>
    /// Maps difficulty to approximate number of given cells
    /// </summary>
    public static readonly Dictionary<string, int> DifficultyToGivenCells = new()
    {
        { "Easy", 45 },     // ~55% filled
        { "Medium", 35 },   // ~43% filled
        { "Hard", 25 }      // ~31% filled
    };

    /// <summary>
    /// Validates if a difficulty level is valid
    /// </summary>
    public static bool IsValidDifficulty(string? difficulty)
    {
        return !string.IsNullOrWhiteSpace(difficulty) && 
               ValidDifficulties.Contains(difficulty, StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Gets the normalized difficulty name (proper case)
    /// </summary>
    public static string? NormalizeDifficulty(string? difficulty)
    {
        if (string.IsNullOrWhiteSpace(difficulty))
            return null;

        return ValidDifficulties.FirstOrDefault(d => 
            d.Equals(difficulty, StringComparison.OrdinalIgnoreCase));
    }
}

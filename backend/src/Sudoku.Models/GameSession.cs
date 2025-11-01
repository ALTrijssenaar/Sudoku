namespace Sudoku.Models;

/// <summary>
/// Represents an active game session for a user playing a Sudoku puzzle.
/// </summary>
public class GameSession
{
    /// <summary>
    /// Unique identifier for the session.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Identifier of the user playing (optional for MVP).
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// The puzzle being played.
    /// </summary>
    public Guid PuzzleId { get; set; }

    /// <summary>
    /// The current state of the board (9x9 grid). Empty cells are represented by 0.
    /// </summary>
    public int[,] CurrentBoard { get; set; } = new int[9, 9];

    /// <summary>
    /// Current status of the game session.
    /// </summary>
    public SessionStatus Status { get; set; } = SessionStatus.InProgress;

    /// <summary>
    /// When this session started.
    /// </summary>
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// When this session was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// When this session was completed (if applicable).
    /// </summary>
    public DateTime? CompletedAt { get; set; }
}

/// <summary>
/// Status of a game session.
/// </summary>
public enum SessionStatus
{
    InProgress = 1,
    Completed = 2,
    Abandoned = 3
}

namespace Sudoku.Api.Models;

public class CreateSessionRequest
{
    public Guid PuzzleId { get; set; }
}

public class SessionResponse
{
    public Guid Id { get; set; }
    public Guid PuzzleId { get; set; }
    public int[] CurrentState { get; set; } = new int[81];
    public int[] InitialState { get; set; } = new int[81];
    public DateTime StartedAt { get; set; }
    public DateTime LastSavedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class CompleteSessionRequest
{
    public int[] Board { get; set; } = new int[81];
}

public class PuzzleResponse
{
    public Guid Id { get; set; }
    public string Difficulty { get; set; } = string.Empty;
    public int[] InitialCells { get; set; } = new int[81];
    public DateTime GeneratedAt { get; set; }
}

public class UpdateCellRequest
{
    public int Row { get; set; }
    public int Col { get; set; }
    public int Value { get; set; }
}

// Authentication models
public class RegisterRequest
{
    public string Email { get; set; } = string.Empty;
    public string? DisplayName { get; set; }
    public string Password { get; set; } = string.Empty;
}

public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class AuthResponse
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? DisplayName { get; set; }
    public string Token { get; set; } = string.Empty;
}

public class UserHistoryResponse
{
    public Guid SessionId { get; set; }
    public Guid PuzzleId { get; set; }
    public string Difficulty { get; set; } = string.Empty;
    public DateTime StartedAt { get; set; }
    public DateTime CompletedAt { get; set; }
}

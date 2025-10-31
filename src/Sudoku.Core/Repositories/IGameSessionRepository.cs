using Sudoku.Core.Entities;

namespace Sudoku.Core.Repositories;

public interface IGameSessionRepository
{
    Task<GameSession?> GetByIdAsync(Guid id);
    Task<GameSession?> GetByUserAndPuzzleAsync(Guid userId, Guid puzzleId);
    Task<IEnumerable<GameSession>> GetUserSessionsAsync(Guid userId, bool completedOnly = false);
    Task<GameSession> CreateAsync(GameSession session);
    Task UpdateAsync(GameSession session);
    Task SaveChangesAsync();
}

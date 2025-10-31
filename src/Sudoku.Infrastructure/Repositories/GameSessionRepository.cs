using Microsoft.EntityFrameworkCore;
using Sudoku.Core.Entities;
using Sudoku.Core.Repositories;
using Sudoku.Infrastructure.Data;

namespace Sudoku.Infrastructure.Repositories;

public class GameSessionRepository : IGameSessionRepository
{
    private readonly SudokuDbContext _context;

    public GameSessionRepository(SudokuDbContext context)
    {
        _context = context;
    }

    public async Task<GameSession?> GetByIdAsync(Guid id)
    {
        return await _context.GameSessions
            .Include(gs => gs.Puzzle)
            .Include(gs => gs.User)
            .FirstOrDefaultAsync(gs => gs.Id == id);
    }

    public async Task<GameSession?> GetByUserAndPuzzleAsync(Guid userId, Guid puzzleId)
    {
        return await _context.GameSessions
            .Include(gs => gs.Puzzle)
            .Where(gs => gs.UserId == userId && gs.PuzzleId == puzzleId && gs.Status == "in-progress")
            .OrderByDescending(gs => gs.LastSavedAt)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<GameSession>> GetUserSessionsAsync(Guid userId, bool completedOnly = false)
    {
        var query = _context.GameSessions
            .Include(gs => gs.Puzzle)
            .Where(gs => gs.UserId == userId);

        if (completedOnly)
        {
            query = query.Where(gs => gs.Status == "completed");
        }

        return await query
            .OrderByDescending(gs => gs.LastSavedAt)
            .ToListAsync();
    }

    public async Task<GameSession> CreateAsync(GameSession session)
    {
        await _context.GameSessions.AddAsync(session);
        await _context.SaveChangesAsync();
        return session;
    }

    public async Task UpdateAsync(GameSession session)
    {
        _context.GameSessions.Update(session);
        await _context.SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}

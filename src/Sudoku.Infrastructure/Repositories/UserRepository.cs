using Microsoft.EntityFrameworkCore;
using Sudoku.Core.Entities;
using Sudoku.Core.Repositories;
using Sudoku.Infrastructure.Data;

namespace Sudoku.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly SudokuDbContext _context;

    public UserRepository(SudokuDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User> CreateAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<IList<GameSession>> GetUserHistoryAsync(Guid userId)
    {
        return await _context.GameSessions
            .Include(s => s.Puzzle)
            .Where(s => s.UserId == userId && s.Status == "completed")
            .OrderByDescending(s => s.CompletedAt)
            .ToListAsync();
    }
}

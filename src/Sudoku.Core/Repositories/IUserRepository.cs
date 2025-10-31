using Sudoku.Core.Entities;

namespace Sudoku.Core.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(string email);
    Task<User> CreateAsync(User user);
    Task<IList<GameSession>> GetUserHistoryAsync(Guid userId);
}

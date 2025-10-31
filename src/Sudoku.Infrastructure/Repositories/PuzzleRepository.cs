using Microsoft.EntityFrameworkCore;
using Sudoku.Core.Entities;
using Sudoku.Core.Repositories;
using Sudoku.Infrastructure.Data;

namespace Sudoku.Infrastructure.Repositories;

public class PuzzleRepository : IPuzzleRepository
{
    private readonly SudokuDbContext _context;

    public PuzzleRepository(SudokuDbContext context)
    {
        _context = context;
    }

    public async Task<SudokuPuzzle?> GetByIdAsync(Guid id)
    {
        return await _context.SudokuPuzzles.FindAsync(id);
    }

    public async Task<List<SudokuPuzzle>> GetByDifficultyAsync(string difficulty)
    {
        return await _context.SudokuPuzzles
            .Where(p => p.Difficulty == difficulty)
            .OrderByDescending(p => p.GeneratedAt)
            .Take(10)
            .ToListAsync();
    }

    public async Task<SudokuPuzzle> CreateAsync(SudokuPuzzle puzzle)
    {
        _context.SudokuPuzzles.Add(puzzle);
        await _context.SaveChangesAsync();
        return puzzle;
    }
}

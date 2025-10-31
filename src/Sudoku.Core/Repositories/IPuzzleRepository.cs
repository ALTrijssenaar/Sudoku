using Sudoku.Core.Entities;

namespace Sudoku.Core.Repositories;

public interface IPuzzleRepository
{
    Task<SudokuPuzzle?> GetByIdAsync(Guid id);
    Task<List<SudokuPuzzle>> GetByDifficultyAsync(string difficulty);
    Task<SudokuPuzzle> CreateAsync(SudokuPuzzle puzzle);
}

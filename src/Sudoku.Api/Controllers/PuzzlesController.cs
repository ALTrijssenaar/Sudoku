using Microsoft.AspNetCore.Mvc;
using Sudoku.Api.Models;
using Sudoku.Core.Entities;
using Sudoku.Core.Repositories;
using Sudoku.Core.Services;

namespace Sudoku.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PuzzlesController : ControllerBase
{
    private readonly IPuzzleRepository _puzzleRepository;
    private readonly IPuzzleGenerator _puzzleGenerator;

    public PuzzlesController(IPuzzleRepository puzzleRepository, IPuzzleGenerator puzzleGenerator)
    {
        _puzzleRepository = puzzleRepository;
        _puzzleGenerator = puzzleGenerator;
    }

    /// <summary>
    /// Get puzzles by difficulty or generate a new one
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PuzzleResponse>>> GetPuzzles([FromQuery] string? difficulty = null)
    {
        if (string.IsNullOrEmpty(difficulty))
        {
            return BadRequest(new { error = "Difficulty parameter is required" });
        }

        // Validate difficulty
        if (!IsValidDifficulty(difficulty))
        {
            return BadRequest(new { error = "Invalid difficulty. Must be Easy, Medium, or Hard" });
        }

        // Try to get existing puzzles
        var puzzles = await _puzzleRepository.GetByDifficultyAsync(difficulty);
        
        // If no puzzles exist, generate one
        if (puzzles.Count == 0)
        {
            var (initialCells, solution) = _puzzleGenerator.GeneratePuzzle(difficulty);
            var newPuzzle = new SudokuPuzzle
            {
                Id = Guid.NewGuid(),
                Difficulty = difficulty,
                InitialCells = initialCells,
                Solution = solution,
                GeneratedAt = DateTime.UtcNow
            };
            
            await _puzzleRepository.CreateAsync(newPuzzle);
            puzzles = new List<SudokuPuzzle> { newPuzzle };
        }

        var response = puzzles.Select(p => new PuzzleResponse
        {
            Id = p.Id,
            Difficulty = p.Difficulty,
            InitialCells = p.InitialCells,
            GeneratedAt = p.GeneratedAt
        });

        return Ok(response);
    }

    /// <summary>
    /// Get a specific puzzle by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<PuzzleResponse>> GetPuzzle(Guid id)
    {
        var puzzle = await _puzzleRepository.GetByIdAsync(id);
        
        if (puzzle == null)
        {
            return NotFound(new { error = "Puzzle not found" });
        }

        var response = new PuzzleResponse
        {
            Id = puzzle.Id,
            Difficulty = puzzle.Difficulty,
            InitialCells = puzzle.InitialCells,
            GeneratedAt = puzzle.GeneratedAt
        };

        return Ok(response);
    }

    private bool IsValidDifficulty(string difficulty)
    {
        var validDifficulties = new[] { "Easy", "Medium", "Hard" };
        return validDifficulties.Contains(difficulty, StringComparer.OrdinalIgnoreCase);
    }
}

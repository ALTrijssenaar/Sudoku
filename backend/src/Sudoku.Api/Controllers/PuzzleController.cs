using Microsoft.AspNetCore.Mvc;
using Sudoku.Models;
using Sudoku.Services;

namespace Sudoku.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PuzzleController : ControllerBase
{
    private readonly IPuzzleService _puzzleService;
    private readonly SudokuDbContext _dbContext;
    private readonly ILogger<PuzzleController> _logger;

    public PuzzleController(
        IPuzzleService puzzleService,
        SudokuDbContext dbContext,
        ILogger<PuzzleController> logger)
    {
        _puzzleService = puzzleService;
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Generates a new Sudoku puzzle.
    /// </summary>
    [HttpPost("generate")]
    public async Task<ActionResult<PuzzleResponse>> GeneratePuzzle([FromBody] GeneratePuzzleRequest request)
    {
        try
        {
            _logger.LogInformation("Generating new puzzle with difficulty: {Difficulty}", request.Difficulty);
            
            var puzzle = _puzzleService.GeneratePuzzle(request.Difficulty);
            
            _dbContext.Puzzles.Add(puzzle);
            await _dbContext.SaveChangesAsync();

            return Ok(new PuzzleResponse
            {
                Id = puzzle.Id,
                InitialBoard = puzzle.InitialBoard,
                Difficulty = puzzle.Difficulty
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating puzzle");
            return StatusCode(500, new { message = "Failed to generate puzzle" });
        }
    }

    /// <summary>
    /// Gets an existing puzzle by ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<PuzzleResponse>> GetPuzzle(Guid id)
    {
        var puzzle = await _dbContext.Puzzles.FindAsync(id);
        
        if (puzzle == null)
        {
            return NotFound(new { message = "Puzzle not found" });
        }

        return Ok(new PuzzleResponse
        {
            Id = puzzle.Id,
            InitialBoard = puzzle.InitialBoard,
            Difficulty = puzzle.Difficulty
        });
    }

    /// <summary>
    /// Validates a move on the puzzle.
    /// </summary>
    [HttpPost("{id}/validate-move")]
    public ActionResult<ValidateMoveResponse> ValidateMove(Guid id, [FromBody] ValidateMoveRequest request)
    {
        try
        {
            var isValid = _puzzleService.IsValidMove(request.CurrentBoard, request.Row, request.Col, request.Value);
            
            return Ok(new ValidateMoveResponse
            {
                IsValid = isValid,
                Message = isValid ? "Valid move" : "Invalid move: conflicts with existing numbers"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating move");
            return BadRequest(new { message = "Invalid move request" });
        }
    }

    /// <summary>
    /// Validates if the puzzle is complete.
    /// </summary>
    [HttpPost("{id}/validate-solution")]
    public async Task<ActionResult<ValidateSolutionResponse>> ValidateSolution(Guid id, [FromBody] ValidateSolutionRequest request)
    {
        try
        {
            var puzzle = await _dbContext.Puzzles.FindAsync(id);
            
            if (puzzle == null)
            {
                return NotFound(new { message = "Puzzle not found" });
            }

            var isComplete = _puzzleService.IsPuzzleComplete(request.CurrentBoard, puzzle.Solution);
            
            return Ok(new ValidateSolutionResponse
            {
                IsComplete = isComplete,
                Message = isComplete ? "Congratulations! Puzzle solved correctly!" : "Not quite right. Keep trying!"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating solution");
            return BadRequest(new { message = "Invalid solution request" });
        }
    }
}

public record GeneratePuzzleRequest(DifficultyLevel Difficulty = DifficultyLevel.Easy);

public record PuzzleResponse
{
    public Guid Id { get; init; }
    public int[,] InitialBoard { get; init; } = new int[9, 9];
    public DifficultyLevel Difficulty { get; init; }
}

public record ValidateMoveRequest
{
    public int[,] CurrentBoard { get; init; } = new int[9, 9];
    public int Row { get; init; }
    public int Col { get; init; }
    public int Value { get; init; }
}

public record ValidateMoveResponse
{
    public bool IsValid { get; init; }
    public string Message { get; init; } = string.Empty;
}

public record ValidateSolutionRequest
{
    public int[,] CurrentBoard { get; init; } = new int[9, 9];
}

public record ValidateSolutionResponse
{
    public bool IsComplete { get; init; }
    public string Message { get; init; } = string.Empty;
}

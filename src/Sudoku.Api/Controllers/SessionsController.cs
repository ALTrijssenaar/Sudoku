using Microsoft.AspNetCore.Mvc;
using Sudoku.Api.Models;
using Sudoku.Core.Entities;
using Sudoku.Core.Repositories;
using Sudoku.Core.Services;

namespace Sudoku.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SessionsController : ControllerBase
{
    private readonly IGameSessionRepository _sessionRepository;
    private readonly IPuzzleRepository _puzzleRepository;
    private readonly BoardValidator _boardValidator;

    public SessionsController(
        IGameSessionRepository sessionRepository,
        IPuzzleRepository puzzleRepository,
        BoardValidator boardValidator)
    {
        _sessionRepository = sessionRepository;
        _puzzleRepository = puzzleRepository;
        _boardValidator = boardValidator;
    }

    /// <summary>
    /// Create a new game session from a puzzle
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<SessionResponse>> CreateSession([FromBody] CreateSessionRequest request)
    {
        // For MVP, we'll use a hardcoded user ID. In Phase 5, this will come from JWT auth
        var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

        var puzzle = await _puzzleRepository.GetByIdAsync(request.PuzzleId);
        if (puzzle == null)
        {
            return NotFound(new { error = "Puzzle not found" });
        }

        var session = new GameSession
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            PuzzleId = puzzle.Id,
            CurrentState = (int[])puzzle.InitialCells.Clone(),
            StartedAt = DateTime.UtcNow,
            LastSavedAt = DateTime.UtcNow,
            Status = "in-progress"
        };

        await _sessionRepository.CreateAsync(session);

        var response = new SessionResponse
        {
            Id = session.Id,
            PuzzleId = session.PuzzleId,
            CurrentState = session.CurrentState,
            InitialState = puzzle.InitialCells,
            StartedAt = session.StartedAt,
            LastSavedAt = session.LastSavedAt,
            CompletedAt = session.CompletedAt,
            Status = session.Status
        };

        return CreatedAtAction(nameof(GetSession), new { id = session.Id }, response);
    }

    /// <summary>
    /// Get a game session by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<SessionResponse>> GetSession(Guid id)
    {
        var session = await _sessionRepository.GetByIdAsync(id);
        if (session == null)
        {
            return NotFound(new { error = "Session not found" });
        }

        var response = new SessionResponse
        {
            Id = session.Id,
            PuzzleId = session.PuzzleId,
            CurrentState = session.CurrentState,
            InitialState = session.Puzzle.InitialCells,
            StartedAt = session.StartedAt,
            LastSavedAt = session.LastSavedAt,
            CompletedAt = session.CompletedAt,
            Status = session.Status
        };

        return Ok(response);
    }

    /// <summary>
    /// Complete a session by validating the full board
    /// </summary>
    [HttpPost("{id}/complete")]
    public async Task<ActionResult> CompleteSession(Guid id, [FromBody] CompleteSessionRequest request)
    {
        var session = await _sessionRepository.GetByIdAsync(id);
        if (session == null)
        {
            return NotFound(new { error = "Session not found" });
        }

        if (session.Status == "completed")
        {
            return BadRequest(new { error = "Session is already completed" });
        }

        // Validate the board
        if (!_boardValidator.IsValidSolution(request.Board))
        {
            return BadRequest(new { error = "Invalid solution", isValid = false });
        }

        // Check if the solution matches the puzzle's solution
        var puzzle = session.Puzzle;
        bool matchesSolution = request.Board.SequenceEqual(puzzle.Solution);

        if (!matchesSolution)
        {
            return BadRequest(new { error = "Solution does not match puzzle solution", isValid = false });
        }

        // Update session
        session.CurrentState = request.Board;
        session.CompletedAt = DateTime.UtcNow;
        session.LastSavedAt = DateTime.UtcNow;
        session.Status = "completed";

        await _sessionRepository.UpdateAsync(session);

        return Ok(new 
        { 
            message = "Puzzle completed successfully!",
            isValid = true,
            completedAt = session.CompletedAt
        });
    }
}

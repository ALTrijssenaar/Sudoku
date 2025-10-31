using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sudoku.Api.Models;
using Sudoku.Core.Repositories;

namespace Sudoku.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IUserRepository userRepository, ILogger<UsersController> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    /// <summary>
    /// Get user puzzle history (completed sessions)
    /// </summary>
    [HttpGet("{userId}/history")]
    public async Task<ActionResult<IEnumerable<UserHistoryResponse>>> GetHistory(Guid userId)
    {
        // Verify user exists
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            return NotFound(new { error = "User not found" });
        }

        // Get user's completed sessions
        var sessions = await _userRepository.GetUserHistoryAsync(userId);

        var response = sessions.Select(s => new UserHistoryResponse
        {
            SessionId = s.Id,
            PuzzleId = s.PuzzleId,
            Difficulty = s.Puzzle.Difficulty,
            StartedAt = s.StartedAt,
            CompletedAt = s.CompletedAt ?? DateTime.MinValue
        });

        return Ok(response);
    }
}

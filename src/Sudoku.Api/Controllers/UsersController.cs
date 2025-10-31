using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sudoku.Api.Models;
using Sudoku.Core.Repositories;

namespace Sudoku.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
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
        // Verify the requesting user matches the userId parameter
        var requestingUserId = GetUserIdFromClaims();
        if (requestingUserId != userId)
        {
            return Forbid(); // User can only access their own history
        }

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
            CompletedAt = s.CompletedAt!.Value // Safe since we filter for completed sessions
        });

        return Ok(response);
    }

    private Guid GetUserIdFromClaims()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException("Invalid user ID in token");
        }
        return userId;
    }
}

using System.Net;
using System.Net.Http.Json;
using Sudoku.Api.Models;

namespace Sudoku.Integration.Tests;

public class SessionLifecycleTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public SessionLifecycleTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetPuzzles_ReturnsSuccessAndPuzzle()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/puzzles?difficulty=Easy");

        // Assert
        response.EnsureSuccessStatusCode();
        var puzzles = await response.Content.ReadFromJsonAsync<List<PuzzleResponse>>();
        
        Assert.NotNull(puzzles);
        Assert.NotEmpty(puzzles);
        Assert.Equal("Easy", puzzles[0].Difficulty);
        Assert.Equal(81, puzzles[0].InitialCells.Length);
    }

    [Fact]
    public async Task GetPuzzles_WithoutDifficulty_ReturnsBadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/puzzles");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetPuzzles_InvalidDifficulty_ReturnsBadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/puzzles?difficulty=VeryHard");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task SessionLifecycle_CreateAndGet_Success()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Step 1: Get a puzzle
        var puzzlesResponse = await client.GetAsync("/api/puzzles?difficulty=Medium");
        puzzlesResponse.EnsureSuccessStatusCode();
        var puzzles = await puzzlesResponse.Content.ReadFromJsonAsync<List<PuzzleResponse>>();
        Assert.NotNull(puzzles);
        var puzzle = puzzles[0];

        // Step 2: Create a session
        var createRequest = new CreateSessionRequest { PuzzleId = puzzle.Id };
        var createResponse = await client.PostAsJsonAsync("/api/sessions", createRequest);
        
        // Assert session created
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
        var session = await createResponse.Content.ReadFromJsonAsync<SessionResponse>();
        Assert.NotNull(session);
        Assert.Equal(puzzle.Id, session.PuzzleId);
        Assert.Equal("in-progress", session.Status);
        Assert.Equal(81, session.CurrentState.Length);
        Assert.Equal(81, session.InitialState.Length);

        // Step 3: Get the session
        var getResponse = await client.GetAsync($"/api/sessions/{session.Id}");
        getResponse.EnsureSuccessStatusCode();
        var retrievedSession = await getResponse.Content.ReadFromJsonAsync<SessionResponse>();
        
        Assert.NotNull(retrievedSession);
        Assert.Equal(session.Id, retrievedSession.Id);
        Assert.Equal(session.PuzzleId, retrievedSession.PuzzleId);
    }

    [Fact]
    public async Task GetSession_NotFound_ReturnsNotFound()
    {
        // Arrange
        var client = _factory.CreateClient();
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await client.GetAsync($"/api/sessions/{nonExistentId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetPuzzle_ById_ReturnsSuccess()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Get a puzzle first
        var puzzlesResponse = await client.GetAsync("/api/puzzles?difficulty=Easy");
        var puzzles = await puzzlesResponse.Content.ReadFromJsonAsync<List<PuzzleResponse>>();
        Assert.NotNull(puzzles);
        var puzzleId = puzzles[0].Id;

        // Act - get specific puzzle
        var response = await client.GetAsync($"/api/puzzles/{puzzleId}");

        // Assert
        response.EnsureSuccessStatusCode();
        var puzzle = await response.Content.ReadFromJsonAsync<PuzzleResponse>();
        
        Assert.NotNull(puzzle);
        Assert.Equal(puzzleId, puzzle.Id);
        Assert.Equal(81, puzzle.InitialCells.Length);
    }

    [Fact]
    public async Task GetPuzzle_NotFound_ReturnsNotFound()
    {
        // Arrange
        var client = _factory.CreateClient();
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await client.GetAsync($"/api/puzzles/{nonExistentId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}

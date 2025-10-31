using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Sudoku.Api.Models;

namespace Sudoku.Integration.Tests;

public class AuthAndHistoryTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public AuthAndHistoryTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    private HttpClient CreateAuthenticatedClient(string token)
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return client;
    }

    [Fact]
    public async Task Register_WithValidData_ReturnsCreatedAndToken()
    {
        // Arrange
        var registerRequest = new RegisterRequest
        {
            Email = $"test{Guid.NewGuid()}@example.com",
            DisplayName = "Test User",
            Password = "SecurePassword123"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/register", registerRequest);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
        Assert.NotNull(authResponse);
        Assert.Equal(registerRequest.Email, authResponse.Email);
        Assert.Equal(registerRequest.DisplayName, authResponse.DisplayName);
        Assert.NotEmpty(authResponse.Token);
        Assert.NotEqual(Guid.Empty, authResponse.UserId);
    }

    [Fact]
    public async Task Register_WithDuplicateEmail_ReturnsBadRequest()
    {
        // Arrange
        var email = $"test{Guid.NewGuid()}@example.com";
        var registerRequest = new RegisterRequest
        {
            Email = email,
            Password = "SecurePassword123"
        };

        // Register first time
        await _client.PostAsJsonAsync("/api/auth/register", registerRequest);

        // Act - Try to register again with same email
        var response = await _client.PostAsJsonAsync("/api/auth/register", registerRequest);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Register_WithInvalidEmail_ReturnsBadRequest()
    {
        // Arrange
        var registerRequest = new RegisterRequest
        {
            Email = "not-an-email",
            Password = "SecurePassword123"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/register", registerRequest);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Register_WithShortPassword_ReturnsBadRequest()
    {
        // Arrange
        var registerRequest = new RegisterRequest
        {
            Email = $"test{Guid.NewGuid()}@example.com",
            Password = "short"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/register", registerRequest);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsOkAndToken()
    {
        // Arrange
        var email = $"test{Guid.NewGuid()}@example.com";
        var password = "SecurePassword123";
        
        var registerRequest = new RegisterRequest
        {
            Email = email,
            Password = password
        };
        await _client.PostAsJsonAsync("/api/auth/register", registerRequest);

        var loginRequest = new LoginRequest
        {
            Email = email,
            Password = password
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
        Assert.NotNull(authResponse);
        Assert.Equal(email, authResponse.Email);
        Assert.NotEmpty(authResponse.Token);
    }

    [Fact]
    public async Task Login_WithInvalidEmail_ReturnsUnauthorized()
    {
        // Arrange
        var loginRequest = new LoginRequest
        {
            Email = "nonexistent@example.com",
            Password = "Password123"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Login_WithInvalidPassword_ReturnsUnauthorized()
    {
        // Arrange
        var email = $"test{Guid.NewGuid()}@example.com";
        var registerRequest = new RegisterRequest
        {
            Email = email,
            Password = "CorrectPassword123"
        };
        await _client.PostAsJsonAsync("/api/auth/register", registerRequest);

        var loginRequest = new LoginRequest
        {
            Email = email,
            Password = "WrongPassword123"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetUserHistory_ReturnsEmptyListForNewUser()
    {
        // Arrange - Register user
        var registerRequest = new RegisterRequest
        {
            Email = $"test{Guid.NewGuid()}@example.com",
            Password = "SecurePassword123"
        };
        var registerResponse = await _client.PostAsJsonAsync("/api/auth/register", registerRequest);
        var authResponse = await registerResponse.Content.ReadFromJsonAsync<AuthResponse>();
        Assert.NotNull(authResponse);
        var userId = authResponse.UserId;
        var token = authResponse.Token;

        // Create authenticated client
        var authClient = CreateAuthenticatedClient(token);

        // Act - Get user history
        var historyResponse = await authClient.GetAsync($"/api/users/{userId}/history");

        // Assert
        Assert.Equal(HttpStatusCode.OK, historyResponse.StatusCode);
        var history = await historyResponse.Content.ReadFromJsonAsync<List<UserHistoryResponse>>();
        Assert.NotNull(history);
        Assert.Empty(history);
    }

    [Fact]
    public async Task GetUserHistory_WithoutAuth_ReturnsUnauthorized()
    {
        // Arrange
        var someUserId = Guid.NewGuid();

        // Act - Try to access history without authentication
        var response = await _client.GetAsync($"/api/users/{someUserId}/history");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task FullAuthAndGameFlow_RegisterLoginPlayGetSession()
    {
        // Arrange & Act
        // Step 1: Register
        var email = $"test{Guid.NewGuid()}@example.com";
        var password = "SecurePassword123";
        var registerRequest = new RegisterRequest
        {
            Email = email,
            DisplayName = "Test Player",
            Password = password
        };
        var registerResponse = await _client.PostAsJsonAsync("/api/auth/register", registerRequest);
        Assert.Equal(HttpStatusCode.Created, registerResponse.StatusCode);
        var authResponse = await registerResponse.Content.ReadFromJsonAsync<AuthResponse>();
        Assert.NotNull(authResponse);
        var userId = authResponse.UserId;
        var token = authResponse.Token;

        // Step 2: Login
        var loginRequest = new LoginRequest { Email = email, Password = password };
        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);
        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);
        var loginAuthResponse = await loginResponse.Content.ReadFromJsonAsync<AuthResponse>();
        Assert.NotNull(loginAuthResponse);
        Assert.Equal(userId, loginAuthResponse.UserId);
        Assert.NotEmpty(loginAuthResponse.Token);

        // Create authenticated client
        var authClient = CreateAuthenticatedClient(token);

        // Step 3: Get a puzzle (public endpoint, no auth needed)
        var puzzleResponse = await _client.GetAsync("/api/puzzles?difficulty=Easy");
        Assert.Equal(HttpStatusCode.OK, puzzleResponse.StatusCode);
        var puzzles = await puzzleResponse.Content.ReadFromJsonAsync<List<PuzzleResponse>>();
        Assert.NotNull(puzzles);
        Assert.NotEmpty(puzzles);

        // Step 4: Create session (requires auth)
        var createSessionRequest = new CreateSessionRequest { PuzzleId = puzzles[0].Id };
        var sessionResponse = await authClient.PostAsJsonAsync("/api/sessions", createSessionRequest);
        Assert.Equal(HttpStatusCode.Created, sessionResponse.StatusCode);
        var session = await sessionResponse.Content.ReadFromJsonAsync<SessionResponse>();
        Assert.NotNull(session);
        Assert.Equal("in-progress", session.Status);

        // Step 5: Get session (requires auth)
        var getSessionResponse = await authClient.GetAsync($"/api/sessions/{session.Id}");
        Assert.Equal(HttpStatusCode.OK, getSessionResponse.StatusCode);
        var retrievedSession = await getSessionResponse.Content.ReadFromJsonAsync<SessionResponse>();
        Assert.NotNull(retrievedSession);
        Assert.Equal(session.Id, retrievedSession.Id);

        // Assert - Full flow completed successfully
        Assert.Equal(email, authResponse.Email);
        Assert.Equal(puzzles[0].Id, session.PuzzleId);
    }
}

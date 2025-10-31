using System.Net.Http.Headers;
using System.Net.Http.Json;
using Sudoku.UI.Models;

namespace Sudoku.UI.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private string? _authToken;

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public void SetAuthToken(string token)
    {
        _authToken = token;
        _httpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", token);
    }

    public void ClearAuthToken()
    {
        _authToken = null;
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }

    // Auth endpoints
    public async Task<AuthResponse?> RegisterAsync(RegisterRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/auth/register", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<AuthResponse>();
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/auth/login", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<AuthResponse>();
    }

    // Puzzle endpoints
    public async Task<List<Puzzle>?> GetPuzzlesAsync(string difficulty)
    {
        return await _httpClient.GetFromJsonAsync<List<Puzzle>>($"/api/puzzles?difficulty={difficulty}");
    }

    public async Task<Puzzle?> GetPuzzleAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<Puzzle>($"/api/puzzles/{id}");
    }

    // Session endpoints
    public async Task<GameSession?> CreateSessionAsync(Guid puzzleId)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/sessions", new { puzzleId });
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<GameSession>();
    }

    public async Task<GameSession?> GetSessionAsync(Guid sessionId)
    {
        return await _httpClient.GetFromJsonAsync<GameSession>($"/api/sessions/{sessionId}");
    }

    public async Task<GameSession?> UpdateSessionAsync(Guid sessionId, int[] currentState)
    {
        var response = await _httpClient.PutAsJsonAsync($"/api/sessions/{sessionId}", new { currentState });
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<GameSession>();
    }

    public async Task<GameSession?> CompleteSessionAsync(Guid sessionId)
    {
        var response = await _httpClient.PostAsync($"/api/sessions/{sessionId}/complete", null);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<GameSession>();
    }

    public async Task<List<GameSession>?> GetUserSessionsAsync(Guid userId)
    {
        return await _httpClient.GetFromJsonAsync<List<GameSession>>($"/api/users/{userId}/sessions");
    }

    // User history
    public async Task<List<GameSession>?> GetUserHistoryAsync(Guid userId)
    {
        return await _httpClient.GetFromJsonAsync<List<GameSession>>($"/api/users/{userId}/history");
    }
}

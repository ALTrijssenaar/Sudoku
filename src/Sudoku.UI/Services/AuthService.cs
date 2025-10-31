using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Sudoku.UI.Models;

namespace Sudoku.UI.Services;

public class AuthService : AuthenticationStateProvider
{
    private readonly ApiService _apiService;
    private AuthResponse? _currentUser;

    public AuthService(ApiService apiService)
    {
        _apiService = apiService;
    }

    public AuthResponse? CurrentUser => _currentUser;

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var identity = _currentUser != null
            ? new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, _currentUser.UserId.ToString()),
                new Claim(ClaimTypes.Email, _currentUser.Email),
                new Claim(ClaimTypes.Name, _currentUser.DisplayName)
            }, "apiauth")
            : new ClaimsIdentity();

        var user = new ClaimsPrincipal(identity);
        return Task.FromResult(new AuthenticationState(user));
    }

    public async Task<bool> RegisterAsync(RegisterRequest request)
    {
        try
        {
            var response = await _apiService.RegisterAsync(request);
            if (response != null)
            {
                _currentUser = response;
                _apiService.SetAuthToken(response.Token);
                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
                return true;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> LoginAsync(LoginRequest request)
    {
        try
        {
            var response = await _apiService.LoginAsync(request);
            if (response != null)
            {
                _currentUser = response;
                _apiService.SetAuthToken(response.Token);
                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
                return true;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }

    public Task LogoutAsync()
    {
        _currentUser = null;
        _apiService.ClearAuthToken();
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        return Task.CompletedTask;
    }
}

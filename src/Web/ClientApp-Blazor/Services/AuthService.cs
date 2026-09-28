using System.Net.Http.Json;

namespace ClientApp.Services;

public sealed class AuthService(HttpClient httpClient)
{
    private Task? _initialization;

    public event Action? AuthenticationStateChanged;

    public bool IsAuthenticated { get; private set; }

    public bool IsLoading { get; private set; } = true;

    public Task InitializeAsync() => _initialization ??= RefreshAsync();

    public async Task<bool> LoginAsync(string email, string password)
    {
        using var response = await httpClient.PostAsJsonAsync(
            "/api/Users/login?useCookies=true",
            new LoginRequest(email, password));

        if (!response.IsSuccessStatusCode)
        {
            return false;
        }

        IsAuthenticated = true;
        IsLoading = false;
        AuthenticationStateChanged?.Invoke();
        return true;
    }

    public async Task RegisterAsync(string email, string password)
    {
        using var response = await httpClient.PostAsJsonAsync(
            "/api/Users/register",
            new RegisterRequest(email, password));

        response.EnsureSuccessStatusCode();
    }

    public async Task LogoutAsync()
    {
        using var response = await httpClient.PostAsJsonAsync("/api/Users/logout", new { });

        if (response.IsSuccessStatusCode)
        {
            IsAuthenticated = false;
            AuthenticationStateChanged?.Invoke();
        }
    }

    private async Task RefreshAsync()
    {
        try
        {
            using var response = await httpClient.GetAsync("/api/Users/info");
            IsAuthenticated = response.IsSuccessStatusCode;
        }
        catch (HttpRequestException)
        {
            IsAuthenticated = false;
        }
        finally
        {
            IsLoading = false;
            AuthenticationStateChanged?.Invoke();
        }
    }

    private sealed record LoginRequest(string Email, string Password);

    private sealed record RegisterRequest(string Email, string Password);
}

using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components;
using TaskManager_API.Contracts.DTOs;
using Microsoft.AspNetCore.Components.Authorization;
using TaskManager_Client.Services;

namespace TaskManager_Client.Clients;

public class AuthClient
{
    private readonly HttpClient _httpClient;
    private readonly AuthService _authService;
    private readonly CustomAuthStateProvider _authStateProvider;
    private readonly NavigationManager _navigation;

    public AuthClient(HttpClient httpClient, AuthService authService, AuthenticationStateProvider authStateProvider)
    {
        _httpClient = httpClient;
        _authService = authService;
        _authStateProvider = (CustomAuthStateProvider)authStateProvider;
    }
   
    public async Task<AuthResponseDTO?> LoginAsync(UserDTO request)
    {
        var response = await _httpClient.PostAsJsonAsync("auth/login", request);
        if (!response.IsSuccessStatusCode) return null;

        var result = await response.Content.ReadFromJsonAsync<AuthResponseDTO>();
        if (result != null && !string.IsNullOrEmpty(result.AccessToken))
        {
            await _authService.SetAuthDataAsync(result.AccessToken, result.RefreshToken);
            _authStateProvider.NotifyAuthStateChanged();
        }

        return result;
    }
    
    public async Task<AuthResponseDTO?> RegisterAsync(UserDTO request)
    {
        var response = await _httpClient.PostAsJsonAsync("auth/register", request);
        if (!response.IsSuccessStatusCode) return null;

        return await response.Content.ReadFromJsonAsync<AuthResponseDTO>();
    }
    
    public async Task LogoutAsync()
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", await _authService.GetAccessTokenAsync());

        await _httpClient.PostAsync("auth/logout", null);

        await _authService.ClearAuthDataAsync();
    }
    
    public async Task<AuthResponseDTO?> RefreshTokenAsync(RefreshTokenRequestDTO request)
    {
        var response = await _httpClient.PostAsJsonAsync("auth/refresh-token", request);
        if (!response.IsSuccessStatusCode) return null;

        var result = await response.Content.ReadFromJsonAsync<AuthResponseDTO>();
        if (result != null && !string.IsNullOrEmpty(result.AccessToken))
        {
            _authService.SetAuthDataAsync(result.AccessToken, result.RefreshToken);
        }

        return result;
    }
}
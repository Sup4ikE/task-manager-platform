using System.Net.Http.Headers;
using TaskManager_API.Contracts.DTOs;
using TaskManager_Client.Services;

namespace TaskManager_Client.Clients;

public class UsersClient
{
    private readonly HttpClient _httpClient;
    private readonly AuthService _authService;

    public UsersClient(HttpClient httpClient, AuthService authService)
    {
        _httpClient = httpClient;
        _authService = authService;
    }

    private async Task AddAuthHeaderAsync()
    {
        var token = await _authService.GetAccessTokenAsync();
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
        else
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }

    public async Task<List<UserResponseDTO>> GetUsersAsync()
    {
        await AddAuthHeaderAsync();
        return await _httpClient.GetFromJsonAsync<List<UserResponseDTO>>("users")
               ?? new List<UserResponseDTO>();
    }

    public async Task<UserResponseDTO> GetUserAsync(int id)
    {
        await AddAuthHeaderAsync();
        return await _httpClient.GetFromJsonAsync<UserResponseDTO>($"users/{id}")
               ?? throw new Exception("User not found");
    }

    public async Task DeleteUserAsync(int id)
    {
        await AddAuthHeaderAsync();
        await _httpClient.DeleteAsync($"users/{id}");
    }
}


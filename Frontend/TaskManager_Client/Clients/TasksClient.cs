using System.Net.Http.Headers;
using Blazored.LocalStorage;
using TaskManager_API.Contracts.DTOs;

namespace TaskManager_Client.Clients;

public class TasksClient
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;

    public TasksClient(HttpClient httpClient, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    private async Task AddAuthHeaderAsync()
    {
        var token = await _localStorage.GetItemAsync<string>("accessToken");
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<TaskDTO[]> GetTasksAsync()
    {
        await AddAuthHeaderAsync();
        return await _httpClient.GetFromJsonAsync<TaskDTO[]>("tasks") ?? [];
    }

    public async Task<TaskDTO> GetTaskAsync(int id)
    {
        await AddAuthHeaderAsync();
        return await _httpClient.GetFromJsonAsync<TaskDTO>($"tasks/{id}") 
               ?? throw new Exception("Task not found");
    }

    public async Task PostTaskAsync(TaskDTO newTaskDTO)
    {
        await AddAuthHeaderAsync();
        await _httpClient.PostAsJsonAsync("tasks", newTaskDTO);
    }

    public async Task PutTaskAsync(int id, TaskDTO updateTaskDTO)
    {
        await AddAuthHeaderAsync();
        await _httpClient.PutAsJsonAsync($"tasks/{id}", updateTaskDTO);
    }

    public async Task DeleteTaskAsync(int id)
    {
        await AddAuthHeaderAsync();
        await _httpClient.DeleteAsync($"tasks/{id}");
    }
}
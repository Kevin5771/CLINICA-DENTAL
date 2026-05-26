using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace ClinicaDental.Web.Services;

public sealed class ApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true
    };

    public ApiClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
    }

    private void AttachToken()
    {
        var token = _httpContextAccessor.HttpContext?.Session.GetString("JWToken");
        _httpClient.DefaultRequestHeaders.Authorization = null;

        if (!string.IsNullOrWhiteSpace(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<T?> GetAsync<T>(string url)
    {
        AttachToken();
        using var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            return default;

        return await response.Content.ReadFromJsonAsync<T>(JsonOptions);
    }

    public async Task<(bool Ok, string? Error, T? Data)> PostAsync<T>(string url, object payload)
    {
        AttachToken();
        using var response = await _httpClient.PostAsJsonAsync(url, payload, JsonOptions);
        var body = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            return (false, body, default);

        var data = JsonSerializer.Deserialize<T>(body, JsonOptions);
        return (true, null, data);
    }

    public async Task<(bool Ok, string? Error)> PostAsync(string url, object payload)
    {
        AttachToken();
        using var response = await _httpClient.PostAsJsonAsync(url, payload, JsonOptions);
        var body = await response.Content.ReadAsStringAsync();

        return response.IsSuccessStatusCode
            ? (true, null)
            : (false, body);
    }

    public async Task<(bool Ok, string? Error, T? Data)> PutAsync<T>(string url, object payload)
    {
        AttachToken();
        using var response = await _httpClient.PutAsJsonAsync(url, payload, JsonOptions);
        var body = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            return (false, body, default);

        var data = JsonSerializer.Deserialize<T>(body, JsonOptions);
        return (true, null, data);
    }

    public async Task<(bool Ok, string? Error)> PutAsync(string url, object payload)
    {
        AttachToken();
        using var response = await _httpClient.PutAsJsonAsync(url, payload, JsonOptions);
        var body = await response.Content.ReadAsStringAsync();

        return response.IsSuccessStatusCode
            ? (true, null)
            : (false, body);
    }
}
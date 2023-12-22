using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Selah.Infrastructure.Services.Extensions;

public static class HttpClientExtensions
{
    public static async Task<HttpResponseMessage> GetAsync(this HttpClient httpClient, string servicePath,
        string token = "")
    {
        if (!string.IsNullOrEmpty(token))
        {
            httpClient.AddBearerToken(token);
        }

        Uri uri = new Uri($"{httpClient.BaseAddress}/{servicePath}");
        return await httpClient.GetAsync(uri);
    }

    public static async Task<HttpResponseMessage> PostAsync<T>(this HttpClient httpClient, T data, string servicePath) where T : class
    {
        var body = JsonSerializer.Serialize(data);
        var httpContent = new StringContent(body, Encoding.UTF8, "application/json");

        Uri uri = new Uri($"{httpClient.BaseAddress}/{servicePath}");
        
        return await httpClient.PostAsync(uri, httpContent);
    }
    
    public static async Task<HttpResponseMessage> PutAsync<T>(this HttpClient httpClient, T data, string servicePath) where T : class
    {
        var body = JsonSerializer.Serialize(data);
        var httpContent = new StringContent(body, Encoding.UTF8, "application/json");

        Uri uri = new Uri($"{httpClient.BaseAddress}/{servicePath}");
        
        return await httpClient.PutAsync(uri, httpContent);
    }

    private static void AddBearerToken(this HttpClient httpClient, string token)
    {
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}
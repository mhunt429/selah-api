using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Selah.Application.Services.Interfaces;

namespace Selah.Application.Services
{
  public class HttpService: IHttpService
  { 
    private readonly ILogger _logger;
    private bool disposed;
    private HttpClient _httpClient;
    public HttpService(ILogger<HttpService> logger)
    {
        _logger = logger;
        _httpClient = new HttpClient
        {
           // BaseAddress = new Uri(baseUri)
        };
        _httpClient.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
        
    }
    public void AddBearerToken(string token)
    {
        if (_httpClient == null)
        {
            throw new InvalidOperationException("Invalid REST configuration");
        }
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public void AddApiKeyHeader(string apiKey)
    {
      if (_httpClient == null)
      {
        throw new InvalidOperationException("Invalid REST configuration");
      }
      _httpClient.DefaultRequestHeaders.Add("X-API-KEY", apiKey);
    }

    /// <summary>
    /// Catch all headers. I'm choosing to key auth headers as separate methods
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>

    public void AddRequestHeader(string key, string value)
    {
        if (_httpClient == null)
        {
            throw new InvalidOperationException("Invalid REST configuration");
        }
        _httpClient.DefaultRequestHeaders.Add(key, value);
    }

    public async Task<(T, HttpResponseMessage)> GetAsync<T>(Uri servicePath)
    {
        T result;

        _logger.LogInformation($"Sending GET request to {servicePath}");

        var response = await _httpClient.GetAsync(servicePath);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError($"{servicePath} responded with an error: Code: {response.StatusCode} | Message: {response.ReasonPhrase}");
        }

        var jsonString = await response.Content.ReadAsStringAsync();
        _logger.LogDebug($"Response Object: {jsonString}");
        result = JsonConvert.DeserializeObject<T>(jsonString);

        return (result, response);
    }

    public async Task<(T, HttpResponseMessage)> PostAsync<T>(object data, Uri servicePath)
    {
        var body = JsonConvert.SerializeObject(data, new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });
        
        _logger.LogDebug($"Sending POST request to {servicePath}");

        var httpContent = new StringContent(body, Encoding.UTF8, "application/json");
        var response = data != null ? await _httpClient.PostAsync(servicePath, httpContent) : await _httpClient.PostAsync(servicePath, null);

       
        if (!response.IsSuccessStatusCode)
        { 
            _logger.LogError($"{_httpClient.BaseAddress} responded with an error: Message: {await response.Content.ReadAsStringAsync()}");
        }

        var jsonString = await response.Content.ReadAsStringAsync();
        _logger.LogDebug($"Response Object: {jsonString}");
        T result = JsonConvert.DeserializeObject<T>(jsonString);

        return (result, response);
    }

    public async Task<(T, HttpResponseMessage)> PutAsync<T>(object data, Uri servicePath)
    {
        var body = JsonConvert.SerializeObject(data, new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });

        _logger.LogDebug($"Sending PUT request to {servicePath} with body: {body}");

        var httpContent = new StringContent(body, Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync(servicePath, httpContent);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError($"{_httpClient.BaseAddress} responded with an error: Code: {response.StatusCode}");
        }
        var jsonString = await response.Content.ReadAsStringAsync();
        _logger.LogDebug($"Response Object: {jsonString}");
        T result = JsonConvert.DeserializeObject<T>(jsonString);

        return (result, response);
    }

    public async Task<(T, HttpResponseMessage)> PatchAsync<T>(object data, Uri servicePath)
    {
        var body = JsonConvert.SerializeObject(data, new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });

        _logger.LogDebug($"Sending PATCH request to {servicePath} with body: {body}");

        var httpContent = new StringContent(body, Encoding.UTF8, "application/json");
        var response = await _httpClient.PatchAsync(servicePath, httpContent);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError($"{_httpClient.BaseAddress} responded with an error: Code: {response.StatusCode}");
        }

        var jsonString = await response.Content.ReadAsStringAsync();
        _logger.LogDebug($"Response Object: {jsonString}");
        T result = JsonConvert.DeserializeObject<T>(jsonString);

        return (result, response);
    }

    public async Task<(T, HttpResponseMessage)> DeleteAsync<T>(Uri servicePath)
    {
        _logger.LogDebug($"Sending DELETE request to {servicePath}");

        var response = await _httpClient.DeleteAsync(servicePath);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError($"{_httpClient.BaseAddress} responded with an error: Code: {response.StatusCode}");
        }

        var jsonString = await response.Content.ReadAsStringAsync();
        _logger.LogDebug($"Response Object: {jsonString}");
        T result = JsonConvert.DeserializeObject<T>(jsonString);

        return (result, response);
    }
    
    public async Task<(T, HttpResponseMessage)> PostUrlEncodedFormData<T>(Uri servicePath, FormUrlEncodedContent data)
    {
        _logger.LogInformation($"Sending Post request to {servicePath}");
        
        var response = await _httpClient.PostAsync(servicePath, data);
        var responseMessage = response.Content;
        
        if (!response.IsSuccessStatusCode)
        {
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(@$"{_httpClient.BaseAddress} responded with an error: Code: 
                    {response.StatusCode} and body: {await response.Content.ReadAsStringAsync()}");
            }
        }

        var jsonString = await responseMessage.ReadAsStringAsync();
        _logger.LogDebug($"Response Object: {jsonString}");
        T result = JsonConvert.DeserializeObject<T>(jsonString);
        return (result, response);
    }
  }
}
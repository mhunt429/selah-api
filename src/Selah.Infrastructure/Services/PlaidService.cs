using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Selah.Domain.Data.Models.Plaid;
using Selah.Infrastructure.Services.Extensions;
using Selah.Infrastructure.Services.Interfaces;

namespace Selah.Infrastructure.Services;

public class PlaidService : IPlaidService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;

    public PlaidService(HttpClient httpClient, ILogger<PlaidService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<TokenLinkResponse> GenerateLinkToken(TokenLinkRequest linkRequest)
    {
        var response = await _httpClient.PostAsync<TokenLinkRequest>(linkRequest, "link/token/create");
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError(
                $"Plaid Token Link request failed with error {await response.Content.ReadAsStringAsync()}");
            return null;
        }

        var content = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<TokenLinkResponse>(content);
    }

    public Task ExchangeToken()
    {
        throw new System.NotImplementedException();
    }
}
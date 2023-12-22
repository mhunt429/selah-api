using System.Threading.Tasks;
using Selah.Domain.Data.Models.Plaid;

namespace Selah.Infrastructure.Services.Interfaces;

public interface IPlaidService
{
    Task<TokenLinkResponse> GenerateLinkToken(TokenLinkRequest linkRequest);

    Task ExchangeToken();
}
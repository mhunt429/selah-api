using System.Threading.Tasks;
using Selah.Domain.Data.Models.MasterCard;

namespace Selah.Infrastructure.Services.Interfaces;

public interface IMasterCardApiService
{
    Task<AccessToken> GetToken();
}
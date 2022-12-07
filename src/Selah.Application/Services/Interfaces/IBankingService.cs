using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Selah.Domain.Data.Models.Banking;
using Selah.Domain.Data.Models.Plaid;

namespace Selah.Application.Services.Interfaces
{
  public interface IBankingService
  {
    public Task<UserInstitution> CreateUserInstitution(PlaidAccountLinkRequest institution);
    Task<IEnumerable<BankAccount>> GetAccounts(Guid? userId);
    
    public Task ImportAccounts(Guid userId);
  }
}
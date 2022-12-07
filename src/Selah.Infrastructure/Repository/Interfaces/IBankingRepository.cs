using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Selah.Domain.Data.Models.Banking;

namespace Selah.Infrastructure.Repository.Interfaces
{
  public interface IBankingRepository
  {
    public Task<long> ImportAccounts(List<BankAccount> accounts);

    public Task<IEnumerable<BankAccount>> GetAccounts(Guid userId);

    public Task<IEnumerable<BankAccount>> GetAccountsByInstitutionId(Guid institutionId);
  }
}
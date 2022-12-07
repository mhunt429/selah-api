using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Selah.Domain.Data.Models.Plaid;

namespace Selah.Infrastructure.Repository.Interfaces
{
  public interface IUserInstitutionRepository
  {
    /// <summary>
    /// Creates a record after linking with plaid. A new record is created for each bank
    /// </summary>
    /// <param name="institutionCreate"></param>
    /// <returns></returns>
    public Task<Guid?> CreateUserInstitution(PlaidUserInstitutionCreate institutionCreate);
    
    /// <summary>
    /// Returns a collection of all institutions for a given user
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<IEnumerable<UserInstitution>> GetByUser(Guid userId);
    
    /// <summary>
    /// Used when linking accounts and transactions for a given institution
    /// Every record has a unique access token per institution 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="institutionId"></param>
    /// <returns></returns>
    public Task<UserInstitution> GetById(Guid id);
    
  }
}
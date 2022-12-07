using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Selah.Application.Services.Interfaces;
using Selah.Domain.Data.Models.Banking;
using Selah.Domain.Data.Models.Integrations.Plaid.PlaidAccounts;
using Selah.Domain.Data.Models.Plaid;
using Selah.Domain.Internal;
using Selah.Infrastructure.Repository.Interfaces;

namespace Selah.Application.Services
{
  public class BankingService: IBankingService
  {
    private readonly IPlaidService _plaidService;
    private readonly ISecurityService _securityService;
    private readonly IUserInstitutionRepository _institutionRepository;
    private ILogger _logger;
    private readonly IOptions<EnvVariablesConfig> _envVariables;
    private readonly IBankingRepository _bankingRepository;
    public BankingService(IPlaidService plaidService,
      ISecurityService securityService, 
      IUserInstitutionRepository institutionRepository, 
      IBankingRepository bankingRepository,
      ILogger<BankingService> logger, IOptions<EnvVariablesConfig> envVariables)
    {
      _securityService = securityService;
      _plaidService = plaidService;
      _institutionRepository = institutionRepository;
      _logger = logger;
      _envVariables = envVariables;
      _bankingRepository = bankingRepository;
    }
   
    public async Task<UserInstitution> CreateUserInstitution(PlaidAccountLinkRequest institution)
    {
      var accessTokenResponse = await _plaidService.GetAccessToken(institution.TokenLink.PublicToken);
      var encryptedAccessToken = await _securityService.Encrypt(accessTokenResponse.AccessToken);
      try
      {
        var id = await _institutionRepository.CreateUserInstitution(new PlaidUserInstitutionCreate
        {
          EncryptedAccessToken = encryptedAccessToken,
          InstitutionId = institution.User.InstitutionId,
          InstitutionName = institution.User.InstitutionName,
          UserId = institution.User.UserId
        }); //Inserted id
        if (id == null)
        {
          return null;
        }

        return new UserInstitution
        {
          Id = id.Value,
          InstitutionId = institution.User.InstitutionId,
          EncryptedAccessToken = encryptedAccessToken,
          UserId = institution.User.UserId,
          InstitutionName = institution.User.InstitutionName
        };
      }

      catch (Exception ex)
      {
        _logger.LogError($"Failed to save institution with error: {errorText(ex)}");
        return null;
      }
    }

    public async Task ImportAccounts(Guid institutionId)
    {
      var userInstitution = await _institutionRepository.GetById(institutionId);
      var decryptedAccessToken = await _securityService.Decrypt(userInstitution.EncryptedAccessToken);
      var accountsRequest = new PlaidAccountRequest
      {
        AccessToken = decryptedAccessToken,
        ClientId = _envVariables.Value.PlaidClientId,
        Secret = _envVariables.Value.PlaidClientSecret
      };
      var importedAccounts = await _plaidService.ImportAccounts(accountsRequest);
      var accounts = new List<BankAccount>();
      foreach (var item in importedAccounts.Accounts)
      {
        accounts.Add(new BankAccount()
        {
          AvailableBalance = item.Balances?.Available,
          CurrentBalance = item.Balances?.Current,
          InstitutionId = institutionId,
          ExternalAccountId = item.AccountId,
          Name = item.Name,
          UserId = userInstitution.UserId,
          Mask = item.Mask,
          Subtype = item.Subtype
        });
      }

      await _bankingRepository.ImportAccounts(accounts);
    }

    //TODO Add paging support
    public async Task<IEnumerable<BankAccount>> GetAccounts(Guid? userId)
    {
      return await _bankingRepository.GetAccounts(userId.Value);
    }

    private string errorText(Exception ex)
    {
      return $"\nError: {ex.Source}\nMessage: {ex.Message}\nStack: {ex.StackTrace}";
    }
  }
}
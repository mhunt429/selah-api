using MediatR;
using Microsoft.AspNetCore.Mvc;
using Selah.Domain.Data.Models.Banking;
using Selah.Infrastructure.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Selah.Application.Services.Interfaces;

namespace Selah.Application.Queries.Banking
{
    public class GetAllBankAccountsQuery : IRequest<IEnumerable<BankAccount>>
    {
        public string UserId { get; set; }

        //Pagination Support
        [FromQuery] public int Limit { get; set; } = 25;
        [FromQuery] public int Offset { get; set; } = 1;

        public class Handler : IRequestHandler<GetAllBankAccountsQuery, IEnumerable<BankAccount>>
        {
            private readonly IBankingRepository _bankingRepository;
            private readonly ISecurityService _securityService;

            public Handler(IBankingRepository bankingRepository, ISecurityService securityService)
            {
                _bankingRepository = bankingRepository;
                _securityService = securityService;
            }

            public async Task<IEnumerable<BankAccount>> Handle(GetAllBankAccountsQuery query,
                CancellationToken cancellationToken)
            {
                long id = _securityService.DecodeHashId(query.UserId);

                List<BankAccount> accounts = new List<BankAccount>();
                var queryResult = (await _bankingRepository.GetAccounts(id, query.Limit, query.Offset));
                if (!queryResult.Any()) return accounts;
                foreach (var account in queryResult)
                {
                    accounts.Add(new BankAccount
                    {
                        Id = _securityService.EncodeHashId(account.Id),
                        AccountMask = account.AccountMask,
                        Name = account.AccountName,
                        AvailableBalance = account.AvailableBalance,
                        CurrentBalance = account.CurrentBalance,
                        UserId = query.UserId,
                        Subtype = account.Subtype,
                        InstitutionId = _securityService.EncodeHashId(account.InstitutionId)
                    });
                }
                return accounts;
            }
        }
    }
}
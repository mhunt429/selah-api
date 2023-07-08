using MediatR;
using Microsoft.AspNetCore.Mvc;
using Selah.Domain.Data.Models.Banking;
using Selah.Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Selah.Application.Services.Interfaces;

namespace Selah.Application.Queries.Banking
{
    public class GetAllBankAccountsQuery: IRequest<IEnumerable<BankAccount>>
    {
        public string UserId { get; set; }

        //Pagination Support
        [FromQuery]
        public int Limit { get; set; } = 25;
        [FromQuery]
        public int Offset { get; set; } = 1;

        public class Handler : IRequestHandler<GetAllBankAccountsQuery, IEnumerable<BankAccount>>
        {
            private readonly IBankingRepository _bankingRepository;
            private readonly ISecurityService _securityService;
            public Handler(IBankingRepository bankingRepository, ISecurityService securityService)
            {
                _bankingRepository = bankingRepository;
                _securityService = securityService;
            }

            public async Task<IEnumerable<BankAccount>> Handle(GetAllBankAccountsQuery query, CancellationToken cancellationToken)
            {
                int id = _securityService.DecodeHashId(query.UserId);
               return await _bankingRepository.GetAccounts(id, query.Limit, query.Offset);
            }
        }
    }
}

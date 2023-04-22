using MediatR;
using Microsoft.AspNetCore.Mvc;
using Selah.Domain.Data.Models.Banking;
using Selah.Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Selah.Application.Queries.Banking
{
    public class GetAllBankAccountsQuery: IRequest<IEnumerable<BankAccount>>
    {
        public Guid UserId { get; set; }

        //Pagination Support
        [FromQuery]
        public int Limit { get; set; } = 25;
        [FromQuery]
        public int Offset { get; set; } = 1;

        public class Handler : IRequestHandler<GetAllBankAccountsQuery, IEnumerable<BankAccount>>
        {
            private readonly IBankingRepository _bankingRepository;

            public Handler(IBankingRepository bankingRepository)
            {
                _bankingRepository = bankingRepository;
            }

            public async Task<IEnumerable<BankAccount>> Handle(GetAllBankAccountsQuery query, CancellationToken cancellationToken)
            {
               return await _bankingRepository.GetAccounts(query.UserId, query.Limit, query.Offset);
            }
        }
    }
}

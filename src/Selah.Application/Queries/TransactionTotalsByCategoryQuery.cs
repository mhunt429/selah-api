using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Selah.Application.Services.Interfaces;
using Selah.Domain.Data.Models.Transactions;
using Selah.Domain.Data.Models.Transactions.Sql;
using Selah.Infrastructure.Repository.Interfaces;

namespace Selah.Application.Queries;

public class TransactionTotalsByCategoryQuery : IRequest<IEnumerable<TransactionAmountByCategory>>
{
    [FromRoute] [DisplayName("userId")] public string UserId { get; set; }

    public class Handler : IRequestHandler<TransactionTotalsByCategoryQuery, IEnumerable<TransactionAmountByCategory>>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ISecurityService _securityService;


        public Handler(ITransactionRepository transactionRepository, ISecurityService securityService)
        {
            _transactionRepository = transactionRepository;
            _securityService = securityService;
        }

        public async Task<IEnumerable<TransactionAmountByCategory>> Handle(TransactionTotalsByCategoryQuery query,
            CancellationToken cancellationToken)
        {
            long userId = _securityService.DecodeHashId(query.UserId);
            List<TransactionAmountByCategory> transactionAmountByCategories = new List<TransactionAmountByCategory>();
            List<TransactionAmountByCategorySql> transactionCategoryAmountSql =
                (await _transactionRepository.GetTransactionTotalsByCategory(userId)).ToList();
            if (!transactionCategoryAmountSql.Any())
            {
                return transactionAmountByCategories;
            }

            foreach (TransactionAmountByCategorySql category in transactionCategoryAmountSql)
            {
                transactionAmountByCategories.Add(new TransactionAmountByCategory
                {
                    Id = _securityService.EncodeHashId(category.Id),
                    Name = category.Name,
                    Total = category.Total
                });
            }

            return transactionAmountByCategories;
        }
    }
}
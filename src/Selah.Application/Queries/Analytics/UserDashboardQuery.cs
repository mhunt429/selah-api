using MediatR;
using Selah.Domain.Data.Models.Analytics.Dashboard;
using Selah.Domain.Data.Models.Transactions;
using Selah.Domain.Data.Models.Transactions.Sql;
using Selah.Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Selah.Application.Services.Interfaces;

namespace Selah.Application.Queries.Analytics
{
    public class UserDashboardQuery : IRequest<DashboardSummary>
    {
        [FromRoute] [DisplayName("userId")] public string UserId { get; set; }

        public class Handler : IRequestHandler<UserDashboardQuery, DashboardSummary>
        {
            private readonly ITransactionRepository _transactionRepository;
            private readonly ISecurityService _securityService;

            public Handler(ITransactionRepository transactionRepository, ISecurityService securityService)
            {
                _transactionRepository = transactionRepository;
                _securityService = securityService;
            }

            public async Task<DashboardSummary> Handle(UserDashboardQuery query, CancellationToken cancellationToken)
            {
                long id = _securityService.DecodeHashId(query.UserId);
                (IEnumerable<TransactionSummarySql>, IEnumerable<TransactionSummarySql>) transactionSummaryTuple = await GetLastAndCurrentMonthTrxSummary(id);
                return new DashboardSummary
                {
                    RecentTransactions = await GetRecentTransactions(id),
                    LastMonthSpending = transactionSummaryTuple.Item1.ToList(),
                    CurrentMonthSpending = transactionSummaryTuple.Item2.ToList(),
                    UpcomingTransactions = new List<RecurringTransaction>(),
                    PortfolioSummary = new { },
                    NetWorthSummary = new NetWorthSummary { }
                };
            }

            private async Task<List<RecentTransaction>> GetRecentTransactions(long userId)
            {
                IEnumerable<RecentTransactionSql> recentTransactionSql =
                    await _transactionRepository.GetRecentTransactions(userId);
                List<RecentTransaction> recentTransactions = new List<RecentTransaction>();

                foreach (RecentTransactionSql trx in recentTransactionSql)
                {
                    recentTransactions.Add(new RecentTransaction
                    {
                        TransactionId = _securityService.EncodeHashId(trx.TransactionId),
                        TransactionDate = trx.TransactionDate,
                        Merchant = trx.Merchant,
                        TransactionAmount = trx.TransactionAmount,
                        AccountName = trx.AccountName,
                    });
                }

                return recentTransactions;
            }

            /// <summary>
            /// Returns a tuple of a list of transaction summary where the first item is last month's
            /// summary and the second item is the current months summary
            /// </summary>
            /// <returns></returns>
            //TODO probably a good idea to move this to a separate handler/endpoint
            private async Task<(IEnumerable<TransactionSummarySql>, IEnumerable<TransactionSummarySql>)>
                GetLastAndCurrentMonthTrxSummary(long userId)
            {
                DateTime currentTimeUtc = DateTime.UtcNow;

                //First day of the previous month
                DateTime startOfLastMonth = new DateTime(currentTimeUtc.Year, currentTimeUtc.Month, 1).AddMonths(-1);
                DateTime endOfLastMonth = startOfLastMonth.AddMonths(1).AddSeconds(-1);

                DateTime startOfCurrentMonth = new DateTime(currentTimeUtc.Year, currentTimeUtc.Month, 1);

                IEnumerable<TransactionSummarySql> lastMonthTrxSummary =
                    await _transactionRepository.GetTransactionSummaryByDateRange(userId, startOfLastMonth,
                        endOfLastMonth);

                IEnumerable<TransactionSummarySql> currentMonthSummary =
                    await _transactionRepository.GetTransactionSummaryByDateRange(userId, startOfCurrentMonth,
                        currentTimeUtc);

                return (lastMonthTrxSummary, currentMonthSummary);
            }
        }
    }
}
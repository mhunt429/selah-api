using Selah.Domain.Data.Models.Transactions;
using System.Collections.Generic;

namespace Selah.Domain.Data.Models.Analytics.Dashboard
{
    public class DashboardSummary
    {
        /// <summary>
        /// 5 most recent transactions sorted by transaction date desc
        /// </summary>
        public List<RecentTransaction> RecentTransactions { get; set; }

        /// <summary>
        /// list of upcoming recuring transactions
        /// </summary>
        public List<RecurringTransaction> UpcomingTransactions { get; set; }

        public List<TransactionSummarySql> CurrentMonthSpending { get; set; }

        public List<TransactionSummarySql> LastMonthSpending { get; set; }

        /// <summary>
        /// Percentage of user's monthly/weekly budget they have utilized
        /// </summary>
        public decimal AllocatedBudget { get; set; }

        /// <summary>
        /// TODO Add model for non-real estate summary
        /// </summary>
        public object PortfolioSummary { get; set; }

        /// <summary>
        /// Summary of user's net worth
        /// Net Worth = (Assets + Equity) - Liabilities
        /// </summary>
        public NetWorthSummary NetWorthSummary { get; set; }
    }

    public class NetWorthSummary
    {
        public decimal NetWorth { get; set; }

        public decimal Assets { get; set; }

        public decimal Equity { get; set; }

        public decimal Liabilities { get; set; }
    }
}

using Dapper.FluentMap;
using Selah.Domain.Data.SchemaMappings.AppUser;
using Selah.Domain.Data.SchemaMappings.FinancialAccounts;
using Selah.Domain.Data.SchemaMappings.Transactions;

namespace Selah.Domain.Data.SchemaMappings
{
    public class SelahMappings
    {
        public static void RegisterMaps()
        {
            FluentMapper.Initialize(config =>
            {
                config.AddMap(new AppUserSchemaMap());
            });

            FluentMapper.Initialize(config =>
            {
                config.AddMap(new BankAccountSchemaMap());
            });

            FluentMapper.Initialize(config =>
            {
                config.AddMap(new UserTransactionSchemaMap());
            });

            FluentMapper.Initialize(config =>
            {
                config.AddMap(new UserTransactionQueryResultSchemaMap());
            });

            FluentMapper.Initialize(config =>
            {
                config.AddMap(new UserTransactionCategorySchemaMap());
            });

            FluentMapper.Initialize(config =>
            {
                config.AddMap(new ItemizedTransactionSchemaMap());
            });

            FluentMapper.Initialize(config =>
            {
                config.AddMap(new RecentTransactionSchemaMap());
            });

            FluentMapper.Initialize(config =>
            {
                config.AddMap(new TransactionSummarySchemaMap());
            });
        }
    }
}
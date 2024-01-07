using Dapper.FluentMap.Mapping;
using Selah.Domain.Data.Models.Transactions;

namespace Selah.Domain.Data.SchemaMappings.Transactions
{
    public class UserTransactionCategorySchemaMap : EntityMap<UserTransactionCategory>
    {
        public UserTransactionCategorySchemaMap()
        {
            Map(b => b.Id)
                .ToColumn("id");

            Map(b => b.UserId)
                .ToColumn("user_id");

            Map(b => b.Name)
                .ToColumn("name");
        }
    }
}
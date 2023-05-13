using Dapper.FluentMap.Mapping;
namespace Selah.Domain.Data.SchemaMappings.AppUser
{
    public class AppUserSchemaMap : EntityMap<Models.ApplicationUser.AppUser>
    {
        public AppUserSchemaMap()
        {
            Map(u => u.Id)
              .ToColumn("id");

            Map(u => u.Email)
              .ToColumn("email");

            Map(u => u.UserName)
              .ToColumn("user_name");

            Map(u => u.Password)
              .ToColumn("password");

            Map(u => u.FirstName)
              .ToColumn("first_name");

            Map(u => u.LastName)
              .ToColumn("last_name");

            Map(u => u.DateCreated)
              .ToColumn("date_created");
        }
    }
}
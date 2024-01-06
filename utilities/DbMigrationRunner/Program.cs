using MySqlConnector;
using Npgsql;

namespace DbMigrationRunner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var connectionStrings = new ConnectionStrings();
            //To Generate a new migration script run this bash command, Date +"%Y%m%d%H%M%S"__<NAME_OF_TASK>.sql
            var connectionString = !string.IsNullOrEmpty(connectionStrings.SelahDbLocal)
                ? connectionStrings.SelahDbLocal
                : "User ID=mysqluser;Password=mysqlpassword;Host=localhost;Database=selah_db";
          Console.WriteLine(connectionString);
            string location = "migrations";
            try
            {
                var connection = new MySqlConnection(connectionString);
                var evolve = new Evolve.Evolve(connection,
                    msg => Console.WriteLine($"Beginning database migrations with {msg}"))
                {
                    Locations = new[] { location }
                };
                evolve.Migrate();
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace.ToString());
                throw;
            }
        }
    }
}
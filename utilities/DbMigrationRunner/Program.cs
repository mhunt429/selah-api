using Npgsql;

namespace DbMigrationRunner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var connectionStrings = new ConnectionStrings();
            //To Generate a new migration script run this bash command, Date +"%Y%m%d%H%M%S"__<NAME_OF_TASK>.sql
            string location = "migrations";
            try
            {
                var connection = new NpgsqlConnection(connectionStrings.SelahDbLocal ?? "User ID=postgres;Password=postgres;Host=localhost;Port=65432;Database=postgres");
                var evolve = new Evolve.Evolve(connection, msg => Console.WriteLine($"Beginning database migrations with {msg}"))
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
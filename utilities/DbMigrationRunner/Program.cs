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
                var connection = new NpgsqlConnection(connectionStrings.SelahDbLocal);
                var evolve = new Evolve.Evolve(connection, msg => Console.WriteLine($"Beginning database migrations with {msg}"))
                {
                    Locations = new[] { location }
                };
                evolve.Migrate();
            }

            catch (Exception ex)
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                Console.WriteLine(ex.StackTrace.ToString());
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                throw;
            }

            try
            {
                var connection = new NpgsqlConnection(connectionStrings.SelahDbTest);
                var evolve = new Evolve.Evolve(connection, msg => Console.WriteLine($"Beginning database migrations with {msg}"))
                {
                    Locations = new[] { location }
                };
                evolve.Migrate();
            }

            catch (Exception ex)
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                Console.WriteLine(ex.StackTrace.ToString());
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                throw;
            }
        }
    }
}
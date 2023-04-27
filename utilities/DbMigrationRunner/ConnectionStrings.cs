using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbMigrationRunner
{
    public class ConnectionStrings
    {
        public string SelahDbLocal => Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
        public string SelahDbTest => Environment.GetEnvironmentVariable("Selah_Db_Integration_Test_Connection_String");
    }
}

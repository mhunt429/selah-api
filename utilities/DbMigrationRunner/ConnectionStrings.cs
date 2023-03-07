using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbMigrationRunner
{
    public class ConnectionStrings
    {
        public string SelahDbLocal => Environment.GetEnvironmentVariable("Selah_Db_Local_Connection_String");
        public string SelahDbTest => Environment.GetEnvironmentVariable("Selah_Db_Integration_Test_Connection_String");
    }
}

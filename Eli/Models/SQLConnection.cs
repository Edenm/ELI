using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eli.Models
{
    public class SQLConnection
    {
        private static string connectionString = "Data Source=SHAHAR-PC\\SQLEXPRESS;Initial Catalog=Eli;Integrated Security=True";

        public static DataConnectionDataContext GetDataContextInstance()
        {
            DataConnectionDataContext dbDataContext = new DataConnectionDataContext(connectionString);
            return dbDataContext;
        }
    }
}

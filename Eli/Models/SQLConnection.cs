using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eli.Models
{
    public class SQLConnection
    {
        //for shahar
        //private static string connectionString = "Data Source=SHAHAR-PC\\SQLEXPRESS;Initial Catalog=Eli;Integrated Security=True";
        //for eden
        //private static string connectionString = "Data Source=Eden-PC\\SQLEXPRESS1;Initial Catalog=Eli;Integrated Security=True";
        //for server
        private static string connectionString = "Data Source=132.75.252.109;Initial Catalog=es2015;Persist Security Info=True;User ID=es2015;Password=th41pn9";
        //for Hila  -- MISRAD-PC\SQLEXPRESS
        //private static string connectionString = "Data Source=10.0.0.2\\SQLEXPRESS;Initial Catalog=es2015;Persist Security Info=True;User ID=sa;Password=ism149";
        
        public static DataConnectionDataContext GetDataContextInstance()
        {
            DataConnectionDataContext dbDataContext = new DataConnectionDataContext(connectionString);
            return dbDataContext;
        }

        public static String GetConnectionString()
        {
            return connectionString;
        }

    }
}
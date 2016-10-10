using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DataAccess
{
    internal class DBConnection  // this is the only place to get a sqlconnection
    {
        // here's the connection string most people would need at home
        // const string ConnectionString = @"Data Source=localhost\sqlexpress;Initial Catalog=dunebuggies;Integrated Security=True";
        const string ConnectionString = @"Data Source=localhost;Initial Catalog=library;Integrated Security=True";

        public static SqlConnection GetDBConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ToDoListWPF.Dao
{
    public class DBCon
    {
        public DBCon()
        {
        }

        public SqlConnection sqlConnection()
        {
            ConnectionStringSettings connstr = ConfigurationManager.ConnectionStrings["sqlserverDBConn"];

            string StrConn = connstr.ToString();
            Console.WriteLine(StrConn);
            SqlConnection sconnection = new SqlConnection(StrConn);

            sconnection.Open();
            Console.WriteLine(sconnection.State);
            return sconnection;
        }

        public SqlCommand sqlCommand(string sql)
        {
            SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection());
            return sqlCommand;
        }

        //增删改
        public int sqlExcute(string sql)
        {
            return sqlCommand(sql).ExecuteNonQuery();
        }


        //查
        public SqlDataReader sqlRead(string sql)
        {
            return sqlCommand(sql).ExecuteReader();
        }
    }
}

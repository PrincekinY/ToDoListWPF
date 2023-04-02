using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListWPF.Dao
{
    public class MysqlDBCon
    {
        public MysqlDBCon()
        {
            ConnectionStringSettings connstr = ConfigurationManager.ConnectionStrings["mysqlDBConn"];
            string StrConn = connstr.ToString();
            sconnection = new MySqlConnection(StrConn);
            sconnection.Open();
        }

        private string strConn;

        public string StrConn
        {
            get { return strConn; }
            set { strConn = value; }
        }

        public MySqlConnection sconnection { get; private set; }


        public MySqlCommand sqlCommand(string sql)
        {
            if (sconnection.State == ConnectionState.Open) { }
            else { sconnection.Close(); sconnection.Open(); }
            MySqlCommand sqlCommand = new MySqlCommand(sql, sconnection);
            return sqlCommand;
        }

        //增删改
        public int sqlExcute(string sql)
        {
            return sqlCommand(sql).ExecuteNonQuery();
        }


        //查
        public MySqlDataReader sqlRead(string sql)
        {
            return sqlCommand(sql).ExecuteReader();
        }
    }
}

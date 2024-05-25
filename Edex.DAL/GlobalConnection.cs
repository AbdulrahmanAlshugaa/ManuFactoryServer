using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
 

namespace Edex.DAL
{
   public class GlobalConnection
    {
        public SqlConnection Conn { get; set; }
        public GlobalConnection()
        {
            Conn = new SqlConnection(cConnectionString.ConnectionString);
            CloseConn();
              
        }
        public void OpenConn()
        {
            if (Conn != null && Conn.State != ConnectionState.Open)
            {
                Conn.Open();
            }
        }
        public void CloseConn()
        {
            if (Conn != null && Conn.State != ConnectionState.Closed)
            {
                Conn.Close();
            }
        }
        public void OpenConnOra()
        {
             
        }
        public void CloseConnOra()
        {
            
        }
    }
}

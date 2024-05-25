//using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.DAL
{
  public static   class cConnectionString
    {
        public static string DataBasename { get; set; }
        public static SqlConnection Conn { get; set; }
        public static string ConnectionString { get; set; }

        //public static MySqlConnection ConnMySql { get; set; }
        public static string ConnectionStringMySql { get; set; }

        //public static MySqlConnection GetConnectionMySqlSetting()
        //{
        //    ConnectionStringMySql = "server=localhost;uid=root;" + "pwd='';database=	maxstationdb";
        //    ConnMySql = new MySql.Data.MySqlClient.MySqlConnection();
        //    ConnMySql.ConnectionString = ConnectionStringMySql;
        //    return ConnMySql;
        //} 

        public static string UserName { get; set; } 
        public static string PassWordtxt { get; set; }
        public static SqlConnection GetConnectionSetting()
        {
              //سيرفر
             // string strCon = "Data Source=95.168.176.203\\MSSQLSERVER2014;Integrated Security=False;Initial Catalog=" + DataBasename  + ";User ID=omexinituser;Connect Timeout=15;Encrypt=False;Packet Size=4096;password=S0fJRoz1d9ea^ymr";
            // string strCon = "Data Source=95.168.176.203\\MSSQLSERVER2014;Integrated Security=False;Initial Catalog=omexinit_;User ID=omexinituser;Connect Timeout=15;Encrypt=False;Packet Size=4096;password=S0fJRoz1d9ea^ymr";
            string strCon = "Data Source=" + cConnectionString.ServerName + ";Integrated Security=False;Initial Catalog=" + DataBasename + ";User ID=" + UserName + ";Connect Timeout=15;Encrypt=False;Packet Size=4096;password="+ PassWordtxt + "";
          //لوكال
          //  string strCon = @"server=ENG-ABDULRAHAMN\MSSQLSERVER2022;Database=DaimondDBDemo;integrated security=true;";
         // string strCon = @"server=DESKTOP-7O413EL;Database=DaimondDBDemo;integrated security=true;";
        // string strCon = "Data Source=.;Integrated Security=False;Initial Catalog=" + DataBasename + ";User ID=IT@inn;Connect Timeout=15;Encrypt=False;Packet Size=4096;password=IT@inn";
       ConnectionString = strCon;
            return new SqlConnection(ConnectionString);
        }

        public static SqlConnection GetConnectionDatabase()
        {
           // string strCon = "Data Source=.;Integrated Security=False;Initial Catalog=" + DataBasename + ";User ID=IT-inn;Connect Timeout=15;Encrypt=False;Packet Size=4096;password=IT@inn";

            //سيرفر
           // string strCon = "Data Source=95.168.176.203\\MSSQLSERVER2014;Integrated Security=False;Initial Catalog=omexinitDB;User ID=omexinituser;Connect Timeout=15;Encrypt=False;Packet Size=4096;password=S0fJRoz1d9ea^ymr";
            string strCon = "Data Source=" + ServerName + ";Integrated Security=False;Initial Catalog=" + cConnectionString.DataBasename + ";User ID=" + UserName + ";Connect Timeout=15;Encrypt=False;Packet Size=4096;password=" + PassWordtxt + "";

            ConnectionString = strCon;
            return new SqlConnection(ConnectionString);

        }
        public static string GetDataBaseName()
        {


            try
            {
                using (SqlConnection objCnn = new ConnectionHelper().OpenConnSetting())
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.Text;
                        string f="AccountSystem";
                        objCmd.CommandText = "Select DataBaseName from SMSSettings where ProgramName ='" + f + "'" ;
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        return dt.Rows[0]["DataBaseName"].ToString();

                    }
                }
            }
            catch (Exception)
            {
                return "";
            }
        }
        public static string ServerName { get; set; }
    }
}

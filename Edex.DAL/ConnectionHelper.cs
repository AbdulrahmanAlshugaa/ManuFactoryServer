using System.Data.SqlClient;

namespace Edex.DAL
{
    public class ConnectionHelper
    {
       
        public static string  DataBasename { get; set; }
        public static string  ServerNamename { get; set; }

        //public static MySqlConnection conn;
        public static string myConnectionString;


        //public static MySqlConnection GetConnectionMySqlSetting()
        //{ 
        //    myConnectionString = "server=localhost;uid=root;" + "pwd='';database=	maxstationdb";
        //    conn = new MySql.Data.MySqlClient.MySqlConnection();
        //    conn.ConnectionString = myConnectionString;
        //    return   conn;       
        //}
       
        public static SqlConnection GetConnectionSetting()
        {  
           //Server Aldhaher
           string strCon = "Data Source=" + cConnectionString.ServerName + ";Integrated Security=False;Initial Catalog=" + DataBasename + ";User ID=IT-inn;Connect Timeout=15;Encrypt=False;Packet Size=4096;password=IT@inn";
           return new SqlConnection(strCon);
        }


        public static SqlConnection GetConnectionDatabase()
        {
            //Server Aldhaher
            string strCon = "Data Source=" + cConnectionString.ServerName + ";Integrated Security=False;Initial Catalog=ITDataBaseSeting;User ID=" + cConnectionString.UserName + ";Connect Timeout=15;Encrypt=False;Packet Size=4096;password=" + cConnectionString.PassWordtxt + "";
            return new SqlConnection(strCon);
        }

        public static string GetConnectionString()
        {
            //Server Aldhaher
            string strCon = "Data Source=" + cConnectionString.ServerName + ";Integrated Security=False;Initial Catalog=" + cConnectionString.DataBasename + ";User ID=" + cConnectionString.UserName + ";Connect Timeout=15;Encrypt=False;Packet Size=4096;password="+ cConnectionString.PassWordtxt + "";
           
           //string strCon = "Data Source=" + ServerNamename + ";Integrated Security=False;Initial Catalog=ITDataBaseSeting;User ID=IT-inn;Connect Timeout=15;Encrypt=False;Packet Size=4096;password=IT@inn";
            
            //Server Local 
           /// string strCon = @"server=ENG-ABDULRAHAMN\MSSQLSERVER2022;Database=DaimondDBDemo;integrated security=true;";
            //string strCon = @"server=DESKTOP-7O413EL;Database=DaimondDBDemo;integrated security=true;";
            return strCon;

        }

        public static string GetConnectionStringDataBase()
        {
            //Server Aldhaher
            string strCon = "Data Source=" + cConnectionString.ServerName + ";Integrated Security=False;Initial Catalog=" + cConnectionString.DataBasename + ";User ID=" + cConnectionString.UserName + ";Connect Timeout=15;Encrypt=False;Packet Size=4096;password=S0fJRoz1d9ea^ymr";
           
            return strCon;

        }


        public SqlConnection OpenConnSetting()
        {
            return GetConnectionDatabase();

        }

    }                                                           
}
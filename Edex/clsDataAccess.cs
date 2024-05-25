using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Edex
{
    class clsDataAccess
    {
        SqlConnection SqlCon; // كائن الاتصال

        public clsDataAccess()
        {
            SqlCon = new SqlConnection(@"Data Source=.; Initial Catalog = ObraDB20184; Integrated Security=false; User ID = WafeTopPOS; password = WafeTopPOS");

            //SqlCon = new SqlConnection(@"Data Source=(LocalDB)\v11.0; AttachDbFilename=|DataDirectory|\DB_SabaUniWebsite.mdf; Integrated Security=True");
            //Data Source =.; Integrated Security = False; Initial Catalog = " + DataBasename + "; User ID = WafeTopPOS; password = WafeTopPOS"
        }


        // اجراء لفتح الاتصال بقاعدة البيانات اذا كان الاتصال غير مفتوح
        public void Open()
        {
            if (SqlCon.State != ConnectionState.Open)
            {
                SqlCon.Open();
            }
        }

        //*******************************************************************
        // اجراء لإغلاق الاتصال بقاعدة البيانات
        public void Close()
        {
            if (SqlCon.State == ConnectionState.Open)
            {
                SqlCon.Close();
            }
        }

        //*******************************************************************
        // (Insert, Delete, Update) اجراء لتنفيذ عمليات على قاعدة البيانات  
        public int ExecuteCommand(string Stored_Procedure_Name, SqlParameter[] SP_Param)
        {
            int rows = 0;

            SqlCommand SqlCmd = new SqlCommand();
            SqlCmd.CommandType = CommandType.StoredProcedure;
            SqlCmd.CommandText = Stored_Procedure_Name;
            SqlCmd.Connection = SqlCon;

            if (SP_Param != null)
            {
                for (int i = 0; i < SP_Param.Length; i++)
                {
                    SqlCmd.Parameters.Add(SP_Param[i]);
                }
                // ويمكن كتابة السطرين السابقين بالسطر التالي
                //SqlCmd.Parameters.AddRange(SP_Param);
            }

            try
            {
                rows = SqlCmd.ExecuteNonQuery();
            }

            catch
            {
                ;
            }

            return rows;
        }


        //*******************************************************************
        // دالة لقراءة البيانات من قاعدة البيانات
        public DataTable SelectData(string Stored_Procedure_Name, SqlParameter[] SP_Param)
        {
            SqlCommand SqlCmd = new SqlCommand();
            SqlCmd.CommandType = CommandType.StoredProcedure;
            SqlCmd.CommandText = Stored_Procedure_Name;
            SqlCmd.Connection = SqlCon;

            if (SP_Param != null)
            {
                //SqlCmd.Parameters.AddRange(SP_Param);

                // او بالطريقة الاخرى بالستخدام الحلقة التكرارية

                for (int i = 0; i < SP_Param.Length; i++)
                {
                    SqlCmd.Parameters.Add(SP_Param[i]);
                }
            }

            SqlDataAdapter DA = new SqlDataAdapter(SqlCmd);
            DataTable DT = new DataTable();

            try
            {
                DA.Fill(DT);
            }

            catch
            {
                ;
            }

            return DT;

        }

        //*******************************************************************
    }
}

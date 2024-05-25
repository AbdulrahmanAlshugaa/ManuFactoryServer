using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Edex
{
    class clsSalseInvoice
    {

        // جلب كل فواتير المبيعات
        public DataTable getAllSalseInvoices(string barCode)
        {
            clsDataAccess DA = new clsDataAccess();

            SqlParameter[] Param = new SqlParameter[1];
            Param[0] = new SqlParameter("@BarCode", SqlDbType.NVarChar, 100);
            Param[0].Value = barCode;

            DataTable DT = new DataTable();
            DT = DA.SelectData("sp_SalesInvoices", Param);

            DA.Close();
            return DT;
        }

        public DataTable GetInfoProdForSalseInvoice(string barCode)
        {
            clsDataAccess DA = new clsDataAccess();

            SqlParameter[] Param = new SqlParameter[1];
            Param[0] = new SqlParameter("@BarCode", SqlDbType.NVarChar, 100);
            Param[0].Value = barCode;

            DataTable DT = new DataTable();
            DT = DA.SelectData("sp_GetInfoProdForSalseInvoice", Param);

            DA.Close();
            return DT;
        }

    }
}

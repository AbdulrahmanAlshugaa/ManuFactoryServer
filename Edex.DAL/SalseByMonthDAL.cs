using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
 
using Edex.Model;
using Edex.DAL;

namespace Edex.DAL
{
    public class SalseByMonthDAL
    {
        public static SalseByMonthBo ConvertRowToObj(DataRow dr)
        {
            SalseByMonthBo Obj = new SalseByMonthBo();
            Obj.MounthName = dr["MounthName"].ToString();
            Obj.TotalSale = dr["TotalSale"].ToString();
            return Obj;
        }
    }
}

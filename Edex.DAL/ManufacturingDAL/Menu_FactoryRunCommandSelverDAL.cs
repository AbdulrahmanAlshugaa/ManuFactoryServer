using Edex.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.DAL.ManufacturingDAL
{
    public class Menu_FactoryRunCommandSelverDAL
    {
        #region Declare
        public static readonly string TableName = "Menu_FactoryRunCommandSelver";
        public static readonly string PremaryKey = "ID";
        public bool FoundResult;
        public bool NeedSaving;
        public bool IsNewRecord;
        private DataTable dt;
        private string strSQL;
        private object Result;
        #endregion
        public static DataTable frmGetDataDetalByID(float ComandID,int   BRANCHID, int FacilityID, int TypeOpration)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Menu_FactoryRunCommandSelver_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@ComandID", ComandID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BRANCHID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@TypeOpration", TypeOpration));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 5));


                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                            return dt;
                        else
                            return null;
                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
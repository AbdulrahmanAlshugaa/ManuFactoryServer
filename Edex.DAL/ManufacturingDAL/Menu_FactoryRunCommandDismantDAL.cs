using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.DAL.ManufacturingDAL
{
     
    public class Menu_FactoryRunCommandDismantDAL
    {

        public static readonly string TableName = "Menu_FactoryRunCommandDismant";
        public static readonly string PremaryKey = "ComandID";
        public static DataTable frmGetDataDetalByID(float ComandID, int BranchID, int FacilityID, int TypeOpration, int PollutionTypeID = 0, int TypeStageID=10)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Menu_FactoryRunCommandDismant_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@ComandID", ComandID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@TypeOpration", TypeOpration));
                        objCmd.Parameters.Add(new SqlParameter("@TypeStageID", TypeStageID)); 
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
            catch (Exception)
            {
                return null;
            }
        }
    }
}

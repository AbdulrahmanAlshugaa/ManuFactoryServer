
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

    public class Manu_TypeOrdersDAL
    {
        /// <summary>
        /// this function is used to convert row to object Manu_TypeOrders
        /// </summary>
        /// <param name="dr"></param>
        /// <returns>return data by object Manu_TypeOrders </returns>
        public static Manu_TypeOrders ConvertRowToObj(DataRow dr)
        {
            Manu_TypeOrders Obj = new Manu_TypeOrders();
            Obj.ID = int.Parse(dr["ID"].ToString());
            Obj.ArbName = dr["ARBNAME"].ToString();
            Obj.EngName = dr["ENGNAME"].ToString();
            Obj.Notes = dr["Notes"].ToString();
            Obj.UserID = int.Parse(dr["UserID"].ToString());
            Obj.Cancel = int.Parse(dr["Cancel"].ToString());
            Obj.BranchID = int.Parse(dr["BranchID"].ToString());
            Obj.FacilityID = int.Parse(dr["FacilityID"].ToString());
            Obj.EditUserID = Comon.cInt(dr["EditUserID"].ToString());
            Obj.EditDate = Comon.cLong(dr["EditDate"].ToString());
            Obj.EditTime = Comon.cLong(dr["EditTime"].ToString());
            Obj.ComputerInfo = dr["ComputerInfo"].ToString();
            Obj.EditComputerInfo = dr["EditComputerInfo"].ToString();
            //Obj.RegDate = Comon.ConvertSerialDateTo(long.Parse(dr["RegDate"].ToString()));
            //Obj.EditDate = Com.ConvertSerialToDate(long.Parse(dr["EditDate"].ToString()));

            return Obj;
        }

        /// <summary>
        /// this is function is used to get data size  By id 
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="BranchID"></param>
        /// <param name="FacilityID"></param>
        /// <returns></returns>
        public static Manu_TypeOrders GetDataByID(int ID, int BranchID, int FacilityID)
        {
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Manu_TypeOrders_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@ID", ID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 5));
                    SqlDataReader myreader = objCmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(myreader);
                    if (dt != null)
                    {
                        Manu_TypeOrders Returned = new Manu_TypeOrders();
                        Returned = (ConvertRowToObj(dt.Rows[0]));
                        return Returned;
                    }
                    else
                        return null;
                }
            }
        }
        /// <summary>
        /// this function is used to Get all data 
        /// </summary>
        /// <param name="BranchID"></param>
        /// <param name="FacilityID"></param>
        /// <returns>return data by list Manu_TypeOrders</returns>
        public static List<Manu_TypeOrders> GetAllData(int BranchID, int FacilityID)
        {
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Manu_TypeOrders_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 3));
                    SqlDataReader myreader = objCmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(myreader);
                    if (dt != null)
                    {
                        List<Manu_TypeOrders> Returned = new List<Manu_TypeOrders>();
                        foreach (DataRow rows in dt.Rows)
                            Returned.Add(ConvertRowToObj(rows));
                        return Returned;
                    }
                    else
                        return null;
                }
            }
        }

        /// <summary>
        /// this function is used to insert into Manu_TypeOrders table 
        /// </summary>
        /// <param name="objRecord"></param>
        /// <returns>return number opration is saccsess insert or error or onther  </returns>
        public static Int32 Insert(Manu_TypeOrders objRecord, bool IsNewRecord)
        {
            Int32 objRet = 0;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Manu_TypeOrders_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@ID", objRecord.ID)); 
                    objCmd.Parameters.Add(new SqlParameter("@ArbName", objRecord.ArbName));
                    objCmd.Parameters.Add(new SqlParameter("@EngName", objRecord.EngName));
                    objCmd.Parameters.Add(new SqlParameter("@Notes", objRecord.Notes));
                    objCmd.Parameters.Add(new SqlParameter("@UserID", objRecord.UserID));
                    objCmd.Parameters.Add(new SqlParameter("@RegDate", objRecord.RegDate));
                    objCmd.Parameters.Add(new SqlParameter("@RegTime", objRecord.RegTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                    objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditDate", objRecord.EditDate));
                    objCmd.Parameters.Add(new SqlParameter("@ComputerInfo", objRecord.ComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@EditComputerInfo", objRecord.EditComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@Cancel", objRecord.Cancel));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    if (IsNewRecord)
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 1));
                    else
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 2));
                    object obj = objCmd.ExecuteScalar();
                    if (obj != null)
                        objRet = Convert.ToInt32(obj);
                }
            }
            return objRet;
        }
        /// <summary>
        /// this function is used to update to Manu_TypeOrders table 
        /// </summary>
        /// <param name="objRecord"></param>
        /// <returns></returns>
        public static bool Update(Manu_TypeOrders objRecord)
        {
            bool objRet = false;
            objRet = false;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Manu_TypeOrders_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@ID", objRecord.ID));
                    objCmd.Parameters.Add(new SqlParameter("@ArbName", objRecord.ArbName));
                    objCmd.Parameters.Add(new SqlParameter("@EngName", objRecord.EngName));
                    objCmd.Parameters.Add(new SqlParameter("@Notes", objRecord.Notes));
                    objCmd.Parameters.Add(new SqlParameter("@UserID", objRecord.UserID));
                    objCmd.Parameters.Add(new SqlParameter("@RegDate", objRecord.RegDate));
                    objCmd.Parameters.Add(new SqlParameter("@RegTime", objRecord.RegTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                    objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditDate", objRecord.EditDate));
                    objCmd.Parameters.Add(new SqlParameter("@EditComputerInfo", objRecord.EditComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@Cancel", objRecord.Cancel));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 2));
                    objCmd.ExecuteNonQuery();
                }
            }
            objRet = true;
            return objRet;
        }

        /// <summary>
        /// this function is used to delete from Manu_TypeOrders
        /// </summary>
        /// <param name="objRecord"></param>
        /// <returns>return value boolen true if the opration delete is saccess or false if error</returns>
        public static bool Delete(Manu_TypeOrders objRecord)
        {
            bool objRet = false;
            objRet = false;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Manu_TypeOrders_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@ID", objRecord.ID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                    objCmd.Parameters.Add(new SqlParameter("@editdate", objRecord.EditDate));
                    objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 4));
                    objCmd.ExecuteNonQuery();
                }
            }
            objRet = true;
            return objRet;
        }
    }
}

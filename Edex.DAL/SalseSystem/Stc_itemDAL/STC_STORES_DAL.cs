using Edex.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.DAL
{
    public class STC_STORES_DAL
    {
       public Boolean FoundResult = false;
        public static Stc_Stores ConvertRowToObj(DataRow dr)
        { 
            Stc_Stores Obj = new Stc_Stores();
            Obj.StoreID = int.Parse(dr["StoreID"].ToString());
            Obj.ArbName = dr["ARBNAME"].ToString();
            Obj.AccountID =Comon.cLong( dr["AccountID"].ToString());
            Obj.EngName = dr["ENGNAME"].ToString();
            Obj.Notes = dr["NOTES"].ToString();
            Obj.UserID = int.Parse(dr["UserID"].ToString());
            Obj.Fax = dr["FAX"].ToString();
            Obj.Mobile = dr["Mobile"].ToString();
            Obj.Tel = dr["Tel"].ToString();
            Obj.StoreManger = dr["StoreManger"].ToString();
            Obj.Address = dr["Address"].ToString();
            Obj.EditUserID = Comon.cInt(dr["EditUserID"].ToString());
            Obj.ParentAccountID = Comon.cDbl(dr["ParentAccountID"].ToString());
            Obj.StopAccount = Comon.cInt(dr["StopAccount"].ToString());
            //Obj.EditDate = Com.ConvertSerialToDate(long.Parse(dr["EditDate"].ToString()));
            return Obj;
        }
        public static Stc_Stores GetDataByID(int ID, int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Stc_Stores_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@StoreID  ", ID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 3));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            Stc_Stores Returned = new Stc_Stores();
                            Returned = (ConvertRowToObj(dt.Rows[0]));
                            return Returned;
                        }
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
        public static List<Stc_Stores> GetAllData(int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Stc_Stores_SP]";
                        objCmd.Parameters.AddWithValue("@BranchID", BranchID);
                        objCmd.Parameters.AddWithValue("@FacilityID", FacilityID);

                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 5));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            List<Stc_Stores> Returned = new List<Stc_Stores>();
                            foreach (DataRow rows in dt.Rows)
                                Returned.Add(ConvertRowToObj(rows));
                            return Returned;
                        }
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
        public static Int32 InsertStc_Stores(Stc_Stores objRecord)
        {
            Int32 objRet = 0;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Stc_Stores_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@StoreID", objRecord.StoreID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@AccountID", objRecord.AccountID));
                    objCmd.Parameters.Add(new SqlParameter("@StopAccount", objRecord.StopAccount));
                    objCmd.Parameters.Add(new SqlParameter("@ParentAccountID", objRecord.ParentAccountID));
                    objCmd.Parameters.Add(new SqlParameter("@ArbName", objRecord.ArbName));
                    objCmd.Parameters.Add(new SqlParameter("@EngName", objRecord.EngName));
                    objCmd.Parameters.Add(new SqlParameter("@Address", objRecord.Address));
                    objCmd.Parameters.Add(new SqlParameter("@Tel", objRecord.Tel));
                    objCmd.Parameters.Add(new SqlParameter("@Fax", objRecord.Fax));
                    objCmd.Parameters.Add(new SqlParameter("@StoreManger", objRecord.StoreManger));
                    objCmd.Parameters.Add(new SqlParameter("@Mobile", objRecord.Mobile));
                    objCmd.Parameters.Add(new SqlParameter("@Notes", objRecord.Notes));
                    objCmd.Parameters.Add(new SqlParameter("@UserID", objRecord.UserID));
                    objCmd.Parameters.Add(new SqlParameter("@RegDate", objRecord.RegDate));
                    objCmd.Parameters.Add(new SqlParameter("@RegTime", objRecord.RegTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                    objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditDate", objRecord.EditDate));
                    objCmd.Parameters.Add(new SqlParameter("@ComputerInfo", objRecord.ComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@EditComputerInfo", objRecord.EditComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@Cancel", 0));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 1));
                    object obj = objCmd.ExecuteScalar();
                    if (obj != null)
                        objRet = Convert.ToInt32(obj);
                }
            }
            return objRet;
        }
        public static bool UpdateStc_Stores(Stc_Stores objRecord)
        {
            bool objRet = false;
            objRet = false;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Stc_Stores_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@StoreID", objRecord.StoreID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@AccountID", objRecord.AccountID));

                    objCmd.Parameters.Add(new SqlParameter("@StopAccount", objRecord.StopAccount));
                    objCmd.Parameters.Add(new SqlParameter("@ParentAccountID", objRecord.ParentAccountID));
                    objCmd.Parameters.Add(new SqlParameter("@ArbName", objRecord.ArbName));
                    objCmd.Parameters.Add(new SqlParameter("@EngName", objRecord.EngName));
                    objCmd.Parameters.Add(new SqlParameter("@Address", objRecord.Address));
                    objCmd.Parameters.Add(new SqlParameter("@Tel", objRecord.Tel));
                    objCmd.Parameters.Add(new SqlParameter("@Fax", objRecord.Fax));
                    objCmd.Parameters.Add(new SqlParameter("@StoreManger", objRecord.StoreManger));
                    objCmd.Parameters.Add(new SqlParameter("@Mobile", objRecord.Mobile));
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
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 2));
                    objCmd.ExecuteNonQuery();
                }
            }
            objRet = true;
            return objRet;
        }


        public static bool DeleteStc_Stores(Stc_Stores objRecord)
        {
            bool objRet = false;
            objRet = false;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Stc_Stores_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@StoreID", objRecord.StoreID));
                    objCmd.Parameters.Add(new SqlParameter("@ModifiedBy", objRecord.UserID));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 4));
                    objCmd.ExecuteNonQuery();
                }
            }
            objRet = true;
            return objRet;
        }
        public static bool DeleteStc_StoresByAccountID(Stc_Stores objRecord)
        {
            bool objRet = false;
            objRet = false;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Stc_Stores_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@AccountID", objRecord.AccountID));
                    objCmd.Parameters.Add(new SqlParameter("@ModifiedBy", objRecord.UserID));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 6));
                    objCmd.ExecuteNonQuery();
                }
            }
            objRet = true;
            return objRet;
        }


        public   List<Stc_Stores> GetAll(int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Stc_Stores_SP]";
                        objCmd.Parameters.AddWithValue("@BranchID", BranchID);
                        objCmd.Parameters.AddWithValue("@FacilityID", FacilityID);

                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 5));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            List<Stc_Stores> Returned = new List<Stc_Stores>();
                            foreach (DataRow rows in dt.Rows)
                                Returned.Add(ConvertRowToObj(rows));
                            return Returned;
                        }
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

        public static  DataTable  GetAllDt(int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Stc_Stores_SP]";
                        objCmd.Parameters.AddWithValue("@BranchID", BranchID);
                        objCmd.Parameters.AddWithValue("@FacilityID", FacilityID);

                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 5));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {

                            return dt;
                        }
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


        public static Stc_Stores GetRecordSetBySQL(string strSQL)
        {
            try
            {

                DataTable dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count > 0)
                    return ConvertRowToObj(dt.Rows[0]);
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;

            }
        }
        

        public static  long GetNewID()
        {
            try
            {
                DataTable dt;
                string strSQL;
                Stc_Stores cClass = new Stc_Stores();
                strSQL = "SELECT Max(" + cClass.PremaryKey + ") + 1 FROM " + cClass.TableName+"  where BranchID="+MySession.GlobalBranchID;
                dt = Lip.SelectRecord(strSQL);
                string GetNewID = dt.Rows[0][0] == DBNull.Value ? "1" : dt.Rows[0][0].ToString();
                return Convert.ToInt32(GetNewID);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}

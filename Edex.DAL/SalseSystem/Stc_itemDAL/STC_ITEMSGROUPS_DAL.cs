using Edex.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.DAL
{
    public class STC_ITEMSGROUPS_DAL
    {

        public static Stc_ItemsGroups ConvertRowToObj(DataRow dr)
        {
            Stc_ItemsGroups Obj = new Stc_ItemsGroups();
            Obj.GroupID =  dr["GroupID"].ToString();
            Obj.ArbName = dr["ARBNAME"].ToString();
            Obj.EngName = dr["ENGNAME"].ToString();
            Obj.Notes = dr["NOTES"].ToString();
            Obj.UserID = int.Parse(dr["UserID"].ToString());
            Obj.EditUserID = Comon.cInt(dr["EditUserID"].ToString());
            Obj.MaxLimit = Comon.cDbl(dr["MaxLimit"].ToString());
            Obj.MinLimit = Comon.cDbl(dr["MinLimit"].ToString());
            Obj.ParentAccountID =dr["ParentAccountID"].ToString();
            Obj.AccountLevel = Comon.cInt(dr["AccountLevel"].ToString());

            Obj.AccountTypeID =Comon.cInt( dr["AccountTypeID"].ToString());
            Obj.StopAccount = Comon.cInt(dr["StopAccount"].ToString());
            //Obj.RegDate = Comon.ConvertSerialToDate(long.Parse(dr["RegDate"].ToString()));
            //Obj.EditDate = Com.ConvertSerialToDate(long.Parse(dr["EditDate"].ToString()));
            return Obj;
        }

        public static Stc_ItemsGroups GetDataByID(string ID, int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Stc_ItemsGroups_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@GroupID  ", ID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 3));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            Stc_ItemsGroups Returned = new Stc_ItemsGroups();
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

        public static List<Stc_ItemsGroups> GetAllData()
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Stc_ItemsGroups_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", MySession.GlobalBranchID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 5));

                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            List<Stc_ItemsGroups> Returned = new List<Stc_ItemsGroups>();
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

        public static Int32 InsertStc_Groups(Stc_ItemsGroups objRecord, bool IsNewRecord)
        {
            Int32 objRet = 0;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Stc_ItemsGroups_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@GroupID", objRecord.GroupID));
                    objCmd.Parameters.Add(new SqlParameter("@ArbName", objRecord.ArbName));
                    objCmd.Parameters.Add(new SqlParameter("@EngName", objRecord.EngName));
                    objCmd.Parameters.Add(new SqlParameter("@Notes", objRecord.Notes));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@UserID", objRecord.UserID));
                    objCmd.Parameters.Add(new SqlParameter("@RegDate", objRecord.RegDate));
                    objCmd.Parameters.Add(new SqlParameter("@RegTime", objRecord.RegTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                    objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditDate", objRecord.EditDate));
                    objCmd.Parameters.Add(new SqlParameter("@ComputerInfo", objRecord.ComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@EditComputerInfo", objRecord.EditComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@Cancel", objRecord.Cancel));
                    objCmd.Parameters.Add(new SqlParameter("@ParentAccountID", objRecord.ParentAccountID));
                    objCmd.Parameters.Add(new SqlParameter("@AccountLevel", objRecord.AccountLevel));
                    objCmd.Parameters.Add(new SqlParameter("@AccountTypeID", objRecord.AccountTypeID));
                    objCmd.Parameters.Add(new SqlParameter("@StopAccount", objRecord.StopAccount));
                    objCmd.Parameters.Add(new SqlParameter("@MinLimit", objRecord.MinLimit));
                    objCmd.Parameters.Add(new SqlParameter("@MaxLimit", objRecord.MaxLimit));

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
     

        public static bool UpdateStc_Stores(Stc_ItemsGroups objRecord)
        {
            bool objRet = false;
            objRet = false;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Stc_ItemsGroups_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@GroupID", objRecord.GroupID));
                    objCmd.Parameters.Add(new SqlParameter("@ArbName", objRecord.ArbName));
                    objCmd.Parameters.Add(new SqlParameter("@EngName", objRecord.EngName));
                    objCmd.Parameters.Add(new SqlParameter("@Notes", objRecord.Notes));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@UserID", objRecord.UserID));
                    objCmd.Parameters.Add(new SqlParameter("@RegDate", objRecord.RegDate));
                    objCmd.Parameters.Add(new SqlParameter("@RegTime", objRecord.RegTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                    objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditDate", objRecord.EditDate));
                    objCmd.Parameters.Add(new SqlParameter("@EditComputerInfo", objRecord.EditComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@Cancel", objRecord.Cancel));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 2));
                    objCmd.ExecuteNonQuery();
                }
            }
            objRet = true;
            return objRet;
        }

        public static bool DeleteStc_Groups(Stc_ItemsGroups objRecord)
        {
            bool objRet = false;
            objRet = false;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Stc_ItemsGroups_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@GroupID", objRecord.GroupID));
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
      

        public static Stc_ItemsGroups GetRecordSetBySQL(string strSQL)
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


        public static long GetNewID()
        {
            try
            {
                DataTable dt;
                string strSQL;
                Stc_ItemsGroups cClass = new Stc_ItemsGroups();
                strSQL = "SELECT Max(" + cClass.PremaryKey + ") + 1 FROM " + cClass.TableName;
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

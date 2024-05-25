using Edex.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.DAL.Accounting
{
    public class DeclaringIncomeAccountsDAL
    {
        public static Acc_DeclaringIncomeAccounts ConvertRowToObj(DataRow dr)
        {

            Acc_DeclaringIncomeAccounts Obj = new Acc_DeclaringIncomeAccounts();
            Obj.ID = Comon.cInt(dr["ID"].ToString());
            Obj.AccountID = double.Parse(dr["AccountID"].ToString());
            Obj.DeclareAccountName = dr["DeclareAccountName"].ToString();
            Obj.AccountName = dr["AccountName"].ToString();
            Obj.AccountArbName = dr["AlisedAccountName"].ToString();
            // Obj.AccountEngName = dr["AccountEngName"].ToString();
            //Obj.BranchID = int.Parse(dr["BranchID"].ToString());
            //Obj.FacilityID = int.Parse(dr["FacilityID"].ToString());
            //Obj.UserID = int.Parse(dr["UserID"].ToString());
            //Obj.RegDate = (long.Parse(dr["RegDate"].ToString()));
            //Obj.EditUserID = Comon.cInt(dr["EditUserID"].ToString());
            //Obj.RegTime = (long.Parse(dr["RegTime"].ToString()));
            //Obj.EditUserID = (int.Parse(dr["EditUserID"].ToString()));
            //Obj.EditDate = (long.Parse(dr["EditDate"].ToString()));
            //Obj.EditTime = (int.Parse(dr["EditTime"].ToString()));
            //Obj.ComputerInfo = dr["ComputerInfo"].ToString();
            //Obj.EditComputerInfo = dr["EditComputerInfo"].ToString();
            return Obj;
        }
        public static Acc_DeclaringIncomeAccounts GetDataByID(int ID, int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Acc_DeclaringIncomeAccounts_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@ID", ID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));

                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 3));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            Acc_DeclaringIncomeAccounts Returned = new Acc_DeclaringIncomeAccounts();
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
        public static List<Acc_DeclaringIncomeAccounts> GetAllData(int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Acc_DeclaringIncomeAccounts_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 5));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            List<Acc_DeclaringIncomeAccounts> Returned = new List<Acc_DeclaringIncomeAccounts>();
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
        public static DataTable Get_DeclaringIncomeAccounts(int BranchID, int FacilityID)
        {
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Acc_DeclaringIncomeAccounts_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 5));
                    SqlDataReader myreader = objCmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(myreader);
                    return dt;
                }
            }
        }
        public static bool Update_DeclaringIncomeAccounts(Acc_DeclaringIncomeAccounts objRecord)
        {
            bool objRet = false;
            objRet = false;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Acc_DeclaringIncomeAccounts_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@AccountID", objRecord.AccountID));
                    objCmd.Parameters.Add(new SqlParameter("@ID", objRecord.ID));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                    objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditDate", objRecord.EditDate));
                    objCmd.Parameters.Add(new SqlParameter("@EditComputerInfo", objRecord.EditComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 2));
                    objCmd.ExecuteNonQuery();
                }
            }
            objRet = true;
            return objRet;
        }
    }
}

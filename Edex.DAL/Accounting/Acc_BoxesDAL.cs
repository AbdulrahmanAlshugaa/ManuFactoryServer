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
    public class Acc_BoxesDAL
    {

        public static Acc_Boxes ConvertRowToObj(DataRow dr)
            {
                Acc_Boxes Obj = new Acc_Boxes();
                Obj.BoxID = Comon.cInt(dr["BoxID"].ToString());
                Obj.BranchID = Comon.cInt(dr["BranchID"].ToString());
                Obj.FacilityID = Comon.cInt(dr["FacilityID"].ToString());
                Obj.AccountID = Comon.cLong(dr["AccountID"].ToString());
                Obj.ArbName = dr["ArbName"].ToString();
                Obj.EngName = dr["EngName"].ToString(); 
                Obj.Notes = dr["Notes"].ToString();
                Obj.UserID = Comon.cInt(dr["UserID"].ToString());
                Obj.RegDate = Comon.cLong(dr["RegDate"].ToString());
                Obj.RegTime = Comon.cLong(dr["RegTime"].ToString());
                Obj.EditUserID = Comon.cInt(dr["EditUserID"].ToString());
                Obj.EditTime = Comon.cLong(dr["EditTime"].ToString());
                Obj.EditDate = Comon.cLong(dr["EditDate"].ToString());
                Obj.ComputerInfo = dr["ComputerInfo"].ToString();
                Obj.EditComputerInfo = dr["EditComputerInfo"].ToString();
                Obj.Cancel = Comon.cInt(dr["Cancel"].ToString());
               
                return Obj;
            }

        public static Acc_Boxes GetDataByID(int ID, int BranchID, int FacilityID)
            {
                try
                {
                    using (SqlConnection objCnn = new GlobalConnection().Conn)
                    {
                        objCnn.Open();
                        using (SqlCommand objCmd = objCnn.CreateCommand())
                        {
                            objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                            objCmd.CommandText = "[Acc_Boxes_SP]";
                            objCmd.Parameters.Add(new SqlParameter("@BoxID", ID));
                            objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                            objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                            objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 3));
                            SqlDataReader myreader = objCmd.ExecuteReader();
                            DataTable dt = new DataTable();
                            dt.Load(myreader);
                            if (dt != null)
                            {
                                Acc_Boxes Returned = new Acc_Boxes();
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

            public static List<Acc_Boxes> GetAllData(int BranchID, int FacilityID)
            {
                try
                {
                    using (SqlConnection objCnn = new GlobalConnection().Conn)
                    {
                        objCnn.Open();
                        using (SqlCommand objCmd = objCnn.CreateCommand())
                        {
                            objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                            objCmd.CommandText = "[Acc_Boxes_SP]";
                            objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                            objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                            objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 5));
                            SqlDataReader myreader = objCmd.ExecuteReader();
                            DataTable dt = new DataTable();
                            dt.Load(myreader);
                            if (dt != null)
                            {
                                List<Acc_Boxes> Returned = new List<Acc_Boxes>();
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

            public DataTable GetAcc_Boxes(int FacilityID, int BranchID)
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Acc_Boxes_SP]";
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

            public static Int32 Insert_Acc_Boxes(Acc_Boxes objRecord)
            {
                int objRet = 0;
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Acc_Boxes_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@BoxID", objRecord.BoxID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@StopAccount", objRecord.StopAccount));
                        objCmd.Parameters.Add(new SqlParameter("@AccountID", objRecord.AccountID));
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
                        objCmd.Parameters.Add(new SqlParameter("@ParentAccountID", objRecord.ParentAccountID)); 
                        SqlParameter pvNewId = new SqlParameter();
                        pvNewId.ParameterName = "@product_count";
                        pvNewId.DbType = DbType.Int32;
                        pvNewId.Direction = ParameterDirection.Output;
                        objCmd.Parameters.Add(pvNewId);

                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 1));
                        object obj = objCmd.ExecuteScalar();
                        string val = objCmd.Parameters["@product_count"].Value.ToString();

                        if (val != null)
                            objRet = Convert.ToInt32(val);
                    }
                }
                return objRet;
            }

            public static Int32 InsertSales_Drivers(Acc_Boxes objRecord)
            {
                int objRet = 0;
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Sales_Drivers_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@DriverID", objRecord.BoxID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@AccountID", objRecord.AccountID));
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

                        SqlParameter pvNewId = new SqlParameter();
                        pvNewId.ParameterName = "@product_count";
                        pvNewId.DbType = DbType.Int32;
                        pvNewId.Direction = ParameterDirection.Output;
                        objCmd.Parameters.Add(pvNewId);

                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 1));
                        object obj = objCmd.ExecuteScalar();
                        string val = objCmd.Parameters["@product_count"].Value.ToString();

                        if (val != null)
                            objRet = Convert.ToInt32(val);
                    }
                }
                return objRet;
            }


            public static Int32 Update_Acc_Boxes(Acc_Boxes objRecord)
            {
                Int32 objRet = 0;
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Acc_Boxes_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@BoxID", objRecord.BoxID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@StopAccount", objRecord.StopAccount));
                        objCmd.Parameters.Add(new SqlParameter("@AccountID", objRecord.AccountID));
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
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 2));
                        objCmd.Parameters.Add(new SqlParameter("@ParentAccountID", objRecord.ParentAccountID)); 
                        SqlParameter pvNewId = new SqlParameter();
                        pvNewId.ParameterName = "@product_count";
                        pvNewId.DbType = DbType.Int32;
                        pvNewId.Direction = ParameterDirection.Output;
                        objCmd.Parameters.Add(pvNewId);



                        object obj = objCmd.ExecuteScalar();
                        string val = objCmd.Parameters["@product_count"].Value.ToString();

                        if (val != null)
                            objRet = Convert.ToInt32(val);
                    }
                }
                return objRet;
            }

            public static bool DeleteAcc_Boxes(Acc_Boxes objRecord)
            {
                bool objRet = false;
                objRet = false;
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Acc_Boxes_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@BoxID", objRecord.BoxID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                        objCmd.Parameters.Add(new SqlParameter("@editdate", objRecord.EditDate));
                        objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));

                        SqlParameter pvNewId = new SqlParameter();
                        pvNewId.ParameterName = "@product_count";
                        pvNewId.DbType = DbType.Int32;
                        pvNewId.Direction = ParameterDirection.Output;
                        objCmd.Parameters.Add(pvNewId);

                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 4));
                        objCmd.ExecuteNonQuery();
                    }
                }
                objRet = true;
                return objRet;
            }
            public static bool DeleteAcc_BoxesByAccountID(Acc_Boxes objRecord)
            {
                bool objRet = false;
                objRet = false;
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Acc_Boxes_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@AccountID", objRecord.AccountID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                        objCmd.Parameters.Add(new SqlParameter("@editdate", objRecord.EditDate));
                        objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));

                        SqlParameter pvNewId = new SqlParameter();
                        pvNewId.ParameterName = "@product_count";
                        pvNewId.DbType = DbType.Int32;
                        pvNewId.Direction = ParameterDirection.Output;
                        objCmd.Parameters.Add(pvNewId);

                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 6));
                        objCmd.ExecuteNonQuery();
                    }
                }
                objRet = true;
                return objRet;
            }
            public static Acc_Boxes GetRecordSetBySQL(string strSQL)
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
                    Acc_Boxes cClass = new Acc_Boxes();
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

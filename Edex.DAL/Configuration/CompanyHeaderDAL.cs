using Edex.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.DAL.Configuration
{
    public class CompanyHeaderDAL
    {


        public static CompanyHeader ConvertRowToObj(DataRow dr)
            {
                CompanyHeader Obj = new CompanyHeader();
                Obj.ID = Comon.cInt(dr["ID"].ToString());
                Obj.pic = (dr["pic"] == DBNull.Value ? null : (byte[])dr["pic"]);
                Obj.BranchID = Comon.cInt(dr["BranchID"].ToString());
                Obj.FacilityID = Comon.cInt(dr["FacilityID"].ToString());;
                Obj.CompanyArbName = dr["CompanyArbName"].ToString();
                Obj.CompanyEngName = dr["CompanyEngName"].ToString();
                Obj.ArbTel = dr["ArbTel"].ToString();
                Obj.ArbFax = dr["ArbFax"].ToString();
                Obj.ActivityArbName = dr["ActivityArbName"].ToString();
                Obj.ActivityEngName = dr["ActivityEngName"].ToString();
                Obj.EngTel = dr["EngTel"].ToString();
                Obj.EngFax = dr["EngFax"].ToString();
                Obj.EngAddress = dr["EngAddress"].ToString();
                Obj.ArbAddress = dr["ArbAddress"].ToString();
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
            public static CompanyHeader GetDataByID(int ID, int BranchID, int FacilityID)
            {
                try
                {
                    using (SqlConnection objCnn = new GlobalConnection().Conn)
                    {
                        objCnn.Open();
                        using (SqlCommand objCmd = objCnn.CreateCommand())
                        {
                            objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                            objCmd.CommandText = "[CompanyHeader_SP]";
                            objCmd.Parameters.Add(new SqlParameter("@ID  ", ID));
                            objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                            objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                            objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 3));
                            SqlDataReader myreader = objCmd.ExecuteReader();
                            DataTable dt = new DataTable();
                            dt.Load(myreader);
                            if (dt != null)
                            {
                                if (dt.Rows.Count> 0)
                                {
                                    CompanyHeader Returned = new CompanyHeader();
                                    Returned = (ConvertRowToObj(dt.Rows[0]));
                                    return Returned;
                                }
                                else
                                    return null;
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
            public static List<CompanyHeader> GetAllData(int BranchID, int FacilityID)
            {
                try
                {
                    using (SqlConnection objCnn = new GlobalConnection().Conn)
                    {
                        objCnn.Open();
                        using (SqlCommand objCmd = objCnn.CreateCommand())
                        {
                            objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                            objCmd.CommandText = "[CompanyHeader_SP]";
                            objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                            objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                            objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 5));
                            SqlDataReader myreader = objCmd.ExecuteReader();
                            DataTable dt = new DataTable();
                            dt.Load(myreader);
                            if (dt != null)
                            {
                                List<CompanyHeader> Returned = new List<CompanyHeader>();
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
            public DataTable GetCompanyHeader(int FacilityID, int BranchID)
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[CompanyHeader_SP]";
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
            public static Int32 InsertCompanyHeader(CompanyHeader objRecord)
            {
                Int32 objRet = 0;
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[CompanyHeader_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@pic", objRecord.pic));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@ActivityArbName", objRecord.ActivityArbName));
                        objCmd.Parameters.Add(new SqlParameter("@ActivityEngName", objRecord.ActivityEngName));
                        objCmd.Parameters.Add(new SqlParameter("@ArbAddress", objRecord.ArbAddress));
                        objCmd.Parameters.Add(new SqlParameter("@ArbFax", objRecord.ArbFax));
                        objCmd.Parameters.Add(new SqlParameter("@ArbTel", objRecord.ArbTel));
                        objCmd.Parameters.Add(new SqlParameter("@CompanyArbName", objRecord.CompanyArbName));
                        objCmd.Parameters.Add(new SqlParameter("@CompanyEngName", objRecord.CompanyEngName));
                        objCmd.Parameters.Add(new SqlParameter("@EngAddress", objRecord.EngAddress));
                        objCmd.Parameters.Add(new SqlParameter("@UserID", objRecord.UserID));
                        objCmd.Parameters.Add(new SqlParameter("@RegDate", objRecord.RegDate));
                        objCmd.Parameters.Add(new SqlParameter("@RegTime", objRecord.RegTime));
                        objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                        objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));
                        objCmd.Parameters.Add(new SqlParameter("@EditDate", objRecord.EditDate));
                        objCmd.Parameters.Add(new SqlParameter("@ComputerInfo", objRecord.ComputerInfo));
                        objCmd.Parameters.Add(new SqlParameter("@EditComputerInfo", objRecord.EditComputerInfo));
                        objCmd.Parameters.Add(new SqlParameter("@Cancel", objRecord.Cancel));
                        objCmd.Parameters.Add(new SqlParameter("@EngFax", objRecord.EngFax));
                        objCmd.Parameters.Add(new SqlParameter("@EngTel", objRecord.EngTel));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 1));
                        object obj = objCmd.ExecuteScalar();
                        if (obj != null)
                            objRet = Convert.ToInt32(obj);
                    }
                }
                return objRet;
            }
            public static bool UpdateCompanyHeader(CompanyHeader objRecord)
            {
                bool objRet = false;
                objRet = false;
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[CompanyHeader_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@ID", objRecord.ID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@ActivityArbName", objRecord.ActivityArbName));
                        objCmd.Parameters.Add(new SqlParameter("@ActivityEngName", objRecord.ActivityEngName));
                        objCmd.Parameters.Add(new SqlParameter("@ArbFax", objRecord.ArbFax));
                        objCmd.Parameters.Add(new SqlParameter("@ArbTel", objRecord.ArbTel));
                        objCmd.Parameters.Add(new SqlParameter("@EngFax", objRecord.EngFax));
                        objCmd.Parameters.Add(new SqlParameter("@EngTel", objRecord.EngTel));
                        objCmd.Parameters.Add(new SqlParameter("@CompanyArbName", objRecord.CompanyArbName));
                        objCmd.Parameters.Add(new SqlParameter("@CompanyEngName", objRecord.CompanyEngName));
                        objCmd.Parameters.Add(new SqlParameter("@EngAddress", objRecord.EngAddress));
                        objCmd.Parameters.Add(new SqlParameter("@ArbAddress", objRecord.ArbAddress));
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
            public static bool DeleteCompanyHeader(CompanyHeader objRecord)
            {
                bool objRet = false;
                objRet = false;
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[CompanyHeader_SP]";
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

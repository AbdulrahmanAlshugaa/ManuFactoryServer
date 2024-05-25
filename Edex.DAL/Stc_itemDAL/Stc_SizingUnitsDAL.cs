﻿using Edex.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.DAL
{
    public class Stc_SizingUnitsDAL
    {
        /// <summary>
        /// this function is used to convert row to object Stc_SizingUnits
        /// </summary>
        /// <param name="dr"></param>
        /// <returns>return data by object Stc_SizingUnits </returns>
        public static Stc_SizingUnits ConvertRowToObj(DataRow dr)
        {
            Stc_SizingUnits Obj = new Stc_SizingUnits();
            Obj.SizeID = int.Parse(dr["SizeID"].ToString());
            Obj.ArbName = dr["ARBNAME"].ToString();
            Obj.EngName = dr["ENGNAME"].ToString();
            Obj.Notes = dr["Notes"].ToString();
            Obj.typeUnit = int.Parse(dr["typeUnit"].ToString());
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
        /// <param name="SizeID"></param>
        /// <param name="BranchID"></param>
        /// <param name="FacilityID"></param>
        /// <returns></returns>
        public static Stc_SizingUnits GetDataByID(int SizeID, int BranchID, int FacilityID)
        {
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Stc_SizingUnits_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@SizeID", SizeID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 5));
                    SqlDataReader myreader = objCmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(myreader);
                    if (dt != null)
                    {
                        Stc_SizingUnits Returned = new Stc_SizingUnits();
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
        /// <returns>return data by list Stc_SizingUnits</returns>
        public static List<Stc_SizingUnits> GetAllData(int BranchID, int FacilityID)
        {
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Stc_SizingUnits_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 3));
                    SqlDataReader myreader = objCmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(myreader);
                    if (dt != null)
                    {
                        List<Stc_SizingUnits> Returned = new List<Stc_SizingUnits>();
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
        /// this function is used to insert into Stc_SizingUnits table 
        /// </summary>
        /// <param name="objRecord"></param>
        /// <returns>return number opration is saccsess insert or error or onther  </returns>
        public static Int32 Insert(Stc_SizingUnits objRecord)
        {
            Int32 objRet = 0;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Stc_SizingUnits_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@SizeID", objRecord.SizeID));
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
                    objCmd.Parameters.Add(new SqlParameter("@typeUnit", objRecord.typeUnit));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    if (objRecord.SizeID == 0)
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
        /// this function is used to update to Stc_SizingUnits table 
       /// </summary>
       /// <param name="objRecord"></param>
       /// <returns></returns>
        public static bool Update(Stc_SizingUnits objRecord)
        {
            bool objRet = false;
            objRet = false;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Stc_SizingUnits_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@SizeID", objRecord.SizeID));
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
                    objCmd.Parameters.Add(new SqlParameter("@typeUnit", objRecord.typeUnit));
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
      /// this function is used to delete from Stc_SizingUnits
      /// </summary>
      /// <param name="objRecord"></param>
      /// <returns>return value boolen true if the opration delete is saccess or false if error</returns>
        public static bool Delete(Stc_SizingUnits objRecord)
        {
            bool objRet = false;
            objRet = false;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Stc_SizingUnits_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@SizeID", objRecord.SizeID));
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

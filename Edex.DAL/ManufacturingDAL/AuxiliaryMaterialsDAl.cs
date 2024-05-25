﻿using Edex.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Edex.DAL.ManufacturingDAL
{
  public  class AuxiliaryMaterialsDAl
    {

        #region Declare
      public static readonly string TableName = "Manu_AuxiliaryMaterialsMaster";
        public static readonly string PremaryKey = "CommandID";
        public bool FoundResult;
        public bool NeedSaving;
        public bool IsNewRecord;
        private DataTable dt;
        private string strSQL;
        private object Result;
        #endregion

        /// <summary>
        /// This Function is used to Convert DataRow to object  Manu_AuxiliaryMaterialsMaster
        /// </summary>
        /// <param name="dr"></param>
        /// <returns> return object Stc_ItemUnits </returns>
        public static Manu_AuxiliaryMaterialsMaster ConvertRowToObj(DataRow dr)
        {
            Manu_AuxiliaryMaterialsMaster Menu_F_AuxiliaryMaterial = new Manu_AuxiliaryMaterialsMaster();

            Menu_F_AuxiliaryMaterial.CommandID = Comon.cInt(dr["CommandID"].ToString());
            Menu_F_AuxiliaryMaterial.CommandDate =Comon.ConvertDateToSerial(dr["CommandDate"].ToString());
            Menu_F_AuxiliaryMaterial.CustomerID = Comon.cDbl(dr["CustomerID"].ToString());
            Menu_F_AuxiliaryMaterial.DelegetID = Comon.cInt(dr["DelegetID"].ToString());
            Menu_F_AuxiliaryMaterial.ReferanceID = Comon.cInt(dr["ReferanceID"].ToString());
          
            Menu_F_AuxiliaryMaterial.TypeCommand =Comon.cInt( dr["TypeCommand"].ToString());
            Menu_F_AuxiliaryMaterial.UserID = Comon.cInt(dr["UserID"].ToString());
            Menu_F_AuxiliaryMaterial.RegTime =Comon.cDbl( dr["RegTime"].ToString());
            Menu_F_AuxiliaryMaterial.BranchID = Comon.cInt(dr["BranchID"].ToString());
            Menu_F_AuxiliaryMaterial.FacilityID = Comon.cInt(dr["FacilityID"].ToString());
            Menu_F_AuxiliaryMaterial.Cancel = Comon.cInt(dr["Cancel"].ToString());
            Menu_F_AuxiliaryMaterial.RegDate = Comon.cDbl(dr["RegDate"].ToString());
            Menu_F_AuxiliaryMaterial.Notes = dr["Notes"].ToString();
            Menu_F_AuxiliaryMaterial.EmployeeStokID = Comon.cDbl(dr["EmployeeStokID"].ToString());
            Menu_F_AuxiliaryMaterial.EditUserID = Comon.cInt(dr["EditUserID"].ToString());
            Menu_F_AuxiliaryMaterial.EditTime = Comon.cDbl(dr["EditTime"].ToString());
            Menu_F_AuxiliaryMaterial.EditDate = Comon.cDbl(dr["EditDate"].ToString());
            Menu_F_AuxiliaryMaterial.EditComputerInfo = dr["EditComputerInfo"].ToString();
            Menu_F_AuxiliaryMaterial.CurrencyID = Comon.cInt(dr["CurrencyID"].ToString());
            Menu_F_AuxiliaryMaterial.ComputerInfo = (dr["ComputerInfo"].ToString());
            Menu_F_AuxiliaryMaterial.StoreID = Comon.cDbl(dr["AccountID"].ToString());
           

            return Menu_F_AuxiliaryMaterial;
        }
        /// <summary>
        /// This function is used to get PrntageAndPulishn
        /// </summary>
        /// <param name="ComandID"></param>
        /// <param name="BranchID"></param>
        /// <param name="FacilityID"></param>
        /// <returns>return object DataTable </returns>
        public static DataTable GetAuxiliaryMaterials(int ComandID, int BranchID, int FacilityID)
        {
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Manu_AuxiliaryMaterialsMaster_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@CommandID", ComandID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID ", FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 3));
                    SqlDataReader myreader = objCmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(myreader);
                    return dt;
                }
            }
        }
        /// <summary>
        /// This function is used to Convert Object data To XML String
        /// </summary>
        /// <param name="classObject"></param>
        /// <returns>return data with  string type </returns>
        static string ConvertObjectToXMLString(object classObject)
        {
            string xmlString = null;
            XmlSerializer xmlSerializer = new XmlSerializer(classObject.GetType());
            using (MemoryStream memoryStream = new MemoryStream())
            {
                xmlSerializer.Serialize(memoryStream, classObject);
                memoryStream.Position = 0;
                xmlString = new StreamReader(memoryStream).ReadToEnd();
            }
            return xmlString;
        }

        /// <summary>
        /// this function is used Insert Using XML
        /// </summary>
        /// <param name="objRecord"></param>
        /// <param name="IsNewRecord"></param>
        /// <returns></returns>
        public static string InsertUsingXML(Manu_AuxiliaryMaterialsMaster objRecord, Boolean IsNewRecord)
        {
            string objRet = "0";

            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    string DitmeXML = ConvertObjectToXMLString(objRecord.Menu_F_AuxiliaryMaterials);
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Manu_AuxiliaryMaterialsMaster_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@XmlDataAlcad", SqlDbType.Xml)).Value = DitmeXML;
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@StoreID", objRecord.StoreID));
                    objCmd.Parameters.Add(new SqlParameter("@AccountID", objRecord.AccountID));
                    objCmd.Parameters.Add(new SqlParameter("@Cancel", 0));
                    objCmd.Parameters.Add(new SqlParameter("@CommandDate", objRecord.CommandDate));
                    objCmd.Parameters.Add(new SqlParameter("@FactorID", objRecord.FactorID));
                    objCmd.Parameters.Add(new SqlParameter("@CostCenterID", objRecord.CostCenterID)); 
                    objCmd.Parameters.Add(new SqlParameter("@CommandID", objRecord.CommandID));
                    objCmd.Parameters.Add(new SqlParameter("@ComputerInfo", objRecord.ComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@CurrencyID", objRecord.CurrencyID));
                    objCmd.Parameters.Add(new SqlParameter("@CustomerID", objRecord.CustomerID));
                    objCmd.Parameters.Add(new SqlParameter("@DelegetID", objRecord.DelegetID));
                    objCmd.Parameters.Add(new SqlParameter("@EditComputerInfo", objRecord.EditComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@EditDate", objRecord.EditDate));
                    objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));              
                    objCmd.Parameters.Add(new SqlParameter("@EmployeeStokID", objRecord.EmployeeStokID));
                    objCmd.Parameters.Add(new SqlParameter("@Notes", objRecord.Notes));
                    objCmd.Parameters.Add(new SqlParameter("@ReferanceID", objRecord.ReferanceID));
                    objCmd.Parameters.Add(new SqlParameter("@RegDate", objRecord.RegDate));
                    objCmd.Parameters.Add(new SqlParameter("@RegTime", objRecord.RegTime));
                 
                    objCmd.Parameters.Add(new SqlParameter("@TypeCommand", objRecord.TypeCommand));
                    objCmd.Parameters.Add(new SqlParameter("@UserID", objRecord.UserID));
                

                    if (IsNewRecord == true)
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 1));
                    else
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 2));
                    object obj = objCmd.ExecuteScalar();

                    if (obj != null)
                        objRet = Convert.ToString(obj);

                }
            }
            return objRet;

        }
        public static long GetNewID(int FacilityID, int BranchID, int TypeCommand)
        {
            try
            {
                long ID = 0;
                DataTable dt;
                string strSQL;

                strSQL = "SELECT Max(" + PremaryKey + ")+1 FROM " + TableName + " Where  BranchID =" + BranchID + " and TypeCommand=" + TypeCommand;
                dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count > 0)
                {
                    ID = Comon.cLong(dt.Rows[0][0].ToString());
                    if (ID == 0) ID = 1;
                }

                strSQL = "Select Top 1 StartFrom From StartNumbering Where BranchID=" + BranchID
                    + " And FormName='frmSaleInvoice'";
                dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count > 0)
                {
                    if (Comon.cLong(dt.Rows[0]["StartFrom"].ToString()) > ID)
                        ID = (Comon.cLong(dt.Rows[0]["StartFrom"].ToString()));
                }

                return ID;
            }
            catch (Exception ex)
            {
                return 1;
            }
        }
        /// <summary>
        /// this function is used to  Delete  Prentag And Pulishn by Comand ID
        /// </summary>
        /// <param name="objRecord"></param>
        /// <returns></returns>
        public static string Delete(Manu_AuxiliaryMaterialsMaster objRecord)
        {
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Manu_AuxiliaryMaterialsMaster_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@CommandID", objRecord.CommandID));
                    objCmd.Parameters.Add(new SqlParameter("@TypeCommand", objRecord.TypeCommand));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                    objCmd.Parameters.Add(new SqlParameter("@EditDate", objRecord.EditDate));
                    objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 4));
                    object obj = objCmd.ExecuteNonQuery();
                    if (obj != null)
                        return Convert.ToString(obj);
                }
            }
            return "";
        }

        /// <summary>
        /// This function is used to Get Record which is Set By SQL
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns>return id </returns>
        public int GetRecordSetBySQL(string strSQL)
        {
            int ID = 0;
            try
            {
                FoundResult = false;
                dt = Lip.SelectRecord(strSQL);//execute selected
                if (dt.Rows.Count > 0)
                {
                    ID = Comon.cInt(dt.Rows[0][0].ToString());
                    FoundResult = true;
                }
            }
            catch (Exception ex)
            {
                FoundResult = false;
            }
            return ID;
        }

        public static DataTable frmGetDataDetalByID(int ComandID, int BranchID, int FacilityID,int TypeCommand)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Manu_AuxiliaryMaterialsMaster_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@CommandID", ComandID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@TypeCommand", TypeCommand));
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
        public static DataTable frmGetDataDetalByReferance(int refreance, int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Manu_AuxiliaryMaterialsMaster_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@ReferanceID", refreance));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                       
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 6));
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

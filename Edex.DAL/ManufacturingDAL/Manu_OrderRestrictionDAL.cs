using Edex.Model;
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
     
    public class Manu_OrderRestrictionDAL
    {

        #region Declare
        public static readonly string TableName = "Manu_OrderRestriction";
        public static readonly string PremaryKey = "OrderID";
        public bool FoundResult;
        public bool NeedSaving;
        public bool IsNewRecord;
        private DataTable dt;
        private string strSQL;
        private object Result;
        #endregion

        /// <summary>
        /// This Function is used to Convert DataRow to object  Manu_OrderRestriction
        /// </summary>
        /// <param name="dr"></param>
        /// <returns> return object Stc_ItemUnits </returns>
        public static Manu_OrderRestriction ConvertRowToObj(DataRow dr)
        {
            Manu_OrderRestriction Manu_OrderRestrictionObj = new Manu_OrderRestriction();

            Manu_OrderRestrictionObj.OrderID =  dr["OrderID"].ToString();
            Manu_OrderRestrictionObj.OrderDate = Comon.ConvertSerialToDate(dr["OrderDate"].ToString()).ToString() ;
            Manu_OrderRestrictionObj.CustomerID = Comon.cDbl(dr["CustomerID"].ToString());
            Manu_OrderRestrictionObj.DelegateID = Comon.cInt(dr["DelegetID"].ToString());

            Manu_OrderRestrictionObj.TypeOrdersID = Comon.cInt(dr["TypeOrdersID"].ToString());
            Manu_OrderRestrictionObj.UserID = Comon.cInt(dr["UserID"].ToString());
            Manu_OrderRestrictionObj.RegTime = Comon.cDbl(dr["RegTime"].ToString());
            Manu_OrderRestrictionObj.BranchID = Comon.cInt(dr["BranchID"].ToString());
            Manu_OrderRestrictionObj.FacilityID = Comon.cInt(dr["FacilityID"].ToString());
            Manu_OrderRestrictionObj.Cancel = Comon.cInt(dr["Cancel"].ToString());
            Manu_OrderRestrictionObj.RegDate = Comon.cDbl(dr["RegDate"].ToString());
            Manu_OrderRestrictionObj.Notes = dr["Notes"].ToString();
            Manu_OrderRestrictionObj.GuidanceID = Comon.cDbl(dr["GuidanceID"].ToString());
            Manu_OrderRestrictionObj.EditUserID = Comon.cInt(dr["EditUserID"].ToString());
            Manu_OrderRestrictionObj.EditTime = Comon.cDbl(dr["EditTime"].ToString());
            Manu_OrderRestrictionObj.EditDate = Comon.cDbl(dr["EditDate"].ToString());
            Manu_OrderRestrictionObj.EditComputerInfo = dr["EditComputerInfo"].ToString(); 
            Manu_OrderRestrictionObj.ComputerInfo = (dr["ComputerInfo"].ToString());
          


            return Manu_OrderRestrictionObj;
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
                    objCmd.CommandText = "[Manu_OrderRestriction_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@OrderID", ComandID));
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
        public static string InsertUsingXML(Manu_OrderRestriction objRecord, Boolean IsNewRecord)
        {
            string objRet = "0";

            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                   
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Manu_OrderRestriction_SP]"; 
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@GuidanceID", objRecord.GuidanceID));
                    objCmd.Parameters.Add(new SqlParameter("@Cancel", 0));
                    objCmd.Parameters.Add(new SqlParameter("@OrderDate", objRecord.OrderDate)); 
                    objCmd.Parameters.Add(new SqlParameter("@OrderID", objRecord.OrderID));
                    objCmd.Parameters.Add(new SqlParameter("@ImageCode", objRecord.ImageCode)); 
                    objCmd.Parameters.Add(new SqlParameter("@ComputerInfo", objRecord.ComputerInfo)); 
                    objCmd.Parameters.Add(new SqlParameter("@CustomerID", objRecord.CustomerID));
                    objCmd.Parameters.Add(new SqlParameter("@DelegateID", objRecord.DelegateID));
                    objCmd.Parameters.Add(new SqlParameter("@EditComputerInfo", objRecord.EditComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@EditDate", objRecord.EditDate));
                    objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID)); 
                    objCmd.Parameters.Add(new SqlParameter("@Notes", objRecord.Notes)); 
                    objCmd.Parameters.Add(new SqlParameter("@RegDate", objRecord.RegDate));
                    objCmd.Parameters.Add(new SqlParameter("@RegTime", objRecord.RegTime));
                    objCmd.Parameters.Add(new SqlParameter("@TypeAuxiliaryMatirialID", objRecord.TypeAuxiliaryMatirialID));
                    objCmd.Parameters.Add(new SqlParameter("@TypeID", objRecord.TypeID));
                    objCmd.Parameters.Add(new SqlParameter("@TypeOrdersID", objRecord.TypeOrdersID));
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
        public static long GetNewID(int FacilityID, int BranchID)
        {
            try
            {
                long ID = 0;
                DataTable dt;
                string strSQL;

                strSQL = "SELECT Max(" + PremaryKey + ")+1 FROM " + TableName + " Where  BranchID =" + BranchID  ;
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
        public static string Delete(Manu_OrderRestriction objRecord)
        {
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Manu_OrderRestriction_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@OrderID", objRecord.OrderID)); 
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
        public string GetRecordSetBySQL(string strSQL)
        {
            string ID = "";
            try
            {
                FoundResult = false;
                dt = Lip.SelectRecord(strSQL);//execute selected
                if (dt.Rows.Count > 0)
                {
                    ID =  dt.Rows[0][0].ToString();
                    FoundResult = true;
                }
            }
            catch (Exception ex)
            {
                FoundResult = false;
            }
            return ID;
        }

        public static DataTable frmGetDataDetalByID(string OrderID, int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Manu_OrderRestriction_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@OrderID", OrderID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
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

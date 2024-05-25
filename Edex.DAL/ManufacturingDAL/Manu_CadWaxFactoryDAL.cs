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

    public class Manu_CadWaxFactoryDAL
    {

        #region Declare
        public static readonly string TableName = "Manu_CadWaxFactoryMaster";
        public static readonly string PremaryKey = "CommandID";
        public bool FoundResult;
        public bool NeedSaving;
        public bool IsNewRecord;
        private DataTable dt;
        private string strSQL;
        private object Result;
        #endregion

        /// <summary>
        /// This Function is used to Convert DataRow to object  Manu_CadWaxFactoryMaster
        /// </summary>
        /// <param name="dr"></param>
        /// <returns> return object Stc_ItemUnits </returns>
        public static Manu_CadWaxFactoryMaster ConvertRowToObj(DataRow dr)
        {
            Manu_CadWaxFactoryMaster Manu_CadWaxFactory = new Manu_CadWaxFactoryMaster();

            Manu_CadWaxFactory.CommandID = Comon.cInt(dr["CommandID"].ToString());
            Manu_CadWaxFactory.OrderID =  dr["OrderID"].ToString();
            Manu_CadWaxFactory.DateAfter = Comon.ConvertDateToSerial(dr["DateAfter"].ToString());
            Manu_CadWaxFactory.DateBefore = Comon.cDbl(dr["DateBefore"].ToString());
            Manu_CadWaxFactory.StoreMangerAfter =  dr["StoreMangerAfter"].ToString();
            Manu_CadWaxFactory.StoreMangerBefore =  dr["StoreMangerBefore"].ToString();

            
            Manu_CadWaxFactory.UserID = Comon.cInt(dr["UserID"].ToString());
            Manu_CadWaxFactory.RegTime = Comon.cDbl(dr["RegTime"].ToString());
            Manu_CadWaxFactory.BranchID = Comon.cInt(dr["BranchID"].ToString());
            Manu_CadWaxFactory.FacilityID = Comon.cInt(dr["FacilityID"].ToString());
            Manu_CadWaxFactory.Cancel = Comon.cInt(dr["Cancel"].ToString());
            Manu_CadWaxFactory.RegDate = Comon.cDbl(dr["RegDate"].ToString());
            Manu_CadWaxFactory.Notes = dr["Notes"].ToString();
             Manu_CadWaxFactory.EditUserID = Comon.cInt(dr["EditUserID"].ToString());
            Manu_CadWaxFactory.EditTime = Comon.cDbl(dr["EditTime"].ToString());
            Manu_CadWaxFactory.EditDate = Comon.cDbl(dr["EditDate"].ToString());
            Manu_CadWaxFactory.EditComputerInfo = dr["EditComputerInfo"].ToString();
            Manu_CadWaxFactory.CurrencyID = Comon.cInt(dr["CurrencyID"].ToString());
            Manu_CadWaxFactory.ComputerInfo = (dr["ComputerInfo"].ToString());
            Manu_CadWaxFactory.StoreIDAfter = Comon.cDbl(dr["StoreIDAfter"].ToString());
            Manu_CadWaxFactory.StoreIDBefore = Comon.cDbl(dr["StoreIDBefore"].ToString());
            Manu_CadWaxFactory.PeriodDay = Comon.cInt(dr["PeriodDay"].ToString());
            Manu_CadWaxFactory.Posted = Comon.cInt(dr["Posted"].ToString());

            return Manu_CadWaxFactory;
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
                    objCmd.CommandText = "[Manu_CadWaxFactoryMaster_SP]";
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
        public static string InsertUsingXML(Manu_CadWaxFactoryMaster objRecord, Boolean IsNewRecord)
        {
            string objRet = "0";

            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    string DitmeXML = ConvertObjectToXMLString(objRecord.Manu_CadWaxFactorys);
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Manu_CadWaxFactoryMaster_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@XmlDataAlcad", SqlDbType.Xml)).Value = DitmeXML;
                    string DitmeDetilsXML = ConvertObjectToXMLString(objRecord.Manu_OrderDetils);
                    objCmd.Parameters.Add(new SqlParameter("@XmlDataOrders", SqlDbType.Xml)).Value = DitmeDetilsXML;
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@StoreIDBefore", objRecord.StoreIDBefore));
                    objCmd.Parameters.Add(new SqlParameter("@OrderID", objRecord.OrderID));
                    objCmd.Parameters.Add(new SqlParameter("@StoreIDAfter", objRecord.StoreIDAfter));
                    objCmd.Parameters.Add(new SqlParameter("@StoreMangerAfter", objRecord.StoreMangerAfter));
                    objCmd.Parameters.Add(new SqlParameter("@TypeStageID", objRecord.TypeStageID));
                    objCmd.Parameters.Add(new SqlParameter("@StoreMangerBefore", objRecord.StoreMangerBefore));
                    objCmd.Parameters.Add(new SqlParameter("@Cancel", 0));
                    objCmd.Parameters.Add(new SqlParameter("@DateBefore", objRecord.DateBefore));
                    objCmd.Parameters.Add(new SqlParameter("@DateAfter", objRecord.DateAfter));
                    objCmd.Parameters.Add(new SqlParameter("@FactorID", objRecord.FactorID));
                    objCmd.Parameters.Add(new SqlParameter("@CostCenterID", objRecord.CostCenterID));
                    objCmd.Parameters.Add(new SqlParameter("@CommandID", objRecord.CommandID));
                    objCmd.Parameters.Add(new SqlParameter("@ComputerInfo", objRecord.ComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@CurrencyID", objRecord.CurrencyID)); 
                    objCmd.Parameters.Add(new SqlParameter("@EditComputerInfo", objRecord.EditComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@EditDate", objRecord.EditDate));
                    objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                    objCmd.Parameters.Add(new SqlParameter("@Posted", objRecord.Posted));
                    objCmd.Parameters.Add(new SqlParameter("@Notes", objRecord.Notes));
                 
                    objCmd.Parameters.Add(new SqlParameter("@RegDate", objRecord.RegDate));
                    objCmd.Parameters.Add(new SqlParameter("@RegTime", objRecord.RegTime));
                    objCmd.Parameters.Add(new SqlParameter("@PeriodDay", objRecord.PeriodDay));
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
        public static long GetNewID(int FacilityID, int BranchID,int  TypeStageID)
        {
            try
            {
                long ID = 0;
                DataTable dt;
                string strSQL;

                strSQL = "SELECT Max(" + PremaryKey + ")+1 FROM " + TableName + " Where  BranchID =" + BranchID + " and TypeStageID=" + TypeStageID;
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
        public static string Delete(Manu_CadWaxFactoryMaster objRecord)
        {
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Manu_CadWaxFactoryMaster_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@CommandID", objRecord.CommandID)); 
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                    objCmd.Parameters.Add(new SqlParameter("@EditDate", objRecord.EditDate));
                    objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));
                    objCmd.Parameters.Add(new SqlParameter("@TypeStageID", objRecord.TypeStageID));
                    objCmd.Parameters.Add(new SqlParameter("@OrderID", objRecord.OrderID));
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

        public static DataTable frmGetDataDetalByID(int ComandID, int BranchID, int FacilityID, int TypeStageID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Manu_CadWaxFactoryMaster_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@CommandID", ComandID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
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
                        objCmd.CommandText = "[Manu_CadWaxFactoryMaster_SP]";
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


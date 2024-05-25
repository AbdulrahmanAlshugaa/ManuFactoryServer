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
  public  class Menu_FactoryRunCommandCompundDAL
    {

        #region Declare
        public static readonly string TableName = "Menu_FactoryRunCommandCompund";
        public static readonly string PremaryKey = "BarcodCompond";
        public bool FoundResult;
        public bool NeedSaving;
        public bool IsNewRecord;
        private DataTable dt;
        private string strSQL;
        private object Result;

        #endregion

        /// <summary>
        /// This Function is used to Convert DataRow to object  Menu_FactoryRunCommandCompund
        /// </summary>
        /// <param name="dr"></param>
        /// <returns> return object Stc_ItemUnits </returns>
        public static Menu_FactoryRunCommandCompund ConvertRowToObj(DataRow dr)
        {
            Menu_FactoryRunCommandCompund Menu_F_CommandCompund = new Menu_FactoryRunCommandCompund();

            Menu_F_CommandCompund.ID = Comon.cInt(dr["ID"].ToString());
            Menu_F_CommandCompund.InvoiceImage = Comon.cbyte(dr["InvoiceImage"].ToString());
         
            Menu_F_CommandCompund.BarcodCompond = dr["BarcodCompond"].ToString();
            Menu_F_CommandCompund.BranchID = Comon.cInt(dr["BranchID"].ToString());

            Menu_F_CommandCompund.ComandID = Comon.cInt(dr["ComandID"].ToString());
            Menu_F_CommandCompund.Cancel = Comon.cInt(dr["Cancel"].ToString());

            Menu_F_CommandCompund.ComStoneCom = Comon.cInt(dr["ComStoneCom"].ToString());

            Menu_F_CommandCompund.ComStoneNumin = Comon.cDbl(dr["ComStoneNumin"].ToString());
            Menu_F_CommandCompund.ComStoneNumlas = Comon.cInt(dr["ComStoneNumlas"].ToString());
            Menu_F_CommandCompund.ComStoneNumout = Comon.cDbl(dr["ComStoneNumout"].ToString());

            Menu_F_CommandCompund.ComWeightSton = Comon.cDec(dr["ComWeightSton"].ToString());

            Menu_F_CommandCompund.ComWeightStonin = Comon.cDbl(dr["ComWeightStonin"].ToString());
            Menu_F_CommandCompund.ComWeightStonLas = Comon.cDbl(dr["ComWeightStonLas"].ToString());
            Menu_F_CommandCompund.ComWeightStonOUt = Comon.cDbl(dr["ComWeightStonOUt"].ToString());
            Menu_F_CommandCompund.EmpCompondID = Comon.cInt(dr["EmpCompondID"].ToString());
            Menu_F_CommandCompund.FacilityID = Comon.cInt(dr["FacilityID"].ToString());
            Menu_F_CommandCompund.FromAccountID = Comon.cInt(dr["FromAccountID"].ToString());
            Menu_F_CommandCompund.GoldCompundNet = Comon.cInt(dr["GoldCompundNet"].ToString());
            Menu_F_CommandCompund.GoldCredit = Comon.cInt(dr["GoldCredit"].ToString());
            Menu_F_CommandCompund.GoldDebit = Comon.cInt(dr["GoldDebit"].ToString());

            return Menu_F_CommandCompund;
        }


        /// <summary>
        /// this function is used Insert Using XML
        /// </summary>
        /// <param name="objRecord"></param>
        /// <param name="IsNewRecord"></param>
        /// <returns></returns>
        public static string InsertUsingXML(Menu_FactoryRunCommandCompund objRecord, Boolean IsNewRecord)
        {
            string objRet = "0";

            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    string DitmeXML = ConvertObjectToXMLString(objRecord.Menu_F_PrentagAndPulishn);
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Menu_FactoryRunCommandCompund_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@xmlData", SqlDbType.Xml)).Value = DitmeXML;
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
        /// this function is used to  Delete  Prentag And Pulishn by Comand ID
        /// </summary>
        /// <param name="objRecord"></param>
        /// <returns></returns>
        public static string Delete(Menu_FactoryRunCommandCompund objRecord)
        {
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Menu_FactoryRunCommandCompund_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@ComandID", objRecord.ComandID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                     
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
        public long GetRecordSetBySQL(string strSQL)
        {
            long ID = 0;
            try
            {
                FoundResult = false;
                dt = Lip.SelectRecord(strSQL);//execute selected
                if (dt.Rows.Count > 0)
                {
                    ID = Comon.cLong(dt.Rows[0][0].ToString());
                    FoundResult = true;
                }
            }
            catch (Exception ex)
            {
                FoundResult = false;
            }
            return ID;
        }

        public static DataTable frmGetDataDetalByID(float ComandID, int BranchID, int FacilityID, int TypeOpration)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Menu_FactoryRunCommandMaster_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@ComandID", ComandID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@TypeOpration", TypeOpration));
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

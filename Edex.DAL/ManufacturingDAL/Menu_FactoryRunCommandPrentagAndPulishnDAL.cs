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
   public class Menu_FactoryRunCommandPrentagAndPulishnDAL
    {
        #region Declare
        public static readonly string TableName = "Menu_FactoryRunCommandPrentagAndPulishn";
        public static readonly string PremaryKey = "ID";
        public bool FoundResult;
        public bool NeedSaving;
        public bool IsNewRecord;
        private DataTable dt;
        private string strSQL;
        private object Result;
        #endregion

        /// <summary>
        /// This Function is used to Convert DataRow to object  Menu_FactoryRunCommandPrentagAndPulishn
        /// </summary>
        /// <param name="dr"></param>
        /// <returns> return object Stc_ItemUnits </returns>
        public static Menu_FactoryRunCommandPrentagAndPulishn ConvertRowToObj(DataRow dr)
        {
            Menu_FactoryRunCommandPrentagAndPulishn Menu_F_PrentagAndPulishn = new Menu_FactoryRunCommandPrentagAndPulishn();
            Menu_F_PrentagAndPulishn.ID = Comon.cInt(dr["ID"].ToString());
            Menu_F_PrentagAndPulishn.ComandID = Comon.cInt(dr["ComandID"].ToString());
            Menu_F_PrentagAndPulishn.MachinID =Comon.cInt (dr["MachinID"].ToString());
            Menu_F_PrentagAndPulishn.PrentagDebit =Comon.cInt ( dr["PrentagDebit"].ToString());
            Menu_F_PrentagAndPulishn.PrentagCredit = Comon.cInt(dr["PrentagCredit"].ToString());
            Menu_F_PrentagAndPulishn.EmpPolishnID = Comon.cInt(dr["EmpPolishnID"].ToString());
            Menu_F_PrentagAndPulishn.EmpPrentagID = Comon.cInt(dr["EmpPrentagID"].ToString());
            Menu_F_PrentagAndPulishn.BarcodePrentag = dr["BarcodePrentag"].ToString();
            Menu_F_PrentagAndPulishn.TypeOpration = Comon.cInt(dr["TypeOpration"].ToString());
            Menu_F_PrentagAndPulishn.PrSignature = dr["PrSignature"].ToString();
            Menu_F_PrentagAndPulishn.BranchID = Comon.cInt(dr["BranchID"].ToString());
            Menu_F_PrentagAndPulishn.FacilityID =Comon.cInt(dr["FacilityID"].ToString());
            Menu_F_PrentagAndPulishn.Cancel = Comon.cInt(dr["Cancel"].ToString());

            return Menu_F_PrentagAndPulishn;
        }
        /// <summary>
        /// This function is used to get PrntageAndPulishn
        /// </summary>
        /// <param name="ComandID"></param>
        /// <param name="BranchID"></param>
        /// <param name="FacilityID"></param>
        /// <returns>return object DataTable </returns>
        public static DataTable GetPrntageAndPulishn(int ComandID, int BranchID, int FacilityID)
        {
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Menu_FactoryRunCommandPrentagAndPulishn_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@ComandID", ComandID));
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
        public static string InsertUsingXML(Menu_FactoryRunCommandPrentagAndPulishn objRecord, Boolean IsNewRecord)
        {
            string objRet = "0";

            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    string DitmeXML = ConvertObjectToXMLString(objRecord.Menu_F_PrentagAndPulishn);
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Menu_FactoryRunCommandPrentagAndPulishn_SP]";
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
        /// this function is used to  Delete  Prentag And Pulishn by Comand ID
        /// </summary>
        /// <param name="objRecord"></param>
        /// <returns></returns>
        public static string Delete(Menu_FactoryRunCommandPrentagAndPulishn objRecord)
        {
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Menu_FactoryRunCommandPrentagAndPulishn_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@ComandID", objRecord.ComandID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                    objCmd.Parameters.Add(new SqlParameter("@editdate", objRecord.EditDate));
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
                        objCmd.CommandText = "[Menu_FactoryRunCommandPrentagAndPulishn_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@ComandID", ComandID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@TypeOpration", TypeOpration));
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
        public static DataTable frmGetDataDetalByIDPrntageTypeID(float ComandID, int BranchID, int FacilityID, int TypeOpration, int PrntageTypeID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Menu_FactoryRunCommandPrentagAndPulishn_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@ComandID", ComandID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@TypeOpration", TypeOpration));
                        objCmd.Parameters.Add(new SqlParameter("@PrntageTypeID", PrntageTypeID));
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

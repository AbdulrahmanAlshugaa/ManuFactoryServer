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
    

    public class Mnu_OrderRunningReportDAL
    {

        #region Declare
        public static readonly string TableName = "Manu_AfforestationFactoryMaster";
        public static readonly string PremaryKey = "CommandID";
        public bool FoundResult;
        public bool NeedSaving;
        public bool IsNewRecord;
        private DataTable dt;
        private string strSQL;
        private object Result;
        #endregion

        /// <summary>
        /// This Function is used to Convert DataRow to object  Manu_AfforestationFactoryMaster
        /// </summary>
        /// <param name="dr"></param>
        /// <returns> return object Stc_ItemUnits </returns>
        public static Manu_AfforestationFactoryMaster ConvertRowToObj(DataRow dr)
        {
            Manu_AfforestationFactoryMaster Manu_CadWaxFactory = new Manu_AfforestationFactoryMaster();

            Manu_CadWaxFactory.CommandID = Comon.cInt(dr["CommandID"].ToString());
            Manu_CadWaxFactory.OrderID = dr["OrderID"].ToString();
            Manu_CadWaxFactory.DateAfter = Comon.ConvertDateToSerial(dr["DateAfter"].ToString());
            Manu_CadWaxFactory.DateBefore = Comon.cDbl(dr["DateBefore"].ToString());
            Manu_CadWaxFactory.AfterFactorID = Comon.cDbl(dr["AfterFactorID"].ToString());
            Manu_CadWaxFactory.StoreMangerBefore = dr["StoreMangerBefore"].ToString();


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
            Manu_CadWaxFactory.AfterAccountID = Comon.cDbl(dr["StoreIDAfter"].ToString());
            Manu_CadWaxFactory.StoreIDBefore = Comon.cDbl(dr["StoreIDBefore"].ToString());
            Manu_CadWaxFactory.PeriodDay = Comon.cInt(dr["PeriodDay"].ToString());
            Manu_CadWaxFactory.Posted = Comon.cInt(dr["Status"].ToString());
            Manu_CadWaxFactory.EquQty = Comon.cDec(dr["EquQty"].ToString());
            Manu_CadWaxFactory.GoldQTYCloves = Comon.cDec(dr["GoldQTYCloves"].ToString());

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
                    objCmd.CommandText = "[Manu_AfforestationFactoryMaster_SP]";
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

    
        public static DataTable frmGetDataDetailByOrderID(string OrderID, int BranchID, int FacilityID, int CMDTYPE, int TypeStageID=0)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Mnu_OrderRunningReport_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@OrderID", OrderID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));

                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", CMDTYPE));
                        objCmd.Parameters.Add(new SqlParameter("@TypeStageID", TypeStageID));
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

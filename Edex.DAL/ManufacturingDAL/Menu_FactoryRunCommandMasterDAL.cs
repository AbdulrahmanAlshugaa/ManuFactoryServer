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
 public   class Menu_FactoryRunCommandMasterDAL
    {

        #region Declare
        public static readonly string TableName = "Menu_FactoryRunCommandMaster";
        public static readonly string PremaryKey = "ComandID";
        public bool FoundResult;
        public bool NeedSaving;
        public bool IsNewRecord;
        private DataTable dt;
        private string strSQL;
        private object Result;
        #endregion

        /// <summary>
        /// This Function is used to Convert DataRow to object  Menu_FactoryRunCommandMaster
        /// </summary>
        /// <param name="dr"></param>
        /// <returns> return object Stc_ItemUnits </returns>
        public static Menu_FactoryRunCommandMaster ConvertRowToObj(DataRow dr)
        {
            Menu_FactoryRunCommandMaster Menu_F_CommandMaster = new Menu_FactoryRunCommandMaster();

            Menu_F_CommandMaster.Barcode = dr["Barcode"].ToString();
            Menu_F_CommandMaster.DocumentID = Comon.cInt(dr["DocumentID"].ToString());

            Menu_F_CommandMaster.ComandID = Comon.cInt(dr["ComandID"].ToString());
            Menu_F_CommandMaster.ComandDate = Comon.ConvertDateToSerial(dr["ComandDate"].ToString());         
             
            Menu_F_CommandMaster.BranchID = Comon.cInt(dr["BranchID"].ToString());

            Menu_F_CommandMaster.EmployeeID = Comon.cInt(dr["EmployeeID"].ToString());
            Menu_F_CommandMaster.CustomerID = Comon.cInt(dr["CustomerID"].ToString());
            Menu_F_CommandMaster.piece = Comon.cDbl(dr["piece"].ToString());
            
            Menu_F_CommandMaster.EmpFactorID = Comon.cDbl(dr["EmpFactorID"].ToString());

            Menu_F_CommandMaster.Goldweight1 = Comon.cDbl(dr["Goldweight1"].ToString());
            Menu_F_CommandMaster.GivenDate = Comon.cDbl(dr["GivenDate"].ToString());
            Menu_F_CommandMaster.ReciveDate = Comon.cDbl(dr["ReciveDate"].ToString());
            Menu_F_CommandMaster.GivenTime = Comon.cDbl(dr["GivenTime"].ToString());
            Menu_F_CommandMaster.TimeRecive = Comon.cDbl(dr["TimeRecive"].ToString());
            Menu_F_CommandMaster.EmployeeStokID = Comon.cDbl(dr["EmployeeStokID"].ToString());
            Menu_F_CommandMaster.GoldWeight = Comon.cDbl(dr["GoldWeight"].ToString());
            Menu_F_CommandMaster.WeightDaimond = Comon.cDbl(dr["WeightDaimond"].ToString());
            Menu_F_CommandMaster.WeightBaguettes = Comon.cDbl(dr["WeightBaguettes"].ToString());
            Menu_F_CommandMaster.StoneWeight = Comon.cDbl(dr["StoneWeight"].ToString());
            Menu_F_CommandMaster.netGoldWeight = Comon.cDbl(dr["netGoldWeight"].ToString());
            Menu_F_CommandMaster.TotalLost = Comon.cDbl(dr["TotalLost"].ToString());
            Menu_F_CommandMaster.TotalEquvelentGold = Comon.cDbl(dr["TotalEquvelentGold"].ToString());
            Menu_F_CommandMaster.TotalDaimondSecound = Comon.cDbl(dr["TotalDaimondSecound"].ToString());
            Menu_F_CommandMaster.SigntureDate = Comon.cDbl(dr["SigntureDate"].ToString());
            Menu_F_CommandMaster.SigntureTime = Comon.cDbl(dr["SigntureTime"].ToString());
            Menu_F_CommandMaster.Notes = dr["Notes"].ToString();
            Menu_F_CommandMaster.EmployeeIDCompuningID = Comon.cDbl(dr["EmployeeIDCompuningID"].ToString());
            Menu_F_CommandMaster.Cancel = Comon.cInt(dr["Cancel"].ToString());
            // Menu_F_CommandMaster.InvoiceImage= 
            // Menu_F_CommandMaster.InvoiceImage2= 
            Menu_F_CommandMaster.ComandStatues=Comon.cInt(dr["ComandStatues"].ToString());
            Menu_F_CommandMaster.DebditGoldAccountID=Comon.cDbl(dr["DebditGoldAccountID"].ToString());
            Menu_F_CommandMaster.ItemID=Comon.cInt(dr["ItemID"].ToString());
            Menu_F_CommandMaster.GroupID=Comon.cInt(dr["GroupID"].ToString());
            Menu_F_CommandMaster.BrandID=Comon.cInt(dr["BrandID"].ToString());
            Menu_F_CommandMaster.TypeID=Comon.cInt(dr["TypeID"].ToString());
            Menu_F_CommandMaster.fromAccountID=Comon.cDbl(dr["fromAccountID"].ToString());
             
            Menu_F_CommandMaster.SpendAmount=Comon.cDbl(dr["SpendAmount"].ToString());
            Menu_F_CommandMaster.EmpPrentagID=Comon.cDbl(dr["EmpPrentagID"].ToString());
            Menu_F_CommandMaster.EmpPolishnID=Comon.cDbl(dr["EmpPolishnID"].ToString());
            Menu_F_CommandMaster.GoldDebit=Comon.cDbl(dr["GoldDebit"].ToString());
           
             Menu_F_CommandMaster.GoldCredit=Comon.cDbl(dr["GoldCredit"].ToString());
            Menu_F_CommandMaster.GoldCompundNet=Comon.cDbl(dr["GoldCompundNet"].ToString());
            
             Menu_F_CommandMaster.ThefactoriID=Comon.cInt(dr["ThefactoriID"].ToString());
            Menu_F_CommandMaster.OpretionID=Comon.cInt(dr["OpretionID"].ToString());
            Menu_F_CommandMaster.FacilityID=Comon.cInt(dr["FacilityID"].ToString());
       
            return Menu_F_CommandMaster;
        }

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
        public static string InsertUsingXML(Menu_FactoryRunCommandMaster objRecord, Boolean IsNewRecord)
        {
            string objRet = "0";

            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    if (objRecord.Manu_OrderDetils != null)
                        if (objRecord.Manu_OrderDetils.Count > 0)
                        {
                            string DitmeXML = ConvertObjectToXMLString(objRecord.Manu_OrderDetils);
                            objCmd.Parameters.Add(new SqlParameter("@XmlDataOrders", SqlDbType.Xml)).Value = DitmeXML;
                        }

                    //تفكيك 
                    if (objRecord.Menu_F_Dismant != null)
                    if (objRecord.Menu_F_Dismant.Count > 0)
                    {
                        string DitmeXML = ConvertObjectToXMLString(objRecord.Menu_F_Dismant);
                        objCmd.Parameters.Add(new SqlParameter("@XmlDataDismant", SqlDbType.Xml)).Value = DitmeXML;
                    }
                    if (objRecord.Menu_F_Prentag != null)
                    if (objRecord.Menu_F_Prentag.Count > 0)
                    {
                        string DitmeXML = ConvertObjectToXMLString(objRecord.Menu_F_Prentag);
                        objCmd.Parameters.Add(new SqlParameter("@XmlDataPrentag", SqlDbType.Xml)).Value = DitmeXML;
                    }
                    if (objRecord.Menu_F_Compund!=null)
                    //تركيب
                    if (objRecord.Menu_F_Compund.Count > 0)
                    {
                        string DitmeXMLCompund = ConvertObjectToXMLString(objRecord.Menu_F_Compund);
                        objCmd.Parameters.Add(new SqlParameter("@XmlDataCompund", SqlDbType.Xml)).Value = DitmeXMLCompund;
                    }
                    if (objRecord.Menu_F_Talmee!=null)
                     //تلميع
                    if(objRecord.Menu_F_Talmee.Count>0)
                    {
                        string DitmeXMLPolushin = ConvertObjectToXMLString(objRecord.Menu_F_Talmee);
                        objCmd.Parameters.Add(new SqlParameter("@XmlDataTalmee", SqlDbType.Xml)).Value = DitmeXMLPolushin;
                    }
                    if (objRecord.Menu_F_Selver!=null)
                    //الاضافات
                    if (objRecord.Menu_F_Selver.Count > 0)
                    {
                        string DitmeXMLSelver = ConvertObjectToXMLString(objRecord.Menu_F_Selver);
                        objCmd.Parameters.Add(new SqlParameter("@XmlDataSelver", SqlDbType.Xml)).Value = DitmeXMLSelver;
                    }

                    if (objRecord.Menu_F_Factory != null)
                    //وزن الذهب
                        if (objRecord.Menu_F_Factory.Count > 0)
                    {
                        string DitmeXMLFactory = ConvertObjectToXMLString(objRecord.Menu_F_Factory);
                        objCmd.Parameters.Add(new SqlParameter("@XmlDataFactory", SqlDbType.Xml)).Value = DitmeXMLFactory;
                    }
                    if (objRecord.Menu_F_ProductionExpenses != null)
                        //وزن الذهب
                        if (objRecord.Menu_F_ProductionExpenses.Count > 0)
                        {
                            string DitmeXMLProductionExpenses = ConvertObjectToXMLString(objRecord.Menu_F_ProductionExpenses);
                            objCmd.Parameters.Add(new SqlParameter("@XmlDataProductionExpenses", SqlDbType.Xml)).Value = DitmeXMLProductionExpenses;
                        }

                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Menu_FactoryRunCommandMaster_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@Barcode", objRecord.Barcode));
                    objCmd.Parameters.Add(new SqlParameter("@CostCenterID", objRecord.CostCenterID));
                    objCmd.Parameters.Add(new SqlParameter("@TypeStageID", objRecord.TypeStageID));
                    objCmd.Parameters.Add(new SqlParameter("@Posted", objRecord.Posted));
                    objCmd.Parameters.Add(new SqlParameter("@TypeStageBeforeID", objRecord.TypeStageBeforeID));
                    objCmd.Parameters.Add(new SqlParameter("@PrntageTypeID", objRecord.PrntageTypeID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@BrandID", objRecord.BrandID));
                    objCmd.Parameters.Add(new SqlParameter("@PollutionTypeID", objRecord.PollutionTypeID));
                    objCmd.Parameters.Add(new SqlParameter("@Cancel", 0));
                    objCmd.Parameters.Add(new SqlParameter("@ComandDate", objRecord.ComandDate));
                    objCmd.Parameters.Add(new SqlParameter("@ComandID", objRecord.ComandID));
                    objCmd.Parameters.Add(new SqlParameter("@ComandStatues", objRecord.ComandStatues));
                    objCmd.Parameters.Add(new SqlParameter("@CustomerID", objRecord.CustomerID));
                    objCmd.Parameters.Add(new SqlParameter("@DebditGoldAccountID", objRecord.DebditGoldAccountID));
                    objCmd.Parameters.Add(new SqlParameter("@DocumentID", objRecord.DocumentID));
                    objCmd.Parameters.Add(new SqlParameter("@EmpFactorID", objRecord.EmpFactorID));
                    objCmd.Parameters.Add(new SqlParameter("@EmployeeID", objRecord.EmployeeID));
                    objCmd.Parameters.Add(new SqlParameter("@DelegateID", objRecord.DelegateID));
                    objCmd.Parameters.Add(new SqlParameter("@EmployeeIDCompuningID", objRecord.EmployeeIDCompuningID));
                    objCmd.Parameters.Add(new SqlParameter("@CurrencyID", objRecord.CurrencyID));
                       

                    objCmd.Parameters.Add(new SqlParameter("@EmployeeStokID", objRecord.EmployeeStokID));
                    objCmd.Parameters.Add(new SqlParameter("@EmpPolishnID", objRecord.EmpPolishnID));
                    objCmd.Parameters.Add(new SqlParameter("@EmpPrentagID", objRecord.EmpPrentagID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@fromAccountID", objRecord.fromAccountID));
                    objCmd.Parameters.Add(new SqlParameter("@GivenDate", objRecord.GivenDate));
                    objCmd.Parameters.Add(new SqlParameter("@GivenTime", objRecord.GivenTime));
                    objCmd.Parameters.Add(new SqlParameter("@GoldCompundNet", objRecord.GoldCompundNet));
                    objCmd.Parameters.Add(new SqlParameter("@GoldCredit", objRecord.GoldCredit));
                    objCmd.Parameters.Add(new SqlParameter("@GoldDebit", objRecord.GoldDebit));
                    objCmd.Parameters.Add(new SqlParameter("@GoldWeight", objRecord.GoldWeight));
                    objCmd.Parameters.Add(new SqlParameter("@Goldweight1", objRecord.Goldweight1));
                    objCmd.Parameters.Add(new SqlParameter("@GroupID", objRecord.GroupID));
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceImage", objRecord.InvoiceImage));
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceImage2", objRecord.InvoiceImage2));
                    objCmd.Parameters.Add(new SqlParameter("@ItemID", objRecord.ItemID));
                    objCmd.Parameters.Add(new SqlParameter("@PeiceName", objRecord.PeiceName));
                    objCmd.Parameters.Add(new SqlParameter("@netGoldWeight", objRecord.netGoldWeight));
                    objCmd.Parameters.Add(new SqlParameter("@Notes", objRecord.Notes));

                    objCmd.Parameters.Add(new SqlParameter("@OpretionID", objRecord.OpretionID));
                    objCmd.Parameters.Add(new SqlParameter("@piece", objRecord.piece));
                    objCmd.Parameters.Add(new SqlParameter("@ReciveDate", objRecord.ReciveDate));
                    objCmd.Parameters.Add(new SqlParameter("@SigntureDate", objRecord.SigntureDate));
                    objCmd.Parameters.Add(new SqlParameter("@SigntureTime", objRecord.SigntureTime));
                    objCmd.Parameters.Add(new SqlParameter("@SpendAmount", objRecord.SpendAmount));

                    objCmd.Parameters.Add(new SqlParameter("@StoneWeight", objRecord.StoneWeight));
                    objCmd.Parameters.Add(new SqlParameter("@ThefactoriID", objRecord.ThefactoriID));
                    objCmd.Parameters.Add(new SqlParameter("@TheType", objRecord.TheType));
                    objCmd.Parameters.Add(new SqlParameter("@TimeRecive", objRecord.TimeRecive));

                    objCmd.Parameters.Add(new SqlParameter("@TotalDaimondSecound", objRecord.TotalDaimondSecound));
                    objCmd.Parameters.Add(new SqlParameter("@TotalEquvelentGold", objRecord.TotalEquvelentGold));
                    objCmd.Parameters.Add(new SqlParameter("@TotalLost", objRecord.TotalLost));
                    objCmd.Parameters.Add(new SqlParameter("@TypeID", objRecord.TypeID));
                    objCmd.Parameters.Add(new SqlParameter("@WeightBaguettes", objRecord.WeightBaguettes));
                    objCmd.Parameters.Add(new SqlParameter("@WeightDaimond", objRecord.WeightDaimond));
                    
                    //الحسابات
                    objCmd.Parameters.Add(new SqlParameter("@AccountIDFactory", objRecord.AccountIDFactory));
                    objCmd.Parameters.Add(new SqlParameter("@StoreIDFactory", objRecord.StoreIDFactory));
                    objCmd.Parameters.Add(new SqlParameter("@EmployeeStokIDFactory", objRecord.EmployeeStokIDFactory));
                    objCmd.Parameters.Add(new SqlParameter("@EmpIDFactor", objRecord.EmpIDFactor));


                    objCmd.Parameters.Add(new SqlParameter("@AccountIDPrentage", objRecord.AccountIDPrentage));
                    objCmd.Parameters.Add(new SqlParameter("@StoreIDPrentage", objRecord.StoreIDPrentage));
                    objCmd.Parameters.Add(new SqlParameter("@EmployeeStokIDPrentage", objRecord.EmployeeStokIDPrentage));
                    objCmd.Parameters.Add(new SqlParameter("@EmpIDPrentage", objRecord.EmpIDPrentage));


                    objCmd.Parameters.Add(new SqlParameter("@AccountIDBeforCompond", objRecord.AccountIDBeforCompond));
                    objCmd.Parameters.Add(new SqlParameter("@StoreIDBeforComond", objRecord.StoreIDBeforComond));
                    objCmd.Parameters.Add(new SqlParameter("@EmployeeStokIDBeforCompond", objRecord.EmployeeStokIDBeforCompond));
                    objCmd.Parameters.Add(new SqlParameter("@EmpIDBeforCompond", objRecord.EmpIDBeforCompond));
                     



                    objCmd.Parameters.Add(new SqlParameter("@AccountIDAfterCompond", objRecord.AccountIDAfterCompond));
                    objCmd.Parameters.Add(new SqlParameter("@EmployeeStokIDAfterCompond", objRecord.EmployeeStokIDAfterCompond));
                    objCmd.Parameters.Add(new SqlParameter("@StoreIDAfterComond", objRecord.StoreIDAfterComond));
                    objCmd.Parameters.Add(new SqlParameter("@EmpIDAfterCompond", objRecord.EmpIDAfterCompond));
                    
                    objCmd.Parameters.Add(new SqlParameter("@AccountIDAdditions", objRecord.AccountIDAdditions));
                    objCmd.Parameters.Add(new SqlParameter("@StoreIDAdditions", objRecord.StoreIDAdditions));
                    objCmd.Parameters.Add(new SqlParameter("@EmployeeStokIDAdditions", objRecord.EmployeeStokIDAdditions));
                    objCmd.Parameters.Add(new SqlParameter("@EmpIDAdditions", objRecord.EmpIDAdditions));
                    

                    objCmd.Parameters.Add(new SqlParameter("@AccountIDPolishing", objRecord.AccountIDPolishing));
                    objCmd.Parameters.Add(new SqlParameter("@StoreIDPolishing", objRecord.StoreIDPolishing));
                    objCmd.Parameters.Add(new SqlParameter("@EmployeeStokIDPolishing", objRecord.EmployeeStokIDPolishing));
                    objCmd.Parameters.Add(new SqlParameter("@EmplooyIDPolishing", objRecord.EmplooyIDPolishing));
                    
                    objCmd.Parameters.Add(new SqlParameter("@AccountIDBarcodeItem", objRecord.AccountIDBarcodeItem));
                    objCmd.Parameters.Add(new SqlParameter("@StoreIDBarcod", objRecord.StoreIDBarcod));
                    objCmd.Parameters.Add(new SqlParameter("@EmployeeStokIDBarcode", objRecord.EmployeeStokIDBarcode));


                    if (IsNewRecord == true)
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 1));
                    else
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 2));
                    string obj =Convert.ToString( objCmd.ExecuteScalar());

                    if (obj != null)
                        objRet = Convert.ToString(obj);

                }
            }
            return objRet;

        }

        public static string Delete(Menu_FactoryRunCommandMaster objRecord)
        {
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Menu_FactoryRunCommandMaster_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@ComandID", objRecord.ComandID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@TypeStageID", objRecord.TypeStageID));
                    objCmd.Parameters.Add(new SqlParameter("@Barcode", objRecord.Barcode));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 4));
                    object obj = objCmd.ExecuteNonQuery();
                    if (obj != null)
                        return Convert.ToString(obj);
                }
            }
            return "";
        }

        /// <summary>
        /// this functio is used to get max id Comand
        /// </summary>
        /// <returns> return max id Comand +1 </returns>
        public static long GetNewID(int BranchID ,int TypeStageID,string Condion="")
        {
            try
            {
                long ID = 0;
                DataTable dt;
                string strSQL;
                //select max id item 
                strSQL = "SELECT Max(" + PremaryKey + ")+1 FROM " + TableName + " Where  BranchID =" + BranchID + " and TypeStageID=" + TypeStageID+ Condion;
                dt = Lip.SelectRecord(strSQL);//execute sql selected  stetment 
                if (dt.Rows.Count > 0)
                {
                    ID = Comon.cLong(dt.Rows[0][0].ToString());
                    if (dt.Rows[0][0].ToString() == "")
                        ID = 1;
                }

                strSQL = "Select Top 1 StartFrom From StartNumbering Where BranchID=" + MySession.GlobalBranchID
                    + " And FormName='frmManufacturingCommand'";
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

        public int GetRecordSetBySQL(string strSQL)
        {
            int ID = 0;
            try
            {
                FoundResult = false;
                dt = Lip.SelectRecord(strSQL);
                if (  dt.Rows.Count > 0)
                {
                    ID = Comon.cInt(dt.Rows[0]["ComandID"].ToString());
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
                        objCmd.CommandText = "[Menu_FactoryRunCommandMaster_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@ComandID", ComandID));
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
        public static DataTable frmGetDataDetalByIDPollutionTypeID(int ComandID, int BranchID, int FacilityID, int TypeStageID, int PollutionTypeID)
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
                        objCmd.Parameters.Add(new SqlParameter("@TypeStageID", TypeStageID));
                        objCmd.Parameters.Add(new SqlParameter("@PollutionTypeID", PollutionTypeID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 7));
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
        public static DataTable frmGetDataDetalByIDPrntageID(int ComandID, int BranchID, int FacilityID, int TypeStageID, int PrntageTypeID)
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
                        objCmd.Parameters.Add(new SqlParameter("@TypeStageID", TypeStageID));
                        objCmd.Parameters.Add(new SqlParameter("@PrntageTypeID", PrntageTypeID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 9));
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

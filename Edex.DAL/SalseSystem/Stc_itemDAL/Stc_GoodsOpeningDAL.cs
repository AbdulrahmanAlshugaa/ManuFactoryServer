using Edex.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
 


namespace Edex.DAL.Stc_itemDAL
{
    public class Stc_GoodsOpeningDAL
    {



        public static readonly string TableName = "Stc_GoodOpeningMaster";
        public static readonly string PremaryKey = "InvoiceID";


        public long InvoiceID;
        public long MethodeID;
        public long InvoiceDate;
        public string NetProcessID;
        public string CheckID;
        public long CheckSpendDate;
        public long WarningDate;
        public long SupplierID;
        public long CostCenterID;
        public long DelegateID;
        public long StoreID;
        public string Notes;
        public decimal DiscountOnTotal;
        public decimal TransportDebitAmount;
        public long SupplierInvoiceID;
        public long DebitAccount;
        public long CreditAccount;
        public long AdditionalAccountID;
        public decimal AdditionaAmmountTotal;
        public decimal InvoiceTotal;
        public long DocumentID;

        public bool FoundResult;
        public bool NeedSaving;
        public bool IsNewRecord;

        private DataTable dt;
        private string strSQL;
        private object Result;
        public static Stc_GoodOpeningDetails ConvertRowToObj(DataRow dr)
        {

            Stc_GoodOpeningMaster ObjMaster = new Stc_GoodOpeningMaster();
            ObjMaster.InvoiceID = Comon.cInt(dr["InvoiceID"].ToString());
            ObjMaster.BranchID = Comon.cInt(dr["BranchID"].ToString());
            ObjMaster.FacilityID = Comon.cInt(dr["FacilityID"].ToString());
            ObjMaster.TransportDebitAmount = Comon.cInt(dr["TransportDebitAmount"].ToString());

            ObjMaster.ReceiveDate = dr["ReceiveDate"].ToString();
            ObjMaster.InvoiceDate = dr["InvoiceDate"].ToString();
            ObjMaster.MethodeID = Comon.cInt(dr["MethodeID"].ToString());
            ObjMaster.SupplierID = Comon.cDbl(dr["SupplierID"].ToString());
            ObjMaster.CostCenterID = Comon.cInt(dr["CostCenterID"].ToString());


            ObjMaster.SupplierInvoiceID = Comon.cInt(dr["SupplierInvoiceID"].ToString());
            ObjMaster.StoreID = Comon.cInt(dr["StoreID"].ToString());
            ObjMaster.DelegateID = Comon.cInt(dr["DelegateID"].ToString());
            ObjMaster.Notes = dr["Notes"].ToString();
            ObjMaster.DiscountOnTotal = Comon.cDbl(dr["DiscountOnTotal"].ToString());

            ObjMaster.DebitAccount = Comon.cDbl(dr["DebitAccount"].ToString());
            ObjMaster.CreditAccount = Comon.cDbl(dr["CreditAccount"].ToString());
            ObjMaster.TransportDebitAccount = Comon.cDbl(dr["TransportDebitAccount"].ToString());
            ObjMaster.DiscountCreditAccount = Comon.cDbl(dr["DiscountCreditAccount"].ToString());
            ObjMaster.CheckAccount = Comon.cDbl(dr["CheckAccount"].ToString());
            ObjMaster.NetAccount = Comon.cDbl(dr["NetAccount"].ToString());
            ObjMaster.AdditionalAccount = Comon.cDbl(dr["AdditionalAccount"].ToString());
            ObjMaster.NetProcessID = dr["NetProcessID"].ToString();
            ObjMaster.CheckID = dr["CheckID"].ToString();

            ObjMaster.CurencyID = Comon.cInt(dr["CurencyID"].ToString());
            ObjMaster.CheckSpendDate = dr["CheckSpendDate"].ToString();
            ObjMaster.WarningDate = dr["WarningDate"].ToString();
            ObjMaster.DocumentID = Comon.cInt(dr["DocumentID"].ToString());
            ObjMaster.UserID = Comon.cInt(dr["UserID"].ToString());
            ObjMaster.RegDate = Comon.cInt(dr["RegDate"].ToString());
            ObjMaster.RegTime = Comon.cInt(dr["RegTime"].ToString());
            ObjMaster.EditUserID = Comon.cInt(dr["EditUserID"].ToString());
            ObjMaster.EditTime = Comon.cInt(dr["EditTime"].ToString());
            ObjMaster.EditDate = Comon.cInt(dr["EditDate"].ToString());
            ObjMaster.ComputerInfo = dr["ComputerInfo"].ToString();
            ObjMaster.EditComputerInfo = dr["EditComputerInfo"].ToString();
            ObjMaster.Cancel = Comon.cInt(dr["Cancel"].ToString());
            ObjMaster.Posted = Comon.cInt(dr["Posted"].ToString());
            ObjMaster.NetAmount = Comon.cInt(dr["NetAmount"].ToString());
            ObjMaster.Mobile = dr["Mobile"].ToString();


            Stc_GoodOpeningDetails SaleDetalObject = new Stc_GoodOpeningDetails();
            SaleDetalObject.ID = Comon.cInt(dr["ID"].ToString());
            SaleDetalObject.BarCode = dr["BarCode"].ToString();
            SaleDetalObject.SizeID = Comon.cInt(dr["SizeID"].ToString());
            SaleDetalObject.StoreID = Comon.cInt(dr["StoreID"].ToString());
            SaleDetalObject.ItemID = Comon.cInt(dr["ItemID"].ToString());
            SaleDetalObject.ArbItemName = dr["ItemName"].ToString();
            SaleDetalObject.ArbSizeName = dr["SizeName"].ToString();
            SaleDetalObject.QTY = Comon.cDec(dr["QTY"].ToString());
            SaleDetalObject.SalePrice = Comon.cDec(dr["SalePrice"].ToString());
            SaleDetalObject.Discount = Comon.cDec(dr["Discount"].ToString());
            SaleDetalObject.CostPrice = Comon.cDec(dr["CostPrice"].ToString());
            SaleDetalObject.Description = dr["Description"].ToString();
            SaleDetalObject.ExpiryDate = Comon.cDate(dr["ExpiryDate"].ToString());
            SaleDetalObject.Serials = dr["Serials"].ToString();
            SaleDetalObject.Equivalen = Comon.cDec(dr["Equivalen"].ToString());
            SaleDetalObject.Caliber = Comon.cDec(dr["Caliber"].ToString());
            SaleDetalObject.Net = Comon.cDec(dr["Net"].ToString());

            SaleDetalObject.DIAMOND_W = Comon.cDbl(dr["DIAMOND_W"].ToString());
            SaleDetalObject.BAGET_W = Comon.cDbl(dr["BAGET_W"].ToString());
            SaleDetalObject.STONE_W = Comon.cDbl(dr["STONE_W"].ToString());

            SaleDetalObject.Height = Comon.cDec(dr["Height"].ToString());
            SaleDetalObject.Width = Comon.cDec(dr["Width"].ToString());

            SaleDetalObject.AdditionalValue = Comon.cDec(dr["AdditionalValue"].ToString());
            SaleDetalObject.GoodopeningMaster = ObjMaster;
            return SaleDetalObject;
        }

        public static Stc_GoodOpeningMaster ConvertRowToObjMaster(DataRow dr)
        {
            Stc_GoodOpeningMaster ObjMaster = new Stc_GoodOpeningMaster();
            ObjMaster.InvoiceID = Comon.cInt(dr["InvoiceID"].ToString());
            ObjMaster.BranchID = Comon.cInt(dr["BranchID"].ToString());
            ObjMaster.InvoiceDate = Comon.ConvertSerialDateTo(dr["InvoiceDate"].ToString());
            ObjMaster.MethodeID = Comon.cInt(dr["MethodeID"].ToString());
            ObjMaster.SupplierID = Comon.cInt(dr["SupplierID"].ToString());
            ObjMaster.CostCenterID = Comon.cInt(dr["CostCenterID"].ToString());
            ObjMaster.StoreID = Comon.cInt(dr["StoreID"].ToString());
            ObjMaster.DelegateID = Comon.cInt(dr["DelegateID"].ToString());
            ObjMaster.Notes = dr["Notes"].ToString();
            ObjMaster.DiscountOnTotal = Comon.cDbl(dr["DiscountOnTotal"].ToString());
            ObjMaster.DebitAccount = Comon.cDbl(dr["DebitAccount"].ToString());
            ObjMaster.CreditAccount = Comon.cDbl(dr["CreditAccount"].ToString());
            ObjMaster.DiscountCreditAccount = Comon.cDbl(dr["DiscountCreditAccount"].ToString());
            ObjMaster.NetProcessID = dr["NetProcessID"].ToString();
            ObjMaster.CheckID = dr["CheckID"].ToString();
            ObjMaster.CheckAccount = Comon.cInt(dr["CheckAccount"].ToString());
            ObjMaster.Posted = Comon.cInt(dr["Posted"].ToString());
            ObjMaster.CheckSpendDate = Comon.ConvertSerialDateTo(dr["CheckSpendDate"].ToString());
            ObjMaster.WarningDate = Comon.ConvertSerialDateTo(dr["WarningDate"].ToString());
            ObjMaster.DocumentID = Comon.cInt(dr["DocumentID"].ToString());
            ObjMaster.UserID = Comon.cInt(dr["UserID"].ToString());
            ObjMaster.RegDate = Comon.cDbl(dr["RegDate"].ToString());
            ObjMaster.RegTime = Comon.cInt(dr["RegTime"].ToString());
            ObjMaster.EditUserID = Comon.cInt(dr["EditUserID"].ToString());
            ObjMaster.EditTime = Comon.cInt(dr["EditTime"].ToString());
            ObjMaster.EditDate = Comon.cInt(dr["EditDate"].ToString());
            ObjMaster.ComputerInfo = dr["ComputerInfo"].ToString();
            ObjMaster.EditComputerInfo = dr["EditComputerInfo"].ToString();
            ObjMaster.Cancel = Comon.cInt(dr["Cancel"].ToString());
            ObjMaster.NetAmount = Comon.cDbl(dr["NetAmount"].ToString());
            ObjMaster.NetAccount = Comon.cDbl(dr["NetAccount"].ToString());
            ObjMaster.Mobile = dr["Mobile"].ToString();
            return ObjMaster;
        }

        public static List<Stc_GoodOpeningDetails> GetDataDetalByID(int ID, int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Stc_GoodsOpening_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@InvoiceID", ID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 5));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            List<Stc_GoodOpeningDetails> Returned = new List<Stc_GoodOpeningDetails>();
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

        public static BindingList<Stc_GoodOpeningDetails> GetDataDetalByID_BindingList(int ID, int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Stc_GoodsOpening_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@InvoiceID", ID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 5));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);

                        if (dt != null)
                        {
                            BindingList<Stc_GoodOpeningDetails> Returned = new BindingList<Stc_GoodOpeningDetails>();
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

        public static DataTable frmGetDataDetalByID(long ID, int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Stc_GoodsOpening_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@InvoiceID", ID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 5));
                        SqlParameter pvNewId = new SqlParameter();
                        pvNewId.ParameterName = "@product_count";
                        pvNewId.DbType = DbType.Int32;
                        pvNewId.Direction = ParameterDirection.Output;
                        objCmd.Parameters.Add(pvNewId);

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
            catch (Exception ex)
            {
                return null;
            }
        }

        public static Stc_GoodOpeningMaster GetDataMasterByID(int ID, int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Stc_GoodsOpening_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@InvoiceID", ID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 3));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            Stc_GoodOpeningMaster Returned = new Stc_GoodOpeningMaster();
                            Returned = (ConvertRowToObjMaster(dt.Rows[0]));
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
        public static List<Stc_GoodOpeningMaster> GetAllMasterData(int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Stc_GoodsOpening_SP]";
                        objCmd.Parameters.AddWithValue("@BranchID", BranchID);
                        objCmd.Parameters.AddWithValue("@FacilityID", FacilityID);

                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 7));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            List<Stc_GoodOpeningMaster> Returned = new List<Stc_GoodOpeningMaster>();
                            foreach (DataRow rows in dt.Rows)
                                Returned.Add(ConvertRowToObjMaster(rows));
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
        public static string InsertUsingXML(Stc_GoodOpeningMaster objRecord, int USERCREATED, Boolean IsNewrecord)
        {
            string objRet = "";
            string DitmeXML = ConvertObjectToXMLString(objRecord.Datails);
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Stc_GoodsOpening_SP]";
                    objCmd.Parameters.Add(new SqlParameter("xmlData", SqlDbType.Xml)).Value = DitmeXML;
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceID", objRecord.InvoiceID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceDate", Comon.cInt(objRecord.InvoiceDate)));
                    objCmd.Parameters.Add(new SqlParameter("@ReceiveDate", Comon.cInt(objRecord.ReceiveDate)));
                    objCmd.Parameters.Add(new SqlParameter("@MethodeID", objRecord.MethodeID));
                    objCmd.Parameters.Add(new SqlParameter("@CurrencyID", objRecord.CurencyID));
                    objCmd.Parameters.Add(new SqlParameter("@SupplierID", objRecord.SupplierID));
                    objCmd.Parameters.Add(new SqlParameter("@SupplierInvoiceID", objRecord.SupplierInvoiceID));
                    objCmd.Parameters.Add(new SqlParameter("@CostCenterID", objRecord.CostCenterID));
                    objCmd.Parameters.Add(new SqlParameter("@StoreID", objRecord.StoreID));
                    objCmd.Parameters.Add(new SqlParameter("@DelegateID", objRecord.DelegateID));
                    objCmd.Parameters.Add(new SqlParameter("@Notes", objRecord.Notes));
                    objCmd.Parameters.Add(new SqlParameter("@DebitAccount", objRecord.DebitAccount));
                    objCmd.Parameters.Add(new SqlParameter("@CreditAccount", objRecord.CreditAccount));
                    objCmd.Parameters.Add(new SqlParameter("@CreditGoldAccountID", objRecord.DebitAccount));
                    objCmd.Parameters.Add(new SqlParameter("@DebitGoldAccountID", objRecord.DebitGoldAccountID));
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceEquivalenTotal", objRecord.InvoiceGoldTotal));
                    objCmd.Parameters.Add(new SqlParameter("@DiscountCreditAccount", objRecord.DiscountCreditAccount));
                    objCmd.Parameters.Add(new SqlParameter("@TransportDebitAccount", objRecord.TransportDebitAccount));
                    objCmd.Parameters.Add(new SqlParameter("@TransportDebitAmount", objRecord.TransportDebitAmount));
                    objCmd.Parameters.Add(new SqlParameter("@NetAccount", objRecord.NetAccount));
                    objCmd.Parameters.Add(new SqlParameter("@AdditionalAccount", objRecord.AdditionalAccount));
                    objCmd.Parameters.Add(new SqlParameter("@CheckAccount", objRecord.CheckAccount));
                    objCmd.Parameters.Add(new SqlParameter("@NetProcessID", objRecord.NetProcessID));
                    objCmd.Parameters.Add(new SqlParameter("@CheckID", objRecord.CheckID));
                    objCmd.Parameters.Add(new SqlParameter("@CheckSpendDate", Comon.cInt(objRecord.CheckSpendDate)));
                    objCmd.Parameters.Add(new SqlParameter("@WarningDate", Comon.cInt(objRecord.WarningDate)));
                    objCmd.Parameters.Add(new SqlParameter("@DocumentID", objRecord.DocumentID));
                    objCmd.Parameters.Add(new SqlParameter("@UserID", objRecord.UserID));
                    objCmd.Parameters.Add(new SqlParameter("@RegDate", objRecord.RegDate));
                    objCmd.Parameters.Add(new SqlParameter("@RegTime", objRecord.RegTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                    objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditDate", objRecord.EditDate));
                    objCmd.Parameters.Add(new SqlParameter("@ComputerInfo", ""));
                    objCmd.Parameters.Add(new SqlParameter("@EditComputerInfo", objRecord.EditComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@Cancel", objRecord.Cancel));
                    objCmd.Parameters.Add(new SqlParameter("@Posted", objRecord.Posted));
                    objCmd.Parameters.Add(new SqlParameter("@NetAmount", objRecord.NetAmount));
                    objCmd.Parameters.Add(new SqlParameter("@DiscountOnTotal", objRecord.DiscountOnTotal));
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceTotal", objRecord.InvoiceTotal));
                    objCmd.Parameters.Add(new SqlParameter("@AdditionaAmountTotal", objRecord.AdditionaAmountTotal));
                    objCmd.Parameters.Add(new SqlParameter("@NetBalance", objRecord.NetBalance));
                    objCmd.Parameters.Add(new SqlParameter("@RegistrationNo", objRecord.RegistrationNo));
                    objCmd.Parameters.Add(new SqlParameter("@VATID", objRecord.VATID));
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceImage", objRecord.InvoiceImage));
                    objCmd.Parameters.Add(new SqlParameter("@OperationTypeName", objRecord.OperationTypeName));
                    objCmd.Parameters.Add(new SqlParameter("@NetType", objRecord.NetType));
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceDiamondTotal", 0));
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceGoldTotal", objRecord.InvoiceGoldTotal));
                    objCmd.Parameters.Add(new SqlParameter("@WeightDiamondTotal", 0));
                    objCmd.Parameters.Add(new SqlParameter("@WeightGoldTotal", objRecord.InvoiceGoldTotal));
                    objCmd.Parameters.Add(new SqlParameter("@GoldUsing", objRecord.GoldUsing));
                    objCmd.Parameters.Add(new SqlParameter("@Mobile", objRecord.Mobile));
                    objCmd.Parameters.Add(new SqlParameter("@SupplierName", objRecord.SupplierName));
                    objCmd.Parameters.Add(new SqlParameter("@TypeGold", objRecord.TypeGold));

                    SqlParameter pvNewId = new SqlParameter();
                    pvNewId.ParameterName = "@Product_count";
                    pvNewId.DbType = DbType.Int32;
                    pvNewId.Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add(pvNewId);
                    if (IsNewrecord)
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 1));
                    else
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 2));
                    object obj = objCmd.ExecuteScalar();

                    string val = objCmd.Parameters["@Product_count"].Value.ToString();
                     
                        if (val != null)
                            objRet = Convert.ToString(val);

                }
            }
            return objRet;

        }

        public static string InsertUsingSaveXML(Stc_GoodOpeningMaster objRecord, int USERCREATED, Boolean IsNewrecord)
        {
            string objRet = "";
            string DitmeXML = ConvertObjectToXMLString(objRecord.Datails);
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Stc_GoodsOpening_SP]";
                    objCmd.Parameters.Add(new SqlParameter("xmlData", SqlDbType.Xml)).Value = DitmeXML;
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceID", objRecord.InvoiceID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceDate", Comon.cInt(objRecord.InvoiceDate)));
                    objCmd.Parameters.Add(new SqlParameter("@ReceiveDate", Comon.cInt(objRecord.ReceiveDate)));
                    objCmd.Parameters.Add(new SqlParameter("@MethodeID", objRecord.MethodeID));
                    objCmd.Parameters.Add(new SqlParameter("@CurrencyID", 1));
                    objCmd.Parameters.Add(new SqlParameter("@SupplierID", objRecord.SupplierID));
                    objCmd.Parameters.Add(new SqlParameter("@SupplierInvoiceID", objRecord.SupplierInvoiceID));
                    objCmd.Parameters.Add(new SqlParameter("@CostCenterID", objRecord.CostCenterID));
                    objCmd.Parameters.Add(new SqlParameter("@StoreID", objRecord.StoreID));
                    objCmd.Parameters.Add(new SqlParameter("@DelegateID", objRecord.DelegateID));
                    objCmd.Parameters.Add(new SqlParameter("@Notes", objRecord.Notes));
                    objCmd.Parameters.Add(new SqlParameter("@DebitAccount", objRecord.DebitAccount));
                    objCmd.Parameters.Add(new SqlParameter("@CreditAccount", objRecord.CreditAccount));
                    objCmd.Parameters.Add(new SqlParameter("@CreditGoldAccountID", objRecord.DebitAccount));
                    objCmd.Parameters.Add(new SqlParameter("@DebitGoldAccountID", objRecord.DebitGoldAccountID));
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceEquivalenTotal", objRecord.InvoiceGoldTotal));
                    objCmd.Parameters.Add(new SqlParameter("@DiscountCreditAccount", objRecord.DiscountCreditAccount));
                    objCmd.Parameters.Add(new SqlParameter("@TransportDebitAccount", objRecord.TransportDebitAccount));
                    objCmd.Parameters.Add(new SqlParameter("@TransportDebitAmount", objRecord.TransportDebitAmount));
                    objCmd.Parameters.Add(new SqlParameter("@NetAccount", objRecord.NetAccount));
                    objCmd.Parameters.Add(new SqlParameter("@AdditionalAccount", objRecord.AdditionalAccount));
                    objCmd.Parameters.Add(new SqlParameter("@CheckAccount", objRecord.CheckAccount));
                    objCmd.Parameters.Add(new SqlParameter("@NetProcessID", objRecord.NetProcessID));
                    objCmd.Parameters.Add(new SqlParameter("@CheckID", objRecord.CheckID));
                    objCmd.Parameters.Add(new SqlParameter("@CheckSpendDate", Comon.cInt(objRecord.CheckSpendDate)));
                    objCmd.Parameters.Add(new SqlParameter("@WarningDate", Comon.cInt(objRecord.WarningDate)));
                    objCmd.Parameters.Add(new SqlParameter("@DocumentID", objRecord.DocumentID));
                    objCmd.Parameters.Add(new SqlParameter("@UserID", objRecord.UserID));
                    objCmd.Parameters.Add(new SqlParameter("@RegDate", objRecord.RegDate));
                    objCmd.Parameters.Add(new SqlParameter("@RegTime", objRecord.RegTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                    objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditDate", objRecord.EditDate));
                    objCmd.Parameters.Add(new SqlParameter("@ComputerInfo", ""));
                    objCmd.Parameters.Add(new SqlParameter("@EditComputerInfo", objRecord.EditComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@Cancel", objRecord.Cancel));
                    objCmd.Parameters.Add(new SqlParameter("@Posted", objRecord.Posted));
                    objCmd.Parameters.Add(new SqlParameter("@NetAmount", objRecord.NetAmount));
                    objCmd.Parameters.Add(new SqlParameter("@DiscountOnTotal", objRecord.DiscountOnTotal));
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceTotal", objRecord.InvoiceTotal));
                    objCmd.Parameters.Add(new SqlParameter("@AdditionaAmountTotal", objRecord.AdditionaAmountTotal));
                    objCmd.Parameters.Add(new SqlParameter("@NetBalance", objRecord.NetBalance));
                    objCmd.Parameters.Add(new SqlParameter("@RegistrationNo", objRecord.RegistrationNo));
                    objCmd.Parameters.Add(new SqlParameter("@VATID", objRecord.VATID));
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceImage", objRecord.InvoiceImage));
                    objCmd.Parameters.Add(new SqlParameter("@OperationTypeName", objRecord.OperationTypeName));
                    objCmd.Parameters.Add(new SqlParameter("@NetType", objRecord.NetType));
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceDiamondTotal", 0));
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceGoldTotal", objRecord.InvoiceGoldTotal));
                    objCmd.Parameters.Add(new SqlParameter("@WeightDiamondTotal", 0));
                    objCmd.Parameters.Add(new SqlParameter("@WeightGoldTotal", objRecord.InvoiceGoldTotal));
                    objCmd.Parameters.Add(new SqlParameter("@GoldUsing", objRecord.GoldUsing));
                    objCmd.Parameters.Add(new SqlParameter("@Mobile", objRecord.Mobile));
                    objCmd.Parameters.Add(new SqlParameter("@SupplierName", objRecord.SupplierName));
                    SqlParameter pvNewId = new SqlParameter();
                    pvNewId.ParameterName = "@Product_count";
                    pvNewId.DbType = DbType.Int32;
                    pvNewId.Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add(pvNewId);
                    if (IsNewrecord)
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 1));
                    else
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 2));
                    object obj = objCmd.ExecuteScalar();

                    string val = objCmd.Parameters["@Product_count"].Value.ToString();
                    if (!IsNewrecord)
                        objRet = Convert.ToString(obj);
                    else
                        if (val != null)
                            objRet = Convert.ToString(val);

                }
            }
            return objRet;

        }

        public static Int32 InsertStc_GoodOpeningMaster(Stc_GoodOpeningMaster objRecord)
        {
            Int32 objRet = 0;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Stc_GoodsOpening_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceID", objRecord.InvoiceID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceDate", objRecord.InvoiceDate));
                    objCmd.Parameters.Add(new SqlParameter("@MethodeID", objRecord.MethodeID));
                    objCmd.Parameters.Add(new SqlParameter("@SupplierID", objRecord.SupplierID));
                    objCmd.Parameters.Add(new SqlParameter("@CostCenterID", objRecord.CostCenterID));
                    objCmd.Parameters.Add(new SqlParameter("@SupplierInvoiceID", objRecord.SupplierInvoiceID));
                    objCmd.Parameters.Add(new SqlParameter("@StoreID", objRecord.StoreID));
                    objCmd.Parameters.Add(new SqlParameter("@DelegateID", objRecord.DelegateID));
                    objCmd.Parameters.Add(new SqlParameter("@Notes", objRecord.Notes));
                    objCmd.Parameters.Add(new SqlParameter("@DiscountOnTotal", objRecord.DiscountOnTotal));
                    objCmd.Parameters.Add(new SqlParameter("@DebitAccount", objRecord.DebitAccount));
                    objCmd.Parameters.Add(new SqlParameter("@CreditAccount", objRecord.CreditAccount));
                    objCmd.Parameters.Add(new SqlParameter("@DiscountCreditAccount", objRecord.DiscountCreditAccount));
                    objCmd.Parameters.Add(new SqlParameter("@NetProcessID", objRecord.NetProcessID));
                    objCmd.Parameters.Add(new SqlParameter("@CheckID", objRecord.CheckID));
                    objCmd.Parameters.Add(new SqlParameter("@CheckSpendDate", objRecord.CheckSpendDate));
                    objCmd.Parameters.Add(new SqlParameter("@WarningDate", objRecord.WarningDate));

                    objCmd.Parameters.Add(new SqlParameter("@DocumentID", objRecord.DocumentID));
                    objCmd.Parameters.Add(new SqlParameter("@UserID", objRecord.UserID));
                    objCmd.Parameters.Add(new SqlParameter("@RegDate", objRecord.RegDate));
                    objCmd.Parameters.Add(new SqlParameter("@RegTime", objRecord.RegTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                    objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditDate", objRecord.EditDate));
                    objCmd.Parameters.Add(new SqlParameter("@ComputerInfo", objRecord.ComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@EditComputerInfo", objRecord.EditComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@Cancel", objRecord.Cancel));

                    objCmd.Parameters.Add(new SqlParameter("@NetAmount", objRecord.NetAmount));
                    objCmd.Parameters.Add(new SqlParameter("@NetAccount", objRecord.NetAccount));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 1));
                    object obj = objCmd.ExecuteScalar();
                    if (obj != null)
                        objRet = Convert.ToInt32(obj);
                }
            }
            return objRet;
        }

        public static long UpdateUsingXML(Stc_GoodOpeningMaster objRecord, int USERCREATED)
        {
            Int32 objRet = 0;
            string DitmeXML = ConvertObjectToXMLString(objRecord.Datails);
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Stc_GoodsOpening_SP]";
                    objCmd.Parameters.Add("@xmlSaleDatial", SqlDbType.Xml, 1500, DitmeXML);
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceID", objRecord.InvoiceID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceDate", objRecord.InvoiceDate));
                    objCmd.Parameters.Add(new SqlParameter("@MethodeID", objRecord.MethodeID));
                    objCmd.Parameters.Add(new SqlParameter("@SupplierID", objRecord.SupplierID));
                    objCmd.Parameters.Add(new SqlParameter("@CostCenterID", objRecord.CostCenterID));

                    objCmd.Parameters.Add(new SqlParameter("@StoreID", objRecord.StoreID));
                    objCmd.Parameters.Add(new SqlParameter("@DelegateID", objRecord.DelegateID));
                    objCmd.Parameters.Add(new SqlParameter("@Notes", objRecord.Notes));
                    objCmd.Parameters.Add(new SqlParameter("@DiscountOnTotal", objRecord.DiscountOnTotal));
                    objCmd.Parameters.Add(new SqlParameter("@DebitAccount", objRecord.DebitAccount));
                    objCmd.Parameters.Add(new SqlParameter("@CreditAccount", objRecord.CreditAccount));
                    objCmd.Parameters.Add(new SqlParameter("@DiscountCreditAccount", objRecord.DiscountCreditAccount));
                    objCmd.Parameters.Add(new SqlParameter("@NetProcessID", objRecord.NetProcessID));
                    objCmd.Parameters.Add(new SqlParameter("@CheckID", objRecord.CheckID));
                    objCmd.Parameters.Add(new SqlParameter("@CheckSpendDate", objRecord.CheckSpendDate));
                    objCmd.Parameters.Add(new SqlParameter("@WarningDate", objRecord.WarningDate));

                    objCmd.Parameters.Add(new SqlParameter("@DocumentID", objRecord.DocumentID));
                    objCmd.Parameters.Add(new SqlParameter("@UserID", objRecord.UserID));
                    objCmd.Parameters.Add(new SqlParameter("@RegDate", objRecord.RegDate));
                    objCmd.Parameters.Add(new SqlParameter("@RegTime", objRecord.RegTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                    objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditDate", objRecord.EditDate));
                    objCmd.Parameters.Add(new SqlParameter("@ComputerInfo", objRecord.ComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@EditComputerInfo", objRecord.EditComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@Cancel", objRecord.Cancel));

                    objCmd.Parameters.Add(new SqlParameter("@NetAmount", objRecord.NetAmount));
                    objCmd.Parameters.Add(new SqlParameter("@NetAccount", objRecord.NetAccount));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 1));
                    object obj = objCmd.ExecuteScalar();
                    if (obj != null)
                        objRet = Convert.ToInt32(obj);
                }
            }
            return objRet;

        }

        public static int DeleteStc_GoodOpeningMaster(Stc_GoodOpeningMaster objRecord)
        {

            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Stc_GoodsOpening_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceID", objRecord.InvoiceID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@EditDate", objRecord.EditDate));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                    SqlParameter pvNewId = new SqlParameter();
                    pvNewId.ParameterName = "@product_count";
                    pvNewId.DbType = DbType.Int32;
                    pvNewId.Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add(pvNewId);
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 4));
                    object obj = objCmd.ExecuteNonQuery();
                    if (obj != null)
                        return Convert.ToInt32(obj);
                }
            }
            return 0;

        }


        public static SalseInvoicesReport ConvertRowToObjReport1(DataRow dr)
        {
            SalseInvoicesReport ObjMaster = new SalseInvoicesReport();
            ObjMaster.InvoiceID = dr["InvoiceID"].ToString();
            ObjMaster.InvoiceDate = Comon.ConvertSerialDateTo(dr["InvoiceDate"].ToString());
            ObjMaster.SaleMethod = dr["MethodeName"].ToString();
            ObjMaster.CustomerName = dr["SupplierName"].ToString();
            ObjMaster.CostCenterName = dr["CostCenterName"].ToString();
            ObjMaster.DescountTotal = Comon.cInt(dr["DiscountLines"].ToString()) + Comon.cInt(dr["DiscountOnTotal"].ToString()).ToString();
            ObjMaster.StoreName = dr["StoreName"].ToString();
            ObjMaster.Notes = dr["Notes"].ToString();
            ObjMaster.Total = dr["Total"].ToString();
            //ObjMaster.SaleDelegateName = dr["SaleDelegateName"].ToString();
            ObjMaster.SumVAt = dr["SumVat"].ToString();
            ObjMaster.Net = Comon.cInt(dr["Total"].ToString()) - Comon.cInt(dr["DiscountOnTotal"].ToString()) + Comon.cInt(dr["SumVat"].ToString()).ToString();
            // ObjMaster.BranchID = dr["BranchID"].ToString();
            return ObjMaster;

        }

        public static SalseInvoicesReport ConvertRowToObjReport(DataRow dr)
        {
            SalseInvoicesReport ObjMaster = new SalseInvoicesReport();
            ObjMaster.InvoiceID = dr["InvoiceID"].ToString();
            // ObjMaster.BranchID = dr["BranchID"].ToString();
            ObjMaster.InvoiceDate = dr["InvoiceDate"].ToString();
            ObjMaster.SaleMethod = dr["Method"].ToString();
            ObjMaster.CustomerName = dr["SupplierName"].ToString();
            ObjMaster.CostCenterName = dr["CostCenterName"].ToString();
            // ObjMaster.SellerName = dr["PurchaseDelegate"].ToString();
            ObjMaster.StoreName = dr["StoreName"].ToString();
            return ObjMaster;
        }

        public static List<SalseInvoicesReport> GetAllDataForReport(int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Stc_GoodsOpening_SP]";
                        objCmd.Parameters.AddWithValue("@BranchID", BranchID);
                        objCmd.Parameters.AddWithValue("@FacilityID", FacilityID);
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 7));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            List<SalseInvoicesReport> Returned = new List<SalseInvoicesReport>();
                            foreach (DataRow rows in dt.Rows)
                                Returned.Add(ConvertRowToObjReport(rows));
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

        public static List<SalseInvoicesReport> GetReport(string sql)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Reports_SP]";
                        objCmd.Parameters.AddWithValue("@sqlCommand", sql);
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 8));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            List<SalseInvoicesReport> Returned = new List<SalseInvoicesReport>();
                            foreach (DataRow rows in dt.Rows)
                                Returned.Add(ConvertRowToObjReport1(rows));
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

        public static long GetNewID(int FacilityID, int BranchID, int USERCREATED)
        {
            try
            {
                long ID = 0;
                DataTable dt;
                string strSQL;

                strSQL = "SELECT Max(" + PremaryKey + ")+1 FROM " + TableName + " Where  BranchID =" + BranchID;
                dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count > 0)
                    ID = Comon.cLong(dt.Rows[0][0].ToString());
                if (ID == 0)
                    ID = 1;

                strSQL = "Select Top 1 StartFrom From StartNumbering Where BranchID=" + BranchID
                    + " And FormName='frmPurchaseInvoice'";
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

        public long GetRecordSetBySQL(string strSQL)
        {
            long ID = 0;
            try
            {
                FoundResult = false;
                dt = Lip.SelectRecord(strSQL);
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
        public static long InsertUsingXML_GoodOpening(Stc_GoodOpeningMaster objRecord, int USERCREATED, Boolean IsNewrecord)
        {
            Int32 objRet = 0;
            string DitmeXML = ConvertObjectToXMLString(objRecord.Datails);
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Stc_GoodOpening_SP]";
                    objCmd.Parameters.Add(new SqlParameter("xmlData", SqlDbType.Xml)).Value = DitmeXML;
                    // objCmd.Parameters.Add("@xmlSaleDatial", SqlDbType.Xml, 1500, DitmeXML);
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceID", objRecord.InvoiceID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceDate", objRecord.InvoiceDate));
                    objCmd.Parameters.Add(new SqlParameter("@ReceiveDate", objRecord.ReceiveDate));
                    objCmd.Parameters.Add(new SqlParameter("@MethodeID", objRecord.MethodeID));
                    objCmd.Parameters.Add(new SqlParameter("@CurencyID", objRecord.CurencyID));
                    objCmd.Parameters.Add(new SqlParameter("@SupplierID", objRecord.SupplierID));
                    objCmd.Parameters.Add(new SqlParameter("@SupplierInvoiceID", objRecord.SupplierInvoiceID));
                    objCmd.Parameters.Add(new SqlParameter("@CostCenterID", objRecord.CostCenterID));
                    objCmd.Parameters.Add(new SqlParameter("@StoreID", objRecord.StoreID));
                    objCmd.Parameters.Add(new SqlParameter("@DelegateID", objRecord.DelegateID));
                    objCmd.Parameters.Add(new SqlParameter("@Notes", objRecord.Notes));

                    objCmd.Parameters.Add(new SqlParameter("@DebitAccount", objRecord.DebitAccount));
                    objCmd.Parameters.Add(new SqlParameter("@CreditAccount", objRecord.CreditAccount));
                    objCmd.Parameters.Add(new SqlParameter("@DiscountCreditAccount", objRecord.DiscountCreditAccount));
                    objCmd.Parameters.Add(new SqlParameter("@TransportDebitAccount", objRecord.TransportDebitAccount));
                    objCmd.Parameters.Add(new SqlParameter("@NetAccount", objRecord.NetAccount));
                    objCmd.Parameters.Add(new SqlParameter("@AdditionalAccount", objRecord.AdditionalAccount));
                    objCmd.Parameters.Add(new SqlParameter("@CheckAccount", objRecord.CheckAccount));


                    objCmd.Parameters.Add(new SqlParameter("@NetProcessID", objRecord.NetProcessID));
                    objCmd.Parameters.Add(new SqlParameter("@CheckID", objRecord.CheckID));
                    objCmd.Parameters.Add(new SqlParameter("@CheckSpendDate", objRecord.CheckSpendDate));
                    objCmd.Parameters.Add(new SqlParameter("@WarningDate", objRecord.WarningDate));


                    objCmd.Parameters.Add(new SqlParameter("@DocumentID", objRecord.DocumentID));
                    objCmd.Parameters.Add(new SqlParameter("@UserID", objRecord.UserID));
                    objCmd.Parameters.Add(new SqlParameter("@RegDate", objRecord.RegDate));
                    objCmd.Parameters.Add(new SqlParameter("@RegTime", objRecord.RegTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                    objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditDate", objRecord.EditDate));
                    objCmd.Parameters.Add(new SqlParameter("@ComputerInfo", objRecord.ComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@EditComputerInfo", objRecord.EditComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@Cancel", objRecord.Cancel));
                    objCmd.Parameters.Add(new SqlParameter("@Posted", objRecord.Posted));

                    objCmd.Parameters.Add(new SqlParameter("@NetAmount", objRecord.NetAmount));
                    objCmd.Parameters.Add(new SqlParameter("@TransportDebitAmount", objRecord.TransportDebitAmount));
                    objCmd.Parameters.Add(new SqlParameter("@DiscountOnTotal", objRecord.DiscountOnTotal));
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceTotal", objRecord.InvoiceTotal));
                    objCmd.Parameters.Add(new SqlParameter("@AdditionaAmountTotal", objRecord.AdditionaAmountTotal));
                    objCmd.Parameters.Add(new SqlParameter("@NetBalance", objRecord.NetBalance));

                    objCmd.Parameters.Add(new SqlParameter("@VATID", objRecord.VATID));
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceImage", objRecord.InvoiceImage));

                    objCmd.Parameters.Add(new SqlParameter("@NetType", objRecord.NetType));


                    if (IsNewrecord == true)
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

        public static DataTable frmGetDataDetalByID_GoodOpening(long ID, int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Stc_GoodOpening_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@InvoiceID", ID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 5));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);

                        if (dt != null)
                        {
                            return dt;
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


        public DataTable FillDataGridDT(long Id)
        {
            try
            {
                DataTable dt;
                string strSQL;
                strSQL = " SELECT  Stc_GoodOpeningDetails.HavVat, Stc_GoodOpeningDetails.BarCode, Stc_GoodOpeningDetails.Serials, Stc_GoodOpeningDetails.ItemID, "
                 + " Stc_Items.ArbName AS ItemName, Stc_GoodOpeningDetails.SizeID, Stc_SizingUnits.ArbName AS SizeName, Stc_GoodOpeningDetails.QTY, "
                 + " Stc_GoodOpeningDetails.CostPrice, Stc_GoodOpeningDetails.AdditionaAmmount, Stc_GoodOpeningDetails.Discount, "
                 + " Stc_GoodOpeningDetails.Caliber, Stc_GoodOpeningDetails.Equivalen, Stc_GoodOpeningDetails.ExpiryDate, Stc_GoodOpeningDetails.Bones, "
                 + " Stc_GoodOpeningDetails.SalePrice, Stc_GoodOpeningDetails.BarCode AS Expr1, Stc_ItemsBrands.ArbName AS BrandName, "
                 + " Stc_ItemsSizes.ArbName AS Size FROM  Stc_ItemsBrands RIGHT OUTER JOIN   Stc_Items ON Stc_ItemsBrands.BrandID = Stc_Items.BrandID LEFT OUTER JOIN "
                 + " Stc_ItemsSizes ON Stc_Items.SizeID = Stc_ItemsSizes.SizeID RIGHT OUTER JOIN    Stc_GoodOpeningDetails INNER JOIN "
                 + " Stc_GoodOpeningMaster ON Stc_GoodOpeningDetails.InvoiceID = Stc_GoodOpeningMaster.InvoiceID AND "
                 + " Stc_GoodOpeningDetails.BranchID = Stc_GoodOpeningMaster.BranchID LEFT OUTER JOIN "
                 + " Stc_SizingUnits ON Stc_GoodOpeningDetails.SizeID = Stc_SizingUnits.SizeID ON Stc_Items.ItemID = Stc_GoodOpeningDetails.ItemID "
                 + " WHERE  (Stc_GoodOpeningMaster.Cancel = 0) AND (Stc_GoodOpeningDetails.Cancel = 0) AND (Stc_GoodOpeningMaster.InvoiceID = " + Id + ") AND "
                 + " (Stc_GoodOpeningMaster.BranchID = " + 1 + ") ORDER BY Stc_GoodOpeningDetails.ID";

                //Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL);
                dt = Lip.SelectRecord(strSQL);
                return dt;
            }
            catch (Exception ex)
            {
                return null;

            }
        }




        public DataTable FillDataGridDT_GoodOpening(long Id)
        {
            try
            {
                DataTable dt;
                string strSQL;
                strSQL = " SELECT  Stc_GoodOpeningDetails.HavVat, Stc_GoodOpeningDetails.BarCode, Stc_GoodOpeningDetails.Serials, Stc_GoodOpeningDetails.ItemID, "
                 + " Stc_Items.ArbName AS ItemName, Stc_GoodOpeningDetails.SizeID, Stc_SizingUnits.ArbName AS SizeName, Stc_GoodOpeningDetails.QTY, "
                 + " Stc_GoodOpeningDetails.CostPrice, Stc_GoodOpeningDetails.AdditionaAmmount, Stc_GoodOpeningDetails.Discount, "
                 + " Stc_GoodOpeningDetails.Caliber, Stc_GoodOpeningDetails.Equivalen, Stc_GoodOpeningDetails.ExpiryDate, Stc_GoodOpeningDetails.Bones, "
                 + " Stc_GoodOpeningDetails.SalePrice, Stc_GoodOpeningDetails.BarCode AS Expr1, Stc_ItemsBrands.ArbName AS BrandName, "
                 + " Stc_ItemsSizes.ArbName AS Size FROM  Stc_ItemsBrands RIGHT OUTER JOIN   Stc_Items ON Stc_ItemsBrands.BrandID = Stc_Items.BrandID LEFT OUTER JOIN "
                 + " Stc_ItemsSizes ON Stc_Items.SizeID = Stc_ItemsSizes.SizeID RIGHT OUTER JOIN    Stc_GoodOpeningDetails INNER JOIN "
                 + " Stc_GoodOpeningMaster ON Stc_GoodOpeningDetails.InvoiceID = Stc_GoodOpeningMaster.InvoiceID AND "
                 + " Stc_GoodOpeningDetails.BranchID = Stc_GoodOpeningMaster.BranchID LEFT OUTER JOIN "
                 + " Stc_SizingUnits ON Stc_GoodOpeningDetails.SizeID = Stc_SizingUnits.SizeID ON Stc_Items.ItemID = Stc_GoodOpeningDetails.ItemID "
                 + " WHERE  (Stc_GoodOpeningMaster.Cancel = 0) AND (Stc_GoodOpeningDetails.Cancel = 0) AND (Stc_GoodOpeningMaster.InvoiceID = " + Id + ") AND "
                 + " (Stc_GoodOpeningMaster.BranchID = " + 1 + ") ORDER BY Stc_GoodOpeningDetails.ID";

                //Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL);
                dt = Lip.SelectRecord(strSQL);
                return dt;
            }
            catch (Exception ex)
            {
                return null;

            }
        }

        public static long GetNewID_GoodOpening(int FacilityID, int BranchID, int USERCREATED)
        {
            try
            {
                long ID = 0;
                DataTable dt;
                string strSQL;

                strSQL = "SELECT Max(" + PremaryKey + ")+1 FROM Stc_GoodOpeningMaster Where  BranchID =" + BranchID;
                dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count > 0)
                    ID = Comon.cLong(dt.Rows[0][0].ToString());


                strSQL = "Select Top 1 StartFrom From StartNumbering Where BranchID=" + BranchID
                    + " And FormName='frmPurchaseInvoice'";
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



        public bool CheckItemHasTransactions(string Barcode)
        {

            if (Stc_ItemsInonBail_Details(Barcode) == true)
                return true;
            else if (Stc_ItemsOutonBail_Details(Barcode) == true)
                return true;

            else if (ReceiptVoucherDetails(Barcode) == true)
                return true;
            else if (SpendVoucherDetails(Barcode) == true)
                return true;
            else if (SalesInvoiceDetails(Barcode) == true)
                return true;
            else if (PurchaseInvoiceDetails(Barcode) == true)
                return true;
            return false;
        }

        private bool PurchaseInvoiceDetails(string Barcode)
        {
             
            strSQL = "SELECT ID FROM Sales_PurchaseInvoiceDetails  WHERE InvoiceID!=-1 and   BarCode ='" + Barcode + "'";
            DataTable dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// This Functon is used to Check if the Item has any Transactions in Acc_ReceiptVoucherDetails table 
        /// </summary>
        /// <param name="Barcode"></param>
        /// <returns>return value boolen :true if has, false if has not</returns>
        public bool ReceiptVoucherDetails(string Barcode)
        {
            strSQL = "SELECT Barcode FROM Acc_ReceiptVoucherDetails WHERE   Barcode = '" + Barcode + "'";
            DataTable dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// This function is used check if the Item has Spend Voucher Details
        /// </summary>
        /// <param name="ItemID"></param>
        /// <returns>return value boolen: Ture if has , false has not</returns>
        public bool SpendVoucherDetails(string Barcode)
        {
            strSQL = "SELECT Barcode FROM Acc_SpendVoucherDetails WHERE   Barcode = '" + Barcode + "'";
            DataTable dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;


        }
        private bool Stc_ItemsInonBail_Details(string BarCode)
        {
            strSQL = "SELECT ItemID FROM Stc_ItemsInonBail_Details  WHERE    BarCode = '" + BarCode + "'";
            DataTable dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;

        }

        private bool Stc_ItemsOutonBail_Details(string BarCode)
        {
            strSQL = "SELECT ItemID FROM Stc_ItemsOutonBail_Details  WHERE    BarCode = '" + BarCode + "'";
            DataTable dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// This function is used to check if the item Sales Invoice Master
        /// </summary>
        /// <param name="ItemID"></param>
        /// <returns>return value boolen:true if has Sales Invoice , false has not</returns>
        public bool SalesInvoiceDetails(string BarCode)
        {
            strSQL = "SELECT ID FROM Sales_SalesInvoiceDetails  WHERE   BarCode ='" + BarCode + "'";
            DataTable dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }


    }
}

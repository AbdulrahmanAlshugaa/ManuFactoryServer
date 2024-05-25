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

namespace Edex.DAL.SalseSystem
{
   public class Sale_CashierPurchaseSaveDaimondReturnDAL
    {

       public static readonly string TableName = "Sales_PurchaseInvoiceSaveReturnMaster";
        public static readonly string PremaryKey = "InvoiceID";
        public bool FoundResult;
        public bool NeedSaving;
        public bool IsNewRecord;

        private DataTable dt;
        private string strSQL;
        private object Result;

        public static Sales_PurchaseSaveInvoiceReturnDetails ConvertRowToObj(DataRow dr)
        {
            Sales_PurchaseInvoiceSaveReturnMaster ObjMaster = new Sales_PurchaseInvoiceSaveReturnMaster();
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
            ObjMaster.DiscountOnTotal = Comon.cInt(dr["DiscountOnTotal"].ToString());

            ObjMaster.DebitAccount = Comon.cDbl(dr["DebitAccount"].ToString());
            ObjMaster.CreditAccount = Comon.cDbl(dr["CreditAccount"].ToString());
            ObjMaster.TransportDebitAccount = Comon.cDbl(dr["TransportDebitAccount"].ToString());

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


            Sales_PurchaseSaveInvoiceReturnDetails SaleDetalObject = new Sales_PurchaseSaveInvoiceReturnDetails();
            SaleDetalObject.ID = Comon.cInt(dr["ID"].ToString());
            SaleDetalObject.BarCode = dr["BarCode"].ToString();
            SaleDetalObject.SizeID = Comon.cInt(dr["SizeID"].ToString());
            SaleDetalObject.StoreID = Comon.cInt(dr["StoreID"].ToString());
            SaleDetalObject.ItemID = Comon.cInt(dr["ItemID"].ToString());


            SaleDetalObject.QTY = Comon.cInt(dr["QTY"].ToString());
            SaleDetalObject.SalePrice = Comon.cDec(dr["SalePrice"].ToString());
            SaleDetalObject.Discount = Comon.cDec(dr["Discount"].ToString());
            SaleDetalObject.CostPrice = Comon.cDec(dr["CostPrice"].ToString());
            SaleDetalObject.Description = dr["Description"].ToString();
            //SaleDetalObject.ExpiryDate = Comon.cDec(dr["ExpiryDate"].ToString());
            SaleDetalObject.Serials = dr["Serials"].ToString();

            SaleDetalObject.AdditionalValue = Comon.cInt(dr["AdditionalValue"].ToString());
            SaleDetalObject.PurchaseSaveReturnMaster = ObjMaster;
            return SaleDetalObject;
        }

        public static Sales_PurchaseInvoiceSaveReturnMaster ConvertRowToObjMaster(DataRow dr)
        {


            Sales_PurchaseInvoiceSaveReturnMaster ObjMaster = new Sales_PurchaseInvoiceSaveReturnMaster();
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
            return ObjMaster;
        }



        public static List<Sales_PurchaseSaveInvoiceReturnDetails> GetDataDetalByID(int ID, int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Sales_PurchaseInvoiceSaveReturnMaster_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@InvoiceID", ID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 5));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);

                        if (dt != null)
                        {
                            List<Sales_PurchaseSaveInvoiceReturnDetails> Returned = new List<Sales_PurchaseSaveInvoiceReturnDetails>();
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

        public static Sales_PurchaseInvoiceSaveReturnMaster GetDataMasterByID(int ID, int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Sales_PurchaseInvoiceSaveReturnMaster_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@InvoiceID", ID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 3));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            Sales_PurchaseInvoiceSaveReturnMaster Returned = new Sales_PurchaseInvoiceSaveReturnMaster();
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

        public static List<Sales_PurchaseInvoiceSaveReturnMaster> GetAllMasterData(int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Sales_PurchaseInvoiceSaveReturnMaster_SP]";
                        objCmd.Parameters.AddWithValue("@BranchID", BranchID);
                        objCmd.Parameters.AddWithValue("@FacilityID", FacilityID);

                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 6));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            List<Sales_PurchaseInvoiceSaveReturnMaster> Returned = new List<Sales_PurchaseInvoiceSaveReturnMaster>();
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
        public static string InsertUsingXML(Sales_PurchaseInvoiceSaveReturnMaster objRecord, bool IsNewRecord)
        {
            Int32 objRet = 0;
            string DitmeXML = ConvertObjectToXMLString(objRecord.PurchaseSaveReturnDatails);
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Sales_PurchaseInvoiceSaveReturnMaster_SP]";
                    objCmd.Parameters.Add(new SqlParameter("xmlData", SqlDbType.Xml)).Value = DitmeXML;

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

                    objCmd.Parameters.Add(new SqlParameter("@DebitGoldAccountID", objRecord.DebitGoldAccountID));
                    objCmd.Parameters.Add(new SqlParameter("@CreditGoldAccountID", objRecord.CreditGoldAccountID));

                    objCmd.Parameters.Add(new SqlParameter("@DebitAccount", objRecord.DebitAccount));
                    objCmd.Parameters.Add(new SqlParameter("@CreditAccount", objRecord.CreditAccount));

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
                    objCmd.Parameters.Add(new SqlParameter("@RegistrationNo", objRecord.RegistrationNo));
                    objCmd.Parameters.Add(new SqlParameter("@NetAmount", objRecord.NetAmount));
                    objCmd.Parameters.Add(new SqlParameter("@TransportDebitAmount", objRecord.TransportDebitAmount));
                    objCmd.Parameters.Add(new SqlParameter("@DiscountOnTotal", objRecord.DiscountOnTotal));
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceTotal", objRecord.InvoiceTotal));
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceGoldTotal", objRecord.InvoiceGoldTotal));

                    objCmd.Parameters.Add(new SqlParameter("@AdditionaAmountTotal", objRecord.AdditionaAmountTotal));
                    objCmd.Parameters.Add(new SqlParameter("@NetBalance", objRecord.NetBalance));

                    objCmd.Parameters.Add(new SqlParameter("@VATID", objRecord.VATID));
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceImage", objRecord.InvoiceImage));
                    objCmd.Parameters.Add(new SqlParameter("@GoldUsing", objRecord.GoldUsing));

                    objCmd.Parameters.Add(new SqlParameter("@NetType", objRecord.NetType));
                    SqlParameter pvNewId = new SqlParameter();
                    pvNewId.ParameterName = "@Product_count";
                    pvNewId.DbType = DbType.Int32;
                    pvNewId.Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add(pvNewId);

                    if (IsNewRecord)
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 1));
                    else
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 2));
                    object obj = objCmd.ExecuteScalar();

                    string val = objCmd.Parameters["@Product_count"].Value.ToString();
                    if (Comon.cInt(obj) > 0)
                    {
                        if (val != null)
                            objRet = Convert.ToInt32(val);
                    }
                    else
                        return obj.ToString();

                }
            }
            return objRet.ToString();

        }

        public static Int32 InsertSales_PurchaseInvoiceSaveReturnMaster(Sales_PurchaseInvoiceSaveReturnMaster objRecord)
        {
            Int32 objRet = 0;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Sales_PurchaseInvoiceSaveReturnMaster_SP]";
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

                    objCmd.Parameters.Add(new SqlParameter("@NetProcessID", objRecord.NetProcessID));
                    objCmd.Parameters.Add(new SqlParameter("@CheckID", objRecord.CheckID));
                    objCmd.Parameters.Add(new SqlParameter("@CheckSpendDate", objRecord.CheckSpendDate));
                    objCmd.Parameters.Add(new SqlParameter("@WarningDate", objRecord.WarningDate));
                    objCmd.Parameters.Add(new SqlParameter("@ReceiveDate", objRecord.ReceiveDate));
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

        public static long UpdateUsingXML(Sales_PurchaseInvoiceSaveReturnMaster objRecord, int USERCREATED)
        {
            Int32 objRet = 0;
            string DitmeXML = ConvertObjectToXMLString(objRecord.PurchaseSaveReturnDatails);
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Sales_PurchaseInvoiceSaveReturnMaster_SP]";
                    objCmd.Parameters.Add("@xmlSaleDatial", SqlDbType.Xml, 1500, DitmeXML);
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceID", objRecord.InvoiceID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceDate", objRecord.InvoiceDate));
                    objCmd.Parameters.Add(new SqlParameter("@MethodeID", objRecord.MethodeID));
                    objCmd.Parameters.Add(new SqlParameter("@SupplierID", objRecord.SupplierID));
                    objCmd.Parameters.Add(new SqlParameter("@CostCenterID", objRecord.CostCenterID));
                    objCmd.Parameters.Add(new SqlParameter("@ReceiveDate", objRecord.ReceiveDate));
                    objCmd.Parameters.Add(new SqlParameter("@StoreID", objRecord.StoreID));
                    objCmd.Parameters.Add(new SqlParameter("@DelegateID", objRecord.DelegateID));
                    objCmd.Parameters.Add(new SqlParameter("@Notes", objRecord.Notes));
                    objCmd.Parameters.Add(new SqlParameter("@DiscountOnTotal", objRecord.DiscountOnTotal));
                    objCmd.Parameters.Add(new SqlParameter("@DebitAccount", objRecord.DebitAccount));
                    objCmd.Parameters.Add(new SqlParameter("@CreditAccount", objRecord.CreditAccount));

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

        public static int DeleteSales_PurchaseInvoiceSaveReturnMaster(Sales_PurchaseInvoiceSaveReturnMaster objRecord)
        {

            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Sales_PurchaseInvoiceSaveReturnMaster_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceID", objRecord.InvoiceID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@EditDate", objRecord.EditDate));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 4));
                    SqlParameter pvNewId = new SqlParameter();
                    pvNewId.ParameterName = "@product_count";
                    pvNewId.DbType = DbType.Int32;
                    pvNewId.Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add(pvNewId);

                    object obj = objCmd.ExecuteNonQuery();

                    string val = objCmd.Parameters["@product_count"].Value.ToString();

                    if (val != null)
                        return Convert.ToInt32(val);
                }
            }
            return 0;

        }


        public static SalseInvoicesReport ConvertRowToObjReport(DataRow dr)
        {
            SalseInvoicesReport ObjMaster = new SalseInvoicesReport();
            ObjMaster.InvoiceID = dr["InvoiceID"].ToString();
            ObjMaster.BranchID = dr["BranchID"].ToString();
            ObjMaster.InvoiceDate = dr["InvoiceDate"].ToString();
            ObjMaster.SaleMethod = dr["PurchaseMethod"].ToString();
            ObjMaster.CustomerName = dr["SupplierName"].ToString();
            ObjMaster.CostCenterName = dr["CostCenterName"].ToString();
            ObjMaster.SellerName = dr["PurchaseDelegate"].ToString();
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
                        objCmd.CommandText = "[Sales_PurchaseInvoiceSaveReturnMaster_SP]";
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
        public static long GetNewID(int BranchID)
        {
            try
            {
                long ID = 0;
                DataTable dt;
                string strSQL;

                strSQL = "SELECT Max(ISNULL(" + PremaryKey + ", 0))+1 FROM " + TableName + " Where  BranchID =" + BranchID;
                dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count > 0)
                {
                    ID = Comon.cLong(dt.Rows[0][0].ToString());
                    if (ID == 0)
                        ID = 1;
                }
                strSQL = "Select Top 1 StartFrom From StartNumbering Where BranchID=" + MySession.GlobalBranchID
                    + " And FormName='frmPurchaseReturnInvoice'";
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
                        objCmd.CommandText = "[Sales_PurchaseInvoiceSaveReturnMaster_SP]";
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


    }
}

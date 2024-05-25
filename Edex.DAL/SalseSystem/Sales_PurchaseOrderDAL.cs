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

namespace Edex.DAL.SalseSystem
{
     
    public class Sales_PurchaseOrderDAL
    {


        public static readonly string TableName = "Sales_PurchaseOrderMaster";
        public static readonly string PremaryKey = "OrderID";


        public long OrderID;
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
        public long SupplierOrderID;
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
        public static Sales_PurchaseOrderDetails ConvertRowToObj(DataRow dr)
        {

            Sales_PurchaseOrderMaster ObjMaster = new Sales_PurchaseOrderMaster();
            ObjMaster.OrderID = Comon.cInt(dr["OrderID"].ToString());
            ObjMaster.BranchID = Comon.cInt(dr["BranchID"].ToString());
            ObjMaster.FacilityID = Comon.cInt(dr["FacilityID"].ToString());
            ObjMaster.TransportDebitAmount = Comon.cInt(dr["TransportDebitAmount"].ToString());

            ObjMaster.ReceiveDate = dr["ReceiveDate"].ToString();
            ObjMaster.InvoiceDate = dr["InvoiceDate"].ToString();
        
            ObjMaster.SupplierID = Comon.cDbl(dr["SupplierID"].ToString());
           
     
            ObjMaster.StoreID = Comon.cDbl(dr["StoreID"].ToString());
    
            ObjMaster.Notes = dr["Notes"].ToString();
            
            
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
            
            ObjMaster.Mobile = dr["Mobile"].ToString();


            Sales_PurchaseOrderDetails SaleDetalObject = new Sales_PurchaseOrderDetails();
            SaleDetalObject.ID = Comon.cInt(dr["ID"].ToString());
            SaleDetalObject.BarCode = dr["BarCode"].ToString();
            SaleDetalObject.SizeID = Comon.cInt(dr["SizeID"].ToString());
            SaleDetalObject.StoreID = Comon.cDbl(dr["StoreID"].ToString());
            SaleDetalObject.ItemID = Comon.cInt(dr["ItemID"].ToString());
            SaleDetalObject.ArbItemName = dr["ItemName"].ToString();
            SaleDetalObject.ArbSizeName = dr["SizeName"].ToString();
            SaleDetalObject.QTY = Comon.cDec(dr["QTY"].ToString());
          
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

     
            SaleDetalObject.PurchaseMaster = ObjMaster;
            return SaleDetalObject;
        }

        public static Sales_PurchaseOrderMaster ConvertRowToObjMaster(DataRow dr)
        {
            Sales_PurchaseOrderMaster ObjMaster = new Sales_PurchaseOrderMaster();
            ObjMaster.OrderID = Comon.cInt(dr["OrderID"].ToString());
            ObjMaster.BranchID = Comon.cInt(dr["BranchID"].ToString());
            ObjMaster.InvoiceDate = Comon.ConvertSerialDateTo(dr["InvoiceDate"].ToString());
           
            ObjMaster.SupplierID = Comon.cInt(dr["SupplierID"].ToString());
          
            ObjMaster.StoreID = Comon.cDbl(dr["StoreID"].ToString());
         
            ObjMaster.Notes = dr["Notes"].ToString();
     
            ObjMaster.NetProcessID = dr["NetProcessID"].ToString();
            ObjMaster.CheckID = dr["CheckID"].ToString();
          
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
         
            ObjMaster.Mobile = dr["Mobile"].ToString();
            return ObjMaster;
        }

        public static List<Sales_PurchaseOrderDetails> GetDataDetalByID(int ID, int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Sales_PurchaseOrderMaster_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@OrderID", ID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 5));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            List<Sales_PurchaseOrderDetails> Returned = new List<Sales_PurchaseOrderDetails>();
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

        public static BindingList<Sales_PurchaseOrderDetails> GetDataDetalByID_BindingList(int ID, int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Sales_PurchaseOrderMaster_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@OrderID", ID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 5));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);

                        if (dt != null)
                        {
                            BindingList<Sales_PurchaseOrderDetails> Returned = new BindingList<Sales_PurchaseOrderDetails>();
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
                        objCmd.CommandText = "[Sales_PurchaseOrderMaster_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@OrderID", ID));
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
            catch (Exception)
            {
                return null;
            }
        }

        public static Sales_PurchaseOrderMaster GetDataMasterByID(int ID, int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Sales_PurchaseOrderMaster_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@OrderID", ID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 3));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            Sales_PurchaseOrderMaster Returned = new Sales_PurchaseOrderMaster();
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
        public static List<Sales_PurchaseOrderMaster> GetAllMasterData(int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Sales_PurchaseOrderMaster_SP]";
                        objCmd.Parameters.AddWithValue("@BranchID", BranchID);
                        objCmd.Parameters.AddWithValue("@FacilityID", FacilityID);

                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 7));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            List<Sales_PurchaseOrderMaster> Returned = new List<Sales_PurchaseOrderMaster>();
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
        public static string InsertUsingXML(Sales_PurchaseOrderMaster objRecord, int USERCREATED, Boolean IsNewrecord)
        {
            string objRet = "";
            string DitmeXML = ConvertObjectToXMLString(objRecord.PurchaseOrderDatails);
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Sales_PurchaseOrderMaster_SP]";
                    objCmd.Parameters.Add(new SqlParameter("xmlData", SqlDbType.Xml)).Value = DitmeXML;
                    objCmd.Parameters.Add(new SqlParameter("@OrderID", objRecord.OrderID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceDate", Comon.cInt(objRecord.InvoiceDate)));
                    objCmd.Parameters.Add(new SqlParameter("@ReceiveDate", Comon.cInt(objRecord.ReceiveDate)));
                   
                    objCmd.Parameters.Add(new SqlParameter("@CurrencyID", objRecord.CurencyID));
                    objCmd.Parameters.Add(new SqlParameter("@CurrencyEquivalent", objRecord.CurrencyEquivalent));
                    objCmd.Parameters.Add(new SqlParameter("@CurrencyName", objRecord.CurrencyName));
                    objCmd.Parameters.Add(new SqlParameter("@CurrencyPrice", objRecord.CurrencyPrice));
                    objCmd.Parameters.Add(new SqlParameter("@SupplierID", objRecord.SupplierID));
                 
                    objCmd.Parameters.Add(new SqlParameter("@StoreID", objRecord.StoreID)); 
                    objCmd.Parameters.Add(new SqlParameter("@Notes", objRecord.Notes));  
                  
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceEquivalenTotal", objRecord.InvoiceEquivalenTotal));
           
                    objCmd.Parameters.Add(new SqlParameter("@TransportDebitAmount", objRecord.TransportDebitAmount));
                     
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
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceTotal", objRecord.InvoiceTotal)); 
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceImage", objRecord.InvoiceImage));
                    objCmd.Parameters.Add(new SqlParameter("@OperationTypeName", objRecord.OperationTypeName)); 
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceDiamondTotal", objRecord.InvoiceDiamondTotal));
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

        public static string InsertUsingSaveXML(Sales_PurchaseOrderMaster objRecord, int USERCREATED, Boolean IsNewrecord)
        {
            string objRet = "";
            string DitmeXML = ConvertObjectToXMLString(objRecord.PurchaseOrderDatails);
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Sales_PurchaseOrderMaster_SP]";
                    objCmd.Parameters.Add(new SqlParameter("xmlData", SqlDbType.Xml)).Value = DitmeXML;
                    objCmd.Parameters.Add(new SqlParameter("@OrderID", objRecord.OrderID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceDate", Comon.cInt(objRecord.InvoiceDate)));
                    objCmd.Parameters.Add(new SqlParameter("@ReceiveDate", Comon.cInt(objRecord.ReceiveDate)));
                
                    objCmd.Parameters.Add(new SqlParameter("@CurrencyID", 1));
                    objCmd.Parameters.Add(new SqlParameter("@SupplierID", objRecord.SupplierID)); 
                    objCmd.Parameters.Add(new SqlParameter("@StoreID", objRecord.StoreID));
      
                    objCmd.Parameters.Add(new SqlParameter("@Notes", objRecord.Notes)); 
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceEquivalenTotal", objRecord.InvoiceEquivalenTotal)); 
                    objCmd.Parameters.Add(new SqlParameter("@TransportDebitAmount", objRecord.TransportDebitAmount)); 
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
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceTotal", objRecord.InvoiceTotal));   
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceImage", objRecord.InvoiceImage));
                    objCmd.Parameters.Add(new SqlParameter("@OperationTypeName", objRecord.OperationTypeName)); 
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceDiamondTotal", objRecord.InvoiceDiamondTotal));
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

        public static Int32 InsertSales_PurchaseOrderMaster(Sales_PurchaseOrderMaster objRecord)
        {
            Int32 objRet = 0;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Sales_PurchaseOrderMaster_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@OrderID", objRecord.OrderID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceDate", objRecord.InvoiceDate)); 
                    objCmd.Parameters.Add(new SqlParameter("@SupplierID", objRecord.SupplierID)); 
                    objCmd.Parameters.Add(new SqlParameter("@StoreID", objRecord.StoreID)); 
                    objCmd.Parameters.Add(new SqlParameter("@Notes", objRecord.Notes));  
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
                     
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 1));
                    object obj = objCmd.ExecuteScalar();
                    if (obj != null)
                        objRet = Convert.ToInt32(obj);
                }
            }
            return objRet;
        }

        public static long UpdateUsingXML(Sales_PurchaseOrderMaster objRecord, int USERCREATED)
        {
            Int32 objRet = 0;
            string DitmeXML = ConvertObjectToXMLString(objRecord.PurchaseOrderDatails);
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Sales_PurchaseOrderMaster_SP]";
                    objCmd.Parameters.Add("@xmlSaleDatial", SqlDbType.Xml, 1500, DitmeXML);
                    objCmd.Parameters.Add(new SqlParameter("@OrderID", objRecord.OrderID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceDate", objRecord.InvoiceDate)); 
                    objCmd.Parameters.Add(new SqlParameter("@SupplierID", objRecord.SupplierID)); 

                    objCmd.Parameters.Add(new SqlParameter("@StoreID", objRecord.StoreID)); 
                    objCmd.Parameters.Add(new SqlParameter("@Notes", objRecord.Notes)); 
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
                     
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 1));
                    object obj = objCmd.ExecuteScalar();
                    if (obj != null)
                        objRet = Convert.ToInt32(obj);
                }
            }
            return objRet;

        }

        public static int DeleteSales_PurchaseOrderMaster(Sales_PurchaseOrderMaster objRecord)
        {

            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Sales_PurchaseOrderMaster_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@OrderID", objRecord.OrderID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
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


        public static SalseInvoicesReport ConvertRowToObjReport1(DataRow dr)
        {
            SalseInvoicesReport ObjMaster = new SalseInvoicesReport();
            ObjMaster.InvoiceID = dr["OrderID"].ToString();
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
            ObjMaster.InvoiceID = dr["OrderID"].ToString();
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
                        objCmd.CommandText = "[Sales_PurchaseOrderMaster_SP]";
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
        public static long InsertUsingXML_GoodOpening(Sales_PurchaseOrderMaster objRecord, int USERCREATED, Boolean IsNewrecord)
        {
            Int32 objRet = 0;
            string DitmeXML = ConvertObjectToXMLString(objRecord.PurchaseOrderDatails);
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Stc_GoodOpening_SP]";
                    objCmd.Parameters.Add(new SqlParameter("xmlData", SqlDbType.Xml)).Value = DitmeXML;
                    // objCmd.Parameters.Add("@xmlSaleDatial", SqlDbType.Xml, 1500, DitmeXML);
                    objCmd.Parameters.Add(new SqlParameter("@OrderID", objRecord.OrderID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceDate", objRecord.InvoiceDate));
                    objCmd.Parameters.Add(new SqlParameter("@ReceiveDate", objRecord.ReceiveDate)); 
                    objCmd.Parameters.Add(new SqlParameter("@CurencyID", objRecord.CurencyID));
                    objCmd.Parameters.Add(new SqlParameter("@SupplierID", objRecord.SupplierID));  
                    objCmd.Parameters.Add(new SqlParameter("@StoreID", objRecord.StoreID)); 
                    objCmd.Parameters.Add(new SqlParameter("@Notes", objRecord.Notes));
                     
                


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
                     
                    objCmd.Parameters.Add(new SqlParameter("@TransportDebitAmount", objRecord.TransportDebitAmount)); 
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceTotal", objRecord.InvoiceTotal));   
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceImage", objRecord.InvoiceImage));
                     


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
                        objCmd.Parameters.Add(new SqlParameter("@OrderID", ID));
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
                strSQL = " SELECT  Sales_PurchaseOrderDetails.HavVat, Sales_PurchaseOrderDetails.BarCode, Sales_PurchaseOrderDetails.Serials, Sales_PurchaseOrderDetails.ItemID, "
                 + " Stc_Items.ArbName AS ItemName, Sales_PurchaseOrderDetails.SizeID, Stc_SizingUnits.ArbName AS SizeName, Sales_PurchaseOrderDetails.QTY, "
                 + " Sales_PurchaseOrderDetails.CostPrice, Sales_PurchaseOrderDetails.AdditionaAmmount, Sales_PurchaseOrderDetails.Discount, "
                 + " Sales_PurchaseOrderDetails.Caliber, Sales_PurchaseOrderDetails.Equivalen, Sales_PurchaseOrderDetails.ExpiryDate, Sales_PurchaseOrderDetails.Bones, "
                 + " Sales_PurchaseOrderDetails.SalePrice, Sales_PurchaseOrderDetails.BarCode AS Expr1, Stc_ItemsBrands.ArbName AS BrandName, "
                 + " Stc_ItemsSizes.ArbName AS Size FROM  Stc_ItemsBrands RIGHT OUTER JOIN   Stc_Items ON Stc_ItemsBrands.BrandID = Stc_Items.BrandID LEFT OUTER JOIN "
                 + " Stc_ItemsSizes ON Stc_Items.SizeID = Stc_ItemsSizes.SizeID RIGHT OUTER JOIN    Sales_PurchaseOrderDetails INNER JOIN "
                 + " Sales_PurchaseOrderMaster ON Sales_PurchaseOrderDetails.OrderID = Sales_PurchaseOrderMaster.OrderID AND "
                 + " Sales_PurchaseOrderDetails.BranchID = Sales_PurchaseOrderMaster.BranchID LEFT OUTER JOIN "
                 + " Stc_SizingUnits ON Sales_PurchaseOrderDetails.SizeID = Stc_SizingUnits.SizeID ON Stc_Items.ItemID = Sales_PurchaseOrderDetails.ItemID "
                 + " WHERE  (Sales_PurchaseOrderMaster.Cancel = 0) AND (Sales_PurchaseOrderDetails.Cancel = 0) AND (Sales_PurchaseOrderMaster.OrderID = " + Id + ") AND "
                 + " (Sales_PurchaseOrderMaster.BranchID = " + 1 + ") ORDER BY Sales_PurchaseOrderDetails.ID";

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
                 + " Stc_GoodOpeningMaster ON Stc_GoodOpeningDetails.OrderID = Stc_GoodOpeningMaster.OrderID AND "
                 + " Stc_GoodOpeningDetails.BranchID = Stc_GoodOpeningMaster.BranchID LEFT OUTER JOIN "
                 + " Stc_SizingUnits ON Stc_GoodOpeningDetails.SizeID = Stc_SizingUnits.SizeID ON Stc_Items.ItemID = Stc_GoodOpeningDetails.ItemID "
                 + " WHERE  (Stc_GoodOpeningMaster.Cancel = 0) AND (Sales_PurchaseOrderDetails.Cancel = 0) AND (Stc_GoodOpeningMaster.OrderID = " + Id + ") AND "
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


    }
}

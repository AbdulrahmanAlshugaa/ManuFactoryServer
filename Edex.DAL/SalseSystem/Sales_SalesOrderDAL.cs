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
     
    public class Sales_SalesOrderDAL
    {
        public static readonly string TableName = "Sales_SalesOrderMaster";
        public static readonly string PremaryKey = "OrderID";
        public bool FoundResult;
        private DataTable dt;
        public static Sales_SalesOrderDetails ConvertRowToObj(DataRow dr)
        {

            Sales_SalesOrderMaster ObjMaster = new Sales_SalesOrderMaster();
            ObjMaster.OrderID = Comon.cInt(dr["OrderID"].ToString());
            ObjMaster.BranchID = Comon.cInt(dr["BranchID"].ToString());
            ObjMaster.FacilityID = Comon.cInt(dr["FacilityID"].ToString());


            ObjMaster.InvoiceDate = dr["InvoiceDate"].ToString(); 
            ObjMaster.CustomerID = Comon.cDbl(dr["CustomerID"].ToString());
            ObjMaster.CostCenterID = Comon.cInt(dr["CostCenterID"].ToString());
            ObjMaster.SellerID = Comon.cInt(dr["SellerID"].ToString());
            ObjMaster.StoreID = Comon.cDbl(dr["StoreID"].ToString());
            ObjMaster.DelegateID = Comon.cInt(dr["DelegateID"].ToString());
            ObjMaster.Notes = dr["Notes"].ToString();
            ObjMaster.DiscountOnTotal = Comon.cDec(dr["DiscountOnTotal"].ToString()); 
            ObjMaster.FromCashierScreen = Comon.cInt(dr["FromCashierScreen"].ToString());
            ObjMaster.CloseCashier = Comon.cInt(dr["CloseCashier"].ToString());
            ObjMaster.CloseCashierDate = Comon.cDbl(dr["CloseCashierDate"].ToString());
            ObjMaster.NetProcessID = dr["NetProcessID"].ToString();
            ObjMaster.CheckID = dr["CheckID"].ToString();
            ObjMaster.CheckAccount = Comon.cDbl(dr["CheckAccount"].ToString());
            ObjMaster.CurrencyID = Comon.cInt(dr["CurencyID"].ToString());

            ObjMaster.CheckSpendDate = dr["CheckSpendDate"].ToString();
            ObjMaster.WarningDate = dr["WarningDate"].ToString(); 
            ObjMaster.OrderType = dr["OrderType"].ToString();
            ObjMaster.SectionID = Comon.cInt(dr["SectionID"].ToString());
            ObjMaster.TableID = Comon.cInt(dr["TableID"].ToString());
            ObjMaster.NeedReview = Comon.cInt(dr["NeedReview"].ToString());
            ObjMaster.ReviewType = dr["ReviewType"].ToString();
            ObjMaster.IsSendReview = Comon.cInt(dr["IsSendReview"].ToString());
            ObjMaster.WorkDetails = dr["WorkDetails"].ToString();
            ObjMaster.Status = dr["Status"].ToString();
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
            ObjMaster.EmployeeID = Comon.cInt(dr["EmployeeID"].ToString());
            ObjMaster.PateintID = Comon.cInt(dr["PateintID"].ToString());
            ObjMaster.TempInvoiceID = Comon.cInt(dr["TempInvoiceID"].ToString());
            ObjMaster.EnduranceRatio = Comon.cInt(dr["EnduranceRatio"].ToString()); 

            Sales_SalesOrderDetails SaleDetailObject = new Sales_SalesOrderDetails();
            SaleDetailObject.ID = Comon.cInt(dr["ID"].ToString());
            SaleDetailObject.BarCode = dr["BarCode"].ToString();
            SaleDetailObject.SizeID = Comon.cInt(dr["SizeID"].ToString());
            SaleDetailObject.StoreID = Comon.cDbl(dr["StoreID"].ToString());
            SaleDetailObject.ItemID = Comon.cInt(dr["ItemID"].ToString());
            SaleDetailObject.ArbItemName = dr["ItemName"].ToString();
            SaleDetailObject.ArbSizeName = dr["SizeName"].ToString();

            SaleDetailObject.QTY = Comon.cDec(dr["QTY"].ToString());
            SaleDetailObject.SalePrice = Comon.cDec(dr["SalePrice"].ToString());
            SaleDetailObject.Discount = Comon.cDec(dr["Discount"].ToString());
            SaleDetailObject.CostPrice = Comon.cDec(dr["CostPrice"].ToString());
            SaleDetailObject.Description = dr["Description"].ToString();
            SaleDetailObject.ExpiryDateStr = Comon.cLong(dr["ExpiryDate"].ToString());
            SaleDetailObject.Serials = dr["Serials"].ToString();
            SaleDetailObject.Height = Comon.cInt(dr["Height"].ToString());
            SaleDetailObject.Width = Comon.cInt(dr["Width"].ToString());
            SaleDetailObject.TheCount = Comon.cInt(dr["TheCount"].ToString());
            SaleDetailObject.AdditionalValue = Comon.cDec(dr["AdditionalValue"].ToString());

            SaleDetailObject.Net = Comon.cDec(dr["Net"].ToString());




            SaleDetailObject.SaleMaster = ObjMaster;
            return SaleDetailObject;
        }

        public static Sales_SalesOrderMaster ConvertRowToObjMaster(DataRow dr)
        {


            Sales_SalesOrderMaster ObjMaster = new Sales_SalesOrderMaster();
            ObjMaster.OrderID = Comon.cInt(dr["OrderID"].ToString());
            ObjMaster.BranchID = Comon.cInt(dr["BranchID"].ToString());
            ObjMaster.InvoiceDate = Comon.ConvertSerialDateTo(dr["InvoiceDate"].ToString());

            
            ObjMaster.CustomerID = Comon.cInt(dr["CustomerID"].ToString());
            ObjMaster.CostCenterID = Comon.cInt(dr["CostCenterID"].ToString());
            ObjMaster.SellerID = Comon.cInt(dr["SellerID"].ToString());
            ObjMaster.StoreID = Comon.cDbl(dr["StoreID"].ToString());
            ObjMaster.DelegateID = Comon.cInt(dr["DelegateID"].ToString());
            ObjMaster.Notes = dr["Notes"].ToString();
            ObjMaster.DiscountOnTotal = Comon.cDec(dr["DiscountOnTotal"].ToString());
          
            ObjMaster.FromCashierScreen = Comon.cInt(dr["FromCashierScreen"].ToString());
            ObjMaster.CloseCashier = Comon.cInt(dr["CloseCashier"].ToString());
            ObjMaster.CloseCashierDate = Comon.cInt(dr["CloseCashierDate"].ToString());
            ObjMaster.NetProcessID = dr["NetProcessID"].ToString();
            ObjMaster.CheckID = dr["CheckID"].ToString();
            ObjMaster.CheckAccount = Comon.cInt(dr["CheckAccount"].ToString());

            ObjMaster.CheckSpendDate = Comon.ConvertSerialDateTo(dr["CheckSpendDate"].ToString());
            ObjMaster.WarningDate = Comon.ConvertSerialDateTo(dr["WarningDate"].ToString());
         
            ObjMaster.OrderType = dr["OrderType"].ToString();
            ObjMaster.SectionID = Comon.cInt(dr["SectionID"].ToString());
            ObjMaster.TableID = Comon.cInt(dr["TableID"].ToString());
            ObjMaster.NeedReview = Comon.cInt(dr["NeedReview"].ToString());
            ObjMaster.ReviewType = dr["ReviewType"].ToString();
            ObjMaster.IsSendReview = Comon.cInt(dr["IsSendReview"].ToString());
            ObjMaster.WorkDetails = dr["WorkDetails"].ToString();
            ObjMaster.Status = dr["Status"].ToString();
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
            ObjMaster.EmployeeID = Comon.cInt(dr["EmployeeID"].ToString());
            ObjMaster.PateintID = Comon.cInt(dr["PateintID"].ToString());
            ObjMaster.TempInvoiceID = Comon.cInt(dr["TempInvoiceID"].ToString());
            ObjMaster.EnduranceRatio = Comon.cInt(dr["EnduranceRatio"].ToString());
          
            return ObjMaster;
        }

        public static Sales_SalesOrderMaster ConvertRowToObjMasterForShowAllRecords(DataRow dr)
        {


            Sales_SalesOrderMaster ObjMaster = new Sales_SalesOrderMaster();
            ObjMaster.OrderID = Comon.cInt(dr["OrderID"].ToString());
            ObjMaster.BranchID = Comon.cInt(dr["BranchID"].ToString());
            ObjMaster.InvoiceDate = dr["InvoiceDate"].ToString(); 
            ObjMaster.CustomerName = dr["CustomerName"].ToString();
            ObjMaster.CostCenterName = dr["CostCenterName"].ToString();
            ObjMaster.StoreName = dr["StoreName"].ToString();
            ObjMaster.NetBalance = Comon.cDec(dr["NetBalance"].ToString());


            return ObjMaster;
        }
        public static List<Sales_SalesOrderDetails> GetDataDetailByID(int ID, int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Sales_SalesOrderMaster_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@OrderID", ID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 5));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);

                        if (dt != null)
                        {
                            List<Sales_SalesOrderDetails> Returned = new List<Sales_SalesOrderDetails>();
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

        public static Sales_SalesOrderMaster GetDataMasterByID(int ID, int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Sales_SalesOrderMaster_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@OrderID", ID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 3));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            Sales_SalesOrderMaster Returned = new Sales_SalesOrderMaster();
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

        public static List<Sales_SalesOrderMaster> GetAllMasterData(int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Sales_SalesOrderMaster_SP]";
                        objCmd.Parameters.AddWithValue("@BranchID", BranchID);
                        objCmd.Parameters.AddWithValue("@FacilityID", FacilityID);

                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 7));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            List<Sales_SalesOrderMaster> Returned = new List<Sales_SalesOrderMaster>();
                            foreach (DataRow rows in dt.Rows)
                                Returned.Add(ConvertRowToObjMasterForShowAllRecords(rows));
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

        public static string InsertUsingXML(Sales_SalesOrderMaster objRecord, bool IsNewRecord)
        {
            string objRet = "";
            string DitmeXML = ConvertObjectToXMLString(objRecord.SaleDatails);
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Sales_SalesOrderMaster_SP]";
                    objCmd.Parameters.Add(new SqlParameter("xmlData", SqlDbType.Xml)).Value = DitmeXML;
                    objCmd.Parameters.Add(new SqlParameter("@OrderID", objRecord.OrderID)); 
                    objCmd.Parameters.Add(new SqlParameter("@CustomerName", objRecord.CustomerName));
                    objCmd.Parameters.Add(new SqlParameter("@CustomerID", objRecord.CustomerID));
                    objCmd.Parameters.Add(new SqlParameter("@CostCenterID", objRecord.CostCenterID));
                    objCmd.Parameters.Add(new SqlParameter("@SellerID", objRecord.SellerID));
                    objCmd.Parameters.Add(new SqlParameter("@StoreID", objRecord.StoreID));
                    objCmd.Parameters.Add(new SqlParameter("@DelegateID", objRecord.DelegateID));
                    objCmd.Parameters.Add(new SqlParameter("@Notes", objRecord.Notes));
                    objCmd.Parameters.Add(new SqlParameter("@VATID", objRecord.VATID));
                    objCmd.Parameters.Add(new SqlParameter("@DocumentID", objRecord.DocumentID));
                    objCmd.Parameters.Add(new SqlParameter("@CurrencyID", objRecord.CurrencyID));

                    objCmd.Parameters.Add(new SqlParameter("@CurrencyEquivalent", objRecord.CurrencyEquivalent));
                    objCmd.Parameters.Add(new SqlParameter("@CurrencyName", objRecord.CurrencyName));
                     objCmd.Parameters.Add(new SqlParameter("@CurrencyPrice", objRecord.CurrencyPrice));
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceDate", objRecord.InvoiceDate));
                    objCmd.Parameters.Add(new SqlParameter("@CheckSpendDate", objRecord.CheckSpendDate));
                    objCmd.Parameters.Add(new SqlParameter("@WarningDate", objRecord.WarningDate));

                    objCmd.Parameters.Add(new SqlParameter("@NetProcessID", objRecord.NetProcessID));
                    objCmd.Parameters.Add(new SqlParameter("@CheckID", objRecord.CheckID));

                    objCmd.Parameters.Add(new SqlParameter("@FromCashierScreen", objRecord.FromCashierScreen));
                    objCmd.Parameters.Add(new SqlParameter("@CloseCashier", objRecord.CloseCashier));
                    objCmd.Parameters.Add(new SqlParameter("@CloseCashierDate", objRecord.CloseCashierDate));
                     
                    objCmd.Parameters.Add(new SqlParameter("@OrderType", objRecord.OrderType));
                    objCmd.Parameters.Add(new SqlParameter("@SectionID", objRecord.SectionID));
                    objCmd.Parameters.Add(new SqlParameter("@TableID", objRecord.TableID));
                    objCmd.Parameters.Add(new SqlParameter("@NeedReview", objRecord.NeedReview));
                    objCmd.Parameters.Add(new SqlParameter("@ReviewType", objRecord.ReviewType));
                    objCmd.Parameters.Add(new SqlParameter("@IsSendReview", objRecord.IsSendReview));
                    objCmd.Parameters.Add(new SqlParameter("@WorkDetails", objRecord.WorkDetails));
                    objCmd.Parameters.Add(new SqlParameter("@Status", objRecord.Status));
                     

                     
                    objCmd.Parameters.Add(new SqlParameter("@CheckAccount", objRecord.CheckAccount)); 

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

                    objCmd.Parameters.Add(new SqlParameter("@EmployeeID", objRecord.EmployeeID));
                    objCmd.Parameters.Add(new SqlParameter("@PateintID", objRecord.PateintID));
                    objCmd.Parameters.Add(new SqlParameter("@TempOrderID", objRecord.TempInvoiceID));
                    objCmd.Parameters.Add(new SqlParameter("@EnduranceRatio", objRecord.EnduranceRatio));
                    objCmd.Parameters.Add(new SqlParameter("@RegistrationNo", objRecord.RegistrationNo));
                    objCmd.Parameters.Add(new SqlParameter("@NetBalance", objRecord.NetBalance));
                    objCmd.Parameters.Add(new SqlParameter("@NetType", objRecord.NetType));
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceTotal", objRecord.InvoiceTotal));
                    objCmd.Parameters.Add(new SqlParameter("@AdditionaAmountTotal", objRecord.AdditionaAmountTotal));
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceGoldTotal", objRecord.InvoiceGoldTotal));
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceEquivalenTotal", objRecord.InvoiceEquivalenTotal));
                    objCmd.Parameters.Add(new SqlParameter("@WeightGoldTotal", objRecord.InvoiceGoldTotal));
                    objCmd.Parameters.Add(new SqlParameter("@DiscountOnTotal", objRecord.DiscountOnTotal));
                    objCmd.Parameters.Add(new SqlParameter("@OperationTypeName", objRecord.OperationTypeName));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@RemaindAmount", objRecord.RemaindAmount));
                    objCmd.Parameters.Add(new SqlParameter("@CustomerMobile", objRecord.CustomerMobile));
                    objCmd.Parameters.Add(new SqlParameter("@TypeGold", objRecord.TypeGold));
                    objCmd.Parameters.Add(new SqlParameter("@GoldUsing", objRecord.GoldUsing));
                     
                    SqlParameter pvNewId = new SqlParameter();
                    pvNewId.ParameterName = "@product_count";
                    pvNewId.DbType = DbType.Int32;
                    pvNewId.Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add(pvNewId);

                    if (IsNewRecord)
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 1));
                    else
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 2));
                    object obj = objCmd.ExecuteScalar();
                    string val = objCmd.Parameters["@product_count"].Value.ToString();
                    if (val != null)
                        objRet = Convert.ToString(val);
                }
            }
            return objRet;
        }
        public static Int32 InsertSales_SalesOrderMaster(Sales_SalesOrderMaster objRecord)
        {
            Int32 objRet = 0;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Sales_SalesOrderMaster_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@OrderID", objRecord.OrderID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceDate", objRecord.InvoiceDate)); 
                    objCmd.Parameters.Add(new SqlParameter("@CustomerID", objRecord.CustomerID));
                    objCmd.Parameters.Add(new SqlParameter("@CostCenterID", objRecord.CostCenterID));
                    objCmd.Parameters.Add(new SqlParameter("@SellerID", objRecord.SellerID));
                    objCmd.Parameters.Add(new SqlParameter("@StoreID", objRecord.StoreID));
                    objCmd.Parameters.Add(new SqlParameter("@DelegateID", objRecord.DelegateID));
                    objCmd.Parameters.Add(new SqlParameter("@Notes", objRecord.Notes));
                    objCmd.Parameters.Add(new SqlParameter("@DiscountOnTotal", objRecord.DiscountOnTotal)); 
                    objCmd.Parameters.Add(new SqlParameter("@FromCashierScreen", objRecord.FromCashierScreen));
                    objCmd.Parameters.Add(new SqlParameter("@CloseCashier", objRecord.CloseCashier));
                    objCmd.Parameters.Add(new SqlParameter("@CloseCashierDate", objRecord.CloseCashierDate));
                    objCmd.Parameters.Add(new SqlParameter("@NetProcessID", objRecord.NetProcessID));
                    objCmd.Parameters.Add(new SqlParameter("@CheckID", objRecord.CheckID));
                    objCmd.Parameters.Add(new SqlParameter("@CheckSpendDate", objRecord.CheckSpendDate));
                    objCmd.Parameters.Add(new SqlParameter("@WarningDate", objRecord.WarningDate)); 
                    objCmd.Parameters.Add(new SqlParameter("@OrderType", objRecord.OrderType));
                    objCmd.Parameters.Add(new SqlParameter("@SectionID", objRecord.SectionID));
                    objCmd.Parameters.Add(new SqlParameter("@TableID", objRecord.TableID));
                    objCmd.Parameters.Add(new SqlParameter("@NeedReview", objRecord.NeedReview));
                    objCmd.Parameters.Add(new SqlParameter("@ReviewType", objRecord.ReviewType));
                    objCmd.Parameters.Add(new SqlParameter("@IsSendReview", objRecord.IsSendReview));
                    objCmd.Parameters.Add(new SqlParameter("@WorkDetails", objRecord.WorkDetails));
                    objCmd.Parameters.Add(new SqlParameter("@Status", objRecord.Status));
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
                    objCmd.Parameters.Add(new SqlParameter("@EmployeeID", objRecord.EmployeeID));
                    objCmd.Parameters.Add(new SqlParameter("@PateintID", objRecord.PateintID));
                    objCmd.Parameters.Add(new SqlParameter("@TempInvoiceID", objRecord.TempInvoiceID));
                    objCmd.Parameters.Add(new SqlParameter("@EnduranceRatio", objRecord.EnduranceRatio)); 
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 1));
                    object obj = objCmd.ExecuteScalar();
                    if (obj != null)
                        objRet = Convert.ToInt32(obj);
                }
            }
            return objRet;
        }
        public static long UpdateUsingXML(Sales_SalesOrderMaster objRecord, int USERCREATED)
        {
            Int32 objRet = 0;
            string DitmeXML = ConvertObjectToXMLString(objRecord.SaleDatails);
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Sales_SalesOrderMaster_SP]";
                    objCmd.Parameters.Add("@xmlSaleDatial", SqlDbType.Xml, 1500, DitmeXML);
                    objCmd.Parameters.Add(new SqlParameter("@OrderID", objRecord.OrderID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceDate", objRecord.InvoiceDate)); 
                    objCmd.Parameters.Add(new SqlParameter("@CustomerID", objRecord.CustomerID));
                    objCmd.Parameters.Add(new SqlParameter("@CostCenterID", objRecord.CostCenterID));
                    objCmd.Parameters.Add(new SqlParameter("@SellerID", objRecord.SellerID));
                    objCmd.Parameters.Add(new SqlParameter("@StoreID", objRecord.StoreID));
                    objCmd.Parameters.Add(new SqlParameter("@DelegateID", objRecord.DelegateID));
                    objCmd.Parameters.Add(new SqlParameter("@Notes", objRecord.Notes));
                    objCmd.Parameters.Add(new SqlParameter("@DiscountOnTotal", objRecord.DiscountOnTotal)); 
                    objCmd.Parameters.Add(new SqlParameter("@FromCashierScreen", objRecord.FromCashierScreen));
                    objCmd.Parameters.Add(new SqlParameter("@CloseCashier", objRecord.CloseCashier));
                    objCmd.Parameters.Add(new SqlParameter("@CloseCashierDate", objRecord.CloseCashierDate));
                    objCmd.Parameters.Add(new SqlParameter("@NetProcessID", objRecord.NetProcessID));
                    objCmd.Parameters.Add(new SqlParameter("@CheckID", objRecord.CheckID));
                    objCmd.Parameters.Add(new SqlParameter("@CheckSpendDate", objRecord.CheckSpendDate));
                    objCmd.Parameters.Add(new SqlParameter("@WarningDate", objRecord.WarningDate)); 
                    objCmd.Parameters.Add(new SqlParameter("@OrderType", objRecord.OrderType));
                    objCmd.Parameters.Add(new SqlParameter("@SectionID", objRecord.SectionID));
                    objCmd.Parameters.Add(new SqlParameter("@TableID", objRecord.TableID));
                    objCmd.Parameters.Add(new SqlParameter("@NeedReview", objRecord.NeedReview));
                    objCmd.Parameters.Add(new SqlParameter("@ReviewType", objRecord.ReviewType));
                    objCmd.Parameters.Add(new SqlParameter("@IsSendReview", objRecord.IsSendReview));
                    objCmd.Parameters.Add(new SqlParameter("@WorkDetails", objRecord.WorkDetails));
                    objCmd.Parameters.Add(new SqlParameter("@Status", objRecord.Status));
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
                    objCmd.Parameters.Add(new SqlParameter("@EmployeeID", objRecord.EmployeeID));
                    objCmd.Parameters.Add(new SqlParameter("@PateintID", objRecord.PateintID));
                    objCmd.Parameters.Add(new SqlParameter("@TempInvoiceID", objRecord.TempInvoiceID));
                    objCmd.Parameters.Add(new SqlParameter("@EnduranceRatio", objRecord.EnduranceRatio)); 
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 1));
                    object obj = objCmd.ExecuteScalar();
                    if (obj != null)
                        objRet = Convert.ToInt32(obj);
                }
            }
            return objRet;

        }
        public static string DeleteSales_SalesOrderMaster(Sales_SalesOrderMaster objRecord)
        {
            string objRet = "";
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Sales_SalesOrderMaster_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@OrderID", objRecord.OrderID));
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
                    object obj = objCmd.ExecuteScalar();
                    if (obj != null)
                        objRet = Convert.ToString(obj);
                }
            }
            return objRet;

        }
        /////////////reports////////////

        public static SalseInvoicesReport ConvertRowToObjReport(DataRow dr)
        {
            SalseInvoicesReport ObjMaster = new SalseInvoicesReport();
            ObjMaster.InvoiceID = dr["OrderID"].ToString();
            ObjMaster.BranchID = dr["BranchID"].ToString();
            ObjMaster.InvoiceDate = dr["InvoiceDate"].ToString();
            ObjMaster.SaleMethod = dr["SaleMethod"].ToString();
            ObjMaster.CustomerName = dr["CustomerName"].ToString();
            ObjMaster.CostCenterName = dr["CostCenterName"].ToString();
            ObjMaster.SellerName = dr["SellerName"].ToString();
            ObjMaster.StoreName = dr["StoreName"].ToString();
            return ObjMaster;
        }
        public static SalseInvoicesReport ConvertRowToObjReport1(DataRow dr)
        {
            SalseInvoicesReport ObjMaster = new SalseInvoicesReport();
            ObjMaster.InvoiceID = dr["OrderID"].ToString();
            ObjMaster.InvoiceDate = Comon.ConvertSerialDateTo(dr["InvoiceDate"].ToString());
            ObjMaster.SaleMethod = dr["MethodeName"].ToString();
            ObjMaster.CustomerName = dr["CustomerName"].ToString();
            ObjMaster.CostCenterName = dr["CostCenterName"].ToString();
            ObjMaster.SellerName = dr["SellerName"].ToString();
            ObjMaster.DescountTotal = Comon.cInt(dr["DiscountLines"].ToString()) + Comon.cInt(dr["DiscountOnTotal"].ToString()).ToString();
            ObjMaster.Profit = dr["Profit"].ToString();
            ObjMaster.StoreName = dr["StoreName"].ToString();
            ObjMaster.Notes = dr["Notes"].ToString();
            ObjMaster.Total = dr["Total"].ToString();
            ObjMaster.SaleDelegateName = dr["SaleDelegateName"].ToString();
            ObjMaster.SumVAt = dr["SumVat"].ToString();
            ObjMaster.Net = Comon.cInt(dr["Total"].ToString()) - Comon.cInt(dr["DiscountOnTotal"].ToString()) + Comon.cInt(dr["SumVat"].ToString()).ToString();
            // ObjMaster.BranchID = dr["BranchID"].ToString();
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
                        objCmd.CommandText = "[Sales_SalesOrderMaster_SP]";
                        objCmd.Parameters.AddWithValue("@BranchID", BranchID);
                        // objCmd.Parameters.AddWithValue("@FacilityID", FacilityID);
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

        public static DataTable frmGetDataDetalByID(int ID, int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Sales_SalesOrderMaster_SP]";
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

        public static DataTable frmGetDataDetalByRegistrationNo(long ID, int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Sales_SalesOrderMaster_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@OrderID", ID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 8));
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


        public long GetRecordSetBySQL(string strSQL)
        {
            long ID = 0;
            try
            {
                FoundResult = false;
                dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count > 0)
                {
                    ID = Comon.cLong(dt.Rows[0][1].ToString());
                    FoundResult = true;
                }
            }
            catch (Exception ex)
            {
                FoundResult = false;
            }
            return ID;
        }

        public static long GetNewDialyID(int FacilityID, int BranchID, int USERCREATED)
        {
            try
            {
                long ID = 0;
                DataTable dt;
                string strSQL;
                strSQL = "SELECT Max(DailyID)+1 FROM " + TableName + " Where  BranchID =" + BranchID + " and InvoiceDate=" + Lip.GetServerDateSerial();
                dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count > 0)
                {
                    ID = Comon.cLong(dt.Rows[0][0].ToString());
                    if (ID == 0) ID = 1;
                }

                return ID;
            }
            catch (Exception ex)
            {
                return 1;
            }
        }

        public static long GetNewDialyIDPYcLOSEcASHIER(int FacilityID, int BranchID, int USERCREATED)
        {
            try
            {
                long ID = 0;
                DataTable dt;
                string strSQL;
                strSQL = "SELECT Max(DailyID)+1 FROM " + TableName + " Where  BranchID =" + BranchID + " and CloseCashier=" + 0;
                dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count > 0)
                {
                    ID = Comon.cLong(dt.Rows[0][0].ToString());
                    if (ID == 0) ID = 1;
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

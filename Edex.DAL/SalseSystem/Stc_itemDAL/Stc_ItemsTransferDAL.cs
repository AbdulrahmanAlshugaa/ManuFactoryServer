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
    public class Stc_ItemsTransferDAL
    {

        #region declare 
        public static readonly string TableName = "Stc_ItemsTransferMaster";
        public static readonly string PremaryKey = "OutID";
        public bool FoundResult;

        private DataTable dt;
        #endregion
       
        public static Stc_ItemsTransferDetails ConvertRowToObj(DataRow dr)
        {
            Stc_ItemsTransferMaster ObjMaster = new Stc_ItemsTransferMaster();
            ObjMaster.InvoiceID = Comon.cInt(dr["OutID"].ToString());
            ObjMaster.BranchID = Comon.cInt(dr["BranchID"].ToString());
            ObjMaster.FacilityID = Comon.cInt(dr["FacilityID"].ToString());


            ObjMaster.InvoiceDate = dr["OutDate"].ToString();
            ObjMaster.MethodeID = Comon.cInt(dr["MethodeID"].ToString());
            ObjMaster.CustomerID = Comon.cDbl(dr["CustomerID"].ToString());
            ObjMaster.CostCenterID = Comon.cInt(dr["CostCenterID"].ToString());
            ObjMaster.SellerID = Comon.cInt(dr["SellerID"].ToString());
            ObjMaster.FromStoreID = Comon.cInt(dr["StoreID"].ToString());
            ObjMaster.DelegateID = Comon.cInt(dr["DelegateID"].ToString());
            ObjMaster.Notes = dr["Notes"].ToString();
            ObjMaster.DiscountOnTotal = Comon.cDec(dr["DiscountOnTotal"].ToString());
            ObjMaster.DebitAccount = Comon.cDbl(dr["DebitAccount"].ToString());
            ObjMaster.CreditAccount = Comon.cDbl(dr["CreditAccount"].ToString());

            ObjMaster.DiscountDebitAccount = Comon.cDbl(dr["DiscountDebitAccount"].ToString());
            ObjMaster.FromCashierScreen = Comon.cInt(dr["FromCashierScreen"].ToString());
            ObjMaster.CloseCashier = Comon.cInt(dr["CloseCashier"].ToString());
            ObjMaster.CloseCashierDate = Comon.cDbl(dr["CloseCashierDate"].ToString());
            ObjMaster.NetProcessID = dr["NetProcessID"].ToString();
            ObjMaster.CheckID = dr["CheckID"].ToString();
            ObjMaster.CheckAccount = Comon.cDbl(dr["CheckAccount"].ToString());
            ObjMaster.CurencyID = Comon.cInt(dr["CurencyID"].ToString());

            ObjMaster.CheckSpendDate = dr["CheckSpendDate"].ToString();
            ObjMaster.WarningDate = dr["WarningDate"].ToString();
            ObjMaster.DailyID = Comon.cInt(dr["DailyID"].ToString());
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
            ObjMaster.TempInvoiceID = Comon.cInt(dr["TempOutID"].ToString());
            ObjMaster.EnduranceRatio = Comon.cInt(dr["EnduranceRatio"].ToString());
            ObjMaster.NetAmount = Comon.cInt(dr["NetAmount"].ToString());
            ObjMaster.NetAccount = Comon.cDbl(dr["NetAccount"].ToString());
            ObjMaster.AdditionalAccount = Comon.cDbl(dr["AdditionalAccount"].ToString());

            Stc_ItemsTransferDetails SaleDetailObject = new Stc_ItemsTransferDetails();
            SaleDetailObject.ID = Comon.cInt(dr["ID"].ToString());
            SaleDetailObject.BarCode = dr["BarCode"].ToString();
            SaleDetailObject.SizeID = Comon.cInt(dr["SizeID"].ToString());
            SaleDetailObject.FromStoreID = Comon.cInt(dr["StoreID"].ToString());
            SaleDetailObject.ItemID = Comon.cInt(dr["ItemID"].ToString());
            SaleDetailObject.ArbItemName = dr["ItemName"].ToString();
            SaleDetailObject.ArbSizeName = dr["SizeName"].ToString();

            SaleDetailObject.QTY = Comon.cDec(dr["QTY"].ToString());
            SaleDetailObject.SalePrice = Comon.cDec(dr["SalePrice"].ToString());
            SaleDetailObject.Discount = Comon.cDec(dr["Discount"].ToString());
            SaleDetailObject.CostPrice = Comon.cDec(dr["CostPrice"].ToString());
            SaleDetailObject.Description = dr["Description"].ToString();
            SaleDetailObject.Serials = dr["Serials"].ToString();
            SaleDetailObject.Height = Comon.cInt(dr["Height"].ToString());
            SaleDetailObject.Width = Comon.cInt(dr["Width"].ToString());
            SaleDetailObject.TheCount = Comon.cInt(dr["TheCount"].ToString());
            SaleDetailObject.AdditionalValue = Comon.cDec(dr["AdditionalValue"].ToString());
            SaleDetailObject.ItemsOutonBailMaster = ObjMaster;
            return SaleDetailObject;
        }

        public static Stc_ItemsTransferMaster ConvertRowToObjMaster(DataRow dr)
        {


            Stc_ItemsTransferMaster ObjMaster = new Stc_ItemsTransferMaster();
            ObjMaster.InvoiceID = Comon.cInt(dr["OutID"].ToString());
            ObjMaster.BranchID = Comon.cInt(dr["BranchID"].ToString());
            ObjMaster.InvoiceDate = Comon.ConvertSerialDateTo(dr["OutDate"].ToString());

            ObjMaster.MethodeID = Comon.cInt(dr["MethodeID"].ToString());
            ObjMaster.CustomerID = Comon.cInt(dr["CustomerID"].ToString());
            ObjMaster.CostCenterID = Comon.cInt(dr["CostCenterID"].ToString());
            ObjMaster.SellerID = Comon.cInt(dr["SellerID"].ToString());
            ObjMaster.FromStoreID = Comon.cInt(dr["StoreID"].ToString());
            ObjMaster.DelegateID = Comon.cInt(dr["DelegateID"].ToString());
            ObjMaster.Notes = dr["Notes"].ToString();
            ObjMaster.DiscountOnTotal = Comon.cDec(dr["DiscountOnTotal"].ToString());
            ObjMaster.DebitAccount = Comon.cDbl(dr["DebitAccount"].ToString());
            ObjMaster.CreditAccount = Comon.cDbl(dr["CreditAccount"].ToString());
            ObjMaster.DiscountDebitAccount = Comon.cDbl(dr["DiscountDebitAccount"].ToString());
            ObjMaster.FromCashierScreen = Comon.cInt(dr["FromCashierScreen"].ToString());
            ObjMaster.CloseCashier = Comon.cInt(dr["CloseCashier"].ToString());
            ObjMaster.CloseCashierDate = Comon.cInt(dr["CloseCashierDate"].ToString());
            ObjMaster.NetProcessID = dr["NetProcessID"].ToString();
            ObjMaster.CheckID = dr["CheckID"].ToString();
            ObjMaster.CheckAccount = Comon.cInt(dr["CheckAccount"].ToString());

            ObjMaster.CheckSpendDate = Comon.ConvertSerialDateTo(dr["CheckSpendDate"].ToString());
            ObjMaster.WarningDate = Comon.ConvertSerialDateTo(dr["WarningDate"].ToString());
            ObjMaster.DailyID = Comon.cInt(dr["DailyID"].ToString());
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
            ObjMaster.TempInvoiceID = Comon.cInt(dr["TempOutID"].ToString());
            ObjMaster.EnduranceRatio = Comon.cInt(dr["EnduranceRatio"].ToString());
            ObjMaster.NetAmount = Comon.cDbl(dr["NetAmount"].ToString());
            ObjMaster.NetAccount = Comon.cDbl(dr["NetAccount"].ToString());
            return ObjMaster;
        }

        public static Stc_ItemsTransferMaster ConvertRowToObjMasterForShowAllRecords(DataRow dr)
        {


            Stc_ItemsTransferMaster ObjMaster = new Stc_ItemsTransferMaster();
            ObjMaster.InvoiceID = Comon.cInt(dr["OutID"].ToString());
            ObjMaster.BranchID = Comon.cInt(dr["BranchID"].ToString());
            ObjMaster.InvoiceDate = dr["OutDate"].ToString();
            ObjMaster.MethodeID = Comon.cInt(dr["SaleMethod"].ToString());
            ObjMaster.CustomerName = dr["CustomerName"].ToString();
            ObjMaster.CostCenterName = dr["CostCenterName"].ToString();
            ObjMaster.StoreName = dr["StoreName"].ToString();
            ObjMaster.NetBalance = Comon.cDec(dr["NetBalance"].ToString());


            return ObjMaster;
        }

        public static List<Stc_ItemsTransferDetails> GetDataDetailByID(int ID, int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Stc_ItemsTransferMaster_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@OutID", ID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 5));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);

                        if (dt != null)
                        {
                            List<Stc_ItemsTransferDetails> Returned = new List<Stc_ItemsTransferDetails>();
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

        public static Stc_ItemsTransferMaster GetDataMasterByID(int ID, int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Stc_ItemsTransferMaster_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@OutID", ID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 3));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            Stc_ItemsTransferMaster Returned = new Stc_ItemsTransferMaster();
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

        public static List<Stc_ItemsTransferMaster> GetAllMasterData(int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Stc_ItemsTransferMaster_SP]";
                        objCmd.Parameters.AddWithValue("@BranchID", BranchID);
                        objCmd.Parameters.AddWithValue("@FacilityID", FacilityID);

                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 7));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            List<Stc_ItemsTransferMaster> Returned = new List<Stc_ItemsTransferMaster>();
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

        public static string InsertUsingXML(Stc_ItemsTransferMaster objRecord, int USERCREATED)
        {
            string objRet = "";
            string DitmeXML = ConvertObjectToXMLString(objRecord.OutonBailDetails);
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Stc_ItemsTransferMaster_SP]";
                    objCmd.Parameters.Add(new SqlParameter("xmlData", SqlDbType.Xml)).Value = DitmeXML;
                    // objCmd.Parameters.Add("@xmlSaleDatial", SqlDbType.Xml, 1500, DitmeXML);
                    objCmd.Parameters.Add(new SqlParameter("@OutID", objRecord.InvoiceID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@OutDate", objRecord.InvoiceDate));
                    objCmd.Parameters.Add(new SqlParameter("@MethodeID", objRecord.MethodeID));
                    objCmd.Parameters.Add(new SqlParameter("@CustomerID", objRecord.CustomerID));
                    objCmd.Parameters.Add(new SqlParameter("@SellerID", objRecord.SellerID));
                    objCmd.Parameters.Add(new SqlParameter("@CostCenterID", objRecord.CostCenterID));
                    objCmd.Parameters.Add(new SqlParameter("@FromStoreID", objRecord.FromStoreID));
                    objCmd.Parameters.Add(new SqlParameter("@ToStoreID", objRecord.ToStoreID));

                    objCmd.Parameters.Add(new SqlParameter("@DelegateID", objRecord.DelegateID));
                    objCmd.Parameters.Add(new SqlParameter("@CurencyID", objRecord.CurencyID));
                    objCmd.Parameters.Add(new SqlParameter("@Notes", objRecord.Notes));
                    objCmd.Parameters.Add(new SqlParameter("@DiscountOnTotal", objRecord.DiscountOnTotal));
                    objCmd.Parameters.Add(new SqlParameter("@DebitAccount", objRecord.DebitAccount));
                    objCmd.Parameters.Add(new SqlParameter("@CreditAccount", objRecord.CreditAccount));
                    objCmd.Parameters.Add(new SqlParameter("@DiscountDebitAccount", objRecord.DiscountDebitAccount));
                    objCmd.Parameters.Add(new SqlParameter("@FromCashierScreen", objRecord.FromCashierScreen));
                    objCmd.Parameters.Add(new SqlParameter("@CloseCashier", objRecord.CloseCashier));
                    objCmd.Parameters.Add(new SqlParameter("@CloseCashierDate", objRecord.CloseCashierDate));
                    objCmd.Parameters.Add(new SqlParameter("@NetProcessID", objRecord.NetProcessID));
                    objCmd.Parameters.Add(new SqlParameter("@CheckID", objRecord.CheckID));
                    objCmd.Parameters.Add(new SqlParameter("@CheckAccount", objRecord.CheckAccount));
                    objCmd.Parameters.Add(new SqlParameter("@CheckSpendDate", objRecord.CheckSpendDate));
                    objCmd.Parameters.Add(new SqlParameter("@WarningDate", objRecord.WarningDate));
                    objCmd.Parameters.Add(new SqlParameter("@DailyID", objRecord.DailyID));
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
                    objCmd.Parameters.Add(new SqlParameter("@TempOutID", objRecord.TempInvoiceID));
                    objCmd.Parameters.Add(new SqlParameter("@EnduranceRatio", objRecord.EnduranceRatio));
                    objCmd.Parameters.Add(new SqlParameter("@NetAmount", objRecord.NetAmount));
                    objCmd.Parameters.Add(new SqlParameter("@NetAccount", objRecord.NetAccount));
                    objCmd.Parameters.Add(new SqlParameter("@NetBalance", objRecord.NetBalance));
                    objCmd.Parameters.Add(new SqlParameter("@AdditionalAccount", objRecord.AdditionalAccount));
                    if (objRecord.InvoiceID == 0)
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

        public static Int32 InsertStc_ItemsTransferMaster(Stc_ItemsTransferMaster objRecord)
        {
            Int32 objRet = 0;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Stc_ItemsTransferMaster_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@OutID", objRecord.InvoiceID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@OutDate", objRecord.InvoiceDate));
                    objCmd.Parameters.Add(new SqlParameter("@MethodeID", objRecord.MethodeID));
                    objCmd.Parameters.Add(new SqlParameter("@CustomerID", objRecord.CustomerID));
                    objCmd.Parameters.Add(new SqlParameter("@CostCenterID", objRecord.CostCenterID));
                    objCmd.Parameters.Add(new SqlParameter("@SellerID", objRecord.SellerID));
                    objCmd.Parameters.Add(new SqlParameter("@StoreID", objRecord.FromStoreID));
                    objCmd.Parameters.Add(new SqlParameter("@DelegateID", objRecord.DelegateID));
                    objCmd.Parameters.Add(new SqlParameter("@Notes", objRecord.Notes));
                    objCmd.Parameters.Add(new SqlParameter("@DiscountOnTotal", objRecord.DiscountOnTotal));
                    objCmd.Parameters.Add(new SqlParameter("@DebitAccount", objRecord.DebitAccount));
                    objCmd.Parameters.Add(new SqlParameter("@CreditAccount", objRecord.CreditAccount));
                    objCmd.Parameters.Add(new SqlParameter("@DiscountDebitAccount", objRecord.DiscountDebitAccount));
                    objCmd.Parameters.Add(new SqlParameter("@FromCashierScreen", objRecord.FromCashierScreen));
                    objCmd.Parameters.Add(new SqlParameter("@CloseCashier", objRecord.CloseCashier));
                    objCmd.Parameters.Add(new SqlParameter("@CloseCashierDate", objRecord.CloseCashierDate));
                    objCmd.Parameters.Add(new SqlParameter("@NetProcessID", objRecord.NetProcessID));
                    objCmd.Parameters.Add(new SqlParameter("@CheckID", objRecord.CheckID));
                    objCmd.Parameters.Add(new SqlParameter("@CheckSpendDate", objRecord.CheckSpendDate));
                    objCmd.Parameters.Add(new SqlParameter("@WarningDate", objRecord.WarningDate));
                    objCmd.Parameters.Add(new SqlParameter("@DailyID", objRecord.DailyID));
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
                    objCmd.Parameters.Add(new SqlParameter("@TempOutID", objRecord.TempInvoiceID));
                    objCmd.Parameters.Add(new SqlParameter("@EnduranceRatio", objRecord.EnduranceRatio));
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

        public static long UpdateUsingXML(Stc_ItemsTransferMaster objRecord, int USERCREATED)
        {
            Int32 objRet = 0;
            string DitmeXML = ConvertObjectToXMLString(objRecord.OutonBailDetails);
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Stc_ItemsTransferMaster_SP]";
                    objCmd.Parameters.Add("@xmlSaleDatial", SqlDbType.Xml, 1500, DitmeXML);
                    objCmd.Parameters.Add(new SqlParameter("@OutID", objRecord.InvoiceID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@OutDate", objRecord.InvoiceDate));
                    objCmd.Parameters.Add(new SqlParameter("@MethodeID", objRecord.MethodeID));
                    objCmd.Parameters.Add(new SqlParameter("@CustomerID", objRecord.CustomerID));
                    objCmd.Parameters.Add(new SqlParameter("@CostCenterID", objRecord.CostCenterID));
                    objCmd.Parameters.Add(new SqlParameter("@SellerID", objRecord.SellerID));
                    objCmd.Parameters.Add(new SqlParameter("@FromStoreID", objRecord.FromStoreID));
                    objCmd.Parameters.Add(new SqlParameter("@ToStoreID", objRecord.ToStoreID));
                    objCmd.Parameters.Add(new SqlParameter("@DelegateID", objRecord.DelegateID));
                    objCmd.Parameters.Add(new SqlParameter("@Notes", objRecord.Notes));
                    objCmd.Parameters.Add(new SqlParameter("@DiscountOnTotal", objRecord.DiscountOnTotal));
                    objCmd.Parameters.Add(new SqlParameter("@DebitAccount", objRecord.DebitAccount));
                    objCmd.Parameters.Add(new SqlParameter("@CreditAccount", objRecord.CreditAccount));
                    objCmd.Parameters.Add(new SqlParameter("@DiscountDebitAccount", objRecord.DiscountDebitAccount));
                    objCmd.Parameters.Add(new SqlParameter("@FromCashierScreen", objRecord.FromCashierScreen));
                    objCmd.Parameters.Add(new SqlParameter("@CloseCashier", objRecord.CloseCashier));
                    objCmd.Parameters.Add(new SqlParameter("@CloseCashierDate", objRecord.CloseCashierDate));
                    objCmd.Parameters.Add(new SqlParameter("@NetProcessID", objRecord.NetProcessID));
                    objCmd.Parameters.Add(new SqlParameter("@CheckID", objRecord.CheckID));
                    objCmd.Parameters.Add(new SqlParameter("@CheckSpendDate", objRecord.CheckSpendDate));
                    objCmd.Parameters.Add(new SqlParameter("@WarningDate", objRecord.WarningDate));
                    objCmd.Parameters.Add(new SqlParameter("@DailyID", objRecord.DailyID));
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
                    objCmd.Parameters.Add(new SqlParameter("@TempOutID", objRecord.TempInvoiceID));
                    objCmd.Parameters.Add(new SqlParameter("@EnduranceRatio", objRecord.EnduranceRatio));
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

        public static string DeleteStc_ItemsTransferMaster(Stc_ItemsTransferMaster objRecord)
        {
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Stc_ItemsTransferMaster_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@OutID", objRecord.InvoiceID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", MySession.GlobalFacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                    objCmd.Parameters.Add(new SqlParameter("@EditDate", objRecord.EditDate));
                    objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 4));
                    object obj = objCmd.ExecuteNonQuery();
                    if (obj != null)
                        return obj.ToString();
                }
            }

            return "";
        }

        /////////////reports////////////

        public static SalseInvoicesReport ConvertRowToObjReport(DataRow dr)
        {
            SalseInvoicesReport ObjMaster = new SalseInvoicesReport();
            ObjMaster.InvoiceID = dr["OutID"].ToString();
            ObjMaster.InvoiceDate = dr["OutDate"].ToString();
            ObjMaster.SaleMethod = dr["SaleMethod"].ToString();
            ObjMaster.CustomerName = dr["CustomerName"].ToString();
            ObjMaster.CostCenterName = dr["CostCenterName"].ToString();
            ObjMaster.SellerName = dr["SellerName"].ToString();
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
                        objCmd.CommandText = "[Stc_ItemsTransferMaster_SP]";
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
                        objCmd.CommandText = "[Stc_ItemsTransferMaster_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@OutID", ID));
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
                    ID = Convert.ToInt32(dt.Rows[0][0] == DBNull.Value ? "1" : dt.Rows[0][0].ToString());
                }


                strSQL = "Select Top 1 StartFrom From StartNumbering Where BranchID=" + BranchID
                    + " And FormName='frmItemsOutonBail'";
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
    }
}

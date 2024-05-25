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
namespace Edex.DAL.Accounting
{
    public class ReceiptVoucherDAL
    {
        public static readonly string TableName = "Acc_ReceiptVoucherMaster";
        public static readonly string PremaryKey = "ReceiptVoucherID";
        public bool FoundResult;

        private string strSQL = "";

        public static Acc_ReceiptVoucherDetails ConvertRowToObj(DataRow dr)
        {
            Acc_ReceiptVoucherMaster ObjMaster = new Acc_ReceiptVoucherMaster();
            ObjMaster.ReceiptVoucherID = Comon.cInt(dr["ReceiptVoucherID"].ToString());
            ObjMaster.ReceiptVoucherDate = Comon.ConvertSerialDateTo(dr["ReceiptVoucherDate"].ToString());
            ObjMaster.InvoiceID = Comon.cInt(dr["InvoiceID"].ToString());
            ObjMaster.BranchID = Comon.cInt(dr["BranchID"].ToString());
            ObjMaster.FacilityID = Comon.cInt(dr["FacilityID"].ToString());
            ObjMaster.DelegateID = Comon.cInt(dr["DelegateID"].ToString());
            ObjMaster.Notes = dr["Notes"].ToString();
            ObjMaster.DiscountAmount = Comon.cInt(dr["DiscountAmount"].ToString());
            ObjMaster.DebitAccountID = Comon.cDbl(dr["DebitAccountID"].ToString());
            ObjMaster.DebitAmount = Comon.cInt(dr["DebitAmount"].ToString());
            ObjMaster.DiscountAccountID = Comon.cDbl(dr["DiscountAccountID"].ToString());
            ObjMaster.CurrencyID = Comon.cInt(dr["CurrencyID"].ToString());
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
            Acc_ReceiptVoucherDetails DetailsObject = new Acc_ReceiptVoucherDetails();
            DetailsObject.ID = Comon.cInt(dr["ID"].ToString());
            DetailsObject.AccountID = Comon.cDbl(dr["AccountID"].ToString());
            DetailsObject.CostCenterID = Comon.cInt(dr["CostCenterID"].ToString());
            DetailsObject.CreditAmount = Comon.cDbl(dr["CreditAmount"].ToString());
            DetailsObject.Discount = Comon.cInt(dr["Discount"].ToString());
            DetailsObject.Declaration = dr["Declaration"].ToString();
            DetailsObject.ReceiptVoucherMaster = ObjMaster;
            return DetailsObject;
        }
        public static Acc_ReceiptVoucherMaster ConvertRowToObjMaster(DataRow dr)
        {
            Acc_ReceiptVoucherMaster ObjMaster = new Acc_ReceiptVoucherMaster();
            ObjMaster.ReceiptVoucherID = Comon.cInt(dr["ReceiptVoucherID"].ToString());
            ObjMaster.ReceiptVoucherDate = Comon.ConvertSerialDateTo(dr["ReceiptVoucherDate"].ToString());
            ObjMaster.BranchID = Comon.cInt(dr["BranchID"].ToString());
            ObjMaster.FacilityID = Comon.cInt(dr["FacilityID"].ToString());
            ObjMaster.DelegateID = Comon.cInt(dr["DelegateID"].ToString());
            ObjMaster.Notes = dr["Notes"].ToString();
            ObjMaster.DiscountAmount = Comon.cInt(dr["DiscountAmount"].ToString());
            ObjMaster.DebitAccountID = Comon.cDbl(dr["DebitAccountID"].ToString());
            ObjMaster.DebitAmount = Comon.cInt(dr["DebitAmount"].ToString());
            ObjMaster.DiscountAccountID = Comon.cDbl(dr["DiscountAccountID"].ToString());
            ObjMaster.CurrencyID = Comon.cInt(dr["CurrencyID"].ToString());
            ObjMaster.DocumentID = Comon.cInt(dr["DocumentID"].ToString());
            ObjMaster.UserID = Comon.cInt(dr["UserID"].ToString());
            ObjMaster.RegDate = Comon.cInt(dr["RegDate"].ToString());
            ObjMaster.RegTime = Comon.cInt(dr["RegTime"].ToString());
            ObjMaster.EditUserID = Comon.cInt(dr["EditUserID"].ToString());
            ObjMaster.EditTime = Comon.cInt(dr["EditTime"].ToString());
            ObjMaster.EditDate = Comon.cInt(dr["EditDate"].ToString());
            ObjMaster.ComputerInfo = dr["ComputerInfo"].ToString();
            ObjMaster.EditComputerInfo = dr["EditComputerInfo"].ToString();

            return ObjMaster;
        }
        public static List<Acc_ReceiptVoucherDetails> GetDataDetalByID(long ID, int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Acc_ReceiptVoucher_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@InvoiceID", ID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 5));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);

                        if (dt != null)
                        {
                            List<Acc_ReceiptVoucherDetails> Returned = new List<Acc_ReceiptVoucherDetails>();
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
        public static Acc_ReceiptVoucherMaster GetDataMasterByID(int ID, int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Acc_ReceiptVoucher_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@ReceiptVoucherID", ID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 4));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            Acc_ReceiptVoucherMaster Returned = new Acc_ReceiptVoucherMaster();
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
        public static List<Acc_ReceiptVoucherMaster> GetAllMasterData(int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Acc_ReceiptVoucher_SP]";
                        objCmd.Parameters.AddWithValue("@BranchID", BranchID);
                        objCmd.Parameters.AddWithValue("@FacilityID", FacilityID);

                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 3));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            List<Acc_ReceiptVoucherMaster> Returned = new List<Acc_ReceiptVoucherMaster>();
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
        public static DataTable GetAcc_ReceiptVoucherMaster(int BranchID, int FacilityID)
        {
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Acc_ReceiptVoucher_SP]";
                    objCmd.CommandText = "[Acc_ReceiptVoucher_SP]";
                    objCmd.Parameters.AddWithValue("@BranchID", BranchID);
                    objCmd.Parameters.AddWithValue("@FacilityID", FacilityID);
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 6));
                    SqlDataReader myreader = objCmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(myreader);
                    return dt;
                }
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
        public static long InsertUsingXML(Acc_ReceiptVoucherMaster objRecord, bool IsNewRecord)
        {
            Int32 objRet = 0;
            string DitmeXML = ConvertObjectToXMLString(objRecord.ReceiptVoucherDetails);
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Acc_ReceiptVoucher_SP]";
                    objCmd.Parameters.Add(new SqlParameter("xmlData", SqlDbType.Xml)).Value = DitmeXML;
                    objCmd.Parameters.Add(new SqlParameter("@ReceiptVoucherID", objRecord.ReceiptVoucherID));
                    objCmd.Parameters.Add(new SqlParameter("@TypeOpration", objRecord.TypeOpration));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@ReceiptVoucherDate", objRecord.ReceiptVoucherDate));
                    objCmd.Parameters.Add(new SqlParameter("@Notes", objRecord.Notes));
                    objCmd.Parameters.Add(new SqlParameter("@DebitGoldAccountID", objRecord.DebitGoldAccountID));
                    objCmd.Parameters.Add(new SqlParameter("@DebitAccountID", objRecord.DebitAccountID));
                    objCmd.Parameters.Add(new SqlParameter("@DebitAmount", objRecord.DebitAmount));
                    objCmd.Parameters.Add(new SqlParameter("@TotalGold", objRecord.TotalGold));
                    objCmd.Parameters.Add(new SqlParameter("@TotalWeightDiamond", objRecord.TotalWeightDiamond));

                    objCmd.Parameters.Add(new SqlParameter("@CurrencyEquivalent", objRecord.CurrencyEquivalent));
                    objCmd.Parameters.Add(new SqlParameter("@CurrencyName", objRecord.CurrencyName));
                    objCmd.Parameters.Add(new SqlParameter("@CurrencyPrice", objRecord.CurrencyPrice));
                    objCmd.Parameters.Add(new SqlParameter("@CurrencyID", objRecord.CurrencyID));

                    objCmd.Parameters.Add(new SqlParameter("@DiscountAccountID", objRecord.DiscountAccountID));
                    objCmd.Parameters.Add(new SqlParameter("@DiscountAmount", objRecord.DiscountAmount));
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceID", objRecord.InvoiceID));
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceServiceID", objRecord.InvoiceServiceID));
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

                    objCmd.Parameters.Add(new SqlParameter("@DelegateID", objRecord.DelegateID));
                    objCmd.Parameters.Add(new SqlParameter("@SpendImage", objRecord.SpendImage));
                    objCmd.Parameters.Add(new SqlParameter("@RegistrationNo", objRecord.RegistrationNo));
                    objCmd.Parameters.Add(new SqlParameter("@OperationTypeName", 2));
                    SqlParameter pvNewId = new SqlParameter();
                    pvNewId.ParameterName = "@ProductId";
                    pvNewId.DbType = DbType.Int32;
                    pvNewId.Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add(pvNewId);
                    if (IsNewRecord)
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 1));
                    else
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 2));
                    object obj = objCmd.ExecuteScalar();
                    string val = objCmd.Parameters["@ProductId"].Value.ToString();
                    if (val != null)
                        objRet = Convert.ToInt32(val);
                }
            }
            return objRet;
        }
        public static Int32 InsertAcc_ReceiptVoucherMaster(Acc_ReceiptVoucherMaster objRecord)
        {
            Int32 objRet = 0;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Acc_ReceiptVoucher_SP]";

                    objCmd.Parameters.Add(new SqlParameter("@ReceiptVoucherID", objRecord.ReceiptVoucherID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@ReceiptVoucherDate", objRecord.ReceiptVoucherDate));
                    objCmd.Parameters.Add(new SqlParameter("@Notes", objRecord.Notes));
                    objCmd.Parameters.Add(new SqlParameter("@DebitAccountID", objRecord.DebitAccountID));
                    objCmd.Parameters.Add(new SqlParameter("@DebitAmount", objRecord.DebitAmount));
                    objCmd.Parameters.Add(new SqlParameter("@CurrencyID", objRecord.CurrencyID));
                    objCmd.Parameters.Add(new SqlParameter("@DiscountAccountID", objRecord.DiscountAccountID));
                    objCmd.Parameters.Add(new SqlParameter("@DiscountAmount", objRecord.DiscountAmount));
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceID", objRecord.InvoiceID));
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
                    objCmd.Parameters.Add(new SqlParameter("@DelegateID", objRecord.DelegateID));
                    objCmd.Parameters.Add(new SqlParameter("@SpendImage", objRecord.SpendImage));
                    objCmd.Parameters.Add(new SqlParameter("@RegistrationNo", objRecord.RegistrationNo));

                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 1));
                    object obj = objCmd.ExecuteScalar();
                    if (obj != null)
                        objRet = Convert.ToInt32(obj);
                }
            }
            return objRet;
        }
        public static bool UpdateAcc_ReceiptVoucherMaster(Acc_ReceiptVoucherMaster objRecord)
        {
            bool objRet = false;
            objRet = false;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Acc_ReceiptVoucher_SP]";

                    objCmd.Parameters.Add(new SqlParameter("@ReceiptVoucherID", objRecord.ReceiptVoucherID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@ReceiptVoucherDate", objRecord.ReceiptVoucherDate));
                    objCmd.Parameters.Add(new SqlParameter("@Notes", objRecord.Notes));
                    objCmd.Parameters.Add(new SqlParameter("@DebitAccountID", objRecord.DebitAccountID));
                    objCmd.Parameters.Add(new SqlParameter("@DebitAmount", objRecord.DebitAmount));
                    objCmd.Parameters.Add(new SqlParameter("@CurrencyID", objRecord.CurrencyID));
                    objCmd.Parameters.Add(new SqlParameter("@DiscountAccountID", objRecord.DiscountAccountID));
                    objCmd.Parameters.Add(new SqlParameter("@DiscountAmount", objRecord.DiscountAmount));
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceID", objRecord.InvoiceID));
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
                    objCmd.Parameters.Add(new SqlParameter("@DelegateID", objRecord.DelegateID));
                    objCmd.Parameters.Add(new SqlParameter("@SpendImage", objRecord.SpendImage));
                    objCmd.Parameters.Add(new SqlParameter("@RegistrationNo", objRecord.RegistrationNo));

                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 2));
                    objCmd.ExecuteNonQuery();
                }
            }
            objRet = true;
            return objRet;
        }
        public static int DeleteAcc_ReceiptVoucherMaster(Acc_ReceiptVoucherMaster objRecord)
        {
            
            
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Acc_ReceiptVoucher_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@ReceiptVoucherID", objRecord.ReceiptVoucherID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@EditDate", objRecord.EditDate));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));

                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 6));
                    SqlParameter pvNewId = new SqlParameter();
                    pvNewId.ParameterName = "@ProductId";
                    pvNewId.DbType = DbType.Int32;
                    pvNewId.Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add(pvNewId);

                    object obj = objCmd.ExecuteNonQuery();

                    string val = objCmd.Parameters["@ProductId"].Value.ToString();

                    if (val != null)
                        return Convert.ToInt32(val);
                }
            }

            return 0;
        }
        public static AccountVoucherReport ConvertRowToObjReport(DataRow dr)
        {

            AccountVoucherReport ObjMaster = new AccountVoucherReport();

            ObjMaster.VoucherID = dr["VoucherID"].ToString();
            ObjMaster.VoucherDate = Comon.ConvertSerialDateTo(dr["VoucherDate"].ToString());
            ObjMaster.DocumentID = dr["DocumentID"].ToString();
            ObjMaster.RegistrationNo = dr["RegistrationNo"].ToString();
            ObjMaster.Amount = dr["Amount"].ToString();
            ObjMaster.Description = dr["Description"].ToString();
            ObjMaster.UserName = dr["UserName"].ToString();

            return ObjMaster;
        }
        public static List<AccountVoucherReport> GetReport(string sql)
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
                            List<AccountVoucherReport> Returned = new List<AccountVoucherReport>();
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
                        objCmd.CommandText = "[Acc_ReceiptVoucher_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@ReceiptVoucherID", ID));
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

        public static long GetNewID()
        {
            try
            {
                long ID = 0;
                DataTable dt;
                string strSQL;

                strSQL = "SELECT Max(" + PremaryKey + ")+1 FROM " + TableName + " Where  BranchID =" + MySession.GlobalBranchID;
                dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count > 0)
                    ID = Comon.cLong(dt.Rows[0][0].ToString());

                if (ID == 0)
                    ID = 1;

                strSQL = "Select Top 1 StartFrom From StartNumbering Where BranchID=" + MySession.GlobalBranchID + " And FormName='frmReceiptVoucher'";
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
            DataTable dt;
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

        public DataTable FillDataGrid(long ReceiptVoucherID)
        {
            try
            {
                DataTable dt = new DataTable();
                strSQL = ("SELECT  dbo.Acc_ReceiptVoucherDetails.CreditAmount, dbo.Acc_ReceiptVoucherDetails.Discount, dbo.Acc_R" +
                "eceiptVoucherDetails.Declaration, " + (" dbo.Acc_ReceiptVoucherDetails.CostCenterID, dbo.Acc_ReceiptVoucherDetails.AccountID, dbo.Acc_Account" +
                "s.ArbName AS AccountName, dbo.Acc_CostCenters.ArbName AS CostCenterName " + (" FROM   dbo.Acc_ReceiptVoucherDetails LEFT OUTER JOIN" + (" dbo.Acc_CostCenters ON dbo.Acc_ReceiptVoucherDetails.BranchID = dbo.Acc_CostCenters.BranchID AND " + (" dbo.Acc_ReceiptVoucherDetails.CostCenterID = dbo.Acc_CostCenters.CostCenterID LEFT OUTER JOIN" + (" dbo.Acc_Accounts ON dbo.Acc_ReceiptVoucherDetails.BranchID = dbo.Acc_Accounts.BranchID AND" + (" dbo.Acc_ReceiptVoucherDetails.AccountID = dbo.Acc_Accounts.AccountID " + (" WHERE  (dbo.Acc_ReceiptVoucherDetails.BranchID = "
                 + (MySession.GlobalBranchID + (") AND (dbo.Acc_ReceiptVoucherDetails.ReceiptVoucherID = "
                            + (ReceiptVoucherID + (")" + " ORDER BY dbo.Acc_ReceiptVoucherDetails.ID"))))))))))));

                return Lip.SelectRecord(strSQL); ;
            }
            catch (Exception ex)
            {
                //WT.msgError(this.GetType.Name, System.Reflection.MethodBase.GetCurrentMethod.Name);
                return null;
            }

        }


        public static int InsertUsingXMLRecipt(Acc_ReceiptVoucherMaster objRecord, int USERCREATED)
        {
            int objRet = 0;
            string DitmeXML = ConvertObjectToXMLString(objRecord.ReceiptVoucherDetails);
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Acc_ReceiptVoucher_SP]";
                    objCmd.Parameters.Add(new SqlParameter("xmlData", SqlDbType.Xml)).Value = DitmeXML;
                    objCmd.Parameters.Add(new SqlParameter("@ReceiptVoucherID", objRecord.ReceiptVoucherID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@ReceiptVoucherDate", objRecord.ReceiptVoucherDate));
                    objCmd.Parameters.Add(new SqlParameter("@Notes", objRecord.Notes));
                    objCmd.Parameters.Add(new SqlParameter("@DebitAccountID", objRecord.DebitAccountID));
                    objCmd.Parameters.Add(new SqlParameter("@DebitAmount", objRecord.DebitAmount));
                    objCmd.Parameters.Add(new SqlParameter("@CurrencyID", objRecord.CurrencyID));
                    objCmd.Parameters.Add(new SqlParameter("@DiscountAccountID", objRecord.DiscountAccountID));
                    objCmd.Parameters.Add(new SqlParameter("@DiscountAmount", objRecord.DiscountAmount));
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceID", objRecord.InvoiceID));
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
                    objCmd.Parameters.Add(new SqlParameter("@DelegateID", objRecord.DelegateID));
                    objCmd.Parameters.Add(new SqlParameter("@SpendImage", objRecord.SpendImage));
                    objCmd.Parameters.Add(new SqlParameter("@RegistrationNo", objRecord.RegistrationNo));
                    objCmd.Parameters.Add(new SqlParameter("@OperationTypeName", objRecord.OperationTypeName));
                    if (objRecord.ReceiptVoucherID == 0)
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 1));
                    else
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 2));
                    object obj = objCmd.ExecuteScalar();
                    if (obj != null)
                        objRet = Comon.cInt(obj);
                }
            }
            return objRet;

        }

    }
}

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
    public class CheckSpendVoucherDAL
    {
        public static readonly string TableName = "Acc_CheckSpendVoucherMaster";
        public static readonly string PremaryKey = "CheckSpendVoucherID";
        public bool FoundResult;

        private string strSQL = "";
        public static Acc_CheckSpendVoucherDetails ConvertRowToObj(DataRow dr)
        {

            Acc_CheckSpendVoucherMaster ObjMaster = new Acc_CheckSpendVoucherMaster();
            ObjMaster.CheckSpendVoucherID = Comon.cInt(dr["CheckSpendVoucherID"].ToString());
            ObjMaster.CheckSpendVoucherDate = Comon.ConvertSerialDateTo(dr["CheckSpendVoucherDate"].ToString());
            ObjMaster.BankID = Comon.cDbl(dr["BankID"].ToString());
            ObjMaster.CreditAmount = Comon.cDbl(dr["CreditAmount"].ToString());
            ObjMaster.BranchID = Comon.cInt(dr["BranchID"].ToString());
            ObjMaster.CurrencyID = Comon.cInt(dr["CurrencyID"].ToString());
            ObjMaster.DocumentID = Comon.cInt(dr["DocumentID"].ToString());
            ObjMaster.Notes = dr["Notes"].ToString();

            ObjMaster.HijriDate = Comon.ConvertSerialDateTo(dr["HijriDate"].ToString());
            ObjMaster.GreDate = Comon.ConvertSerialDateTo(dr["HijriDate"].ToString());
            ObjMaster.BeneficiaryName = dr["txtBeneficiaryName"].ToString();
            ObjMaster.WroteInCity = dr["WroteInCity"].ToString();
           

            ObjMaster.RegistrationNo = Comon.cDbl(dr["RegistrationNo "].ToString());
            ObjMaster.DiscountAccountID = Comon.cDbl(dr["DiscountAccountID"].ToString());
            ObjMaster.DiscountAmount = Comon.cDbl(dr["DiscountAmount"].ToString());
            ObjMaster.BranchID = Comon.cInt(dr["BranchID"].ToString());
            ObjMaster.FacilityID = Comon.cInt(dr["FacilityID"].ToString());
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

            Acc_CheckSpendVoucherDetails SaleDetalObject = new Acc_CheckSpendVoucherDetails();
            SaleDetalObject.ID = Comon.cInt(dr["ID"].ToString());
            SaleDetalObject.CheckSpendVoucherID = Comon.cInt(dr["CheckSpendVoucherID"].ToString());
            SaleDetalObject.BranchID = Comon.cInt(dr["BranchID"].ToString());
            SaleDetalObject.FacilityID = Comon.cInt(dr["FacilityID"].ToString());
            SaleDetalObject.Declaration = dr["Declaration"].ToString();
            SaleDetalObject.CostCenterID = Comon.cInt(dr["CostCenterID"].ToString());
            SaleDetalObject.AccountID = Comon.cDbl(dr["AccountID"].ToString());
            SaleDetalObject.AccountName = dr["AccountName"].ToString(); 
            SaleDetalObject.DebitAmount = Comon.cDbl(dr["DebitAmount"].ToString());
            SaleDetalObject.ReceiptName = dr["ReceiptName"].ToString();
            SaleDetalObject.CheckSpendVoucherMaster = ObjMaster;
            return SaleDetalObject;
        }

        public static Acc_CheckSpendVoucherMaster ConvertRowToObjMaster(DataRow dr)
        {

            Acc_CheckSpendVoucherMaster ObjMaster = new Acc_CheckSpendVoucherMaster();

            ObjMaster.CheckSpendVoucherID = Comon.cInt(dr["CheckSpendVoucherID"].ToString());
            ObjMaster.CheckSpendVoucherDate = Comon.ConvertSerialDateTo(dr["CheckSpendVoucherDate"].ToString());
            ObjMaster.BankID = Comon.cDbl(dr["BankID"].ToString());
            ObjMaster.CreditAmount = Comon.cDbl(dr["CreditAmount"].ToString());
            ObjMaster.BranchID = Comon.cInt(dr["BranchID"].ToString());
            ObjMaster.DocumentID = Comon.cInt(dr["DocumentID"].ToString());
            ObjMaster.Notes = dr["Notes"].ToString();
            ObjMaster.RegistrationNo = Comon.cDbl(dr["RegistrationNo"].ToString());
            ObjMaster.DiscountAccountID = Comon.cDbl(dr["DiscountAccountID"].ToString());
            ObjMaster.DiscountAmount = Comon.cDbl(dr["DiscountAmount"].ToString());
            ObjMaster.BranchID = Comon.cInt(dr["BranchID"].ToString());
            ObjMaster.FacilityID = Comon.cInt(dr["FacilityID"].ToString());
            ObjMaster.DocumentID = Comon.cInt(dr["DocumentID"].ToString());
            ObjMaster.UserID = Comon.cInt(dr["UserID"].ToString());
            ObjMaster.RegDate = Comon.cDbl(dr["RegDate"].ToString());
            ObjMaster.RegTime = Comon.cInt(dr["RegTime"].ToString());
            ObjMaster.EditUserID = Comon.cInt(dr["EditUserID"].ToString());
            ObjMaster.EditTime = Comon.cInt(dr["EditTime"].ToString());
            ObjMaster.EditDate = Comon.cInt(dr["EditDate"].ToString());
            ObjMaster.ComputerInfo = dr["ComputerInfo"].ToString();
            ObjMaster.EditComputerInfo = dr["EditComputerInfo"].ToString();
            return ObjMaster;
        }

        public static List<Acc_CheckSpendVoucherDetails> GetDataDetalByID(int ID, int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Acc_CheckSpendVoucher_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@CheckSpendVoucherID", ID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 5));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);

                        if (dt != null)
                        {
                            List<Acc_CheckSpendVoucherDetails> Returned = new List<Acc_CheckSpendVoucherDetails>();
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

        public static Acc_CheckSpendVoucherMaster GetDataMasterByID(int ID, int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Acc_CheckSpendVoucher_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@CheckSpendVoucherID", ID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 4));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            Acc_CheckSpendVoucherMaster Returned = new Acc_CheckSpendVoucherMaster();
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

        public static List<Acc_CheckSpendVoucherMaster> GetAllMasterData(int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Acc_CheckSpendVoucher_SP]";
                        objCmd.Parameters.AddWithValue("@BranchID", BranchID);
                        objCmd.Parameters.AddWithValue("@FacilityID", FacilityID);
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 3));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            List<Acc_CheckSpendVoucherMaster> Returned = new List<Acc_CheckSpendVoucherMaster>();
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

        public static int InsertUsingXML(Acc_CheckSpendVoucherMaster objRecord, bool IsNewRecord)
        {
            Int32 objRet = 0;
            string DitmeXML = ConvertObjectToXMLString(objRecord.CheckSpendVoucherDetails);
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Acc_CheckSpendVoucher_SP]";
                    objCmd.Parameters.Add(new SqlParameter("xmlData", SqlDbType.Xml)).Value = DitmeXML;
                    objCmd.Parameters.Add(new SqlParameter("@CheckSpendVoucherID", objRecord.CheckSpendVoucherID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@CheckSpendVoucherDate", objRecord.CheckSpendVoucherDate));
                    objCmd.Parameters.Add(new SqlParameter("@Notes", objRecord.Notes));
                    objCmd.Parameters.Add(new SqlParameter("@BeneficiaryName", objRecord.BeneficiaryName));
                    objCmd.Parameters.Add(new SqlParameter("@BankID", objRecord.BankID));
                    objCmd.Parameters.Add(new SqlParameter("@CreditAmount", objRecord.CreditAmount));
                    objCmd.Parameters.Add(new SqlParameter("@DiscountAccountID", objRecord.DiscountAccountID));
                    objCmd.Parameters.Add(new SqlParameter("@DiscountAmount", objRecord.DiscountAmount));
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceID", objRecord.InvoiceID));
                    objCmd.Parameters.Add(new SqlParameter("@DocumentID", objRecord.DocumentID));
                
                    objCmd.Parameters.Add(new SqlParameter("@WroteInCity", objRecord.WroteInCity));
                    objCmd.Parameters.Add(new SqlParameter("@GreDate", objRecord.GreDate));
                    objCmd.Parameters.Add(new SqlParameter("@HijriDate", objRecord.HijriDate));
                    objCmd.Parameters.Add(new SqlParameter("@UserID", objRecord.UserID));
                    objCmd.Parameters.Add(new SqlParameter("@RegDate", objRecord.RegDate));
                    objCmd.Parameters.Add(new SqlParameter("@RegTime", objRecord.RegTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                    objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditDate", objRecord.EditDate));
                    objCmd.Parameters.Add(new SqlParameter("@ComputerInfo", objRecord.ComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@EditComputerInfo", objRecord.EditComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@Cancel", objRecord.Cancel));
                    objCmd.Parameters.Add(new SqlParameter("@RegistrationNo", objRecord.RegistrationNo));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@CurrencyEquivalent", objRecord.CurrencyEquivalent));
                    objCmd.Parameters.Add(new SqlParameter("@CurrencyName", objRecord.CurrencyName));
                    objCmd.Parameters.Add(new SqlParameter("@CurrencyPrice", objRecord.CurrencyPrice));
                    objCmd.Parameters.Add(new SqlParameter("@CurrencyID", objRecord.CurrencyID));
                  //  objCmd.Parameters.Add(new SqlParameter("@DelegateID", objRecord.DelegateID));
                 //   objCmd.Parameters.Add(new SqlParameter("@SpendImage", objRecord.SpendImage));
                   // objCmd.Parameters.Add(new SqlParameter("@OperationTypeName ", objRecord.OperationTypeName));
                    if (IsNewRecord)
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

        public static Int32 InsertAcc_CheckSpendVoucherMaster(Acc_CheckSpendVoucherMaster objRecord)
        {
            Int32 objRet = 0;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Acc_CheckSpendVoucher_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@CheckSpendVoucherID", objRecord.CheckSpendVoucherID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@CheckSpendVoucherDate", objRecord.CheckSpendVoucherDate));
                    objCmd.Parameters.Add(new SqlParameter("@Notes", objRecord.Notes));
                    objCmd.Parameters.Add(new SqlParameter("@BeneficiaryName", objRecord.BeneficiaryName));
                    objCmd.Parameters.Add(new SqlParameter("@BankID", objRecord.BankID));
                    objCmd.Parameters.Add(new SqlParameter("@CreditAmount", objRecord.CreditAmount));
                    objCmd.Parameters.Add(new SqlParameter("@DiscountAccountID", objRecord.DiscountAccountID));
                    objCmd.Parameters.Add(new SqlParameter("@DiscountAmount", objRecord.DiscountAmount));
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceID", objRecord.InvoiceID));
                    objCmd.Parameters.Add(new SqlParameter("@DocumentID", objRecord.DocumentID));
               
                    objCmd.Parameters.Add(new SqlParameter("@WroteInCity", objRecord.WroteInCity));
                    objCmd.Parameters.Add(new SqlParameter("@GreDate", objRecord.GreDate));
                    objCmd.Parameters.Add(new SqlParameter("@HijriDate", objRecord.HijriDate));
                    objCmd.Parameters.Add(new SqlParameter("@UserID", objRecord.UserID));
                    objCmd.Parameters.Add(new SqlParameter("@RegDate", objRecord.RegDate));
                    objCmd.Parameters.Add(new SqlParameter("@RegTime", objRecord.RegTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                    objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditDate", objRecord.EditDate));
                    objCmd.Parameters.Add(new SqlParameter("@ComputerInfo", objRecord.ComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@EditComputerInfo", objRecord.EditComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@Cancel", objRecord.Cancel));
                    objCmd.Parameters.Add(new SqlParameter("@RegistrationNo", objRecord.RegistrationNo));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@CurrencyID", objRecord.CurrencyID));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 1));
                    object obj = objCmd.ExecuteScalar();
                    if (obj != null)
                        objRet = Convert.ToInt32(obj);
                }
            }
            return objRet;
        }

        public static long UpdateUsingXML(Acc_CheckSpendVoucherMaster objRecord, int USERCREATED)
        {
            Int32 objRet = 0;
            string DitmeXML = ConvertObjectToXMLString(objRecord.CheckSpendVoucherDetails);
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Acc_CheckSpendVoucher_SP]";
                    objCmd.Parameters.Add(new SqlParameter("xmlData", SqlDbType.Xml)).Value = DitmeXML;
                    objCmd.Parameters.Add(new SqlParameter("@CheckSpendVoucherID", objRecord.CheckSpendVoucherID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@CheckSpendVoucherDate", objRecord.CheckSpendVoucherDate));
                    objCmd.Parameters.Add(new SqlParameter("@Notes", objRecord.Notes));
                    objCmd.Parameters.Add(new SqlParameter("@BeneficiaryName", objRecord.BeneficiaryName));
                    objCmd.Parameters.Add(new SqlParameter("@BankID", objRecord.BankID));
                    objCmd.Parameters.Add(new SqlParameter("@CreditAmount", objRecord.CreditAmount));
                    objCmd.Parameters.Add(new SqlParameter("@DiscountAccountID", objRecord.DiscountAccountID));
                    objCmd.Parameters.Add(new SqlParameter("@DiscountAmount", objRecord.DiscountAmount));
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceID", objRecord.InvoiceID));
                    objCmd.Parameters.Add(new SqlParameter("@DocumentID", objRecord.DocumentID));
              
                    objCmd.Parameters.Add(new SqlParameter("@WroteInCity", objRecord.WroteInCity));
                    objCmd.Parameters.Add(new SqlParameter("@GreDate", objRecord.GreDate));
                    objCmd.Parameters.Add(new SqlParameter("@HijriDate", objRecord.HijriDate));
                    objCmd.Parameters.Add(new SqlParameter("@UserID", objRecord.UserID));
                    objCmd.Parameters.Add(new SqlParameter("@RegDate", objRecord.RegDate));
                    objCmd.Parameters.Add(new SqlParameter("@RegTime", objRecord.RegTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                    objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditDate", objRecord.EditDate));
                    objCmd.Parameters.Add(new SqlParameter("@ComputerInfo", objRecord.ComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@EditComputerInfo", objRecord.EditComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@Cancel", objRecord.Cancel));
                    objCmd.Parameters.Add(new SqlParameter("@RegistrationNo", objRecord.RegistrationNo));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@CurrencyID", objRecord.CurrencyID));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 1));
                    object obj = objCmd.ExecuteScalar();
                    if (obj != null)
                        objRet = Convert.ToInt32(obj);
                }
            }
            return objRet;

        }

        public static bool DeleteAcc_CheckSpendVoucherMaster(Acc_CheckSpendVoucherMaster objRecord)
        {
            bool objRet = false;
            objRet = false;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Acc_CheckSpendVoucher_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@CheckSpendVoucherID", objRecord.CheckSpendVoucherID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@EditDate", objRecord.EditDate));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 6));
                    objCmd.ExecuteNonQuery();
                }
            }
            objRet = true;
            return objRet;
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
                        objCmd.CommandText = "[Acc_CheckSpendVoucher_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@CheckSpendVoucherID", ID));
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
                {
                    ID = Comon.cLong(dt.Rows[0][0].ToString());
                    if (ID == 0)
                        ID = 1;
                }

                strSQL = "Select Top 1 StartFrom From StartNumbering Where BranchID=" + MySession.GlobalBranchID + " And FormName='frmCheckSpendVoucher'";
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

    }
}

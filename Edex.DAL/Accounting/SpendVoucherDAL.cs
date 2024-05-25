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
   public class SpendVoucherDAL
    {
        public static readonly string TableName = "Acc_SpendVoucherMaster";
        public static readonly string PremaryKey = "SpendVoucherID";
        public bool FoundResult;

        private string strSQL = "";
       public static Acc_SpendVoucherDetails ConvertRowToObj(DataRow dr)
        {

            Acc_SpendVoucherMaster ObjMaster = new Acc_SpendVoucherMaster();
            ObjMaster.SpendVoucherID = Comon.cInt(dr["SpendVoucherID"].ToString());
            ObjMaster.SpendVoucherDate = Comon.ConvertSerialDateTo(dr["SpendVoucherDate"].ToString());
            ObjMaster.CreditAccountID = Comon.cDbl(dr["CreditAccountID"].ToString());
            ObjMaster.CreditAmount = Comon.cDbl(dr["CreditAmount"].ToString());
            ObjMaster.BranchID = Comon.cInt(dr["BranchID"].ToString());
            ObjMaster.CurrencyID = Comon.cInt(dr["CurrencyID"].ToString());
            ObjMaster.DocumentID = Comon.cInt(dr["DocumentID"].ToString());
            ObjMaster.Notes = dr["Notes"].ToString();
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


            Acc_SpendVoucherDetails SaleDetalObject = new Acc_SpendVoucherDetails();
            SaleDetalObject.ID = Comon.cInt(dr["ID"].ToString());
            SaleDetalObject.SpendVoucherID = Comon.cInt(dr["SpendVoucherID"].ToString());
            SaleDetalObject.BranchID = Comon.cInt(dr["BranchID"].ToString());
            SaleDetalObject.FacilityID = Comon.cInt(dr["FacilityID"].ToString());
            SaleDetalObject.Declaration = dr["Declaration"].ToString();
            SaleDetalObject.CostCenterID = Comon.cInt(dr["CostCenterID"].ToString());
            SaleDetalObject.AccountID = Comon.cDbl(dr["AccountID"].ToString());
            SaleDetalObject.Discount = Comon.cDbl(dr["Discount"].ToString());
            SaleDetalObject.DebitAmount = Comon.cDbl(dr["DebitAmount"].ToString());
            SaleDetalObject.SpendVoucherMaster = ObjMaster;
            return SaleDetalObject;
        }
       public static DataTable frmGetDataDetalTransPortByID(long ID, int BranchID, int FacilityID)
       {
           try
           {
               using (SqlConnection objCnn = new GlobalConnection().Conn)
               {
                   objCnn.Open();
                   using (SqlCommand objCmd = objCnn.CreateCommand())
                   {
                       objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                       objCmd.CommandText = "[Acc_SpendVoucherTransPort_SP]";
                       objCmd.Parameters.Add(new SqlParameter("@SpendVoucherID", ID));
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
           catch (Exception ex)
           {
               return null;
           }
       }
       public static long InsertUsingXMLTransPortDetails(Acc_SpendVoucherMaster objRecord, bool IsNewRecord)
       {
           Int32 objRet = 0;
           string DitmeXML = ConvertObjectToXMLString(objRecord.SpendVoucherDetails);
           using (SqlConnection objCnn = new GlobalConnection().Conn)
           {
               objCnn.Open();
               using (SqlCommand objCmd = objCnn.CreateCommand())
               {
                   objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                   objCmd.CommandText = "[Acc_SpendVoucherTransPort_SP]";
                   objCmd.Parameters.Add(new SqlParameter("xmlData", SqlDbType.Xml)).Value = DitmeXML;
                   objCmd.Parameters.Add(new SqlParameter("@SpendVoucherID", objRecord.SpendVoucherID));
                   objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                   objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));

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
        public static Acc_SpendVoucherMaster ConvertRowToObjMaster(DataRow dr)
        {

            Acc_SpendVoucherMaster ObjMaster = new Acc_SpendVoucherMaster();

            ObjMaster.SpendVoucherID = Comon.cInt(dr["SpendVoucherID"].ToString());
            ObjMaster.SpendVoucherDate = Comon.ConvertSerialDateTo(dr["SpendVoucherDate"].ToString());
            ObjMaster.CreditAccountID = Comon.cDbl(dr["CreditAccountID"].ToString());
            ObjMaster.CreditAmount = Comon.cDbl(dr["CreditAmount"].ToString());    
            ObjMaster.BranchID = Comon.cInt(dr["BranchID"].ToString());
            ObjMaster.DocumentID = Comon.cInt(dr["DocumentID"].ToString());
            ObjMaster.Notes = dr["Notes"].ToString();
            ObjMaster.RegistrationNo  = Comon.cDbl(dr["RegistrationNo"].ToString());
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

        public static List<Acc_SpendVoucherDetails> GetDataDetalByID(int ID, int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Acc_SpendVoucher_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@SpendVoucherID", ID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 5));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);

                        if (dt != null)
                        {
                            List<Acc_SpendVoucherDetails> Returned = new List<Acc_SpendVoucherDetails>();
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

        public static Acc_SpendVoucherMaster GetDataMasterByID(int ID, int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Acc_SpendVoucher_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@SpendVoucherID", ID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 4));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            Acc_SpendVoucherMaster Returned = new Acc_SpendVoucherMaster();
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

        public static List<Acc_SpendVoucherMaster> GetAllMasterData(int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Acc_SpendVoucher_SP]";
                        objCmd.Parameters.AddWithValue("@BranchID", BranchID);
                        objCmd.Parameters.AddWithValue("@FacilityID", FacilityID);
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 3));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            List<Acc_SpendVoucherMaster> Returned = new List<Acc_SpendVoucherMaster>();
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

        public static long InsertUsingXML(Acc_SpendVoucherMaster objRecord, bool IsNewRecord)
        {
            Int32 objRet = 0;
            string DitmeXML = ConvertObjectToXMLString(objRecord.SpendVoucherDetails);
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Acc_SpendVoucher_SP]";
                    objCmd.Parameters.Add(new SqlParameter("xmlData", SqlDbType.Xml)).Value = DitmeXML;
                    objCmd.Parameters.Add(new SqlParameter("@SpendVoucherID", objRecord.SpendVoucherID));
                    objCmd.Parameters.Add(new SqlParameter("@SpendVoucherDate", objRecord.SpendVoucherDate));
                    objCmd.Parameters.Add(new SqlParameter("@CreditGoldAccountID", objRecord.CreditGoldAccountID));
                    objCmd.Parameters.Add(new SqlParameter("@VatAccountID", objRecord.VatAccountID));
                    objCmd.Parameters.Add(new SqlParameter("@CreditAccountID", objRecord.CreditAccountID));
                    objCmd.Parameters.Add(new SqlParameter("@TotalGold", objRecord.TotalGold));

                    objCmd.Parameters.Add(new SqlParameter("@PaidGold", objRecord.PaidGold));
                    objCmd.Parameters.Add(new SqlParameter("@PaidDiamond", objRecord.PaidDiamond));
                    objCmd.Parameters.Add(new SqlParameter("@PaidOjore", objRecord.PaidOjore));

                    objCmd.Parameters.Add(new SqlParameter("@AmountForDiamond", objRecord.AmountForDiamond));
                    objCmd.Parameters.Add(new SqlParameter("@AmountForGold", objRecord.AmountForGold));
                    

                    objCmd.Parameters.Add(new SqlParameter("@CreditAmount", objRecord.CreditAmount));



                    objCmd.Parameters.Add(new SqlParameter("@VatAmountTotal", objRecord.VatAmountTotal));
                 

                    objCmd.Parameters.Add(new SqlParameter("@DiscountAccountID", objRecord.DiscountAccountID));
                    objCmd.Parameters.Add(new SqlParameter("@DiscountAmount", objRecord.DiscountAmount));
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceID", objRecord.InvoiceID));
                    objCmd.Parameters.Add(new SqlParameter("@DocumentID", objRecord.DocumentID));
                    objCmd.Parameters.Add(new SqlParameter("@SpendImage", objRecord.SpendImage));
                    objCmd.Parameters.Add(new SqlParameter("@RegistrationNo ", objRecord.RegistrationNo));
                    objCmd.Parameters.Add(new SqlParameter("@CurrencyID", objRecord.CurencyID));
                    objCmd.Parameters.Add(new SqlParameter("@CurrencyEquivalent", objRecord.CurrencyEquivalent));
                    objCmd.Parameters.Add(new SqlParameter("@CurrencyName", objRecord.CurrencyName));
                    objCmd.Parameters.Add(new SqlParameter("@CurrencyPrice", objRecord.CurrencyPrice));
                    objCmd.Parameters.Add(new SqlParameter("@Notes", objRecord.Notes));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@UserID", objRecord.UserID));
                    objCmd.Parameters.Add(new SqlParameter("@RegDate", objRecord.RegDate));
                    objCmd.Parameters.Add(new SqlParameter("@RegTime", objRecord.RegTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                    objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditDate", objRecord.EditDate));
                    objCmd.Parameters.Add(new SqlParameter("@ComputerInfo", objRecord.ComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@EditComputerInfo", objRecord.EditComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@DelegateID", objRecord.DelegateID));
                    objCmd.Parameters.Add(new SqlParameter("@Cancel", objRecord.Cancel));
                    objCmd.Parameters.Add(new SqlParameter("@Posted", objRecord.Posted));
                    objCmd.Parameters.Add(new SqlParameter("@OperationTypeName", 1));
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

        public static Int32 InsertAcc_SpendVoucherMaster(Acc_SpendVoucherMaster objRecord)
        {
            Int32 objRet = 0;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Acc_SpendVoucher_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@SpendVoucherID", objRecord.SpendVoucherID));
        
                    objCmd.Parameters.Add(new SqlParameter("@SpendVoucherDate", objRecord.SpendVoucherDate));
                    objCmd.Parameters.Add(new SqlParameter("@CreditAccountID", objRecord.CreditAccountID));
                    objCmd.Parameters.Add(new SqlParameter("@CreditAmount", objRecord.CreditAmount));
                    objCmd.Parameters.Add(new SqlParameter("@DiscountAccountID", objRecord.DiscountAccountID));
                    objCmd.Parameters.Add(new SqlParameter("@DiscountAmount", objRecord.DiscountAmount));
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceID", objRecord.InvoiceID));
                    objCmd.Parameters.Add(new SqlParameter("@SpendImage", objRecord.SpendImage));
                    objCmd.Parameters.Add(new SqlParameter("@CurrencyID", objRecord.CurrencyID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@Notes", objRecord.Notes));
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
                    objCmd.Parameters.Add(new SqlParameter("@RegistrationNo ", objRecord.RegistrationNo));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 1));
                    object obj = objCmd.ExecuteScalar();
                    if (obj != null)
                        objRet = Convert.ToInt32(obj);
                }
            }
            return objRet;
        }

        public static long UpdateUsingXML(Acc_SpendVoucherMaster objRecord, int USERCREATED)
        {
            Int32 objRet = 0;
            string DitmeXML = ConvertObjectToXMLString(objRecord.SpendVoucherDetails);
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Acc_SpendVoucher_SP]";
                    objCmd.Parameters.Add(new SqlParameter("xmlData", SqlDbType.Xml)).Value = DitmeXML;
                    objCmd.Parameters.Add(new SqlParameter("@SpendVoucherID", objRecord.SpendVoucherID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@SpendVoucherDate", objRecord.SpendVoucherDate));
                    objCmd.Parameters.Add(new SqlParameter("@CreditAccountID", objRecord.CreditAccountID));
                    objCmd.Parameters.Add(new SqlParameter("@CreditAmount", objRecord.CreditAmount));
                    objCmd.Parameters.Add(new SqlParameter("@DiscountAccountID", objRecord.DiscountAccountID));
                    objCmd.Parameters.Add(new SqlParameter("@DiscountAmount", objRecord.DiscountAmount));
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceID", objRecord.InvoiceID));
                    objCmd.Parameters.Add(new SqlParameter("@SpendImage", objRecord.SpendImage));
                    objCmd.Parameters.Add(new SqlParameter("@CurrencyID", objRecord.CurrencyID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@Notes", objRecord.Notes));
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
                    objCmd.Parameters.Add(new SqlParameter("@RegistrationNo ", objRecord.RegistrationNo));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 1));
                    object obj = objCmd.ExecuteScalar();
                    if (obj != null)
                        objRet = Convert.ToInt32(obj);
                }
            }
            return objRet;

        }

        public static int DeleteAcc_SpendVoucherMaster(Acc_SpendVoucherMaster objRecord)
        {
             
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Acc_SpendVoucher_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@SpendVoucherID", objRecord.SpendVoucherID));
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
                        objCmd.CommandText = "[Acc_SpendVoucher_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@SpendVoucherID", ID));
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

        public static long GetNewID(int BranchID)
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
                    if (ID == 0)
                        ID = 1;
                }


                strSQL = "Select Top 1 StartFrom From StartNumbering Where BranchID=" + MySession.GlobalBranchID + " And FormName='frmSpendVoucher'";
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

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

namespace Edex.DAL.SalseSystem.Stc_itemDAL
{
   public class Stc_GoldOutOnBailDAL
    {
    

        public static readonly string TableName = "Stc_GoldOutOnBail_Master";
        public static readonly string PremaryKey = "InvoiceID";
        public bool FoundResult;
        public bool NeedSaving;
        public bool IsNewRecord;

        private DataTable dt;
        private string strSQL;
        private object Result;

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

        public static int InsertUsingXML(Stc_GoldOutOnBail_Master objRecord, bool IsNewRecord)
        {
            Int32 objRet = 0;
            string DitmeXML = ConvertObjectToXMLString(objRecord.GoldOutOnBailDatails);
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Stc_GoldOutOnBail_Master_SP]";
                    objCmd.Parameters.Add(new SqlParameter("xmlData", SqlDbType.Xml)).Value = DitmeXML;
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceID", objRecord.InvoiceID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceDate", objRecord.InvoiceDate));
                    objCmd.Parameters.Add(new SqlParameter("@CurrencyID", objRecord.CurrencyID));
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceEquivalenTotal", objRecord.InvoiceEquivalenTotal));
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceGoldTotal", objRecord.InvoiceTotalGold));

                    

                    objCmd.Parameters.Add(new SqlParameter("@CostCenterID", objRecord.CostCenterID));
                    objCmd.Parameters.Add(new SqlParameter("@StoreID", objRecord.StoreID));

                    objCmd.Parameters.Add(new SqlParameter("@Notes", objRecord.Notes));

                    objCmd.Parameters.Add(new SqlParameter("@CreditAccount", objRecord.CreditAccount));

                    objCmd.Parameters.Add(new SqlParameter("@DocumentID", objRecord.DocumentID));

                    objCmd.Parameters.Add(new SqlParameter("@UserID", objRecord.UserID));
                    objCmd.Parameters.Add(new SqlParameter("@RegDate", objRecord.RegDate));
                    objCmd.Parameters.Add(new SqlParameter("@RegTime", objRecord.RegTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                    objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditDate", objRecord.EditDate));
                    objCmd.Parameters.Add(new SqlParameter("@ComputerInfo", objRecord.ComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@EditComputerInfo", objRecord.EditComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@Posted", objRecord.Posted));

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

        public static int Delete(Stc_GoldOutOnBail_Master objRecord)
        {

            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Stc_GoldOutOnBail_Master_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@InvoiceID", objRecord.InvoiceID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                    objCmd.Parameters.Add(new SqlParameter("@EditDate", objRecord.EditDate));
                    objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));

                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 4));

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

        public static DataTable frmGetDataDetailByID(long ID, int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Stc_GoldOutOnBail_Master_SP]";
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

        public static long GetNewID(int FacilityID, int BranchID, int UerID)
        {
            try
            {
                long ID = 0;
                DataTable dt;
                string strSQL;

                strSQL = "SELECT Max(" + PremaryKey + ")+1 FROM Stc_GoldOutOnBail_Master Where  BranchID =" + MySession.GlobalBranchID;
                dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count > 0)
                {
                    ID = Comon.cLong(dt.Rows[0][0].ToString());
                    if (dt.Rows[0][0].ToString() == "")
                        ID = 1;
                }

                strSQL = "Select Top 1 StartFrom From StartNumbering Where BranchID=" + MySession.GlobalBranchID
                    + " And FormName='frmGoodsOpening'";
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

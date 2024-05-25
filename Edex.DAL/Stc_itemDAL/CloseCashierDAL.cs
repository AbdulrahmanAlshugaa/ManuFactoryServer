using Edex.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Edex.DAL.Stc_itemDAL
{
    public class CloseCashierDAL
    {
        public static Stc_ItemsBrands ConvertRowToObj(DataRow dr)
        {
            Stc_ItemsBrands Obj = new Stc_ItemsBrands();
            Obj.BrandID = int.Parse(dr["BrandID"].ToString());
            Obj.ArbName = dr["ArbName"].ToString();
            Obj.EngName = dr["EngName"].ToString();
            Obj.UserID = int.Parse(dr["UserID"].ToString());
            Obj.Cancel = int.Parse(dr["Cancel"].ToString());
            Obj.EditUserID = Comon.cInt(dr["EditUserID"].ToString());
            Obj.EditDate = Comon.cLong(dr["EditDate"].ToString());
            Obj.EditTime = Comon.cLong(dr["EditTime"].ToString());
            Obj.ComputerInfo = dr["ComputerInfo"].ToString();
            Obj.EditComputerInfo = dr["EditComputerInfo"].ToString();
            //Obj.RegDate = Comon.ConvertSerialDateTo(long.Parse(dr["RegDate"].ToString()));
            //Obj.EditDate = Com.ConvertSerialToDate(long.Parse(dr["EditDate"].ToString()));

            return Obj;
        }

        public static Stc_ItemsBrands GetDataByID(int BrandID)
        {
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Stc_ItemsBrands_sp]";
                    objCmd.Parameters.Add(new SqlParameter("@BrandID", BrandID));

                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 5));
                    SqlDataReader myreader = objCmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(myreader);
                    if (dt != null)
                    {
                        Stc_ItemsBrands Returned = new Stc_ItemsBrands();
                        Returned = (ConvertRowToObj(dt.Rows[0]));
                        return Returned;
                    }
                    else
                        return null;
                }
            }
        }
        public static List<Stc_ItemsBrands> GetAllData()
        {
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Stc_ItemsBrands_sp]";

                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 3));
                    SqlDataReader myreader = objCmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(myreader);
                    if (dt != null)
                    {
                        List<Stc_ItemsBrands> Returned = new List<Stc_ItemsBrands>();
                        foreach (DataRow rows in dt.Rows)
                            Returned.Add(ConvertRowToObj(rows));
                        return Returned;
                    }
                    else
                        return null;
                }
            }
        }
        public static Int32 Insert(SalesCashierClose objRecord)
        {
            Int32 objRet = 0;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[SalesCashierClose_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@CloseCashierDate", objRecord.CloseCashierDate));
                    objCmd.Parameters.Add(new SqlParameter("@CloseCashierID", objRecord.CloseCashierID));
                    objCmd.Parameters.Add(new SqlParameter("@EnterCost", objRecord.EnterCost));
                    objCmd.Parameters.Add(new SqlParameter("@CashSum", objRecord.CashSum));
                    objCmd.Parameters.Add(new SqlParameter("@FutureSum", objRecord.FutureSum));
                    objCmd.Parameters.Add(new SqlParameter("@PrevoiusCash", objRecord.PrevoiusCash));
                    objCmd.Parameters.Add(new SqlParameter("@NetSum", objRecord.NetSum));
                    objCmd.Parameters.Add(new SqlParameter("@SellerID", objRecord.SellerID));

                    objCmd.Parameters.Add(new SqlParameter("@FromSaleInvoice", objRecord.FromSaleInvoice));
                    objCmd.Parameters.Add(new SqlParameter("@ToSaleInvoice ", objRecord.ToSaleInvoice));
                    objCmd.Parameters.Add(new SqlParameter("@FromSaleInvoiceReturn", objRecord.FromSaleInvoiceReturn));
                    objCmd.Parameters.Add(new SqlParameter("@ToSaleInvoiceReturn", objRecord.ToSaleInvoiceReturn));

                    objCmd.Parameters.Add(new SqlParameter("@UserID", objRecord.UserID));
                    objCmd.Parameters.Add(new SqlParameter("@WasteCost", objRecord.WasteCost));

                    object obj = objCmd.ExecuteScalar();
                    if (obj != null)
                        return objRet;
                }
            }
            return objRet;
        }
        public static bool Update(Stc_ItemsBrands objRecord)
        {
            bool objRet = false;
            objRet = false;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Stc_ItemsBrands_sp]";
                    objCmd.Parameters.Add(new SqlParameter("@BrandID", objRecord.BrandID));
                    objCmd.Parameters.Add(new SqlParameter("@ArbName", objRecord.ArbName));
                    objCmd.Parameters.Add(new SqlParameter("@EngName", objRecord.EngName));

                    objCmd.Parameters.Add(new SqlParameter("@UserID", objRecord.UserID));
                    objCmd.Parameters.Add(new SqlParameter("@RegDate", objRecord.RegDate));
                    objCmd.Parameters.Add(new SqlParameter("@RegTime", objRecord.RegTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                    objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditDate", objRecord.EditDate));
                    objCmd.Parameters.Add(new SqlParameter("@EditComputerInfo", objRecord.EditComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@Cancel", objRecord.Cancel));

                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 2));
                    objCmd.ExecuteNonQuery();
                }
            }
            objRet = true;
            return objRet;
        }
        public static bool Delete(Stc_ItemsBrands objRecord)
        {
            bool objRet = false;
            objRet = false;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Stc_ItemsBrands_sp]";
                    objCmd.Parameters.Add(new SqlParameter("@BrandID", objRecord.BrandID));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                    objCmd.Parameters.Add(new SqlParameter("@editdate", objRecord.EditDate));
                    objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 4));
                    objCmd.ExecuteNonQuery();
                }
            }
            objRet = true;
            return objRet;
        }
    }
}

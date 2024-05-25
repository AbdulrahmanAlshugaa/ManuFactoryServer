using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.DAL.Common
{
    public class GetMaxID
    {
        /// <summary>
        /// This function is to query the Max ID of all the tables mentioned in the function
        /// </summary>
        /// <param name="inputNo"></param>
        /// <param name="BranchID"></param>
        /// <param name="FacilityID"></param>
        /// <returns>The function returns the Max ID </returns>
        public static Int32 MaxID(string inputNo, int BranchID, int FacilityID)
        {
            Int32 objRet = 0;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[GetMaxID_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@inputNo", BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                    if ("ItemID" == inputNo)
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 1));
                    if ("GroupItemsID" == inputNo)
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 2));
                    if ("TypesID" == inputNo)
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 3));
                    if ("SizingUnitsID" == inputNo)
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 4));
                    if ("StoresID" == inputNo)
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 5));
                    if ("SellersID" == inputNo)
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 6));
                    if ("CustomersID" == inputNo)
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 7));
                    if ("SuppliersID" == inputNo)
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 8));
                    if ("GroupCustomersID" == inputNo)
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 9));
                    if ("GroupSuppliersID" == inputNo)
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 10));
                    if ("SalesDelegateID" == inputNo)
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 11));
                    if ("PurchasesDelegateID" == inputNo)
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 12));
                    if ("CostCenterID" == inputNo)
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 13));
                    if ("BranchID" == inputNo)
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 14));
                    if ("CurrencyID" == inputNo)
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 15));
                    if ("PurchaseInvoiceID" == inputNo)
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 16));
                    if ("PurchaseInvoiceReturnID" == inputNo)
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 17));
                    if ("SaleInvoiceID" == inputNo)
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 18));
                    if ("SaleInvoiceReturnID" == inputNo)
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 19));
                    if ("SpendVoucherID" == inputNo)
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 20));
                    if ("ReceiptVoucherID" == inputNo)
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 21));
                    if ("CheckSpendVoucherID" == inputNo)
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 22));
                    if ("CheckReceiptVoucherID" == inputNo)
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 23));
                    if ("UserID" == inputNo)
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 30));
                    if ("InID" == inputNo)
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 31));
                    if ("OutID" == inputNo)
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 32));


                    object obj = objCmd.ExecuteScalar();
                    if (obj != null)
                        objRet = Convert.ToInt32(obj);
                }
            }
            return objRet;
        }

        /// <summary>
        /// This function inquires about the remaining quantity of any item.
        /// </summary>
        /// <param name="Barcode"></param>
        /// <param name="StoreID"></param>
        /// <param name="BranchID"></param>
        /// <param name="FacilityID"></param>
        /// <returns>The function returns the remaining quantity</returns>
        public static double GetRemindQty(string Barcode, int StoreID, int BranchID, int FacilityID)
        {
            Int32 objRet = 0;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[RemindQty_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@StoreID", StoreID));
                    objCmd.Parameters.Add(new SqlParameter("@Barcode_P", Barcode));
                    object obj = objCmd.ExecuteScalar();
                    if (obj != null)
                        objRet = Convert.ToInt32(obj);
                }
            }
            return objRet;
        }

    }
}

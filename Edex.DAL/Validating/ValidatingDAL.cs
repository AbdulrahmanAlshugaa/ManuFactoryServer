using Edex.DAL.Common;
using Edex.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.DAL.Validating
{
    public class ValidatingDAL
    {
        public delegate ClassManyFields ConvertRowToObj(DataRow dr);

        public static ClassManyFields ConvertRowToObjWithTwoColumns(DataRow dr)
        {
            ClassManyFields ClassManyFields = new ClassManyFields();
            ClassManyFields.ID = Comon.cLong(dr["ID"].ToString());
            ClassManyFields.Name = dr["Name"].ToString();
            return ClassManyFields;
        }
        public static ClassManyFields ConvertRowToObjWithThreeColumns(DataRow dr)
        {
            ClassManyFields ClassManyFields = new ClassManyFields();
            ClassManyFields.ID = Comon.cLong(dr["ID"].ToString());
            ClassManyFields.Name = dr["Name"].ToString();
            ClassManyFields.StringField_3 = dr["Col_3"].ToString();
            return ClassManyFields;
        }
        public static ClassManyFields GetName(double ID, string Form, int BranchID, int FacilityID)
        {
            try
            {
                ConvertRowToObj ConvertRowToObj = ConvertRowToObjWithTwoColumns;
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Validating_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@ID", ID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        if (Form == "AccountName")
                            objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 1));
                        else if (Form == "ItemGroupName")
                            objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 2));
                        else if (Form == "ItemTypeName")
                            objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 3));
                        else if (Form == "ItemSizeName")
                            objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 4));
                        else if (Form == "ItemBrandName")
                            objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 5));
                        else if (Form == "ItemColorName")
                            objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 6));
                        else if (Form == "StoreName")
                            objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 7));
                        else if (Form == "SupplierName")
                        {
                            ConvertRowToObj = ConvertRowToObjWithThreeColumns;
                            objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 8));
                        }
                        else if (Form == "CostCenterName")
                            objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 9));
                        else if (Form == "PurchasesDelegateName"|| Form == "PurchaseDelegateName")
                            objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 10));
                        else if (Form == "SalesDelegateName" || Form == "SaleDelegateName")
                            objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 14));
                        else if (Form == "SalesSellerName" || Form == "SellerName")
                            objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 15));
                        else if (Form == "SalesCustomerName" || Form == "CustomerName")
                            objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 16));
                        else if (Form == "BankName")
                            objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 17));
                        else if (Form == "InID")
                            objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 18));
                        else if (Form == "RoleName")
                            objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 19));
                        else if (Form == "UserName")
                            objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 20));
                        if ("PermissionID" == Form)
                            objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 21));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            ClassManyFields Returned = new ClassManyFields();
                            Returned = ConvertRowToObj(dt.Rows[0]);
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
        public static int GetMaxItemID(string inputNo, int BranchID, int FacilityID)
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


                    object obj = objCmd.ExecuteScalar();
                    if (obj != null)
                        objRet = Convert.ToInt32(obj);
                }
            }
            return objRet;
        }

    }
}

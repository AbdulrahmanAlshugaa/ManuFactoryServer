using Edex.DAL.Common;
using Edex.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.DAL.Popup
{

    public class PopupDAL
    {

        public delegate ClassManyFields ConvertRowToObj(DataRow dr);

        public static ClassManyFields ConvertRowToObjWithTwoColumns(DataRow dr)
        {
            ClassManyFields ClassManyFields = new ClassManyFields();
            ClassManyFields.ID = Comon.cDbl(dr["ID"].ToString());
            ClassManyFields.Name = dr["Name"].ToString();
            return ClassManyFields;
        }
        public static ClassManyFields ConvertRowToObjWithThreeColumns(DataRow dr)
        {
            ClassManyFields ClassManyFields = new ClassManyFields();
            ClassManyFields.ID = Comon.cDbl(dr["ID"].ToString());
            ClassManyFields.Name = dr["Name"].ToString();
            ClassManyFields.StringField_3 = dr["Col_3"].ToString();
            return ClassManyFields;
        }

        public static List<ClassManyFields> GetAllData(string Form, int BranchID, int FacilityID)
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
                        objCmd.CommandText = "[Popup_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@Form", Form));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
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
                        else if (Form == "CustomerName" || Form == "SalesCustomerName")
                        {
                            ConvertRowToObj = ConvertRowToObjWithThreeColumns;
                            objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 16));
                        }

                        else if (Form == "CostCenterName")
                            objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 9));
                        else if (Form == "PurchasesDelegateName" || Form == "PurchaseDelegateName")
                            objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 10));
                        else if (Form == "SalesDelegateName" || Form == "SaleDelegateName")
                            objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 14));
                        else if (Form == "SalesSellerName" || Form == "SellerName")
                            objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 15));
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
                        if (dt != null)
                        {
                            List<ClassManyFields> Returned = new List<ClassManyFields>();
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


        public static  DataTable GetAllDataAsDataTable(string Form, int BranchID, int FacilityID)
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
                        objCmd.CommandText = "[Popup_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@Form", Form));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
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
                        else if (Form == "CustomerName" || Form == "SalesCustomerName")
                        {
                            ConvertRowToObj = ConvertRowToObjWithThreeColumns;
                            objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 16));
                        }

                        else if (Form == "CostCenterName")
                            objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 9));
                        else if (Form == "PurchasesDelegateName" || Form == "PurchaseDelegateName")
                            objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 10));
                        else if (Form == "SalesDelegateName" || Form == "SaleDelegateName")
                            objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 14));
                        else if (Form == "SalesSellerName" || Form == "SellerName")
                            objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 15));
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
 
    }
}

using Edex.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.DAL.Configuration
{
    public class GeneralSettingsDAL
    {
        public static GeneralSettings ConvertRowToObj(DataRow dr)
        {
            GeneralSettings Obj = new GeneralSettings();
            Obj.UsingExpiryDate = Comon.cInt(dr["UsingExpiryDate"].ToString());
            Obj.PriceDigits = Comon.cInt(dr["PriceDigits"].ToString());
            Obj.QtyDigits = Comon.cInt(dr["QtyDigits"].ToString());
            Obj.ItemDigits = Comon.cInt(dr["ItemDigits"].ToString());
            Obj.ItemPriceDigits = Comon.cInt(dr["ItemPriceDigits"].ToString()); ;
            Obj.BranchID = Comon.cInt(dr["BranchID"].ToString());
            Obj.FacilityID = Comon.cInt(dr["FacilityID"].ToString()); ;
            Obj.AllowedPercentDiscount = Comon.cDbl(dr["AllowedPercentDiscount"].ToString());
            Obj.WayOfOutItems = dr["WayOfOutItems"].ToString();
            Obj.CalcStockBy = dr["CalcStockBy"].ToString();
            Obj.ItemProfit = Comon.cInt(dr["ItemProfit"].ToString());
            Obj.MaxBarcodeDigits = Comon.cInt(dr["MaxBarcodeDigits"].ToString());
            Obj.AutoCalcFixAssetsDepreciation = Comon.cInt(dr["AutoCalcFixAssetsDepreciation"].ToString());
            Obj.UsingItemsSerials = Comon.cInt(dr["UsingItemsSerials"].ToString());
            Obj.DepreciationType = Comon.cInt(dr["DepreciationType"].ToString());
            Obj.BackupPath = dr["BackupPath"].ToString();
            Obj.InventoryType =Comon.cInt( dr["InventoryType"].ToString());
            Obj.CostCalculationType = Comon.cInt(dr["CostCalculationType"].ToString());
            return Obj;
        }

        public static GeneralSettings GetDataByID(int ID, int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[GeneralSettings_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@ID  ", ID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 3));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            GeneralSettings Returned = new GeneralSettings();
                            Returned = (ConvertRowToObj(dt.Rows[0]));
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

        public static List<GeneralSettings> GetAllData(int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[GeneralSettings_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 5));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            List<GeneralSettings> Returned = new List<GeneralSettings>();
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

        public DataTable GetGeneralSettings(int FacilityID, int BranchID)
        {
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[GeneralSettings_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 5));
                    SqlDataReader myreader = objCmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(myreader);
                    return dt;
                }
            }
        }

        public static Int32 InsertGeneralSettings(GeneralSettings objRecord)
        {
            Int32 objRet = 0;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[GeneralSettings_SP]";

                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@PriceDigits", objRecord.PriceDigits));
                    objCmd.Parameters.Add(new SqlParameter("@QtyDigits", objRecord.QtyDigits));
                    objCmd.Parameters.Add(new SqlParameter("@ItemPriceDigits", objRecord.ItemPriceDigits));
                    objCmd.Parameters.Add(new SqlParameter("@ItemDigits", objRecord.ItemDigits));
                    objCmd.Parameters.Add(new SqlParameter("@UsingExpiryDate", objRecord.UsingExpiryDate));
                    objCmd.Parameters.Add(new SqlParameter("@AllowedPercentDiscount", objRecord.AllowedPercentDiscount));
                    objCmd.Parameters.Add(new SqlParameter("@ItemProfit", objRecord.ItemProfit));
                    objCmd.Parameters.Add(new SqlParameter("@CalcStockBy", objRecord.CalcStockBy));
                    objCmd.Parameters.Add(new SqlParameter("@MaxBarcodeDigits", objRecord.MaxBarcodeDigits));
                    objCmd.Parameters.Add(new SqlParameter("@WayOfOutItems", objRecord.WayOfOutItems));
                    objCmd.Parameters.Add(new SqlParameter("@AutoCalcFixAssetsDepreciation", objRecord.AutoCalcFixAssetsDepreciation));
                    objCmd.Parameters.Add(new SqlParameter("@UsingItemsSerials", objRecord.UsingItemsSerials));
                    objCmd.Parameters.Add(new SqlParameter("@BackupPath", objRecord.BackupPath));
                    objCmd.Parameters.Add(new SqlParameter("@DepreciationType", objRecord.DepreciationType));

                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 1));
                    object obj = objCmd.ExecuteScalar();
                    if (obj != null)
                        objRet = Convert.ToInt32(obj);
                }
            }
            return objRet;
        }

        public static bool UpdateGeneralSettings(GeneralSettings objRecord)
        {
            bool objRet = false;
            
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[GeneralSettings_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@ID",0));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@PriceDigits", objRecord.PriceDigits));
                    objCmd.Parameters.Add(new SqlParameter("@QtyDigits", objRecord.QtyDigits));
                    objCmd.Parameters.Add(new SqlParameter("@ItemPriceDigits", objRecord.ItemPriceDigits));
                    objCmd.Parameters.Add(new SqlParameter("@ItemDigits", objRecord.ItemDigits));
                    objCmd.Parameters.Add(new SqlParameter("@UsingExpiryDate", objRecord.UsingExpiryDate));
                    objCmd.Parameters.Add(new SqlParameter("@AllowedPercentDiscount", objRecord.AllowedPercentDiscount));
                    objCmd.Parameters.Add(new SqlParameter("@ItemProfit", objRecord.ItemProfit));
                    objCmd.Parameters.Add(new SqlParameter("@CalcStockBy", objRecord.CalcStockBy));
                    objCmd.Parameters.Add(new SqlParameter("@MaxBarcodeDigits", objRecord.MaxBarcodeDigits));
                    objCmd.Parameters.Add(new SqlParameter("@WayOfOutItems", objRecord.WayOfOutItems));
                    objCmd.Parameters.Add(new SqlParameter("@AutoCalcFixAssetsDepreciation", objRecord.AutoCalcFixAssetsDepreciation));
                    objCmd.Parameters.Add(new SqlParameter("@UsingItemsSerials", objRecord.UsingItemsSerials));
                    objCmd.Parameters.Add(new SqlParameter("@BackupPath", objRecord.BackupPath));
                    objCmd.Parameters.Add(new SqlParameter("@DepreciationType", objRecord.DepreciationType));
                    objCmd.Parameters.Add(new SqlParameter("@InventoryType", objRecord.InventoryType));
                    objCmd.Parameters.Add(new SqlParameter("@CostCalculationType", objRecord.CostCalculationType));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 1));
                    objCmd.ExecuteNonQuery();
                }
            }
            objRet = true;
            return objRet;
        }

        public static bool DeleteGeneralSettings(GeneralSettings objRecord)
        {
            bool objRet = false;
            objRet = false;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[GeneralSettings_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@ID", 0));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 4));
                       objCmd.ExecuteNonQuery();
                }
            }
            objRet = true;
            return objRet;
        }
    }
}

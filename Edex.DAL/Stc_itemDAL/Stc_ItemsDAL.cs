using Edex.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Xml.Serialization;

namespace Edex.DAL.Stc_itemDAL
{
    public class Stc_itemsDAL
    {
        #region Declare
        public static readonly string TableName = "Stc_Items";
        public static readonly string PremaryKey = "ItemID";
        public bool FoundResult;
        public bool NeedSaving;
        public bool IsNewRecord;
        private DataTable dt;
        private string strSQL;
        private object Result;
        #endregion

        /// <summary>
        /// This Function is used to Convert DataRow to object  Stc_ItemUnits
        /// </summary>
        /// <param name="dr"></param>
        /// <returns> return object Stc_ItemUnits </returns>
        public static Stc_ItemUnits ConvertRowToObj(DataRow dr)
        {
            Stc_Items Stc_Items = new Stc_Items();
            Stc_Items.ItemID = Comon.cInt(dr["ItemID"].ToString());
            Stc_Items.ArbName = dr["ArbName"].ToString();
            Stc_Items.EngName = dr["EngName"].ToString();
            Stc_Items.GroupID = Comon.cInt(dr["GroupID"].ToString());
            Stc_Items.TypeID = Comon.cInt(dr["TypeID"].ToString());
            Stc_Items.Notes = dr["Notes"].ToString();
            Stc_Items.IsVAT = Comon.cInt(dr["IsVat"].ToString());
            Stc_Items.UserID = Comon.cInt(dr["UserID"].ToString());
            Stc_Items.Cancel = Comon.cInt(dr["Cancel"].ToString());
            Stc_Items.EditDate = Comon.cLong(dr["RegDate"].ToString());
            Stc_Items.EditTime = Comon.cLong(dr["RegTime"].ToString());
            Stc_Items.EditUserID = Comon.cInt(dr["EditUserID"].ToString());
            Stc_Items.EditDate = Comon.cLong(dr["EditDate"].ToString());
            Stc_Items.EditTime = Comon.cLong(dr["EditTime"].ToString());
            Stc_Items.ComputerInfo = dr["ComputerInfo"].ToString();
            Stc_Items.EditComputerInfo = dr["EditComputerInfo"].ToString();
            Stc_Items.BranchID = Comon.cInt(dr["BranchID"].ToString());
            Stc_Items.FacilityID = Comon.cInt(dr["FacilityID"].ToString());


            Stc_ItemUnits Stc_ItemUnits = new Stc_ItemUnits();
            Stc_ItemUnits.ID = Comon.cInt(dr["ID"].ToString());
            Stc_ItemUnits.ItemID = Comon.cInt(dr["ItemID"].ToString());
            Stc_ItemUnits.SizeID = Comon.cInt(dr["SizeID"].ToString());
            Stc_ItemUnits.CostPrice = Comon.ConvertToDecimalPrice(dr["CostPrice"].ToString());
            Stc_ItemUnits.BarCode = dr["BarCode"].ToString();
            Stc_ItemUnits.ItemProfit = Comon.ConvertToDecimalPrice(dr["ItemProfit"].ToString());
            Stc_ItemUnits.PackingQty = Comon.ConvertToDecimalQty(dr["PackingQty"].ToString());
            Stc_ItemUnits.MaxLimitQty = Comon.ConvertToDecimalQty(dr["MaxLimitQty"].ToString());
            Stc_ItemUnits.MinLimitQty = Comon.ConvertToDecimalQty(dr["MinLimitQty"].ToString());
            Stc_ItemUnits.LastCostPrice = Comon.ConvertToDecimalPrice(dr["LastCostPrice"].ToString());
            Stc_ItemUnits.LastSalePrice = Comon.ConvertToDecimalPrice(dr["LastSalePrice"].ToString());
            Stc_ItemUnits.SalePrice = Comon.ConvertToDecimalPrice(dr["SalePrice"].ToString());
            Stc_ItemUnits.SpecialCostPrice = Comon.ConvertToDecimalPrice(dr["SpecialCostPrice"].ToString());
            Stc_ItemUnits.SpecialSalePrice = Comon.ConvertToDecimalPrice(dr["SpecialSalePrice"].ToString());
            Stc_ItemUnits.UnitCancel = Comon.cInt(dr["UnitCancel"].ToString());
            Stc_ItemUnits.AverageCostPrice = Comon.ConvertToDecimalPrice(dr["AverageCostPrice"].ToString());
            Stc_ItemUnits.AllowedPercentDiscount = Comon.cDbl(dr["AllowedPercentDiscount"].ToString());
            Stc_ItemUnits.Stc_Items = Stc_Items;
            return Stc_ItemUnits;
        }
       
        /// <summary>
        /// This [Function is used to convert row object master
        /// </summary>
        /// <param name="dr"></param>
        /// <returns> return object Stc_Items </returns>
        public static Stc_Items ConvertRowToObjMaster(DataRow dr)
        {
            Stc_Items Stc_Items = new Stc_Items();
            Stc_Items.ItemID = Comon.cInt(dr["ItemID"].ToString());
            Stc_Items.ArbName = dr["ArbName"].ToString();
            Stc_Items.EngName = dr["EngName"].ToString();
            Stc_Items.GroupID = Comon.cInt(dr["GroupID"].ToString());
            Stc_Items.TypeID = Comon.cInt(dr["TypeID"].ToString());
            Stc_Items.Notes = dr["Notes"].ToString();
            Stc_Items.GroupName = dr["GroupName"].ToString();
            Stc_Items.TypeName = dr["TypeName"].ToString();
            Stc_Items.Notes = dr["Notes"].ToString();
            Stc_Items.IsVAT = Comon.cInt(dr["IsVat"].ToString());
            Stc_Items.UserID = Comon.cInt(dr["UserID"].ToString());
            Stc_Items.Cancel = Comon.cInt(dr["Cancel"].ToString());
            Stc_Items.EditDate = Comon.cLong(dr["RegDate"].ToString());
            Stc_Items.EditTime = Comon.cLong(dr["RegTime"].ToString());
            Stc_Items.EditUserID = Comon.cInt(dr["EditUserID"].ToString());
            Stc_Items.EditDate = Comon.cLong(dr["EditDate"].ToString());
            Stc_Items.EditTime = Comon.cLong(dr["EditTime"].ToString());
            Stc_Items.ComputerInfo = dr["ComputerInfo"].ToString();
            Stc_Items.EditComputerInfo = dr["EditComputerInfo"].ToString();
            Stc_Items.BranchID = Comon.cInt(dr["BranchID"].ToString());
            Stc_Items.FacilityID = Comon.cInt(dr["FacilityID"].ToString());
            return Stc_Items;
        }

        /// <summary>
        /// This function is used to Convert Row To Object For Taciking Report
        /// </summary>
        /// <param name="dr"></param>
        /// <returns> return object Stc_Items </returns>
        public static Stc_Items ConvertRowToObjForTacikingReport(DataRow dr)
        {
            Stc_Items Stc_Items = new Stc_Items();
            Stc_Items.ItemID = Comon.cInt(dr["ItemID"].ToString());
            Stc_Items.ArbName = dr["ItemName"].ToString();
            Stc_Items.SizeName = dr["SizeName"].ToString();
            Stc_Items.BarCode = dr["BarCode"].ToString();
            Stc_Items.ExpiryDate = dr["ExpiryDate"].ToString();
            Stc_Items.CostPrice = Comon.cDbl(dr["CostPrice"].ToString());
            Stc_Items.SalePrice = Comon.cDbl(dr["SalePrice"].ToString());
            Stc_Items.RemindQty = Comon.cDbl(dr["RemindQty"].ToString());
            return Stc_Items;
        }


        /// <summary>
        /// This function is used to get stc_items  
        /// </summary>
        /// <param name="ItemID"></param>
        /// <param name="BranchID"></param>
        /// <param name="FacilityID"></param>
        /// <returns>return object DataTable </returns>
        public static DataTable GetStc_Items(int ItemID, int BranchID, int FacilityID)
        {
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Stc_Items_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@ItemID", ItemID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 3));
                    SqlDataReader myreader = objCmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(myreader);
                    return dt;
                }
            }
        }
      /// <summary>
        /// This Function To used Is check this Barcode Is Exist or not 
      /// </summary>
      /// <param name="model"></param>
      /// <param name="BranchID"></param>
      /// <param name="FacilityID"></param>
      /// <param name="CMDTYPE"></param>
      /// <returns> return The Barcode </returns>
        public static string IsBarCodesExist(Stc_Items model, int BranchID, int FacilityID, int CMDTYPE)
        {
            string objRet = "";
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Stc_IsBarCodesExist_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@BarCode1", model.Stc_ItemUnits[0].BarCode));
                        if (model.Stc_ItemUnits.Count > 1)
                            objCmd.Parameters.Add(new SqlParameter("@BarCode2", model.Stc_ItemUnits[1].BarCode));
                        if (model.Stc_ItemUnits.Count > 2)
                            objCmd.Parameters.Add(new SqlParameter("@BarCode3", model.Stc_ItemUnits[2].BarCode));
                        if (model.Stc_ItemUnits.Count > 3)
                            objCmd.Parameters.Add(new SqlParameter("@BarCode4", model.Stc_ItemUnits[3].BarCode));

                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", CMDTYPE));

                        object obj = objCmd.ExecuteScalar();
                        if (obj != null)
                            objRet = Convert.ToString(obj);
                        return objRet;

                    }
                }
            }
            catch (Exception)
            {
                return "";
            }

        }
      
        /// <summary>
        /// This functioin is use to Get Data Detail By ID
        /// </summary>
        /// <param name="ItemID"></param>
        /// <param name="BranchID"></param>
        /// <param name="FacilityID"></param>
        /// <returns></returns>
        public static List<Stc_ItemUnits> GetDataDetailByID(int ItemID, int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Stc_Items_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@ItemID", ItemID));

                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 6));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);

                        if (dt != null)
                        {
                            List<Stc_ItemUnits> Returned = new List<Stc_ItemUnits>();
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

        /// <summary>
        /// This Function is used to Get Data Master Detail By ID
        /// </summary>
        /// <param name="ItemID"></param>
        /// <param name="BranchID"></param>
        /// <param name="FacilityID"></param>
        /// <returns> return object  </returns>
        public static Stc_ItemUnits GetDataMasterDetailByID(int ItemID, int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Stc_Items_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@ItemID", ItemID));

                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 6));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            Stc_ItemUnits Returned = new Stc_ItemUnits();
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

        /// <summary>
        /// This function is used to Get all Data By Branch ID 
        /// </summary>
        /// <param name="BranchID"></param>
        /// <param name="FacilityID"></param>
        /// <returns></returns>
        public static List<Stc_ItemUnits> GetAllData(int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Stc_Items_SP]";

                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 5));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            List<Stc_ItemUnits> Returned = new List<Stc_ItemUnits>();
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

        /// <summary>
        /// This Function is used to Get All Master Fot Report
        /// </summary>
        /// <param name="BranchID"></param>
        /// <param name="FacilityID"></param>
        /// <returns>return object DataTable </returns>
        public static DataTable GetAllMasterFotReport(int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Stc_Items_SP]";

                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 14));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                            return dt;
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

        /// <summary>
        /// this  function is used to Get all data Master 
        /// </summary>
        /// <param name="BranchID"></param>
        /// <param name="FacilityID"></param>
        /// <returns> return object list Stc_Items</returns>
        public static List<Stc_Items> GetAllDataMaster(int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Stc_Items_SP]";

                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 5));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            List<Stc_Items> Returned = new List<Stc_Items>();
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

        /// <summary>
        /// This function is used to Get All Data For Taciking Report
        /// </summary>
        /// <param name="BranchID"></param>
        /// <param name="FacilityID"></param>
        /// <returns>return object list Stc_Items</returns>
        public static List<Stc_Items> GetAllDataForTacikingReport(int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Stc_Items_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 7));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            List<Stc_Items> Returned = new List<Stc_Items>();
                            foreach (DataRow rows in dt.Rows)
                                Returned.Add(ConvertRowToObjForTacikingReport(rows));
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

        /// <summary>
        /// This function is used to Convert Object data To XML String
        /// </summary>
        /// <param name="classObject"></param>
        /// <returns>return data with  string type </returns>
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

        /// <summary>
        /// this function is used Insert Using XML
        /// </summary>
        /// <param name="objRecord"></param>
        /// <param name="IsNewRecord"></param>
        /// <returns></returns>
        public static string InsertUsingXML(Stc_Items objRecord, Boolean IsNewRecord)
        {
            string objRet = "0";

            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    string DitmeXML = ConvertObjectToXMLString(objRecord.Stc_ItemUnits);
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Stc_Items_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@xmlData", SqlDbType.Xml)).Value = DitmeXML;
                    objCmd.Parameters.Add(new SqlParameter("@ItemID", objRecord.ItemID));
                    objCmd.Parameters.Add(new SqlParameter("@ArbName", objRecord.ArbName));
                    objCmd.Parameters.Add(new SqlParameter("@EngName", objRecord.EngName));
                    objCmd.Parameters.Add(new SqlParameter("@ItemImage", objRecord.picItemImage));
                    objCmd.Parameters.Add(new SqlParameter("@BrandID", objRecord.BrandID));
                    objCmd.Parameters.Add(new SqlParameter("@GroupID", objRecord.GroupID));
                    objCmd.Parameters.Add(new SqlParameter("@TypeID", objRecord.TypeID));
                    objCmd.Parameters.Add(new SqlParameter("@IsVAT", Comon.cInt(objRecord.IsVAT)));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@Notes", objRecord.Notes));
                    objCmd.Parameters.Add(new SqlParameter("@UserID", objRecord.UserID));
                    objCmd.Parameters.Add(new SqlParameter("@RegDate", objRecord.RegDate));
                    objCmd.Parameters.Add(new SqlParameter("@RegTime", objRecord.RegTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                    objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditDate", objRecord.EditDate));
                    objCmd.Parameters.Add(new SqlParameter("@ComputerInfo", objRecord.ComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@EditComputerInfo", objRecord.EditComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@Cancel", Comon.cInt(objRecord.Cancel)));

                    objCmd.Parameters.Add(new SqlParameter("@DIAMOND_W", objRecord.DIAMOND_W));
                    objCmd.Parameters.Add(new SqlParameter("@STONE_W", objRecord.STONE_W));
                    objCmd.Parameters.Add(new SqlParameter("@BAGET_W", Comon.cInt(objRecord.BAGET_W)));
                    if (IsNewRecord == true)
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


       /// <summary>
       /// this function is used to  Delete Item by id 
       /// </summary>
       /// <param name="objRecord"></param>
       /// <returns></returns>
        public static string Delete(Stc_Items objRecord)
        {
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Stc_Items_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@ItemID", objRecord.ItemID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                    objCmd.Parameters.Add(new SqlParameter("@editdate", objRecord.EditDate));
                    objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 4));
                    object obj = objCmd.ExecuteNonQuery();
                    if (obj != null)
                        return Convert.ToString(obj);
                }
            }
            return "";
        }

      
        /// <summary>
        /// this function is used to delete unit by id 
        /// </summary>
        /// <param name="objRecord"></param>
        /// <param name="SizeID"></param>
        /// <returns>return value boolen opration is sucsess or not </returns>
        public static bool DeleteBySizeID(Stc_Items objRecord, int SizeID)
        {
            bool objRet = false;
            objRet = false;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    //Set Value to  propriteis objCmd
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Stc_Items_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@SizeID", SizeID));
                    objCmd.Parameters.Add(new SqlParameter("@ItemID", objRecord.ItemID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                    objCmd.Parameters.Add(new SqlParameter("@editdate", objRecord.EditDate));
                    objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 13));
                    objCmd.ExecuteNonQuery();
                }
            }
            objRet = true;
            return objRet;
        }
        

        /// <summary>
        /// This function is used  Get Report 
        /// </summary>
        /// <param name="sql"></param>
        /// <returns> return object  BindingList Stc_Items </returns>
        public static BindingList<Stc_Items> GetReport(string sql)
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
                            BindingList<Stc_Items> Returned = new BindingList<Stc_Items>();
                            foreach (DataRow rows in dt.Rows)
                                Returned.Add(ConvertRowToObjForTacikingReport(rows));
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

        /// <summary>
        /// This function is used to Check If Stop(cancel) Item Unit
        /// </summary>
        /// <param name="Barcode"></param>
        /// <param name="BranchID"></param>
        /// <param name="FacilityID"></param>
        /// <returns></returns>
        public static int CheckIfStopItemUnit(string Barcode, int BranchID, int FacilityID)
        {
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Stc_Items_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@Barcode", Barcode));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 11));
                    object obj = objCmd.ExecuteScalar();
                    if (obj != null)
                        return Convert.ToInt32(obj);
                }
            }
            return 1;



        }

        /// <summary>
        /// This function is used to Check if cancel Item Unite
        /// </summary>
        /// <param name="ItemID"></param>
        /// <param name="SizeID"></param>
        /// <param name="BranchID"></param>
        /// <param name="FacilityID"></param>
        /// <returns></returns>
        public static int CheckIfStopItemUnit(int ItemID, int SizeID, int BranchID, int FacilityID)
        {
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Stc_Items_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@ItemID", ItemID));
                    objCmd.Parameters.Add(new SqlParameter("@SizeID", SizeID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));

                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 12));
                    object obj = objCmd.ExecuteScalar();
                    if (obj != null)
                        return Convert.ToInt32(obj);
                }
            }
            return 0;



        }


        /// <summary>
        /// This function is used to Get Item Data With Packing Qty
        /// </summary>
        /// <param name="Barcode"></param>
        /// <param name="FacilityID"></param>
        /// <returns> return object DataTable </returns>
        public static DataTable GetItemDataWithPackingQty(string Barcode, int FacilityID)
        {
            DataTable AllRecords = new DataTable();
            // AllRecords = BARCODESERACHDAL.GetDataDetailsWithPackingQtyByBarcode(Barcode, FacilityID);
            if (AllRecords != null)
            {
                return AllRecords;
            }
            else
            {
                return null;

            }


        }

        /// <summary>
        /// This function is used to   Get Item Data Expiry
        /// </summary>
        /// <param name="Barcode"></param>
        /// <param name="FacilityID"></param>
        /// <returns></returns>
        public static DataTable GetItemDataExpiry(string Barcode, int FacilityID)
        {
            DataTable AllRecords = new DataTable();
            //call functio  GetDataDetailsByBarcodExpiry from BARCODESERACHDAL class 
            AllRecords = BARCODESERACHDAL.GetDataDetailsByBarcodExpiry(Barcode, FacilityID);
            if (AllRecords != null)
            {
                return AllRecords;
            }
            else
            {
                return null;

            }


        }

        /// <summary>
        /// This function is used to  Get Item Data Expiry
        /// </summary>
        /// <param name="Barcode"></param>
        /// <param name="FacilityID"></param>
        /// <param name="StoreID"></param>
        /// <returns>return object DataTable with data</returns>
        public static DataTable GetItemDataExpiry1(string Barcode, int FacilityID, int StoreID)
        {
            DataTable AllRecords = new DataTable();
            AllRecords = BARCODESERACHDAL.GetDataDetailsByBarcodExpiry1(Barcode, FacilityID, StoreID);
            if (AllRecords != null)
            {
                return AllRecords;
            }
            else
            {
                return null;

            }


        }

        /// <summary>
        /// This function is used to Get Item Data by Barcode 
        /// </summary>
        /// <param name="Barcode"></param>
        /// <param name="FacilityID"></param>
        /// <returns> return data with object DataTable</returns>
        public static DataTable GetItemData(string Barcode, int FacilityID)
        {
            DataTable AllRecords = new DataTable();
            //call function to get  data details By Barcod from BARCODESERACHDAL class 
            AllRecords = BARCODESERACHDAL.GetDataDetailsByBarcod(Barcode, FacilityID);
            if (AllRecords != null)
            {
                return AllRecords;
            }
            else
            {
                return null;

            }


        }
       

        /// <summary>
        /// this function is used to  Get Item Data
        /// </summary>
        /// <param name="Barcode"></param>
        /// <param name="FacilityID"></param>
        /// <returns>return data with object DataTable </returns>
        public static DataTable GetItemData1(string Barcode, int FacilityID)
        {
            DataTable AllRecords = new DataTable();
            AllRecords = BARCODESERACHDAL.GetDataDetailsByBarcod1(Barcode, FacilityID);
            if (AllRecords != null)
            {
                return AllRecords;
            }
            else
            {
                return null;

            }




        }

        /// <summary>
        /// This function is used to Get Item Data By Item ID and  Size ID
        /// </summary>
        /// <param name="ItemID"></param>
        /// <param name="SizeID"></param>
        /// <param name="FacilityID"></param>
        /// <returns>return data item with object DataTable </returns>
        public static DataTable GetItemDataByItemID_SizeID(int ItemID, int SizeID, int FacilityID)
        {
            DataTable AllRecords = new DataTable();
            AllRecords = BARCODESERACHDAL.GetDataByItemID_SizeID(ItemID, SizeID, FacilityID);
            if (AllRecords != null)
            {
                return AllRecords;
            }
            else
            {
                return null;

            }


        }
      
        /// <summary>
        /// This function is used to Get TopItemDataByItemID
        /// </summary>
        /// <param name="ItemID"></param>
        /// <param name="FacilityID"></param>
        /// <returns>return data by  object type DataTable</returns>
        public static DataTable GetTopItemDataByItemID(int ItemID, int FacilityID)
        {
            DataTable AllRecords = new DataTable();
            AllRecords = BARCODESERACHDAL.GetTopItemDataByItemID(ItemID, FacilityID);
            if (AllRecords != null)
            {
                return AllRecords;
            }
            else
            {
                return null;

            }
        }
        /// <summary>
        /// This function is used to Get Record which is Set By SQL
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns>return id </returns>
        public long GetRecordSetBySQL(string strSQL)
        {
            long ID = 0;
            try
            {
                FoundResult = false;
                dt = Lip.SelectRecord(strSQL);//execute selected
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

        /// <summary>
        /// this function is used to Get Data Detail By ID
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="BranchID"></param>
        /// <param name="FacilityID"></param>
        /// <returns>return data detail by object type DataTable </returns>
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
                        objCmd.CommandText = "[Stc_Items_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@ItemID", ID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 14));
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

        /// <summary>
        /// this functio is used to get max id item
        /// </summary>
        /// <returns> return max id item +1 </returns>
        public static long GetNewID()
        {
            try
            {
                long ID = 0;
                DataTable dt;
                string strSQL;
                //select max id item 
                strSQL = "SELECT Max(" + PremaryKey + ")+1 FROM Stc_Items ";
                dt = Lip.SelectRecord(strSQL);//execute sql selected  stetment 
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

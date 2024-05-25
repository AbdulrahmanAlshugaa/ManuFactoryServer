using Edex.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.DAL.Stc_itemDAL
{
    public class HR_District_DAL
    {

        public static Stc_ItemsGroups ConvertRowToObj(DataRow dr)
        {
            Stc_ItemsGroups Obj = new Stc_ItemsGroups();
            Obj.GroupID = int.Parse(dr["GroupID"].ToString());
            Obj.ArbName = dr["ARBNAME"].ToString();
            Obj.EngName = dr["ENGNAME"].ToString();
            Obj.Notes = dr["NOTES"].ToString();
            Obj.UserID = int.Parse(dr["UserID"].ToString());
            Obj.EditUserID = Comon.cInt(dr["EditUserID"].ToString());
            //Obj.RegDate = Comon.ConvertSerialToDate(long.Parse(dr["RegDate"].ToString()));
            //Obj.EditDate = Com.ConvertSerialToDate(long.Parse(dr["EditDate"].ToString()));
            return Obj;
        }
        public static Int32 InsertStc_PriceOffers(PriceItemsOffers objRecord)
        {
            Int32 objRet = 0;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[PriceItemsOffers_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@SetOntherAmount", objRecord.SetOntherAmount));
                    objCmd.Parameters.Add(new SqlParameter("@GetOntherAmount", objRecord.GetOntherAmount));
                    objCmd.Parameters.Add(new SqlParameter("@SetSameAmount", objRecord.SetSameAmount));
                    objCmd.Parameters.Add(new SqlParameter("@GetSameAmount", objRecord.GetSameAmount));
                    objCmd.Parameters.Add(new SqlParameter("@BarCode", objRecord.BarCode));
                    objCmd.Parameters.Add(new SqlParameter("@QTY", objRecord.QTY));
                    objCmd.Parameters.Add(new SqlParameter("@IsGetOnther", objRecord.IsGetOnther));
                    objCmd.Parameters.Add(new SqlParameter("@IsGetSame", objRecord.IsGetSame));
                    objCmd.Parameters.Add(new SqlParameter("@IsTakeOne", objRecord.IsTakeOne));
                    objCmd.Parameters.Add(new SqlParameter("@AmountCost", objRecord.AmountCost));
                    objCmd.Parameters.Add(new SqlParameter("@PercentCost", objRecord.PercentCost));
                    objCmd.Parameters.Add(new SqlParameter("@IsOffers", objRecord.IsOffers));
                    objCmd.Parameters.Add(new SqlParameter("@IsPercent", objRecord.IsPercent));
                    objCmd.Parameters.Add(new SqlParameter("@IsAmount", objRecord.IsAmount));
                    objCmd.Parameters.Add(new SqlParameter("@ToTime", objRecord.ToTime));
                    objCmd.Parameters.Add(new SqlParameter("@FromTime", objRecord.FromTime));
                    objCmd.Parameters.Add(new SqlParameter("@ToDate", objRecord.ToDate));
                    objCmd.Parameters.Add(new SqlParameter("@FromDate", objRecord.FromDate));
                    objCmd.Parameters.Add(new SqlParameter("@ToItemID", objRecord.ToItemID));
                    objCmd.Parameters.Add(new SqlParameter("@FromItemID", objRecord.FromItemID));
                    objCmd.Parameters.Add(new SqlParameter("@ToISizeID", objRecord.ToSizeID));
                    objCmd.Parameters.Add(new SqlParameter("@FromSizeID", objRecord.FromSizeID));

                    objCmd.Parameters.Add(new SqlParameter("@ToItemID1", objRecord.ToItemID1));
                    objCmd.Parameters.Add(new SqlParameter("@FromItemID1", objRecord.FromItemID1));
                    objCmd.Parameters.Add(new SqlParameter("@ToISizeID1", objRecord.ToSizeID1));
                    objCmd.Parameters.Add(new SqlParameter("@FromSizeID1", objRecord.FromSizeID1));
                     


                    objCmd.Parameters.Add(new SqlParameter("@ToGroupID", objRecord.ToGroupID));
                    objCmd.Parameters.Add(new SqlParameter("@FromGroupID", objRecord.FromGroupID));
                    objCmd.Parameters.Add(new SqlParameter("@Description", objRecord.Description));

                    objCmd.Parameters.Add(new SqlParameter("@OrderType", objRecord.OrderType));
                    objCmd.Parameters.Add(new SqlParameter("@OfferID", objRecord.OfferID));
                    objCmd.Parameters.Add(new SqlParameter("@ISRepeat", objRecord.ISRepeat));


                    if (objRecord.OfferID == 0)
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 1));
                    else
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 2));
                    object obj = objCmd.ExecuteScalar();
                    if (obj != null)
                        objRet = Convert.ToInt32(obj);
                }
            }
            return objRet;
        }


        public static Stc_ItemsGroups GetDataByID(int ID, int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Stc_ItemsGroups_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@GroupID  ", ID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 3));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            Stc_ItemsGroups Returned = new Stc_ItemsGroups();
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

        public static BindingList<Stc_ItemsGroups> GetAllData(int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Stc_ItemsGroups_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 5));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            BindingList<Stc_ItemsGroups> Returned = new BindingList<Stc_ItemsGroups>();
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

        public static Int32 InsertStc_Stores(HR_District objRecord)
        {
            Int32 objRet = 0;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[HR_District_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@ID", objRecord.ID));
                    objCmd.Parameters.Add(new SqlParameter("@ArbName", objRecord.ArbName));
                    objCmd.Parameters.Add(new SqlParameter("@EngName", objRecord.EngName));
                    objCmd.Parameters.Add(new SqlParameter("@TimeDelivery", objRecord.TimeDelivery));
                    objCmd.Parameters.Add(new SqlParameter("@TransCost", objRecord.TransCost));
                    objCmd.Parameters.Add(new SqlParameter("@Cancel", objRecord.Cancel));
                    if (objRecord.ID == 0)
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 1));
                    else
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 2));
                    object obj = objCmd.ExecuteScalar();
                    if (obj != null)
                        objRet = Convert.ToInt32(obj);
                }
            }
            return objRet;
        }

        public static Int32 InsertStc_Steet(HR_District objRecord)
        {
            Int32 objRet = 0;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[HR_Street_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@ID", objRecord.ID));
                    objCmd.Parameters.Add(new SqlParameter("@ArbName", objRecord.ArbName));
                    objCmd.Parameters.Add(new SqlParameter("@EngName", objRecord.EngName));
                    objCmd.Parameters.Add(new SqlParameter("@TimeDelivery", objRecord.TimeDelivery));
                    objCmd.Parameters.Add(new SqlParameter("@TransCost", objRecord.TransCost));
                    objCmd.Parameters.Add(new SqlParameter("@Cancel", objRecord.Cancel));
                    if (objRecord.ID == 0)
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 1));
                    else
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 2));
                    object obj = objCmd.ExecuteScalar();
                    if (obj != null)
                        objRet = Convert.ToInt32(obj);
                }
            }
            return objRet;
        }
        public static bool DeleteStc_Steet(HR_District objRecord)
        {
            bool objRet = false;
            objRet = false;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[HR_Street_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@GroupID", objRecord.ID));
                    objCmd.Parameters.Add(new SqlParameter("@ArbName", objRecord.ArbName));
                    objCmd.Parameters.Add(new SqlParameter("@EngName", objRecord.EngName));
                    objCmd.Parameters.Add(new SqlParameter("@Notes", objRecord.TimeDelivery));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.TransCost));
                    objCmd.Parameters.Add(new SqlParameter("@Cancel", objRecord.Cancel));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 4));
                    objCmd.ExecuteNonQuery();
                }
            }
            objRet = true;
            return objRet;
        }
        
           
        public static bool UpdateStc_Stores(Stc_ItemsGroups objRecord)
        {
            bool objRet = false;
            objRet = false;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Stc_ItemsGroups_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@GroupID", objRecord.GroupID));
                    objCmd.Parameters.Add(new SqlParameter("@ArbName", objRecord.ArbName));
                    objCmd.Parameters.Add(new SqlParameter("@EngName", objRecord.EngName));
                    objCmd.Parameters.Add(new SqlParameter("@Notes", objRecord.Notes));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
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

        public static bool DeleteStc_Stores(HR_District objRecord)
        {
            bool objRet = false;
            objRet = false;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[HR_District_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@GroupID", objRecord.ID));
                    objCmd.Parameters.Add(new SqlParameter("@ArbName", objRecord.ArbName));
                    objCmd.Parameters.Add(new SqlParameter("@EngName", objRecord.EngName));
                    objCmd.Parameters.Add(new SqlParameter("@Notes", objRecord.TimeDelivery));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.TransCost));
                    objCmd.Parameters.Add(new SqlParameter("@Cancel", objRecord.Cancel));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 4));
                    objCmd.ExecuteNonQuery();
                }
            }
            objRet = true;
            return objRet;
        }

         

        public static bool DeleteStc_Stores(PriceItemsOffers objRecord)
        {
            bool objRet = false;
            objRet = false;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[PriceItemsOffers_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@OfferID", objRecord.OfferID));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 4));
                    objCmd.ExecuteNonQuery();
                }
            }
            objRet = true;
            return objRet;
        }


         

    }
}

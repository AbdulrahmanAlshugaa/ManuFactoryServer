using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Edex.Model;
using Edex.DAL;

namespace Edex.DAL
{

  public static  class BARCODESERACHDAL
    {
      public static BARCODESERACH_BO ConvertRowToObjITEM_DTL(DataRow dr)
      {
          BARCODESERACH_BO Obj = new BARCODESERACH_BO();
          Obj.ItemID = Comon.cLong( dr["ItemID"].ToString());
          Obj.ItemName = dr["ArbName"].ToString();
          Obj.SalePrice = Comon.cDec(dr["SalePrice"].ToString());
          Obj.SizeID = Comon.cInt(dr["SizeID"].ToString()); ;
          Obj.SizeName = dr["SizeName"].ToString();
          Obj.BarCode = dr["BarCode"].ToString();
          Obj.Caliber = Comon.cDec(dr["Caliber"].ToString());
          Obj.Equivalen = Comon.cDec(dr["Equivalen"].ToString());
          Obj.QTY = Comon.cDec(dr["QTY"].ToString());

          Obj.DIAMOND_W = Comon.cDbl(dr["DIAMOND_W"].ToString());
          Obj.STONE_W = Comon.cDbl(dr["STONE_W"].ToString());
          Obj.BAGET_W = Comon.cDbl(dr["BAGET_W"].ToString());

          Obj.CostPrice = 0;
          return Obj;
      }
      public static Sales_PurchaseInvoiceDetails ConvertRowToObjDetails(DataRow dr)
      {
          Sales_PurchaseInvoiceDetails Obj = new Sales_PurchaseInvoiceDetails();
          Obj.ItemID = Comon.cInt( dr["ItemID"].ToString());
          Obj.ArbItemName = dr["ArbName"].ToString();
          Obj.SalePrice = Comon.cDec(dr["SalePrice"].ToString());
          Obj.SizeID = Comon.cInt(dr["SizeID"].ToString()); ;
          Obj.ArbSizeName = dr["SizeName"].ToString();
          Obj.BarCode = dr["BarCode"].ToString();
          Obj.Caliber = Comon.cDec(dr["Caliber"].ToString());
          Obj.Equivalen = Comon.cDec(dr["Equivalen"].ToString());
          Obj.QTY = Comon.cDec(dr["QTY"].ToString());

          Obj.DIAMOND_W = Comon.cDbl(dr["DIAMOND_W"].ToString());
          Obj.STONE_W = Comon.cDbl(dr["STONE_W"].ToString());
          Obj.BAGET_W = Comon.cDbl(dr["BAGET_W"].ToString());

          Obj.CostPrice = 0;
          return Obj;
      }
      public static BARCODESERACH_BO GetDataByBarcod(string Barcod, int BranchID, int FacilityID)
      {
          try
          {
              using (SqlConnection objCnn = new GlobalConnection().Conn)
              {
                  objCnn.Open();
                  using (SqlCommand objCmd = objCnn.CreateCommand())
                  {
                      objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                      objCmd.CommandText = "Stc_IsBarCodesExist_SP";
                      objCmd.Parameters.Add(new SqlParameter("@BarCode1", Barcod));
                      objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 3));
                      SqlDataReader myreader = objCmd.ExecuteReader();
                      DataTable dt = new DataTable();
                      dt.Load(myreader);
                      if (dt != null)
                      {
                          BARCODESERACH_BO Returned = new BARCODESERACH_BO();
                          Returned = (ConvertRowToObjITEM_DTL(dt.Rows[0]));
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
      public static DataTable  GetDataDetailsByBarcod(string Barcod, int FacilityID)
      {
          try
          {
              using (SqlConnection objCnn = new GlobalConnection().Conn)
              {
                  objCnn.Open();
                  using (SqlCommand objCmd = objCnn.CreateCommand())
                  {
                      objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                      objCmd.CommandText = "Stc_IsBarCodesExist_SP";
                      objCmd.Parameters.Add(new SqlParameter("@BarCode1", Barcod));
                      objCmd.Parameters.Add(new SqlParameter("@BranchID", MySession.GlobalBranchID));
                      objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 3));
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
      public static List<BARCODESERACH_BO> GetAllData()
      {
          try
          {
              using (SqlConnection objCnn = new GlobalConnection().Conn)
              {
                  objCnn.Open();
                  using (SqlCommand objCmd = objCnn.CreateCommand())
                  {
                      objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                      objCmd.CommandText = "Stc_Items_SP";
                      objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 7));
                      SqlDataReader myreader = objCmd.ExecuteReader();
                      DataTable dt = new DataTable();
                      dt.Load(myreader);
                      if (dt != null)
                      {
                          List<BARCODESERACH_BO> Returned = new List<BARCODESERACH_BO>();
                          foreach (DataRow rows in dt.Rows)
                              Returned.Add(ConvertRowToObjITEM_DTL(rows));
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
      public static BARCODESERACH_BO GetDataByItemIDAndSizeID(int ItemID, int SizeID, int FacilityID)
      {
          try
          {
              using (SqlConnection objCnn = new GlobalConnection().Conn)
              {
                  objCnn.Open();
                  using (SqlCommand objCmd = objCnn.CreateCommand())
                  {
                      objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                      objCmd.CommandText = "Stc_Items_SP";
                      objCmd.Parameters.Add(new SqlParameter("@ItemID", ItemID));
                      objCmd.Parameters.Add(new SqlParameter("@SizeID", ItemID));
                      objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 8));
                      SqlDataReader myreader = objCmd.ExecuteReader();
                      DataTable dt = new DataTable();
                      dt.Load(myreader);
                      if (dt != null)
                      {
                          BARCODESERACH_BO Returned = new BARCODESERACH_BO();
                          Returned = (ConvertRowToObjITEM_DTL(dt.Rows[0]));
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
      public static DataTable GetDataByItemID_SizeID(int ItemID, int SizeID, int FacilityID)
      {
          try
          {
              using (SqlConnection objCnn = new GlobalConnection().Conn)
              {
                  objCnn.Open();
                  using (SqlCommand objCmd = objCnn.CreateCommand())
                  {
                      objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                      objCmd.CommandText = "Stc_Items_SP";
                      objCmd.Parameters.Add(new SqlParameter("@ItemID", ItemID));
                      objCmd.Parameters.Add(new SqlParameter("@SizeID", SizeID));
                      objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 8));
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
      /// This Function is used to  Get Top Item Data By ItemID
      /// </summary>
      /// <param name="ItemID"></param>
      /// <param name="FacilityID"></param>
      /// <returns></returns>
      public static DataTable GetTopItemDataByItemID(int ItemID, int FacilityID)
      {
          try
          {
              using (SqlConnection objCnn = new GlobalConnection().Conn)
              {
                  objCnn.Open();
                  using (SqlCommand objCmd = objCnn.CreateCommand())
                  {
                      //Set value to proprities 
                      objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                      objCmd.CommandText = "Stc_Items_SP";
                      objCmd.Parameters.Add(new SqlParameter("@ItemID", ItemID));
                      objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 13));
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

      public static DataTable GetDataDetailsByBarcodExpiry(string Barcod, int FacilityID)
      {
          try
          {
              using (SqlConnection objCnn = new GlobalConnection().Conn)
              {
                  objCnn.Open();
                  using (SqlCommand objCmd = objCnn.CreateCommand())
                  {
                      objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                      objCmd.CommandText = "Stc_IsBarCodesExist_SP";
                      objCmd.Parameters.Add(new SqlParameter("@BarCode1", Barcod));
                      objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 3));
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


      public static DataTable GetDataDetailsByBarcodExpiry1(string Barcod, int FacilityID, int storeID)
      {
          try
          {
              using (SqlConnection objCnn = new GlobalConnection().Conn)
              {
                  objCnn.Open();
                  using (SqlCommand objCmd = objCnn.CreateCommand())
                  {
                      objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                      objCmd.CommandText = "Stc_IsBarCodesExist_SP2";
                      objCmd.Parameters.Add(new SqlParameter("@BarCode1", Barcod));
                      objCmd.Parameters.Add(new SqlParameter("@StoreID", storeID));

                      objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 4));
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
     
      public static DataTable GetDataDetailsByBarcod1(string Barcod, int FacilityID)
      {
          try
          {
              using (SqlConnection objCnn = new GlobalConnection().Conn)
              {
                  objCnn.Open();
                  using (SqlCommand objCmd = objCnn.CreateCommand())
                  {
                      objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                      objCmd.CommandText = "Stc_IsBarCodesExist_SP";
                      objCmd.Parameters.Add(new SqlParameter("@BarCode1", Barcod));
                      objCmd.Parameters.Add(new SqlParameter("@BranchID", MySession.GlobalBranchID));
                      objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 3));
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

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Collections;
using System.Data.SqlClient;
using System.Data;
using Edex.Model;
namespace Edex.DAL
{
   public class Acc_AccountsDAL
   {  
        public readonly string TableName = "MAINMENUs";
        public readonly string PremaryKey = "ID";
        public int IsActive;
        public bool FoundResult;
        public static bool IsNewRecord;
        private static string strSQL;
        public static DataTable dt = new DataTable();
       public static MAINMENU ConvertRowToObjmENU(DataRow dr)
       {
           MAINMENU Obj = new MAINMENU();
           Obj.ID = Comon.cLong(dr["ID"].ToString());
           Obj.MENUID = Comon.cLong(dr["MENUID"].ToString());
           Obj.ARBNAME = dr["ArbName"].ToString();
           Obj.ENGNAME = dr["EngName"].ToString();
           Obj.PARENTMENUID = Comon.cLong(dr["PARENTMENUID"].ToString());
           Obj.MENULEVELID = Comon.cInt(dr["MENULEVELID"].ToString());
           Obj.MENUTYPEID = Comon.cInt(dr["MENUTYPEID"].ToString());
           Obj.ENGCAPTION = dr["ENGCAPTION"].ToString();
           Obj.FORMNAME = dr["FORMNAME"].ToString();
           return Obj;
       }
       public static List<MAINMENU> GetMainMenuSub(int BranchID, int FacilityID)
       {
           DataTable dt = new DataTable();
           string StrSQL = "Select * From MAINMENU Where FACILITYID=" + FacilityID + "  and DELETED=0 And MENULEVELID =3";
           dt = Lip.SelectRecord(StrSQL);
           if (dt != null)
           {
               List<MAINMENU> Returned = new List<MAINMENU>();
               foreach (DataRow rows in dt.Rows)
                   Returned.Add(ConvertRowToObjmENU(rows));
               return Returned;
           }
           return null;
       }
      public static Acc_Accounts ConvertRowToObj(DataRow dr)
      {
          Acc_Accounts Obj = new Acc_Accounts();
          Obj.AccountID = Comon.cLong(dr["AccountID"].ToString());
          Obj.BranchID = Comon.cInt(dr["BranchID"].ToString());
          Obj.FacilityID = Comon.cInt(dr["FacilityID"].ToString());
          Obj.ArbName = dr["ArbName"].ToString();
          Obj.EngName = dr["EngName"].ToString();
          Obj.ParentAccountID = Comon.cLong(dr["ParentAccountID"].ToString());
          Obj.AccountLevel = Comon.cInt(dr["AccountLevel"].ToString());
          Obj.AccountTypeID = Comon.cInt(dr["AccountTypeID"].ToString());
          Obj.StopAccount = Comon.cInt(dr["StopAccount"].ToString());
          Obj.MinLimit = Comon.cLong(dr["MinLimit"].ToString());
          Obj.MaxLimit = Comon.cLong(dr["MaxLimit"].ToString());
          Obj.UserID = Comon.cInt(dr["UserID"].ToString());
          Obj.RegDate = Comon.cLong(dr["RegDate"].ToString());
          Obj.RegTime = Comon.cLong(dr["RegTime"].ToString());
          Obj.EditUserID = Comon.cInt(dr["EditUserID"].ToString());
          Obj.EditTime = Comon.cLong(dr["EditTime"].ToString());
          Obj.EditDate = Comon.cLong(dr["EditDate"].ToString());
          Obj.ComputerInfo = dr["ComputerInfo"].ToString();
          Obj.EditComputerInfo = dr["EditComputerInfo"].ToString();
          Obj.Cancel = Comon.cInt(dr["Cancel"].ToString());
          Obj.Description = dr["Description"].ToString();
          Obj.Location = dr["Location"].ToString();
          Obj.EndType = Comon.cInt(dr["EndType"].ToString());
          Obj.CashState = Comon.cInt(dr["CashState"].ToString());
          Obj.AllowMaxLimit = Comon.cInt(dr["AllowMaxLimit"].ToString());
          return Obj;
      }
      public static Acc_Accounts ConvertRowToObjFromEmport(DataRow dr)
      {
          try
          {
              Acc_Accounts Obj = new Acc_Accounts();
              Obj.AccountID = Comon.cLong(dr["AccountID"].ToString());
              Obj.BranchID = Comon.cInt(MySession.GlobalBranchID);
              Obj.FacilityID = Comon.cInt(MySession.GlobalFacilityID);
              Obj.ArbName = dr["ArbName"].ToString();
              Obj.EngName = dr["EngName"].ToString();
              Obj.ParentAccountID = Comon.cLong(dr["ParentAccountID"].ToString());
              Obj.AccountLevel = Comon.cInt(dr["AccountLevel"].ToString());
              Obj.AccountTypeID = Comon.cInt(dr["AccountTypeID"].ToString());
              Obj.StopAccount = Comon.cInt(dr["StopAccount"].ToString());
              Obj.MaxLimit = Comon.cLong(dr["MaxLimit"].ToString());
              Obj.UserID = Comon.cInt(UserInfo.ID);
              Obj.RegDate = Comon.cLong(Lip.GetServerDateSerial());
              Obj.RegTime = Comon.cLong(Lip.GetServerTimeSerial());
              Obj.ComputerInfo = UserInfo.ComputerInfo;
              Obj.Cancel = 0;
              Obj.Description = dr["Description"].ToString();
              Obj.Location = dr["Location"].ToString();
              Obj.EndType = Comon.cInt(dr["EndType"].ToString());
              Obj.CashState = Comon.cInt(dr["CashState"].ToString());
              Obj.TypeAccount = Comon.cInt(dr["TypeAccount"].ToString());
              Obj.AllowMaxLimit = Comon.cInt(dr["AllowMaxLimit"].ToString());
              return Obj;
          }
          catch (Exception ex)
          {
              return null;

          }
      }
      public static List<MAINMENU> GetByParent(long ParentID)
      {
          try
          {
              strSQL = "Select * from MAINMENU Where  DELETED=0 and   PARENTMENUID=" + ParentID;
              dt = Lip.SelectRecord(strSQL);
              List<MAINMENU> Returned = new List<MAINMENU>();
              foreach (DataRow rows in dt.Rows)
                  Returned.Add(ConvertRowToObjmENU(rows));
              return Returned;
          }
          catch (Exception ex)
          {
              return null;

          }
      }
      public static Acc_Accounts GetDataByID(long ID, int BranchID, int FacilityID)
      {
          try
          {
              using (SqlConnection objCnn = new GlobalConnection().Conn)
              {
                  objCnn.Open();
                  using (SqlCommand objCmd = objCnn.CreateCommand())
                  {
                      objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                      objCmd.CommandText = "[Acc_Accounts_SP]";
                      objCmd.Parameters.Add(new SqlParameter("@AccountID", ID));
                      objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                      objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                      objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 3));
                      SqlDataReader myreader = objCmd.ExecuteReader();
                      DataTable dt = new DataTable();
                      dt.Load(myreader);
                      if (dt != null)
                      {
                          Acc_Accounts Returned = new Acc_Accounts();
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
      public static List<Acc_Accounts> GetAllData(int BranchID, int FacilityID)
      {
          try
          {
              using (SqlConnection objCnn = new GlobalConnection().Conn)
              {
                  objCnn.Open();
                  using (SqlCommand objCmd = objCnn.CreateCommand())
                  {
                      objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                      objCmd.CommandText = "[Acc_Accounts_SP]";
                      objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                      objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                      objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 5));
                      SqlDataReader myreader = objCmd.ExecuteReader();
                      DataTable dt = new DataTable();
                      dt.Load(myreader);
                      if (dt != null)
                      {
                          List<Acc_Accounts> Returned = new List<Acc_Accounts>();
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
      public DataTable GetAcc_Accounts(int BranchID, int FacilityID)
      {
          using (SqlConnection objCnn = new GlobalConnection().Conn)
         {
            objCnn.Open();
             using (SqlCommand objCmd = objCnn.CreateCommand())
            {
               objCmd.CommandType = System.Data.CommandType.StoredProcedure;
               objCmd.CommandText = "[Acc_Accounts_SP]";
               objCmd.Parameters.Add(new SqlParameter("@BranchID",  BranchID));
               objCmd.Parameters.Add(new SqlParameter("@FacilityID",   FacilityID));
               objCmd.Parameters.Add(new SqlParameter("@CMDTYPE",5));
               SqlDataReader myreader = objCmd.ExecuteReader();
               DataTable dt = new DataTable();
                  dt.Load(myreader);
         return dt;
      }
      }
      }
       public DataTable GetAcc_AccountsByLevel(int BranchID, int FacilityID)
      {
          using (SqlConnection objCnn = new GlobalConnection().Conn)
         {
            objCnn.Open();
             using (SqlCommand objCmd = objCnn.CreateCommand())
            {
               objCmd.CommandType = System.Data.CommandType.StoredProcedure;
               objCmd.CommandText = "[Acc_Accounts_SP]";
               objCmd.Parameters.Add(new SqlParameter("@BranchID",  BranchID));
               objCmd.Parameters.Add(new SqlParameter("@FacilityID",   FacilityID));
               objCmd.Parameters.Add(new SqlParameter("@CMDTYPE",6));
               SqlDataReader myreader = objCmd.ExecuteReader();
               DataTable dt = new DataTable();
                  dt.Load(myreader);
         return dt;
      }
      }
      }



        // This Function For Insert The Data Acc_Accounts
        public static int InsertAcc_Accounts(Acc_Accounts objRecord)
        {
            Int32 objRet = 0;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Acc_Accounts_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@AccountID", objRecord.AccountID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@ArbName", objRecord.ArbName));
                    objCmd.Parameters.Add(new SqlParameter("@EngName", objRecord.EngName));
                    objCmd.Parameters.Add(new SqlParameter("@ParentAccountID", objRecord.ParentAccountID));
                    objCmd.Parameters.Add(new SqlParameter("@AccountLevel", objRecord.AccountLevel));
                    objCmd.Parameters.Add(new SqlParameter("@AccountTypeID", objRecord.AccountTypeID));
                    objCmd.Parameters.Add(new SqlParameter("@StopAccount", objRecord.StopAccount));
                    objCmd.Parameters.Add(new SqlParameter("@EndType", objRecord.EndType));
                    objCmd.Parameters.Add(new SqlParameter("@MinLimit", objRecord.MinLimit));
                    objCmd.Parameters.Add(new SqlParameter("@MaxLimit", objRecord.MaxLimit));
                    objCmd.Parameters.Add(new SqlParameter("@AllowMaxLimit", objRecord.AllowMaxLimit));
                    objCmd.Parameters.Add(new SqlParameter("@UserID", objRecord.UserID));
                    objCmd.Parameters.Add(new SqlParameter("@RegDate", objRecord.RegDate));
                    objCmd.Parameters.Add(new SqlParameter("@RegTime", objRecord.RegTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                    objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditDate", objRecord.EditDate));
                    objCmd.Parameters.Add(new SqlParameter("@ComputerInfo", objRecord.ComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@EditComputerInfo", objRecord.EditComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@Cancel", objRecord.Cancel));
                    objCmd.Parameters.Add(new SqlParameter("@CashState", objRecord.CashState));
                    objCmd.Parameters.Add(new SqlParameter("@Description", objRecord.Description));
                    objCmd.Parameters.Add(new SqlParameter("@Location", objRecord.Location));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 1));
                    object obj = objCmd.ExecuteScalar();
                    if (obj != null)
                        objRet = Convert.ToInt32(obj);
                }
            }
            return objRet;
        }

      // This Function For Update The Data Acc_Accounts
       public static bool  UpdateAcc_Accounts(Acc_Accounts objRecord)
      {
         bool objRet = false;
         objRet = false;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Acc_Accounts_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@AccountID", objRecord.AccountID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@ArbName", objRecord.ArbName));
                    objCmd.Parameters.Add(new SqlParameter("@EngName", objRecord.EngName));
                    objCmd.Parameters.Add(new SqlParameter("@EndType", objRecord.EndType));
                    objCmd.Parameters.Add(new SqlParameter("@ParentAccountID", objRecord.ParentAccountID));
                    objCmd.Parameters.Add(new SqlParameter("@AllowMaxLimit", objRecord.AllowMaxLimit));
                    objCmd.Parameters.Add(new SqlParameter("@AccountLevel", objRecord.AccountLevel));
                    objCmd.Parameters.Add(new SqlParameter("@AccountTypeID", objRecord.AccountTypeID));
                    objCmd.Parameters.Add(new SqlParameter("@StopAccount", objRecord.StopAccount));
                    objCmd.Parameters.Add(new SqlParameter("@MinLimit", objRecord.MinLimit));
                    objCmd.Parameters.Add(new SqlParameter("@MaxLimit", objRecord.MaxLimit));
                    objCmd.Parameters.Add(new SqlParameter("@UserID", objRecord.UserID));
                    objCmd.Parameters.Add(new SqlParameter("@RegDate", objRecord.RegDate));
                    objCmd.Parameters.Add(new SqlParameter("@RegTime", objRecord.RegTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                    objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditDate", objRecord.EditDate));
                    objCmd.Parameters.Add(new SqlParameter("@ComputerInfo", objRecord.ComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@EditComputerInfo", objRecord.EditComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@Cancel", objRecord.Cancel));
                    objCmd.Parameters.Add(new SqlParameter("@CashState", objRecord.CashState));
                    objCmd.Parameters.Add(new SqlParameter("@Description", objRecord.Description));
                    objCmd.Parameters.Add(new SqlParameter("@Location", objRecord.Location));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 2));
                    objCmd.ExecuteNonQuery();
                }
            }
         objRet = true;
         return objRet;
      }
      
       // This Function For Delete The Data Acc_Accounts
       public static bool DeleteAcc_Accounts(Acc_Accounts objRecord)
      {
         bool objRet = false;
         objRet = false;
         using (SqlConnection objCnn = new GlobalConnection().Conn)
         {
            objCnn.Open();
             using (SqlCommand objCmd = objCnn.CreateCommand())
            {
               objCmd.CommandType = System.Data.CommandType.StoredProcedure;
               objCmd.CommandText = "[Acc_Accounts_SP]";
               objCmd.Parameters.Add(new SqlParameter("@AccountID",objRecord. AccountID));
               objCmd.Parameters.Add(new SqlParameter("@FacilityID",objRecord. FacilityID));
               objCmd.Parameters.Add(new SqlParameter("@BranchID",objRecord. BranchID));
               objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
               objCmd.Parameters.Add(new SqlParameter("@CMDTYPE",4));
               objCmd.ExecuteNonQuery();
            }
         }
         objRet = true;
         return objRet;
      }
      public static List<MAINMENU> GetMainMenu(int BranchID, int FacilityID,int Level=3)
      {
          DataTable dt = new DataTable();
          string StrSQL = "Select * From MAINMENU Where FACILITYID=" + FacilityID + "  and DELETED=0 And MENULEVELID <=" + Level + " order by ID";
          dt = Lip.SelectRecord(StrSQL);
          if (dt != null)
          {
              List<MAINMENU> Returned = new List<MAINMENU>();
              foreach (DataRow rows in dt.Rows)
                  Returned.Add(ConvertRowToObjmENU(rows));
              return Returned;
          }
          return null;
      }
 

   }
}

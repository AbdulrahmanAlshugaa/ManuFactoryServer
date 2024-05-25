using System;
using System.Collections.Generic;
using System.Configuration;
using System.Collections;
using System.Data.SqlClient;
using System.Data;
using Edex.Model;

namespace Edex.DAL
{
   public class HR_EmployeeFileDAL
   {


      public DataTable GetHR_EmployeeFile(int EmployeeID)
      {
          using (SqlConnection objCnn = new GlobalConnection().Conn)
         {
            objCnn.Open();
             using (SqlCommand objCmd = objCnn.CreateCommand())
            {
               objCmd.CommandType = System.Data.CommandType.StoredProcedure;
               objCmd.CommandText = "[HR_EmployeeFile_SP]";
               objCmd.Parameters.Add(new SqlParameter("@EmployeeID",  EmployeeID));
               objCmd.Parameters.Add(new SqlParameter("@CMDTYPE",3));
               SqlDataReader myreader = objCmd.ExecuteReader();
               DataTable dt = new DataTable();
                  dt.Load(myreader);
         return dt;
      }
      }
      }

      public DataTable GetALL(int EmployeeID)
      {
          using (SqlConnection objCnn = new GlobalConnection().Conn)
          {
              objCnn.Open();
              using (SqlCommand objCmd = objCnn.CreateCommand())
              {
                  objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                  objCmd.CommandText = "[HR_EmployeeFile_SP]";
                  objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 5));
                  SqlDataReader myreader = objCmd.ExecuteReader();
                  DataTable dt = new DataTable();
                  dt.Load(myreader);
                  return dt;
              }
          }
      }

      public static Int64 InsertHR_EmployeeFile(HR_EmployeeFile objRecord, bool IsNewRecord = true)
      {
         long objRet = 0;
         using (SqlConnection objCnn = new GlobalConnection().Conn)
         {
            objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[HR_EmployeeFile_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@EmployeeID", objRecord.EmployeeID));
                    objCmd.Parameters.Add(new SqlParameter("@OnAccountID", objRecord.OnAccountID));

                    objCmd.Parameters.Add(new SqlParameter("@StopAccount", objRecord.StopAccount));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@ArbName", objRecord.ArbName));
                    objCmd.Parameters.Add(new SqlParameter("@EngName", objRecord.EngName));
                    objCmd.Parameters.Add(new SqlParameter("@FootprintEmpID", objRecord.FootprintEmpID));
                    objCmd.Parameters.Add(new SqlParameter("@ParentAccountID", objRecord.ParentAccountID));
                    objCmd.Parameters.Add(new SqlParameter("@Sex", objRecord.Sex));
                    objCmd.Parameters.Add(new SqlParameter("@MaritalStatus", objRecord.MaritalStatus));

                    objCmd.Parameters.Add(new SqlParameter("@BirthPlace", objRecord.BirthPlace));
                    objCmd.Parameters.Add(new SqlParameter("@Nationality", objRecord.Nationality));
                    objCmd.Parameters.Add(new SqlParameter("@Religions", objRecord.Religions));
                    objCmd.Parameters.Add(new SqlParameter("@WorkType", objRecord.WorkType));
                    objCmd.Parameters.Add(new SqlParameter("@Occupation", objRecord.Occupation));
                    objCmd.Parameters.Add(new SqlParameter("@IqamaOccupation", objRecord.IqamaOccupation));
                    objCmd.Parameters.Add(new SqlParameter("@CurrentSponsor", objRecord.CurrentSponsor));
                    objCmd.Parameters.Add(new SqlParameter("@ContractType", objRecord.ContractType));
                    
                    objCmd.Parameters.Add(new SqlParameter("@BeginningDate", objRecord.BeginningDate));
                    objCmd.Parameters.Add(new SqlParameter("@DateStartWork", objRecord.DateStartWork));
                    objCmd.Parameters.Add(new SqlParameter("@ContractEnd", objRecord.ContractEnd));
                    objCmd.Parameters.Add(new SqlParameter("@BirthDate", objRecord.BirthDate));
                    objCmd.Parameters.Add(new SqlParameter("@DateIssuanceID", objRecord.DateIssuanceID));
                    objCmd.Parameters.Add(new SqlParameter("@CurrentSponsorMobile ", objRecord.CurrentSponsorMobile));

                    objCmd.Parameters.Add(new SqlParameter("@Administration", objRecord.Administration));
                    objCmd.Parameters.Add(new SqlParameter("@ScientificDisciplines", objRecord.ScientificDisciplines));

                    objCmd.Parameters.Add(new SqlParameter("@Department", objRecord.Department));
                    objCmd.Parameters.Add(new SqlParameter("@BankAccountID", objRecord.BankAccountID));
                    objCmd.Parameters.Add(new SqlParameter("@PaymentMethod", objRecord.PaymentMethod));
                    objCmd.Parameters.Add(new SqlParameter("@EmpNotes", objRecord.EmpNotes));
                    objCmd.Parameters.Add(new SqlParameter("@TerminationReason", objRecord.TerminationReason));
                    objCmd.Parameters.Add(new SqlParameter("@ReportingDate", objRecord.ReportingDate));
                    objCmd.Parameters.Add(new SqlParameter("@ValidFromDate", objRecord.ValidFromDate));
                    objCmd.Parameters.Add(new SqlParameter("@LeaveNotes", objRecord.LeaveNotes));
                    objCmd.Parameters.Add(new SqlParameter("@WorkAddress", objRecord.WorkAddress));
                    objCmd.Parameters.Add(new SqlParameter("@WorkTel", objRecord.WorkTel));
                    objCmd.Parameters.Add(new SqlParameter("@WorkMobile", objRecord.WorkMobile));
                    objCmd.Parameters.Add(new SqlParameter("@WorkEmail", objRecord.WorkEmail));
                    objCmd.Parameters.Add(new SqlParameter("@CompanyVehicle", objRecord.CompanyVehicle));
                    objCmd.Parameters.Add(new SqlParameter("@HomeWorkDistance", objRecord.HomeWorkDistance));
                    objCmd.Parameters.Add(new SqlParameter("@AddressNotes", objRecord.AddressNotes));
                    objCmd.Parameters.Add(new SqlParameter("@WorkingHours", objRecord.WorkingHours));
                    objCmd.Parameters.Add(new SqlParameter("@HomeAddress", objRecord.HomeAddress));
                    objCmd.Parameters.Add(new SqlParameter("@HomeTel", objRecord.HomeTel));
                    objCmd.Parameters.Add(new SqlParameter("@HomeMobil", objRecord.HomeMobil));
                     
                    objCmd.Parameters.Add(new SqlParameter("@UserID", objRecord.UserID));
                    objCmd.Parameters.Add(new SqlParameter("@RegDate", objRecord.RegDate));
                    objCmd.Parameters.Add(new SqlParameter("@RegTime", objRecord.RegTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                    objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditDate", objRecord.EditDate));
                    objCmd.Parameters.Add(new SqlParameter("@ComputerInfo", objRecord.ComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@EditComputerInfo", objRecord.EditComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@Cancel", objRecord.Cancel));
                    objCmd.Parameters.Add(new SqlParameter("@AllowQTYPer", objRecord.AllowQTYPer));
                    objCmd.Parameters.Add(new SqlParameter("@Termination", objRecord.Termination));
                    objCmd.Parameters.Add(new SqlParameter("@StopSalary", objRecord.StopSalary));
                    objCmd.Parameters.Add(new SqlParameter("@ClinicID", Comon.cLong(objRecord.CardID)));
                    objCmd.Parameters.Add(new SqlParameter("@Emptype", objRecord.Emptype));
                    objCmd.Parameters.Add(new SqlParameter("@CheckSpendDate", objRecord.CheckSpendDate));
                    objCmd.Parameters.Add(new SqlParameter("@AccountMeter", objRecord.AccountMeter));
                    objCmd.Parameters.Add(new SqlParameter("@CostCenterID", objRecord.CostCenterID));
                    

                    SqlParameter pvNewId = new SqlParameter();
                    pvNewId.ParameterName = "@product_count";
                    pvNewId.DbType = DbType.Int32;
                    pvNewId.Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add(pvNewId);

                    if(IsNewRecord==true)
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 1));
                    else
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 2));

                    object obj = objCmd.ExecuteScalar();
                    string val = objCmd.Parameters["@product_count"].Value.ToString();
                    if (val != null)
                        objRet = Convert.ToInt64(val);

                }
         }
         return objRet;
      }
      public bool UpdateHR_EmployeeFile(HR_EmployeeFile objRecord)
      {
         bool objRet = false;
         objRet = false;
         using (SqlConnection objCnn = new GlobalConnection().Conn)
         {
             objCnn.Open();
             using (SqlCommand objCmd = objCnn.CreateCommand())
            {
               objCmd.CommandType = System.Data.CommandType.StoredProcedure;
               objCmd.CommandText = "[HR_EmployeeFile_SP]";
               objCmd.Parameters.Add(new SqlParameter("@EmployeeID", objRecord.EmployeeID));
               objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
               objCmd.Parameters.Add(new SqlParameter("@ArbName", objRecord.ArbName));
               objCmd.Parameters.Add(new SqlParameter("@EngName", objRecord.EngName));

               objCmd.Parameters.Add(new SqlParameter("@StopAccount", objRecord.StopAccount));
               objCmd.Parameters.Add(new SqlParameter("@FootprintEmpID", objRecord.FootprintEmpID));
               objCmd.Parameters.Add(new SqlParameter("@ParentAccountID", objRecord.ParentAccountID));
               objCmd.Parameters.Add(new SqlParameter("@Sex", objRecord.Sex));
               objCmd.Parameters.Add(new SqlParameter("@MaritalStatus", objRecord.MaritalStatus));
               objCmd.Parameters.Add(new SqlParameter("@BirthDate", objRecord.BirthDate));
               objCmd.Parameters.Add(new SqlParameter("@BirthPlace", objRecord.BirthPlace));
               objCmd.Parameters.Add(new SqlParameter("@Nationality", objRecord.Nationality));
               objCmd.Parameters.Add(new SqlParameter("@Religions", objRecord.Religions));
               objCmd.Parameters.Add(new SqlParameter("@WorkType", objRecord.WorkType));
               objCmd.Parameters.Add(new SqlParameter("@Occupation", objRecord.Occupation));
               objCmd.Parameters.Add(new SqlParameter("@IqamaOccupation", objRecord.IqamaOccupation));
               objCmd.Parameters.Add(new SqlParameter("@CurrentSponsor", objRecord.CurrentSponsor));
               objCmd.Parameters.Add(new SqlParameter("@ContractType", objRecord.ContractType));
               objCmd.Parameters.Add(new SqlParameter("@BeginningDate", objRecord.BeginningDate));
               objCmd.Parameters.Add(new SqlParameter("@ContractEnd", objRecord.ContractEnd));
               objCmd.Parameters.Add(new SqlParameter("@Administration", objRecord.Administration));
               objCmd.Parameters.Add(new SqlParameter("@Department", objRecord.Department));
               objCmd.Parameters.Add(new SqlParameter("@BankAccountID", objRecord.BankAccountID));
               objCmd.Parameters.Add(new SqlParameter("@PaymentMethod", objRecord.PaymentMethod));
               objCmd.Parameters.Add(new SqlParameter("@EmpNotes", objRecord.EmpNotes));
               objCmd.Parameters.Add(new SqlParameter("@TerminationReason", objRecord.TerminationReason));
               objCmd.Parameters.Add(new SqlParameter("@ReportingDate", objRecord.ReportingDate));
               objCmd.Parameters.Add(new SqlParameter("@ValidFromDate", objRecord.ValidFromDate));
               objCmd.Parameters.Add(new SqlParameter("@LeaveNotes", objRecord.LeaveNotes));
               objCmd.Parameters.Add(new SqlParameter("@WorkAddress", objRecord.WorkAddress));
               objCmd.Parameters.Add(new SqlParameter("@WorkTel", objRecord.WorkTel));
               objCmd.Parameters.Add(new SqlParameter("@WorkMobile", objRecord.WorkMobile));
               objCmd.Parameters.Add(new SqlParameter("@WorkEmail", objRecord.WorkEmail));
               objCmd.Parameters.Add(new SqlParameter("@CompanyVehicle", objRecord.CompanyVehicle));
               objCmd.Parameters.Add(new SqlParameter("@HomeWorkDistance", objRecord.HomeWorkDistance));
               objCmd.Parameters.Add(new SqlParameter("@AddressNotes", objRecord.AddressNotes));
               objCmd.Parameters.Add(new SqlParameter("@WorkingHours", objRecord.WorkingHours));
               objCmd.Parameters.Add(new SqlParameter("@HomeAddress", objRecord.HomeAddress));
               objCmd.Parameters.Add(new SqlParameter("@HomeTel", objRecord.HomeTel));
               objCmd.Parameters.Add(new SqlParameter("@HomeMobil", objRecord.HomeMobil));
               objCmd.Parameters.Add(new SqlParameter("@EmpImage", objRecord.EmpImage));
               objCmd.Parameters.Add(new SqlParameter("@UserID", objRecord.UserID));
               objCmd.Parameters.Add(new SqlParameter("@RegDate", objRecord.RegDate));
               objCmd.Parameters.Add(new SqlParameter("@RegTime", objRecord.RegTime));
               objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
               objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));
               objCmd.Parameters.Add(new SqlParameter("@EditDate", objRecord.EditDate));
               objCmd.Parameters.Add(new SqlParameter("@ComputerInfo", objRecord.ComputerInfo));
               objCmd.Parameters.Add(new SqlParameter("@EditComputerInfo", objRecord.EditComputerInfo));
               objCmd.Parameters.Add(new SqlParameter("@Cancel", objRecord.Cancel));
               objCmd.Parameters.Add(new SqlParameter("@Termination", objRecord.Termination));
               objCmd.Parameters.Add(new SqlParameter("@StopSalary", objRecord.StopSalary));
               objCmd.Parameters.Add(new SqlParameter("@ClinicID", objRecord.ClinicID));
               objCmd.Parameters.Add(new SqlParameter("@Emptype", objRecord.Emptype));
               objCmd.Parameters.Add(new SqlParameter("@CheckSpendDate", objRecord.CheckSpendDate));
               objCmd.Parameters.Add(new SqlParameter("@AccountMeter", objRecord.AccountMeter));
               objCmd.Parameters.Add(new SqlParameter("@CostCenterID", objRecord.CostCenterID));
               objCmd.Parameters.Add(new SqlParameter("@OnAccountID", objRecord.OnAccountID));

                    SqlParameter pvNewId = new SqlParameter();
                    pvNewId.ParameterName = "@product_count";
                    pvNewId.DbType = DbType.Int32;
                    pvNewId.Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add(pvNewId);


                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE",2));
               objCmd.ExecuteNonQuery();
            }
         }
         objRet = true;
         return objRet;
      }
      public bool DeleteHR_EmployeeFile(HR_EmployeeFile objRecord)
      {
         bool objRet = false;
         objRet = false;
         using (SqlConnection objCnn = new GlobalConnection().Conn)
         {
            objCnn.Open();
             using (SqlCommand objCmd = objCnn.CreateCommand())
            {
               objCmd.CommandType = System.Data.CommandType.StoredProcedure;
               objCmd.CommandText = "[HR_EmployeeFile_SP]";
               objCmd.Parameters.Add(new SqlParameter("@EmployeeID",objRecord. EmployeeID));
               objCmd.Parameters.Add(new SqlParameter("@BranchID",objRecord. BranchID));
               objCmd.Parameters.Add(new SqlParameter("@ModifiedBy",objRecord.UserID));
               objCmd.Parameters.Add(new SqlParameter("@CMDTYPE",4));
               objCmd.ExecuteNonQuery();
            }
         }
         objRet = true;
         return objRet;
      }
   }
}

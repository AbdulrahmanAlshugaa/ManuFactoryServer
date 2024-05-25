using Edex.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Edex.DAL.UsersManagement
{
    public class UsersManagementDAL
    {
        public static Users ConvertRowToObj(DataRow dr)
        {
            Users Obj = new Users();
            Obj.UserID = int.Parse(dr["UserID"].ToString());
            Obj.ArbName = dr["ArbName"].ToString();
            Obj.EngName = dr["EngName"].ToString();
            Obj.Email = dr["Email"].ToString();
            Obj.pic = (dr["pic"] == DBNull.Value ? null : (byte[])dr["pic"]);
            Obj.Address = dr["Address"].ToString();
            Obj.Mobile = dr["Mobile"].ToString();
            Obj.Gender = Comon.cInt(dr["Gender"].ToString());
            Obj.IsActive = Comon.cInt(dr["IsActive"].ToString());
            Obj.EmployeeID = Comon.cLong(dr["EmployeeID"].ToString());
            Obj.IsActive = Comon.cInt(dr["IsActive"].ToString());
            Obj.NumberAllowedDays = Comon.cInt(dr["NumberAllowedDays"].ToString());
            Obj.AllowedDate = Comon.cLong(dr["AllowedDate"].ToString());
            Obj.IsActiveAllowedDays = Comon.cInt(dr["IsActiveAllowedDays"].ToString());
            Obj.BranchID = Comon.cInt(dr["BranchID"].ToString());
            Obj.FacilityID = Comon.cInt(dr["FacilityID"].ToString());
            Obj.EmployeeID = Comon.cDbl(dr["EmployeeID"].ToString());
            Obj.RegDate = (Comon.cInt(dr["RegDate"].ToString()));
            Obj.EditUserID = Comon.cInt(dr["EditUserID"].ToString());
            Obj.RegTime = (Comon.cInt(dr["RegTime"].ToString()));
            Obj.EditUserID = (Comon.cInt(dr["EditUserID"].ToString()));
            Obj.EditDate = (Comon.cInt(dr["EditDate"].ToString()));
            Obj.EditTime = (Comon.cInt(dr["EditTime"].ToString()));
            Obj.ComputerInfo = dr["ComputerInfo"].ToString();
            Obj.EditComputerInfo = dr["EditComputerInfo"].ToString();
            Obj.Cancel = Comon.cInt(dr["Cancel"].ToString());

            Obj.CostCenterID = Comon.cInt(dr["DefaultStoreID"].ToString());
            Obj.MainBoxAccountID = Comon.cDbl(dr["MainBoxAccountID"].ToString());
            Obj.StoreID = Comon.cInt(dr["DefaultStoreID"].ToString());

           
        

            return Obj;
        }
        public static Users GetDataByID(int ID, int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Users_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@UserID", ID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 3));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            Users Returned = new Users();
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
        public static List<Users> GetAllData(int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Users_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 5));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            List<Users> Returned = new List<Users>();
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
        public static DataTable GetUser(int BranchID, int FacilityID)
        {
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Users_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 5));
                    SqlDataReader myreader = objCmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(myreader);
                    return dt;
                }
            }
        }
        public static Int32 InsertUser(Users objRecord)
        {
            Int32 objRet = 0;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Users_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@UserID", objRecord.UserID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@ArbName", objRecord.ArbName));
                    objCmd.Parameters.Add(new SqlParameter("@EngName", objRecord.EngName));
                    objCmd.Parameters.Add(new SqlParameter("@Mobile", objRecord.Mobile));
                    objCmd.Parameters.Add(new SqlParameter("@Gender", objRecord.Gender));
                    objCmd.Parameters.Add(new SqlParameter("@Address", objRecord.Address));
                    objCmd.Parameters.Add(new SqlParameter("@Notes", objRecord.Notes));
                    objCmd.Parameters.Add(new SqlParameter("@pic", objRecord.pic));
                    objCmd.Parameters.Add(new SqlParameter("@Email", objRecord.Email));
                    objCmd.Parameters.Add(new SqlParameter("@EmployeeID", objRecord.EmployeeID));
                    objCmd.Parameters.Add(new SqlParameter("@Password", objRecord.Password));
                    objCmd.Parameters.Add(new SqlParameter("@IsActive", objRecord.IsActive));
                    objCmd.Parameters.Add(new SqlParameter("@NumberAllowedDays", objRecord.NumberAllowedDays));
                    objCmd.Parameters.Add(new SqlParameter("@IsActiveAllowedDays", objRecord.IsActiveAllowedDays));
                    objCmd.Parameters.Add(new SqlParameter("@AllowedDate", objRecord.AllowedDate));
                    objCmd.Parameters.Add(new SqlParameter("@AddByUserID", objRecord.AddByUserID));
                    objCmd.Parameters.Add(new SqlParameter("@RegDate", objRecord.RegDate));
                    objCmd.Parameters.Add(new SqlParameter("@RegTime", objRecord.RegTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                    objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditDate", objRecord.EditDate));
                    objCmd.Parameters.Add(new SqlParameter("@ComputerInfo", objRecord.ComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@EditComputerInfo", objRecord.EditComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@Cancel", objRecord.Cancel));
                    if(objRecord.UserID==0)
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 1));
                    else
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 2));

                    object obj = objCmd.ExecuteScalar();
                    if (obj != null)
                           objRet= Convert.ToInt32(obj);
                }
            }
            return objRet;
        }
        public static bool UpdateUser(Users objRecord)
        {
            bool objRet = false;
            objRet = false;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Users_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@UserID", objRecord.UserID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@ArbName", objRecord.ArbName));
                    objCmd.Parameters.Add(new SqlParameter("@EngName", objRecord.EngName));
                    objCmd.Parameters.Add(new SqlParameter("@Mobile", objRecord.Mobile));
                    objCmd.Parameters.Add(new SqlParameter("@Gender", objRecord.Gender));
                    objCmd.Parameters.Add(new SqlParameter("@Address", objRecord.Address));
                    objCmd.Parameters.Add(new SqlParameter("@Notes", objRecord.Notes));
                    objCmd.Parameters.Add(new SqlParameter("@pic", objRecord.pic));
                    objCmd.Parameters.Add(new SqlParameter("@Email", objRecord.Email));
                    objCmd.Parameters.Add(new SqlParameter("@EmployeeID", objRecord.EmployeeID));
                    objCmd.Parameters.Add(new SqlParameter("@Password", (Security.HashSHA1(objRecord.Password)).ToString()));
                    objCmd.Parameters.Add(new SqlParameter("@IsActive", objRecord.IsActive));
                    objCmd.Parameters.Add(new SqlParameter("@NumberAllowedDays", objRecord.NumberAllowedDays));
                    objCmd.Parameters.Add(new SqlParameter("@IsActiveAllowedDays", objRecord.IsActiveAllowedDays));
                    objCmd.Parameters.Add(new SqlParameter("@AllowedDate", objRecord.AllowedDate));
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
        public static bool DeleteUser(Users objRecord)
        {
            bool objRet = false;
            objRet = false;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Users_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@UserID", objRecord.UserID));
                    objCmd.Parameters.Add(new SqlParameter("@EditDate", objRecord.EditDate));
                    objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                    objCmd.Parameters.Add(new SqlParameter("@EditComputerInfo", objRecord.EditComputerInfo));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 4));
                    objCmd.ExecuteNonQuery();
                }
            }
            objRet = true;
            return objRet;
        }



        /*******************************************************************************/
        public static UserRoles ConvertRowToUserRolesObj(DataRow dr)
        {
            UserRoles Obj = new UserRoles();
            Obj.RoleID = Comon.cInt(dr["RoleID"].ToString());
            Obj.RoleArbName = dr["RoleArbName"].ToString();
            Obj.RoleEngName = dr["RoleEngName"].ToString();
            Obj.RoleDescription = dr["RoleDescription"].ToString();
            Obj.IsSystemAdmin = Comon.cbool(dr["IsSystemAdmin"].ToString());
            Obj.Selected = "";
            return Obj;
        }
        public static List<UserRoles> GetAllUserRolesData(int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[UserRoles_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 5));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            List<UserRoles> Returned = new List<UserRoles>();
                            foreach (DataRow rows in dt.Rows)
                                Returned.Add(ConvertRowToUserRolesObj(rows));
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
        public static UserRoles GetUserRolesDataByID(int ID, int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[UserRoles_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@RoleID", ID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 3));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null && dt.Rows.Count > 0)
                        {

                            UserRoles Returned = new UserRoles();
                            Returned = (ConvertRowToUserRolesObj(dt.Rows[0]));
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
        public static Int32 InsertUserRoles(UserRoles objRecord)
        {
            Int32 objRet = 0;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[UserRoles_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@RoleID", objRecord.RoleID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@RoleArbName", objRecord.RoleArbName));
                    objCmd.Parameters.Add(new SqlParameter("@RoleEngName", objRecord.RoleEngName));
                    objCmd.Parameters.Add(new SqlParameter("@RoleDescription", objRecord.RoleDescription));
                    objCmd.Parameters.Add(new SqlParameter("@IsSystemAdmin", objRecord.IsSystemAdmin));
                    objCmd.Parameters.Add(new SqlParameter("@AddByUserID", objRecord.AddByUserID));
                    objCmd.Parameters.Add(new SqlParameter("@RegDate", objRecord.RegDate));
                    objCmd.Parameters.Add(new SqlParameter("@RegTime", objRecord.RegTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                    objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditDate", objRecord.EditDate));
                    objCmd.Parameters.Add(new SqlParameter("@Cancel", objRecord.Cancel));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 1));
                    object obj = objCmd.ExecuteScalar();
                    if (obj != null)
                        objRet = Convert.ToInt32(obj);
                }
            }
            return objRet;
        }
        public static bool UpdateUserRoles(UserRoles objRecord)
        {
            bool objRet = false;
            objRet = false;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[UserRoles_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@RoleID", objRecord.RoleID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@RoleArbName", objRecord.RoleArbName));
                    objCmd.Parameters.Add(new SqlParameter("@RoleEngName", objRecord.RoleEngName));
                    objCmd.Parameters.Add(new SqlParameter("@RoleDescription", objRecord.RoleDescription));
                    objCmd.Parameters.Add(new SqlParameter("@IsSystemAdmin", objRecord.IsSystemAdmin));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                    objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditDate", objRecord.EditDate));
                    objCmd.Parameters.Add(new SqlParameter("@Cancel", objRecord.Cancel));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 2));
                    objCmd.ExecuteNonQuery();
                }
            }
            objRet = true;
            return objRet;
        }
        public static bool DeleteUserRoles(UserRoles objRecord)
        {
            bool objRet = false;
            objRet = false;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[UserRoles_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@RoleID", objRecord.RoleID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@EditDate", objRecord.EditDate));
                    objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 4));
                    objCmd.ExecuteNonQuery();
                }
            }
            objRet = true;
            return objRet;
        }
        /******************************************************************/
        public static UserPermissions ConvertRowToUserPermissionsObj(DataRow dr)
        {
            UserPermissions Obj = new UserPermissions();
            Obj.PermissionID = Comon.cInt(dr["PermissionID"].ToString());
            Obj.PermissionArbName = dr["PermissionArbName"].ToString();
            Obj.PermissionEngName = dr["PermissionEngName"].ToString();
            Obj.Selected = "";
            return Obj;
        }
        public static List<UserPermissions> GetAllUserPermissionsData(int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[UserPermissions_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 5));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            List<UserPermissions> Returned = new List<UserPermissions>();
                            foreach (DataRow rows in dt.Rows)
                                Returned.Add(ConvertRowToUserPermissionsObj(rows));
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
        public static UserPermissions GetUserPermissionsDataByID(int ID, int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[UserPermissions_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@PermissionID", ID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 3));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null && dt.Rows.Count > 0)
                        {

                            UserPermissions Returned = new UserPermissions();
                            Returned = (ConvertRowToUserPermissionsObj(dt.Rows[0]));
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
        public static Int32 InsertUserPermissions(UserPermissions objRecord)
        {
            Int32 objRet = 0;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[UserPermissions_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@PermissionID", objRecord.PermissionID));
                    objCmd.Parameters.Add(new SqlParameter("@PermissionArbName", objRecord.PermissionArbName));
                    objCmd.Parameters.Add(new SqlParameter("@PermissionEngName", objRecord.PermissionEngName));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@Cancel", objRecord.Cancel));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 1));
                    object obj = objCmd.ExecuteScalar();
                    if (obj != null)
                        objRet = Convert.ToInt32(obj);
                }
            }
            return objRet;
        }
        public static bool UpdateUserPermissions(UserPermissions objRecord)
        {
            bool objRet = false;
            objRet = false;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[UserPermissions_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@PermissionID", objRecord.PermissionID));
                    objCmd.Parameters.Add(new SqlParameter("@PermissionArbName", objRecord.PermissionArbName));
                    objCmd.Parameters.Add(new SqlParameter("@PermissionEngName", objRecord.PermissionEngName));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@Cancel", objRecord.Cancel));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 2));
                    objCmd.ExecuteNonQuery();
                }
            }
            objRet = true;
            return objRet;
        }
        public static bool DeleteUserPermissions(UserPermissions objRecord)
        {
            bool objRet = false;
            objRet = false;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[UserPermissions_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@PermissionID", objRecord.PermissionID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 4));
                    objCmd.ExecuteNonQuery();
                }
            }
            objRet = true;
            return objRet;
        }
        /***********************************************/
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
        public static int InsertLinkRolePermissionUsingXML(string RoleID, List<UserPermissions> listUserRoleLinkPermission, int BranchID, int FacilityID)
        {

            Int32 objRet = 0;
            string DitmeXML = ConvertObjectToXMLString(listUserRoleLinkPermission);

         //  DitmeXML= DitmeXML.Replace("<int>", "<int><PermissionID>").Replace("</int>", "</PermissionID></int>");
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[UserLinkRolePermission_SP]";
                    objCmd.Parameters.Add(new SqlParameter("xmlData", SqlDbType.Xml)).Value = DitmeXML;
                    objCmd.Parameters.Add(new SqlParameter("@RoleID", Comon.cInt(RoleID)));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 1));

                    object obj = objCmd.ExecuteScalar();
                    if (obj != null)
                        objRet = Convert.ToInt32(obj);
                }
            }
            return objRet;

        }
        public static UserPermissions ConvertRowToLinkRolePermissionObj(DataRow dr)
        {
            UserPermissions Obj = new UserPermissions();
            Obj.PermissionID = Comon.cInt(dr["PermissionID"].ToString());
            Obj.PermissionArbName = dr["PermissionArbName"].ToString();
            Obj.PermissionEngName = dr["PermissionEngName"].ToString();
            Obj.Selected = "Selected";
            return Obj;
        }
        public static List<UserPermissions> GetAllLinkRolePermissionData(int RoleID, int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[UserLinkRolePermission_SP]";
 
                        objCmd.Parameters.Add(new SqlParameter("@RoleID", Comon.cInt(RoleID)));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 2));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            List<UserPermissions> Returned = new List<UserPermissions>();
                            foreach (DataRow rows in dt.Rows)
                                Returned.Add(ConvertRowToLinkRolePermissionObj(rows));
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
        /******************************************************************/
        public static int InsertUserLinkRoleUsingXML(string UserID, List<int> ArrayID, int BranchID, int FacilityID)
        {


            Int32 objRet = 0;
            string DitmeXML = ConvertObjectToXMLString(ArrayID);

            DitmeXML = DitmeXML.Replace("<int>", "<int><RoleID>").Replace("</int>", "</RoleID></int>");
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[UserLinkRole_SP]";
                    objCmd.Parameters.Add(new SqlParameter("xmlData", SqlDbType.Xml)).Value = DitmeXML;
                    objCmd.Parameters.Add(new SqlParameter("@UserID", Comon.cInt(UserID)));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 1));

                    object obj = objCmd.ExecuteScalar();
                    if (obj != null)
                        objRet = Convert.ToInt32(obj);
                }
            }
            return objRet;

        }
        public static UserRoles ConvertRowToUserLinkRoleObj(DataRow dr)
        {
            UserRoles Obj = new UserRoles();
            Obj.RoleID = Comon.cInt(dr["RoleID"].ToString());
            Obj.RoleArbName = dr["RoleArbName"].ToString();
            Obj.RoleEngName = dr["RoleEngName"].ToString();
            Obj.Selected = "Selected";
            return Obj;
        }
        public static List<UserRoles> GetAllUserLinkRoleData(int UserID, int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[UserLinkRole_SP]";

                        objCmd.Parameters.Add(new SqlParameter("@UserID", Comon.cInt(UserID)));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 2));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            List<UserRoles> Returned = new List<UserRoles>();
                            foreach (DataRow rows in dt.Rows)
                                Returned.Add(ConvertRowToUserLinkRoleObj(rows));
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
        /******************************************************************/
        public static UserPermissions ConvertRowToGetUserAuthoriseObj(DataRow dr)
        {
            UserPermissions Obj = new UserPermissions();
            Obj.PermissionID = Comon.cInt(dr["PermissionID"].ToString());
            Obj.PermissionArbName = dr["PermissionArbName"].ToString();
            Obj.PermissionEngName = dr["PermissionEngName"].ToString();
            Obj.VIEW = Comon.cInt(dr["VIEW"].ToString());
            Obj.ADD = Comon.cInt( dr["ADD"].ToString());
            Obj.UPDATE = Comon.cInt(dr["UPDATE"].ToString());
            Obj.DELETE = Comon.cInt(dr["DELETE"].ToString());
            Obj.DaysAllowedForEdit = Comon.cInt(dr["DaysAllowedForEdit"].ToString());
            return Obj;
        }
    

        public static UserPermissions GetUserAuthorise(string UserName, string RoleName, string PageName, int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[UserAuthorise_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@UserName", UserName));
                        objCmd.Parameters.Add(new SqlParameter("@RoleName", RoleName));
                        objCmd.Parameters.Add(new SqlParameter("@PageName", PageName));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 1));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            UserPermissions Returned = new UserPermissions();
                            Returned = (ConvertRowToGetUserAuthoriseObj(dt.Rows[0]));
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

        public static UserPermissions GetUserAuthorise(int UserID, int RoleID, string PageName, int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[UserAuthorise_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@UserID", UserID));
                        objCmd.Parameters.Add(new SqlParameter("@RoleID", RoleID));
                        objCmd.Parameters.Add(new SqlParameter("@PageName", PageName));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 2));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            UserPermissions Returned = new UserPermissions();
                            Returned = (ConvertRowToGetUserAuthoriseObj(dt.Rows[0]));
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
        public static List<UserPermissions> GetUserAuthoriseForMultiPages(int UserID, int RoleID, string PageName, int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[UserAuthorise_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@UserID", UserID));
                        objCmd.Parameters.Add(new SqlParameter("@RoleID", RoleID));
                        objCmd.Parameters.Add(new SqlParameter("@PageName", PageName));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 4));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            List<UserPermissions> Returned = new List<UserPermissions>();
                            foreach (DataRow rows in dt.Rows)
                                Returned.Add(ConvertRowToGetUserAuthoriseObj(rows));
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
        /*******************************Desktop Windows Forms Applaction**************************************/


        public static UserReportsPermissions frmConvertRowToUserReportsPermissionsObj(DataRow dr)
        {
            UserReportsPermissions Obj = new UserReportsPermissions();
            
            Obj.ReportName = dr["ReportName"].ToString();
            Obj.UserID = Comon.cInt(dr["UserID"].ToString());
            Obj.ReportView = Comon.cInt(dr["ReportView"].ToString());
            Obj.ReportExport = Comon.cInt(dr["ReportExport"].ToString());
            Obj.ShowReportInReportViewer = Comon.cInt(dr["ShowReportInReportViewer"].ToString());
            return Obj;
        }
        public static List<UserReportsPermissions> frmGetAllUserReportsPermissions(int User, int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[frmUserPermissions_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@UserID", User));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 10));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            List<UserReportsPermissions> Returned = new List<UserReportsPermissions>();
                            foreach (DataRow rows in dt.Rows)
                                Returned.Add(frmConvertRowToUserReportsPermissionsObj(rows));
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
        public static UserFormsPermissions frmConvertRowToUserFormsPermissionsObj(DataRow dr)
        {
            UserFormsPermissions Obj = new UserFormsPermissions();
            Obj.FormName = dr["FormName"].ToString();
            Obj.UserID = Comon.cInt(dr["UserID"].ToString());
            Obj.FormAdd = Comon.cInt(dr["FormAdd"].ToString());
            Obj.FormDelete = Comon.cInt(dr["FormDelete"].ToString());
            Obj.FormUpdate = Comon.cInt(dr["FormUpdate"].ToString());
            Obj.FormView = Comon.cInt(dr["FormView"].ToString());
            Obj.DaysAllowedForEdit = Comon.cInt(dr["DaysAllowedForEdit"].ToString());
            return Obj;
        }
        public static List<UserFormsPermissions> frmGetAllUserFormsPermissions(int User,int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[frmUserPermissions_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@UserID", User));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 9));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            List<UserFormsPermissions> Returned = new List<UserFormsPermissions>();
                            foreach (DataRow rows in dt.Rows)
                                Returned.Add(frmConvertRowToUserFormsPermissionsObj(rows));
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

        public static UserMenusPermissions frmConvertRowToUserMenusPermissionsObj(DataRow dr)
        {
            UserMenusPermissions Obj = new UserMenusPermissions();
            Obj.UserID = Comon.cInt(dr["UserID"].ToString());
            Obj.MenuName = dr["MenuName"].ToString();
            Obj.MenuView =Comon.cInt(dr["MenuView"].ToString());
            return Obj;
        }
        public static List<UserMenusPermissions> frmGetAllUserMenusPermissions(int User, int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[frmUserPermissions_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@UserID", User));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 8));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            List<UserMenusPermissions> Returned = new List<UserMenusPermissions>();
                            foreach (DataRow rows in dt.Rows)
                                Returned.Add(frmConvertRowToUserMenusPermissionsObj(rows));
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

        public static Int32 frmInsertUserFormsPermissions(int SelectedUserID,int SelectedBranchID,List<UserFormsPermissions> listUserFormsPermissions)
        {

            Int32 objRet = 0;
            string DitmeXML = ConvertObjectToXMLString(listUserFormsPermissions);
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[frmUserPermissions_SP]";
                    objCmd.Parameters.Add(new SqlParameter("xmlData", SqlDbType.Xml)).Value = DitmeXML;
                    objCmd.Parameters.Add(new SqlParameter("@UserID", SelectedUserID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", SelectedBranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", MySession.GlobalFacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 1));

                    object obj = objCmd.ExecuteScalar();
                    if (obj != null)
                        objRet = Convert.ToInt32(1);
                }
            }
            return objRet;
        }

        public static Int32 frmInsertUserReportsPermissions(int SelectedUserID, int SelectedBranchID, List<UserReportsPermissions> listUserReportsPermissions)
        {

            Int32 objRet = 0;
            string DitmeXML = ConvertObjectToXMLString(listUserReportsPermissions);
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[frmUserPermissions_SP]";
                    objCmd.Parameters.Add(new SqlParameter("xmlData", SqlDbType.Xml)).Value = DitmeXML;
                    objCmd.Parameters.Add(new SqlParameter("@UserID", SelectedUserID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", SelectedBranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", MySession.GlobalFacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 2));

                    object obj = objCmd.ExecuteScalar();
                    if (obj != null)
                        objRet = Convert.ToInt32(1);
                }
            }
            return objRet;
        }
        public static Int32 frmInsertUserMenusPermissions(int SelectedUserID, int SelectedBranchID, List<UserMenusPermissions> listUserMenusPermissions)
        {

            Int32 objRet = 0;
            string DitmeXML = ConvertObjectToXMLString(listUserMenusPermissions);
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[frmUserPermissions_SP]";
                    objCmd.Parameters.Add(new SqlParameter("xmlData", SqlDbType.Xml)).Value = DitmeXML;
                    objCmd.Parameters.Add(new SqlParameter("@UserID", SelectedUserID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", SelectedBranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", MySession.GlobalFacilityID));

                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 3));

                    object obj = objCmd.ExecuteScalar();
                    if (obj != null)
                        objRet = Convert.ToInt32(1);
                }
            }
            return objRet;
        }
        public static Int32 frmInsertUserOtherPermissions(int SelectedUserID, int SelectedBranchID, List<UserOtherPermissions> listUserOtherPermissions)
        {

            Int32 objRet = 0;
            string DitmeXML = ConvertObjectToXMLString(listUserOtherPermissions);
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[frmUserPermissions_SP]";
                    objCmd.Parameters.Add(new SqlParameter("xmlData", SqlDbType.Xml)).Value = DitmeXML;
                    objCmd.Parameters.Add(new SqlParameter("@UserID", SelectedUserID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", SelectedBranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", MySession.GlobalFacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 4));

                    object obj = objCmd.ExecuteScalar();
                    if (obj != null)
                        objRet = Convert.ToInt32(1);
                }
            }
            return objRet;
        }


    }
}

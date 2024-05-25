using Edex.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.DAL.UsersManagement
{
    public class LoginValidation
    {

        public static UserBO ConvertRowToObj(DataRow dr)
        {
            UserBO Obj = new UserBO();
            Obj.ID = int.Parse(dr["U_ID"].ToString());
            Obj.UserName = dr["USERNAME"].ToString();
            Obj.FacilityName = dr["COMPANYNAME"].ToString();
            Obj.BranchName = dr["BRANCHNAME"].ToString();
            Obj.Notes = dr["Notes"].ToString();
            return Obj;
        }
        public static UserBO Login(string UserName, string password,int BranchID)
        {
            UserBO _User = null;            
            SqlConnection Con = new GlobalConnection().Conn;
            try
            {
                string hashedPassword = Security.HashSHA1(password);
                SqlCommand cmd = new SqlCommand("loginuser", Con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@_User_ID", UserName);
                cmd.Parameters.AddWithValue("@_pass", hashedPassword);
                cmd.Parameters.AddWithValue("@BranchID", BranchID);
                if ((Con.State == ConnectionState.Closed))
                {
                    Con.Open();
                }
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    _User = new UserBO();
                    _User.SYSUSERARBNAME = reader["ArbName"].ToString();
                    _User.SYSUSERID = int.Parse(reader["UserID"].ToString());
                    _User.SYSUSERENGNAME = reader["EngName"].ToString();
                    _User.pic = (reader["pic"] == DBNull.Value ? null : (byte[])reader["pic"]);
                    _User.NumberAllowedDays = int.Parse(reader["NumberAllowedDays"].ToString());
                    _User.IsActiveAllowedDays = int.Parse(reader["IsActiveAllowedDays"].ToString());
                    _User.AllowedDate = Comon.ConvertSerialDateTo(reader["AllowedDate"].ToString());
                    _User.IsActive = int.Parse(reader["IsActive"].ToString());
                    _User.FacilityID = Comon.cInt(reader["FacilityID"].ToString());
                    _User.BRANCHID = Comon.cInt(reader["BRANCHID"].ToString());
                    _User.BranchName = reader["BranchName"].ToString();
                    _User.FacilityName = reader["FacilityName"].ToString();
                    _User.Notes = reader["Notes"].ToString();
                    _User.MainTyepScreen = Comon.cInt(reader["MainTyepScreen"].ToString());
                    UserInfo.SYSUSERARBNAME = _User.SYSUSERARBNAME;
                    UserInfo.ID = _User.SYSUSERID;
                    UserInfo.SYSUSERENGNAME = _User.SYSUSERENGNAME;
                    UserInfo.UserName = _User.SYSUSERARBNAME;
                    UserInfo.FacilityName = _User.FacilityName;
                    UserInfo.BranchName = _User.BranchName;
                    UserInfo.pic = _User.pic;
                    UserInfo.BRANCHID = _User.BRANCHID;
                    UserInfo.FacilityID = _User.FacilityID;

                }
            }
            catch (Exception ex)
            {
                _User = null;
                Con.Close();
            }
            return _User;

        }

    }
}

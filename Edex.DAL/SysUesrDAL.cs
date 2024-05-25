using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
 
using Edex.Model;
using Edex.DAL;

namespace EDEXV3.DAL
{
  public   class SysUesrDAL
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
        public static UserBO Login(int User_ID, string password)
        {
            UserBO _User = null;
            SqlConnection Con = new GlobalConnection().Conn;
            try
            {
                string hashedPassword = Security.HashSHA1(password);
                SqlCommand cmd = new SqlCommand("loginuser", Con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@_User_ID", User_ID);
                cmd.Parameters.AddWithValue("@_pass", hashedPassword);
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
                }
            }
            catch (Exception ex)
            {
                _User = null;
                 Con.Close();
            }
            return  _User;

        }

    }
}

using Edex.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.DAL.Inbox
{
    public class InboxMessagesDAL
    {
        public static InboxMessages ConvertRowToObj(DataRow dr)
        {

            InboxMessages Obj = new InboxMessages();
            Obj.ID = int.Parse(dr["ID"].ToString());
            Obj.Message = dr["Message"].ToString();
            Obj.Subject = dr["Subject"].ToString();
            Obj.status = int.Parse(dr["status"].ToString());
            Obj.SenderID = int.Parse(dr["SenderID"].ToString());
            Obj.SenderName = dr["SenderName"].ToString();
            Obj.ReceiverID = int.Parse(dr["ReceiverID"].ToString());
            Obj.BranchID = int.Parse(dr["BranchID"].ToString());
            Obj.FacilityID = int.Parse(dr["FacilityID"].ToString());
            Obj.RegDate = (long.Parse(dr["RegDate"].ToString()));
            Obj.EditUserID = Comon.cInt(dr["EditUserID"].ToString());
            Obj.RegTime = (long.Parse(dr["RegTime"].ToString()));
            Obj.EditUserID = (int.Parse(dr["EditUserID"].ToString()));
            Obj.EditDate = (long.Parse(dr["EditDate"].ToString()));
            Obj.EditTime = (int.Parse(dr["EditTime"].ToString()));
            Obj.BranchID = int.Parse(dr["BranchID"].ToString());
            Obj.Cancel = int.Parse(dr["Cancel"].ToString());
            return Obj;
        }
        public static InboxMessages ConvertRowToObjSender(DataRow dr)
        {
            InboxMessages Obj = new InboxMessages();
            Obj.ID = int.Parse(dr["ID"].ToString());
            Obj.Message = dr["Message"].ToString();
            Obj.Subject = dr["Subject"].ToString();
            Obj.status = int.Parse(dr["status"].ToString());
            Obj.SenderID = int.Parse(dr["SenderID"].ToString());
            Obj.SenderName = dr["SenderName"].ToString();
            Obj.AddDate = Comon.ConvertSerialDateTo(dr["RegDate"].ToString());
            Obj.RegTime = (long.Parse(dr["RegTime"].ToString()));
            return Obj;
        }
        public static InboxMessages ConvertRowToObjReceiver(DataRow dr)
        {
            InboxMessages Obj = new InboxMessages();
            Obj.ID = int.Parse(dr["ID"].ToString());
            Obj.Message = dr["Message"].ToString();
            Obj.Subject = dr["Subject"].ToString();
            Obj.status = int.Parse(dr["status"].ToString());
            Obj.ReceiverID = int.Parse(dr["ReceiverID"].ToString());
            Obj.ReceiverName = dr["ReceiverName"].ToString();
            Obj.AddDate = Comon.ConvertSerialDateTo(dr["RegDate"].ToString());
            Obj.RegTime = (long.Parse(dr["RegTime"].ToString()));
            return Obj;
        }
        public static List<InboxMessages> GetDataBySender(int ID, int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[InboxMessages_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@SenderID", ID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 3));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            List<InboxMessages> Returned = new List<InboxMessages>();
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
        public static List<InboxMessages> GetDataByReceiver(int ID, int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[InboxMessages_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@ReceiverID", ID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 4));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            List<InboxMessages> Returned = new List<InboxMessages>();
                            foreach (DataRow rows in dt.Rows)
                                Returned.Add(ConvertRowToObjSender(rows));
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
        public static List<InboxMessages> GetMailInbox(int ID, int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[InboxMessages_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@ReceiverID", ID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 6));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            List<InboxMessages> Returned = new List<InboxMessages>();
                            foreach (DataRow rows in dt.Rows)
                                Returned.Add(ConvertRowToObjSender(rows));
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
        public static List<InboxMessages> GetMailSent(int ID, int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[InboxMessages_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@SenderID", ID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 7));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            List<InboxMessages> Returned = new List<InboxMessages>();
                            foreach (DataRow rows in dt.Rows)
                                Returned.Add(ConvertRowToObjReceiver(rows));
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
        public static InboxMessages ReadMessageByID(int ID, int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[InboxMessages_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@ID", ID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 5));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            List<InboxMessages> Returned = new List<InboxMessages>();
                            foreach (DataRow rows in dt.Rows)
                                Returned.Add(ConvertRowToObjSender(rows));
                            return Returned[0];
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
        public static List<InboxMessages> GetAllData(int BranchID, int FacilityID)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[InboxMessages_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 4));
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(myreader);
                        if (dt != null)
                        {
                            List<InboxMessages> Returned = new List<InboxMessages>();
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
        public static Int32 InsertInboxMessages(InboxMessages objRecord)
        {
            Int32 objRet = 0;
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[InboxMessages_SP]";

                    objCmd.Parameters.Add(new SqlParameter("@ID", objRecord.ID));
                    objCmd.Parameters.Add(new SqlParameter("@Subject", objRecord.Subject));
                    objCmd.Parameters.Add(new SqlParameter("@Message", objRecord.Message));
                    objCmd.Parameters.Add(new SqlParameter("@ReceiverID", objRecord.ReceiverID));
                    objCmd.Parameters.Add(new SqlParameter("@SenderID", objRecord.SenderID));
                    objCmd.Parameters.Add(new SqlParameter("@AddByUserID", objRecord.AddByUserID));
                    objCmd.Parameters.Add(new SqlParameter("@status", objRecord.status));
                    objCmd.Parameters.Add(new SqlParameter("@RegDate", objRecord.RegDate));
                    objCmd.Parameters.Add(new SqlParameter("@RegTime", objRecord.RegTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditUserID", objRecord.EditUserID));
                    objCmd.Parameters.Add(new SqlParameter("@EditTime", objRecord.EditTime));
                    objCmd.Parameters.Add(new SqlParameter("@EditDate", objRecord.EditDate));
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

    }
}

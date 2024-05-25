using Edex.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Edex.DAL.SalseSystem.Stc_itemDAL
{
    public class Stc_ItemsMoviingDAL
    {
        public static Stc_ItemsMoviing ConvertRowToObj(DataRow dr)
        {
            Stc_ItemsMoviing Obj = new Stc_ItemsMoviing();
            Obj.BarCode = dr["BarCode"].ToString();
            Obj.Bones =Comon.cDbl( dr["Bones"].ToString());
            Obj.BranchID =Comon.cInt( dr["BranchID"].ToString());
            Obj.Cancel = int.Parse(dr["UserID"].ToString());
            Obj.CostCenterID = Comon.cInt(dr["CostCenterID"].ToString());
            Obj.DocumentTypeID = Comon.cInt( dr["DocumentTypeID"].ToString());
            Obj.ExpiryDate = Comon.cDbl(dr["ExpiryDate"].ToString());
            Obj.FacilityID =Comon.cInt( dr["FacilityID"].ToString());
            Obj.ID =Comon.cInt( dr["ID"].ToString());
            Obj.InPrice = Comon.cDbl(dr["InPrice"].ToString());
            Obj.ItemID = Comon.cInt(dr["ItemID"].ToString());
            Obj.MoveDate =   dr["MoveDate"].ToString() ;
            Obj.MoveID = Comon.cInt(dr["MoveID"].ToString());
            Obj.MoveType = Comon.cInt(dr["MoveType"].ToString());
            Obj.OutPrice = Comon.cDbl(dr["OutPrice"].ToString());
            Obj.QTY = Comon.cDbl(dr["QTY"].ToString());
            Obj.SizeID = Comon.cInt(dr["SizeID"].ToString());
            Obj.GroupID = Comon.cDbl(dr["GroupID"].ToString());
            Obj.StoreID = Comon.cDbl(dr["StoreID"].ToString());
            Obj.AccountID = Comon.cDbl(dr["AccountID"].ToString());
            Obj.TranseID = Comon.cInt(dr["TranseID"].ToString());
            return Obj;
        }
        public static Stc_ItemsMoviing GetDataByID(int TranseID)
        {
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Stc_ItemsMoviing_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@TranseID", TranseID));

                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 5));
                    SqlDataReader myreader = objCmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(myreader);
                    if (dt != null)
                    {
                        Stc_ItemsMoviing Returned = new Stc_ItemsMoviing();
                        Returned = (ConvertRowToObj(dt.Rows[0]));
                        return Returned;
                    }
                    else
                        return null;
                }
            }
        }
        public static List<Stc_ItemsMoviing> GetAllData()
        {
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Stc_ItemsMoviing_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 3));
                    SqlDataReader myreader = objCmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(myreader);
                    if (dt != null)
                    {
                        List<Stc_ItemsMoviing> Returned = new List<Stc_ItemsMoviing>();
                        foreach (DataRow rows in dt.Rows)
                            Returned.Add(ConvertRowToObj(rows));
                        return Returned;
                    }
                    else
                        return null;
                }
            }
        }
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
        public static string Insert(Stc_ItemsMoviing objRecord,bool isNewRecord)
        {
            Int32 objRet = 0;
            string DitmeXML = ConvertObjectToXMLString(objRecord.ObjDatails);
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Stc_ItemsMoviing_SP]";
                    objCmd.Parameters.Add(new SqlParameter("XmlData", SqlDbType.Xml)).Value = DitmeXML;
                    objCmd.Parameters.Add(new SqlParameter("@TranseID", objRecord.TranseID));
                    objCmd.Parameters.Add(new SqlParameter("@DocumentTypeID", objRecord.DocumentTypeID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID));
                    objCmd.Parameters.Add(new SqlParameter("@FacilityID", objRecord.FacilityID));
                    objCmd.Parameters.Add(new SqlParameter("@Posted", objRecord.Posted)); 
                    if (isNewRecord)
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 1));
                    else
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 2));
                    object obj = objCmd.ExecuteScalar();
                    if (obj != null)
                        return  obj.ToString();
                }
            }
            return "0";
        }
        public static int Delete(Stc_ItemsMoviing objRecord)
        {
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[Stc_ItemsMoviing_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@DocumentTypeID", objRecord.DocumentTypeID));
                    objCmd.Parameters.Add(new SqlParameter("@TranseID", objRecord.TranseID));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", objRecord.BranchID)); 
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 4));
                   object Obj= objCmd.ExecuteNonQuery();
                   return Comon.cInt(Obj);
                }
            }
           
            return 0;
        }
        public static DataTable GetCountElementID(int FacilityID, int BranchID, int TranseID, int DocumentType)
        {
            try
            {
                using (SqlConnection objCnn = new GlobalConnection().Conn)
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[Stc_ItemsMoviing_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@DocumentTypeID", DocumentType));
                        objCmd.Parameters.Add(new SqlParameter("@TranseID", TranseID));
                        objCmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                        objCmd.Parameters.Add(new SqlParameter("@FacilityID", FacilityID));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 5));
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
    }
}

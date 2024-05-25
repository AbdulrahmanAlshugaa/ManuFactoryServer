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

namespace Edex.DAL.UsersManagement
{
  public  class UserScreenFovoriteDAL
    {
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
        public static Int32 frmInsertUserScreenFovorite(int SelectedUserID, int SelectedBranchID, List<UserFormsFovorite> listUserScreenFovorite)
        {

            Int32 objRet = 0;
            string DitmeXML = ConvertObjectToXMLString(listUserScreenFovorite);
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[UserScreenFovorite_SP]";
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

    }
}

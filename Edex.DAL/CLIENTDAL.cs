using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Edex.Model;

namespace Edex.DAL
{
    public class CLIENTDAL
    {
        public static CLIENTBO ConvertRowToObj(DataRow dr)
        {
            CLIENTBO Obj = new CLIENTBO();
            Obj.CLNT_ID = long.Parse(dr["CLNT_ID"].ToString());
            Obj.C_F_NAME = dr["C_F_NAME"].ToString();
            Obj.C_L_NAME = dr["C_L_NAME"].ToString();
            Obj.C_AC_ID = dr["C_AC_ID"].ToString();
            return Obj;
        }
        public static CST_CNTRBO ConvertRowToObjCST_CNTR(DataRow dr)
        {
            CST_CNTRBO Obj = new CST_CNTRBO();
            Obj.CC_NO = long.Parse(dr["CC_NO"].ToString());
            Obj.CST_CNT_ID = dr["CST_CNT_ID"].ToString();
            Obj.COSTCENTERNAME = dr["COSTCENTERNAME"].ToString();

            return Obj;
        }
        public static AR_REPRS_DTLBO ConvertRowToObjAR_REPRS_DTL(DataRow dr)
        {
            AR_REPRS_DTLBO Obj = new AR_REPRS_DTLBO();
            Obj.REPRS_CODE = long.Parse(dr["REPRS_CODE"].ToString());
            Obj.R_AC_ID = dr["R_AC_ID"].ToString();
            Obj.REPRS_F_NAME = dr["REPRS_F_NAME"].ToString();
            return Obj;
        }
        public static JS_WH_DTLBO ConvertRowToObjJS_WH_DTL(DataRow dr)
        {
            JS_WH_DTLBO Obj = new JS_WH_DTLBO();
            Obj.WH_CODE = long.Parse(dr["WH_CODE"].ToString());
            Obj.W_F_NAME = dr["W_F_NAME"].ToString();
            Obj.TR_AC_ID = long.Parse(dr["TR_AC_ID"].ToString());
            return Obj;
        }
        public static ITEM_DTLBO ConvertRowToObjITEM_DTL(DataRow dr)
        {
            ITEM_DTLBO Obj = new ITEM_DTLBO();
            Obj.ITEM_ID = dr["ITEM_ID"].ToString();
            Obj.I_F_NAME = dr["I_F_NAME"].ToString();
            return Obj;
        }
        public static GRP_ITEM_DTLBO ConvertRowToObjGRP_ITEM_DTLBO(DataRow dr)
        {
            GRP_ITEM_DTLBO Obj = new GRP_ITEM_DTLBO();
            Obj.G_CODE = dr["G_CODE"].ToString();
            Obj.G_F_NAME = dr["G_F_NAME"].ToString();
            return Obj;
        }
        
      
    }
}

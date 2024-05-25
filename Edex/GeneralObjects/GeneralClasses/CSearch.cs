using DevExpress.XtraEditors;
using Edex.Model;
using Edex.Model.Language;
using Edex.ModelSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Edex.GeneralObjects.GeneralClasses
{
     

    public class CSearch
    {
        private Hashtable _hastable;
        private DataTable _datatable;
        private string _tablename;
        private CField _cfield;
        private string _primaryKeyField;
        private string _primarykeyheader;
        private string _strFilter;
        private int _primarykeyWidth;
        private string _PrimaryKeyName;
        public int SearchCol = 1;
        public string SQLStr;
        private string _strArbNameValue;
        public string TableName
        {
            get
            {
                return _tablename;
              
            }
            set
            {
                _tablename = value;
            }
        }

        public string strFilter
        {
            get
            {
                return _strFilter;
            }
            set
            {
                _strFilter = value;
            }
        }


        public string strArbNameValue
        {
            get
            {
                return _strArbNameValue;
            }
            set
            {
                _strArbNameValue = value;
            }
        }

        public CField cField
        {
            get
            {
                return _cfield;
            }
        }

        public DataTable returnTable
        {
            get
            {
                return _datatable;
            }
        }

        public string PrimaryKey
        {
            get
            {
                return _primaryKeyField;
            }
        }

        private string _primarykeyvalue;
        public string PrimaryKeyValue
        {
            get
            {
                return _primarykeyvalue;
            }
            set
            {
                _primarykeyvalue = value;
            }
        }
        public string PrimaryKeyName
        {
            get
            {
                return _PrimaryKeyName;
            }
            set
            {
                _PrimaryKeyName = value;
            }
        }
        public string PrimaryKeyField
        {
            get
            {
                return _primaryKeyField;
            }
            set
            {
                _primaryKeyField = value;
            }
        }

        public string PrimaryKeyHeader
        {
            get
            {
                return _primarykeyheader;
            }
        }

        public int PrimaryKeyWidth
        {
            get
            {
                return _primarykeyWidth;
            }
        }



        public void AddField(string pFieldName, string pFieldHeader) // , ByVal IsPrimaryKey As Boolean, ByVal pFieldWidth As Integer
        {
        }

        public void LoadData()
        {

            DataTable dt = new DataTable();
            dt = Lip.SelectRecord(SQLStr);
            _datatable = dt; // sb.ToString() db.GetDataSet(sb.ToString()).Tables(0)
        }

        public int GetFieldWidth(string FieldHeader)
        {
            foreach (DictionaryEntry entry in _hastable)
            {
                // If DirectCast(entry.Value, CField).FieldName = FieldName Then
                // Return DirectCast(entry.Value, CField).FieldWidth

            }

            return 0;
        }

        public static void ControlValidating(TextBox ctrl, Control ReturnCtrl, String strSQL)
        {
            if (ctrl.Text != String.Empty)
            {
                //ConvertStrSQLToEnglishOrArabicLanguage(strSQL)
                DataTable dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count > 0)
                {
                    ReturnCtrl.Text = dt.Rows[0][0].ToString();
                }
                else
                {
                    ReturnCtrl.Text = "";
                    ctrl.Text = "";
                }
            }
            else
            {
                ctrl.Text = "";
                ReturnCtrl.Text = "";
            }
        }

        public static void ControlValidating(TextEdit ctrl, Control ReturnCtrl, String strSQL)
        {
            if (ctrl.Text != String.Empty && Comon.cLong(ctrl.Text) != 0)
            {
                //ConvertStrSQLToEnglishOrArabicLanguage(strSQL)
                DataTable dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count > 0)
                {
                    //if (Lip.CheckTheAccountIsStope(Comon.cDbl(ctrl.Text), Comon.cInt(MySession.GlobalBranchID)))
                    //{
                    //    Messages.MsgWarning(Messages.TitleWorning, Messages.msgAccountIsStope);
                    //    ReturnCtrl.Text = "";
                    //    ctrl.Text = "";
                    //    return;
                    //}
                    ReturnCtrl.Text = dt.Rows[0][0].ToString();
                }
                else
                {
                    ReturnCtrl.Text = "";
                    ctrl.Text = "";
                }
            }
            else
            {
                ctrl.Text = "";
                ReturnCtrl.Text = "";
            }
        }
        public static void ControlValidating(TextEdit ctrl, Control ReturnCtrl, String TableName, long PremaryKey)
        {
            if (ctrl.Text != String.Empty && Comon.cLong(ctrl.Text) != 0)
            {
                string strSQL = "SELECT   " + (UserInfo.Language == iLanguage.Arabic ? "ArbName" : "EngName") + "  as Name FROM " + TableName + " WHERE ID =" + PremaryKey + " And DELETED =0 ";
                DataTable dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count > 0)
                {
                    ReturnCtrl.Text = dt.Rows[0][0].ToString();
                }
                else
                {
                    ReturnCtrl.Text = "";
                    ctrl.Text = "";
                }
            }
            else
            {
                ctrl.Text = "";
                ReturnCtrl.Text = "";
            }
        }
        public static void ControlValidatingAcc(TextEdit ctrl, Control ReturnCtrl, String TableName, long PremaryKey)
        {
            if (ctrl.Text != String.Empty && Comon.cLong(ctrl.Text) != 0)
            {
                string strSQL = "SELECT   " + (UserInfo.Language == iLanguage.Arabic ? "ArbName" : "EngName") + "  as Name FROM " + TableName + " WHERE ACCOUNTID =" + PremaryKey + " And DELETED =0 ";
                DataTable dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count > 0)
                {
                    ReturnCtrl.Text = dt.Rows[0][0].ToString();
                }
                else
                {
                    ReturnCtrl.Text = "";
                    ctrl.Text = "";
                }
            }
            else
            {
                ctrl.Text = "";
                ReturnCtrl.Text = "";
            }
        }


    }
    public class CField
    {
        private string _fieldname;
        private string _fieldCaption;
        private int _FieldWidth;
        private bool _isprimarykey;
        public CField(string pFieldName, string pFieldCaption, int pFieldWidth)
        {
            _fieldname = pFieldName;
            _fieldCaption = pFieldCaption;
            _FieldWidth = pFieldWidth;
        }

        public CField(string pFieldName, string pFieldCaption, bool pIsprimarykey, int pFieldWidth)
        {
            _fieldname = pFieldName;
            _fieldCaption = pFieldCaption;
            _isprimarykey = pIsprimarykey;
            _FieldWidth = pFieldWidth;
        }

        public string FieldName
        {
            get
            {
                return _fieldname;
            }
        }

        public string FieldCaption
        {
            get
            {
                return _fieldCaption;
            }
        }

        public bool IsPrimaryKey
        {
            get
            {
                return _isprimarykey;
            }
        }

        public int FieldWidth
        {
            get
            {
                return _FieldWidth;
            }
        }
    }
}

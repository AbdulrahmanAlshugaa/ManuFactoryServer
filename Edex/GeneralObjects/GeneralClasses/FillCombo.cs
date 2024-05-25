using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using Edex.GeneralObjects.GeneralForms;
using Edex.Model;
using Edex.Model.Language;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Edex.GeneralObjects.GeneralClasses
{
    public static class FillCombo
    {

        public static void FillComboBoxRepositoryItemLookUpEdit(RepositoryItemLookUpEdit cmb, string Tablename, string Code, string Name, string OrderByField = "", string Where = "", string ShowNullText = "")
        {
            string strSQL = "";
            strSQL = "SELECT " + Code + " AS الرقم," + Name + "  AS الاسم FROM " + Tablename;

            if (UserInfo.Language == iLanguage.English)
                strSQL = "SELECT " + Code + " AS NO ," + Name + " AS NAME FROM " + Tablename;

            if (OrderByField != "")
                strSQL = strSQL + " Order By " + OrderByField;

            if (Where != "")
                strSQL = strSQL + " Where " + Where;
            else
                strSQL = strSQL + " Where  BranchID=" + UserInfo.BRANCHID;

            if (UserInfo.Language == iLanguage.English)
            {
                cmb.DataSource = Lip.SelectRecord(strSQL).DefaultView;
                cmb.DisplayMember = "NAME";
                cmb.ValueMember = "NO";
                cmb.NullText = ShowNullText;
                cmb.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Optimistic;
            }
            else
            {
                cmb.DataSource = Lip.SelectRecord(strSQL).DefaultView;
                cmb.ValueMember = "الرقم";
                cmb.DisplayMember = "الاسم";
                cmb.NullText = ShowNullText;
                cmb.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Optimistic;

            }


        }

        public static void FillComboBoxLookUpEdit(DevExpress.XtraEditors.LookUpEdit cmb, string Tablename, string Code, string Name, string OrderByField = "", string Where = "", string ShowNullText = "",string NameCol="0")
        { 
            string strSQL = "";
            strSQL = "SELECT  " + Code + " AS الرقم," + Name + "  AS الاسم FROM " + Tablename;

            if (UserInfo.Language == iLanguage.English)
                strSQL = "SELECT " + Code + "," + Name + " FROM " + Tablename;


            if (OrderByField != "")
                strSQL = strSQL + " Order By " + OrderByField;

            if (Where != "")
                strSQL = strSQL + " Where " + Where;
            else
                strSQL = strSQL + " Where  BranchID=" + UserInfo.BRANCHID;


            DataTable dt = Lip.SelectRecord(strSQL);
            DataRow toInsert = dt.NewRow();
            toInsert[0] = "0";
            toInsert[1] = NameCol;
            dt.Rows.InsertAt(toInsert, 0);

            if (UserInfo.Language == iLanguage.English)
            {
                cmb.Properties.DataSource = dt.DefaultView;
                cmb.Properties.DisplayMember = Name;
                cmb.Properties.ValueMember = Code;
                cmb.Properties.NullText = ShowNullText;

            }
            else
            {
                cmb.Properties.DataSource = dt.DefaultView;
                cmb.Properties.ValueMember = "الرقم";
                cmb.Properties.DisplayMember = "الاسم";
                cmb.Properties.NullText = ShowNullText;
            }
        }
        public static void FillComboBox(DevExpress.XtraEditors.LookUpEdit cmb, string Tablename, string Code, string Name, string OrderByField = "", string Where = "", string ShowNullText = "")
        {

            string strSQL = "SELECT " + Code + " AS  الرقم ," + Name + "  AS  الاسم  FROM " + Tablename;

            if (UserInfo.Language == iLanguage.English)
                strSQL = "SELECT " + Code + " AS No ," + Name + " AS Name FROM " + Tablename;


            if (Where != "")
                strSQL = strSQL + " Where " + Where;

            if (OrderByField != "")
                strSQL = strSQL + " Order By " + OrderByField;


            if (UserInfo.Language == iLanguage.English)
            {
                var dt = Lip.SelectRecord(strSQL);
                DataRow row;


                if (dt.Rows.Count > 0)
                {

                    row = dt.NewRow();
                    row["NO"] = 0;
                    row["Name"] = "---";

                }
                cmb.Properties.DataSource = dt.DefaultView;
                cmb.Properties.DisplayMember = "Name";
                cmb.Properties.ValueMember = "No";
                cmb.Properties.NullText = ShowNullText;
                cmb.Properties.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Optimistic;
            }
            else
            {
                var dt = Lip.SelectRecord(strSQL);
                DataRow row;


                if (dt.Rows.Count > 0)
                {

                    row = dt.NewRow();
                    row["الرقم"] = 0;
                    row["الاسم"] = "---";
                    dt.Rows.Add(row);
                }
                cmb.Properties.DataSource = dt.DefaultView;
                cmb.Properties.ValueMember = "الرقم";
                cmb.Properties.DisplayMember = "الاسم";
                cmb.Properties.NullText = ShowNullText;
                cmb.Properties.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Optimistic;
            }


        }
        public static void FillComboBoxSys(DevExpress.XtraEditors.LookUpEdit cmb, string Tablename, string Code, string Name, string OrderByField = "", string Where = "", string ShowNullText = "")
        {

            string strSQL = "SELECT " + Code + " AS الرقم," + Name + "  AS الاسم FROM " + Tablename;

            if (UserInfo.Language == iLanguage.English)
                strSQL = "SELECT " + Code + " AS NO ," + Name + " AS NAME FROM " + Tablename;


            if (Where != "")
                strSQL = strSQL + " Where " + Where;
            else
                strSQL = strSQL + " Where  BranchID=" + UserInfo.BRANCHID;

            if (OrderByField != "")
                strSQL = strSQL + " Order By " + OrderByField;


            if (UserInfo.Language == iLanguage.English)
            {
                var dt = Lip.SelectRecord(strSQL);
                DataRow row;
                if (dt.Rows.Count > 0)
                {

                    row = dt.NewRow();
                    row["NO"] = 0;
                    row["NAME"] = "---";

                }

                cmb.Properties.DataSource = dt.DefaultView;
                cmb.Properties.DisplayMember = "NAME";
                cmb.Properties.ValueMember = "NO";
                cmb.Properties.NullText = ShowNullText;
                cmb.Properties.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Optimistic;
            }
            else
            {

                var dt = Lip.SelectRecord(strSQL);
                DataRow row;
                if (dt.Rows.Count > 0)
                {

                    row = dt.NewRow();
                    row["الرقم"] = 0;
                    row["الاسم"] = "---";
                    dt.Rows.Add(row);
                }

                cmb.Properties.DataSource = dt.DefaultView;
                cmb.Properties.ValueMember = "الرقم";
                cmb.Properties.DisplayMember = "الاسم";
                cmb.Properties.NullText = ShowNullText;
                cmb.Properties.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Optimistic;

            }

        }
        public static void FillComboBoxNumaric(System.Windows.Forms.ComboBox cmb, string Tablename, string Code, string Name, string OrderByField = "", string Where = "", string ShowNullText = "")
        {

            string strSQL = "SELECT " + Code + " AS الرقم," + Name + "  AS الاسم FROM " + Tablename;

            if (UserInfo.Language == iLanguage.English)
                strSQL = "SELECT " + Code + " AS NO ," + Name + " AS NAME FROM " + Tablename;



            if (Where != "")
                strSQL = strSQL + " Where " + Where;


            if (OrderByField != "")
                strSQL = strSQL + " Order By " + OrderByField;

            if (UserInfo.Language == iLanguage.English)
            {
                var dt = Lip.SelectRecord(strSQL);
                cmb.DataSource = dt.DefaultView;
                cmb.DisplayMember = "NAME";
                cmb.ValueMember = "NO";
            }
            else
            {
                var dt = Lip.SelectRecord(strSQL);


                cmb.DataSource = dt.DefaultView;
                cmb.ValueMember = "الرقم";
                cmb.DisplayMember = "الاسم";
            }
        }
        public static void FillComboBox(System.Windows.Forms.ComboBox cmb, string Tablename, string Code, string Name, string OrderByField = "", string Where = "", string ShowNullText = "")
        {

            string strSQL = "SELECT " + Code + " AS الرقم," + Name + "  AS الاسم FROM " + Tablename;

            if (UserInfo.Language == iLanguage.English)
                strSQL = "SELECT " + Code + " AS NO ," + Name + " AS NAME FROM " + Tablename;



            if (Where != "")
                strSQL = strSQL + " Where " + Where;


            if (OrderByField != "")
                strSQL = strSQL + " Order By " + OrderByField;

            if (UserInfo.Language == iLanguage.English)
            {
                var dt = Lip.SelectRecord(strSQL);
                DataRow row;
                if (dt.Rows.Count > 0)
                {

                    row = dt.NewRow();
                    row["NO"] = 0;
                    row["NAME"] = "---";

                }
                cmb.DataSource = dt.DefaultView;
                cmb.DisplayMember = "NAME";
                cmb.ValueMember = "NO";
            }
            else
            {
                var dt = Lip.SelectRecord(strSQL);
                DataRow row;
                if (dt.Rows.Count > 0)
                {

                    row = dt.NewRow();
                    row["الرقم"] = 0;
                    row["الاسم"] = "---";
                    dt.Rows.Add(row);
                }
                cmb.DataSource = dt.DefaultView;
                cmb.ValueMember = "الرقم";
                cmb.DisplayMember = "الاسم";
            }
        }
        public static void FillComboBox(System.Windows.Forms.CheckedListBox cmb, string Tablename, string Code, string Name, string OrderByField = "", string Where = "", string ShowNullText = "")
        {

            string strSQL = "SELECT " + Code + " AS الرقم," + Name + "  AS الاسم FROM " + Tablename;

            if (UserInfo.Language == iLanguage.English)
                strSQL = "SELECT " + Code + " AS NO ," + Name + " AS NAME FROM " + Tablename;



            if (Where != "")
                strSQL = strSQL + " Where " + Where;


            if (OrderByField != "")
                strSQL = strSQL + " Order By " + OrderByField;

            if (UserInfo.Language == iLanguage.English)
            {
                var dt = Lip.SelectRecord(strSQL);
                DataRow row;
                if (dt.Rows.Count > 0)
                {

                    row = dt.NewRow();
                    row["NO"] = 0;
                    row["NAME"] = "---";

                }
                cmb.DataSource = dt.DefaultView;
                cmb.DisplayMember = "NAME";
                cmb.ValueMember = "NO";
            }
            else
            {
                var dt = Lip.SelectRecord(strSQL);
                DataRow row;
                if (dt.Rows.Count > 0)
                {

                    row = dt.NewRow();
                    row["الرقم"] = 0;
                    row["الاسم"] = "---";
                    dt.Rows.Add(row);
                }
                cmb.DataSource = dt.DefaultView;
                cmb.ValueMember = "الرقم";
                cmb.DisplayMember = "الاسم";
            }
        }
        public static void FillComboBoxWithOutSelectedField(System.Windows.Forms.CheckedListBox cmb, string Tablename, string Code, string Name, string OrderByField = "", string Where = "", string ShowNullText = "")
        {

            string strSQL = "SELECT " + Code + " AS الرقم," + Name + "  AS الاسم FROM " + Tablename;

            if (UserInfo.Language == iLanguage.English)
                strSQL = "SELECT " + Code + " AS NO ," + Name + " AS NAME FROM " + Tablename;



            if (Where != "")
                strSQL = strSQL + " Where " + Where;


            if (OrderByField != "")
                strSQL = strSQL + " Order By " + OrderByField;

            if (UserInfo.Language == iLanguage.English)
            {
                var dt = Lip.SelectRecord(strSQL);
                DataRow row;

                cmb.DataSource = dt.DefaultView;
                cmb.DisplayMember = "NAME";
                cmb.ValueMember = "NO";
            }
            else
            {
                var dt = Lip.SelectRecord(strSQL);
                DataRow row;
                cmb.DataSource = dt.DefaultView;
                cmb.ValueMember = "الرقم";
                cmb.DisplayMember = "الاسم";
            }
        }
        public static void FillComboBoxSys(System.Windows.Forms.ComboBox cmb, string Tablename, string Code, string Name, string OrderByField = "", string Where = "", string ShowNullText = "")
        {

            string strSQL = "SELECT " + Code + " AS الرقم," + Name + "  AS الاسم FROM " + Tablename;

            if (UserInfo.Language == iLanguage.English)
                strSQL = "SELECT " + Code + " AS NO ," + Name + " AS NAME FROM " + Tablename;

            if (Where != "")
                strSQL = strSQL + " Where " + Where;
            else
                strSQL = strSQL + " Where  BranchID=" + UserInfo.BRANCHID;


            if (OrderByField != "")
                strSQL = strSQL + " Order By " + OrderByField;

            if (UserInfo.Language == iLanguage.English)
            {
                var dt = Lip.SelectRecord(strSQL);
                DataRow row;
                if (dt.Rows.Count > 0)
                {

                    row = dt.NewRow();
                    row["NO"] = 0;
                    row["NAME"] = "---";

                }
                cmb.DataSource = dt.DefaultView;
                cmb.DisplayMember = "NAME";
                cmb.ValueMember = "NO";
            }
            else
            {
                var dt = Lip.SelectRecord(strSQL);
                DataRow row;
                if (dt.Rows.Count > 0)
                {

                    row = dt.NewRow();
                    row["الرقم"] = 0;
                    row["الاسم"] = "---";
                    dt.Rows.Add(row);
                }
                cmb.DataSource = dt.DefaultView;
                cmb.ValueMember = "الرقم";
                cmb.DisplayMember = "الاسم";
            }
        }
        public static void FillComboBox1(DevExpress.XtraEditors.LookUpEdit cmb, string Tablename, string Code, string Name)
        {
            string strSQL = "SELECT " + Code + " AS  الرقم ," + Name + "  AS  الاسم  FROM " + Tablename;

            if (UserInfo.Language == iLanguage.English)
                strSQL = "SELECT " + Code + " AS NO ," + Name + " AS NAME FROM " + Tablename;


            if (UserInfo.Language == iLanguage.English)
            {
                cmb.Properties.DataSource = Lip.SelectRecord(strSQL).DefaultView;
                cmb.Properties.DisplayMember = "NAME";
                cmb.Properties.ValueMember = "NO";
                cmb.Properties.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Optimistic;
            }
            else
            {
                cmb.Properties.DataSource = Lip.SelectRecord(strSQL).DefaultView;
                cmb.Properties.ValueMember = "الرقم";
                cmb.Properties.DisplayMember = "الاسم";
                cmb.Properties.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Optimistic;

            }

        }
        public static void FillComboBox(DevExpress.XtraTab.XtraTabPage Tap)
        {
            try
            {
                string PrimaryKeyName = (UserInfo.Language == iLanguage.English ? "ENGNAME " : "ARBNAME");
                foreach (Control item in Tap.Controls)
                {
                    if (item is DevExpress.XtraEditors.LookUpEdit)
                    {
                        DevExpress.XtraEditors.LookUpEdit cmb = (DevExpress.XtraEditors.LookUpEdit)item;
                        if (cmb.Tag != null)
                            FillCombo.FillComboBox(cmb, cmb.Tag.ToString(), "ID", PrimaryKeyName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
                    }

                }
            }
            catch (Exception ex)
            {
            }
        }
        public static void FillComboBox(Form frm)
        {
            string PrimaryKeyName = (UserInfo.Language == iLanguage.English ? "ENGNAME " : "ARBNAME");
            foreach (Control item in frm.Controls)
            {
                if (item is DevExpress.XtraEditors.LookUpEdit)
                {

                    DevExpress.XtraEditors.LookUpEdit cmb = (DevExpress.XtraEditors.LookUpEdit)item;
                    if (cmb.Tag != null)
                        if (Comon.cInt(cmb.Tag) == 0 && cmb.Tag.ToString() != "ImportantField")
                            FillCombo.FillComboBox(cmb, cmb.Tag.ToString(), "ID", PrimaryKeyName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
                }
            }
        }
        public static void FillComboBoxSys(DevExpress.XtraTab.XtraTabPage Tap)
        {
            try
            {
                string PrimaryKeyName = (UserInfo.Language == iLanguage.English ? "ENGNAME " : "ARBNAME");
                foreach (Control item in Tap.Controls)
                {
                    if (item is DevExpress.XtraEditors.LookUpEdit)
                    {
                        DevExpress.XtraEditors.LookUpEdit cmb = (DevExpress.XtraEditors.LookUpEdit)item;
                        if (cmb.Tag != null)
                            FillCombo.FillComboBoxSys(cmb, cmb.Tag.ToString(), "ID", PrimaryKeyName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
                    }

                }
            }
            catch (Exception ex)
            {
            }
        }
        public static void FillComboBoxSysSeting(DevExpress.XtraTab.XtraTabPage Tab)
        {
            try
            {
                string PrimaryKeyName = (UserInfo.Language == iLanguage.English ? "ENGNAME " : "ARBNAME");
                foreach (Control item in Tab.Controls)
                {
                    if (item is DevExpress.XtraEditors.LookUpEdit)
                    {
                        DevExpress.XtraEditors.LookUpEdit cmb = (DevExpress.XtraEditors.LookUpEdit)item;
                        if (cmb.Tag != null)
                            FillCombo.FillComboBox(cmb, "SETTINGSSYSTEMLIST", "ID", PrimaryKeyName, "", " LISTID = " + cmb.Tag.ToString(), (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
                    }

                    if (item is System.Windows.Forms.Panel)
                    {
                        if (item is System.Windows.Forms.Panel)
                        {
                            System.Windows.Forms.Panel pnl = (System.Windows.Forms.Panel)item;
                            FillCombo.FillComboBoxSysSeting(pnl);
                        }

                    }


                }
            }
            catch (Exception ex)
            {
            }
        }
        public static void FillComboBoxSysSeting(System.Windows.Forms.TabPage Tab)
        {
            try
            {
                string PrimaryKeyName = (UserInfo.Language == iLanguage.English ? "ENGNAME " : "ARBNAME");
                foreach (Control item in Tab.Controls)
                {
                    if (item is DevExpress.XtraEditors.LookUpEdit)
                    {
                        DevExpress.XtraEditors.LookUpEdit cmb = (DevExpress.XtraEditors.LookUpEdit)item;
                        if (cmb.Tag != null)
                            FillCombo.FillComboBoxSys(cmb, "STC_STORESSETTINGS", "ID", PrimaryKeyName, "", " TYPEID = " + cmb.Tag.ToString(), (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
                    }

                    if (item is System.Windows.Forms.Panel)
                    {
                        if (item is System.Windows.Forms.Panel)
                        {
                            System.Windows.Forms.Panel pnl = (System.Windows.Forms.Panel)item;
                            FillCombo.FillComboBoxSysSeting(pnl);
                        }

                    }


                }
            }
            catch (Exception ex)
            {
            }
        }

        public static void FillComboBoxSysSeting(System.Windows.Forms.Panel Pnl)
        {
            try
            {
                string PrimaryKeyName = (UserInfo.Language == iLanguage.English ? "ENGNAME " : "ARBNAME");
                foreach (Control item in Pnl.Controls)
                {
                    if (item is DevExpress.XtraEditors.LookUpEdit)
                    {
                        DevExpress.XtraEditors.LookUpEdit cmb = (DevExpress.XtraEditors.LookUpEdit)item;
                        if (cmb.Tag != null)
                            FillCombo.FillComboBoxSys(cmb, "SETTINGSSYSTEMLIST", "ID", PrimaryKeyName, "ID", " LISTID = " + cmb.Tag.ToString(), (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
                    }

                    if (item is System.Windows.Forms.ComboBox)
                    {
                        System.Windows.Forms.ComboBox cmb = (System.Windows.Forms.ComboBox)item;
                        if (cmb.Tag != null)
                            FillCombo.FillComboBoxSys(cmb, "SETTINGSSYSTEMLIST", "ID", PrimaryKeyName, "ID", " LISTID = " + cmb.Tag.ToString(), (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
                    }

                }
            }
            catch (Exception ex)
            {
            }
        }

        public static void FillComboBoxSysSetingReports(System.Windows.Forms.Panel Pnl)
        {
            try
            {
                string PrimaryKeyName = (UserInfo.Language == iLanguage.English ? "ENGNAME " : "ARBNAME");
                foreach (Control item in Pnl.Controls)
                {
                    if (item is DevExpress.XtraEditors.LookUpEdit)
                    {
                        DevExpress.XtraEditors.LookUpEdit cmb = (DevExpress.XtraEditors.LookUpEdit)item;
                        if (cmb.Tag != null)
                            FillCombo.FillComboBoxSys(cmb, "REPORTSSETTINGSSYSTEMLIST", "ID", PrimaryKeyName, "ID", " LISTID = " + cmb.Tag.ToString(), (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
                    }

                    if (item is System.Windows.Forms.ComboBox)
                    {
                        System.Windows.Forms.ComboBox cmb = (System.Windows.Forms.ComboBox)item;
                        if (cmb.Tag != null)
                            FillCombo.FillComboBoxSys(cmb, "REPORTSSETTINGSSYSTEMLIST", "ID", PrimaryKeyName, "ID", " LISTID = " + cmb.Tag.ToString(), (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
                    }

                }
            }
            catch (Exception ex)
            {
            }
        }
        public static void FillComboBoxSysSeting(Form frm)
        {
            try
            {
                foreach (Control item in frm.Controls)
                {
                    if (item is System.Windows.Forms.Panel)
                    {
                        System.Windows.Forms.Panel pnl = (System.Windows.Forms.Panel)item;
                        FillCombo.FillComboBoxSysSeting(pnl);
                    }

                    if (item is DevExpress.XtraEditors.LookUpEdit)
                    {
                        string PrimaryKeyName = (UserInfo.Language == iLanguage.English ? "ENGNAME " : "ARBNAME");
                        DevExpress.XtraEditors.LookUpEdit cmb = (DevExpress.XtraEditors.LookUpEdit)item;
                        if (cmb.Tag != null)
                            FillCombo.FillComboBoxSys(cmb, "SETTINGSSYSTEMLIST", "ID", PrimaryKeyName, "ID", " LISTID = " + cmb.Tag.ToString(), (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));

                    }

                }
            }
            catch (Exception ex)
            {
            }
        }


       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Reports Setting List"></param>
        /// <param name="Dl"></param>


        public static void FillComboBoxSysSetingReports(DevExpress.XtraTab.XtraTabPage Tab)
        {
            try
            {
                string PrimaryKeyName = (UserInfo.Language == iLanguage.English ? "ENGNAME " : "ARBNAME");
                foreach (Control item in Tab.Controls)
                {
                    if (item is DevExpress.XtraEditors.LookUpEdit)
                    {
                        DevExpress.XtraEditors.LookUpEdit cmb = (DevExpress.XtraEditors.LookUpEdit)item;
                        if (cmb.Tag != null)
                            FillCombo.FillComboBox(cmb, "REPORTSSETTINGSSYSTEMLIST", "ID", PrimaryKeyName, "", " LISTID = " + cmb.Tag.ToString(), (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
                    }

                    if (item is System.Windows.Forms.Panel)
                    {
                        if (item is System.Windows.Forms.Panel)
                        {
                            System.Windows.Forms.Panel pnl = (System.Windows.Forms.Panel)item;
                            FillCombo.FillComboBoxSysSetingReports(pnl);
                        }

                    }


                }
            }
            catch (Exception ex)
            {
            }
        }

        public static void FillComboBoxSysSetingReports(System.Windows.Forms.TabPage Tab)
        {
            try
            {
                string PrimaryKeyName = (UserInfo.Language == iLanguage.English ? "ENGNAME " : "ARBNAME");
                foreach (Control item in Tab.Controls)
                {
                    if (item is DevExpress.XtraEditors.LookUpEdit)
                    {
                        DevExpress.XtraEditors.LookUpEdit cmb = (DevExpress.XtraEditors.LookUpEdit)item;
                        if (cmb.Tag != null)
                            FillCombo.FillComboBoxSys(cmb, "STC_STORESSETTINGS", "ID", PrimaryKeyName, "", " TYPEID = " + cmb.Tag.ToString(), (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
                    }

                    if (item is System.Windows.Forms.Panel)
                    {
                        if (item is System.Windows.Forms.Panel)
                        {
                            System.Windows.Forms.Panel pnl = (System.Windows.Forms.Panel)item;
                            FillCombo.FillComboBoxSysSeting(pnl);
                        }

                    }


                }
            }
            catch (Exception ex)
            {
            }
        }




       
        public static void FillComboBoxSys(System.Windows.Forms.Panel Pnl, string Where)
        {
            try
            {
                string PrimaryKeyName = (UserInfo.Language == iLanguage.English ? "ENGNAME " : "ARBNAME");
                foreach (Control item in Pnl.Controls)
                {
                    if (item is DevExpress.XtraEditors.LookUpEdit)
                    {
                        DevExpress.XtraEditors.LookUpEdit cmb = (DevExpress.XtraEditors.LookUpEdit)item;
                        FillCombo.FillComboBoxSys(cmb, cmb.Tag.ToString(), "ID", PrimaryKeyName, "", Where, (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
                    }

                }
            }
            catch (Exception ex)
            {
            }
        }

        public static void FillComboBoxSys(Form frm)
        {
            string PrimaryKeyName = (UserInfo.Language == iLanguage.English ? "ENGNAME " : "ARBNAME");
            foreach (Control item in frm.Controls)
            {
                if (item is DevExpress.XtraEditors.LookUpEdit)
                {
                    DevExpress.XtraEditors.LookUpEdit cmb = (DevExpress.XtraEditors.LookUpEdit)item;
                    if (cmb.Tag != null)
                        FillCombo.FillComboBoxSys(cmb, cmb.Tag.ToString(), "ID", PrimaryKeyName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
                }
            }
        }

        public static void EnabledControl(bool Value, Control ControlItem)
        {
            foreach (Control item in ControlItem.Controls)
            {
                foreach (Control itemForm in item.Controls)
                {
                    if (itemForm is System.Windows.Forms.TabPage)
                    {
                        foreach (Control itemControl in itemForm.Controls)
                        {
                            if (itemControl is System.Windows.Forms.Panel)
                            {
                                System.Windows.Forms.Panel pnl = (System.Windows.Forms.Panel)itemControl;
                                foreach (Control itemControl1 in pnl.Controls)
                                {
                                    if (itemControl1 is TextEdit || itemControl1 is LookUpEdit || itemControl1 is CheckEdit)
                                        itemControl.Enabled = Value;
                                }
                            }
                        }

                    }

                }
            }

        }


    }
    public static class FillGrid
        {
            static string strSQL = "";
            public static void FillGridView(DevExpress.XtraGrid.Views.Grid.GridView Grd, string TableName, string PremaryKey)
            {
                try
                {
                    strSQL = "SELECT  " + PremaryKey + " as الرقم, ArbName as  الاسم   FROM " + TableName + " WHERE Cancel =0 and BranchID= " + MySession.GlobalBranchID+" Order by " + PremaryKey;
                    if (UserInfo.Language == iLanguage.English)
                        strSQL = "SELECT  " + PremaryKey + " as ID, EngName as   Name  FROM " + TableName + " WHERE Cancel =0 and BranchID= " + MySession.GlobalBranchID+"  Order by  " + PremaryKey;
                    DataTable dt = new DataTable();
                    dt = Lip.SelectRecord(strSQL);
                    Grd.GridControl.DataSource = dt;
                    Grd.Columns[0].Width = 60;
                    Grd.Columns[1].Width = 120;
                }
                catch (Exception ex)
                {
                }
            }

            public static void FillGridView(DevExpress.XtraGrid.Views.Grid.GridView Grd, string TableName, string PremaryKey, string Where)
            {
                try
                {
                    strSQL = "SELECT  " + PremaryKey + " as الرقم, ArbName as  الاسم   FROM " + TableName + " WHERE Cancel =0 And " + Where + "  Order by " + PremaryKey;
                    if (UserInfo.Language == iLanguage.English)
                        strSQL = "SELECT  " + PremaryKey + " as ID, EngName as   Name  FROM " + TableName + " WHERE Cancel =0   And " + Where + " Order by  " + PremaryKey;
                    DataTable dt = new DataTable();
                    dt = Lip.SelectRecord(strSQL);
                    Grd.GridControl.DataSource = dt;
                    Grd.Columns[0].Width = 50;
                    Grd.Columns[1].Width = 100;
                }
                catch (Exception ex)
                {

                }

            }


            public static void FillGridView(DevExpress.XtraGrid.Views.Grid.GridView Grd, string strSQL)
            {
                try
                {

                    DataTable dt = new DataTable();
                    dt = Lip.SelectRecord(strSQL);
                    Grd.GridControl.DataSource = dt;
                    Grd.Columns[0].Width = 50;
                    Grd.Columns[1].Width = 100;
                }
                catch (Exception ex)
                {

                }
            }
        }
        public static class Lovs
        {

            public static CSearch Find(ref CSearch cls, Control IDCtrl, Control NameCtrl, string PrimaryKeyName, string strFilter, int BranchID, string SearchType = "")
            {
                try
                {
                    int[] ColumnWidth = new int[] { 100, 450 };
                    PrepareSearchQuery.PrepareSearchScreen(SearchType, ref cls, ref ColumnWidth, UserInfo.Language, BranchID);
                    if (cls.SQLStr != "")
                    {
                        frmSearch frm = new frmSearch();
                        frm.AddSearchData(cls);
                        frm.ColumnWidth = ColumnWidth;
                        if (UserInfo.Language == iLanguage.English)
                        {
                            cls.PrimaryKeyName = "ID";
                            cls.strFilter = "ID";
                        }
                        else
                        {
                            cls.PrimaryKeyName = PrimaryKeyName;
                            cls.strFilter = strFilter;
                        }
                        frm.ShowDialog();
                        if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
                        {
                            if (IDCtrl != null)
                            {
                                IDCtrl.Text = (cls.PrimaryKeyValue.Trim());
                                if (cls.strArbNameValue != null)
                                    NameCtrl.Text = (cls.strArbNameValue.Trim());

                            }
                        }
                    }
                    return cls;
                }
                catch (Exception ex)
                {
                    return cls;
                }
            }



            public static CSearch FindBySql(ref CSearch cls, Control IDCtrl, Control NameCtrl, string PrimaryKeyName, string strFilter, int[] ColumnWidth)
            {
                try
                {

                    cls.AddField(PrimaryKeyName, strFilter);
                    cls.SearchCol = 1;
                    if (cls.SQLStr != "")
                    {
                        frmSearch frm = new frmSearch();
                        frm.AddSearchData(cls);
                        frm.ColumnWidth = ColumnWidth;
                        cls.PrimaryKeyName = PrimaryKeyName;
                        cls.strFilter = strFilter;

                        frm.ShowDialog();

                        if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
                        {
                            if (IDCtrl != null)
                            {
                                IDCtrl.Text = (cls.PrimaryKeyValue.Trim());
                                if (cls.strArbNameValue != null)
                                    NameCtrl.Text = (cls.strArbNameValue.Trim());

                            }
                        }
                    }
                    return cls;
                }
                catch (Exception ex)
                {
                    return cls;
                }
            }



            //البحث عن بيانات جدول من عمودين مع الفترة
            public static CSearch Find(string TableName, string PrimaryKeyName, string Fillter = "")
            {
                try
                {
                    CSearch cls = new CSearch();
                    int[] ColumnWidth = new int[] { 130, 450 };
                    cls.SQLStr = "SELECT  " + PrimaryKeyName + " as الرقم, ArbName as الاسم   FROM " + TableName + " WHERE Cancel =0  ";
                    if (UserInfo.Language == iLanguage.English)
                        cls.SQLStr = "SELECT  " + PrimaryKeyName + " as ID, EngName as    Name  FROM " + TableName + " WHERE Cancel =0  ";


                    if (Fillter != string.Empty)
                        cls.SQLStr = cls.SQLStr + " ANd  " + Fillter;

                    cls.SQLStr = cls.SQLStr + "  Order by " + PrimaryKeyName;


                    if (cls.SQLStr != "")
                    {
                        frmSearch frm = new frmSearch();
                        frm.AddSearchData(cls);
                        frm.ColumnWidth = ColumnWidth;
                        if (UserInfo.Language == iLanguage.English)
                        {
                            cls.PrimaryKeyName = "ID";
                            cls.strFilter = "ID";
                        }
                        else
                        {
                            cls.PrimaryKeyName = PrimaryKeyName;
                            cls.strFilter = "الرقم";
                        }
                        frm.ShowDialog();

                    }
                    return cls;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }


            public static CSearch FindAccounts(string Fillter = "")
            {
                try
                {
                    CSearch cls = new CSearch();
                    int[] ColumnWidth = new int[] { 130, 450 };
                    cls.SQLStr = "SELECT  ACCOUNTID as الرقم, ArbName as الاسم   FROM  ACC_ACCOUNTS WHERE Cancel =0  And ACCOUNTTYPEID= 2 And FACILITYID=" + UserInfo.FacilityID;
                    cls.strArbNameValue = "الاسم";

                    if (UserInfo.Language == iLanguage.English)
                    {

                        cls.SQLStr = "SELECT ACCOUNTID as ID, EngName as    Name  FROM  ACC_ACCOUNTS WHERE Cancel =0  And ACCOUNTTYPEID= 2 And FACILITYID=" + UserInfo.FacilityID;
                        cls.strArbNameValue = "Name";

                    }
                    if (Fillter != string.Empty)
                        cls.SQLStr = cls.SQLStr + " ANd  " + Fillter;

                    cls.SQLStr = cls.SQLStr + "  Order by ACCOUNTID";


                    if (cls.SQLStr != "")
                    {
                        frmSearch frm = new frmSearch();
                        frm.AddSearchData(cls);
                        frm.ColumnWidth = ColumnWidth;
                        if (UserInfo.Language == iLanguage.English)
                        {
                            cls.PrimaryKeyName = "ID";
                            cls.strFilter = "ID";
                        }
                        else
                        {
                            cls.PrimaryKeyName = "ACCOUNTID";
                            cls.strFilter = "الرقم";
                        }
                        frm.ShowDialog();

                    }
                    return cls;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }



            public static CSearch Find(string TableName, string PremaryKey, string Fillter = "", bool PubSearchMultiRows = false, BaseForm form = null, string SerchField = "قائمة البحث")
            {
                CSearch cls = new CSearch();
                int[] ColumnWidth = new int[] { 130, 300 };

                cls.SQLStr = "SELECT  " + PremaryKey + " as الرقم, ArbName as الاسم   FROM " + TableName + " WHERE Cancel =0  ";
                if (UserInfo.Language == iLanguage.English)
                    cls.SQLStr = "SELECT  " + PremaryKey + " as ID, EngName as    Name  FROM " + TableName + " WHERE Cancel =0  ";


                if (Fillter != string.Empty)
                    cls.SQLStr = cls.SQLStr + " ANd  " + Fillter;

                cls.SQLStr = cls.SQLStr + "  Order by " + PremaryKey;
                cls.strArbNameValue = "الاسم";

                if (cls.SQLStr != "")
                {
                    frmSearch frm = new frmSearch();
                    cls.strFilter = "الرقم";
                    if (UserInfo.Language == iLanguage.English)
                        cls.strFilter = "ID";

                    frm.AddSearchData(cls);
                    frm.ColumnWidth = ColumnWidth;


                    frm.frmFromForm = form;
                    frm.PubSearchMultiRows = PubSearchMultiRows;
                    frm.Show();
                    frm.Text = SerchField;
                    return cls;

                }
                else
                    return null;
            }



            public static CSearch Find(string TableName, string[] Fields, int[] ColumnWidth, string Fillter = "", string FieldOrderBy = "")
            {
                CSearch cls = new CSearch();

                string fieldsList = "";
                for (int i = 0; i <= Fields.Length - 1; i++)
                    fieldsList = fieldsList + Fields[i] + ",";

                fieldsList = fieldsList.Remove(fieldsList.Length - 1, 1);

                cls.SQLStr = "SELECT  " + " " + fieldsList + "  FROM " + TableName + " WHERE Cancel =0 ";

                if (Fillter != string.Empty)
                    cls.SQLStr = cls.SQLStr + " ANd  " + Fillter;

                if (FieldOrderBy != string.Empty)
                    cls.SQLStr = cls.SQLStr + " order by " + FieldOrderBy;



                if (cls.SQLStr != "")
                {
                    frmSearch frm = new frmSearch();
                    cls.strFilter = "الرقم";
                    if (UserInfo.Language == iLanguage.English)
                        cls.strFilter = "ID";

                    frm.AddSearchData(cls);
                    frm.ColumnWidth = ColumnWidth;
                    frm.ShowDialog();
                    return cls;

                }
                else
                    return null;
            }


            public static CSearch Find(string TableName, string PremaryKey, int LISTID)
            {
                CSearch cls = new CSearch();
                int[] ColumnWidth = new int[] { 130, 300 };

                cls.SQLStr = "SELECT  " + PremaryKey + " as الرقم, ArbName as  الاسم  FROM " + TableName + " WHERE Cancel =0 AND LISTID=" + LISTID;
                if (UserInfo.Language == iLanguage.English)
                    cls.SQLStr = "SELECT  " + PremaryKey + " as ID, EngName as    Name  FROM " + TableName + " WHERE Cancel =0 AND LISTID=" + LISTID;

                ColumnWidth = new int[] { 50, 300 };

                if (cls.SQLStr != "")
                {
                    frmSearch frm = new frmSearch();
                    {
                        cls.strFilter = "الرقم";
                        cls.strArbNameValue = "الاسم";
                    }
                    if (UserInfo.Language == iLanguage.English)
                    {
                        cls.strFilter = "ID";
                        cls.strArbNameValue = "Name";
                    }
                    frm.AddSearchData(cls);
                    frm.ColumnWidth = ColumnWidth;
                    frm.ShowDialog();
                    return cls;

                }
                else
                    return null;
            }


            public static CSearch Find(string TableName, string PremaryKey)
            {
                CSearch cls = new CSearch();
                int[] ColumnWidth = new int[] { 130, 300 };

                cls.SQLStr = "SELECT  " + PremaryKey + " as الرقم, ArbName as  الاسم  FROM " + TableName + " WHERE Cancel =0";
                if (UserInfo.Language == iLanguage.English)
                    cls.SQLStr = "SELECT  " + PremaryKey + " as ID, EngName as    Name  FROM " + TableName + " WHERE Cancel =0 ";

                ColumnWidth = new int[] { 50, 300 };

                if (cls.SQLStr != "")
                {
                    frmSearch frm = new frmSearch();
                    {
                        cls.strFilter = "الرقم";
                        cls.strArbNameValue = "الاسم";
                    }
                    if (UserInfo.Language == iLanguage.English)
                    {
                        cls.strFilter = "ID";
                        cls.strArbNameValue = "Name";
                    }
                    frm.AddSearchData(cls);
                    frm.ColumnWidth = ColumnWidth;
                    frm.ShowDialog();
                    return cls;

                }
                else
                    return null;
            }

            public static CSearch BranchsList(BaseForm form)
            {
                return Lovs.Find("Branches", "BranchID", "", false, form, "قائمة الفروع");

            }
            public static CSearch FacilitiesList(BaseForm form)
            {
                return Lovs.Find("Facilityes", "FacilityID", "", false, form, "قائمة الشركات");

            }

            public static CSearch AccountsList(BaseForm form)
            {
                return Lovs.Find("ACC_ACCOUNTS", "ACCOUNTID", "", false, form, "قائمة الحسابات");
            }

            public static CSearch AccountsLastLevelList(BaseForm form)
            {
                return Lovs.Find("ACC_ACCOUNTS", "ACCOUNTID", "ACCOUNTTYPEID=2 ", false, form, "قائمة الحسابات");
            }

            public static CSearch SuppliersAccountlList(BaseForm form)
            {
                return Lovs.Find("SAL_SUPPLIERS", "ID", "FACILITYID=" + UserInfo.FacilityID, false, form, "قائمة الموردين");
            }


            public static CSearch CustomersAccountlList(BaseForm form)
            {
                return Lovs.Find("SAL_CUSTOMERS", "ID", "FACILITYID=" + UserInfo.FacilityID, false, form, "قائمة العملاء");
            }
            public static CSearch FacilitysList(BaseForm form)
            {
                return Lovs.Find("GLB_FACILITY", "ID", "", false, form, "قائمة الشركات");

            }

            public static CSearch Find(string TableName, string PremaryKey, BaseForm form)
            {
                return Lovs.Find(TableName, PremaryKey, "", false, form, form.Text);
            }


            public static CSearch CityList(BaseForm form)
            {
                return Lovs.Find("GLB_CITY", "ID", "", false, form, "قائمة المدن");

            }
            public static CSearch RegionList(BaseForm form)
            {
                return Lovs.Find("GLB_REGION", "ID", "", false, form, "قائمة الدول");

            }
            public static CSearch EmbLoyeesList(BaseForm form)
            {
                return Lovs.Find("HR_EMPLOYEEFILE", "ID", "", false, form, "قائمة الموظفين");
            }
            public static CSearch ScreenList(BaseForm form)
            {
                return Lovs.Find("MAINMENU", "MENUID", " MENULEVELID=3 ", false, form, "قائمة الشاشات  ");
            }
            public static CSearch CurrenciesList(BaseForm form)
            {
                return Lovs.Find("ACC_CURRENCY", "ID", "", false, form, "قائمة العملات");

            }
            public static CSearch RolesList(BaseForm form)
            {
                return Lovs.Find("GLB_ROLLS", "ID", "", false, form, "قائمة مجموعات الصلاحيات");

            }

            public static CSearch UsersList(BaseForm form)
            {
                return Lovs.Find("GLB_SYSUSER", "ID", "", false, form, "قائمة المستخدمين  ");

            }

            public static CSearch StoresList(BaseForm form)
            {
                return Lovs.Find("STC_STORES", "ID", " BRANCHID =" + UserInfo.BRANCHID, false, form, "المخازن");

            }
            public static CSearch StcClassList(BaseForm form, int ListID)
            {
                return Lovs.Find("STC_CLASSES", "ID", "LISTID=" + ListID, false, form, "قائمة تصنيفات المخازن ");

            }

            public static CSearch CostCentersList(BaseForm form)
            {
                return Lovs.Find("ACC_COSTCENTERS", "ID", " BRANCHID =" + UserInfo.BRANCHID, false, form, "مراكز التكلفة");

            }

            public static CSearch ProjectList(BaseForm form)
            {
                return Lovs.Find("ACC_PROJECTS", "ID", " BRANCHID =" + UserInfo.BRANCHID, false, form, "قائمة مجموعات الصلاحيات");

            }


            public static CSearch Find(string SQLStr, int[] ColumnWidth)
            {
                CSearch cls = new CSearch();
                cls.SQLStr = SQLStr;
                if (cls.SQLStr != "")
                {
                    frmSearch frm = new frmSearch();
                    {
                        cls.strFilter = "الرقم";
                        cls.PrimaryKeyName = "الرقم";
                    }
                    if (UserInfo.Language == iLanguage.English)
                    {
                        cls.strFilter = "ID";
                        cls.PrimaryKeyName = "ID";
                    }
                    frm.AddSearchData(cls);
                    frm.ColumnWidth = ColumnWidth;
                    frm.ShowDialog();
                    return cls;

                }
                else
                    return null;
            }


        }


}

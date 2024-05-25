using System;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraEditors;
using Edex.DAL;
using Edex.Model;
using Edex.GeneralObjects.GeneralClasses;
using Edex.GeneralObjects.GeneralForms;
using Edex.ModelSystem;
//using Edex.StockObjects.StoresClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Edex.Model.Language;
using Edex.DAL.Stc_itemDAL;
using DevExpress.XtraReports.UI;
using Edex.Reports;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraEditors.Repository;
namespace Edex.AccountsObjects.Codes
{ 
    public partial class frmDeclaringEstimatedSpends : Edex.GeneralObjects.GeneralForms.BaseForm
    {
        bool HasColumnErrors;
        public int rowIndex;
        private string PrimaryName;

        BindingList<Acc_DeclaringEstimatedSpends> AllRecords = new BindingList<Acc_DeclaringEstimatedSpends>();
        DataTable dt = new DataTable();
        public string strSQL = "SELECT dbo.Acc_DeclaringEstimatedSpends.AccountID,   "
             + " dbo.Acc_DeclaringEstimatedSpends.Evaluation , dbo.Acc_Accounts.ArbName AS AccountName,Acc_DeclaringEstimatedSpends.Notes, "
             + " dbo.Acc_DeclaringEstimatedSpends.BranchID "
             + " FROM  dbo.Acc_DeclaringEstimatedSpends LEFT OUTER JOIN"
             + " dbo.Acc_Accounts ON dbo.Acc_DeclaringEstimatedSpends.AccountID = dbo.Acc_Accounts.AccountID AND "
             + " dbo.Acc_DeclaringEstimatedSpends.BranchID = dbo.Acc_Accounts.BranchID"
             + " WHERE dbo.Acc_DeclaringEstimatedSpends.BranchID =" +MySession.GlobalBranchID + ""
             + " ORDER BY dbo.Acc_DeclaringEstimatedSpends.ID";
        Acc_DeclaringEstimatedSpends acc = new Acc_DeclaringEstimatedSpends();
        public frmDeclaringEstimatedSpends()
        {
            InitializeComponent();

            ribbonControl1.Pages[0].Groups[0].ItemLinks[4].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[5].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[7].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[8].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[9].Visible = false;

            ribbonControl1.Pages[0].Groups[0].ItemLinks[11].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[12].Visible = true;
            PrimaryName = "ArbName";
            if (UserInfo.Language == iLanguage.English)
            {
                PrimaryName = "EngName";
                  strSQL = "SELECT dbo.Acc_DeclaringEstimatedSpends.AccountID,   "
             + " dbo.Acc_DeclaringEstimatedSpends.Evaluation , dbo.Acc_Accounts.EngName AS AccountName,Acc_DeclaringEstimatedSpends.Notes, "
             + " dbo.Acc_DeclaringEstimatedSpends.BranchID "
             + " FROM  dbo.Acc_DeclaringEstimatedSpends LEFT OUTER JOIN"
             + " dbo.Acc_Accounts ON dbo.Acc_DeclaringEstimatedSpends.AccountID = dbo.Acc_Accounts.AccountID AND "
             + " dbo.Acc_DeclaringEstimatedSpends.BranchID = dbo.Acc_Accounts.BranchID"
             + " WHERE dbo.Acc_DeclaringEstimatedSpends.BranchID =" + MySession.GlobalBranchID + ""
             + " ORDER BY dbo.Acc_DeclaringEstimatedSpends.ID";
            }
        }
        protected override void DoPrint()
        {
            gridControl.ShowRibbonPrintPreview();
        }

        private void frmDeclaringMainAccounts_Load(object sender, EventArgs e)
        {
            AllRecords = new BindingList<Acc_DeclaringEstimatedSpends>();
            AllRecords.AllowNew = true;
            AllRecords.AllowEdit = true;
            AllRecords.AllowRemove = true;
            gridControl.DataSource = AllRecords;
            gridView2.Columns["AccountName"].Caption = "اسم الحساب";
            gridView2.Columns["AccountID"].Caption = "رقم الحساب";
            gridView2.Columns["ID"].Caption = "مسلسل";
            gridView2.Columns["Evaluation"].Caption = "القيمة التقديرية";
            gridView2.Columns["Notes"].Caption = "الملاحظــــات";
            gridView2.Columns["ID"].Visible = false;
            if (UserInfo.Language == iLanguage.English)
            {
                gridView2.Columns["AccountName"].Caption = "Account Name";
                gridView2.Columns["AccountID"].Caption = "Account ID";
                gridView2.Columns["ID"].Caption = "ID";
                gridView2.Columns["Evaluation"].Caption = "Evaluation";
                gridView2.Columns["Notes"].Caption = "Notes";
            }
            RepositoryItemLookUpEdit rAccountName = Common.LookUpEditAccountName();
            gridView2.Columns["AccountName"].ColumnEdit = rAccountName;
            gridControl.RepositoryItems.Add(rAccountName);
            dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    acc.ID = i + 1;
                    acc.AccountID = Comon.cDbl(dt.Rows[i]["AccountID"].ToString());
                    acc.AccountName = dt.Rows[i]["AccountName"].ToString();
                    acc.Evaluation = dt.Rows[i]["Evaluation"].ToString();
                    acc.Notes = dt.Rows[i]["Notes"].ToString();
                    AllRecords.Add(acc);
                }
                gridControl.DataSource = dt;
                DoEdit();
            }
        }
        public void Find()
        {
            CSearch cls = new CSearch();
            int[] ColumnWidth = new int[] { 100, 300 };

            cls.SQLStr = " SELECT  AccountID as [الرقم], ArbName as [اسم الحساب] FROM Acc_Accounts"
            + " WHERE Cancel =0 And BranchID = " + MySession.GlobalBranchID;



            if (UserInfo.Language == iLanguage.English)
                cls.SQLStr = " SELECT  AccountID as [Account ID], EngName as [Account Name] FROM Acc_Accounts"
          + " WHERE Cancel =0  And BranchID = " + MySession.GlobalBranchID;


            ColumnWidth = new int[] { 80, 200 };




            if (cls.SQLStr != "")
            {
                frmSearch frm = new frmSearch();

                cls.strFilter = "الرقم";
                cls.PrimaryKeyValue = "الرقم";
                cls.PrimaryKeyField = "اسم الحساب";

                if (UserInfo.Language == iLanguage.English)
                {
                    cls.strFilter = "ID";
                    cls.PrimaryKeyValue = "IDe";
                    cls.PrimaryKeyField = "Account Name";

                }

                frm.AddSearchData(cls);
                frm.ColumnWidth = ColumnWidth;
                frm.ShowDialog();
                GetSelectedSearchValue(cls);
            }
        }
        protected override void DoSearch()
        {
            try
            {
                //    Find();
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        public void GetSelectedSearchValue(CSearch cls)
        {
            if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
            {

                // gridView1.SetRowCellValue(rowIndex, gridView1.Columns["BarCode"], cls.PrimaryKeyValue);
                strSQL = "SELECT ArbName FROM Acc_Accounts WHERE AccountID =" + Comon.cDbl(cls.PrimaryKeyValue) + " And BranchID =" + MySession.GlobalBranchID;
                if (UserInfo.Language == iLanguage.English)
                    strSQL = "SELECT EngName FROM Acc_Accounts WHERE AccountID =" + Comon.cDbl(cls.PrimaryKeyValue) + " And BranchID =" + MySession.GlobalBranchID;

                //DataTable dd = new DataTable();
                // dd = Lip.SelectRecord(strSQL);
                string sst = Lip.GetValue(strSQL);
                if (sst != "")
                {

                    gridView2.SetFocusedRowCellValue(gridView2.Columns["AccountName"], sst);
                    gridView2.SetFocusedRowCellValue(gridView2.Columns["AccountID"], cls.PrimaryKeyValue);
                }

            }
        }

        private void gridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {

                if (gridView2.FocusedColumn.Name == "AccountID")
                    Find();
            }
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            rowIndex = e.FocusedRowHandle;
        }

        private void frmDeclaringMainAccounts_KeyDown(object sender, KeyEventArgs e)
        {
            // Find();
        }



        /*************************************************************/
        protected override void DoSave()
        {
            try
            {
                if (!FormAdd)
                {
                    Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToAddNewRecord);
                    return;

                }


                if (!FormUpdate)
                {
                    Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToUpdateRecord);
                    return;
                }
                else
                {
                    bool Yes = Messages.MsgWarningYesNo(Messages.TitleInfo, Messages.msgConfirmUpdate);
                    if (!Yes)
                        return;
                }


                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                for (int i = 0; i <= gridView2.DataRowCount - 1; ++i)
                {
                    if (gridView2.GetRowCellValue(i, "AccountID").ToString() == "")
                    {
                        Messages.MsgExclamationk(Messages.TitleInfo, (UserInfo.Language == iLanguage.English ? "Enter The Correct Account ID." : "لا يوجد حساب رقمة 0 تأكد من ارقام الحساب المدخله"));
                    }
                }
                Lip.NewFields();
                Lip.Table = "Acc_DeclaringEstimatedSpends";
                Lip.sCondition = "BranchID=" +MySession.GlobalBranchID;
                Lip.ExecuteDelete();
                gridView2.MoveLast();

                for (int i = 0; i <= gridView2.DataRowCount - 1; ++i)
                {

                    Lip.NewFields();
                    Lip.Table = "Acc_DeclaringEstimatedSpends";
                    Lip.AddNumericField("BranchID",MySession.GlobalBranchID);
                    Lip.AddNumericField("AccountID", gridView2.GetRowCellValue(i, "AccountID").ToString());
                    Lip.AddNumericField("Evaluation", gridView2.GetRowCellValue(i, "Evaluation").ToString());
                    Lip.AddStringField("Notes", gridView2.GetRowCellValue(i, "Notes").ToString());
                    Lip.AddNumericField("UserID", MySession.UserID);
                    Lip.AddNumericField("RegDate", Comon.cLong(Lip.GetServerDateSerial()).ToString());
                    Lip.AddNumericField("RegTime", Comon.cLong(Lip.GetServerDateSerial()).ToString());
                    Lip.AddStringField("ComputerInfo", (!string.IsNullOrEmpty(MySession.GlobalComputerInfo) ? MySession.GlobalComputerInfo : " "));


                    Lip.AddNumericField("EditUserID", MySession.UserID);
                    Lip.AddNumericField("EditDate", Comon.cLong(Lip.GetServerDateSerial()).ToString());
                    Lip.AddNumericField("EditTime", Comon.cLong(Lip.GetServerDateSerial()).ToString());
                    Lip.AddStringField("EditComputerInfo", (!string.IsNullOrEmpty(MySession.GlobalComputerInfo) ? MySession.GlobalComputerInfo : " "));




                    Lip.ExecuteInsert();

                }

                SplashScreenManager.CloseForm(false);
                Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
            }

            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);

                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
            /*******************************************************************/
        }

        private void gridView2_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            double num;
            GridView view = sender as GridView;
            view.ClearColumnErrors();
            HasColumnErrors = false;
            string ColName = view.FocusedColumn.FieldName;

            if (ColName == "AccountID")
            {
                if (e.Value == null || string.IsNullOrWhiteSpace(e.Value.ToString()))
                {
                    e.Valid = false;
                    HasColumnErrors = true;
                    e.ErrorText = Messages.msgInputIsRequired;
                }
                else if (!(double.TryParse(e.Value.ToString(), out num)))
                {
                    e.Valid = false;
                    HasColumnErrors = true;
                    e.ErrorText = Messages.msgInputShouldBeNumber;
                }
                else if (Comon.ConvertToDecimalPrice(e.Value.ToString()) <= 0)
                {
                    e.Valid = false;
                    HasColumnErrors = true;
                    e.ErrorText = Messages.msgInputIsGreaterThanZero;
                }

                /****************************************/
                if (ColName == "AccountID" && e.Valid == true)
                {
                    DataTable dt = new Acc_AccountsDAL().GetAcc_AccountsByLevel(MySession.GlobalBranchID, MySession.GlobalFacilityID);
                    {
                        DataRow[] row = dt.Select("AccountID=" + e.Value.ToString());
                        if (row.Length == 0)
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundThisAccountID;
                        }
                        else
                        {
                            FileItemData(row[0]);
                        }
                    }

                }

            }
            else if (ColName == "AccountName")
            {
                DataTable dtAccountName = Lip.SelectRecord("Select AccountID, " + PrimaryName + " AS  AccountName from Acc_Accounts Where  Cancel=0 and BranchID="+ MySession.GlobalBranchID+" And LOWER (" + PrimaryName + ")=LOWER ('" + e.Value.ToString() + "') And FacilityID=" + UserInfo.FacilityID + "   AND AccountLevel=" + MySession.GlobalNoOfLevels);
                if (dtAccountName == null && dtAccountName.Rows.Count == 0)
                {
                    e.Valid = false;
                    HasColumnErrors = true;
                    e.ErrorText = Messages.msgNoFoundThisAccountID;
                }
                else
                {
                    gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["AccountID"], dtAccountName.Rows[0]["AccountID"]);
                    gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["AccountName"], dtAccountName.Rows[0]["AccountName"]);
                    gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["BranchID"],MySession.GlobalBranchID);

                }

            }
        }
        protected override void DoEdit()
        {
            DataTable dtItem = new DataTable();
            dtItem.Columns.Add("ID", System.Type.GetType("System.String"));
            dtItem.Columns.Add("BranchID", System.Type.GetType("System.String"));
            dtItem.Columns.Add("FacilityID", System.Type.GetType("System.String"));
            dtItem.Columns.Add("AccountName", System.Type.GetType("System.String"));
            dtItem.Columns.Add("Evaluation", System.Type.GetType("System.String"));
            dtItem.Columns.Add("AccountID", System.Type.GetType("System.String"));
            dtItem.Columns.Add("Notes", System.Type.GetType("System.String"));
            for (int i = 0; i <= gridView2.DataRowCount - 1; i++)
            {
                dtItem.Rows.Add();
                dtItem.Rows[i]["ID"] = i;
                dtItem.Rows[i]["BranchID"] = UserInfo.FacilityID;
                dtItem.Rows[i]["AccountID"] = gridView2.GetRowCellValue(i, gridView2.Columns["AccountID"]);
                dtItem.Rows[i]["AccountName"] = gridView2.GetRowCellValue(i, gridView2.Columns["AccountName"]);
                dtItem.Rows[i]["Evaluation"] = gridView2.GetRowCellValue(i, gridView2.Columns["Evaluation"]);
                dtItem.Rows[i]["Notes"] = gridView2.GetRowCellValue(i, gridView2.Columns["Notes"]);
            }
            gridControl.DataSource = dtItem;
            //EnabledControl(true);
            //Validations.DoEditRipon(this, ribbonControl1);
        }
        private void FileItemData(DataRow dr)
        {
            gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["AccountID"], dr["AccountID"]);
            gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["AccountName"], dr[PrimaryName]);
            gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["BranchID"],MySession.GlobalBranchID);

        }
    }
}
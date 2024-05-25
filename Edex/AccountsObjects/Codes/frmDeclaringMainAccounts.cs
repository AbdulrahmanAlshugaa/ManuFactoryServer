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
namespace Edex.AccountsObjects.Codes
{
    public partial class frmDeclaringMainAccounts : Edex.GeneralObjects.GeneralForms.BaseForm
    {
        bool HasColumnErrors;
        public int rowIndex;
        BindingList<Acc_DeclaringMainAccounts> AllRecords = new BindingList<Acc_DeclaringMainAccounts>();
        DataTable dt = new DataTable();
        public string strSQL = "SELECT dbo.Acc_DeclaringMainAccounts.AccountID, dbo.Acc_DeclaringMainAccounts.DeclareAccountName, "
             + " dbo.Acc_DeclaringMainAccounts.AccountArbName AS AlisedAccountName, dbo.Acc_Accounts.ArbName AS AccountName, "
             + " dbo.Acc_DeclaringMainAccounts.BranchID "
             + " FROM  dbo.Acc_DeclaringMainAccounts LEFT OUTER JOIN"
             + " dbo.Acc_Accounts ON dbo.Acc_DeclaringMainAccounts.AccountID = dbo.Acc_Accounts.AccountID AND "
             + " dbo.Acc_DeclaringMainAccounts.BranchID = dbo.Acc_Accounts.BranchID"
             + " WHERE dbo.Acc_DeclaringMainAccounts.BranchID =" + UserInfo.BRANCHID + ""
             + " ORDER BY dbo.Acc_DeclaringMainAccounts.ID";
        Acc_DeclaringMainAccounts acc = new Acc_DeclaringMainAccounts();
        public frmDeclaringMainAccounts()
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

        }
        protected override void DoPrint()
        {
            gridControl1.ShowRibbonPrintPreview();
        }

        private void frmDeclaringMainAccounts_Load(object sender, EventArgs e)
        {
            dt = Lip.SelectRecord(strSQL);
            //new Acc_DeclaringMainAccountsDAL().GetAcc_DeclaringMainAccounts(UserInfo.BRANCHID, UserInfo.FacilityID);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    //acc.ID =Comon.cInt(dt.Rows[i]["ID"].ToString());
                    acc.AccountID = Comon.cDbl(dt.Rows[i]["AccountID"].ToString());
                    acc.AccountName = dt.Rows[i]["AccountName"].ToString();
                    acc.DeclareAccountName = dt.Rows[i]["DeclareAccountName"].ToString();
                    AllRecords.Add(acc);
                }

                gridControl1.DataSource = dt;



            }
        }
        public void Find()
        {
            CSearch cls = new CSearch();
            int[] ColumnWidth = new int[] { 100, 300 };

            cls.SQLStr = " SELECT  AccountID as [الرقم], ArbName as [اسم الحساب] FROM Acc_Accounts"
            + " WHERE Cancel =0 And BranchID = " + UserInfo.BRANCHID;



            if (UserInfo.Language == iLanguage.English)
                cls.SQLStr = " SELECT  AccountID as [Account ID], EngName as [Account Name] FROM Acc_Accounts"
          + " WHERE Cancel =0  And BranchID = " + UserInfo.BRANCHID;


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
                strSQL = "SELECT ArbName FROM Acc_Accounts WHERE AccountID =" + Comon.cDbl(cls.PrimaryKeyValue) + " And BranchID =" + UserInfo.BRANCHID;
                if (UserInfo.Language == iLanguage.English)
                    strSQL = "SELECT EngName FROM Acc_Accounts WHERE AccountID =" + Comon.cDbl(cls.PrimaryKeyValue) + " And BranchID =" + UserInfo.BRANCHID;

                //DataTable dd = new DataTable();
                // dd = Lip.SelectRecord(strSQL);
                string sst = Lip.GetValue(strSQL);
                if (sst != "")
                {

                    gridView1.SetFocusedRowCellValue(gridView1.Columns["AccountName"], sst);

                    gridView1.SetFocusedRowCellValue(gridView1.Columns["AccountID"], cls.PrimaryKeyValue);



                }

            }
        }

        private void gridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {

                if (gridView1.FocusedColumn.Name == "AccountID")
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
                for (int i = 0; i <= gridView1.DataRowCount - 1; ++i)
                {
                    if (gridView1.GetRowCellValue(i, "AccountID").ToString() == "")
                    {
                        Messages.MsgExclamationk(Messages.TitleInfo, (UserInfo.Language == iLanguage.English ? "Enter The Correct Account ID." : "لا يوجد حساب رقمة 0 تأكد من ارقام الحساب المدخله"));
                    }
                }
                for (int i = 0; i <= gridView1.DataRowCount - 1; ++i)
                {



                    Lip.NewFields();
                    Lip.Table = "Acc_DeclaringMainAccounts";

                    Lip.AddNumericField("BranchID", UserInfo.BRANCHID);
                    Lip.AddNumericField("AccountID", gridView1.GetRowCellValue(i, "AccountID").ToString());
                    Lip.AddNumericField("RegTime", Comon.cLong(Lip.GetServerTimeSerial()).ToString());
                    Lip.AddNumericField("EditUserID", MySession.UserID);
                    Lip.AddNumericField("EditDate", Comon.cLong(Lip.GetServerDateSerial()).ToString());

                    Lip.AddNumericField("EditTime", Comon.cLong(Lip.GetServerDateSerial()).ToString());
                    Lip.AddStringField("EditComputerInfo", (!string.IsNullOrEmpty(MySession.GlobalComputerInfo) ? MySession.GlobalComputerInfo : " "));

                    Lip.sCondition = "DeclareAccountName ='" + gridView1.GetRowCellValue(i, "DeclareAccountName").ToString() + "' And  BranchID=" + UserInfo.BRANCHID;
                    Lip.ExecuteUpdate();


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
    }
}




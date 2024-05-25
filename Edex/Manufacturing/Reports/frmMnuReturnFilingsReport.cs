using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Edex.GeneralObjects.GeneralForms;
using System.Globalization;
using Edex.Model;
using Edex.Model.Language;
using DevExpress.XtraSplashScreen;
using Edex.ModelSystem;
using Edex.GeneralObjects.GeneralClasses;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.Utils;

namespace Edex.Manufacturing.Reports
{
    
    public partial class frmMnuReturnFilingsReport : BaseForm
    {
        #region Declare
        private bool IsNewRecord;
        private string strSQL;
        private string PrimaryName;
        string FocusedControl = "";

        private string filter = "";
        public const int xMoveFirst = 7;
        public const int xMovePrev = 8;
        public const int xMoveNext = 9;
        public const int xMoveLast = 10;
        private string ItemName;
        private string SizeName;
        private string CaptionItemName;
        public CultureInfo culture = new CultureInfo("en-US");
        public bool HasColumnErrors = false;
        private DataTable dt;
        private int TypeStage = 0;
        #endregion
        public frmMnuReturnFilingsReport()
        {
            InitializeComponent();

            try
            {
                ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[2].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[3].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[4].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[5].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[7].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[8].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[9].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Visible = true;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Caption = (UserInfo.Language == iLanguage.Arabic ? "استعلام جديد" : "New Query");
                InitializeFormatDate(txtFromDate);
                InitializeFormatDate(txtToDate);
                GridLost.OptionsBehavior.ReadOnly = true;
                GridLost.OptionsBehavior.Editable = false;
                ItemName = "ArbItemName";
                SizeName = "ArbSizeName";
                PrimaryName = "ArbName";
                CaptionItemName = "اسم الصنف";
                if (UserInfo.Language == iLanguage.English)
                {
                    ItemName = "EngItemName";
                    SizeName = "EngSizeName";
                    PrimaryName = "EngName";
                    CaptionItemName = "Item Name";
                    dvgColCustomer.Caption = "Customer Name";
                    dvgColQTY.Caption = "Debit";
                    dvgColTypeStage.Caption = "Type Stage";
                    dvgColAccountName.Caption = "Account Name";
                    dvgColAccountID.Caption = "Account ID";

                }
                FillCombo.FillComboBox(cmbTypeStage, "Manu_TypeStages", "ID", PrimaryName, "", "", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
            
                FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
               
                
                cmbBranchesID.EditValue = MySession.GlobalBranchID;
                cmbBranchesID.ReadOnly = !MySession.GlobalAllowBranchModificationAllScreens;
                txtStoreID.Validating += txtCustomerID_Validating;
                txtAccountIDFactory.Validating += txtAccountIDFactory_Validating;

            }
            catch { }
        }
        private void InitializeFormatDate(DateEdit Obj)
        {
            Obj.Properties.Mask.UseMaskAsDisplayFormat = true;
            Obj.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            Obj.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            Obj.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            Obj.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            Obj.Properties.Mask.EditMask = "dd/MM/yyyy";
            Obj.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            Obj.EditValue = DateTime.Now;
        }
        protected override void DoAddFrom()
        {
            try
            {
                dt.Clear();
                gridControl1.RefreshDataSource();
                btnShow.Visible = true;
                DoNew();

            }
            catch (Exception ex)
            {
                //WT.msgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void btnShow_Click(object sender, EventArgs e)
        {
            try
            {

                DataRow row;
                btnShow.Visible = false;
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                Application.DoEvents();
                filter = "  Mnu_ReturnFilingsMaster.Cancel = 0   And ";

                if (Comon.cInt(cmbTypeStage.EditValue) > 0)
                    filter = filter + "   Mnu_ReturnFilingsMaster.TypeStageID = " +Comon.cInt(cmbTypeStage.EditValue) + " And ";
                if (Comon.cInt(cmbBranchesID.Text) != 0)
                    filter = filter + "   Mnu_ReturnFilingsMaster.BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + " AND ";

                if (Comon.cDbl(txtAccountIDFactory.Text) > 0)
                    filter = filter + " Mnu_ReturnFilingsMaster.StoreIDBefore=" + Comon.cDbl(txtAccountIDFactory.Text) + " AND ";
               
                if (Comon.cDbl(txtStoreID.Text) > 0)
                    filter = filter + " Mnu_ReturnFilingsMaster.StoreIDAfter=" + Comon.cDbl(txtStoreID.Text) + " AND ";

                long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));
                long FromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text));
                if (FromDate != 0)
                    filter = filter + " dbo.Mnu_ReturnFilingsMaster.CommandDate>=" + FromDate + " AND ";

                if (ToDate != 0)
                    filter = filter + " dbo.Mnu_ReturnFilingsMaster.CommandDate<=" + ToDate + " AND ";

                filter = filter.Remove(filter.Length - 4, 4);

                string str = @"SELECT  Mnu_ReturnFilingsMaster.StoreIDBefore as AccountID,   dbo.Acc_Accounts." + PrimaryName + @"  AS AccountName, dbo.Manu_TypeStages." + PrimaryName + @" AS TypeStageName,
                             ISNULL(SUM( dbo.Mnu_ReturnFilingsDetails.QTY),0) as QTY 
                             FROM dbo.Mnu_ReturnFilingsDetails INNER JOIN
                             dbo.Mnu_ReturnFilingsMaster ON dbo.Mnu_ReturnFilingsDetails.CommandID = dbo.Mnu_ReturnFilingsMaster.CommandID and  dbo.Mnu_ReturnFilingsDetails.BranchID = dbo.Mnu_ReturnFilingsMaster.BranchID INNER JOIN
                             dbo.Acc_Accounts ON dbo.Mnu_ReturnFilingsMaster.StoreIDBefore = dbo.Acc_Accounts.AccountID  and dbo.Mnu_ReturnFilingsMaster.BranchID = dbo.Acc_Accounts.BranchID 
						      INNER JOIN dbo.Manu_TypeStages ON dbo.Mnu_ReturnFilingsMaster.TypeStageID = dbo.Manu_TypeStages.ID  where " + filter+@" 
						     GROUP BY   Acc_Accounts." + PrimaryName + @", dbo.Manu_TypeStages." + PrimaryName + @", Mnu_ReturnFilingsMaster.StoreIDBefore ";
                dt = Lip.SelectRecord(str);
                if (dt.Rows.Count > 0)
                {
                    gridControl1.DataSource = dt;
                }
                else
                {
                    SplashScreenManager.CloseForm(false);
                    Messages.MsgInfo(Messages.TitleInfo, MySession.GlobalLanguageName == iLanguage.Arabic ? "لايوجد بيانات لعرضها" : "There is no Data to show it");

                    btnShow.Visible = true;

                    DoNew();
                }
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
        }
        protected override void DoNew()
        {
            try
            {
                txtStoreID.Text = "";
                txtCustomerID_Validating(null, null);
      
                txtAccountIDFactory.Text = "";
                txtAccountIDFactory_Validating(null, null);

            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        public void Find()
        {
            CSearch cls = new CSearch();
            int[] ColumnWidth = new int[] { 100, 300 };
            string SearchSql = "";
            string Condition = " Where 1=1 ";

            FocusedControl = GetIndexFocusedControl();

            if (FocusedControl == null) return;

            else if (FocusedControl.Trim() == txtStoreID.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtStoreID,lblCustomerName , "StoreID", "رقم الحساب",  Comon.cInt(cmbBranchesID.EditValue));
                else
                    PrepareSearchQuery.Find(ref cls, txtStoreID, lblCustomerName, "StoreID", "Account ID",  Comon.cInt(cmbBranchesID.EditValue));
            }
            else if (FocusedControl.Trim() == txtAccountIDFactory.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtAccountIDFactory, lblAccountNameFactory, "AccountID", "رقم الحساب", Comon.cInt(cmbBranchesID.EditValue));
                else
                    PrepareSearchQuery.Find(ref cls, txtAccountIDFactory, lblAccountNameFactory, "AccountID", "Account ID", Comon.cInt(cmbBranchesID.EditValue));
            }
          
            GetSelectedSearchValue(cls);
        }
        string GetIndexFocusedControl()
        {
            Control c = this.ActiveControl;
            if (c is DevExpress.XtraLayout.LayoutControl)
            {
                if (!(((DevExpress.XtraLayout.LayoutControl)ActiveControl).ActiveControl == null))
                {
                    c = ((DevExpress.XtraLayout.LayoutControl)ActiveControl).ActiveControl;
                }
            }
            if (c is DevExpress.XtraEditors.TextBoxMaskBox)
            {
                c = c.Parent;
            }

            if (c.Parent is DevExpress.XtraGrid.GridControl)
            {
                return c.Parent.Name;
            }

            return c.Name;
        }
        public void GetSelectedSearchValue(CSearch cls)
        {
            if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
            {
                 
                if (FocusedControl == txtStoreID.Name)
                {
                    txtStoreID.Text = cls.PrimaryKeyValue.ToString();
                    txtCustomerID_Validating(null, null);
                }

                else if (FocusedControl == txtAccountIDFactory.Name)
                {
                    txtAccountIDFactory.Text = cls.PrimaryKeyValue.ToString();
                    txtAccountIDFactory_Validating(null, null);
                }
            }
        }
        private void txtAccountIDFactory_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as AccountName FROM Acc_Accounts WHERE AccountID =" + Comon.cDbl(txtAccountIDFactory.Text) + " And Cancel =0 and BranchID="+MySession.GlobalBranchID;
                CSearch.ControlValidating(txtAccountIDFactory, lblAccountNameFactory, strSQL);//This Call  Function For Set  TypeName to txttypeName when The user Select TypeID

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void txtCustomerID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE AccountID =" + Comon.cLong(txtStoreID.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                CSearch.ControlValidating(txtStoreID,lblCustomerName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void frmMnuReturnFilingsReport_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
                Find();
        }

        private void GridLost_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            e.Graphics.FillRectangle(e.Cache.GetSolidBrush(Color.Beige), e.Bounds);
            e.Graphics.DrawRectangle(e.Cache.GetPen(Color.Black, 1), e.Bounds);
            e.Appearance.DrawString(e.Cache, e.DisplayText, e.Bounds);
            e.Handled = true;
            ((GridView)sender).Appearance.Row.TextOptions.HAlignment = HorzAlignment.Center;
            ((GridView)sender).Appearance.Row.TextOptions.VAlignment = VertAlignment.Center;
        }
        private void cmbTypeStage_EditValueChanged(object sender, EventArgs e)
        {
            TypeStage =Comon.cInt(cmbTypeStage.EditValue);
        }
    }
}
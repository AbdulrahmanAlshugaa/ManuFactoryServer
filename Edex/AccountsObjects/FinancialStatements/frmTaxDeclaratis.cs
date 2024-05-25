using DevExpress.XtraEditors;
using Edex.DAL;
using Edex.Model;
using Edex.GeneralObjects.GeneralClasses;
using Edex.GeneralObjects.GeneralForms;
using Edex.ModelSystem;
using Edex.StockObjects.StoresClasses;
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
using Edex.SalesAndPurchaseObjects.SalesClasses;
using DevExpress.Utils;
using DevExpress.XtraRichEdit.API.Native;
using Edex.AccountsObjects.Codes;
using Permissions = Edex.ModelSystem.Permissions;

namespace Edex.AccountsObjects.FinancialStatements
{
    public partial class frmTaxDeclaratis : Edex.GeneralObjects.GeneralForms.BaseForm
    {
        public DataTable importData = new DataTable();
        public bool sendFromExel = false;

        #region Declare
        private cSuppliers cClass = new cSuppliers();
        string FocusedControl = "";
        public string GetNewID;
        public const int xMoveFirst = 7;
        public const int xMovePrev = 8;
        public const int xMoveNext = 9;
        public const int xMoveLast = 10;
        public long rowCount = 0;
        public DataTable dt = new DataTable();
        private string strSQL;
        private bool IsNewRecord;
        public string ParentAccountID;
        public int AccountLevel;
        public string ArbName;
        public string EngName;
        public long AccountID;
        public bool IsNew = false;
        #endregion

        #region Form Event
        public frmTaxDeclaratis()
        {
            InitializeComponent();
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
            /***************************Edit & Print & Export ****************************/
            /*****************************************************************************/
            this.txtImportTaxCustoms.EditValueChanged += new System.EventHandler(this.txtEmail_EditValueChanged);
            this.txtImportTaxCustoms.Validating += new System.ComponentModel.CancelEventHandler(this.txtEmail_Validating);
            this.txtTaxableSales.Validating += new System.ComponentModel.CancelEventHandler(this.txtSupplierID_Validating);
            this.txtTaxableSales.EditValueChanged += new System.EventHandler(this.txtSupplierID_EditValueChanged);
            this.txtZeroDomesticSales.EditValueChanged += new System.EventHandler(this.txtArbName_EditValueChanged);
            this.txtZeroDomesticSales.Validating += new System.ComponentModel.CancelEventHandler(this.txtArbName_Validating);
            this.txtExports.Validating += new System.ComponentModel.CancelEventHandler(this.txtEngName_Validating);
        }

        #endregion
        #region Function
        protected override void DoAddFrom()
        {
            ClearFields();
        }
        public long GetNewAccountID()
        {
            try
            {
                int code; 
                int sNode;
                int SumDigitsCountBeforeSelectedLevel;
                int DigitsCountForSelectedLevel;
                long MaxID;
                string str;
                string strDigits = "";
                ParentAccountID = Lip.GetValue("SELECT AccountID FROM Acc_DeclaringMainAccounts WHERE DeclareAccountName='SupplierAccount'");
                AccountLevel = int.Parse(Lip.GetValue("SELECT AccountLevel FROM  Acc_Accounts WHERE AccountID = " + ParentAccountID)) + 1;
                str = Lip.GetValue("SELECT  MAX(AccountID) FROM  Acc_Accounts Where ParentAccountID=" + ParentAccountID + "  And BranchID =" + MySession.GlobalBranchID);
                strSQL = "SELECT Sum(DigitsNumber) FROM  Acc_AccountsLevels WHERE  BranchID = " + MySession.GlobalBranchID + " And LevelNumber <" + AccountLevel;
                SumDigitsCountBeforeSelectedLevel = int.Parse(Lip.GetValue(strSQL));
                strSQL = "SELECT  DigitsNumber FROM  Acc_AccountsLevels WHERE  BranchID = " + MySession.GlobalBranchID + " And LevelNumber =" + AccountLevel;
                DigitsCountForSelectedLevel = int.Parse(Lip.GetValue(strSQL));
                if (str == "")
                    code = 0;
                else
                    code = int.Parse(str.Substring(SumDigitsCountBeforeSelectedLevel, DigitsCountForSelectedLevel));
                MaxID = 1;
                for (int i = 1; i <= DigitsCountForSelectedLevel; ++i)
                {
                    MaxID = MaxID * 10;
                    strDigits = strDigits + "0";

                }
                if (code < MaxID)
                {

                    code = code + 1;
                    GetNewID = ParentAccountID.Substring(0, SumDigitsCountBeforeSelectedLevel) + code.ToString(strDigits);

                    // GetNewID +=code.ToString(strDigits);

                }
                else
                {
                    if (UserInfo.Language == iLanguage.English)
                        XtraMessageBox.Show("You Cannot Add More Than " + MaxID + " Accounts in This Level");
                    else
                        XtraMessageBox.Show("لا يمكن إضافة اكثر من " + MaxID + " حسابات في هذا المستوى");
                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

            return long.Parse(GetNewID.PadRight(MySession.GlobalAccountsLevelDigits, '0'));




        }
        string GetIndexFocusedControl()
        {
            Control c = this.ActiveControl;
            if (c == null) return null;
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
        protected override void Find()
        {
            CSearch cls = new CSearch();
            int[] ColumnWidth = new int[] { 100, 300 };
            FocusedControl = GetIndexFocusedControl();
            if (FocusedControl.Trim() == txtCostCenterID.Name)
            {

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtCostCenterID, lblCostCenterName, "CostCenterID", "رقم مركز التكلفة", Comon.cInt(cmbBranchesID.EditValue));
                else
                    PrepareSearchQuery.Find(ref cls, txtCostCenterID, lblCostCenterName, "CostCenterID", "Cost Center ID", Comon.cInt(cmbBranchesID.EditValue));

            }
            GetSelectedSearchValue(cls);
        }
        public void GetSelectedSearchValue(CSearch cls)
        {
            if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
            {
                if (FocusedControl == txtCostCenterID.Name)
                {
                    txtCostCenterID.Text = cls.PrimaryKeyValue.ToString();
                    txtCostCenterID_Validating(null, null);
                }
            }
        }
        public void ReadRecord()
        {
            try
            {
                IsNewRecord = false;
                ClearFields();
                    txtTaxableSales.Text = cClass.SupplierID.ToString();
                    txtZeroDomesticSales.Text = cClass.ArbName;
                    txtExports.Text = cClass.EngName;
                    txtTotalSales.Text = cClass.Mobile;
                    txtExemptSales.Text = cClass.Tel;
                    txtReverseArithmeticImport.Text = cClass.Address;
                    txtTaxablePurchases.Text = cClass.Fax;
                    txtZeroDomesticPurchases.Text = cClass.Notes;
                    txtImportTaxCustoms.Text = cClass.Email;
                    txtVAT.Text = cClass.VATID;
                    txtCooperationCouncilSales.Text = cClass.AccountID.ToString();
                
            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        public void ClearFields()
        {
            try
            {
                txtTaxableSales.Text = " ";
                txtZeroDomesticSales.Text = " ";
                txtExports.Text = " ";
                txtTotalSales.Text = " ";
                txtExemptSales.Text = " ";
                txtReverseArithmeticImport.Text = " ";
                txtTaxablePurchases.Text = " ";
                txtZeroDomesticPurchases.Text = " ";
                txtImportTaxCustoms.Text = "";
                txtVAT.Text = "";
                txtCooperationCouncilSales.Text = " ";
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        /******************** MoveRec ************************/
        public void MoveRec(long PremaryKeyValue, int Direction)
        {

        }
        /*******************Do Functions *************************/
        protected override void DoNew()
        {
            try
            {

                IsNewRecord = true;
                ClearFields();
                txtZeroDomesticSales.Focus();

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        protected override void DoLast()
        {

        }

        protected override void DoFirst()
        {

        }

        protected override void DoNext()
        {

        }

        protected override void DoPrevious()
        {

        }
        protected override void DoSearch()
        {
            try
            {
                Find();
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        protected override void DoSave()
        {

        }

        protected override void DoDelete()
        {

        }
        protected override void DoPrint()
        {



        }

        #endregion


        #region Event
        private void txtSupplierID_Validating(object sender, CancelEventArgs e)
        {

        }


        private void txtSupplierID_EditValueChanged(object sender, EventArgs e)
        {

        }
        private void txtArbName_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void GridView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {


        }
        private void txtArbName_EditValueChanged_1(object sender, EventArgs e)
        {
            ((TextEdit)sender).Properties.Appearance.BorderColor = Color.Black;

        }
        private void GridView_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {

        }

        #endregion

        private void txtArbName_Validating(object sender, CancelEventArgs e)
        {
        }
        private void txtEngName_Validating(object sender, CancelEventArgs e)
        {
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtBankName_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void frmSuppliers_Load(object sender, EventArgs e)
        {

            DoNew();
            InitializeFormatDate(txtFromDate);
            InitializeFormatDate(txtToDate);
            txtToDate.EditValue = DateTime.Now;
            txtFromDate.EditValue = DateTime.Now.AddMonths(-3);

            FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", "ArbName", "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));

            cmbBranchesID.EditValue = MySession.GlobalBranchID;
            cmbBranchesID.ReadOnly = !MySession.GlobalAllowBranchModificationAllScreens;

            //ribbonControl1.Visible = false;
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            try
            {


            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }


        private void txtEmail_EditValueChanged(object sender, EventArgs e)
        {
            ((TextEdit)sender).Properties.Appearance.BorderColor = Color.Black;

        }


        private bool EmailAddressChecker(string emailAddress)
        {

            string regExPattern = "^[_a-z0-9-]+(.[a-z0-9-]+)@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})$";
            bool emailAddressMatch = Match.Equals(emailAddress, regExPattern);

            return emailAddressMatch;
        }

        private void btnImbort_Click(object sender, EventArgs e)
        {


        }

        private void btnShow_Click(object sender, EventArgs e)
        {

            string filter = ".TotalSalPeroid.FacilityID >0  AND";
            strSQL = "";
            long FromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text));
            long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));
            long BranchesID = Comon.cLong(Comon.cInt(cmbBranchesID.EditValue));
            DataTable dt;
            if (BranchesID > 0)
                filter = filter + " .TotalSalPeroid.BranchID =" + BranchesID + " AND ";
            // حسب التاريخ
            if (FromDate != 0)
                filter = filter + " .TotalSalPeroid.InvoiceDate >=" + FromDate + " AND ";
            if (ToDate != 0)
                filter = filter + " .TotalSalPeroid.InvoiceDate <=" + ToDate + " AND ";

            if (txtCostCenterID.Text != string.Empty)
                filter = filter + " .TotalSalPeroid.CostCenterID  =" + Comon.cInt(txtCostCenterID.Text) + "  AND ";
            // '''''''''''''
            //المبيعات الغير معفاه
            filter = filter.Remove(filter.Length - 4, 4);
            strSQL = "Select sum(Total) AS Total From TotalSalPeroid Where AdditionalValue>0 And " + filter;
            dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                txtTaxableSales.Text = dt.Rows[0]["Total"].ToString();

            textEdit15.Text= Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice( txtTaxableSales.Text) /100* MySession.GlobalPercentVat).ToString();

            //مبيعات معفاه
            strSQL = "Select sum(Total) AS Total From TotalSalPeroid Where AdditionalValue=0 And " + filter;
            dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
            {
                txtExemptSales.Text = Comon.ConvertToDecimalPrice(dt.Rows[0]["Total"].ToString()).ToString();
            }
            ///////////////////////////
            ///مردودات المبيعات
            txtTotalSalesReturn.Text = "0";
            txtAmountSaleReturnTaxTotal.Text  = "0";

             filter = ".TotalSalReturnPeroid.FacilityID >0  AND";

            if (BranchesID > 0)
                filter = filter + " .TotalSalReturnPeroid.BranchID =" + BranchesID + " AND ";
            // حسب التاريخ
            if (FromDate != 0)
                filter = filter + " .TotalSalReturnPeroid.InvoiceDate >=" + FromDate + " AND ";
            if (ToDate != 0)
                filter = filter + " .TotalSalReturnPeroid.InvoiceDate <=" + ToDate + " AND ";

            if (txtCostCenterID.Text != string.Empty)
                filter = filter + " .TotalSalReturnPeroid.CostCenterID  =" + Comon.cInt(txtCostCenterID.Text) + "  AND ";
            // '''''''''''''
            //المردودات الغير معفاه
            filter = filter.Remove(filter.Length - 4, 4);
            strSQL = "Select sum(Total) AS Total From TotalSalReturnPeroid Where AdditionalValue>0 And " + filter;
            dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
            {
                txtTotalSalesReturn.Text = Comon.ConvertToDecimalPrice(dt.Rows[0]["Total"].ToString()).ToString();
                txtAmountSaleReturnTaxTotal.Text = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(txtTotalSalesReturn.Text) / 100 * MySession.GlobalPercentVat).ToString();
            }

            //المردودات  معفاه
            strSQL = "Select sum(Total) AS Total From TotalSalReturnPeroid Where AdditionalValue=0 And " + filter;
            dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
            {
                txtTotalSalesReturn24.Text = Comon.ConvertToDecimalPrice(dt.Rows[0]["Total"].ToString()).ToString();
                txtAmountSaleReturnTaxTotal24.Text = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(txtTotalSalesReturn24.Text) / 100 * MySession.GlobalPercentVat).ToString();
            }

            //////////////////////////////
            txtTotalSalesReturn.Text = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(txtTotalSalesReturn.Text) - Comon.ConvertToDecimalPrice(txtCooperationCouncilSales.Text)).ToString();
            txtAmountSaleReturnTaxTotal.Text = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(txtTotalSalesReturn.Text) / 100 * MySession.GlobalPercentVat).ToString();
            ////////////////////// PURCHASE



            //////////////////////////////
            txtTotalSales.Text = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(txtTaxableSales.Text) + Comon.ConvertToDecimalPrice(txtCooperationCouncilSales.Text)).ToString();
            txtAmountSaleTaxTotal.Text = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(txtTotalSales.Text) / 100 * MySession.GlobalPercentVat).ToString();
            ////////////////////// PURCHASE
            ///




            filter = ".View_PurchaseTax.FacilityID >0  AND";
            if (BranchesID > 0)
                filter = filter + " .View_PurchaseTax.BranchID =" + BranchesID + " AND ";
            // حسب التاريخ
            if (FromDate != 0)
                filter = filter + " .View_PurchaseTax.InvoiceDate >=" + FromDate + " AND ";
            if (ToDate != 0)
                filter = filter + " .View_PurchaseTax.InvoiceDate <=" + ToDate + " AND ";
            if (txtCostCenterID.Text != string.Empty)
                filter = filter + " .View_PurchaseTax.CostCenterID  =" + Comon.cInt(txtCostCenterID.Text) + "  AND ";
            // '''''''''''''المشتريات الغير معفاه
            filter = filter.Remove(filter.Length - 4, 4);
            strSQL = "Select sum(Total) AS Total, sum(AdditionalValue) AS AdditionalValue  From View_PurchaseTax Where AdditionalValue>0 And " + filter;
            dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
            {
                txtTaxablePurchases.Text = Comon.ConvertToDecimalPrice(dt.Rows[0]["Total"].ToString()).ToString();
                txtTotalPurchases.Text = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(txtTaxablePurchases.Text) + Comon.ConvertToDecimalPrice(txtImportTaxCustoms.Text) + Comon.ConvertToDecimalPrice(txtReverseArithmeticImport.Text)).ToString();
                txtAmountPurchseTaxTotal.Text = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(txtTotalPurchases.Text) / 100 * MySession.GlobalPercentVat).ToString();
              
            }
            //المشتريات المعفاه
            strSQL = "Select sum(Total) AS Total From View_PurchaseTax Where AdditionalValue=0 And " + filter;
            dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
            {
                txtExemptPurchases.Text = Comon.ConvertToDecimalPrice(dt.Rows[0]["Total"].ToString()).ToString();
            }
            //صافي ضريبة المبيعات
            txtNetTax.Text = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(txtAmountSaleTaxTotal.Text) - Comon.ConvertToDecimalPrice(txtAmountSaleReturnTaxTotal.Text)).ToString();
            //صافي الضريبية الكلية
            txtVAT.Text = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(txtNetTax.Text) - Comon.ConvertToDecimalPrice(txtAmountPurchseTaxTotal.Text)).ToString();
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

        }

        private void txtCostCenterID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT ArbName as CostCenterName FROM Acc_CostCenters WHERE CostCenterID =" + Comon.cInt(txtCostCenterID.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                CSearch.ControlValidating(txtCostCenterID, lblCostCenterName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void btnCostCenterSearch_Click(object sender, EventArgs e)
        {
            txtCostCenterID.Focus();
            Find();
        }

        private void cmbBranchesID_EditValueChanged(object sender, EventArgs e)
        {
            txtCostCenterID.Text = "";
            lblCostCenterName.Text = "";
        }
    }

}


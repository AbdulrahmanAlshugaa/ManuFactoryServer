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
using DevExpress.XtraSplashScreen;
using Edex.Model;
using System.IO;
using Edex.DAL.SalseSystem;
using System.Globalization;
using DevExpress.XtraGrid.Menu;
using Edex.GeneralObjects.GeneralClasses;
using Edex.ModelSystem;
using DevExpress.XtraEditors.Repository;
using Edex.Model.Language;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;
using Edex.DAL.Stc_itemDAL;
using DevExpress.XtraGrid;
using Edex.SalesAndPurchaseObjects.Codes;
using Edex.StockObjects.Codes;
using Edex.AccountsObjects.Codes;
using System.Data.OleDb;
using Edex.StockObjects.StoresClasses;
using DevExpress.XtraReports.UI;
using System.Data.SqlClient;
using Edex.DAL;
using Edex.SalesAndSaleObjects.Transactions;

namespace Edex.SalesAndPurchaseObjects.Transactions
{
    
    public partial class frmCashierSalesOrder : BaseForm
    {

        CompanyHeader cmpheader = new CompanyHeader();
        public int DiscountCustomer = 0;
        #region Declare
        public const int DocumentType = 4;
        int Caliber = 18;
        bool IdPrint = false;
        string MethodName = "";
        string invoiceNo = " ";
        int MethodID = 0;
        DataTable dtDeclaration;
        int flagError = 0;
        DataTable dtSize;
        string barcodeLast = "";
        int rowIndex;
        public string strQty = "";
        string QualityCasher;
        string FocusedControl = "";
        private string strSQL;
        private string PrimaryName;
        private string ItemName;
        private string SizeName;
        private string GroupName;
        private string CaptionBarCode;
        private string CaptionItemID;
        private string CaptionItemName;
        private string CaptionSizeID;
        private string CaptionSizeName;
        private string CaptionExpiryDate;
        private string CaptionQTY;
        private string CaptionTotal;
        private string CaptionDiscount;
        private string CaptionAdditionalValue;
        private string CaptionNet;
        private string CaptionSalePrice;
        private string CaptionCartPrice;
        private string CaptionCostPrice;
        private string CaptionSpendPrice;

        private string CaptionDescription;
        private string CaptionHavVat;
        private string CaptionRemainQty;
        DataTable dVat = new DataTable();
        public MemoryStream TheImage;
        private bool IsNewRecord;
        private Sales_SalesOrderDAL cClass;
        public const int xMoveFirst = 7;
        public const int xMovePrev = 8;
        public const int xMoveNext = 9;
        public const int xMoveLast = 10;
        public bool HasColumnErrors = false;
        Boolean StopSomeCode = false;
        bool falgPrint = false;
        public decimal QtyItem = 0;
        public CultureInfo culture = new CultureInfo("en-US");
        OpenFileDialog OpenFileDialog1 = null;
        DataTable dt = new DataTable();
        GridViewMenu menu;
        int ItemIDImage = 0;
        public int GoldUsing = 3;
        public string Typevat = "1";
        int StoreItemID = 1;
        //all record master and detail
        BindingList<Sales_SalesInvoiceDetails> AllRecords = new BindingList<Sales_SalesInvoiceDetails>();

        //list detail
        BindingList<Sales_SalesInvoiceDetails> lstDetail = new BindingList<Sales_SalesInvoiceDetails>();

        //Detail
        Sales_SalesInvoiceDetails BoDetail = new Sales_SalesInvoiceDetails();
        string VAt = "Select CompanyVATID from  VATIDCOMPANY ";

        #endregion



        public frmCashierSalesOrder()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);

                InitializeComponent();

                GroupName = "ArbGroupName";
                ItemName = "ArbItemName";
                SizeName = "ArbSizeName";
                PrimaryName = "ArbName";
                CaptionBarCode = "الباركود";
                CaptionItemID = "رقم الصنف";
                CaptionItemName = "اسم الصنف";
                CaptionSizeID = "رقم الوحدة";
                CaptionSizeName = "اسم الوحدة ";
                CaptionExpiryDate = "تاريخ الصلاحية";
                CaptionQTY = "الوزن";
                CaptionTotal = "الإجمالي";
                CaptionDiscount = "الخصم";
                CaptionAdditionalValue = "الضريبة";
                CaptionNet = "الصافي";

                CaptionCartPrice = "سعر الكرت";
                CaptionCostPrice = "سعر تكلفة";
                CaptionSalePrice = " سعر الوحدة ";
                CaptionSpendPrice = "تكلفة وأجور";

                CaptionDescription = "البيان";
                CaptionHavVat = "عليه ضريبة";
                CaptionRemainQty = "الكمية المتبقية";

                lblNetBalance.BackColor = Color.WhiteSmoke;
                // lblNetBalance.ForeColor = Color.Black;
                strSQL = "ArbName";
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, "Arb");
                if (UserInfo.Language == iLanguage.English)
                {
                    GroupName = "EngGroupName";
                    ItemName = "EngItemName";
                    SizeName = "EngSizeName";
                    PrimaryName = "EngName";
                    CaptionBarCode = "Bar Code";
                    CaptionItemID = "Item ID";
                    CaptionItemName = "ItemName";
                    CaptionSizeID = "Size ID ";
                    CaptionSizeName = "Size Name";
                    CaptionExpiryDate = "Expiry Date";
                    CaptionQTY = "Quantity";
                    CaptionTotal = "Total";
                    CaptionDiscount = "Discount";
                    CaptionAdditionalValue = "Additional Value";
                    CaptionNet = "Net";
                    CaptionSalePrice = "Sale Price";
                    CaptionDescription = "Description";
                    CaptionHavVat = "Has VAT";
                    CaptionRemainQty = "Quantity Remaining";
                    Lip.ConvertStrSQLToEnglishOrArabicLanguage(PrimaryName, "Eng");


               
                    labelControl34.Text = labelControl34.Tag.ToString();
                    labelControl25.Text = labelControl25.Tag.ToString();
                    labelControl26.Text = labelControl26.Tag.ToString();
                    labelControl27.Text = labelControl27.Tag.ToString();
                    labelControl7.Text = labelControl7.Tag.ToString();

                }
                InitGrid();
                /*********************** Fill Data ComboBox  ****************************/

                FillCombo.FillComboBox(cmbCurency, "Acc_Currency", "ID", strSQL, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));

                // FillCombo.FillComboBox(cmbBank, "[Acc_Banks]", "ID", PrimaryName, "", " 1=1 ", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
                /***********************Component ReadOnly  ****************************/
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("NO", typeof(string)));
                dt.Columns.Add(new DataColumn("Name", typeof(string)));
                DataRow row;
                row = dt.NewRow();
                row["NO"] = 0;
                row["Name"] = "---";
                dt.Rows.Add(row);
                row = dt.NewRow();
                row["NO"] = 1;
                row["Name"] = "عربي";
                dt.Rows.Add(row);
                row = dt.NewRow();
                row["NO"] = 2;
                row["Name"] = "English ";
                dt.Rows.Add(row);
                row = dt.NewRow();
                row["NO"] = 3;
                row["Name"] = "عربي-English";
                dt.Rows.Add(row);

                cmbLanguagePrint.Properties.DataSource = dt.DefaultView;
                cmbLanguagePrint.Properties.DisplayMember = "Name";
                cmbLanguagePrint.Properties.ValueMember = "NO";

                cmbLanguagePrint.Properties.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Optimistic;
                cmbLanguagePrint.EditValue = 1;
                TextEdit[] txtEdit = new TextEdit[4];
                txtEdit[0] = lblStoreName;
                txtEdit[1] = lblStoreName;

                txtEdit[2] = lblDiscountDebitAccountName;

                txtEdit[3] = lblSellerName;
                //foreach (TextEdit item in txtEdit)
                //{
                //    item.ReadOnly = true;
                //    item.Enabled = false;
                //    item.Properties.AppearanceDisabled.ForeColor = Color.Black;
                //    item.Properties.AppearanceDisabled.BackColor = Color.WhiteSmoke;
                //}
                /*********************** Date Format dd/MM/yyyy ****************************/
                InitializeFormatDate(txtInvoiceDate);


                /************************  Form Printing ***************************************/
                cmbFormPrinting.EditValue = Comon.cInt(MySession.GlobalDefaultSaleFormPrintingID);
                /*********************** Roles From ****************************/
                txtInvoiceDate.ReadOnly = !MySession.GlobalAllowChangefrmOrderSaleDate;
                txtStoreID.ReadOnly = !MySession.GlobalAllowChangefrmOrderSaleStoreID;
                txtCostCenterID.ReadOnly = !MySession.GlobalAllowChangefrmOrderSaleCostCenterID;
                cmbCurency.ReadOnly = !MySession.GlobalAllowChangefrmOrderSaleCurencyID;
                txtSellerID.ReadOnly = !MySession.GlobalAllowChangefrmOrderSaleSellerID;
                txtCustomerID.ReadOnly = !MySession.GlobalAllowChangefrmOrderSaleCustomerID;
                
                /************TextEdit Account ID ***************/

                /************ Button Search Account ID ***************/
                RolesButtonSearchAccountID();
                /********************* Event For Account Component ****************************/

                this.btnDebitSearch.Click += new System.EventHandler(this.btnDebitSearch_Click);






                /********************* Event For TextEdit Component **************************/
                if (MySession.GlobalAllowWhenEnterOpenPopup)
                {
                    this.txtInvoiceDate.Enter += new System.EventHandler(this.PublicTextEdit_Enter);

                }
                if (MySession.GlobalAllowWhenClickOpenPopup)
                {
                    this.txtInvoiceDate.Click += new System.EventHandler(this.PublicTextEdit_Click);

                }


                this.txtOrderID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.txtStoreID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.txtCostCenterID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);



                this.txtDiscountOnTotal.Validating += new System.ComponentModel.CancelEventHandler(this.txtDiscountOnTotal_Validating);
                this.txtDiscountPercent.Validating += new System.ComponentModel.CancelEventHandler(this.txtDiscountPercent_Validating);
                this.txtOrderID.Validating += new System.ComponentModel.CancelEventHandler(this.txtOrderID_Validating);
                this.txtStoreID.Validating += new System.ComponentModel.CancelEventHandler(this.txtStoreID_Validating);

                this.txtSellerID.Validating += new System.ComponentModel.CancelEventHandler(this.txtSellerID_Validating);

                this.txtDiscountOnTotal.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.txtDiscountPercent.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);


                /***************************** Event For GridView *****************************/
                this.KeyPreview = true;
                this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSaleInvoice_KeyDown);
                this.gridControl.ProcessGridKey += new System.Windows.Forms.KeyEventHandler(this.gridControl_ProcessGridKey);
                this.gridView1.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.gridView1_InitNewRow);
                this.gridView1.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView1_FocusedRowChanged);
                this.gridView1.FocusedColumnChanged += new DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventHandler(this.gridView1_FocusedColumnChanged);
                this.gridView1.CellValueChanging += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridView1_CellValueChanging);
                this.gridView1.ShownEditor += new System.EventHandler(this.gridView1_ShownEditor);
                this.gridView1.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.gridView1_ValidatingEditor);
                this.gridView1.InvalidRowException += new DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventHandler(this.gridView1_InvalidRowException);
                this.gridView1.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.gridView1_ValidateRow);
                this.gridView1.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gridView1_CustomUnboundColumnData);
                this.gridView1.RowUpdated += gridView1_RowUpdated;
                this.gridView1.PopupMenuShowing += gridView1_PopupMenuShowing;
                /******************************************/


                FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", "ArbName", "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
                cmbBranchesID.EditValue =MySession.GlobalBranchID;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[10].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[11].Visible = false;

                DoNew();

                SplashScreenManager.CloseForm(false);
                string startupPath = Directory.GetCurrentDirectory() + "\\";
                var Type = new FileStream(@startupPath + "typevat.txt", FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(Type, Encoding.UTF8))
                {
                    Typevat = streamReader.ReadToEnd();
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

        void gridView1_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            CalculateRow();
        }
        #region GridView
        void InitGrid()
        {
            lstDetail = new BindingList<Sales_SalesInvoiceDetails>();
            lstDetail.AllowNew = true;
            lstDetail.AllowEdit = true;
            lstDetail.AllowRemove = true;
            gridControl.DataSource = lstDetail;

            /******************* Columns Visible=false ********************/
            gridView1.Columns["BranchID"].Visible = false;
            gridView1.Columns["PackingQty"].Visible = false;
            gridView1.Columns["BAGET_W"].Visible = false;
            gridView1.Columns["QTY"].Visible = true;
            gridView1.Columns["STONE_W"].Visible = false;
            gridView1.Columns["DIAMOND_W"].Visible = false;
            gridView1.Columns["Equivalen"].Visible = false;
            gridView1.Columns["Caliber"].Visible = false;
            gridView1.Columns["CostPrice"].Visible = false;
            gridView1.Columns["ExpiryDateStr"].Visible = false;
            gridView1.Columns["Bones"].Visible = false;
            gridView1.Columns["Height"].Visible = false;
            gridView1.Columns["Width"].Visible = false;
            gridView1.Columns["TheCount"].Visible = false;
            gridView1.Columns["ItemImage"].Visible = false;

            gridView1.Columns["Serials"].Visible = false;
            gridView1.Columns["InvoiceID"].Visible = false;
            gridView1.Columns["ID"].Visible = false;
            gridView1.Columns["FacilityID"].Visible = false;
            gridView1.Columns["StoreID"].Visible = false;
            gridView1.Columns["Cancel"].Visible = false;
            gridView1.Columns["SaleMaster"].Visible = false;
            gridView1.Columns["ArbItemName"].Visible = gridView1.Columns["ArbItemName"].Name == "col" + ItemName ? true : false;
            gridView1.Columns["EngItemName"].Visible = gridView1.Columns["EngItemName"].Name == "col" + ItemName ? true : false;
            gridView1.Columns["ArbSizeName"].Visible = gridView1.Columns["ArbSizeName"].Name == "col" + SizeName ? true : false;
            gridView1.Columns["EngSizeName"].Visible = gridView1.Columns["EngSizeName"].Name == "col" + SizeName ? true : false;
            gridView1.Columns["BarCode"].Visible = true;
            gridView1.Columns["ExpiryDate"].Visible = false;
            gridView1.Columns["Description"].Visible = false;

            gridView1.Columns["DateFirst"].Visible = false;
            gridView1.Columns["ExpiryDateStr"].Visible = false;
            gridView1.Columns["ItemImage"].Visible = false;
            gridView1.Columns["DateFirstStr"].Visible = false;


            gridView1.Columns["CaratPrice"].Visible = false;
            gridView1.Columns["SpendPrice"].Visible = false;
            gridView1.Columns["GroupID"].Visible = false;
            gridView1.Columns["ArbGroupName"].Visible = false;
            gridView1.Columns["EngGroupName"].Visible = false;
            gridView1.Columns["Total"].Visible = false;


            /******************* Columns Visible=true *******************/

            gridView1.Columns[ItemName].Visible = true;
            gridView1.Columns[SizeName].Visible = true;
            gridView1.Columns["SizeID"].Visible = false;
            gridView1.Columns["Discount"].Visible = false;
            gridView1.Columns["HavVat"].Visible = false;
            gridView1.Columns["RemainQty"].Visible = false;
            gridView1.Columns["ItemID"].Visible = false;

            if (MySession.GlobalHaveVat != "1")
                gridView1.Columns["AdditionalValue"].Visible = false;
            else
                gridView1.Columns["AdditionalValue"].Visible = true;

            gridView1.Columns["BarCode"].Caption = CaptionBarCode;
            gridView1.Columns["ItemID"].Caption = CaptionItemID;
            gridView1.Columns[ItemName].Caption = CaptionItemName;
            gridView1.Columns[ItemName].Width = 100;
            gridView1.Columns["Description"].Width = 180;

            gridView1.Columns["SizeID"].Caption = CaptionSizeID;
            gridView1.Columns[SizeName].Caption = CaptionSizeName;
            gridView1.Columns["ExpiryDate"].Caption = CaptionExpiryDate;
            gridView1.Columns["QTY"].Caption = CaptionQTY;
            gridView1.Columns["STONE_W"].Caption = "وزن الأحجار";
            gridView1.Columns["BAGET_W"].Caption = "وزن الباجيت";
            gridView1.Columns["DIAMOND_W"].Caption = "وزن الألماس";
            gridView1.Columns["Total"].Caption = CaptionTotal;
            gridView1.Columns["Discount"].Caption = CaptionDiscount;
            gridView1.Columns["AdditionalValue"].Caption = CaptionAdditionalValue;
            gridView1.Columns["Net"].Caption = CaptionNet;
            gridView1.Columns["SalePrice"].Caption = CaptionSalePrice;
            gridView1.Columns["Description"].Caption = CaptionDescription;
            gridView1.Columns["HavVat"].Caption = CaptionHavVat;
            gridView1.Columns["RemainQty"].Caption = CaptionRemainQty;

            gridView1.Columns["Color"].Caption = "اللون";
            gridView1.Columns["CLARITY"].Caption = "النقاء";
            gridView1.Columns["Color"].Visible = false;
            gridView1.Columns["CLARITY"].Visible = false;
            gridView1.Columns["CurrencyID"].Visible = false;
            gridView1.Columns["CurrencyPrice"].Visible = false;
            gridView1.Columns["CurrencyName"].Visible = false;
            gridView1.Columns["CurrencyEquivalent"].Visible = false;
            gridView1.Columns["CurrencyEquivalent"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["CurrencyEquivalent"].OptionsColumn.AllowFocus = false;
            gridView1.Columns["CurrencyEquivalent"].VisibleIndex = gridView1.Columns["Net"].VisibleIndex + 1;
            DataTable dtCurrncy = Lip.SelectRecord("SELECT " + PrimaryName + " FROM Acc_Currency where Cancel=0   and BranchID=" + MySession.GlobalBranchID);
            string[] CurrncyName = new string[dtCurrncy.Rows.Count];
            for (int i = 0; i <= dtCurrncy.Rows.Count - 1; i++)
                CurrncyName[i] = dtCurrncy.Rows[i]["ArbName"].ToString();
            RepositoryItemComboBox riComboBoxitems1 = new RepositoryItemComboBox();
            riComboBoxitems1.Items.AddRange(CurrncyName);
            gridControl.RepositoryItems.Add(riComboBoxitems1);
            gridView1.Columns["CurrencyName"].ColumnEdit = riComboBoxitems1;
            gridView1.Columns["CurrencyPrice"].Caption = "سعر العملة";
            gridView1.Columns["CurrencyID"].Caption = "رقم العملة";
            gridView1.Columns["CurrencyName"].Caption = "اسم العملة";
            gridView1.Columns["CurrencyEquivalent"].Caption = "المقابل بالعملة المحلية";
            if (UserInfo.Language == iLanguage.English)
            {
                gridView1.Columns["Calipar"].Caption = "Calipar";
                gridView1.Columns["CurrencyPrice"].Caption = "Currency Price  ";
                gridView1.Columns["CurrencyID"].Caption = "Currency ID  ";
                gridView1.Columns["CurrencyName"].Caption = "Currency Name";
                gridView1.Columns["CurrencyEquivalent"].Caption = "Currency Equivalent";
            }
            gridView1.Focus();
            /*************************Columns Properties ****************************/
            gridView1.Columns["SalePrice"].OptionsColumn.ReadOnly = !MySession.GlobalCanChangeInvoicePrice;
            gridView1.Columns["Total"].OptionsColumn.ReadOnly = true;
            gridView1.Columns["Total"].OptionsColumn.AllowFocus = false;
            // gridView1.Columns["Net"].OptionsColumn.ReadOnly = !MySession.GlobalAllowChangefrmSaleInvoiceNetPrice;
            // gridView1.Columns["Net"].OptionsColumn.AllowFocus = MySession.GlobalAllowChangefrmSaleInvoiceNetPrice;
            gridView1.Columns["AdditionalValue"].OptionsColumn.ReadOnly = true;
            gridView1.Columns["AdditionalValue"].OptionsColumn.AllowFocus = false;
            /************************ Date Time **************************/

            RepositoryItemDateEdit RepositoryDateEdit = new RepositoryItemDateEdit();
            RepositoryDateEdit.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime;
            RepositoryDateEdit.Mask.EditMask = "dd/MM/yyyy";
            RepositoryDateEdit.Mask.UseMaskAsDisplayFormat = true;
            gridControl.RepositoryItems.Add(RepositoryDateEdit);
            gridView1.Columns["ExpiryDate"].ColumnEdit = RepositoryDateEdit;
            gridView1.Columns["ExpiryDate"].UnboundType = DevExpress.Data.UnboundColumnType.DateTime;
            gridView1.Columns["ExpiryDate"].DisplayFormat.FormatString = "dd/MM/yyyy";
            gridView1.Columns["ExpiryDate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            gridView1.Columns["ExpiryDate"].OptionsColumn.AllowEdit = true;
            gridView1.Columns["ExpiryDate"].OptionsColumn.ReadOnly = false;


            /************************ Look Up Edit **************************/
            /////////////////////////Item COLOR
            ///
            DataTable dtitems = Lip.SelectRecord("SELECT   ArbName   FROM Stc_ItemsColors where BranchID=" + MySession.GlobalBranchID);
            string[] companiesitems = new string[dtitems.Rows.Count];
            for (int i = 0; i <= dtitems.Rows.Count - 1; i++)
                companiesitems[i] = dtitems.Rows[i]["ArbName"].ToString();

            RepositoryItemComboBox riComboBoxitems = new RepositoryItemComboBox();
            riComboBoxitems.Items.AddRange(companiesitems);

            gridControl.RepositoryItems.Add(riComboBoxitems);
            gridView1.Columns["Color"].ColumnEdit = riComboBoxitems;
            /////////////////////////


            /////////////////////////Item CLARITY
            DataTable dtitemsCLARITY = Lip.SelectRecord("SELECT   ArbName   FROM Stc_ItemsSizes where BranchID=" + MySession.GlobalBranchID);
            string[] companiesitemsCLARITY = new string[dtitemsCLARITY.Rows.Count];
            for (int i = 0; i <= dtitemsCLARITY.Rows.Count - 1; i++)
                companiesitemsCLARITY[i] = dtitemsCLARITY.Rows[i]["ArbName"].ToString();

            RepositoryItemComboBox riComboBoxitemsCLARITY = new RepositoryItemComboBox();
            riComboBoxitemsCLARITY.Items.AddRange(companiesitemsCLARITY);

            gridControl.RepositoryItems.Add(riComboBoxitemsCLARITY);
            gridView1.Columns["CLARITY"].ColumnEdit = riComboBoxitemsCLARITY;
            /////////////////////////

            /////////////////////////Description
            DataTable dt = Lip.SelectRecord("SELECT ArbName FROM Stc_ItemsGroups WHERE Cancel=0 and BranchID=" + MySession.GlobalBranchID);
            string[] companies = new string[dt.Rows.Count];
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
                companies[i] = dt.Rows[i]["ArbName"].ToString();

            RepositoryItemComboBox riComboBox = new RepositoryItemComboBox();
            riComboBox.Items.AddRange(companies);
            gridControl.RepositoryItems.Add(riComboBox);
            gridView1.Columns["Description"].ColumnEdit = riComboBox;
            ///////////////////////////
            gridView1.Columns["Description"].Width = 150;
            gridView1.Columns[ItemName].Width = 150;
            gridView1.Columns["SalePrice"].Width = 90;

            //RepositoryItemLookUpEdit rBarCode = Common.LookUpEditBarCode();
            //gridView1.Columns["BarCode"].ColumnEdit = rBarCode;
            //gridControl.RepositoryItems.Add(rBarCode);

            //RepositoryItemLookUpEdit rItemID = Common.LookUpEditItemID();
            //gridView1.Columns["ItemID"].ColumnEdit = rItemID;
            //gridControl.RepositoryItems.Add(rItemID);

            /************************ Auto Number **************************/
            DevExpress.XtraGrid.Columns.GridColumn col = gridView1.Columns.AddVisible("#");
            col.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            col.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            col.OptionsColumn.AllowEdit = false;
            col.OptionsColumn.ReadOnly = true;
            col.OptionsColumn.AllowFocus = false;
            col.Width = 20;
            gridView1.BestFitColumns();
            /******************************** Menu ***************************************/
            menu = new GridViewMenu(gridView1);
            menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("أسعار الصنف", new EventHandler(Price_Click)));
            menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("بيانات الصنف", new EventHandler(item_Click)));
            menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("كرت الصنف"));
            menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("باركود الصنف"));

        }
        private void item_Click(object sender, EventArgs e)
        {


        }

        private void Price_Click(object sender, EventArgs e)
        {
            frmItemPricesAndCosts frm = new frmItemPricesAndCosts();
            var ItemID = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ItemID");
            var SizeID = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "SizeID");
            frm.SizeID = Comon.cInt(SizeID);
            frm.ItemID = Comon.cLong(ItemID);

            frm.ShowDialog();
            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SalePrice"], Comon.ConvertToDecimalPrice(frm.CelValue));

        }

        private void gridView1_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            try
            {
                if (e.HitInfo != null && (e.HitInfo.Column.Name == "colSalePrice" || e.HitInfo.Column.Name == "colExpiryDate"))
                    if (e.HitInfo.HitTest == DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitTest.RowCell)
                        e.Menu = menu;
            }
            catch { }
        }
        private void gridView1_ShownEditor(object sender, EventArgs e)
        {



            //CalculateRow();
        }
        private void gridView1_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            try
            {
                if (!gridView1.IsLastVisibleRow)
                    gridView1.MoveLast();
                if (DiscountCustomer != 0)
                {
                    txtDiscountPercent.Text = DiscountCustomer.ToString();
                    txtDiscountPercent_Validating(null, null);
                }
                foreach (GridColumn col in gridView1.Columns)
                {
                    if (col.FieldName == "BarCode" || col.FieldName == "Net" || col.FieldName == "Total" || col.FieldName == "ItemID" || col.FieldName == "QTY" || col.FieldName == "SizeID" || col.FieldName == "SalePrice")
                    {

                        var val = gridView1.GetRowCellValue(e.RowHandle, col);
                        double num;
                        if (val == null || string.IsNullOrWhiteSpace(val.ToString()))
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            gridView1.SetColumnError(col, Messages.msgInputIsRequired);
                        }
                        else if (!(double.TryParse(val.ToString(), out num)) && col.FieldName != "BarCode")
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            gridView1.SetColumnError(col, Messages.msgInputShouldBeNumber);
                        }
                        else if (Comon.ConvertToDecimalPrice(val.ToString()) <= 0 && col.FieldName != "BarCode")
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            gridView1.SetColumnError(col, Messages.msgInputIsGreaterThanZero);
                        }
                        else
                        {
                            e.Valid = true;
                            gridView1.SetColumnError(col, "");
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void gridView1_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {

            if (this.gridView1.ActiveEditor is CheckEdit)
            {
                if (e.Value != null)
                {
                    gridView1.Columns["HavVat"].OptionsColumn.AllowEdit = true;
                    CalculateRow(gridView1.FocusedRowHandle, Comon.cbool(e.Value.ToString()));
                }
            }
            else if (this.gridView1.ActiveEditor is TextEdit)
            {
                GridView view = sender as GridView;
                double num;
                object val = e.Value;
                HasColumnErrors = false;

                string ColName = view.FocusedColumn.FieldName;

                if (ColName == "BarCode" || ColName == "Net" || ColName == "SizeID" || ColName == "Total" || ColName == "ItemID" || ColName == "QTY" || ColName == "SalePrice" || ColName == "CurrencyPrice")
                {
                    if (val == null || string.IsNullOrWhiteSpace(val.ToString()))
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgInputIsRequired;
                    }
                    else if (!(double.TryParse(val.ToString(), out num)) && ColName != "BarCode")
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgInputShouldBeNumber;
                    }
                    else if (Comon.ConvertToDecimalPrice(val.ToString()) <= 0 && ColName != "BarCode")
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgInputIsGreaterThanZero;
                    }
                    else
                    {
                        e.Valid = true;
                        view.SetColumnError(gridView1.Columns[ColName], "");

                    }
                    /****************************************/
                    //if (ColName == "SalePrice" && Comon.ConvertToDecimalPrice(val.ToString()) < Comon.ConvertToDecimalPrice(gridView1.GetFocusedRowCellValue("CostPrice").ToString()))
                    //{

                    //    e.Valid = false;
                    //    HasColumnErrors = true;
                    //    e.ErrorText = "لايمكن البيع باقل من سعر التكلفة ";
                    //    gridView1.SetFocusedRowCellValue("Net", 0.ToString());

                    //}
                    if (ColName == "SalePrice")
                    {
                        
                        bool HasVat = Comon.cbool(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["HavVat"]).ToString());
                        decimal QTY = Comon.ConvertToDecimalPrice(gridView1.GetFocusedRowCellValue("QTY"));

                        decimal SalePrice = Comon.ConvertToDecimalPrice(val.ToString());
                        decimal TotalSale = QTY * SalePrice;
                        decimal Discount = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Discount"));
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "Total", Comon.ConvertToDecimalPrice(TotalSale) - Comon.ConvertToDecimalPrice(Discount));
                        decimal Total = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Total"));
                        decimal additonalVAlue = 0;
                        if(MySession.GlobalHaveVat=="1")
                          additonalVAlue = Comon.ConvertToDecimalPrice((Total * MySession.GlobalPercentVat) / 100);
                        if (HasVat == true&&(MySession.GlobalHaveVat=="1"))
                            additonalVAlue = Comon.ConvertToDecimalPrice(((Total) * MySession.GlobalPercentVat) / 100);
                        else
                            additonalVAlue = 0;
                        gridView1.SetFocusedRowCellValue("AdditionalValue", additonalVAlue.ToString());
                        decimal Net = Comon.ConvertToDecimalPrice(Total + additonalVAlue);
                        gridView1.SetFocusedRowCellValue("Total", Net.ToString());
                        gridView1.SetFocusedRowCellValue("Net", Net.ToString());
                        CalculateRow();
                    }
                    if (ColName == "QTY")
                    {
                   
                        HasColumnErrors = false;
                        e.Valid = true;
                        gridView1.SetColumnError(gridView1.Columns["QTY"], "");
                        e.ErrorText = "";

                        decimal additonalVAlue = 0;

                        bool HasVat = Comon.cbool(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["HavVat"]).ToString());

                        decimal QTY = Comon.ConvertToDecimalPrice(val.ToString());

                        decimal SalePrice = Comon.ConvertToDecimalPrice(gridView1.GetFocusedRowCellValue("SalePrice"));

                        decimal TotalSale = Comon.ConvertToDecimalPrice(QTY * SalePrice);
                        decimal Discount = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Discount"));
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "Total", Comon.ConvertToDecimalPrice(TotalSale) - Comon.ConvertToDecimalPrice(Discount));

                        decimal Total = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Total"));
                        if (HasVat == true&&(MySession.GlobalHaveVat=="1"))
                            additonalVAlue = Comon.ConvertToDecimalPrice((Total * MySession.GlobalPercentVat) / 100);
                        else
                            additonalVAlue = 0;
                        gridView1.SetFocusedRowCellValue("AdditionalValue", additonalVAlue.ToString());
                        decimal Net = Comon.ConvertToDecimalPrice(Total + additonalVAlue);
                        gridView1.SetFocusedRowCellValue("Total", Net.ToString());
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Net"], Net);
                     
                    }
                    if (ColName == "CurrencyPrice")
                    {
                        if (Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Net")) > 0)
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CurrencyEquivalent", Comon.ConvertToDecimalPrice(Comon.cDec(e.Value) * Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Net"))).ToString());

                    }
                    if (ColName == "Net")
                    {
                        decimal additonalVAlue = 0;
                        if (Comon.ConvertToDecimalPrice(val.ToString()) > 0)
                        {
                            //    decimal additonalVAlue=(Comon.ConvertToDecimalPrice(val.ToString())*Comon.ConvertToDecimalPrice( 5))/100;
                            //    decimal Total = Comon.ConvertToDecimalPrice(val.ToString()) - additonalVAlue;
                            //    decimal PriceUnit = Total / Comon.ConvertToDecimalQty(gridView1.GetFocusedRowCellValue("QTY"));
                            decimal PriceUnit = 0;
                            bool HasVat = Comon.cbool(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["HavVat"]).ToString());
                            decimal Net = (Comon.ConvertToDecimalPrice(val.ToString()) + Comon.ConvertToDecimalPrice(gridView1.GetFocusedRowCellValue("Discount")));
                            if (HasVat == true)
                                additonalVAlue = Comon.ConvertToDecimalPrice(Net - ((Net * 100) / (100 + MySession.GlobalPercentVat)));
                            else
                                additonalVAlue = 0;
                            string BarCode = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BarCode"]).ToString();
                            if (BarCode == "24")
                                additonalVAlue = 0;
                            PriceUnit = Comon.ConvertToDecimalPrice(Net - additonalVAlue);
                            decimal CostPriceUnit = Comon.ConvertToDecimalPrice(gridView1.GetFocusedRowCellValue("CostPrice").ToString());
                            //if (Comon.ConvertToDecimalPrice(PriceUnit.ToString()) < CostPriceUnit)
                            //{

                            //   // e.Valid = false;
                            //   // HasColumnErrors = true;
                            //   // e.ErrorText = "لايمكن البيع باقل من سعر التكلفة ";
                            //    //gridView1.SetFocusedRowCellValue("Net", 0.ToString());

                            //}
                            //else
                            {
                                decimal Total = Comon.ConvertToDecimalPrice(Net) - additonalVAlue;
                                gridView1.SetFocusedRowCellValue("AdditionalValue", additonalVAlue.ToString());
                                gridView1.SetFocusedRowCellValue("SalePrice", PriceUnit.ToString());
                                gridView1.SetFocusedRowCellValue("Total", Total.ToString());
                                gridView1.SetFocusedRowCellValue("Net", val.ToString());
                            }
                        }
                    }
                    if (ColName == "BarCode")
                    {

                        DataTable dt;
                        var flagb = false;

                        dt = Stc_itemsDAL.GetItemData(val.ToString(), UserInfo.FacilityID);

                        if (dt.Rows.Count == 0)
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundThisBarCode;
                        }
                        else
                        {
                            //RepositoryItemLookUpEdit rSize = Common.LookUpEditSize(Comon.cInt(dt.Rows[0]["ItemID"].ToString()));
                            //gridView1.Columns[SizeName].ColumnEdit = rSize;
                            //gridControl.RepositoryItems.Add(rSize);

                            if (flagb == true)
                            {
                                MySession.GlobalAllowUsingDateItems = false;
                                FileItemData(dt);

                                MySession.GlobalAllowUsingDateItems = true;
                            }
                            else
                                FileItemData(dt);
                            if (HasColumnErrors == false)
                            {
                                e.Valid = true;
                                view.SetColumnError(gridView1.Columns[ColName], "");
                                gridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                                gridView1.FocusedColumn = gridView1.VisibleColumns[0];
                            }
                            //else {
                            //   // HasColumnErrors = true;
                            //    e.Valid = false;
                            //    gridView1.SetColumnError(gridView1.Columns["QTY"], "");
                            //    e.ErrorText = "الكمية غير متوفرة";
                            //    gridView1.FocusedColumn = gridView1.VisibleColumns[6];

                            //}
                        }

                    }
                    else if (ColName == "ItemID")
                    {
                        DataTable dt = Stc_itemsDAL.GetTopItemDataByItemID(Comon.cInt(val.ToString()), UserInfo.FacilityID);
                        if (dt.Rows.Count == 0)
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundThisBarCode;

                        }
                        else
                        {


                            //RepositoryItemLookUpEdit rSize = Common.LookUpEditSize(Comon.cInt(val.ToString()));
                            //gridView1.Columns[SizeName].ColumnEdit = rSize;
                            //gridControl.RepositoryItems.Add(rSize);
                            if (MySession.GlobalAllowUsingDateItems)
                            {
                                MySession.GlobalAllowUsingDateItems = false;
                                FileItemData(dt);
                                MySession.GlobalAllowUsingDateItems = true;
                            }
                            else
                                FileItemData(dt);
                            e.Valid = true;
                            view.SetColumnError(gridView1.Columns[ColName], "");
                        }

                    }
                    else if (ColName == "SizeID")
                    {

                        int ItemID = Comon.cInt(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemID"]).ToString());
                        DataTable dt = Stc_itemsDAL.GetItemDataByItemID_SizeID(ItemID, Comon.cInt(val.ToString()), UserInfo.FacilityID);

                        if (dt == null || dt.Rows.Count == 0)
                        {

                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundSizeForItem;
                            view.SetColumnError(gridView1.Columns[ColName], Messages.msgNoFoundThisItem);
                        }
                        else
                        {
                            //RepositoryItemLookUpEdit rSize = Common.LookUpEditSize(Comon.cInt(dt.Rows[0]["ItemID"].ToString()));
                            //gridView1.Columns[SizeName].ColumnEdit = rSize;
                            //gridControl.RepositoryItems.Add(rSize);
                            if (MySession.GlobalAllowUsingDateItems)
                            {
                                MySession.GlobalAllowUsingDateItems = false;
                                FileItemData(dt);
                                MySession.GlobalAllowUsingDateItems = true;
                            }
                            else
                                FileItemData(dt);
                            e.Valid = true;
                            view.SetColumnError(gridView1.Columns[ColName], "");
                        }
                    }
                }
                else if (ColName == ItemName)
                {
                    DataTable dtItemID = Lip.SelectRecord("Select ItemID from Stc_Items Where Cancel=0 and BranchID=" + MySession.GlobalBranchID+" And LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') And FacilityID=" + UserInfo.FacilityID);
                    if (dtItemID.Rows.Count > 0)
                    {
                        DataTable dtItem = Stc_itemsDAL.GetTopItemDataByItemID(Comon.cInt(dtItemID.Rows[0]["ItemID"].ToString()), UserInfo.FacilityID);
                        if (dtItem.Rows.Count == 0)
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundThisItem;
                            view.SetColumnError(gridView1.Columns[ColName], Messages.msgNoFoundThisItem);
                        }
                        else
                        {
                            //RepositoryItemLookUpEdit rSize = Common.LookUpEditSize(Comon.cInt(dtItemID.Rows[0]["ItemID"].ToString()));
                            //gridView1.Columns[SizeName].ColumnEdit = rSize;
                            //gridControl.RepositoryItems.Add(rSize);
                            if (MySession.GlobalAllowUsingDateItems)
                            {
                                MySession.GlobalAllowUsingDateItems = false;
                                FileItemData(dtItem);
                                MySession.GlobalAllowUsingDateItems = true;
                            }
                            else
                                FileItemData(dtItem);
                            e.Valid = true;
                            view.SetColumnError(gridView1.Columns[ColName], "");
                        }
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundThisItem;
                        view.SetColumnError(gridView1.Columns[ColName], Messages.msgNoFoundThisItem);
                    }
                }
                else if (ColName == SizeName)
                {
                    DataTable dtSize = Lip.SelectRecord("Select SizeID, " + PrimaryName + " AS " + SizeName + " from Stc_SizingUnits Where Cancel=0 and BranchID=" + MySession.GlobalBranchID+" And LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') And FacilityID=" + UserInfo.FacilityID);
                    if (dtSize.Rows.Count > 0)
                    {
                        var ItemID = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ItemID");
                        if (ItemID != null)
                        {
                            DataTable dt = Stc_itemsDAL.GetItemDataByItemID_SizeID(Comon.cInt(ItemID.ToString()), Comon.cInt(dtSize.Rows[0]["SizeID"].ToString()), UserInfo.FacilityID);
                            if (dt.Rows.Count == 0)
                            {
                                e.Valid = false;
                                HasColumnErrors = true;
                                e.ErrorText = Messages.msgNoFoundSizeForItem;
                            }
                            else
                            {
                                if (MySession.GlobalAllowUsingDateItems)
                                {
                                    MySession.GlobalAllowUsingDateItems = false;
                                    FileItemData(dt);
                                    MySession.GlobalAllowUsingDateItems = true;
                                }
                                else
                                    FileItemData(dt);
                                e.Valid = true;
                                view.SetColumnError(gridView1.Columns[ColName], "");
                            }
                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgInputIsRequired;
                            view.SetColumnError(gridView1.Columns["ItemID"], Messages.msgNoFoundSizeForItem);
                        }

                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundSizeForItem;
                        view.SetColumnError(gridView1.Columns[ColName], Messages.msgNoFoundSizeForItem);

                    }
                }
                else if (ColName == "Discount")
                {
                    decimal QTY = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "QTY").ToString());
                    decimal SalePrice = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "SalePrice").ToString());
                    decimal Total = QTY * SalePrice;
                    decimal PercentDiscount = Total * (MySession.GlobalDiscountPercentOnItem / 100);
                    if (!(double.TryParse(val.ToString(), out num)))
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgInputShouldBeNumber;
                    }
                    else if (Comon.ConvertToDecimalPrice(val.ToString()) > 0 && (MySession.GlobalDiscountPercentOnItem <= 0)) { Messages.MsgError(Messages.TitleError, Messages.msgNotAllowedPercentDiscount); return; }
                    else if (Comon.ConvertToDecimalPrice(val.ToString()) > PercentDiscount)
                    {
                        Messages.MsgWarning(Messages.TitleWorning, "لا يمكن ان يكون الخصم أكبر من النسبة المسموح بها للخصم على مستوى الصنف ");
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNotAllowedPercentDiscount;
                    }
                    else
                    {
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "Total", Comon.ConvertToDecimalPrice(Total) - Comon.ConvertToDecimalPrice(val.ToString()));
                        bool HasVat = Comon.cbool(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["HavVat"]).ToString());

                        Total = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Total"));
                        decimal additonalVAlue = 0;
                        if (MySession.GlobalHaveVat == "1")
                          additonalVAlue = Comon.ConvertToDecimalPrice((Total * MySession.GlobalPercentVat) / 100);
                        if (HasVat == true&&(MySession.GlobalHaveVat=="1"))
                            additonalVAlue = Comon.ConvertToDecimalPrice(((Total) * MySession.GlobalPercentVat) / 100);
                        else
                            additonalVAlue = 0;
                        gridView1.SetFocusedRowCellValue("AdditionalValue", additonalVAlue.ToString());


                        decimal Net = Comon.ConvertToDecimalPrice(Total + additonalVAlue);
                        gridView1.SetFocusedRowCellValue("Total", Net.ToString());

                    }
                    CalculateRow();
                }
                if (ColName == "CurrencyName")
                {
                    DataTable dt = Lip.SelectRecord("Select ID ,ExchangeRate from Acc_Currency Where Cancel=0 and BranchID=" + MySession.GlobalBranchID+" And LOWER (" + PrimaryName + ")=LOWER ('" + e.Value.ToString() + "')");
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CurrencyID", dt.Rows[0]["ID"]);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CurrencyPrice", dt.Rows[0]["ExchangeRate"]);
                    if (Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Net")) > 0)
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CurrencyEquivalent", Comon.ConvertToDecimalPrice(Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "CurrencyPrice")) * Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Net"))).ToString());
                }
            }
            CalculateRow();

        }
        private void gridControl_ProcessGridKey(object sender, KeyEventArgs e)
        {
            try
            {
                var grid = sender as GridControl;
                var view = grid.FocusedView as GridView;
                if (view.FocusedColumn == null)
                    return;



                if (e.KeyCode == Keys.Escape)
                {
                    HasColumnErrors = false;
                }
                if (e.KeyValue == 107)
                {
                    if (this.gridView1.ActiveEditor is CheckEdit)
                    {
                        view.SetFocusedValue(!Comon.cbool(view.GetFocusedValue()));
                        CalculateRow(gridView1.FocusedRowHandle, Comon.cbool(view.GetFocusedValue()));


                    }
                }
                else if (e.KeyCode == Keys.Tab || e.KeyCode == Keys.Enter)
                {
                    if (view.ActiveEditor is TextEdit)
                    {
                        if (HasColumnErrors == true)
                            return;
                        double num;
                        HasColumnErrors = false;
                        var cellValue = view.GetRowCellValue(view.FocusedRowHandle, view.FocusedColumn.FieldName);
                        string ColName = view.FocusedColumn.FieldName;
                        if (ColName == "BarCode" || ColName == "Net" || ColName == "Total" || ColName == "ItemID" || ColName == "QTY" || ColName == "SizeID" || ColName == "SalePrice")
                        {

                            if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                            {
                                HasColumnErrors = true;
                                view.SetColumnError(gridView1.Columns[ColName], Messages.msgInputIsRequired);

                            }
                            else if (!(double.TryParse(cellValue.ToString(), out num)) && ColName != "BarCode")
                            {

                                HasColumnErrors = true;
                                view.SetColumnError(gridView1.Columns[ColName], Messages.msgInputShouldBeNumber);
                            }
                            else if (Comon.ConvertToDecimalPrice(cellValue.ToString()) <= 0 && ColName != "BarCode")
                            {

                                HasColumnErrors = true;
                                view.SetColumnError(gridView1.Columns[ColName], Messages.msgInputIsGreaterThanZero);
                            }
                            else
                            {
                                view.SetColumnError(gridView1.Columns[ColName], "");
                            }

                            if (ColName == "QTY")
                            {
                                string BarCode = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BarCode"]).ToString();
                                double RemindQty = 0;
                                RemindQty = Comon.cDbl(Lip.SelectRecord("SELECT [dbo].[RemindQty]('" + BarCode + "'," + Comon.cInt(txtStoreID.Text) + ","+MySession.GlobalBranchID+") AS RemainQty").Rows[0]["RemainQty"].ToString());
                                double Qty = Comon.cDbl(QtyItem);

                                if (RemindQty <= 0)
                                {

                                    if (MySession.GlobalWayOfOutItems == "PreventOutItemsWithOutBalance")
                                    {

                                        HasColumnErrors = true;
                                        view.SetColumnError(gridView1.Columns[ColName], "الكمية  غير متوفرة ");
                                        // view.DeleteRow(view.FocusedRowHandle);
                                        return;
                                    }
                                }

                            }

                        }

                    }
                }
                else if (e.KeyData == Keys.Delete)
                {
                    if (!IsNewRecord)
                    {
                        if (!FormDelete)
                        {
                            Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToDeleteRecord);
                            return;
                        }
                        else
                        {
                            bool Yes = Messages.MsgStopYesNo(Messages.TitleConfirm, Messages.msgConfirmDelete);
                            if (!Yes)
                                return;
                        }
                    }
                    int index = view.FocusedRowHandle;

                    view.DeleteSelectedRows();
                    e.Handled = true;
                    if (index > 0)
                    {
                        if (index > 0)
                            index = index - 1;
                        else if (index < 0)
                        {
                            index = view.DataRowCount;
                            index = index - 1;
                        }
                        view.SelectRow(index);
                        view.FocusedRowHandle = index;
                    }
                    CalculateRow();
                }
                else if (e.KeyData == Keys.F5)
                    grid.ShowPrintPreview();

            }
            catch (Exception ex)
            {
                e.Handled = false;
                Messages.MsgError(Messages.TitleInfo, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void gridView1_InvalidRowException(object sender, DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
        }
        private void gridView1_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            e.Value = (e.ListSourceRowIndex + 1);
        }
        private void gridView1_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (this.gridView1.ActiveEditor is CheckEdit)
            {
                gridView1.Columns["HavVat"].OptionsColumn.AllowEdit = true;
                CalculateRow(gridView1.FocusedRowHandle, Comon.cbool(e.Value.ToString()));
            }
            if (this.gridView1.ActiveEditor is TextEdit)
            {
                GridView view = sender as GridView;
                string ColName = view.FocusedColumn.FieldName;
                if (ColName == "QTY")
                {
                    QtyItem = Comon.cDec(e.Value);
                    Caliber = Comon.cInt(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, SizeName).ToString());
                }
            }
        }
        private void gridView1_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e)
        {


        }
        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {


                if (e.FocusedRowHandle >= 0)
                {
                    ItemIDImage = Comon.cInt(gridView1.GetRowCellValue(e.FocusedRowHandle, "ItemID").ToString());
                    var dtimg = Lip.SelectRecord("Select * from Stc_Items Where   BranchID=" + MySession.GlobalBranchID+" and ItemID=" + ItemIDImage);
                    if (dtimg.Rows.Count > 0)
                    {
                        byte[] imgByte = null;
                        if (DBNull.Value != dtimg.Rows[0]["ItemImage"])
                        {
                            imgByte = (byte[])dtimg.Rows[0]["ItemImage"];
                            picItemImage.Image = byteArrayToImage(imgByte);
                        }
                        else
                            picItemImage.Image = null;
                    }
                }

            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleInfo, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

        }
        private void gridView1_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            rowIndex = e.RowHandle;
        }

        private void FileItemData(DataTable dt)
        {

            if (dt != null && dt.Rows.Count > 0)
            {
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["PackingQty"], "1");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BarCode"], dt.Rows[0]["BarCode"].ToString().ToUpper());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemID"], dt.Rows[0]["ItemID"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[ItemName], dt.Rows[0]["ArbName"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SizeID"], dt.Rows[0]["SizeID"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[SizeName], dt.Rows[0]["SizeName"].ToString());
                if (UserInfo.Language == iLanguage.English)
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["EngItemName"], dt.Rows[0]["ItemName"].ToString());
                if (UserInfo.Language == iLanguage.English)
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[SizeName], dt.Rows[0]["SizeName"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["EngItemName"], dt.Rows[0]["ItemName"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ExpiryDate"], Comon.ConvertSerialToDate(dt.Rows[0]["ExpiryDate"].ToString()));
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SalePrice"], dt.Rows[0]["SalePrice"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CostPrice"], dt.Rows[0]["CostPrice"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Color"], dt.Rows[0]["Color"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CLARITY"], dt.Rows[0]["CLARITY"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CurrencyName"], cmbCurency.Text.ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CurrencyPrice"], txtCurrncyPrice.Text);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CurrencyEquivalent"], Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(txtCurrncyPrice.Text) * Comon.ConvertToDecimalPrice(dt.Rows[0]["SalePrice"].ToString())));
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CurrencyID"], Comon.cInt(cmbCurency.EditValue));
                try
                {
                    if (DBNull.Value != dt.Rows[0]["ItemImage"])
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemImage"], dt.Rows[0]["ItemImage"]);
                }
                catch (Exception) { }

                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["StoreID"], txtStoreID.Text);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Bones"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Discount"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Bones"], 0);
                decimal AdditionalValue = 0;
                if (MySession.GlobalHaveVat == "1")
                  AdditionalValue = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(dt.Rows[0]["SalePrice"].ToString()) / 100 * MySession.GlobalPercentVat);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["AdditionalValue"], AdditionalValue);
                decimal Net = Comon.ConvertToDecimalPrice(AdditionalValue + Comon.ConvertToDecimalPrice(dt.Rows[0]["SalePrice"].ToString()));
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Cancel"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Serials"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Description"], dt.Rows[0]["ItemName"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["PageNo"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemStatus"], 1);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Caliber"], dt.Rows[0]["Caliber"]);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Equivalen"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Net"], Net);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["TheCount"], 1);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Height"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Width"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Total"], dt.Rows[0]["SalePrice"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["QTY"], dt.Rows[0]["QTY"]);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["HavVat"], true);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["RemainQty"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["DIAMOND_W"], dt.Rows[0]["DIAMOND_W"]);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["STONE_W"], dt.Rows[0]["STONE_W"]);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BAGET_W"], dt.Rows[0]["BAGET_W"]);


                var ItemId = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ItemID").ToString();
                var dtimg = Lip.SelectRecord("Select * from Stc_Items Where   BranchID=" + MySession.GlobalBranchID+" and ItemID=" + ItemId);

                if (dtimg.Rows.Count > 0)
                {
                    byte[] imgByte = null;
                    if (DBNull.Value != dtimg.Rows[0]["ItemImage"])
                    {
                        imgByte = (byte[])dtimg.Rows[0]["ItemImage"];
                        picItemImage.Image = byteArrayToImage(imgByte);
                    }
                    else
                        picItemImage.Image = null;
                }
            }
            else
            {
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemID"], "0");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[ItemName], " ");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[SizeName], " ");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ExpiryDate"], DateTime.Now.ToShortDateString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SalePrice"], "0");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CostPrice"], "0");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SizeID"], "0");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BarCode"], "0");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["StoreID"], txtStoreID.Text);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Bones"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Description"], "");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["PageNo"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemStatus"], 1);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Caliber"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Equivalen"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Net"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["TheCount"], 1);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Bones"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["AdditionalValue"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Cancel"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Serials"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Height"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Width"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Total"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["HavVat"], true);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["RemindQty"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CurrencyName"], cmbCurency.Text.ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CurrencyPrice"], txtCurrncyPrice.Text);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CurrencyEquivalent"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CurrencyID"], Comon.cInt(cmbCurency.EditValue));

            }
        }


        private void EnabledControl(bool Value)
        {
            foreach (Control item in this.Controls)
            {
                if (item is TextEdit && ((!(item.Name.Contains("AccountID"))) && (!(item.Name.Contains("AccountName")))))
                {
                    if (!(item.Name.Contains("lbl") && item.Name.Contains("Name")))
                    {
                        item.Enabled = Value;
                        ((TextEdit)item).Properties.AppearanceDisabled.ForeColor = Color.Black;
                        ((TextEdit)item).Properties.AppearanceDisabled.BackColor = Color.White;
                        if (Value == true)
                            ((TextEdit)item).Properties.AppearanceDisabled.BackColor = Color.White;
                    }
                }
                else if (item is TextEdit && (((item.Name.Contains("AccountID"))) || ((item.Name.Contains("AccountName")))))
                {
                    item.Enabled = Value;
                    ((TextEdit)item).Properties.AppearanceDisabled.ForeColor = Color.Black;
                    ((TextEdit)item).Properties.AppearanceDisabled.BackColor = Color.White;
                    if (Value)
                        ((TextEdit)item).Properties.AppearanceDisabled.BackColor = Color.White;
                }
                else if (item is SimpleButton && (((item.Name.Contains("btn"))) && ((item.Name.Contains("Search")))))
                {
                    ((SimpleButton)item).Enabled = Value;
                }
            }


            foreach (GridColumn col in gridView1.Columns)
            {

                gridView1.Columns[col.FieldName].OptionsColumn.AllowEdit = Value;
                gridView1.Columns[col.FieldName].OptionsColumn.AllowFocus = Value;
                gridView1.Columns[col.FieldName].OptionsColumn.ReadOnly = !Value;

            }
            if (Value)
                RolesButtonSearchAccountID();

            cmbFormPrinting.Enabled = true;

        }
        bool IsValidGrid()
        {
            double num;

            if (HasColumnErrors)
            {
                Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                return !HasColumnErrors;
            }

            gridView1.MoveLast();

            int length = gridView1.RowCount - 1;
            if (length <= 0)
            {
                Messages.MsgError(Messages.TitleError, Messages.msgThereIsNoRecordInput);
                return false;
            }
            for (int i = 0; i < length; i++)
            {
                foreach (GridColumn col in gridView1.Columns)
                {
                    if (col.FieldName == "BarCode" || col.FieldName == "Net" || col.FieldName == "Total" || col.FieldName == "SizeID" || col.FieldName == "ItemID" || col.FieldName == "QTY" || col.FieldName == "SizeID" || col.FieldName == "SalePrice")
                    {

                        var cellValue = gridView1.GetRowCellValue(i, col); ;

                        if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                        {
                            gridView1.SetColumnError(col, Messages.msgInputIsRequired);
                            Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                            return false;
                        }
                        if (col.FieldName == "BarCode")
                            return true;

                        else if (!(double.TryParse(cellValue.ToString(), out num)))
                        {
                            gridView1.SetColumnError(col, Messages.msgInputShouldBeNumber);
                            Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                            return false;
                        }
                        else if (Comon.cDbl(cellValue.ToString()) <= 0)
                        {
                            gridView1.SetColumnError(col, Messages.msgInputIsGreaterThanZero);
                            Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        #region Calculate
        private void CalculateRow(int Row = -1, bool IsHavVat = false)
        {
            try
            {
                SumTotalBalanceAndDiscount(Row, IsHavVat);
                //Remove Icon Validtion
                var Net = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Net");
                var Total = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Total");
                if ((Total != null && !(string.IsNullOrWhiteSpace(Total.ToString())) && Comon.ConvertToDecimalPrice(Total.ToString()) > 0))
                    gridView1.SetColumnError(gridView1.Columns["Total"], "");
                if ((Net != null && !(string.IsNullOrWhiteSpace(Net.ToString())) && Comon.ConvertToDecimalPrice(Net.ToString()) > 0))
                    gridView1.SetColumnError(gridView1.Columns["Net"], "");
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void SumTotalBalanceAndDiscount(int row = -1, bool IsHavVat = false)
        {
            try
            {
                decimal TotalAfterDiscount = 0;
                decimal TotalBeforeDiscount = 0;
                decimal Net = 0;
                decimal DiscountTotal = 0;
                decimal DiscountOnTotal = 0;
                decimal AdditionalAmount = 0;
                decimal DiscountRow = 0;
                decimal QTYRow = 0;
                decimal SalePriceRow = 0;
                decimal TotalRow = 0;
                decimal NetRow = 0;
                decimal TotalBeforeDiscountRow = 0;
                decimal AdditionalAmountRow = 0;
                bool HavVatRow = false;
                decimal TotalQty = 0;
                //RefreshOffers();
                for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                {

                    QTYRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "QTY").ToString());
                    TotalQty += QTYRow;
                    SalePriceRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "SalePrice").ToString());
                    DiscountRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Discount"));
                    HavVatRow = row == i ? IsHavVat : Comon.cbool(gridView1.GetRowCellValue(i, "HavVat"));
                    //TotalBeforeDiscountRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Total"));
                    TotalBeforeDiscountRow = Comon.ConvertToDecimalPrice(QTYRow * SalePriceRow);
                    TotalRow = Comon.ConvertToDecimalPrice(TotalBeforeDiscountRow - DiscountRow);
                    if (MySession.GlobalHaveVat == "1")
                        AdditionalAmountRow = HavVatRow == true ? Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "AdditionalValue")) : 0;
                    
                    NetRow = Comon.ConvertToDecimalPrice(TotalRow + AdditionalAmountRow);
                    //gridView1.SetRowCellValue(i, gridView1.Columns["Total"], NetRow.ToString());
                    if (Comon.cDec(txtCurrncyPrice.Text) > 0)
                        gridView1.SetRowCellValue(i, "CurrencyEquivalent", Comon.ConvertToDecimalPrice(Comon.cDec(NetRow) * Comon.cDec(Comon.cDec(txtCurrncyPrice.Text))).ToString());

                    //gridView1.SetRowCellValue(i, gridView1.Columns["Net"], NetRow.ToString());
                    // gridView1.SetRowCellValue(i, gridView1.Columns["SalePrice"], SalePriceRow.ToString());
                    gridView1.SetRowCellValue(i, gridView1.Columns["#"], (i + 1).ToString());
                    TotalBeforeDiscount += TotalBeforeDiscountRow;
                    TotalAfterDiscount += TotalRow;
                    DiscountTotal += DiscountRow;
                    AdditionalAmount += AdditionalAmountRow;
                    Net += NetRow;
                }

                if (rowIndex < 0)
                {
                    var ResultQTY = gridView1.GetRowCellValue(rowIndex, "QTY");
                    var ResultSalePrice = gridView1.GetRowCellValue(rowIndex, "SalePrice");
                    var ResultDiscount = gridView1.GetRowCellValue(rowIndex, "Discount");
                    var ResultHavVat = gridView1.GetRowCellValue(rowIndex, "HavVat");

                    QTYRow = ResultQTY != null ? Comon.ConvertToDecimalPrice(ResultQTY.ToString()) : 0;
                    TotalQty += QTYRow;
                    SalePriceRow = ResultSalePrice != null ? Comon.ConvertToDecimalPrice(ResultSalePrice.ToString()) : 0;
                    DiscountRow = ResultDiscount != null ? Comon.ConvertToDecimalPrice(ResultDiscount.ToString()) : 0;
                    HavVatRow = ResultDiscount != null ? Comon.cbool(ResultHavVat.ToString()) : false;
                    HavVatRow = row == rowIndex ? IsHavVat : Comon.cbool(gridView1.GetRowCellValue(rowIndex, "HavVat"));
                    TotalBeforeDiscountRow = Comon.ConvertToDecimalPrice(QTYRow * SalePriceRow);
                    TotalRow = Comon.ConvertToDecimalPrice(QTYRow * SalePriceRow - DiscountRow);
                    if (MySession.GlobalHaveVat == "1")
                       AdditionalAmountRow = HavVatRow == true ? Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "AdditionalValue")) : 0;

                    NetRow = Comon.ConvertToDecimalPrice(TotalRow + AdditionalAmountRow);

                    ////////

                    //gridView1.SetRowCellValue(rowIndex, gridView1.Columns["Total"], NetRow.ToString());
                    if (Comon.cDec(txtCurrncyPrice.Text) > 0)
                        gridView1.SetRowCellValue(rowIndex, "CurrencyEquivalent", Comon.ConvertToDecimalPrice(Comon.cDec(NetRow) * Comon.cDec(Comon.cDec(txtCurrncyPrice.Text))).ToString());

                    //gridView1.SetRowCellValue(rowIndex, gridView1.Columns["Net"], NetRow.ToString());
                    //gridView1.SetRowCellValue(rowIndex, gridView1.Columns["SalePrice"], SalePriceRow.ToString());

                    TotalBeforeDiscount += TotalBeforeDiscountRow;
                    TotalAfterDiscount += TotalRow;
                    DiscountTotal += DiscountRow;
                    AdditionalAmount += AdditionalAmountRow;
                    Net += NetRow;
                }
                lblUnitDiscount.Text = DiscountTotal.ToString("N" + MySession.GlobalPriceDigits);
                DiscountOnTotal = Comon.ConvertToDecimalPrice(txtDiscountOnTotal.Text);
                if (DiscountOnTotal > 0)
                {
                    decimal Total = TotalAfterDiscount - DiscountOnTotal;
                    if(MySession.GlobalHaveVat=="1")
                       AdditionalAmount = (Total) / 100 * MySession.GlobalPercentVat;
                    Net = Comon.ConvertToDecimalPrice(Total + AdditionalAmount);
                }

                //if (Typevat == "1")
                //{
                //    TotalAfterDiscount = Comon.ConvertToDecimalPrice((Net) - DiscountOnTotal);
                //    TotalBeforeDiscount = Comon.ConvertToDecimalPrice(Net) + Comon.ConvertToDecimalPrice(lblUnitDiscount.Text);
                //}


                lblDiscountTotal.Text = (DiscountTotal + DiscountOnTotal).ToString("N" + MySession.GlobalPriceDigits);
                lblInvoiceTotalBeforeDiscount.Text = Comon.ConvertToDecimalPrice(TotalBeforeDiscount).ToString("N" + MySession.GlobalPriceDigits);
                lblInvoiceTotal.Text = (Comon.ConvertToDecimalPrice(TotalAfterDiscount) - Comon.ConvertToDecimalPrice(DiscountOnTotal)).ToString("N" + MySession.GlobalPriceDigits);
                lblTotalQTY.Text = TotalQty + "";
                lblAdditionaAmmount.Text = Comon.ConvertToDecimalPrice(AdditionalAmount).ToString("N" + MySession.GlobalPriceDigits);
                lblNetBalance.Text = Comon.ConvertToDecimalPrice(Net).ToString("N" + MySession.GlobalPriceDigits);
                 
                int isLocalCurrncy = Comon.cInt(Lip.GetValue("select TypeCurrency from Acc_Currency where   BranchID=" + MySession.GlobalBranchID +" and ID=" + Comon.cInt(cmbCurency.EditValue) + " and Cancel=0"));
                if (isLocalCurrncy > 1)
                {
                    decimal CurrncyPrice = Comon.cDec(Lip.GetValue("select ExchangeRate from Acc_Currency where   BranchID=" + MySession.GlobalBranchID +" and ID=" + Comon.cInt(cmbCurency.EditValue) + " and Cancel=0"));
                    lblCurrencyEqv.Text = Comon.cDec(Comon.cDec(lblNetBalance.Text) * Comon.cDec(txtCurrncyPrice.Text)) + "";
                }
                else
                {
                    txtCurrncyPrice.Text = "1";
                    lblCurrencyEqv.Visible = false;
                    lblCurrncyPric.Visible = false;
                    lblcurrncyEquvilant.Visible = false;
                    txtCurrncyPrice.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }


        private void SumTotalBalanceAndDiscountread(int row = -1, bool IsHavVat = false)
        {
            try
            {

                decimal TotalAfterDiscount = 0;
                decimal TotalBeforeDiscount = 0;
                decimal Net = 0;
                decimal DiscountTotal = 0;
                decimal DiscountOnTotal = 0;
                decimal AdditionalAmount = 0;
                decimal DiscountRow = 0;
                decimal CaratPriceTotal = 0;
                decimal SpendPriceTotal = 0;
                decimal SalePriceTotal = 0;

                decimal TotalDaimond_W = 0;
                decimal TotalStown_W = 0;
                decimal TotalBagat_W = 0;

                decimal QTYRow = 0;
                decimal QTY18 = 0;
                decimal QTY21 = 0;

                decimal QTY22 = 0;
                decimal QTY24 = 0;

                decimal InvoiceTotalGold = 0;

                decimal SalePriceRow = 0;
                decimal TotalRow = 0;
                decimal NetRow = 0;
                decimal TotalBeforeDiscountRow = 0;
                decimal AdditionalAmountRow = 0;
                bool HavVatRow = false;
                MySession.UseNetINInvoiceSales = 1;
                gridView1.MoveLast();
                for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                {
                    int Caliber = Comon.cInt(gridView1.GetRowCellValue(i, SizeName).ToString());
                    QTYRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "QTY").ToString());
                    SalePriceRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "SalePrice").ToString());
                    DiscountRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Discount"));
                    HavVatRow = row == i ? IsHavVat : Comon.cbool(gridView1.GetRowCellValue(i, "HavVat"));
                    if (MySession.GlobalHaveVat == "1")
                       AdditionalAmountRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "AdditionalValue"));
                    NetRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Net"));

                    TotalDaimond_W += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "DIAMOND_W"));
                    TotalStown_W += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "STONE_W"));
                    TotalBagat_W += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "BAGET_W"));

                    TotalBeforeDiscountRow = Comon.ConvertToDecimalPrice(QTYRow * SalePriceRow);


                    TotalRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Total"));
                    if (MySession.GlobalHaveVat == "1")
                       AdditionalAmountRow = HavVatRow == true ? Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "AdditionalValue")) : 0;
                    NetRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Net"));
                    TotalBeforeDiscountRow = TotalRow;

                    CaratPriceTotal += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "CaratPrice"));
                    SpendPriceTotal += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "SpendPrice"));
                    SalePriceTotal += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "SalePrice"));


                    if (Caliber == 18)
                        QTY18 = QTY18 + QTYRow;
                    if (Caliber == 21)
                        QTY21 = QTY21 + QTYRow;

                    if (Caliber == 22)
                        QTY22 = QTY22 + QTYRow;
                    if (Caliber == 24)
                        QTY24 = QTY24 + QTYRow;



                    NetRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "Net"));
                    TotalBeforeDiscount += TotalBeforeDiscountRow;
                    TotalAfterDiscount += TotalRow;
                    DiscountTotal += DiscountRow;
                    AdditionalAmount += AdditionalAmountRow;
                    Net += NetRow;
                }

                if (rowIndex < 0)
                {
                    var ResultCaliber = Comon.cInt(gridView1.GetRowCellValue(rowIndex, SizeName));
                    var ResultQTY = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "QTY");
                    var ResultSalePrice = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "SalePrice");
                    var ResultDiscount = gridView1.GetRowCellValue(rowIndex, "Discount");
                    var ResultHavVat = gridView1.GetRowCellValue(rowIndex, "HavVat");
                    QTYRow = ResultQTY != null ? Comon.ConvertToDecimalPrice(ResultQTY.ToString()) : 0;
                    SalePriceRow = ResultSalePrice != null ? Comon.ConvertToDecimalPrice(ResultSalePrice.ToString()) : 0;
                    DiscountRow = ResultDiscount != null ? Comon.ConvertToDecimalPrice(ResultDiscount.ToString()) : 0;
                    HavVatRow = ResultDiscount != null ? Comon.cbool(ResultHavVat.ToString()) : false;
                    if (MySession.GlobalHaveVat == "1")
                       AdditionalAmountRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "AdditionalValue"));
                    NetRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "Net"));
                    TotalBeforeDiscountRow = Comon.ConvertToDecimalPrice(QTYRow * SalePriceRow);

                    TotalDaimond_W += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "DIAMOND_W"));
                    TotalStown_W += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "STONE_W"));
                    TotalBagat_W += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "BAGET_W"));


                    if (Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "Net")) > 0 && MySession.UseNetINInvoiceSales == 1)
                    {
                        TotalRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "Total"));
                        if (MySession.GlobalHaveVat == "1")
                            AdditionalAmountRow = HavVatRow == true ? Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "AdditionalValue")) : 0;
                        NetRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "Net"));
                        TotalBeforeDiscountRow = TotalRow;
                    }
                    else
                    {
                        TotalRow = Comon.ConvertToDecimalPrice(QTYRow * SalePriceRow - DiscountRow);
                        AdditionalAmountRow = HavVatRow == true ? Comon.ConvertToDecimalPrice((TotalRow) / 100 * MySession.GlobalPercentVat) : 0;
                        NetRow = Comon.ConvertToDecimalPrice(TotalRow + AdditionalAmountRow);
                    }

                    if (ResultCaliber == 18)
                        QTY18 = QTY18 + Comon.ConvertToDecimalPrice(QTYRow);
                    if (ResultCaliber == 21)
                        QTY21 = QTY21 + Comon.ConvertToDecimalPrice(QTYRow);

                    if (ResultCaliber == 22)
                        QTY22 = QTY22 + Comon.ConvertToDecimalPrice(QTYRow);
                    if (ResultCaliber == 24)
                        QTY24 = QTY24 + Comon.ConvertToDecimalPrice(QTYRow);
                    CaratPriceTotal += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "CaratPrice"));
                    SpendPriceTotal += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "SpendPrice"));
                    SalePriceTotal += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "SalePrice"));



                    NetRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "Net"));
                    TotalBeforeDiscount += TotalBeforeDiscountRow;
                    TotalAfterDiscount += TotalRow;
                    DiscountTotal += DiscountRow;
                    AdditionalAmount += AdditionalAmountRow;
                    Net += NetRow;
                }

                DiscountOnTotal = Comon.ConvertToDecimalPrice(txtDiscountOnTotal.Text);
                lblDiscountTotal.Text = (DiscountTotal + DiscountOnTotal).ToString("N" + MySession.GlobalPriceDigits);
                lblInvoiceTotalBeforeDiscount.Text = Comon.ConvertToDecimalPrice(TotalBeforeDiscount).ToString("N" + MySession.GlobalPriceDigits);
                lblInvoiceTotal.Text = (Comon.ConvertToDecimalPrice(TotalAfterDiscount) - Comon.ConvertToDecimalPrice(DiscountOnTotal)).ToString("N" + MySession.GlobalPriceDigits);

                if (DiscountOnTotal > 0)
                {
                    decimal Total = TotalAfterDiscount - DiscountOnTotal;
                    if (AdditionalAmount > 0)
                        AdditionalAmount = (Total) / 100 * MySession.GlobalPercentVat;
                    Net = Comon.ConvertToDecimalPrice(Total + AdditionalAmount);
                }
                Net = Comon.ConvertToDecimalPrice(lblInvoiceTotal.Text) + AdditionalAmount;
                lblNetBalance.Text = Comon.ConvertToDecimalPrice(Net).ToString("N" + MySession.GlobalPriceDigits);

                decimal Eq = 0;


                Eq = Comon.ConvertTo21Caliber(QTY18, 18, 18);
                Eq = Eq + Comon.ConvertTo21Caliber(QTY21, 21, 18);
                Eq = Eq + Comon.ConvertTo21Caliber(QTY22, 22, 18);
                Eq = Eq + Comon.ConvertTo21Caliber(QTY24, 24, 18);

                lblInvoiceTotalGold.Text = Comon.ConvertToDecimalQty(Eq).ToString("N" + MySession.GlobalQtyDigits);

                lblTotalQTY.Text = Comon.ConvertToDecimalQty(QTY18).ToString("N" + MySession.GlobalQtyDigits);
                lbl21.Text = Comon.ConvertToDecimalQty(QTY21).ToString("N" + MySession.GlobalQtyDigits);

                lbl22.Text = Comon.ConvertToDecimalQty(QTY22).ToString("N" + MySession.GlobalQtyDigits);
                lbl24.Text = Comon.ConvertToDecimalQty(QTY24).ToString("N" + MySession.GlobalQtyDigits);

                lblTotalStown.Text = TotalStown_W.ToString("N" + MySession.GlobalQtyDigits);
                lblTotalDaimond.Text = TotalDaimond_W.ToString("N" + MySession.GlobalQtyDigits);
                lblTotalBagat.Text = TotalBagat_W.ToString("N" + MySession.GlobalQtyDigits);
            }

            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        #endregion
        #endregion
        #region Function
        private void ShortcutOpen()
        {
            FocusedControl = GetIndexFocusedControl();
            if (FocusedControl == null) return;

            if (FocusedControl.Trim() == txtCustomerID.Name)
            {
                frmSuppliers frm = new frmSuppliers();
                if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                {
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm);
                    frm.Show();
                }
                else
                    frm.Dispose();
            }

            if (FocusedControl.Trim() == txtStoreID.Name)
            {
                frmStores frm = new frmStores();
                if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                {
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm);
                    frm.Show();
                }
                else
                    frm.Dispose();
            }
            else if (FocusedControl.Trim() == txtCostCenterID.Name)
            {
                frmCostCenter frm = new frmCostCenter();
                if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                {
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm);
                    frm.Show();
                }
                else
                    frm.Dispose();
            }

            else if (FocusedControl.Trim() == txtSellerID.Name)
            {
                frmSellers frm = new frmSellers();
                if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                {
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm);
                    frm.Show();
                }
                else
                    frm.Dispose();
            }
            else if (FocusedControl.Trim() == gridControl.Name)
            {

                if (gridView1.FocusedColumn.Name == "colItemID" || gridView1.FocusedColumn.Name == "col" + ItemName || gridView1.FocusedColumn.Name == "colBarCode")
                {
                    frmItems frm = new frmItems();
                    if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm);

                         frm.Show();
                        {
                            bool b = true;
                        };
                        frm.Dispose();
                        if (frm.IsDisposed)
                        {
                            RepositoryItemLookUpEdit rItem = Common.LookUpEditItemName();
                            gridView1.Columns[ItemName].ColumnEdit = rItem;
                            gridControl.RepositoryItems.Add(rItem);

                        };
                    }
                    else
                        frm.Dispose();
                }
                else if (gridView1.FocusedColumn.Name == "colSizeName" || gridView1.FocusedColumn.Name == "colSizeID")
                {
                    frmSizingUnits frm = new frmSizingUnits();
                    if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm);
                        frm.Show();
                    }
                    else
                        frm.Dispose();
                }
            }
        }
        private void AddRow()
        {
            try
            {
                if ((gridView1.IsNewItemRow(gridView1.FocusedRowHandle)))
                    gridView1.AddNewRow();
            }
            catch (Exception ex)
            {

            }

        }
        #region Other Function
        public void Find()
        {

            CSearch cls = new CSearch();
            int[] ColumnWidth = new int[] { 100, 300 };
            string SearchSql = "";
            string Condition = "Where 1=1";

            FocusedControl = GetIndexFocusedControl();

            if (FocusedControl == null) return;


            if (FocusedControl.Trim() == txtCustomerID.Name)
            {
                if (!MySession.GlobalAllowChangefrmOrderSaleCustomerID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtCustomerID, lblSupplierName, "SublierID", "رقم المـــورد", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtCustomerID, lblSupplierName, "SublierID", "SublierID ID", MySession.GlobalBranchID);
            }

            else if (FocusedControl.Trim() == txtStoreID.Name)
            {
               if (!MySession.GlobalAllowChangefrmOrderSaleStoreID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtStoreID, lblStoreName, "StoreID", "رقم الحساب", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtStoreID, lblStoreName, "StoreID", "Account ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == txtOrderID.Name)
            {
                if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtOrderID, null, "SalseOrder", "رقم الطلب", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtOrderID, null, "SalseOrder", "Invoice ID", MySession.GlobalBranchID);
            }

            else if (FocusedControl.Trim() == txtSellerID.Name)
            {
                if (!MySession.GlobalAllowChangefrmOrderSaleSellerID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtSellerID, lblSellerName, "SellerID", "رقم البائع", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtSellerID, lblSellerName, "SellerID", "Seller ID", MySession.GlobalBranchID);
            }

            else if (FocusedControl.Trim() == gridControl.Name)
            {
                if (gridView1.FocusedColumn == null) return;

                if (gridView1.FocusedColumn.Name == "colBarCode" || gridView1.FocusedColumn.Name == "colItemName" || gridView1.FocusedColumn.Name == "colItemID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "البـاركـود", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "البـاركـود", MySession.GlobalBranchID);
                }

                if (gridView1.FocusedColumn.Name == "colSizeName" || gridView1.FocusedColumn.Name == "colSizeID")
                {
                    var itemID = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemID"]);
                    var Barcode = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BarCode"]);
                    if (itemID != null && Barcode != null)
                    {

                        Condition += " And ItemID=" + Comon.cInt(itemID);
                        if (UserInfo.Language == iLanguage.Arabic)
                            PrepareSearchQuery.Find(ref cls, null, null, "ItemBySize", "رقـم الـوحـــده", MySession.GlobalBranchID, Condition);
                        else
                            PrepareSearchQuery.Find(ref cls, null, null, "ItemBySize", "Size ID", MySession.GlobalBranchID, Condition);
                    }
                }
            }
            GetSelectedSearchValue(cls);
        }
        public void GetSelectedSearchValue(CSearch cls)
        {
            if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
            {

                if (FocusedControl == txtCustomerID.Name)
                {
                    txtCustomerID.Text = cls.PrimaryKeyValue.ToString();
                    txtSupplierID_Validating(null, null);
                }

                else if (FocusedControl == txtStoreID.Name)
                {
                    txtStoreID.Text = cls.PrimaryKeyValue.ToString();
                    txtStoreID_Validating(null, null);
                }

                else if (FocusedControl == txtCostCenterID.Name)
                {
                    txtCostCenterID.Text = cls.PrimaryKeyValue.ToString();

                }
                else if (FocusedControl == txtSellerID.Name)
                {
                    txtSellerID.Text = cls.PrimaryKeyValue.ToString();
                    txtSellerID_Validating(null, null);
                }
                else if (FocusedControl == txtOrderID.Name)
                {
                    txtOrderID.Text = cls.PrimaryKeyValue.ToString();
                    txtOrderID_Validating(null, null);
                }



                else if (FocusedControl == gridControl.Name)
                {
                    if (gridView1.FocusedColumn.Name == "colBarCode" || gridView1.FocusedColumn.Name == "colItemName" || gridView1.FocusedColumn.Name == "colItemID")
                    {
                        string Barcode = cls.PrimaryKeyValue.ToString();
                        if (Stc_itemsDAL.CheckIfStopItemUnit(Barcode, MySession.GlobalBranchID, MySession.GlobalFacilityID) == 1)
                        {

                            Messages.MsgStop(Messages.TitleInfo, Messages.msgWorningThisUnitIsStop);
                            return;
                        }
                        gridView1.AddNewRow();

                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BarCode"], Barcode);
                        FileItemData(Stc_itemsDAL.GetItemData1(Barcode, UserInfo.FacilityID));
                        CalculateRow();
                        gridView1.Columns["SalePrice"].OptionsColumn.ReadOnly = !MySession.GlobalCanChangeInvoicePrice;
                        Find();
                    }
                    else if (gridView1.FocusedColumn.Name == "colSizeName" || gridView1.FocusedColumn.Name == "colSizeID")
                    {

                        int SizeID = Comon.cInt(cls.PrimaryKeyValue.ToString());
                        var itemID = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemID"]);
                        var Barcode = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BarCode"]);
                        if (itemID != null && Barcode != null)
                        {

                            if (Stc_itemsDAL.CheckIfStopItemUnit(Comon.cInt(itemID), SizeID, MySession.GlobalBranchID, MySession.GlobalFacilityID) == 1)
                            {
                                Messages.MsgStop(Messages.TitleError, Messages.msgWorningThisUnitIsStop);
                                return;
                            }
                            FileItemData(Stc_itemsDAL.GetItemDataByItemID_SizeID(Comon.cInt(itemID), SizeID, UserInfo.FacilityID));
                            CalculateRow();
                        }

                    }
                }
            }

        }

        public System.Drawing.Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
            return returnImage;
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
        public void ClearFields()
        {
            try
            {

                lblTotalStown.Text = "0";
                lblTotalBagat.Text = "0";
                lblTotalDaimond.Text = "0";

                button1.Visible = false;
                lblInvoiceTotalGold.Text = "0";
                chkNoSale.Checked = false;
                DiscountCustomer = 0;
           

                txtVatID.Text = "";
                txtDocumentID.Text = "";
                lblTotalQTY.Text = "0";
                lbl21.Text = "0";
                lbl22.Text = "0";
                lbl24.Text = "0";

                txtCustomerMobile.Text = "";

                txtNotes.Text = "";

                /////////////////////////////

                /////////////////////////////////////////////////
                txtInvoiceDate.EditValue = DateTime.Now;

                checkBox1.Checked = false;
                checkBox2.Checked = true;

                txtNotes.Text = "";

                lblInvoiceTotalBeforeDiscount.Text = "";

                lblCreditAccountName.Text = "";

                lblInvoiceTotal.Text = "0";

                txtDiscountOnTotal.Text = "0";
                txtDiscountPercent.Text = "0";
                lblDiscountTotal.Text = "0";

                lblNetBalance.Text = "0";


                picItemUnits.Image = null;


                txtCostCenterID.Text = MySession.GlobalDefaultOrderSaleCostCenterID;

                txtSellerID.Text = MySession.GlobalDefaultSellerID;
                txtSellerID_Validating(null, null);
                txtStoreID.Text = MySession.GlobalDefaultOrderSaleStoreID;
                txtStoreID_Validating(null, null);

                cmbCurency.EditValue = Comon.cInt(MySession.GlobalDefaultOrderSaleCurrncyID);
                txtCustomerID.Text = MySession.GlobalDefaultOrderSaleCustomerID;
                txtCustomerID_Validating(null, null);

                lstDetail = new BindingList<Sales_SalesInvoiceDetails>();
                lstDetail.AllowNew = true;
                lstDetail.AllowEdit = true;
                lstDetail.AllowRemove = true;
                gridControl.DataSource = lstDetail;
                dt = new DataTable();
                picItemImage.Image = null;


            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        public void MoveRec(long PremaryKeyValue, int Direction)
        {
            try
            {
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                #region If
                if (FormView == true)
                {
                    SplashScreenManager.CloseForm(false);
                    strSQL = "SELECT TOP 1 * FROM " + Sales_SalesOrderDAL.TableName + " Where  GoldUsing= " + GoldUsing + " and Cancel =0 And BranchID= " + Comon.cInt(cmbBranchesID.EditValue);
                    switch (Direction)
                    {
                        case xMoveFirst:
                            {
                                strSQL = strSQL + " ORDER BY " + Sales_SalesOrderDAL.PremaryKey + " ASC";
                                break;
                            }

                        case xMoveNext:
                            {
                                strSQL = strSQL + " And " + Sales_SalesOrderDAL.PremaryKey + ">" + PremaryKeyValue + " ORDER BY " + Sales_SalesOrderDAL.PremaryKey + " asc";
                                break;
                            }

                        case xMovePrev:
                            {
                                strSQL = strSQL + " And " + Sales_SalesOrderDAL.PremaryKey + "<" + PremaryKeyValue + " ORDER BY " + Sales_SalesOrderDAL.PremaryKey + " desc";
                                break;
                            }

                        case xMoveLast:
                            {
                                strSQL = strSQL + " ORDER BY " + Sales_SalesOrderDAL.PremaryKey + " DESC";
                                break;
                            }
                    }
                    cClass = new Sales_SalesOrderDAL();
                    long InvoicIDTemp = Comon.cLong(txtOrderID.Text);
                    InvoicIDTemp = cClass.GetRecordSetBySQL(strSQL);
                    if (cClass.FoundResult == true)
                    {
                        ReadRecord(InvoicIDTemp);

                    }
                    SendKeys.Send("{Escape}");
                }
                #endregion
                else
                {
                    SplashScreenManager.CloseForm(false);
                    Messages.MsgStop(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord);
                    return;
                }


            }

            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);

            }
        }

        #endregion
        #region Do Function
        protected override void DoNew()
        {
            try
            {
                IsNewRecord = true;
                txtOrderID.Text = Sales_SalesOrderDAL.GetNewID(MySession.GlobalFacilityID, Comon.cInt(cmbBranchesID.EditValue), MySession.UserID).ToString();

                ClearFields();
                IdPrint = false;
                EnabledControl(true);
                cmbFormPrinting.EditValue = 1;
                gridView1.Focus();
                gridView1.MoveNext();
                gridView1.FocusedColumn = gridView1.VisibleColumns[1];
                gridView1.Columns["SalePrice"].OptionsColumn.ReadOnly = !MySession.GlobalCanChangeInvoicePrice;

            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        protected override void DoLast()
        {
            try
            {
                MoveRec(0, xMoveLast);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        protected override void DoFirst()
        {
            try
            {
                MoveRec(0, xMoveFirst);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        protected override void DoNext()
        {
            try
            {
                MoveRec(Comon.cInt(txtOrderID.Text), xMoveNext);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        protected override void DoPrevious()
        {
            try
            {
                MoveRec(Comon.cInt(txtOrderID.Text), xMovePrev);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        protected override void DoSearch()
        {
            try
            {
                txtOrderID.Enabled = true;
                txtOrderID.Focus();
                Find();
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        protected override void DoEdit()
        {

            DataTable dtItem = new DataTable();
            dtItem.Columns.Add("ID", System.Type.GetType("System.String"));
            dtItem.Columns.Add("FacilityID", System.Type.GetType("System.String"));
            dtItem.Columns.Add("ItemID", System.Type.GetType("System.String"));
            dtItem.Columns.Add("SizeID", System.Type.GetType("System.String"));
            dtItem.Columns.Add("Description", System.Type.GetType("System.String"));
            dtItem.Columns.Add("StoreID", System.Type.GetType("System.String"));
            dtItem.Columns.Add("Discount", System.Type.GetType("System.String"));
            dtItem.Columns.Add("AdditionalValue", System.Type.GetType("System.String"));
            dtItem.Columns.Add("Net", System.Type.GetType("System.String"));
            dtItem.Columns.Add("Cancel", System.Type.GetType("System.String"));
            dtItem.Columns.Add("BarCode", System.Type.GetType("System.String"));
            dtItem.Columns.Add(ItemName, System.Type.GetType("System.String"));
            dtItem.Columns.Add(SizeName, System.Type.GetType("System.String"));

            dtItem.Columns.Add("GroupID", System.Type.GetType("System.String"));
            dtItem.Columns.Add(GroupName, System.Type.GetType("System.String"));



            dtItem.Columns.Add("QTY", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("CostPrice", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("Total", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("ExpiryDateStr", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("ExpiryDate", System.Type.GetType("System.DateTime"));
            dtItem.Columns.Add("Bones", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("HavVat", System.Type.GetType("System.Boolean"));

            dtItem.Columns.Add("SalePrice", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("Serials", System.Type.GetType("System.String"));
            dtItem.Columns.Add("BAGET_W", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("STONE_W", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("DIAMOND_W", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("Color", System.Type.GetType("System.String"));
            dtItem.Columns.Add("CLARITY", System.Type.GetType("System.String"));
            dtItem.Columns.Add("CaratPrice", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("SpendPrice", System.Type.GetType("System.Decimal"));

            dtItem.Columns.Add("CurrencyID", System.Type.GetType("System.String"));
            dtItem.Columns.Add("CurrencyName", System.Type.GetType("System.String"));
            dtItem.Columns.Add("CurrencyPrice", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("CurrencyEquivalent", System.Type.GetType("System.Decimal"));

            for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
            {
                dtItem.Rows.Add();
                dtItem.Rows[i]["ID"] = i;
                dtItem.Rows[i]["FacilityID"] = UserInfo.FacilityID; ;
                dtItem.Rows[i]["BarCode"] = gridView1.GetRowCellValue(i, "BarCode").ToString();
                dtItem.Rows[i]["ItemID"] = Comon.cInt(gridView1.GetRowCellValue(i, "ItemID").ToString());
                dtItem.Rows[i]["SizeID"] = Comon.cInt(gridView1.GetRowCellValue(i, "SizeID").ToString());
                dtItem.Rows[i][ItemName] = gridView1.GetRowCellValue(i, ItemName).ToString();
                dtItem.Rows[i][SizeName] = gridView1.GetRowCellValue(i, SizeName).ToString();

                dtItem.Rows[i]["GroupID"] = gridView1.GetRowCellValue(i, "GroupID").ToString();
                dtItem.Rows[i][GroupName] = gridView1.GetRowCellValue(i, GroupName).ToString();

                dtItem.Rows[i]["QTY"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "QTY").ToString());
                dtItem.Rows[i]["SalePrice"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "SalePrice").ToString()); ;
                dtItem.Rows[i]["Bones"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Bones").ToString());
                dtItem.Rows[i]["Description"] = gridView1.GetRowCellValue(i, "Description").ToString();
                dtItem.Rows[i]["StoreID"] = Comon.cInt(gridView1.GetRowCellValue(i, "StoreID").ToString());
                dtItem.Rows[i]["Discount"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Discount").ToString());
                dtItem.Rows[i]["ExpiryDateStr"] = Comon.ConvertDateToSerial(gridView1.GetRowCellValue(i, "ExpiryDate").ToString());
                dtItem.Rows[i]["ExpiryDate"] = "01/01/1900";
                dtItem.Rows[i]["CostPrice"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "CostPrice").ToString());
                dtItem.Rows[i]["HavVat"] = Comon.cbool(gridView1.GetRowCellValue(i, "HavVat").ToString());
                dtItem.Rows[i]["Total"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Total").ToString());
                dtItem.Rows[i]["AdditionalValue"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "AdditionalValue").ToString());
                dtItem.Rows[i]["Net"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Net").ToString());
                dtItem.Rows[i]["Serials"] = gridView1.GetRowCellValue(i, "Serials").ToString();
                dtItem.Rows[i]["BAGET_W"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "BAGET_W").ToString());
                dtItem.Rows[i]["STONE_W"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "STONE_W").ToString());
                dtItem.Rows[i]["DIAMOND_W"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "DIAMOND_W").ToString());
                dtItem.Rows[i]["Color"] = gridView1.GetRowCellValue(i, "Color").ToString();
                dtItem.Rows[i]["CLARITY"] = gridView1.GetRowCellValue(i, "CLARITY").ToString();
                dtItem.Rows[i]["CaratPrice"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "CaratPrice").ToString()); 

                dtItem.Rows[i]["CurrencyID"] = gridView1.GetRowCellValue(i, "CurrencyID").ToString();
                dtItem.Rows[i]["CurrencyName"] = gridView1.GetRowCellValue(i, "CurrencyName").ToString();
                dtItem.Rows[i]["CurrencyPrice"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "CurrencyPrice").ToString());
                dtItem.Rows[i]["CurrencyEquivalent"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "CurrencyEquivalent").ToString());



                dtItem.Rows[i]["Cancel"] = 0;

            }
            gridControl.DataSource = dtItem;
            EnabledControl(true);
            Validations.DoEditRipon(this, ribbonControl1);


        }
        private void EmportItems()
        {
            DoNew();
            OleDbConnection oledbConn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + txtExcelPath.Text + ";Extended Properties=Excel 12.0");
            cItemsStores Store = new cItemsStores();
            bool Yes = Messages.MsgStopYesNo(Messages.TitleConfirm, "تأكيد الاسنيراد  ؟");
            if (!Yes)
                return;

            Application.DoEvents();
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
            oledbConn.Open();

            OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Sheet$]", oledbConn);

            OleDbDataAdapter oleda = new OleDbDataAdapter();
            oleda.SelectCommand = cmd;
            DataTable dt = new DataTable();
            oleda.Fill(dt);
            oledbConn.Close();

            if (dt.Rows.Count < 1)
                return;
            lstDetail = new BindingList<Sales_SalesInvoiceDetails>();
            lstDetail.AllowNew = true;
            lstDetail.AllowEdit = true;
            lstDetail.AllowRemove = true;
            gridControl.DataSource = lstDetail;

            Sales_SalesInvoiceDetails obj = new Sales_SalesInvoiceDetails();


            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                obj = new Sales_SalesInvoiceDetails();
                obj.ArbItemName = dt.Rows[i]["ITEM_NAME"].ToString();
                obj.EngItemName = dt.Rows[i]["ITEM_NAME"].ToString();
                obj.GroupID = Comon.cDbl(dt.Rows[i]["GroupID"].ToString());
                obj.DIAMOND_W = Comon.ConvertToDecimalPrice(dt.Rows[i]["DIAMOND_W"].ToString());
                obj.CostPrice = Comon.ConvertToDecimalPrice(dt.Rows[i]["price"].ToString());
                obj.QTY = Comon.ConvertToDecimalPrice(dt.Rows[i]["GOLD_GRAM_W"].ToString());
                obj.Caliber = Comon.cInt(dt.Rows[i]["GOLD_CALIBER"].ToString());
                obj.STONE_W = Comon.cDec(dt.Rows[i]["STONE_W"].ToString());
                obj.Serials = dt.Rows[i]["ITEM_NO"].ToString();
                obj.BarCode = dt.Rows[i]["BarCode"].ToString();
                obj.BAGET_W = Comon.ConvertToDecimalPrice(dt.Rows[i]["BAGET_W"].ToString());
                string Barcode = Lip.GetValue("select itemid from Sales_SalesOrderDetails Where Barcode='" + obj.BarCode + "'");
                if (Comon.cInt(Barcode) > 0)
                {
                    Yes = Messages.MsgStopYesNo(Messages.TitleConfirm, "الصنف موجود مسبقا" + " هل تريد الاستمرار " + obj.BarCode);
                    if (Yes)
                        continue;
                    else
                    {
                        SplashScreenManager.CloseForm(false);
                        return;
                    }
                }


                obj.Color = dt.Rows[i]["Color"].ToString();
                obj.CLARITY = dt.Rows[i]["CLARITY"].ToString();

                decimal CostPrice = Comon.ConvertToDecimalPrice(obj.CostPrice.ToString());
                decimal Bones = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(obj.CostPrice.ToString()));

                decimal additonalVAlue = Comon.ConvertToDecimalPrice((CostPrice * MySession.GlobalPercentVat) / 100);
                if (Bones > 0&&(MySession.GlobalHaveVat=="1"))
                    additonalVAlue = Comon.ConvertToDecimalPrice((Bones * MySession.GlobalPercentVat) / 100);

                //سعر تكلفة مع مصاريف
                decimal SpendPrice = Comon.ConvertToDecimalPrice(CostPrice);
                //سعر تكلفة المحل
                decimal CaratPrice = Comon.ConvertToDecimalPrice(Comon.cDec(SpendPrice * Comon.cDec(MySession.Cost)));
                //سعر الكارت وهو البيع
                decimal SalePrice = Comon.ConvertToDecimalPrice(CaratPrice * Comon.cDec(MySession.sumvalue));

                obj.AdditionalValue = additonalVAlue;
                obj.SalePrice = SalePrice;
                obj.SpendPrice = SpendPrice;
                obj.CaratPrice = CaratPrice;
                obj.Total = CostPrice;
                obj.Net = SpendPrice;
                obj.ArbGroupName = Lip.GetValue("Select  ArbName from Stc_ItemsGroups Where GroupID=" + obj.GroupID + " And BranchID =" + MySession.GlobalBranchID);
                obj.EngGroupName = obj.ArbGroupName;


                obj.ArbSizeName = obj.Caliber.ToString();
                obj.EngSizeName = obj.Caliber.ToString();


                lstDetail.Add(obj);

            }
            SumTotalBalanceAndDiscountread();

            gridControl.DataSource = lstDetail;
            SplashScreenManager.CloseForm(false);
        }

        protected override void DoSave()
        {
            try
            {
                if (!Validations.IsValidForm(this))
                    return;
                if (!IsValidGrid())
                    return;
                if (IsNewRecord && !FormAdd)
                {
                    Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToAddNewRecord);
                    return;
                }
                else if (!IsNewRecord)
                {
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

                }

                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);

                Save();
                //    scope.Complete(); 
                //  }

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
        public void ReadRecord(long OrderID, bool flag = false)
        {
            try
            {

                ClearFields();
                {

                    dt = Sales_SalesOrderDAL.frmGetDataDetalByID(Comon.cInt( OrderID), Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        IsNewRecord = false;
                        button1.Visible = true;
                        //Validate

                        txtStoreID.Text = dt.Rows[0]["StoreID"].ToString();
                        txtStoreID_Validating(null, null);

                        txtCostCenterID.Text = dt.Rows[0]["CostCenterID"].ToString();

                        StopSomeCode = true;
                        StopSomeCode = false;

                        txtCurrncyPrice.Text = dt.Rows[0]["CurrencyPrice"].ToString();
                        lblCurrencyEqv.Text = dt.Rows[0]["CurrencyEquivalent"].ToString();
                        cmbCurency.EditValue = Comon.cInt(dt.Rows[0]["CurrencyID"].ToString());

                        txtCustomerID.Text = dt.Rows[0]["CustomerID"].ToString();
                        txtSupplierID_Validating(null, null);


                        //cmbSellerID.EditValue = dt.Rows[0]["SellerID"].ToString();
                        //Masterdata
                        txtOrderID.Text = dt.Rows[0]["OrderID"].ToString();
                        txtNotes.Text = dt.Rows[0]["Notes"].ToString();
                        txtDocumentID.Text = dt.Rows[0]["DocumentID"].ToString();

                        //txtCustomerMobile.Text = dt.Rows[0]["Mobile"].ToString();


                        //Date
                        //txtInvoiceDate.Text = Comon.ConvertSerialDateTo(dt.Rows[0]["InvoiceDate"].ToString());
                        if (Comon.ConvertSerialDateTo(dt.Rows[0]["InvoiceDate"].ToString()) == "")
                            txtInvoiceDate.Text = "";
                        else
                            txtInvoiceDate.DateTime = DateTime.ParseExact(Comon.ConvertSerialDateTo(dt.Rows[0]["InvoiceDate"].ToString()), "dd/MM/yyyy", culture);//CultureInfo.InvariantCulture);

                        //Ammount




                        //حقول محسوبة  

                        lblInvoiceTotal.Text = dt.Rows[0]["InvoiceTotal"].ToString();



                        //GridVeiw
                        gridControl.DataSource = dt;

                        lstDetail.AllowNew = true;
                        lstDetail.AllowEdit = true;
                        lstDetail.AllowRemove = true;
                        CalculateRow();
                        this.pictureBox1.Image = Common.GenratCod("ID = " + txtOrderID.Text + " Date= " + txtInvoiceDate.Text);

                        Validations.DoReadRipon(this, ribbonControl1);
                        // ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Caption = txtOrderID.Text;
                    }
                }


            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void Save()
        {

            gridView1.FocusedColumn = gridView1.VisibleColumns[1];
            Sales_SalesOrderMaster objRecord = new Sales_SalesOrderMaster();
            objRecord.OrderID = 0;
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.FacilityID = UserInfo.FacilityID;
            objRecord.InvoiceDate = Comon.ConvertDateToSerial(txtInvoiceDate.Text).ToString();

            objRecord.CurrencyID = Comon.cInt(cmbCurency.EditValue);


            objRecord.CurrencyName = cmbCurency.Text.ToString();
            objRecord.CurrencyPrice = Comon.cDec(txtCurrncyPrice.Text);
            objRecord.CurrencyEquivalent = Comon.cDec(lblcurrncyEquvilant.Text);




            objRecord.StoreID = Comon.cDbl(txtStoreID.Text);

            objRecord.DocumentID = Comon.cInt(txtDocumentID.Text);
            objRecord.CustomerID = Comon.cDbl(txtCustomerID.Text);
            objRecord.CustomerName = lblSupplierName.Text;



            //objRecord.SellerID = Comon.cInt(cmbSellerID.EditValue);



            objRecord.OperationTypeName = (UserInfo.Language == iLanguage.English ? "Purchase  Order" : " طلـــب بيع ");
            txtNotes.Text = (txtNotes.Text.Trim() != "" ? txtNotes.Text.Trim() : (UserInfo.Language == iLanguage.English ? "Purchase  Order" : " طلـــب بيع "));


            objRecord.Notes = txtNotes.Text;
            //Account


            objRecord.InvoiceGoldTotal = Comon.cDec(lblInvoiceTotalGold.Text);
            objRecord.InvoiceEquivalenTotal = Comon.cDec(lblInvoiceTotalGold.Text);
            objRecord.WeightDiamondTotal = Comon.cDbl(lblTotalDaimond.Text);



            objRecord.GoldUsing = GoldUsing;
            //Date

            //Ammount

            objRecord.InvoiceTotal = (Comon.cDec(lblInvoiceTotalBeforeDiscount.Text));

             
            objRecord.Cancel = 0;

            //user Info
            objRecord.UserID = UserInfo.ID;
            objRecord.RegDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
            objRecord.RegTime = Comon.cDbl(Lip.GetServerTimeSerial()); ;
            objRecord.ComputerInfo = UserInfo.ComputerInfo;

            objRecord.EditUserID = 0;
            objRecord.EditTime = 0;
            objRecord.EditDate = 0;
            objRecord.EditComputerInfo = "";

            if (IsNewRecord == false)
            {
                objRecord.OrderID = Comon.cInt(txtOrderID.Text);
                objRecord.EditUserID = UserInfo.ID;
                objRecord.EditTime = Comon.cDbl(Lip.GetServerTimeSerial());
                objRecord.EditDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
                objRecord.EditComputerInfo = UserInfo.ComputerInfo;
            }

            Sales_SalesOrderDetails returned;
            List<Sales_SalesOrderDetails> listreturned = new List<Sales_SalesOrderDetails>();

            for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
            {
                returned = new Sales_SalesOrderDetails();
                returned.ID = i;
                returned.FacilityID = UserInfo.FacilityID;
                returned.BarCode = gridView1.GetRowCellValue(i, "BarCode").ToString();
                returned.Color = gridView1.GetRowCellValue(i, "Color").ToString();
                returned.CLARITY = gridView1.GetRowCellValue(i, "CLARITY").ToString();
                returned.ItemID = Comon.cInt(gridView1.GetRowCellValue(i, "ItemID").ToString());
                returned.SizeID = Comon.cInt(gridView1.GetRowCellValue(i, "SizeID").ToString());
                returned.QTY = Comon.cDec(gridView1.GetRowCellValue(i, "QTY").ToString());
                returned.STONE_W = Comon.cDec(gridView1.GetRowCellValue(i, "STONE_W").ToString());
                returned.DIAMOND_W = Comon.cDec(gridView1.GetRowCellValue(i, "DIAMOND_W").ToString());
                returned.BAGET_W = Comon.cDec(gridView1.GetRowCellValue(i, "BAGET_W").ToString());
                returned.Caliber = Comon.cInt(gridView1.GetRowCellValue(i, SizeName).ToString());
                returned.Equivalen = Comon.ConvertToDecimalPrice(Comon.ConvertTo21Caliber(returned.QTY, Comon.cInt(returned.Caliber), 18));
                returned.Bones = Comon.cDec(gridView1.GetRowCellValue(i, "Bones").ToString());
                returned.Description = gridView1.GetRowCellValue(i, ItemName).ToString();
                returned.StoreID = Comon.cDbl(txtStoreID.Text);
                returned.AdditionalValue = Comon.cDec(gridView1.GetRowCellValue(i, "AdditionalValue").ToString());
                returned.SalePrice = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "SalePrice").ToString()); 
                returned.ExpiryDateStr = 0;
                returned.CostPrice = Comon.cDec(gridView1.GetRowCellValue(i, "CostPrice").ToString()); ;


                returned.SpendPrice = Comon.cDec(gridView1.GetRowCellValue(i, "SpendPrice").ToString());
                returned.CurrencyID = Comon.cInt(gridView1.GetRowCellValue(i, "CurrencyID").ToString());
                returned.CurrencyName = gridView1.GetRowCellValue(i, "CurrencyName").ToString();
                returned.CurrencyPrice = Comon.cDbl(gridView1.GetRowCellValue(i, "CurrencyPrice").ToString());
                returned.CurrencyEquivalent = Comon.cDbl(gridView1.GetRowCellValue(i, "CurrencyEquivalent").ToString());
                returned.Serials = gridView1.GetRowCellValue(i, "Serials").ToString();

                returned.Net = Comon.cDec(gridView1.GetRowCellValue(i, "Net").ToString());
                returned.Total = Comon.cDec(gridView1.GetRowCellValue(i, "Total").ToString());

                returned.Cancel = 0;


                listreturned.Add(returned);
            }
            if (listreturned.Count > 0)
            {

                objRecord.SaleDatails = listreturned;


                string Result = Sales_SalesOrderDAL.InsertUsingXML(objRecord,   IsNewRecord).ToString();


                SplashScreenManager.CloseForm(false);


                if (IsNewRecord == true)
                {
                    if (Comon.cLong(Result) > 0)
                    {

                        IsNewRecord = false;

                        Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
                        txtOrderID.Text = Result.ToString();
                        if (falgPrint == true)
                            DoPrint();
                        DoNew();
                    }
                    else
                    {
                        Messages.MsgError(Messages.TitleError, Messages.msgErrorSave + " " + Result);

                    }

                }
                else
                {

                    if (Comon.cLong(Result) > 0)
                    {
                        txtOrderID_Validating(null, null);

                        Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
                    }
                    else
                    {
                        Messages.MsgError(Messages.TitleError, Messages.msgErrorSave + " " + Result);
                    }
                }

            }
            else
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(Messages.TitleError, Messages.msgInputIsRequired);
            }

        }


        protected override void DoDelete()
        {
            try
            {
                if (!FormDelete)
                {
                    Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToDeleteRecord);
                    return;
                }
                else
                {
                    bool Yes = Messages.MsgStopYesNo(Messages.TitleConfirm, Messages.msgConfirmDelete);
                    if (!Yes)
                        return;
                }
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                int TempID = Comon.cInt(txtOrderID.Text);
                bool FlageChecKDelete = false;

                for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                {

                    FlageChecKDelete = Lip.CheckTheItemIsHaveTransactionByBarCode(gridView1.GetRowCellValue(i, "BarCode").ToString(), "Sales_SalesOrderDetails");

                    if (FlageChecKDelete)
                    {
                        SplashScreenManager.CloseForm();
                        Messages.MsgError("Error Delete ", "لا يمكن حذف الصنف بسبب وجود عمليات محاسبية علية");
                        return;
                    }

                }



                Sales_SalesOrderMaster model = new Sales_SalesOrderMaster();
                model.OrderID = Comon.cInt(txtOrderID.Text);
                model.EditUserID = UserInfo.ID;
                model.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                model.FacilityID = UserInfo.FacilityID;
                model.EditComputerInfo = UserInfo.ComputerInfo;
                model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                model.EditTime = Comon.cLong(Lip.GetServerTimeSerial());

                string Result = Sales_SalesOrderDAL.DeleteSales_SalesOrderMaster(model);

                SplashScreenManager.CloseForm(false);
                if (Comon.cInt(Result) >= 0)
                {
                    Messages.MsgInfo(Messages.TitleInfo, Messages.msgDeleteComplete);
                    ClearFields();
                    txtOrderID.Text = model.OrderID.ToString();
                    MoveRec(model.OrderID, xMovePrev);
                }
                else
                {
                    Messages.MsgInfo(Messages.TitleInfo, Messages.msgErrorSave + " " + Result);
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


        void PrintPyCostPrice()
        {

            try
            {
                if (IsNewRecord)
                {
                    Messages.MsgWarning(Messages.TitleWorning, Messages.msgYouShouldSaveDataBeforePrinting);
                    return;
                }
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                /******************** Report Body *************************/
                ReportName = "rptPurchaseInvoice";
                bool IncludeHeader = true;
                string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");
                XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);

                /********************** Master *****************************/
                rptForm.RequestParameters = false;
                rptForm.Parameters["InvoiceID"].Value = txtOrderID.Text.Trim().ToString();
                rptForm.Parameters["SupplierName"].Value = lblSupplierName.Text.Trim().ToString();

                rptForm.Parameters["InvoiceDate"].Value = txtInvoiceDate.Text.Trim().ToString();
                rptForm.Parameters["VatID"].Value = txtVatID.Text.Trim().ToString();
                /********Total*********/

                rptForm.Parameters["InvoiceTotal"].Value = lblInvoiceTotal.Text.Trim().ToString();


                rptForm.Parameters["StoreName"].Value = txtCustomerMobile.Text.Trim().ToString();
                // rptForm.Parameters["TransportAmount"].Value = (Comon.ConvertToDecimalPrice(lblTotalSalePrice17.Text)).ToString();
          
                //rptForm.Parameters["TotalSale"].Value = Comon.ConvertToDecimalQty(lblTotalCost.Text);
                rptForm.Parameters["TotalWightGold"].Value = (Comon.ConvertToDecimalQty(lblTotalQTY.Text) + Comon.ConvertToDecimalQty(lbl21.Text) + Comon.ConvertToDecimalQty(lbl22.Text) + Comon.ConvertToDecimalQty(lbl24.Text)).ToString();

                rptForm.Parameters["TotalGold"].Value = Comon.ConvertToDecimalQty(lblInvoiceTotalGold.Text);
                rptForm.Parameters["TotalDaimond"].Value = Comon.ConvertToDecimalQty(lblTotalDaimond.Text);

                rptForm.Parameters["TotalStown"].Value = Comon.ConvertToDecimalQty(lblTotalStown.Text);

                rptForm.Parameters["TotalBagat"].Value = Comon.ConvertToDecimalQty(lblTotalBagat.Text);
                rptForm.Parameters["TotalSale"].Value = (Comon.ConvertToDecimalQty(lblInvoiceTotal.Text)).ToString();
                rptForm.Parameters["ReportName"].Value = " طلــب بيع ";
                rptForm.Parameters["G18"].Value = lblTotalQTY.Text;
                rptForm.Parameters["G21"].Value = lbl21.Text;
                rptForm.Parameters["G22"].Value = lbl22.Text;
                rptForm.Parameters["G24"].Value = lbl24.Text;

                for (int i = 0; i < rptForm.Parameters.Count; i++)
                    rptForm.Parameters[i].Visible = false;
                /********************** Details ****************************/
                var dataTable = new dsReports.rptPurchaseInvoiceDataTable();
                for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                {
                    var row = dataTable.NewRow();
                    row["#"] = i + 1;
                    row["BarCode"] = gridView1.GetRowCellValue(i, "BarCode").ToString();
                    row["ItemName"] = gridView1.GetRowCellValue(i, ItemName).ToString();

                    row["SizeName"] = gridView1.GetRowCellValue(i, SizeName).ToString();
                    row["QTY"] = gridView1.GetRowCellValue(i, "QTY").ToString();
                    row["Total"] = Comon.cLong(gridView1.GetRowCellValue(i, "CostPrice")).ToString();
                    row["Net"] = gridView1.GetRowCellValue(i, "Net").ToString();
                    row["CostPrice"] = gridView1.GetRowCellValue(i, "CostPrice").ToString();
                    row["Description"] = gridView1.GetRowCellValue(i, "Description").ToString();
                    row["STONE_W"] = gridView1.GetRowCellValue(i, "STONE_W").ToString();
                    row["DIAMOND_W"] = gridView1.GetRowCellValue(i, "DIAMOND_W").ToString();
                    row["BAGET_W"] = gridView1.GetRowCellValue(i, "BAGET_W").ToString();
                    row["CLARITY"] = gridView1.GetRowCellValue(i, "CLARITY").ToString();
                    row["Color"] = gridView1.GetRowCellValue(i, "Color").ToString();
                    row["Serials"] = gridView1.GetRowCellValue(i, "Serials").ToString();
                    row["Bones"] = gridView1.GetRowCellValue(i, "Bones").ToString();

                    dataTable.Rows.Add(row);
                }
                rptForm.DataSource = dataTable;
                rptForm.DataMember = ReportName;
                /******************** Report Binding ************************/
                XRSubreport subreport = (XRSubreport)rptForm.FindControl("subRptCompanyHeader", true);
                subreport.Visible = IncludeHeader;
                subreport.ReportSource = ReportComponent.CompanyHeader();
                rptForm.ShowPrintStatusDialog = false;
                rptForm.ShowPrintMarginsWarning = false;
                rptForm.CreateDocument();

                SplashScreenManager.CloseForm(false);
                ShowReportInReportViewer = true;
                if (ShowReportInReportViewer)
                {
                    frmReportViewer frmRptViewer = new frmReportViewer();
                    frmRptViewer.documentViewer1.DocumentSource = rptForm;
                    frmRptViewer.ShowDialog();
                }
                else
                {
                    bool IsSelectedPrinter = false;
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                    DataTable dt = ReportComponent.SelectRecord("SELECT *  from Printers where ReportName='" + ReportName + "'");
                    if (dt.Rows.Count > 0)
                        for (int i = 1; i < 6; i++)
                        {
                            string PrinterName = dt.Rows[0]["PrinterName" + i.ToString()].ToString().ToUpper();
                            if (!string.IsNullOrEmpty(PrinterName))
                            {
                                rptForm.PrinterName = PrinterName;
                                rptForm.Print(PrinterName);
                                IsSelectedPrinter = true;
                            }
                        }
                    SplashScreenManager.CloseForm(false);
                    if (!IsSelectedPrinter)
                        Messages.MsgWarning(Messages.TitleWorning, Messages.msgThereIsNotPrinterSelected);
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        void PrintPySalePrice()
        {
            try
            {
                if (IsNewRecord)
                {
                    Messages.MsgWarning(Messages.TitleWorning, Messages.msgYouShouldSaveDataBeforePrinting);
                    return;
                }
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);

                /******************** Report Body *************************/
                ReportName = "rptPurchaseInvoiceSalePricre";
                bool IncludeHeader = true;
                string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");
                XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);

                /********************** Master *****************************/
                rptForm.RequestParameters = false;
                rptForm.Parameters["OrderID"].Value = txtOrderID.Text.Trim().ToString();
                rptForm.Parameters["SupplierName"].Value = lblSupplierName.Text.Trim().ToString();

                rptForm.Parameters["InvoiceDate"].Value = txtInvoiceDate.Text.Trim().ToString();
                rptForm.Parameters["VatID"].Value = txtVatID.Text.Trim().ToString();
                /********Total*********/

                rptForm.Parameters["InvoiceTotal"].Value = lblInvoiceTotal.Text.Trim().ToString();
                rptForm.Parameters["UnitDiscount"].Value = lblNetBalance.Text.Trim().ToString();

                rptForm.Parameters["NetBalance"].Value = lblNetBalance.Text.Trim().ToString();
                rptForm.Parameters["StoreName"].Value = txtCustomerMobile.Text.Trim().ToString();

                rptForm.Parameters["TotalGold"].Value = (Comon.ConvertToDecimalQty(lblTotalQTY.Text) + Comon.ConvertToDecimalQty(lbl21.Text) + Comon.ConvertToDecimalQty(lbl22.Text) + Comon.ConvertToDecimalQty(lbl24.Text)).ToString();

                rptForm.Parameters["TotalDaimond"].Value = Comon.ConvertToDecimalQty(lblTotalDaimond.Text);
                rptForm.Parameters["TotalStown"].Value = Comon.ConvertToDecimalQty(lblTotalStown.Text);
                rptForm.Parameters["TotalBagat"].Value = Comon.ConvertToDecimalQty(lblTotalBagat.Text);
                // rptForm.Parameters["TotalGold"].Value = (Comon.ConvertToDecimalQty(lbl18.Text) + Comon.ConvertToDecimalQty(lbl21.Text) + Comon.ConvertToDecimalQty(lbl22.Text) + Comon.ConvertToDecimalQty(lbl24.Text)).ToString();

                rptForm.Parameters["ReportName"].Value = "طلب مشتريات";



                for (int i = 0; i < rptForm.Parameters.Count; i++)
                    rptForm.Parameters[i].Visible = false;
                /********************** Details ****************************/
                var dataTable = new dsReports.rptPurchaseInvoiceDataTable();
                for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                {

                    var row = dataTable.NewRow();
                    row["#"] = i + 1;
                    row["BarCode"] = gridView1.GetRowCellValue(i, "BarCode").ToString();
                    row["ItemName"] = gridView1.GetRowCellValue(i, ItemName).ToString();
                    row["SizeName"] = gridView1.GetRowCellValue(i, "BAGET_W").ToString();
                    row["QTY"] = gridView1.GetRowCellValue(i, "QTY").ToString();
                    row["Total"] = gridView1.GetRowCellValue(i, "SalePrice").ToString();
                    row["Discount"] = gridView1.GetRowCellValue(i, SizeName).ToString();
                    row["AdditionalValue"] = gridView1.GetRowCellValue(i, "AdditionalValue").ToString();
                    row["Net"] = gridView1.GetRowCellValue(i, "Net").ToString();
                    row["CostPrice"] = gridView1.GetRowCellValue(i, "CostPrice").ToString();
                    row["Description"] = gridView1.GetRowCellValue(i, "Description").ToString();
                    row["Bones"] = gridView1.GetRowCellValue(i, "STONE_W").ToString();
                    row["ExpiryDate"] = gridView1.GetRowCellValue(i, "DIAMOND_W").ToString();
                    row["CLARITY"] = gridView1.GetRowCellValue(i, "CLARITY").ToString();
                    row["Color"] = gridView1.GetRowCellValue(i, "Color").ToString();
                    row["Serials"] = gridView1.GetRowCellValue(i, "Serials").ToString();
                    dataTable.Rows.Add(row);

                }
                rptForm.DataSource = dataTable;
                rptForm.DataMember = ReportName;
                /******************** Report Binding ************************/
                XRSubreport subreport = (XRSubreport)rptForm.FindControl("subRptCompanyHeader", true);
                subreport.Visible = IncludeHeader;
                subreport.ReportSource = ReportComponent.CompanyHeader();
                rptForm.ShowPrintStatusDialog = false;
                rptForm.ShowPrintMarginsWarning = false;
                rptForm.CreateDocument();

                SplashScreenManager.CloseForm(false);
                ShowReportInReportViewer = true;
                if (ShowReportInReportViewer)
                {
                    frmReportViewer frmRptViewer = new frmReportViewer();
                    frmRptViewer.documentViewer1.DocumentSource = rptForm;
                    frmRptViewer.ShowDialog();
                }
                else
                {
                    bool IsSelectedPrinter = false;
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                    DataTable dt = ReportComponent.SelectRecord("SELECT *  from Printers where ReportName='" + ReportName + "'");
                    if (dt.Rows.Count > 0)
                        for (int i = 1; i < 6; i++)
                        {
                            string PrinterName = dt.Rows[0]["PrinterName" + i.ToString()].ToString().ToUpper();
                            if (!string.IsNullOrEmpty(PrinterName))
                            {
                                rptForm.PrinterName = PrinterName;
                                rptForm.Print(PrinterName);
                                IsSelectedPrinter = true;
                            }
                        }
                    SplashScreenManager.CloseForm(false);
                    if (!IsSelectedPrinter)
                        Messages.MsgWarning(Messages.TitleWorning, Messages.msgThereIsNotPrinterSelected);
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        protected override void DoPrint()
        {

            PrintPyCostPrice();
        }

        #endregion
        #endregion
        #region Event

        #region Validating


        private void txtOrderID_Validating(object sender, CancelEventArgs e)
        {
            if (FormView == true)
                ReadRecord(Comon.cLong(txtOrderID.Text));
            else
            {
                Messages.MsgInfo(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord);
                return;
            }

        }
        private void txtStoreID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE AccountID =" + Comon.cLong(txtStoreID.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                CSearch.ControlValidating(txtStoreID, lblStoreName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

        }



        private void txtCustomerID_Validating(object sender, CancelEventArgs e)
        {



        }

        private void txtSellerID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as SellerName FROM Sales_Sellers WHERE SellerID =" + txtSellerID.Text + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                CSearch.ControlValidating(txtSellerID, lblSellerName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void txtDiscountOnTotal_Validating(object sender, CancelEventArgs e)
        {
            try
            {

                if (txtDiscountOnTotal.Text != string.Empty & lblInvoiceTotal.Text != string.Empty)
                {
                    decimal DiscountOnTotal = Comon.ConvertToDecimalPrice(txtDiscountOnTotal.Text);
                    decimal whole = Comon.ConvertToDecimalPrice(lblInvoiceTotal.Text);

                    decimal TotalDiscount = DiscountOnTotal;
                    if (Comon.ConvertToDecimalPrice(txtDiscountOnTotal.Text) != 0)
                    {
                        txtDiscountPercent.Text = ((DiscountOnTotal / whole) * 100).ToString("N" + MySession.GlobalPriceDigits);
                        decimal TotalDiscountPercent = Comon.ConvertToDecimalPrice((((TotalDiscount) / whole) * 100).ToString("N" + MySession.GlobalPriceDigits));
                        if (TotalDiscountPercent > MySession.GlobalDiscountPercentOnTotal)
                        {
                            Messages.MsgError(Messages.TitleError, Messages.msgNotAllowedPercentDiscount);
                            txtDiscountPercent.Text = "0";
                            txtDiscountOnTotal.Text = "0";
                            txtDiscountOnTotal.Focus();
                            return;
                        }
                    }
                    else
                    {
                        txtDiscountPercent.Text = "0";
                    }
                    // if (Comon.ConvertToDecimalPrice(txtDiscountOnTotal.Text) > 0 && !MySession.GlobalAllowedPercentDiscount) { Messages.MsgError(Messages.TitleError, Messages.msgNotAllowedPercentDiscount); return; }

                    DiscountOnTotal = Comon.ConvertToDecimalPrice(txtDiscountOnTotal.Text);
                }
                CalculateRow();
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);

            }
        }
        private void txtDiscountPercent_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (txtDiscountPercent.Text != string.Empty & lblInvoiceTotal.Text != string.Empty)
                {
                    decimal percent = Comon.ConvertToDecimalPrice(txtDiscountPercent.Text);
                    decimal whole = Comon.ConvertToDecimalPrice(lblInvoiceTotal.Text);
                    if (Comon.ConvertToDecimalPrice(txtDiscountOnTotal.Text) != Comon.ConvertToDecimalPrice(Math.Round(((percent * whole) / 100))) && Comon.ConvertToDecimalPrice(txtDiscountOnTotal.Text) == 0)
                    {
                        txtDiscountOnTotal.Text = ((percent * whole) / 100).ToString("N" + MySession.GlobalPriceDigits);

                        decimal DiscountOnTotal = Comon.ConvertToDecimalPrice(txtDiscountOnTotal.Text);

                        lblDiscountTotal.Text = (DiscountOnTotal).ToString("N" + MySession.GlobalPriceDigits);
                        txtDiscountOnTotal_Validating(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);

            }

        }







        #endregion
        #region Search
        /***************************Event Search ***************************/
        private void btnDebitSearch_Click(object sender, EventArgs e)
        {
            try
            {


            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }






        #endregion
        /************************Event From **************************/
        private void frmSaleInvoice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
                Find();
            else if (e.KeyCode == Keys.F2)
                ShortcutOpen();
        }
        /*******************Event CheckBoc***************************/

        public void chForVat_EditValueChanged(object sender, EventArgs e)
        {
            decimal Total = 0;
            decimal Net = 0;
            decimal DiscountTotal = 0;
            decimal DiscountOnTotal = 0;
            decimal AdditionalAmount = 0;
            decimal DiscountRow = 0;
            decimal QTYRow = 0;
            decimal CostPriceRow = 0;
            decimal TotalRow = 0;
            decimal NetRow = 0;
            decimal AdditionalAmountRow = 0;
            DataTable dtItem = new DataTable();

            dtItem.Columns.Add("ID", System.Type.GetType("System.String"));
            dtItem.Columns.Add("FacilityID", System.Type.GetType("System.String"));
            dtItem.Columns.Add("ItemID", System.Type.GetType("System.String"));
            dtItem.Columns.Add("SizeID", System.Type.GetType("System.String"));
            dtItem.Columns.Add("Description", System.Type.GetType("System.String"));
            dtItem.Columns.Add("StoreID", System.Type.GetType("System.String"));
            dtItem.Columns.Add("Discount", System.Type.GetType("System.String"));
            dtItem.Columns.Add("AdditionalValue", System.Type.GetType("System.String"));
            dtItem.Columns.Add("Net", System.Type.GetType("System.String"));
            dtItem.Columns.Add("Cancel", System.Type.GetType("System.String"));
            dtItem.Columns.Add("BarCode", System.Type.GetType("System.String"));
            dtItem.Columns.Add(ItemName, System.Type.GetType("System.String"));
            dtItem.Columns.Add(SizeName, System.Type.GetType("System.String"));

            dtItem.Columns.Add("GroupID", System.Type.GetType("System.String"));
            dtItem.Columns.Add(GroupName, System.Type.GetType("System.String"));
            dtItem.Columns.Add("QTY", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("CostPrice", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("Total", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("ExpiryDateStr", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("ExpiryDate", System.Type.GetType("System.DateTime"));
            dtItem.Columns.Add("Bones", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("HavVat", System.Type.GetType("System.Boolean"));
            dtItem.Columns.Add("SalePrice", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("Serials", System.Type.GetType("System.String"));
            dtItem.Columns.Add("BAGET_W", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("STONE_W", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("DIAMOND_W", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("Color", System.Type.GetType("System.String"));
            dtItem.Columns.Add("CLARITY", System.Type.GetType("System.String"));
            dtItem.Columns.Add("CaratPrice", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("SpendPrice", System.Type.GetType("System.Decimal"));

            for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
            {
                dtItem.Rows.Add();
                dtItem.Rows[i]["ID"] = i;
                dtItem.Rows[i]["FacilityID"] = UserInfo.FacilityID; ;
                dtItem.Rows[i]["BarCode"] = gridView1.GetRowCellValue(i, "BarCode").ToString();
                dtItem.Rows[i]["ItemID"] = Comon.cInt(gridView1.GetRowCellValue(i, "ItemID").ToString());
                dtItem.Rows[i]["SizeID"] = Comon.cInt(gridView1.GetRowCellValue(i, "SizeID").ToString());
                dtItem.Rows[i][ItemName] = gridView1.GetRowCellValue(i, ItemName).ToString();
                dtItem.Rows[i][SizeName] = gridView1.GetRowCellValue(i, SizeName).ToString();
                dtItem.Rows[i]["GroupID"] = gridView1.GetRowCellValue(i, "GroupID").ToString();
                dtItem.Rows[i][GroupName] = gridView1.GetRowCellValue(i, GroupName).ToString();
                dtItem.Rows[i]["QTY"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "QTY").ToString());
                dtItem.Rows[i]["SalePrice"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "SalePrice").ToString()); ;
                dtItem.Rows[i]["Description"] = gridView1.GetRowCellValue(i, "Description").ToString();
                dtItem.Rows[i]["StoreID"] = Comon.cInt(gridView1.GetRowCellValue(i, "StoreID").ToString());
                dtItem.Rows[i]["Discount"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Discount").ToString());
                dtItem.Rows[i]["ExpiryDateStr"] = Comon.ConvertDateToSerial(gridView1.GetRowCellValue(i, "ExpiryDate").ToString());
                dtItem.Rows[i]["ExpiryDate"] = "01/01/1900";
                dtItem.Rows[i]["CostPrice"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "CostPrice").ToString());
                dtItem.Rows[i]["Total"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Total").ToString());
                dtItem.Rows[i]["Bones"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Bones").ToString());






                dtItem.Rows[i]["Net"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Net").ToString());
                dtItem.Rows[i]["Serials"] = gridView1.GetRowCellValue(i, "Serials").ToString();
                dtItem.Rows[i]["BAGET_W"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "BAGET_W").ToString());
                dtItem.Rows[i]["STONE_W"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "STONE_W").ToString());
                dtItem.Rows[i]["DIAMOND_W"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "DIAMOND_W").ToString());
                dtItem.Rows[i]["Color"] = gridView1.GetRowCellValue(i, "Color").ToString();
                dtItem.Rows[i]["CLARITY"] = gridView1.GetRowCellValue(i, "CLARITY").ToString();
                dtItem.Rows[i]["CaratPrice"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "CaratPrice").ToString());
                dtItem.Rows[i]["SpendPrice"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "SpendPrice").ToString());
                dtItem.Rows[i]["Cancel"] = 0;

            }
            gridControl.DataSource = dtItem;
            gridView1.Focus();
            gridView1.MoveNext();
            gridView1.FocusedColumn = gridView1.VisibleColumns[1];
            CalculateRow();
        }

        #region Event TextEdit
        private void PublicTextEdit_EditValueChanged(object sender, EventArgs e)
        {
            ((TextEdit)sender).Properties.Appearance.BorderColor = Color.Black;

        }
        private void PublicTextEdit_Enter(object sender, EventArgs e)
        {
            (sender as DateEdit).ShowPopup();
        }
        private void PublicTextEdit_Click(object sender, EventArgs e)
        {
            (sender as DateEdit).ShowPopup();
        }

        #endregion
        #region Event Combox






        #endregion
        #endregion
        #region InitializeComponent
        private void RolesButtonSearchAccountID()
        {

            btnDebitSearch.Enabled = MySession.GlobalAllowChangefrmSaleDebitAccountID;
            btnCreditSearch.Enabled = MySession.GlobalAllowChangefrmSaleCreditAccountID;
            btnAdditionalSearch.Enabled = MySession.GlobalAllowChangefrmSaleAdditionalAccountID;
            btnNetSearch.Enabled = MySession.GlobalAllowChangefrmSaleNetAccountID;
            btnChequeSearch.Enabled = MySession.GlobalAllowChangefrmSaleChequeAccountID;
            btnDiscountDebitSearch.Enabled = MySession.GlobalAllowChangefrmSaleDiscountDebitAccountID;


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


        #endregion


















        private void txtInvoiceDate_EditValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtInvoiceDate.Text.Trim()))
                txtInvoiceDate.EditValue = DateTime.Now;
            //if (Comon.ConvertDateToSerial(txtInvoiceDate.Text) > Comon.cLong((Lip.GetServerDateSerial())))
            //    txtInvoiceDate.Text = Lip.GetServerDate();
            if (Lip.CheckDateISAvilable(txtInvoiceDate.Text))
            {
                Messages.MsgWarning(Messages.TitleWorning, Messages.msgTheDateGreaterCurrntDate);
                txtInvoiceDate.Text = Lip.GetServerDate();
                return;
            }
        }


        private void showCustomers(bool p, int f)
        {

            txtVatID.Text = "";
            labelControl6.Visible = p;
            labelControl4.BringToFront();
            labelControl4.Visible = p;
            txtVatID.Visible = p;

        }

        private void EmportItemsFromDb(DataTable dtitems)
        {

            bool Yes = Messages.MsgStopYesNo(Messages.TitleConfirm, "تأكيد الاسنيراد  ؟");
            if (!Yes)
                return;

            Application.DoEvents();

            if (dtitems.Rows.Count < 1)
                return;
            lstDetail = new BindingList<Sales_SalesInvoiceDetails>();
            lstDetail.AllowNew = true;
            lstDetail.AllowEdit = true;
            lstDetail.AllowRemove = true;
            gridControl.DataSource = lstDetail;

            Sales_SalesInvoiceDetails obj = new Sales_SalesInvoiceDetails();


            for (int i = 0; i <= dtitems.Rows.Count - 1; i++)
            {
                obj = new Sales_SalesInvoiceDetails();
                obj.Serials = dtitems.Rows[i]["Serials"].ToString();
                obj.BarCode = dtitems.Rows[i]["BarCode"].ToString();
                obj.ArbItemName = dtitems.Rows[i]["ItemName"].ToString();
                obj.EngItemName = dtitems.Rows[i]["ItemName"].ToString();
                obj.GroupID = Comon.cDbl(dtitems.Rows[i]["GroupID"].ToString());
                obj.DIAMOND_W = Comon.ConvertToDecimalPrice(dtitems.Rows[i]["DIAMOND_W"].ToString());
                obj.CostPrice = Comon.ConvertToDecimalPrice(dtitems.Rows[i]["CostPrice"].ToString());
                obj.QTY = Comon.ConvertToDecimalPrice(dtitems.Rows[i]["QTY"].ToString());
                obj.Caliber = 18;
                obj.STONE_W = Comon.cDec(dtitems.Rows[i]["STONE_W"].ToString());

                obj.Color = dtitems.Rows[i]["Color"].ToString();
                obj.CLARITY = dtitems.Rows[i]["ThePurity"].ToString();

                decimal CostPrice = Comon.ConvertToDecimalPrice(obj.CostPrice.ToString());
                decimal additonalVAlue = 0;
                if(MySession.GlobalHaveVat=="1")
                  additonalVAlue = Comon.ConvertToDecimalPrice((CostPrice * MySession.GlobalPercentVat) / 100);
                //سعر تكلفة مع مصاريف
                decimal SpendPrice = Comon.ConvertToDecimalPrice(CostPrice);
                //سعر تكلفة المحل
                decimal CaratPrice = Comon.ConvertToDecimalPrice(SpendPrice * Comon.cDec(MySession.Cost));
                //سعر الكارت وهو البيع
                decimal SalePrice = Comon.ConvertToDecimalPrice(CaratPrice * Comon.cDec(MySession.sumvalue));

                obj.AdditionalValue = additonalVAlue;
                obj.SalePrice = SalePrice;
                obj.SpendPrice = SpendPrice;
                obj.CaratPrice = CaratPrice;
                obj.Total = CostPrice;
                obj.Net = SpendPrice;
                obj.ArbGroupName = Lip.GetValue("Select  ArbName from Stc_ItemsGroups Where GroupID=" + obj.GroupID + " And BranchID =" + MySession.GlobalBranchID);
                obj.EngGroupName = obj.ArbGroupName;


                obj.ArbSizeName = obj.Caliber.ToString();
                obj.EngSizeName = obj.Caliber.ToString();


                lstDetail.Add(obj);

            }
            SumTotalBalanceAndDiscountread();

            gridControl.DataSource = lstDetail;
            SplashScreenManager.CloseForm(false);
        }
        private void txtSupplierID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                string strSql;
                DataTable dt;
                if (txtCustomerID.Text != string.Empty && txtCustomerID.Text != "0")
                {
                    strSQL = "SELECT " + PrimaryName + " as CustomerName ,VATID,Mobile FROM Sales_CustomerAnSublierListArb Where    AcountID =" + txtCustomerID.Text + " And BranchID =" + MySession.GlobalBranchID;
                    Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, UserInfo.Language.ToString());
                    dt = Lip.SelectRecord(strSQL);
                    if (dt.Rows.Count > 0)
                    {
                        lblSupplierName.Text = dt.Rows[0]["CustomerName"].ToString();
                        txtCustomerMobile.Text = dt.Rows[0]["Mobile"].ToString();


                        if (Comon.cLong(dt.Rows[0]["VATID"]) > 0)
                            txtVatID.Text = dt.Rows[0]["VATID"].ToString();
                        else
                            txtVatID.Text = "";


                    }
                    else
                    {
                        strSql = "SELECT " + PrimaryName + " as CustomerName,SpecialDiscount,VATID, CustomerID,Mobile   FROM Sales_Customers Where  Cancel =0 And  AccountID =" + txtCustomerID.Text + " And BranchID =" + MySession.GlobalBranchID;
                        Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSql, UserInfo.Language.ToString());
                        dt = Lip.SelectRecord(strSql);
                        if (dt.Rows.Count > 0)
                        {

                            lblSupplierName.Text = dt.Rows[0]["CustomerName"].ToString();
                            txtCustomerMobile.Text = dt.Rows[0]["Mobile"].ToString();

                            if (Comon.cLong(dt.Rows[0]["VATID"]) > 0)
                                txtVatID.Text = dt.Rows[0]["VATID"].ToString();
                            else
                                txtVatID.Text = "";
                        }
                        else
                        {
                            lblSupplierName.Text = "";
                            txtCustomerID.Text = "";
                            txtVatID.Text = "";
                        }
                    }
                }
                else
                {
                    lblSupplierName.Text = "";
                    txtCustomerID.Text = "";
                    txtVatID.Text = "";
                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void btnemport_Click(object sender, EventArgs e)
        {
        label1: if (txtExcelPath.Text == string.Empty)
            {
                Messages.MsgError(Messages.TitleConfirm, "يجب تحديد مسار ملف الأكسل");
                txtExcelPath.Focus();
                simpleButton18_Click(null, null);
                goto label1;
            }

            EmportItems();
            txtExcelPath.Text = "";
            Messages.MsgInfo(Messages.TitleConfirm, "تم الاستيراد بنجاح - يجب حفظ عملية الاستيراد");

        }

        private void simpleButton18_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog1 = new OpenFileDialog();
                OpenFileDialog1.Filter = "All Files|*.*";
                OpenFileDialog1.FileName = "";
                OpenFileDialog1.ShowDialog();
                if ((OpenFileDialog1.FileName != ""))
                {
                    txtExcelPath.Text = OpenFileDialog1.FileName;

                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);

            }
        }

        private void btnprintBarcode_Click(object sender, EventArgs e)
        {
            frmPrintItemSticker frm = new frmPrintItemSticker();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
                BindingSource bs = new BindingSource();
                bs.DataSource = gridControl.DataSource;
                frm.Show();
                frm.gridControl.DataSource = bs;

            }
            else
                frm.Dispose();
        }


        private void SaveImage(byte[] data, int ItemID)
        {
            try
            {

                SqlConnection Con = new GlobalConnection().Conn;
                if (Con.State == ConnectionState.Closed)
                    Con.Open();

                SqlCommand sc;
                sc = new SqlCommand("Update  Stc_Items Set ItemImage=@p Where ItemID=" + ItemID + " And BranchID =" + MySession.GlobalBranchID, Con);
                sc.Parameters.AddWithValue("@p", data);
                sc.ExecuteNonQuery();

            }
            catch
            {

            }
        }

        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }

        private void lnkAddImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                OpenFileDialog1 = new OpenFileDialog();
                OpenFileDialog1.Filter = "All Files|*.*|Bitmaps|*.bmp|GIFs|*.gif|JPEGs|*.jpg";
                OpenFileDialog1.FileName = "";
                OpenFileDialog1.ShowDialog();
                if ((OpenFileDialog1.FileName != ""))
                {

                    picItemImage.Image = Image.FromFile(OpenFileDialog1.FileName);
                    picItemImage.Visible = true;
                    byte[] Imagebyte = imageToByteArray(picItemImage.Image);
                    SaveImage(Imagebyte, ItemIDImage);

                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);

            }
        }
        private void cmbCurency_EditValueChanged(object sender, EventArgs e)
        {
            int isLocalCurrncy = Comon.cInt(Lip.GetValue("select TypeCurrency from Acc_Currency where ID=" + Comon.cInt(cmbCurency.EditValue) + " and Cancel=0 And BranchID =" + MySession.GlobalBranchID));
            if (isLocalCurrncy > 1)
            {
                decimal CurrncyPrice = Comon.cDec(Lip.GetValue("select ExchangeRate from Acc_Currency where ID=" + Comon.cInt(cmbCurency.EditValue) + " and Cancel=0 And BranchID =" + MySession.GlobalBranchID));
                txtCurrncyPrice.Text = CurrncyPrice + "";
                lblCurrencyEqv.Visible = true;
                lblCurrncyPric.Visible = true;
                lblcurrncyEquvilant.Visible = true;
                txtCurrncyPrice.Visible = true;
                gridView1.Columns["CurrencyEquivalent"].Visible = true;
            }
            else
            {
                txtCurrncyPrice.Text = "1";
                lblCurrencyEqv.Visible = false;
                lblCurrncyPric.Visible = false;
                lblcurrncyEquvilant.Visible = false;
                txtCurrncyPrice.Visible = false;
                gridView1.Columns["CurrencyEquivalent"].Visible = false;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            frmCashierSalesAlmas frm = new frmCashierSalesAlmas();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.txtOrderID.Text = this.txtOrderID.Text;                
                frm.Show();
                frm.txtOrderID_Validating(null, null);
            }
        }

        private void frmCashierSalesOrder_Load(object sender, EventArgs e)
        {
            DoNew();
        }

        private void frmCashierSalesOrder_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F9)
            {
                falgPrint = true;
                DoSave();
            }
            if (e.KeyCode == Keys.F6)
            {
                DoSave();
            }
        }









    }
}
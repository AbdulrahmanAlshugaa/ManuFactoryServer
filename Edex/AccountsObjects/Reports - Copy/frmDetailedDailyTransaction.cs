using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraReports.UI;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraTreeList;
using Edex.Model;
using Edex.Model.Language;
using Edex.GeneralObjects.GeneralForms;
using Edex.ModelSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using Edex.SalesAndPurchaseObjects.Transactions;
using Edex.StockObjects.Transactions;
using Edex.StockObjects.Codes;
using Edex.AccountsObjects.Transactions;
using Edex.SalesAndSaleObjects.Transactions;
namespace Edex.AccountsObjects.Reports
{
    public partial class frmDetailedDailyTransaction : Edex.GeneralObjects.GeneralForms.BaseForm
    {
        #region Declare
        private DataTable dtMain;
        public DataTable _sampleData = new DataTable();

        private string strSQL = "";
        private string Where = "";
        private string lang = "Arb";
        private string FocusedControl = "";
        private string PrimaryName;
        #endregion

        public frmDetailedDailyTransaction()
        {
            try
            {

                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
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
                ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Visible = true;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Caption = (UserInfo.Language == iLanguage.Arabic ? "استعلام جديد" : "New Query");
                InitializeFormatDate(txtFromDate);
                InitializeFormatDate(txtToDate);
                GridView1.OptionsView.EnableAppearanceEvenRow = true;
                GridView1.OptionsView.EnableAppearanceOddRow = true;
                GridView1.OptionsBehavior.ReadOnly = true;
                GridView1.OptionsBehavior.Editable = false;
                Where = "FACILITYID=" + MySession.GlobalFacilityID + " AND BRANCHID=" + MySession.GlobalBranchID;
                lang = "Arb";
                strSQL = "ArbName";
                PrimaryName = "ArbName";
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, "Arb");
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(PrimaryName, "Arb");
                if (MySession.GlobalLanguageName == iLanguage.English)
                {
                    lang = "Eng";
                    strSQL = "EngName";
                    PrimaryName = "EngName";
                    Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, "Eng");
                    Lip.ConvertStrSQLToEnglishOrArabicLanguage(PrimaryName, "Eng");
                }
                if (MySession.GlobalAllowWhenClickOpenPopup)
                {
                    //this.txtFromDate.Click += new System.EventHandler(this.PublicTextEdit_Click);
                    //this.txtToDate.Click += new System.EventHandler(this.PublicTextEdit_Click);
                }
                else if (MySession.GlobalAllowWhenEnterOpenPopup)
                {
                    //this.txtFromDate.Enter += new System.EventHandler(this.PublicTextEdit_Enter);
                    //this.txtToDate.Enter += new System.EventHandler(this.PublicTextEdit_Enter);
                }

                this.btnShow.Click += new System.EventHandler(this.btnShow_Click);

                /***************************** Event For GridView *****************************/
                GridView1.OptionsView.EnableAppearanceEvenRow = true;
                GridView1.OptionsView.EnableAppearanceOddRow = true;
                this.KeyPreview = true;
                this.GridView1.InvalidRowException += new DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventHandler(this.GridView1_InvalidRowException);
                this.GridView1.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.GridView1_CustomUnboundColumnData);
                this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmDetailedDailyTransaction_KeyDown);
                this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmDetailedDailyTransaction_KeyUp);
                //this.GridView1.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.GridView1_RowClick);

                if ((MySession.GlobalLanguageName == iLanguage.Arabic))
                {
                    strSQL = ("SELECT * FROM DetailedDailyTransactionArb Where BranchID ="
                                + (MySession.GlobalBranchID + (" AND TheDate ="
                                + (Comon.ConvertDateToSerial(Lip.GetServerDate()) + "  ORDER BY TheDate"))));
                }
                else
                {
                    strSQL = ("SELECT * FROM DetailedDailyTransactionEng Where BranchID ="
                                + (MySession.GlobalBranchID + (" AND TheDate ="
                                + (Comon.ConvertDateToSerial(Lip.GetServerDate()) + "  ORDER BY TheDate"))));
                }
                txtFromDate.EditValue = DateTime.Now.AddDays(-1);
                txtToDate.EditValue = DateTime.Now;
                GridView();
                SplashScreenManager.CloseForm(false);
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

        #region Function
        void GridView()
        {
            _sampleData.Columns.Add(new DataColumn("ID", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("TheDate", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("AccountName", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Debit", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("Credit", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("PostPonedDebit", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("PostPonedCredit", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("NetDebit", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("NetCredit", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("ChequeDebit", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("ChequeCredit", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("VariousVoucher", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("OppsiteAccountName", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Declaration", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("RecordType", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("TempRecordType", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("MethodeID", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("UserName", typeof(string)));
            /************************ Auto Number **************************/
            DevExpress.XtraGrid.Columns.GridColumn col = GridView1.Columns.AddVisible("#");
            col.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            col.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            col.OptionsColumn.AllowEdit = false;
            col.OptionsColumn.ReadOnly = true;
            col.OptionsColumn.AllowFocus = false;
            col.Width = 20;
            GridView1.BestFitColumns();
   
         
            /************************ Fill Grid View **************************/

            FillGridView(strSQL);


        }
        public void FillGridView(string strSQL)
        {
            try
            {

                DataTable dtCredit = new DataTable();
                DataTable dtDebit = new DataTable();
                DataRow row;

                decimal Debit = 0;
                decimal Credit = 0;
                decimal PostPonedDebit = 0;
                decimal PostPonedCredit = 0;
                decimal NetDebit = 0;
                decimal NetCredit = 0;
                decimal ChequeDebit = 0;
                decimal ChequeCredit = 0;
                decimal VariousVoucher = 0;
           
                ClearRows();
                dtMain = Lip.SelectRecord(strSQL);
                if (dtMain != null && dtMain.Rows.Count > 0)
                {
                    for (int i = 0; i <= dtMain.Rows.Count - 1; i++)
                    {
                        row = _sampleData.NewRow();
                        _sampleData.Rows.Add(row);
                        row["ID"] = dtMain.Rows[i]["ID"];
                        row["TheDate"] = Comon.ConvertSerialDateTo (dtMain.Rows[i]["TheDate"].ToString());
                        row["Declaration"] = dtMain.Rows[i]["Declaration"].ToString();
                        row["AccountName"] = dtMain.Rows[i]["AccountName"];
                        row["OppsiteAccountName"] = dtMain.Rows[i]["OppsiteAccountName"];
                        row["MethodeID"] = dtMain.Rows[i]["MethodeID"];
                        row["RecordType"] = CaseRecordType(dtMain.Rows[i]["RecordType"].ToString(), Comon.cLong(dtMain.Rows[i]["ID"]), Comon.cInt(dtMain.Rows[i]["MethodeID"]), i);
                        row["TempRecordType"] = dtMain.Rows[i]["RecordType"];
                        row["UserName"] = dtMain.Rows[i]["UserName"];
                    }

                    for (int i = 0; i <= _sampleData.Rows.Count - 1; i++)
                    {
                        Debit += Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Debit"]);
                        Credit += Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Credit"]);
                        PostPonedDebit += Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["PostPonedDebit"]);
                        PostPonedCredit += Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["PostPonedCredit"]);
                        NetDebit += Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["NetDebit"]);
                        NetCredit += Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["NetCredit"]);
                        ChequeDebit += Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["ChequeDebit"]);
                        ChequeCredit += Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["ChequeCredit"]);
                        VariousVoucher += Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["VariousVoucher"]);
                    }
                    row = _sampleData.NewRow();
                    row["Debit"] = Debit;
                    row["Credit"] = Credit;
                    row["PostPonedDebit"] = PostPonedDebit;
                    row["PostPonedCredit"] = PostPonedCredit;
                    row["NetDebit"] = NetDebit;
                    row["NetCredit"] = NetCredit;
                    row["ChequeDebit"] = ChequeDebit;
                    row["ChequeCredit"] = ChequeCredit;
                    row["VariousVoucher"] = VariousVoucher;
                    row["TheDate"] = "";
                    row["Declaration"] = "";
                    row["AccountName"] = "";
                    row["OppsiteAccountName"] = "";
                    row["MethodeID"] = "";
                    row["RecordType"] = "";
                    row["UserName"] = "";
                    row["ID"] = "";
                    row["TempRecordType"] = "";
                    row["ID"] = MySession.GlobalLanguageName == iLanguage.English ? "Total" : "الاجـمـالي";
                    _sampleData.Rows.Add(row);
                    gridControl1.DataSource = _sampleData.DefaultView;
             
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
        private void CheckSpendVoucherCreditBalance(long id, int i)
        {
            try
            {
                DataTable dtTotal, dt;
                decimal NetBalance = 0;
                string strSQL;

                strSQL = "SELECT CreditAmount - DiscountAmount AS NetBalance FROM dbo.Acc_CheckSpendVoucherMaster "
                + " WHERE (Cancel = 0) AND (BranchID = " + MySession.GlobalBranchID + ") AND (CheckSpendVoucherID = " + id + ") ";
                dtTotal = Lip.SelectRecord(strSQL);
                if (dtTotal.Rows.Count > 0)
                {
                    NetBalance = Comon.ConvertToDecimalPrice(dtTotal.Rows[0]["NetBalance"]);
                    _sampleData.Rows[i]["ChequeCredit"] = NetBalance.ToString("N" + MySession.GlobalPriceDigits);
                }

                strSQL = "SELECT dbo.Acc_Accounts.ArbName FROM dbo.Acc_CheckSpendVoucherMaster INNER JOIN dbo.Acc_CheckSpendVoucherDetails ON "
                + " dbo.Acc_CheckSpendVoucherMaster.CheckSpendVoucherID = dbo.Acc_CheckSpendVoucherDetails.CheckSpendVoucherID AND dbo.Acc_CheckSpendVoucherMaster.BranchID "
                + " = dbo.Acc_CheckSpendVoucherDetails.BranchID LEFT OUTER JOIN dbo.Acc_Accounts ON dbo.Acc_CheckSpendVoucherDetails.BranchID = dbo.Acc_Accounts.BranchID"
                + " AND dbo.Acc_CheckSpendVoucherDetails.AccountID = dbo.Acc_Accounts.AccountID WHERE (dbo.Acc_CheckSpendVoucherMaster.Cancel = 0) "
                + " AND (dbo.Acc_CheckSpendVoucherMaster.BranchID = " + MySession.GlobalBranchID + ") AND (dbo.Acc_CheckSpendVoucherMaster.CheckSpendVoucherID = " + id + ")";
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, lang);
                dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count == 1)
                    _sampleData.Rows[i]["AccountName"] = dt.Rows[0][0];
                else
                    _sampleData.Rows[i]["AccountName"] = (MySession.GlobalLanguageName == iLanguage.English ? "Mentioned" : "مذكورين");
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void CheckReceiptVoucherDebitBalance(long id, int i)
        {
            try
            {
                DataTable dtTotal, dt;
                decimal NetBalance = 0;
                string strSQL;

                strSQL = "SELECT DebitAmount - DiscountAmount AS NetBalance"
                + " FROM dbo.Acc_CheckReceiptVoucherMaster WHERE (Cancel = 0) AND "
                + " (BranchID = " + MySession.GlobalBranchID + ") AND (CheckReceiptVoucherID = " + id + ")";
                dtTotal = Lip.SelectRecord(strSQL);
                if (dtTotal.Rows.Count > 0)
                {
                    NetBalance = Comon.ConvertToDecimalPrice(dtTotal.Rows[0]["NetBalance"]);
                    _sampleData.Rows[i]["ChequeDebit"] = NetBalance.ToString("N" + MySession.GlobalPriceDigits);
                }

                strSQL = "SELECT dbo.Acc_Accounts.ArbName FROM dbo.Acc_CheckReceiptVoucherMaster INNER JOIN dbo.Acc_CheckReceiptVoucherDetails ON "
                + " dbo.Acc_CheckReceiptVoucherMaster.CheckReceiptVoucherID = dbo.Acc_CheckReceiptVoucherDetails.CheckReceiptVoucherID AND  "
                + " dbo.Acc_CheckReceiptVoucherMaster.BranchID = dbo.Acc_CheckReceiptVoucherDetails.BranchID LEFT OUTER JOIN dbo.Acc_Accounts "
                + " ON dbo.Acc_CheckReceiptVoucherDetails.BranchID = dbo.Acc_Accounts.BranchID AND dbo.Acc_CheckReceiptVoucherDetails.AccountID"
                + " = dbo.Acc_Accounts.AccountID WHERE (dbo.Acc_CheckReceiptVoucherMaster.Cancel = 0) AND "
                + " (dbo.Acc_CheckReceiptVoucherMaster.BranchID = " + MySession.GlobalBranchID + ") AND (dbo.Acc_CheckReceiptVoucherMaster.CheckReceiptVoucherID = " + id + ")";
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, lang);
                dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count == 1)
                    _sampleData.Rows[i]["OppsiteAccountName"] = dt.Rows[0][0];
                else
                    _sampleData.Rows[i]["OppsiteAccountName"] = (MySession.GlobalLanguageName == iLanguage.English ? "Mentioned" : "مذكورين");
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void PurchaseInvoiceCreditBalance(long id, long MethodeID, int i)
        {
            try
            {
                DataTable dtTotal;
                decimal TotalBalance = 0;
                decimal TotalDiscount = 0;
                decimal Net = 0;
                decimal SumVat = 0;
                string strSQL = "SELECT  SUM(dbo.Sales_PurchaseInvoiceDetails.QTY * dbo.Sales_PurchaseInvoiceDetails.CostPrice) AS TotalBalance,  Sales_PurchaseInvoiceMaster.AdditionaAmountTotal AS SumVat,"
                + " SUM(dbo.Sales_PurchaseInvoiceDetails.Discount) + dbo.Sales_PurchaseInvoiceMaster.DiscountOnTotal AS TotalDiscount"
                + " FROM dbo.Sales_PurchaseInvoiceMaster INNER JOIN dbo.Sales_PurchaseInvoiceDetails ON dbo.Sales_PurchaseInvoiceMaster.InvoiceID"
                + " = dbo.Sales_PurchaseInvoiceDetails.InvoiceID AND dbo.Sales_PurchaseInvoiceMaster.BranchID = dbo.Sales_PurchaseInvoiceDetails.BranchID"
                + " GROUP BY    Sales_PurchaseInvoiceMaster.AdditionaAmountTotal , dbo.Sales_PurchaseInvoiceMaster.InvoiceID, dbo.Sales_PurchaseInvoiceMaster.DiscountOnTotal, dbo.Sales_PurchaseInvoiceMaster.Cancel, "
                + " dbo.Sales_PurchaseInvoiceDetails.Cancel, dbo.Sales_PurchaseInvoiceMaster.BranchID HAVING (dbo.Sales_PurchaseInvoiceMaster.InvoiceID = " + id + ") "
                + " AND (dbo.Sales_PurchaseInvoiceMaster.Cancel = 0)  AND (dbo.Sales_PurchaseInvoiceDetails.Cancel = 0)"
                + "  AND (dbo.Sales_PurchaseInvoiceMaster.BranchID = " + MySession.GlobalBranchID + ")";
                dtTotal = Lip.SelectRecord(strSQL);
                if (dtTotal.Rows.Count > 0)
                {
                    TotalBalance = Comon.ConvertToDecimalPrice(dtTotal.Rows[0]["TotalBalance"]);
                    TotalDiscount = Comon.ConvertToDecimalPrice(dtTotal.Rows[0]["TotalDiscount"]);
                    SumVat = Comon.ConvertToDecimalPrice(dtTotal.Rows[0]["SumVat"]);

                    Net = TotalBalance - TotalDiscount + SumVat;
                    if (MethodeID == 1)
                        _sampleData.Rows[i]["Credit"] = Net.ToString("N" + MySession.GlobalPriceDigits);
                    else if (MethodeID == 2)
                        _sampleData.Rows[i]["PostPonedCredit"] = Net.ToString("N" + MySession.GlobalPriceDigits);
                    else if (MethodeID == 3)
                        _sampleData.Rows[i]["NetCredit"] = Net.ToString("N" + MySession.GlobalPriceDigits);
                    else if (MethodeID == 4)
                        _sampleData.Rows[i]["ChequeCredit"] = Net.ToString("N" + MySession.GlobalPriceDigits);
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void PurchaseInvoiceReturnCreditBalance(long id, long MethodeID, int i)
        {
            try
            {
                DataTable dtTotal;
                decimal TotalBalance = 0;
                decimal SumVat = 0;
                decimal TotalDiscount = 0;
                decimal Net = 0;

                string strSQL = "SELECT   SUM(dbo.Sales_PurchaseInvoiceReturnDetails.QTY * dbo.Sales_PurchaseInvoiceReturnDetails.CostPrice) AS TotalBalance,Sales_PurchaseInvoiceReturnMaster.AdditionaAmountTotal AS SumVat,"
                + " SUM(dbo.Sales_PurchaseInvoiceReturnDetails.Discount) + dbo.Sales_PurchaseInvoiceReturnMaster.DiscountOnTotal AS TotalDiscount "
                + " FROM dbo.Sales_PurchaseInvoiceReturnMaster INNER JOIN dbo.Sales_PurchaseInvoiceReturnDetails ON dbo.Sales_PurchaseInvoiceReturnMaster.InvoiceID "
                + " = dbo.Sales_PurchaseInvoiceReturnDetails.InvoiceID AND dbo.Sales_PurchaseInvoiceReturnMaster.BranchID = dbo.Sales_PurchaseInvoiceReturnDetails.BranchID "
                + " GROUP BY dbo.Sales_PurchaseInvoiceReturnMaster.InvoiceID,Sales_PurchaseInvoiceReturnMaster.AdditionaAmountTotal, dbo.Sales_PurchaseInvoiceReturnMaster.DiscountOnTotal, dbo.Sales_PurchaseInvoiceReturnMaster.Cancel,"
                + " dbo.Sales_PurchaseInvoiceReturnMaster.Cancel, dbo.Sales_PurchaseInvoiceReturnMaster.BranchID HAVING (dbo.Sales_PurchaseInvoiceReturnMaster.InvoiceID = " + id + ") "
                + " AND (dbo.Sales_PurchaseInvoiceReturnMaster.Cancel = 0) "
                + " AND (dbo.Sales_PurchaseInvoiceReturnMaster.BranchID = " + MySession.GlobalBranchID + ") ";
                dtTotal = Lip.SelectRecord(strSQL);
                if (dtTotal.Rows.Count > 0)
                {
                    TotalBalance = Comon.ConvertToDecimalPrice(dtTotal.Rows[0]["TotalBalance"]);
                    TotalDiscount = Comon.ConvertToDecimalPrice(dtTotal.Rows[0]["TotalDiscount"]);
                    Net = TotalBalance - TotalDiscount;
                    SumVat = Comon.ConvertToDecimalPrice(dtTotal.Rows[0]["SumVat"]);

                    if (MethodeID == 1)
                        _sampleData.Rows[i]["Debit"] = Net.ToString("N" + MySession.GlobalPriceDigits);
                    else if (MethodeID == 2)
                        _sampleData.Rows[i]["PostPonedDebit"] = Net.ToString("N" + MySession.GlobalPriceDigits);
                    else if (MethodeID == 3)
                        _sampleData.Rows[i]["NetDebit"] = Net.ToString("N" + MySession.GlobalPriceDigits);
                    else if (MethodeID == 4)
                        _sampleData.Rows[i]["ChequeDebit"] = Net.ToString("N" + MySession.GlobalPriceDigits);
                }
            }
            catch (Exception ex)
            {

                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void SalesInvoiceDebitBalance(long id, long MethodeID, int i)
        {
            try
            {
                DataTable dtTotal;
                decimal TotalBalance = 0;
                decimal TotalDiscount = 0;
                decimal Net = 0;
                decimal SumVat = 0;
                decimal NetAmount = 0;


                string strSQL = "SELECT SUM(dbo.Sales_SalesInvoiceDetails.QTY * dbo.Sales_SalesInvoiceDetails.SalePrice) AS TotalBalance , Sales_SalesInvoiceMaster.AdditionaAmountTotal AS SumVat,   Sales_SalesInvoiceMaster.NetAmount , "
                + " SUM(dbo.Sales_SalesInvoiceDetails.Discount) + dbo.Sales_SalesInvoiceMaster.DiscountOnTotal AS TotalDiscount "
                + " FROM dbo.Sales_SalesInvoiceMaster INNER JOIN dbo.Sales_SalesInvoiceDetails ON dbo.Sales_SalesInvoiceMaster.InvoiceID "
                + " = dbo.Sales_SalesInvoiceDetails.InvoiceID AND dbo.Sales_SalesInvoiceMaster.BranchID = dbo.Sales_SalesInvoiceDetails.BranchID"
                + " GROUP BY  Sales_SalesInvoiceMaster.NetAmount , Sales_SalesInvoiceMaster.AdditionaAmountTotal ,  dbo.Sales_SalesInvoiceMaster.InvoiceID, dbo.Sales_SalesInvoiceMaster.DiscountOnTotal, dbo.Sales_SalesInvoiceMaster.Cancel, "
                + " dbo.Sales_SalesInvoiceMaster.BranchID HAVING (dbo.Sales_SalesInvoiceMaster.Cancel = 0) AND (dbo.Sales_SalesInvoiceMaster.InvoiceID = " + id + ")"
                + " AND (dbo.Sales_SalesInvoiceMaster.BranchID = " + MySession.GlobalBranchID + ")";
                dtTotal = Lip.SelectRecord(strSQL);
                if (dtTotal.Rows.Count > 0)
                {
                    TotalBalance = Comon.ConvertToDecimalPrice(dtTotal.Rows[0]["TotalBalance"]);
                    TotalDiscount = Comon.ConvertToDecimalPrice(dtTotal.Rows[0]["TotalDiscount"]);
                    SumVat = Comon.ConvertToDecimalPrice(dtTotal.Rows[0]["SumVat"]);
                    NetAmount = Comon.ConvertToDecimalPrice(dtTotal.Rows[0]["NetAmount"]);

                    Net = TotalBalance - TotalDiscount + SumVat;

                    if (MethodeID == 1)
                        _sampleData.Rows[i]["Debit"] = Net.ToString("N" + MySession.GlobalPriceDigits);
                    else if (MethodeID == 2)
                        _sampleData.Rows[i]["PostPonedDebit"] = Net.ToString("N" + MySession.GlobalPriceDigits);
                    else if (MethodeID == 3)
                        _sampleData.Rows[i]["PostPonedDebit"] = Net.ToString("N" + MySession.GlobalPriceDigits);
                    else if (MethodeID == 4)
                        _sampleData.Rows[i]["ChequeDebit"] = Net.ToString("N" + MySession.GlobalPriceDigits);
                    else if (MethodeID == 5)
                    {
                        _sampleData.Rows[i]["Debit"] = Net.ToString("N" + MySession.GlobalPriceDigits);
                        _sampleData.Rows[i]["NetDebit"] = Net.ToString("N" + MySession.GlobalPriceDigits);
                    }
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void SpendVoucherCreditBalance(long id, int i)
        {
            try
            {
                DataTable dtTotal, dt;
                decimal NetBalance = 0;
                string strSQL;

                strSQL = "SELECT CreditAmount - DiscountAmount AS NetBalance FROM dbo.Acc_SpendVoucherMaster "
                + " WHERE (Cancel = 0) AND (BranchID = " + MySession.GlobalBranchID + ") AND (SpendVoucherID = " + id + ") ";
                dtTotal = Lip.SelectRecord(strSQL);
                if (dtTotal.Rows.Count > 0)
                {
                    NetBalance = Comon.ConvertToDecimalPrice(dtTotal.Rows[0]["NetBalance"]);
                    _sampleData.Rows[i]["Debit"] = NetBalance.ToString("N" + MySession.GlobalPriceDigits);
                }

                strSQL = "SELECT dbo.Acc_Accounts.ArbName FROM dbo.Acc_SpendVoucherMaster INNER JOIN dbo.Acc_SpendVoucherDetails ON "
                + " dbo.Acc_SpendVoucherMaster.SpendVoucherID = dbo.Acc_SpendVoucherDetails.SpendVoucherID AND dbo.Acc_SpendVoucherMaster.BranchID "
                + " = dbo.Acc_SpendVoucherDetails.BranchID LEFT OUTER JOIN dbo.Acc_Accounts ON dbo.Acc_SpendVoucherDetails.BranchID = dbo.Acc_Accounts.BranchID"
                + " AND dbo.Acc_SpendVoucherDetails.AccountID = dbo.Acc_Accounts.AccountID WHERE (dbo.Acc_SpendVoucherMaster.Cancel = 0) "
                + " AND (dbo.Acc_SpendVoucherMaster.BranchID = " + MySession.GlobalBranchID + ") AND (dbo.Acc_SpendVoucherMaster.SpendVoucherID = " + id + ")";
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, lang);
                dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count == 1)
                    _sampleData.Rows[i]["AccountName"] = dt.Rows[0][0];

                else
                    _sampleData.Rows[i]["AccountName"] = (MySession.GlobalLanguageName == iLanguage.English ? "Mentioned" : "مذكورين");
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void VariousVoucherBalance(long id, int i)
        {
            try
            {
                DataTable dtTotal;
                decimal NetBalance = 0;
                string strSQL;

                strSQL = "SELECT SUM(dbo.Acc_VariousVoucherDetails.Debit) AS NetBalance FROM dbo.Acc_VariousVoucherDetails INNER JOIN"
                + " dbo.Acc_VariousVoucherMaster ON dbo.Acc_VariousVoucherDetails.VoucherID = dbo.Acc_VariousVoucherMaster.VoucherID AND "
                + " dbo.Acc_VariousVoucherDetails.BranchID = dbo.Acc_VariousVoucherMaster.BranchID GROUP BY dbo.Acc_VariousVoucherMaster.VoucherID,"
                + " dbo.Acc_VariousVoucherMaster.BranchID, dbo.Acc_VariousVoucherMaster.Cancel HAVING (dbo.Acc_VariousVoucherMaster.VoucherID = " + id + ")"
                + " AND (dbo.Acc_VariousVoucherMaster.BranchID = " + MySession.GlobalBranchID + ") AND (dbo.Acc_VariousVoucherMaster.Cancel = 0)";
                dtTotal = Lip.SelectRecord(strSQL); ;
                if (dtTotal.Rows.Count > 0)
                {
                    NetBalance = Comon.ConvertToDecimalPrice(dtTotal.Rows[0]["NetBalance"]);
                    _sampleData.Rows[i]["VariousVoucher"] = NetBalance.ToString("N" + MySession.GlobalPriceDigits);
                    _sampleData.Rows[i]["AccountName"] = (MySession.GlobalLanguageName == iLanguage.English ? "Mentioned" : "مذكورين");
                    _sampleData.Rows[i]["OppsiteAccountName"] = (MySession.GlobalLanguageName == iLanguage.English ? "Mentioned" : "مذكورين");
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void ReceiptVoucherDebitBalance(long id, int i)
        {
            try
            {
                DataTable dtTotal, dt;
                decimal NetBalance = 0;
                string strSQL;

                strSQL = "SELECT DebitAmount - DiscountAmount AS NetBalance"
                + " FROM dbo.Acc_ReceiptVoucherMaster WHERE (Cancel = 0) AND "
                + " (BranchID = " + MySession.GlobalBranchID + ") AND (ReceiptVoucherID = " + id + ")";
                dtTotal = Lip.SelectRecord(strSQL); ;
                if (dtTotal.Rows.Count > 0)
                {
                    NetBalance = Comon.ConvertToDecimalPrice(dtTotal.Rows[0]["NetBalance"]);
                    _sampleData.Rows[i]["Debit"] = NetBalance.ToString("N" + MySession.GlobalPriceDigits);
                }

                strSQL = "SELECT dbo.Acc_Accounts.ArbName FROM dbo.Acc_ReceiptVoucherMaster INNER JOIN dbo.Acc_ReceiptVoucherDetails ON "
                + " dbo.Acc_ReceiptVoucherMaster.ReceiptVoucherID = dbo.Acc_ReceiptVoucherDetails.ReceiptVoucherID AND dbo.Acc_ReceiptVoucherMaster.BranchID"
                + " = dbo.Acc_ReceiptVoucherDetails.BranchID LEFT OUTER JOIN dbo.Acc_Accounts ON dbo.Acc_ReceiptVoucherDetails.BranchID = dbo.Acc_Accounts.BranchID AND "
                + " dbo.Acc_ReceiptVoucherDetails.AccountID = dbo.Acc_Accounts.AccountID WHERE (dbo.Acc_ReceiptVoucherMaster.ReceiptVoucherID = " + id + ")"
                + " AND (dbo.Acc_ReceiptVoucherMaster.Cancel = 0) AND (dbo.Acc_ReceiptVoucherMaster.BranchID = " + MySession.GlobalBranchID + ")";
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, lang);
                dt = Lip.SelectRecord(strSQL); ;
                if (dt.Rows.Count == 1)
                    _sampleData.Rows[i]["OppsiteAccountName"] = dt.Rows[0][0];
                else
                    _sampleData.Rows[i]["OppsiteAccountName"] = (MySession.GlobalLanguageName == iLanguage.English ? "Mentioned" : "مذكورين");
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void SalesInvoiceReturnCreditBalance(long id, long MethodeID, int i)
        {
            try
            {
                DataTable dtTotal;
                decimal TotalBalance = 0;
                decimal TotalDiscount = 0;
                decimal Net = 0;

                string strSQL = "SELECT SUM(dbo.Sales_SalesInvoiceReturnDetails.QTY * dbo.Sales_SalesInvoiceReturnDetails.SalePrice) AS TotalBalance,"
                + " SUM(dbo.Sales_SalesInvoiceReturnDetails.Discount) + dbo.Sales_SalesInvoiceReturnMaster.DiscountOnTotal AS TotalDiscount "
                + " FROM dbo.Sales_SalesInvoiceReturnMaster INNER JOIN dbo.Sales_SalesInvoiceReturnDetails ON dbo.Sales_SalesInvoiceReturnMaster.InvoiceID "
                + " = dbo.Sales_SalesInvoiceReturnDetails.InvoiceID AND dbo.Sales_SalesInvoiceReturnMaster.BranchID = dbo.Sales_SalesInvoiceReturnDetails.BranchID"
                + " GROUP BY dbo.Sales_SalesInvoiceReturnMaster.InvoiceID, dbo.Sales_SalesInvoiceReturnMaster.DiscountOnTotal, dbo.Sales_SalesInvoiceReturnMaster.Cancel, "
                + " dbo.Sales_SalesInvoiceReturnMaster.BranchID HAVING (dbo.Sales_SalesInvoiceReturnMaster.Cancel = 0) AND (dbo.Sales_SalesInvoiceReturnMaster.InvoiceID = " + id + ")"
                + " AND (dbo.Sales_SalesInvoiceReturnMaster.BranchID = " + MySession.GlobalBranchID + ")";
                dtTotal = Lip.SelectRecord(strSQL);
                if (dtTotal.Rows.Count > 0)
                {
                    TotalBalance = Comon.ConvertToDecimalPrice(dtTotal.Rows[0]["TotalBalance"]);
                    TotalDiscount = Comon.ConvertToDecimalPrice(dtTotal.Rows[0]["TotalDiscount"]);
                    Net = TotalBalance - TotalDiscount;

                    if (MethodeID == 1)
                        _sampleData.Rows[i]["Credit"] = Net.ToString("N" + MySession.GlobalPriceDigits);
                    else if (MethodeID == 2)
                        _sampleData.Rows[i]["PostPonedCredit"] = Net.ToString("N" + MySession.GlobalPriceDigits);
                    else if (MethodeID == 3)
                        _sampleData.Rows[i]["NetCredit"] = Net.ToString("N" + MySession.GlobalPriceDigits);
                    else if (MethodeID == 4)
                        _sampleData.Rows[i]["ChequeCredit"] = Net.ToString("N" + MySession.GlobalPriceDigits);
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private string CaseRecordType(string recordType, long id, int MethodeID, int i)
        {
            string _recordType = "";
            try
            {
                switch (recordType)
                {
                    case "CheckSpendVoucher":
                        {
                            if (MySession.GlobalLanguageName == iLanguage.Arabic)
                                _recordType = "سند صرف شيك";
                            else
                                _recordType = "Check Spend Voucher";
                            CheckSpendVoucherCreditBalance(id, i);
                            break;
                        }

                    case "CheckReceiptVoucher":
                        {
                            if (MySession.GlobalLanguageName == iLanguage.Arabic)
                                _recordType = "سند قبض شيك";
                            else
                                _recordType = "Check Receipt Voucher";
                            CheckReceiptVoucherDebitBalance(id, i);
                            break;
                        }

                    case "PurchaseInvoice":
                        {
                            if (MySession.GlobalLanguageName == iLanguage.Arabic)
                                _recordType = "فاتورة مشتريات";
                            else
                                _recordType = "Purchase Invoice";
                            PurchaseInvoiceCreditBalance(id, MethodeID, i);
                            break;
                        }

                    case "PurchaseInvoiceReturn":
                        {
                            if (MySession.GlobalLanguageName == iLanguage.Arabic)
                                _recordType = "مردود فاتورة مشتريات";
                            else
                                _recordType = "Purchase Invoice Return";
                            PurchaseInvoiceReturnCreditBalance(id, MethodeID, i);
                            break;
                        }

                    case "ReceiptVoucher":
                        {
                            if (MySession.GlobalLanguageName == iLanguage.Arabic)
                                _recordType = "سند قبض";
                            else
                                _recordType = "Receipt Voucher";
                            ReceiptVoucherDebitBalance(id, i);
                            break;
                        }

                    case "SalesInvoice":
                        {
                            if (MySession.GlobalLanguageName == iLanguage.Arabic)
                                _recordType = "فاتورة مبيعات";
                            else
                                _recordType = "Sales Invoice";
                            SalesInvoiceDebitBalance(id, MethodeID, i);
                            break;
                        }

                    case "SalesInvoiceReturn":
                        {
                            if (MySession.GlobalLanguageName == iLanguage.Arabic)
                                _recordType = "مردود فاتورة مبيعات";
                            else
                                _recordType = "Sales Invoice Return";
                            SalesInvoiceReturnCreditBalance(id, MethodeID, i);
                            break;
                        }

                    case "SpendVoucher":
                        {
                            if (MySession.GlobalLanguageName == iLanguage.Arabic)
                                _recordType = "سند صرف";
                            else
                                _recordType = "Spend Voucher";
                            SpendVoucherCreditBalance(id, i);
                            break;
                        }

                    case "VariousVoucher":
                        {
                            if (MySession.GlobalLanguageName == iLanguage.Arabic)
                                _recordType = "سند مختلف";
                            else
                                _recordType = "Various Voucher";
                            VariousVoucherBalance(id, i);
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            return _recordType;
        }
        protected override void DoNew()
        {
            try
            {
                _sampleData.Clear();
                gridControl1.RefreshDataSource();

                txtFromDate.Text = "";
                txtToDate.Text = "";
             
                txtFromDate.Enabled = true;
                txtToDate.Enabled = true;
           


            }
            catch (Exception ex)
            {
                //WT.msgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        protected override void DoPrint()
        {
            try
            {
                if (GridView1.DataRowCount - 1 == 0 && GridView1.GetRowCellValue(0, "ID").ToString() == "")
                {

                    return;

                }
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);

                /******************** Report Body *************************/

                bool IncludeHeader = true;
                string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");

                if (UserInfo.Language == iLanguage.English)
                    rptFormName = ReportName + "Arb";
                XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);

                /********************** Master *****************************/
                rptForm.RequestParameters = false;
                rptForm.Parameters["FromDate"].Value = txtFromDate.Text.Trim().ToString();
                rptForm.Parameters["ToDate"].Value = txtToDate.Text.Trim().ToString();

                for (int i = 0; i <= rptForm.Parameters.Count - 1; i++)
                { rptForm.Parameters[i].Visible = false; }

                /********************** Details ****************************/
                var dataTable = new dsReports.rptDetailedDailyTransactionDataTable();
                try
                {
                    for (int i = 0; i <= GridView1.DataRowCount - 1; i++)
                    {
                        var row = dataTable.NewRow();
                        row["#"] = i + 1;
                        row["ID"] = GridView1.GetRowCellValue(i, "ID").ToString();
                        row["TheDate"] = GridView1.GetRowCellValue(i, "TheDate").ToString();
                        row["Debit"] = GridView1.GetRowCellValue(i, "Debit").ToString();
                        row["Credit"] = GridView1.GetRowCellValue(i, "Credit").ToString();
                        row["PostPonedDebit"] = GridView1.GetRowCellValue(i, "PostPonedDebit").ToString();
                        row["PostPonedCredit"] = GridView1.GetRowCellValue(i, "PostPonedCredit").ToString();
                        row["NetDebit"] = GridView1.GetRowCellValue(i, "NetDebit").ToString();
                        row["NetCredit"] = GridView1.GetRowCellValue(i, "NetCredit").ToString();
                        row["ChequeDebit"] = GridView1.GetRowCellValue(i, "ChequeDebit").ToString();
                        row["ChequeCredit"] = GridView1.GetRowCellValue(i, "ChequeCredit").ToString();
                        row["VariousVoucher"] = GridView1.GetRowCellValue(i, "VariousVoucher").ToString();
                        row["AccountName"] = GridView1.GetRowCellValue(i, "AccountName").ToString();
                        row["OppsiteAccountName"] = GridView1.GetRowCellValue(i, "OppsiteAccountName").ToString();
                        row["RecordType"] = GridView1.GetRowCellValue(i, "RecordType").ToString();
                        row["Declaration"] = GridView1.GetRowCellValue(i, "Declaration").ToString();
                        row["UserName"] = GridView1.GetRowCellValue(i, "UserName").ToString();

                        dataTable.Rows.Add(row);
                    }

                }
                catch (Exception ex)
                {

                }
                rptForm.DataSource = dataTable;
                rptForm.DataMember = "rptDetailedDailyTransaction";

                /******************** Report Binding ************************/
                XRSubreport subreport = (XRSubreport)rptForm.FindControl("subRptCompanyHeader", true);
                subreport.Visible = IncludeHeader;
                subreport.ReportSource = ReportComponent.CompanyHeader();
                rptForm.ShowPrintStatusDialog = false;
                rptForm.ShowPrintMarginsWarning = false;
                rptForm.CreateDocument();
                rptForm.RequestParameters = false;
                SplashScreenManager.CloseForm(false);
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
        #endregion
        #region Event
        /************************Event From **************************/
        private void GridView1_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            e.Value = (e.ListSourceRowIndex + 1);
        }
        private void GridView1_InvalidRowException(object sender, DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
        }
        private void frmDetailedDailyTransaction_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void frmDetailedDailyTransaction_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void frmPurchaseInvoice_Load(object sender, EventArgs e)
        {
            try
            {
                
                //InitialFiveRows(_sampleData, 1);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }


        #region Event TextEdit
        //private void PublicTextEdit_EditValueChanged(object sender, EventArgs e)
        //{
        //    ((TextEdit)sender).Properties.Appearance.BorderColor = Color.Black;

        //}
        //private void PublicTextEdit_Enter(object sender, EventArgs e)
        //{
        //    (sender as DateEdit).ShowPopup();
        //}
        //private void PublicTextEdit_Click(object sender, EventArgs e)
        //{
        //    (sender as DateEdit).ShowPopup();
        //}
     
        void fun()
        {

         
        }
        private void btnShow_Click(object sender, EventArgs e)
        {
            try
            {

                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                btnShow.Visible = false;
                Application.DoEvents();
                string strSQL = "";
                if ((MySession.GlobalLanguageName == iLanguage.Arabic))
                {
                    strSQL = ("SELECT * FROM DetailedDailyTransactionArb Where BranchID ="
                                + (MySession.GlobalBranchID + (" AND TheDate >="
                                + (Comon.ConvertDateToSerial(txtFromDate.Text) + (" AND TheDate <="
                                + (Comon.ConvertDateToSerial(txtToDate.Text) + "  ORDER BY TheDate ASC,TheTime ASC"))))));
                }
                else
                {
                    strSQL = ("SELECT * FROM DetailedDailyTransactionEng Where BranchID ="
                                + (MySession.GlobalBranchID + (" AND TheDate >="
                                + (Comon.ConvertDateToSerial(txtFromDate.Text) + (" AND TheDate <="
                                + (Comon.ConvertDateToSerial(txtToDate.Text) + "  ORDER BY TheDate ASC,TheTime ASC"))))));
                }
                
                FillGridView(strSQL);
                lblDebit.Text=(Comon.ConvertToDecimalPrice(GridView1.GetRowCellValue(GridView1.RowCount-1,"Debit").ToString())+Comon.ConvertToDecimalPrice(GridView1.GetRowCellValue(GridView1.RowCount-1,"PostPonedDebit").ToString())+Comon.ConvertToDecimalPrice(GridView1.GetRowCellValue(GridView1.RowCount-1,"NetDebit").ToString())+Comon.ConvertToDecimalPrice(GridView1.GetRowCellValue(GridView1.RowCount-1,"ChequeDebit").ToString())).ToString();

                lblCredit.Text = (Comon.ConvertToDecimalPrice(GridView1.GetRowCellValue(GridView1.RowCount - 1, "Credit").ToString()) + Comon.ConvertToDecimalPrice(GridView1.GetRowCellValue(GridView1.RowCount - 1, "PostPonedCredit").ToString()) + Comon.ConvertToDecimalPrice(GridView1.GetRowCellValue(GridView1.RowCount - 1, "NetCredit").ToString()) + Comon.ConvertToDecimalPrice(GridView1.GetRowCellValue(GridView1.RowCount - 1, "ChequeCredit").ToString())).ToString();
                lblBalanceSum.Text = (Comon.ConvertToDecimalPrice(lblDebit.Text) - Comon.ConvertToDecimalPrice(lblCredit.Text)).ToString();




                btnShow.Visible = true;
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
        #endregion

        #endregion
        #region InitializeComponent
        private void ClearRows()
        {
            for (int i = 0; i < GridView1.RowCount; )
                GridView1.DeleteRow(i);
        }
        private DataTable InitialFiveRows(DataTable dt, int RowsCount)
        {
            int currDatatableCount = dt.Rows.Count;
            for (int i = currDatatableCount; i < RowsCount + currDatatableCount; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = i + 1;
                dr[5] = "";

                dt.Rows.Add(dr);
                makeGridBind(dt);
            }
            return dt;
        }
        void makeGridBind(DataTable dt)
        {
            DataView dv = dt.DefaultView;
            _sampleData = dt;
            gridControl1.DataSource = dt;


        }
        private void InitializeFormatDate(DateEdit Obj)
        {
            Obj.Properties.Mask.UseMaskAsDisplayFormat = true;
            Obj.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            Obj.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            Obj.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            Obj.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            Obj.Properties.Mask.EditMask = "dd/MM/yyyy";
            Obj.EditValue = DateTime.Now;
        }
        #endregion

        private void frmDetailedDailyTransaction_Load(object sender, EventArgs e)
        {

        }

        private void GridView1_DoubleClick(object sender, EventArgs e)
        {
            GridView view = sender as GridView;

            switch (view.GetFocusedRowCellValue("TempRecordType").ToString())
            {
                case "PurchaseInvoice":
                    frmPurchaseInvoice frm = new frmPurchaseInvoice();
                    if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm);
                        frm.Show();
                        frm.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frm.Dispose();


                    break;

                case "ItemsOutOnBail":
                    frmItemsOutOnBail frm11 = new frmItemsOutOnBail();
                    if (Permissions.UserPermissionsFrom(frm11, frm11.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm11);
                        frm11.Show();
                        frm11.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frm11.Dispose();


                    break;


                case "ItemsInOnBail":
                    frmItemsInonBail frm12 = new frmItemsInonBail();
                    if (Permissions.UserPermissionsFrom(frm12, frm12.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm12);
                        frm12.Show();
                        frm12.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frm12.Dispose();


                    break;

                case "GoodsOpening":
                    frmGoodsOpening frm1 = new frmGoodsOpening();
                    if (Permissions.UserPermissionsFrom(frm1, frm1.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm1);
                        frm1.Show();
                        frm1.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frm1.Dispose();
                    break;
                //case "ItemsTransfer":
                //   frmItemsTransfer   frm =new frmItemsTransfer();
                //   //  Lip.Ch(frm, Language)
                //     frm.Show();
                //     frm.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString())+1,8);
                //    break;
                case "ItemsDismantling":
                    frmItemsDismantling frm10 = new frmItemsDismantling();
                    if (Permissions.UserPermissionsFrom(frm10, frm10.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm10);
                        frm10.Show();
                        frm10.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frm10.Dispose();
                    break;
                case "SalesInvoiceReturn":
                    frmSalesInvoiceReturn frm2 = new frmSalesInvoiceReturn();
                    if (Permissions.UserPermissionsFrom(frm2, frm2.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm2);
                        frm2.Show();
                        frm2.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frm2.Dispose();
                    break;
                case "SalesInvoice":
                    frmSalesInvoice frm3 = new frmSalesInvoice();
                    if (Permissions.UserPermissionsFrom(frm3, frm3.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm3);
                        frm3.Show();
                        frm3.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frm3.Dispose();
                    break;
                case "PurchaseInvoiceReturn":
                    frmPurchaseInvoiceReturn frm4 = new frmPurchaseInvoiceReturn();
                    if (Permissions.UserPermissionsFrom(frm4, frm4.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm4);
                        frm4.Show();
                        frm4.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frm4.Dispose();
                    break;

                case "ReceiptVoucher":
                    frmReceiptVoucher frm20 = new frmReceiptVoucher();
                    if (Permissions.UserPermissionsFrom(frm20, frm20.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm20);
                        frm20.Show();
                        frm20.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frm20.Dispose();
                    break;

                case "CheckSpendVoucher":
                    frmCheckSpendVoucher frm24 = new frmCheckSpendVoucher();
                    if (Permissions.UserPermissionsFrom(frm24, frm24.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm24);
                        frm24.Show();
                        frm24.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frm24.Dispose();
                    break;


                case "CheckReceiptVoucher":
                    frmCheckReceiptVoucher frm23 = new frmCheckReceiptVoucher();
                    if (Permissions.UserPermissionsFrom(frm23, frm23.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm23);
                        frm23.Show();
                        frm23.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frm23.Dispose();
                    break;

                case "VariousVoucher":
                    frmVariousVoucher frm22 = new frmVariousVoucher();
                    if (Permissions.UserPermissionsFrom(frm22, frm22.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm22);
                        frm22.Show();
                        frm22.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frm22.Dispose();
                    break;

                case "SpendVoucher":
                    frmSpendVoucher frm21 = new frmSpendVoucher();
                    if (Permissions.UserPermissionsFrom(frm21, frm21.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm21);
                        frm21.Show();
                        frm21.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frm21.Dispose();
                    break;



            }
        }
        private void txtFromDate_EditValueChanged(object sender, EventArgs e)
        {
            if (Comon.ConvertDateToSerial(txtFromDate.Text) > Comon.cLong((Lip.GetServerDateSerial())))
                txtFromDate.Text = Lip.GetServerDate();
        }

        private void txtToDate_EditValueChanged(object sender, EventArgs e)
        {
            if (Comon.ConvertDateToSerial(txtToDate.Text) > Comon.cLong((Lip.GetServerDateSerial())))
                txtToDate.Text = Lip.GetServerDate();
        }

    }


}

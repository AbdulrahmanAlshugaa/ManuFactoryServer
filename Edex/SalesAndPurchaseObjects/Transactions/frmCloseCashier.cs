using DevExpress.XtraReports.UI;
using DevExpress.XtraSplashScreen;
using Edex.DAL.Stc_itemDAL;
using Edex.Model;
using Edex.Model.Language;
using Edex.GeneralObjects.GeneralClasses;
using Edex.GeneralObjects.GeneralForms;
using Edex.ModelSystem;
using Edex.SalesAndPurchaseObjects.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Edex.SalesAndPurchaseObjects.Transactions
{
    public partial class frmCloseCashier : Edex.GeneralObjects.GeneralForms.BaseForm
    {
        public bool IsPrinting = false;
        public bool IsClosed = false;
        public string PrimaryName = "ArbName";
        public string strSQL;
        string TableName = "SalesCashierClose";
        string PremaryKey = "CloseCashierID";
        int goOn = 0;
        public frmCloseCashier()
        {
            InitializeComponent();
            lblUserName.ReadOnly = true; 

            this.txtUserID.Validating += new System.ComponentModel.CancelEventHandler(this.txtUserID_Validating);
            if (UserInfo.ID != 1) {
                txtUserID.ReadOnly = true;
                btnEditCloseCashierDate.Enabled = false;
            
            }

            ///////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////
            this.txtFromDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtFromDate.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.txtFromDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtFromDate.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.txtFromDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtFromDate.Properties.Mask.EditMask = "dd/MM/yyyy";
            // this.txtFromDate.EditValue = DateTime.Now;
            /////////////////////////////////////////////////////////////////
            this.txtToDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtToDate.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.txtToDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtToDate.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.txtToDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtToDate.Properties.Mask.EditMask = "dd/MM/yyyy";

            txtToDate.EditValue = DateTime.Now;
            txtFromDate.EditValue = DateTime.Now;
        }

        private void btnViewTotalInvoices_Click(object sender, EventArgs e)
        {
            decimal SumReturn = 0;
            SalesCashierClose model = new SalesCashierClose();
            model.FromSaleInvoice = 0;
            model.ToSaleInvoice = 0;
            model.FromSaleInvoiceReturn = 0;
            model.ToSaleInvoiceReturn = 0;
            try
            {
                if (string.IsNullOrEmpty(txtUserID.Text))
                {
                    Messages.MsgWarning(Messages.TitleWorning, (UserInfo.Language == iLanguage.Arabic ? " الرحاء ادخال رقم المستخدم " : "You must Enter User ID"));

                    return;
                }
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);


                DataTable dt = new DataTable();
                //,dbo.Sales_SalesInvoiceMaster.DiscountOnTotal,SUM(dbo.Sales_SalesInvoiceDetails.QTY * dbo.Sales_SalesInvoiceDetails.SalePrice - dbo.Sales_SalesInvoiceDetails.Discount+ dbo.Sales_SalesInvoiceDetails.AdditionalValue) AS SumTotal1,
                strSQL = "SELECT dbo.Sales_SalesInvoiceMaster.InvoiceID,dbo.Sales_SalesInvoiceMaster.ComputerInfo,Sales_SalesInvoiceMaster.NetBalance as SumTotal,dbo.Sales_SalesInvoiceMaster.DiscountOnTotal"

                       + " ,dbo.Sales_SalesInvoiceMaster.NetAmount, dbo.Sales_SalesMethodes.ArbName As SaleTypeName,dbo.Sales_SalesMethodes.MethodID As SaleTypeID"
                       + " FROM dbo.Sales_SalesMethodes INNER JOIN dbo.Sales_SalesInvoiceMaster ON dbo.Sales_SalesMethodes.MethodID = dbo.Sales_SalesInvoiceMaster.MethodeID LEFT OUTER JOIN"
                       + " dbo.Sales_SalesInvoiceDetails ON dbo.Sales_SalesInvoiceMaster.BranchID = dbo.Sales_SalesInvoiceDetails.BranchID AND dbo.Sales_SalesInvoiceMaster.InvoiceID = "
                       + " dbo.Sales_SalesInvoiceDetails.InvoiceID Where Sales_SalesInvoiceMaster.BranchID=" + MySession.GlobalBranchID + " And Sales_SalesInvoiceMaster.UserID=" + txtUserID.Text
                       + " And dbo.Sales_SalesInvoiceMaster.CloseCashierDate=0   and  dbo.Sales_SalesInvoiceMaster.Cancel=0   GROUP BY dbo.Sales_SalesInvoiceMaster.DiscountOnTotal, Sales_SalesInvoiceMaster.NetBalance,dbo.Sales_SalesInvoiceMaster.NetAmount,dbo.Sales_SalesInvoiceMaster.ComputerInfo,dbo.Sales_SalesInvoiceMaster.InvoiceID, "
                       + " dbo.Sales_SalesMethodes.ArbName,dbo.Sales_SalesMethodes.MethodID Order By InvoiceID ASC";
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, MySession.GlobalLanguageName == iLanguage.Arabic ? "Arb" : "Eng");
                dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count > 0)
                {
                    model.FromSaleInvoice = Comon.cInt(dt.Rows[0]["InvoiceID"].ToString());
                    model.ToSaleInvoice = Comon.cInt(dt.Rows[dt.Rows.Count - 1]["InvoiceID"].ToString());
                    strSQL = "SELECT dbo.Sales_SalesInvoiceReturnMaster.InvoiceID,dbo.Sales_SalesInvoiceReturnMaster.ComputerInfo,Sales_SalesInvoiceReturnMaster.NetBalance as SumTotal,dbo.Sales_SalesInvoiceReturnMaster.DiscountOnTotal"

                      + " ,dbo.Sales_SalesInvoiceReturnMaster.NetAmount, dbo.Sales_SalesMethodes.ArbName As SaleTypeName,dbo.Sales_SalesMethodes.MethodID As SaleTypeID"
                      + " FROM dbo.Sales_SalesMethodes INNER JOIN dbo.Sales_SalesInvoiceReturnMaster ON dbo.Sales_SalesMethodes.MethodID = dbo.Sales_SalesInvoiceReturnMaster.MethodeID LEFT OUTER JOIN"
                      + " dbo.Sales_SalesInvoiceReturnDetails ON dbo.Sales_SalesInvoiceReturnMaster.BranchID = dbo.Sales_SalesInvoiceReturnDetails.BranchID AND dbo.Sales_SalesInvoiceReturnMaster.InvoiceID = "
                      + " dbo.Sales_SalesInvoiceReturnDetails.InvoiceID Where Sales_SalesInvoiceReturnMaster.BranchID=" + MySession.GlobalBranchID + " And Sales_SalesInvoiceReturnMaster.UserID=" + txtUserID.Text
                      + " And dbo.Sales_SalesInvoiceReturnMaster.CloseCashier=0   and  dbo.Sales_SalesInvoiceReturnMaster.Cancel=0   GROUP BY dbo.Sales_SalesInvoiceReturnMaster.DiscountOnTotal, Sales_SalesInvoiceReturnMaster.NetBalance,dbo.Sales_SalesInvoiceReturnMaster.NetAmount,dbo.Sales_SalesInvoiceReturnMaster.ComputerInfo,dbo.Sales_SalesInvoiceReturnMaster.InvoiceID, "
                      + " dbo.Sales_SalesMethodes.ArbName,dbo.Sales_SalesMethodes.MethodID Order By InvoiceID ASC";

                    DataTable dtReturn = new DataTable();

                    dtReturn = Lip.SelectRecord(strSQL);
                    decimal netSumReturn = 0;
                    decimal TotalNetReturn = 0;
                    decimal TotalCashReturn = 0;
                    decimal netCashSumReturn = 0;
                    decimal caschPaidWithNetReturn = 0;
                    decimal cashReturn = 0;
                    decimal futureReturn = 0;
                    decimal baqiReturn = 0;
                    decimal TotalReturn = 0;
                    int ContReturn = 0;

                    if (dtReturn.Rows.Count > 0)
                    {
                        model.FromSaleInvoiceReturn = Comon.cInt(dtReturn.Rows[0]["InvoiceID"].ToString());
                        model.ToSaleInvoiceReturn = Comon.cInt(dtReturn.Rows[dtReturn.Rows.Count - 1]["InvoiceID"].ToString());
                        foreach (DataRow drow1 in dtReturn.Rows)
                        {
                            TotalReturn += Comon.ConvertToDecimalPrice(drow1["SumTotal"]);
                            switch (Comon.cInt(drow1["SaleTypeID"].ToString()))
                            {

                                case (1):
                                    cashReturn += (Comon.ConvertToDecimalPrice(drow1["SumTotal"]));//- Comon.ConvertToDecimalPrice(dt.Rows[i]["DiscountOnTotal"])));

                                    break;
                                case (2):
                                    futureReturn += (Comon.ConvertToDecimalPrice(drow1["SumTotal"]));//- Comon.ConvertToDecimalPrice(dt.Rows[i]["DiscountOnTotal"])));

                                    break;
                                case (3):
                                    netSumReturn += (Comon.ConvertToDecimalPrice(drow1["SumTotal"]));//- Comon.ConvertToDecimalPrice(dt.Rows[i]["DiscountOnTotal"])));
                                    //   row["NetPaid"] = dt.Rows[i]["NetProcessID"].ToString();
                                    break;
                                case (5):
                                    netCashSumReturn += Comon.ConvertToDecimalPrice(drow1["SumTotal"]);// - Comon.ConvertToDecimalPrice(dt.Rows[i]["DiscountOnTotal"])));
                                    caschPaidWithNetReturn += Comon.ConvertToDecimalPrice(drow1["NetAmount"]);
                                    // row["NetPaid"] = dt.Rows[i]["NetProcessID"].ToString();
                                    break;
                            }


                        }
                        SumReturn = Comon.ConvertToDecimalPrice(dtReturn.Rows[0][0].ToString());
                    }
                    baqiReturn = netCashSumReturn - caschPaidWithNetReturn;
                    TotalCashReturn = baqiReturn + cashReturn;
                    TotalNetReturn = netSumReturn + caschPaidWithNetReturn;
                    ContReturn = dtReturn.Rows.Count;

                    ReportName = "rptTotalCashierSales";
                    string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");
                    XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);
                    decimal netSum = 0;
                    decimal netCashSum = 0;
                    decimal caschPaidWithNet = 0;
                    decimal cash = 0;
                    decimal future = 0;
                    decimal TotalNetSales = 0;
                    decimal TotalCashSales = 0;
                    decimal baqi = 0;
                    /********************** Master *****************************/
                    rptForm.RequestParameters = false;
                    rptForm.Parameters["ComputerInfo"].Value =  lblUserName.Text;
                    rptForm.Parameters["TheDate"].Value = Lip.GetServerDate().ToString();
                    rptForm.Parameters["TheTime"].Value = DateTime.Today.TimeOfDay.ToString();
                    rptForm.Parameters["SumReturn"].Value = SumReturn.ToString();

                    var dataTable = new dsReports.rptTotalCashierSalesDataTable();

                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        var row = dataTable.NewRow();

                        row["InvoiceID"] = dt.Rows[i]["InvoiceID"].ToString();
                        row["Total"] = dt.Rows[i]["SumTotal"].ToString();
                        row["TypeName"] = dt.Rows[i]["SaleTypeName"].ToString();
                        row["Discount"] = dt.Rows[i]["DiscountOnTotal"].ToString();
                        row["Net"] = (Comon.ConvertToDecimalPrice(dt.Rows[i]["SumTotal"]));


                        // - Comon.ConvertToDecimalPrice(dt.Rows[i]["DiscountOnTotal"])).ToString();
                        switch (Comon.cInt(dt.Rows[i]["SaleTypeID"].ToString()))
                        {

                            case (1):
                                cash += (Comon.ConvertToDecimalPrice(dt.Rows[i]["SumTotal"]));//- Comon.ConvertToDecimalPrice(dt.Rows[i]["DiscountOnTotal"])));
                                row["NetPaid"] = "-";
                                break;
                            case (2):
                                future += (Comon.ConvertToDecimalPrice(dt.Rows[i]["SumTotal"]));//- Comon.ConvertToDecimalPrice(dt.Rows[i]["DiscountOnTotal"])));
                                row["NetPaid"] = "-";
                                break;
                            case (3):
                                netSum += (Comon.ConvertToDecimalPrice(dt.Rows[i]["SumTotal"]));//- Comon.ConvertToDecimalPrice(dt.Rows[i]["DiscountOnTotal"])));
                                //   row["NetPaid"] = dt.Rows[i]["NetProcessID"].ToString();
                                break;
                            case (5):
                                netCashSum += Comon.ConvertToDecimalPrice(dt.Rows[i]["SumTotal"]);// - Comon.ConvertToDecimalPrice(dt.Rows[i]["DiscountOnTotal"])));
                                caschPaidWithNet += Comon.ConvertToDecimalPrice(dt.Rows[i]["NetAmount"]);
                                // row["NetPaid"] = dt.Rows[i]["NetProcessID"].ToString();
                                break;
                        }
                        dataTable.Rows.Add(row);
                    }
                    baqi = netCashSum - caschPaidWithNet;
                    TotalCashSales = baqi + cash;
                    TotalNetSales = netSum + caschPaidWithNet;


                    rptForm.Parameters["NetCahSum"].Value = netCashSum.ToString();
                    rptForm.Parameters["NetSum"].Value = netSum.ToString();
                    rptForm.Parameters["CashSum"].Value = cash.ToString();
                    rptForm.Parameters["caschPaidWithNet"].Value = caschPaidWithNet.ToString();
                    rptForm.Parameters["baqi"].Value = baqi.ToString();
                    rptForm.Parameters["TotalCashSales"].Value = TotalCashSales.ToString();
                    rptForm.Parameters["TotalNetSales"].Value = TotalNetSales.ToString();



                    rptForm.Parameters["NetBalanceReturn"].Value = TotalReturn.ToString();
                    rptForm.Parameters["NetCahSumReturn"].Value = netCashSumReturn.ToString();
                    rptForm.Parameters["NetSumReturn"].Value = netSumReturn.ToString();
                    rptForm.Parameters["CashSumReturn"].Value = cashReturn.ToString();
                    rptForm.Parameters["caschPaidWithNetReturn"].Value = caschPaidWithNetReturn.ToString();
                    rptForm.Parameters["baqiReturn"].Value = baqiReturn.ToString();
                    rptForm.Parameters["ContReturn"].Value = ContReturn.ToString();
                    rptForm.Parameters["TotalCashReturn"].Value = TotalCashReturn.ToString();
                    rptForm.Parameters["TotalNetReturn"].Value = TotalNetReturn.ToString();

                    rptForm.Parameters["FutureSumReturn"].Value = TotalCashSales - TotalCashReturn;


                    var dtGroups = new dsReports.ByCatgeryReportDataTable();
                    var dtSalee = new dsReports.rptPurchaseInvoiceDataTable();
                    DataSet dst = new DataSet();
                   var row1 = dtSalee.NewRow();
                   row1["ItemName"] = "";
                   row1["SizeName"] = "";
                   dtSalee.Rows.Add(row1);
                    dst.Tables.Add(dtSalee);




                    var dr = Lip.SelectRecord(GetStrSQL());
                    if (dr.Rows.Count > 0)
                    {
                        foreach (DataRow drow in dr.Rows)
                        {
                            DataRow row;
                            row = dtGroups.NewRow();
                            row["GroupName"] = drow["ItemName"].ToString();
                            row["INQty"] = Comon.ConvertToDecimalQty(drow["TotalSales"].ToString());
                            row["OutQty"] = Comon.ConvertToDecimalQty(drow["TotalQTY"].ToString());
                            dtGroups.Rows.Add(row);

                            
                        }

                        dst.Tables.Add(dtGroups);

                    }

                    dst.Tables.Add(dataTable);



                    rptForm.DataSource = dst;

                    /******************** Report Binding ************************/

                    rptForm.ShowPrintStatusDialog = false;
                    rptForm.ShowPrintMarginsWarning = false;
                    rptForm.CreateDocument();
                    for (int i = 0; i < rptForm.Parameters.Count; i++)
                        rptForm.Parameters[i].Visible = false;

                  
                    SplashScreenManager.CloseForm(false);

                    if (ShowReportInReportViewer==true)
                    {
                        frmReportViewer frmRptViewer = new frmReportViewer();
                        frmRptViewer.documentViewer1.DocumentSource = rptForm;
                        frmRptViewer.ShowDialog();
                    }
                    else
                    {
                        bool IsSelectedPrinter = false;
                        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                        DataTable dt1 = ReportComponent.SelectRecord("SELECT *  from Printers where ReportName='" + ReportName + "'");
                        if (dt1.Rows.Count > 0) for (int i = 1; i < 6; i++)
                            {
                                string PrinterName = dt1.Rows[0]["PrinterName" + i.ToString()].ToString().ToUpper();
                                if (!string.IsNullOrEmpty(PrinterName))
                                {
                                    rptForm.PrinterName = PrinterName;
                                    rptForm.Print(PrinterName);
                                    IsSelectedPrinter = true;
                                    IsPrinting = true;
                                }
                            }
                        SplashScreenManager.CloseForm(false);
                        if (!IsSelectedPrinter)
                            Messages.MsgWarning(Messages.TitleWorning, Messages.msgThereIsNotPrinterSelected);
                    }
                }

                else
                {
                    SplashScreenManager.CloseForm(false);
                    Messages.MsgWarning(Messages.TitleWorning, (UserInfo.Language == iLanguage.Arabic ? "لا توجد فواتير سابقة لطباعتها" : "There isnNo Previous Invoices to print it."));
                }
            }
            catch (Exception ex)
            {

                goOn = 0;
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void frmCloseCashier_Load(object sender, EventArgs e)
        {
           
            ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[2].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[3].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[4].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[5].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[7].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[8].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[9].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[10].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[11].Visible = false;
            txtUserID.Text = MySession.UserID.ToString();
            txtUserID_Validating(null, null);
            PrimaryName = "ArbName";
            Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, "Arb");
            if (UserInfo.Language == iLanguage.English)
            {
                PrimaryName = "EngName";
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, "Eng");
            }

        }

        private void btnEditCloseCashierDate_Click(object sender, EventArgs e)
        {
            try
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

                frmEditCloseCashierDate frm = new frmEditCloseCashierDate();
                frm.ShowDialog();

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void btnCloseCashier_Click(object sender, EventArgs e)
        {
            DataTable dtt;

            dtt = Lip.SelectRecord("SELECT BranchID FROM  Users  where UserID=" + txtUserID.Text);
            DataTable dt = new DataTable();
            try
            {
                if (!FormAdd)
                {
                    Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToAddNewRecord);
                    return;
                }

                strSQL = "SELECT dbo.Sales_SalesInvoiceMaster.InvoiceID, "
                + " SUM(  dbo.Sales_SalesInvoiceDetails.SalePrice - dbo.Sales_SalesInvoiceDetails.Discount+ dbo.Sales_SalesInvoiceDetails.AdditionalValue) AS SumTotal, "
                + " dbo.Sales_SalesInvoiceMaster.DiscountOnTotal, dbo.Sales_SalesMethodes.ArbName As SaleTypeName,dbo.Sales_SalesMethodes.MethodID As SaleTypeID"
                + " FROM dbo.Sales_SalesMethodes INNER JOIN dbo.Sales_SalesInvoiceMaster ON dbo.Sales_SalesMethodes.MethodID = dbo.Sales_SalesInvoiceMaster.MethodeID LEFT OUTER JOIN"
                + " dbo.Sales_SalesInvoiceDetails ON dbo.Sales_SalesInvoiceMaster.BranchID = dbo.Sales_SalesInvoiceDetails.BranchID AND dbo.Sales_SalesInvoiceMaster.InvoiceID = "
                + " dbo.Sales_SalesInvoiceDetails.InvoiceID Where Sales_SalesInvoiceMaster.BranchID=" +dtt.Rows[0]["BranchID"]+ " And Sales_SalesInvoiceMaster.UserID=" + txtUserID.Text + ""
                + " And dbo.Sales_SalesInvoiceMaster.CloseCashierDate=0 and  dbo.Sales_SalesInvoiceMaster.Cancel=0 GROUP BY  dbo.Sales_SalesInvoiceMaster.AdditionaAmountTotal,dbo.Sales_SalesInvoiceMaster.InvoiceID, dbo.Sales_SalesInvoiceMaster.DiscountOnTotal, "
                + " dbo.Sales_SalesMethodes.ArbName,dbo.Sales_SalesMethodes.MethodID Order By SaleTypeID,InvoiceID ASC";
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, MySession.GlobalLanguageName == iLanguage.Arabic ? "Arb" : "Eng");
                dt = Lip.SelectRecord(strSQL);

                if (dt.Rows.Count == 0)
                {
                    Messages.MsgWarning(Messages.TitleWorning, (UserInfo.Language == iLanguage.Arabic ? "لا توجد مبيعات جديدة لإغلاقها" : "There Is No New Sales To Close It"));
                    return;
                }

                long closeDate = 0;
                if (true)
                {
                    bool Yes = Messages.MsgStopYesNo(Messages.TitleConfirm, (UserInfo.Language == iLanguage.Arabic ? "هل تريد إغلاق الكاشير مع طباعة إجمالي أو تفصيل الفواتير ؟" : "Are You Sure You Want To Close Cashier With Out Print Total Or Details Invoices ?"));
                    if (!Yes)
                        return;
                   
                    //frmExpiryDate frm = new frmExpiryDate();
                    //frm.ShowDialog();
                    //if (frm.GetDateExpire() != -1)
                    //    closeDate = frm.GetDateExpire();
                    closeDate = Comon.cLong((Lip.GetServerDateSerial()));
                    if (closeDate < 1) return;
                    goOn = 1;
                    try{
                    btnPrintTotalInvoices_Click(null, null);
                    }
                    catch { return; }
                    goOn = 0;
                    strSQL = "Update Sales_SalesInvoiceMaster Set CloseCashier=1 , CloseCashierDate =" + closeDate + " Where BranchID=" + dtt.Rows[0]["BranchID"]
                           + " AND CloseCashier=0 And CloseCashierDate=0 And UserID=" + txtUserID.Text;
                    Lip.ExecututeSQL(strSQL);
                    strSQL = "Update Sales_SalesInvoiceReturnMaster set CloseCashier =1     , CloseCashierDate =" + closeDate + " Where BranchID=" + dtt.Rows[0]["BranchID"]
                           + " AND CloseCashier=0 And CloseCashierDate=0 And UserID=" + txtUserID.Text;
                    Lip.ExecututeSQL(strSQL);
                    Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
                    this.Close();
                }

            }
            catch (Exception ex)
            {
                goOn = 0;
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void btnViewInvoicesByDet1ails_Click(object sender, EventArgs e)
        {
            DataTable dtInvoicesIDs = new DataTable();
            DataTable dt = new DataTable();
            try
            {
                if (string.IsNullOrEmpty(txtUserID.Text))
                {
                    Messages.MsgWarning(Messages.TitleWorning, (UserInfo.Language == iLanguage.Arabic ? " الرحاء ادخال رقم المستخدم " : "You must Enter User ID"));
                    return;
                }

                strSQL = "SELECT dbo.Sales_SalesInvoiceMaster.InvoiceID,dbo.Sales_SalesInvoiceMaster.CustomerID,dbo.Sales_SalesInvoiceMaster.Notes,dbo.Sales_SalesInvoiceMaster.StoreID,dbo.Sales_SalesInvoiceMaster.CostCenterID, dbo.Sales_SalesInvoiceMaster.MethodeID,dbo.Sales_SalesInvoiceMaster.RegTime,dbo.Sales_SalesInvoiceMaster.RegDate, dbo.Sales_SalesInvoiceMaster.DiscountOnTotal, dbo.Sales_SalesInvoiceMaster.RemaindAmount,"
                   + " dbo.Sales_SalesInvoiceMaster.RegTime, dbo.Sales_SalesInvoiceMaster.InvoiceDate, dbo.Sales_SalesMethodes.ArbName As SaleTypeName,"
                   + " dbo.Sales_Sellers.ArbName AS SellerName"
                   + " FROM dbo.Sales_SalesInvoiceMaster LEFT OUTER JOIN"
                   + " dbo.Sales_Sellers ON dbo.Sales_SalesInvoiceMaster.BranchID = dbo.Sales_Sellers.BranchID AND "
                   + " dbo.Sales_SalesInvoiceMaster.SellerID = dbo.Sales_Sellers.SellerID LEFT OUTER JOIN "
                   + " dbo.Sales_SalesMethodes ON dbo.Sales_SalesInvoiceMaster.MethodeID = dbo.Sales_SalesMethodes.MethodID"
                   + " Where Sales_SalesInvoiceMaster.BranchID = " + MySession.GlobalBranchID
                   +"And Sales_SalesInvoiceMaster.UserID=" + txtUserID.Text 
                   + " And Sales_SalesInvoiceMaster.ComputerInfo='" + UserInfo.ComputerInfo + "'"
                   + " And Sales_SalesInvoiceMaster.CloseCashier=0  and  dbo.Sales_SalesInvoiceMaster.Cancel=0 Order By Sales_SalesInvoiceMaster.MethodeID,Sales_SalesInvoiceMaster.InvoiceID";

                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, MySession.GlobalLanguageName == iLanguage.Arabic ? "Arb" : "Eng");
                dtInvoicesIDs = Lip.SelectRecord(strSQL);
                if (dtInvoicesIDs.Rows.Count > 0)
                {
                    for (int x = 0; x <= dtInvoicesIDs.Rows.Count - 1; ++x)
                    {

                        strSQL = "SELECT dbo.Sales_SalesInvoiceDetails.BarCode, dbo.Stc_Items.ArbName AS ItemName, dbo.Stc_SizingUnits.ArbName AS SizeName, dbo.Sales_SalesInvoiceDetails.QTY, "
                            + " dbo.Sales_SalesInvoiceDetails.SalePrice, dbo.Sales_SalesInvoiceDetails.QTY * dbo.Sales_SalesInvoiceDetails.SalePrice AS Total, dbo.Sales_SalesInvoiceMaster.DiscountOnTotal As Discount, 0 AS Net, "
                            + " 0 AS ExpiryDate, 0 AS Bones, '' AS Description, dbo.Sales_SalesInvoiceDetails.InvoiceID,"
                            + " Sales_SalesInvoiceMaster.RegTime"
                            + " FROM dbo.Sales_SalesMethodes INNER JOIN"
                            + " dbo.Sales_SalesInvoiceMaster ON dbo.Sales_SalesMethodes.MethodID = dbo.Sales_SalesInvoiceMaster.MethodeID INNER JOIN"
                            + " dbo.Sales_Sellers ON dbo.Sales_SalesInvoiceMaster.BranchID = dbo.Sales_Sellers.BranchID AND "
                            + " dbo.Sales_SalesInvoiceMaster.SellerID = dbo.Sales_Sellers.SellerID RIGHT OUTER JOIN"
                            + " dbo.Sales_SalesInvoiceDetails INNER JOIN"
                            + " dbo.Stc_Items ON dbo.Sales_SalesInvoiceDetails.ItemID = dbo.Stc_Items.ItemID INNER JOIN"
                            + " dbo.Stc_SizingUnits ON dbo.Sales_SalesInvoiceDetails.SizeID = dbo.Stc_SizingUnits.SizeID "
                            + " ON dbo.Sales_SalesInvoiceMaster.BranchID = dbo.Sales_SalesInvoiceDetails.BranchID AND"
                            + " dbo.Sales_SalesInvoiceMaster.InvoiceID = dbo.Sales_SalesInvoiceDetails.InvoiceID"
                            + " Where Sales_SalesInvoiceDetails.BranchID=" + MySession.GlobalBranchID
                            + " and  dbo.Sales_SalesInvoiceDetails.Cancel=0 And dbo.Sales_SalesInvoiceDetails.InvoiceID=" + Comon.cInt(dtInvoicesIDs.Rows[x]["InvoiceID"].ToString());

                        Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, MySession.GlobalLanguageName == iLanguage.Arabic ? "Arb" : "Eng");
                        dt = Lip.SelectRecord(strSQL);


                        ReportName = "rptCashierSales";
                        string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");
                        XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);

                        /********************** Master *****************************/
                        rptForm.RequestParameters = false;
                        if (dt.Rows.Count > 0)
                        {
                            rptForm.Parameters["InvoiceID"].Value = dtInvoicesIDs.Rows[x]["InvoiceID"].ToString();
                            rptForm.Parameters["SaleType"].Value = dtInvoicesIDs.Rows[x]["SaleTypeName"].ToString();
                            rptForm.Parameters["SaleDate"].Value = Comon.ConvertSerialDateTo(dtInvoicesIDs.Rows[x]["InvoiceDate"].ToString());
                            rptForm.Parameters["StoreName"].Value = getStoreName(dtInvoicesIDs.Rows[x]["StoreID"].ToString());
                            rptForm.Parameters["CustomerName"].Value = getCustomerName(dtInvoicesIDs.Rows[x]["CustomerID"].ToString()); ;
                            rptForm.Parameters["SellerName"].Value = dtInvoicesIDs.Rows[x]["SellerName"].ToString();
                            rptForm.Parameters["CostCenterName"].Value = getCostCenterName(dtInvoicesIDs.Rows[x]["CostCenterID"].ToString()); ;
                            rptForm.Parameters["Notes"].Value = dtInvoicesIDs.Rows[x]["Notes"].ToString();

                            decimal InvoiceTotalBeforeDiscount = 0;
                            decimal TotalQty = 0;
                            for (int j = 0; j <= dt.Rows.Count - 1; ++j)
                            {

                                InvoiceTotalBeforeDiscount = Comon.ConvertToDecimalPrice(InvoiceTotalBeforeDiscount + (Comon.ConvertToDecimalPrice(dt.Rows[j]["Qty"].ToString()) * Comon.ConvertToDecimalPrice(dt.Rows[j]["SalePrice"].ToString())));
                                TotalQty = Comon.ConvertToDecimalQty(TotalQty + Comon.ConvertToDecimalPrice(dt.Rows[j]["Qty"].ToString()));
                            }

                            rptForm.Parameters["InvoiceTotalBeforeDiscount"].Value = InvoiceTotalBeforeDiscount.ToString();
                            rptForm.Parameters["SumDiscount"].Value = dtInvoicesIDs.Rows[x]["DiscountOnTotal"].ToString();
                            rptForm.Parameters["DiscountOnTotal"].Value = dtInvoicesIDs.Rows[x]["DiscountOnTotal"].ToString(); ;
                            rptForm.Parameters["DiscountTotal"].Value = dtInvoicesIDs.Rows[x]["DiscountOnTotal"].ToString();
                            rptForm.Parameters["NetBalance"].Value = Comon.ConvertToDecimalPrice(InvoiceTotalBeforeDiscount - Comon.ConvertToDecimalPrice(dtInvoicesIDs.Rows[x]["DiscountOnTotal"].ToString()));
                            rptForm.Parameters["TotalQTY"].Value = TotalQty.ToString();
                            rptForm.Parameters["SaleTime"].Value = (dtInvoicesIDs.Rows[x]["RegDate"] + " " + dtInvoicesIDs.Rows[x]["RegTime"]);
                            rptForm.Parameters["PaidAmount"].Value = Comon.ConvertToDecimalPrice(InvoiceTotalBeforeDiscount - Comon.ConvertToDecimalPrice(dtInvoicesIDs.Rows[x]["DiscountOnTotal"]));
                            rptForm.Parameters["RemaindAmount"].Value = Comon.ConvertToDecimalPrice(dtInvoicesIDs.Rows[x]["RemaindAmount"].ToString());

                        }

                        var dataTable = new dsReports.rptCashierSalesDataTable();

                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            var row = dataTable.NewRow();

                            row["BarCode"] = dt.Rows[i]["BarCode"].ToString();
                            row["ItemName"] = dt.Rows[i]["ItemName"].ToString() + dt.Rows[i]["BarCode"].ToString();
                            row["SizeName"] = dt.Rows[i]["SizeName"].ToString();
                            row["QTY"] = dt.Rows[i]["QTY"].ToString();
                            row["SalePrice"] = dt.Rows[i]["SalePrice"].ToString();
                            row["Total"] = dt.Rows[i]["Total"].ToString();
                            row["Discount"] = dt.Rows[i]["Discount"].ToString();

                            dataTable.Rows.Add(row);
                        }
                        rptForm.DataSource = dataTable;
                        rptForm.DataMember = "rptCashierSales";

                        /******************** Report Binding ************************/

                        rptForm.ShowPrintStatusDialog = false;
                        rptForm.ShowPrintMarginsWarning = false;
                        rptForm.CreateDocument();

                        SplashScreenManager.CloseForm(false);
                        if (!ShowReportInReportViewer)
                        {
                            frmReportViewer frmRptViewer = new frmReportViewer();
                            frmRptViewer.documentViewer1.DocumentSource = rptForm;
                            frmRptViewer.ShowDialog();
                        }
                        else
                        {
                            bool IsSelectedPrinter = false;
                            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                            DataTable dt1 = ReportComponent.SelectRecord("SELECT *  from Printers where ReportName='" + ReportName + "'");
                            if (dt1.Rows.Count > 0)
                                for (int i = 1; i < 6; i++)
                                {
                                    string PrinterName = dt1.Rows[0]["PrinterName" + i.ToString()].ToString().ToUpper();
                                    if (!string.IsNullOrEmpty(PrinterName))
                                    {
                                        frmReportViewer frmRptViewer = new frmReportViewer();
                                        frmRptViewer.documentViewer1.DocumentSource = rptForm;
                                        frmRptViewer.ShowDialog();
                                        IsSelectedPrinter = true;
                                    }
                                }
                            SplashScreenManager.CloseForm(false);
                            if (!IsSelectedPrinter)
                            {
                                SplashScreenManager.CloseForm(false);
                                Messages.MsgWarning(Messages.TitleWorning, Messages.msgThereIsNotPrinterSelected);
                                break;
                            }
                        }
                        if (x >= 3)
                            break;
                    }
                    btnPrintTotalInvoices_Click(null, null);
                }
                else

                    Messages.MsgWarning(Messages.TitleWorning, (UserInfo.Language == iLanguage.Arabic ? "لا توجد مبيعات لعرضها" : "There Is No New Sales To Show It"));
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void btnPrintInvoicesByDetails_Click(object sender, EventArgs e)
        {
            DataTable dtInvoicesIDs = new DataTable();
            DataTable dt = new DataTable();
            decimal SumReturn = 0;
            try
            {
                if (string.IsNullOrEmpty(txtUserID.Text))
                {
                    Messages.MsgWarning(Messages.TitleWorning, (UserInfo.Language == iLanguage.Arabic ? " الرحاء ادخال رقم المستخدم " : "You must Enter User ID"));

                    return;
                }


                strSQL = "SELECT dbo.Sales_SalesInvoiceMaster.InvoiceID,dbo.Sales_SalesInvoiceMaster.CustomerID,dbo.Sales_SalesInvoiceMaster.Notes,dbo.Sales_SalesInvoiceMaster.StoreID,dbo.Sales_SalesInvoiceMaster.CostCenterID, dbo.Sales_SalesInvoiceMaster.MethodeID,dbo.Sales_SalesInvoiceMaster.RegTime,dbo.Sales_SalesInvoiceMaster.RegDate, dbo.Sales_SalesInvoiceMaster.DiscountOnTotal, dbo.Sales_SalesInvoiceMaster.RemaindAmount,"
                       + " dbo.Sales_SalesInvoiceMaster.RegTime, dbo.Sales_SalesInvoiceMaster.InvoiceDate, dbo.Sales_SalesMethodes.ArbName As SaleTypeName,"
                       + " dbo.Sales_Sellers.ArbName AS SellerName"
                       + " FROM dbo.Sales_SalesInvoiceMaster LEFT OUTER JOIN"
                       + " dbo.Sales_Sellers ON dbo.Sales_SalesInvoiceMaster.BranchID = dbo.Sales_Sellers.BranchID AND "
                       + " dbo.Sales_SalesInvoiceMaster.SellerID = dbo.Sales_Sellers.SellerID LEFT OUTER JOIN "
                       + " dbo.Sales_SalesMethodes ON dbo.Sales_SalesInvoiceMaster.MethodeID = dbo.Sales_SalesMethodes.MethodID"
                       + " Where Sales_SalesInvoiceMaster.BranchID = " + MySession.GlobalBranchID
                       + " And Sales_SalesInvoiceMaster.ComputerInfo='" + UserInfo.ComputerInfo + "'"
                       + " And  Sales_SalesInvoiceMaster.UserID=" + txtUserID.Text + "  And  Sales_SalesInvoiceMaster.CloseCashier=0   and  dbo.Sales_SalesInvoiceMaster.Cancel=0 Order By Sales_SalesInvoiceMaster.MethodeID,Sales_SalesInvoiceMaster.InvoiceID";

                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, MySession.GlobalLanguageName == iLanguage.Arabic ? "Arb" : "Eng");
                dtInvoicesIDs = Lip.SelectRecord(strSQL);
                if (dtInvoicesIDs.Rows.Count > 0)
                {
                    for (int x = 0; x <= dtInvoicesIDs.Rows.Count - 1; ++x)
                    {

                        strSQL = "SELECT dbo.Sales_SalesInvoiceDetails.BarCode, dbo.Stc_Items.ArbName AS ItemName, dbo.Stc_SizingUnits.ArbName AS SizeName, dbo.Sales_SalesInvoiceDetails.QTY, "
                            + " dbo.Sales_SalesInvoiceDetails.SalePrice, dbo.Sales_SalesInvoiceDetails.QTY * dbo.Sales_SalesInvoiceDetails.SalePrice AS Total, dbo.Sales_SalesInvoiceMaster.DiscountOnTotal As Discount, 0 AS Net, "
                            + " 0 AS ExpiryDate, 0 AS Bones, '' AS Description, dbo.Sales_SalesInvoiceDetails.InvoiceID,"
                            + " Sales_SalesInvoiceMaster.RegTime"
                            + " FROM dbo.Sales_SalesMethodes INNER JOIN"
                            + " dbo.Sales_SalesInvoiceMaster ON dbo.Sales_SalesMethodes.MethodID = dbo.Sales_SalesInvoiceMaster.MethodeID INNER JOIN"
                            + " dbo.Sales_Sellers ON dbo.Sales_SalesInvoiceMaster.BranchID = dbo.Sales_Sellers.BranchID AND "
                            + " dbo.Sales_SalesInvoiceMaster.SellerID = dbo.Sales_Sellers.SellerID RIGHT OUTER JOIN"
                            + " dbo.Sales_SalesInvoiceDetails INNER JOIN"
                            + " dbo.Stc_Items ON dbo.Sales_SalesInvoiceDetails.ItemID = dbo.Stc_Items.ItemID INNER JOIN"
                            + " dbo.Stc_SizingUnits ON dbo.Sales_SalesInvoiceDetails.SizeID = dbo.Stc_SizingUnits.SizeID "
                            + " ON dbo.Sales_SalesInvoiceMaster.BranchID = dbo.Sales_SalesInvoiceDetails.BranchID AND"
                            + " dbo.Sales_SalesInvoiceMaster.InvoiceID = dbo.Sales_SalesInvoiceDetails.InvoiceID"
                            + " Where Sales_SalesInvoiceDetails.BranchID=" + MySession.GlobalBranchID
                            + "    and  dbo.Sales_SalesInvoiceDetails.Cancel=0 And dbo.Sales_SalesInvoiceDetails.InvoiceID=" + Comon.cInt(dtInvoicesIDs.Rows[x]["InvoiceID"].ToString());

                        Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, MySession.GlobalLanguageName == iLanguage.Arabic ? "Arb" : "Eng");
                        dt = Lip.SelectRecord(strSQL);

                        bool IncludeHeader = false;
                        ReportName = "rptCashierSales";
                        string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");

                        XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);
                        /********************** Master *****************************/
                        rptForm.RequestParameters = false;
                        if (dt.Rows.Count > 0)
                        {

                            // rptForm.Parameters["SaleType"].Value = dtInvoicesIDs.Rows[x]["SaleTypeName"].ToString();
                            // rptForm.Parameters["SaleDate"].Value = Comon.ConvertSerialToDate(dtInvoicesIDs.Rows[x]["InvoiceDate"].ToString());
                            // rptForm.Parameters["StoreName"].Value = getStoreName(dtInvoicesIDs.Rows[x]["StoreID"].ToString());
                            // rptForm.Parameters["CustomerName"].Value = getCustomerName(dtInvoicesIDs.Rows[x]["CustomerID"].ToString()); ;
                            // rptForm.Parameters["SellerName"].Value = dtInvoicesIDs.Rows[x]["SellerName"].ToString();
                            // rptForm.Parameters["CostCenterName"].Value = getCostCenterName(dtInvoicesIDs.Rows[x]["CostCenterID"].ToString()); ;
                            // rptForm.Parameters["Notes"].Value = dtInvoicesIDs.Rows[x]["Notes"].ToString();

                            decimal InvoiceTotalBeforeDiscount = 0;
                            decimal TotalQty = 0;
                            for (int j = 0; j <= dt.Rows.Count - 1; ++j)
                            {

                                InvoiceTotalBeforeDiscount = Comon.ConvertToDecimalPrice(InvoiceTotalBeforeDiscount + (Comon.ConvertToDecimalPrice(dt.Rows[j]["Qty"].ToString()) * Comon.ConvertToDecimalPrice(dt.Rows[x]["SalePrice"].ToString())));
                                TotalQty = Comon.ConvertToDecimalQty(TotalQty + Comon.ConvertToDecimalPrice(dt.Rows[j]["Qty"].ToString()));
                            }

                            //rptForm.Parameters["InvoiceTotalBeforeDiscount"].Value = InvoiceTotalBeforeDiscount.ToString();
                            //rptForm.Parameters["SumDiscount"].Value = dtInvoicesIDs.Rows[x]["DiscountOnTotal"].ToString();
                            //rptForm.Parameters["DiscountOnTotal"].Value = dtInvoicesIDs.Rows[x]["DiscountOnTotal"].ToString();
                            //rptForm.Parameters["DiscountTotal"].Value = dtInvoicesIDs.Rows[x]["DiscountOnTotal"].ToString();
                            //rptForm.Parameters["NetBalance"].Value = Comon.ConvertToDecimalPrice(InvoiceTotalBeforeDiscount - Comon.ConvertToDecimalPrice(dtInvoicesIDs.Rows[x]["DiscountOnTotal"].ToString()));
                            //rptForm.Parameters["TotalQTY"].Value = TotalQty.ToString();
                            //rptForm.Parameters["SaleTime"].Value = (dtInvoicesIDs.Rows[x]["RegDate"] + " " + dtInvoicesIDs.Rows[x]["RegTime"]);
                            //rptForm.Parameters["PaidAmount"].Value = Comon.ConvertToDecimalPrice(InvoiceTotalBeforeDiscount - Comon.ConvertToDecimalPrice(dtInvoicesIDs.Rows[x]["DiscountOnTotal"]));
                            //rptForm.Parameters["RemaindAmount"].Value = Comon.ConvertToDecimalPrice(dtInvoicesIDs.Rows[x]["RemaindAmount"].ToString());



                        }

                        var dataTable = new dsReports.rptCashierSalesDataTable();

                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            var row = dataTable.NewRow();

                            row["BarCode"] = dt.Rows[i]["BarCode"].ToString();
                            row["ItemName"] = dt.Rows[i]["ItemName"].ToString() + dt.Rows[i]["BarCode"].ToString();
                            row["SizeName"] = dt.Rows[i]["SizeName"].ToString();
                            row["QTY"] = dt.Rows[i]["QTY"].ToString();
                            row["SalePrice"] = dt.Rows[i]["SalePrice"].ToString();
                            row["Total"] = dt.Rows[i]["Total"].ToString();
                            row["Discount"] = dt.Rows[i]["Discount"].ToString();


                            dataTable.Rows.Add(row);
                        }
                        rptForm.DataSource = dataTable;
                        rptForm.DataMember = "rptCashierSales";

                        /******************** Report Binding ************************/
                        IncludeHeader = false;
                      //  XRSubreport subreport = (XRSubreport)rptForm.FindControl("subRptCompanyHeader", true);
                       // subreport.Visible = IncludeHeader;
                      //  subreport.ReportSource = ReportComponent.CompanyHeader();
                        rptForm.ShowPrintStatusDialog = false;
                        rptForm.ShowPrintMarginsWarning = false;
                        rptForm.CreateDocument();

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
                            DataTable dt1 = ReportComponent.SelectRecord("SELECT *  from Printers where ReportName='" + ReportName + "'");
                            if (dt1.Rows.Count > 0)
                                for (int i = 1; i < 6; i++)
                                {
                                    string PrinterName = dt1.Rows[0]["PrinterName" + i.ToString()].ToString().ToUpper();
                                    if (!string.IsNullOrEmpty(PrinterName))
                                    {
                                        rptForm.PrinterName = PrinterName;
                                        rptForm.Print(PrinterName);
                                        IsSelectedPrinter = true;
                                    }
                                }
                            SplashScreenManager.CloseForm(false);
                            if (!IsSelectedPrinter)
                            {
                                Messages.MsgWarning(Messages.TitleWorning, Messages.msgThereIsNotPrinterSelected);
                                break;
                            }
                        }



                    }
                    btnPrintTotalInvoices_Click(null, null);

                }
                else

                    Messages.MsgWarning(Messages.TitleWorning, (UserInfo.Language == iLanguage.Arabic ? "لا توجد مبيعات جديدة لإغلاقها" : "There Is No New Sales To Close It"));
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void btnPrintPreviousInvoices_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtUserID.Text))
                {
                    Messages.MsgWarning(Messages.TitleWorning, (UserInfo.Language == iLanguage.Arabic ? " الرحاء ادخال رقم المستخدم " : "You must Enter User ID"));
                    return;
                }
                long closeDate = 0;

                frmExpiryDate frm = new frmExpiryDate();
                frm.ShowDialog();
                if (frm.GetDateExpire() != -1)
                    closeDate = frm.GetDateExpire();

                if (closeDate < 1) return;
                decimal SumReturn = 0;
                SalesCashierClose model = new SalesCashierClose();
                model.FromSaleInvoice = 0;
                model.ToSaleInvoice = 0;
                model.FromSaleInvoiceReturn = 0;
                model.ToSaleInvoiceReturn = 0;

                if (string.IsNullOrEmpty(txtUserID.Text))
                {
                    Messages.MsgWarning(Messages.TitleWorning, (UserInfo.Language == iLanguage.Arabic ? " الرحاء ادخال رقم المستخدم " : "You must Enter User ID"));

                    return;
                }
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);


                DataTable dt = new DataTable();
                //,dbo.Sales_SalesInvoiceMaster.DiscountOnTotal,SUM(dbo.Sales_SalesInvoiceDetails.QTY * dbo.Sales_SalesInvoiceDetails.SalePrice - dbo.Sales_SalesInvoiceDetails.Discount+ dbo.Sales_SalesInvoiceDetails.AdditionalValue) AS SumTotal1,
                strSQL = "SELECT dbo.Sales_SalesInvoiceMaster.InvoiceID,dbo.Sales_SalesInvoiceMaster.ComputerInfo,Sales_SalesInvoiceMaster.NetBalance as SumTotal,dbo.Sales_SalesInvoiceMaster.DiscountOnTotal"

                       + " ,dbo.Sales_SalesInvoiceMaster.NetAmount, dbo.Sales_SalesMethodes.ArbName As SaleTypeName,dbo.Sales_SalesMethodes.MethodID As SaleTypeID"
                       + " FROM dbo.Sales_SalesMethodes INNER JOIN dbo.Sales_SalesInvoiceMaster ON dbo.Sales_SalesMethodes.MethodID = dbo.Sales_SalesInvoiceMaster.MethodeID LEFT OUTER JOIN"
                       + " dbo.Sales_SalesInvoiceDetails ON dbo.Sales_SalesInvoiceMaster.BranchID = dbo.Sales_SalesInvoiceDetails.BranchID AND dbo.Sales_SalesInvoiceMaster.InvoiceID = "
                       + " dbo.Sales_SalesInvoiceDetails.InvoiceID Where Sales_SalesInvoiceMaster.BranchID=" + MySession.GlobalBranchID + " And Sales_SalesInvoiceMaster.UserID=" + txtUserID.Text
                       + " And dbo.Sales_SalesInvoiceMaster.CloseCashierDate=" + closeDate.ToString() + "   and  dbo.Sales_SalesInvoiceMaster.Cancel=0   GROUP BY dbo.Sales_SalesInvoiceMaster.DiscountOnTotal, Sales_SalesInvoiceMaster.NetBalance,dbo.Sales_SalesInvoiceMaster.NetAmount,dbo.Sales_SalesInvoiceMaster.ComputerInfo,dbo.Sales_SalesInvoiceMaster.InvoiceID, "
                       + " dbo.Sales_SalesMethodes.ArbName,dbo.Sales_SalesMethodes.MethodID Order By InvoiceID ASC";
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, MySession.GlobalLanguageName == iLanguage.Arabic ? "Arb" : "Eng");
                dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count > 0)
                {
                    model.FromSaleInvoice = Comon.cInt(dt.Rows[0]["InvoiceID"].ToString());
                    model.ToSaleInvoice = Comon.cInt(dt.Rows[dt.Rows.Count - 1]["InvoiceID"].ToString());
                    strSQL = "SELECT dbo.Sales_SalesInvoiceReturnMaster.InvoiceID,dbo.Sales_SalesInvoiceReturnMaster.ComputerInfo,Sales_SalesInvoiceReturnMaster.NetBalance as SumTotal,dbo.Sales_SalesInvoiceReturnMaster.DiscountOnTotal"

                      + " ,dbo.Sales_SalesInvoiceReturnMaster.NetAmount, dbo.Sales_SalesMethodes.ArbName As SaleTypeName,dbo.Sales_SalesMethodes.MethodID As SaleTypeID"
                      + " FROM dbo.Sales_SalesMethodes INNER JOIN dbo.Sales_SalesInvoiceReturnMaster ON dbo.Sales_SalesMethodes.MethodID = dbo.Sales_SalesInvoiceReturnMaster.MethodeID LEFT OUTER JOIN"
                      + " dbo.Sales_SalesInvoiceReturnDetails ON dbo.Sales_SalesInvoiceReturnMaster.BranchID = dbo.Sales_SalesInvoiceReturnDetails.BranchID AND dbo.Sales_SalesInvoiceReturnMaster.InvoiceID = "
                      + " dbo.Sales_SalesInvoiceReturnDetails.InvoiceID Where Sales_SalesInvoiceReturnMaster.BranchID=" + MySession.GlobalBranchID + " And Sales_SalesInvoiceReturnMaster.UserID=" + txtUserID.Text
                      + " And dbo.Sales_SalesInvoiceReturnMaster.CloseCashierDate=" + closeDate.ToString() + "   and  dbo.Sales_SalesInvoiceReturnMaster.Cancel=0   GROUP BY dbo.Sales_SalesInvoiceReturnMaster.DiscountOnTotal, Sales_SalesInvoiceReturnMaster.NetBalance,dbo.Sales_SalesInvoiceReturnMaster.NetAmount,dbo.Sales_SalesInvoiceReturnMaster.ComputerInfo,dbo.Sales_SalesInvoiceReturnMaster.InvoiceID, "
                      + " dbo.Sales_SalesMethodes.ArbName,dbo.Sales_SalesMethodes.MethodID Order By InvoiceID ASC";

                    DataTable dtReturn = new DataTable();

                    dtReturn = Lip.SelectRecord(strSQL);
                    decimal netSumReturn = 0;
                    decimal TotalNetReturn = 0;
                    decimal TotalCashReturn = 0;
                    decimal netCashSumReturn = 0;
                    decimal caschPaidWithNetReturn = 0;
                    decimal cashReturn = 0;
                    decimal futureReturn = 0;
                    decimal baqiReturn = 0;
                    decimal TotalReturn = 0;
                    int ContReturn = 0;

                    if (dtReturn.Rows.Count > 0)
                    {
                        model.FromSaleInvoiceReturn = Comon.cInt(dtReturn.Rows[0]["InvoiceID"].ToString());
                        model.ToSaleInvoiceReturn = Comon.cInt(dtReturn.Rows[dtReturn.Rows.Count - 1]["InvoiceID"].ToString());
                        foreach (DataRow drow1 in dtReturn.Rows)
                        {
                            TotalReturn += Comon.ConvertToDecimalPrice(drow1["SumTotal"]);
                            switch (Comon.cInt(drow1["SaleTypeID"].ToString()))
                            {

                                case (1):
                                    cashReturn += (Comon.ConvertToDecimalPrice(drow1["SumTotal"]));//- Comon.ConvertToDecimalPrice(dt.Rows[i]["DiscountOnTotal"])));

                                    break;
                                case (2):
                                    futureReturn += (Comon.ConvertToDecimalPrice(drow1["SumTotal"]));//- Comon.ConvertToDecimalPrice(dt.Rows[i]["DiscountOnTotal"])));

                                    break;
                                case (3):
                                    netSumReturn += (Comon.ConvertToDecimalPrice(drow1["SumTotal"]));//- Comon.ConvertToDecimalPrice(dt.Rows[i]["DiscountOnTotal"])));
                                    //   row["NetPaid"] = dt.Rows[i]["NetProcessID"].ToString();
                                    break;
                                case (5):
                                    netCashSumReturn += Comon.ConvertToDecimalPrice(drow1["SumTotal"]);// - Comon.ConvertToDecimalPrice(dt.Rows[i]["DiscountOnTotal"])));
                                    caschPaidWithNetReturn += Comon.ConvertToDecimalPrice(drow1["NetAmount"]);
                                    // row["NetPaid"] = dt.Rows[i]["NetProcessID"].ToString();
                                    break;
                            }


                        }
                        SumReturn = Comon.ConvertToDecimalPrice(dtReturn.Rows[0][0].ToString());
                    }
                    baqiReturn = netCashSumReturn - caschPaidWithNetReturn;
                    TotalCashReturn = baqiReturn + cashReturn;
                    TotalNetReturn = netSumReturn + caschPaidWithNetReturn;
                    ContReturn = dtReturn.Rows.Count;

                    ReportName = "rptTotalCashierSales";
                    string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");
                    XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);
                    decimal netSum = 0;
                    decimal netCashSum = 0;
                    decimal caschPaidWithNet = 0;
                    decimal cash = 0;
                    decimal future = 0;
                    decimal TotalNetSales = 0;
                    decimal TotalCashSales = 0;
                    decimal baqi = 0;
                    /********************** Master *****************************/
                    rptForm.RequestParameters = false;
                    rptForm.Parameters["ComputerInfo"].Value = MySession.GlobalComputerInfo.ToString() + lblUserName.Text;
                    rptForm.Parameters["TheDate"].Value = Comon.ConvertSerialDateTo(closeDate.ToString());
                    rptForm.Parameters["TheTime"].Value = Lip.GetServerTime().ToString();
                    rptForm.Parameters["SumReturn"].Value = SumReturn.ToString();

                    var dataTable = new dsReports.rptTotalCashierSalesDataTable();

                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        var row = dataTable.NewRow();

                        row["InvoiceID"] = dt.Rows[i]["InvoiceID"].ToString();
                        row["Total"] = dt.Rows[i]["SumTotal"].ToString();
                        row["TypeName"] = dt.Rows[i]["SaleTypeName"].ToString();
                        row["Discount"] = dt.Rows[i]["DiscountOnTotal"].ToString();
                        row["Net"] = (Comon.ConvertToDecimalPrice(dt.Rows[i]["SumTotal"]));


                        // - Comon.ConvertToDecimalPrice(dt.Rows[i]["DiscountOnTotal"])).ToString();
                        switch (Comon.cInt(dt.Rows[i]["SaleTypeID"].ToString()))
                        {

                            case (1):
                                cash += (Comon.ConvertToDecimalPrice(dt.Rows[i]["SumTotal"]));//- Comon.ConvertToDecimalPrice(dt.Rows[i]["DiscountOnTotal"])));
                                row["NetPaid"] = "-";
                                break;
                            case (2):
                                future += (Comon.ConvertToDecimalPrice(dt.Rows[i]["SumTotal"]));//- Comon.ConvertToDecimalPrice(dt.Rows[i]["DiscountOnTotal"])));
                                row["NetPaid"] = "-";
                                break;
                            case (3):
                                netSum += (Comon.ConvertToDecimalPrice(dt.Rows[i]["SumTotal"]));//- Comon.ConvertToDecimalPrice(dt.Rows[i]["DiscountOnTotal"])));
                                //   row["NetPaid"] = dt.Rows[i]["NetProcessID"].ToString();
                                break;
                            case (5):
                                netCashSum += Comon.ConvertToDecimalPrice(dt.Rows[i]["SumTotal"]);// - Comon.ConvertToDecimalPrice(dt.Rows[i]["DiscountOnTotal"])));
                                caschPaidWithNet += Comon.ConvertToDecimalPrice(dt.Rows[i]["NetAmount"]);
                                // row["NetPaid"] = dt.Rows[i]["NetProcessID"].ToString();
                                break;
                        }
                        dataTable.Rows.Add(row);
                    }
                    baqi = netCashSum - caschPaidWithNet;
                    TotalCashSales = baqi + cash;
                    TotalNetSales = netSum + caschPaidWithNet;


                    rptForm.Parameters["NetCahSum"].Value = netCashSum.ToString();
                    rptForm.Parameters["NetSum"].Value = netSum.ToString();
                    rptForm.Parameters["CashSum"].Value = cash.ToString();
                    rptForm.Parameters["caschPaidWithNet"].Value = caschPaidWithNet.ToString();
                    rptForm.Parameters["baqi"].Value = baqi.ToString();
                    rptForm.Parameters["TotalCashSales"].Value = TotalCashSales.ToString();
                    rptForm.Parameters["TotalNetSales"].Value = TotalNetSales.ToString();




                    rptForm.Parameters["NetBalanceReturn"].Value = TotalReturn.ToString();
                    rptForm.Parameters["NetCahSumReturn"].Value = netCashSumReturn.ToString();
                    rptForm.Parameters["NetSumReturn"].Value = netSumReturn.ToString();
                    rptForm.Parameters["CashSumReturn"].Value = cashReturn.ToString();
                    rptForm.Parameters["caschPaidWithNetReturn"].Value = caschPaidWithNetReturn.ToString();
                    rptForm.Parameters["baqiReturn"].Value = baqiReturn.ToString();
                    rptForm.Parameters["ContReturn"].Value = ContReturn.ToString();
                    rptForm.Parameters["TotalCashReturn"].Value = TotalCashReturn.ToString();
                    rptForm.Parameters["TotalNetReturn"].Value = TotalNetReturn.ToString();

                    rptForm.DataSource = dataTable;
                    rptForm.DataMember = "rptTotalCashierSales";

                    /******************** Report Binding ************************/

                    rptForm.ShowPrintStatusDialog = false;
                    rptForm.ShowPrintMarginsWarning = false;
                    rptForm.CreateDocument();
                    for (int i = 0; i < rptForm.Parameters.Count; i++)
                        rptForm.Parameters[i].Visible = false;
                    goOn = 0;
                   
                    SplashScreenManager.CloseForm(false);

                    if (ShowReportInReportViewer==false)
                    {
                        frmReportViewer frmRptViewer = new frmReportViewer();
                        frmRptViewer.documentViewer1.DocumentSource = rptForm;
                        frmRptViewer.ShowDialog();
                    }
                    else
                    {
                        bool IsSelectedPrinter = false;
                        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                        DataTable dt1 = ReportComponent.SelectRecord("SELECT *  from Printers where ReportName='" + ReportName + "'");
                        if (dt1.Rows.Count > 0) for (int i = 1; i < 6; i++)
                            {
                                string PrinterName = dt1.Rows[0]["PrinterName" + i.ToString()].ToString().ToUpper();
                                if (!string.IsNullOrEmpty(PrinterName))
                                {
                                    rptForm.PrinterName = PrinterName;
                                    rptForm.Print(PrinterName);
                                    IsSelectedPrinter = true;
                                    IsPrinting = true;
                                }
                            }
                        SplashScreenManager.CloseForm(false);
                        if (!IsSelectedPrinter)
                            Messages.MsgWarning(Messages.TitleWorning, Messages.msgThereIsNotPrinterSelected);
                    }
                }

                else
                {
                    SplashScreenManager.CloseForm(false);
                    Messages.MsgWarning(Messages.TitleWorning, (UserInfo.Language == iLanguage.Arabic ? "لا توجد فواتير سابقة لطباعتها" : "There isnNo Previous Invoices to print it."));
                }
            }
            catch (Exception ex)
            {

                goOn = 0;
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);

            }
        }

        private void btnPrintTotalInvoices_Click(object sender, EventArgs e)
        {
            DataTable dtt;
            dtt = Lip.SelectRecord("SELECT BranchID FROM  Users  where UserID=" + txtUserID.Text);
            decimal SumReturn = 0;
            SalesCashierClose model = new SalesCashierClose();
            model.FromSaleInvoice = 0;
            model.ToSaleInvoice = 0;
            model.FromSaleInvoiceReturn = 0;
            model.ToSaleInvoiceReturn = 0;
            try
            {
                if (string.IsNullOrEmpty(txtUserID.Text))
                {
                    Messages.MsgWarning(Messages.TitleWorning, (UserInfo.Language == iLanguage.Arabic ? " الرحاء ادخال رقم المستخدم " : "You must Enter User ID"));

                    return;
                }
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                DataTable dt = new DataTable();
                //,dbo.Sales_SalesInvoiceMaster.DiscountOnTotal,SUM(dbo.Sales_SalesInvoiceDetails.QTY * dbo.Sales_SalesInvoiceDetails.SalePrice - dbo.Sales_SalesInvoiceDetails.Discount+ dbo.Sales_SalesInvoiceDetails.AdditionalValue) AS SumTotal1,
                strSQL = "SELECT dbo.Sales_SalesInvoiceMaster.InvoiceID,dbo.Sales_SalesInvoiceMaster.ComputerInfo,Sales_SalesInvoiceMaster.NetBalance as SumTotal,dbo.Sales_SalesInvoiceMaster.DiscountOnTotal"
                     
                       + " ,dbo.Sales_SalesInvoiceMaster.NetAmount, dbo.Sales_SalesMethodes.ArbName As SaleTypeName,dbo.Sales_SalesMethodes.MethodID As SaleTypeID"
                       + " FROM dbo.Sales_SalesMethodes INNER JOIN dbo.Sales_SalesInvoiceMaster ON dbo.Sales_SalesMethodes.MethodID = dbo.Sales_SalesInvoiceMaster.MethodeID LEFT OUTER JOIN"
                       + " dbo.Sales_SalesInvoiceDetails ON dbo.Sales_SalesInvoiceMaster.BranchID = dbo.Sales_SalesInvoiceDetails.BranchID AND dbo.Sales_SalesInvoiceMaster.InvoiceID = "
                       + " dbo.Sales_SalesInvoiceDetails.InvoiceID Where Sales_SalesInvoiceMaster.BranchID=" + dtt.Rows[0]["BranchID"] + " And Sales_SalesInvoiceMaster.UserID=" + txtUserID.Text
                       + " And dbo.Sales_SalesInvoiceMaster.CloseCashierDate=0   and  dbo.Sales_SalesInvoiceMaster.Cancel=0   GROUP BY dbo.Sales_SalesInvoiceMaster.DiscountOnTotal, Sales_SalesInvoiceMaster.NetBalance,dbo.Sales_SalesInvoiceMaster.NetAmount,dbo.Sales_SalesInvoiceMaster.ComputerInfo,dbo.Sales_SalesInvoiceMaster.InvoiceID, "
                       + " dbo.Sales_SalesMethodes.ArbName,dbo.Sales_SalesMethodes.MethodID Order By InvoiceID ASC";
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, MySession.GlobalLanguageName == iLanguage.Arabic ? "Arb" : "Eng");
                dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count > 0)
                {
                    model.FromSaleInvoice = Comon.cInt(dt.Rows[0]["InvoiceID"].ToString());
                    model.ToSaleInvoice = Comon.cInt(dt.Rows[dt.Rows.Count-1]["InvoiceID"].ToString());
                    strSQL = "SELECT dbo.Sales_SalesInvoiceReturnMaster.InvoiceID,dbo.Sales_SalesInvoiceReturnMaster.ComputerInfo,Sales_SalesInvoiceReturnMaster.NetBalance as SumTotal,dbo.Sales_SalesInvoiceReturnMaster.DiscountOnTotal"
                      + " ,dbo.Sales_SalesInvoiceReturnMaster.NetAmount, dbo.Sales_SalesMethodes.ArbName As SaleTypeName,dbo.Sales_SalesMethodes.MethodID As SaleTypeID"
                      + " FROM dbo.Sales_SalesMethodes INNER JOIN dbo.Sales_SalesInvoiceReturnMaster ON dbo.Sales_SalesMethodes.MethodID = dbo.Sales_SalesInvoiceReturnMaster.MethodeID LEFT OUTER JOIN"
                      + " dbo.Sales_SalesInvoiceReturnDetails ON dbo.Sales_SalesInvoiceReturnMaster.BranchID = dbo.Sales_SalesInvoiceReturnDetails.BranchID AND dbo.Sales_SalesInvoiceReturnMaster.InvoiceID = "
                      + " dbo.Sales_SalesInvoiceReturnDetails.InvoiceID Where Sales_SalesInvoiceReturnMaster.BranchID=" + dtt.Rows[0]["BranchID"] + " And Sales_SalesInvoiceReturnMaster.UserID=" + txtUserID.Text
                      + " And dbo.Sales_SalesInvoiceReturnMaster.CloseCashier=0   and  dbo.Sales_SalesInvoiceReturnMaster.Cancel=0   GROUP BY dbo.Sales_SalesInvoiceReturnMaster.DiscountOnTotal, Sales_SalesInvoiceReturnMaster.NetBalance,dbo.Sales_SalesInvoiceReturnMaster.NetAmount,dbo.Sales_SalesInvoiceReturnMaster.ComputerInfo,dbo.Sales_SalesInvoiceReturnMaster.InvoiceID, "
                      + " dbo.Sales_SalesMethodes.ArbName,dbo.Sales_SalesMethodes.MethodID Order By InvoiceID ASC";
                    DataTable dtReturn = new DataTable();
                    dtReturn = Lip.SelectRecord(strSQL);
                    decimal netSumReturn = 0;
                    decimal TotalNetReturn = 0;
                    decimal TotalCashReturn = 0;
                    decimal netCashSumReturn = 0;
                    decimal caschPaidWithNetReturn = 0;
                    decimal cashReturn = 0;
                    decimal futureReturn = 0;
                    decimal baqiReturn = 0;
                    decimal TotalReturn = 0;
                    int ContReturn = 0;

                    if (dtReturn.Rows.Count > 0) {
                        model.FromSaleInvoiceReturn = Comon.cInt(dtReturn.Rows[0]["InvoiceID"].ToString());
                        model.ToSaleInvoiceReturn = Comon.cInt(dtReturn.Rows[dtReturn.Rows.Count - 1]["InvoiceID"].ToString());
                        foreach(DataRow drow1 in dtReturn.Rows ){
                            TotalReturn += Comon.ConvertToDecimalPrice(drow1["SumTotal"]);
                           switch (Comon.cInt(drow1["SaleTypeID"].ToString()))
                        {

                            case (1):
                                cashReturn += (Comon.ConvertToDecimalPrice(drow1["SumTotal"]) );//- Comon.ConvertToDecimalPrice(dt.Rows[i]["DiscountOnTotal"])));
                               
                                break;
                            case (2):
                                futureReturn += (Comon.ConvertToDecimalPrice(drow1["SumTotal"]));//- Comon.ConvertToDecimalPrice(dt.Rows[i]["DiscountOnTotal"])));
                              
                                break;
                            case (3):
                                netSumReturn += (Comon.ConvertToDecimalPrice(drow1["SumTotal"])) ;//- Comon.ConvertToDecimalPrice(dt.Rows[i]["DiscountOnTotal"])));
                            //   row["NetPaid"] = dt.Rows[i]["NetProcessID"].ToString();
                                break;
                            case (5):
                                netCashSumReturn += Comon.ConvertToDecimalPrice(drow1["SumTotal"]);// - Comon.ConvertToDecimalPrice(dt.Rows[i]["DiscountOnTotal"])));
                                caschPaidWithNetReturn += Comon.ConvertToDecimalPrice(drow1["NetAmount"]);
                               // row["NetPaid"] = dt.Rows[i]["NetProcessID"].ToString();
                                break;
                        }


                        }
                        SumReturn = Comon.ConvertToDecimalPrice(dtReturn.Rows[0][0].ToString());
                    }
                    baqiReturn = netCashSumReturn - caschPaidWithNetReturn;
                    TotalCashReturn = baqiReturn + cashReturn;
                    TotalNetReturn = netSumReturn + caschPaidWithNetReturn;
                    ContReturn = dtReturn.Rows.Count;

                  if(chkPrintInvoice.Checked==true)
                       ReportName = "rptTotalCashierSalesInvoices";
                  else
                      ReportName = "rptTotalCashierSales";

                   
                    string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");
                    XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);
                    decimal netSum = 0;
                    decimal netCashSum = 0;
                    decimal caschPaidWithNet = 0;
                    decimal cash = 0;
                    decimal future = 0;
                    decimal TotalNetSales = 0;
                    decimal TotalCashSales = 0;
                    decimal baqi = 0;
                    decimal NetPostPaidSum = 0;
                    /********************** Master *****************************/
                    rptForm.RequestParameters = false;
                    rptForm.Parameters["ComputerInfo"].Value = lblUserName.Text;
                    rptForm.Parameters["TheDate"].Value = Lip.GetServerDate().ToString();
                    rptForm.Parameters["TheTime"].Value = Lip.GetServerTime().ToString();// DateTime.Today.TimeOfDay;
                    rptForm.Parameters["SumReturn"].Value = SumReturn.ToString();

                    var dataTable = new dsReports.rptTotalCashierSalesDataTable();

                    for (int i = 0; i <= dt.Rows.Count-1; i++)
                    {
                        var row = dataTable.NewRow();
                        row["InvoiceID"] = dt.Rows[i]["InvoiceID"].ToString();
                        row["Total"] = dt.Rows[i]["SumTotal"].ToString();
                        row["TypeName"] = dt.Rows[i]["SaleTypeName"].ToString();
                        row["Discount"] = dt.Rows[i]["DiscountOnTotal"].ToString();
                        row["Net"] = (Comon.ConvertToDecimalPrice(dt.Rows[i]["SumTotal"]));
                        // - Comon.ConvertToDecimalPrice(dt.Rows[i]["DiscountOnTotal"])).ToString();
                        switch (Comon.cInt(dt.Rows[i]["SaleTypeID"].ToString()))
                        {

                            case (1):
                                cash += (Comon.ConvertToDecimalPrice(dt.Rows[i]["SumTotal"]) );//- Comon.ConvertToDecimalPrice(dt.Rows[i]["DiscountOnTotal"])));
                                row["NetPaid"] = "-";
                                break;
                            case (2):
                                future += (Comon.ConvertToDecimalPrice(dt.Rows[i]["SumTotal"]));//- Comon.ConvertToDecimalPrice(dt.Rows[i]["DiscountOnTotal"])));
                                row["NetPaid"] = "-";
                                break;
                            case (3):
                                netSum += (Comon.ConvertToDecimalPrice(dt.Rows[i]["SumTotal"])) ;//- Comon.ConvertToDecimalPrice(dt.Rows[i]["DiscountOnTotal"])));
                            //   row["NetPaid"] = dt.Rows[i]["NetProcessID"].ToString();
                                break;
                                 
                                
                            case (5):
                                netCashSum += Comon.ConvertToDecimalPrice(dt.Rows[i]["SumTotal"]);// - Comon.ConvertToDecimalPrice(dt.Rows[i]["DiscountOnTotal"])));
                                caschPaidWithNet += Comon.ConvertToDecimalPrice(dt.Rows[i]["NetAmount"]);
                               // row["NetPaid"] = dt.Rows[i]["NetProcessID"].ToString();
                                break;
                        }
                        dataTable.Rows.Add(row);
                    }
                    baqi = netCashSum - caschPaidWithNet;
                    TotalCashSales = baqi + cash;
                   
                    TotalNetSales = netSum + caschPaidWithNet;
                  
                    rptForm.Parameters["NetCahSum"].Value = netCashSum.ToString();
                    rptForm.Parameters["NetSum"].Value = netSum.ToString();
                    rptForm.Parameters["CashSum"].Value = cash.ToString();
                    rptForm.Parameters["caschPaidWithNet"].Value = caschPaidWithNet.ToString();
                    rptForm.Parameters["NetPostPaidSum"].Value = future.ToString();
                    rptForm.Parameters["baqi"].Value = baqi.ToString();
                    rptForm.Parameters["TotalCashSales"].Value = TotalCashSales.ToString();
                    rptForm.Parameters["TotalNetSales"].Value = TotalNetSales.ToString();
                    rptForm.Parameters["NetBalanceReturn"].Value = TotalReturn.ToString();
                    rptForm.Parameters["NetCahSumReturn"].Value = netCashSumReturn.ToString();
                    rptForm.Parameters["NetSumReturn"].Value = netSumReturn.ToString();
                    rptForm.Parameters["CashSumReturn"].Value = cashReturn.ToString();
                    rptForm.Parameters["NetReturnPostPaidSum"].Value = futureReturn.ToString();
                    rptForm.Parameters["caschPaidWithNetReturn"].Value = caschPaidWithNetReturn.ToString();
                    rptForm.Parameters["baqiReturn"].Value = baqiReturn.ToString();
                    rptForm.Parameters["ContReturn"].Value = ContReturn.ToString();
                    rptForm.Parameters["TotalCashReturn"].Value = TotalCashReturn.ToString();
                    rptForm.Parameters["TotalNetReturn"].Value = TotalNetReturn.ToString();

                    decimal TotalCashSalesWNet = 0;

                    //TotalCashSalesWNet = TotalNetSales + TotalCashSales;
                    TotalCashSalesWNet = netCashSum + netSum + cash + future;
                    rptForm.Parameters["FutureSumReturn"].Value = TotalCashSalesWNet - TotalReturn;


                    var dtGroups = new dsReports.ByCatgeryReportDataTable();
                    var dtSalee = new dsReports.rptPurchaseInvoiceDataTable();
                    DataSet dst = new DataSet();
                    var row1 = dtSalee.NewRow();
                    row1["ItemName"] = "";
                    row1["SizeName"] = "";
                    dtSalee.Rows.Add(row1);
                    dst.Tables.Add(dtSalee);
                    var dr = Lip.SelectRecord(GetStrSQL());
                    if (dr.Rows.Count > 0)
                    {
                        foreach (DataRow drow in dr.Rows)
                        {
                            DataRow row;
                            row = dtGroups.NewRow();
                            row["GroupName"] = drow["ItemName"].ToString();
                            row["INQty"] = Comon.ConvertToDecimalQty(drow["TotalSales"].ToString());
                            row["OutQty"] = Comon.ConvertToDecimalQty(drow["TotalQTY"].ToString());
                            dtGroups.Rows.Add(row);


                        }
                       
                            dst.Tables.Add(dtGroups);

                    }

                    dst.Tables.Add(dataTable);



                    rptForm.DataSource = dst;
                    /******************** Report Binding ************************/

                    rptForm.ShowPrintStatusDialog = false;
                    rptForm.ShowPrintMarginsWarning = false;
                    rptForm.CreateDocument();
                    for (int i = 0; i < rptForm.Parameters.Count; i++)
                        rptForm.Parameters[i].Visible = false;
                    if (goOn == 1)
                    {
                        try
                        {

                            model.SellerID = Comon.cInt(MySession.GlobalDefaultSellerID);
                            model.CloseCashierID = GetNewID();
                            model.CloseCashierDate = Comon.cLong((Lip.GetServerDateSerial()));
                            model.EnterCost = TotalCashReturn;
                            model.NetSum = TotalNetSales;
                            model.FutureSum = future;
                            model.UserID = UserInfo.ID;
                            model.CashSum = TotalCashSales;
                            model.PrevoiusCash = TotalNetReturn;
                            int result = CloseCashierDAL.Insert(model);
                        }
                        catch { goOn = 0; }
                    }
                    SplashScreenManager.CloseForm(false);
                   
                    if (ShowReportInReportViewer==false)
                    {
                        frmReportViewer frmRptViewer = new frmReportViewer();
                        frmRptViewer.documentViewer1.DocumentSource = rptForm;
                        frmRptViewer.ShowDialog();
                    }
                    else
                    {
                        bool IsSelectedPrinter = false;
                        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                        DataTable dt1 = ReportComponent.SelectRecord("SELECT *  from Printers where ReportName='" + ReportName + "'");
                        if (dt1.Rows.Count > 0) for (int i = 1; i < 6; i++)
                            {
                                string PrinterName = dt1.Rows[0]["PrinterName" + i.ToString()].ToString().ToUpper();
                                if (!string.IsNullOrEmpty(PrinterName))
                                {
                                    rptForm.PrinterName = PrinterName;
                                    rptForm.Print(PrinterName);
                                    IsSelectedPrinter = true;
                                    IsPrinting = true;
                                }
                            }
                        SplashScreenManager.CloseForm(false);
                        if (!IsSelectedPrinter)
                            Messages.MsgWarning(Messages.TitleWorning, Messages.msgThereIsNotPrinterSelected);
                    }
                }

                else
                {
                    SplashScreenManager.CloseForm(false);
                    Messages.MsgWarning(Messages.TitleWorning, (UserInfo.Language == iLanguage.Arabic ? "لا توجد فواتير سابقة لطباعتها" : "There isnNo Previous Invoices to print it."));
                }
            }
            catch (Exception ex)
            {

                goOn = 0 ;
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                this.Close();
            }
        }
        private string getStoreName(string StoreID)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE StoreID =" + Comon.cInt(StoreID) + " And Cancel =0 And  BranchID =" + UserInfo.BRANCHID;
                return Lip.SelectRecord(strSQL).Rows[0][0].ToString();
            }
            catch (Exception ex)
            {

                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return "";
            }

        }
        public int GetNewID()
        {
            try
            {
                DataTable dt;
                string strSQL;
                strSQL = "SELECT Max(" + PremaryKey + ") + 1 FROM " + TableName;
                dt = Lip.SelectRecord(strSQL);
                string GetNewID = dt.Rows[0][0] == DBNull.Value ? "1" : dt.Rows[0][0].ToString();
                return Convert.ToInt32(GetNewID);

            }
            catch (Exception ex)
            {
                return 0;
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private string getCustomerName(string AccountID)
        {
            try
            {

                strSQL = "SELECT " + PrimaryName + " as [CustomerName]  FROM Sales_CustomerAnSublierListArb Where AcountID=" + AccountID + " AND  BranchID =" + MySession.GlobalBranchID;
                DataTable dt = Lip.SelectRecord(strSQL);
                if (dt != null && dt.Rows.Count > 0)
                    return dt.Rows[0][0].ToString();
                else
                    return "";
            }
            catch (Exception ex)
            {

                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return "";
            }

        }
        private string getCostCenterName(string CostCenterID)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as CostCenterName FROM Acc_CostCenters WHERE CostCenterID =" + Comon.cInt(CostCenterID) + " And Cancel =0 And  BranchID =" + UserInfo.BRANCHID;
                return Lip.SelectRecord(strSQL).Rows[0][0].ToString();
            }
            catch (Exception ex)
            {

                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return "";
            }

        }

        private void txtUserID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as UserName FROM Users WHERE (UserID =" + txtUserID.Text + ") And Cancel =0 ";
                CSearch.ControlValidating(txtUserID, lblUserName, strSQL);

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

       



        string GetStrSQL()
        {
            try
            {
              
               
              
                string filter = "(.Sales_TotalSales.BranchID = " + UserInfo.BRANCHID + ")    AND";

                strSQL = "";
                long FromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text));
                long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));

                // حسب التاريخ
                if (FromDate != 0)
                    filter = filter + " .Sales_TotalSales.InvoiceDate >=" + FromDate + " AND ";

                if (ToDate != 0)
                    filter = filter + " .Sales_TotalSales.InvoiceDate <=" + FromDate + " AND ";

                filter = filter + " .Stc_Items.GroupID >" + 1 + " AND ";

                filter = filter.Remove(filter.Length - 4, 4);
                //  dbo.Sales_PurchaseInvoiceReturnMaster.AdditionaAmmountTotal, غير موجود في جدول مردود المشتريات
                strSQL = "SELECT SUM(dbo.Sales_TotalSales.QTY) AS TotalQTY, SUM(dbo.Sales_TotalSales.TotalSales) AS TotalSales, "

     + " SUM(dbo.Sales_TotalSales.Discount) AS TotalDiscount, dbo.Sales_TotalSales.BarCode, dbo.Stc_Items.ArbName AS ItemName "

   + " FROM dbo.Stc_Items RIGHT OUTER JOIN dbo.Stc_ItemUnits ON dbo.Stc_Items.ItemID = dbo.Stc_ItemUnits.ItemID RIGHT OUTER JOIN"
              + " dbo.Sales_TotalSales ON dbo.Stc_ItemUnits.BarCode = dbo.Sales_TotalSales.BarCode WHERE " + filter
              + "  GROUP BY dbo.Sales_TotalSales.BarCode, dbo.Stc_Items.ArbName ";
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());


            }

            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);


                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

           return strSQL ;
        }

    }
}

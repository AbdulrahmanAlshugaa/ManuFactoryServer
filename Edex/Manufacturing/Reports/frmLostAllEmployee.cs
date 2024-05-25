﻿using System;
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
using DevExpress.XtraReports.UI;

namespace Edex.Manufacturing.Reports
{
    public partial class frmLostAllEmployee : BaseForm
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
        #endregion
        public frmLostAllEmployee()
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
                    dvgColDebit.Caption = "Debit";
                    dvgColCredit.Caption = "Credit";
                    dvgColBalance.Caption = "Balance";
                    dvgColBusy.Caption = "Worked gold";
                    dvgColLost.Caption = "Lost";
                    dvgColAllowPer.Caption = "Allow QTY";
                    dvgColDeffirant.Caption = "Difference";
                    dvgColEmployeeName.Caption = "Employee Name";
                    dvgColEmpFactorID.Caption = "Employee ID";
                }

                FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
                cmbBranchesID.EditValue = MySession.GlobalBranchID;
                cmbBranchesID.ReadOnly = !MySession.GlobalAllowBranchModificationAllScreens;
                txtCustomerID.Validating += txtCustomerID_Validating;
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
        protected override void DoPrint()
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

                bool IncludeHeader = true;
                string rptFromName = "rptKhayasAllEmpolyees";
                rptFromName += (UserInfo.Language == iLanguage.English ? "Eng" : "Arb");
                XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFromName + ".repx", true);

                /********************** Master *****************************/
                rptForm.RequestParameters = false;
                rptForm.Parameters["ReportName"].Value = this.Text;
                //rptForm.Parameters["EmpNameFactor"].Value = lblEmpNameFactor.Text.ToString();
                rptForm.Parameters["AccountNameFactory"].Value = lblAccountNameFactory.Text.Trim().ToString();
                rptForm.Parameters["CustomerName"].Value = lblCustomerName.Text.Trim().ToString();
                rptForm.Parameters["BranchName"].Value = cmbBranchesID.Text.Trim().ToString();
                rptForm.Parameters["ToDate"].Value = txtToDate.Text.Trim().ToString();
                rptForm.Parameters["FromDate"].Value = txtFromDate.Text.Trim().ToString();
                /********Total*********/



                for (int i = 0; i < rptForm.Parameters.Count; i++)
                    rptForm.Parameters[i].Visible = false;
                /********************** Details ****************************/
                var dataTable = new dsReports.rptKhayasAllEmpolyeesDataTable();

                for (int i = 0; i <= GridLost.DataRowCount - 1; i++)
                {
                    var row = dataTable.NewRow();
                    row["FactoreID"] = GridLost.GetRowCellValue(i, "EmpFactorID").ToString();
                    row["FactoreName"] = GridLost.GetRowCellValue(i, "EmployeeName").ToString();
                    //row["Customer"] = GridLost.GetRowCellValue(i, "Customer").ToString();
                    row["Debit"] = GridLost.GetRowCellValue(i, "Debit").ToString();
                    row["Credit"] = GridLost.GetRowCellValue(i, "Credit").ToString();
                    row["Balance"] = GridLost.GetRowCellValue(i, "Balance").ToString();
                    row["Busy"] = GridLost.GetRowCellValue(i, "Busy").ToString();
                    row["Lost"] = GridLost.GetRowCellValue(i, "Lost").ToString();
                    row["AllowPer"] = GridLost.GetRowCellValue(i, "AllowPer").ToString();
                    row["Deffirant"] = GridLost.GetRowCellValue(i, "Deffirant").ToString();

                    dataTable.Rows.Add(row);
                }
                rptForm.DataSource = dataTable;
                rptForm.DataMember = "rptKhayasAllEmpolyees";

                /******************** Report Binding ************************/

                XRSubreport subreport = (XRSubreport)rptForm.FindControl("subRptCompanyHeader", true);
                subreport.Visible = IncludeHeader;
                subreport.ReportSource = ReportComponent.CompanyHeader();

                rptForm.CreateDocument();

                SplashScreenManager.CloseForm(false);

                frmReportViewer frmRptViewer = new frmReportViewer();
                frmRptViewer.documentViewer1.DocumentSource = rptForm;

                frmRptViewer.ShowDialog();
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
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
                filter = "  MFCM.Cancel = 0    AND MFCM.TypeStageID = 6 And ";

                if (Comon.cInt(cmbBranchesID.Text) != 0)
                    filter = filter + "   MFCM.BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + " AND ";

                if (Comon.cDbl(txtAccountIDFactory.Text) > 0)
                    filter = filter + " MFCM.AccountIDFactory=" + Comon.cDbl(txtAccountIDFactory.Text) + " AND ";
                if (Comon.cDbl(txtOrderID.Text) > 0)
                    filter = filter + " MFCM.Barcode='" + txtOrderID.Text + "' AND ";
                if (Comon.cDbl(txtCustomerID.Text) > 0)
                    filter = filter + " MOR.CustomerID=" + Comon.cDbl(txtCustomerID.Text) + " AND ";
              
                long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));
                long FromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text));
                if (FromDate != 0)
                    filter = filter + " MFCM.ComandDate>=" + FromDate + " AND ";

                if (ToDate != 0)
                    filter = filter + " MFCM.ComandDate<=" + ToDate + " AND ";

                filter = filter.Remove(filter.Length - 4, 4);
//                string str = @"SELECT  MFCM.EmpFactorID ,HREF.ArbName as EmployeeName ,
//                    ISNULL(SUM(MFCF.Debit), 0) AS Debit,
//                    ISNULL(SUM(MFCF.Credit), 0) AS Credit,
//                    (ISNULL(SUM(MFCF.Debit), 0) - ISNULL(SUM(MFCF.Credit), 0)) AS Balance,
//                    (SELECT ISNULL(SUM(Credit), 0) 
//                     FROM Menu_FactoryRunCommandfactory 
//                     WHERE HimLost = 1 AND ComandID = MFCM.ComandID) AS Busy,
//	                 ((ISNULL(SUM(MFCF.Debit), 0)-
//	                  (SELECT ISNULL(SUM(Menu_FactoryRunCommandfactory.Credit), 0) FROM Menu_FactoryRunCommandfactory  WHERE HimLost <>1 AND ComandID = MFCM.ComandID)
//	                  -(SELECT ISNULL(SUM(Credit), 0)   FROM Menu_FactoryRunCommandfactory   WHERE HimLost = 1 AND ComandID = MFCM.ComandID))) AS Lost,
//					  ((SELECT ISNULL(SUM(Credit), 0)   FROM Menu_FactoryRunCommandfactory    WHERE HimLost = 1 AND ComandID = MFCM.ComandID)*(select AllowQTYPer from HR_EmployeeFile where EmployeeID= MFCM.EmpFactorID  and Cancel=0)/100) as AllowPer,
//                  0.000 as Deffirant,
//                    SC.ArbName as Customer,
//                    MOR.CustomerID
//                FROM
//                    dbo.Manu_OrderRestriction AS MOR
//                INNER JOIN dbo.Menu_FactoryRunCommandfactory AS MFCF ON MOR.BranchID = MFCF.BranchID
//                INNER JOIN dbo.Menu_FactoryRunCommandMaster AS MFCM ON MOR.OrderID = MFCM.Barcode AND MFCF.ComandID = MFCM.ComandID AND MFCF.TypeStageID = MFCM.TypeStageID 
//                LEFT OUTER JOIN dbo.Sales_Customers AS SC ON MOR.CustomerID = SC.AccountID
//                INNER JOIN dbo.HR_EmployeeFile AS HREF ON MFCM.EmpFactorID = HREF.EmployeeID
//                WHERE " + filter + @"    GROUP BY  
//                    SC.ArbName,
//                    MOR.CustomerID, 
//                    MFCM.ComandID,MFCM.EmpFactorID,HREF.ArbName";
                string str = @"SELECT  MFCM.EmpFactorID ,HREF.ArbName as EmployeeName ,
                    ISNULL(SUM(MFCF.Debit), 0) AS Debit,
                    ISNULL(SUM(MFCF.Credit), 0) AS Credit,
                    (ISNULL(SUM(MFCF.Debit), 0) - ISNULL(SUM(MFCF.Credit), 0)) AS Balance,
                    0.00 AS Busy,
                    0.00 AS Lost,
                    0.00 AS AllowPer, 
                    0.000 as Deffirant 
                FROM
                    dbo.Manu_OrderRestriction AS MOR
                INNER JOIN dbo.Menu_FactoryRunCommandfactory AS MFCF ON MOR.BranchID = MFCF.BranchID
                INNER JOIN dbo.Menu_FactoryRunCommandMaster AS MFCM ON MOR.OrderID = MFCM.Barcode AND MFCF.ComandID = MFCM.ComandID and MFCF.BranchID = MFCM.BranchID AND MFCF.TypeStageID = MFCM.TypeStageID 
                
                INNER JOIN dbo.HR_EmployeeFile AS HREF ON MFCM.EmpFactorID = HREF.EmployeeID and MFCM.BranchID = HREF.BranchID
                WHERE " + filter + @"    GROUP BY    MFCM.EmpFactorID,HREF.ArbName";
                dt = Lip.SelectRecord(str);
                if (dt.Rows.Count > 0)
                {

                    dt.Columns["Deffirant"].ReadOnly = false;
                    dt.Columns["Busy"].ReadOnly = false;
                    dt.Columns["Lost"].ReadOnly = false;
                    dt.Columns["AllowPer"].ReadOnly = false;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                   
                        dt.Rows[i]["Busy"] = Comon.cDec(Lip.GetValue(@"SELECT        ISNULL(SUM( MFCF.Credit), 0)  FROM dbo.Menu_FactoryRunCommandfactory as MFCF INNER JOIN
                                  dbo.Menu_FactoryRunCommandMaster as MFCM ON  MFCF.TypeStageID = MFCM.TypeStageID AND 
                                  MFCF.ComandID =  MFCM.ComandID and  MFCF.BranchID =  MFCM.BranchID
                                  WHERE ( MFCF.HimLost = 1) and  MFCM.EmpFactorID =" + dt.Rows[i]["EmpFactorID"] + " and " + filter));
                        dt.Rows[i]["Lost"] = Comon.cDec(dt.Rows[i]["Debit"]) - Comon.cDec(Lip.GetValue(@"SELECT        ISNULL(SUM( MFCF.Credit), 0)  FROM dbo.Menu_FactoryRunCommandfactory as MFCF INNER JOIN
                                  dbo.Menu_FactoryRunCommandMaster as MFCM ON  MFCF.TypeStageID = MFCM.TypeStageID AND 
                                  MFCF.ComandID =  MFCM.ComandID and     MFCF.BranchID =  MFCM.BranchID
                                  WHERE ( MFCF.HimLost <> 1) and  MFCM.EmpFactorID =" + dt.Rows[i]["EmpFactorID"] + " and " + filter)) - Comon.cDec(dt.Rows[i]["Busy"]);
                        int str1 = Comon.cInt(Lip.GetValue(" select AllowQTYPer from HR_EmployeeFile where EmployeeID=" + dt.Rows[i]["EmpFactorID"] + " and Cancel=0 and BranchID="+  Comon.cInt(cmbBranchesID.EditValue)));
                        dt.Rows[i]["AllowPer"] = Comon.cDec(Comon.cDec(dt.Rows[i]["Busy"]) * (Comon.cDec(str1) / 100));
                        dt.Rows[i]["Deffirant"] = Comon.cDec(Comon.cDec(dt.Rows[i]["Lost"]) - Comon.cDec(dt.Rows[i]["AllowPer"]));

                    }
                    gridControl1.DataSource = dt;
                }
                else
                {
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
                txtCustomerID.Text = "";
                txtCustomerID_Validating(null, null);
                txtOrderID.Text = "";
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
            
            else if (FocusedControl.Trim() == txtCustomerID.Name)
            {

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtCustomerID, lblCustomerName, "CustomerIDAndSublierID", "رقم الــعـــمـــيــل",   Comon.cInt(cmbBranchesID.EditValue));
                else
                    PrepareSearchQuery.Find(ref cls, txtCustomerID, lblCustomerName, "CustomerIDAndSublierID", "SublierID ID",   Comon.cInt(cmbBranchesID.EditValue));
            }
            else if (FocusedControl.Trim() == txtAccountIDFactory.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtAccountIDFactory, lblAccountNameFactory, "AccountID", "رقم الحساب", Comon.cInt(cmbBranchesID.EditValue));
                else
                    PrepareSearchQuery.Find(ref cls, txtAccountIDFactory, lblAccountNameFactory, "AccountID", "Account ID", Comon.cInt(cmbBranchesID.EditValue));
            }
            else if (FocusedControl.Trim() == txtOrderID.Name)
            {

                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, txtOrderID, txtOrderID, "OrderID", "رقم الطلب", Comon.cInt(cmbBranchesID.EditValue));
                    else
                        PrepareSearchQuery.Find(ref cls, txtOrderID, txtOrderID, "OrderID", "Order ID", Comon.cInt(cmbBranchesID.EditValue));
                }
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
                if (FocusedControl == txtOrderID.Name)
                {
                    txtOrderID.Text = cls.PrimaryKeyValue.ToString();
                    //txtOrderID_Validating(null, null);
                }
                else if (FocusedControl == txtCustomerID.Name)
                {
                    txtCustomerID.Text = cls.PrimaryKeyValue.ToString();
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
                strSQL = "SELECT " + PrimaryName + " as AccountName FROM Acc_Accounts WHERE AccountID =" + Comon.cDbl(txtAccountIDFactory.Text) + " And Cancel =0 and BranchID="+  Comon.cInt(cmbBranchesID.EditValue);
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
                string strSql;
                DataTable dt;
                if (txtCustomerID.Text != string.Empty && txtCustomerID.Text != "0")
                {
                    strSQL = "SELECT " + PrimaryName + " as CustomerName  FROM Sales_CustomerAnSublierListArb Where  AcountID =" + txtCustomerID.Text + " and BranchID=" +   Comon.cInt(cmbBranchesID.EditValue);
                    Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, UserInfo.Language.ToString());
                    dt = Lip.SelectRecord(strSQL);
                    if (dt.Rows.Count > 0)
                    {
                        lblCustomerName.Text = dt.Rows[0]["CustomerName"].ToString();
                    }
                }
                else
                {
                    lblCustomerName.Text = "";
                    txtCustomerID.Text = "";
                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void frmLostAllEmployee_KeyDown(object sender, KeyEventArgs e)
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
        private void GridLost_DoubleClick(object sender, EventArgs e)
        {
                frmLostEmployeeReport frm = new frmLostEmployeeReport();
                    if (Edex.ModelSystem.Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm);

                        frm.Show();
                        if (GridLost.GetRowCellValue(GridLost.FocusedRowHandle, "EmpFactorID") != null)
                            frm.SetEmployeeFromOutScreen(GridLost.GetRowCellValue(GridLost.FocusedRowHandle, "EmpFactorID").ToString(),txtFromDate ,txtToDate );
                    }
                    else
                        frm.Dispose();
        }

    }
}
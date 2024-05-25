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
  
    public partial class frmSummaryLost : BaseForm
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
        public frmSummaryLost()
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
                    dvgColBalance.Caption = "Lost";
                    dvgColEmployeeName.Caption = "Employee Name";
                    dvgColEmpFactorID.Caption = "Employee ID";
                    dvgColCountStone.Caption = "Count Stone";
                    dvgColDiamond.Caption = "Diamond QTY";
                    dvgColStoneQTY.Caption = "Stone QTY";
                    dvgColZirconQTY.Caption = "Zircon QTY";

                }

                FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
                cmbBranchesID.EditValue =   MySession.GlobalBranchID;
                cmbBranchesID.ReadOnly = !MySession.GlobalAllowBranchModificationAllScreens;
                TypeStage = 9; 
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
        private void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow row;
                btnShow.Visible = false;
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                Application.DoEvents();
                filter = "  MFRM.Cancel = 0  And ";
                if (Comon.cInt(cmbBranchesID.Text) != 0)
                    filter = filter + "   MFRM.BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + " AND ";
                if (Comon.cDbl(txtAccountIDFactory.Text) > 0)
                    filter = filter + " MFRM.AccountIDFactory=" + Comon.cDbl(txtAccountIDFactory.Text) + " AND ";
                if (Comon.cDbl(txtOrderID.Text) > 0)
                    filter = filter + " MFRM.Barcode='" + txtOrderID.Text + "' AND ";
                if (Comon.cDbl(txtCustomerID.Text) > 0)
                    filter = filter + " MFRM.CustomerID=" + Comon.cDbl(txtCustomerID.Text) + " AND ";
                long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));
                long FromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text));
                if (FromDate != 0)
                    filter = filter + " MFRM.ComandDate>=" + FromDate + " AND ";
                if (ToDate != 0)
                    filter = filter + " MFRM.ComandDate<=" + ToDate + " AND ";
                filter = filter.Remove(filter.Length - 4, 4);
                string str = @"SELECT MFRM.AccountIDFactory as EmpFactorID,Acc." + PrimaryName + @" AS EmployeeName,
                        ISNULL(SUM(MFCF.Debit), 0) AS DebitMnuf, 
                        ISNULL(SUM(MFCF.Credit), 0) AS CreditMnuf,
                        (ISNULL(SUM(MFCF.Debit), 0) - ISNULL(SUM(MFCF.Credit), 0)) AS BalanceMnuf,
                        ISNULL(SUM(MFRCP.PrentagDebit), 0) AS PrentagDebit,
                        ISNULL(SUM(MFRCP.PrentagCredit), 0) AS PrentagCredit, 
                        (ISNULL(SUM(MFRCP.PrentagDebit), 0) - ISNULL(SUM(MFRCP.PrentagCredit), 0)) AS LostPrntage,
                        ISNULL(SUM(MFRCS.Debit), 0) AS AdditionDebit,
                        ISNULL(SUM(MFRCS.Credit), 0) AS AdditionCredit,
                        (ISNULL(SUM(MFRCS.Debit), 0) - ISNULL(SUM(MFRCS.Credit), 0)) AS LostAddition,
                        ISNULL(SUM(MFRCT.Debit), 0) AS PolishinDebit, 
                        ISNULL(SUM(MFRCT.Credit), 0) AS PolishinCredit,
                        (ISNULL(SUM(MFRCT.Debit), 0) - ISNULL(SUM(MFRCT.Credit), 0)) AS LostPolishin,                       
                        ISNULL(SUM(CASE WHEN MFRC.TypeOpration = 1 AND (MFRC.SizeID <> 2) THEN MFRC.ComWeightSton WHEN MFRC.TypeOpration = 1 AND (MFRC.SizeID = 2) THEN MFRC.ComWeightSton/5 ELSE 0 END), 0) AS ComStoneComDebit,
                        ISNULL(SUM(CASE WHEN MFRC.TypeOpration = 2 AND (MFRC.SizeID <> 2) THEN MFRC.ComWeightSton WHEN MFRC.TypeOpration = 2 AND (MFRC.SizeID = 2) THEN MFRC.ComWeightSton/5 ELSE 0 END), 0) AS ComStoneComCredit,
                        (ISNULL(SUM(CASE WHEN MFRC.TypeOpration = 1 AND (MFRC.SizeID <> 2) THEN MFRC.ComWeightSton WHEN MFRC.TypeOpration = 1 AND (MFRC.SizeID = 2) THEN MFRC.ComWeightSton/5 ELSE 0 END), 0) - ISNULL(SUM(CASE WHEN MFRC.TypeOpration = 2 AND (MFRC.SizeID <> 2) THEN MFRC.ComWeightSton WHEN MFRC.TypeOpration = 2 AND (MFRC.SizeID = 2) THEN MFRC.ComWeightSton/5 ELSE 0 END), 0)) AS ComStoneComLost
                        ,0.0 as AllLost,
                        0.0 as AllReturnFilings
                    FROM Acc_Accounts Acc
                    INNER JOIN Menu_FactoryRunCommandMaster MFRM ON Acc.AccountID = MFRM.AccountIDFactory
                    left outer JOIN Menu_FactoryRunCommandfactory MFCF ON MFRM.ComandID = MFCF.ComandID and MFRM.BranchID = MFCF.BranchID AND    MFRM.TypeStageID = MFCF.TypeStageID and MFRM.TypeStageID=6
                    left outer JOIN Menu_FactoryRunCommandCompund MFRC ON MFRM.ComandID = MFRC.ComandID and MFRM.BranchID = MFRC.BranchID AND    MFRM.TypeStageID = MFRC.TypeStageID and MFRM.TypeStageID=9
                    left outer JOIN Menu_FactoryRunCommandTalmee MFRCT ON MFRM.ComandID = MFRCT.ComandID and MFRM.BranchID = MFRCT.BranchID  AND   MFRM.TypeStageID = MFRCT.TypeStageID and (MFRM.TypeStageID=8 or MFRM.TypeStageID=13)
                    left outer JOIN Menu_FactoryRunCommandSelver MFRCS ON MFRM.ComandID = MFRCS.ComandID and MFRM.BranchID = MFRCS.BranchID   AND  MFRM.TypeStageID = MFRCS.TypeStageID and (MFRM.TypeStageID=11)
                    left outer JOIN Menu_FactoryRunCommandPrentagAndPulishn MFRCP ON MFRM.ComandID = MFRCP.ComandID  and MFRM.BranchID = MFRCP.BranchID AND  MFRM.TypeStageID = MFRCP.TypeStageID and (MFRM.TypeStageID=7 or MFRM.TypeStageID=12)
                    WHERE " + filter + @"
                    GROUP BY MFRM.AccountIDFactory, Acc." + PrimaryName;
                dt = Lip.SelectRecord(str);
                dt.Columns["BalanceMnuf"].ReadOnly = false;
                dt.Columns["AllLost"].ReadOnly = false;
                dt.Columns["AllReturnFilings"].ReadOnly = false;
                decimal Busy = 0;
                decimal Lost = 0;
                decimal AllowPer = 0;
                int str1 = 0;
                decimal GetReturnFilings = 0;
                string filter12 = " Mnu_ReturnFilingsMaster.Cancel=0  And ";
                if (FromDate != 0)
                    filter12 = filter12 + " Mnu_ReturnFilingsMaster.CommandDate>=" + FromDate + " AND ";
                if (ToDate != 0)
                    filter12 = filter12 + " Mnu_ReturnFilingsMaster.CommandDate<=" + ToDate + " AND ";
                filter12 = filter12.Remove(filter12.Length - 4, 4);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    GetReturnFilings = 0;
                    Busy = Comon.cDec(Lip.GetValue(@"SELECT  ISNULL(SUM( MFCF.Credit),0)  FROM Menu_FactoryRunCommandfactory as MFCF INNER JOIN
                                  dbo.Menu_FactoryRunCommandMaster as MFRM ON  MFCF.TypeStageID = MFRM.TypeStageID AND 
                                  MFCF.ComandID =  MFRM.ComandID and MFCF.BranchID =  MFRM.BranchID
                                  WHERE ( MFCF.HimLost = 1) and  MFRM.AccountIDFactory =" + dt.Rows[i]["EmpFactorID"] + " and " + filter));
                    Lost = Comon.cDec(dt.Rows[i]["DebitMnuf"]) - Comon.cDec(Lip.GetValue(@"SELECT ISNULL(SUM( MFCF.Credit), 0)  FROM dbo.Menu_FactoryRunCommandfactory as MFCF INNER JOIN
                                  dbo.Menu_FactoryRunCommandMaster as MFRM ON  MFCF.TypeStageID = MFRM.TypeStageID AND 
                                  MFCF.ComandID =  MFRM.ComandID and  MFCF.BranchID =  MFRM.BranchID
                                  WHERE ( MFCF.HimLost <> 1) and MFCF.TypeStageID=6 and  MFRM.AccountIDFactory =" + dt.Rows[i]["EmpFactorID"] + " and " + filter)) - Comon.cDec(Busy);
                    
                    if (Comon.cDbl(dt.Rows[i]["EmpFactorID"]) > 0)
                        str1 = Comon.cInt(Lip.GetValue(" select AllowQTYPer from HR_EmployeeFile where OnAccountID=" + dt.Rows[i]["EmpFactorID"] + " and Cancel=0  and BranchID="+Comon.cInt(cmbBranchesID.EditValue)));
                    
                    AllowPer = Comon.cDec(Comon.cDec(Busy) * (Comon.cDec(str1) / 100));
                    dt.Rows[i]["BalanceMnuf"] = Comon.cDec(Comon.cDec(Lost) - Comon.cDec(AllowPer));
                    dt.Rows[i]["AllLost"] = Comon.cDec(Comon.cDec(dt.Rows[i]["BalanceMnuf"]) + Comon.cDec(dt.Rows[i]["LostPrntage"]) + Comon.cDec(dt.Rows[i]["LostAddition"]) + Comon.cDec(dt.Rows[i]["LostPolishin"]) + Comon.cDec(dt.Rows[i]["ComStoneComLost"]));
                    
                    
                   
                    GetReturnFilings = Comon.cDec(Lip.GetValue(@"SELECT sum(dbo.Mnu_ReturnFilingsDetails.QTY) as QTY FROM  dbo.Mnu_ReturnFilingsDetails INNER JOIN
                         dbo.Mnu_ReturnFilingsMaster ON dbo.Mnu_ReturnFilingsDetails.CommandID = Mnu_ReturnFilingsMaster.CommandID and dbo.Mnu_ReturnFilingsDetails.BranchID = Mnu_ReturnFilingsMaster.BranchID WHERE " + filter12 + " and Mnu_ReturnFilingsMaster.StoreIDBefore=" + dt.Rows[i]["EmpFactorID"] + " and  (dbo.Mnu_ReturnFilingsMaster.BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + ")"));
                    dt.Rows[i]["AllReturnFilings"] = GetReturnFilings;
                    dt.Rows[i]["AllLost"] = Comon.ConvertToDecimalPrice(Comon.cDec(dt.Rows[i]["AllLost"]) - Comon.cDec(dt.Rows[i]["AllReturnFilings"]));
              
                }
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
                strSQL = "SELECT " + PrimaryName + " as AccountName FROM Acc_Accounts WHERE AccountID =" + Comon.cDbl(txtAccountIDFactory.Text) + " And Cancel =0 and BranchID="+Comon.cInt(cmbBranchesID.EditValue);
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
                    strSQL = "SELECT " + PrimaryName + " as CustomerName  FROM Sales_CustomerAnSublierListArb Where  AcountID =" + txtCustomerID.Text + " and BranchID=" + Comon.cInt(cmbBranchesID.EditValue);
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
        private void frmSummaryLost_KeyDown(object sender, KeyEventArgs e)
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

        }





    }
}
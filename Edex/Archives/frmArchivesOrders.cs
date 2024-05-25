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
using Edex.Model;
using Edex.Model.Language;
using Edex.GeneralObjects.GeneralClasses;
using Edex.ModelSystem;
using DevExpress.XtraGrid.Views.Grid;
using Edex.StockObjects.Reports;

namespace Edex.Archives
{
     
    public partial class frmArchivesOrders : BaseForm
    {
        #region Declare
        private string strSQL = "";
        private string where = "";
        private string lang = "";
        private string FocusedControl = "";
        private string PrimaryName;
        DataTable dtFactoryOprationType = new DataTable();
        public DataTable _sampleData = new DataTable();
        DataTable dt = new DataTable();
        #endregion
        public frmArchivesOrders()
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
            ribbonControl1.Pages[0].Groups[0].ItemLinks[12].Visible = true;
            InitializeFormatDate(txtFromDate);
            InitializeFormatDate(txtToDate);
            gridView1.OptionsBehavior.ReadOnly = true;
            gridView1.OptionsBehavior.Editable = false;
            PrimaryName = "ArbName";
            if (UserInfo.Language == iLanguage.English)
            {
                PrimaryName = "EngName";
            }
            ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Visible = true;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Caption = (UserInfo.Language == iLanguage.Arabic ? "استعلام جديد" : "New Query");
            FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
            FillCombo.FillComboBoxLookUpEdit(cmbFromType, "Arc_ArchivesType", "ID", PrimaryName, "", "ID>14 and ID<=18", (UserInfo.Language == iLanguage.English ? "Select Type" : "حدد النوع "));
            FillCombo.FillComboBoxLookUpEdit(cmbToType, "Arc_ArchivesType", "ID", PrimaryName, "", "ID>14 and ID<=18", (UserInfo.Language == iLanguage.English ? "Select Type" : "حدد النوع "));
            cmbBranchesID.EditValue = MySession.GlobalBranchID;
            cmbBranchesID.ReadOnly = !MySession.GlobalAllowBranchModificationAllScreens;
            FillCombo.FillComboBoxLookUpEdit(cmbStatus, "Manu_TypeStatus", "ID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select StatUs" : "حدد الحالة "));
            FillCombo.FillComboBox(cmbCurency, "Acc_Currency", "ID", PrimaryName, "", "BranchID=" +MySession.GlobalBranchID, (UserInfo.Language == iLanguage.English ? "Select " : "حدد العملة"));
            this.KeyDown += frmArchivesOrders_KeyDown;
            this.txtCostCenterID.Validating += txtCostCenterID_Validating;
            this.txtDelegeteID.Validating += txtDelegeteID_Validating;
             this.Load += frmArchivesOrders_Load;

            this.txtUserIDEntry.Validating += txtUserIDEntry_Validating;
 
            this.gridView1.RowCellStyle += gridView1_RowCellStyle;
            this.gridView1.DoubleClick += gridView1_DoubleClick;
        }

        void gridView1_DoubleClick(object sender, EventArgs e)
        { 

            try
            {
                GridView view = sender as GridView;
                string OrderID = view.GetFocusedRowCellValue("ID").ToString();
                if(OrderID!="")
                {
                    frmOrdersReportBeforCasting frm = new frmOrdersReportBeforCasting();
                   
                    frm.Show();
                    frm.ShowOrderFormArchive(OrderID);
                }
            }
            catch (Exception ex)
            {

            }
        
        }

        void gridView1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            if (e.Column.FieldName == "StatUs")
            {
                if (gridView1.GetRowCellValue(e.RowHandle, "StatUs").ToString() == (UserInfo.Language == iLanguage.Arabic ? "محذوف" : "Deleted"))
                {
                    e.Appearance.BackColor = Color.Red;
                }
                else
                {
                    e.Appearance.BackColor = e.Appearance.BackColor;
                }
            }
        }

        void txtUserIDEntry_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as UserName FROM Users WHERE UserID =" + Comon.cInt(txtUserIDEntry.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                CSearch.ControlValidating(txtUserIDEntry, lblUserNameEntry, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
         }
        void frmArchivesOrders_Load(object sender, EventArgs e)
        {
            _sampleData.Columns.Add(new DataColumn("Sn", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("RecordType", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("ID", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("RefranceID", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("InvoiceDate", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("CurrncyName", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("NetAmmount", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("IncomeOrder", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("CustomerName", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("DelegateName", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("GuidanceName", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("StatUs", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("ProfitOrder", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("TotalQTYOrder", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("CostCenterName", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("TypeOpration", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Notes", typeof(string)));
        }

        void frmArchivesOrders_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
                Find();
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
      
        public void Find()
        {

            CSearch cls = new CSearch();
            int[] ColumnWidth = new int[] { 100, 300 };
            string SearchSql = "";
            string Condition = "Where 1=1";

            FocusedControl = GetIndexFocusedControl();

            if (FocusedControl == null) return;

            else if (FocusedControl.Trim() == txtUserIDEntry.Name)
            {

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtUserIDEntry, lblUserNameEntry, "UserID", "رقم المستخدم", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtUserIDEntry, lblUserNameEntry, "UserID", "User ID", MySession.GlobalBranchID);
            }
           
 

            else if (FocusedControl.Trim() == txtDelegeteID.Name)
            {
                if (!MySession.GlobalAllowChangefrmPurchaseDelegateID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtDelegeteID, lblDelegeteName, "SaleDelegateID", "رقم مندوب المبيعات", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtDelegeteID, lblDelegeteName, "SaleDelegateID", "Delegate ID", MySession.GlobalBranchID);
            }

            else if (FocusedControl.Trim() == txtCostCenterID.Name)
            {
                if (!MySession.GlobalAllowChangefrmPurchaseCostCenterID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtCostCenterID, lblCostCenterName, "CostCenterID", "رقم مركز التكلفة", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtCostCenterID, lblCostCenterName, "CostCenterID", "Cost Center ID", MySession.GlobalBranchID);
            }

            GetSelectedSearchValue(cls);
        }



        private void txtCostCenterID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as CostCenterName FROM Acc_CostCenters WHERE CostCenterID =" + Comon.cInt(txtCostCenterID.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                CSearch.ControlValidating(txtCostCenterID, lblCostCenterName, strSQL);
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

               
                if (FocusedControl == txtUserIDEntry.Name)
                {
                    txtUserIDEntry.Text = cls.PrimaryKeyValue.ToString();
                    txtUserIDEntry_Validating(null, null);
                }
                 
                else if (FocusedControl == txtCostCenterID.Name)
                {
                    txtCostCenterID.Text = cls.PrimaryKeyValue.ToString();
                    txtCostCenterID_Validating(null, null);
                }
                else if (FocusedControl == txtDelegeteID.Name)
                {
                    txtDelegeteID.Text = cls.PrimaryKeyValue.ToString();
                    txtDelegeteID_Validating(null, null);
                }
            }

        }
        private void txtDelegeteID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as DelegateName FROM Sales_SalesDelegate WHERE DelegateID =" + txtDelegeteID.Text + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                CSearch.ControlValidating(txtDelegeteID, lblDelegeteName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
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
                _sampleData.Clear();
                gridControl1.RefreshDataSource();
                txtToTransactionsID.Text = "";
                txtFromTransactionID.Text = "";
                txtDelegeteID.Text = "";
                txtDelegeteID_Validating(null, null);
              
                txtCostCenterID.Text = "";
                txtCostCenterID_Validating(null, null);
                txtReferanceID.Text = "";
                txtUserIDEntry.Text = "";
                
                txtFromDate.Text = "";
                txtToDate.Text = "";
                txtCostCenterID.Text = "";
                lblCostCenterName.Text = "";

                txtFromDate.Enabled = true;
                txtToDate.Enabled = true;
                txtCostCenterID.Enabled = true;

                txtCostCenterID.Enabled = true;
                txtFromDate.Enabled = true;
                txtToDate.Enabled = true;
                txtToTransactionsID.Enabled = true;
                txtFromTransactionID.Enabled = true;

                gridControl1.DataSource = _sampleData;

            }
            catch (Exception ex)
            {
                //WT.msgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        string GetStrSQLReceiptVoucher()
        {

            Dictionary<int, int> ToOrder = new Dictionary<int, int>();
            ToOrder.Add(15, 1);
            ToOrder.Add(16, 2);
            ToOrder.Add(17, 3);
            ToOrder.Add(18, 4);
            //btnShowOrders.Visible = false;
            Application.DoEvents();

            string filter = "(dbo.Manu_OrderRestriction.BranchID = " +  MySession.GlobalBranchID + ") AND dbo.Manu_OrderRestriction.OrderID >0    AND ";
            strSQL = "";
            if (Comon.cInt(cmbBranchesID.EditValue) > 0)
                filter = "(dbo.Manu_OrderRestriction.BranchID = " + cmbBranchesID.EditValue + ") AND dbo.Manu_OrderRestriction.OrderID >0     AND ";

            long FromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text));
            long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));

            if(Comon.cInt(cmbFromType.EditValue)>0)
                     filter = filter + " dbo.Manu_OrderRestriction.TypeID >=" + ToOrder[Comon.cInt(cmbFromType.EditValue)] + " AND ";
            if (Comon.cInt(cmbToType.EditValue) > 0)
                filter = filter + " dbo.Manu_OrderRestriction.TypeID <=" + ToOrder[Comon.cInt(cmbToType.EditValue)] + " AND ";

            DataTable dt;
            // حسب الرقم
            if (Comon.cInt(cmbStatus.EditValue) == 1)
                filter = filter + " dbo.Manu_OrderRestriction.Cancel =1 AND ";

            if (txtFromTransactionID.Text != string.Empty)
                filter = filter + " dbo.Manu_OrderRestriction.OrderID >=" + txtFromTransactionID.Text + " AND ";

            else if (Comon.cInt(cmbCurency.EditValue) > 0)
                filter = filter + " dbo.Menu_ProductionExpensesMaster.CurencyID=" + Comon.cInt(cmbCurency.EditValue) + " AND ";
            if (Comon.cInt(txtUserIDEntry.Text) > 0)
                filter = filter + " Acc_ReceiptVoucherMaster.UserID=" + Comon.cInt(txtUserIDEntry.Text) + " AND ";
             
            if (txtToTransactionsID.Text != string.Empty)
                filter = filter + " dbo.Manu_OrderRestriction.OrderID <=" + txtToTransactionsID.Text + " AND ";
            //if (txtReferanceID.Text.Trim() != string.Empty)
            //    filter = filter + "  Acc_ReceiptVoucherMaster.DocumentID=" + txtReferanceID.Text + " AND ";
            
            // حسب التاريخ
            if (FromDate != 0)
                filter = filter + " dbo.Manu_OrderRestriction.OrderDate >=" + FromDate + " AND ";
            if (ToDate != 0)
                filter = filter + " dbo.Manu_OrderRestriction.OrderDate <=" + ToDate + " AND ";

            if (txtCostCenterID.Text != string.Empty)
                filter = filter + " dbo.Menu_ProductionExpensesMaster.CostCenterID  =" + Comon.cInt(txtCostCenterID.Text) + "  AND ";
            // '''''''''''''
            string ToAccount = (UserInfo.Language == iLanguage.Arabic ? "الى مذكورين" : "To those mentioned");
            filter = filter.Remove(filter.Length - 4, 4);
            strSQL = "SELECT Manu_OrderRestriction.Cancel as Cancel,dbo.Acc_Currency." + PrimaryName + " as CurrncyName,   Manu_OrderRestriction.OrderID, Manu_OrderRestriction.OrderDate,Manu_OrderRestriction.TypeOrdersID,Menu_ProductionExpensesMaster.QTYGram , Menu_ProductionExpensesMaster.QTYOrder,Menu_ProductionExpensesMaster.SalesPriceQram, "
                   +"  Manu_OrderRestriction.TypeAuxiliaryMatirialID,Manu_OrderRestriction.TypeID, "
                   + " Manu_TypeOrders." + PrimaryName + " AS TypeOrdersName, Sales_SalesDelegate." + PrimaryName + " AS DelegateName, Sales_Customers." + PrimaryName + " AS CustomerName, HR_EmployeeFile." + PrimaryName + " AS GuidanceName  "      
                   +" FROM   Manu_OrderRestriction " 
                   +" LEFT OUTER JOIN HR_EmployeeFile ON Manu_OrderRestriction.GuidanceID = HR_EmployeeFile.EmployeeID AND Manu_OrderRestriction.BranchID = HR_EmployeeFile.BranchID "
                   +"  LEFT OUTER JOIN  Sales_Customers ON Manu_OrderRestriction.CustomerID = Sales_Customers.AccountID AND Manu_OrderRestriction.BranchID = Sales_Customers.BranchID LEFT OUTER JOIN     Sales_SalesDelegate ON Manu_OrderRestriction.DelegateID = Sales_SalesDelegate.DelegateID AND Manu_OrderRestriction.BranchID = Sales_SalesDelegate.BranchID "
                   +"  LEFT OUTER JOIN Manu_TypeOrders ON Manu_OrderRestriction.TypeOrdersID = Manu_TypeOrders.ID "
                   + " left outer join Menu_ProductionExpensesMaster  on Manu_OrderRestriction.OrderID= Menu_ProductionExpensesMaster.OrderID and Manu_OrderRestriction.BranchID= Menu_ProductionExpensesMaster.BranchID and Menu_ProductionExpensesMaster.Cancel=0 "
                   + "  LEFT OUTER JOIN Acc_Currency on Menu_ProductionExpensesMaster.CurencyID=Acc_Currency.ID  and Menu_ProductionExpensesMaster.BranchID=Acc_Currency.BranchID Where" + filter;
                 
            
            Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());
            return strSQL;


        }
       
      
        private void PurchaseInvoice()
        {
            try
            {
                decimal netSum = 0;
                decimal netCashSum = 0;
                decimal caschPaidWithNet = 0;
                decimal cash = 0;
                decimal future = 0;
                decimal check1 = 0;
                decimal total = 0;
                DataRow row;
                dt.Clear();
                _sampleData.Clear();

                int fromType = Comon.cInt(cmbFromType.EditValue);
                int toType = Comon.cInt(cmbToType.EditValue);

                //if (fromType == 11 || (toType >= 11 && fromType <= 11) || (toType == fromType && fromType <= 0))
                {
                    dt = Lip.SelectRecord(GetStrSQLReceiptVoucher());
                }

                //if ((toType == fromType && fromType <= 0) ||
                //     ((fromType >= 11 && fromType <= 12 && toType >= 12 && (toType != fromType && toType >= 12)) ||
                //     (fromType >= 11 && fromType <= 12 && toType <= 0) ||
                //     (toType >= 12 && fromType <= 0)) ||
                //     (toType == fromType && toType == 12))
                //{
                //    dt.Merge(Lip.SelectRecord(GetStrSQLSpendVoucher()));
                //}
                //if ((toType == fromType && fromType <= 0) ||
                //     ((fromType >= 11 && toType >= 13 && (toType != fromType && toType >= 13)) ||
                //     (fromType >= 11 && fromType <= 13 && toType <= 0) || (toType >= 13 && fromType <= 0)) ||
                //     (toType == fromType && toType == 13))
                //{
                //    dt.Merge(Lip.SelectRecord(GetStrSQLVariousVoucher()));
                //}
                //if ((toType == fromType && fromType <= 0) ||
                //    ((fromType >= 11 && toType >= 14 && (toType != fromType && toType >= 14)) ||
                //    (fromType >= 11 && fromType <= 14 && toType <= 0) || (toType >= 14 && fromType <= 0)) ||
                //    (toType == fromType && toType == 14))
                //{
                //    dt.Merge(Lip.SelectRecord(GetStrSQLOpeningVoucher()));
                //}
                 

                if (strSQL != null || strSQL != "")
                {
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            row = _sampleData.NewRow();
                            row["Sn"] = _sampleData.Rows.Count + 1;
                            row["ID"] = dt.Rows[i]["OrderID"].ToString();
                            //row["RefranceID"] = dt.Rows[i]["DocumentID"].ToString(); 
                            row["InvoiceDate"] = Comon.ConvertSerialDateTo(dt.Rows[i]["OrderDate"].ToString());
                            decimal OrderCost = Comon.cDec(Comon.cDec(dt.Rows[i]["QTYGram"]) * Comon.cDec(dt.Rows[i]["QTYOrder"]));
                            decimal OrderSales = Comon.cDec(Comon.cDec(dt.Rows[i]["SalesPriceQram"]) * Comon.cDec(dt.Rows[i]["QTYOrder"]));
                            row["TotalQTYOrder"] = Comon.cDec(dt.Rows[i]["QTYOrder"]);
                            row["NetAmmount"] = Comon.ConvertToDecimalPrice(OrderCost);
                            row["IncomeOrder"] = Comon.ConvertToDecimalPrice(OrderSales);
                            row["ProfitOrder"] = Comon.cDec(Comon.cDec(row["IncomeOrder"]) - Comon.cDec(row["NetAmmount"]));
                            row["CurrncyName"] = (dt.Rows[i]["CurrncyName"].ToString() != string.Empty ? dt.Rows[i]["CurrncyName"] : "");
                            if (Comon.cInt(dt.Rows[i]["Cancel"]) == 1)
                                row["StatUs"] = UserInfo.Language == iLanguage.Arabic ? "محذوف" : "Deleted";
                            else
                                row["StatUs"] = UserInfo.Language == iLanguage.Arabic ? "مرحل " : "Aported";
                            if (Comon.cInt(dt.Rows[i]["TypeID"]) == 1)
                                row["RecordType"] = UserInfo.Language == iLanguage.Arabic ? "زركون " : "Zircon";
                            else if (Comon.cInt(dt.Rows[i]["TypeID"]) == 2)
                                row["RecordType"] = UserInfo.Language == iLanguage.Arabic ? "الماس" : "Diamond";
                            else if (Comon.cInt(dt.Rows[i]["TypeID"]) == 3)
                                row["RecordType"] = UserInfo.Language == iLanguage.Arabic ? "صافي" : "Net";
                            else if (Comon.cInt(dt.Rows[i]["TypeID"]) ==4)
                                row["RecordType"] = UserInfo.Language == iLanguage.Arabic ? "صيانة" : "Maintenance";
                            row["CustomerName"] = (dt.Rows[i]["CustomerName"].ToString() != string.Empty ? dt.Rows[i]["CustomerName"] : "");
                            row["DelegateName"] = (dt.Rows[i]["DelegateName"].ToString() != string.Empty ? dt.Rows[i]["DelegateName"] : "");
                            row["GuidanceName"] = (dt.Rows[i]["GuidanceName"].ToString() != string.Empty ? dt.Rows[i]["GuidanceName"] : "");
                        
                            _sampleData.Rows.Add(row);
                        }
                    }
                }
            }
            catch { }
        }
        private void btnShow_Click(object sender, EventArgs e)
        {
            PurchaseInvoice();
            gridControl1.DataSource = _sampleData;
            if (gridView1.RowCount > 0)
            {
                //btnShowOrders.Visible = true;

                //txtCostCenterID.Enabled = false;
                //txtFromDate.Enabled = false;
                //txtToDate.Enabled = false;
                //txtToTransactionsID.Enabled = false;
                //txtFromTransactionID.Enabled = false;

            }
            else
            {

                Messages.MsgInfo(Messages.TitleInfo, MySession.GlobalLanguageName == iLanguage.Arabic ? "لايوجد بيانات لعرضها" : "There is no Data to show it");
                //btnShowOrders.Visible = true;
                //  DoNew();
            }
        }
    }
}
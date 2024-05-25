using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraReports.UI;
using DevExpress.XtraSplashScreen;
using Edex.DAL;
using Edex.DAL.Stc_itemDAL;
using Edex.Model;
using Edex.Model.Language;
using Edex.AccountsObjects.Reports;
using Edex.GeneralObjects.GeneralClasses;
using Edex.GeneralObjects.GeneralForms;
using Edex.ModelSystem;
using Edex.Reports;
using Edex.StockObjects.Codes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Edex.DAL.ManufacturingDAL;
namespace Edex.StockObjects.Reports
{
    public partial class frmOrdersReportBeforCasting : Edex.GeneralObjects.GeneralForms.BaseForm
    {
        private string strSQL = "";
        private string filter = "";
        DataTable dt = new DataTable();
        DataTable _nativeData = new DataTable();
        public DataTable _sampleData = new DataTable();
        public DataTable _sampleDataAfter = new DataTable();
        public DataTable _sampleDataOrderCost = new DataTable();
        string FocusedControl;
        private string PrimaryName;
        private string ItemName;
        private string SizeName;
        private string GroupName;
        public frmOrdersReportBeforCasting()
        {
            try
            {
                InitializeComponent();
                ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Visible = true;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Caption = (UserInfo.Language == iLanguage.Arabic ? "استعلام جديد" : "New Query");
                ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[2].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[3].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[4].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[5].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[7].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[8].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[9].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[11].Visible = false;
                InitializeFormatDate(txtFromDate);
                InitializeFormatDate(txtToDate);
                strSQL = "EngName";

                if (UserInfo.Language == iLanguage.Arabic)
                {
                    strSQL = "ArbName";
                    PrimaryName = "ArbName";
                }
                else 
                {
                    strSQL = "EngName";
                    PrimaryName = "EngName";

                    // Caption 
                    gridBand1.Caption = "Order Detils";
                    gridBand2.Caption = "Order Detils Before Casting ";

                    dgvColIOrderID.Caption ="Order ID";
                    dgvCollOrderDate.Caption = "Date Order";
                    dgvColICustomerName.Caption = "Customer Name";
                    dvgColGuidance.Caption = "Guidance";
                    dvgColTypeOrder.Caption = "Order Type";
                    dgvColCad.Caption = "Cad QTY";
                    dgvColZircon.Caption = "Zircon";
                    dvgColBagit.Caption = "Bagit";
                    dvgColRounde.Caption = "Round";
                    dvgColWaxPillar.Caption = "Wax Pillar";
                    dvgColMAtirial.Caption = "Matirial QTY";
                    dvgColMAtirialEquivalent.Caption = "Matirial Equivalent";
                    dvgColEstimatedGold.Caption = "Estimated Gold";
                    dgvColPackingQty.Caption = "Normal Wax ";
                    dvgColDelegete.Caption = "Delegete Name";

                    gridBand3.Caption = "Order Detils";
                    gridBand4.Caption = "Order Detils After Casting";

                    dvgColCustomerNameAfter.Caption = "Customer Name";
                    dvgColOrderIDAfter.Caption = "Order ID";
                    dvgColDateOrder.Caption = "Order Date";
                    dvgColGuidanceAfter.Caption = "Guidance";
                    dvgColTypeAfter.Caption = "Type Order";
                    dvgColOrderGold18.Caption = "Gold cloves 18";
                    dvgColBeforeManuFactory.Caption = "Before Weight ManuFactory";
                    dvgColZirconAfter.Caption = "Zircone";
                    dvgColBagitAfter.Caption = "Bagit";
                    dvgColRoundAfter.Caption = "Round QTY";
                    dvgColGoldChain.Caption = "Gold Chain";
                    dvgColOrderWeightReady.Caption = "Order Weight Ready";
                    dvgColOrderLost.Caption = "Order Lost";
                    dvgColTotalOrderWeight.Caption = "Total Order Weight";
                    dvgColDelegeteAfter.Caption = "Delegete Name";

                    dvgColSnAfter.Caption = dvgColSnCost.Caption = dvgColSn.Caption = "Sn";

                    gridBand5.Caption = "Order Detils";
                    gridBand6.Caption = "Orders Cost";

                    dvgColCustmerNameCost.Caption = "Customer Name";
                    dvgColDateOrderCost.Caption = "Date Order";
                    dvgColDelegeteCost.Caption = "Delegete Name";
                    dvgColOrderIDCost.Caption = "Order ID";
                    dvgColTotalLostCost.Caption = "Total Lost";
                    dvgColOrderWeightReadyCost.Caption = "Order Weight Ready";
                    dvgColOrderProfit.Caption = "Profit";
                    dvgColGuidanceCost.Caption = "Guidance";
                    dvgColOrderTotalOrderWeightCost.Caption = "Total Order Weight";
                    dvgColOrderIncome.Caption = "Order Income";
                    dvgColOrderCost.Caption = "Order Cost";
                    dvgColTypeOrderCost.Caption = "Type Order";


                }


                 FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
                 cmbBranchesID.EditValue = MySession.GlobalBranchID;
                cmbBranchesID.ReadOnly = !MySession.GlobalAllowBranchModificationAllScreens;
                FillCombo.FillComboBox(cmbTypeID, "Stc_ItemTypes", "TypeID", strSQL, "", "BranchID=" + cmbBranchesID.EditValue);
                this.txtToDate.Properties.Mask.UseMaskAsDisplayFormat = true;
                this.txtToDate.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
                this.txtToDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                this.txtToDate.Properties.EditFormat.FormatString = "dd/MM/yyyy";
                this.txtToDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                this.txtToDate.Properties.Mask.EditMask = "dd/MM/yyyy";

                this.txtDelegeteID.Validating += new System.ComponentModel.CancelEventHandler(this.txtStoreID_Validating);
                this.txtUserID.Validating += new System.ComponentModel.CancelEventHandler(this.txtCostCenterID_Validating);
                this.txtGroupID.Validating += new System.ComponentModel.CancelEventHandler(this.txtGroupID_Validating);
                 

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
        public void Find()
        {
            try{
            CSearch cls = new CSearch();
            int[] ColumnWidth = new int[] { 100, 300 };
            string SearchSql = "";
            string Condition = "Where 1=1";

            FocusedControl = GetIndexFocusedControl();
            if (FocusedControl.Trim() == txtDelegeteID.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtDelegeteID, lblDelegeteName, "SaleDelegateID", "رقم مندوب المبيعات", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtDelegeteID, lblDelegeteName, "SaleDelegateID", "Delegate ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == txtUserID.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtUserID, lblUserName, "UserID", "رقم المستخدم", MySession.GlobalBranchID); 
                else
                    PrepareSearchQuery.Find(ref cls, txtUserID, lblUserName, "UserID", "User ID", MySession.GlobalBranchID); 
            }
                else if (FocusedControl.Trim() == txtBarCode.Name)
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Search(txtBarCode, lblBarCodeName, "BarCodeForPurchaseInvoice", "اسـم الـمـادة", "البـاركـود");
                    else
                        PrepareSearchQuery.Search(txtBarCode, lblBarCodeName, "BarCodeForPurchaseInvoice", "Item Name", "BarCode");
                }
                else if (FocusedControl.Trim() == txtGroupID.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Search(txtGroupID, lblGroupID, "GroupID", "اسـم المجـمـوعة", "رقـم المجـمـوعة");
                else
                    PrepareSearchQuery.Search(txtGroupID, lblGroupID, "GroupID", "Group Name", "Group ID");
            }


            }
            catch { }



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
        private void txtGroupID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT   " + (UserInfo.Language == iLanguage.Arabic ? "ArbName" : "EngName") + "   as GroupName FROM dbo.Stc_ItemsGroups WHERE GroupID =" + Comon.cInt(txtGroupID.Text) + " And Cancel =0 And  BranchID =" +Comon.cInt(cmbBranchesID.EditValue);
                CSearch.ControlValidating(txtGroupID, lblGroupID, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

        }

        public void frmOrdersReportBeforCasting_Load(object sender, EventArgs e)
        {
            DataTable dtQty = new DataTable();
            dtQty.Columns.Add("ID", System.Type.GetType("System.Int16"));
            dtQty.Columns.Add("Name", System.Type.GetType("System.String"));
            dtQty.Rows.Add("0", (UserInfo.Language == iLanguage.Arabic ? "جميع الطلبيات" : "All Qty Balance"));
            dtQty.Rows.Add("1", (UserInfo.Language == iLanguage.Arabic ? "الطلبيات المرحلة" : "Qty Balance > 0"));
            dtQty.Rows.Add("2", (UserInfo.Language == iLanguage.Arabic ? "الطلبيات الغير مرحله" : "Qty Balance = 0"));

            cmbQtyBalance.Properties.DataSource = dtQty.DefaultView;
            cmbQtyBalance.Properties.DisplayMember = "Name";
            cmbQtyBalance.Properties.ValueMember = "ID";

             DataTable dtPrice = new DataTable();
            dtPrice.Columns.Add("ID", System.Type.GetType("System.String"));
            dtPrice.Columns.Add("Name", System.Type.GetType("System.String"));

            dtPrice.Rows.Add("Zircone", (UserInfo.Language == iLanguage.Arabic ? "زركون" : "Zircone"));
            dtPrice.Rows.Add("Daimond", (UserInfo.Language == iLanguage.Arabic ? "الماس" : "Diamond"));
            cmbTypeOrder.Properties.DataSource = dtPrice.DefaultView;
            cmbTypeOrder.Properties.DisplayMember = "Name";
            cmbTypeOrder.Properties.ValueMember = "ID";
            cmbTypeOrder.ItemIndex = 0;

            InitalGrid(_sampleData);
            InitalGrid(_sampleDataAfter);
            InitalGrid(_sampleDataOrderCost);

        }

        private void InitalGrid(DataTable dt)
        {

            dt.Columns.Add(new DataColumn("Sn", typeof(string)));
            dt.Columns.Add(new DataColumn("OrderID", typeof(string)));
            dt.Columns.Add(new DataColumn("OrderDate", typeof(string)));

            dt.Columns.Add(new DataColumn("CustomerName", typeof(string)));
            dt.Columns.Add(new DataColumn("DelegateName", typeof(string)));
            dt.Columns.Add(new DataColumn("GuidanceName", typeof(string)));
            dt.Columns.Add(new DataColumn("TypeOrdersName", typeof(string)));

            //قبل الصب
            dt.Columns.Add(new DataColumn("CadQTY", typeof(string)));
            dt.Columns.Add(new DataColumn("NormalQTY", typeof(string)));
            dt.Columns.Add(new DataColumn("ZirconeQTY", typeof(string)));
            dt.Columns.Add(new DataColumn("BagateQTY", typeof(string)));
            dt.Columns.Add(new DataColumn("RountDaimondQTY", typeof(string)));
            dt.Columns.Add(new DataColumn("WaxPillar", typeof(string)));
            dt.Columns.Add(new DataColumn("MaterialWeight", typeof(string)));
            dt.Columns.Add(new DataColumn("MaterialEquivalent", typeof(string)));
            dt.Columns.Add(new DataColumn("EstimatedWeight", typeof(string)));

            //بعد الصب
            dt.Columns.Add(new DataColumn("OrderGold18", typeof(string)));
            dt.Columns.Add(new DataColumn("WeightBeforeManufacturing", typeof(string)));
            dt.Columns.Add(new DataColumn("BagateAfterCompondQTY", typeof(string)));
            dt.Columns.Add(new DataColumn("RountDaimondAfterCompondQTY", typeof(string)));
            dt.Columns.Add(new DataColumn("GoldChain", typeof(string)));
            dt.Columns.Add(new DataColumn("OrderWeightReady", typeof(string)));
            dt.Columns.Add(new DataColumn("TotalOrderLost", typeof(string)));
            dt.Columns.Add(new DataColumn("TotalOrderWeight", typeof(string)));
            //التكاليف
            dt.Columns.Add(new DataColumn("OrderCost", typeof(string)));
            dt.Columns.Add(new DataColumn("OrderIncome", typeof(string)));
            dt.Columns.Add(new DataColumn("OrderProfit", typeof(string)));



        }

        private void InitalGridAfter()
        {
  

        }
        private void txtStoreID_Validating(object sender, CancelEventArgs e)
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
        private void txtCostCenterID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT   " + (UserInfo.Language == iLanguage.Arabic ? "ArbName" : "EngName") + "  as CostCenterName FROM Acc_CostCenters WHERE CostCenterID =" + Comon.cInt(txtUserID.Text) + " And Cancel =0 And  BranchID =" +   Comon.cInt(cmbBranchesID.EditValue);
                CSearch.ControlValidating(txtUserID, lblUserName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        public void ShowOrderFormArchive(string OrderID)
        {
            DoAddFrom();
            txtFromItemNo.Text = OrderID;
            txtToItemNo.Text = OrderID;
            txtFromDate.Text = "";
            txtToDate.Text = "";
            btnShow_Click(null, null);
        }
        private void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow row;
                btnShow.Visible = false;
                lblStockValu.Text = "0";
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                Application.DoEvents();
                _sampleData.Clear();
                filter = " Manu_OrderRestriction.Cancel = 0 And ";
                if(cmbBranchesID.EditValue!=string.Empty)
                    filter += " Manu_OrderRestriction.BranchID ="+Comon.cInt( cmbBranchesID.EditValue)+" And ";
                if (txtFromItemNo.Text != string.Empty)
                    filter += " OrderID >=" + txtFromItemNo.Text + " AND ";

                if (txtToItemNo.Text != string.Empty)
                    filter = filter + " OrderID <=" + txtToItemNo.Text + " AND ";
                long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));
                long FromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text));
                if (FromDate != 0)
                    filter = filter + " dbo.Manu_OrderRestriction.OrderDate>=" + FromDate + " AND ";

                if (ToDate != 0)
                    filter = filter + " dbo.Manu_OrderRestriction.OrderDate<=" + ToDate + " AND ";
                 
                filter = filter.Remove(filter.Length - 4, 4);

                //قبل الصب
                string str = @"SELECT  Manu_OrderRestriction.OrderID, Manu_OrderRestriction.OrderDate, Manu_OrderRestriction.CustomerID, Manu_OrderRestriction.DelegateID, Manu_OrderRestriction.GuidanceID, Manu_OrderRestriction.TypeOrdersID, 
                         Manu_OrderRestriction.TypeAuxiliaryMatirialID, Manu_OrderRestriction.TypeID, 
                         0 AS CadQTY,0 AS ZirconeQTY , 0 AS NormalQTY, 0  AS  BagateQTY , 0 AS RountDaimondQTY ,0 AS WaxPillar,0 AS MaterialWeight , 0  AS MaterialEquivalent , 0 AS EstimatedWeight , 0 AS OrderGold18,0 AS WeightBeforeManufacturing , 0 AS BagateAfterCompondQTY, 0  AS  GoldChain , 0 AS OrderWeightReady ,0 AS TotalOrderLost,0 AS BagateAfterCompondQTY , 0  AS TotalOrderWeight , 0 AS OrderCost, 0 AS OrderIncome, 0 AS OrderProfit, 
                         Manu_TypeOrders.ArbName AS TypeOrdersName, 
                         Sales_SalesDelegate.ArbName AS DelegateName, Sales_Customers.ArbName AS CustomerName, HR_EmployeeFile.ArbName AS GuidanceName
                         FROM   Manu_OrderRestriction LEFT OUTER JOIN
                         HR_EmployeeFile ON Manu_OrderRestriction.GuidanceID = HR_EmployeeFile.EmployeeID AND Manu_OrderRestriction.BranchID = HR_EmployeeFile.BranchID LEFT OUTER JOIN
                         Sales_Customers ON Manu_OrderRestriction.CustomerID = Sales_Customers.AccountID AND Manu_OrderRestriction.BranchID = Sales_Customers.BranchID LEFT OUTER JOIN
                         Sales_SalesDelegate ON Manu_OrderRestriction.DelegateID = Sales_SalesDelegate.DelegateID AND Manu_OrderRestriction.BranchID = Sales_SalesDelegate.BranchID LEFT OUTER JOIN
                         Manu_TypeOrders ON Manu_OrderRestriction.TypeOrdersID = Manu_TypeOrders.ID where " + filter;

                dt = Lip.SelectRecord(str);

                if (dt.Rows.Count < 1)
                    return;
                
                _sampleData.Clear();

                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    row = _sampleData.NewRow();
                    row["Sn"] = _sampleData.Rows.Count + 1;
                    row["OrderID"] = dt.Rows[i]["OrderID"].ToString();
                    row["OrderDate"] = Comon.ConvertSerialDateTo(dt.Rows[i]["OrderDate"].ToString());
                    row["CustomerName"] = dt.Rows[i]["CustomerName"].ToString();
                    row["DelegateName"] = dt.Rows[i]["DelegateName"].ToString();
                    row["GuidanceName"] = dt.Rows[i]["GuidanceName"].ToString();
                    row["TypeOrdersName"] = dt.Rows[i]["TypeOrdersName"].ToString();
                    row["CadQTY"] = 0;
                    row["NormalQTY"] = 0;
                    row["ZirconeQTY"] = 0;
                    row["BagateQTY"] = 0;
                    row["RountDaimondQTY"] = 0;
                    row["WaxPillar"] = 0;

                    DataTable dtOrderDetail = GetOrderDetail(dt.Rows[i]["OrderID"].ToString(),6);
                    if(dtOrderDetail.Rows.Count>0)
                    {

                        var CadQTY = dtOrderDetail.Compute("SUM(QTY)", "BaseID = 7");
                        var NormalQTY = dtOrderDetail.Compute("SUM(QTY)", "BaseID = 6");
                        var ZirconeQTY = dtOrderDetail.Compute("SUM(QTY)", "BaseID = 4");
                        var BagateQTY = dtOrderDetail.Compute("SUM(QTY)", "BaseID = 2");
                        var RountDaimondQTY = dtOrderDetail.Compute("SUM(QTY)", "BaseID = 3");
                        var WaxPillar = dtOrderDetail.Compute("SUM(QTY)", "BaseID = 12");

                        row["CadQTY"] = row["CadQTY"] == DBNull.Value ? "0" : CadQTY.ToString();
                        row["NormalQTY"] = row["NormalQTY"] == DBNull.Value ? "0" : NormalQTY.ToString();
                        row["ZirconeQTY"] = row["ZirconeQTY"] == DBNull.Value ? "0" : ZirconeQTY.ToString();
                        row["BagateQTY"] = row["BagateQTY"] == DBNull.Value ? "0" : BagateQTY.ToString();
                        row["RountDaimondQTY"] = row["RountDaimondQTY"] == DBNull.Value ? "0" : RountDaimondQTY.ToString();
                        row["WaxPillar"] = row["WaxPillar"] == DBNull.Value ? "0" : WaxPillar.ToString();
                    }
                    decimal MaterialWeight=Comon.cDec( Comon.cDec(row["CadQTY"])+Comon.cDec( row["NormalQTY"])+Comon.cDec( row["ZirconeQTY"])+Comon.cDec(row["BagateQTY"])+Comon.cDec(row["RountDaimondQTY"])+Comon.cDec( row["WaxPillar"]));
                    row["MaterialWeight"] = MaterialWeight.ToString();
                    row["MaterialEquivalent"] =Comon.cDec(Lip.GetValue("SELECT sum( [EquQty])  FROM  [Manu_AfforestationFactoryMaster] where Cancel=0 and BranchID="+Comon.cInt(cmbBranchesID.EditValue)+" and [OrderID]='"+row["OrderID"]+"'")).ToString();
                    row["EstimatedWeight"] = Comon.cDec( row["MaterialWeight"])*Comon.cDec( row["MaterialEquivalent"]);
                    _sampleData.Rows.Add(row);
                }
                gridControl1.DataSource = _sampleData;


                //بعد الصب                
                dt = Lip.SelectRecord(str);
                _sampleDataAfter.Clear();
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    row = _sampleDataAfter.NewRow();
                    row["Sn"] = _sampleDataAfter.Rows.Count + 1;
                    row["OrderID"] = dt.Rows[i]["OrderID"].ToString();
                    row["OrderDate"] = Comon.ConvertSerialDateTo(dt.Rows[i]["OrderDate"].ToString());
                    row["CustomerName"] = dt.Rows[i]["CustomerName"].ToString();
                    row["DelegateName"] = dt.Rows[i]["DelegateName"].ToString();
                    row["GuidanceName"] = dt.Rows[i]["GuidanceName"].ToString();
                    row["TypeOrdersName"] = dt.Rows[i]["TypeOrdersName"].ToString();

                    DataTable dtt = Lip.SelectRecord("SELECT  sum([Debit]) as QTYBefore , sum([Credit]) as QTYAfter   FROM  [Menu_FactoryRunCommandfactory] inner join [Menu_FactoryRunCommandMaster] on Menu_FactoryRunCommandMaster.ComandID=[Menu_FactoryRunCommandfactory].ComandID and Menu_FactoryRunCommandMaster.BranchID=[Menu_FactoryRunCommandfactory].BranchID where Menu_FactoryRunCommandfactory.HimLost=1 and  Menu_FactoryRunCommandMaster.BarCode='" + row["OrderID"] + "' and Menu_FactoryRunCommandMaster.Cancel=0 and Menu_FactoryRunCommandMaster.BranchID="+Comon.cInt(cmbBranchesID.EditValue));
                    decimal OrderGold18 = Comon.cDec(Lip.GetValue("SELECT  [GoldQTYCloves] FROM  [Manu_AfforestationFactoryMaster] where [OrderID]='" + row["OrderID"] + "'  and Cancel=0 and BranchID=" + Comon.cInt(cmbBranchesID.EditValue)));
                   
                    //بعد الصب
                    row["OrderGold18"] = OrderGold18.ToString();
                    if (dtt.Rows.Count > 0)
                        row["WeightBeforeManufacturing"] = dtt.Rows[0]["QTYAfter"].ToString();
                    else
                        row["WeightBeforeManufacturing"] = 0;
                    DataTable dtOrderDetail = GetOrderDetail(dt.Rows[i]["OrderID"].ToString(),7);
                    var ZirconeQTY = "0";
                    var BagateQTY = "";
                    var RountDaimondQTY = "";
                    if (dtOrderDetail.Rows.Count > 0)
                    {
                        ZirconeQTY = dtOrderDetail.Compute("SUM(QTY)", "BaseID = 4").ToString();
                        BagateQTY = dtOrderDetail.Compute("SUM(QTY)", "BaseID = 2").ToString();
                        RountDaimondQTY = dtOrderDetail.Compute("SUM(QTY)", "BaseID = 3").ToString();
                    }
                    //var TotalQTY = dtOrderDetail.Compute("SUM(QTY)", "1 = 1");
                    row["ZirconeQTY"] = ZirconeQTY;
                    row["BagateQTY"] = BagateQTY;
                    row["RountDaimondQTY"] = RountDaimondQTY;
                    row["BagateAfterCompondQTY"] = dt.Rows[i]["BagateAfterCompondQTY"].ToString();
                    row["GoldChain"] = dt.Rows[i]["GoldChain"].ToString();
                    row["OrderWeightReady"] = dt.Rows[i]["OrderWeightReady"].ToString();
                    row["TotalOrderLost"] = dt.Rows[i]["TotalOrderLost"].ToString();
                    row["BagateAfterCompondQTY"] = dt.Rows[i]["BagateAfterCompondQTY"].ToString();
                    row["TotalOrderWeight"] = dt.Rows[i]["TotalOrderWeight"].ToString();
                    _sampleDataAfter.Rows.Add(row);
                }
                gridControl2.DataSource = _sampleDataAfter;

                //تكلفة الطلبية

                dt = Lip.SelectRecord(str);
                _sampleDataOrderCost.Clear();
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    row = _sampleDataOrderCost.NewRow();
                    row["Sn"] = _sampleDataOrderCost.Rows.Count + 1;
                    row["OrderID"] = dt.Rows[i]["OrderID"].ToString();
                    row["OrderDate"] = Comon.ConvertSerialDateTo(dt.Rows[i]["OrderDate"].ToString());
                    row["CustomerName"] = dt.Rows[i]["CustomerName"].ToString();
                    row["DelegateName"] = dt.Rows[i]["DelegateName"].ToString();
                    row["GuidanceName"] = dt.Rows[i]["GuidanceName"].ToString();
                    row["TypeOrdersName"] = dt.Rows[i]["TypeOrdersName"].ToString();
                    DataTable dtt = Lip.SelectRecord("SELECT  [QTYGram], [QTYOrder],SalesPriceQram    FROM  [Menu_ProductionExpensesMaster] where [Cancel]=0 and BranchID="+Comon.cInt(cmbBranchesID.EditValue)+" and OrderID='" + row["OrderID"] + "'");
                    // التكاليف
                    if (dtt.Rows.Count > 0)
                    {
                        decimal OrderCost = Comon.cDec(Comon.cDec(dtt.Rows[0]["QTYGram"]) * Comon.cDec(dtt.Rows[0]["QTYOrder"]));
                        decimal OrderSales = Comon.cDec(Comon.cDec(dtt.Rows[0]["SalesPriceQram"]) * Comon.cDec(dtt.Rows[0]["QTYOrder"]));
                        row["OrderCost"] = OrderCost;
                        row["OrderIncome"] = OrderSales;
                        row["OrderProfit"] =  Comon.cDec(Comon.cDec(row["OrderIncome"])-Comon.cDec(row["OrderCost"]));
                        row["TotalOrderWeight"] = Comon.cDec(dtt.Rows[0]["QTYOrder"]).ToString();
                    }
                    else
                    {
                        row["OrderCost"] = 0;
                        row["OrderIncome"] = 0;
                        row["OrderProfit"] = 0;
                        row["TotalOrderWeight"] = 0;
                    }
                    _sampleDataOrderCost.Rows.Add(row);
                }
                gridControl3.DataSource = _sampleDataOrderCost;

                //SortData();
                Totals();

                

                if (gridView1.RowCount > 0)
                {

                    btnShow.Visible = true;
                    txtGroupID.Enabled = false;
                    txtDelegeteID.Enabled = false;
                    txtToItemNo.Enabled = false;
                    txtFromItemNo.Enabled = false;
                    txtToDate.Enabled = false;
                    cmbTypeID.Enabled = false;
                    cmbQtyBalance.Enabled = false;
                    txtUserID.Enabled = false;
                    cmbTypeOrder.Enabled = false;
                   
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

        
        private DataTable GetOrderDetail(string OrderID,int CNDTYPE)
        {

            DataTable dt = Manu_ZirconDiamondFactoryDAL.frmGetDataDetailByOrderID(OrderID, Comon.cInt(  Comon.cInt(cmbBranchesID.EditValue)), UserInfo.FacilityID, 6, CNDTYPE);

            return dt;
        }
        
           ////////////// print_COde//////////////////////////////////
        protected override void DoPrint()
        {
            try
            {
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);

                /******************** Report Body *************************/

                bool IncludeHeader = true;
                string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");
                rptFormName = "rptStocktakingByQTYArb";
                if (UserInfo.Language == iLanguage.English)
                    rptFormName = ReportName + "Arb";
                XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);

                /********************** Master *****************************/
                rptForm.RequestParameters = false;

                rptForm.Parameters["StoreName"].Value = lblDelegeteName.Text.Trim().ToString();
                rptForm.Parameters["ByPrice"].Value = cmbTypeOrder.Text.Trim().ToString();


                  rptForm.Parameters["FromItem"].Value = txtFromItemNo.Text.Trim().ToString();
                rptForm.Parameters["ToItem"].Value = txtToItemNo .Text.Trim().ToString();
                rptForm.Parameters["ItemType"].Value = cmbTypeID.Text.Trim().ToString();
                rptForm.Parameters["Group"].Value = lblGroupID.Text.Trim().ToString();

                rptForm.Parameters["CostCenter"].Value = lblUserName .Text.Trim().ToString();
                rptForm.Parameters["ToDate"].Value = txtToDate.Text.Trim().ToString();
                    rptForm.Parameters["BalanceType"].Value = lblDelegeteName.Text.Trim().ToString();
                    rptForm.Parameters["parameter1"].Value = "الاجمالي بحسب " + cmbTypeOrder.Text.Trim().ToString();
                


                for (int i = 0; i < rptForm.Parameters.Count; i++)
                    rptForm.Parameters[i].Visible = false;
                /********************** Details ****************************/
                var dataTable = new dsReports.rptStocktakingDataTable();

                for (int i = 0; i <= gridView1.DataRowCount - 2; i++)
                {
                    var row = dataTable.NewRow();

                    row["#"] = i + 1;
                    row["Barcode"] = gridView1.GetRowCellValue(i, "Barcode").ToString();
                    row["ItemName"] = gridView1.GetRowCellValue(i, "ItemName").ToString();
                    row["ItemID"] = gridView1.GetRowCellValue(i, "ItemID").ToString();
                    row["SizeName"] = gridView1.GetRowCellValue(i, "SizeName").ToString();
                    row["QTY"] = gridView1.GetRowCellValue(i, "Qty").ToString();
                    row["QtyVisical"] = gridView1.GetRowCellValue(i, "QtyVisical").ToString();
                    row["Price"] = gridView1.GetRowCellValue(i, "Price").ToString();
                    row["Total"] = gridView1.GetRowCellValue(i, "Total").ToString();
                    row["QTYBIN"] = gridView1.GetRowCellValue(i, "QBIN").ToString();
                    row["QTYBOUT"] = gridView1.GetRowCellValue(i, "QBOUT").ToString();
                    row["QAIN"] = gridView1.GetRowCellValue(i, "QAIN").ToString();
                    row["QAOUT"] = gridView1.GetRowCellValue(i, "QAOUT").ToString();
                    row["ANET"] = gridView1.GetRowCellValue(i, "ANET").ToString();
                    row["BNET"] = gridView1.GetRowCellValue(i, "BNET").ToString();
                    row["FinalQTY"] = gridView1.GetRowCellValue(i, "FinalQTY").ToString();
                    row["SAlEPRICE"] = gridView1.GetRowCellValue(i, "SalePrice").ToString();
                 
                    dataTable.Rows.Add(row);
                }
                rptForm.DataSource = dataTable;
                rptForm.DataMember = "rptStocktaking";

                /******************** Report Binding ************************/
                XRSubreport subreport = (XRSubreport)rptForm.FindControl("subRptCompanyHeader", true);
                subreport.Visible = IncludeHeader;
                subreport.ReportSource = ReportComponent.CompanyHeaderLand();
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
                    DataTable dt = ReportComponent.SelectRecord("SELECT *  from Printers where ReportName='" + ReportName + "'");
                   if (dt.Rows.Count > 0)for (int i = 1; i < 6; i++)
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

        private void frmStocktaking_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3 || e.KeyCode == Keys.F4)
                Find();
            if (e.KeyCode == Keys.F5)
                DoPrint();
        }

  

        private void Label2_Click(object sender, EventArgs e)
        {

        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            try{
            GridColumn col;
            {
                col = gridView1.Columns[0]; ;
                var cellValue = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, col);
                if (cellValue != null)
                {
                    frmItemBalance frm3 = new frmItemBalance();
                    if (Permissions.UserPermissionsFrom(frm3, frm3.ribbonControl1, UserInfo.ID,   Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm3);
                        frm3.Show();
                      //  frm3.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                        frm3.txtBarCode.Text = cellValue.ToString();
                        frm3.txtOldBarcodeID_Validating(null, null);
                        if (!string.IsNullOrEmpty (txtDelegeteID.Text) )
                        frm3.StoreChange(Comon.cLong(txtDelegeteID.Text));
                        frm3.btnShow_Click(null, null);
                    }
                    else
                        frm3.Dispose();
               
                
                   
                }
            }
            }
            catch { }

        }


        private void OpenWindow(BaseForm frm)
        {

            try{
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID,   Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
            }
            catch { }

        }


        /// <summary>


        public DataTable ChangeStoreID(long stroeID)
        {
            try{
            txtDelegeteID.Text = stroeID.ToString();
            cmbTypeOrder.EditValue = "CostPrice" ;// MySession.GlobalCalcStockBy;

            _nativeData.Columns.Add(new DataColumn("Sn", typeof(string)));
            _nativeData.Columns.Add(new DataColumn("Barcode", typeof(string)));
            _nativeData.Columns.Add(new DataColumn("ItemID", typeof(string)));
            _nativeData.Columns.Add(new DataColumn("SizeID", typeof(string)));

            _nativeData.Columns.Add(new DataColumn("ItemName", typeof(string)));
            _nativeData.Columns.Add(new DataColumn("SizeName", typeof(string)));
            _nativeData.Columns.Add(new DataColumn("Qty", typeof(string)));
            _nativeData.Columns.Add(new DataColumn("QtyVisical", typeof(string)));
            _nativeData.Columns.Add(new DataColumn("Price", typeof(string)));
            _nativeData.Columns.Add(new DataColumn("Total", typeof(string)));
            _nativeData.Columns.Add(new DataColumn("MinLimitQty", typeof(string)));
            _nativeData.Columns.Add(new DataColumn("GroupID", typeof(string)));

            // dt.Columns.Add("MinLimitQty", System.Type.GetType("System.Decimal"))
            //dt.Columns.Add("GroupID", System.Type.GetType("System.Int32"))
            btnShow_Click(null, null);

            for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
            {
                var row = _nativeData.NewRow();

                row["Sn"] = i + 1;
                row["Barcode"] = gridView1.GetRowCellValue(i, "Barcode").ToString();
                row["ItemName"] = gridView1.GetRowCellValue(i, "ItemName").ToString();
                row["ItemID"] = gridView1.GetRowCellValue(i, "ItemID").ToString();
                row["SizeName"] = gridView1.GetRowCellValue(i, "SizeName").ToString();
                row["QTY"] = gridView1.GetRowCellValue(i, "Qty").ToString();
                row["QtyVisical"] = gridView1.GetRowCellValue(i, "QtyVisical").ToString();
                row["Price"] = gridView1.GetRowCellValue(i, "Price").ToString();
                row["Total"] = gridView1.GetRowCellValue(i, "Total").ToString();


                _nativeData.Rows.Add(row);
            }
            }
            catch { }

            return _nativeData;
        }

        private void cmbPriceBy_EditValueChanged(object sender, EventArgs e)
        {
            label1.Text = "الاجمالي بحسب " + cmbTypeOrder.Text+":";
        }

        private void labelControl5_Click(object sender, EventArgs e)
        {

        }

        private void ribbonControl1_Click(object sender, EventArgs e)
        {

        }
        protected override void DoAddFrom()
        {
            try
            {
                _sampleData.Clear();
                gridControl1.RefreshDataSource();
                txtGroupID.Text = "";
                txtGroupID_Validating(null, null);
                txtDelegeteID.Text = "";
                txtUserID.Text = "";
                txtCostCenterID_Validating(null, null);
                txtStoreID_Validating(null, null);
                txtGroupID.Enabled = true;
                txtDelegeteID.Enabled = true;
                txtToItemNo.Enabled = true;
                txtFromItemNo.Enabled = true;
                txtToDate.Enabled = true;
                cmbTypeID.Enabled = true;
                cmbQtyBalance.Enabled = true;
                txtUserID.Enabled = true;
                cmbTypeID.ItemIndex = -1;
                cmbTypeOrder.ItemIndex = 0;
                cmbTypeOrder.Enabled = true;


            }
            catch (Exception ex)
            {
                //WT.msgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        { 
        }
        private void btnStockiApproval_Click(object sender, EventArgs e)
        {
            

        }
        private void Totals()
        {
            try
            {
                decimal Total = 0;
                decimal t = 0;
                DataRow row;
                for (int i = 0; i <= _sampleData.Rows.Count - 1; i++)
                {
                    t = Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["FinalQTY"]) * Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Price"]);

                    Total += t;
                    
                }
                lblStockValu.Text = Comon.ConvertToDecimalPrice(Total).ToString();
                row = _sampleData.NewRow();
                row["Sn"] = _sampleData.Rows.Count + 1;
                row["Barcode"] ="";
                row["ItemID"] = "";
                row["SizeID"] = "";
                row["Total1"] = " 0";
                row["ItemName"] = " الاجمالي حسب سعر " + cmbTypeOrder.Text;
                row["SizeName"] = "";
                row["Qty"] =  "";
                row["QtyVisical"] = "";
                row["Price"] ="";
                row["Total"] = Comon.ConvertToDecimalPrice(Total);
                _sampleData.Rows.Add(row);
                 
            }
            catch { }

        }
     
       


        /// //////////////////////////////////////////////////////////    }

        


       

       
        private void gridView1_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            GridView View = sender as GridView;
            //if (e.RowHandle >= 0)
            //{
            //    //  string category = View.GetRowCellDisplayText(e.RowHandle, View.Columns["Category"]);
            //    if (View.GetRowCellDisplayText(e.RowHandle, View.Columns["ItemID"]).ToString() == "")
            //    {
            //        if (Comon.cInt(View.GetRowCellDisplayText(e.RowHandle, View.Columns["ItemID"]).ToString()) > 0)
            //        {
            //            e.Appearance.BackColor = Color.LightYellow;
            //            e.Appearance.BackColor2 = Color.LightYellow;
            //            //  e.Appearance.Font.Styl;
                       
            //        }
            //        else {



            //            e.Appearance.BackColor = Color.LightBlue;
            //            e.Appearance.BackColor2 = Color.LightBlue;
            //            //  e.Appearance.Font.Styl;
                  
                    
            //        }

            //        e.HighPriority = true;
                    
            //    }

                
            //}
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }

        private void gridControl2_Click(object sender, EventArgs e)
        {

        }

    }

}
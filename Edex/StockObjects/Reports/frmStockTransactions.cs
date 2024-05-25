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
using DevExpress.XtraSplashScreen;
using DevExpress.XtraReports.UI;
using DevExpress.XtraGrid.Columns;
using Edex.AccountsObjects.Reports;
using Edex.DAL;
using Edex.StockObjects.Codes;
namespace Edex.StockObjects.Reports
{
    public partial class frmStockTransactions : BaseForm
    {
        private string strSQL = "";
        private string filter = "";
        DataTable dt = new DataTable();
        DataTable _nativeData = new DataTable();
        public DataTable _sampleData = new DataTable();
        DataTable dtStockInput = new DataTable();
        DataTable dtStockoutput = new DataTable();
        string FocusedControl;
        private string PrimaryName;
        private string ItemName;
        private string SizeName;
        private string GroupName;
        public frmStockTransactions()
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
                _sampleData.Columns.Add(new DataColumn("Sn", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("BarCode", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("ItemID", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("SizeID", typeof(string)));     
                _sampleData.Columns.Add(new DataColumn("ItemName", typeof(string))); 
                _sampleData.Columns.Add(new DataColumn("QtyOpening", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("SizeNameOpening", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("QtyIncomming", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("SizeNameIncoming", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("QtyOut", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("SizeNameOut", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("QtyBalance", typeof(decimal)));
                _sampleData.Columns.Add(new DataColumn("SizeNameBalance", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("StoreName", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("Total", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("AverageCost", typeof(string)));
                strSQL = "EngName";
                if (UserInfo.Language == iLanguage.Arabic)
                {
                    strSQL = "ArbName";
                    ItemName = "ArbItemName";
                    SizeName = "ArbSizeName";
                    GroupName = "ArbGroupName";
                    PrimaryName = "ArbName";
                }
                else
                {
                    ItemName = "EngItemName";
                    SizeName = "EngSizeName";
                    GroupName = "EngGroupName";
                    PrimaryName = "EngName";
                }
                FillCombo.FillComboBox(cmbTypeID, "Stc_ItemTypes", "TypeID", strSQL, "", "BranchID="+Comon.cInt(cmbBranchesID.EditValue));
                this.txtToDate.Properties.Mask.UseMaskAsDisplayFormat = true;
                this.txtToDate.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
                this.txtToDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                this.txtToDate.Properties.EditFormat.FormatString = "dd/MM/yyyy";
                this.txtToDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                this.txtToDate.Properties.Mask.EditMask = "dd/MM/yyyy";

                this.txtFromDate.Properties.Mask.UseMaskAsDisplayFormat = true;
                this.txtFromDate.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
                this.txtFromDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                this.txtFromDate.Properties.EditFormat.FormatString = "dd/MM/yyyy";
                this.txtFromDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                this.txtFromDate.Properties.Mask.EditMask = "dd/MM/yyyy";

                this.txtStoreID.Validating += new System.ComponentModel.CancelEventHandler(this.txtStoreID_Validating);
                this.txtCostCenterID.Validating += new System.ComponentModel.CancelEventHandler(this.txtCostCenterID_Validating);
                this.txtGroupID.Validating += new System.ComponentModel.CancelEventHandler(this.txtGroupID_Validating);

                this.txtBarCode.Validating += TxtBarCode_Validating;
                PrimaryName = "ArbName";
                if (UserInfo.Language == iLanguage.English)
                {
                    dgvColBarcode.Caption = "Bar Code ";
                    dgvColItemID.Caption = "Item No ";
                    dgvColItemName.Caption = "Item Name ";
                    dgvColTotalPrice.Caption = "Price  ";
                    dgvColSizeName.Caption = "Size Name "; 
                    dgvColQtyVisical.Caption = "Quantity Visical";
                    dgvColTotal.Caption = "Total";
                    btnShow.Text = btnShow.Tag.ToString();
                    // Label8.Text = btnShow.Tag.ToString();
                }   
            }
            catch { }
            FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
            FillCombo.FillComboBoxLookUpEdit(cmbSizeItem, "Stc_SizingUnits", "SizeID", PrimaryName, "", "BranchID=" + MySession.GlobalBranchID, (UserInfo.Language == iLanguage.English ? "Select Size" : "حدد الوحدة"));
            cmbBranchesID.EditValue = MySession.GlobalBranchID;
            cmbBranchesID.ReadOnly = !MySession.GlobalAllowBranchModificationAllScreens;
        }

        private void TxtBarCode_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                string strSQLForBarcode = " SELECT   TOP (1)  Stc_Items.ArbName AS ItemName   FROM  Stc_Items RIGHT OUTER JOIN     Sales_PurchaseInvoiceDetails LEFT OUTER JOIN "
                + " Stc_SizingUnits ON Sales_PurchaseInvoiceDetails.SizeID = Stc_SizingUnits.SizeID and Sales_PurchaseInvoiceDetails.BranchID = Stc_SizingUnits.BranchID ON Stc_Items.ItemID = Sales_PurchaseInvoiceDetails.ItemID  and Stc_Items.BranchID = Sales_PurchaseInvoiceDetails.BranchID "
                + "  WHERE  (Sales_PurchaseInvoiceDetails.BarCode ='" + txtBarCode.Text + "') AND (Sales_PurchaseInvoiceDetails.Cancel = 0) and Sales_PurchaseInvoiceDetails.BranchID=" + Comon.cInt(cmbBranchesID.EditValue);

                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQLForBarcode, "arb");

                DataTable barc = new DataTable();
                barc = Lip.SelectRecord(strSQLForBarcode);
                if (barc.Rows.Count > 0)
                {
                    lblBarCodeName.Text = barc.Rows[0][0].ToString().ToUpper();
                    txtBarCode.Text = txtBarCode.Text.ToString().ToUpper();
                }
                else
                {
                    lblBarCodeName.Text = "";
                    txtBarCode.Text = "";
                }

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        public bool Find()
        {
           
                CSearch cls = new CSearch();
                int[] ColumnWidth = new int[] { 100, 300 };
                string SearchSql = "";
                string Condition = "Where 1=1";

                FocusedControl = GetIndexFocusedControl();

           if (FocusedControl.Trim() == txtStoreID.Name)
            {                  
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtStoreID, lblStoreName, "StoreID", "رقم الحساب", Comon.cInt(cmbBranchesID.EditValue));
                else
                    PrepareSearchQuery.Find(ref cls, txtStoreID, lblStoreName, "StoreID", "Account ID", Comon.cInt(cmbBranchesID.EditValue));
            }
                else if (FocusedControl.Trim() == txtCostCenterID.Name)
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Search(txtCostCenterID, lblCostCenterName, "CostCenterID", "اسم مركز التكلفة", "رقم مركز التكلفة");
                    else
                        PrepareSearchQuery.Search(txtCostCenterID, lblCostCenterName, "CostCenterID", "Cost Center Name", "Cost Center ID");
                }
                else if (FocusedControl.Trim() == txtGroupID.Name)
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Search(txtGroupID, lblGroupID, "GroupID", "اسـم المجـمـوعة", "رقـم المجـمـوعة");
                    else
                        PrepareSearchQuery.Search(txtGroupID, lblGroupID, "GroupID", "Group Name", "Group ID");
                }
               else if (FocusedControl.Trim() == txtBarCode.Name)
               {
                   if (UserInfo.Language == iLanguage.Arabic)
                       PrepareSearchQuery.Search(txtBarCode, lblBarCodeName, "BarCodeForPurchaseInvoice", "اسـم الـمـادة", "البـاركـود");
                   else
                       PrepareSearchQuery.Search(txtBarCode, lblBarCodeName, "BarCodeForPurchaseInvoice", "Item Name", "BarCode");
               }
           if (FocusedControl.Trim() == txtToItemNo.Name)
           {
               if (UserInfo.Language == iLanguage.Arabic)
                   PrepareSearchQuery.Find(ref cls, txtToItemNo, null, "Items", "رقـم الـمــادة", Comon.cInt(cmbBranchesID.EditValue));
               else
                   PrepareSearchQuery.Find(ref cls, txtToItemNo, null, "Items", "Item ID", Comon.cInt(cmbBranchesID.EditValue));
           }
           if (FocusedControl.Trim() == txtFromItemNo.Name)
           {
               if (UserInfo.Language == iLanguage.Arabic)
                   PrepareSearchQuery.Find(ref cls, txtFromItemNo, null, "Items", "رقـم الـمــادة", Comon.cInt(cmbBranchesID.EditValue));
               else
                   PrepareSearchQuery.Find(ref cls, txtFromItemNo, null, "Items", "Item ID", Comon.cInt(cmbBranchesID.EditValue));
           }

            

            return GetSelectedSearchValue(cls);

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
        public bool GetSelectedSearchValue(CSearch cls)
        {
            if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
            {

                if (FocusedControl == txtStoreID.Name)
                {
                    txtStoreID.Text = cls.PrimaryKeyValue.ToString();
                    txtStoreID_Validating(null, null);
                }

                else if (FocusedControl == txtCostCenterID.Name)
                {
                    txtCostCenterID.Text = cls.PrimaryKeyValue.ToString();
                    txtCostCenterID_Validating(null, null);
                }
                return true;
            }
            return false;
        }
        private void txtGroupID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT   " + (UserInfo.Language == iLanguage.Arabic ? "ArbName" : "EngName") + "   as GroupName FROM dbo.Stc_ItemsGroups WHERE GroupID =" + Comon.cInt(txtGroupID.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                CSearch.ControlValidating(txtGroupID, lblGroupID, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

        }

     


        
     

    
        private void txtStoreID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT ArbName AS AccountName FROM Acc_Accounts WHERE   (Cancel = 0) and BranchID="+Comon.cInt( cmbBranchesID.EditValue)+" AND (AccountID = " + txtStoreID.Text + ") ";
                CSearch.ControlValidating(txtStoreID, lblStoreName, strSQL);
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
                strSQL = "SELECT   " + (UserInfo.Language == iLanguage.Arabic ? "ArbName" : "EngName") + "  as CostCenterName FROM Acc_CostCenters WHERE CostCenterID =" + Comon.cInt(txtCostCenterID.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                CSearch.ControlValidating(txtCostCenterID, lblCostCenterName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
           
            try
            {
                btnShow.Visible = false;
                lblStockValu.Text = "0";
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                Application.DoEvents();
                _sampleData.Clear();
                GetStocktaking();
                SortData();
                Totals();
                gridControl1.DataSource = _sampleData;
                if (gridView1.DataRowCount > 0)
                {
                    btnShow.Visible = true;
                    txtGroupID.Enabled = false;
                    txtStoreID.Enabled = false;
                    txtToItemNo.Enabled = false;
                    txtFromItemNo.Enabled = false;
                    txtToDate.Enabled = false;
                    cmbTypeID.Enabled = false;
                    cmbQtyBalance.Enabled = false;
                    txtCostCenterID.Enabled = false; 
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
        private void SortData()
        {
            try
            {
                DataTable dt = new DataTable(); DataRow row;
                dt = _sampleData.Copy();
                DataView view = dt.DefaultView;
                if (cmbQtyBalance.ItemIndex == 1)//' المواد التي رصيدها اكبر من الصفر
                    view.RowFilter = "QtyBalance > 0";
                if (cmbQtyBalance.ItemIndex == 2) // ' المواد التي رصيدها يساوي الصفر
                    view.RowFilter = "QtyBalance = 0";
                if (cmbQtyBalance.ItemIndex == 3) // ' المواد التي رصيدها يساوي الصفر
                    view.RowFilter = "QtyBalance < 0";
                //    view.RowFilter = "dgvColQTY < 0"
                //End If
                _sampleData.Rows.Clear();
                //  DataRow row;
                for (int i = 0; i <= view.Count - 1; i++)
                {
                    row = _sampleData.NewRow();
                    row["Sn"] = _sampleData.Rows.Count + 1;
                    row["BarCode"] = view[i]["BarCode"].ToString();
                    row["ItemID"] = Comon.cLong(view[i]["ItemID"].ToString());
                    row["SizeID"] = Comon.cLong(view[i]["SizeID"].ToString());
                    row["StoreName"] =  view[i]["StoreName"].ToString();
                    row["QtyOpening"] = view[i]["QtyOpening"].ToString();
                    row["QtyIncomming"] = view[i]["QtyIncomming"].ToString();
                    row["QtyOut"] = view[i]["QtyOut"].ToString();
                    row["QtyBalance"] = view[i]["QtyBalance"].ToString();
                    row["ItemName"] = view[i]["ItemName"].ToString();
                    row["SizeNameOpening"] = view[i]["SizeNameOpening"].ToString();
                    row["SizeNameIncoming"] = view[i]["SizeNameOpening"].ToString();
                    row["SizeNameOut"] = view[i]["SizeNameOpening"].ToString();
                    row["SizeNameBalance"] = view[i]["SizeNameOpening"].ToString();

                    row["AverageCost"] = view[i]["AverageCost"].ToString();
                    row["Total"] = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(row["QtyBalance"]) * Comon.ConvertToDecimalPrice(row["AverageCost"]));
                    _sampleData.Rows.Add(row);
                }
            }
            catch(Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
   
        //private void GetStocktaking()
        //{
        //    try
        //    {
        //        DataRow row;

        //        dt = Lip.SelectRecord(StockTransaction());
        //        if (strSQL != null || strSQL != "")
        //        {
        //            if (dt.Rows.Count > 0)
        //            {
        //                for (int i = 0; i <= dt.Rows.Count - 1; i++)
        //                {
        //                    row = _sampleData.NewRow();

        //                    row["Sn"] = _sampleData.Rows.Count + 1;
        //                    row["BarCode"] = dt.Rows[i]["BarCode"].ToString();
        //                    row["ItemID"] = Comon.cLong(dt.Rows[i]["ItemID"].ToString());
        //                    row["SizeID"] = Comon.cLong(dt.Rows[i]["SizeID"].ToString());
        //                    row["StoreName"] = Lip.GetValue("SELECT   [ArbName] as StoreName FROM [Stc_Stores]  where [AccountID]  =" + dt.Rows[i]["StoreID"].ToString() + "   and Cancel =0 and [BranchID]="+cmbBranchesID.EditValue  );
        //                    row["ItemName"] = Lip.GetValue("SELECT [ArbName] FROM  [Stc_Items] where [ItemID]=" + Comon.cLong(row["ItemID"]));
        //                    row["SizeNameOpening"] = dt.Rows[i]["SizeName"].ToString();
        //                    row["SizeNameIncoming"] = dt.Rows[i]["SizeName"].ToString();
        //                    row["SizeNameOut"] = dt.Rows[i]["SizeName"].ToString();
        //                    row["SizeNameBalance"] = dt.Rows[i]["SizeName"].ToString();
        //                    row["QtyOpening"] = Comon.ConvertToDecimalQty(dt.Rows[i]["QtyOpening"].ToString());
        //                    row["QtyIncomming"] = Comon.ConvertToDecimalQty(dt.Rows[i]["QtyIncomming"].ToString());
        //                    row["QtyOut"] = Comon.ConvertToDecimalQty(dt.Rows[i]["QtyOut"].ToString());
        //                    row["QtyBalance"] = Comon.ConvertToDecimalQty(dt.Rows[i]["QtyBalance"].ToString());
                            
        //                    DataTable dtt = frmItems.GetItemMoving(Comon.cLong(dt.Rows[i]["ItemID"]), Comon.cInt(dt.Rows[i]["SizeID"]), Comon.cDbl(dt.Rows[i]["StoreID"]), Comon.cInt(dt.Rows[i]["BranchID"]), 0, Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text)), Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text)));
                          
        //                    row["AverageCost"] = dtt.Rows[dtt.Rows.Count-1]["CurentAverageCostPrice"];
        //                    //row["AverageCost"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["AverageCost"]);
        //                    row["Total"] = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(row["QtyBalance"] )*Comon.ConvertToDecimalPrice( row["AverageCost"]));
        //                    _sampleData.Rows.Add(row);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        SplashScreenManager.CloseForm(false);
        //        Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
        //    }
        //}


        private void GetStocktaking()
        {
            try
            {
                DataRow row;

                dt = Lip.SelectRecord(StockTransaction());
                if (strSQL != null || strSQL != "")
                {
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            if (cmbSizeItem.EditValue!=null&& Comon.cInt(cmbSizeItem.EditValue.ToString()) > 0)
                            {
                                if (Comon.cInt(cmbSizeItem.EditValue.ToString()) == Comon.cInt(dt.Rows[i]["SizeID"].ToString()))
                                {
                                    row = _sampleData.NewRow();
                                    row["Sn"] = _sampleData.Rows.Count + 1;
                                    row["BarCode"] = dt.Rows[i]["BarCode"].ToString();
                                    row["ItemID"] = Comon.cLong(dt.Rows[i]["ItemID"].ToString());
                                    row["SizeID"] = Comon.cLong(dt.Rows[i]["SizeID"].ToString());
                                    row["StoreName"] = Lip.GetValue("SELECT   [ArbName] as StoreName FROM [Stc_Stores]  where [AccountID]  =" + dt.Rows[i]["StoreID"].ToString() + "   and Cancel =0 and [BranchID]=" +Comon.cInt( cmbBranchesID.EditValue));
                                    row["ItemName"] = Lip.GetValue("SELECT [ArbName] FROM  [Stc_Items] where [ItemID]=" + Comon.cLong(row["ItemID"])+" and BranchID="+Comon.cInt(cmbBranchesID.EditValue));
                                    row["SizeNameOpening"] = dt.Rows[i]["SizeName"].ToString();
                                    row["SizeNameIncoming"] = dt.Rows[i]["SizeName"].ToString();
                                    row["SizeNameOut"] = dt.Rows[i]["SizeName"].ToString();
                                    row["SizeNameBalance"] = dt.Rows[i]["SizeName"].ToString();
                                    row["QtyOpening"] = Comon.ConvertToDecimalQty(dt.Rows[i]["QtyOpening"].ToString());
                                    row["QtyIncomming"] = Comon.ConvertToDecimalQty(dt.Rows[i]["QtyIncomming"].ToString());
                                    row["QtyOut"] = Comon.ConvertToDecimalQty(dt.Rows[i]["QtyOut"].ToString());

                                    decimal RemindQtyItemByMinUnit = Lip.GetRemindQTY(Comon.cInt(row["ItemID"].ToString()), Comon.cInt(cmbSizeItem.EditValue.ToString()), Comon.cDbl(txtStoreID.Text));
                                    row["QtyBalance"] = Comon.ConvertToDecimalQty(RemindQtyItemByMinUnit);

                                    //DataTable dtt = frmItems.GetItemMoving(Comon.cLong(dt.Rows[i]["ItemID"]), Comon.cInt(dt.Rows[i]["SizeID"]), Comon.cDbl(dt.Rows[i]["StoreID"]), Comon.cInt(dt.Rows[i]["BranchID"]), 0, Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text)), Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text)));
                                    //row["AverageCost"] = dtt.Rows[dtt.Rows.Count - 1]["CurentAverageCostPrice"];
                                    //row["AverageCost"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["AverageCost"]);
                                    row["AverageCost"] = Comon.cDec(Lip.AverageUnit(Comon.cInt(row["ItemID"].ToString()), Comon.cInt(cmbSizeItem.EditValue.ToString()), Comon.cDbl(txtStoreID.Text)));
                                    row["Total"] = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(row["QtyBalance"]) * Comon.ConvertToDecimalPrice(row["AverageCost"]));
                                    _sampleData.Rows.Add(row);
                                }
                            }
                            else
                            {
                                row = _sampleData.NewRow();
                                row["Sn"] = _sampleData.Rows.Count + 1;
                                row["BarCode"] = dt.Rows[i]["BarCode"].ToString();
                                row["ItemID"] = Comon.cLong(dt.Rows[i]["ItemID"].ToString());
                                row["SizeID"] = Comon.cLong(dt.Rows[i]["SizeID"].ToString());
                                row["StoreName"] = Lip.GetValue("SELECT   [ArbName] as StoreName FROM [Stc_Stores]  where [AccountID]  =" + dt.Rows[i]["StoreID"].ToString() + "   and Cancel =0 and [BranchID]=" + cmbBranchesID.EditValue);
                                row["ItemName"] = Lip.GetValue("SELECT [ArbName] FROM  [Stc_Items] where [ItemID]=" + Comon.cLong(row["ItemID"])+" and BranchID="+Comon.cInt(cmbBranchesID.EditValue));
                                row["SizeNameOpening"] = dt.Rows[i]["SizeName"].ToString();
                                row["SizeNameIncoming"] = dt.Rows[i]["SizeName"].ToString();
                                row["SizeNameOut"] = dt.Rows[i]["SizeName"].ToString();
                                row["SizeNameBalance"] = dt.Rows[i]["SizeName"].ToString();
                                row["QtyOpening"] = Comon.ConvertToDecimalQty(dt.Rows[i]["QtyOpening"].ToString());
                                row["QtyIncomming"] = Comon.ConvertToDecimalQty(dt.Rows[i]["QtyIncomming"].ToString());
                                row["QtyOut"] = Comon.ConvertToDecimalQty(dt.Rows[i]["QtyOut"].ToString());
                                //row["QtyBalance"] = Comon.ConvertToDecimalQty(dt.Rows[i]["QtyBalance"].ToString());

                                decimal RemindQtyItemByMinUnit = Lip.GetRemindQTY(Comon.cInt(row["ItemID"].ToString()), Comon.cInt(row["SizeID"]), Comon.cDbl(dt.Rows[i]["StoreID"]));
                                //DataTable dtt = frmItems.GetItemMoving(Comon.cLong(dt.Rows[i]["ItemID"]), Comon.cInt(dt.Rows[i]["SizeID"]), Comon.cDbl(dt.Rows[i]["StoreID"]), Comon.cInt(dt.Rows[i]["BranchID"]), 0, Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text)), Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text)));
                                row["AverageCost"] = Comon.cDec(Lip.AverageUnit(Comon.cInt(row["ItemID"].ToString()), Comon.cInt(row["SizeID"]), Comon.cDbl(dt.Rows[i]["StoreID"])));
                                row["QtyBalance"] = Comon.ConvertToDecimalQty(RemindQtyItemByMinUnit);
                                //row["AverageCost"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["AverageCost"]);
                                row["Total"] = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(row["QtyBalance"]) * Comon.ConvertToDecimalPrice(row["AverageCost"]));
                                _sampleData.Rows.Add(row);
                            }


                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        ////////////// print_COde//////////////////////////////////
        protected override void DoPrint()
        {
            try
            {
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                /******************** Report Body *************************/
                ReportName = "‏‏rptStockTransaction";
                if (!checkEdit1.Checked)
                    ReportName = "rptStockTransactionBlanceQty";
                bool IncludeHeader = true;
                string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");

                if (UserInfo.Language == iLanguage.English)
                    rptFormName = ReportName + "Arb";
                XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);

                /********************** Master *****************************/
                rptForm.RequestParameters = false;
                rptForm.Parameters["StoreName"].Value = lblStoreName.Text.Trim().ToString();
                rptForm.Parameters["FromItem"].Value = txtFromItemNo.Text.Trim().ToString();
                rptForm.Parameters["ToItem"].Value = txtToItemNo.Text.Trim().ToString();
                rptForm.Parameters["ItemType"].Value = cmbTypeID.Text.Trim().ToString();
                rptForm.Parameters["Group"].Value = lblGroupID.Text.Trim().ToString();
                rptForm.Parameters["CostCenter"].Value = lblCostCenterName.Text.Trim().ToString();
                rptForm.Parameters["ToDate"].Value = txtToDate.Text.Trim().ToString();
                rptForm.Parameters["BalanceType"].Value = cmbQtyBalance.Text.Trim().ToString();

                for (int i = 0; i < rptForm.Parameters.Count; i++)
                    rptForm.Parameters[i].Visible = false;

                /********************** Details ****************************/
                var dataTable = new dsReports.rptStockTransactionDataTable();
                for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                {
                    var row = dataTable.NewRow();
                    row["#"] = i + 1;
                    row["BarCode"] = gridView1.GetRowCellValue(i, "BarCode").ToString();
                    row["ItemName"] = gridView1.GetRowCellValue(i, "ItemName").ToString();
                    row["ItemID"] = gridView1.GetRowCellValue(i, "ItemID").ToString();
                    row["QtyBalance"] = gridView1.GetRowCellValue(i, "QtyBalance").ToString();
                    row["SizeName"] = gridView1.GetRowCellValue(i, "SizeNameBalance").ToString();
                    row["QtyOpening"] = gridView1.GetRowCellValue(i, "QtyOpening").ToString();
                    row["QtyIncomming"] = gridView1.GetRowCellValue(i, "QtyIncomming").ToString();
                    row["QtyOut"] = gridView1.GetRowCellValue(i, "QtyOut").ToString();
                    row["AverageCost"] = gridView1.GetRowCellValue(i, "AverageCost").ToString(); 
                    row["Total"] = gridView1.GetRowCellValue(i, "Total").ToString();
                   
                    dataTable.Rows.Add(row);
                }
                rptForm.DataSource = dataTable;
                rptForm.DataMember = "rptStockTransaction";
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
                    if (dt.Rows.Count > 0) for (int i = 1; i < 6; i++)
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
        

        
        private void gridview1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
            
                {
                     
                    var cellValue = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "BarCode");
                    if (cellValue != null)
                    {

                        frmItemBalanceByStores frm3 = new frmItemBalanceByStores();
                        if (Permissions.UserPermissionsFrom(frm3, frm3.ribbonControl1, UserInfo.ID, MySession.GlobalBranchID, UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frm3);
                            frm3.Show();
                            frm3.ClearFilds();
                            frm3.txtBarCode.Text = cellValue.ToString();
                         
                            if (!string.IsNullOrEmpty(txtStoreID.Text))
                                frm3.txtStoreID.Text=txtStoreID.Text;
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

            try
            {
                if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, MySession.GlobalBranchID, UserInfo.FacilityID))
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
            try
            {
                txtStoreID.Text = stroeID.ToString();
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
        
         
         
        protected override void DoAddFrom()
        {
            try
            {
                _sampleData.Clear();
                gridControl1.RefreshDataSource();
                txtGroupID.Text = "";
                txtGroupID_Validating(null, null);
                txtStoreID.Text = "";
                txtCostCenterID.Text = "";
                txtCostCenterID_Validating(null, null);
                txtStoreID_Validating(null, null);
                txtGroupID.Enabled = true;
                txtStoreID.Enabled = true;
                txtToItemNo.Enabled = true;
                txtFromItemNo.Enabled = true;
                txtFromDate.Text = "";
                txtToDate.Enabled = true;
                cmbTypeID.Enabled = true;
                cmbQtyBalance.Enabled = true;
                txtCostCenterID.Enabled = true;
                cmbTypeID.ItemIndex = -1;
                cmbBranchesID.EditValue = MySession.GlobalBranchID;
                lblStockValu.Text = "";
            }
            catch (Exception ex)
            {
                //WT.msgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        
    
        private void Totals()
        {
            try
            {
                decimal Total = 0;
                DataRow row;
                for (int i = 0; i <= _sampleData.Rows.Count - 1; i++)
                    Total += (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Total"]));
                lblStockValu.Text = Comon.ConvertToDecimalPrice(Total).ToString("N" + MySession.GlobalPriceDigits);

            }
            catch { }
        }      
        ////////////////////////
        public string StockTransaction()
        {
            try
            {
                //long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));
                filter = "";
                filter = "( dbo.Stc_ItemsMoviing.BranchID = " + Comon.cInt(cmbBranchesID.EditValue)+ ") AND dbo.Stc_ItemsMoviing.Cancel =0   AND  dbo.Stc_ItemsMoviing.ItemID >0  AND";
                strSQL = "";
                if (txtCostCenterID.Text != string.Empty)
                    filter += "  dbo.Stc_ItemsMoviing.CostCenterID=" + Comon.cInt(txtCostCenterID.Text) + " AND ";
                if (txtFromItemNo.Text != string.Empty)
                    filter += "  dbo.Stc_ItemsMoviing.ItemID >=" + Comon.cInt(txtFromItemNo.Text) + " AND ";
                if (txtToItemNo.Text != string.Empty)
                    filter = filter + "  dbo.Stc_ItemsMoviing.ItemID <=" + Comon.cInt(txtToItemNo.Text) + " AND ";
                if (txtGroupID.Text != string.Empty)
                    filter = filter + "  dbo.Stc_ItemsMoviing.GroupID =" + txtGroupID.Text + " AND ";
                if (cmbTypeID.Text != string.Empty)
                    filter = filter + "  dbo.Stc_ItemsMoviing.TypeID =" + cmbTypeID.EditValue + " AND ";
                if (txtBarCode.Text != string.Empty)
                    filter = filter + " dbo.Stc_ItemsMoviing.BarCode  ='" + txtBarCode.Text + "'  AND ";
                if (txtStoreID.Text != string.Empty)
                    filter = filter + " dbo.Stc_ItemsMoviing.StoreID  =" + Comon.cDbl(txtStoreID.Text) + "  And ";
                long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));
                long FromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text));
                if (FromDate != 0)
                    filter = filter + " dbo.Stc_ItemsMoviing.MoveDate>=" + FromDate + " AND ";

                if (ToDate != 0)
                    filter = filter + " dbo.Stc_ItemsMoviing.MoveDate<=" + ToDate + " AND ";

                filter += " Stc_ItemsMoviing.Posted=3  And ";


                filter = filter.Remove(filter.Length - 4, 4);
                strSQL = " SELECT  dbo.Stc_ItemsMoviing.BarCode,'' as StoreName, dbo.Stc_ItemsMoviing.ItemID,"
                    + "  SUM(CASE WHEN dbo.Stc_ItemsMoviing.DocumentTypeID = 15 THEN dbo.Stc_ItemsMoviing.QTY ELSE 0 END) AS QtyOpening, "
                    + "  SUM(CASE WHEN dbo.Stc_ItemsMoviing.MoveType = 1 AND dbo.Stc_ItemsMoviing.DocumentTypeID <> 15 THEN dbo.Stc_ItemsMoviing.QTY ELSE 0 END) AS QtyIncomming, "
                    + " SUM(CASE WHEN dbo.Stc_ItemsMoviing.MoveType = 2 THEN dbo.Stc_ItemsMoviing.QTY ELSE 0 END) AS QtyOut, "
                    + "   SUM(CASE WHEN dbo.Stc_ItemsMoviing.MoveType = 1 THEN dbo.Stc_ItemsMoviing.QTY ELSE -dbo.Stc_ItemsMoviing.QTY END) AS QtyBalance, "
                    + "  SUM(CASE WHEN dbo.Stc_ItemsMoviing.DocumentTypeID = 15 OR dbo.Stc_ItemsMoviing.DocumentTypeID = 23 THEN (dbo.Stc_ItemsMoviing.InPrice + dbo.Stc_ItemsMoviing.Bones) ELSE 0 END) / "
                    + "  NULLIF(SUM(CASE WHEN dbo.Stc_ItemsMoviing.DocumentTypeID = 15 OR dbo.Stc_ItemsMoviing.DocumentTypeID = 23 THEN dbo.Stc_ItemsMoviing.QTY ELSE 0 END), 0) AS AverageCost, "
                    + " dbo.Stc_SizingUnits.ArbName AS SizeName, dbo.Stc_SizingUnits.SizeID,dbo.Stc_ItemsMoviing.StoreID,dbo.Stc_ItemsMoviing.BranchID  FROM dbo.Stc_ItemsMoviing "
                    + " LEFT OUTER JOIN dbo.Stc_SizingUnits ON dbo.Stc_ItemsMoviing.SizeID = dbo.Stc_SizingUnits.SizeID and dbo.Stc_ItemsMoviing.BranchID = dbo.Stc_SizingUnits.BranchID "
                    +" WHERE  "+filter
                    + " GROUP BY dbo.Stc_ItemsMoviing.BarCode,dbo.Stc_ItemsMoviing.StoreID ,dbo.Stc_ItemsMoviing.BranchID, dbo.Stc_SizingUnits.ArbName, dbo.Stc_ItemsMoviing.SizeID,dbo.Stc_ItemsMoviing.ItemID,dbo.Stc_SizingUnits.SizeID  ";
               
                   Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            return strSQL;
        }
      
        private void frmStockTransactions_Load(object sender, EventArgs e)
        {
            DataTable dtQty = new DataTable();
            dtQty.Columns.Add("ID", System.Type.GetType("System.Int16"));
            dtQty.Columns.Add("Name", System.Type.GetType("System.String"));
            dtQty.Rows.Add("0", (UserInfo.Language == iLanguage.Arabic ? "جميع الأرصدة" : "All Qty Balance"));
            dtQty.Rows.Add("1", (UserInfo.Language == iLanguage.Arabic ? "المواد التي رصيدها اكبر من الصفر" : "Qty Balance > 0"));
            dtQty.Rows.Add("2", (UserInfo.Language == iLanguage.Arabic ? "المواد التي رصيدها يساوي الصفر" : "Qty Balance = 0"));
            dtQty.Rows.Add("3", (UserInfo.Language == iLanguage.Arabic ? "المواد التي رصيدها أقل الصفر" : "Qty Balance < 0"));
            cmbQtyBalance.Properties.DataSource = dtQty.DefaultView;
            cmbQtyBalance.Properties.DisplayMember = "Name";
            cmbQtyBalance.Properties.ValueMember = "ID";

           
            checkEdit1_CheckedChanged(null, null);
        }

        private void frmStockTransactions_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3 || e.KeyCode == Keys.F4)
                Find();
            if (e.KeyCode == Keys.F5)
                DoPrint();
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
           
             gridBand2.Visible = checkEdit1.Checked;
             gridBand3.Visible = checkEdit1.Checked;
             gridBand5.Visible = checkEdit1.Checked;
            
        }
    }
}
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
using Edex.SalesAndSaleObjects.Transactions;
using Edex.SalesAndPurchaseObjects.Transactions;
using Edex.StockObjects.Transactions;

namespace Edex.StockObjects.Reports
{
    
    public partial class frmItemBalanceByStores : Edex.GeneralObjects.GeneralForms.BaseForm
    {
        private string strSQL = "";
        private string filter = "";
        DataTable dt = new DataTable();
        DataTable _nativeData = new DataTable();
        public DataTable _sampleData = new DataTable();
        string FocusedControl;
        private string PrimaryName;
        DataTable dtFactoryOprationType = new DataTable();
        private string ItemName;
        private string SizeName;
        private string GroupName;

        public frmItemBalanceByStores()
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
                _sampleData.Columns.Add(new DataColumn("Sn", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("GroupName", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("BarCode", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("TranseID", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("DocumentTypeID", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("ItemName", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("RecordType", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("StoreName", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("QTYInB", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("SizeNameB", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("QTYInA", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("SizeNameA", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("QTYOut", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("SizeNameOut", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("MoveDate", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("InPrice", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("OutPrice", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("FinalQTY", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("FinalPrice", typeof(string)));
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
                strSQL = "Select * from Manu_ManuFactoryOprationType ";
                dtFactoryOprationType = Lip.SelectRecord(strSQL);

                FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
                cmbBranchesID.EditValue = MySession.GlobalBranchID;
                cmbBranchesID.ReadOnly = !MySession.GlobalAllowBranchModificationAllScreens;
                FillCombo.FillComboBox(cmbTypeID, "Stc_ItemTypes", "TypeID", strSQL, "", "BranchID="+MySession.GlobalBranchID);
                this.txtToDate.Properties.Mask.UseMaskAsDisplayFormat = true;
                this.txtToDate.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
                this.txtToDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                this.txtToDate.Properties.EditFormat.FormatString = "dd/MM/yyyy";
                this.txtToDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                this.txtToDate.Properties.Mask.EditMask = "dd/MM/yyyy";

                this.txtStoreID.Validating += new System.ComponentModel.CancelEventHandler(this.txtStoreID_Validating);
                this.txtCostCenterID.Validating += new System.ComponentModel.CancelEventHandler(this.txtCostCenterID_Validating);
                this.txtGroupID.Validating += new System.ComponentModel.CancelEventHandler(this.txtGroupID_Validating);
             
                this.gridView1.RowStyle+=gridView1_RowStyle;
                PrimaryName = "ArbName";

                if (UserInfo.Language == iLanguage.English)
                {
                    dgvColTranseID.Caption = "Item No ";
                    dgvColRecordType.Caption = "Item Name ";       
                    dgvColQTYIn.Caption = "Size Name ";
                    dgvColQTYInB.Caption = "Quantity";
                    dgvColQtyVisical.Caption = "Quantity Visical";
                
                    btnShow.Text = btnShow.Tag.ToString();
                    // Label8.Text = btnShow.Tag.ToString();
                }
              

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
            try
            {
                CSearch cls = new CSearch();
                int[] ColumnWidth = new int[] { 100, 300 };
                string SearchSql = "";
                string Condition = "Where 1=1";

                FocusedControl = GetIndexFocusedControl();
                if (FocusedControl.Trim() == txtStoreID.Name)
                {
                    if (!MySession.GlobalAllowChangefrmPurchaseStoreID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, txtStoreID, lblStoreName, "StoreID", "رقم الحساب", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, txtStoreID, lblStoreName, "StoreID", "Account ID", MySession.GlobalBranchID);
                }
                else if (FocusedControl.Trim() == txtCostCenterID.Name)
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Search(txtCostCenterID, lblCostCenterName, "CostCenterID", "اسم مركز التكلفة", "رقم مركز التكلفة");
                    else
                        PrepareSearchQuery.Search(txtCostCenterID, lblCostCenterName, "CostCenterID", "Cost Center Name", "Cost Center ID");
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

                if (FocusedControl.Trim() == txtToItemNo.Name)
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, txtToItemNo, null, "Items", "رقـم الـمــادة", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, txtToItemNo, null, "Items", "Item ID", MySession.GlobalBranchID);
                }
                if (FocusedControl.Trim() == txtFromItemNo.Name)
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, txtFromItemNo, null, "Items", "رقـم الـمــادة", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, txtFromItemNo, null, "Items", "Item ID", MySession.GlobalBranchID);
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

        public void frmItemsList_Load(object sender, EventArgs e)
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



            DataTable dtPrice = new DataTable();
            dtPrice.Columns.Add("ID", System.Type.GetType("System.String"));
            dtPrice.Columns.Add("Name", System.Type.GetType("System.String"));

            dtPrice.Rows.Add("CostPrice", (UserInfo.Language == iLanguage.Arabic ? "سعر التكلفة" : "All Qty Balance"));
            dtPrice.Rows.Add("SalePrice", (UserInfo.Language == iLanguage.Arabic ? "سعر البيع" : "Qty Balance > 0"));
            dtPrice.Rows.Add("LastCostPrice", (UserInfo.Language == iLanguage.Arabic ? "أخر سعر شراء" : "Qty Balance = 0"));
            dtPrice.Rows.Add("LastSalePrice", (UserInfo.Language == iLanguage.Arabic ? "اخر سعر بيع " : "Qty Balance < 0"));
            dtPrice.Rows.Add("AverageCostPrice", (UserInfo.Language == iLanguage.Arabic ? "متوسط سعر التلكفة" : "Qty Balance < 0"));

            cmbPriceBy.Properties.DataSource = dtPrice.DefaultView;
            cmbPriceBy.Properties.DisplayMember = "Name";
            cmbPriceBy.Properties.ValueMember = "ID";
            cmbPriceBy.ItemIndex = 0;


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

        public void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                btnShow.Visible = false;
                lblStockValu.Text = "0";
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                Application.DoEvents();
                _sampleData.Clear();
               
                GetStocktaking();
              
            
                
                _sampleData.DefaultView.Sort = "BarCode ASC";
                _sampleData = _sampleData.DefaultView.ToTable();

          
                DataTable duplicateRows = _sampleData.Clone();

                Dictionary<string, List<DataRow>> barcodeRowsDict = new Dictionary<string, List<DataRow>>();
                foreach (DataRow row in _sampleData.Rows)
                {
                    string barcode = row["BarCode"].ToString();
                    if (barcodeRowsDict.ContainsKey(barcode))
                    {
                        barcodeRowsDict[barcode].Add(row);
                    }
                    else
                    {
                        List<DataRow> rowsList = new List<DataRow>();
                        rowsList.Add(row);
                        barcodeRowsDict.Add(barcode, rowsList);
                    }
                }
                foreach (KeyValuePair<string, List<DataRow>> kvp in barcodeRowsDict)
                {
                    foreach (DataRow row in kvp.Value)
                    {
                        DataRow newRow = duplicateRows.NewRow();
                        newRow.ItemArray = row.ItemArray;
                        duplicateRows.Rows.Add(newRow);
                    }
         
                    decimal sumQtyB = 0;
                    decimal sumQtyA = 0;
                    decimal sumQtyOut = 0;
                    decimal InPrice = 0;
                    decimal OutPrice = 0;
                    foreach (DataRow row in kvp.Value)
                    {
                        sumQtyB += Comon.cDec(row["QTYInB"]);
                        sumQtyA += Comon.cDec(row["QTYInA"]);
                        sumQtyOut += Comon.cDec(row["QTYOut"]);
                        InPrice += Comon.cDec(row["InPrice"]);
                        OutPrice += Comon.cDec(row["OutPrice"]);
                    }

                    // Add the total row
                    DataRow totalRow = duplicateRows.NewRow();
                    totalRow["BarCode"] = "اجمـــالي";
                    totalRow["QTYInB"] = sumQtyB;
                    totalRow["QTYInA"] = sumQtyA;
                    totalRow["QTYOut"] = sumQtyOut;
                    totalRow["InPrice"] = InPrice;
                    totalRow["OutPrice"] = OutPrice;
                    totalRow["FinalQTY"] = Comon.cDec(sumQtyB+sumQtyA)-Comon.cDec(sumQtyOut);
                    totalRow["FinalPrice"] = Comon.cDec(OutPrice) - Comon.cDec(InPrice);
                    duplicateRows.Rows.Add(totalRow);
                }

                gridControl1.DataSource = duplicateRows;




                btnShow.Visible = true;
                if (gridView1.RowCount > 0)
                {
                    txtGroupID.Enabled = false;
                    txtStoreID.Enabled = false;
                    txtToItemNo.Enabled = false;
                    txtFromItemNo.Enabled = false;
                    txtToDate.Enabled = false;
                    cmbTypeID.Enabled = false;
                    cmbQtyBalance.Enabled = false;
                    txtCostCenterID.Enabled = false;
                    cmbPriceBy.Enabled = false;
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
         
     
        private void GetStocktaking()
        {
            try
            {
                DataRow row;
                dt = Lip.SelectRecord(GetStrSQL());
                
                decimal QAIN = 0;
                decimal QAOUT = 0;
                decimal QBOUT = 0;
                decimal QBIN = 0;
                decimal FinalQTY = 0;
                int count = 0;
                decimal QAINTotal = 0;
                decimal QAOUTTotal = 0;
                decimal ANET = 0;
                decimal QTYB = 0;
                decimal QTYA = 0;
                string DocumentTypeName = "";
                if (strSQL != null || strSQL != "")
                {
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            row = _sampleData.NewRow();
                            row["Sn"] = _sampleData.Rows.Count + 1;
                            row["BarCode"] = dt.Rows[i]["BarCode"].ToString();
                            row["Sn"] = _sampleData.Rows.Count + 1;
                            row["ItemName"] = dt.Rows[i]["ItemName"].ToString();
                            row["TranseID"] = dt.Rows[i]["TranseID"].ToString();
                            row["StoreName"] = dt.Rows[i]["StoreName"].ToString();
                            row["GroupName"] = dt.Rows[i]["GroupName"].ToString();
                            row["DocumentTypeID"] = dt.Rows[i]["DocumentTypeID"].ToString();
                            row["MoveDate"] = Comon.ConvertSerialDateTo(dt.Rows[i]["MoveDate"].ToString());
                            DataRow[] rowtyp = dtFactoryOprationType.Select("ID =" + Comon.cInt(dt.Rows[i]["DocumentTypeID"]));
                            if (rowtyp.Length > 0)
                            {
                                DocumentTypeName = UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? rowtyp[0]["ArbCaptionType"].ToString() : rowtyp[0]["EngCaptionType"].ToString();
                                row["RecordType"] = DocumentTypeName;
                            }
                            else
                            {
                                if (Comon.cInt(dt.Rows[i]["DocumentTypeID"]) == 6)
                                    row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "فاتورة مبيعات سلعية " : "Sales Invoice");
                                else if (Comon.cInt(dt.Rows[i]["DocumentTypeID"]) == 7)
                                    row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "فاتورة مردود مبيعات  " : "Sales Invoice");
                                else if (Comon.cInt(dt.Rows[i]["DocumentTypeID"]) == 14)
                                    row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? " توريد مخزني- ذهب " : "InOn Store- Gold");
                                else if (Comon.cInt(dt.Rows[i]["DocumentTypeID"]) == 15)
                                    row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "بضاعة أول المدة" : "Goods Opening");
                                else if (Comon.cInt(dt.Rows[i]["DocumentTypeID"]) == 16)
                                    row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? " صرف مخزني- ذهب " : "OutOn Store- Gold");
                                else if (Comon.cInt(dt.Rows[i]["DocumentTypeID"]) == 17)
                                    row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? " توريد مخزني- مواد خام " : "InOn Store- Matirial");
                                else if (Comon.cInt(dt.Rows[i]["DocumentTypeID"]) == 18)
                                    row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? " صرف مخزني- مواد خام " : "OutOn Store- Matirial");
                                else if (Comon.cInt(dt.Rows[i]["DocumentTypeID"]) == 19)
                                    row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "تحويل  مخزني متعدد - ذهب" : "Transefer Stoer Multi Gold");
                                else if (Comon.cInt(dt.Rows[i]["DocumentTypeID"]) == 20)
                                    row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "تحويل  مخزني متعدد - مواد خام" : "Transefer Stoer Multi Matirial");
                                else if (Comon.cInt(dt.Rows[i]["DocumentTypeID"]) == 23)
                                    row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "فاتورة مشتريات " : "Purchase Invoice ");
                                else if (Comon.cInt(dt.Rows[i]["DocumentTypeID"]) == 24)
                                    row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "فاتورة مردود مشتريات " : "Purchase Return Invoice ");
                                else if (Comon.cInt(dt.Rows[i]["DocumentTypeID"]) == 39)
                                    row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "فاتورة مبيعات خدمية  " : "Sales Invoice Service");
                            }
                            if (Comon.cDec(dt.Rows[i]["MoveType"].ToString()) == 1)
                            {
                                if (Comon.cInt(dt.Rows[i]["DocumentTypeID"].ToString()) == 15)
                                {
                                    row["QTYInA"] = dt.Rows[i]["QTY"].ToString();
                                    row["SizeNameA"] = dt.Rows[i]["SizeName"].ToString();
                                    row["QTYInB"] = "...";
                                    row["SizeNameB"] = "...";
                                }
                                else
                                {

                                    row["QTYInB"] = dt.Rows[i]["QTY"].ToString();
                                    row["SizeNameB"] = dt.Rows[i]["SizeName"].ToString();
                                    row["QTYInA"] = "...";
                                    row["SizeNameA"] = "...";
                                }
                                row["QTYOut"] = "...";
                                row["SizeNameOut"] = "...";
                                row["InPrice"] = dt.Rows[i]["InPrice"].ToString();
                                row["OutPrice"] = "...";
                            }
                            else   if (Comon.cDec(dt.Rows[i]["MoveType"].ToString())== 2)
                            {
                                row["QTYOut"] = dt.Rows[i]["QTY"].ToString();
                                row["SizeNameOut"] = dt.Rows[i]["SizeName"].ToString();

                                row["OutPrice"] = dt.Rows[i]["OutPrice"].ToString();
                                row["InPrice"] = "...";
                                row["QTYInB"] = "...";
                                row["SizeNameB"] = "...";
                                row["QTYInA"] = "...";
                                row["SizeNameA"] = "...";
                            }

                             

                            //row["FinalQTY"] = dt.Rows[i]["FinalQTY"].ToString();
 
                            _sampleData.Rows.Add(row);
                            count += 1;

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
       public void ClearFilds()
        {
            _sampleData.Clear();
            gridControl1.RefreshDataSource();
            txtBarCode.Text = "";
            txtStoreID.Text = "";
            txtStoreID_Validating(null, null);
            txtToItemNo.Text = "";
            txtFromItemNo.Text = "";
            txtFromDate.Text = "";
            txtToDate.Text = "";
            txtCostCenterID.Text = "";
            lblCostCenterName.Text = "";

            txtFromDate.Enabled = true;
            txtToDate.Enabled = true;
            txtCostCenterID.Enabled = true;
            txtFromItemNo.Enabled = true;
            txtToItemNo.Enabled = true;
            txtGroupID.Enabled = true;
            cmbTypeID.Enabled = true;
            cmbPriceBy.Enabled = true;
            txtStoreID.Enabled = true;  
            gridControl1.DataSource = _sampleData;
        }
        protected override void DoAddFrom()
        {
            try
            {
                ClearFilds();
            }
            catch (Exception ex)
            {
                //WT.msgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        string GetStrSQL()
        {
            try
            {
                btnShow.Visible = false;
                Application.DoEvents();
                //  int storeID=0;
                strSQL = "";
                filter = "";
                filter = " BranchID =" +Comon.cInt(cmbBranchesID.EditValue)+" and ItemID >0  AND ";
                long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));
                long FromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text));
                if (FromDate != 0)
                 filter = filter + "   MoveDate>='" + FromDate + "' AND ";
                 
                if (ToDate != 0)
                 filter = filter + "   MoveDate<='" + ToDate + "' AND ";
                //if (txtStoreID.Text != string.Empty)
                //    storeID = Comon.cInt(txtStoreID.Text) ;
                if (txtFromItemNo.Text != string.Empty)
                    filter = " ItemID >=" + txtFromItemNo.Text + " AND ";
                if (txtToItemNo.Text != string.Empty)
                    filter = filter + " ItemID <=" + txtToItemNo.Text + " AND ";
                if (txtBarCode.Text != string.Empty)
                    filter = filter + " BarCode ='" + txtBarCode.Text + "' AND ";
                if (txtGroupID.Text != string.Empty)
                    filter = filter + " GroupID =" + txtGroupID.Text + " AND ";
                if (txtStoreID.Text != string.Empty)
                    filter = filter + " StoreID =" + txtStoreID.Text + " AND ";
                if (cmbTypeID.Text != string.Empty)
                    filter = filter + " TypeID =" + cmbTypeID.EditValue + " AND ";
                if(cmbBranchesID.Text!=string.Empty)
                    filter = filter + " BranchID =" + cmbBranchesID.EditValue + " AND ";

                filter += "  Posted=3  And ";
                filter = filter.Remove(filter.Length - 4, 4);

                strSQL = " SELECT *   from  Stc_ItemMoveEng_Find   where " + filter;

                if (UserInfo.Language == iLanguage.English)
                    strSQL = strSQL.Replace("ArbName", "EngName");

                return strSQL;
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            if (UserInfo.Language == iLanguage.Arabic)
                strSQL = strSQL.Replace("Sales_BarCodeForPurchaseInvoiceEng_Find", "Stc_ItemMoveEng_Find");
            else
                strSQL = strSQL.Replace("Sales_BarCodeForPurchaseInvoiceEng_Find", "Stc_ItemMoveEng_Find");
            return strSQL;
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
                rptFormName = "rptItemBalanceByStoresArb";
                if (UserInfo.Language == iLanguage.English)
                    rptFormName = ReportName + "Arb";
                XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);

                /********************** Master *****************************/
                rptForm.RequestParameters = false;

                rptForm.Parameters["StoreName"].Value = lblStoreName.Text.Trim().ToString();
                rptForm.Parameters["ByPrice"].Value = cmbPriceBy.Text.Trim().ToString();


                rptForm.Parameters["FromItem"].Value = txtFromItemNo.Text.Trim().ToString();
                rptForm.Parameters["ToItem"].Value = txtToItemNo.Text.Trim().ToString();
                rptForm.Parameters["ItemType"].Value = cmbTypeID.Text.Trim().ToString();
                rptForm.Parameters["Group"].Value = lblGroupID.Text.Trim().ToString();

                rptForm.Parameters["CostCenter"].Value = lblCostCenterName.Text.Trim().ToString();
                rptForm.Parameters["ToDate"].Value = txtToDate.Text.Trim().ToString();
                rptForm.Parameters["BalanceType"].Value = lblStoreName.Text.Trim().ToString();
                 


                for (int i = 0; i < rptForm.Parameters.Count; i++)
                    rptForm.Parameters[i].Visible = false;
                /********************** Details ****************************/
                var dataTable = new dsReports.rptItemBalanceByStoresDataTable();

                for (int i = 0; i <= gridView1.DataRowCount - 2; i++)
                {
                    var row = dataTable.NewRow();

                    row["Sn"] = i + 1;
                    row["Barcode"] = gridView1.GetRowCellValue(i, "BarCode").ToString();
                    row["ItemName"] = gridView1.GetRowCellValue(i, "ItemName").ToString();

                    row["GroupName"] = gridView1.GetRowCellValue(i, "GroupName").ToString();
                    row["TranseID"] = gridView1.GetRowCellValue(i, "TranseID").ToString();
                    row["RecordType"] = gridView1.GetRowCellValue(i, "RecordType").ToString();
                    row["StoreName"] = gridView1.GetRowCellValue(i, "StoreName").ToString();
                    row["QTYOut"] = gridView1.GetRowCellValue(i, "QTYOut").ToString();
                    row["QTYInB"] = gridView1.GetRowCellValue(i, "QTYInB").ToString();
                    row["SizeNameB"] = gridView1.GetRowCellValue(i, "SizeNameB").ToString();
                    row["QTYInA"] = gridView1.GetRowCellValue(i, "QTYInA").ToString();
                    row["SizeNameA"] = gridView1.GetRowCellValue(i, "SizeNameA").ToString();
                    row["SizeNameOut"] = gridView1.GetRowCellValue(i, "SizeNameOut").ToString();
                    row["MoveDate"] = gridView1.GetRowCellValue(i, "MoveDate").ToString();
                    row["FinalQTY"] = gridView1.GetRowCellValue(i, "FinalQTY").ToString();
                    row["InPrice"] = gridView1.GetRowCellValue(i, "InPrice").ToString();
                    row["OutPrice"] = gridView1.GetRowCellValue(i, "OutPrice").ToString();
                    row["FinalPrice"] = gridView1.GetRowCellValue(i, "FinalPrice").ToString();

                    dataTable.Rows.Add(row);
                }
                rptForm.DataSource = dataTable;
                rptForm.DataMember = "rptItemBalanceByStores";

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

        private void frmStocktaking_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3 || e.KeyCode == Keys.F4)
                Find();
            if (e.KeyCode == Keys.F5)
                DoPrint();
        }

        private void gridView_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                GridView view = sender as GridView;
                switch (view.GetFocusedRowCellValue("DocumentTypeID").ToString())
                {
                    case "10":
                        frmCashierPurchaseGold frm = new frmCashierPurchaseGold();
                        if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frm);
                            frm.Show();
                            frm.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                            frm.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("TranseID").ToString()) + 1, 8);
                        }
                        else
                            frm.Dispose();
                        break;
                    case "4":
                        frmCashierPurchaseDaimond frm1 = new frmCashierPurchaseDaimond();
                        if (Permissions.UserPermissionsFrom(frm1, frm1.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frm1);
                            frm1.Show();
                            frm1.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                            frm1.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("TranseID").ToString()) + 1, 8);
                        }
                        else
                            frm1.Dispose();
                        break;
                    case "ItemsOutOnBail":
                        frmItemsOutOnBail frm11 = new frmItemsOutOnBail();
                        if (Permissions.UserPermissionsFrom(frm11, frm11.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frm11);
                            frm11.Show();
                            frm11.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                            frm11.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("TranseID").ToString()) + 1, 8);
                        }
                        else
                            frm11.Dispose();
                        break;
                    case "ItemsInOnBail":
                        frmItemsInonBail frm12 = new frmItemsInonBail();
                        if (Permissions.UserPermissionsFrom(frm12, frm12.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frm12);
                            frm12.Show();
                            frm12.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                            frm12.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("TranseID").ToString()) + 1, 8);
                        }
                        else
                            frm12.Dispose();
                        break;
                    case "GoodsOpening":
                        frmGoodsOpeningOld frm112 = new frmGoodsOpeningOld();
                        if (Permissions.UserPermissionsFrom(frm112, frm112.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frm112);
                            frm112.Show();
                            frm112.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                            frm112.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("TranseID").ToString()) + 1, 8);
                        }
                        else
                            frm112.Dispose();
                        break;
                    
                    case "ItemsDismantling":
                        frmItemsDismantling frm10 = new frmItemsDismantling();
                        if (Permissions.UserPermissionsFrom(frm10, frm10.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frm10);
                            frm10.Show();
                            frm10.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("TranseID").ToString()) + 1, 8);
                        }
                        else
                            frm10.Dispose();
                        break;

                    case "7":
                        frmSalesInvoiceReturn frm2 = new frmSalesInvoiceReturn();
                        if (Permissions.UserPermissionsFrom(frm2, frm2.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frm2);
                            frm2.Show();
                            frm2.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                            frm2.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("TranseID").ToString()) + 1, 8);
                        }
                        else
                            frm2.Dispose();
                        break;

                    case "9":
                        frmCashierSalesGold frm3 = new frmCashierSalesGold();
                        if (Permissions.UserPermissionsFrom(frm3, frm3.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frm3);
                            frm3.Show();
                            frm3.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                            frm3.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("TranseID").ToString()) + 1, 8);
                        }
                        else
                            frm3.Dispose();
                        break;
                    case "6":
                        frmCashierSalesAlmas frm30 = new frmCashierSalesAlmas();
                        if (Permissions.UserPermissionsFrom(frm30, frm30.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frm30);
                            frm30.Show();
                            frm30.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                            frm30.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("TranseID").ToString()) + 1, 8);
                        }
                        else
                            frm30.Dispose();
                        break;
                    case "5":
                        frmCashierPurchaseReturnDaimond frm4 = new frmCashierPurchaseReturnDaimond();
                        if (Permissions.UserPermissionsFrom(frm4, frm4.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frm4);
                            frm4.Show();
                            frm4.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                            frm4.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("TranseID").ToString()) + 1, 8);
                        }
                        else
                            frm4.Dispose();
                        break;
                    case "11":
                        frmCashierPurchaseReturnGold frm15 = new frmCashierPurchaseReturnGold();
                        if (Permissions.UserPermissionsFrom(frm15, frm15.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frm15);
                            frm15.Show();
                            frm15.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                            frm15.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("TranseID").ToString()) + 1, 8);
                        }
                        else
                            frm15.Dispose();
                        break;
                    case "13":
                        frmCashierPurchaseSaveDaimond frm201 = new frmCashierPurchaseSaveDaimond();
                        if (Permissions.UserPermissionsFrom(frm201, frm201.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frm201);
                            frm201.Show();
                            frm201.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                            frm201.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("TranseID").ToString()) + 1, 8);
                        }
                        else
                            frm201.Dispose();
                        break;
                    case "14":
                        frmGoldInOnBail frm2101 = new frmGoldInOnBail();
                        if (Permissions.UserPermissionsFrom(frm2101, frm2101.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frm2101);
                            frm2101.Show();
                            frm2101.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                            frm2101.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("TranseID").ToString()) + 1, 8);
                        }
                        else
                            frm2101.Dispose();
                        break;
                    case "15":
                        frmGoodsOpening frmGoodsOp = new frmGoodsOpening();
                        if (Permissions.UserPermissionsFrom(frmGoodsOp, frmGoodsOp.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frmGoodsOp);

                            frmGoodsOp.Show();
                            frmGoodsOp.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                            frmGoodsOp.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("TranseID").ToString()) + 1, 8);
                        }
                        else
                            frmGoodsOp.Dispose();
                        break;
                    case "16":
                        frmGoldOutOnBail frmGolOut = new frmGoldOutOnBail();
                        if (Permissions.UserPermissionsFrom(frmGolOut, frmGolOut.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frmGolOut);

                            frmGolOut.Show();
                            frmGolOut.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                            frmGolOut.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("TranseID").ToString()) + 1, 8);
                        }
                        else
                            frmGolOut.Dispose();
                        break;
                    case "17":
                        frmMatirialInOnBail frmMatiralIn = new frmMatirialInOnBail();
                        if (Permissions.UserPermissionsFrom(frmMatiralIn, frmMatiralIn.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frmMatiralIn);

                            frmMatiralIn.Show();
                            frmMatiralIn.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                            frmMatiralIn.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("TranseID").ToString()) + 1, 8);
                        }
                        else
                            frmMatiralIn.Dispose();
                        break;
                    case "18":
                        frmMatirialOutOnBail frmMatiralOut = new frmMatirialOutOnBail();
                        if (Permissions.UserPermissionsFrom(frmMatiralOut, frmMatiralOut.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frmMatiralOut);

                            frmMatiralOut.Show();
                            frmMatiralOut.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                            frmMatiralOut.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("TranseID").ToString()) + 1, 8);
                        }
                        else
                            frmMatiralOut.Dispose();
                        break;
                    case "19":
                        frmTransferMultipleStoresGold frmGoldMulti = new frmTransferMultipleStoresGold();
                        if (Permissions.UserPermissionsFrom(frmGoldMulti, frmGoldMulti.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frmGoldMulti);

                            frmGoldMulti.Show();
                            frmGoldMulti.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                            frmGoldMulti.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("TranseID").ToString()) + 1, 8);
                        }
                        else
                            frmGoldMulti.Dispose();
                        break;
                    case "20":
                        frmTransferMultipleStoreMatirial frmMatiralMulti = new frmTransferMultipleStoreMatirial();
                        if (Permissions.UserPermissionsFrom(frmMatiralMulti, frmMatiralMulti.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frmMatiralMulti);

                            frmMatiralMulti.Show();
                            frmMatiralMulti.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                            frmMatiralMulti.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("TranseID").ToString()) + 1, 8);
                        }
                        else
                            frmMatiralMulti.Dispose();
                        break;
                    case "23":
                        frmCashierPurchaseServicesEqv frmGoldMatirialInvoice = new frmCashierPurchaseServicesEqv();
                        if (Permissions.UserPermissionsFrom(frmGoldMatirialInvoice, frmGoldMatirialInvoice.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frmGoldMatirialInvoice);

                            frmGoldMatirialInvoice.Show();
                            frmGoldMatirialInvoice.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                            frmGoldMatirialInvoice.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("TranseID").ToString()) + 1, 8);
                        }
                        else
                            frmGoldMatirialInvoice.Dispose();
                        break;
                    case "24":
                        frmCashierPurchaseReturnMatirial frmMatiralInvReturn = new frmCashierPurchaseReturnMatirial();
                        if (Permissions.UserPermissionsFrom(frmMatiralInvReturn, frmMatiralInvReturn.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frmMatiralInvReturn);

                            frmMatiralInvReturn.Show();
                            frmMatiralInvReturn.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                            frmMatiralInvReturn.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("TranseID").ToString()) + 1, 8);
                        }
                        else
                            frmMatiralInvReturn.Dispose();
                        break;
                    case "39":
                        frmCashierSales frmSales = new frmCashierSales();
                        if (Permissions.UserPermissionsFrom(frmSales, frmSales.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frmSales);

                            frmSales.Show();
                            //frmSales.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                            frmSales.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("TranseID").ToString()) + 1, 8);
                        }
                        else
                            frmSales.Dispose();

                        break;

                }
            }
            catch { }
        }


        private void OpenWindow(BaseForm frm)
        {

            try
            {
                if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
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


    

        private void cmbPriceBy_EditValueChanged(object sender, EventArgs e)
        {
            label1.Text = "الاجمالي بحسب " + cmbPriceBy.Text + ":";
        }

        private void labelControl5_Click(object sender, EventArgs e)
        {

        }

        private void ribbonControl1_Click(object sender, EventArgs e)
        {

        }

        protected override void DoNew()
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
                txtToDate.Enabled = true;
                cmbTypeID.Enabled = true;
                cmbQtyBalance.Enabled = true;
                txtCostCenterID.Enabled = true;
                cmbTypeID.ItemIndex = -1;
                cmbPriceBy.ItemIndex = 0;
                cmbPriceBy.Enabled = true;
               

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
            try
            {
                if (txtToDate.EditValue == null)
                {
                    MessageBox.Show("يجب اختيار التايخ");
                    return;
                }

                btnShow_Click(null, null);
                if (_sampleData.Rows.Count > 0)
                {

                    decimal StockValue = Comon.ConvertToDecimalPrice(lblStockValu.Text);
                    string ToDate = Comon.ConvertDateToSerial(txtToDate.Text).ToString();
                    strSQL = "Update Stc_GoodAmount Set Amount=" + StockValue + " , DateClose=" + ToDate + " , TypePrice= " + cmbPriceBy.ItemIndex + "   Where EngName='GoodLast' ";
                    Lip.ExecututeSQL(strSQL);

                }
            }
            catch { }

        }


        private void gridView1_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.RowHandle >= 0)
            {
                if (View.GetRowCellDisplayText(e.RowHandle, View.Columns["DocumentTypeID"]).ToString() == "")
                {
                    if (Comon.cInt(View.GetRowCellDisplayText(e.RowHandle, View.Columns["DocumentTypeID"]).ToString()) > 0)
                    {
                        e.Appearance.BackColor = Color.LightYellow;
                        e.Appearance.BackColor2 = Color.LightYellow;
                    }
                    else
                    {
                        e.Appearance.BackColor = Color.LightBlue;
                        e.Appearance.BackColor2 = Color.LightBlue;
                    }
                    e.HighPriority = true;
                }
            }
        }

    }
}
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

namespace Edex.StockObjects.Reports
{
    public partial class frmStocktakingByStoresOld : Edex.GeneralObjects.GeneralForms.BaseForm
    {
        private string strSQL = "";
        private string filter = "";
        DataTable dt = new DataTable();
        DataTable _nativeData = new DataTable ();
        public DataTable _sampleData = new DataTable();
        string FocusedControl;
        private string PrimaryName;

        private string ItemName;
        private string SizeName;
        private string GroupName;

        public frmStocktakingByStoresOld()
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
                _sampleData.Columns.Add(new DataColumn("Barcode", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("TheDate", typeof(string)));

                _sampleData.Columns.Add(new DataColumn("ItemID", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("SizeID", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("Total1", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("ItemName", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("SizeName", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("Qty", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("QtyVisical", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("Price", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("Total", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("SalePrice", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("PackingQty", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("ExpiryDate", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("GroupName", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("GroupID", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("StoreName", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("QBIN", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("QBOUT", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("BNET", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("QAIN", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("QAOUT", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("ANET", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("FinalQTY", typeof(string)));
              






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



                FillCombo.FillComboBox(cmbTypeID, "Stc_ItemTypes", "TypeID", strSQL, "", "1=1");
                this.txtToDate.Properties.Mask.UseMaskAsDisplayFormat = true;
                this.txtToDate.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
                this.txtToDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                this.txtToDate.Properties.EditFormat.FormatString = "dd/MM/yyyy";
                this.txtToDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                this.txtToDate.Properties.Mask.EditMask = "dd/MM/yyyy";

                this.txtStoreID.Validating += new System.ComponentModel.CancelEventHandler(this.txtStoreID_Validating);
                this.txtCostCenterID.Validating += new System.ComponentModel.CancelEventHandler(this.txtCostCenterID_Validating);
                this.txtGroupID.Validating += new System.ComponentModel.CancelEventHandler(this.txtGroupID_Validating);
                PrimaryName = "ArbName";

                if (UserInfo.Language == iLanguage.English)
                {
                    dgvColBarcode.Caption = "Bar Code ";
                    dgvColItemID.Caption = "Item No ";
                    dgvColItemName.Caption = "Item Name ";
                    dgvColPrice.Caption = "Price  ";
                    dgvColSizeName.Caption = "Size Name ";
                    dgvColQty.Caption = "Quantity";
                    dgvColQtyVisical.Caption = "Quantity Visical";
                    dgvColTotal.Caption = "Total";
                    btnShow.Text = btnShow.Tag.ToString();
                    // Label8.Text = btnShow.Tag.ToString();
                }
                GetAccountsDeclaration();

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


            if (FocusedControl.Trim() == txtStoreID.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Search(txtStoreID, lblStoreName, "StoreID", "اسـم الـمـســتـودع", "رقم الـمـســتـودع");
                else
                    PrepareSearchQuery.Search(txtStoreID, lblStoreName, "StoreID", "Store ID", "Store Name");
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
                strSQL = "SELECT   " + (UserInfo.Language == iLanguage.Arabic ? "ArbName" : "EngName") + "   as GroupName FROM dbo.Stc_ItemsGroups WHERE GroupID =" + Comon.cInt(txtGroupID.Text) + " And Cancel =0 And  BranchID =" + UserInfo.BRANCHID;
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
                strSQL = "SELECT   " + (UserInfo.Language == iLanguage.Arabic ? "ArbName" : "EngName") + "    as StoreName FROM Stc_Stores WHERE StoreID =" + Comon.cInt(txtStoreID.Text) + " And Cancel =0 And  BranchID =" + UserInfo.BRANCHID;
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
                strSQL = "SELECT   " + (UserInfo.Language == iLanguage.Arabic ? "ArbName" : "EngName") + "  as CostCenterName FROM Acc_CostCenters WHERE CostCenterID =" + Comon.cInt(txtCostCenterID.Text) + " And Cancel =0 And  BranchID =" + UserInfo.BRANCHID;
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
                string str = " select storeID , ArbName from Stc_Stores where Cancel =0";
                var dr = Lip.SelectRecord(str);
                if (dr.Rows.Count < 1)
                    return;
                for (int i = 0; i <= dr.Rows.Count-1;++i )
                    GetStocktaking(Comon.cInt(dr.Rows[i][0].ToString()), dr.Rows[i][1].ToString());
                //SortData();
               Totals();

                gridControl1.DataSource = _sampleData;

                if (gridView1.RowCount > 0)
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
        private void SortData()
        {

            try
            {
                DataTable dt = new DataTable(); DataRow row;
                dt = _sampleData.Copy();
                DataView view = dt.DefaultView;
                view.Sort = "ItemID ASC";
                 if( cmbQtyBalance.ItemIndex  == 1 )//' المواد التي رصيدها اكبر من الصفر
                     view.RowFilter = "Qty > 0";

                 if (cmbQtyBalance.ItemIndex == 2) // ' المواد التي رصيدها يساوي الصفر
                     view.RowFilter = "Qty = 0";

                 if (cmbQtyBalance.ItemIndex == 3) // ' المواد التي رصيدها يساوي الصفر
                     view.RowFilter = "Qty < 0";
            //    view.RowFilter = "dgvColQTY < 0"
            //End If
                _sampleData.Rows.Clear();
             //  DataRow row;

                for (int i = 0; i <= view.Count - 1; i++)
                {
                     

                    row = _sampleData.NewRow();
                    row["Sn"] = _sampleData.Rows.Count + 1;
                    row["Barcode"] = view[i]["Barcode"].ToString();
                    row["ItemID"] = Comon.cLong(view[i]["ItemID"].ToString());
                    row["SizeID"] = Comon.cLong(view[i]["SizeID"].ToString());

                    row["ItemName"] = view[i]["ItemName"].ToString();
                    row["SizeName"] = view[i]["SizeName"].ToString();
                    row["Qty"] = view[i]["Qty"].ToString();
                    row["SalePrice"] = view[i]["SalePrice"].ToString();
                    row["StoreName"] = view[i]["StoreName"].ToString();
                    row["QBIN"] = view[i]["QBIN"].ToString();
                    row["QBOUT"] = view[i]["QBOUT"].ToString();
                    row["BNET"] = view[i]["BNET"].ToString();
                    row["QAIN"] = view[i]["QAIN"].ToString();
                    row["QAOUT"] = view[i]["QAOUT"].ToString();
                    row["ANET"] = view[i]["ANET"].ToString();
                    row["FinalQTY"] = view[i]["FinalQTY"].ToString();


                    row["PackingQty"] = view[i]["PackingQty"].ToString();
                    row["ExpiryDate"] = view[i]["ExpiryDate"].ToString();
                    row["GroupName"] = view[i]["GroupName"].ToString();
                    row["GroupID"] = view[i]["GroupID"].ToString();
                    row["QtyVisical"] = "0";
                    row["Price"] = Comon.ConvertToDecimalPrice(view[i]["Price"]);
                    row["Total"] = Comon.ConvertToDecimalPrice(row["Qty"]) * Comon.ConvertToDecimalPrice(row["Price"]);
                    _sampleData.Rows.Add(row);



                }

            

            }
            catch { }
        }
        private void ProcessBalance(ref DataRow row, int FacilityID, int BranchID, int StoreID, int MoveDate = 0)
        {
            decimal sum = 0;
            try
            {
                DataTable inPrice;
                string BarCode = row["BarCode"].ToString();


                // حسب التاريخ


                #region GoodInput
                DataTable dtQty = new DataTable();
                dtQty = Lip.SelectRecord(GoodItems(BarCode));

                if (strSQL != null || strSQL != "")
                {
                    if (dtQty.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dtQty.Rows.Count - 1; i++)
                        {
                            row["Qty"] = Comon.cDec(row["Qty"]) + Comon.cDec(dtQty.Rows[i]["Qty"].ToString());
                            //row["DiamondWeight"] = Comon.cDec(row["DiamondWeight"]) + Comon.cDec(dtQty.Rows[i]["WeightDiamond"].ToString());
                            //row["BagetWeight"] = Comon.cDec(row["BagetWeight"]) + Comon.cDec(dtQty.Rows[i]["WeightBaget"].ToString());
                            //row["StoneWeight"] = Comon.cDec(row["StoneWeight"]) + Comon.cDec(dtQty.Rows[i]["WeightStone"].ToString());
                            //row["StartQty"] = Comon.cDec(dtQty.Rows[i]["Qty"].ToString());

                        }
                    }

                }

                #endregion

                #region PurchaseInvoice
                dtQty = Lip.SelectRecord(PurchaseInvoice(BarCode));
                if (strSQL != null || strSQL != "")
                {
                    if (dtQty.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dtQty.Rows.Count - 1; i++)
                        {
                            row["Qty"] = Comon.cDec(row["Qty"]) + Comon.cDec(dtQty.Rows[i]["Qty"].ToString());
                            //row["DiamondWeight"] = Comon.cDec(row["DiamondWeight"]) + Comon.cDec(dtQty.Rows[i]["WeightDiamond"].ToString());
                            //row["BagetWeight"] = Comon.cDec(row["BagetWeight"]) + Comon.cDec(dtQty.Rows[i]["WeightBaget"].ToString());
                            //row["StoneWeight"] = Comon.cDec(row["StoneWeight"]) + Comon.cDec(dtQty.Rows[i]["WeightStone"].ToString());
                        }
                    }

                }
                #endregion
                #region PurchaseInvoiceReturn
                dtQty.Rows.Clear();

                dtQty = Lip.SelectRecord(PurchaseInvoiceReturn(BarCode));
                if (strSQL != null || strSQL != "")
                {
                    if (dtQty.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dtQty.Rows.Count - 1; i++)
                        {
                            row["Qty"] = Comon.cDec(row["Qty"]) - Comon.cDec(dtQty.Rows[i]["Qty"].ToString());
                            //row["DiamondWeight"] = Comon.cDec(row["DiamondWeight"]) - Comon.cDec(dtQty.Rows[i]["WeightDiamond"].ToString());
                            //row["BagetWeight"] = Comon.cDec(row["BagetWeight"]) - Comon.cDec(dtQty.Rows[i]["WeightBaget"].ToString());
                            //row["StoneWeight"] = Comon.cDec(row["StoneWeight"]) - Comon.cDec(dtQty.Rows[i]["WeightStone"].ToString());

                        }
                    }

                }

                #endregion




                #region SalesInvoice
                dtQty.Rows.Clear();

                dtQty = Lip.SelectRecord(SalesInvoice(BarCode));
                if (strSQL != null || strSQL != "")
                {
                    if (dtQty.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dtQty.Rows.Count - 1; i++)
                        {
                            row["Qty"] = Comon.cDec(row["Qty"]) - Comon.cDec(dtQty.Rows[i]["Qty"].ToString());
                            //row["DiamondWeight"] = Comon.cDec(row["DiamondWeight"]) - Comon.cDec(dtQty.Rows[i]["WeightDiamond"].ToString());
                            //row["BagetWeight"] = Comon.cDec(row["BagetWeight"]) - Comon.cDec(dtQty.Rows[i]["WeightBaget"].ToString());
                            //row["StoneWeight"] = Comon.cDec(row["StoneWeight"]) - Comon.cDec(dtQty.Rows[i]["WeightStone"].ToString());

                        }
                    }

                }

                #endregion
                #region SalesInvoiceReturn

                dtQty.Rows.Clear();
                dtQty = Lip.SelectRecord(SalesInvoiceReturn(BarCode));
                if (strSQL != null || strSQL != "")
                {
                    if (dtQty.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dtQty.Rows.Count - 1; i++)
                        {
                            row["Qty"] = Comon.cDec(row["Qty"]) + Comon.cDec(dtQty.Rows[i]["Qty"].ToString());
                            //row["DiamondWeight"] = Comon.cDec(row["DiamondWeight"]) + Comon.cDec(dtQty.Rows[i]["WeightDiamond"].ToString());
                            //row["BagetWeight"] = Comon.cDec(row["BagetWeight"]) + Comon.cDec(dtQty.Rows[i]["WeightBaget"].ToString());
                            //row["StoneWeight"] = Comon.cDec(row["StoneWeight"]) + Comon.cDec(dtQty.Rows[i]["WeightStone"].ToString());

                        }
                    }
                }

                #endregion

                #region ItemsOutOnBail
                dtQty.Rows.Clear();

                dtQty = Lip.SelectRecord(ItemsOutOnBail(BarCode));
                if (strSQL != null || strSQL != "")
                {
                    if (dtQty.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dtQty.Rows.Count - 1; i++)
                        {
                            row["Qty"] = Comon.cDec(row["Qty"]) - Comon.cDec(dtQty.Rows[i]["Qty"].ToString());
                            //row["DiamondWeight"] = Comon.cDec(row["DiamondWeight"]) - Comon.cDec(dtQty.Rows[i]["WeightDiamond"].ToString());
                            //row["BagetWeight"] = Comon.cDec(row["BagetWeight"]) - Comon.cDec(dtQty.Rows[i]["WeightBaget"].ToString());
                            //row["StoneWeight"] = Comon.cDec(row["StoneWeight"]) - Comon.cDec(dtQty.Rows[i]["WeightStone"].ToString());

                        }
                    }

                }

                #endregion
                #region ItemsInOnBail
                dtQty.Rows.Clear();
                dtQty = Lip.SelectRecord(ItemsInOnBail(BarCode));
                if (strSQL != null || strSQL != "")
                {
                    if (dtQty.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dtQty.Rows.Count - 1; i++)
                        {
                            row["Qty"] = Comon.cDec(row["Qty"]) + Comon.cDec(dtQty.Rows[i]["Qty"].ToString());
                            //row["DiamondWeight"] = Comon.cDec(row["DiamondWeight"]) + Comon.cDec(dtQty.Rows[i]["WeightDiamond"].ToString());
                            //row["BagetWeight"] = Comon.cDec(row["BagetWeight"]) + Comon.cDec(dtQty.Rows[i]["WeightBaget"].ToString());
                            //row["StoneWeight"] = Comon.cDec(row["StoneWeight"]) + Comon.cDec(dtQty.Rows[i]["WeightStone"].ToString());
                        }
                    }
                }


                #endregion


                #region ItemsTransferTo
                dtQty.Rows.Clear();

                dtQty = Lip.SelectRecord(ItemsTransferTo(BarCode));
                if (strSQL != null || strSQL != "")
                {
                    if (dtQty.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dtQty.Rows.Count - 1; i++)
                        {
                            row["Qty"] = Comon.cDec(row["Qty"]) - Comon.cDec(dtQty.Rows[i]["Qty"].ToString());
                            //row["DiamondWeight"] = Comon.cDec(row["DiamondWeight"]) + Comon.cDec(dtQty.Rows[i]["WeightDiamond"].ToString());
                            //row["BagetWeight"] = Comon.cDec(row["BagetWeight"]) + Comon.cDec(dtQty.Rows[i]["WeightBaget"].ToString());
                            //row["StoneWeight"] = Comon.cDec(row["StoneWeight"]) + Comon.cDec(dtQty.Rows[i]["WeightStone"].ToString());

                        }
                    }

                }

                #endregion
                #region ItemsTransferFrom
                dtQty.Rows.Clear();

                dtQty = Lip.SelectRecord(ItemsTransferFrom(BarCode));
                if (strSQL != null || strSQL != "")
                {
                    if (dtQty.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dtQty.Rows.Count - 1; i++)
                        {
                            row["Qty"] = Comon.cDec(row["Qty"]) + Comon.cDec(dtQty.Rows[i]["Qty"].ToString());
                            //row["DiamondWeight"] = Comon.cDec(row["DiamondWeight"]) - Comon.cDec(dtQty.Rows[i]["WeightDiamond"].ToString());
                            //row["BagetWeight"] = Comon.cDec(row["BagetWeight"]) - Comon.cDec(dtQty.Rows[i]["WeightBaget"].ToString());
                            //row["StoneWeight"] = Comon.cDec(row["StoneWeight"]) - Comon.cDec(dtQty.Rows[i]["WeightStone"].ToString());

                        }
                    }

                }

                #endregion



                #region ItemsDismantlingFrom
                dtQty.Rows.Clear();
                dtQty = Lip.SelectRecord(ItemsDismantlingFrom(BarCode));
                if (strSQL != null || strSQL != "")
                {
                    if (dtQty.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dtQty.Rows.Count - 1; i++)
                        {

                            row["Qty"] = Comon.cDec(row["Qty"]) + Comon.cDec(dtQty.Rows[i]["Qty"].ToString());


                        }
                    }

                }

                #endregion
                #region ItemsDismantlingTo
                dtQty.Rows.Clear();
                dtQty = Lip.SelectRecord(ItemsDismantlingTo(BarCode));
                 if (strSQL != null || strSQL != "")
                {
                    if (dtQty.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dtQty.Rows.Count - 1; i++)
                        {

                            row["Qty"] = Comon.cDec(row["Qty"]) -Comon.cDec(dtQty.Rows[i]["Qty"].ToString());


                        }
                    }

                }

              

                #endregion







            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
          
        }
        private void GetStocktaking(int StoreID,string StoreName)
        {
            try
            {
                DataRow row;
                dt = Lip.SelectRecord(GetStrSQL(StoreID));
                decimal BNET = 0;
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
                if (strSQL != null || strSQL != "")
                {
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            QBIN = 0;
                            QBOUT = 0;
                            QAIN = 0;
                            QAOUT = 0;
                            if (Comon.cLong(dt.Rows[i]["MoveDate"]) > Comon.ConvertDateToSerial(txtFromDate.Text) && Comon.cLong(dt.Rows[i]["MoveDate"]) < Comon.ConvertDateToSerial(txtToDate.Text))
                            {
                                if (Comon.cInt(dt.Rows[i]["MoveType"]) == 1)
                                {
                                    QBIN = Comon.cDec(dt.Rows[i]["Qty"]);
                                    QBOUT = 0;
                                }
                                else
                                {
                                    QBIN = 0;
                                    QBOUT = Comon.cDec(dt.Rows[i]["Qty"]);
                                }
                            }

                            if (Comon.cLong(dt.Rows[i]["MoveDate"]) >= Comon.ConvertDateToSerial(txtFromDate.Text) && Comon.cLong(dt.Rows[i]["MoveDate"]) <= Comon.ConvertDateToSerial(txtToDate.Text))
                            {
                                if (Comon.cInt(dt.Rows[i]["MoveType"]) ==1)
                                {

                                    QAIN = Comon.cDec(dt.Rows[i]["Qty"]);
                                    QAOUT = 0;
                                }
                                else
                                {
                                    QAIN = 0;
                                    QAOUT = Comon.cDec(dt.Rows[i]["Qty"]);
                                }
                            }

                            if (QAIN == 0 && QAOUT == 0 && QBIN == 0 && QBOUT == 0)
                                continue;

                            row = _sampleData.NewRow();
                            row["Sn"] = _sampleData.Rows.Count + 1;
                            row["Barcode"] =Lip.GetValue("Select ArbName From Stc_SizingUnits Where SizeID=" + dt.Rows[i]["SizeID"].ToString()) ;
                            row["ItemID"] = Comon.cLong(dt.Rows[i]["MoveID"].ToString());
                            row["SizeID"] = Comon.cLong(dt.Rows[i]["SizeID"].ToString());
                            row["Total1"] = "0";
                            row["ItemName"] =GetDocType( dt.Rows[i]["DocumentTypeID"].ToString());
                            row["SizeName"] = dt.Rows[i]["MoveType"].ToString();
                            row["TheDate"] = Comon.ConvertSerialDateTo( dt.Rows[i]["MoveDate"].ToString());
                            row["StoreName"] = StoreName;
                            // string[] QtyBefore = dt.Rows[i]["Qty1"].ToString().Split(',');  
                            row["Qty"] = QBIN;
                            row["QBIN"] = QBIN;
                            row["QBOUT"] = QBOUT;
                            row["BNET"] = Comon.ConvertToDecimalQty(row["QBIN"]) - Comon.ConvertToDecimalQty(row["QBOUT"]);
                            row["QAIN"] = QAIN;
                            row["QAOUT"] = QAOUT;
                            row["ANET"] = Comon.ConvertToDecimalQty(row["QAIN"]) - Comon.ConvertToDecimalQty(row["QAOUT"]);
                            row["FinalQTY"] = Comon.ConvertToDecimalQty(row["ANET"]) + Comon.ConvertToDecimalQty(row["BNET"]);
                            //ProcessBalance(ref row, UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(txtStoreID.Text), Comon.ConvertDateToSerial(txtToDate.Text));
                            BNET += Comon.ConvertToDecimalQty(row["BNET"]);
                            QAINTotal += Comon.ConvertToDecimalQty(row["QAIN"]);
                            QAOUTTotal += Comon.ConvertToDecimalQty(row["QAOUT"]);
                            ANET += Comon.ConvertToDecimalQty(row["ANET"]);
                            FinalQTY += Comon.ConvertToDecimalQty(row["FinalQTY"]);
                            row["PackingQty"] = dt.Rows[i]["PackingQty"].ToString();
                            row["ExpiryDate"] = dt.Rows[i]["ExpiryDate"].ToString();
                            row["GroupName"] = lblGroupID.Text;
                            row["GroupID"] = txtGroupID.Text;
                            row["SalePrice"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["InPrice"].ToString());
                            row["Price"] = dt.Rows[i]["OutPrice"].ToString();



                            row["QtyVisical"] = "";
                            row["Total"] = Comon.ConvertToDecimalPrice(row["FinalQTY"]) * Comon.ConvertToDecimalPrice(row["Price"]);
                            _sampleData.Rows.Add(row);
                            count += 1;

                        }


                        row = _sampleData.NewRow();
                        row["Barcode"] = count;
                        row["StoreName"] = StoreName;

                        row["BNET"] = BNET;
                        row["QAIN"] = QAINTotal;
                        row["QAOUT"] = QAOUTTotal;
                        row["ANET"] = ANET;
                        row["Sn"] = count;
                        row["FinalQTY"] = FinalQTY;
                        _sampleData.Rows.Add(row);
                    }
                }

            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);

            }
        }

        private string GetDocType(string v)
        {
            string value = "";
            switch (v)
            {
                case "1":
                    value = "فاتورة مشتريات";
                    break;

                case "2":
                    value = "فاتورة مردودات مشتريات";
                    break;

                case "3":
                    value = "فاتورة مبيعات";
                    break;

                case "4":
                    value = "مردود مبيعات";
                    break;

                default:
                    value = " ---------- "; 
                    break;
            }
            return value;
        }

        string GetStrSQL(int StoreID)
        {
            try
            {
                btnShow.Visible = false;
                Application.DoEvents();
                //  int storeID=0;
                strSQL = "";
                filter = "";
                filter = " ItemID >0  AND ";
                long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));
                long FromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text));

                if (StoreID > 0)
                    filter = " StoreID =" + StoreID + " AND ";

                if (txtFromItemNo.Text != string.Empty)
                    filter = " ItemID >=" + txtFromItemNo.Text + " AND ";

                if (txtToItemNo.Text != string.Empty)
                    filter = filter + " ItemID <=" + txtToItemNo.Text + " AND ";

                if (txtBarCode.Text != string.Empty)
                    filter = filter + " BarCode ='" + txtBarCode.Text + "' AND ";


                filter = filter.Remove(filter.Length - 4, 4);


                strSQL = @"SELECT   SUM(Qty + Bones) AS Qty, MoveID, MoveDate, MoveType, DocumentTypeID, TranseID, ID, ItemID, SizeID, Barcode, CostCenterID, InPrice, BranchID, FacilityID, ExpiryDate, Cancel, StoreID, OutPrice, PackingQty
                FROM    stc_ItemsMoviing
                WHERE  (FacilityID = " + UserInfo.FacilityID + ")  AND (BranchID = " + UserInfo.BRANCHID + ") And " + filter +
                "GROUP BY MoveID, MoveDate, MoveType, DocumentTypeID, TranseID, ID, ItemID, SizeID, Barcode, CostCenterID, InPrice, BranchID, FacilityID, ExpiryDate, Cancel, StoreID, OutPrice , PackingQty";


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
                strSQL = strSQL.Replace("Sales_BarCodeForPurchaseInvoiceEng_Find", "Sales_BarCodeForPurchaseInvoiceArb_FindStockByStore");
            else
                strSQL = strSQL.Replace("Sales_BarCodeForPurchaseInvoiceEng_Find", "Sales_BarCodeForPurchaseInvoiceEng_FindStockByStore");
            return strSQL;

        }


        decimal GetItemQtyIn(int FacilityID, int BranchID, int StoreID, int ItemID, int SizeID, int MoveDate = 0)
        {
            decimal ItemQtyOut = 0;
            decimal ItemQtyIn = 0;
            try
            {

                DataTable dtQtyIn;
                DataTable dtQtyOut;
                strSQL = " SELECT  SUM(Qty + Bones) AS Qty  FROM   Stc_ItemsMoviing "
                + "Where MoveType =1  AND (FacilityID = " + FacilityID + " )  AND (ItemID = " + ItemID + " )   AND (SizeID = " + SizeID + " ) AND ";

                if (StoreID > 0)
                    strSQL = strSQL + "   (StoreID = " + StoreID + ") AND ";
                if (BranchID > 0)
                    strSQL = strSQL + "   (BranchID = " + BranchID + ") AND ";

                if (MoveDate > 0)
                    strSQL = strSQL + "   (MoveDate <= " + MoveDate + ") AND ";
                if (!string.IsNullOrEmpty(txtCostCenterID.Text))
                    strSQL = strSQL + "   (CostCenterID = " + txtCostCenterID.Text + ") AND ";
                strSQL = strSQL.Remove(strSQL.Length - 4, 4);

                dtQtyIn = Lip.SelectRecord(strSQL);
                if (dtQtyIn.Rows.Count > 0)
                    ItemQtyIn = Comon.ConvertToDecimalPrice(dtQtyIn.Rows[0][0].ToString());



                ItemQtyOut = 0;

            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);

            }
            return Comon.ConvertToDecimalPrice(ItemQtyIn - ItemQtyOut);



        }

        decimal GetItemQtyOut(int FacilityID, int BranchID, int StoreID, int ItemID, int SizeID, int MoveDate = 0)
        {
            decimal ItemQtyOut = 0;
            decimal ItemQtyIn = 0;
            try
            {

                 
                DataTable dtQtyOut;
                ItemQtyIn =0;


                strSQL = "   SELECT  SUM(Qty + Bones) AS Qty  FROM   Stc_ItemsMoviing "
                + " Where MoveType =2   AND (FacilityID = " + FacilityID + " )  AND (ItemID = " + ItemID + " )   AND (SizeID = " + SizeID + " ) AND ";

                if (StoreID > 0)
                    strSQL = strSQL + "   (StoreID = " + StoreID + ") AND ";
                if (BranchID > 0)
                    strSQL = strSQL + "   (BranchID = " + BranchID + ") AND ";

                if (MoveDate > 0)
                    strSQL = strSQL + "   (MoveDate <= " + MoveDate + ") AND ";
                if (!string.IsNullOrEmpty(txtCostCenterID.Text))
                    strSQL = strSQL + "   (CostCenterID = " + txtCostCenterID.Text + ") AND ";


                strSQL = strSQL.Remove(strSQL.Length - 4, 4);

                dtQtyOut = Lip.SelectRecord(strSQL);
                if (dtQtyOut.Rows.Count > 0)
                    ItemQtyOut = Comon.ConvertToDecimalPrice(dtQtyOut.Rows[0][0].ToString());

            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);

            }
            return Comon.ConvertToDecimalPrice(ItemQtyIn - ItemQtyOut);



        }

        decimal GetItemQty(int FacilityID, int BranchID, int StoreID, int ItemID, int SizeID, int MoveDate = 0)
        {
            decimal ItemQtyOut = 0;
            decimal ItemQtyIn = 0;
            try
            {

                DataTable dtQtyIn;
                DataTable dtQtyOut;
                strSQL = " SELECT  SUM(Qty + Bones) AS Qty  FROM   Stc_ItemsMoviing "
                + "Where MoveType =1  AND (FacilityID = " + FacilityID + " )  AND (ItemID = " + ItemID + " )   AND (SizeID = " + SizeID + " ) AND ";

                if (StoreID > 0)
                    strSQL = strSQL + "   (StoreID = " + StoreID + ") AND ";
                if (BranchID > 0)
                    strSQL = strSQL + "   (BranchID = " + BranchID + ") AND ";

                if (MoveDate > 0)
                    strSQL = strSQL + "   (MoveDate <= " + MoveDate + ") AND ";
                if (!string.IsNullOrEmpty(txtCostCenterID.Text))
                    strSQL = strSQL + "   (CostCenterID = " + txtCostCenterID.Text + ") AND ";
                strSQL = strSQL.Remove(strSQL.Length - 4, 4);

                dtQtyIn = Lip.SelectRecord(strSQL);
                if (dtQtyIn.Rows.Count > 0)
                    ItemQtyIn = Comon.ConvertToDecimalPrice(dtQtyIn.Rows[0][0].ToString());


                strSQL = "   SELECT  SUM(Qty + Bones) AS Qty  FROM   Stc_ItemsMoviing "
                + " Where MoveType =2   AND (FacilityID = " + FacilityID + " )  AND (ItemID = " + ItemID + " )   AND (SizeID = " + SizeID + " ) AND ";

                if (StoreID > 0)
                    strSQL = strSQL + "   (StoreID = " + StoreID + ") AND ";
                if (BranchID > 0)
                    strSQL = strSQL + "   (BranchID = " + BranchID + ") AND ";

                if (MoveDate > 0)
                    strSQL = strSQL + "   (MoveDate <= " + MoveDate + ") AND ";
                if (!string.IsNullOrEmpty(txtCostCenterID.Text))
                    strSQL = strSQL + "   (CostCenterID = " + txtCostCenterID.Text + ") AND ";


                strSQL = strSQL.Remove(strSQL.Length - 4, 4);

                dtQtyOut = Lip.SelectRecord(strSQL);
                if (dtQtyOut.Rows.Count > 0)
                    ItemQtyOut = Comon.ConvertToDecimalPrice(dtQtyOut.Rows[0][0].ToString());

            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);

            }
            return Comon.ConvertToDecimalPrice(ItemQtyIn - ItemQtyOut);



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

                rptForm.Parameters["StoreName"].Value = lblStoreName.Text.Trim().ToString();
                rptForm.Parameters["ByPrice"].Value = cmbPriceBy.Text.Trim().ToString();


                  rptForm.Parameters["FromItem"].Value = txtFromItemNo.Text.Trim().ToString();
                rptForm.Parameters["ToItem"].Value = txtToItemNo .Text.Trim().ToString();
                rptForm.Parameters["ItemType"].Value = cmbTypeID.Text.Trim().ToString();
                rptForm.Parameters["Group"].Value = lblGroupID.Text.Trim().ToString();

                rptForm.Parameters["CostCenter"].Value = lblCostCenterName .Text.Trim().ToString();
                rptForm.Parameters["ToDate"].Value = txtToDate.Text.Trim().ToString();
                    rptForm.Parameters["BalanceType"].Value = lblStoreName.Text.Trim().ToString();
                    rptForm.Parameters["parameter1"].Value = "الاجمالي بحسب " + cmbPriceBy.Text.Trim().ToString();
                


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

        private void txtBranchID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as BranchName FROM Branches WHERE BranchID =" + Comon.cInt(txtBranchID.Text) + " And Cancel =0 And  BranchID =" + UserInfo.BRANCHID;
                CSearch.ControlValidating(txtBranchID, lblBranchName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
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
                    if (Permissions.UserPermissionsFrom(frm3, frm3.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm3);
                        frm3.Show();
                      //  frm3.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                        frm3.txtBarCode.Text = cellValue.ToString();
                        frm3.txtOldBarcodeID_Validating(null, null);
                        if (!string.IsNullOrEmpty (txtStoreID.Text) )
                        frm3.StoreChange(Comon.cLong(txtStoreID.Text));
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


        public DataTable ChangeStoreID(long stroeID)
        {
            try{
            txtStoreID.Text = stroeID.ToString();
            cmbPriceBy.EditValue = "CostPrice" ;// MySession.GlobalCalcStockBy;

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
            label1.Text = "الاجمالي بحسب " + cmbPriceBy.Text+":";
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
                row["ItemName"] = " الاجمالي حسب سعر " + cmbPriceBy.Text;
                row["SizeName"] = "";
                row["Qty"] =  "";
                row["QtyVisical"] = "";
                row["Price"] ="";
                row["Total"] = Comon.ConvertToDecimalPrice(Total);
                _sampleData.Rows.Add(row);
                 
            }
            catch { }

        }
        private void btnGoodOpeningAproval_Click(object sender, EventArgs e)
        {
            try
            {
                string ToDate = Comon.ConvertDateToSerial(txtToDate.Text).ToString();
                frmAccountStatement frm = new frmAccountStatement();
                frm.Show();
                frm.Hide();
                frm.txtAccountID.Text = lblDebitAccountID.Text;
                if (frm.txtAccountID.Text == string.Empty)
                    return;

                frm.btnShow_Click(null, null);
                strSQL = "Update Stc_GoodAmount Set Amount=" + Comon.ConvertToDecimalPrice(frm.lblBalanceSum.Text) + " , DateClose=" + ToDate + " , TypePrice= " + 0 + "   Where EngName='GoodOpening' ";
                Lip.ExecututeSQL(strSQL);
                MessageBox.Show(" تم اعتماد بضاعة أول مدة بقيمة " + frm.lblBalanceSum.Text);
                frm.Close();
            }
            catch { }

        }
        ////////////////////////
        public string PurchaseInvoice(string BarCode)
        {
            try
            {
                //long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));
                filter = "";
                filter = "(.Sales_PurchaseInvoiceDetails.BranchID = " + UserInfo.BRANCHID + ") AND dbo.Sales_PurchaseInvoiceDetails.Cancel =0   AND";
                strSQL = "";

                if (BarCode != string.Empty)
                    filter = filter + " .Sales_PurchaseInvoiceDetails.BarCode  ='" + BarCode + "'  AND ";
                if (txtStoreID.Text != string.Empty)
                    filter = filter + " .Sales_PurchaseInvoiceDetails.StoreID  =" + Comon.cInt(txtStoreID.Text) + "  AND ";
                long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));
                if (ToDate != 0)
                    filter = filter + " dbo.Sales_PurchaseInvoiceMaster.InvoiceDate <=" + ToDate + " AND ";
                filter = filter.Remove(filter.Length - 4, 4);

                strSQL = "SELECT dbo.Sales_PurchaseInvoiceMaster.InvoiceDate AS TheDate, 'PurchaseInvoice' AS RecordType, (dbo.Sales_PurchaseInvoiceDetails.Qty) AS Qty, "
                + " dbo.Sales_PurchaseInvoiceMaster.RegTime,"
                + " dbo.Sales_PurchaseInvoiceDetails.CostPrice AS InPrice, dbo.Sales_PurchaseInvoiceDetails.QTY * dbo.Sales_PurchaseInvoiceDetails.CostPrice AS InTotal, "
               + " dbo.Sales_PurchaseInvoiceDetails.InvoiceID AS ID FROM dbo.Sales_PurchaseInvoiceDetails LEFT OUTER JOIN dbo.Sales_PurchaseInvoiceMaster ON  "
                + " dbo.Sales_PurchaseInvoiceDetails.BranchID = dbo.Sales_PurchaseInvoiceMaster.BranchID AND dbo.Sales_PurchaseInvoiceDetails.InvoiceID = "
                + " dbo.Sales_PurchaseInvoiceMaster.InvoiceID WHERE " + filter;
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());



            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);


                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            return strSQL;
        }
        public string GoodItems(string BarCode)
        {
            try
            {

                filter = "";
                filter = "(.Stc_GoodOpeningDetails.BranchID = " + UserInfo.BRANCHID + ") AND dbo.Stc_GoodOpeningDetails.Cancel =0   AND";
                strSQL = "";

                if (BarCode != string.Empty)
                    filter = filter + " .Stc_GoodOpeningDetails.BarCode  ='" + BarCode + "'  AND ";
                if (txtStoreID.Text != string.Empty)
                    filter = filter + " .Stc_GoodOpeningDetails.StoreID  =" + Comon.cInt(txtStoreID.Text) + "  AND ";
                long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));
                if (ToDate != 0)
                    filter = filter + " dbo.Stc_GoodOpeningMaster.InvoiceDate <=" + ToDate + " AND ";
                filter = filter.Remove(filter.Length - 4, 4);

                strSQL = "SELECT  sum(dbo.Stc_GoodOpeningDetails.QTY) AS QTY "
          
                + " FROM dbo.Stc_GoodOpeningDetails LEFT OUTER JOIN dbo.Stc_GoodOpeningMaster ON  "
                + " dbo.Stc_GoodOpeningDetails.BranchID = dbo.Stc_GoodOpeningMaster.BranchID AND dbo.Stc_GoodOpeningDetails.InvoiceID = "
                + " dbo.Stc_GoodOpeningMaster.InvoiceID WHERE " + filter;
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());


            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);


                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }


            return strSQL;
        }
        public string SalesInvoiceReturn(string BarCode)
        {
            try
            {
                filter = "";
                filter = "(.Sales_SalesInvoiceReturnDetails.BranchID = " + UserInfo.BRANCHID + ") AND dbo.Sales_SalesInvoiceReturnMaster.Cancel =0   AND";
                strSQL = "";

                if (BarCode != string.Empty)
                    filter = filter + " .Sales_SalesInvoiceReturnDetails.BarCode ='" + BarCode + "'  AND ";
                if (txtStoreID.Text != string.Empty)
                    filter = filter + " .Sales_SalesInvoiceReturnMaster.StoreID  =" + Comon.cInt(txtStoreID.Text) + "  AND ";
                long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));
                if (ToDate != 0)
                    filter = filter + " dbo.Sales_SalesInvoiceReturnMaster.InvoiceDate <=" + ToDate + " AND ";
                filter = filter.Remove(filter.Length - 4, 4);
                strSQL = "SELECT dbo.Sales_SalesInvoiceReturnMaster.InvoiceDate AS TheDate, 'SalesInvoiceReturn' AS RecordType, (dbo.Sales_SalesInvoiceReturnDetails.QTY) AS QTY,dbo.Sales_SalesInvoiceReturnMaster.RegTime, "
                + " dbo.Sales_SalesInvoiceReturnDetails.SalePrice AS InPrice, CONVERT(DECIMAL(10, 2), dbo.Sales_SalesInvoiceReturnDetails.QTY * dbo.Sales_SalesInvoiceReturnDetails.SalePrice) AS InTotal,  "
                + " dbo.Sales_SalesInvoiceReturnDetails.InvoiceID AS ID FROM dbo.Sales_SalesInvoiceReturnMaster INNER JOIN dbo.Sales_SalesInvoiceReturnDetails ON"
                + " dbo.Sales_SalesInvoiceReturnMaster.InvoiceID = dbo.Sales_SalesInvoiceReturnDetails.InvoiceID AND dbo.Sales_SalesInvoiceReturnMaster.BranchID = "
                + " dbo.Sales_SalesInvoiceReturnDetails.BranchID WHERE " + filter;
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());

            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);


                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            return strSQL;
        }
        public string SalesInvoice(string BarCode)
        {
            try
            {
                filter = "";
                filter = "(.Sales_SalesInvoiceDetails.BranchID = " + UserInfo.BRANCHID + ") AND dbo.Sales_SalesInvoiceMaster.Cancel =0   AND";
                strSQL = "";

                if (BarCode != string.Empty)
                    filter = filter + " .Sales_SalesInvoiceDetails.BarCode ='" + BarCode + "'  AND ";
                if (txtStoreID.Text != string.Empty)
                    filter = filter + " .Sales_SalesInvoiceMaster.StoreID  =" + Comon.cInt(txtStoreID.Text) + "  AND ";
                long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));
                if (ToDate != 0)
                    filter = filter + " dbo.Sales_SalesInvoiceMaster.InvoiceDate <=" + ToDate + " AND ";
                filter = filter.Remove(filter.Length - 4, 4);

                strSQL = "SELECT dbo.Sales_SalesInvoiceMaster.InvoiceDate AS TheDate, 'SalesInvoice' AS RecordType, dbo.Sales_SalesInvoiceDetails.InvoiceID AS ID, dbo.Sales_SalesInvoiceMaster.RegTime, "
               + " (dbo.Sales_SalesInvoiceDetails.QTY) AS QTY, "
               + " dbo.Sales_SalesInvoiceDetails.SalePrice AS OutPrice, CONVERT(DECIMAL(10, 2), dbo.Sales_SalesInvoiceDetails.QTY * dbo.Sales_SalesInvoiceDetails.SalePrice) AS OutTotal"
                + " FROM dbo.Sales_SalesInvoiceDetails INNER JOIN dbo.Sales_SalesInvoiceMaster ON dbo.Sales_SalesInvoiceDetails.InvoiceID = dbo.Sales_SalesInvoiceMaster.InvoiceID AND"

                + " dbo.Sales_SalesInvoiceDetails.BranchID = dbo.Sales_SalesInvoiceMaster.BranchID WHERE " + filter;
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());

            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);


                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            return strSQL;
        }
        public string PurchaseInvoiceReturn(string BarCode)
        {
            try
            {
                filter = " ";
                filter = "(.Sales_PurchaseInvoiceReturnMaster.BranchID = " + UserInfo.BRANCHID + ") AND dbo.Sales_PurchaseInvoiceReturnMaster.Cancel =0   AND";
                strSQL = "";

                if (BarCode != string.Empty)
                    filter = filter + " .Sales_PurchaseInvoiceReturnDetails.BarCode  ='" + BarCode + "'  AND ";
                if (txtStoreID.Text != string.Empty)
                    filter = filter + " .Sales_PurchaseInvoiceReturnMaster.StoreID  =" + Comon.cInt(txtStoreID.Text) + "  AND ";
                long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));
                if (ToDate != 0)
                    filter = filter + " dbo.Sales_PurchaseInvoiceReturnMaster.InvoiceDate <=" + ToDate + " AND ";
                filter = filter.Remove(filter.Length - 4, 4);
                strSQL = "SELECT dbo.Sales_PurchaseInvoiceReturnMaster.InvoiceDate AS TheDate, 'PurchaseInvoiceReturn' AS RecordType,dbo.Sales_PurchaseInvoiceReturnDetails.InvoiceID AS ID , dbo.Sales_PurchaseInvoiceReturnMaster.RegTime"
                + " , (dbo.Sales_PurchaseInvoiceReturnDetails.QTY) AS Qty,  "
               + " dbo.Sales_PurchaseInvoiceReturnDetails.CostPrice AS OutPrice,CONVERT(DECIMAL(10, 2), dbo.Sales_PurchaseInvoiceReturnDetails.QTY * dbo.Sales_PurchaseInvoiceReturnDetails.CostPrice) AS OutTotal"
               + " FROM dbo.Sales_PurchaseInvoiceReturnDetails INNER JOIN dbo.Sales_PurchaseInvoiceReturnMaster ON dbo.Sales_PurchaseInvoiceReturnDetails.InvoiceID = dbo.Sales_PurchaseInvoiceReturnMaster.InvoiceID AND "
                + " dbo.Sales_PurchaseInvoiceReturnDetails.BranchID = dbo.Sales_PurchaseInvoiceReturnMaster.BranchID WHERE " + filter;
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());

            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);


                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            return strSQL;
        }
        public string ItemsTransferTo(string BarCode)
        {
            try
            {
                filter = " ";
                filter = "(.Stc_ItemsTransferMaster.ToBranchID = " + UserInfo.BRANCHID + ") AND dbo.Stc_ItemsTransferMaster.Cancel =0   AND";
                strSQL = "";

                if (BarCode != string.Empty)
                    filter = filter + " .Stc_ItemsTransferDetails.BarCode ='" + BarCode + "'  AND ";
                if (txtStoreID.Text != string.Empty)
                    filter = filter + " .Stc_ItemsTransferMaster.ToStoreID  =" + Comon.cInt(txtStoreID.Text) + "  AND ";
                long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));
                if (ToDate != 0)
                    filter = filter + " dbo.Stc_ItemsTransferMaster.TransferDate <=" + ToDate + " AND ";
                filter = filter.Remove(filter.Length - 4, 4);
                strSQL = "SELECT dbo.Stc_ItemsTransferMaster.TransferDate AS TheDate, 'ItemsTransfer' AS RecordType, dbo.Stc_ItemsTransferMaster.TransferID AS ID, dbo.Stc_ItemsTransferMaster.RegTime, "
               + " dbo.Stc_ItemsTransferDetails.QTY AS InQty FROM dbo.Stc_ItemsTransferDetails INNER JOIN dbo.Stc_ItemsTransferMaster ON dbo.Stc_ItemsTransferDetails.TransferID "
               + " = dbo.Stc_ItemsTransferMaster.TransferID "
               + " WHERE " + filter;
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());

            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);


                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            return strSQL;
        }
        public string ItemsTransferFrom(string BarCode)
        {
            try
            {
                filter = " ";

                filter = "(.Stc_ItemsTransferMaster.FromBranchID = " + UserInfo.BRANCHID + ") AND dbo.Stc_ItemsTransferMaster.Cancel =0   AND";
                strSQL = "";

                if (BarCode != string.Empty)
                    filter = filter + " .Stc_ItemsTransferDetails.BarCode  ='" + BarCode + "'  AND ";
                if (txtStoreID.Text != string.Empty)
                    filter = filter + " .Stc_ItemsTransferMaster.FromStoreID  =" + Comon.cInt(txtStoreID.Text) + "  AND ";
                long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));
                if (ToDate != 0)
                    filter = filter + " dbo.Stc_ItemsTransferMaster.TransferDate <=" + ToDate + " AND ";
                filter = filter.Remove(filter.Length - 4, 4);

                strSQL = "SELECT dbo.Stc_ItemsTransferMaster.TransferDate AS TheDate, 'ItemsTransfer' AS RecordType, dbo.Stc_ItemsTransferMaster.TransferID AS ID, dbo.Stc_ItemsTransferMaster.RegTime, "
                + " dbo.Stc_ItemsTransferDetails.QTY AS OutQty FROM dbo.Stc_ItemsTransferDetails INNER JOIN dbo.Stc_ItemsTransferMaster ON dbo.Stc_ItemsTransferDetails.TransferID "
                + " = dbo.Stc_ItemsTransferMaster.TransferID "
               + " WHERE" + filter;
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());

            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);


                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            return strSQL;
        }
        public string ItemsOutOnBail(string BarCode)
        {

            try
            {
                filter = "";
                filter = "(.Stc_ItemsOutonBail_Details.BranchID = " + UserInfo.BRANCHID + ") AND dbo.Stc_ItemsOutonBail_Master.Cancel =0   AND";
                strSQL = "";

                if (BarCode != string.Empty)
                    filter = filter + " .Stc_ItemsOutonBail_Details.BarCode ='" + BarCode + "'  AND ";
                if (txtStoreID.Text != string.Empty)
                    filter = filter + " .Stc_ItemsOutonBail_Master.StoreID  =" + Comon.cInt(txtStoreID.Text) + "  AND ";
                long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));
                if (ToDate != 0)
                    filter = filter + "dbo.Stc_ItemsOutonBail_Master.OutDate  <=" + ToDate + " AND ";
                filter = filter.Remove(filter.Length - 4, 4);

                strSQL = "SELECT dbo.Stc_ItemsOutonBail_Master.OutDate AS TheDate, 'ItemsOutOnBail' AS RecordType, dbo.Stc_ItemsOutonBail_Details.OutID AS ID, "
               + " dbo.Stc_ItemsOutonBail_Master.RegTime, dbo.Stc_ItemsOutonBail_Details.QTY AS QTY ,"
                + " dbo.Stc_ItemsOutonBail_Details.SalePrice AS OutPrice, "
                + " CONVERT(DECIMAL(10, 2), dbo.Stc_ItemsOutonBail_Details.QTY * dbo.Stc_ItemsOutonBail_Details.SalePrice) AS OutTotal FROM dbo.Stc_ItemsOutonBail_Details"
                + " INNER JOIN dbo.Stc_ItemsOutonBail_Master ON dbo.Stc_ItemsOutonBail_Details.OutID = dbo.Stc_ItemsOutonBail_Master.OutID AND dbo.Stc_ItemsOutonBail_Details.BranchID"
                + " = dbo.Stc_ItemsOutonBail_Master.BranchID WHERE " + filter;
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());

            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);


                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            return strSQL;
        }
        public string ItemsInOnBail(string BarCode)
        {
            try
            {

                filter = "";
                filter = "(.Stc_ItemsInonBail_Details.BranchID = " + UserInfo.BRANCHID + ") AND dbo.Stc_ItemsInonBail_Master.Cancel =0   AND";
                strSQL = "";

                if (BarCode != string.Empty)
                    filter = filter + " .Stc_ItemsInonBail_Details.BarCode ='" + BarCode + "'  AND ";
                if (txtStoreID.Text != string.Empty)
                    filter = filter + " .Stc_ItemsInonBail_Master.StoreID  =" + Comon.cInt(txtStoreID.Text) + "  AND ";
                long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));
                if (ToDate != 0)
                    filter = filter + "dbo.Stc_ItemsInonBail_Master.InDate  <=" + ToDate + " AND ";
                filter = filter.Remove(filter.Length - 4, 4);
                strSQL = "SELECT dbo.Stc_ItemsInonBail_Master.InDate AS TheDate, 'ItemsInOnBail' AS RecordType, dbo.Stc_ItemsInonBail_Details.InID AS ID, "
               + " dbo.Stc_ItemsInonBail_Master.RegTime, dbo.Stc_ItemsInonBail_Details.QTY AS QTY ,     "
                + " dbo.Stc_ItemsInonBail_Details.CostPrice AS InPrice, "
               + " CONVERT(DECIMAL(10, 2), dbo.Stc_ItemsInonBail_Details.QTY * dbo.Stc_ItemsInonBail_Details.CostPrice) AS InTotal FROM dbo.Stc_ItemsInonBail_Details"
               + " INNER JOIN dbo.Stc_ItemsInonBail_Master ON dbo.Stc_ItemsInonBail_Details.InID = dbo.Stc_ItemsInonBail_Master.InID AND dbo.Stc_ItemsInonBail_Details.BranchID"
               + " = dbo.Stc_ItemsInonBail_Master.BranchID WHERE " + filter;
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());

            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);


                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            return strSQL;
        }
        public string ItemsDismantlingTo(string BarCode)
        {
            try
            {
                filter = "";
                filter = "(.Stc_ItemsDismantlingMaster.BranchID = " + UserInfo.BRANCHID + ") AND dbo.Stc_ItemsDismantlingMaster.Cancel =0   AND";
                strSQL = "";

                if (BarCode != string.Empty)
                    filter = filter + " .Stc_ItemsDismantlingDetails.FromBarCode ='" + BarCode + "'  AND ";
                if (txtStoreID.Text != string.Empty)
                    filter = filter + " .Stc_ItemsDismantlingMaster.StoreID  =" + Comon.cInt(txtStoreID.Text) + "  AND ";

                filter = filter.Remove(filter.Length - 4, 4);

                strSQL = "SELECT  dbo.Stc_ItemsDismantlingMaster.DismantleDate AS TheDate, 'ItemsDismantling' AS RecordType, dbo.Stc_ItemsDismantlingMaster.DismantleID AS ID,"
               + " dbo.Stc_ItemsDismantlingDetails.QTY AS Qty, dbo.Stc_ItemsDismantlingMaster.RegTime FROM dbo.Stc_ItemsDismantlingDetails INNER JOIN dbo.Stc_ItemsDismantlingMaster ON "
               + " dbo.Stc_ItemsDismantlingDetails.DismantleID = dbo.Stc_ItemsDismantlingMaster.DismantleID AND dbo.Stc_ItemsDismantlingDetails.BranchID = "
                + " dbo.Stc_ItemsDismantlingMaster.BranchID WHERE " + filter;
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());

            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);


                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            return strSQL;
        }


        public string ItemsDismantlingFrom(string BarCode)
        {
            try
            {
                filter = "";
                filter = "(.Stc_ItemsDismantlingMaster.BranchID = " + UserInfo.BRANCHID + ") AND dbo.Stc_ItemsDismantlingMaster.Cancel =0   AND";
                strSQL = "";

                if (BarCode != string.Empty)
                    filter = filter + " .Stc_ItemsDismantlingDetails.ToBarCode ='" + BarCode + "'  AND ";
                if (txtStoreID.Text != string.Empty)
                    filter = filter + " .Stc_ItemsDismantlingMaster.StoreID  =" + Comon.cInt(txtStoreID.Text) + "  AND ";

                filter = filter.Remove(filter.Length - 4, 4);

                strSQL = "SELECT  dbo.Stc_ItemsDismantlingMaster.DismantleDate AS TheDate, 'ItemsDismantling' AS RecordType, dbo.Stc_ItemsDismantlingMaster.DismantleID AS ID,"
                + " dbo.Stc_ItemsDismantlingDetails.DismantledQTY AS Qty, dbo.Stc_ItemsDismantlingMaster.RegTime FROM dbo.Stc_ItemsDismantlingDetails INNER JOIN dbo.Stc_ItemsDismantlingMaster ON "
                + " dbo.Stc_ItemsDismantlingDetails.DismantleID = dbo.Stc_ItemsDismantlingMaster.DismantleID AND dbo.Stc_ItemsDismantlingDetails.BranchID = "
                + " dbo.Stc_ItemsDismantlingMaster.BranchID WHERE " + filter;
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());

            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);


                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            return strSQL;
        }

        /// <summary>
        /// //////////////////////////////////////////////////////
        /// </summary>
        /// <param name="stroeID"></param>
        /// <returns></returns>
        //public DataTable ChangeStoreID(long stroeID)
        //{

        //    txtStoreID.Text = stroeID.ToString();
        //    cmbPriceBy.EditValue = "CostPrice";// MySession.GlobalCalcStockBy;

        //    _nativeData.Columns.Add(new DataColumn("Sn", typeof(string)));
        //    _nativeData.Columns.Add(new DataColumn("Barcode", typeof(string)));
        //    _nativeData.Columns.Add(new DataColumn("ItemID", typeof(string)));
        //    _nativeData.Columns.Add(new DataColumn("SizeID", typeof(string)));

        //    _nativeData.Columns.Add(new DataColumn("ItemName", typeof(string)));
        //    _nativeData.Columns.Add(new DataColumn("SizeName", typeof(string)));
        //    _nativeData.Columns.Add(new DataColumn("Qty", typeof(string)));
        //    _nativeData.Columns.Add(new DataColumn("QtyVisical", typeof(string)));
        //    _nativeData.Columns.Add(new DataColumn("Price", typeof(string)));
        //    _nativeData.Columns.Add(new DataColumn("Total", typeof(string)));
        //    _nativeData.Columns.Add(new DataColumn("MinLimitQty", typeof(string)));
        //    _nativeData.Columns.Add(new DataColumn("GroupID", typeof(string)));

        //    // dt.Columns.Add("MinLimitQty", System.Type.GetType("System.Decimal"))
        //    //dt.Columns.Add("GroupID", System.Type.GetType("System.Int32"))
        //    btnShow_Click(null, null);

        //    for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
        //    {
        //        var row = _nativeData.NewRow();

        //        row["Sn"] = i + 1;
        //        row["Barcode"] = gridView1.GetRowCellValue(i, "Barcode").ToString();
        //        row["ItemName"] = gridView1.GetRowCellValue(i, "ItemName").ToString();
        //        row["ItemID"] = gridView1.GetRowCellValue(i, "ItemID").ToString();
        //        row["SizeName"] = gridView1.GetRowCellValue(i, "SizeName").ToString();
        //        row["QTY"] = gridView1.GetRowCellValue(i, "Qty").ToString();
        //        row["QtyVisical"] = gridView1.GetRowCellValue(i, "QtyVisical").ToString();
        //        row["Price"] = gridView1.GetRowCellValue(i, "Price").ToString();
        //        row["Total"] = gridView1.GetRowCellValue(i, "Total").ToString();


        //        _nativeData.Rows.Add(row);
        //    }

        //    return _nativeData;
        //}

        //private void cmbPriceBy_EditValueChanged(object sender, EventArgs e)
        //{
        //    label1.Text = "الاجمالي بحسب " + cmbPriceBy.Text + ":";
        //}
         


        /// //////////////////////////////////////////////////////////    }

        public void GetAccountsDeclaration()
        {
            #region get accounts declaration
            try
            {
                List<Acc_DeclaringMainAccounts> AllAccounts = new List<Acc_DeclaringMainAccounts>();
                int BRANCHID = UserInfo.BRANCHID;
                int FacilityID = UserInfo.FacilityID;
                DataTable dtDeclaration = new DataTable();
                dtDeclaration = new Acc_DeclaringMainAccountsDAL().GetAcc_DeclaringMainAccounts(BRANCHID, FacilityID);
                if (dtDeclaration != null && dtDeclaration.Rows.Count > 0)
                {
                    //حساب بضاعة اول المدة
                    DataRow[] row = dtDeclaration.Select("DeclareAccountName = 'GoodsOpening'");
                    if (row.Length > 0)
                    {
                        lblDebitAccountID.Text = row[0]["AccountID"].ToString();
                        lblDebitAccountName.Text = row[0]["AccountName"].ToString();
                    } 
                }
            }
            catch (Exception)
            {

                return;
            }
            #endregion

        }


        public void GetStock()
        {
            btnShow_Click(null, null);
        }

       
        private void gridView1_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.RowHandle >= 0)
            {
                //  string category = View.GetRowCellDisplayText(e.RowHandle, View.Columns["Category"]);
                if (View.GetRowCellDisplayText(e.RowHandle, View.Columns["ItemID"]).ToString() == "")
                {
                    if (Comon.cInt(View.GetRowCellDisplayText(e.RowHandle, View.Columns["ItemID"]).ToString()) > 0)
                    {
                        e.Appearance.BackColor = Color.LightYellow;
                        e.Appearance.BackColor2 = Color.LightYellow;
                        //  e.Appearance.Font.Styl;
                       
                    }
                    else {



                        e.Appearance.BackColor = Color.LightBlue;
                        e.Appearance.BackColor2 = Color.LightBlue;
                        //  e.Appearance.Font.Styl;
                  
                    
                    }

                    e.HighPriority = true;
                    
                }

                
            }
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }

    }

}
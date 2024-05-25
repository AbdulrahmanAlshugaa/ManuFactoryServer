using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraReports.UI;
using DevExpress.XtraSplashScreen;
using Edex.DAL;
using Edex.Model;
using Edex.Model.Language;
using Edex.GeneralObjects.GeneralClasses;
using Edex.GeneralObjects.GeneralForms;
using Edex.ModelSystem;
using Edex.SalesAndPurchaseObjects.Transactions;
using Edex.SalesAndSaleObjects.Transactions;
using Edex.StockObjects.Codes;
using Edex.StockObjects.Transactions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
namespace Edex.StockObjects.Reports
{
    public partial class frmItemBalance : Edex.GeneralObjects.GeneralForms.BaseForm
    {
        public string FocusedControl;
        private string filter = "";
        private string strSQL = "";
        private string getItemSQL = "";
        private string where = "";
        DataTable dt = new DataTable();
        public DataTable _sampleData = new DataTable();
        public frmItemBalance()
        {
            try{
            InitializeComponent();
            ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Visible = true;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Caption =(UserInfo.Language==iLanguage.Arabic ? "استعلام جديد":"New Query");
            ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Caption = "استعلام جديد";
           // ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[2].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[3].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[4].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[5].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[7].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[8].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[9].Visible = false;
            ///////////////////////////////////////////////////////
            this.txtStoreID.Validating += new System.ComponentModel.CancelEventHandler(this.txtStoreID_Validating);
            this.txtBarCode.Validating += new System.ComponentModel.CancelEventHandler(this.txtOldBarcodeID_Validating);
            this.gridView1.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.gridView1_RowClick);
            gridView1.OptionsView.EnableAppearanceEvenRow = true;
            gridView1.OptionsView.EnableAppearanceOddRow = true;
            gridView1.OptionsBehavior.ReadOnly = true;
            gridView1.OptionsBehavior.Editable = false;
            if (UserInfo.Language == iLanguage.English)
            {
                dgvColInTotal.Caption = "  IN TOTAL";
                dgvColOutQty.Caption = "OUT Quantity";
                dgvColOutTotal.Caption = "OUT  Total ";
                dgvColOutPrice.Caption = "Out price ";
                
                dgvColBalance.Caption = "Balance ";
                dgvColSN.Caption = "# ";


                 dgvColRecordType.Caption = "Record Type ";
                dgvColID.Caption = "Trans ID";
                dgvColTheDate.Caption = "Date ";
                dgvColTempRecordType.Caption = "Item Name ";
                
                dgvColInPrice.Caption = "IN Price ";
                                dgvColSizeID.Caption = "Size NO ";

                                dgvColInQty.Caption = "In  Quantity ";

      
                btnShow.Text = btnShow.Tag.ToString();
                Label8.Text = btnShow.Tag.ToString();
            }

            }
            catch { }

        }
        private void frmItemBalance_Load(object sender, EventArgs e)
        {
            _sampleData.Columns.Add(new DataColumn("SN", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("RegTime", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("TheDate", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("DateOfSort", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("ID", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("InPrice", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("CaratPrice", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("InTotal", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("OutPrice", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("OutTotal", typeof(decimal)));


            _sampleData.Columns.Add(new DataColumn("InQty", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("OutQty", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("Balance", typeof(decimal)));

            _sampleData.Columns.Add(new DataColumn("InQtyDaimond_W", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("OutQtyDaimond_W", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("BalanceDaimond_W", typeof(decimal)));

            _sampleData.Columns.Add(new DataColumn("InQtyStone_W", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("OutQtyStone_W", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("BalanceStone_W", typeof(decimal)));

            _sampleData.Columns.Add(new DataColumn("InQtyBagate_W", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("OutQtyBagate", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("BalanceBagate", typeof(decimal)));


          
            ///  _sampleData.Columns.Add(new DataColumn("OutTotal", typeof(decimal)));

            _sampleData.Columns.Add(new DataColumn("RecordType", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("TempRecordType", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("F1", typeof(string)));
            FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", "ArbName", "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
            cmbBranchesID.EditValue = UserInfo.BRANCHID;

            if (UserInfo.ID == 1)
            {
                cmbBranchesID.Visible = true;
                labelControl9.Visible = true;
            }

            else
            {
                cmbBranchesID.Visible = false;
                labelControl9.Visible = false;
            }



        }
        private void ProcessBalance()
        {
            decimal sum = 0;
            decimal sumDIAMOND_W = 0;
            decimal sumSTONE_W = 0;
            decimal sumBAGET_W = 0;

            try
            {
                DataTable inPrice;
                DataRow row;
                #region PurchaseInvoice
                dt.Rows.Clear();

                dt = Lip.SelectRecord(PurchaseInvoice());
                _sampleData.Clear();
                if (strSQL != null || strSQL != "")
                {
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            row = _sampleData.NewRow();
                            row["SN"] = _sampleData.Rows.Count + 1;
                            row["TheDate"] =(dt.Rows[i]["TheDate"].ToString());
                            row["RegTime"] = dt.Rows[i]["RegTime"].ToString();
                            row["DateOfSort"] = dt.Rows[i]["TheDate"].ToString();
                            row["InQty"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["InQty"]).ToString("N" + 2);
                            row["InPrice"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["InPrice"]).ToString("N" + MySession.GlobalPriceDigits);
                            row["CaratPrice"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["CaratPrice"]).ToString("N" + MySession.GlobalPriceDigits);


                            row["InQtyDaimond_W"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["DIAMOND_W"]).ToString("N" + 2);
                            row["BalanceDaimond_W"] = sumDIAMOND_W + (Comon.ConvertToDecimalPrice(row["InQtyDaimond_W"]));
                            sumDIAMOND_W = Comon.ConvertToDecimalPrice(row["BalanceDaimond_W"]);



                            row["InQtyStone_W"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["STONE_W"]).ToString("N" + 2);
                            row["BalanceStone_W"] = sumSTONE_W + (Comon.ConvertToDecimalPrice(row["InQtyStone_W"]));
                            sumSTONE_W = Comon.ConvertToDecimalPrice(row["BalanceStone_W"]);


                            row["InQtyBagate_W"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["BAGET_W"]).ToString("N" + 2);
                            row["BalanceBagate"] = sumBAGET_W + (Comon.ConvertToDecimalPrice(row["InQtyBagate_W"]));
                            sumBAGET_W = Comon.ConvertToDecimalPrice(row["BalanceBagate"]);
                            row["InTotal"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["InTotal"]).ToString("N" + MySession.GlobalPriceDigits);
                            row["Balance"] = sum + (Comon.ConvertToDecimalPrice(row["InQty"]));
                            sum = Comon.ConvertToDecimalPrice(row["Balance"]);

                            row["OutQtyDaimond_W"] =0;
                            row["OutQtyStone_W"] = 0;
                            row["OutQtyBagate"] = 0;

                            row["ID"] = dt.Rows[i]["ID"].ToString();
                            if(Comon.cInt(dt.Rows[i]["GoldUsing"])==3)
                                row["RecordType"] = CaseRecordType("PurchaseInvoiceAlmas", Comon.cLong(dt.Rows[i]["ID"]), gridView1.RowCount - 1);
                            else if (Comon.cInt(dt.Rows[i]["GoldUsing"]) == 1)
                                row["RecordType"] = CaseRecordType("PurchaseInvoiceGold", Comon.cLong(dt.Rows[i]["ID"]), gridView1.RowCount - 1);
                            row["TempRecordType"] = dt.Rows[i]["RecordType"].ToString();


                            _sampleData.Rows.Add(row);

                        }
                    }

                }  
                #endregion
                #region PurchaseSaveInvoice
                dt.Rows.Clear();
                dt = Lip.SelectRecord(GetStrSQLSave());
                if (strSQL != null || strSQL != "")
                {
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            row = _sampleData.NewRow();
                            row["Sn"] = _sampleData.Rows.Count + 1;
                            row["TheDate"] = (dt.Rows[i]["TheDate"].ToString());



                            row["SN"] = _sampleData.Rows.Count + 1;
                            row["TheDate"] = (dt.Rows[i]["TheDate"].ToString());
                            row["RegTime"] = dt.Rows[i]["RegTime"].ToString();
                            row["DateOfSort"] = dt.Rows[i]["TheDate"].ToString();
                            row["InQty"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["InQty"]).ToString("N" + 2);
                            row["InPrice"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["InPrice"]).ToString("N" + MySession.GlobalPriceDigits);
                            row["CaratPrice"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["CaratPrice"]).ToString("N" + MySession.GlobalPriceDigits);


                            row["InQtyDaimond_W"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["DIAMOND_W"]).ToString("N" + 2);
                            row["BalanceDaimond_W"] = sumDIAMOND_W + (Comon.ConvertToDecimalPrice(row["InQtyDaimond_W"]));
                            sumDIAMOND_W = Comon.ConvertToDecimalPrice(row["BalanceDaimond_W"]);


                            row["InQtyStone_W"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["STONE_W"]).ToString("N" + 2);
                            row["BalanceStone_W"] = sumSTONE_W + (Comon.ConvertToDecimalPrice(row["InQtyStone_W"]));                           
                            sumSTONE_W = Comon.ConvertToDecimalPrice(row["BalanceStone_W"]);
                            row["InQtyBagate_W"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["BAGET_W"]).ToString("N" + 2);
                            row["BalanceBagate"] = sumBAGET_W + (Comon.ConvertToDecimalPrice(row["InQtyBagate_W"]));
                            sumBAGET_W = Comon.ConvertToDecimalPrice(row["BalanceBagate"]);
                            row["InTotal"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["InTotal"]).ToString("N" + MySession.GlobalPriceDigits);
                            row["Balance"] = sum + (Comon.ConvertToDecimalPrice(row["InQty"]));
                            sum = Comon.ConvertToDecimalPrice(row["Balance"]);

                            row["OutQtyDaimond_W"] = 0;
                            row["OutQtyStone_W"] = 0;
                            row["OutQtyBagate"] = 0;

                            row["ID"] = dt.Rows[i]["ID"].ToString();
                            row["RecordType"] = CaseRecordType(dt.Rows[i]["RecordType"].ToString(), Comon.cLong(dt.Rows[i]["ID"]), gridView1.RowCount - 1);
                            row["TempRecordType"] = dt.Rows[i]["RecordType"].ToString();

                            _sampleData.Rows.Add(row);

                        }
                    }

                }
                #endregion

                #region PurchaseSaveInvoiceReturn
                dt.Rows.Clear();
                dt = Lip.SelectRecord(GetStrSQLSaveReturn());
                if (strSQL != null || strSQL != "")
                {
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            row = _sampleData.NewRow();
                            row["Sn"] = _sampleData.Rows.Count + 1;
                            row["TheDate"] = (dt.Rows[i]["TheDate"].ToString());



                            row["SN"] = _sampleData.Rows.Count + 1;
                            row["TheDate"] = (dt.Rows[i]["TheDate"].ToString());
                            row["RegTime"] = dt.Rows[i]["RegTime"].ToString();
                            row["DateOfSort"] = dt.Rows[i]["TheDate"].ToString();
                            row["OutQty"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["OutQty"]).ToString("N" + 2);
                            row["OutPrice"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["OutPrice"]).ToString("N" + MySession.GlobalPriceDigits);
                            row["CaratPrice"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["CaratPrice"]).ToString("N" + MySession.GlobalPriceDigits);


                            row["OutQtyDaimond_W"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["DIAMOND_W"]).ToString("N" + 2);
                            row["BalanceDaimond_W"] = sumDIAMOND_W - (Comon.ConvertToDecimalPrice(row["OutQtyDaimond_W"]));
                            sumDIAMOND_W = Comon.ConvertToDecimalPrice(row["BalanceDaimond_W"]);


                            row["OutQtyStone_W"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["STONE_W"]).ToString("N" + 2);
                            row["BalanceStone_W"] = sumSTONE_W - (Comon.ConvertToDecimalPrice(row["OutQtyStone_W"]));
                            sumSTONE_W = Comon.ConvertToDecimalPrice(row["BalanceStone_W"]);
                            row["OutQtyBagate"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["BAGET_W"]).ToString("N" + 2);
                            row["BalanceBagate"] = sumBAGET_W - (Comon.ConvertToDecimalPrice(row["OutQtyBagate"]));
                            sumBAGET_W = Comon.ConvertToDecimalPrice(row["BalanceBagate"]);
                            row["OutTotal"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["OutTotal"]).ToString("N" + MySession.GlobalPriceDigits);
                            row["Balance"] = sum - (Comon.ConvertToDecimalPrice(row["OutQty"]));
                            sum = Comon.ConvertToDecimalPrice(row["Balance"]);
                             
                                
                                
                            row["InQtyDaimond_W"] = 0;
                            row["InQtyStone_W"] = 0;
                            row["InQtyBagate_W"] = 0;

                            row["ID"] = dt.Rows[i]["ID"].ToString();
                            row["RecordType"] = CaseRecordType(dt.Rows[i]["RecordType"].ToString(), Comon.cLong(dt.Rows[i]["ID"]), gridView1.RowCount - 1);
                            row["TempRecordType"] = dt.Rows[i]["RecordType"].ToString();

                            _sampleData.Rows.Add(row);

                        }
                    }

                }
                #endregion
                #region GoodInput
                dt.Rows.Clear();

                dt = Lip.SelectRecord(GoodItems());
              
                if (strSQL != null || strSQL != "")
                {
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            row = _sampleData.NewRow();
                            row["SN"] = _sampleData.Rows.Count + 1;
                            row["TheDate"] =(dt.Rows[i]["TheDate"].ToString());
                            row["InQty"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["InQty"]).ToString("N" + 2);
                            row["InPrice"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["InPrice"]).ToString("N" + MySession.GlobalPriceDigits);

                            row["CaratPrice"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["CaratPrice"]).ToString("N" + MySession.GlobalPriceDigits);

                            row["InQtyDaimond_W"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["DIAMOND_W"]).ToString("N" + 2);
                            row["BalanceDaimond_W"] = sumDIAMOND_W + (Comon.ConvertToDecimalPrice(row["InQtyDaimond_W"]));
                            sumDIAMOND_W = Comon.ConvertToDecimalPrice(row["BalanceDaimond_W"]);



                            row["InQtyStone_W"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["STONE_W"]).ToString("N" + 2);
                            row["BalanceStone_W"] = sumSTONE_W + (Comon.ConvertToDecimalPrice(row["InQtyStone_W"]));
                            sumSTONE_W = Comon.ConvertToDecimalPrice(row["BalanceStone_W"]);


                            row["InQtyBagate_W"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["BAGET_W"]).ToString("N" + 2);
                            row["BalanceBagate"] = sumBAGET_W + (Comon.ConvertToDecimalPrice(row["InQtyBagate_W"]));
                            sumBAGET_W = Comon.ConvertToDecimalPrice(row["BalanceBagate"]);



                            row["DateOfSort"] = dt.Rows[i]["TheDate"].ToString();
                            row["InTotal"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["InTotal"]).ToString("N" + MySession.GlobalPriceDigits);
                            row["Balance"] = sum + (Comon.ConvertToDecimalPrice(row["InQty"]));
                            sum = Comon.ConvertToDecimalPrice(row["Balance"]);
                            row["ID"] = dt.Rows[i]["ID"].ToString();
                            row["RecordType"] = CaseRecordType(dt.Rows[i]["RecordType"].ToString(), Comon.cLong(dt.Rows[i]["ID"]), gridView1.RowCount - 1);
                            row["TempRecordType"] = dt.Rows[i]["RecordType"].ToString();



                            _sampleData.Rows.Add(row);

                        }
                    }

                }

                #endregion
           
                #region SalesInvoice
                dt.Rows.Clear();

                dt = Lip.SelectRecord(SalesInvoice());
                if (strSQL != null || strSQL != "")
                {
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            row = _sampleData.NewRow();
                            row["Sn"] = _sampleData.Rows.Count + 1;
                            row["TheDate"] =(dt.Rows[i]["TheDate"].ToString());
                            row["RegTime"] = dt.Rows[i]["RegTime"].ToString();
                            row["OutQty"] = dt.Rows[i]["OutQty"].ToString();
                            row["OutPrice"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["OutPrice"]).ToString("N" + MySession.GlobalPriceDigits);
                           
                            row["Balance"] = sum - (Comon.ConvertToDecimalPrice(row["OutQty"]));
                            sum = Comon.ConvertToDecimalPrice(row["Balance"]);
                           
                           
                           
                           


                            row["OutTotal"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["OutTotal"]).ToString("N" + MySession.GlobalPriceDigits);
                            row["DateOfSort"] = dt.Rows[i]["TheDate"].ToString();

                            row["OutQtyDaimond_W"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["DIAMOND_W"]).ToString("N" + 2);
                            row["BalanceDaimond_W"] = sumDIAMOND_W - (Comon.ConvertToDecimalPrice(row["OutQtyDaimond_W"]));
                            sumDIAMOND_W = Comon.ConvertToDecimalPrice(row["BalanceDaimond_W"]);


                            row["OutQtyStone_W"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["STONE_W"]).ToString("N" + 2);
                            row["BalanceStone_W"] = sumSTONE_W - (Comon.ConvertToDecimalPrice(row["OutQtyStone_W"]));
                            sumSTONE_W = Comon.ConvertToDecimalPrice(row["BalanceStone_W"]);


                            row["OutQtyBagate"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["BAGET_W"]).ToString("N" + 2);
                            row["BalanceBagate"] = sumBAGET_W - (Comon.ConvertToDecimalPrice(row["OutQtyBagate"]));
                            sumBAGET_W = Comon.ConvertToDecimalPrice(row["BalanceBagate"]);

                            row["InQtyDaimond_W"] = 0;
                            row["InQtyStone_W"] = 0;
                            row["InQtyBagate_W"] = 0;




                            row["ID"] = dt.Rows[i]["ID"].ToString();
                            row["RecordType"] = CaseRecordType(dt.Rows[i]["RecordType"].ToString(), Comon.cLong(dt.Rows[i]["ID"]), gridView1.RowCount - 1);
                            row["TempRecordType"] = dt.Rows[i]["RecordType"].ToString();

                            _sampleData.Rows.Add(row);

                        }
                    }

                }

              

                #endregion
                #region SalesInvoiceReturn
                dt.Rows.Clear();

                dt = Lip.SelectRecord(SalesInvoiceReturn());
                if (strSQL != null || strSQL != "")
                {
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            row = _sampleData.NewRow();
                            row["Sn"] = _sampleData.Rows.Count + 1;
                            row["TheDate"] = (dt.Rows[i]["TheDate"].ToString());
                            row["InQty"] = dt.Rows[i]["InQty"].ToString();
                            row["InPrice"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["InPrice"]).ToString("N" + MySession.GlobalPriceDigits);

                            row["DateOfSort"] = dt.Rows[i]["TheDate"].ToString();
                            row["InTotal"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["InTotal"]).ToString("N" + MySession.GlobalPriceDigits);
                            row["Balance"] = sum + (Comon.ConvertToDecimalPrice(row["InQty"]));
                            sum = Comon.ConvertToDecimalPrice(row["Balance"]);
                            row["ID"] = dt.Rows[i]["ID"].ToString();
                            row["RecordType"] = CaseRecordType(dt.Rows[i]["RecordType"].ToString(), Comon.cLong(dt.Rows[i]["ID"]), gridView1.RowCount - 1);
                            row["TempRecordType"] = dt.Rows[i]["RecordType"].ToString();
                            row["InQtyDaimond_W"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["DIAMOND_W"]).ToString("N" + 2);
                            row["BalanceDaimond_W"] = sumDIAMOND_W + (Comon.ConvertToDecimalPrice(row["InQtyDaimond_W"]));
                            sumDIAMOND_W = Comon.ConvertToDecimalPrice(row["BalanceDaimond_W"]);



                            row["InQtyStone_W"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["STONE_W"]).ToString("N" + 2);
                            row["BalanceStone_W"] = sumSTONE_W + (Comon.ConvertToDecimalPrice(row["InQtyStone_W"]));
                            sumSTONE_W = Comon.ConvertToDecimalPrice(row["BalanceStone_W"]);


                            row["InQtyBagate_W"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["BAGET_W"]).ToString("N" + 2);
                            row["BalanceBagate"] = sumBAGET_W + (Comon.ConvertToDecimalPrice(row["InQtyBagate_W"]));
                            sumBAGET_W = Comon.ConvertToDecimalPrice(row["BalanceBagate"]);


                            _sampleData.Rows.Add(row);

                        }
                    }

                }

                #endregion
             
                #region PurchaseInvoiceReturn
                dt.Rows.Clear();

                dt = Lip.SelectRecord(PurchaseInvoiceReturn());
                if (strSQL != null || strSQL != "")
                {
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            row = _sampleData.NewRow();
                            row["Sn"] = _sampleData.Rows.Count + 1;
                            row["TheDate"] =(dt.Rows[i]["TheDate"].ToString());
                            row["OutQty"] = dt.Rows[i]["OutQty"].ToString();
                            row["OutPrice"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["OutPrice"]).ToString("N" + MySession.GlobalPriceDigits);
                            row["DateOfSort"] = dt.Rows[i]["TheDate"].ToString();
                            row["OutTotal"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["OutTotal"]).ToString("N" + MySession.GlobalPriceDigits);
                            row["Balance"] = sum - (Comon.ConvertToDecimalPrice(row["OutQty"]));
                            sum = Comon.ConvertToDecimalPrice(row["Balance"]);


                            row["OutQtyDaimond_W"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["DIAMOND_W"]).ToString("N" + 2);
                            row["BalanceDaimond_W"] = sumDIAMOND_W - (Comon.ConvertToDecimalPrice(row["OutQtyDaimond_W"]));
                            sumDIAMOND_W = Comon.ConvertToDecimalPrice(row["BalanceDaimond_W"]);


                            row["OutQtyStone_W"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["STONE_W"]).ToString("N" + 2);
                            row["BalanceStone_W"] = sumSTONE_W - (Comon.ConvertToDecimalPrice(row["OutQtyStone_W"]));
                            sumSTONE_W = Comon.ConvertToDecimalPrice(row["BalanceStone_W"]);


                            row["OutQtyBagate"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["BAGET_W"]).ToString("N" + 2);
                            row["BalanceBagate"] = sumBAGET_W - (Comon.ConvertToDecimalPrice(row["OutQtyBagate"]));
                            sumBAGET_W = Comon.ConvertToDecimalPrice(row["BalanceBagate"]);

                            row["InQtyDaimond_W"] = 0;
                            row["InQtyStone_W"] = 0;
                            row["InQtyBagate_W"] = 0;


                            row["ID"] = dt.Rows[i]["ID"].ToString();
                            row["RecordType"] = CaseRecordType(dt.Rows[i]["RecordType"].ToString(), Comon.cLong(dt.Rows[i]["ID"]), gridView1.RowCount - 1);
                            row["TempRecordType"] = dt.Rows[i]["RecordType"].ToString();



                            _sampleData.Rows.Add(row);

                        }
                    }

                }

                #endregion
                #region ItemsDismantlingFrom
                dt.Rows.Clear();
                dt = Lip.SelectRecord(ItemsDismantlingFrom());
                if (strSQL != null || strSQL != "")
                {
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            row = _sampleData.NewRow();
                            row["Sn"] = _sampleData.Rows.Count + 1;
                            row["TheDate"] =(dt.Rows[i]["TheDate"].ToString());
                            row["InQty"] = dt.Rows[i]["InQty"].ToString();
                            inPrice = Lip.SelectRecord(InPrice());
                            row["DateOfSort"] = dt.Rows[i]["TheDate"].ToString();
                            if (inPrice.Rows.Count > 0)
                            {
                                row["InPrice"] = Comon.ConvertToDecimalPrice(inPrice.Rows[0]["InPrice"]).ToString("N" + MySession.GlobalPriceDigits);
                                row["InTotal"] = (Comon.ConvertToDecimalPrice(inPrice.Rows[0]["InPrice"]) * Comon.ConvertToDecimalPrice(dt.Rows[i]["InQty"])).ToString("N" + MySession.GlobalPriceDigits);

                            }

                            row["Balance"] = sum + (Comon.ConvertToDecimalPrice(row["InQty"]));
                            sum = Comon.ConvertToDecimalPrice(row["Balance"]);
                            row["ID"] = dt.Rows[i]["ID"].ToString();
                            row["RecordType"] = CaseRecordType(dt.Rows[i]["RecordType"].ToString(), Comon.cLong(dt.Rows[i]["ID"]), gridView1.RowCount - 1);
                            row["TempRecordType"] = dt.Rows[i]["RecordType"].ToString();


                            _sampleData.Rows.Add(row);

                        }
                    }

                }

                #endregion
                #region ItemsDismantlingTo
                dt = Lip.SelectRecord(ItemsDismantlingTo());
                if (strSQL != null || strSQL != "")
                {
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            row = _sampleData.NewRow();
                            row["Sn"] = _sampleData.Rows.Count + 1;
                            row["TheDate"] =(dt.Rows[i]["TheDate"].ToString());
                            row["OutQty"] = dt.Rows[i]["OutQty"].ToString();
                            inPrice = Lip.SelectRecord(InPrice());
                            row["DateOfSort"] = dt.Rows[i]["TheDate"].ToString();
                            if (inPrice.Rows.Count > 0)
                            {
                                row["OutPrice"] = Comon.ConvertToDecimalPrice(inPrice.Rows[0]["InPrice"]).ToString("N" + MySession.GlobalPriceDigits);
                                row["OutTotal"] = (Comon.ConvertToDecimalPrice(inPrice.Rows[0]["InPrice"]) * Comon.ConvertToDecimalPrice(dt.Rows[i]["OutQty"])).ToString("N" + MySession.GlobalPriceDigits);

                            }
                            row["Balance"] = sum - (Comon.ConvertToDecimalPrice(row["OutQty"]));
                            sum = Comon.ConvertToDecimalPrice(row["Balance"]);

                            row["ID"] = dt.Rows[i]["ID"].ToString();
                            row["RecordType"] = CaseRecordType(dt.Rows[i]["RecordType"].ToString(), Comon.cLong(dt.Rows[i]["ID"]), gridView1.RowCount - 1);
                            row["TempRecordType"] = dt.Rows[i]["RecordType"].ToString();



                            _sampleData.Rows.Add(row);

                        }
                    }

                }

                #endregion
                #region ManufacturingOperations_Master
                dt = Lip.SelectRecord(ManufacturingOperations_Master());

                if (strSQL != null || strSQL != "")
                {
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            row = _sampleData.NewRow();
                            row["Sn"] = _sampleData.Rows.Count + 1;
                            row["DateOfSort"] = dt.Rows[i]["TheDate"].ToString();
                            row["TheDate"] =(dt.Rows[i]["TheDate"].ToString());
                            row["InQty"] = dt.Rows[i]["InQty"].ToString();
                            row["InPrice"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["InPrice"]).ToString("N" + MySession.GlobalPriceDigits);

                            row["InTotal"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["InTotal"]).ToString("N" + MySession.GlobalPriceDigits);
                            row["Balance"] = sum + (Comon.ConvertToDecimalPrice(row["InQty"]));
                            sum = Comon.ConvertToDecimalPrice(row["Balance"]);


                            row["ID"] = dt.Rows[i]["ID"].ToString();
                            row["RecordType"] = CaseRecordType(dt.Rows[i]["RecordType"].ToString(), Comon.cLong(dt.Rows[i]["ID"]), gridView1.RowCount - 1);
                            row["TempRecordType"] = dt.Rows[i]["RecordType"].ToString();



                            _sampleData.Rows.Add(row);

                        }
                    }

                }

                #endregion
                #region ManufacturingOperations_Details
                //dt.Rows.Clear();
                //dt = Lip.SelectRecord(ManufacturingOperations_Details());

                //if (strSQL != null || strSQL != "")
                //{
                //    if (dt.Rows.Count > 0)
                //    {
                //        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                //        {
                //            row = _sampleData.NewRow();
                //            row["Sn"] = _sampleData.Rows.Count + 1;
                //            row["TheDate"] = dt.Rows[i]["TheDate"].ToString();
                //            row["OutQty"] = dt.Rows[i]["OutQty"].ToString();
                //            row["OutPrice"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["OutPrice"]).ToString("N" + 2);

                //            row["OutTotal"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["OutTotal"]).ToString("N" + 2);

                //            row["ID"] = dt.Rows[i]["ID"].ToString();
                //            row["RecordType"] = CaseRecordType(dt.Rows[i]["RecordType"].ToString(), Comon.cLong(dt.Rows[i]["ID"]), gridView1.RowCount - 1);
                //            row["TempRecordType"] = dt.Rows[i]["RecordType"].ToString();


                //            _sampleData.Rows.Add(row);

                //        }
                //    }

                //}

                #endregion
                #region ItemsTransferTo
                dt.Rows.Clear();

                dt = Lip.SelectRecord(ItemsTransferTo());
                if (strSQL != null || strSQL != "")
                {
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            row = _sampleData.NewRow();
                            row["Sn"] = _sampleData.Rows.Count + 1;
                            row["TheDate"] =(dt.Rows[i]["TheDate"].ToString());
                            row["InQty"] = dt.Rows[i]["InQty"].ToString();
                            row["DateOfSort"] = dt.Rows[i]["TheDate"].ToString();
                            inPrice = Lip.SelectRecord(InPrice());
                            if (inPrice.Rows.Count > 0)
                            {
                                row["InPrice"] = Comon.ConvertToDecimalPrice(inPrice.Rows[0]["InPrice"]).ToString("N" + MySession.GlobalPriceDigits);
                                row["InTotal"] = (Comon.ConvertToDecimalPrice(inPrice.Rows[0]["InPrice"]) * Comon.ConvertToDecimalPrice(dt.Rows[i]["InQty"])).ToString("N" + MySession.GlobalPriceDigits);

                            }
                            row["Balance"] = sum + (Comon.ConvertToDecimalPrice(row["InQty"]));
                            sum = Comon.ConvertToDecimalPrice(row["Balance"]);

                            row["ID"] = dt.Rows[i]["ID"].ToString();
                            row["RecordType"] = CaseRecordType(dt.Rows[i]["RecordType"].ToString(), Comon.cLong(dt.Rows[i]["ID"]), gridView1.RowCount - 1);
                            row["TempRecordType"] = dt.Rows[i]["RecordType"].ToString();


                            _sampleData.Rows.Add(row);

                        }
                    }

                }

                #endregion
                #region ItemsTransferFrom
                dt.Rows.Clear();

                dt = Lip.SelectRecord(ItemsTransferFrom());
                if (strSQL != null || strSQL != "")
                {
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            row = _sampleData.NewRow();
                            row["Sn"] = _sampleData.Rows.Count + 1;
                            row["DateOfSort"] = dt.Rows[i]["TheDate"].ToString();
                            row["TheDate"] =(dt.Rows[i]["TheDate"].ToString());
                            row["OutQty"] = dt.Rows[i]["OutQty"].ToString();
                            inPrice = Lip.SelectRecord(OutPrice());
                            if (inPrice.Rows.Count > 0)
                            {
                                row["OutPrice"] = Comon.ConvertToDecimalPrice(inPrice.Rows[0]["OutPrice"]).ToString("N" + MySession.GlobalPriceDigits);
                                row["OuTotal"] = (Comon.ConvertToDecimalPrice(inPrice.Rows[0]["OutPrice"]) * Comon.ConvertToDecimalPrice(dt.Rows[i]["OutQty"])).ToString("N" + MySession.GlobalPriceDigits);

                            }
                            row["Balance"] = sum -(Comon.ConvertToDecimalPrice(row["OutQty"]));
                            sum = Comon.ConvertToDecimalPrice(row["Balance"]);

                            row["ID"] = dt.Rows[i]["ID"].ToString();
                            row["RecordType"] = CaseRecordType(dt.Rows[i]["RecordType"].ToString(), Comon.cLong(dt.Rows[i]["ID"]), gridView1.RowCount - 1);
                            row["TempRecordType"] = dt.Rows[i]["RecordType"].ToString();



                            _sampleData.Rows.Add(row);

                        }
                    }

                }

                #endregion
                #region ItemsOutOnBail
                dt.Rows.Clear();

                dt = Lip.SelectRecord(ItemsOutOnBail());
                if (strSQL != null || strSQL != "")
                {
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            row = _sampleData.NewRow();
                            row["Sn"] = _sampleData.Rows.Count + 1;
                            row["DateOfSort"] = dt.Rows[i]["TheDate"].ToString();
                            row["TheDate"] =(dt.Rows[i]["TheDate"].ToString());
                            row["OutQty"] = dt.Rows[i]["OutQty"].ToString();
                            row["OutPrice"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["OutPrice"]).ToString("N" + MySession.GlobalPriceDigits);

                            row["OutTotal"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["OutTotal"]).ToString("N" + MySession.GlobalPriceDigits);
                            row["Balance"] = sum - (Comon.ConvertToDecimalPrice(row["OutQty"]));
                            sum = Comon.ConvertToDecimalPrice(row["Balance"]);


                            row["OutQtyDaimond_W"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["DIAMOND_W"]).ToString("N" + 2);
                            row["BalanceDaimond_W"] = sumDIAMOND_W - (Comon.ConvertToDecimalPrice(row["OutQtyDaimond_W"]));
                            sumDIAMOND_W = Comon.ConvertToDecimalPrice(row["BalanceDaimond_W"]);


                            row["OutQtyStone_W"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["STONE_W"]).ToString("N" + 2);
                            row["BalanceStone_W"] = sumSTONE_W - (Comon.ConvertToDecimalPrice(row["OutQtyStone_W"]));
                            sumSTONE_W = Comon.ConvertToDecimalPrice(row["BalanceStone_W"]);


                            row["OutQtyBagate"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["BAGET_W"]).ToString("N" + 2);
                            row["BalanceBagate"] = sumBAGET_W - (Comon.ConvertToDecimalPrice(row["OutQtyBagate"]));
                            sumBAGET_W = Comon.ConvertToDecimalPrice(row["BalanceBagate"]);

                            row["InQtyDaimond_W"] = 0;
                            row["InQtyStone_W"] = 0;
                            row["InQtyBagate_W"] = 0;


                            row["ID"] = dt.Rows[i]["ID"].ToString();
                            row["RecordType"] = CaseRecordType(dt.Rows[i]["RecordType"].ToString(), Comon.cLong(dt.Rows[i]["ID"]), gridView1.RowCount - 1);
                            row["TempRecordType"] = dt.Rows[i]["RecordType"].ToString();

                            _sampleData.Rows.Add(row);

                        }
                    }

                }

                #endregion
                #region ItemsInOnBail
                dt.Rows.Clear();

                dt = Lip.SelectRecord(ItemsInOnBail());
                if (strSQL != null || strSQL != "")
                {
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            row = _sampleData.NewRow();
                            row["Sn"] = _sampleData.Rows.Count + 1;
                            row["DateOfSort"] = dt.Rows[i]["TheDate"].ToString();
                            row["TheDate"] =(dt.Rows[i]["TheDate"].ToString());
                            row["InQty"] = dt.Rows[i]["InQty"].ToString();
                            row["InPrice"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["InPrice"]).ToString("N" + MySession.GlobalPriceDigits);

                            row["InTotal"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["InTotal"]).ToString("N" + MySession.GlobalPriceDigits);

                            row["ID"] = dt.Rows[i]["ID"].ToString();
                            row["Balance"] = sum + (Comon.ConvertToDecimalPrice(row["InQty"]));
                            sum = Comon.ConvertToDecimalPrice(row["Balance"]);

                            row["InQtyDaimond_W"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["DIAMOND_W"]).ToString("N" + 2);
                            row["BalanceDaimond_W"] = sumDIAMOND_W + (Comon.ConvertToDecimalPrice(row["OutQtyDaimond_W"]));
                            sumDIAMOND_W = Comon.ConvertToDecimalPrice(row["BalanceDaimond_W"]);


                            row["InQtyStone_W"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["STONE_W"]).ToString("N" + 2);
                            row["BalanceStone_W"] = sumSTONE_W + (Comon.ConvertToDecimalPrice(row["OutQtyStone_W"]));
                            sumSTONE_W = Comon.ConvertToDecimalPrice(row["BalanceStone_W"]);


                            row["InQtyBagate_W"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["BAGET_W"]).ToString("N" + 2);
                            row["BalanceBagate"] = sumBAGET_W + (Comon.ConvertToDecimalPrice(row["OutQtyBagate"]));
                            sumBAGET_W = Comon.ConvertToDecimalPrice(row["BalanceBagate"]);

                            row["OutQtyDaimond_W"] = 0;
                            row["OutQtyStone_W"] = 0;
                            row["OutQtyBagate"] = 0;
                             



                            row["RecordType"] = CaseRecordType(dt.Rows[i]["RecordType"].ToString(), Comon.cLong(dt.Rows[i]["ID"]), gridView1.RowCount - 1);
                            row["TempRecordType"] = dt.Rows[i]["RecordType"].ToString();
                            _sampleData.Rows.Add(row);


                        }




                    }
                }



                #endregion
                #region SpentVochar
                dt.Rows.Clear();

                dt = Lip.SelectRecord(SpentVochar());
                if (strSQL != null || strSQL != "")
                {
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            row = _sampleData.NewRow();
                            row["Sn"] = _sampleData.Rows.Count + 1;
                            row["DateOfSort"] = dt.Rows[i]["TheDate"].ToString();
                            row["TheDate"] =(dt.Rows[i]["TheDate"].ToString());
                            row["OutQty"] = dt.Rows[i]["OutQty"].ToString();
                            row["OutPrice"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["OutPrice"]).ToString("N" + MySession.GlobalPriceDigits);

                            row["OutTotal"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["OutTotal"]).ToString("N" + MySession.GlobalPriceDigits);
                            row["Balance"] = sum - (Comon.ConvertToDecimalPrice(row["OutQty"]));
                            sum = Comon.ConvertToDecimalPrice(row["Balance"]);


                            row["OutQtyDaimond_W"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["DIAMOND_W"]).ToString("N" + 2);
                            row["BalanceDaimond_W"] = sumDIAMOND_W - (Comon.ConvertToDecimalPrice(row["OutQtyDaimond_W"]));
                            sumDIAMOND_W = Comon.ConvertToDecimalPrice(row["BalanceDaimond_W"]);


                            row["OutQtyStone_W"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["STONE_W"]).ToString("N" + 2);
                            row["BalanceStone_W"] = sumSTONE_W - (Comon.ConvertToDecimalPrice(row["OutQtyStone_W"]));
                            sumSTONE_W = Comon.ConvertToDecimalPrice(row["BalanceStone_W"]);


                            row["OutQtyBagate"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["BAGET_W"]).ToString("N" + 2);
                            row["BalanceBagate"] = sumBAGET_W - (Comon.ConvertToDecimalPrice(row["OutQtyBagate"]));
                            sumBAGET_W = Comon.ConvertToDecimalPrice(row["BalanceBagate"]);

                            row["InQtyDaimond_W"] = 0;
                            row["InQtyStone_W"] = 0;
                            row["InQtyBagate_W"] = 0;

                            row["ID"] = dt.Rows[i]["ID"].ToString();
                            row["RecordType"] = CaseRecordType(dt.Rows[i]["RecordType"].ToString(), Comon.cLong(dt.Rows[i]["ID"]), gridView1.RowCount - 1);
                            row["TempRecordType"] = dt.Rows[i]["RecordType"].ToString();

                            _sampleData.Rows.Add(row);

                        }
                    }

                }

                #endregion
                #region ReciptVochar
                dt.Rows.Clear();

                dt = Lip.SelectRecord(ReciptVochar());
                if (strSQL != null || strSQL != "")
                {
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            row = _sampleData.NewRow();
                            row["Sn"] = _sampleData.Rows.Count + 1;
                            row["DateOfSort"] = dt.Rows[i]["TheDate"].ToString();
                            row["TheDate"] =(dt.Rows[i]["TheDate"].ToString());
                            row["RegTime"] =  dt.Rows[i]["RegTime"].ToString();
                           
                            row["InQty"] = dt.Rows[i]["InQty"].ToString();
                            row["InPrice"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["InPrice"]).ToString("N" + MySession.GlobalPriceDigits);

                            row["InTotal"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["InTotal"]).ToString("N" + MySession.GlobalPriceDigits);
                            row["Balance"] = sum + (Comon.ConvertToDecimalPrice(row["InQty"]));
                            sum = Comon.ConvertToDecimalPrice(row["Balance"]);


                            row["InQtyDaimond_W"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["DIAMOND_W"]).ToString("N" + 2);
                            row["BalanceDaimond_W"] = sumDIAMOND_W + (Comon.ConvertToDecimalPrice(row["InQtyDaimond_W"]));
                            sumDIAMOND_W = Comon.ConvertToDecimalPrice(row["BalanceDaimond_W"]);



                            row["InQtyStone_W"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["STONE_W"]).ToString("N" + 2);
                            row["BalanceStone_W"] = sumSTONE_W + (Comon.ConvertToDecimalPrice(row["InQtyStone_W"]));
                            sumSTONE_W = Comon.ConvertToDecimalPrice(row["BalanceStone_W"]);


                            row["InQtyBagate_W"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["BAGET_W"]).ToString("N" + 2);
                            row["BalanceBagate"] = sumBAGET_W + (Comon.ConvertToDecimalPrice(row["InQtyBagate_W"]));
                            sumBAGET_W = Comon.ConvertToDecimalPrice(row["BalanceBagate"]);


                            row["OutQtyDaimond_W"] = 0;
                            row["OutQtyStone_W"] = 0;
                            row["OutQtyBagate"] = 0;

                            row["ID"] = dt.Rows[i]["ID"].ToString();
                            row["RecordType"] = CaseRecordType(dt.Rows[i]["RecordType"].ToString(), Comon.cLong(dt.Rows[i]["ID"]), gridView1.RowCount - 1);
                            row["TempRecordType"] = dt.Rows[i]["RecordType"].ToString();

                            _sampleData.Rows.Add(row);

                        }
                    }

                }

                #endregion

            }
            catch (Exception ex)
            {
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
                // Copy data from GridView into DataTable----------------------
                DataTable dt = new DataTable(); DataRow row;
                
                dt = _sampleData.Copy();
                DataView view = dt.DefaultView;
                view.Sort = "TheDate,RegTime ASC";
                _sampleData.Rows.Clear();
                decimal sumGold = 0;
                
                decimal QtyIn = 0;
                decimal QtyOut = 0;

                decimal QtyInDaimond = 0;
                decimal QtyOutDaimond = 0;

                decimal QtyInStone = 0;
                decimal QtyOutStone = 0;


                decimal QtyInBagate = 0;
                decimal QtyOutBagate = 0;


                decimal PriceIn = 0;
                decimal PriceOut= 0;

                for (int i = 0; i <= view.Count - 1; i++)
                {
                   
                    row = _sampleData.NewRow();
                    row = _sampleData.NewRow();
                    row["SN"] = view[i]["SN"]; ;
                    row["TheDate"] = Comon.ConvertSerialDateTo(view[i]["TheDate"].ToString());


                    row["DateOfSort"] = view[i]["DateOfSort"]; ;
                    row["InQty"] = Comon.ConvertToDecimalPrice(view[i]["InQty"]).ToString("N" + 2);
                    row["InPrice"] = Comon.ConvertToDecimalPrice(view[i]["InPrice"]).ToString("N" + MySession.GlobalPriceDigits);
                    row["CaratPrice"] = Comon.ConvertToDecimalPrice(view[i]["CaratPrice"]).ToString("N" + MySession.GlobalPriceDigits);

                    row["OutQty"] = view[i]["OutQty"]; ;
                    row["OutPrice"] = Comon.ConvertToDecimalPrice(view[i]["OutPrice"]).ToString("N" + MySession.GlobalPriceDigits);
                    row["OutTotal"] = Comon.ConvertToDecimalPrice(view[i]["OutTotal"]).ToString("N" + MySession.GlobalPriceDigits);

                    row["InTotal"] = Comon.ConvertToDecimalPrice(view[i]["InTotal"]).ToString("N" + MySession.GlobalPriceDigits);
                    row["Balance"] = Comon.ConvertToDecimalPrice(view[i]["Balance"]);



                    row["InQtyDaimond_W"] = Comon.ConvertToDecimalPrice(view[i]["InQtyDaimond_W"]);
                    row["OutQtyDaimond_W"] = Comon.ConvertToDecimalPrice(view[i]["OutQtyDaimond_W"]);
                    row["BalanceDaimond_W"] = Comon.ConvertToDecimalPrice(view[i]["BalanceDaimond_W"]);


                    row["InQtyStone_W"] = Comon.ConvertToDecimalPrice(view[i]["InQtyStone_W"]);
                    row["OutQtyStone_W"] = Comon.ConvertToDecimalPrice(view[i]["OutQtyStone_W"]);
                    row["BalanceStone_W"] = Comon.ConvertToDecimalPrice(view[i]["BalanceStone_W"]);


                    row["InQtyBagate_W"] = Comon.ConvertToDecimalPrice(view[i]["InQtyBagate_W"]);
                    row["OutQtyBagate"] = Comon.ConvertToDecimalPrice(view[i]["OutQtyBagate"]);
                    row["BalanceBagate"] = Comon.ConvertToDecimalPrice(view[i]["BalanceBagate"]);




                    row["ID"] = view[i]["ID"];
                    row["RecordType"] = view[i]["RecordType"];
                    row["TempRecordType"] = view[i]["TempRecordType"];


                    QtyIn += Comon.ConvertToDecimalQty(view[i]["InQty"]);
                    QtyOut += Comon.ConvertToDecimalQty(view[i]["OutQty"]);

                    QtyInDaimond += Comon.ConvertToDecimalQty(view[i]["InQtyDaimond_W"]);
                    QtyOutDaimond += Comon.ConvertToDecimalQty(view[i]["OutQtyDaimond_W"]);

                    QtyInStone += Comon.ConvertToDecimalQty(view[i]["InQtyStone_W"]);
                    QtyOutStone += Comon.ConvertToDecimalQty(view[i]["OutQtyStone_W"]);


                    QtyInBagate += Comon.ConvertToDecimalQty(view[i]["InQtyBagate_W"]);
                    QtyOutBagate += Comon.ConvertToDecimalQty(view[i]["OutQtyBagate"]);

                    PriceIn += Comon.ConvertToDecimalPrice(view[i]["CaratPrice"]);

                    PriceOut += Comon.ConvertToDecimalPrice(view[i]["OutPrice"]);

                    _sampleData.Rows.Add(row);

                }
                decimal QtyBalance = Comon.ConvertToDecimalQty(QtyIn - QtyOut);

                decimal QtyBalanceDaimond = Comon.ConvertToDecimalQty(QtyInDaimond - QtyOutDaimond);

                decimal QtyBalanceStone = Comon.ConvertToDecimalQty(QtyInStone - QtyOutStone);

                decimal QtyBalanceBagate = Comon.ConvertToDecimalPrice(QtyInBagate - QtyOutBagate);

               // PriceIn = Comon.ConvertToDecimalPrice(PriceIn + PriceIn / 100 * MySession.GlobalPercentVat);

              //  double Pricecoste = (Comon.cDbl(PriceIn)*1.74);

                double Pricecoste =  ( Comon.cDbl(PriceIn )  );


                decimal BalancePrice = Comon.ConvertToDecimalQty(Comon.cDbl(PriceOut) - Pricecoste);


                lblQtyInTotal.Text = Comon.ConvertToDecimalQty(QtyIn).ToString();
                lblQtyOutTotal.Text = Comon.ConvertToDecimalQty(QtyOut).ToString();
                lblQtyBalance.Text = Comon.ConvertToDecimalQty(QtyBalance).ToString();



                lblQtyInTotalDaimond.Text = Comon.ConvertToDecimalQty(QtyInDaimond).ToString();
                lblQtyOutTotalDaimond.Text = Comon.ConvertToDecimalQty(QtyOutDaimond).ToString();
                lblQtyBalanceDaimond.Text = Comon.ConvertToDecimalQty(QtyBalanceDaimond ).ToString();


                lblQtyInTotalStone.Text = Comon.ConvertToDecimalQty(QtyInStone).ToString();
                lblQtyOutTotalStone.Text = Comon.ConvertToDecimalQty(QtyOutStone).ToString();
                lblQtyBalanceStone.Text = Comon.ConvertToDecimalQty(QtyBalanceStone).ToString();


                lblQtyInTotalBagate.Text = Comon.ConvertToDecimalQty(QtyInBagate).ToString();
                lblQtyOutTotalBagate.Text = Comon.ConvertToDecimalQty(QtyOutBagate).ToString();
                lblQtyBalanceBagate.Text = Comon.ConvertToDecimalQty(QtyBalanceBagate).ToString();

                lblTotalInPrice.Text = Comon.ConvertToDecimalQty(Pricecoste).ToString();
                lblTotalOutPrice.Text = Comon.ConvertToDecimalQty(PriceOut).ToString();
                lblBalancePrice.Text = Comon.ConvertToDecimalQty(BalancePrice).ToString();



            }
            catch { }
        }
        public void btnShow_Click(object sender, EventArgs e)
        {

            try
            {
                if (txtBarCode.Text == string.Empty && txtStoreID.Text == string.Empty)
                    XtraMessageBox.Show((UserInfo.Language ==iLanguage.Arabic?"يجب ادخال قيمة في الحقول":"There is no Data to Show it "), "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                {
                    btnShow.Visible = false;
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                    Application.DoEvents();
                    ProcessBalance();
                    SortData();
                    gridControl1.DataSource = _sampleData;
                    if (gridView1.RowCount > 0)
                    {
                        btnShow.Visible = true;
                        txtStoreID.Enabled = false;
                        txtBarCode.Enabled = false;
                    }
                    else
                    {
                        Messages.MsgInfo(Messages.TitleInfo, MySession.GlobalLanguageName == iLanguage.Arabic ? "لايوجد بيانات لعرضها" : "There is no Data to show it");
                        btnShow.Visible = true;
                        DoNew();
                    }
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
        protected override void DoAddFrom()
        {
            try
            {
                lblQtyInTotal.Text = "";
                lblQtyOutTotal.Text = "";
                lblQtyBalance.Text = "";
                _sampleData.Clear();
                gridControl1.RefreshDataSource();
                txtBarCode.Text = "";
                txtStoreID.Text = "";
                txtStoreID_Validating(null, null);
                txtOldBarcodeID_Validating(null, null);
                txtBarCode.Enabled = true;
                txtStoreID.Enabled = true;



            }
            catch (Exception ex)
            {
                //WT.msgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        #region StringSQL
        public string ManufacturingOperations_Master()
        {
            try
            {
                filter = "";
                filter = "(.Manu_ManufacturingOperations_Master.BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + ") AND dbo.Manu_ManufacturingOperations_Master.Cancel =0   AND";
                strSQL = "";

                if (txtBarCode.Text != string.Empty)
                    filter = filter + " .Manu_ManufacturingOperations_Details.ParentBarCode  =" + Comon.cInt(txtBarCode.Text) + "  AND ";
                if (txtStoreID.Text != string.Empty)
                    filter = filter + " .Manu_ManufacturingOperations_Master.StoreID  =" + Comon.cInt(txtStoreID.Text) + "  AND ";

                filter = filter.Remove(filter.Length - 4, 4);

                strSQL = "SELECT Manu_ManufacturingOperations_Master.OperationDate As TheDate,'Manufacturing' AS RecordType, "

                 + " Manu_ManufacturingOperations_Details.Qty As InQty, Manu_ManufacturingOperations_Master.RegTime,0 As InPrice,"
                    + " 0 As InTotal,Manu_ManufacturingOperations_Master.OperationID As ID"
                    + " FROM dbo.Manu_ManufacturingOperations_Details LEFT OUTER JOIN"
                   + " dbo.Manu_ManufacturingOperations_Master ON dbo.Manu_ManufacturingOperations_Details.OperationID = dbo.Manu_ManufacturingOperations_Master.OperationID"
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

        //public string ManufacturingOperations_Details(){
        //    try
        //    {
        //        filter = "";
        //        filter = "(.Manu_ManufacturingOperations_Master.BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + ") AND dbo.Manu_ManufacturingOperations_Master.Cancel =0   AND";
        //        strSQL = "";

        //        if (txtBarCode.Text != string.Empty)
        //            filter = filter + " .Manu_ManufacturingOperations_StuffDetails.BarCode  =" + Comon.cInt(txtBarCode.Text) + "  AND ";
        //        if (txtStoreID.Text != string.Empty)
        //            filter = filter + " .Manu_ManufacturingOperations_Master.StoreID  =" + Comon.cInt(txtStoreID.Text) + "  AND ";

        //        filter = filter.Remove(filter.Length - 4, 4);

        //        strSQL = "SELECT dbo.Manu_ManufacturingOperations_Master.OperationDate AS TheDate, 'Manufacturing' AS RecordType, dbo.Manu_ManufacturingOperations_Master.OperationID AS ID, dbo.Manu_ManufacturingOperations_Master.RegTime, "
        //        + " dbo.Manu_ManufacturingOperations_StuffDetails.QTY AS OutQty, 0 AS OutPrice, 0 AS OutTotal"
        //        + " FROM dbo.Manu_ManufacturingOperations_StuffDetails LEFT OUTER JOIN"
        //        + " dbo.Manu_ManufacturingOperations_Master ON "
        //        + " dbo.Manu_ManufacturingOperations_StuffDetails.OperationID = dbo.Manu_ManufacturingOperations_Master.OperationID And "
        //        + " dbo.Manu_ManufacturingOperations_StuffDetails.BranchID = dbo.Manu_ManufacturingOperations_Master.BranchID"
        //        + "WHERE" + filter;
        //        Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());

        //    }
        //    catch (Exception ex)
        //    {
        //        SplashScreenManager.CloseForm(false);


        //        Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
        //    }

        //    return strSQL;

        //}


        public string PurchaseInvoice()
        {
            try
            {
                filter = "";
                filter = "(.Sales_PurchaseInvoiceDetails.BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + ") AND dbo.Sales_PurchaseInvoiceMaster.InvoiceID > 0 AND dbo.Sales_PurchaseInvoiceDetails.Cancel =0   AND";
                strSQL = "";

                if (txtBarCode.Text != string.Empty)
                    filter = filter + " .Sales_PurchaseInvoiceDetails.BarCode  ='" + txtBarCode.Text + "'  AND ";
                if (txtStoreID.Text != string.Empty)
                    filter = filter + " .Sales_PurchaseInvoiceDetails.StoreID  =" + Comon.cInt(txtStoreID.Text) + "  AND ";

                filter = filter.Remove(filter.Length - 4, 4);

                strSQL = "SELECT dbo.Sales_PurchaseInvoiceMaster.InvoiceDate AS TheDate,Sales_PurchaseInvoiceMaster.GoldUsing, 'PurchaseInvoice' AS RecordType, (dbo.Sales_PurchaseInvoiceDetails.QTY) AS InQty,(dbo.Sales_PurchaseInvoiceDetails.DIAMOND_W),(dbo.Sales_PurchaseInvoiceDetails.STONE_W),(dbo.Sales_PurchaseInvoiceDetails.BAGET_W) , dbo.Sales_PurchaseInvoiceMaster.RegTime, "
                + "dbo.Sales_PurchaseInvoiceDetails.CaratPrice, dbo.Sales_PurchaseInvoiceDetails.CostPrice AS InPrice, dbo.Sales_PurchaseInvoiceDetails.QTY * dbo.Sales_PurchaseInvoiceDetails.CostPrice AS InTotal, "
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
        public string GoodItems()
        {
            try
            {
                filter = "";
                filter = "(.Stc_GoodOpeningDetails.BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + ")  AND dbo.Stc_GoodOpeningDetails.Cancel =0   AND";
                strSQL = "";
                if (txtBarCode.Text != string.Empty)
                    filter = filter + " .Stc_GoodOpeningDetails.BarCode  ='" + txtBarCode.Text + "'  AND ";
                if (txtStoreID.Text != string.Empty)
                    filter = filter + " .Stc_GoodOpeningDetails.StoreID  =" + Comon.cInt(txtStoreID.Text) + "  AND ";

                filter = filter.Remove(filter.Length - 4, 4);
                strSQL = "SELECT dbo.Stc_GoodOpeningMaster.InvoiceDate AS TheDate, 'GoodsOpening' AS RecordType, (dbo.Stc_GoodOpeningDetails.QTY) AS InQty,(dbo.Stc_GoodOpeningDetails.DIAMOND_W),(dbo.Stc_GoodOpeningDetails.STONE_W),(dbo.Stc_GoodOpeningDetails.BAGET_W) , dbo.Stc_GoodOpeningMaster.RegTime, "
                + " dbo.Stc_GoodOpeningDetails.CostPrice AS InPrice,dbo.Stc_GoodOpeningDetails.CaratPrice, dbo.Stc_GoodOpeningDetails.QTY * dbo.Stc_GoodOpeningDetails.CostPrice AS InTotal, "
               + " dbo.Stc_GoodOpeningDetails.InvoiceID AS ID FROM dbo.Stc_GoodOpeningDetails LEFT OUTER JOIN dbo.Stc_GoodOpeningMaster ON  "
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
        public string SalesInvoiceReturn()
        {
            try
            {
                filter = "";
                filter = "(.Sales_SalesInvoiceReturnDetails.BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + ") AND dbo.Sales_SalesInvoiceReturnMaster.Cancel =0   AND";
                strSQL = "";

                if (txtBarCode.Text != string.Empty)
                    filter = filter + " .Sales_SalesInvoiceReturnDetails.BarCode ='" + txtBarCode.Text + "'  AND ";
                if (txtStoreID.Text != string.Empty)
                    filter = filter + " .Sales_SalesInvoiceReturnMaster.StoreID  =" + Comon.cInt(txtStoreID.Text) + "  AND ";

                filter = filter.Remove(filter.Length - 4, 4);
                strSQL = "SELECT dbo.Sales_SalesInvoiceReturnMaster.InvoiceDate AS TheDate, 'SalesInvoiceReturn' AS RecordType, (dbo.Sales_SalesInvoiceReturnDetails.QTY) AS InQty,(dbo.Sales_SalesInvoiceReturnDetails.DIAMOND_W),(dbo.Sales_SalesInvoiceReturnDetails.STONE_W),(dbo.Sales_SalesInvoiceReturnDetails.BAGET_W),dbo.Sales_SalesInvoiceReturnMaster.RegTime, "
                + " dbo.Sales_SalesInvoiceReturnDetails.SalePrice AS InPrice, CONVERT(DECIMAL(10, 2),  dbo.Sales_SalesInvoiceReturnDetails.SalePrice) AS InTotal,"
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

        public string SalesInvoice()
        {
            try
            {
                filter = "";
                filter = "(.Sales_SalesInvoiceDetails.BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + ") AND dbo.Sales_SalesInvoiceMaster.Cancel =0   AND";
                strSQL = "";

                if (txtBarCode.Text != string.Empty)
                    filter = filter + " .Sales_SalesInvoiceDetails.BarCode ='" + txtBarCode.Text + "'  AND ";
                if (txtStoreID.Text != string.Empty)
                    filter = filter + " .Sales_SalesInvoiceMaster.StoreID  =" + Comon.cInt(txtStoreID.Text) + "  AND ";

                filter = filter.Remove(filter.Length - 4, 4);

                strSQL = "SELECT dbo.Sales_SalesInvoiceMaster.InvoiceDate AS TheDate, 'SalesInvoice' AS RecordType, dbo.Sales_SalesInvoiceDetails.InvoiceID AS ID, dbo.Sales_SalesInvoiceMaster.RegTime, "
               + " (dbo.Sales_SalesInvoiceDetails.QTY) AS OutQty,dbo.Sales_SalesInvoiceDetails.DIAMOND_W ,dbo.Sales_SalesInvoiceDetails.STONE_W,dbo.Sales_SalesInvoiceDetails.BAGET_W, dbo.Sales_SalesInvoiceDetails.SalePrice AS OutPrice, CONVERT(DECIMAL(10, 2), dbo.Sales_SalesInvoiceDetails.QTY * dbo.Sales_SalesInvoiceDetails.SalePrice) AS OutTotal"
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

        string GetStrSQLSave()
        {
            try
            {
                //SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);               
                Application.DoEvents();
                string filter = "";
                filter = "(dbo.Sales_PurchaseInvoiceSaveMaster.BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + ") AND dbo.Sales_PurchaseInvoiceSaveMaster.Cancel =0   AND ";

                DataTable dt;

                if (txtBarCode.Text != string.Empty)
                    filter = filter + " dbo.Sales_PurchaseSaveInvoiceDetails.BarCode ='" + txtBarCode.Text + "'  AND ";
                if (txtStoreID.Text != string.Empty)
                    filter = filter + " dbo.Sales_PurchaseSaveInvoiceDetails.StoreID  =" + Comon.cInt(txtStoreID.Text) + "  AND ";
                filter = filter.Remove(filter.Length - 4, 4);
                strSQL = "SELECT dbo.Sales_PurchaseSaveInvoiceDetails.DIAMOND_W,Sales_PurchaseSaveInvoiceDetails.InvoiceID as ID, 'SalesInvoiceSave' AS RecordType  , dbo.Sales_PurchaseSaveInvoiceDetails.STONE_W, dbo.Sales_PurchaseSaveInvoiceDetails.BAGET_W, dbo.Sales_PurchaseSaveInvoiceDetails.CostPrice, "
              + " dbo.Sales_PurchaseSaveInvoiceDetails.CaratPrice, dbo.Sales_PurchaseSaveInvoiceDetails.CostPrice as InTotal, dbo.Sales_PurchaseSaveInvoiceDetails.CostPrice as InPrice, dbo.Sales_PurchaseSaveInvoiceDetails.Caliber, "
              + " dbo.Sales_PurchaseSaveInvoiceDetails.QTY as InQty, dbo.Sales_PurchaseInvoiceSaveMaster.InvoiceDate TheDate, Sales_PurchaseInvoiceSaveMaster.RegTime, dbo.Sales_PurchaseSaveInvoiceDetails.BarCode "
                       
              + " FROM     dbo.Sales_PurchaseInvoiceSaveMaster INNER JOIN "
                + "  dbo.Sales_PurchaseSaveInvoiceDetails ON dbo.Sales_PurchaseInvoiceSaveMaster.InvoiceID = dbo.Sales_PurchaseSaveInvoiceDetails.InvoiceID "                                                            
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
        string GetStrSQLSaveReturn()
        {
            try
            {
                //SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);               
                Application.DoEvents();
                string filter = "";
                filter = "(dbo.Sales_PurchaseInvoiceSaveReturnMaster.BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + ") AND dbo.Sales_PurchaseInvoiceSaveReturnMaster.Cancel =0   AND ";

                DataTable dt;

                if (txtBarCode.Text != string.Empty)
                    filter = filter + " dbo.Sales_PurchaseSaveInvoiceReturnDetails.BarCode ='" + txtBarCode.Text + "'  AND ";
                if (txtStoreID.Text != string.Empty)
                    filter = filter + " dbo.Sales_PurchaseSaveInvoiceReturnDetails.StoreID  =" + Comon.cInt(txtStoreID.Text) + "  AND ";
                filter = filter.Remove(filter.Length - 4, 4);
                strSQL = "SELECT dbo.Sales_PurchaseSaveInvoiceReturnDetails.DIAMOND_W,Sales_PurchaseSaveInvoiceReturnDetails.InvoiceID as ID, 'SalesInvoiceSaveReturn' AS RecordType  , dbo.Sales_PurchaseSaveInvoiceReturnDetails.STONE_W, dbo.Sales_PurchaseSaveInvoiceReturnDetails.BAGET_W, dbo.Sales_PurchaseSaveInvoiceReturnDetails.CostPrice as OutPrice, "
              + " dbo.Sales_PurchaseSaveInvoiceReturnDetails.CaratPrice, dbo.Sales_PurchaseSaveInvoiceReturnDetails.CostPrice, dbo.Sales_PurchaseSaveInvoiceReturnDetails.CostPrice as OutTotal,  dbo.Sales_PurchaseSaveInvoiceReturnDetails.Caliber, "
              + " (dbo.Sales_PurchaseSaveInvoiceReturnDetails.QTY) AS OutQty, dbo.Sales_PurchaseInvoiceSaveReturnMaster.InvoiceDate TheDate, Sales_PurchaseInvoiceSaveReturnMaster.RegTime, dbo.Sales_PurchaseSaveInvoiceReturnDetails.BarCode "
              + " FROM     dbo.Sales_PurchaseInvoiceSaveReturnMaster INNER JOIN "
                + "  dbo.Sales_PurchaseSaveInvoiceReturnDetails ON dbo.Sales_PurchaseInvoiceSaveReturnMaster.InvoiceID = dbo.Sales_PurchaseSaveInvoiceReturnDetails.InvoiceID "
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

        public string PurchaseInvoiceReturn()
        {
            try
            {
                filter = " ";
                filter = "(dbo.Sales_PurchaseInvoiceReturnMaster.BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + ") AND dbo.Sales_PurchaseInvoiceReturnMaster.Cancel =0   AND";
                strSQL = "";

                if (txtBarCode.Text != string.Empty)
                    filter = filter + " dbo.Sales_PurchaseInvoiceReturnDetails.BarCode  ='" + txtBarCode.Text + "'  AND ";
                if (txtStoreID.Text != string.Empty)
                    filter = filter + " dbo.Sales_PurchaseInvoiceReturnMaster.StoreID  =" + Comon.cInt(txtStoreID.Text) + "  AND ";

                filter = filter.Remove(filter.Length - 4, 4);
                strSQL = "SELECT dbo.Sales_PurchaseInvoiceReturnMaster.InvoiceDate AS TheDate, 'PurchaseInvoiceReturn' AS RecordType,dbo.Sales_PurchaseInvoiceReturnDetails.InvoiceID AS ID , dbo.Sales_PurchaseInvoiceReturnMaster.RegTime"
                + " , (dbo.Sales_PurchaseInvoiceReturnDetails.QTY) AS OutQty,(dbo.Sales_PurchaseInvoiceReturnDetails.DIAMOND_W),(dbo.Sales_PurchaseInvoiceReturnDetails.STONE_W),(dbo.Sales_PurchaseInvoiceReturnDetails.BAGET_W), dbo.Sales_PurchaseInvoiceReturnDetails.CostPrice AS OutPrice,CONVERT(DECIMAL(10, 2),  dbo.Sales_PurchaseInvoiceReturnDetails.CostPrice) AS OutTotal "
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

        public string ItemsDismantlingTo()
        {
            try
            {
                filter = "";
                filter = "(.Stc_ItemsDismantlingMaster.BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + ") AND dbo.Stc_ItemsDismantlingMaster.Cancel =0   AND";
                strSQL = "";

                if (txtBarCode.Text != string.Empty)
                    filter = filter + " .Stc_ItemsDismantlingDetails.FromBarCode ='" + txtBarCode.Text + "'  AND ";
                if (txtStoreID.Text != string.Empty)
                    filter = filter + " .Stc_ItemsDismantlingMaster.StoreID  =" + Comon.cInt(txtStoreID.Text) + "  AND ";

                filter = filter.Remove(filter.Length - 4, 4);

                strSQL = "SELECT  dbo.Stc_ItemsDismantlingMaster.DismantleDate AS TheDate, 'ItemsDismantling' AS RecordType, dbo.Stc_ItemsDismantlingMaster.DismantleID AS ID,"
               + " dbo.Stc_ItemsDismantlingDetails.QTY AS OutQty, dbo.Stc_ItemsDismantlingMaster.RegTime FROM dbo.Stc_ItemsDismantlingDetails INNER JOIN dbo.Stc_ItemsDismantlingMaster ON "
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


        public string ItemsDismantlingFrom()
        {
            try
            {
                filter = "";
                filter = "(.Stc_ItemsDismantlingMaster.BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + ") AND dbo.Stc_ItemsDismantlingMaster.Cancel =0   AND";
                strSQL = "";

                if (txtBarCode.Text != string.Empty)
                    filter = filter + " dbo.Stc_ItemsDismantlingDetails.ToBarCode ='" + txtBarCode.Text + "'  AND ";
                if (txtStoreID.Text != string.Empty)
                    filter = filter + " dbo.Stc_ItemsDismantlingMaster.StoreID  =" + Comon.cInt(txtStoreID.Text) + "  AND ";

                filter = filter.Remove(filter.Length - 4, 4);

                strSQL = "SELECT  dbo.Stc_ItemsDismantlingMaster.DismantleDate AS TheDate, 'ItemsDismantling' AS RecordType, dbo.Stc_ItemsDismantlingMaster.DismantleID AS ID,"
                + " dbo.Stc_ItemsDismantlingDetails.DismantledQTY AS InQty, dbo.Stc_ItemsDismantlingMaster.RegTime FROM dbo.Stc_ItemsDismantlingDetails INNER JOIN dbo.Stc_ItemsDismantlingMaster ON "
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

        public string ItemsTransferTo()
        {
            try
            {
                filter = " ";
                filter = "(.Stc_ItemsTransferMaster.ToBranchID = " + Comon.cInt(cmbBranchesID.EditValue) + ") AND dbo.Stc_ItemsTransferMaster.Cancel =0   AND";
                strSQL = "";

                if (txtBarCode.Text != string.Empty)
                    filter = filter + " .Stc_ItemsTransferDetails.BarCode ='" + txtBarCode.Text + "'  AND ";
                if (txtStoreID.Text != string.Empty)
                    filter = filter + " .Stc_ItemsTransferMaster.ToStoreID  =" + Comon.cInt(txtStoreID.Text) + "  AND ";

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

        public string ItemsTransferFrom()
        {
            try
            {
                filter = " ";

                filter = "(.Stc_ItemsTransferMaster.FromBranchID = " + Comon.cInt(cmbBranchesID.EditValue) + ") AND dbo.Stc_ItemsTransferMaster.Cancel =0   AND";
                strSQL = "";

                if (txtBarCode.Text != string.Empty)
                    filter = filter + " .Stc_ItemsTransferDetails.BarCode  ='" + txtBarCode.Text + "'  AND ";
                if (txtStoreID.Text != string.Empty)
                    filter = filter + " .Stc_ItemsTransferMaster.FromStoreID  =" + Comon.cInt(txtStoreID.Text) + "  AND ";

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
        public string SpentVochar()
        {

            try
            {
                filter = "";
                filter = "(.Acc_SpendVoucherDetails.BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + ") AND dbo.Acc_SpendVoucherMaster.Cancel =0   AND";
                strSQL = "";

                if (txtBarCode.Text != string.Empty)
                    filter = filter + " .Acc_SpendVoucherDetails.BarCode ='" + txtBarCode.Text + "'  AND ";
                
                filter = filter.Remove(filter.Length - 4, 4);

                strSQL = "SELECT dbo.Acc_SpendVoucherMaster.SpendVoucherDate AS TheDate, 'SpentVochar' AS RecordType, dbo.Acc_SpendVoucherDetails.SpendVoucherID AS ID, "
               + " dbo.Acc_SpendVoucherMaster.RegTime, dbo.Acc_SpendVoucherDetails.WeightGold AS OutQty, 0 AS OutPrice,(dbo.Acc_SpendVoucherDetails.DIAMOND_W),(dbo.Acc_SpendVoucherDetails.STONE_W),(dbo.Acc_SpendVoucherDetails.BAGET_W), "
                + " 0 AS OutTotal FROM dbo.Acc_SpendVoucherDetails"
                + " INNER JOIN dbo.Acc_SpendVoucherMaster ON dbo.Acc_SpendVoucherDetails.SpendVoucherID = dbo.Acc_SpendVoucherMaster.SpendVoucherID AND dbo.Acc_SpendVoucherDetails.BranchID"
                + " = dbo.Acc_SpendVoucherMaster.BranchID WHERE " + filter;
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());

            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);


                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            return strSQL;
        }


        public string ReciptVochar()
        {

            try
            {
                filter = "";
                filter = "(.Acc_ReceiptVoucherDetails.BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + ") AND dbo.Acc_ReceiptVoucherMaster.Cancel =0   AND";
                strSQL = "";

                if (txtBarCode.Text != string.Empty)
                    filter = filter + " .Acc_ReceiptVoucherDetails.Barcode ='" + txtBarCode.Text + "'  AND ";

                filter = filter.Remove(filter.Length - 4, 4);

                strSQL = "SELECT dbo.Acc_ReceiptVoucherMaster.ReceiptVoucherDate AS TheDate, 'ReciptVochar' AS RecordType, dbo.Acc_ReceiptVoucherDetails.ReceiptVoucherID AS ID, "
                + " dbo.Acc_ReceiptVoucherMaster.RegTime, dbo.Acc_ReceiptVoucherDetails.WeightGold AS InQty, 0 AS InPrice, (dbo.Acc_ReceiptVoucherDetails.DIAMOND_W),(dbo.Acc_ReceiptVoucherDetails.STONE_W),(dbo.Acc_ReceiptVoucherDetails.BAGET_W) ,"
                + " 0 AS InTotal FROM dbo.Acc_ReceiptVoucherDetails"
                + " INNER JOIN dbo.Acc_ReceiptVoucherMaster ON dbo.Acc_ReceiptVoucherDetails.ReceiptVoucherID  = dbo.Acc_ReceiptVoucherMaster.ReceiptVoucherID  AND dbo.Acc_ReceiptVoucherDetails.BranchID"
                + " = dbo.Acc_ReceiptVoucherMaster.BranchID WHERE " + filter;
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());

            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);


                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            return strSQL;
        }
        public string ItemsOutOnBail()
        {

            try
            {
                filter = "";
                filter = "(.Stc_ItemsOutonBail_Details.BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + ") AND dbo.Stc_ItemsOutonBail_Master.Cancel =0   AND";
                strSQL = "";

                if (txtBarCode.Text != string.Empty)
                    filter = filter + " .Stc_ItemsOutonBail_Details.BarCode ='" + txtBarCode.Text + "'  AND ";
                if (txtStoreID.Text != string.Empty)
                    filter = filter + " .Stc_ItemsOutonBail_Master.StoreID  =" + Comon.cInt(txtStoreID.Text) + "  AND ";

                filter = filter.Remove(filter.Length - 4, 4);

                strSQL = "SELECT dbo.Stc_ItemsOutonBail_Master.InvoiceDate AS TheDate, 'ItemsOutOnBail' AS RecordType, dbo.Stc_ItemsOutonBail_Details.InvoiceID AS ID, "
               + " dbo.Stc_ItemsOutonBail_Master.RegTime, dbo.Stc_ItemsOutonBail_Details.QTY AS OutQty, (dbo.Stc_ItemsOutonBail_Details.DIAMOND_W),(dbo.Stc_ItemsOutonBail_Details.STONE_W),(dbo.Stc_ItemsOutonBail_Details.BAGET_W), dbo.Stc_ItemsOutonBail_Details.SalePrice AS OutPrice, "
                + " CONVERT(DECIMAL(10, 2),  dbo.Stc_ItemsOutonBail_Details.SalePrice) AS OutTotal FROM dbo.Stc_ItemsOutonBail_Details"
                + " INNER JOIN dbo.Stc_ItemsOutonBail_Master ON dbo.Stc_ItemsOutonBail_Details.InvoiceID = dbo.Stc_ItemsOutonBail_Master.InvoiceID AND dbo.Stc_ItemsOutonBail_Details.BranchID"
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

        public string ItemsInOnBail()
        {
            try
            {

                filter = "";
                filter = "(.Stc_ItemsInonBail_Details.BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + ") AND dbo.Stc_ItemsInonBail_Master.Cancel =0   AND";
                strSQL = "";

                if (txtBarCode.Text != string.Empty)
                    filter = filter + " .Stc_ItemsInonBail_Details.BarCode ='" + txtBarCode.Text + "'  AND ";
                if (txtStoreID.Text != string.Empty)
                    filter = filter + " .Stc_ItemsInonBail_Master.StoreID  =" + Comon.cInt(txtStoreID.Text) + "  AND ";

                filter = filter.Remove(filter.Length - 4, 4);
                strSQL = "SELECT dbo.Stc_ItemsInonBail_Master.InvoiceDate AS TheDate, 'ItemsInOnBail' AS RecordType, dbo.Stc_ItemsInonBail_Details.InvoiceID AS ID, "
               + " dbo.Stc_ItemsInonBail_Master.RegTime, dbo.Stc_ItemsInonBail_Details.QTY AS InQty, (dbo.Stc_ItemsInonBail_Details.DIAMOND_W),(dbo.Stc_ItemsInonBail_Details.STONE_W),(dbo.Stc_ItemsInonBail_Details.BAGET_W), dbo.Stc_ItemsInonBail_Details.CostPrice AS InPrice, "
               + " CONVERT(DECIMAL(10, 2), dbo.Stc_ItemsInonBail_Details.QTY * dbo.Stc_ItemsInonBail_Details.CostPrice) AS InTotal FROM dbo.Stc_ItemsInonBail_Details"
               + " INNER JOIN dbo.Stc_ItemsInonBail_Master ON dbo.Stc_ItemsInonBail_Details.InvoiceID = dbo.Stc_ItemsInonBail_Master.InvoiceID AND dbo.Stc_ItemsInonBail_Details.BranchID"
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
        string InPrice()
        {

            strSQL = "";
            strSQL = "SELECT TOP (1) CostPrice  as InPrice FROM dbo.Sales_PurchaseInvoiceDetails WHERE (Cancel = 0) AND "
                          + " (BarCode = '" + txtBarCode.Text + "') AND (BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + ") ORDER BY ID DESC";
            Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());

            return strSQL;





        }
        string OutPrice()
        {

            strSQL = "";
            strSQL = "SELECT TOP (1) CostPrice  as OutPrice FROM dbo.Sales_PurchaseInvoiceDetails WHERE (Cancel = 0) AND "
                           + " (BarCode = '" + txtBarCode.Text + "') AND (BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + ") ORDER BY ID DESC";
            Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());

            return strSQL;





        }
        #region GetTypeRecord
        public string CaseRecordType(string recordType, long ID, int i)
        {

            string _recordType = "";
            switch (recordType)
            {

                case "PurchaseInvoiceGold":
                    if (UserInfo.Language == iLanguage.Arabic)


                        _recordType = "فاتورة مشتريات ذهب ";
                        else
                        _recordType = "Purchase Invoice Gold";

                   
                    break;
                case "PurchaseInvoiceAlmas":
                    if (UserInfo.Language == iLanguage.Arabic)


                        _recordType = " فاتورة مشتريات الماس";
                    else
                        _recordType = "Purchase Invoice  Almas";


                    break;
                case "SalesInvoiceSave":
                    if (UserInfo.Language == iLanguage.Arabic)


                        _recordType = " سند العرض";
                    else
                        _recordType = "Purchase save Invoice";


                    break;
                case "SalesInvoiceSaveReturn":
                    if (UserInfo.Language == iLanguage.Arabic)


                        _recordType = "مردود سند العرض";
                    else
                        _recordType = "Purchase save Invoice Return";


                    break;

                case "SpentVochar":
                    if (UserInfo.Language == iLanguage.Arabic)
                        _recordType = "سند صرف";
                    else
                        _recordType = "Spend";
                    break;
                case "ReciptVochar":
                    if (UserInfo.Language == iLanguage.Arabic)
                        _recordType = "سند قبض";
                    else
                        _recordType = "Recipt";
                    break;

                case "GoodsOpening":
                    {
                        if (UserInfo.Language == iLanguage.Arabic)


                            _recordType = "بضاعة اول مدة ";
                        else
                            _recordType = "Goods Opening";

                   

                           
                    }
                    break;
                case "PurchaseInvoiceReturn":
                    if (UserInfo.Language == iLanguage.Arabic)
                        _recordType = "مردود فاتورة مشتريات";
                    else
                        _recordType = "Purchase Invoice Return";

                    break;
                case "SalesInvoice":
                    if (UserInfo.Language == iLanguage.Arabic)
                        _recordType = "فاتورة مبيعات";
                    else
                        _recordType = "Sales Invoice";


                    break;
                case "SalesInvoiceReturn":
                    if (UserInfo.Language == iLanguage.Arabic)
                        _recordType = "مردود فاتورة مبيعات";
                    else
                        _recordType = "Sales Invoice Return";
                    break;
                case "ItemsDismantling":
                    if (UserInfo.Language == iLanguage.Arabic)
                        _recordType = "تـفـكـيـك وتجـمـيع مــواد";
                    else
                        _recordType = "Dismantling and Assembling Items";
                    break;
                case "ItemsTransfer":

                    if (UserInfo.Language == iLanguage.Arabic)
                        _recordType = "مناقلة مواد";
                    else
                        _recordType = "Items Transfer";
                    break;
                case "ItemsOutOnBail":
                    if (UserInfo.Language == iLanguage.Arabic)
                        _recordType = "صرف مخزني";
                    else
                        _recordType = "Items Out On Bail";
                    break;
                case "Manufacturing":
                    if (UserInfo.Language == iLanguage.Arabic)
                        _recordType = "تصنيع";
                    else
                        _recordType = "Manufacturing";
                    break;

                case "ItemsInOnBail":

                    if (UserInfo.Language == iLanguage.Arabic)
                        _recordType = "استلام مخزني ";
                    else
                        _recordType = "Items InOnBail";
                    break;



            }




            return _recordType;






        }

        #endregion



        //public void  SortData(){


        //        Dim dt As New DataTable()
        //        Dim dcs As DataColumn() = New DataColumn() 
        //        For Each c As DataGridViewColumn In GridView.Columns
        //            Dim dc As New DataColumn()
        //            dc.ColumnName = c.Name
        //            dc.DataType = System.Type.GetType("System.String") 'c.ValueType
        //            dt.Columns.Add(dc)
        //        Next
        //        For Each r As DataGridViewRow In GridView.Rows
        //            Dim drow As DataRow = dt.NewRow()
        //            For Each cell As DataGridViewCell In r.Cells
        //                drow(cell.OwningColumn.Name) = cell.Value
        //            Next
        //            dt.Rows.Add(drow)
        //        Next

        //        Dim view As DataView = dt.DefaultView
        //        view.Sort = "dgvColTheDate ASC,dgvColRegTime ASC"
        //        GridView.Rows.Clear()

        //        Dim sum As Decimal = 0
        //        For i As Integer = 0 To view.Count - 1
        //            GridView.Rows.Add()
        //            GridView.Rows(i).Cells(dgvColTheDate.Name).Value = view(i)("dgvColTheDate")
        //            GridView.Rows(i).Cells(dgvColRecordType.Name).Value = view(i)("dgvColRecordType")
        //            GridView.Rows(i).Cells(dgvColID.Name).Value = view(i)("dgvColID")
        //            GridView.Rows(i).Cells(dgvColInQty.Name).Value = view(i)("dgvColInQty")
        //            GridView.Rows(i).Cells(dgvColInPrice.Name).Value = view(i)("dgvColInPrice")
        //            GridView.Rows(i).Cells(dgvColInTotal.Name).Value = view(i)("dgvColInTotal")
        //            GridView.Rows(i).Cells(dgvColOutQty.Name).Value = view(i)("dgvColOutQty")
        //            GridView.Rows(i).Cells(dgvColOutPrice.Name).Value = view(i)("dgvColOutPrice")
        //            GridView.Rows(i).Cells(dgvColOutTotal.Name).Value = view(i)("dgvColOutTotal")
        //            GridView.Rows(i).Cells(dgvColRegTime.Name).Value = view(i)("dgvColRegTime")
        //            GridView.Rows(i).Cells(dgvColTempRecordType.Name).Value = view(i)("dgvColTempRecordType")
        //            GridView.Rows(i).Cells(dgvColBalance.Name).Value = sum + ConvertToDecimalQty(view(i)("dgvColInQty")) - ConvertToDecimalQty(view(i)("dgvColOutQty"))
        //            sum = GridView.Rows(i).Cells(dgvColBalance.Name).Value
        //        Next

        //    Catch ex As Exception
        //        WT.msgError(Me.GetType.Name, System.Reflection.MethodBase.GetCurrentMethod.Name)
        //    End Try
        //End Sub
        //}
        #endregion
        #region Function
        ////////////// print_COde//////////////////////////////////
        protected override void DoPrint()
        {
            try
            {
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                /******************** Report Body *************************/
                bool IncludeHeader = true;
                ReportName = "rptItemBalance";
                string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");
                if (UserInfo.Language == iLanguage.English)
                    rptFormName = ReportName + "Arb";
                XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);
                /********************** Master *****************************/
                rptForm.RequestParameters = false;
                rptForm.Parameters["ItemName"].Value = lblBarCodeName.Text.Trim().ToString();
                rptForm.Parameters["BarCode"].Value = txtBarCode.Text.Trim().ToString();
                rptForm.Parameters["storeName"].Value = lblStoreName .Text.Trim().ToString();
                for (int i = 0; i < rptForm.Parameters.Count; i++)
                    rptForm.Parameters[i].Visible = false;
                /********************** Details ****************************/
                var dataTable = new dsReports.rptItemBalanceDataTable();
                for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                {
                    var row = dataTable.NewRow();
                    row["#"] = i + 1;
                    row["TheDate"] = gridView1.GetRowCellValue(i, "TheDate").ToString();
                    row["ID"] = gridView1.GetRowCellValue(i, "ID").ToString();
                    row["RecordType"] = gridView1.GetRowCellValue(i, "RecordType").ToString();
                    row["InQty"] = gridView1.GetRowCellValue(i, "InQty").ToString();
                    row["InPrice"] = gridView1.GetRowCellValue(i, "InPrice").ToString();
                    row["InTotal"] = gridView1.GetRowCellValue(i, "InTotal").ToString();
                    row["OutQty"] = gridView1.GetRowCellValue(i, "OutQty").ToString();
                    row["OutPrice"] = gridView1.GetRowCellValue(i, "OutPrice").ToString();
                    row["OutTotal"] = gridView1.GetRowCellValue(i, "OutTotal").ToString();
                    row["Balance"] = gridView1.GetRowCellValue(i, "Balance").ToString();

                    row["InDIAMOND_W"] = gridView1.GetRowCellValue(i, "InQtyDaimond_W").ToString();
                    row["OutDIAMOND_W"] = gridView1.GetRowCellValue(i, "OutQtyDaimond_W").ToString();
                    row["BalanceDIAMOND_W"] = gridView1.GetRowCellValue(i, "BalanceDaimond_W").ToString();

                    row["InSTONE_W"] = gridView1.GetRowCellValue(i, "InQtyStone_W").ToString();
                    row["OutSTONE_W"] = gridView1.GetRowCellValue(i, "OutQtyStone_W").ToString();
                    row["BalanceSTONE_W"] = gridView1.GetRowCellValue(i, "BalanceStone_W").ToString();

                    row["InBAGET_W"] = gridView1.GetRowCellValue(i, "InQtyBagate_W").ToString();
                    row["OutBAGET_W"] = gridView1.GetRowCellValue(i, "OutQtyBagate").ToString();
                    row["BalanceBAGET_W"] = gridView1.GetRowCellValue(i, "BalanceBagate").ToString();


                    dataTable.Rows.Add(row);
                }
                rptForm.DataSource = dataTable;
                rptForm.DataMember = "rptItemBalance";
                /******************** Report Binding ************************/
                XRSubreport subreport = (XRSubreport)rptForm.FindControl("xrSubreport1", true);
                subreport.Visible = IncludeHeader;
                subreport.ReportSource = ReportComponent.CompanyHeader();
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

        /// <summary>
        /// //////////////////////////////////////////////////////////
        /// </summary>
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
                    //  PrepareSearchQuery.Search(txtStoreID, lblStoreName, "StoreID", "اسم الـمـســتـودع","رقم الـمـســتـودع");
                    PrepareSearchQuery.Search(txtStoreID, lblStoreName, "StoreID", "اسـم الـمـســتـودع", "رقم الـمـســتـودع");
                else
                    PrepareSearchQuery.Search(txtStoreID, lblStoreName, "StoreID", "Store Name", "Store ID");
            }


            else if (FocusedControl.Trim() == txtBarCode.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Search(txtBarCode, lblBarCodeName, "BarCodeForPurchaseInvoice", "اسـم الـمـادة", "البـاركـود");
                else
                    PrepareSearchQuery.Search(txtBarCode, lblBarCodeName, "BarCodeForPurchaseInvoice", "Item Name", "BarCode");
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

        public void txtStoreID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT   " + (UserInfo.Language == iLanguage.Arabic ? "ArbName" : "EngName") + "  as StoreName FROM Stc_Stores WHERE StoreID =" + Comon.cInt(txtStoreID.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, "arb"); 
                CSearch.ControlValidating(txtStoreID, lblStoreName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

        }

        public void txtOldBarcodeID_Validating(object sender, CancelEventArgs e)
        {
            try
            {


                //      strSQLForBarcode = " SELECT   TOP (1)  Stc_Items.ArbName AS ItemName, Sales_SalesInvoiceDetails.SizeID, Stc_SizingUnits.ArbName AS SizeName, "
                //+ " Sales_SalesInvoiceDetails.BarCode, Sales_SalesInvoiceDetails.ExpiryDate, Sales_SalesInvoiceDetails.InvoiceID, Stc_ItemsSizes.ArbName AS Size, "
                // + " Stc_ItemsBrands.ArbName AS BrandName   FROM  Stc_ItemsSizes RIGHT OUTER JOIN   Stc_Items ON Stc_ItemsSizes.SizeID = Stc_Items.SizeID LEFT OUTER JOIN "
                // + " Stc_ItemsBrands ON Stc_Items.BrandID = Stc_ItemsBrands.BrandID RIGHT OUTER JOIN    Sales_SalesInvoiceDetails LEFT OUTER JOIN "
                //+ " Stc_SizingUnits ON Sales_SalesInvoiceDetails.SizeID = Stc_SizingUnits.SizeID ON Stc_Items.ItemID = Sales_SalesInvoiceDetails.ItemID "
                //+ "  WHERE  (Sales_SalesInvoiceDetails.BarCode ='" + txtBarCode.Text + "') AND (Sales_SalesInvoiceDetails.Cancel = 0)";
                string strSQLForBarcode = " SELECT   TOP (1)  Stc_Items.ArbName AS ItemName   FROM  Stc_Items RIGHT OUTER JOIN       Sales_PurchaseInvoiceDetails LEFT OUTER JOIN "
             + " Stc_SizingUnits ON Sales_PurchaseInvoiceDetails.SizeID = Stc_SizingUnits.SizeID ON Stc_Items.ItemID = Sales_PurchaseInvoiceDetails.ItemID "
             + "  WHERE  (Sales_PurchaseInvoiceDetails.BarCode ='" + txtBarCode.Text + "') AND (Sales_PurchaseInvoiceDetails.Cancel = 0)";

                //    strSQLForBarcode = " SELECT   TOP (1) Sales_PurchaseInvoiceDetails.ItemID, Stc_Items.ArbName AS ItemName, Sales_PurchaseInvoiceDetails.SizeID, Stc_SizingUnits.ArbName AS SizeName, "
                //+ " Sales_PurchaseInvoiceDetails.BarCode, Sales_PurchaseInvoiceDetails.ExpiryDate, Sales_PurchaseInvoiceDetails.InvoiceID, Stc_ItemsSizes.ArbName AS Size, "
                //+ " Stc_ItemsBrands.ArbName AS BrandName   FROM  Stc_ItemsSizes RIGHT OUTER JOIN   Stc_Items ON Stc_ItemsSizes.SizeID = Stc_Items.SizeID LEFT OUTER JOIN "
                //+ " Stc_ItemsBrands ON Stc_Items.BrandID = Stc_ItemsBrands.BrandID RIGHT OUTER JOIN    Sales_PurchaseInvoiceDetails LEFT OUTER JOIN "
                //+ " Stc_SizingUnits ON Sales_PurchaseInvoiceDetails.SizeID = Stc_SizingUnits.SizeID ON Stc_Items.ItemID = Sales_PurchaseInvoiceDetails.ItemID "
                //+ "  WHERE  (Sales_PurchaseInvoiceDetails.BarCode ='" + txtBarCode.Text + "') AND (Sales_PurchaseInvoiceDetails.Cancel = 0)";
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
        public string GetItemData(string barcode)
        {


            getItemSQL = " SELECT   TOP (1)  Stc_Items.ArbName AS ItemName "

          + "   FROM  Stc_Items    "
          + "  RIGHT OUTER JOIN   Sales_PurchaseInvoiceDetails "
          + "  ON Stc_Items.ItemID = Sales_PurchaseInvoiceDetails.ItemID "
          + "  WHERE  (Sales_PurchaseInvoiceDetails.BarCode ='" + barcode + "') AND (Sales_PurchaseInvoiceDetails.Cancel = 0)";

            return getItemSQL;



        }
        #endregion
        protected override void DoSearch()
        {
            try
            {
                Find();
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void frmItemBalance_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
                Find();
            if (e.KeyCode == Keys.F5)
                DoPrint();



        }

        private void frmItemBalance_MouseClick(object sender, MouseEventArgs e)
        {






        }
        public void getVAluation()
        {
            txtOldBarcodeID_Validating(null, null);
            btnShow_Click(null, null);



        }
        private void gridView1_RowClick(object sender, RowClickEventArgs e)
        {






        }
        public void StoreChange(long stor)
        {
            try{
            txtStoreID.Text = stor.ToString();
            txtStoreID_Validating(null, null);
            }
            catch { }
        }
        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            GridView view = sender as GridView;
try{
            switch (view.GetFocusedRowCellValue("TempRecordType").ToString())
            {
                case "PurchaseInvoice":
                        frmCashierPurchaseDaimond frm = new frmCashierPurchaseDaimond();
                    if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
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
                    //frmItemsOutOnBail frm11 = new frmItemsOutOnBail();
                    //if (Permissions.UserPermissionsFrom(frm11, frm11.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                    //{
                    //    if (UserInfo.Language == iLanguage.English)
                    //        ChangeLanguage.EnglishLanguage(frm11);
                    //    frm11.Show();
                    //    frm11.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    //}
                    //else
                    //    frm11.Dispose();


                    break;


                case "ItemsInOnBail":
                    frmItemsInonBail frm12 = new frmItemsInonBail();
                    if (Permissions.UserPermissionsFrom(frm12, frm12.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
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
                    frmGoodsOpeningOld frm1 = new frmGoodsOpeningOld();
                    if (Permissions.UserPermissionsFrom(frm1, frm1.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
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
                    //frmItemsDismantling frm10 = new frmItemsDismantling();
                    //if (Permissions.UserPermissionsFrom(frm10, frm10.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                    //{
                    //    if (UserInfo.Language == iLanguage.English)
                    //        ChangeLanguage.EnglishLanguage(frm10);
                    //    frm10.Show();
                    //    frm10.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    //}
                    //else
                    //    frm10.Dispose();
                    //break;
                case "SalesInvoiceReturn":
                    frmSalesInvoiceReturn frm2 = new frmSalesInvoiceReturn();
                    if (Permissions.UserPermissionsFrom(frm2, frm2.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
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

                    frmCashierSalesAlmas frm3 = new frmCashierSalesAlmas();
                    if (Permissions.UserPermissionsFrom(frm3, frm3.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
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
                    frmCashierPurchaseReturnDaimond frm4 = new frmCashierPurchaseReturnDaimond();
                    if (Permissions.UserPermissionsFrom(frm4, frm4.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm4);
                        frm4.Show();
                        frm4.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frm4.Dispose();
                    break;

                     case "SalesInvoiceSaveReturn":
                   
                    frmCashierPurchaseSaveDaimondReturn frm41 = new frmCashierPurchaseSaveDaimondReturn();
                    if (Permissions.UserPermissionsFrom(frm41, frm41.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm41);
                        frm41.Show();
                        frm41.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frm41.Dispose();
                    break;
                     case "SalesInvoiceSave":
                   
                    frmCashierPurchaseSaveDaimond frm42 = new frmCashierPurchaseSaveDaimond();
                    if (Permissions.UserPermissionsFrom(frm42, frm42.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm42);
                        frm42.Show();
                        frm42.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frm42.Dispose();
                    break;
            }
            }

catch { }
        }
    }
}

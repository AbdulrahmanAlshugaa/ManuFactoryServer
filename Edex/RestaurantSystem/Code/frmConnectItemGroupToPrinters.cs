using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraSplashScreen;
using Edex.Model;
using Edex.Model.Language;
using Edex.GeneralObjects.GeneralClasses;
using Edex.GeneralObjects.GeneralForms;
using Edex.ModelSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Management;
using System.Text;
using System.Windows.Forms;
namespace Edex.RestaurantSystem.Code
{

    public partial class frmConnectItemGroupToPrinters : Edex.GeneralObjects.GeneralForms.BaseForm
        {

            private string CaptionReport;
            private string CaptionPrinterName1;
            private string CaptionPrinterName2;
            private string CaptionPrinterName3;
            private string CaptionPrinterName4;
            private string CaptionPrinterName5;
            OleDbConnection con;
            string strSQL;
            string CONNECTION_STRING;
            List<string> lstPrinterName;
            //list detail
            BindingList<cPrinterSelecter> lstDetail = new BindingList<cPrinterSelecter>();
            public frmConnectItemGroupToPrinters()
            {
                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                    InitializeComponent();

                    CaptionReport = "ArbCaption";

                    CaptionPrinterName1 = "الطابعة الاولئ";
                    CaptionPrinterName2 = "الطابعةالثانية ";
                    CaptionPrinterName3 = "الطابعة الثالثة";
                    CaptionPrinterName4 = "الطابعة الرابعة";
                    CaptionPrinterName5 = "الطابعة الخامسة";
                    if (UserInfo.Language == iLanguage.English)
                    {
                        CaptionReport = "EngCaption";

                        CaptionPrinterName1 = "First Printer";
                        CaptionPrinterName2 = "Second Printer ";
                        CaptionPrinterName3 = "Third Printer";
                        CaptionPrinterName4 = "Fourth Printer";
                        CaptionPrinterName5 = "Fifth Printer";
                    }
                    InitGrid();
                    FillGridCombo();
                    BindGridView();
                   // EnabledControl(false);
                
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
            #region GridView
            void InitGrid()
            {
                lstDetail = new BindingList<cPrinterSelecter>();
                lstDetail.AllowNew = false;
                lstDetail.AllowEdit = true;
                lstDetail.AllowRemove = false;
                gridControl.DataSource = lstDetail;

                /******************* Columns Visible=fale *******************/
                gridView1.Columns["ReportName"].Visible = true;
                gridView1.Columns["ArbCaption"].Visible = true;
                gridView1.Columns["EngCaption"].Visible = false;
                gridView1.Columns["ReportName"].OptionsColumn.ReadOnly = true;
                gridView1.Columns["ArbCaption"].OptionsColumn.ReadOnly = true;
                gridView1.Columns["EngCaption"].OptionsColumn.ReadOnly = true;
                gridView1.Columns["ReportName"].OptionsColumn.AllowEdit = false;
                gridView1.Columns["ArbCaption"].OptionsColumn.AllowEdit = false;
                gridView1.Columns["EngCaption"].OptionsColumn.AllowEdit = false;
                gridView1.Columns["ReportName"].OptionsColumn.AllowFocus = false;
                gridView1.Columns["ArbCaption"].OptionsColumn.AllowFocus = false;
                gridView1.Columns["EngCaption"].OptionsColumn.AllowFocus = false;
                /******************* Columns Visible=true ********************/
                gridView1.Columns[CaptionReport].Visible = true;
                /******************* Columns  *******************/
                gridView1.Columns[CaptionReport].Caption = (UserInfo.Language == iLanguage.Arabic ? "إسم التقرير" : "Report Name");
                gridView1.Columns[CaptionReport].Width = 150;
                gridView1.Columns["PrinterName1"].Caption = CaptionPrinterName1;
                gridView1.Columns["PrinterName2"].Caption = CaptionPrinterName2;
                gridView1.Columns["PrinterName3"].Caption = CaptionPrinterName3;
                gridView1.Columns["PrinterName4"].Caption = CaptionPrinterName4;
                gridView1.Columns["PrinterName5"].Caption = CaptionPrinterName5;
                gridView1.Focus();


            }
            private void BindGridView()
            {
               string languagename = "ArbName";
                if (UserInfo.Language == iLanguage.English)
                    languagename = "EngName";
                else
                    languagename = "ArbName";
                CONNECTION_STRING = ConfigurationManager.AppSettings["AccessDBConnection"].ToString();
                con = new OleDbConnection(CONNECTION_STRING);
                //string strSQL = (" SELECT   GroupID As ReportName, " + languagename + " As ArbCaption , '0' AS PrinterName1 , '0' AS PrinterName2 , '0' AS PrinterName3,'0' AS PrinterName4, '0' AS PrinterName5  FROM  Stc_ItemsGroups  where Cancel=0  ");
                string strSQL = (" SELECT   BrandID As ReportName, " + languagename + " As ArbCaption , '0' AS PrinterName1 , '0' AS PrinterName2 , '0' AS PrinterName3,'0' AS PrinterName4, '0' AS PrinterName5  FROM  Stc_ItemsBrands  where Cancel=0  ");


                DataTable dt = Lip.SelectRecord(strSQL);
                if ((dt.Rows.Count > 0))
                {
                    gridControl.DataSource = dt;
                }
                ConvertFromObjectToDataTable();
                SetSelectedItem();

            }
            private void SetSelectedItem()
            {
                try
                {

                    var dtGeneral = ReportComponent.SelectRecord("SELECT * From ItemGroupsPrinters ");
                    for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                    {

                        string Filter = ("ReportName ='" + (gridView1.GetRowCellValue(i, "ReportName").ToString() + "' "));
                        DataRow[] dt = dtGeneral.Select(Filter);
                        if ((dt.Length > 0))
                        {
                            foreach (var item in lstPrinterName)
                            {
                                if ((item.ToString().ToUpper() == dt[0]["PrinterName1"].ToString().ToUpper()))
                                    gridView1.SetRowCellValue(i, "PrinterName1", item.ToString());

                                if ((item.ToString().ToUpper() == dt[0]["PrinterName2"].ToString().ToUpper()))
                                    gridView1.SetRowCellValue(i, "PrinterName2", item.ToString());

                                if ((item.ToString().ToUpper() == dt[0]["PrinterName3"].ToString().ToUpper()))
                                    gridView1.SetRowCellValue(i, "PrinterName3", item.ToString());

                                if ((item.ToString().ToUpper() == dt[0]["PrinterName4"].ToString().ToUpper()))
                                    gridView1.SetRowCellValue(i, "PrinterName4", item.ToString());

                                if ((item.ToString().ToUpper() == dt[0]["PrinterName5"].ToString().ToUpper()))
                                    gridView1.SetRowCellValue(i, "PrinterName5", item.ToString());

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
            #endregion
            #region FillGridCombox
            private void FillGridCombo()
            {
                try
                {
                    lstPrinterName = new List<string>();

                    lstPrinterName.Add("");
                    foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                        lstPrinterName.Add(printer);
                    for (int i = 1; (i <= 5); i++)
                    {
                        RepositoryItemLookUpEdit PrinterName = new RepositoryItemLookUpEdit();
                        PrinterName.Name = ("PrinterName" + i.ToString());
                        PrinterName.PopupWidth = 200;
                        PrinterName.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Optimistic;
                        PrinterName.DataSource = lstPrinterName;
                        PrinterName.NullText = (UserInfo.Language == iLanguage.Arabic ? "" : ""); ;
                        gridView1.Columns[PrinterName.Name].ColumnEdit = PrinterName;
                        gridView1.Columns[PrinterName.Name].OptionsColumn.AllowEdit = true;

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
                        ribbonControl1.Pages[0].Groups[0].ItemLinks[11].Visible = false;
                    }

                }
                catch (Exception ex)
                {
                    Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                }
            }
            protected override void DoPrint()
            {
                gridControl.ShowRibbonPrintPreview();
            }

            #endregion
            #region Event
            private void frmPrinterSelecter_Load(object sender, EventArgs e)
            {

            }
            private void btnEdit_Click(object sender, EventArgs e)
            {
                ConvertFromObjectToDataTable();
                EnabledControl(true);
            }
            #endregion
            #region Function
            private void Save()
            {
                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                    DeleteAll();
                    for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                    {
                        strSQL = ("INSERT INTO ItemGroupsPrinters (ReportName,PrinterName1,PrinterName2,PrinterName3,PrinterName4,PrinterName5)" + (" Values( '" + (gridView1.GetRowCellValue(i, "ReportName").ToString() + ("','" + (gridView1.GetRowCellValue(i, "PrinterName1").ToString() + ("','" + (gridView1.GetRowCellValue(i, "PrinterName2").ToString() + ("','" + (gridView1.GetRowCellValue(i, "PrinterName3").ToString() + ("','" + (gridView1.GetRowCellValue(i, "PrinterName4").ToString() + ("','" + (gridView1.GetRowCellValue(i, "PrinterName5").ToString() + "')")))))))))))));
                        OleDbCommand scmd = new OleDbCommand();
                        scmd.CommandText = strSQL;
                        scmd.Connection = con;
                        if ((con.State == ConnectionState.Closed))
                        {
                            con.Open();
                        }

                        scmd.ExecuteNonQuery();
                        con.Close();
                    }
                    //EnabledControl(false);
                    SplashScreenManager.CloseForm(false);
                    Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
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
            private void DeleteAll()
            {
                try
                {

                CONNECTION_STRING = ConfigurationManager.AppSettings["AccessDBConnection"].ToString();
                con = new OleDbConnection(CONNECTION_STRING);
                strSQL = "Delete From ItemGroupsPrinters ";
                    OleDbCommand scmd = new OleDbCommand();
                    scmd.CommandText = strSQL;
                    scmd.Connection = con;
                    if ((con.State == ConnectionState.Closed))
                    {
                        con.Open();
                    }

                    scmd.ExecuteNonQuery();
                    con.Close();
                }
                catch (Exception ex)
                {
                    Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                }

            }
            private void ConvertFromObjectToDataTable()
            {
                DataTable dtItem = new DataTable();
                 dtItem.Columns.Add("ArbCaption", System.Type.GetType("System.String"));
                dtItem.Columns.Add("ReportName", System.Type.GetType("System.String"));
                dtItem.Columns.Add("PrinterName1", System.Type.GetType("System.String"));
                dtItem.Columns.Add("PrinterName2", System.Type.GetType("System.String"));
                dtItem.Columns.Add("PrinterName3", System.Type.GetType("System.String"));
                dtItem.Columns.Add("PrinterName4", System.Type.GetType("System.String"));
                dtItem.Columns.Add("PrinterName5", System.Type.GetType("System.String"));
                for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                {
                    dtItem.Rows.Add();
                    dtItem.Rows[i]["ArbCaption"] = gridView1.GetRowCellValue(i, "ArbCaption").ToString();
                    dtItem.Rows[i]["ReportName"] = gridView1.GetRowCellValue(i, "ReportName").ToString();
                    dtItem.Rows[i]["PrinterName1"] = gridView1.GetRowCellValue(i, "PrinterName1").ToString();
                    dtItem.Rows[i]["PrinterName2"] = gridView1.GetRowCellValue(i, "PrinterName2").ToString();
                    dtItem.Rows[i]["PrinterName3"] = gridView1.GetRowCellValue(i, "PrinterName3").ToString();
                    dtItem.Rows[i]["PrinterName4"] = gridView1.GetRowCellValue(i, "PrinterName4").ToString();
                    dtItem.Rows[i]["PrinterName5"] = gridView1.GetRowCellValue(i, "PrinterName5").ToString();
                }
                gridControl.DataSource = dtItem;


            }
            private void EnabledControl(bool Value)
            {
                foreach (GridColumn col in gridView1.Columns)
                {

                    if (col.FieldName != "ReportName" || col.FieldName != "ArbCaption" || col.FieldName != "EngCaption")
                    {
                        gridView1.Columns[col.FieldName].OptionsColumn.AllowEdit = Value;
                        gridView1.Columns[col.FieldName].OptionsColumn.AllowFocus = Value;
                        gridView1.Columns[col.FieldName].OptionsColumn.ReadOnly = !Value;

                    }
                }

            }
            #endregion

            private void btnSave_Click(object sender, EventArgs e)
            {
                Save();
            }
        }
}

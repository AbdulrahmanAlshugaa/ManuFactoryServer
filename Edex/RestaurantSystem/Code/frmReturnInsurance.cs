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
using System.IO;
using DevExpress.XtraGrid.Views.Grid;
using Edex.Model;
using Edex.GeneralObjects.GeneralForms;
using Edex.ModelSystem;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraReports.UI;
using System.Diagnostics;
using System.Data.OleDb;
using Edex.Model.Language;
using Edex.DAL.SalseSystem;
using Edex.DAL;
using Edex.DAL.Accounting;
using Edex.RestaurantSystem.Transactions;
using DevExpress.XtraBars.Docking2010.Views.WindowsUI;
using DevExpress.XtraBars.Docking2010.Customization;

namespace Edex.TimeStaffScreens
{
    public partial class frmReturnInsurance : DevExpress.XtraEditors.XtraForm
    {
        public DataTable dt = new DataTable();
        public DataTable filtering = new DataTable();
          public DataTable dtimport = new DataTable();
          DataTable dtDeclaration;
          public DataTable _sampleData = new DataTable();
          public long ID = 0;
          public bool ShowReportInReportViewer;
          public bool FormAdd;
          public bool FormDelete;
          public bool FormUpdate;
          public bool FormView;
          public bool ReportView;
          public bool ReportExport;
          public string ReportName;
        public void fillGrid()
        {
            _sampleData.Clear();
            var sr = "    SELECT    Sales_SalesInvoiceMaster.NetBalance ,  Sales_SalesInvoiceMaster.InvoiceID,Sales_SalesInvoiceMaster.InvoiceDate, Sales_SalesInvoiceMaster.UserID,  isnull(Sales_SalesInvoiceMaster.InsuranceAmmount,0)as InsuranceAmmount, isnull(Sales_Customers.CustomerID,0) AS CustomerID, isnull(Sales_Customers.AccountID,0) AS Account, isnull(Sales_Customers.ArbName,'') as ArbName , isnull(Sales_Customers.Mobile,0) as Mobile, isnull(Sales_Customers.Tel,0) as Tel"
+""
+ "  FROM            Sales_SalesInvoiceMaster  LEFT OUTER JOIN"
 + "                          Sales_Customers ON Sales_SalesInvoiceMaster.CustomerID = Sales_Customers.CustomerID"
+ "  where (Sales_SalesInvoiceMaster.isDone=0 or  Sales_SalesInvoiceMaster.isDone IS NUll )"
+" "
;
            long FromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text));
            long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));
            if (FromDate != 0)
                sr = sr + " And  .Sales_SalesInvoiceMaster.InvoiceDate >=" + FromDate + "  ";

            if (ToDate != 0)
                sr = sr + "AND .Sales_SalesInvoiceMaster.InvoiceDate <=" + ToDate + "  ";

            if (txtInvoiceNo.Text != string.Empty)
                sr = sr + " AND Sales_SalesInvoiceMaster.InvoiceID =" + txtInvoiceNo.Text + "  ";
            //else {

            //    Messages.MsgExclamationk(Messages.TitleInfo, "يرجى ادخال رقم الفاتورة");

            //    return;
            //}
            //if (txtInvoiceNo.Text != string.Empty)
            //    sr = sr + " AND Res_ItemsInsurance_Master.InvoiceID =" + txtInvoiceNo.Text + "  ";

            if (txtCustomerName.Text != string.Empty)
                sr = sr + " AND Sales_Customers.ArbName LIKE'%" + txtCustomerName.Text + "&'";
            if (txtMobile.Text != string.Empty)
                sr = sr + " AND Sales_Customers.Mobile =" + txtMobile.Text + "  ";

            var dr = Lip.SelectRecord(sr);

            DataRow drow;
            int i = 0;
     foreach (DataRow row in dr.Rows) {
         ++i;
         drow = _sampleData.NewRow();
         drow["Sn"] =i;
         drow["InvoiceID"] = row["InvoiceID"].ToString();
         drow["InvoiceDate"] = Comon.ConvertSerialDateTo(row["InvoiceDate"].ToString());
         drow["ArbName"] = row["ArbName"].ToString();
         drow["UserID"] = row["UserID"].ToString();
         drow["CustomerID"] = row["CustomerID"].ToString();
         drow["Mobile"] = row["Mobile"].ToString();
         drow["Tel"] = row["Tel"].ToString();
         drow["InsuranceAmmount"] = row["InsuranceAmmount"].ToString();
         drow["Account"] = row["Account"].ToString();
         drow["NetBalance"] = row["NetBalance"].ToString();

         drow["Status"] = "غير مستلم";
         _sampleData.Rows.Add(drow);
     
     }

            
            //if (dt.Rows.Count > 0)
            //{

            //    for (int i = 0; i <= dt.Rows.Count - 1; ++i)
            //    {

            //        if (DBNull.Value == dt.Rows[i]["image"])
            //        {
            //            dt.Rows[i]["image"] = DefaultImage();


            //        }


            //    }

            //}
        }
        public frmReturnInsurance()
        {
            InitializeComponent();
            InitializeFormatDate(txtFromDate);
            InitializeFormatDate(txtToDate);
            Common.filllookupEDit(ref repositoryItemLookUpEdit1, "UserID", "Users", "ArbName", "Cancel=0");
            _sampleData.Columns.Add(new DataColumn("Sn", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("InvoiceID", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("InvoiceDate", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("ArbName", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("CustomerID", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Mobile", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("UserID", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("NetBalance", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Tel", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("InsuranceAmmount", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Status", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Account", typeof(string)));

            
            fillGrid();
            filtering = _sampleData.Copy();

            gridControl2.DataSource = filtering;
          //  Common.filllookupEDit(ref repositoryItemLookUpEdit1, "GroupID", "AdmAfr_Groups", "ArbName", "Cancel=0");

            string[] s = new string[] { "الكل", "ا", "أ", "ب", "ت", "ث", "ج", "ح", "خ", "د", "ذ", "ر", "ز", "س", "ش", "ص", "ض", "ط", "ظ", "ع", "غ", "ف", "ق", "ك", "ل", "م", "ن", "ه", "و", "ي" };

            indexGridControl.DataSource = s;
            dtDeclaration = new Acc_DeclaringMainAccountsDAL().GetAcc_DeclaringMainAccounts(MySession.GlobalBranchID, MySession.GlobalFacilityID);

        }
        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }

        public System.Drawing.Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
            return returnImage;
        }
        //private byte[] DefaultImage()
        //{
        //    string Path = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
        //    Path = Path + @"\Images\Default.png";
        //    System.Drawing.Image img = global::Edex.Properties.Resources.Unknown_user;
        //    MemoryStream ms = new System.IO.MemoryStream();
        //    img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
        //    return ms.ToArray();

        //}

        private void XtraForm2_Load(object sender, EventArgs e)
        {

        }

        private void indexGridView_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {

            var filter = indexGridView.GetFocusedRowCellValue(indexGridView.FocusedColumn.FieldName).ToString();
            filtering = dt.Copy();
            if (filter == "الكل")
            {

                gridControl2.DataSource = filtering;
                return;

            }


            if (filtering.Rows.Count > 0)
            {
                DataRow dr;
                for (int i = 0; i <= filtering.Rows.Count - 1; ++i)
                {

                    if (DBNull.Value != filtering.Rows[i]["ArbName"] || !string.IsNullOrEmpty(filtering.Rows[i]["ArbName"].ToString()))
                    {
                        dr = filtering.Rows[i];
                        //if (dr["ArbName"].ToString().Substring(0, 1) == filter)
                        //{

                        //    DataRow row = filtering.NewRow();
                        //    row = dr;
                        //    filtering.Rows.Add(dt.Rows[i]);



                        //}


                        if (dr["ArbName"].ToString().Substring(0, 1) != filter)
                            dr.Delete();
                    }



                }


                filtering.AcceptChanges();

                gridControl2.DataSource = filtering;
            }

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            gridControl2.MainView = gridView1;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            gridControl2.MainView = layoutView1;
        }

        private void layoutView1_DoubleClick(object sender, EventArgs e)
        {
            //GridView view = sender as GridView;
            //long ID = Comon.cLong(layoutView1.GetFocusedRowCellValue("EmployeeID").ToString());
            //EditTeacherInfo frm = new EditTeacherInfo(ID);
            //frm.Show();

        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            if (gridView1.RowCount < 1)
                this.Close();


            foreach (var rowHandle in gridView1.GetSelectedRows())
            {
                confirmSavingReturn confirm = new confirmSavingReturn(gridView1.GetRowCellValue(rowHandle, "InvoiceID").ToString(), gridView1.GetRowCellValue(rowHandle, "NetBalance").ToString(), gridView1.GetRowCellValue(rowHandle, "InsuranceAmmount").ToString(), gridView1.GetRowCellValue(rowHandle, "Account").ToString());
                FlyoutAction action = new FlyoutAction();
                FlyoutProperties properties = new FlyoutProperties();
                properties.Style = FlyoutStyle.Popup;
                FlyoutDialog.Show(this.ParentForm, confirm, action, properties);
                //  EditTeacherInfo frm = new EditTeacherInfo(Comon.cLong(gridView1.GetRowCellValue(rowHandle, "EmployeeID").ToString()), true);
            }


            simpleButton3_Click(null, null);

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
          //  Obj.EditValue = DateTime.Now;
        }





        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            //GridView view = sender as GridView;
            //ID = Comon.cLong(gridView1.GetFocusedRowCellValue("ID").ToString());
            //this.Close();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            fillGrid();
            filtering = _sampleData.Copy();
            gridControl2.DataSource = filtering;
        }

        private void gridView1_RowStyle(object sender, RowStyleEventArgs e)
        {
            GridView view = sender as GridView;
            if (view.IsRowSelected(e.RowHandle))
            {
                e.Appearance.BackColor = System.Drawing.Color.Yellow;// System.Drawing.Color.FromArgb(25, 71, 138);
                e.Appearance.ForeColor = System.Drawing.Color.Black;
                e.HighPriority = true;
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            //bool Yes = Messages.MsgStopYesNo(Messages.TitleConfirm, Messages.msgConfirmDelete);
            //if (!Yes)
            //    return;
            //foreach (var rowHandle in gridView1.GetSelectedRows())
            //{
            //    EditTeacherInfo frm = new EditTeacherInfo(Comon.cLong(gridView1.GetRowCellValue(rowHandle, "EmployeeID").ToString()), true);
            //}

            //simpleButton3_Click(null, null);

        }

        //private void simpleButton5_Click(object sender, EventArgs e)
        //{

        //    try
        //    {
        //        Application.DoEvents();
        //       SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
        //     //   gridControl2.ShowRibbonPrintPreview();
        //        /******************** Report Body *************************/

        //       bool IncludeHeader = true;
        //       string rptFormName = "rptEmpReport";


        //       XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);

        //       /********************** Master *****************************/
        //       rptForm.RequestParameters = false;


        //       /********************** Details ****************************/
        //       var dataTable = new dsReports.TeacherAttenceDataTable();

        //       for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
        //       {
        //           var row = dataTable.NewRow();

        //           row["#"] = i + 1;

        //           row["TechNO"] = gridView1.GetRowCellValue(i, "EmployeeID").ToString();

        //           row["TechName"] = gridView1.GetRowCellValue(i, "ArbName").ToString();


        //           row["Date"] = gridView1.GetRowCellValue(i, "Telephone").ToString();

        //           row["LateMinute"] = gridView1.GetRowCellValue(i, "IdentityID").ToString();
        //           row["Earlyminute"] = gridView1.GetRowCellValue(i, "specialest").ToString();
        //           row["Status"] = gridView1.GetRowCellDisplayText(i, "GroupID").ToString();
        //           dataTable.Rows.Add(row);
        //       }
        //       rptForm.DataSource = dataTable;
        //       rptForm.DataMember = "TeacherAttence";
        //       /******************** Report Binding ************************/
        //       XRSubreport subreport = (XRSubreport)rptForm.FindControl("subRptCompanyHeader", true);
        //       subreport.Visible = IncludeHeader;
        //       subreport.ReportSource = ReportComponent.CompanyHeader();
        //       rptForm.ShowPrintStatusDialog = false;
        //       rptForm.ShowPrintMarginsWarning = false;
        //       rptForm.CreateDocument();

        //       SplashScreenManager.CloseForm(false);
        //       if (ShowReportInReportViewer = true)
        //       {
        //           frmReportViewer frmRptViewer = new frmReportViewer();
        //           frmRptViewer.documentViewer1.DocumentSource = rptForm;
        //           frmRptViewer.ShowDialog();
        //       }
        //       else
        //       {
        //           bool IsSelectedPrinter = false;
        //           SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
        //           DataTable dt = ReportComponent.SelectRecord("SELECT *  from Printers where ReportName='" + rptFormName + "'");
        //           if (dt.Rows.Count > 0) for (int i = 1; i < 6; i++)
        //               {
        //                   string PrinterName = dt.Rows[0]["PrinterName" + i.ToString()].ToString().ToUpper();
        //                   if (!string.IsNullOrEmpty(PrinterName))
        //                   {
        //                       rptForm.PrinterName = PrinterName;
        //                       rptForm.Print(PrinterName);
        //                       IsSelectedPrinter = true;
        //                   }
        //               }
        //           SplashScreenManager.CloseForm(false);
        //           if (!IsSelectedPrinter)
        //               Messages.MsgWarning(Messages.TitleWorning, Messages.msgThereIsNotPrinterSelected);
        //       }

        //    }
        //    catch (Exception ex)
        //    {
        //        SplashScreenManager.CloseForm(false);
        //        Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
        //    }
        //}

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            //  var sr = "Select * from AdmAfr_Employees where Cancel=0";
            var sr2 = " SELECT [EmployeeID] as [EmployeeID]"
       + " ,[BranchID]as BranchID"
       + " ,[ArbName]as ArbName"
       + " ,[EngName]as EngName"
       + " ,[EmployeeIDInFingerTecDevice]as EmployeeIDInFingerTecDevice"
      + "  ,[IsActive]as IsActive"
      + "  ,[DepartmentID]as DepartmentID"
      + "  ,[SectionID]as SectionID"
      + "  ,[GroupID]as GroupID"
      + "  ,[Job]as Job"
      + "  ,[StartJobDate]as StartJobDate"
      + "  ,[SocialSecurityID]as SocialSecurityID"
      + "  ,[Telephone]as Telephone"
      + "  ,[Address]as Address"
        + ",[JobHours]as JobHours"
     + "   ,[IdentityID]as IdentityID"
      + "  ,[EMail]as EMail"
      + "  ,[Notes]as Notes"


      + "  ,[CompNO]as CompNO"
     + "   ,[FileNo]as FileNo"
     + "   ,[CurrentJob]as CurrentJob"
     + "   ,[LevelName]as LevelName"
      + "  ,[JobNo]as JobNo"
      + "  ,[specialest]as specialest"
      + "  ,[DegreeNO]as DegreeNO"

   + " FROM [dbo].[AdmAfr_Employees]  where Cancel=0";

            var dtExport = Lip.SelectRecord(sr2);
            //  DevExpress.XtraGrid.GridControl gridControl1 = new DevExpress.XtraGrid.GridControl();
            gridControl1.DataSource = dtExport;
            string path = "C:\\Edex_20190611\\Student.xlsx";
            gridControl1.ExportToXlsx(path);
            // Open the created XLSX file with the default application. 
            Process.Start(path);
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            try{
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                dtimport.Clear();
            OleDbConnection oledbConn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Edex_20190611\\Student.xlsx;Extended Properties=Excel 12.0");
            oledbConn.Open();
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Sheet$]", oledbConn);
            OleDbDataAdapter oleda = new OleDbDataAdapter();
            oleda.SelectCommand = cmd;
            oleda.Fill(dtimport);
            oledbConn.Close();
            DataRow[] dtrow;
          
            foreach (DataRow row in dtimport.Rows)
            {
                //EditTeacherInfo frm = new EditTeacherInfo(row);
            
            }
            SplashScreenManager.CloseForm(false);
            Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
            simpleButton3_Click(null, null);
              }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        public void DoPrint(int invoiceID,int InvoiceReturnID,string netBalance,string CustomerName )
        {

            try
            {
               
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);

                /******************** Report Body *************************/
                //rptForm = "rptCashierPrint";
                ReportName = "rptPrepaid";
                bool IncludeHeader = true;
                string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");

                rptFormName = "rptInsuranceReturn";
               
                XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);


                /********************** Master *****************************/
                rptForm.RequestParameters = false;
        
                    rptForm.Parameters["InvoiceID"].Value = invoiceID;//Comon.ConvertDateToSerial(txtInvoiceDate.Text) + "-" + invoiceNo;

                    rptForm.Parameters["DelegateName"].Value = InvoiceReturnID.ToString();

                    rptForm.Parameters["CustomerName"].Value = CustomerName;
               
            
                rptForm.Parameters["TheTime"].Value = Comon.ConvertSerialToTime(Lip.GetServerTimeSerial().ToString().Replace(":", "").Trim());
                rptForm.Parameters["CashierName"].Value = UserInfo.SYSUSERARBNAME.ToString();
                rptForm.Parameters["CompanyName"].Value = MySession.GlobalCompanyName;
             //   rptForm.Parameters["CompanyAddress"].Value = (UserInfo.Language == iLanguage.Arabic ? cmpheader.ArbAddress : cmpheader.ArbAddress);
             


                rptForm.Parameters["InvoiceDate"].Value = Lip.GetServerDate();

                rptForm.Parameters["NetBalance"].Value = netBalance.ToString();
             
                for (int i = 0; i < rptForm.Parameters.Count; i++)
                    rptForm.Parameters[i].Visible = false;
                /********************** Details ****************************/
                var dataTable = new dsReports.rptSalesInvoiceDataTable();



                rptForm.DataSource = dataTable;
                rptForm.DataMember = ReportName;
                rptForm.ShowPrintStatusDialog = false;
                rptForm.ShowPrintMarginsWarning = false;
                rptForm.CreateDocument();
                SplashScreenManager.CloseForm(false);
                ShowReportInReportViewer = false;
                if (ShowReportInReportViewer = false)
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
                    if (dt.Rows.Count > 0)
                        for (int i = 1; i < 6; i++)
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
                //if(IsNewRecord==true)
                //SaveVouchers(Comon.ConvertToDecimalPrice(txtInsuranceAmmount.Text));
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

        }
        private void panelControl10_Paint(object sender, PaintEventArgs e)
        {
                    }

        private void simpleButton2_Click_1(object sender, EventArgs e)
        {
            fillGrid();
            filtering = _sampleData.Copy();
            gridControl2.DataSource = filtering;
        }

        private void simpleButton4_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
        }
    }

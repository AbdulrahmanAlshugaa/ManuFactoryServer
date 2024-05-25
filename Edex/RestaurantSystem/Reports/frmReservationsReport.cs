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
using Edex.GeneralObjects.GeneralClasses;
using Edex.SalesAndPurchaseObjects.Reports;

namespace Edex.TimeStaffScreens
{
    public partial class frmReservationsReport : DevExpress.XtraEditors.XtraForm
    {
        public DataTable dt = new DataTable();
        public DataTable filtering = new DataTable();
          public DataTable dtimport = new DataTable();
          DataTable dtDeclaration;
          public DataTable _sampleData = new DataTable();
          public long ID = 0; 
        public bool ShowReportInReportViewer;
        public void fillGrid()
        {
            _sampleData.Clear();
           // var sr = "Select * from AdmAfr_Employees where Cancel=0";
            var sr = "    SELECT  Sales_SalesReservationsMaster.NetBalance ,  Sales_SalesReservationsMaster.IsGoodOpening,Sales_SalesReservationsMaster.Cancel,   Sales_SalesReservationsMaster.InvoiceID,Sales_SalesReservationsMaster.InvoiceDate,Sales_SalesReservationsMaster.UserID,   Sales_SalesReservationsMaster.InsuranceAmmount, isnull(Sales_Customers.CustomerID,0) AS CustomerID, isnull(Sales_Customers.ArbName,'') as ArbName , isnull(Sales_Customers.Mobile,0) as Mobile, isnull(Sales_Customers.Tel,0) as Tel"
    + "  ,Sales_SalesReservationsMaster.CloseCashierDate,Sales_SalesReservationsMaster.EditUserID"
    +",  isnull(case dbo.Sales_SalesReservationsMaster.IsGoodOpening when 0  then case  dbo.Sales_SalesReservationsMaster.Cancel when 0 then 1 end"
   
  

+" when 1  then case  dbo.Sales_SalesReservationsMaster.Cancel when 1 then 2  end"


+" when 2  then case  dbo.Sales_SalesReservationsMaster.Cancel when 1 then 3 end"
+ " else  -1 end,0)as Status  ,Sales_SalesReservationsMaster.CloseCashier,Sales_SalesReservationsMaster.NetBalance as Net"

    + "  FROM            Sales_SalesReservationsMaster "
     + "                           LEFT OUTER JOIN"
     + "                          Sales_Customers ON Sales_SalesReservationsMaster.CustomerID = Sales_Customers.CustomerID"
    + "  where 0=0";
            long FromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text));
            long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));
            long FromDateReturn = Comon.cLong(Comon.ConvertDateToSerial(txtFromDateReturn.Text));
            long ToDateReturn = Comon.cLong(Comon.ConvertDateToSerial(txtToDateReturn.Text));
            if (FromDate != 0)
                sr = sr + " And  .Sales_SalesReservationsMaster.InvoiceDate >=" + FromDate + "  ";

            if (ToDate != 0)
                sr = sr + "AND .Sales_SalesReservationsMaster.InvoiceDate <=" + ToDate + "  ";

            if (FromDateReturn != 0)
                sr = sr + " And  .Sales_SalesReservationsMaster.CloseCashierDate >=" + FromDateReturn + "  ";

            if (ToDateReturn != 0)
                sr = sr + "AND .Sales_SalesReservationsMaster.CloseCashierDate <=" + ToDateReturn + "  ";
            if (txtFromInvoiceNo.Text != string.Empty)
                sr = sr + " AND Sales_SalesReservationsMaster.InvoiceID >=" + txtFromInvoiceNo.Text + "  ";
            if (txtToInvoiceNo.Text != string.Empty)
                sr = sr + " AND Sales_SalesReservationsMaster.InvoiceID <=" + txtToInvoiceNo.Text + "  ";
            if (txtCustomerName.Text != string.Empty)
                sr = sr + " AND Sales_Customers.ArbName LIKE'" + txtCustomerName.Text + "'  ";
            if (txtMobile.Text != string.Empty)
                sr = sr + " AND Sales_Customers.Mobile =" + txtMobile.Text + "  ";
            if (cmbStatus.Text != string.Empty && Comon.cInt(cmbStatus.EditValue) != 0) {
                switch (Comon.cInt(cmbStatus.EditValue)) {
                    case (1): sr = sr + " AND Sales_SalesReservationsMaster.IsGoodOpening=0 and Sales_SalesReservationsMaster.Cancel=0  "; break;
                    case (2): sr = sr + " AND Sales_SalesReservationsMaster.IsGoodOpening=1 and Sales_SalesReservationsMaster.Cancel=1  "; break;
                    case (3): sr = sr + " AND Sales_SalesReservationsMaster.IsGoodOpening=2 and Sales_SalesReservationsMaster.Cancel=1  "; break;
                }

            
            }
                

//+" and"
//+" Sales_SalesInvoiceMaster.InvoiceID not IN("

//+" SELECT       Sales_SalesInvoiceMaster.InvoiceID"

// +"  FROM            Sales_SalesInvoiceMaster "
// +"   Inner join" 
//  +"  Res_ItemsInsuranceReturn_Master ON Sales_SalesInvoiceMaster.InvoiceID = Res_ItemsInsuranceReturn_Master.InvoiceID) ";


            var dr = Lip.SelectRecord(sr);

            DataRow drow;
            int i = 0;
     foreach (DataRow row in dr.Rows) {
         ++i;
         drow = _sampleData.NewRow();
         drow["Sn"] =i;
         drow["InvoiceID"] = row["InvoiceID"].ToString();
         drow["InvoiceNo"] = row["InvoiceID"].ToString();
         drow["InvoiceDate"] = Comon.ConvertSerialDateTo(row["InvoiceDate"].ToString());
         drow["InvoiceDateReturn"] = Comon.ConvertSerialDateTo(row["CloseCashierDate"].ToString());
         drow["UserID"] = Comon.cInt(row["UserID"].ToString());
         drow["EditUserID"] = Comon.cInt(row["EditUserID"].ToString());
         drow["ArbName"] = row["ArbName"].ToString();
         drow["CustomerID"] = row["CustomerID"].ToString();
         drow["Mobile"] = row["Mobile"].ToString();
         drow["Tel"] = row["Tel"].ToString();
         drow["InsuranceAmmount"] = row["InsuranceAmmount"].ToString();
         drow["CloseCashier"] = Comon.ConvertSerialToTime(row["CloseCashier"].ToString());
         drow["Status"] = row["Status"].ToString();
         drow["NetBalance"] = row["NetBalance"].ToString();
         
         drow["Net"] = Comon.ConvertToDecimalPrice(row["Net"].ToString()) - Comon.ConvertToDecimalPrice(row["InsuranceAmmount"].ToString());
      
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
        public frmReservationsReport()
        {
            InitializeComponent();
            InitializeFormatDate(txtFromDate);
            InitializeFormatDate(txtToDate);
            InitializeFormatDate(txtFromDateReturn);
            InitializeFormatDate(txtToDateReturn);
            _sampleData.Columns.Add(new DataColumn("Sn", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("InvoiceID", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("InvoiceNo", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("InvoiceDate", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Net", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("ArbName", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("CustomerID", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Mobile", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("CloseCashier", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("NetBalance", typeof(string)));
            

            _sampleData.Columns.Add(new DataColumn("UserID", typeof(int)));
            _sampleData.Columns.Add(new DataColumn("EditUserID", typeof(int)));
            _sampleData.Columns.Add(new DataColumn("InvoiceDateReturn", typeof(string)));



            _sampleData.Columns.Add(new DataColumn("Tel", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("InsuranceAmmount", typeof(string)));

            _sampleData.Columns.Add(new DataColumn("Status", typeof(string)));
            //
            FillCombo.FillComboBox(cmbStatus, "ReservationStatus", "ID", "ArbName", "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
            Common.filllookupEDit(ref repositoryItemLookUpEdit1, "UserID", "Users", "ArbName", "Cancel=0");
            Common.filllookupEDit(ref repositoryItemLookUpEdit3, "ID", "ReservationStatus", "ArbName", "Cancel=0");
            fillGrid();
            filtering = _sampleData.Copy();

            gridControl2.DataSource = filtering;

          //  Common.filllookupEDit(ref repositoryItemLookUpEdit1, "GroupID", "AdmAfr_Groups", "ArbName", "Cancel=0");

            string[] s = new string[] { "الكل", "ا", "أ", "ب", "ت", "ث", "ج", "ح", "خ", "د", "ذ", "ر", "ز", "س", "ش", "ص", "ض", "ط", "ظ", "ع", "غ", "ف", "ق", "ك", "ل", "م", "ن", "ه", "و", "ي" };

            indexGridControl.DataSource = s;
            dtDeclaration = new Acc_DeclaringMainAccountsDAL().GetAcc_DeclaringMainAccounts(MySession.GlobalBranchID, MySession.GlobalFacilityID);

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
           // Obj.EditValue = DateTime.Now;
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
           bool Yes = Messages.MsgStopYesNo(Messages.TitleConfirm, "تأكيد عملية الاسترجاع؟");
            if (!Yes)
                return;
            foreach (var rowHandle in gridView1.GetSelectedRows())
            {
                SaveAmount(Comon.cInt(gridView1.GetRowCellValue(rowHandle, "InvoiceID").ToString()), Comon.cDbl(gridView1.GetRowCellValue(rowHandle, "InsuranceAmmount").ToString()));
              //  EditTeacherInfo frm = new EditTeacherInfo(Comon.cLong(gridView1.GetRowCellValue(rowHandle, "EmployeeID").ToString()), true);
            }

        }
        private void SaveAmount(int invoiceID,double InsurementAmount)
        {
            gridView1.MoveLastVisible();
            gridView1.FocusedColumn = gridView1.VisibleColumns[1];
            Res_ItemsInsuranceReturn_Master objRecord = new Res_ItemsInsuranceReturn_Master();
            objRecord.InvoiceID = invoiceID;
            objRecord.BranchID = UserInfo.BRANCHID;
            objRecord.FacilityID = UserInfo.FacilityID;
            objRecord.CustomerMobile ="";
            objRecord.InvoiceDate = Lip.GetServerDateSerial();
            objRecord.MethodeID = 0;
            objRecord.CurencyID = 0;
            objRecord.CustomerID = 0;
            objRecord.CostCenterID = 0;
            objRecord.StoreID = 0;
            objRecord.DelegateID =0;

            objRecord.DocumentID = 0;
            objRecord.SellerID = 0;


            objRecord.RegistrationNo = 0;
            objRecord.OperationTypeName = (UserInfo.Language == iLanguage.English ? "Sale  Invoice" : "مرنجع تأمين ");
         
            objRecord.Notes = "";

            //Account
            objRecord.DebitAccount = 0;
            objRecord.CreditAccount = 0;
            objRecord.CheckID = "";
            objRecord.VATID = "";

            //Date
            objRecord.CheckSpendDate = Lip.GetServerDateSerial();
            objRecord.WarningDate = Lip.GetServerDateSerial();
            objRecord.ReceiveDate = Lip.GetServerDateSerial();

            //Ammount


            objRecord.NetBalance = 0;

            objRecord.Cancel = 0;
            //user Info
            objRecord.UserID = UserInfo.ID;
            objRecord.RegDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
            objRecord.RegTime = Comon.cDbl(Lip.GetServerTimeSerial()); ;
            objRecord.ComputerInfo = UserInfo.ComputerInfo;
            objRecord.RemaindAmount = 0;
            objRecord.PaidAmount = 0;
            objRecord.EditUserID = 0;
            objRecord.EditTime = 0;
            objRecord.EditDate = 0;
            objRecord.EditComputerInfo = "";

            

            Res_ItemsInsuranceReturn_Details returned;
            List<Res_ItemsInsuranceReturn_Details> listreturned = new List<Res_ItemsInsuranceReturn_Details>();
            var sr = "Select * from  Sales_SalesInvoiceDetails where  Description='INS' And InvoiceID= " + invoiceID;

            var dt = Lip.SelectRecord(sr);
            if (dt.Rows.Count < 1) return;
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {


                returned = new Res_ItemsInsuranceReturn_Details();
                returned.ID = i;
                returned.FacilityID = UserInfo.FacilityID;
                returned.BarCode = dt.Rows[i]["BarCode"].ToString();
                returned.ItemID = Comon.cInt( dt.Rows[i]["ItemID"].ToString());
                returned.SizeID = Comon.cInt( dt.Rows[i]["SizeID"].ToString());
                returned.QTY = Comon.ConvertToDecimalQty( dt.Rows[i]["QTY"].ToString());
                returned.SalePrice = Comon.ConvertToDecimalPrice(dt.Rows[i]["SalePrice"] .ToString()); ;
                returned.Bones = Comon.cInt( dt.Rows[i]["Bones"].ToString());
                returned.Description =  dt.Rows[i]["Description"].ToString();
                if ( Comon.cInt( dt.Rows[i]["StoreID"].ToString())== 0)
                    returned.StoreID =Comon.cInt(MySession.GlobalDefaultStoreID);
                else
                    returned.StoreID = Comon.cInt(dt.Rows[i]["StoreID"].ToString());
                returned.Discount = Comon.ConvertToDecimalPrice(dt.Rows[i]["Discount"] .ToString());

                returned.ExpiryDateStr = Comon.ConvertDateToSerial(dt.Rows[i]["ExpiryDate"].ToString());
                returned.CostPrice = Comon.ConvertToDecimalPrice(dt.Rows[i]["CostPrice"] .ToString());
                returned.AdditionalValue = 0;
                returned.Net = Comon.ConvertToDecimalPrice(dt.Rows[i]["Net"].ToString());
                returned.Total = Comon.ConvertToDecimalPrice(dt.Rows[i]["Total"].ToString());

                returned.HavVat = false;
                returned.Cancel = 0;
                returned.Serials = "";
                if (returned.QTY <= 0 || returned.SalePrice <= 0 || returned.SizeID <= 0 || returned.ItemID <= 0)
                    continue;

                listreturned.Add(returned);

            }

            int DocumentID = Comon.cInt(Lip.GetValue("Select InvoiceID From Res_ItemsInsuranceReturn_Master Where DocumentID=" + invoiceID));
            var strSQL = "Delete from Res_ItemsInsuranceReturn_Details Where InvoiceID=" + DocumentID;
            Lip.ExecututeSQL(strSQL);

            strSQL = "Delete from Res_ItemsInsuranceReturn_Master Where DocumentID=" + invoiceID;
            Lip.ExecututeSQL(strSQL);


            objRecord.SaleDatails = listreturned;
            string Result = Sales_SaleInvoicesDAL.InsertUsingXMLInsuranceReturn(objRecord, true);
            if (Result != "")
                SaveVouchers(InsurementAmount, invoiceID);

            Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
            simpleButton3_Click(null,null);
        }

        private int SaveVouchers(double CreditAmount,int invoiceID)
        {

            double AccountID = 0;

            DataRow[] row;
        
                    row = dtDeclaration.Select("DeclareAccountName = 'MainBoxAccount'");
                    if (row.Length > 0)
                    {
                        AccountID = Comon.cDbl(row[0]["AccountID"].ToString());
                    }


               



           






            Acc_VariousVoucherMaster objRecord = new Acc_VariousVoucherMaster();

            objRecord.BranchID = MySession.GlobalBranchID;
            objRecord.FacilityID = MySession.GlobalFacilityID;


            //Date
            objRecord.VoucherDate = Lip.GetServerDateSerial(); ;

            objRecord.CurrencyID = Comon.cInt(MySession.GlobalDefaultCurencyID);

            objRecord.RegistrationNo = 1;
            // objRecord.InvoiceID = Comon.cInt(txtInvoiceID.Text);
            objRecord.DelegateID = 0;
            objRecord.Notes = "مرتجع تأمين فاتورة رقم : " + invoiceID;
            objRecord.DocumentID =1;


            //Ammount
            // objRecord.TotalCredit = Comon.cDbl(lblTotalCredit.Text);
            // objRecord.TotalDebit = Comon.cDbl(lblTotalDebit.Text);

            objRecord.Cancel = 0;
            //user Info
            objRecord.UserID = UserInfo.ID;
            objRecord.RegDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
            objRecord.RegTime = Comon.cDbl(Lip.GetServerTimeSerial()); ;
            objRecord.ComputerInfo = UserInfo.ComputerInfo;

            objRecord.EditUserID = 0;
            objRecord.EditTime = 0;
            objRecord.EditDate = 0;
            objRecord.EditComputerInfo = "";

            //if (IsNewRecord == false)
            //{
            //    objRecord.VoucherID = 0;
            //    objRecord.EditUserID = UserInfo.ID;
            //    objRecord.EditTime = Comon.cDbl(Lip.GetServerTimeSerial());
            //    objRecord.EditDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
            //    objRecord.EditComputerInfo = UserInfo.ComputerInfo;
            //}



            row = dtDeclaration.Select("DeclareAccountName = 'InsurmentItemsAccount'");
            if (row.Length < 1)
                return 0;


            Acc_VariousVoucherDetails returned;
            List<Acc_VariousVoucherDetails> listreturned = new List<Acc_VariousVoucherDetails>();



            returned = new Acc_VariousVoucherDetails();
            returned.ID = 0;
            returned.BranchID = UserInfo.BRANCHID;
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = AccountID; 
            // returned.AccountID = Comon.cDbl(txtCustomerID.Text); ;
            returned.VoucherID = 0;
            returned.Credit = CreditAmount;
            returned.Debit = 0;
            returned.Declaration = "مرتجع تأمين فاتورة رقم : " + invoiceID;
            returned.CostCenterID = 0;

            listreturned.Add(returned);

            returned = new Acc_VariousVoucherDetails();
            returned.ID = 1;
            returned.BranchID = UserInfo.BRANCHID;
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cDbl(row[0]["AccountID"].ToString());
            returned.VoucherID = 0;
            returned.Credit = 0;
            returned.Debit = CreditAmount;
            returned.Declaration = "مرتجع تأمين فاتورة رقم : " + invoiceID;
            returned.CostCenterID = 0;

            listreturned.Add(returned);

            int Result = 0;
            if (listreturned.Count > 0)
            {
                objRecord.VariousVoucherDetails = listreturned;
                string Result1 = VariousVoucherDAL.InsertUsingXML(objRecord, MySession.UserID);
                Result = Comon.cInt(Result1);
                SplashScreenManager.CloseForm(false);



            }
            else
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(Messages.TitleError, Messages.msgInputIsRequired);

            }
            return Result;
        }








        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            //GridView view = sender as GridView;
            //ID = Comon.cLong(gridView1.GetFocusedRowCellValue("ID").ToString());
            //this.Close();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            try
            {
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);

                /******************** Report Body *************************/
                string ReportName = "rptReservationsReport";
                bool IncludeHeader = true;
                string rptFormName = ReportName;// (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");

                //if (UserInfo.Language == iLanguage.English)
                //    rptFormName = ReportName + "Arb";
                XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);

                /********************** Master *****************************/
                rptForm.RequestParameters = false;
                rptForm.Parameters["FromInvoiceNo"].Value = txtFromInvoiceNo.Text.Trim().ToString();
                rptForm.Parameters["ToInvoiceNo"].Value = txtToInvoiceNo.Text.Trim().ToString();
             
                rptForm.Parameters["CustomerName"].Value = txtCustomerName.Text.Trim().ToString();
           
                rptForm.Parameters["SalesDelegateName"].Value = txtMobile.Text.Trim().ToString();
                rptForm.Parameters["MethodName"].Value = cmbStatus.Text.Trim().ToString();
           


                rptForm.Parameters["FromCloseDate"].Value = txtFromDateReturn.Text.Trim().ToString();
                rptForm.Parameters["ToCloseDate"].Value = txtToDateReturn.Text.Trim().ToString();
                rptForm.Parameters["FromDate"].Value = txtFromDate.Text.Trim().ToString();
                rptForm.Parameters["ToDate"].Value = txtToDate.Text.Trim().ToString();
                for (int i = 0; i < rptForm.Parameters.Count; i++)
                    rptForm.Parameters[i].Visible = false;
                /********************** Details ****************************/
                var dataTable = new dsReports.rptSalesInvoiceReportDataTable();

                for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                {
                    var row = dataTable.NewRow();

                    row["#"] = i + 1;
                    row["InvoiceID"] = gridView1.GetRowCellValue(i, "InvoiceID").ToString();
                    row["nvoiceDate"] = gridView1.GetRowCellValue(i, "InvoiceDate").ToString();
                   // row["CloseCashierDate"] = gridView1.GetRowCellValue(i, "CloseCashierDate").ToString();

                    row["Total"] = gridView1.GetRowCellDisplayText(i, "Status").ToString();
                    row["Discount"] = gridView1.GetRowCellValue(i, "InsuranceAmmount").ToString();
                    row["VatID"] = gridView1.GetRowCellValue(i, "Net").ToString();
                   // row["Net"] = gridView1.GetRowCellValue(i, "InsuranceAmmount1").ToString();
                    row["Profit"] = gridView1.GetRowCellDisplayText(i, "UserID").ToString();
                    row["Notes"] = gridView1.GetRowCellValue(i, "ArbName").ToString();
                 
                    row["CustomerName"] = gridView1.GetRowCellValue(i, "Mobile").ToString();
                    row["SellerName"] = gridView1.GetRowCellValue(i, "InvoiceDateReturn").ToString();
                   
                    
                    row["StoreName"] = gridView1.GetRowCellValue(i, "CloseCashier").ToString();
                    row["CostCenterName"] = gridView1.GetRowCellDisplayText(i, "EditUserID").ToString();
                  //  row["DelgateName"] = gridView1.GetRowCellValue(i, "DelgateName").ToString();
                    row["MethodeName"] = gridView1.GetRowCellValue(i, "NetBalance").ToString();

                    dataTable.Rows.Add(row);
                }
                rptForm.DataSource = dataTable;
                rptForm.DataMember = "rptSalesInvoiceReport";

                /******************** Report Binding ************************/
                XRSubreport subreport = (XRSubreport)rptForm.FindControl("subRptCompanyHeader", true);
                subreport.Visible = IncludeHeader;
                subreport.ReportSource = ReportComponent.CompanyHeaderLand();
                rptForm.ShowPrintStatusDialog = false;
                rptForm.ShowPrintMarginsWarning = false;
                rptForm.CreateDocument();

                SplashScreenManager.CloseForm(false);
                if (ShowReportInReportViewer=true)
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
            frmSalesReservationsReport frm = new frmSalesReservationsReport();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.chkRemaind.Checked = true;
                frm.Show();
            }
            else
                frm.Dispose();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
        }
    }

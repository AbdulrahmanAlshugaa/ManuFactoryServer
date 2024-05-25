using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Menu;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraReports.UI;
using DevExpress.XtraSplashScreen;
using Edex.DAL;
using Edex.DAL.Accounting;
using Edex.DAL.SalseSystem;
using Edex.DAL.Stc_itemDAL;
using Edex.Model;
using Edex.Model.Language;
using Edex.GeneralObjects.GeneralClasses;
using Edex.GeneralObjects.GeneralForms;
using Edex.ModelSystem;
//using Edex.StockObjects.Codes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Edex.AccountsObjects.Codes;
using Edex.StockObjects.Codes;
using System.Text;
using Edex.AccountsObjects.Transactions;

namespace Edex.HR.HRClasses
{
    public partial class frmSalaries : Edex.GeneralObjects.GeneralForms.BaseForm
    {
        public DataTable _sampleData = new DataTable();
        private string strSQL = "ArbName";
        private string PrimaryKeyName = "ArbName";


        private string where = "";
        private string FocusedControl;
        public bool HasColumnErrors = false;
       
         
        DAL_Trans[] DB= new DAL_Trans[1];
        DataRow row;
        public frmSalaries()
        {
            InitializeComponent();
            
            _sampleData.Columns.Add("ColFormAdd", System.Type.GetType("System.Boolean"));
            _sampleData.Columns.Add(new DataColumn("SN", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("EmployeeID", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("EmployeeName", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Salary", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Days", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("OfferTimeHours", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("OfferTime", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("TotalAllowance", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("AbsenceDays", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("Absence", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("OnAccount", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("TotalDeductions", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("SalariesNet", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("OnAccountID", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("PaymentMethode", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Allowance", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Deduction", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("MonthlyAllowance", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Allowance1", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Allowance2", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Allowance3", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Allowance4", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Allowance5", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Allowance6", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Allowance7", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Allowance8", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Allowance9", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Allowance10", typeof(string)));

            _sampleData.Columns.Add(new DataColumn("Deduction1", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Deduction2", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Deduction3", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Deduction4", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Deduction5", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Deduction6", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Deduction7", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Deduction9", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Deduction10", typeof(string)));
            PrimaryKeyName = "ArbName";
            if (UserInfo.Language == iLanguage.English)
            {
                strSQL = "EngName";
                PrimaryKeyName = "EngName";
            }

            FillCombo.FillComboBox(cmbMonths, "MonthsGre", "ID", strSQL, "", "1=1");
            FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", strSQL, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
            FillCombo.FillComboBoxLookUpEdit(cmbAbsencesAccordingTo, "HR_Absentby", "ID", strSQL, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Method" : "حدد الطريقة"));
            cmbBranchesID.EditValue = UserInfo.BRANCHID;
            cmbAbsencesAccordingTo.EditValue = 1;
            InitializeFormatDate(txtTheDate);
            /***************************** Event For GridView *****************************/
            this.KeyPreview = true;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSaries_KeyDown);
            this.gridControl1.ProcessGridKey += new System.Windows.Forms.KeyEventHandler(this.gridControl_ProcessGridKey);
            this.gridView1.CellValueChanging += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridView1_CellValueChanging);
            this.gridView1.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.gridView1_ValidatingEditor);
            /******************************************/

            strSQL = "SELECT  *  FROM dbo.HR_AllowancesTypes  Order By ID";
           DataTable  dt = new DataTable();
            dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
            {
                for (int j = 1; j <= dt.Rows.Count - 1; j++)
                {
                    if(j<=5)
                    gridView1.Columns["Allowance" + (j)].Caption = dt.Rows[j][PrimaryKeyName].ToString();
                }
            }

            strSQL = "SELECT  *  FROM dbo.HR_DeductionsTypes  Order By ID";
             dt = new DataTable();
            dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
            {
                for (int j = 0; j <= dt.Rows.Count - 1; j++)
                {
                    if (j <= 5)
                        gridView1.Columns["Deduction" + (j+1)].Caption = dt.Rows[j][PrimaryKeyName].ToString();
                }
            }

        }

        #region Event

        private void btnShow_Click(object sender, EventArgs e)
        {
            if (cmbMonths.ItemIndex > 0)
            { 
                GetEmployeesInfo();
                GetNoOfDaysInEachMonth();
                GetAllowancesAmount();
                GetDeductionMonthlyAmount();
                SumTotalBalanceAndDiscount();

            }
        }
        private void frmSaries_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
                Find();
            
        }

        private void gridControl_ProcessGridKey(object sender, KeyEventArgs e)
        {
            try
            {
                var grid = sender as GridControl;
                var view = grid.FocusedView as GridView;
                if (view.FocusedColumn == null)
                    return;

                if (e.KeyCode == Keys.Enter)
                {
                    SendKeys.Send("{Tab}");
                }
                else if (e.KeyData == Keys.F5)
                    grid.ShowPrintPreview();

            }
            catch (Exception ex)
            {
                e.Handled = false;
                Messages.MsgError(Messages.TitleInfo, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void gridView1_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (this.gridView1.ActiveEditor is CheckEdit)
            { 
                CalculateRow(gridView1.FocusedRowHandle, Comon.cbool(e.Value.ToString()));
            }

        }

        private void gridView1_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {

            decimal MonthlyAllowance = 0;
            decimal TotalAllowance = 0;
            decimal Salary =0;
            decimal Days = 0;
            decimal TotalInHour =0;
            decimal EmpAllowance = 0;
            decimal hourcount = 0;
            decimal hourcountAbsence = 0;
            decimal EmpDeduction = 0;
            decimal OnAccount = 0;
            decimal TotalDeductions = 0;
            decimal TotalInHourAbsence = 0;

            if (this.gridView1.ActiveEditor is TextEdit)
            {
                GridView view = sender as GridView;
                double num;
                object val = e.Value;

                string ColName = view.FocusedColumn.FieldName;
                if (ColName == "Deduction" ||  ColName == "OfferTimeHours" || ColName == "AbsenceDays" || ColName == "OnAccount")
                {
                    if (val == null || string.IsNullOrWhiteSpace(val.ToString()))
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgInputIsRequired;
                    }

                    if (ColName == "OfferTimeHours")
                    {


                         
                        TotalInHourAbsence = Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Absence").ToString());
                        EmpDeduction = Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Deduction").ToString());
                        OnAccount = Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "OnAccount").ToString());

                        //
                         hourcount = Comon.cDec(val);
                         Salary = Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Salary").ToString());
                         Days = Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Days").ToString());
                         TotalInHour = Comon.ConvertToDecimalPrice( (Salary / Days / 8 )* hourcount);
                         EmpAllowance = Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Allowance").ToString());
                         MonthlyAllowance = Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "MonthlyAllowance").ToString());

                        TotalDeductions = Comon.ConvertToDecimalPrice(EmpDeduction + TotalInHourAbsence + OnAccount);
                        TotalAllowance = Comon.ConvertToDecimalPrice(Salary + MonthlyAllowance + TotalInHour + EmpAllowance - TotalDeductions);


                    }

                    if (ColName == "AbsenceDays")
                    {
                        Days = Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Days").ToString());

                        //استحقاق
                         
                         Salary = Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Salary").ToString());
                         MonthlyAllowance = Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "MonthlyAllowance").ToString());
                         TotalInHour = Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "OfferTime").ToString());
                         EmpAllowance = Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Allowance").ToString());


                        //استقطاع
                         hourcountAbsence = Comon.cDec(val);
                         TotalInHourAbsence = Comon.ConvertToDecimalPrice( (Salary / Days / 8 )* hourcountAbsence);
                         EmpDeduction = Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Deduction").ToString());
                         OnAccount = Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "OnAccount").ToString());
                                          


                        TotalDeductions = Comon.ConvertToDecimalPrice(EmpDeduction + TotalInHourAbsence + OnAccount);
                        TotalAllowance = Comon.ConvertToDecimalPrice(Salary + MonthlyAllowance + TotalInHour + EmpAllowance - TotalDeductions);

                    }

                    if (ColName == "OnAccount")
                    { 
                        OnAccount = Comon.cDec(val);
                        //استحقاق
                        Salary = Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Salary").ToString());
                        MonthlyAllowance = Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "MonthlyAllowance").ToString());
                        TotalInHour = Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "OfferTime").ToString());
                        EmpAllowance = Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Allowance").ToString());
                        //استقطاع
                        TotalInHourAbsence = Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Absence").ToString());
                        EmpDeduction = Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Deduction").ToString());
                        //اجمالي
                        TotalDeductions = Comon.ConvertToDecimalPrice(EmpDeduction + TotalInHourAbsence + OnAccount);
                        TotalAllowance = Comon.ConvertToDecimalPrice(Salary + MonthlyAllowance + TotalInHour + EmpAllowance - TotalDeductions);
                    }

                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["TotalAllowance"], TotalAllowance);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Absence"], TotalInHourAbsence);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["TotalDeductions"], TotalDeductions);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["OfferTime"], TotalInHour);

                    decimal TotalA = Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "TotalAllowance").ToString());
                    decimal TotalD = Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "TotalDeductions").ToString());
                    decimal Net = Comon.ConvertToDecimalPrice(TotalA - TotalD);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SalariesNet"], Net);
                }
            }
            SumTotalBalanceAndDiscount();
        }
        #endregion
        void GetEmployeesInfo()
        {

            string strSQL = "SELECT dbo.HR_EmployeeFile.EmployeeID, dbo.HR_EmployeeFile.ArbName AS EmployeeName, dbo.HR_EmployeeFile.BankAccountID, CostCenterID , " +
                " dbo.HR_PaymentMethode.ArbName AS PaymentMethod, dbo.HR_Departments.ArbName AS Department,OnAccountID FROM dbo.HR_EmployeeFile INNER JOIN" +
                " dbo.HR_PaymentMethode ON dbo.HR_EmployeeFile.PaymentMethod = dbo.HR_PaymentMethode.ID INNER JOIN dbo.HR_Departments ON " +
                " dbo.HR_EmployeeFile.Department = dbo.HR_Departments.ID WHERE (dbo.HR_EmployeeFile.BranchID = " + UserInfo.BRANCHID + ")" +
                " AND (dbo.HR_EmployeeFile.Cancel = 0)  AND (dbo.HR_EmployeeFile.ValidFromDate) >= " + 0 + " " +
                " AND  (dbo.HR_EmployeeFile.StopSalary = 0 or dbo.HR_EmployeeFile.ValidFromDate = 0) ";


            if (txtFromEmpNo.Text != string.Empty)
                strSQL = strSQL + " and  HR_EmployeeFile.EmployeeID >=" + Comon.cDbl(txtFromEmpNo.Text);
            if (txtToEmpNo.Text != string.Empty)
                strSQL = strSQL + " and   HR_EmployeeFile.EmployeeID <=" + Comon.cDbl(txtToEmpNo.Text);



            DataTable dt = new DataTable();
            dt = Lip.SelectRecord(strSQL);
            _sampleData.Clear();

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    row = _sampleData.NewRow();
                    row["SN"] = _sampleData.Rows.Count + 1;
                    row["EmployeeID"] = (dt.Rows[i]["EmployeeID"].ToString());
                    row["EmployeeName"] = dt.Rows[i]["EmployeeName"].ToString();
                    row["OnAccountID"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["OnAccountID"]).ToString();
                    row["PaymentMethode"] = dt.Rows[i]["PaymentMethod"].ToString();
                    row["Allowance"] = 0;
                    row["Deduction"] = 0;

                    row["OfferTime"] = 0;
                    row["Absence"] = 0;
                    row["OnAccount"] = 0;
                    row["Deduction"] = 0;
                    row["MonthlyAllowance"] = 0;
                    row["TotalAllowance"] = 0;
                    row["TotalDeductions"] = 0;
                    row["SalariesNet"] = 0;

                    _sampleData.Rows.Add(row);
                }
            }
            gridControl1.DataSource = _sampleData;
        }

        //استحقافات
        void GetAllowancesAmount()
        {
            int currentYear = 2024;
            for (int k = 0; k < _sampleData.Rows.Count; k++)
            {

                long empid = Comon.cLong(gridView1.GetRowCellValue(k, "EmployeeID"));
                 
                DataTable dt = new DataTable();
                 
                //===============جلب البدلات
                strSQL = "SELECT  *  FROM dbo.HR_EmployeeAllowance WHERE   AllowanceID> 1 And  (BranchID = "+UserInfo.BRANCHID+") AND (Cancel = 0) AND (EmployeeID = "+empid+") AND (STR(LTRIM(RTRIM(dbo.HR_EmployeeAllowance.RegDate))) LIKE '%"+ Comon.ConvertDateToSerial(txtTheDate.EditValue.ToString()).ToString().Substring(0, 4)+"%') order by  AllowanceID ";
                dt = new DataTable();
                dt = Lip.SelectRecord(strSQL);
                decimal Amount = 0;
                decimal AmountAllowance = 0;
                decimal AmountAllowanceMounthly = 0;
                decimal Salary = 0;
                if (dt.Rows.Count > 0)
                {
                    for (int j = 0; j <= dt.Rows.Count - 1; j++)
                    {
                        int AllowanceID = Comon.cInt(dt.Rows[j]["AllowanceID"].ToString());
                        if (AllowanceID <= 6)
                        {
                            
                            _sampleData.Rows[k]["Allowance" + (AllowanceID-1)] = dt.Rows[j]["Amount"].ToString();
                            AmountAllowance = AmountAllowance + Comon.cDec(dt.Rows[j]["Amount"].ToString());
                        }
                        else
                        {
                            Amount = Amount + Comon.cDec(dt.Rows[j]["Amount"].ToString());
                        }
                    }

                    _sampleData.Rows[k]["Allowance"] = (AmountAllowance + Amount);//استحقاق قياسي + اخرى
                    _sampleData.Rows[k]["Allowance6"  ] = Amount;


                }
                //===============================
                strSQL = "SELECT EmployeeID,   SUM(Amount) AS Amount  FROM dbo.HR_EmployeeAllowance WHERE   AllowanceID= 1 And  (BranchID = "+UserInfo.BRANCHID+") AND (Cancel = 0) AND (EmployeeID = "+empid+") AND (STR(LTRIM(RTRIM(dbo.HR_EmployeeAllowance.RegDate))) LIKE '%"+ Comon.ConvertDateToSerial(txtTheDate.EditValue.ToString()).ToString().Substring(0, 4)+"%') GROUP BY EmployeeID ";
                dt = new DataTable();
                dt = Lip.SelectRecord(strSQL);
                gridView1.SetRowCellValue(k, gridView1.Columns["Salary"], 0);
                if (dt.Rows.Count > 0)
                {
                    _sampleData.Rows[k]["Salary"] = dt.Rows[0]["Amount"].ToString();
                      Salary =Comon.cDec( dt.Rows[0]["Amount"].ToString());
                }

                 

                strSQL = @"SELECT  SUM(Amount) AS Amount 
                        FROM   HR_MonthlyAllowance
                        WHERE  (ValidFromDate >= " + Comon.ConvertDateToSerial(GetFirstDayOfMonth(cmbMonths.ItemIndex, currentYear)) + ") AND " +
                        "(ValidFromDate <= " + Comon.ConvertDateToSerial(GetLastDayOfMonth(cmbMonths.ItemIndex, currentYear)) + ") AND (EmployeeID = " + empid + ")  ";


                 
                dt = Lip.SelectRecord(strSQL.ToString());
                gridView1.SetRowCellValue(k, gridView1.Columns["MonthlyAllowance"], 0);
                if (dt.Rows.Count > 0)
                {
                    if (Comon.cDec(dt.Rows[0]["Amount"].ToString()) > 0)
                        AmountAllowanceMounthly = Comon.cDec(dt.Rows[0]["Amount"].ToString());
                }
                _sampleData.Rows[k]["MonthlyAllowance"] = AmountAllowanceMounthly  ;
                _sampleData.Rows[k]["Allowance"] =  AmountAllowance + Amount ;
                _sampleData.Rows[k]["TotalAllowance"] = AmountAllowanceMounthly+ AmountAllowance + Amount +  Salary;
                
            }

            

            gridControl1.DataSource = _sampleData;
        }
        
        //   شهرية
        void GetDeductionMonthlyAmount()
        {
            int currentYear = 2024;
            for (int k = 0; k < _sampleData.Rows.Count; k++)
            {

                long empid = Comon.cLong(gridView1.GetRowCellValue(k, "EmployeeID"));

                double OnAccount = Comon.cDbl(gridView1.GetRowCellValue(k, "OnAccountID").ToString());

                strSQL = "SELECT  *   from HR_MonthlyDeduction Where EmployeeID= " + empid 
                    + " And ValidFromDate>=" + Comon.ConvertDateToSerial(GetFirstDayOfMonth(cmbMonths.ItemIndex, currentYear))
                    + " And ValidFromDate <=" + Comon.ConvertDateToSerial(GetLastDayOfMonth(cmbMonths.ItemIndex, currentYear)) + " Order By DeductionID";
                 


                DataTable dt = new DataTable();
                dt = Lip.SelectRecord(strSQL.ToString());
                gridView1.SetRowCellValue(k, gridView1.Columns["Deduction"], 0);
                 
                decimal AmountAllowance = 0;
                decimal Amount = 0;
                decimal Amount3 = 0;
                for (int j = 0; j <= dt.Rows.Count - 1; j++)
                {
                    int AllowanceID = Comon.cInt(dt.Rows[j]["DeductionID"].ToString());
                    if (AllowanceID == 1 )
                    {
                        //معتمده على شاشات اخرى
                        decimal OnAccountAmount= Math.Abs( Comon.cDec( Lip.GetValue("Select (Sum (Debit) - Sum (Credit))  AS Amount from Acc_VariousVoucherMachinDetails Where AccountID= " + OnAccount)));
                        _sampleData.Rows[k]["Deduction" + (AllowanceID)] =Math.Abs( OnAccountAmount);
                        AmountAllowance = AmountAllowance + Comon.cDec(_sampleData.Rows[k]["Deduction" + (AllowanceID)]);
                    }
                    else  if(AllowanceID < 5 && AllowanceID >1 )
                    {
                        //معتمده على شاشات اخرى
                        _sampleData.Rows[k]["Deduction" + (AllowanceID)] = dt.Rows[j]["Amount"].ToString();
                        AmountAllowance = AmountAllowance + Comon.cDec(dt.Rows[j]["Amount"].ToString());
                    }
                    else if (AllowanceID == 5)
                    {
                        //معتمده على الاستقطاع الشهري
                        Amount3 = Amount3 + Comon.cDec(dt.Rows[j]["Amount"].ToString());
                    }

                    else if (AllowanceID > 5)
                    {
                        // معتمدة على الاستقطاعات الشهريه لكن اخرى    
                        Amount = Amount + Comon.cDec(dt.Rows[j]["Amount"].ToString());
                    }

                }

                _sampleData.Rows[k]["TotalDeductions"] = (AmountAllowance + Amount+ Amount3);//استحقاق قياسي + اخرى
                _sampleData.Rows[k]["Deduction6"] = Amount;
                _sampleData.Rows[k]["Deduction5"] = Amount3;


            }
            gridControl1.DataSource = _sampleData;
        }

         
        void GetNoOfDaysInEachMonth()
        {
            string strSQL;
            DataTable dt = new DataTable();
            Lip.FormatType = "Georgian";
            if (Lip.FormatType == "Georgian")
            {
                strSQL = "SELECT DaysNo FROM MonthsGre WHERE ID = "+cmbMonths.EditValue;
                dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < gridView1.RowCount; i++)
                    {
                        
                        gridView1.SetRowCellValue(i, gridView1.Columns["Days"], dt.Rows[0]["DaysNo"].ToString());
                    }
                }
            }
            else
            {
                strSQL = "SELECT DaysNo FROM MonthsHijri WHERE ID = "+cmbMonths.EditValue;
                dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < gridView1.RowCount; i++)
                    {
                        gridView1.SetRowCellValue(i, gridView1.Columns["Days"], dt.Rows[0]["DaysNo"].ToString());
                    }
                }
            }
        }
        private string GetFirstDayOfMonth(int iMonth, int iYear)
        {
            DateTime dtFrom = new DateTime(iYear, iMonth, 1);
            dtFrom = dtFrom.AddDays(-(dtFrom.Day - 1));
            return dtFrom.ToString("yyyy/MM/dd");
        }

        private string GetLastDayOfMonth(int iMonth, int iYear)
        {
            int daysInMonth = DateTime.DaysInMonth(iYear, iMonth);
            DateTime dtTo = new DateTime(iYear, iMonth, 1);
            dtTo = dtTo.AddDays((daysInMonth));
            return dtTo.ToString("yyyy/MM/dd");

            
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

        private void CalculateRow(int Row = -1, bool IsHavVat = false)
        {
            try
            {
                SumTotalBalanceAndDiscount(Row, IsHavVat);
                //Remove Icon Validtion
                var Net = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Net");
                var Total = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Total");
                if ((Total != null && !(string.IsNullOrWhiteSpace(Total.ToString())) && Comon.cDec(Total.ToString()) > 0))
                    gridView1.SetColumnError(gridView1.Columns["Total"], "");
                if ((Net != null && !(string.IsNullOrWhiteSpace(Net.ToString())) && Comon.cDec(Net.ToString()) > 0))
                    gridView1.SetColumnError(gridView1.Columns["Net"], "");
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void SumTotalBalanceAndDiscount(int row = -1, bool IsHavVat = false)
        {
            try
            {
                decimal MonthlyAllowance = 0;
                decimal TotalAllowance = 0;
                decimal Salary = 0;
                decimal Days = 0;
                decimal TotalInHour = 0;
                decimal EmpAllowance = 0;
                decimal hourcount = 0;
                decimal hourcountAbsence = 0;
                decimal EmpDeduction = 0;
                decimal OnAccount = 0;
                decimal TotalDeductions = 0;
                decimal TotalInHourAbsence = 0;

                decimal TotalAllAllowance = 0;
                decimal TotalAllDeductions = 0;
                decimal NetBalance = 0;
                decimal NetBalanceAll = 0;


                for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                {

                    //استحقافات
                    hourcount = Comon.cDec(gridView1.GetRowCellValue(i, "OfferTimeHours").ToString());
                    Salary = Comon.cDec(gridView1.GetRowCellValue(i, "Salary").ToString());
                    Days = Comon.cDec(gridView1.GetRowCellValue(i, "Days").ToString());
                    TotalInHour = Comon.ConvertToDecimalPrice((Salary / Days / 8) * hourcount);
                    EmpAllowance = Comon.cDec(gridView1.GetRowCellValue(i, "Allowance").ToString());
                    MonthlyAllowance = Comon.cDec(gridView1.GetRowCellValue(i, "MonthlyAllowance").ToString());
                    TotalAllowance = Comon.ConvertToDecimalPrice(Salary + MonthlyAllowance + TotalInHour + EmpAllowance);

                    //استقطاعات
                    hourcount = Comon.cDec(gridView1.GetRowCellValue(i, "AbsenceDays").ToString());
                    TotalInHourAbsence = Comon.ConvertToDecimalPrice((Salary / Days / 8) * hourcountAbsence);
                    EmpDeduction = Comon.cDec(gridView1.GetRowCellValue(i, "Deduction").ToString());
                    OnAccount = Comon.cDec(gridView1.GetRowCellValue(i, "OnAccount").ToString());
                    TotalDeductions = Comon.ConvertToDecimalPrice(EmpDeduction + TotalInHourAbsence + OnAccount);

                    TotalAllowance = Comon.cDec(gridView1.GetRowCellValue(i, "TotalAllowance").ToString());
                    TotalDeductions = Comon.cDec(gridView1.GetRowCellValue(i, "TotalDeductions").ToString());
                    NetBalance = Comon.ConvertToDecimalPrice(TotalAllowance - TotalDeductions);
                     

                     
                     
                    gridView1.SetRowCellValue(i, gridView1.Columns["SalariesNet"], NetBalance);



                    TotalAllAllowance += TotalAllowance;
                    TotalAllDeductions += TotalDeductions;
                    NetBalanceAll += NetBalance;

                    
                }

                lblTotalAllowance.Text = Comon.cDec(TotalAllAllowance).ToString("N" + MySession.GlobalPriceDigits);
                lblTotalDeductions.Text = Comon.cDec(TotalAllDeductions).ToString("N" + MySession.GlobalPriceDigits);
                lblNetBalance.Text = Comon.cDec(NetBalanceAll).ToString("N" + MySession.GlobalPriceDigits);
            }

            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void cmbMonths_EditValueChanged(object sender, EventArgs e)
        {
            //btnShow_Click(null, null);
        }

        private void lnkVariousVoucher_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            BindingList<Acc_VariousVoucherDetails> lstDetail = new BindingList<Acc_VariousVoucherDetails>();
            
            frmVariousVoucher frm = new frmVariousVoucher();
            //if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            //{
            if (UserInfo.Language == iLanguage.English)
                ChangeLanguage.EnglishLanguage(frm);
            frm.FormView = true;
            frm.FormAdd = true;
            //frm.FormUpdate = true;

            frm.Show();
            frm.ClearFields();
            frm.NewReord();
            frm.cmbBranchesID.EditValue =Comon.cInt( cmbBranchesID.EditValue.ToString());

            //}
            //else return;

            lstDetail = new BindingList<Acc_VariousVoucherDetails>();
            lstDetail.AllowNew = true;
            lstDetail.AllowEdit = true;
            lstDetail.AllowRemove = true;
            frm.gridControl.DataSource = lstDetail;
            decimal[] arrayName = new decimal[20];
            Acc_VariousVoucherDetails Detail = new Acc_VariousVoucherDetails();
            for (int k = 0; k <= gridView1.RowCount-1; k++)
            {
                 
                Detail = new Acc_VariousVoucherDetails();
                long empid = Comon.cLong(gridView1.GetRowCellValue(k, "EmployeeID"));
                long OnAccountID = Comon.cLong(gridView1.GetRowCellValue(k, "OnAccountID"));
                double Credit = Comon.cDbl(gridView1.GetRowCellValue(k, "TotalAllowance"));
                Detail.AccountID = OnAccountID;
                Detail.Debit = 0;
                Detail.Credit = Credit;
                Detail.Declaration =  " قيد استحقاق عن شهر "+cmbMonths.Text ;
                Detail.ArbAccountName = gridView1.GetRowCellValue(k, "EmployeeName").ToString();
                Detail.EngAccountName = gridView1.GetRowCellValue(k, "EmployeeName").ToString();

                arrayName[0] = arrayName[0] + Comon.cDec(gridView1.GetRowCellValue(k, "Salary"));
                
                arrayName[1] = arrayName[1] + Comon.cDec(gridView1.GetRowCellValue(k, "Allowance1"));
                arrayName[2] = arrayName[2] + Comon.cDec(gridView1.GetRowCellValue(k, "Allowance2"));
                arrayName[3] = arrayName[3] + Comon.cDec(gridView1.GetRowCellValue(k, "Allowance3"));
                arrayName[4] = arrayName[4] + Comon.cDec(gridView1.GetRowCellValue(k, "Allowance4"));
                arrayName[5] = arrayName[5] + Comon.cDec(gridView1.GetRowCellValue(k, "Allowance5"));
                arrayName[6] = arrayName[6] + Comon.cDec(gridView1.GetRowCellValue(k, "Allowance6"));
                arrayName[7] = arrayName[7] + Comon.cDec(gridView1.GetRowCellValue(k, "MonthlyAllowance"));


                arrayName[8] = arrayName[8] + Comon.cDec(gridView1.GetRowCellValue(k, "Deduction1"));
                arrayName[9] = arrayName[9] + Comon.cDec(gridView1.GetRowCellValue(k, "Deduction2"));
                arrayName[10] = arrayName[10] + Comon.cDec(gridView1.GetRowCellValue(k, "Deduction3"));
                arrayName[11] = arrayName[11] + Comon.cDec(gridView1.GetRowCellValue(k, "Deduction4"));
                arrayName[12] = arrayName[12] + Comon.cDec(gridView1.GetRowCellValue(k, "Deduction5"));
                arrayName[13] = arrayName[13] + Comon.cDec(gridView1.GetRowCellValue(k, "Deduction6"));

                arrayName[14] = arrayName[14] + Comon.cDec(gridView1.GetRowCellValue(k, "TotalAllowance"));
                arrayName[15] = arrayName[15] + Comon.cDec(gridView1.GetRowCellValue(k, "TotalDeductions"));


                arrayName[16] = arrayName[16] + Comon.cDec(gridView1.GetRowCellValue(k, "SalariesNet"));

                if (Detail.Credit == 0)
                    continue;
                lstDetail.Add(Detail);
            }
            GetEmpAlwanceTotal(ref Detail, ref arrayName, ref lstDetail);
            //GetEmpDeductionIDTotal(ref Detail, ref arrayName, ref lstDetail);

            frm.gridControl.DataSource = lstDetail;
            frm.CalculatTotalBalance();
        }


        void GetEmpAlwanceTotal(ref Acc_VariousVoucherDetails Detail,ref decimal[] arrayName,ref BindingList<Acc_VariousVoucherDetails> lstDetail)
        {
            DataTable dtAlloance = Lip.SelectRecord("SELECT ID, AccountID,ArbName  FROM HR_AllowancesTypes");

            DataTable dt = new DataTable();
            strSQL = "Select   *  from Acc_Accounts Where Accountlevel=5";
            dt = Lip.SelectRecord(strSQL);
             

            DataTable dtEmployeeAllowance = new DataTable();
            for (int i = 0; i <= dtAlloance.Rows.Count - 1; i++)
            {

                Detail = new Acc_VariousVoucherDetails();
                DataRow[] row = dt.Select("AccountID =" + dtAlloance.Rows[i]["AccountID"].ToString());
                if (row.Length > 0)
                {
                    Detail.ArbAccountName = row[0]["ArbName"].ToString();
                    strSQL = "Select sum(Amount) AS Amount from HR_EmployeeAllowance Where AllowanceID =" + (i+1) ;
                    dtEmployeeAllowance = Lip.SelectRecord(strSQL);
                    if (dtEmployeeAllowance.Rows.Count > 0)
                    {
                        Detail.AccountID = Comon.cDbl(row[0]["AccountID"].ToString());
                        Detail.Debit = Comon.cDbl(dtEmployeeAllowance.Rows[0]["Amount"].ToString());
                        Detail.Credit = 0;
                        Detail.Declaration = " قيد استحقاق عن شهر "+cmbMonths.Text ;
                        if (Detail.Debit > 0)
                        lstDetail.Add(Detail);
                    }


                    Detail.ArbAccountName = row[0]["ArbName"].ToString();
                    int currentYear = 2024;

                    strSQL = @"SELECT  SUM(Amount) AS Amount 
                        FROM   HR_MonthlyAllowance
                        WHERE  (ValidFromDate >= " + Comon.ConvertDateToSerial(GetFirstDayOfMonth(cmbMonths.ItemIndex, currentYear)) + ") AND " +
                        "(ValidFromDate <= " + Comon.ConvertDateToSerial(GetLastDayOfMonth(cmbMonths.ItemIndex, currentYear)) + ") AND (AllowanceID = " + (i+1) + ")  ";

                     
                    dtEmployeeAllowance = Lip.SelectRecord(strSQL);
                    if (dtEmployeeAllowance.Rows.Count > 0)
                    {
                        Detail = new Acc_VariousVoucherDetails();
                        Detail.AccountID = Comon.cDbl(row[0]["AccountID"].ToString());
                        Detail.Debit = Comon.cDbl(dtEmployeeAllowance.Rows[0]["Amount"].ToString());
                        Detail.Credit = 0;
                        Detail.Declaration = " قيد استحقاق عن شهر " + cmbMonths.Text;
                        if (Detail.Debit > 0)
                            lstDetail.Add(Detail);
                    }

                }


            }
        }


        void GetEmpDeductionIDTotal(ref Acc_VariousVoucherDetails Detail, ref decimal[] arrayName, ref BindingList<Acc_VariousVoucherDetails> lstDetail)
        {
            DataTable dtAlloance = Lip.SelectRecord("SELECT ID, AccountID,ArbName  FROM HR_DeductionsTypes");
            DataTable dt = new DataTable();
            strSQL = "Select   *  from Acc_Accounts Where Accountlevel=5";
            dt = Lip.SelectRecord(strSQL);
            DataTable dtEmployeeAllowance = new DataTable();
            for (int i = 0; i <= dtAlloance.Rows.Count - 1; i++)
            { 
                Detail = new Acc_VariousVoucherDetails();
                DataRow[] row = dt.Select("AccountID =" + dtAlloance.Rows[i]["AccountID"].ToString());
                if (row.Length > 0)
                {
                    Detail.ArbAccountName = row[0]["ArbName"].ToString();
                    int currentYear = 2024;
                    strSQL = @"SELECT  SUM(Amount) AS Amount  FROM   HR_MonthlyDeduction  WHERE  (ValidFromDate >= " + Comon.ConvertDateToSerial(GetFirstDayOfMonth(cmbMonths.ItemIndex, currentYear)) + ") AND " +
                        "(ValidFromDate <= " + Comon.ConvertDateToSerial(GetLastDayOfMonth(cmbMonths.ItemIndex, currentYear)) + ") AND (DeductionID = " + (i + 1) + ")  ";
                    dtEmployeeAllowance = Lip.SelectRecord(strSQL);
                    if (dtEmployeeAllowance.Rows.Count > 0)
                    {
                        int DeductionID = Comon.cInt(dtAlloance.Rows[i]["ID"].ToString());
                        long OnAccountID = Comon.cLong(gridView1.GetRowCellValue(i, "OnAccountID"));
                        if (DeductionID == 1)
                        {
                            //معتمده على شاشات اخرى
                            Detail.Credit =Math.Abs( Comon.cDbl(Lip.GetValue("Select (Sum (Debit) - Sum (Credit))  AS Amount from Acc_VariousVoucherMachinDetails Where AccountID= " + OnAccountID)));
                        }
                        else
                        {
                            Detail.Credit = Comon.cDbl(dtEmployeeAllowance.Rows[0]["Amount"].ToString());
                        }

                        
                        Detail.AccountID = Comon.cDbl(row[0]["AccountID"].ToString());
                        Detail.Debit  = 0;
                        Detail.Declaration = " قيد استقطاع عن شهر " + cmbMonths.Text;
                        if (Detail.Credit > 0)
                            lstDetail.Add(Detail);
                    }

                }
            }
        }
         

        private void lnkSpendVoucher_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {


            BindingList<Acc_VariousVoucherDetails> lstDetail = new BindingList<Acc_VariousVoucherDetails>();
            Acc_VariousVoucherDetails Detail = new Acc_VariousVoucherDetails();
            frmVariousVoucher frm = new frmVariousVoucher();
            //if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            //{
            if (UserInfo.Language == iLanguage.English)
                ChangeLanguage.EnglishLanguage(frm);
            frm.FormView = true;
            frm.FormAdd = true;
            //frm.FormUpdate = true;

            frm.Show();
            frm.ClearFields();
            frm.NewReord();
            frm.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue.ToString());

            //}
            //else return;

            lstDetail = new BindingList<Acc_VariousVoucherDetails>();
            lstDetail.AllowNew = true;
            lstDetail.AllowEdit = true;
            lstDetail.AllowRemove = true;
            frm.gridControl.DataSource = lstDetail;
            Detail.ArbAccountName = "  روات وأجور ";
            Detail.AccountID = 1201001001;
            Detail.Debit = 0;
            Detail.Credit = Comon.cDbl(lblNetBalance.Text);
            Detail.Declaration = cmbMonths.Text + " قيد صرف عن شهر ";
            lstDetail.Add(Detail);
            for (int k = 0; k < _sampleData.Rows.Count; k++)
            {
                Detail = new Acc_VariousVoucherDetails();
                long empid = Comon.cLong(gridView1.GetRowCellValue(k, "EmployeeID"));
                long OnAccountID = Comon.cLong(gridView1.GetRowCellValue(k, "OnAccountID"));
                double Credit = Comon.cDbl(gridView1.GetRowCellValue(k, "SalariesNet"));
                Detail.AccountID = OnAccountID;
                Detail.Debit = Credit;
                Detail.Credit = 0;
                Detail.Declaration = cmbMonths.Text + " قيد صرف عن شهر ";
                Detail.ArbAccountName = gridView1.GetRowCellValue(k, "EmployeeName").ToString();
                Detail.EngAccountName = gridView1.GetRowCellValue(k, "EmployeeName").ToString();

                lstDetail.Add(Detail);
            }
            frm.gridControl.DataSource = lstDetail;
            frm.CalculatTotalBalance();
        }
        //protected override void DoPrint()
        //{
        //    try
        //    {
        //        Application.DoEvents();
        //        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
        //        /******************** Report Body *************************/
        //        bool IncludeHeader = true;
        //        ReportName = "rptSalaries";
        //        string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");
        //        ReportName = "rptSalaries";
        //        if (UserInfo.Language == iLanguage.English)
        //            rptFormName = ReportName + "Arb";
        //        XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);
        //        /***************** Master *****************************/
        //        rptForm.RequestParameters = false;
        //        rptForm.Parameters["NetBalance"].Value = lblNetBalance.Text.Trim().ToString();
        //        rptForm.Parameters["MounthName"].Value = cmbMonths.Text.Trim().ToString();
        //        rptForm.Parameters["BranchName"].Value = cmbBranchesID.Text.Trim().ToString();
        //        rptForm.Parameters["DaysNo"].Value = gridView1.GetRowCellValue(0, "Days").ToString();

        //        rptForm.Parameters["TotalAllowance"].Value = lblTotalAllowance.Text.Trim().ToString();
        //        rptForm.Parameters["TotalDeduction"].Value = lblTotalDeductions.Text.Trim().ToString();
        //        rptForm.Parameters["NetBalance"].Value = lblNetBalance.Text.Trim().ToString();
        //        rptForm.Parameters["PrintDate"].Value = DateTime.Now.ToString();
        //        rptForm.Parameters["IssusDate"].Value = txtTheDate.Text.Trim().ToString();
        //        /********************** Details ****************************/
        //        var dataTable = new dsReports.rptHRSalariesDataTable();
        //        for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
        //        {
        //            var row = dataTable.NewRow();
        //            row["SN"] = i + 1;
        //            row["EmployeeID"] = gridView1.GetRowCellValue(i, "EmployeeID").ToString();
        //            row["EmployeeName"] = gridView1.GetRowCellValue(i, "EmployeeName").ToString();
        //            row["Salary"] = gridView1.GetRowCellValue(i, "Salary").ToString();
        //            row["Days"] = gridView1.GetRowCellValue(i, "Days").ToString();
        //            row["OfferTimeHours"] = gridView1.GetRowCellValue(i, "OfferTimeHours").ToString();
        //            row["OfferTime"] = gridView1.GetRowCellValue(i, "OfferTime").ToString();
        //            row["Allowance"] = gridView1.GetRowCellValue(i, "Allowance").ToString();
        //            row["TotalAllowance"] = gridView1.GetRowCellValue(i, "TotalAllowance").ToString();
        //            row["AbsenceDays"] = gridView1.GetRowCellValue(i, "AbsenceDays").ToString();
        //            row["Absence"] = gridView1.GetRowCellValue(i, "Absence").ToString();
        //            row["OnAccount"] = gridView1.GetRowCellValue(i, "OnAccount").ToString();
        //            row["Deduction"] = gridView1.GetRowCellValue(i, "Deduction").ToString();
        //            row["TotalDeductions"] = gridView1.GetRowCellValue(i, "TotalDeductions").ToString();
        //            row["SalariesNet"] = gridView1.GetRowCellValue(i, "SalariesNet").ToString();
        //            row["PaymentMethode"] = gridView1.GetRowCellValue(i, "PaymentMethode").ToString();
        //            dataTable.Rows.Add(row);
        //        }
        //        rptForm.DataSource = dataTable;
        //        rptForm.DataMember = "rptHRSalaries";
        //        /******************** Report Binding ************************/
        //        XRSubreport subreport = (XRSubreport)rptForm.FindControl("subRptCompanyHeader", true);
        //        subreport.Visible = IncludeHeader;
        //        subreport.ReportSource = ReportComponent.CompanyHeader();
        //        rptForm.ShowPrintStatusDialog = false;
        //        rptForm.ShowPrintMarginsWarning = false;
        //        rptForm.CreateDocument();
        //        SplashScreenManager.CloseForm(false);
        //        ShowReportInReportViewer = true;
        //        if (ShowReportInReportViewer)
        //        {
        //            frmReportViewer frmRptViewer = new frmReportViewer();
        //            frmRptViewer.documentViewer1.DocumentSource = rptForm;
        //            frmRptViewer.ShowDialog();
        //        }
        //        else
        //        {
        //            bool IsSelectedPrinter = false;
        //            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
        //            DataTable dt = ReportComponent.SelectRecord("SELECT *  from Printers where ReportName='" + ReportName + "'");
        //            for (int i = 1; i < 6; i++)
        //            {
        //                string PrinterName = dt.Rows[0]["PrinterName" + i.ToString()].ToString().ToUpper();
        //                if (!string.IsNullOrEmpty(PrinterName))
        //                {
        //                    rptForm.PrinterName = PrinterName;
        //                    rptForm.Print(PrinterName);
        //                    IsSelectedPrinter = true;
        //                }
        //            }
        //            SplashScreenManager.CloseForm(false);
        //            if (!IsSelectedPrinter)
        //                Messages.MsgWarning(Messages.TitleWorning, Messages.msgThereIsNotPrinterSelected);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        SplashScreenManager.CloseForm(false);
        //        Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
        //    }
        //}


        private   void  PrintEmpStatment(long EmpID,int rowindex)
        {
            try
            {
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                /******************** Report Body *************************/
                bool IncludeHeader = true;
                ReportName = "‏‏rptSalaryAndAlwanceEmp";
                string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");
                ReportName = "‏‏rptSalaryAndAlwanceEmp";
                if (UserInfo.Language == iLanguage.English)
                    rptFormName = ReportName + "Arb";
                XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);
                /***************** Master *****************************/
                rptForm.RequestParameters = false;
                rptForm.Parameters["NetBalance"].Value = lblNetBalance.Text.Trim().ToString();
                rptForm.Parameters["MounthName"].Value = cmbMonths.Text.Trim().ToString();
                rptForm.Parameters["BranchName"].Value = cmbBranchesID.Text.Trim().ToString();
                rptForm.Parameters["DaysNo"].Value = gridView1.GetRowCellValue(rowindex, "Days").ToString();

                rptForm.Parameters["TotalAllowance"].Value = lblTotalAllowance.Text.Trim().ToString();
                rptForm.Parameters["TotalDeduction"].Value = lblTotalDeductions.Text.Trim().ToString();
                rptForm.Parameters["NetBalance"].Value = lblNetBalance.Text.Trim().ToString();
                rptForm.Parameters["PrintDate"].Value = DateTime.Now.ToString();
                rptForm.Parameters["IssusDate"].Value = txtTheDate.Text.Trim().ToString();
                rptForm.Parameters["EmployeeName"].Value = gridView1.GetRowCellValue(rowindex, "EmployeeName").ToString();


                /********************** Details ****************************/
                var dataTable = new dsReports.rptBalanceReviewDataTable();

                strSQL = "Select * from HR_EmployeeAllowance where EmployeeID=" + EmpID;
                DataTable dtalwanc = Lip.SelectRecord(strSQL);

                for (int i = 0; i <= dtalwanc.Rows.Count  - 1; i++)
                {

                    var row = dataTable.NewRow();
                    row["n_invoice_serial"] = i + 1;
                    row["Balance"] = dtalwanc.Rows[i]["Notes"].ToString();
                    row["Debit"] = dtalwanc.Rows[i]["Amount"].ToString();
                    row["Credit"] = "0";
                    row["DebitGold"] = "0";
                    dataTable.Rows.Add(row);
                }
                
               strSQL = "Select * from  HR_MonthlyAllowance  where EmployeeID=" + EmpID;
                DataTable dtMonthlyDeduction = Lip.SelectRecord(strSQL);

                for (int i = 0; i <= dtMonthlyDeduction.Rows.Count  - 1; i++)
                { 
                    var row = dataTable.NewRow();
                    row["n_invoice_serial"] = i + 1;
                    row["Balance"] = dtMonthlyDeduction.Rows[i]["Notes"].ToString();
                    row["Debit"] = dtMonthlyDeduction.Rows[i]["Amount"].ToString();
                    row["Credit"] = "0";
                    row["DebitGold"] = "0";
                    dataTable.Rows.Add(row);
                }

                strSQL = "Select * from HR_MonthlyDeduction where EmployeeID=" + EmpID;
                DataTable dtonthlyAllowance = Lip.SelectRecord(strSQL);

                for (int i = 0; i <= dtonthlyAllowance.Rows.Count - 1; i++)
                {

                    var row = dataTable.NewRow();
                    row["n_invoice_serial"] = i + 1;
                    row["Balance"] = "0";
                    row["Debit"] ="0";
                    row["Credit"] = dtonthlyAllowance.Rows[i]["Notes"].ToString();
                    row["DebitGold"] = dtonthlyAllowance.Rows[i]["Amount"].ToString();
                    dataTable.Rows.Add(row);
                }



                rptForm.DataSource = dataTable;
                rptForm.DataMember = "rptSalaryAndAlwanceEmp";
                /******************** Report Binding ************************/
                XRSubreport subreport = (XRSubreport)rptForm.FindControl("subRptCompanyHeader", true);
                subreport.Visible = IncludeHeader;
                subreport.ReportSource = ReportComponent.CompanyHeader();
                rptForm.ShowPrintStatusDialog = false;
                rptForm.ShowPrintMarginsWarning = false;
                rptForm.CreateDocument();
                SplashScreenManager.CloseForm(false);
                ShowReportInReportViewer = true;
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
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        protected override void DoSave()
        {
            string mounth = txtTheDate.EditValue.ToString().Substring(3, 2);


            DB = new DAL_Trans[0];
            Lip.ExecututeSQL("Delete from HR_SalariesHistory Where Month=" + mounth);
            InsertMasterData(ref DB);

        }

        public void InsertMasterData(ref DAL_Trans[] DB)
        {
            for (int i = 0; i < gridView1.RowCount; i++)
            { 
                Array.Resize(ref DB, DB.Length + 1);
                DB[DB.Length - 1] =   new DAL_Trans();
                DB[DB.Length - 1].Table = "HR_SalariesHistory";
                DB[DB.Length - 1].AddNumericField("BranchID", UserInfo.BRANCHID.ToString());
                DB[DB.Length - 1].AddNumericField("EmployeeID", gridView1.GetRowCellValue(i, "EmployeeID").ToString());
                DB[DB.Length - 1].AddNumericField("Month", cmbMonths.EditValue.ToString());
                DB[DB.Length - 1].AddNumericField("AbsencesAccordingTo", gridView1.GetRowCellValue(i, "Absence").ToString());
                DB[DB.Length - 1].AddNumericField("TheDate", txtTheDate.Text);
                DB[DB.Length - 1].AddStringField("EmployeeName", gridView1.GetRowCellValue(i, "EmployeeName").ToString());
                DB[DB.Length - 1].AddNumericField("Days", gridView1.GetRowCellValue(i, "Days").ToString());
                DB[DB.Length - 1].AddNumericField("AbsenceDays", gridView1.GetRowCellValue(i, "AbsenceDays").ToString());
                DB[DB.Length - 1].AddNumericField("Absence", gridView1.GetRowCellValue(i, "Absence").ToString());
                DB[DB.Length - 1].AddNumericField("OfferTimeHours", gridView1.GetRowCellValue(i, "OfferTimeHours").ToString());
                DB[DB.Length - 1].AddNumericField("OfferTime", gridView1.GetRowCellValue(i, "OfferTime").ToString());
                DB[DB.Length - 1].AddNumericField("Allowance", gridView1.GetRowCellValue(i, "Allowance").ToString());
                DB[DB.Length - 1].AddNumericField("Deduction", gridView1.GetRowCellValue(i, "Deduction").ToString());
                DB[DB.Length - 1].AddNumericField("TotalAllowance", gridView1.GetRowCellValue(i, "TotalAllowance").ToString());
                DB[DB.Length - 1].AddNumericField("TotalDeductions", gridView1.GetRowCellValue(i, "TotalDeductions").ToString());
                DB[DB.Length - 1].AddNumericField("Salary", gridView1.GetRowCellValue(i, "Salary").ToString());
                DB[DB.Length - 1].AddNumericField("MonthlyAllowance", gridView1.GetRowCellValue(i, "MonthlyAllowance").ToString());

                DB[DB.Length - 1].AddNumericField("SalariesNet", gridView1.GetRowCellValue(i, "SalariesNet").ToString());
                DB[DB.Length - 1].AddNumericField("OnAccount", gridView1.GetRowCellValue(i, "OnAccount").ToString());
                DB[DB.Length - 1].AddNumericField("OnAccountID", gridView1.GetRowCellValue(i, "OnAccountID").ToString());
                DB[DB.Length - 1].AddStringField("PaymentMethode", gridView1.GetRowCellValue(i, "PaymentMethode").ToString());
                DB[DB.Length - 1].AddNumericField("ReceiptVoucherID", "1");
                
                DB[DB.Length - 1].AddNumericField("Year", DateTime.Now.Year.ToString());
                DB[DB.Length - 1].AddNumericField("UserID", UserInfo.ID.ToString());
                DB[DB.Length - 1].AddNumericField("RegDate", Lip.GetServerDateSerial());
                DB[DB.Length - 1].AddNumericField("RegTime", Lip.GetServerTimeSerial());
                DB[DB.Length - 1].AddStringField("ComputerInfo", UserInfo.ComputerInfo);
                DB[DB.Length - 1].AddNumericField("Cancel","0");

                DB[DB.Length - 1].AddNumericField("ChequeNo", "0");
                DB[DB.Length - 1].AddNumericField("SalarySpendVoucherID", "0");
                DB[DB.Length - 1].AddNumericField("EntitlementVoucherID", "0");
                DB[DB.Length - 1].AddNumericField("BankAccountNo", "0");
                DB[DB.Length - 1].AddNumericField("ChequeDate", "0");
                DB[DB.Length - 1].AddStringField("Department", "1");
                DB[DB.Length - 1].StoreInsert();
            }
            bool v =   Lip.ExecuteTransaction(DB);

            if(v)
            {
                Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
            }

            else
            {
                Messages.MsgError(Messages.TitleError, Messages.msgErrorSave );

            }
        }



        private void lnkPrintTotal_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DoPrint();
        }

        private void cmbAbsentBy_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                int totalAbsent = 0;
                if (cmbAbsencesAccordingTo.EditValue.ToString() == "1")
                {
                    dgvColAbsenceDays.Caption = "ساعات الغياب"; // Absence Hours in Arabic
                    dgvColAbsenceDays.Tag = "Absence Hours";
                }
                else
                {
                    dgvColAbsenceDays.Caption = "ايـام الغياب"; // Absence Days in Arabic
                    dgvColAbsenceDays.Tag = "Absence Days";
                }

                for (int i = 0; i < gridView1.RowCount; i++)
                {
                     totalAbsent =  Comon.cInt(gridView1.GetRowCellValue(i, "AbsenceDays"));
                    if (cmbAbsencesAccordingTo.EditValue.ToString() == "2")
                    {
                        totalAbsent = totalAbsent / 60 / 8;
                    }
                    else
                    {
                        totalAbsent = totalAbsent / 60;
                    }

                    gridView1.SetRowCellValue(i, dgvColAbsenceDays.Name, totalAbsent);
                }

            }
            catch (Exception ex)
            {
                
            }
        }

        private void lnkPrintSingle_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i <= gridView1.RowCount-1; i++)
            {
               int FormAdd = Comon.cInt(Comon.cbool(gridView1.GetRowCellValue(i, "ColFormAdd")) == true ? 1 : 0);
                if (FormAdd == 1)
                {
                    long EmpID = Comon.cLong(gridView1.GetRowCellValue(i, "EmployeeID").ToString());
                    PrintEmpStatment(EmpID,i);
                }
            }
            
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            BindingList<Acc_VariousVoucherDetails> lstDetail = new BindingList<Acc_VariousVoucherDetails>();
            frmVariousVoucher frm = new frmVariousVoucher();
            //if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            //{
            if (UserInfo.Language == iLanguage.English)
                ChangeLanguage.EnglishLanguage(frm);
            frm.FormView = true;
            frm.FormAdd = true;
            //frm.FormUpdate = true;

            frm.Show();
            frm.ClearFields();
            frm.NewReord();
            frm.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue.ToString());
            //}
            //else return;
            lstDetail = new BindingList<Acc_VariousVoucherDetails>();
            lstDetail.AllowNew = true;
            lstDetail.AllowEdit = true;
            lstDetail.AllowRemove = true;
            frm.gridControl.DataSource = lstDetail;
            decimal[] arrayName = new decimal[20];
            Acc_VariousVoucherDetails Detail = new Acc_VariousVoucherDetails();
            for (int k = 0; k <= gridView1.RowCount - 1; k++)
            {
                Detail = new Acc_VariousVoucherDetails();
                long empid = Comon.cLong(gridView1.GetRowCellValue(k, "EmployeeID"));
                long OnAccountID = Comon.cLong(gridView1.GetRowCellValue(k, "OnAccountID"));
                double Debit = Comon.cDbl(gridView1.GetRowCellValue(k, "TotalDeductions"));
                Detail.AccountID = OnAccountID;
                Detail.Credit = 0;
                Detail.Debit  = Debit;
                Detail.Declaration =  " قيد استقطاع عن شهر "+ cmbMonths.Text ;
                Detail.ArbAccountName = gridView1.GetRowCellValue(k, "EmployeeName").ToString();
                Detail.EngAccountName = gridView1.GetRowCellValue(k, "EmployeeName").ToString();
                arrayName[0] = arrayName[0] + Comon.cDec(gridView1.GetRowCellValue(k, "Salary"));
                arrayName[1] = arrayName[1] + Comon.cDec(gridView1.GetRowCellValue(k, "Allowance1"));
                arrayName[2] = arrayName[2] + Comon.cDec(gridView1.GetRowCellValue(k, "Allowance2"));
                arrayName[3] = arrayName[3] + Comon.cDec(gridView1.GetRowCellValue(k, "Allowance3"));
                arrayName[4] = arrayName[4] + Comon.cDec(gridView1.GetRowCellValue(k, "Allowance4"));
                arrayName[5] = arrayName[5] + Comon.cDec(gridView1.GetRowCellValue(k, "Allowance5"));
                arrayName[6] = arrayName[6] + Comon.cDec(gridView1.GetRowCellValue(k, "Allowance6"));
                arrayName[7] = arrayName[7] + Comon.cDec(gridView1.GetRowCellValue(k, "MonthlyAllowance"));
                arrayName[8] = arrayName[8] + Comon.cDec(gridView1.GetRowCellValue(k, "Deduction1"));
                arrayName[9] = arrayName[9] + Comon.cDec(gridView1.GetRowCellValue(k, "Deduction2"));
                arrayName[10] = arrayName[10] + Comon.cDec(gridView1.GetRowCellValue(k, "Deduction3"));
                arrayName[11] = arrayName[11] + Comon.cDec(gridView1.GetRowCellValue(k, "Deduction4"));
                arrayName[12] = arrayName[12] + Comon.cDec(gridView1.GetRowCellValue(k, "Deduction5"));
                arrayName[13] = arrayName[13] + Comon.cDec(gridView1.GetRowCellValue(k, "Deduction6"));
                arrayName[14] = arrayName[14] + Comon.cDec(gridView1.GetRowCellValue(k, "TotalAllowance"));
                arrayName[15] = arrayName[15] + Comon.cDec(gridView1.GetRowCellValue(k, "TotalDeductions"));
                arrayName[16] = arrayName[16] + Comon.cDec(gridView1.GetRowCellValue(k, "SalariesNet"));
                if (Detail.Debit == 0)
                    continue;
                lstDetail.Add(Detail);
            }
            //GetEmpAlwanceTotal(ref Detail, ref arrayName, ref lstDetail);
            GetEmpDeductionIDTotal(ref Detail, ref arrayName, ref lstDetail);
            frm.gridControl.DataSource = lstDetail;
            frm.CalculatTotalBalance();
        }
    }
}

using DevExpress.XtraSplashScreen;
using Edex.Model;
using Edex.Model.Language;
using Edex.GeneralObjects.GeneralForms;
using Edex.ModelSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Edex.SalesAndPurchaseObjects.Transactions
{
    public partial class frmEditCloseCashierDate : Edex.GeneralObjects.GeneralForms.BaseForm
    {
        public frmEditCloseCashierDate()
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

            ///////////////////////////////////////////////////////
            this.txtCloseCashierDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtCloseCashierDate.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.txtCloseCashierDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtCloseCashierDate.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.txtCloseCashierDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtCloseCashierDate.Properties.Mask.EditMask = "dd/MM/yyyy";
          //  this.txtCloseCashierDate.EditValue = DateTime.Now;

        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            string strSQL;
            try { 
            
              if (string.IsNullOrEmpty(txtFromInvoiceNo.Text)|| string.IsNullOrEmpty(txtToInvoiceNo.Text)  ||  string.IsNullOrEmpty(txtCloseCashierDate.Text)   )
                {
                    Messages.MsgWarning(Messages.TitleWorning, (UserInfo.Language == iLanguage.Arabic ? " الرحاء ادخال جميع الحقول " : "You must Enter All Text Faild"));

                    return;
                }
                Application.DoEvents();
            //    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);

              
                DataTable dt = new DataTable();
    long CloeseDate = Comon.cLong(Comon.ConvertDateToSerial(txtCloseCashierDate.Text));

    strSQL = "Select * From Sales_SalesInvoiceMaster Where BranchID=" + MySession.GlobalBranchID + " And InvoiceID >=" + txtFromInvoiceNo.Text
        + " And InvoiceID<=" + txtToInvoiceNo.Text + " And ( CloseCashier=0 OR CloseCashierDate=0)";// And FromCashierScreen=1";
            dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0 ){
            Messages.MsgWarning(Messages.TitleWorning, (UserInfo.Language == iLanguage.Arabic ? "لا يمكن تعديل التاريخ لأن بعض الفواتير المحددة غير مغلقة أصلا":  "You Can Not Change The Date Because Some Selected Invoices Did not closed"));
               return;
               }
            strSQL = "Update Sales_SalesInvoiceMaster Set CloseCashierDate =" +CloeseDate+ " Where BranchID=" + MySession.GlobalBranchID
            + " And CloseCashier=1 AND CloseCashierDate<>0 And InvoiceID >= " + txtFromInvoiceNo.Text
                + " And InvoiceID<=" +txtToInvoiceNo.Text;
            Lip.ExecututeSQL(strSQL);
                Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);

            //    SplashScreenManager.CloseForm(false);
            
            }
            catch (Exception ex)
            {
               // SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            finally
            {
               // SplashScreenManager.CloseForm(false);



            }
        }
    }
}

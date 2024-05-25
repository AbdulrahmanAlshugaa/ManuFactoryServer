using Edex.DAL;
using Edex.Model;
using Edex.Model.Language;
using Edex.AccountsObjects.Reports;
using Edex.ModelSystem;
using Edex.StockObjects.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Edex.AccountsObjects.Transactions
{
    public partial class frmPrepareForNewAccountYear : Edex.GeneralObjects.GeneralForms.BaseForm
    {
        public frmPrepareForNewAccountYear()
        {
            InitializeComponent();
        }

        private void frmPrepareForNewAccountYear_Load(object sender, EventArgs e)
        {
            ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[3].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[4].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[5].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[7].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[8].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[9].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[10].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[11].Visible = false;
            GetDatabaseList();
        }

        private void btnStartPrepare_Click(object sender, EventArgs e)
        {
            frmFinancialPositionStatement frmFinancial = new frmFinancialPositionStatement();
            frmFinancial.Show();
            frmFinancial.TransFinance();

            if (frmFinancial._Sampl.Rows.Count < 1)
            {
                MessageBox.Show("لا يمكن ترحيل الميزانية لعدم اغلاق الارباح والخسائر");
                return;
            
            
            }
            //Do Back Up
            //.............................................................................................
            string strSQL = "SELECT Name FROM sys.databases Order By Name ASC";
            DataTable dt = new DataTable();
            dt = Lip.SelectRecord(strSQL);

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                if ((txtNewDatabaseName.Text.ToUpper()) == dt.Rows[i]["Name"].ToString().ToUpper())
                {
                    MessageBox.Show(UserInfo.Language == iLanguage.Arabic ? "هذا الاسم مستخدم من قبل ، يرجى إدخال اسم آخر" : "This Name Was Used Before");
                    return;
                }
            }

            strSQL = "SELECT Name FROM sys.master_files";
            dt = new DataTable();
            dt = Lip.SelectRecord(strSQL);
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                if ((txtNewDatabaseName.Text.ToUpper()) == dt.Rows[i]["Name"].ToString().ToUpper())
                {
                    MessageBox.Show(UserInfo.Language == iLanguage.Arabic ? "هذا الاسم مستخدم من قبل ، يرجى إدخال اسم آخر" : "This Name Was Used Before");
                    return;
                }
            }
            //.............................................................................................
            // delete all files backup.
            string f = MySession.PubCurrentDataBasePath;

            

            string[] ArrayFilles = Directory.GetFiles(f);
            foreach (string name in ArrayFilles)
            {
                Application.DoEvents();
                if (Directory.Exists(name))
                    Directory.Delete(name);
            }


            strSQL = "Backup Database " + cConnectionString.DataBasename + " To disk='" + MySession.defaultBackupPath + cConnectionString.DataBasename + ".bak'";
            Lip.ExecututeSQL(strSQL);
            Application.DoEvents();

            string path = MySession.defaultBackupPath;

            System.Threading.Thread.Sleep(2000);
            strSQL = "RESTORE DATABASE [" + txtNewDatabaseName.Text + "] FROM  DISK = N'" + MySession.defaultBackupPath + cConnectionString.DataBasename + ".bak' WITH  FILE = 1,  MOVE N'" + MySession.PubCurrentLogicalName + "' TO N'" + MySession.PubCurrentDataBasePath + txtNewDatabaseName.Text + ".mdf',  MOVE N'" + MySession.PubCurrentLogicalName + "_log' TO N'" + MySession.PubCurrentDataBasePath + txtNewDatabaseName.Text + "_log.ldf',  NOUNLOAD,  STATS = 10";
            Lip.ExecututeSQL(strSQL);
            Application.DoEvents();        



            frmStocktaking frm = new frmStocktaking();
            frm.Show();
            DataTable dtStore = new DataTable();
            string StrSQL = "Select * from Stc_Stores where Cancel=0 ";

            dtStore = Lip.SelectRecord(StrSQL);
            for (int i = 0; i <= dtStore.Rows.Count - 1; i++)
            {
                // get Stok.............................
                frm.txtStoreID.Text = dtStore.Rows[i]["StoreID"].ToString();
                cConnectionString.ServerName = ".";
                cConnectionString.DataBasename = MySession.PubDatabaseName;

                frm.GetStock();

                //.........حفظ بضاعة أول مدة.............
                Lip.ExecututeSQL("Delete From Stc_GoodOpeningDetails");
                Lip.ExecututeSQL("Delete From Stc_GoodOpeningMaster");

                cConnectionString.ServerName = cConnectionString.ServerName;
                cConnectionString.DataBasename = txtNewDatabaseName.Text;
                SqlConnection con = cConnectionString.GetConnectionSetting();
                if (frm.SaveGoodOpening() == true)

                    MessageBox.Show("تم حفظ بضاعة أول مدة بنجاح");
                else
                {
                    MessageBox.Show(" خطا في حفظ بضاعة اول مدة لم يتم اكمال التهيئة ");
                    return;
                }
               
                

                //.........................................................


               // Lip.ExecututeSQL("ex From Acc_CheckReceiptVoucherDetails");
                Lip.ExecuteProcedure("Dell_ALLTrans_SP");
                //Lip.ExecututeSQL("Delete From Acc_CheckSpendVoucherDetails");
                //Lip.ExecututeSQL("Delete From Acc_CheckSpendVoucherMaster");
                //Lip.ExecututeSQL("Delete From Acc_ReceiptVoucherDetails");
                //Lip.ExecututeSQL("Delete From Acc_ReceiptVoucherMaster");
                //Lip.ExecututeSQL("Delete From Acc_SpendVoucherDetails");
                //Lip.ExecututeSQL("Delete From Acc_SpendVoucherMaster");
                //Lip.ExecututeSQL("Delete From Acc_VariousVoucherDetails");
                //Lip.ExecututeSQL("Delete From Acc_VariousVoucherMaster");
                //Lip.ExecututeSQL("Delete From Sales_PurchaseInvoiceDetails Where InvoiceID>=0");
                //Lip.ExecututeSQL("Delete From Sales_PurchaseInvoiceMaster Where InvoiceID>=0");
                //Lip.ExecututeSQL("Delete From Sales_PurchaseInvoiceReturnDetails");
                //Lip.ExecututeSQL("Delete From Sales_PurchaseInvoiceReturnMaster");
                //Lip.ExecututeSQL("Delete From Sales_SalesInvoiceDetails");
                //Lip.ExecututeSQL("Delete From Sales_SalesInvoiceMaster");
                //Lip.ExecututeSQL("Delete From Sales_SalesInvoiceReturnDetails");
                //Lip.ExecututeSQL("Delete From Sales_SalesInvoiceReturnMaster");
                //Lip.ExecututeSQL("Delete From Stc_ItemsDismantlingDetails");
                //Lip.ExecututeSQL("Delete From Stc_ItemsDismantlingMaster");
                //Lip.ExecututeSQL("Delete From Stc_ItemsTransferDetails");
                //Lip.ExecututeSQL("Delete From Stc_ItemsTransferMaster");
                //Lip.ExecututeSQL("Delete From Manu_ManufacturingOperations_Details");
                //Lip.ExecututeSQL("Delete From Manu_ManufacturingOperations_Master");
                //Lip.ExecututeSQL("Delete From Manu_ManufacturingOperations_StuffDetails");
                //Lip.ExecututeSQL("Delete From Res_ItemsInsurance_Details");
                //Lip.ExecututeSQL("Delete From Res_ItemsInsurance_Master");
                //Lip.ExecututeSQL("Delete From Res_ItemsInsuranceReturn_Details");
                //Lip.ExecututeSQL("Delete From Res_ItemsInsuranceReturn_Master");
                //Lip.ExecututeSQL("Delete From Res_Orders_Details");
                //Lip.ExecututeSQL("Delete From Res_Orders_Master");
                //Lip.ExecututeSQL("Delete From Res_Parties_Details");
                //Lip.ExecututeSQL("Delete From Res_Parties_Master");
                //Lip.ExecututeSQL("Delete From Res_ReservedTables");
                //Lip.ExecututeSQL("Delete From Stc_ItemsOutonBail_Details");
                //Lip.ExecututeSQL("Delete From Stc_ItemsOutonBail_Master");

                frmFinancial.TransFinanceSave();
                frmFinancial.Dispose();

            }

        }


        void GetDatabaseList()
        {
            try
            {
                DataTable dtListDataBase = new DataTable();
           //     cConnectionString.ServerName = ".";
                cConnectionString.GetConnectionSetting();
                string ConString = ConfigurationManager.ConnectionStrings["SettingDBConnection"].ConnectionString;
                ConString = ConString.Replace("IPADDRESS", cConnectionString.ServerName);
                using (SqlConnection con = new SqlConnection(ConString))
                {

                    con.Open();
                    using (SqlCommand objCmd = con.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.Text;
                        objCmd.CommandText = "SELECT database_id as ID,name as Name from sys.databases ";
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        dtListDataBase.Load(myreader);
                        cmbDBName.Properties.DataSource = dtListDataBase;
                        cmbDBName.Properties.ValueMember = "ID";
                        cmbDBName.Properties.DisplayMember = "Name";

                    }
                }
                 
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void chboxSameYear_CheckedChanged(object sender, EventArgs e)
        {
            if (chboxSameYear.Checked == false)
            {
                cmbDBName.Visible = true;
                lblDatabaseName.Visible = true;
            }
            else {

                cmbDBName.Visible = false;
                lblDatabaseName.Visible = false;
            
            }
        }

        private void cmbDBName_EditValueChanged(object sender, EventArgs e)
        {
            txtNewDatabaseName.Text = cmbDBName.Text;

        }
    }
}

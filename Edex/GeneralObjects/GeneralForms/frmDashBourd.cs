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
using Edex.GeneralObjects.GeneralClasses;
using Edex.Model;
using Edex.ModelSystem;
using Edex.Model.Language;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;

namespace Edex.GeneralObjects.GeneralForms
{
    public partial class frmDashBourd : BaseForm
    {
        private DevExpress.XtraNavBar.NavBarItem[] navFavorit = new DevExpress.XtraNavBar.NavBarItem[30];
        public string PrimaryKeyName = "ArbName";

        public string strSQL = "";
        public frmDashBourd()
        {
            InitializeComponent();
            if (UserInfo.Language == iLanguage.English)
            {
                PrimaryKeyName = "EngName";
                this.Text = this.Tag.ToString();
                WorningGroup.Caption = "List of alerts";
            }
            FillCombo.FillComboBox(cmbCurency, "Acc_CodeCurrncyPrice", "CodeCurncy", PrimaryKeyName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
               
            GetListTaskToCurrentUser();
        }
        public void GetListTaskToCurrentUser()
        {



            navFavorit[0] = new DevExpress.XtraNavBar.NavBarItem();
            navFavorit[0].Caption = "1- لم يتم اإنشاء نسخة احتياطيه منذو اسبوع";
            navFavorit[0].ImageOptions.LargeImageIndex = 72;
            navFavorit[0].ImageOptions.SmallImageIndex = 72;
            navFavorit[0].Name = "navfrmFavoritItems";
            navFavorit[0].Tag = "StockObjects.Codes.frmFavoritItems";
            //navFavorit[0].LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.OpenFavoritForm);
            WorningGroup.ItemLinks.Add(navFavorit[0]);



            navFavorit[1] = new DevExpress.XtraNavBar.NavBarItem();
            navFavorit[1].Caption = "2-هناك تنبيه من إدارة الموارد البشرية بتغير كلمة المرور";
            navFavorit[1].ImageOptions.LargeImageIndex = 72;
            navFavorit[1].ImageOptions.SmallImageIndex = 72;
            navFavorit[1].Name = "navfrmFavoritItems1";
            navFavorit[1].Tag = "StockObjects.Codes.frmFavoritItems1";
            //navFavorit[1].LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.OpenFavoritForm);
            WorningGroup.ItemLinks.Add(navFavorit[1]);

            WorningGroup.ItemLinks.Add(navFavorit[1]);
            WorningGroup.ItemLinks.Add(navFavorit[1]);
            WorningGroup.ItemLinks.Add(navFavorit[1]);
            WorningGroup.ItemLinks.Add(navFavorit[1]);
            WorningGroup.ItemLinks.Add(navFavorit[1]);
            WorningGroup.ItemLinks.Add(navFavorit[1]);
            WorningGroup.ItemLinks.Add(navFavorit[1]);
            WorningGroup.ItemLinks.Add(navFavorit[1]);
            WorningGroup.ItemLinks.Add(navFavorit[1]);
            WorningGroup.ItemLinks.Add(navFavorit[1]);
            WorningGroup.ItemLinks.Add(navFavorit[1]);
        }
        private void frmDashBourd_Load(object sender, EventArgs e)
        {

            try
            {
                strSQL = "Select " + PrimaryKeyName + " AS Argument, ItemID AS Value from STC_ITEMS where BranchID="+MySession.GlobalBranchID;
                DataTable dt = new DataTable();
                dt = Lip.SelectRecord(strSQL);
                //عدد السجلات + الداتات + العنصر الذي سيتم عمل فيه التشارت في النموذج
                if (dt.Rows.Count > 0)
                    ChartLip.Chart(10, dt, pnlCharts);
                this.CancelButton = null;
                this.ControlBox = false;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            }
            catch 
            { 
            }

        }
        public static async Task Main1(string CurrncyCode)
        {
            string apiKey = "goldapi-7qsi91sltw42cf2-io";
            //string apiKey = "goldapi-1hvdo4sltwgqtfh-io";
            string symbol = "XAU";
            string curr = CurrncyCode;
            string date = "";

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("x-access-token", apiKey);

                string url = "https://www.goldapi.io/api/" + symbol + "/" + curr + date;
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    dynamic data = JsonConvert.DeserializeObject(responseBody);
                    MySession.GlobalDefaultPricePerGram24k = data.price_gram_24k;
                    MySession.GlobalDefaultPricePerGram21k = data.price_gram_21k;
                    MySession.GlobalDefaultPricePerGram16k = data.price_gram_16k;
                    MySession.GlobalDefaultPricePerGram18k = data.price_gram_18k;
                    MySession.GlobalDefaultPricePerGram22k = data.price_gram_22k;
                    MySession.GlobalDefaultPricePerGram14k = data.price_gram_14k;
                }
                catch (Exception ex)
                {
                    Messages.MsgWarning(Messages.TitleWorning, "Error: " + ex.Message);
                }
            }
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (cmbCurency.EditValue != null)
            {
                string CurrncyCode = cmbCurency.EditValue.ToString();
                Main1(CurrncyCode);
                try
                {
                    if (CurrncyCode == "USD")
                        CurrncyCode = "$";
                    lblCaliber14.Text = (UserInfo.Language == iLanguage.English ? CurrncyCode + MySession.GlobalDefaultPricePerGram14k.ToString() : CurrncyCode + " " + MySession.GlobalDefaultPricePerGram14k.ToString());
                    lblCaliber16.Text = (UserInfo.Language == iLanguage.English ? CurrncyCode + MySession.GlobalDefaultPricePerGram16k.ToString() : CurrncyCode + " " + MySession.GlobalDefaultPricePerGram16k.ToString());
                    lblCaliber18.Text = (UserInfo.Language == iLanguage.English ? CurrncyCode + MySession.GlobalDefaultPricePerGram18k.ToString() : CurrncyCode + " " + MySession.GlobalDefaultPricePerGram18k.ToString());
                    lblCaliber21.Text = (UserInfo.Language == iLanguage.English ? CurrncyCode + MySession.GlobalDefaultPricePerGram21k.ToString() : CurrncyCode + " " + MySession.GlobalDefaultPricePerGram21k.ToString());
                    lblCaliber22.Text = (UserInfo.Language == iLanguage.English ? CurrncyCode + MySession.GlobalDefaultPricePerGram22k.ToString() : CurrncyCode + " " + MySession.GlobalDefaultPricePerGram22k.ToString());
                    lblCaliber24.Text = (UserInfo.Language == iLanguage.English ? CurrncyCode + MySession.GlobalDefaultPricePerGram24k.ToString() : CurrncyCode + " " + MySession.GlobalDefaultPricePerGram24k.ToString());
                }
                catch { }
            }
            else
            {
                Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "الرجاء تحديد العملة " : "Please Select The Currnacy");
                return;
            }
        }
    }
}
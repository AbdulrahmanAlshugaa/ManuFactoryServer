using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Edex.Model;
using Edex.Model.Language;

namespace Edex.RestaurantSystem.UserControls
{
    public partial class ucMainGroup : UserControl
    {

        public const int SizeItemGroupPage = 4;
        public int CountItemGroupPage;
        public int IndexItemGroupPage = 0;
        public int DiscountCustomer = 0;
        public int IndexOrderPage = 0;
        public DataTable dtGroups;
        public int CountOrderPage;
        public const int SizeOrderPage = 6;
        public string GroupSelect = "";
        string languagename = "ArbName";

        public Button_WOC[] ArrbtnItemGroups = new Button_WOC[SizeItemGroupPage];
        public ucMainGroup()
        {
            InitializeComponent();
            if (UserInfo.Language == iLanguage.English)
            {
                ArrbtnItemGroups[0] = btnMain1;
                ArrbtnItemGroups[1] = btnMain2;
                ArrbtnItemGroups[2] = btnMain3;
                ArrbtnItemGroups[3] = btnMain4;
                languagename = "EngName";

            }
            else
            {

                ArrbtnItemGroups[0] = btnMain4;
                ArrbtnItemGroups[1] = btnMain3;
                ArrbtnItemGroups[2] = btnMain2;
                ArrbtnItemGroups[3] = btnMain1;

            }
           
        }

        private void circularButton3_Click(object sender, EventArgs e)
        {

        }

        private void button_WOC1_Click(object sender, EventArgs e)
        {

        }

        private void ucMainGroup_Load(object sender, EventArgs e)
        {
            ButtonItemGroupEvent(ArrbtnItemGroups, SizeItemGroupPage);
            dtGroups = Lip.SelectRecord("SELECT  [ColorID] , " + languagename + " As ArbName, [EngName] FROM Stc_ItemsColors WHERE Cancel=0");
            CountItemGroupPage = getCountPage(dtGroups.Rows.Count, SizeItemGroupPage);
            gprevious_Click(null, null);
        //    btnItemGroup_Click(ArrbtnItemGroups[0], null);
        }
        #region Buttons
        void ButtonItemGroupEvent(Button_WOC[] arr, int SizePage)
        {
            for (int i = 0; i < SizePage; i++)
                arr[i].Click += new System.EventHandler(this.btnItemGroup_Click);
        }
        void resetColor(Button_WOC[] arr, int SizePage)
        {
            for (int i = 0; i < SizePage; i++)
            {
                arr[i].ButtonColor = Color.FromArgb(24, 42, 82);
               // arr[i].BackColor2 = Color.White;
               // arr[i].ForeColor = Color.FromArgb(65, 65, 65);
            }

        }
        private void btnItemGroup_Click(object sender, EventArgs e)
        {
            GroupSelect = "";
            IndexOrderPage = 0;
            resetColor(ArrbtnItemGroups, SizeItemGroupPage);
            string ItemGroupID = ((Button_WOC)sender).Name;
            //  SectionID = ItemGroupID;
            string ItemGroupArbName = ((Button_WOC)sender).Text;
            string ItemGroupEngName = ((Button_WOC)sender).Tag.ToString();
            ((Button_WOC)sender).ButtonColor = Color.Green;
          //  ((SimpleButton)sender).Appearance.BackColor2 = Color.Green;
            ((Button_WOC)sender).ForeColor = Color.White;
            #region filter Table By Section
            try
            {
                //var filter = ItemGroupID;
                //var sr = "Select GroupID," + languagename + " as ArbName from Stc_ItemsGroups where Cancel=0 and MainGroup=" + ItemGroupID;
                //dtOrder = Lip.SelectRecord(sr);
                //CountOrderPage = getCountPage(dtOrder.Rows.Count, SizeOrderPage);
                //oprevious_Click(null, null);
                //btnOrderGroup_Click(ArrbtnOrders[0], null);

            }
            catch
            {

            }



            #endregion


        }
        int getLength(int Count, int SizePage, int indexPage)
        {
            int remain = Count - (SizePage * indexPage);
            if (remain > SizePage)
                return SizePage;
            else
                return remain;
        }
        void HideButton(Button_WOC[] arr, int SizePage)
        {
            for (int i = 0; i < SizePage; i++)
            {
                arr[i].Visible = false;
                arr[i].Name = "";
                arr[i].Text = "";
               // arr[i].Tag = "";
            }
        }
        private void gprevious_Click(object sender, EventArgs e)
        {
            try
            {
                HideButton(ArrbtnItemGroups, SizeItemGroupPage);
                IndexItemGroupPage = getPreviousIndexPage(IndexItemGroupPage);
                int length = getLength(dtGroups.Rows.Count, SizeItemGroupPage, IndexItemGroupPage);
                for (int i = 0; i < length; i++)
                {
                    ArrbtnItemGroups[i].Name = dtGroups.Rows[(SizeItemGroupPage * IndexItemGroupPage) + i]["ColorID"].ToString();
                   // ArrbtnItemGroups[i].Tag = dtGroups.Rows[(SizeItemGroupPage * IndexItemGroupPage) + i]["EngName"].ToString();
                    ArrbtnItemGroups[i].Text = dtGroups.Rows[(SizeItemGroupPage * IndexItemGroupPage) + i]["ArbName"].ToString();
                    ArrbtnItemGroups[i].Visible = true;
                }
            }
            catch { }
        }
        private void gnext_Click(object sender, EventArgs e)
        {
            try
            {
                HideButton(ArrbtnItemGroups, SizeItemGroupPage);
                IndexItemGroupPage = getNextIndexPage(CountItemGroupPage, IndexItemGroupPage);
                int length = getLength(dtGroups.Rows.Count, SizeItemGroupPage, IndexItemGroupPage);
                for (int i = 0; i < length; i++)
                {
                    ArrbtnItemGroups[i].Name = dtGroups.Rows[(SizeItemGroupPage * IndexItemGroupPage) + i]["ColorID"].ToString();
                 //   ArrbtnItemGroups[i].Tag = dtGroups.Rows[(SizeItemGroupPage * IndexItemGroupPage) + i]["EngName"].ToString();
                    ArrbtnItemGroups[i].Text = dtGroups.Rows[(SizeItemGroupPage * IndexItemGroupPage) + i]["ArbName"].ToString();
                    ArrbtnItemGroups[i].Visible = true;
                }
            }
            catch { }

        }
        int getPreviousIndexPage(int IndexPage)
        {
            IndexPage = IndexPage - 1;
            if (IndexPage < 1)
                IndexPage = 0;
            return IndexPage;
        }
        int getNextIndexPage(int CountPage, int IndexPage)
        {
            IndexPage = IndexPage + 1;
            if (IndexPage >= CountPage)
                IndexPage = CountPage;
            return IndexPage;
        }
        int getCountPage(int Count, int SizePage)
        {
            return Count / SizePage;
        }

        #endregion

        private void btnMain4_Click(object sender, EventArgs e)
        {

        }

        private void ucMainGroup_Resize(object sender, EventArgs e)
        {
            foreach (Control ctrl in this.Controls)
            {

                string[] s = ctrl.Tag.ToString().Split(':');
                if (s.Length > 1)
                {
                    ctrl.Height = (int)(Comon.cDbl(this.Height) * Comon.cDbl(s[3]));
                    ctrl.Top = (int)(Comon.cDbl(this.Height) * Comon.cDbl(s[2]));
                    ctrl.Left = (int)(Comon.cDbl(this.Width) * Comon.cDbl(s[0]));

                    if (!(ctrl is CircularButton))
                    {
                        ctrl.Width = (int)(Comon.cDbl(this.Width) * Comon.cDbl(s[1]));
                    }

                }




            }
        }



    }
}

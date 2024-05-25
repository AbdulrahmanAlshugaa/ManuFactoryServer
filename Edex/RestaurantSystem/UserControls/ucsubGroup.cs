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
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraEditors;

namespace Edex.RestaurantSystem.UserControls
{
    public partial class ucsubGroup : UserControl
    {

        public const int SizeItemGroupPage =4;
        public int CountItemGroupPage;
        public int IndexItemGroupPage = 0;
        public int DiscountCustomer = 0;
        public int IndexOrderPage = 0;
        public DataTable dtGroups;
        public int CountOrderPage;
        public const int SizeOrderPage = 6;
        public string GroupSelect = "";
        public Button_WOC[] ArrbtnItemGroups = new Button_WOC[SizeItemGroupPage];
        public ucsubGroup()
        {
            InitializeComponent();

            if (UserInfo.Language == iLanguage.English)
            {
                ArrbtnItemGroups[0] = btnMain1;
                ArrbtnItemGroups[1] = btnMain2;
                ArrbtnItemGroups[2] = btnMain3;
                ArrbtnItemGroups[3] = btnMain4;
              
            }
            else
            {

                ArrbtnItemGroups[0] = btnMain4;
                ArrbtnItemGroups[1] = btnMain3;
                ArrbtnItemGroups[2] = btnMain2;
                ArrbtnItemGroups[3] = btnMain1;
             

            }
        }
       
        private void simpleButton20_Click(object sender, EventArgs e)
        {

        }

        private void panelControl2_Paint(object sender, PaintEventArgs e)
        {

        }
        #region Buttons
        void ButtonItemGroupEvent(Button_WOC[] arr, int SizePage)
        {
            for (int i = 0; i < SizePage; i++)
                arr[i].Click += new System.EventHandler(this.btnItemGroup_Click);
        }
        public void resetColor(Button_WOC[] arr, int SizePage)
        {
            for (int i = 0; i < SizePage; i++)
            {
                arr[i].ButtonColor = Color.FromArgb(24, 66, 82);
                // arr[i].BackColor2 = Color.White;
                // arr[i].ForeColor = Color.FromArgb(65, 65, 65);
            }

        }
        public void btnItemGroup_Click(object sender, EventArgs e)
        {
            GroupSelect = "";
            IndexOrderPage = 0;
            resetColor(ArrbtnItemGroups, SizeItemGroupPage);
            string ItemGroupID = ((Button_WOC)sender).Name;
            //  SectionID = ItemGroupID;
            string ItemGroupArbName = ((Button_WOC)sender).Text;
           // string ItemGroupEngName = ((Button_WOC)sender).Tag.ToString();
          //  ((Button_WOC)sender).ButtonColor = Color.FromArgb(247,41,105);
            ((Button_WOC)sender).ButtonColor = Color.Green;
            //  ((SimpleButton)sender).Appearance.BackColor2 = Color.Green;
            ((Button_WOC)sender).ForeColor = Color.White;
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
        public int getLength(int Count, int SizePage, int indexPage)
        {
            int remain = Count - (SizePage * indexPage);
            if (remain > SizePage)
                return SizePage;
            else
            {
                return remain;

            }
        }
        public void HideButton(Button_WOC[] arr, int SizePage)
        {
            for (int i = 0; i < SizePage; i++)
            {
                arr[i].Visible = false;
                arr[i].Name = "";
                arr[i].Text = "";
             //   arr[i].Tag = "";
            }
        }
        public void gprevious_Click(object sender, EventArgs e)
        {
            try
            {
                HideButton(ArrbtnItemGroups, SizeItemGroupPage);
                IndexItemGroupPage = getPreviousIndexPage(IndexItemGroupPage);
                int length = getLength(dtGroups.Rows.Count, SizeItemGroupPage, IndexItemGroupPage);
                for (int i = 0; i < length; i++)
                {
                    ArrbtnItemGroups[i].Name = dtGroups.Rows[(SizeItemGroupPage * IndexItemGroupPage) + i]["ColorID"].ToString();
                  //  ArrbtnItemGroups[i].Tag = dtGroups.Rows[(SizeItemGroupPage * IndexItemGroupPage) + i]["ArbName"].ToString();
                    ArrbtnItemGroups[i].Text = dtGroups.Rows[(SizeItemGroupPage * IndexItemGroupPage) + i]["ArbName"].ToString();
                    ArrbtnItemGroups[i].Visible = true;
                }
            }
            catch { }
        }
        public void gnext_Click(object sender, EventArgs e)
        {
            try
            {
                HideButton(ArrbtnItemGroups, SizeItemGroupPage);
                IndexItemGroupPage = getNextIndexPage(CountItemGroupPage, IndexItemGroupPage);
                int length = getLength(dtGroups.Rows.Count, SizeItemGroupPage, IndexItemGroupPage);
                for (int i = 0; i < length; i++)
                {
                    ArrbtnItemGroups[i].Name = dtGroups.Rows[(SizeItemGroupPage * IndexItemGroupPage) + i]["ColorID"].ToString();
                   // ArrbtnItemGroups[i].Tag = dtGroups.Rows[(SizeItemGroupPage * IndexItemGroupPage) + i]["ArbName"].ToString();
                    ArrbtnItemGroups[i].Text = dtGroups.Rows[(SizeItemGroupPage * IndexItemGroupPage) + i]["ArbName"].ToString();
                    ArrbtnItemGroups[i].Visible = true;
                }
            }
            catch { }

        }
        public int getPreviousIndexPage(int IndexPage)
        {
            IndexPage = IndexPage - 1;
            if (IndexPage < 1)
                IndexPage = 0;
            return IndexPage;
        }
        public int getNextIndexPage(int CountPage, int IndexPage)
        {
            IndexPage = IndexPage + 1;
            if (IndexPage >= CountPage)
                IndexPage = CountPage;
            return IndexPage;
        }
        public int getCountPage(int Count, int SizePage)
        {
            return Count / SizePage;
        }

        #endregion

        private void ucsubGroup_Load(object sender, EventArgs e)
        {
            ButtonItemGroupEvent(ArrbtnItemGroups, SizeItemGroupPage);

        }

        private void btnMain4_Click(object sender, EventArgs e)
        {

        }

        private void btnMain1_Click(object sender, EventArgs e)
        {

        }

        private void ucsubGroup_Resize(object sender, EventArgs e)
        {
            //foreach (Control ctrl in this.Controls)
            //{

            //    string[] s = ctrl.Tag.ToString().Split(':');
            //    if (s.Length > 1)
            //    {
            //        ctrl.Height = (int)(Comon.cDbl(this.Height) * Comon.cDbl(s[3]));
            //        ctrl.Top = (int)(Comon.cDbl(this.Height) * Comon.cDbl(s[2]));
            //        ctrl.Left = (int)(Comon.cDbl(this.Width) * Comon.cDbl(s[0]));

            //        if (!(ctrl is CircularButton))
            //        {
            //            ctrl.Width = (int)(Comon.cDbl(this.Width) * Comon.cDbl(s[1]));
            //        }

            //    }




            //}
        }

        private void panelControl2_Resize(object sender, EventArgs e)
        {

            PanelControl obj = (PanelControl)sender as PanelControl;
            foreach (Control ctrl in obj.Controls)
            {

                string[] s = ctrl.Tag.ToString().Split(':');
                if (s.Length > 1)
                {
                    ctrl.Height = (int)(Comon.cDbl(obj.Height) * Comon.cDbl(s[3]));
                    ctrl.Top = (int)(Comon.cDbl(obj.Height) * Comon.cDbl(s[2]));
                    ctrl.Left = (int)(Comon.cDbl(obj.Width) * Comon.cDbl(s[0]));

                    if (!(ctrl is CircularButton))
                    {
                        ctrl.Width = (int)(Comon.cDbl(obj.Width) * Comon.cDbl(s[1]));
                    }

                }




            }
        }

        private void panelControl3_Resize(object sender, EventArgs e)
        {
            PanelControl obj = (PanelControl)sender as PanelControl;
            foreach (Control ctrl in obj.Controls)
            {

                string[] s = ctrl.Tag.ToString().Split(':');
                if (s.Length > 1)
                {
                    ctrl.Height = (int)(Comon.cDbl(obj.Height) * Comon.cDbl(s[3]));
                    ctrl.Top = (int)(Comon.cDbl(obj.Height) * Comon.cDbl(s[2]));
                    ctrl.Left = (int)(Comon.cDbl(obj.Width) * Comon.cDbl(s[0]));

                    if (!(ctrl is CircularButton))
                    {
                        ctrl.Width = (int)(Comon.cDbl(obj.Width) * Comon.cDbl(s[1]));
                    }

                }




            }
        }






    }
}

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
using Edex.Model.Language;
using Edex.Model;
using Edex.ModelSystem;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraBars;

namespace Edex.GeneralObjects.GeneralForms
{
    public partial class frmReportViewer : DevExpress.XtraEditors.XtraForm
    {
        public frmReportViewer()
        {
            InitializeComponent();
            Language();
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
        }
        void Language()
        {
            if (UserInfo.Language == iLanguage.English)
            {
                ChangeLanguage.EnglishLanguage(this);
            }

            for (int i = 0; i < ribbonControl1.Pages.Count; i++)
            {

                if (UserInfo.Language == iLanguage.English)
                {
                    ChangeLanguage.LTR(ribbonControl1.Pages[i]);
                }

                foreach (RibbonPageGroup group in ribbonControl1.Pages[i].Groups)
                {
                    if (UserInfo.Language == iLanguage.English)
                    {
                        ChangeLanguage.LTR(group);
                    }
                   


                        foreach (BarItemLink link in group.ItemLinks)
                        {

                            if (UserInfo.Language == iLanguage.English)
                            {
                                ChangeLanguage.LTR((BarButtonItem)link.Item);

                            }


                        }
                   
                }

            }
        }
    }
}
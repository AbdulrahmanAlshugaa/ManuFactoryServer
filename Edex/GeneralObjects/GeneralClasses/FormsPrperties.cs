using Edex.GeneralObjects.GeneralForms;
using DevExpress.XtraEditors.Repository;
using Edex.Model;
using Edex.Model.Language;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using DevExpress.Utils;
using DevExpress.XtraEditors;
using System.Drawing;

namespace Edex.GeneralObjects.GeneralClasses
{
    public static class FormsPrperties
    {
        public static void EnabledControl(bool Value, Control ControlItem)
        {
            foreach (Control item in ControlItem.Controls)
            {
                foreach (Control itemForm in item.Controls)
                {
                    if (itemForm is System.Windows.Forms.TabPage)
                    {
                        foreach (Control itemControl in itemForm.Controls)
                        {
                            if (itemControl is System.Windows.Forms.Panel)
                            {
                                System.Windows.Forms.Panel pnl = (System.Windows.Forms.Panel)itemControl;
                                foreach (Control itemControl1 in pnl.Controls)
                                {
                                    if (itemControl1 is TextEdit || itemControl1 is LookUpEdit || itemControl1 is CheckEdit)
                                        itemControl.Enabled = Value;
                                }
                            }
                        }

                    }

                }
            }
        }
        public static void ColorFormWithTabPageAndPanel(Control ControlItem)
        {
            foreach (Control item in ControlItem.Controls)
            {
                foreach (Control itemForm in item.Controls)
                {
                    if (itemForm is System.Windows.Forms.TabPage)
                    {
                        foreach (Control itemControl in itemForm.Controls)
                        {
                            if (itemControl is System.Windows.Forms.Panel)
                            {
                                System.Windows.Forms.Panel pnl = (System.Windows.Forms.Panel)itemControl;
                                foreach (Control itemControl1 in pnl.Controls)
                                {
                                    if (itemControl1.Tag == null) continue;
                                    if (itemControl1.Tag.ToString().Length < 14) continue;
                                    if (itemControl1.Tag.ToString() == "") continue;


                                    if (itemControl1 is TextEdit || itemControl1 is LookUpEdit)
                                        itemControl1.BackColor = Color.FromArgb(255, 255, 220);
                                }
                            }
                        }
                    }

                }
            }

        }
        public static void ColorFormWithPanel(Form ControlItem)
        {
            foreach (Control item in ControlItem.Controls)
            {

                System.Windows.Forms.Panel pnl = (System.Windows.Forms.Panel)item;
                foreach (Control itemControl1 in pnl.Controls)
                {
                    if (itemControl1.Tag == null) continue;
                    if (itemControl1.Tag.ToString().Length < 14) continue;
                    if (itemControl1.Tag.ToString() == "") continue;

                    if (itemControl1 is TextEdit || itemControl1 is LookUpEdit)
                        itemControl1.BackColor = Color.FromArgb(255, 255, 200);

                }


            }

        }
        public static void EnabledControlFormWithPanel(Form ControlItem,bool Value)
        {
            foreach (Control item in ControlItem.Controls)
            {
                if (item is System.Windows.Forms.Panel)
                {
                    System.Windows.Forms.Panel pnl = (System.Windows.Forms.Panel)item;
                    foreach (Control itemControl1 in pnl.Controls)
                    {
                        if (itemControl1 is TextEdit || itemControl1 is LookUpEdit || itemControl1 is CheckEdit)
                            itemControl1.Enabled = Value;

                    }
                }

            }

        }
        public static void ColorFormWithTabPage(Form ControlItem)
        {
            foreach (Control item in ControlItem.Controls)
            {
                foreach (Control itemForm in item.Controls)
                {
                    if (itemForm.Tag == null) continue;
                    if (itemForm.Tag.ToString().Length < 14) continue;
                    if (itemForm.Tag.ToString() == "") continue;

                    if (itemForm is TextEdit || itemForm is LookUpEdit)
                        itemForm.BackColor = Color.FromArgb(255, 255, 220);



                }
            }

        }
        public static void ColorForm(Control ControlItem)
        {
            foreach (Control item in ControlItem.Controls)
            {
                string t = item.Name;
                if (item.Tag == null) continue;
                if (item.Tag.ToString().Length < 14) continue;
                if (item.Tag.ToString() == "") continue;
                if (item is TextEdit || item is LookUpEdit)
                    if (item.Tag.ToString().Substring(0, 14) == "ImportantField")
                        item.BackColor = Color.FromArgb(255, 255, 220);
            }
        }
        public static void PropertiesGridView(DevExpress.XtraGrid.Views.Grid.GridView Grd, string frmName)
        {
            Grd.Appearance.HeaderPanel.Font = new Font("Arial, 11.50pt, style=Bold", 8F, FontStyle.Bold);
            Grd.Appearance.OddRow.BackColor = Color.White;
            Grd.Appearance.EvenRow.BackColor = Color.LightSteelBlue;
            Grd.RowSeparatorHeight = 2;
            Grd.RowHeight = 25;
         //   Grd.Appearance.Row.Font = new Font("Arial, 11.25pt, style=Bold", 10F, FontStyle.Bold);
            string StrSQL = "Select * from ColorSettingGridVeiw Where FormName='" + frmName + "'";
            DataTable dt = new DataTable();
            dt = Lip.SelectRecord(StrSQL);
            if(dt.Rows.Count>0)
            {
                Grd.Appearance.OddRow.BackColor = Color.FromName(dt.Rows[0]["COLORODD"].ToString());
                Grd.Appearance.EvenRow.BackColor = Color.FromName(dt.Rows[0]["COLOREVEN"].ToString()); ;

            }
        }
    }

}

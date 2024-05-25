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
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Controls;
using DevExpress.XtraEditors.ViewInfo;
using Edex.Model;
using Edex.Model.Language;
using Edex.ModelSystem;

namespace Edex.GeneralObjects.GeneralForms
{
    public partial class frmSearch : DevExpress.XtraEditors.XtraForm
    {

        public frmSearch()
        {
            InitializeComponent();
            /***************************** Event For GridView *****************************/
            this.gridControl1.ProcessGridKey += new System.Windows.Forms.KeyEventHandler(this.gridControl_ProcessGridKey);
        }
        private string _colToSearch;
        private string _coldataType;
        private CSearch _cSearch;
        private DataTable _datatable;
        public DataTable dtDirectAccess;
        public int[] ColumnWidth;
        private int NameIndex;
        private System.Reflection.MethodBase currentMethod;
        private int _searchCol;
        public BaseForm frmFromForm;
        private bool IsDeActivate = true;
        public bool PubSearchMultiRows = false;
        void Language()
        {
            if (UserInfo.Language != iLanguage.Arabic)
            {
                
                foreach (Control childControl in this.Controls)
                {

                    ApplyLanguage(childControl);
                }

            }
        }
        void ApplyLanguage(Control control)
        {
            if (control is TextEdit || control is ComboBoxEdit)
                return;
            if (UserInfo.Language == iLanguage.English)
            {
                ChangeLanguage.LTR(control);
            }
            ApplyLanguageToElement(control);
        }

        void ApplyLanguageToElement(object element)
        {
             if (element is GroupBox)
            {
                GroupBox tabPage = (GroupBox)element;
                tabPage.RightToLeft = RightToLeft.No;

                foreach (Control childControl in tabPage.Controls)
                {
                    try
                    {

                        childControl.Location = new System.Drawing.Point(tabPage.Size.Width - childControl.Size.Width - childControl.Location.X, childControl.Location.Y);
                        if (childControl is TextEdit || childControl is ComboBoxEdit)
                            continue;
                    }
                    catch { }
                    ApplyLanguageToElement(childControl);
                }
            }
            else if (element is Panel)
            {
                Panel tabPage = (Panel)element;
                tabPage.RightToLeft = RightToLeft.No;
                if (UserInfo.Language == iLanguage.English)
                {
                    ChangeLanguage.LTR(tabPage);
                }
                foreach (Control childControl in tabPage.Controls)
                {

                    try
                    {
                        childControl.Location = new System.Drawing.Point(tabPage.Size.Width - childControl.Size.Width - childControl.Location.X, childControl.Location.Y);
                        if (childControl is TextEdit || childControl is ComboBoxEdit)
                            continue;
                    }
                    catch { }
                    ApplyLanguageToElement(childControl);
                }
            }
            else if (element is Control)
            {
                Control control = (Control)element;
            

                // Apply language logic for Control
                control.RightToLeft = RightToLeft.No;
                if (UserInfo.Language == iLanguage.English)
                {
                    ChangeLanguage.LTR(control);
                }

                foreach (Control childControl in control.Controls)
                {
                    if (childControl is TextEdit || childControl is ComboBoxEdit)
                        return;
                    ApplyLanguageToElement(childControl);
                }
            }
           

        }

        private void frmSearch_Load(object sender, EventArgs e)
        {
            Language();
            if (dtDirectAccess != null)
            {
                if (dtDirectAccess.Rows.Count > 0)
                    GridView.GridControl.DataSource = dtDirectAccess;
            }
            else
            {
                _cSearch.LoadData();
                _datatable = _cSearch.returnTable;

                GridView.GridControl.DataSource = _cSearch.returnTable;
            }
            SetFormAndGridWidth();
            GridView.FindFilterText = " ";
            


        }

        public void AddSearchData(CSearch p)
        {
            _cSearch = p;
        }

        private void GridView_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            var PrimaryKeyField = GridView.GetRowCellValue(e.RowHandle, _cSearch.strFilter);
            _cSearch.PrimaryKeyValue = PrimaryKeyField.ToString();
            if (_cSearch.strArbNameValue != null)
                if (_cSearch.strArbNameValue != string.Empty)
                    _cSearch.strArbNameValue = GridView.GetRowCellValue(e.RowHandle, _cSearch.strArbNameValue).ToString();

            if (_cSearch.PrimaryKeyName != "")
            {
                var PrimaryKeyNameValu = GridView.GetRowCellValue(e.RowHandle, _cSearch.PrimaryKeyField);
                if (PrimaryKeyNameValu != null)
                    _cSearch.PrimaryKeyName = PrimaryKeyNameValu.ToString();
            }
            IsDeActivate = false;
            if (frmFromForm != null)
                frmFromForm.CalGetSelectSearchList(_cSearch);
            IsDeActivate = true;
            if (PubSearchMultiRows == false)
                this.Close();
            //this.Close();
        }
        private void GridView_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            var PrimaryKeyField = GridView.GetRowCellValue(e.RowHandle, _cSearch.strFilter);
            if (_cSearch.PrimaryKeyName == "AccountID" || _cSearch.PrimaryKeyName == "ParentAccountID")
            {
                var PrimaryKeyNameValu = GridView.GetRowCellValue(e.RowHandle, _cSearch.strArbNameValue);
                _cSearch.PrimaryKeyName = PrimaryKeyNameValu.ToString();
            }
            else
            {

                if (_cSearch.PrimaryKeyName != "")
                {
                    var PrimaryKeyNameValu = GridView.GetRowCellValue(e.RowHandle, _cSearch.PrimaryKeyName);
                    if (PrimaryKeyNameValu != null)
                        _cSearch.PrimaryKeyName = PrimaryKeyNameValu.ToString();

                }
            }
            //this.Close();
        }

        private void GridView_Click(object sender, EventArgs e)
        {
            var PrimaryKeyField = GridView.GetRowCellValue(0, _cSearch.strFilter);
            _cSearch.PrimaryKeyValue = PrimaryKeyField.ToString();

            if (_cSearch.PrimaryKeyField == "AccountID")
            {
                var PrimaryKeyName = GridView.GetRowCellValue(0, "اسم الحسـاب");
                _cSearch.PrimaryKeyName = PrimaryKeyName.ToString();

            }
            //this.Close();
        }
        private void SetFormAndGridWidth()
        {
            int GridWidth = 0;
            int count = GridView.Columns.Count;
            if (GridView.Columns.Count >= 20)
            {
                count = 7;
            }
            for (int i = 0; i <= count - 1; i++)
            {
                GridWidth += ColumnWidth[i];
                GridView.Columns[i].Width = ColumnWidth[i];


            }
            this.Width = GridWidth + 10;
        }

        private void frmSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }
        private void gridControl_ProcessGridKey(object sender, KeyEventArgs e)
        {
            var grid = sender as GridControl;
            var view = grid.FocusedView as GridView;
            if (e.KeyData == Keys.Escape)
            {
                this.Close();
                //  grid.ShowPrintPreview();
            }
            if (e.KeyData == Keys.Enter)
            {
                var PrimaryKeyField = GridView.GetRowCellValue(view.FocusedRowHandle, _cSearch.strFilter);
                _cSearch.PrimaryKeyValue = PrimaryKeyField.ToString();

                if (_cSearch.PrimaryKeyName == "AccountID")
                {
                    var PrimaryKeyNameValu = GridView.GetRowCellValue(0, "اسم الحسـاب");
                    _cSearch.PrimaryKeyName = PrimaryKeyNameValu.ToString();

                }
                IsDeActivate = false;
                 
                IsDeActivate = true;
                if (PubSearchMultiRows == false)
                    this.Close();
            }
        }

        private void frmSearch_Shown(object sender, EventArgs e)
        {
            GridView.OptionsFind.AlwaysVisible = false;
            GridView.ShowFindPanel();
            GridView.OptionsFind.AlwaysVisible = true;
            if (GridView.IsFindPanelVisible)
            {
                //var FindControl foo = GridView.GridControl.Controls[0];  
                FindControl find = GridView.GridControl.Controls.Find("FindControl", true)[0] as FindControl;
                find.FindEdit.Focus();
            }
            else
                GridView.ShowFindPanel();
        }
        private void frmSearch_Deactivate(object sender, EventArgs e)
        {
            if (IsDeActivate == true)
                this.Close();
            else
                IsDeActivate = true;

        }

        private void frmSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();

            if (e.KeyCode == Keys.Enter)
                GridView_RowCellClick(null, null);
        }
    }
}
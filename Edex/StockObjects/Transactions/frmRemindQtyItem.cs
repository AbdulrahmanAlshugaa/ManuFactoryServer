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
using Edex.GeneralObjects.GeneralForms;
using Edex.Model;
using Edex.ModelSystem;
using DevExpress.XtraSplashScreen;
using Edex.Model.Language;
using Edex.GeneralObjects.GeneralClasses;
using DevExpress.Utils;

namespace Edex.StockObjects.Transactions
{
    public partial class frmRemindQtyItem : BaseForm
    {
        #region Declare 
        private string strSQL;
        private string PrimaryName;
        private string filter = "";
        DataTable dt = null;
        string FocusedControl;
        #endregion
        public frmRemindQtyItem()
        {
            InitializeComponent();
            try
            {
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
                ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Visible = true;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Caption = (UserInfo.Language == iLanguage.Arabic ? "استعلام جديد" : "New Query");
                                
                PrimaryName = "ArbName"; 
                if (UserInfo.Language == iLanguage.English)
                {                 
                    PrimaryName = "EngName";
                    dvgColAvrageCostPrice.Caption = "Avrage Cost Price";
                    dvgColSizeName.Caption = "SizeName";
                    dvgColRemindQTY.Caption = "Remaind QTY";
                    dvgColPacking.Caption = "Packing";

                }
                txtItemID.Validating += txtItemID_Validating;
                txtStoreID.Validating += txtStoreID_Validating;
                //this.gridView1.CustomDrawCell += gridView1_CustomDrawCell;
                
            }
            
            catch { }

        }
        void gridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            {
                e.Graphics.FillRectangle(e.Cache.GetSolidBrush(Color.Beige), e.Bounds);
                e.Graphics.DrawRectangle(e.Cache.GetPen(Color.Black, 1), e.Bounds);
                e.Appearance.DrawString(e.Cache, e.DisplayText, e.Bounds);
                e.Handled = true;
                gridView1.Appearance.Row.TextOptions.HAlignment = HorzAlignment.Center;
                gridView1.Appearance.Row.TextOptions.HAlignment = HorzAlignment.Center;

            }


        }

        void txtItemID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " AS ItemName FROM Stc_Items WHERE   (Cancel = 0) AND (ItemID = " + txtItemID.Text + ") and BranchID=" + MySession.GlobalBranchID;
                CSearch.ControlValidating(txtItemID, lblItemName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        
        private void txtStoreID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT "+PrimaryName+" AS AccountName FROM Acc_Accounts WHERE   (Cancel = 0) AND (AccountID = " + txtStoreID.Text + ") and BranchID="+MySession.GlobalBranchID;
                CSearch.ControlValidating(txtStoreID, lblStoreName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

        }
        protected override void DoAddFrom()
        {
            try
            {
                dt.Clear();
                gridControl.RefreshDataSource();
                btnShow.Visible = true;
                DoNew();

            }
            catch (Exception ex)
            {
                //WT.msgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        protected override void DoNew()
        {
            try
            {
                txtItemID.Text = "";
                txtItemID_Validating(null, null);
                txtStoreID.Text = "";
                txtStoreID_Validating(null, null);
                
                txtItemID.Focus();

            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        string GetIndexFocusedControl()
        {
            Control c = this.ActiveControl;
            if (c is DevExpress.XtraLayout.LayoutControl)
            {
                if (!(((DevExpress.XtraLayout.LayoutControl)ActiveControl).ActiveControl == null))
                {
                    c = ((DevExpress.XtraLayout.LayoutControl)ActiveControl).ActiveControl;
                }
            }
            if (c is DevExpress.XtraEditors.TextBoxMaskBox)
            {
                c = c.Parent;
            }

            if (c.Parent is DevExpress.XtraGrid.GridControl)
            {
                return c.Parent.Name;
            }

            return c.Name;
        }
        public bool GetSelectedSearchValue(CSearch cls)
        {
            if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
            {

                if (FocusedControl == txtStoreID.Name)
                {
                    txtStoreID.Text = cls.PrimaryKeyValue.ToString();
                    txtStoreID_Validating(null, null);
                }

                else if (FocusedControl ==txtItemID.Name)
                {
                    txtItemID.Text = cls.PrimaryKeyValue.ToString();
                    txtItemID_Validating(null, null);
                }
                return true;
            }
            return false;
        }
        public void SetValueToControl(string ItemID, string StoreID)
        {
            txtItemID.Text = ItemID.ToString();
            txtItemID_Validating(null, null);
            txtStoreID.Text = StoreID.ToString();
            txtStoreID_Validating(null, null);
            btnShow_Click(null, null);

        }
        public bool Find()
        {

            CSearch cls = new CSearch();
            int[] ColumnWidth = new int[] { 100, 300 };
            string SearchSql = "";
            string Condition = "Where 1=1";

            FocusedControl = GetIndexFocusedControl();
            if (FocusedControl.Trim() == txtStoreID.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtStoreID, lblStoreName, "StoreID", "رقم الحساب", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtStoreID, lblStoreName, "StoreID", "Account ID", MySession.GlobalBranchID);
            }
            
            if (FocusedControl.Trim() == txtItemID.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtItemID, null, "Items", "رقـم الـمــادة", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtItemID, null, "Items", "Item ID", MySession.GlobalBranchID);
            }          
            return GetSelectedSearchValue(cls);

        }
        protected override void DoPrint()
        {

            try
            {
                gridView1.ShowRibbonPrintPreview();

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

        }
        private void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                if (Comon.cLong(txtItemID.Text) > 0)
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                    dt = null;
                    Application.DoEvents();
                    filter = "";
                    if (txtItemID.Text != string.Empty)
                        filter += "  dbo.Stc_ItemUnits.ItemID =" + Comon.cInt(txtItemID.Text) + " and Stc_ItemUnits.BranchID=" + MySession.GlobalBranchID + " AND ";

                    filter = filter.Remove(filter.Length - 4, 4);
                    dt = Lip.SelectRecord(@"SELECT Stc_ItemUnits.SizeID,  Stc_SizingUnits." + PrimaryName + @" as SizeName,  Stc_ItemUnits.PackingQty as Packing, 0.000 RemindQTY,0.000 AvrageCostPrice  
                                           FROM  Stc_ItemUnits INNER JOIN
                                            Stc_SizingUnits ON  Stc_ItemUnits.SizeID =  Stc_SizingUnits.SizeID and  Stc_ItemUnits.BranchID =Stc_SizingUnits.BranchID  where " + filter);
                    if (dt.Rows.Count > 0)
                    {
                        dt.Columns["RemindQTY"].ReadOnly = false;
                        dt.Columns["AvrageCostPrice"].ReadOnly = false;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {

                            decimal RemindQtyItemByMinUnit = Lip.GetRemindQTY(Comon.cInt(txtItemID.Text), Comon.cInt(dt.Rows[i]["SizeID"]), Comon.cDbl(txtStoreID.Text));
                            dt.Rows[i]["RemindQTY"] = Comon.ConvertToDecimalPrice(RemindQtyItemByMinUnit);
                            dt.Rows[i]["AvrageCostPrice"] = Comon.cDec(Lip.AverageUnit(Comon.cInt(txtItemID.Text), Comon.cInt(dt.Rows[i]["SizeID"]), Comon.cDbl(txtStoreID.Text)));
                        }
                    }
                    else
                    {
                        SplashScreenManager.CloseForm(false);
                        Messages.MsgInfo(Messages.TitleInfo, MySession.GlobalLanguageName == iLanguage.Arabic ? "لايوجد بيانات لعرضها" : "There is no Data to show it");

                    }
                    gridControl.DataSource = dt;
                }
                else
                {
                    Messages.MsgInfo(Messages.TitleInfo, MySession.GlobalLanguageName == iLanguage.Arabic ? "الرجاء اختيار صنف" : "Please select a Item");
                    return;

                }

            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);

            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        private void frmRemindQtyItem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3 || e.KeyCode == Keys.F4)
                Find();
            if (e.KeyCode == Keys.F5)
                DoPrint();
        }    
    }
}
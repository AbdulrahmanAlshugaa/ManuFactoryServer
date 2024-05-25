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
using Edex.DAL.ManufacturingDAL;
using System.Globalization;
using DevExpress.XtraEditors.Repository;
using Edex.Model.Language;
using Edex.ModelSystem;
using Edex.GeneralObjects.GeneralClasses;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraReports.UI;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid;
using System.IO;

namespace Edex.Manufacturing.Codes
{
    public partial class frmOrderRunningReport :BaseForm
    {
        #region declare
        BindingList<Manu_CadWaxDiamondZirconAfforistStages> lstDetail = new BindingList<Manu_CadWaxDiamondZirconAfforistStages>();
        BindingList<Menu_FactoryRunCommandfactory> lstDetailfactory = new BindingList<Menu_FactoryRunCommandfactory>();
        BindingList<Menu_FactoryRunCommandfactory> lstDetailAfterfactory = new BindingList<Menu_FactoryRunCommandfactory>();
        BindingList<Menu_FactoryRunCommandPrentagAndPulishn> lstDetailAfterPrentage1 = new BindingList<Menu_FactoryRunCommandPrentagAndPulishn>();
        BindingList<Menu_FactoryRunCommandPrentagAndPulishn> lstDetailPrentage1 = new BindingList<Menu_FactoryRunCommandPrentagAndPulishn>();

        BindingList<Menu_FactoryRunCommandPrentagAndPulishn> lstDetailAfterPrentage2 = new BindingList<Menu_FactoryRunCommandPrentagAndPulishn>();
        BindingList<Menu_FactoryRunCommandPrentagAndPulishn> lstDetailPrentage2 = new BindingList<Menu_FactoryRunCommandPrentagAndPulishn>();


        BindingList<Menu_FactoryRunCommandSelver> lstDetailAdditional = new BindingList<Menu_FactoryRunCommandSelver>();
        BindingList<Menu_FactoryRunCommandSelver> lstDetailAfterAdditional = new BindingList<Menu_FactoryRunCommandSelver>();

        BindingList<Menu_FactoryRunCommandCompund> lstDetailCompund = new BindingList<Menu_FactoryRunCommandCompund>();
        BindingList<Menu_FactoryRunCommandCompund> lstDetailAfterCompund = new BindingList<Menu_FactoryRunCommandCompund>();
        BindingList<Menu_FactoryRunCommandTalmee> lstDetailTalmee1 = new BindingList<Menu_FactoryRunCommandTalmee>();
        BindingList<Menu_FactoryRunCommandTalmee> lstDetailAfterTalmee1 = new BindingList<Menu_FactoryRunCommandTalmee>();

        BindingList<Menu_FactoryRunCommandTalmee> lstDetailTalmee2 = new BindingList<Menu_FactoryRunCommandTalmee>();
        BindingList<Menu_FactoryRunCommandTalmee> lstDetailAfterTalmee2 = new BindingList<Menu_FactoryRunCommandTalmee>();

        BindingList<Menu_FactoryRunCommandTalmee> lstDetailTalmee3 = new BindingList<Menu_FactoryRunCommandTalmee>();
        BindingList<Menu_FactoryRunCommandTalmee> lstDetailAfterTalmee3 = new BindingList<Menu_FactoryRunCommandTalmee>();

        private bool IsNewRecord;
        private string strSQL;
        private string PrimaryName;
        string FocusedControl = "";
        private Manu_CadWaxFactoryDAL cClassCadWax;
        public const int xMoveFirst = 7;
        public const int xMovePrev = 8;
        public const int xMoveNext = 9;
        public const int xMoveLast = 10;
        private string ItemName;
        private string SizeName;
        private string CaptionItemName;
        public CultureInfo culture = new CultureInfo("en-US");
        public bool HasColumnErrors = false;
        private DataTable dt;
        #endregion
        public frmOrderRunningReport()
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
                ItemName = "ArbItemName";
                SizeName = "ArbSizeName";
                PrimaryName = "ArbName";
                CaptionItemName = "اسم الصنف";
                if (UserInfo.Language == iLanguage.English)
                {
                    ItemName = "EngItemName";
                    SizeName = "EngSizeName";
                    PrimaryName = "EngName";
                    CaptionItemName = "Item Name";
                }
                this.Load += frmOrderRunningReport_Load;
                this.txtOrderID.Validating += txtOrderID_Validating;
                this.txtCustomerID.Validating += txtCustomerID_Validating;
                this.txtGuidanceID.Validating += txtGuidanceID_Validating;
                this.txtDelegateID.Validating += txtDelegateID_Validating;
                FillCombo.FillComboBox(cmbTypeOrders, "Manu_TypeOrders", "ID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
                FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
                cmbBranchesID.EditValue =   MySession.GlobalBranchID;
                cmbBranchesID.ReadOnly = !MySession.GlobalAllowBranchModificationAllScreens;
               
                InitializeFormatDate(txtOrderDate);
                GridCad.CustomDrawCell += GridCad_CustomDrawCell;
                gridView12.CustomDrawCell += GridCad_CustomDrawCell;
                gridView9.CustomDrawCell += GridCad_CustomDrawCell;
                gridView14.CustomDrawCell += GridCad_CustomDrawCell;
                gridView18.CustomDrawCell += GridCad_CustomDrawCell;
                gridView20.CustomDrawCell += GridCad_CustomDrawCell;
                GridViewBeforAddition.CustomDrawCell += GridCad_CustomDrawCell;
            }
            catch { }
        }

    
        #region Event
        void GridCad_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName != "ShownInNext" && e.Column.FieldName != "HimLost")
            {
                e.Graphics.FillRectangle(e.Cache.GetSolidBrush(Color.Beige), e.Bounds);
                e.Graphics.DrawRectangle(e.Cache.GetPen(Color.Black, 1), e.Bounds);
                e.Appearance.DrawString(e.Cache, e.DisplayText, e.Bounds);
                e.Handled = true;
                ((GridView)sender).Appearance.Row.TextOptions.HAlignment = HorzAlignment.Center;
                ((GridView)sender).Appearance.Row.TextOptions.VAlignment = VertAlignment.Center;
            }

        }
        void frmOrderRunningReport_Load(object sender, EventArgs e)
        {
            try
            {
                initGridWax();
                initGridFactory();
                initGridAfterFactory();
                initGridBeforPrentage1();
                initGridAfterPrentage1();

                initGridBeforPrentage2();
                initGridAfterPrentage2();
                initGridBeforCompent();
                initGridAfterCompent();
                initGridBeforTalmee();
                initGridAfterTalmee();

                initGridBeforTalmee2();
                initGridAfterTalmee2();

                initGridBeforTalmee3();
                initGridAfterTalmee3();
                initGridBeforAdditional();
                initGridAfterAdditional();
            }
            catch { }
        }
        public static System.Drawing.Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
            return returnImage;
        }
        private void FileImage(string ImageCode)
        {
            strSQL = "Select ImageID,TheImage,ImageCode From  MNG_ARCHIVINGDOCUMENTSIMAGES where BranchID =" + Comon.cInt(cmbBranchesID.EditValue) + " And ImageCode ='" + ImageCode + "'   Order By ID";
            DataTable dt = Lip.SelectRecord(strSQL);
            picItemImage.Image = null;
            txtImageCode.Text = "";
            if (dt.Rows.Count > 0)
            {
                PictureBox pic = new PictureBox();
                Byte[] imgByte = new Byte[] { };
                imgByte = (Byte[])(dt.Rows[0]["TheImage"]);
                txtImageCode.Text = dt.Rows[0]["ImageCode"].ToString();

                pic.Image = byteArrayToImage(imgByte);
                pic.SizeMode = PictureBoxSizeMode.StretchImage;
                picItemImage.Image = pic.Image;
            }
            else
            {
                Messages.MsgError(this.GetType().Name, " لا يوجد صورة بهذا الكود");
            }
        }
        public void txtOrderID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                //if (FormView == true)
                {
                    if (String.IsNullOrEmpty(txtOrderID.Text) == false)
                    {
                        ReadTopInfo(txtOrderID.Text);
                        GetOrderDetail(txtOrderID.Text);
                        dt = Manu_OrderRestrictionDAL.frmGetDataDetalByID(txtOrderID.Text,Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID);
                        txtImageCode.Text = dt.Rows[0]["ImageCode"].ToString();
                        if (string.IsNullOrEmpty(txtImageCode.Text)==false)
                           FileImage(txtImageCode.Text);

                    }
                }
                //else
                //{
                //    Messages.MsgInfo(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord);
                //    return;
                //}
            }
            catch (Exception ex) { Messages.MsgInfo(Messages.TitleInfo, ex.Message); }
        }
        private void txtGuidanceID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as UserName  FROM  [Users] where [Cancel]=0  and BranchID="+Comon.cInt(cmbBranchesID.EditValue)+" and [UserID]=" + txtGuidanceID.Text.ToString();
                CSearch.ControlValidating(txtGuidanceID, lblGuidanceName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void txtDelegateID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as DelegateName FROM Sales_SalesDelegate WHERE DelegateID =" + txtDelegateID.Text + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                CSearch.ControlValidating(txtDelegateID, lblDelegateName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void txtCustomerID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                string strSql;
                DataTable dt;
                if (txtCustomerID.Text != string.Empty && txtCustomerID.Text != "0")
                {
                    strSQL = "SELECT ArbName as CustomerName  FROM Sales_CustomerAnSublierListArb Where    AcountID =" + txtCustomerID.Text + "  And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue) ;
                    Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, UserInfo.Language.ToString());
                    dt = Lip.SelectRecord(strSQL);
                    if (dt.Rows.Count > 0)
                    {
                        lblCustomerName.Text = dt.Rows[0]["CustomerName"].ToString();
                    }
                }
                else
                {
                    lblCustomerName.Text = "";
                    txtCustomerID.Text = "";

                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        #endregion

        #region InintGrids
        void initGridWax()
        {

            lstDetail = new BindingList<Manu_CadWaxDiamondZirconAfforistStages>();
            lstDetail.AllowNew = true;
            lstDetail.AllowEdit = true;
            lstDetail.AllowRemove = true;

            gridControl1.DataSource = lstDetail;

            DataTable dtitems = Lip.SelectRecord("SELECT   SizeID," + PrimaryName + "   FROM Stc_SizingUnits where BranchID="+Comon.cInt(cmbBranchesID.EditValue));
            string[] NameUnit = new string[dtitems.Rows.Count];
            for (int i = 0; i <= dtitems.Rows.Count - 1; i++)
                NameUnit[i] = dtitems.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems = new RepositoryItemComboBox();
            riComboBoxitems.Items.AddRange(NameUnit);
            gridControl1.RepositoryItems.Add(riComboBoxitems);
            GridCad.Columns[SizeName].ColumnEdit = riComboBoxitems;


            DataTable dtItemname = Lip.SelectRecord("Select " + PrimaryName + " from Stc_Items  Where Cancel=0 and BranchID=" + Comon.cInt(cmbBranchesID.EditValue));
            string[] ItemNames = new string[dtItemname.Rows.Count];
            for (int i = 0; i <= dtItemname.Rows.Count - 1; i++)
                ItemNames[i] = dtItemname.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems4 = new RepositoryItemComboBox();
            riComboBoxitems4.Items.AddRange(ItemNames);
            gridControl1.RepositoryItems.Add(riComboBoxitems4);
            GridCad.Columns[ItemName].ColumnEdit = riComboBoxitems4;

            GridCad.Columns["CommandID"].Visible = false;
            GridCad.Columns["GoldQTYCloves"].Visible = false;
            GridCad.Columns["BranchID"].Visible = false;
            GridCad.Columns["FacilityID"].Visible = false;
            GridCad.Columns["ArbItemName"].Visible = GridCad.Columns["ArbItemName"].Name == "col" + ItemName ? true : false;
            GridCad.Columns["EngItemName"].Visible = GridCad.Columns["EngItemName"].Name == "col" + ItemName ? true : false;

            GridCad.Columns["TotalCost"].OptionsColumn.ReadOnly = false;
            GridCad.Columns[ItemName].Visible = true;
            GridCad.Columns[ItemName].Caption = CaptionItemName;
            GridCad.Columns["TotalCost"].OptionsColumn.ReadOnly = true;
            GridCad.Columns["TotalCost"].OptionsColumn.AllowFocus = false;
            GridCad.Columns["SizeID"].Visible = false;
       
            GridCad.Columns[ItemName].Width = 150;
            GridCad.Columns[SizeName].Width = 120;

            if (UserInfo.Language == iLanguage.Arabic)
            {
                GridCad.Columns["EngItemName"].Visible = false;
                GridCad.Columns["EngSizeName"].Visible = false;
                GridCad.Columns["BarCode"].Caption = "باركود الصنف";
                GridCad.Columns["SizeID"].Caption = "رقم الوحدة";
                GridCad.Columns["ItemID"].Caption = "رقم الصنــف";

                GridCad.Columns[SizeName].Caption = "إسم الوحدة";
                GridCad.Columns["QTY"].Caption = "الكمية ";
                GridCad.Columns["CostPrice"].Caption = "القيمة";
                GridCad.Columns["TotalCost"].Caption = "الإجمالي ";

                GridCad.Columns["FactorName"].Caption = "إسم العامل";
                GridCad.Columns["DateBefore"].Caption = "تاريخ الاستلام ";
                GridCad.Columns["DateAfter"].Caption = "تاريخ التسليم ";
                
            }
            else
            {
                GridCad.Columns["ArbItemName"].Visible = false;
                GridCad.Columns["ArbSizeName"].Visible = false;
                GridCad.Columns["BarCode"].Caption = "BarCode";
                GridCad.Columns["SizeID"].Caption = "Unit ID";
                GridCad.Columns["ItemID"].Caption = "Item ID";
                GridCad.Columns[SizeName].Caption = "Unit Name ";
                GridCad.Columns["CostPrice"].Caption = "Cost Price";
                GridCad.Columns["QTY"].Caption = "QTY";
                GridCad.Columns["TotalCost"].Caption = "Total Cost ";
                //GridCad.Columns["Fingerprint"].Caption = "Fingerprint";

                GridCad.Columns["FactorName"].Caption = "Employee Name";
                GridCad.Columns["DateBefore"].Caption = "Date Before";
                GridCad.Columns["DateAfter"].Caption = "Date After";
                //GridCad.Columns["QTYGoldEqv"].Caption = "QTY Gold Eqv";
                //GridCad.Columns["QTYGold"].Caption = "QTY Gold";
            }

        }
        void initGridFactory()
        {

            lstDetailfactory = new BindingList<Menu_FactoryRunCommandfactory>();
            lstDetailfactory.AllowNew = true;
            lstDetailfactory.AllowEdit = true;
            lstDetailfactory.AllowRemove = true;
            gridControlfactroOpretion.DataSource = lstDetailfactory;

            DataTable dtitems = Lip.SelectRecord("SELECT   ArbName   FROM Menu_FactoryMachine ");
            string[] NameMachine = new string[dtitems.Rows.Count];
            for (int i = 0; i <= dtitems.Rows.Count - 1; i++)
                NameMachine[i] = dtitems.Rows[i]["ArbName"].ToString();
            RepositoryItemComboBox riComboBoxitems = new RepositoryItemComboBox();
            riComboBoxitems.Items.AddRange(NameMachine);
            gridControlfactroOpretion.RepositoryItems.Add(riComboBoxitems);

            DataTable dtStore = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM Stc_Stores WHERE  Cancel =0 and  BranchID=" + Comon.cInt(cmbBranchesID.EditValue));
            string[] StoreName = new string[dtStore.Rows.Count];
            for (int i = 0; i <= dtStore.Rows.Count - 1; i++)
                StoreName[i] = dtStore.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems2 = new RepositoryItemComboBox();
            riComboBoxitems2.Items.AddRange(StoreName);
            gridControlfactroOpretion.RepositoryItems.Add(riComboBoxitems2);
            GridViewBeforfactory.Columns["StoreName"].ColumnEdit = riComboBoxitems2;


            DataTable dtEmp = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM HR_EmployeeFile WHERE  Cancel =0 and  BranchID=" + Comon.cInt(cmbBranchesID.EditValue));
            string[] EmpName = new string[dtEmp.Rows.Count];
            for (int i = 0; i <= dtEmp.Rows.Count - 1; i++)
                EmpName[i] = dtEmp.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems3 = new RepositoryItemComboBox();
            riComboBoxitems3.Items.AddRange(EmpName);
            gridControlfactroOpretion.RepositoryItems.Add(riComboBoxitems3);
            GridViewBeforfactory.Columns["EmpName"].ColumnEdit = riComboBoxitems3;

            DataTable dtItemname = Lip.SelectRecord("Select " + PrimaryName + " from Stc_Items  Where Cancel=0 and  BranchID=" + Comon.cInt(cmbBranchesID.EditValue));
            string[] ItemNames = new string[dtItemname.Rows.Count];
            for (int i = 0; i <= dtItemname.Rows.Count - 1; i++)
                ItemNames[i] = dtItemname.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems4 = new RepositoryItemComboBox();
            riComboBoxitems4.Items.AddRange(ItemNames);
            gridControlfactroOpretion.RepositoryItems.Add(riComboBoxitems4);
            GridViewBeforfactory.Columns[ItemName].ColumnEdit = riComboBoxitems4;

            RepositoryItemLookUpEdit rSize = Common.LookUpEditSize();
            GridViewBeforfactory.Columns[SizeName].ColumnEdit = rSize;
            gridControlfactroOpretion.RepositoryItems.Add(rSize);


            GridViewBeforfactory.Columns["ID"].Visible = false;
            GridViewBeforfactory.Columns["ComandID"].Visible = false;
            GridViewBeforfactory.Columns["BarCode"].Visible = false;
            GridViewBeforfactory.Columns["EmpPolishnID"].Visible = false;
            GridViewBeforfactory.Columns["EmpPrentagID"].Visible = false;
            GridViewBeforfactory.Columns["Cancel"].Visible = false;
            GridViewBeforfactory.Columns["BranchID"].Visible = false;
            GridViewBeforfactory.Columns["FacilityID"].Visible = false;
            GridViewBeforfactory.Columns["SizeID"].Visible = false;
            GridViewBeforfactory.Columns["EditUserID"].Visible = false;
            GridViewBeforfactory.Columns["EditDate"].Visible = false;
            GridViewBeforfactory.Columns["EditTime"].Visible = false;
            GridViewBeforfactory.Columns["RegDate"].Visible = false;
            GridViewBeforfactory.Columns["UserID"].Visible = false;

            GridViewBeforfactory.Columns["ComputerInfo"].Visible = false;
            GridViewBeforfactory.Columns["EditComputerInfo"].Visible = false;
            GridViewBeforfactory.Columns["RegTime"].Visible = false;
            GridViewBeforfactory.Columns["HimLost"].Visible = false;
            GridViewBeforfactory.Columns["Credit"].Visible = false;
            GridViewBeforfactory.Columns["TypeOpration"].Visible = false;
            //GridViewBeforfactory.Columns["SizeID"].Visible = false;
            GridViewBeforfactory.Columns["CostPrice"].Visible = false;

            GridViewBeforfactory.Columns["EmpName"].Width = 120;

            GridViewBeforfactory.Columns["StoreName"].Width = 120;
            GridViewBeforfactory.Columns["EmpID"].Width = 120;
            GridViewBeforfactory.Columns["Signature"].Width = 120;
            GridViewBeforfactory.Columns["DebitDate"].Width = 110;
            GridViewBeforfactory.Columns["DebitTime"].Width = 85;
            GridViewBeforfactory.Columns["EmpID"].Visible = false;
            GridViewBeforfactory.Columns["DebitTime"].Visible = false;
            GridViewBeforfactory.Columns["StoreID"].Visible = false;
            //GridViewBeforfactory.Columns["StoreName"].Visible = false;
            GridViewBeforfactory.Columns["Signature"].Visible = false;
            GridViewBeforfactory.Columns["ShownInNext"].Visible = false;
            if (UserInfo.Language == iLanguage.Arabic)
            {
                GridViewBeforfactory.Columns["EngItemName"].Visible = false;
                GridViewBeforfactory.Columns["EngSizeName"].Visible = false;
                GridViewBeforfactory.Columns["ArbItemName"].Width = 150;
                GridViewBeforfactory.Columns["StoreID"].Caption = "رقم المخزن";
                GridViewBeforfactory.Columns["StoreName"].Caption = "إسم المخزن";
                GridViewBeforfactory.Columns["EmpID"].Caption = "رقم العامل";
                GridViewBeforfactory.Columns["EmpName"].Caption = "إسم العامل";
                GridViewBeforfactory.Columns["Debit"].Caption = "الوزن";
                GridViewBeforfactory.Columns["Credit"].Caption = "دائــن";
                GridViewBeforfactory.Columns["TypeOpration"].Caption = "نوع العملية";
                GridViewBeforfactory.Columns["Signature"].Caption = "التوقيع";
                GridViewBeforfactory.Columns["ItemID"].Caption = "رقم الصنف";
                GridViewBeforfactory.Columns["ArbItemName"].Caption = "اسم الصنف";
                GridViewBeforfactory.Columns["SizeID"].Caption = "رقم الوحده";
                GridViewBeforfactory.Columns[SizeName].Caption = "الوحده";
                GridViewBeforfactory.Columns["CostPrice"].Caption = "التكلفة";
                GridViewBeforfactory.Columns["DebitDate"].Caption = "التاريخ";
                GridViewBeforfactory.Columns["DebitTime"].Caption = "الوقت";
            }
            else
            {


                GridViewBeforfactory.Columns["StoreID"].Caption = "Store ID";
                GridViewBeforfactory.Columns["StoreName"].Caption = "Store Name";

            }
        }
        void initGridAfterFactory()
        {

            lstDetailAfterfactory = new BindingList<Menu_FactoryRunCommandfactory>();
            lstDetailAfterfactory.AllowNew = true;
            lstDetailAfterfactory.AllowEdit = true;
            lstDetailAfterfactory.AllowRemove = true;
            gridControlAfterFactory.DataSource = lstDetailAfterfactory;

            //

            DataTable dtStore = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM Stc_Stores WHERE  Cancel =0 and BranchID=" + Comon.cInt(cmbBranchesID.EditValue));
            string[] StoreName = new string[dtStore.Rows.Count];
            for (int i = 0; i <= dtStore.Rows.Count - 1; i++)
                StoreName[i] = dtStore.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems2 = new RepositoryItemComboBox();
            riComboBoxitems2.Items.AddRange(StoreName);
            gridControlAfterFactory.RepositoryItems.Add(riComboBoxitems2);
            GridViewAfterfactory.Columns["StoreName"].ColumnEdit = riComboBoxitems2;

            DataTable dtEmp = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM HR_EmployeeFile WHERE  Cancel =0  and BranchID=" + Comon.cInt(cmbBranchesID.EditValue));
            string[] EmpName = new string[dtEmp.Rows.Count];
            for (int i = 0; i <= dtEmp.Rows.Count - 1; i++)
                EmpName[i] = dtEmp.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems3 = new RepositoryItemComboBox();
            riComboBoxitems3.Items.AddRange(EmpName);
            gridControlAfterFactory.RepositoryItems.Add(riComboBoxitems3);
            GridViewAfterfactory.Columns["EmpName"].ColumnEdit = riComboBoxitems3;


            DataTable dtItemname = Lip.SelectRecord("Select " + PrimaryName + " from Stc_Items  Where Cancel=0 and BranchID=" + Comon.cInt(cmbBranchesID.EditValue));
            string[] ItemNames = new string[dtItemname.Rows.Count];
            for (int i = 0; i <= dtItemname.Rows.Count - 1; i++)
                ItemNames[i] = dtItemname.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems4 = new RepositoryItemComboBox();
            riComboBoxitems4.Items.AddRange(ItemNames);
            gridControlAfterFactory.RepositoryItems.Add(riComboBoxitems4);
            GridViewAfterfactory.Columns[ItemName].ColumnEdit = riComboBoxitems4;

            RepositoryItemLookUpEdit rSize = Common.LookUpEditSize();
            GridViewAfterfactory.Columns[SizeName].ColumnEdit = rSize;
            gridControlAfterFactory.RepositoryItems.Add(rSize);
            GridViewAfterfactory.Columns[SizeName].OptionsColumn.AllowEdit = true;
            GridViewAfterfactory.Columns[SizeName].OptionsColumn.AllowFocus = true;

            GridViewBeforfactory.Columns[SizeName].ColumnEdit = rSize;
            gridControlAfterFactory.RepositoryItems.Add(rSize);
            GridViewAfterfactory.Columns[SizeName].OptionsColumn.AllowEdit = true;
            GridViewAfterfactory.Columns[SizeName].OptionsColumn.AllowFocus = true;
            //
            GridViewAfterfactory.Columns["ID"].Visible = false;
            GridViewAfterfactory.Columns["ComandID"].Visible = false;
            GridViewAfterfactory.Columns["BarCode"].Visible = false;
            GridViewAfterfactory.Columns["EmpPolishnID"].Visible = false;
            GridViewAfterfactory.Columns["EmpPrentagID"].Visible = false;
            GridViewAfterfactory.Columns["Cancel"].Visible = false;
            GridViewAfterfactory.Columns["BranchID"].Visible = false;
            GridViewAfterfactory.Columns["FacilityID"].Visible = false;
            GridViewAfterfactory.Columns["SizeID"].Visible = false;
            GridViewAfterfactory.Columns["EditUserID"].Visible = false;
            GridViewAfterfactory.Columns["EditDate"].Visible = false;
            GridViewAfterfactory.Columns["EditTime"].Visible = false;
            GridViewAfterfactory.Columns["RegDate"].Visible = false;
            GridViewAfterfactory.Columns["UserID"].Visible = false;

            GridViewAfterfactory.Columns["ComputerInfo"].Visible = false;
            GridViewAfterfactory.Columns["EditComputerInfo"].Visible = false;
            GridViewAfterfactory.Columns["RegTime"].Visible = false;

            GridViewAfterfactory.Columns["Debit"].Visible = false;
            GridViewAfterfactory.Columns["TypeOpration"].Visible = false;
            //GridViewAfterfactory.Columns["SizeID"].Visible = false;
            GridViewAfterfactory.Columns["CostPrice"].Visible = false;

            GridViewAfterfactory.Columns["EmpName"].Width = 120;
            GridViewAfterfactory.Columns["EmpID"].Width = 120;
            GridViewAfterfactory.Columns["StoreName"].Width = 100;
            GridViewAfterfactory.Columns["Signature"].Width = 120;
            GridViewAfterfactory.Columns["DebitDate"].Width = 110;
            GridViewAfterfactory.Columns["DebitTime"].Width = 85;

            GridViewAfterfactory.Columns["EmpID"].Visible = false;
            GridViewAfterfactory.Columns["DebitTime"].Visible = false;
            GridViewAfterfactory.Columns["StoreID"].Visible = false;
            //GridViewAfterfactory.Columns["StoreName"].Visible = false;
            GridViewAfterfactory.Columns["Signature"].Visible = false;

            if (UserInfo.Language == iLanguage.Arabic)
            {
                GridViewAfterfactory.Columns["EngItemName"].Visible = false;
                GridViewAfterfactory.Columns["EngSizeName"].Visible = false;
                GridViewAfterfactory.Columns["ArbItemName"].Width = 150;
                GridViewAfterfactory.Columns["StoreID"].Caption = "رقم المخزن";
                GridViewAfterfactory.Columns["StoreName"].Caption = "إسم المخزن";
                GridViewAfterfactory.Columns["EmpID"].Caption = "رقم العامل";
                GridViewAfterfactory.Columns["EmpName"].Caption = "إسم العامل";
                GridViewAfterfactory.Columns["Debit"].Caption = "الوزن";
                GridViewAfterfactory.Columns["Credit"].Caption = "الـوزن";
                GridViewAfterfactory.Columns["TypeOpration"].Caption = "نوع العملية";
                GridViewAfterfactory.Columns["Signature"].Caption = "التوقيع";
                GridViewAfterfactory.Columns["ItemID"].Caption = "رقم الصنف";
                GridViewAfterfactory.Columns["ArbItemName"].Caption = "اسم الصنف";
                GridViewAfterfactory.Columns["SizeID"].Caption = "رقم الوحده";
                GridViewAfterfactory.Columns["ArbSizeName"].Caption = "الوحده";
                GridViewAfterfactory.Columns["CostPrice"].Caption = "التكلفة";
                GridViewAfterfactory.Columns["DebitDate"].Caption = "التاريخ";
                GridViewAfterfactory.Columns["DebitTime"].Caption = "الوقت";
                GridViewAfterfactory.Columns["HimLost"].Caption = "علية فاقد ";
                GridViewAfterfactory.Columns["ShownInNext"].Caption = "يظهر في التفاصيل ";
            }
            else
            {
                GridViewAfterfactory.Columns["ArbItemName"].Visible = false;
                GridViewAfterfactory.Columns["ArbSizeName"].Visible = false;
                GridViewAfterfactory.Columns["EngItemName"].Width = 150;
                GridViewAfterfactory.Columns["StoreID"].Caption = "Store ID";
                GridViewAfterfactory.Columns["StoreName"].Caption = "Store Name";
                GridViewAfterfactory.Columns["EngItemName"].Caption = "Item Name";
               
                 GridViewAfterfactory.Columns["Debit"].Caption = "debtor ";
                GridViewAfterfactory.Columns["EngSizeName"].Caption = "Unit";
                GridViewAfterfactory.Columns["Credit"].Caption = "QTY";
                GridViewAfterfactory.Columns["TypeOpration"].Caption = "Type Opration";
                GridViewAfterfactory.Columns["Signature"].Caption = "Signature";
                GridViewAfterfactory.Columns["DebitDate"].Caption = "Date";
                GridViewAfterfactory.Columns["DebitTime"].Caption = "Time";
                GridViewAfterfactory.Columns["EmpID"].Caption = "EmpID";
                GridViewAfterfactory.Columns["EmpName"].Caption = "Name";
                GridViewAfterfactory.Columns["HimLost"].Caption = "Him Lost";
                GridViewAfterfactory.Columns["ShownInNext"].Caption = "Shown In Next ";
            }
        }
        void initGridBeforPrentage1()
        {
            lstDetailPrentage1 = new BindingList<Menu_FactoryRunCommandPrentagAndPulishn>();
            lstDetailPrentage1.AllowNew = true;
            lstDetailPrentage1.AllowEdit = true;
            lstDetailPrentage1.AllowRemove = true;
            gridControlBeforPrentag.DataSource = lstDetailPrentage1;

            DataTable dtitems = Lip.SelectRecord("SELECT   " + PrimaryName + "   FROM Menu_FactoryMachine ");
            string[] NameMachine = new string[dtitems.Rows.Count];
            for (int i = 0; i <= dtitems.Rows.Count - 1; i++)
                NameMachine[i] = dtitems.Rows[i][PrimaryName].ToString();

            RepositoryItemComboBox riComboBoxitems = new RepositoryItemComboBox();
            riComboBoxitems.Items.AddRange(NameMachine);

            gridControlBeforPrentag.RepositoryItems.Add(riComboBoxitems);
            GridViewBeforPrentag.Columns["MachineName"].ColumnEdit = riComboBoxitems;

            DataTable dtStore = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM Stc_Stores WHERE  Cancel =0  and BranchID=" + Comon.cInt(cmbBranchesID.EditValue));
            string[] StoreName = new string[dtStore.Rows.Count];
            for (int i = 0; i <= dtStore.Rows.Count - 1; i++)
                StoreName[i] = dtStore.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems2 = new RepositoryItemComboBox();
            riComboBoxitems2.Items.AddRange(StoreName);
            gridControlBeforPrentag.RepositoryItems.Add(riComboBoxitems2);
            GridViewBeforPrentag.Columns["StoreName"].ColumnEdit = riComboBoxitems2;

            DataTable dtEmp = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM HR_EmployeeFile WHERE  Cancel =0  and BranchID=" + Comon.cInt(cmbBranchesID.EditValue));
            string[] EmpName = new string[dtEmp.Rows.Count];
            for (int i = 0; i <= dtEmp.Rows.Count - 1; i++)
                EmpName[i] = dtEmp.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems3 = new RepositoryItemComboBox();
            riComboBoxitems3.Items.AddRange(EmpName);
            gridControlBeforPrentag.RepositoryItems.Add(riComboBoxitems3);
            GridViewBeforPrentag.Columns["EmpName"].ColumnEdit = riComboBoxitems3;

            DataTable dtItemname = Lip.SelectRecord("Select " + PrimaryName + " from Stc_Items  Where Cancel=0 and BranchID=" + Comon.cInt(cmbBranchesID.EditValue));
            string[] ItemNames = new string[dtItemname.Rows.Count];
            for (int i = 0; i <= dtItemname.Rows.Count - 1; i++)
                ItemNames[i] = dtItemname.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems4 = new RepositoryItemComboBox();
            riComboBoxitems4.Items.AddRange(ItemNames);
            gridControlBeforPrentag.RepositoryItems.Add(riComboBoxitems4);
            GridViewBeforPrentag.Columns[ItemName].ColumnEdit = riComboBoxitems4;


            GridViewBeforPrentag.Columns["PrentagDebitTime"].Visible = false;
            GridViewBeforPrentag.Columns["PrSignature"].Visible = false;



            GridViewBeforPrentag.Columns["ID"].Visible = false;
            GridViewBeforPrentag.Columns["ComandID"].Visible = false;
            GridViewBeforPrentag.Columns["BarcodePrentag"].Visible = false;
            GridViewBeforPrentag.Columns["EmpPolishnID"].Visible = false;
            GridViewBeforPrentag.Columns["EmpPrentagID"].Visible = false;
            GridViewBeforPrentag.Columns["Cancel"].Visible = false;
            GridViewBeforPrentag.Columns["BranchID"].Visible = false;
            GridViewBeforPrentag.Columns["FacilityID"].Visible = false;

            GridViewBeforPrentag.Columns["EditUserID"].Visible = false;
            GridViewBeforPrentag.Columns["EditDate"].Visible = false;
            GridViewBeforPrentag.Columns["EditTime"].Visible = false;
            GridViewBeforPrentag.Columns["RegDate"].Visible = false;
            GridViewBeforPrentag.Columns["UserID"].Visible = false;
            GridViewBeforPrentag.Columns["SizeID"].Visible = false;
            GridViewBeforPrentag.Columns["ComputerInfo"].Visible = false;
            GridViewBeforPrentag.Columns["EditComputerInfo"].Visible = false;
            GridViewBeforPrentag.Columns["RegTime"].Visible = false;

            GridViewBeforPrentag.Columns["PrentagCredit"].Visible = false;
            GridViewBeforPrentag.Columns["TypeOpration"].Visible = false;
            //GridViewBeforPrentag.Columns["SizeID"].Visible = false;
            GridViewBeforPrentag.Columns["CostPrice"].Visible = false;

            // GridViewBeforPrentag.Columns["PrentagDebitTime"].Visible = false;
            GridViewBeforPrentag.Columns["MachinID"].Name = "MachinID";
            //dtItem.Columns["Losed"].ReadOnly = true;
            GridViewBeforPrentag.Columns["ShownInNext"].Visible = false;

            GridViewBeforPrentag.Columns["EmpName"].Width = 150;
            GridViewBeforPrentag.Columns["EmpID"].Width = 120;
            GridViewBeforPrentag.Columns["StoreName"].Width = 100;
            GridViewBeforPrentag.Columns["PrSignature"].Width = 85;
            GridViewBeforPrentag.Columns["PrentagDebitDate"].Width = 110;
            GridViewBeforPrentag.Columns["PrentagDebitTime"].Width = 85;
            GridViewBeforPrentag.Columns["EmpID"].Visible = false;

            GridViewBeforPrentag.Columns["MachinID"].Visible = false;
            GridViewBeforPrentag.Columns["MachineName"].Visible = false;

            GridViewBeforPrentag.Columns["StoreID"].Visible = false;
            if (UserInfo.Language == iLanguage.Arabic)
            {

                GridViewBeforPrentag.Columns["EngItemName"].Visible = false;
                GridViewBeforPrentag.Columns["EngSizeName"].Visible = false;
                GridViewBeforPrentag.Columns["ArbItemName"].Width = 150;

                GridViewBeforPrentag.Columns["StoreID"].Caption = "رقم المخزن";
                GridViewBeforPrentag.Columns["StoreName"].Caption = "إسم المخزن";

                GridViewBeforPrentag.Columns["EmpID"].Caption = "رقم العامل";
                GridViewBeforPrentag.Columns["EmpName"].Caption = "إسم العامل";

                GridViewBeforPrentag.Columns["MachinID"].Caption = "رقم المكينة";
                GridViewBeforPrentag.Columns["MachineName"].Caption = "إسم المكينة";
                GridViewBeforPrentag.Columns["PrentagDebit"].Caption = "الوزن";

                GridViewBeforPrentag.Columns["PrentagCredit"].Caption = "دائــن";
                GridViewBeforPrentag.Columns["TypeOpration"].Caption = "نوع العملية";
                GridViewBeforPrentag.Columns["PrSignature"].Caption = "التوقيع";

                GridViewBeforPrentag.Columns["ItemID"].Caption = "رقم الصنف";
                GridViewBeforPrentag.Columns["ArbItemName"].Caption = "اسم الصنف";
                GridViewBeforPrentag.Columns["SizeID"].Caption = "رقم الوحده";
                GridViewBeforPrentag.Columns["ArbSizeName"].Caption = "الوحده";
                GridViewBeforPrentag.Columns["CostPrice"].Caption = "التكلفة";
                GridViewBeforPrentag.Columns["PrentagDebitDate"].Caption = "التاريخ";
                GridViewBeforPrentag.Columns["PrentagDebitTime"].Caption = "الوقت";
            }
            else
            {
                GridViewBeforPrentag.Columns["ArbItemName"].Visible = false;
                GridViewBeforPrentag.Columns["ArbSizeName"].Visible = false;
                GridViewBeforPrentag.Columns["EngItemName"].Width = 150;
                GridViewBeforPrentag.Columns["StoreID"].Caption = "Store ID";
                GridViewBeforPrentag.Columns["StoreName"].Caption = "Store Name";
                GridViewBeforPrentag.Columns["EngItemName"].Caption = "Item Name";
                GridViewBeforPrentag.Columns["MachinID"].Caption = "Machine ID";
                GridViewBeforPrentag.Columns["MachineName"].Caption = "Machin Name";
                GridViewBeforPrentag.Columns["PrentagDebit"].Caption = "debtor ";
                GridViewBeforPrentag.Columns["EngSizeName"].Caption = "Unit";
                GridViewBeforPrentag.Columns["PrentagCredit"].Caption = "Creditor";
                GridViewBeforPrentag.Columns["TypeOpration"].Caption = "Type Opration";
                GridViewBeforPrentag.Columns["PrSignature"].Caption = "Signature";
                GridViewBeforPrentag.Columns["PrentagDebitDate"].Caption = "Date";
                GridViewBeforPrentag.Columns["PrentagDebitTime"].Caption = "Time";
                GridViewBeforPrentag.Columns["EmpID"].Caption = "EmpID";
                GridViewBeforPrentag.Columns["EmpName"].Caption = "Name";
            }
            GridViewBeforPrentag.Columns["MachinID"].OptionsColumn.AllowFocus = false;
            GridViewBeforPrentag.Columns["MachinID"].OptionsColumn.AllowEdit = false;

            GridViewBeforPrentag.Columns["MachineName"].OptionsColumn.AllowFocus = false;
            GridViewBeforPrentag.Columns["MachineName"].OptionsColumn.AllowEdit = false;



        }
        void initGridAfterPrentage1()
        {

            lstDetailAfterPrentage1 = new BindingList<Menu_FactoryRunCommandPrentagAndPulishn>();
            lstDetailAfterPrentage1.AllowNew = true;
            lstDetailAfterPrentage1.AllowEdit = true;
            lstDetailAfterPrentage1.AllowRemove = true;
            gridControlAfterPrentage.DataSource = lstDetailAfterPrentage1;

            DataTable dtitems = Lip.SelectRecord("SELECT   ArbName   FROM Menu_FactoryMachine ");
            string[] NameMachine = new string[dtitems.Rows.Count];
            for (int i = 0; i <= dtitems.Rows.Count - 1; i++)
                NameMachine[i] = dtitems.Rows[i]["ArbName"].ToString();

            RepositoryItemComboBox riComboBoxitems = new RepositoryItemComboBox();
            riComboBoxitems.Items.AddRange(NameMachine);

            gridControlAfterPrentage.RepositoryItems.Add(riComboBoxitems);


            DataTable dtStore = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM Stc_Stores WHERE  Cancel =0  and BranchID=" + Comon.cInt(cmbBranchesID.EditValue));
            string[] StoreName = new string[dtStore.Rows.Count];
            for (int i = 0; i <= dtStore.Rows.Count - 1; i++)
                StoreName[i] = dtStore.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems2 = new RepositoryItemComboBox();

            riComboBoxitems2.Items.AddRange(StoreName);
            gridControlAfterPrentage.RepositoryItems.Add(riComboBoxitems2);
            GridViewAfterPrentag.Columns["StoreName"].ColumnEdit = riComboBoxitems2;

            DataTable dtEmp = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM HR_EmployeeFile WHERE  Cancel =0 and BranchID=" + Comon.cInt(cmbBranchesID.EditValue));
            string[] EmpName = new string[dtEmp.Rows.Count];
            for (int i = 0; i <= dtEmp.Rows.Count - 1; i++)
                EmpName[i] = dtEmp.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems3 = new RepositoryItemComboBox();
            riComboBoxitems3.Items.AddRange(EmpName);
            gridControlAfterPrentage.RepositoryItems.Add(riComboBoxitems3);
            GridViewAfterPrentag.Columns["EmpName"].ColumnEdit = riComboBoxitems3;

            DataTable dtItemname = Lip.SelectRecord("Select " + PrimaryName + " from Stc_Items  Where Cancel=0 and BranchID=" + Comon.cInt(cmbBranchesID.EditValue));
            string[] ItemNames = new string[dtItemname.Rows.Count];
            for (int i = 0; i <= dtItemname.Rows.Count - 1; i++)
                ItemNames[i] = dtItemname.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems4 = new RepositoryItemComboBox();
            riComboBoxitems4.Items.AddRange(ItemNames);
            gridControlAfterPrentage.RepositoryItems.Add(riComboBoxitems4);
            GridViewAfterPrentag.Columns[ItemName].ColumnEdit = riComboBoxitems4;

            GridViewAfterPrentag.Columns["MachineName"].ColumnEdit = riComboBoxitems;
            GridViewAfterPrentag.Columns["ID"].Visible = false;
            GridViewAfterPrentag.Columns["ComandID"].Visible = false;
            GridViewAfterPrentag.Columns["BarcodePrentag"].Visible = false;
            GridViewAfterPrentag.Columns["EmpPolishnID"].Visible = false;
            GridViewAfterPrentag.Columns["EmpPrentagID"].Visible = false;
            GridViewAfterPrentag.Columns["Cancel"].Visible = false;
            GridViewAfterPrentag.Columns["BranchID"].Visible = false;
            GridViewAfterPrentag.Columns["FacilityID"].Visible = false;

            GridViewAfterPrentag.Columns["PrentagDebitTime"].Visible = false;
            GridViewAfterPrentag.Columns["PrSignature"].Visible = false;

            GridViewAfterPrentag.Columns["EditUserID"].Visible = false;
            GridViewAfterPrentag.Columns["EditDate"].Visible = false;
            GridViewAfterPrentag.Columns["EditTime"].Visible = false;
            GridViewAfterPrentag.Columns["RegDate"].Visible = false;
            GridViewAfterPrentag.Columns["UserID"].Visible = false;

            GridViewAfterPrentag.Columns["ComputerInfo"].Visible = false;
            GridViewAfterPrentag.Columns["EditComputerInfo"].Visible = false;
            GridViewAfterPrentag.Columns["RegTime"].Visible = false;
            GridViewAfterPrentag.Columns["SizeID"].Visible = false;
            GridViewAfterPrentag.Columns["PrentagDebit"].Visible = false;
            GridViewAfterPrentag.Columns["TypeOpration"].Visible = false;
            //GridViewAfterPrentag.Columns["SizeID"].Visible = false;
            GridViewAfterPrentag.Columns["CostPrice"].Visible = false;
            // GridViewAfterPrentag.Columns["PrentagDebitTime"].Visible = false;
            GridViewAfterPrentag.Columns["MachinID"].Name = "MachinID";
            //dtItem.Columns["Losed"].ReadOnly = true;
            GridViewAfterPrentag.Columns["EmpName"].Width = 150;
            GridViewAfterPrentag.Columns["EmpID"].Width = 120;
            GridViewAfterPrentag.Columns["StoreName"].Width = 100;
            GridViewAfterPrentag.Columns["PrSignature"].Width = 85;
            GridViewAfterPrentag.Columns["PrentagDebitDate"].Width = 110;
            GridViewAfterPrentag.Columns["PrentagDebitTime"].Width = 85;
            GridViewAfterPrentag.Columns["EmpID"].Visible = false;
            GridViewAfterPrentag.Columns["MachinID"].Visible = false;
            GridViewAfterPrentag.Columns["MachineName"].Visible = false;
            GridViewAfterPrentag.Columns["StoreID"].Visible = false;
            if (UserInfo.Language == iLanguage.Arabic)
            {
                GridViewAfterPrentag.Columns["EngItemName"].Visible = false;
                GridViewAfterPrentag.Columns["EngSizeName"].Visible = false;
                GridViewAfterPrentag.Columns["ArbItemName"].Width = 150;
                GridViewAfterPrentag.Columns["StoreID"].Caption = "رقم المخزن";
                GridViewAfterPrentag.Columns["StoreName"].Caption = "إسم المخزن";
                GridViewAfterPrentag.Columns["EmpID"].Caption = "رقم العامل";
                GridViewAfterPrentag.Columns["EmpName"].Caption = "إسم العامل";
                GridViewAfterPrentag.Columns["MachinID"].Caption = "رقم المكينة";
                GridViewAfterPrentag.Columns["MachineName"].Caption = "إسم المكينة";
                GridViewAfterPrentag.Columns["PrentagDebit"].Caption = "الوزن";
                GridViewAfterPrentag.Columns["PrentagCredit"].Caption = "الوزن";
                GridViewAfterPrentag.Columns["TypeOpration"].Caption = "نوع العملية ";
                GridViewAfterPrentag.Columns["PrSignature"].Caption = "التوقيع";
                GridViewAfterPrentag.Columns["ItemID"].Caption = "رقم الصنف";
                GridViewAfterPrentag.Columns["ArbItemName"].Caption = "اسم الصنف";
                GridViewAfterPrentag.Columns["SizeID"].Caption = "رقم الوحده";
                GridViewAfterPrentag.Columns["ArbSizeName"].Caption = "الوحده";
                GridViewAfterPrentag.Columns["CostPrice"].Caption = "التكلفة";
                GridViewAfterPrentag.Columns["PrentagDebitDate"].Caption = "التاريخ";
                GridViewAfterPrentag.Columns["PrentagDebitTime"].Caption = "الوقت";
                GridViewAfterPrentag.Columns["ShownInNext"].Caption = "يظهر في التفاصيل ";
            }
            else
            {
                GridViewAfterPrentag.Columns["ArbItemName"].Visible = false;
                GridViewAfterPrentag.Columns["ArbSizeName"].Visible = false;
                GridViewAfterPrentag.Columns["EngItemName"].Width = 150;
                GridViewAfterPrentag.Columns["StoreID"].Caption = "Store ID";
                GridViewAfterPrentag.Columns["StoreName"].Caption = "Store Name";
                GridViewAfterPrentag.Columns["EngItemName"].Caption = "Item Name";
                GridViewAfterPrentag.Columns["MachinID"].Caption = "Machine ID";
                GridViewAfterPrentag.Columns["MachineName"].Caption = "Machin Name";
                GridViewAfterPrentag.Columns["PrentagDebit"].Caption = "debtor ";
                GridViewAfterPrentag.Columns["EngSizeName"].Caption = "Unit";
                GridViewAfterPrentag.Columns["PrentagCredit"].Caption = "Creditor";
                GridViewAfterPrentag.Columns["TypeOpration"].Caption = "Type Opration";
                GridViewAfterPrentag.Columns["PrSignature"].Caption = "Signature";
                GridViewAfterPrentag.Columns["PrentagDebitDate"].Caption = "Date";
                GridViewAfterPrentag.Columns["PrentagDebitTime"].Caption = "Time";
                GridViewAfterPrentag.Columns["EmpID"].Caption = "EmpID";
                GridViewAfterPrentag.Columns["EmpName"].Caption = "Name";
                GridViewAfterPrentag.Columns["ShownInNext"].Caption = "Shown In Next ";
            }
        }

        void initGridBeforPrentage2()
        {
            lstDetailPrentage2 = new BindingList<Menu_FactoryRunCommandPrentagAndPulishn>();
            lstDetailPrentage2.AllowNew = true;
            lstDetailPrentage2.AllowEdit = true;
            lstDetailPrentage2.AllowRemove = true;
            gridControl3.DataSource = lstDetailPrentage2;

            DataTable dtitems = Lip.SelectRecord("SELECT   " + PrimaryName + "   FROM Menu_FactoryMachine ");
            string[] NameMachine = new string[dtitems.Rows.Count];
            for (int i = 0; i <= dtitems.Rows.Count - 1; i++)
                NameMachine[i] = dtitems.Rows[i][PrimaryName].ToString();

            RepositoryItemComboBox riComboBoxitems = new RepositoryItemComboBox();
            riComboBoxitems.Items.AddRange(NameMachine);
            
            gridControl3.RepositoryItems.Add(riComboBoxitems);
            gridView12.Columns["MachineName"].ColumnEdit = riComboBoxitems;

            DataTable dtStore = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM Stc_Stores WHERE  Cancel =0  and BranchID=" + Comon.cInt(cmbBranchesID.EditValue));
            string[] StoreName = new string[dtStore.Rows.Count];
            for (int i = 0; i <= dtStore.Rows.Count - 1; i++)
                StoreName[i] = dtStore.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems2 = new RepositoryItemComboBox();
            riComboBoxitems2.Items.AddRange(StoreName);
            gridControl3.RepositoryItems.Add(riComboBoxitems2);
            gridView12.Columns["StoreName"].ColumnEdit = riComboBoxitems2;

            DataTable dtEmp = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM HR_EmployeeFile WHERE  Cancel =0  and BranchID=" + Comon.cInt(cmbBranchesID.EditValue));
            string[] EmpName = new string[dtEmp.Rows.Count];
            for (int i = 0; i <= dtEmp.Rows.Count - 1; i++)
                EmpName[i] = dtEmp.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems3 = new RepositoryItemComboBox();
            riComboBoxitems3.Items.AddRange(EmpName);
            gridControl3.RepositoryItems.Add(riComboBoxitems3);
            gridView12.Columns["EmpName"].ColumnEdit = riComboBoxitems3;

            DataTable dtItemname = Lip.SelectRecord("Select " + PrimaryName + " from Stc_Items  Where Cancel=0 and BranchID=" + Comon.cInt(cmbBranchesID.EditValue));
            string[] ItemNames = new string[dtItemname.Rows.Count];
            for (int i = 0; i <= dtItemname.Rows.Count - 1; i++)
                ItemNames[i] = dtItemname.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems4 = new RepositoryItemComboBox();
            riComboBoxitems4.Items.AddRange(ItemNames);
            gridControl3.RepositoryItems.Add(riComboBoxitems4);
            gridView12.Columns[ItemName].ColumnEdit = riComboBoxitems4;


            gridView12.Columns["PrentagDebitTime"].Visible = false;
            gridView12.Columns["PrSignature"].Visible = false;



            gridView12.Columns["ID"].Visible = false;
            gridView12.Columns["ComandID"].Visible = false;
            gridView12.Columns["BarcodePrentag"].Visible = false;
            gridView12.Columns["EmpPolishnID"].Visible = false;
            gridView12.Columns["EmpPrentagID"].Visible = false;
            gridView12.Columns["Cancel"].Visible = false;
            gridView12.Columns["BranchID"].Visible = false;
            gridView12.Columns["FacilityID"].Visible = false;

            gridView12.Columns["EditUserID"].Visible = false;
            gridView12.Columns["EditDate"].Visible = false;
            gridView12.Columns["EditTime"].Visible = false;
            gridView12.Columns["RegDate"].Visible = false;
            gridView12.Columns["UserID"].Visible = false;
            gridView12.Columns["SizeID"].Visible = false;
            gridView12.Columns["ComputerInfo"].Visible = false;
            gridView12.Columns["EditComputerInfo"].Visible = false;
            gridView12.Columns["RegTime"].Visible = false;

            gridView12.Columns["PrentagCredit"].Visible = false;
            gridView12.Columns["TypeOpration"].Visible = false;
            //gridView12.Columns["SizeID"].Visible = false;
            gridView12.Columns["CostPrice"].Visible = false;

            // gridView12.Columns["PrentagDebitTime"].Visible = false;
            gridView12.Columns["MachinID"].Name = "MachinID";
            //dtItem.Columns["Losed"].ReadOnly = true;


            gridView12.Columns["EmpName"].Width = 150;
            gridView12.Columns["EmpID"].Width = 120;
            gridView12.Columns["StoreName"].Width = 100;
            gridView12.Columns["PrSignature"].Width = 85;
            gridView12.Columns["PrentagDebitDate"].Width = 110;
            gridView12.Columns["PrentagDebitTime"].Width = 85;
            gridView12.Columns["EmpID"].Visible = false;

            gridView12.Columns["MachinID"].Visible = false;
            gridView12.Columns["MachineName"].Visible = false;
            gridView12.Columns["ShownInNext"].Visible = false;
            gridView12.Columns["StoreID"].Visible = false;
            if (UserInfo.Language == iLanguage.Arabic)
            {

                gridView12.Columns["EngItemName"].Visible = false;
                gridView12.Columns["EngSizeName"].Visible = false;
                gridView12.Columns["ArbItemName"].Width = 150;

                gridView12.Columns["StoreID"].Caption = "رقم المخزن";
                gridView12.Columns["StoreName"].Caption = "إسم المخزن";

                gridView12.Columns["EmpID"].Caption = "رقم العامل";
                gridView12.Columns["EmpName"].Caption = "إسم العامل";

                gridView12.Columns["MachinID"].Caption = "رقم المكينة";
                gridView12.Columns["MachineName"].Caption = "إسم المكينة";
                gridView12.Columns["PrentagDebit"].Caption = "الوزن";

                gridView12.Columns["PrentagCredit"].Caption = "دائــن";
                gridView12.Columns["TypeOpration"].Caption = "نوع العملية";
                gridView12.Columns["PrSignature"].Caption = "التوقيع";

                gridView12.Columns["ItemID"].Caption = "رقم الصنف";
                gridView12.Columns["ArbItemName"].Caption = "اسم الصنف";
                gridView12.Columns["SizeID"].Caption = "رقم الوحده";
                gridView12.Columns["ArbSizeName"].Caption = "الوحده";
                gridView12.Columns["CostPrice"].Caption = "التكلفة";
                gridView12.Columns["PrentagDebitDate"].Caption = "التاريخ";
                gridView12.Columns["PrentagDebitTime"].Caption = "الوقت";
            }
            else
            {
                gridView12.Columns["ArbItemName"].Visible = false;
                gridView12.Columns["ArbSizeName"].Visible = false;
                gridView12.Columns["EngItemName"].Width = 150;
                gridView12.Columns["StoreID"].Caption = "Store ID";
                gridView12.Columns["StoreName"].Caption = "Store Name";
                gridView12.Columns["EngItemName"].Caption = "Item Name";
                gridView12.Columns["MachinID"].Caption = "Machine ID";
                gridView12.Columns["MachineName"].Caption = "Machin Name";
                gridView12.Columns["PrentagDebit"].Caption = "debtor ";
                gridView12.Columns["EngSizeName"].Caption = "Unit";
                gridView12.Columns["PrentagCredit"].Caption = "Creditor";
                gridView12.Columns["TypeOpration"].Caption = "Type Opration";
                gridView12.Columns["PrSignature"].Caption = "Signature";
                gridView12.Columns["PrentagDebitDate"].Caption = "Date";
                gridView12.Columns["PrentagDebitTime"].Caption = "Time";
                gridView12.Columns["EmpID"].Caption = "EmpID";
                gridView12.Columns["EmpName"].Caption = "Name";
            }
            gridView12.Columns["MachinID"].OptionsColumn.AllowFocus = false;
            gridView12.Columns["MachinID"].OptionsColumn.AllowEdit = false;

            gridView12.Columns["MachineName"].OptionsColumn.AllowFocus = false;
            gridView12.Columns["MachineName"].OptionsColumn.AllowEdit = false;



        }
        void initGridAfterPrentage2()
        {

            lstDetailAfterPrentage2 = new BindingList<Menu_FactoryRunCommandPrentagAndPulishn>();
            lstDetailAfterPrentage2.AllowNew = true;
            lstDetailAfterPrentage2.AllowEdit = true;
            lstDetailAfterPrentage2.AllowRemove = true;
            gridControl2.DataSource = lstDetailAfterPrentage2;

            DataTable dtitems = Lip.SelectRecord("SELECT   ArbName   FROM Menu_FactoryMachine ");
            string[] NameMachine = new string[dtitems.Rows.Count];
            for (int i = 0; i <= dtitems.Rows.Count - 1; i++)
                NameMachine[i] = dtitems.Rows[i]["ArbName"].ToString();

            RepositoryItemComboBox riComboBoxitems = new RepositoryItemComboBox();
            riComboBoxitems.Items.AddRange(NameMachine);

            gridControl2.RepositoryItems.Add(riComboBoxitems);


            DataTable dtStore = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM Stc_Stores WHERE  Cancel =0  and BranchID=" + Comon.cInt(cmbBranchesID.EditValue) );
            string[] StoreName = new string[dtStore.Rows.Count];
            for (int i = 0; i <= dtStore.Rows.Count - 1; i++)
                StoreName[i] = dtStore.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems2 = new RepositoryItemComboBox();

            riComboBoxitems2.Items.AddRange(StoreName);
            gridControl2.RepositoryItems.Add(riComboBoxitems2);
            gridView9.Columns["StoreName"].ColumnEdit = riComboBoxitems2;

            DataTable dtEmp = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM HR_EmployeeFile WHERE  Cancel =0  and BranchID=" + Comon.cInt(cmbBranchesID.EditValue) );
            string[] EmpName = new string[dtEmp.Rows.Count];
            for (int i = 0; i <= dtEmp.Rows.Count - 1; i++)
                EmpName[i] = dtEmp.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems3 = new RepositoryItemComboBox();
            riComboBoxitems3.Items.AddRange(EmpName);
            gridControl2.RepositoryItems.Add(riComboBoxitems3);
            gridView9.Columns["EmpName"].ColumnEdit = riComboBoxitems3;

            DataTable dtItemname = Lip.SelectRecord("Select " + PrimaryName + " from Stc_Items  Where Cancel=0 and BranchID=" + Comon.cInt(cmbBranchesID.EditValue) );
            string[] ItemNames = new string[dtItemname.Rows.Count];
            for (int i = 0; i <= dtItemname.Rows.Count - 1; i++)
                ItemNames[i] = dtItemname.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems4 = new RepositoryItemComboBox();
            riComboBoxitems4.Items.AddRange(ItemNames);
            gridControl2.RepositoryItems.Add(riComboBoxitems4);
            gridView9.Columns[ItemName].ColumnEdit = riComboBoxitems4;

            gridView9.Columns["MachineName"].ColumnEdit = riComboBoxitems;
            gridView9.Columns["ID"].Visible = false;
            gridView9.Columns["ComandID"].Visible = false;
            gridView9.Columns["BarcodePrentag"].Visible = false;
            gridView9.Columns["EmpPolishnID"].Visible = false;
            gridView9.Columns["EmpPrentagID"].Visible = false;
            gridView9.Columns["Cancel"].Visible = false;
            gridView9.Columns["BranchID"].Visible = false;
            gridView9.Columns["FacilityID"].Visible = false;

            gridView9.Columns["PrentagDebitTime"].Visible = false;
            gridView9.Columns["PrSignature"].Visible = false;

            gridView9.Columns["EditUserID"].Visible = false;
            gridView9.Columns["EditDate"].Visible = false;
            gridView9.Columns["EditTime"].Visible = false;
            gridView9.Columns["RegDate"].Visible = false;
            gridView9.Columns["UserID"].Visible = false;

            gridView9.Columns["ComputerInfo"].Visible = false;
            gridView9.Columns["EditComputerInfo"].Visible = false;
            gridView9.Columns["RegTime"].Visible = false;
            gridView9.Columns["SizeID"].Visible = false;
            gridView9.Columns["PrentagDebit"].Visible = false;
            gridView9.Columns["TypeOpration"].Visible = false;
            //gridView9.Columns["SizeID"].Visible = false;
            gridView9.Columns["CostPrice"].Visible = false;
            // gridView9.Columns["PrentagDebitTime"].Visible = false;
            gridView9.Columns["MachinID"].Name = "MachinID";
            //dtItem.Columns["Losed"].ReadOnly = true;
            gridView9.Columns["EmpName"].Width = 150;
            gridView9.Columns["EmpID"].Width = 120;
            gridView9.Columns["StoreName"].Width = 100;
            gridView9.Columns["PrSignature"].Width = 85;
            gridView9.Columns["PrentagDebitDate"].Width = 110;
            gridView9.Columns["PrentagDebitTime"].Width = 85;
            gridView9.Columns["EmpID"].Visible = false;
            gridView9.Columns["MachinID"].Visible = false;
            gridView9.Columns["MachineName"].Visible = false;
            gridView9.Columns["StoreID"].Visible = false;
            if (UserInfo.Language == iLanguage.Arabic)
            {
                gridView9.Columns["EngItemName"].Visible = false;
                gridView9.Columns["EngSizeName"].Visible = false;
                gridView9.Columns["ArbItemName"].Width = 150;
                gridView9.Columns["StoreID"].Caption = "رقم المخزن";
                gridView9.Columns["StoreName"].Caption = "إسم المخزن";
                gridView9.Columns["EmpID"].Caption = "رقم العامل";
                gridView9.Columns["EmpName"].Caption = "إسم العامل";
                gridView9.Columns["MachinID"].Caption = "رقم المكينة";
                gridView9.Columns["MachineName"].Caption = "إسم المكينة";
                gridView9.Columns["PrentagDebit"].Caption = "الوزن";
                gridView9.Columns["PrentagCredit"].Caption = "الوزن";
                gridView9.Columns["TypeOpration"].Caption = "نوع العملية ";
                gridView9.Columns["PrSignature"].Caption = "التوقيع";
                gridView9.Columns["ItemID"].Caption = "رقم الصنف";
                gridView9.Columns["ArbItemName"].Caption = "اسم الصنف";
                gridView9.Columns["SizeID"].Caption = "رقم الوحده";
                gridView9.Columns["ArbSizeName"].Caption = "الوحده";
                gridView9.Columns["CostPrice"].Caption = "التكلفة";
                gridView9.Columns["PrentagDebitDate"].Caption = "التاريخ";
                gridView9.Columns["PrentagDebitTime"].Caption = "الوقت";
                gridView9.Columns["ShownInNext"].Caption = "يظهر في التفاصيل ";
            }
            else
            {
                gridView9.Columns["ArbItemName"].Visible = false;
                gridView9.Columns["ArbSizeName"].Visible = false;
                gridView9.Columns["EngItemName"].Width = 150;
                gridView9.Columns["StoreID"].Caption = "Store ID";
                gridView9.Columns["StoreName"].Caption = "Store Name";
                gridView9.Columns["EngItemName"].Caption = "Item Name";
                gridView9.Columns["MachinID"].Caption = "Machine ID";
                gridView9.Columns["MachineName"].Caption = "Machin Name";
                gridView9.Columns["PrentagDebit"].Caption = "debtor ";
                gridView9.Columns["EngSizeName"].Caption = "Unit";
                gridView9.Columns["PrentagCredit"].Caption = "Creditor";
                gridView9.Columns["TypeOpration"].Caption = "Type Opration";
                gridView9.Columns["PrSignature"].Caption = "Signature";
                gridView9.Columns["PrentagDebitDate"].Caption = "Date";
                gridView9.Columns["PrentagDebitTime"].Caption = "Time";
                gridView9.Columns["EmpID"].Caption = "EmpID";
                gridView9.Columns["EmpName"].Caption = "Name";
                gridView9.Columns["ShownInNext"].Caption = "Shown In Next ";
            }
        }


        void initGridBeforCompent()
        {
            lstDetailCompund = new BindingList<Menu_FactoryRunCommandCompund>();
            lstDetailCompund.AllowNew = true;
            lstDetailCompund.AllowEdit = true;
            lstDetailCompund.AllowRemove = true;
            gridControlBeforCompond.DataSource = lstDetailAfterCompund;

            gridViewBeforCompond.Columns["ID"].Visible = false;
            DataTable dtEmp = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM HR_EmployeeFile WHERE  Cancel =0  and BranchID=" + Comon.cInt(cmbBranchesID.EditValue) );
            string[] EmpName = new string[dtEmp.Rows.Count];
            for (int i = 0; i <= dtEmp.Rows.Count - 1; i++)
                EmpName[i] = dtEmp.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems3 = new RepositoryItemComboBox();
            riComboBoxitems3.Items.AddRange(EmpName);
            gridControlBeforCompond.RepositoryItems.Add(riComboBoxitems3);
            gridViewBeforCompond.Columns["EmpCompundName"].ColumnEdit = riComboBoxitems3;
            RepositoryItemLookUpEdit rAccountName = Common.LookUpEditAccountName();
            gridControlBeforCompond.RepositoryItems.Add(rAccountName);
            gridViewBeforCompond.Columns["FromAccountName"].ColumnEdit = rAccountName;


            DataTable dtItemname = Lip.SelectRecord("Select " + PrimaryName + " from Stc_Items  Where Cancel=0 and BranchID=" + Comon.cInt(cmbBranchesID.EditValue) );
            string[] ItemNames = new string[dtItemname.Rows.Count];
            for (int i = 0; i <= dtItemname.Rows.Count - 1; i++)
                ItemNames[i] = dtItemname.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems4 = new RepositoryItemComboBox();
            riComboBoxitems4.Items.AddRange(ItemNames);
            gridControlAfterCompond.RepositoryItems.Add(riComboBoxitems4);
            gridViewBeforCompond.Columns[ItemName].ColumnEdit = riComboBoxitems4;

            gridViewBeforCompond.Columns["ComSignature"].Visible = false;
            gridViewBeforCompond.Columns["CostPrice"].Visible = false;
            gridViewBeforCompond.Columns["DebitTime"].Visible = false;
            gridViewBeforCompond.Columns["ComandID"].Visible = false;
            gridViewBeforCompond.Columns["Cancel"].Visible = false;
            gridViewBeforCompond.Columns["BranchID"].Visible = false;
            gridViewBeforCompond.Columns["FacilityID"].Visible = false;
            gridViewBeforCompond.Columns["SizeID"].Visible = false;
            gridViewBeforCompond.Columns["RegTime"].Visible = false;
            gridViewBeforCompond.Columns["RegDate"].Visible = false;
            gridViewBeforCompond.Columns["InvoiceImage"].Visible = false;
            gridViewBeforCompond.Columns["TypeID"].Visible = false;

            gridViewBeforCompond.Columns["EditUserID"].Visible = false;
            gridViewBeforCompond.Columns["EditDate"].Visible = false;
            gridViewBeforCompond.Columns["EditTime"].Visible = false;
            gridViewBeforCompond.Columns["UserID"].Visible = false;
            gridViewBeforCompond.Columns["TypeOpration"].Visible = false;
            gridViewBeforCompond.Columns["ComputerInfo"].Visible = false;
            gridViewBeforCompond.Columns["EditComputerInfo"].Visible = false;
            gridViewBeforCompond.Columns["GoldCompundNet"].Visible = false;

            gridViewBeforCompond.Columns["FromAccountID"].Name = "FromAccountID";
            gridViewBeforCompond.Columns["BarcodCompond"].Name = "BarcodCompond";
            gridViewBeforCompond.Columns["EmpCompondID"].Name = "EmpCompondID";
            gridViewBeforCompond.Columns["EmpCompundName"].Width = 120;
            gridViewBeforCompond.Columns["FromAccountName"].Width = 120;
            gridViewBeforCompond.Columns["ComSignature"].Width = 45;
            gridViewBeforCompond.Columns["GoldDebit"].Visible = false;
            gridViewBeforCompond.Columns["FromAccountID"].Width = 120;
            gridViewBeforCompond.Columns["EmpCompondID"].Width = 120;
            gridViewBeforCompond.Columns["ComStoneNumin"].Visible = false;
            gridViewBeforCompond.Columns["ComWeightStonin"].Visible = false;
            gridViewBeforCompond.Columns["ComStoneNumout"].Visible = false;
            gridViewBeforCompond.Columns["ComWeightStonOUt"].Visible = false;
            gridViewBeforCompond.Columns["FromAccountID"].Visible = false;
            gridViewBeforCompond.Columns["EmpCompondID"].Visible = false;
            gridViewBeforCompond.Columns["FromAccountName"].Visible = false;
            gridViewBeforCompond.Columns["EmpCompundName"].Visible = false;
            gridViewBeforCompond.Columns["ComStoneNumlas"].Visible = false;
            gridViewBeforCompond.Columns["ComWeightStonLas"].Visible = false;
            gridViewBeforCompond.Columns["TypeSton"].Visible = false;
            gridViewBeforCompond.Columns["ShownInNext"].Visible = false;

            gridViewBeforCompond.Columns["ComWeightStonAfter"].Visible = false;
            // بيانات الذهب
            gridViewBeforCompond.Columns["GoldDebit"].Visible = false;
            gridViewBeforCompond.Columns["GoldCredit"].Visible = false;
            //الاحجار المسلمة
            gridViewBeforCompond.Columns["ComStoneNumin"].Visible = false;
            gridViewBeforCompond.Columns["ComWeightStonin"].Visible = false;

            //الاحجار المرجعة
            gridViewBeforCompond.Columns["ComStoneNumout"].Visible = false;
            gridViewBeforCompond.Columns["ComWeightStonOUt"].Visible = false;

            //الاحجار الفاقدة
            gridViewBeforCompond.Columns["ComStoneNumlas"].Visible = false;
            gridViewBeforCompond.Columns["ComWeightStonLas"].Visible = false;
            //احجار مركبة
            gridViewBeforCompond.Columns["ComStoneCom"].Visible = true;
            gridViewBeforCompond.Columns["ComWeightSton"].Visible = true;


            if (UserInfo.Language == iLanguage.Arabic)
            {

                gridViewBeforCompond.Columns["EngSizeName"].Visible = false;
                gridViewBeforCompond.Columns["EngItemName"].Visible = false;
                gridViewBeforCompond.Columns["SizeID"].Caption = "رقم الوحده";
                gridViewBeforCompond.Columns[SizeName].Caption = "الوحده";
                gridViewBeforCompond.Columns["ItemID"].Caption = "رقم الصنف";
                gridViewBeforCompond.Columns["BarcodCompond"].Caption = "الكود";
                gridViewBeforCompond.Columns["TypeSton"].Caption = "نوع الحجر ";
                gridViewBeforCompond.Columns[ItemName].Caption = "اسم الصنف";
                gridViewBeforCompond.Columns["CostPrice"].Caption = "سعر التكلفة";
                gridViewBeforCompond.Columns["FromAccountName"].Caption = "اسم الحساب ";
                gridViewBeforCompond.Columns["EmpCompundName"].Caption = "اسم المركب ";
                // بيانات الذهب
                gridViewBeforCompond.Columns["GoldDebit"].Caption = "مسلم";
                gridViewBeforCompond.Columns["GoldCredit"].Caption = "الوزن ";
                //الاحجار المسلمة
                gridViewBeforCompond.Columns["ComStoneNumin"].Caption = "عدد";
                gridViewBeforCompond.Columns["ComWeightStonin"].Caption = "الوزن";

                //الاحجار المرجعة
                gridViewBeforCompond.Columns["ComStoneNumout"].Caption = "عدد";
                gridViewBeforCompond.Columns["ComWeightStonOUt"].Caption = "الوزن";

                //الاحجار الفاقدة
                gridViewBeforCompond.Columns["ComStoneNumlas"].Caption = "عدد";
                gridViewBeforCompond.Columns["ComWeightStonLas"].Caption = "الوزن";
                //احجار مركبة
                gridViewBeforCompond.Columns["ComStoneCom"].Caption = "عدد";
                gridViewBeforCompond.Columns["ComWeightSton"].Caption = "الوزن";


                gridViewBeforCompond.Columns["ComWeightStonAfter"].Caption = "الوزن بعد";
                gridViewBeforCompond.Columns["FromAccountID"].Caption = "من حساب";

                gridViewBeforCompond.Columns["EmpCompondID"].Caption = "رقم المركب";
                gridViewBeforCompond.Columns["ComSignature"].Caption = "التوقيع";
                gridViewBeforCompond.Columns["SalePrice"].Caption = "سعر البيع";
                gridViewBeforCompond.Columns["DebitDate"].Caption = "التاريخ";
                gridViewBeforCompond.Columns["DebitTime"].Caption = "الوقت";
            }
            else
            {
                gridViewBeforCompond.Columns["ArbSizeName"].Visible = false;
                gridViewBeforCompond.Columns["ArbItemName"].Visible = false;
                gridViewBeforCompond.Columns["SizeID"].Caption = "Size ID";
                gridViewBeforCompond.Columns[SizeName].Caption = "Size Name";
                gridViewBeforCompond.Columns["ItemID"].Caption = "Item ID";
                gridViewBeforCompond.Columns["BarcodCompond"].Caption = "Barcod Compond";
                gridViewBeforCompond.Columns["TypeSton"].Caption = "Type Stone";
                gridViewBeforCompond.Columns[ItemName].Caption = "Item Name";
                gridViewBeforCompond.Columns["CostPrice"].Caption = "Cost Price";
                gridViewBeforCompond.Columns["FromAccountName"].Caption = "Acount Name";
                gridViewBeforCompond.Columns["EmpCompundName"].Caption = "Compund Name";
                // بيانات الذهب
                gridViewBeforCompond.Columns["GoldDebit"].Caption = "Debit";
                gridViewBeforCompond.Columns["GoldCredit"].Caption = "Credit";
                //الاحجار المسلمة
                gridViewBeforCompond.Columns["ComStoneNumin"].Caption = "Count";
                gridViewBeforCompond.Columns["ComWeightStonin"].Caption = "Weight";

                //الاحجار المرجعة
                gridViewBeforCompond.Columns["ComStoneNumout"].Caption = "Count";
                gridViewBeforCompond.Columns["ComWeightStonOUt"].Caption = "Weight";

                //الاحجار الفاقدة
                gridViewBeforCompond.Columns["ComStoneNumlas"].Caption = "Count";
                gridViewBeforCompond.Columns["ComWeightStonLas"].Caption = "Weight";
                //احجار مركبة
                gridViewBeforCompond.Columns["ComStoneCom"].Caption = "Count";
                gridViewBeforCompond.Columns["ComWeightSton"].Caption = "Weight";
                gridViewBeforCompond.Columns["ComWeightStonAfter"].Caption = "Weight After";
                gridViewBeforCompond.Columns["FromAccountID"].Caption = "From Account";

                gridViewBeforCompond.Columns["EmpCompondID"].Caption = "Compond ID";
                gridViewBeforCompond.Columns["ComSignature"].Caption = "Signature";
                gridViewBeforCompond.Columns["SalePrice"].Caption = "Sale Price";
                gridViewBeforCompond.Columns["DebitDate"].Caption = "Date";
                gridViewBeforCompond.Columns["DebitTime"].Caption = "Time";
            }

        }
        void initGridAfterCompent()
        {
            lstDetailAfterCompund = new BindingList<Menu_FactoryRunCommandCompund>();
            lstDetailAfterCompund.AllowNew = true;
            lstDetailAfterCompund.AllowEdit = true;
            lstDetailAfterCompund.AllowRemove = true;
            gridControlAfterCompond.DataSource = lstDetailAfterCompund;

            gridViewAfterCompond.Columns["ID"].Visible = false;
            DataTable dtEmp = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM HR_EmployeeFile WHERE  Cancel =0  and BranchID=" + Comon.cInt(cmbBranchesID.EditValue) );
            string[] EmpName = new string[dtEmp.Rows.Count];
            for (int i = 0; i <= dtEmp.Rows.Count - 1; i++)
                EmpName[i] = dtEmp.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems3 = new RepositoryItemComboBox();
            riComboBoxitems3.Items.AddRange(EmpName);
            gridControlAfterCompond.RepositoryItems.Add(riComboBoxitems3);
            gridViewAfterCompond.Columns["EmpCompundName"].ColumnEdit = riComboBoxitems3;
            RepositoryItemLookUpEdit rAccountName = Common.LookUpEditAccountName();
            gridControlAfterCompond.RepositoryItems.Add(rAccountName);
            gridViewAfterCompond.Columns["FromAccountName"].ColumnEdit = rAccountName;


            DataTable dtItemname = Lip.SelectRecord("Select " + PrimaryName + " from Stc_Items  Where Cancel=0 and BranchID=" + Comon.cInt(cmbBranchesID.EditValue) );
            string[] ItemNames = new string[dtItemname.Rows.Count];
            for (int i = 0; i <= dtItemname.Rows.Count - 1; i++)
                ItemNames[i] = dtItemname.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems4 = new RepositoryItemComboBox();
            riComboBoxitems4.Items.AddRange(ItemNames);
            gridControlAfterCompond.RepositoryItems.Add(riComboBoxitems4);
            gridViewAfterCompond.Columns[ItemName].ColumnEdit = riComboBoxitems4;

            gridViewAfterCompond.Columns["ComSignature"].Visible = false;
            gridViewAfterCompond.Columns["CostPrice"].Visible = false;
            gridViewAfterCompond.Columns["DebitTime"].Visible = false;
            gridViewAfterCompond.Columns["ComandID"].Visible = false;
            gridViewAfterCompond.Columns["Cancel"].Visible = false;
            gridViewAfterCompond.Columns["BranchID"].Visible = false;
            gridViewAfterCompond.Columns["FacilityID"].Visible = false;
            gridViewAfterCompond.Columns["SizeID"].Visible = false;
            gridViewAfterCompond.Columns["RegTime"].Visible = false;
            gridViewAfterCompond.Columns["RegDate"].Visible = false;
            gridViewAfterCompond.Columns["InvoiceImage"].Visible = false;
            gridViewAfterCompond.Columns["TypeID"].Visible = false;

            gridViewAfterCompond.Columns["EditUserID"].Visible = false;
            gridViewAfterCompond.Columns["EditDate"].Visible = false;
            gridViewAfterCompond.Columns["EditTime"].Visible = false;
            gridViewAfterCompond.Columns["UserID"].Visible = false;
            gridViewAfterCompond.Columns["TypeOpration"].Visible = false;
            gridViewAfterCompond.Columns["ComputerInfo"].Visible = false;
            gridViewAfterCompond.Columns["EditComputerInfo"].Visible = false;
            gridViewAfterCompond.Columns["GoldCompundNet"].Visible = false;

            gridViewAfterCompond.Columns["FromAccountID"].Name = "FromAccountID";
            gridViewAfterCompond.Columns["BarcodCompond"].Name = "BarcodCompond";
            gridViewAfterCompond.Columns["EmpCompondID"].Name = "EmpCompondID";
            gridViewAfterCompond.Columns["EmpCompundName"].Width = 120;
            gridViewAfterCompond.Columns["FromAccountName"].Width = 120;
            gridViewAfterCompond.Columns["ComSignature"].Width = 45;
            gridViewAfterCompond.Columns["GoldDebit"].Visible = false;
            gridViewAfterCompond.Columns["FromAccountID"].Width = 120;
            gridViewAfterCompond.Columns["EmpCompondID"].Width = 120;
            gridViewAfterCompond.Columns["ComStoneNumin"].Visible = false;
            gridViewAfterCompond.Columns["ComWeightStonin"].Visible = false;
            gridViewAfterCompond.Columns["ComStoneNumout"].Visible = false;
            gridViewAfterCompond.Columns["ComWeightStonOUt"].Visible = false;
            gridViewAfterCompond.Columns["FromAccountID"].Visible = false;
            gridViewAfterCompond.Columns["EmpCompondID"].Visible = false;
            gridViewAfterCompond.Columns["FromAccountName"].Visible = false;
            gridViewAfterCompond.Columns["EmpCompundName"].Visible = false;
            gridViewAfterCompond.Columns["ComStoneNumlas"].Visible = false;
            gridViewAfterCompond.Columns["ComWeightStonLas"].Visible = false;
            gridViewAfterCompond.Columns["TypeSton"].Visible = false;


            gridViewAfterCompond.Columns["ComWeightStonAfter"].Visible = false;
            // بيانات الذهب
            gridViewAfterCompond.Columns["GoldDebit"].Visible = false;
            gridViewAfterCompond.Columns["GoldCredit"].Visible = false;
            //الاحجار المسلمة
            gridViewAfterCompond.Columns["ComStoneNumin"].Visible = false;
            gridViewAfterCompond.Columns["ComWeightStonin"].Visible = false;

            //الاحجار المرجعة
            gridViewAfterCompond.Columns["ComStoneNumout"].Visible = false;
            gridViewAfterCompond.Columns["ComWeightStonOUt"].Visible = false;

            //الاحجار الفاقدة
            gridViewAfterCompond.Columns["ComStoneNumlas"].Visible = false;
            gridViewAfterCompond.Columns["ComWeightStonLas"].Visible = false;
            //احجار مركبة
            gridViewAfterCompond.Columns["ComStoneCom"].Visible = true;
            gridViewAfterCompond.Columns["ComWeightSton"].Visible = true;


            if (UserInfo.Language == iLanguage.Arabic)
            {

                gridViewAfterCompond.Columns["EngSizeName"].Visible = false;
                gridViewAfterCompond.Columns["EngItemName"].Visible = false;
                gridViewAfterCompond.Columns["SizeID"].Caption = "رقم الوحده";
                gridViewAfterCompond.Columns[SizeName].Caption = "الوحده";
                gridViewAfterCompond.Columns["ItemID"].Caption = "رقم الصنف";
                gridViewAfterCompond.Columns["BarcodCompond"].Caption = "الكود";
                gridViewAfterCompond.Columns["TypeSton"].Caption = "نوع الحجر ";
                gridViewAfterCompond.Columns[ItemName].Caption = "اسم الصنف";
                gridViewAfterCompond.Columns["CostPrice"].Caption = "سعر التكلفة";
                gridViewAfterCompond.Columns["FromAccountName"].Caption = "اسم الحساب ";
                gridViewAfterCompond.Columns["EmpCompundName"].Caption = "اسم المركب ";
                // بيانات الذهب
                gridViewAfterCompond.Columns["GoldDebit"].Caption = "مسلم";
                gridViewAfterCompond.Columns["GoldCredit"].Caption = "الوزن ";
                //الاحجار المسلمة
                gridViewAfterCompond.Columns["ComStoneNumin"].Caption = "عدد";
                gridViewAfterCompond.Columns["ComWeightStonin"].Caption = "الوزن";

                //الاحجار المرجعة
                gridViewAfterCompond.Columns["ComStoneNumout"].Caption = "عدد";
                gridViewAfterCompond.Columns["ComWeightStonOUt"].Caption = "الوزن";

                //الاحجار الفاقدة
                gridViewAfterCompond.Columns["ComStoneNumlas"].Caption = "عدد";
                gridViewAfterCompond.Columns["ComWeightStonLas"].Caption = "الوزن";
                //احجار مركبة
                gridViewAfterCompond.Columns["ComStoneCom"].Caption = "عدد";
                gridViewAfterCompond.Columns["ComWeightSton"].Caption = "الوزن";
                gridViewAfterCompond.Columns["ComWeightStonAfter"].Caption = "الوزن بعد";
                gridViewAfterCompond.Columns["FromAccountID"].Caption = "من حساب";

                gridViewAfterCompond.Columns["EmpCompondID"].Caption = "رقم المركب";
                gridViewAfterCompond.Columns["ComSignature"].Caption = "التوقيع";
                gridViewAfterCompond.Columns["SalePrice"].Caption = "سعر البيع";
                gridViewAfterCompond.Columns["DebitDate"].Caption = "التاريخ";
                gridViewAfterCompond.Columns["DebitTime"].Caption = "الوقت";
                gridViewAfterCompond.Columns["ShownInNext"].Caption = "يظهر في التفاصيل ";
            }
            else
            {
                gridViewAfterCompond.Columns["ArbSizeName"].Visible = false;
                gridViewAfterCompond.Columns["ArbItemName"].Visible = false;
                gridViewAfterCompond.Columns["SizeID"].Caption = "Size ID";
                gridViewAfterCompond.Columns[SizeName].Caption = "Size Name";
                gridViewAfterCompond.Columns["ItemID"].Caption = "Item ID";
                gridViewAfterCompond.Columns["BarcodCompond"].Caption = "Barcod Compond";
                gridViewAfterCompond.Columns["TypeSton"].Caption = "Type Stone";
                gridViewAfterCompond.Columns[ItemName].Caption = "Item Name";
                gridViewAfterCompond.Columns["CostPrice"].Caption = "Cost Price";
                gridViewAfterCompond.Columns["FromAccountName"].Caption = "Acount Name";
                gridViewAfterCompond.Columns["EmpCompundName"].Caption = "Compund Name";
                gridViewAfterCompond.Columns["ShownInNext"].Caption = "Shown In Next ";
                // بيانات الذهب
                gridViewAfterCompond.Columns["GoldDebit"].Caption = "Debit";
                gridViewAfterCompond.Columns["GoldCredit"].Caption = "Credit";
                //الاحجار المسلمة
                gridViewAfterCompond.Columns["ComStoneNumin"].Caption = "Count";
                gridViewAfterCompond.Columns["ComWeightStonin"].Caption = "Weight";

                //الاحجار المرجعة
                gridViewAfterCompond.Columns["ComStoneNumout"].Caption = "Count";
                gridViewAfterCompond.Columns["ComWeightStonOUt"].Caption = "Weight";

                //الاحجار الفاقدة
                gridViewAfterCompond.Columns["ComStoneNumlas"].Caption = "Count";
                gridViewAfterCompond.Columns["ComWeightStonLas"].Caption = "Weight";
                //احجار مركبة
                gridViewAfterCompond.Columns["ComStoneCom"].Caption = "Count";
                gridViewAfterCompond.Columns["ComWeightSton"].Caption = "Weight";
                gridViewAfterCompond.Columns["ComWeightStonAfter"].Caption = "Weight After";
                gridViewAfterCompond.Columns["FromAccountID"].Caption = "From Account";

                gridViewAfterCompond.Columns["EmpCompondID"].Caption = "Compond ID";
                gridViewAfterCompond.Columns["ComSignature"].Caption = "Signature";
                gridViewAfterCompond.Columns["SalePrice"].Caption = "Sale Price";
                gridViewAfterCompond.Columns["DebitDate"].Caption = "Date";
                gridViewAfterCompond.Columns["DebitTime"].Caption = "Time";
            }

        }
        void initGridBeforTalmee()
        {
            lstDetailTalmee1 = new BindingList<Menu_FactoryRunCommandTalmee>();
            lstDetailTalmee1.AllowNew = true;
            lstDetailTalmee1.AllowEdit = true;
            lstDetailTalmee1.AllowRemove = true;
            gridControlBeforePolishing.DataSource = lstDetailTalmee1;

            DataTable dtitems = Lip.SelectRecord("SELECT   ArbName   FROM Menu_FactoryMachine ");
            string[] NameMachine = new string[dtitems.Rows.Count];
            for (int i = 0; i <= dtitems.Rows.Count - 1; i++)
                NameMachine[i] = dtitems.Rows[i]["ArbName"].ToString();
            RepositoryItemComboBox riComboBoxitems = new RepositoryItemComboBox();
            riComboBoxitems.Items.AddRange(NameMachine);
            gridControlBeforePolishing.RepositoryItems.Add(riComboBoxitems);
            DataTable dtStore = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM Stc_Stores WHERE  Cancel =0  and BranchID=" + Comon.cInt(cmbBranchesID.EditValue) );
            string[] StoreName = new string[dtStore.Rows.Count];
            for (int i = 0; i <= dtStore.Rows.Count - 1; i++)
                StoreName[i] = dtStore.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems2 = new RepositoryItemComboBox();

            riComboBoxitems2.Items.AddRange(StoreName);
            gridControlBeforePolishing.RepositoryItems.Add(riComboBoxitems2);
            GridViewBeforPolish.Columns["StoreName"].ColumnEdit = riComboBoxitems2;


            DataTable dtEmp = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM HR_EmployeeFile WHERE  Cancel =0  and BranchID=" + Comon.cInt(cmbBranchesID.EditValue) );
            string[] EmpName = new string[dtEmp.Rows.Count];
            for (int i = 0; i <= dtEmp.Rows.Count - 1; i++)
                EmpName[i] = dtEmp.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems3 = new RepositoryItemComboBox();
            riComboBoxitems3.Items.AddRange(EmpName);
            gridControlBeforePolishing.RepositoryItems.Add(riComboBoxitems3);
            GridViewBeforPolish.Columns["EmpName"].ColumnEdit = riComboBoxitems3;

            DataTable dtItemname = Lip.SelectRecord("Select " + PrimaryName + " from Stc_Items  Where Cancel=0 and BranchID=" + Comon.cInt(cmbBranchesID.EditValue) );
            string[] ItemNames = new string[dtItemname.Rows.Count];
            for (int i = 0; i <= dtItemname.Rows.Count - 1; i++)
                ItemNames[i] = dtItemname.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems4 = new RepositoryItemComboBox();
            riComboBoxitems4.Items.AddRange(ItemNames);
            gridControlBeforePolishing.RepositoryItems.Add(riComboBoxitems4);
            GridViewBeforPolish.Columns[ItemName].ColumnEdit = riComboBoxitems4;

            GridViewBeforPolish.Columns["MachineName"].ColumnEdit = riComboBoxitems;
            GridViewBeforPolish.Columns["ID"].Visible = false;
            GridViewBeforPolish.Columns["ComandID"].Visible = false;
            GridViewBeforPolish.Columns["BarcodeTalmee"].Visible = false;
            GridViewBeforPolish.Columns["EmpPolishnID"].Visible = false;
            GridViewBeforPolish.Columns["EmpPrentagID"].Visible = false;
            GridViewBeforPolish.Columns["Cancel"].Visible = false;
            GridViewBeforPolish.Columns["BranchID"].Visible = false;
            GridViewBeforPolish.Columns["FacilityID"].Visible = false;

            GridViewBeforPolish.Columns["EditUserID"].Visible = false;
            GridViewBeforPolish.Columns["EditDate"].Visible = false;
            GridViewBeforPolish.Columns["EditTime"].Visible = false;
            GridViewBeforPolish.Columns["RegDate"].Visible = false;
            GridViewBeforPolish.Columns["UserID"].Visible = false;

            GridViewBeforPolish.Columns["ComputerInfo"].Visible = false;
            GridViewBeforPolish.Columns["EditComputerInfo"].Visible = false;
            GridViewBeforPolish.Columns["RegTime"].Visible = false;

            GridViewBeforPolish.Columns["Credit"].Visible = false;
            GridViewBeforPolish.Columns["TypeOpration"].Visible = false;
            //GridViewBeforPolish.Columns["SizeID"].Visible = false;
            GridViewBeforPolish.Columns["CostPrice"].Visible = false;
            GridViewBeforPolish.Columns["SizeID"].Visible = false;
            // GridViewBeforPolish.Columns["PrentagDebitTime"].Visible = false;
            GridViewBeforPolish.Columns["MachinID"].Name = "MachinID";
            //dtItem.Columns["Losed"].ReadOnly = true;
            GridViewBeforPolish.Columns["EmpName"].Width = 120;
            GridViewBeforPolish.Columns["StoreName"].Width = 120;
            GridViewBeforPolish.Columns["EmpID"].Width = 120;
            GridViewBeforPolish.Columns["Signature"].Width = 120;
            GridViewBeforPolish.Columns["DebitDate"].Width = 110;
            GridViewBeforPolish.Columns["DebitTime"].Width = 85;
            GridViewBeforPolish.Columns["EmpID"].Visible = false;
            GridViewBeforPolish.Columns["MachineName"].Visible = false;
            GridViewBeforPolish.Columns["MachinID"].Visible = false;
            GridViewBeforPolish.Columns["StoreID"].Visible = false;
            GridViewBeforPolish.Columns["StoreID"].Visible = false;

            GridViewBeforPolish.Columns["Signature"].Visible = false;
            GridViewBeforPolish.Columns["DebitTime"].Visible = false;
            GridViewBeforPolish.Columns["ShownInNext"].Visible = false;
            if (UserInfo.Language == iLanguage.Arabic)
            {
                GridViewBeforPolish.Columns["EngItemName"].Visible = false;
                GridViewBeforPolish.Columns["EngSizeName"].Visible = false;
                GridViewBeforPolish.Columns["ArbItemName"].Width = 150;
                GridViewBeforPolish.Columns["StoreID"].Caption = "رقم المخزن";
                GridViewBeforPolish.Columns["StoreName"].Caption = "إسم المخزن";
                GridViewBeforPolish.Columns["EmpID"].Caption = "رقم العامل";
                GridViewBeforPolish.Columns["EmpName"].Caption = "إسم العامل";
                GridViewBeforPolish.Columns["MachinID"].Caption = "رقم المكينة";
                GridViewBeforPolish.Columns["MachineName"].Caption = "إسم المكينة";
                GridViewBeforPolish.Columns["Debit"].Caption = "الوزن";
                GridViewBeforPolish.Columns["Credit"].Caption = "دائــن";
                GridViewBeforPolish.Columns["TypeOpration"].Caption = "نوع العملية";
                GridViewBeforPolish.Columns["Signature"].Caption = "التوقيع";
                GridViewBeforPolish.Columns["ItemID"].Caption = "رقم الصنف";
                GridViewBeforPolish.Columns["ArbItemName"].Caption = "اسم الصنف";
                GridViewBeforPolish.Columns["SizeID"].Caption = "رقم الوحده";
                GridViewBeforPolish.Columns["ArbSizeName"].Caption = "الوحده";
                GridViewBeforPolish.Columns["CostPrice"].Caption = "التكلفة";
                GridViewBeforPolish.Columns["DebitDate"].Caption = "التاريخ";
                GridViewBeforPolish.Columns["DebitTime"].Caption = "الوقت";
            }
            else
            {
                GridViewBeforPolish.Columns["ArbItemName"].Visible = false;
                GridViewBeforPolish.Columns["ArbSizeName"].Visible = false;
                GridViewBeforPolish.Columns["EngItemName"].Width = 150;
                GridViewBeforPolish.Columns["StoreID"].Caption = "Store ID";
                GridViewBeforPolish.Columns["StoreName"].Caption = "Store Name";
                GridViewBeforPolish.Columns["EngItemName"].Caption = "Item Name";
                GridViewBeforPolish.Columns["MachinID"].Caption = "Machine ID";
                GridViewBeforPolish.Columns["MachineName"].Caption = "Machin Name";
                GridViewBeforPolish.Columns["Debit"].Caption = "debtor ";
                GridViewBeforPolish.Columns["EngSizeName"].Caption = "Unit";
                GridViewBeforPolish.Columns["Credit"].Caption = "Creditor";
                GridViewBeforPolish.Columns["TypeOpration"].Caption = "Type Opration";
                GridViewBeforPolish.Columns["Signature"].Caption = "Signature";
                GridViewBeforPolish.Columns["DebitDate"].Caption = "Date";
                GridViewBeforPolish.Columns["DebitTime"].Caption = "Time";
                GridViewBeforPolish.Columns["EmpID"].Caption = "EmpID";
                GridViewBeforPolish.Columns["EmpName"].Caption = "Name";
            }



        }
        void initGridAfterTalmee()
        {

            lstDetailAfterTalmee1 = new BindingList<Menu_FactoryRunCommandTalmee>();
            lstDetailAfterTalmee1.AllowNew = true;
            lstDetailAfterTalmee1.AllowEdit = true;
            lstDetailAfterTalmee1.AllowRemove = true;
            gridControlAfterPolishing.DataSource = lstDetailAfterTalmee1;

            DataTable dtitems = Lip.SelectRecord("SELECT   ArbName   FROM Menu_FactoryMachine ");
            string[] NameMachine = new string[dtitems.Rows.Count];
            for (int i = 0; i <= dtitems.Rows.Count - 1; i++)
                NameMachine[i] = dtitems.Rows[i]["ArbName"].ToString();

            RepositoryItemComboBox riComboBoxitems = new RepositoryItemComboBox();
            riComboBoxitems.Items.AddRange(NameMachine);
            gridControlAfterPolishing.RepositoryItems.Add(riComboBoxitems);

            DataTable dtStore = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM Stc_Stores WHERE  Cancel =0  and BranchID=" + Comon.cInt(cmbBranchesID.EditValue) );
            string[] StoreName = new string[dtStore.Rows.Count];
            for (int i = 0; i <= dtStore.Rows.Count - 1; i++)
                StoreName[i] = dtStore.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems2 = new RepositoryItemComboBox();
            riComboBoxitems2.Items.AddRange(StoreName);
            gridControlAfterPolishing.RepositoryItems.Add(riComboBoxitems2);
            GridViewAfterPolish.Columns["StoreName"].ColumnEdit = riComboBoxitems2;

            DataTable dtEmp = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM HR_EmployeeFile WHERE  Cancel =0 and  BranchID=" + Comon.cInt(cmbBranchesID.EditValue));
            string[] EmpName = new string[dtEmp.Rows.Count];
            for (int i = 0; i <= dtEmp.Rows.Count - 1; i++)
                EmpName[i] = dtEmp.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems3 = new RepositoryItemComboBox();
            riComboBoxitems3.Items.AddRange(EmpName);
            gridControlAfterPolishing.RepositoryItems.Add(riComboBoxitems3);
            GridViewAfterPolish.Columns["EmpName"].ColumnEdit = riComboBoxitems3;

            DataTable dtItemname = Lip.SelectRecord("Select " + PrimaryName + " from Stc_Items  Where Cancel=0 and  BranchID=" + Comon.cInt(cmbBranchesID.EditValue));
            string[] ItemNames = new string[dtItemname.Rows.Count];
            for (int i = 0; i <= dtItemname.Rows.Count - 1; i++)
                ItemNames[i] = dtItemname.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems4 = new RepositoryItemComboBox();
            riComboBoxitems4.Items.AddRange(ItemNames);
            gridControlAfterPolishing.RepositoryItems.Add(riComboBoxitems4);
            GridViewAfterPolish.Columns[ItemName].ColumnEdit = riComboBoxitems4;
            GridViewAfterPolish.Columns["SizeID"].Visible = false;
            GridViewAfterPolish.Columns["MachineName"].ColumnEdit = riComboBoxitems;
            GridViewAfterPolish.Columns["ID"].Visible = false;
            GridViewAfterPolish.Columns["ComandID"].Visible = false;
            GridViewAfterPolish.Columns["BarcodeTalmee"].Visible = false;
            GridViewAfterPolish.Columns["EmpPolishnID"].Visible = false;
            GridViewAfterPolish.Columns["EmpPrentagID"].Visible = false;
            GridViewAfterPolish.Columns["Cancel"].Visible = false;
            GridViewAfterPolish.Columns["BranchID"].Visible = false;
            GridViewAfterPolish.Columns["FacilityID"].Visible = false;

            GridViewAfterPolish.Columns["EditUserID"].Visible = false;
            GridViewAfterPolish.Columns["EditDate"].Visible = false;
            GridViewAfterPolish.Columns["EditTime"].Visible = false;
            GridViewAfterPolish.Columns["RegDate"].Visible = false;
            GridViewAfterPolish.Columns["UserID"].Visible = false;

            GridViewAfterPolish.Columns["ComputerInfo"].Visible = false;
            GridViewAfterPolish.Columns["EditComputerInfo"].Visible = false;
            GridViewAfterPolish.Columns["RegTime"].Visible = false;

            GridViewAfterPolish.Columns["Debit"].Visible = false;
            GridViewAfterPolish.Columns["TypeOpration"].Visible = false;
            //GridViewAfterPolish.Columns["SizeID"].Visible = false;
            GridViewAfterPolish.Columns["CostPrice"].Visible = false;

            // GridViewAfterPolish.Columns["PrentagDebitTime"].Visible = false;
            GridViewAfterPolish.Columns["MachinID"].Name = "MachinID";
            //dtItem.Columns["Losed"].ReadOnly = true;
            GridViewAfterPolish.Columns["EmpName"].Width = 120;
            GridViewAfterPolish.Columns["StoreName"].Width = 120;
            GridViewAfterPolish.Columns["EmpID"].Width = 120;
            GridViewAfterPolish.Columns["Signature"].Width = 120;
            GridViewAfterPolish.Columns["DebitDate"].Width = 110;
            GridViewAfterPolish.Columns["DebitTime"].Width = 85;
            GridViewAfterPolish.Columns["EmpID"].Visible = false;
            GridViewAfterPolish.Columns["MachineName"].Visible = false;
            GridViewAfterPolish.Columns["MachinID"].Visible = false;
            GridViewAfterPolish.Columns["StoreID"].Visible = false;
            GridViewAfterPolish.Columns["Signature"].Visible = false;
            GridViewAfterPolish.Columns["DebitTime"].Visible = false;
            if (UserInfo.Language == iLanguage.Arabic)
            {
                GridViewAfterPolish.Columns["EngItemName"].Visible = false;
                GridViewAfterPolish.Columns["EngSizeName"].Visible = false;
                GridViewAfterPolish.Columns["ArbItemName"].Width = 150;
                GridViewAfterPolish.Columns["StoreID"].Caption = "رقم المخزن";
                GridViewAfterPolish.Columns["StoreName"].Caption = "إسم المخزن";

                GridViewAfterPolish.Columns["EmpID"].Caption = "رقم العامل";
                GridViewAfterPolish.Columns["EmpName"].Caption = "إسم العامل";

                GridViewAfterPolish.Columns["MachinID"].Caption = "رقم المكينة";
                GridViewAfterPolish.Columns["MachineName"].Caption = "إسم المكينة";
                GridViewAfterPolish.Columns["Debit"].Caption = "الوزن";

                GridViewAfterPolish.Columns["Credit"].Caption = "الوزن";
                GridViewAfterPolish.Columns["TypeOpration"].Caption = "نوع العملية";
                GridViewAfterPolish.Columns["Signature"].Caption = "التوقيع";

                GridViewAfterPolish.Columns["ItemID"].Caption = "رقم الصنف";
                GridViewAfterPolish.Columns["ArbItemName"].Caption = "اسم الصنف";
                GridViewAfterPolish.Columns["SizeID"].Caption = "رقم الوحده";
                GridViewAfterPolish.Columns["ArbSizeName"].Caption = "الوحده";
                GridViewAfterPolish.Columns["CostPrice"].Caption = "التكلفة";
                GridViewAfterPolish.Columns["DebitDate"].Caption = "التاريخ";
                GridViewAfterPolish.Columns["DebitTime"].Caption = "الوقت";
                GridViewAfterPolish.Columns["ShownInNext"].Caption = "يظهر في التفاصيل ";
            }
            else
            {
                GridViewAfterPolish.Columns["ArbItemName"].Visible = false;
                GridViewAfterPolish.Columns["ArbSizeName"].Visible = false;
                GridViewAfterPolish.Columns["EngItemName"].Width = 150;
                GridViewAfterPolish.Columns["StoreID"].Caption = "Store ID";
                GridViewAfterPolish.Columns["StoreName"].Caption = "Store Name";
                GridViewAfterPolish.Columns["EngItemName"].Caption = "Item Name";
                GridViewAfterPolish.Columns["MachinID"].Caption = "Machine ID";
                GridViewAfterPolish.Columns["MachineName"].Caption = "Machin Name";
                GridViewAfterPolish.Columns["Debit"].Caption = "debtor ";
                GridViewAfterPolish.Columns["EngSizeName"].Caption = "Unit";
                GridViewAfterPolish.Columns["Credit"].Caption = "Creditor";
                GridViewAfterPolish.Columns["TypeOpration"].Caption = "Type Opration";
                GridViewAfterPolish.Columns["Signature"].Caption = "Signature";
                GridViewAfterPolish.Columns["DebitDate"].Caption = "Date";
                GridViewAfterPolish.Columns["DebitDate"].Caption = "Time";
                GridViewAfterPolish.Columns["EmpID"].Caption = "EmpID";
                GridViewAfterPolish.Columns["EmpName"].Caption = "Name";
                GridViewAfterPolish.Columns["ShownInNext"].Caption = "Shown In Next ";
            }
        }

        void initGridBeforTalmee2()
        {
            lstDetailTalmee2 = new BindingList<Menu_FactoryRunCommandTalmee>();
            lstDetailTalmee2.AllowNew = true;
            lstDetailTalmee2.AllowEdit = true;
            lstDetailTalmee2.AllowRemove = true;
            gridControl5.DataSource = lstDetailTalmee2;

            DataTable dtitems = Lip.SelectRecord("SELECT   ArbName   FROM Menu_FactoryMachine ");
            string[] NameMachine = new string[dtitems.Rows.Count];
            for (int i = 0; i <= dtitems.Rows.Count - 1; i++)
                NameMachine[i] = dtitems.Rows[i]["ArbName"].ToString();
            RepositoryItemComboBox riComboBoxitems = new RepositoryItemComboBox();
            riComboBoxitems.Items.AddRange(NameMachine);
            gridControl5.RepositoryItems.Add(riComboBoxitems);
            DataTable dtStore = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM Stc_Stores WHERE  Cancel =0 and BranchID=" + Comon.cInt(cmbBranchesID.EditValue) );
            string[] StoreName = new string[dtStore.Rows.Count];
            for (int i = 0; i <= dtStore.Rows.Count - 1; i++)
                StoreName[i] = dtStore.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems2 = new RepositoryItemComboBox();

            riComboBoxitems2.Items.AddRange(StoreName);
            gridControl5.RepositoryItems.Add(riComboBoxitems2);
            gridView18.Columns["StoreName"].ColumnEdit = riComboBoxitems2;


            DataTable dtEmp = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM HR_EmployeeFile WHERE  Cancel =0  and BranchID=" + Comon.cInt(cmbBranchesID.EditValue));
            string[] EmpName = new string[dtEmp.Rows.Count];
            for (int i = 0; i <= dtEmp.Rows.Count - 1; i++)
                EmpName[i] = dtEmp.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems3 = new RepositoryItemComboBox();
            riComboBoxitems3.Items.AddRange(EmpName);
            gridControl5.RepositoryItems.Add(riComboBoxitems3);
            gridView18.Columns["EmpName"].ColumnEdit = riComboBoxitems3;

            DataTable dtItemname = Lip.SelectRecord("Select " + PrimaryName + " from Stc_Items  Where Cancel=0 and BranchID=" + Comon.cInt(cmbBranchesID.EditValue) );
            string[] ItemNames = new string[dtItemname.Rows.Count];
            for (int i = 0; i <= dtItemname.Rows.Count - 1; i++)
                ItemNames[i] = dtItemname.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems4 = new RepositoryItemComboBox();
            riComboBoxitems4.Items.AddRange(ItemNames);
            gridControl5.RepositoryItems.Add(riComboBoxitems4);
            gridView18.Columns[ItemName].ColumnEdit = riComboBoxitems4;

            gridView18.Columns["MachineName"].ColumnEdit = riComboBoxitems;
            gridView18.Columns["ID"].Visible = false;
            gridView18.Columns["ComandID"].Visible = false;
            gridView18.Columns["BarcodeTalmee"].Visible = false;
            gridView18.Columns["EmpPolishnID"].Visible = false;
            gridView18.Columns["EmpPrentagID"].Visible = false;
            gridView18.Columns["Cancel"].Visible = false;
            gridView18.Columns["BranchID"].Visible = false;
            gridView18.Columns["FacilityID"].Visible = false;

            gridView18.Columns["EditUserID"].Visible = false;
            gridView18.Columns["EditDate"].Visible = false;
            gridView18.Columns["EditTime"].Visible = false;
            gridView18.Columns["RegDate"].Visible = false;
            gridView18.Columns["UserID"].Visible = false;

            gridView18.Columns["ComputerInfo"].Visible = false;
            gridView18.Columns["EditComputerInfo"].Visible = false;
            gridView18.Columns["RegTime"].Visible = false;

            gridView18.Columns["Credit"].Visible = false;
            gridView18.Columns["TypeOpration"].Visible = false;
            //gridView18.Columns["SizeID"].Visible = false;
            gridView18.Columns["CostPrice"].Visible = false;
            gridView18.Columns["SizeID"].Visible = false;
            // gridView18.Columns["PrentagDebitTime"].Visible = false;
            gridView18.Columns["MachinID"].Name = "MachinID";
            //dtItem.Columns["Losed"].ReadOnly = true;
            gridView18.Columns["EmpName"].Width = 120;
            gridView18.Columns["StoreName"].Width = 120;
            gridView18.Columns["EmpID"].Width = 120;
            gridView18.Columns["Signature"].Width = 120;
            gridView18.Columns["DebitDate"].Width = 110;
            gridView18.Columns["DebitTime"].Width = 85;
            gridView18.Columns["EmpID"].Visible = false;
            gridView18.Columns["MachineName"].Visible = false;
            gridView18.Columns["MachinID"].Visible = false;
            gridView18.Columns["StoreID"].Visible = false;
            gridView18.Columns["StoreID"].Visible = false;

            gridView18.Columns["Signature"].Visible = false;
            gridView18.Columns["DebitTime"].Visible = false;
            gridView18.Columns["ShownInNext"].Visible = false;
            if (UserInfo.Language == iLanguage.Arabic)
            {
                gridView18.Columns["EngItemName"].Visible = false;
                gridView18.Columns["EngSizeName"].Visible = false;
                gridView18.Columns["ArbItemName"].Width = 150;
                gridView18.Columns["StoreID"].Caption = "رقم المخزن";
                gridView18.Columns["StoreName"].Caption = "إسم المخزن";
                gridView18.Columns["EmpID"].Caption = "رقم العامل";
                gridView18.Columns["EmpName"].Caption = "إسم العامل";
                gridView18.Columns["MachinID"].Caption = "رقم المكينة";
                gridView18.Columns["MachineName"].Caption = "إسم المكينة";
                gridView18.Columns["Debit"].Caption = "الوزن";
                gridView18.Columns["Credit"].Caption = "دائــن";
                gridView18.Columns["TypeOpration"].Caption = "نوع العملية";
                gridView18.Columns["Signature"].Caption = "التوقيع";
                gridView18.Columns["ItemID"].Caption = "رقم الصنف";
                gridView18.Columns["ArbItemName"].Caption = "اسم الصنف";
                gridView18.Columns["SizeID"].Caption = "رقم الوحده";
                gridView18.Columns["ArbSizeName"].Caption = "الوحده";
                gridView18.Columns["CostPrice"].Caption = "التكلفة";
                gridView18.Columns["DebitDate"].Caption = "التاريخ";
                gridView18.Columns["DebitTime"].Caption = "الوقت";
            }
            else
            {
                gridView18.Columns["ArbItemName"].Visible = false;
                gridView18.Columns["ArbSizeName"].Visible = false;
                gridView18.Columns["EngItemName"].Width = 150;
                gridView18.Columns["StoreID"].Caption = "Store ID";
                gridView18.Columns["StoreName"].Caption = "Store Name";
                gridView18.Columns["EngItemName"].Caption = "Item Name";
                gridView18.Columns["MachinID"].Caption = "Machine ID";
                gridView18.Columns["MachineName"].Caption = "Machin Name";
                gridView18.Columns["Debit"].Caption = "debtor ";
                gridView18.Columns["EngSizeName"].Caption = "Unit";
                gridView18.Columns["Credit"].Caption = "Creditor";
                gridView18.Columns["TypeOpration"].Caption = "Type Opration";
                gridView18.Columns["Signature"].Caption = "Signature";
                gridView18.Columns["DebitDate"].Caption = "Date";
                gridView18.Columns["DebitTime"].Caption = "Time";
                gridView18.Columns["EmpID"].Caption = "EmpID";
                gridView18.Columns["EmpName"].Caption = "Name";
            }



        }
        void initGridAfterTalmee2()
        {

            lstDetailAfterTalmee2 = new BindingList<Menu_FactoryRunCommandTalmee>();
            lstDetailAfterTalmee2.AllowNew = true;
            lstDetailAfterTalmee2.AllowEdit = true;
            lstDetailAfterTalmee2.AllowRemove = true;
            gridControl4.DataSource = lstDetailAfterTalmee2;

            DataTable dtitems = Lip.SelectRecord("SELECT   ArbName   FROM Menu_FactoryMachine ");
            string[] NameMachine = new string[dtitems.Rows.Count];
            for (int i = 0; i <= dtitems.Rows.Count - 1; i++)
                NameMachine[i] = dtitems.Rows[i]["ArbName"].ToString();

            RepositoryItemComboBox riComboBoxitems = new RepositoryItemComboBox();
            riComboBoxitems.Items.AddRange(NameMachine);
            gridControl4.RepositoryItems.Add(riComboBoxitems);

            DataTable dtStore = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM Stc_Stores WHERE  Cancel =0  and BranchID=" + Comon.cInt(cmbBranchesID.EditValue) );
            string[] StoreName = new string[dtStore.Rows.Count];
            for (int i = 0; i <= dtStore.Rows.Count - 1; i++)
                StoreName[i] = dtStore.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems2 = new RepositoryItemComboBox();
            riComboBoxitems2.Items.AddRange(StoreName);
            gridControl4.RepositoryItems.Add(riComboBoxitems2);
            gridView14.Columns["StoreName"].ColumnEdit = riComboBoxitems2;

            DataTable dtEmp = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM HR_EmployeeFile WHERE  Cancel =0  and BranchID=" + Comon.cInt(cmbBranchesID.EditValue) );
            string[] EmpName = new string[dtEmp.Rows.Count];
            for (int i = 0; i <= dtEmp.Rows.Count - 1; i++)
                EmpName[i] = dtEmp.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems3 = new RepositoryItemComboBox();
            riComboBoxitems3.Items.AddRange(EmpName);
            gridControl4.RepositoryItems.Add(riComboBoxitems3);
            gridView14.Columns["EmpName"].ColumnEdit = riComboBoxitems3;

            DataTable dtItemname = Lip.SelectRecord("Select " + PrimaryName + " from Stc_Items  Where Cancel=0 and BranchID=" + Comon.cInt(cmbBranchesID.EditValue) );
            string[] ItemNames = new string[dtItemname.Rows.Count];
            for (int i = 0; i <= dtItemname.Rows.Count - 1; i++)
                ItemNames[i] = dtItemname.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems4 = new RepositoryItemComboBox();
            riComboBoxitems4.Items.AddRange(ItemNames);
            gridControl4.RepositoryItems.Add(riComboBoxitems4);
            gridView14.Columns[ItemName].ColumnEdit = riComboBoxitems4;
            gridView14.Columns["SizeID"].Visible = false;
            gridView14.Columns["MachineName"].ColumnEdit = riComboBoxitems;
            gridView14.Columns["ID"].Visible = false;
            gridView14.Columns["ComandID"].Visible = false;
            gridView14.Columns["BarcodeTalmee"].Visible = false;
            gridView14.Columns["EmpPolishnID"].Visible = false;
            gridView14.Columns["EmpPrentagID"].Visible = false;
            gridView14.Columns["Cancel"].Visible = false;
            gridView14.Columns["BranchID"].Visible = false;
            gridView14.Columns["FacilityID"].Visible = false;

            gridView14.Columns["EditUserID"].Visible = false;
            gridView14.Columns["EditDate"].Visible = false;
            gridView14.Columns["EditTime"].Visible = false;
            gridView14.Columns["RegDate"].Visible = false;
            gridView14.Columns["UserID"].Visible = false;

            gridView14.Columns["ComputerInfo"].Visible = false;
            gridView14.Columns["EditComputerInfo"].Visible = false;
            gridView14.Columns["RegTime"].Visible = false;

            gridView14.Columns["Debit"].Visible = false;
            gridView14.Columns["TypeOpration"].Visible = false;
            //gridView14.Columns["SizeID"].Visible = false;
            gridView14.Columns["CostPrice"].Visible = false;

            // gridView14.Columns["PrentagDebitTime"].Visible = false;
            gridView14.Columns["MachinID"].Name = "MachinID";
            //dtItem.Columns["Losed"].ReadOnly = true;
            gridView14.Columns["EmpName"].Width = 120;
            gridView14.Columns["StoreName"].Width = 120;
            gridView14.Columns["EmpID"].Width = 120;
            gridView14.Columns["Signature"].Width = 120;
            gridView14.Columns["DebitDate"].Width = 110;
            gridView14.Columns["DebitTime"].Width = 85;
            gridView14.Columns["EmpID"].Visible = false;
            gridView14.Columns["MachineName"].Visible = false;
            gridView14.Columns["MachinID"].Visible = false;
            gridView14.Columns["StoreID"].Visible = false;
            gridView14.Columns["Signature"].Visible = false;
            gridView14.Columns["DebitTime"].Visible = false;
            if (UserInfo.Language == iLanguage.Arabic)
            {
                gridView14.Columns["EngItemName"].Visible = false;
                gridView14.Columns["EngSizeName"].Visible = false;
                gridView14.Columns["ArbItemName"].Width = 150;
                gridView14.Columns["StoreID"].Caption = "رقم المخزن";
                gridView14.Columns["StoreName"].Caption = "إسم المخزن";

                gridView14.Columns["EmpID"].Caption = "رقم العامل";
                gridView14.Columns["EmpName"].Caption = "إسم العامل";

                gridView14.Columns["MachinID"].Caption = "رقم المكينة";
                gridView14.Columns["MachineName"].Caption = "إسم المكينة";
                gridView14.Columns["Debit"].Caption = "الوزن";

                gridView14.Columns["Credit"].Caption = "الوزن";
                gridView14.Columns["TypeOpration"].Caption = "نوع العملية";
                gridView14.Columns["Signature"].Caption = "التوقيع";

                gridView14.Columns["ItemID"].Caption = "رقم الصنف";
                gridView14.Columns["ArbItemName"].Caption = "اسم الصنف";
                gridView14.Columns["SizeID"].Caption = "رقم الوحده";
                gridView14.Columns["ArbSizeName"].Caption = "الوحده";
                gridView14.Columns["CostPrice"].Caption = "التكلفة";
                gridView14.Columns["DebitDate"].Caption = "التاريخ";
                gridView14.Columns["DebitTime"].Caption = "الوقت";
                gridView14.Columns["ShownInNext"].Caption = "يظهر في التفاصيل ";
            }
            else
            {
                gridView14.Columns["ArbItemName"].Visible = false;
                gridView14.Columns["ArbSizeName"].Visible = false;
                gridView14.Columns["EngItemName"].Width = 150;
                gridView14.Columns["StoreID"].Caption = "Store ID";
                gridView14.Columns["StoreName"].Caption = "Store Name";
                gridView14.Columns["EngItemName"].Caption = "Item Name";
                gridView14.Columns["MachinID"].Caption = "Machine ID";
                gridView14.Columns["MachineName"].Caption = "Machin Name";
                gridView14.Columns["Debit"].Caption = "debtor ";
                gridView14.Columns["EngSizeName"].Caption = "Unit";
                gridView14.Columns["Credit"].Caption = "Creditor";
                gridView14.Columns["TypeOpration"].Caption = "Type Opration";
                gridView14.Columns["Signature"].Caption = "Signature";
                gridView14.Columns["DebitDate"].Caption = "Date";
                gridView14.Columns["DebitDate"].Caption = "Time";
                gridView14.Columns["EmpID"].Caption = "EmpID";
                gridView14.Columns["EmpName"].Caption = "Name";
                gridView14.Columns["ShownInNext"].Caption = "Shown In Next ";
            }
        }


        void initGridBeforTalmee3()
        {
            lstDetailTalmee3 = new BindingList<Menu_FactoryRunCommandTalmee>();
            lstDetailTalmee3.AllowNew = true;
            lstDetailTalmee3.AllowEdit = true;
            lstDetailTalmee3.AllowRemove = true;
            gridControl7.DataSource = lstDetailTalmee3;

            DataTable dtitems = Lip.SelectRecord("SELECT   ArbName   FROM Menu_FactoryMachine ");
            string[] NameMachine = new string[dtitems.Rows.Count];
            for (int i = 0; i <= dtitems.Rows.Count - 1; i++)
                NameMachine[i] = dtitems.Rows[i]["ArbName"].ToString();
            RepositoryItemComboBox riComboBoxitems = new RepositoryItemComboBox();
            riComboBoxitems.Items.AddRange(NameMachine);
            gridControl7.RepositoryItems.Add(riComboBoxitems);
            DataTable dtStore = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM Stc_Stores WHERE  Cancel =0  and BranchID=" + Comon.cInt(cmbBranchesID.EditValue) );
            string[] StoreName = new string[dtStore.Rows.Count];
            for (int i = 0; i <= dtStore.Rows.Count - 1; i++)
                StoreName[i] = dtStore.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems2 = new RepositoryItemComboBox();

            riComboBoxitems2.Items.AddRange(StoreName);
            gridControl7.RepositoryItems.Add(riComboBoxitems2);
            GridPollishBefore3.Columns["StoreName"].ColumnEdit = riComboBoxitems2;


            DataTable dtEmp = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM HR_EmployeeFile WHERE  Cancel =0  and BranchID=" + Comon.cInt(cmbBranchesID.EditValue) );
            string[] EmpName = new string[dtEmp.Rows.Count];
            for (int i = 0; i <= dtEmp.Rows.Count - 1; i++)
                EmpName[i] = dtEmp.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems3 = new RepositoryItemComboBox();
            riComboBoxitems3.Items.AddRange(EmpName);
            gridControl7.RepositoryItems.Add(riComboBoxitems3);
            GridPollishBefore3.Columns["EmpName"].ColumnEdit = riComboBoxitems3;

            DataTable dtItemname = Lip.SelectRecord("Select " + PrimaryName + " from Stc_Items  Where Cancel=0 and BranchID=" + Comon.cInt(cmbBranchesID.EditValue) );
            string[] ItemNames = new string[dtItemname.Rows.Count];
            for (int i = 0; i <= dtItemname.Rows.Count - 1; i++)
                ItemNames[i] = dtItemname.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems4 = new RepositoryItemComboBox();
            riComboBoxitems4.Items.AddRange(ItemNames);
            gridControl7.RepositoryItems.Add(riComboBoxitems4);
            GridPollishBefore3.Columns[ItemName].ColumnEdit = riComboBoxitems4;

            GridPollishBefore3.Columns["MachineName"].ColumnEdit = riComboBoxitems;
            GridPollishBefore3.Columns["ID"].Visible = false;
            GridPollishBefore3.Columns["ComandID"].Visible = false;
            GridPollishBefore3.Columns["BarcodeTalmee"].Visible = false;
            GridPollishBefore3.Columns["EmpPolishnID"].Visible = false;
            GridPollishBefore3.Columns["EmpPrentagID"].Visible = false;
            GridPollishBefore3.Columns["Cancel"].Visible = false;
            GridPollishBefore3.Columns["BranchID"].Visible = false;
            GridPollishBefore3.Columns["FacilityID"].Visible = false;

            GridPollishBefore3.Columns["EditUserID"].Visible = false;
            GridPollishBefore3.Columns["EditDate"].Visible = false;
            GridPollishBefore3.Columns["EditTime"].Visible = false;
            GridPollishBefore3.Columns["RegDate"].Visible = false;
            GridPollishBefore3.Columns["UserID"].Visible = false;

            GridPollishBefore3.Columns["ComputerInfo"].Visible = false;
            GridPollishBefore3.Columns["EditComputerInfo"].Visible = false;
            GridPollishBefore3.Columns["RegTime"].Visible = false;

            GridPollishBefore3.Columns["Credit"].Visible = false;
            GridPollishBefore3.Columns["TypeOpration"].Visible = false;
            //gridView18.Columns["SizeID"].Visible = false;
            GridPollishBefore3.Columns["CostPrice"].Visible = false;
            GridPollishBefore3.Columns["SizeID"].Visible = false;
            // gridView18.Columns["PrentagDebitTime"].Visible = false;
            GridPollishBefore3.Columns["MachinID"].Name = "MachinID";
            //dtItem.Columns["Losed"].ReadOnly = true;
            GridPollishBefore3.Columns["EmpName"].Width = 120;
            GridPollishBefore3.Columns["StoreName"].Width = 120;
            GridPollishBefore3.Columns["EmpID"].Width = 120;
            GridPollishBefore3.Columns["Signature"].Width = 120;
            GridPollishBefore3.Columns["DebitDate"].Width = 110;
            GridPollishBefore3.Columns["DebitTime"].Width = 85;
            GridPollishBefore3.Columns["EmpID"].Visible = false;
            GridPollishBefore3.Columns["MachineName"].Visible = false;
            GridPollishBefore3.Columns["MachinID"].Visible = false;
            GridPollishBefore3.Columns["StoreID"].Visible = false;
            GridPollishBefore3.Columns["StoreID"].Visible = false;

            GridPollishBefore3.Columns["Signature"].Visible = false;
            GridPollishBefore3.Columns["DebitTime"].Visible = false;
            GridPollishBefore3.Columns["ShownInNext"].Visible = false;
            if (UserInfo.Language == iLanguage.Arabic)
            {
                GridPollishBefore3.Columns["EngItemName"].Visible = false;
                GridPollishBefore3.Columns["EngSizeName"].Visible = false;
                GridPollishBefore3.Columns["ArbItemName"].Width = 150;
                GridPollishBefore3.Columns["StoreID"].Caption = "رقم المخزن";
                GridPollishBefore3.Columns["StoreName"].Caption = "إسم المخزن";
                GridPollishBefore3.Columns["EmpID"].Caption = "رقم العامل";
                GridPollishBefore3.Columns["EmpName"].Caption = "إسم العامل";
                GridPollishBefore3.Columns["MachinID"].Caption = "رقم المكينة";
                GridPollishBefore3.Columns["MachineName"].Caption = "إسم المكينة";
                GridPollishBefore3.Columns["Debit"].Caption = "الوزن";
                GridPollishBefore3.Columns["Credit"].Caption = "دائــن";
                GridPollishBefore3.Columns["TypeOpration"].Caption = "نوع العملية";
                GridPollishBefore3.Columns["Signature"].Caption = "التوقيع";
                GridPollishBefore3.Columns["ItemID"].Caption = "رقم الصنف";
                GridPollishBefore3.Columns["ArbItemName"].Caption = "اسم الصنف";
                GridPollishBefore3.Columns["SizeID"].Caption = "رقم الوحده";
                GridPollishBefore3.Columns["ArbSizeName"].Caption = "الوحده";
                GridPollishBefore3.Columns["CostPrice"].Caption = "التكلفة";
                GridPollishBefore3.Columns["DebitDate"].Caption = "التاريخ";
                GridPollishBefore3.Columns["DebitTime"].Caption = "الوقت";
            }
            else
            {
                GridPollishBefore3.Columns["ArbItemName"].Visible = false;
                GridPollishBefore3.Columns["ArbSizeName"].Visible = false;
                GridPollishBefore3.Columns["EngItemName"].Width = 150;
                GridPollishBefore3.Columns["StoreID"].Caption = "Store ID";
                GridPollishBefore3.Columns["StoreName"].Caption = "Store Name";
                GridPollishBefore3.Columns["EngItemName"].Caption = "Item Name";
                GridPollishBefore3.Columns["MachinID"].Caption = "Machine ID";
                GridPollishBefore3.Columns["MachineName"].Caption = "Machin Name";
                GridPollishBefore3.Columns["Debit"].Caption = "debtor ";
                GridPollishBefore3.Columns["EngSizeName"].Caption = "Unit";
                GridPollishBefore3.Columns["Credit"].Caption = "Creditor";
                GridPollishBefore3.Columns["TypeOpration"].Caption = "Type Opration";
                GridPollishBefore3.Columns["Signature"].Caption = "Signature";
                GridPollishBefore3.Columns["DebitDate"].Caption = "Date";
                GridPollishBefore3.Columns["DebitTime"].Caption = "Time";
                GridPollishBefore3.Columns["EmpID"].Caption = "EmpID";
                GridPollishBefore3.Columns["EmpName"].Caption = "Name";
            }



        }
        void initGridAfterTalmee3()
        {

            lstDetailAfterTalmee3 = new BindingList<Menu_FactoryRunCommandTalmee>();
            lstDetailAfterTalmee3.AllowNew = true;
            lstDetailAfterTalmee3.AllowEdit = true;
            lstDetailAfterTalmee3.AllowRemove = true;
            gridControl6.DataSource = lstDetailAfterTalmee3;

            DataTable dtitems = Lip.SelectRecord("SELECT   ArbName   FROM Menu_FactoryMachine ");
            string[] NameMachine = new string[dtitems.Rows.Count];
            for (int i = 0; i <= dtitems.Rows.Count - 1; i++)
                NameMachine[i] = dtitems.Rows[i]["ArbName"].ToString();

            RepositoryItemComboBox riComboBoxitems = new RepositoryItemComboBox();
            riComboBoxitems.Items.AddRange(NameMachine);
            gridControl6.RepositoryItems.Add(riComboBoxitems);

            DataTable dtStore = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM Stc_Stores WHERE  Cancel =0 and BranchID=" + Comon.cInt(cmbBranchesID.EditValue) );
            string[] StoreName = new string[dtStore.Rows.Count];
            for (int i = 0; i <= dtStore.Rows.Count - 1; i++)
                StoreName[i] = dtStore.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems2 = new RepositoryItemComboBox();
            riComboBoxitems2.Items.AddRange(StoreName);
            gridControl6.RepositoryItems.Add(riComboBoxitems2);
            GridPollishAfter3.Columns["StoreName"].ColumnEdit = riComboBoxitems2;

            DataTable dtEmp = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM HR_EmployeeFile WHERE  Cancel =0 and BranchID=" + Comon.cInt(cmbBranchesID.EditValue) );
            string[] EmpName = new string[dtEmp.Rows.Count];
            for (int i = 0; i <= dtEmp.Rows.Count - 1; i++)
                EmpName[i] = dtEmp.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems3 = new RepositoryItemComboBox();
            riComboBoxitems3.Items.AddRange(EmpName);
            gridControl6.RepositoryItems.Add(riComboBoxitems3);
            GridPollishAfter3.Columns["EmpName"].ColumnEdit = riComboBoxitems3;

            DataTable dtItemname = Lip.SelectRecord("Select " + PrimaryName + " from Stc_Items  Where Cancel=0 and BranchID=" + Comon.cInt(cmbBranchesID.EditValue) );
            string[] ItemNames = new string[dtItemname.Rows.Count];
            for (int i = 0; i <= dtItemname.Rows.Count - 1; i++)
                ItemNames[i] = dtItemname.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems4 = new RepositoryItemComboBox();
            riComboBoxitems4.Items.AddRange(ItemNames);

            gridControl6.RepositoryItems.Add(riComboBoxitems4);
            GridPollishAfter3.Columns[ItemName].ColumnEdit = riComboBoxitems4;
            GridPollishAfter3.Columns["SizeID"].Visible = false;
            GridPollishAfter3.Columns["MachineName"].ColumnEdit = riComboBoxitems;
            GridPollishAfter3.Columns["ID"].Visible = false;
            GridPollishAfter3.Columns["ComandID"].Visible = false;
            GridPollishAfter3.Columns["BarcodeTalmee"].Visible = false;
            GridPollishAfter3.Columns["EmpPolishnID"].Visible = false;
            GridPollishAfter3.Columns["EmpPrentagID"].Visible = false;
            GridPollishAfter3.Columns["Cancel"].Visible = false;
            GridPollishAfter3.Columns["BranchID"].Visible = false;
            GridPollishAfter3.Columns["FacilityID"].Visible = false;

            GridPollishAfter3.Columns["EditUserID"].Visible = false;
            GridPollishAfter3.Columns["EditDate"].Visible = false;
            GridPollishAfter3.Columns["EditTime"].Visible = false;
            GridPollishAfter3.Columns["RegDate"].Visible = false;
            GridPollishAfter3.Columns["UserID"].Visible = false;

            GridPollishAfter3.Columns["ComputerInfo"].Visible = false;
            GridPollishAfter3.Columns["EditComputerInfo"].Visible = false;
            GridPollishAfter3.Columns["RegTime"].Visible = false;

            GridPollishAfter3.Columns["Debit"].Visible = false;
            GridPollishAfter3.Columns["TypeOpration"].Visible = false;
            //gridView23.Columns["SizeID"].Visible = false;
            GridPollishAfter3.Columns["CostPrice"].Visible = false;

            // gridView23.Columns["PrentagDebitTime"].Visible = false;
            GridPollishAfter3.Columns["MachinID"].Name = "MachinID";
            //dtItem.Columns["Losed"].ReadOnly = true;
            GridPollishAfter3.Columns["EmpName"].Width = 120;
            GridPollishAfter3.Columns["StoreName"].Width = 120;
            GridPollishAfter3.Columns["EmpID"].Width = 120;
            GridPollishAfter3.Columns["Signature"].Width = 120;
            GridPollishAfter3.Columns["DebitDate"].Width = 110;
            GridPollishAfter3.Columns["DebitTime"].Width = 85;
            GridPollishAfter3.Columns["EmpID"].Visible = false;
            GridPollishAfter3.Columns["MachineName"].Visible = false;
            GridPollishAfter3.Columns["MachinID"].Visible = false;
            GridPollishAfter3.Columns["StoreID"].Visible = false;
            GridPollishAfter3.Columns["Signature"].Visible = false;
            GridPollishAfter3.Columns["DebitTime"].Visible = false;
            if (UserInfo.Language == iLanguage.Arabic)
            {
                GridPollishAfter3.Columns["EngItemName"].Visible = false;
                GridPollishAfter3.Columns["EngSizeName"].Visible = false;
                GridPollishAfter3.Columns["ArbItemName"].Width = 150;
                GridPollishAfter3.Columns["StoreID"].Caption = "رقم المخزن";
                GridPollishAfter3.Columns["StoreName"].Caption = "إسم المخزن";

                GridPollishAfter3.Columns["EmpID"].Caption = "رقم العامل";
                GridPollishAfter3.Columns["EmpName"].Caption = "إسم العامل";

                GridPollishAfter3.Columns["MachinID"].Caption = "رقم المكينة";
                GridPollishAfter3.Columns["MachineName"].Caption = "إسم المكينة";
                GridPollishAfter3.Columns["Debit"].Caption = "الوزن";

                GridPollishAfter3.Columns["Credit"].Caption = "الوزن";
                GridPollishAfter3.Columns["TypeOpration"].Caption = "نوع العملية";
                GridPollishAfter3.Columns["Signature"].Caption = "التوقيع";

                GridPollishAfter3.Columns["ItemID"].Caption = "رقم الصنف";
                GridPollishAfter3.Columns["ArbItemName"].Caption = "اسم الصنف";
                GridPollishAfter3.Columns["SizeID"].Caption = "رقم الوحده";
                GridPollishAfter3.Columns["ArbSizeName"].Caption = "الوحده";
                GridPollishAfter3.Columns["CostPrice"].Caption = "التكلفة";
                GridPollishAfter3.Columns["DebitDate"].Caption = "التاريخ";
                GridPollishAfter3.Columns["DebitTime"].Caption = "الوقت";
                GridPollishAfter3.Columns["ShownInNext"].Caption = "يظهر في التفاصيل ";
            }
            else
            {
                GridPollishAfter3.Columns["ArbItemName"].Visible = false;
                GridPollishAfter3.Columns["ArbSizeName"].Visible = false;
                GridPollishAfter3.Columns["EngItemName"].Width = 150;
                GridPollishAfter3.Columns["StoreID"].Caption = "Store ID";
                GridPollishAfter3.Columns["StoreName"].Caption = "Store Name";
                GridPollishAfter3.Columns["EngItemName"].Caption = "Item Name";
                GridPollishAfter3.Columns["MachinID"].Caption = "Machine ID";
                GridPollishAfter3.Columns["MachineName"].Caption = "Machin Name";
                GridPollishAfter3.Columns["Debit"].Caption = "debtor ";
                GridPollishAfter3.Columns["EngSizeName"].Caption = "Unit";
                GridPollishAfter3.Columns["Credit"].Caption = "Creditor";
                GridPollishAfter3.Columns["TypeOpration"].Caption = "Type Opration";
                GridPollishAfter3.Columns["Signature"].Caption = "Signature";
                GridPollishAfter3.Columns["DebitDate"].Caption = "Date";
                GridPollishAfter3.Columns["DebitDate"].Caption = "Time";
                GridPollishAfter3.Columns["EmpID"].Caption = "EmpID";
                GridPollishAfter3.Columns["EmpName"].Caption = "Name";
                GridPollishAfter3.Columns["ShownInNext"].Caption = "Shown In Next ";
            }
        }

        void initGridBeforAdditional()
        {

            lstDetailAdditional = new BindingList<Menu_FactoryRunCommandSelver>();
            lstDetailAdditional.AllowNew = true;
            lstDetailAdditional.AllowEdit = true;
            lstDetailAdditional.AllowRemove = true;
            gridControlBeforeAdditional.DataSource = lstDetailAdditional;

            DataTable dtitems = Lip.SelectRecord("SELECT   ArbName   FROM Menu_FactoryMachine ");
            string[] NameMachine = new string[dtitems.Rows.Count];
            for (int i = 0; i <= dtitems.Rows.Count - 1; i++)
                NameMachine[i] = dtitems.Rows[i]["ArbName"].ToString();

            RepositoryItemComboBox riComboBoxitems = new RepositoryItemComboBox();
            riComboBoxitems.Items.AddRange(NameMachine);

            gridControlBeforeAdditional.RepositoryItems.Add(riComboBoxitems);


            DataTable dtStore = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM Stc_Stores WHERE  Cancel =0  and BranchID=" + Comon.cInt(cmbBranchesID.EditValue) );
            string[] StoreName = new string[dtStore.Rows.Count];
            for (int i = 0; i <= dtStore.Rows.Count - 1; i++)
                StoreName[i] = dtStore.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems2 = new RepositoryItemComboBox();

            riComboBoxitems2.Items.AddRange(StoreName);
            gridControlBeforeAdditional.RepositoryItems.Add(riComboBoxitems2);
            GridViewBeforAddition.Columns["StoreName"].ColumnEdit = riComboBoxitems2;


            DataTable dtEmp = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM HR_EmployeeFile WHERE  Cancel =0  and BranchID=" + Comon.cInt(cmbBranchesID.EditValue) );
            string[] EmpName = new string[dtEmp.Rows.Count];
            for (int i = 0; i <= dtEmp.Rows.Count - 1; i++)
                EmpName[i] = dtEmp.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems3 = new RepositoryItemComboBox();
            riComboBoxitems3.Items.AddRange(EmpName);
            gridControlBeforeAdditional.RepositoryItems.Add(riComboBoxitems3);
            GridViewBeforAddition.Columns["EmpName"].ColumnEdit = riComboBoxitems3;

            DataTable dtItemname = Lip.SelectRecord("Select " + PrimaryName + " from Stc_Items  Where Cancel=0 and BranchID=" + Comon.cInt(cmbBranchesID.EditValue) );
            string[] ItemNames = new string[dtItemname.Rows.Count];
            for (int i = 0; i <= dtItemname.Rows.Count - 1; i++)
                ItemNames[i] = dtItemname.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems4 = new RepositoryItemComboBox();
            riComboBoxitems4.Items.AddRange(ItemNames);
            gridControlBeforeAdditional.RepositoryItems.Add(riComboBoxitems4);
            GridViewBeforAddition.Columns[ItemName].ColumnEdit = riComboBoxitems4;

            GridViewBeforAddition.Columns["MachineName"].ColumnEdit = riComboBoxitems;
            GridViewBeforAddition.Columns["ID"].Visible = false;
            GridViewBeforAddition.Columns["ComandID"].Visible = false;
            GridViewBeforAddition.Columns["BarcodeAdditional"].Visible = false;
            GridViewBeforAddition.Columns["EmpAdditionalID"].Visible = false;
            GridViewBeforAddition.Columns["Cancel"].Visible = false;
            GridViewBeforAddition.Columns["BranchID"].Visible = false;
            GridViewBeforAddition.Columns["FacilityID"].Visible = false;

            GridViewBeforAddition.Columns["EditUserID"].Visible = false;
            GridViewBeforAddition.Columns["EditDate"].Visible = false;
            GridViewBeforAddition.Columns["EditTime"].Visible = false;
            GridViewBeforAddition.Columns["RegDate"].Visible = false;
            GridViewBeforAddition.Columns["UserID"].Visible = false;

            GridViewBeforAddition.Columns["ComputerInfo"].Visible = false;
            GridViewBeforAddition.Columns["EditComputerInfo"].Visible = false;
            GridViewBeforAddition.Columns["RegTime"].Visible = false;

            GridViewBeforAddition.Columns["Credit"].Visible = false;
            GridViewBeforAddition.Columns["TypeOpration"].Visible = false;
            //GridViewBeforPolish.Columns["SizeID"].Visible = false;
            GridViewBeforAddition.Columns["CostPrice"].Visible = false;
            GridViewBeforAddition.Columns["SizeID"].Visible = false;
            // GridViewBeforPolish.Columns["DebitTime"].Visible = false;
            GridViewBeforAddition.Columns["MachinID"].Name = "MachinID";
            //dtItem.Columns["Losed"].ReadOnly = true;
            GridViewBeforAddition.Columns["EmpName"].Width = 120;
            GridViewBeforAddition.Columns["StoreName"].Width = 120;
            GridViewBeforAddition.Columns["EmpID"].Width = 120;
            GridViewBeforAddition.Columns["Signature"].Width = 120;
            GridViewBeforAddition.Columns["DebitDate"].Width = 110;
            GridViewBeforAddition.Columns["DebitTime"].Width = 85;
            GridViewBeforAddition.Columns["EmpID"].Visible = false;
            GridViewBeforAddition.Columns["StoreName"].Visible = false;
            GridViewBeforAddition.Columns["EmpName"].Visible = false;
            GridViewBeforAddition.Columns["StoreID"].Visible = false;
            GridViewBeforAddition.Columns["MachinID"].Visible = false;
            GridViewBeforAddition.Columns["MachineName"].Visible = false;
            GridViewBeforAddition.Columns["Lost"].Visible = false;
            GridViewBeforAddition.Columns["ShownInNext"].Visible = false;
            if (UserInfo.Language == iLanguage.Arabic)
            {

                GridViewBeforAddition.Columns["EngItemName"].Visible = false;
                GridViewBeforAddition.Columns["EngSizeName"].Visible = false;
                GridViewBeforAddition.Columns["ArbItemName"].Width = 150;

                GridViewBeforAddition.Columns["StoreID"].Caption = "رقم المخزن";
                GridViewBeforAddition.Columns["StoreName"].Caption = "إسم المخزن";

                GridViewBeforAddition.Columns["EmpID"].Caption = "رقم العامل";
                GridViewBeforAddition.Columns["EmpName"].Caption = "إسم العامل";

                GridViewBeforAddition.Columns["MachinID"].Caption = "رقم المكينة";
                GridViewBeforAddition.Columns["MachineName"].Caption = "إسم المكينة";
                GridViewBeforAddition.Columns["Debit"].Caption = "الوزن";

                GridViewBeforAddition.Columns["Credit"].Caption = "دائــن";
                GridViewBeforAddition.Columns["TypeOpration"].Caption = "نوع العملية";
                GridViewBeforAddition.Columns["Signature"].Caption = "التوقيع";

                GridViewBeforAddition.Columns["ItemID"].Caption = "رقم الصنف";
                GridViewBeforAddition.Columns["ArbItemName"].Caption = "اسم الصنف";
                GridViewBeforAddition.Columns["SizeID"].Caption = "رقم الوحده";
                GridViewBeforAddition.Columns["ArbSizeName"].Caption = "الوحده";
                GridViewBeforAddition.Columns["CostPrice"].Caption = "التكلفة";
                GridViewBeforAddition.Columns["DebitDate"].Caption = "التاريخ";
                GridViewBeforAddition.Columns["DebitTime"].Caption = "الوقت";
            }
            else
            {
                GridViewBeforAddition.Columns["ArbItemName"].Visible = false;
                GridViewBeforAddition.Columns["ArbSizeName"].Visible = false;
                GridViewBeforAddition.Columns["EngItemName"].Width = 150;
                GridViewBeforAddition.Columns["StoreID"].Caption = "Store ID";
                GridViewBeforAddition.Columns["StoreName"].Caption = "Store Name";
                GridViewBeforAddition.Columns["EngItemName"].Caption = "Item Name";
                GridViewBeforAddition.Columns["MachinID"].Caption = "Machine ID";
                GridViewBeforAddition.Columns["MachineName"].Caption = "Machin Name";
                GridViewBeforAddition.Columns["Debit"].Caption = "debtor ";
                GridViewBeforAddition.Columns["EngSizeName"].Caption = "Unit";
                GridViewBeforAddition.Columns["Credit"].Caption = "Creditor";
                GridViewBeforAddition.Columns["TypeOpration"].Caption = "Type Opration";
                GridViewBeforAddition.Columns["Signature"].Caption = "Signature";
                GridViewBeforAddition.Columns["DebitDate"].Caption = "Date";
                GridViewBeforAddition.Columns["DebitTime"].Caption = "Time";
                GridViewBeforAddition.Columns["EmpID"].Caption = "EmpID";
                GridViewBeforAddition.Columns["EmpName"].Caption = "Name";
            }



        }
        void initGridAfterAdditional()
        {

            lstDetailAfterAdditional = new BindingList<Menu_FactoryRunCommandSelver>();
            lstDetailAfterAdditional.AllowNew = true;
            lstDetailAfterAdditional.AllowEdit = true;
            lstDetailAfterAdditional.AllowRemove = true;
            gridControlAfterAdditional.DataSource = lstDetailAfterAdditional;

            DataTable dtitems = Lip.SelectRecord("SELECT   ArbName   FROM Menu_FactoryMachine ");
            string[] NameMachine = new string[dtitems.Rows.Count];
            for (int i = 0; i <= dtitems.Rows.Count - 1; i++)
                NameMachine[i] = dtitems.Rows[i]["ArbName"].ToString();

            RepositoryItemComboBox riComboBoxitems = new RepositoryItemComboBox();
            riComboBoxitems.Items.AddRange(NameMachine);
            gridControlAfterAdditional.RepositoryItems.Add(riComboBoxitems);



            DataTable dtStore = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM Stc_Stores WHERE  Cancel =0  and BranchID=" + Comon.cInt(cmbBranchesID.EditValue) );
            string[] StoreName = new string[dtStore.Rows.Count];
            for (int i = 0; i <= dtStore.Rows.Count - 1; i++)
                StoreName[i] = dtStore.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems2 = new RepositoryItemComboBox();
            riComboBoxitems2.Items.AddRange(StoreName);
            gridControlAfterAdditional.RepositoryItems.Add(riComboBoxitems2);
            gridView20.Columns["StoreName"].ColumnEdit = riComboBoxitems2;


            DataTable dtEmp = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM HR_EmployeeFile WHERE  Cancel =0  and BranchID=" + Comon.cInt(cmbBranchesID.EditValue) );
            string[] EmpName = new string[dtEmp.Rows.Count];
            for (int i = 0; i <= dtEmp.Rows.Count - 1; i++)
                EmpName[i] = dtEmp.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems3 = new RepositoryItemComboBox();
            riComboBoxitems3.Items.AddRange(EmpName);
            gridControlAfterAdditional.RepositoryItems.Add(riComboBoxitems3);
            gridView20.Columns["EmpName"].ColumnEdit = riComboBoxitems3;

            DataTable dtItemname = Lip.SelectRecord("Select " + PrimaryName + " from Stc_Items  Where Cancel=0 and BranchID=" + Comon.cInt(cmbBranchesID.EditValue));
            string[] ItemNames = new string[dtItemname.Rows.Count];
            for (int i = 0; i <= dtItemname.Rows.Count - 1; i++)
                ItemNames[i] = dtItemname.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems4 = new RepositoryItemComboBox();
            riComboBoxitems4.Items.AddRange(ItemNames);
            gridControlAfterAdditional.RepositoryItems.Add(riComboBoxitems4);
            gridView20.Columns[ItemName].ColumnEdit = riComboBoxitems4;
            gridView20.Columns["SizeID"].Visible = false;
            gridView20.Columns["MachineName"].ColumnEdit = riComboBoxitems;
            gridView20.Columns["ID"].Visible = false;
            gridView20.Columns["ComandID"].Visible = false;
            gridView20.Columns["BarcodeAdditional"].Visible = false;
            gridView20.Columns["EmpAdditionalID"].Visible = false;
            gridView20.Columns["Cancel"].Visible = false;
            gridView20.Columns["BranchID"].Visible = false;
            gridView20.Columns["FacilityID"].Visible = false;

            gridView20.Columns["EditUserID"].Visible = false;
            gridView20.Columns["EditDate"].Visible = false;
            gridView20.Columns["EditTime"].Visible = false;
            gridView20.Columns["RegDate"].Visible = false;
            gridView20.Columns["UserID"].Visible = false;

            gridView20.Columns["ComputerInfo"].Visible = false;
            gridView20.Columns["EditComputerInfo"].Visible = false;
            gridView20.Columns["RegTime"].Visible = false;

            gridView20.Columns["Debit"].Visible = false;
            gridView20.Columns["TypeOpration"].Visible = false;
            //gridView20.Columns["SizeID"].Visible = false;
            gridView20.Columns["CostPrice"].Visible = false;

            // gridView20.Columns["DebitTime"].Visible = false;
            gridView20.Columns["MachinID"].Name = "MachinID";
            //dtItem.Columns["Losed"].ReadOnly = true;
            gridView20.Columns["EmpName"].Width = 120;
            gridView20.Columns["StoreName"].Width = 120;
            gridView20.Columns["EmpID"].Width = 120;
            gridView20.Columns["Signature"].Width = 120;
            gridView20.Columns["DebitDate"].Width = 110;
            gridView20.Columns["DebitTime"].Width = 85;
            gridView20.Columns["EmpID"].Visible = false;
            gridView20.Columns["StoreName"].Visible = false;
            gridView20.Columns["EmpName"].Visible = false;
            gridView20.Columns["StoreID"].Visible = false;
            gridView20.Columns["Credit"].VisibleIndex = gridView20.Columns["ArbSizeName"].VisibleIndex + 1;
            gridView20.Columns["MachinID"].Visible = false;
            gridView20.Columns["MachineName"].Visible = false;
            gridView20.Columns["Lost"].Visible = false;
            if (UserInfo.Language == iLanguage.Arabic)
            {
                gridView20.Columns["EngItemName"].Visible = false;
                gridView20.Columns["EngSizeName"].Visible = false;
                gridView20.Columns["ArbItemName"].Width = 150;
                gridView20.Columns["StoreID"].Caption = "رقم المخزن";
                gridView20.Columns["StoreName"].Caption = "إسم المخزن";

                gridView20.Columns["EmpID"].Caption = "رقم العامل";
                gridView20.Columns["EmpName"].Caption = "إسم العامل";

                gridView20.Columns["MachinID"].Caption = "رقم المكينة";
                gridView20.Columns["MachineName"].Caption = "إسم المكينة";
                gridView20.Columns["Debit"].Caption = "الوزن";

                gridView20.Columns["Credit"].Caption = "الوزن";
                gridView20.Columns["TypeOpration"].Caption = "نوع العملية";
                gridView20.Columns["Signature"].Caption = "التوقيع";

                gridView20.Columns["ItemID"].Caption = "رقم الصنف";
                gridView20.Columns["ArbItemName"].Caption = "اسم الصنف";
                gridView20.Columns["SizeID"].Caption = "رقم الوحده";
                gridView20.Columns["ArbSizeName"].Caption = "الوحده";
                gridView20.Columns["CostPrice"].Caption = "التكلفة";
                gridView20.Columns["DebitDate"].Caption = "التاريخ";
                gridView20.Columns["DebitTime"].Caption = "الوقت";
                gridView20.Columns["ShownInNext"].Caption = "يظهر في التفاصيل ";
            }
            else
            {
                gridView20.Columns["ArbItemName"].Visible = false;
                gridView20.Columns["ArbSizeName"].Visible = false;
                gridView20.Columns["EngItemName"].Width = 150;
                gridView20.Columns["StoreID"].Caption = "Store ID";
                gridView20.Columns["StoreName"].Caption = "Store Name";
                gridView20.Columns["EngItemName"].Caption = "Item Name";
                gridView20.Columns["MachinID"].Caption = "Machine ID";
                gridView20.Columns["MachineName"].Caption = "Machin Name";
                gridView20.Columns["Debit"].Caption = "debtor ";
                gridView20.Columns["EngSizeName"].Caption = "Unit";
                gridView20.Columns["Credit"].Caption = "Creditor";
                gridView20.Columns["TypeOpration"].Caption = "Type Opration";
                gridView20.Columns["Signature"].Caption = "Signature";
                gridView20.Columns["DebitDate"].Caption = "Date";
                gridView20.Columns["DebitDate"].Caption = "Time";
                gridView20.Columns["EmpID"].Caption = "EmpID";
                gridView20.Columns["EmpName"].Caption = "Name";
                gridView20.Columns["ShownInNext"].Caption = "Shown In Next ";
            }



        }
        #endregion

        #region Do Function
        protected override void DoAddFrom()
        {
            try
            {
                txtOrderID.Text = "";
                txtOrderID_Validating(null, null);
            }
            catch   {   }
        }
            protected override void DoSearch()
            {
                try
                {
                    Find();
                }
                catch { }
            }
        decimal CulclLost(GridView Grid1,GridView Grid2,string ColQTY1,string ColQTY2)
        {
             
            decimal Total1 = 0;
            decimal Total2 = 0;
            for (int i = 0; i <= Grid1.DataRowCount- 1; i++)
              Total1 += Comon.cDec( Grid1.GetRowCellValue(i, ColQTY1).ToString());
            for (int i = 0; i <= Grid2.DataRowCount - 1; i++)
                Total2 += Comon.cDec(Grid2.GetRowCellValue(i, ColQTY2).ToString());
            return Comon.cDec(Total1 - Total2);

        }
        protected override void DoPrint()
            {
                try
                {
                    if (IsNewRecord)
                    {
                        Messages.MsgWarning(Messages.TitleWorning, Messages.msgYouShouldSaveDataBeforePrinting);
                        return;
                    }
                    Application.DoEvents();
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);

                    /******************** Report Body *************************/
                    if (chkBeforeCasting.Checked==true&&chkAddtional.Checked==true)
                       ReportName = "rptManu_FactoryCommend";
                    else if (chkBeforeCasting.Checked == false && chkAddtional.Checked == true)
                        ReportName = "rptManu_FactoryCommend1";
                    else if (chkBeforeCasting.Checked==true&&chkAddtional.Checked==false)
                        ReportName = "rptManu_FactoryCommend2";
                    else if (chkBeforeCasting.Checked == false && chkAddtional.Checked == false)
                        ReportName = "rptManu_FactoryCommend3";
                    bool IncludeHeader = true;

                    string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");
                    XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);
                    /********************** Master *****************************/
                    rptForm.RequestParameters = false;

                    for (int i = 0; i < rptForm.Parameters.Count; i++)
                        rptForm.Parameters[i].Visible = false;
                     
                    rptForm.Parameters["OrderID"].Value =txtOrderID.Text;
                    rptForm.Parameters["OrderDate"].Value =txtOrderDate.Text;
                    rptForm.Parameters["CustomerName"].Value = lblCustomerName.Text;
                    rptForm.Parameters["DelegetName"].Value = lblDelegateName.Text;
                    rptForm.Parameters["GuidanceName"].Value = lblGuidanceName.Text;
                    rptForm.Parameters["TypeOrder"].Value = cmbTypeOrders.Text;
                    rptForm.Parameters["Gold"].Value = txtTotalGold.Text;
                    rptForm.Parameters["LostManuFactory"].Value = CulclLost(GridViewBeforfactory,GridViewAfterfactory, "Debit", "Credit");
                    rptForm.Parameters["LostManuBrntage"].Value = CulclLost(GridViewBeforPrentag, GridViewAfterPrentag, "PrentagDebit", "PrentagCredit");
                    rptForm.Parameters["LostManuBrntage2"].Value = CulclLost(gridView12, gridView9, "PrentagDebit", "PrentagCredit");

                    rptForm.Parameters["LostManuCommpound"].Value = CulclLost(gridViewBeforCompond , gridViewAfterCompond, "ComWeightSton", "ComWeightSton");
                    rptForm.Parameters["LostManuAdditional"].Value = CulclLost(GridViewBeforAddition, gridView20, "Debit", "Credit");

                    rptForm.Parameters["LostManuPolish1"].Value = CulclLost(GridViewBeforPolish, GridViewAfterPolish, "Debit", "Credit");
                    rptForm.Parameters["LostManuPolish2"].Value = CulclLost(gridView18, gridView14, "Debit", "Credit");
                    rptForm.Parameters["LostManuPolish3"].Value = CulclLost(GridPollishBefore3, GridPollishAfter3, "Debit", "Credit");
                    if(chkAllLost.Checked==true)
                        rptForm.Parameters["TotalLost"].Value = (Comon.cDec(rptForm.Parameters["LostManuFactory"].Value) + Comon.cDec(rptForm.Parameters["LostManuBrntage"].Value) +
                        Comon.cDec(rptForm.Parameters["LostManuBrntage2"].Value) + Comon.cDec(rptForm.Parameters["LostManuCommpound"].Value) +
                        Comon.cDec(rptForm.Parameters["LostManuAdditional"].Value) + Comon.cDec(rptForm.Parameters["LostManuPolish1"].Value) +
                        Comon.cDec(rptForm.Parameters["LostManuPolish2"].Value) + Comon.cDec(rptForm.Parameters["LostManuPolish3"].Value));
                    else
                       rptForm.Parameters["TotalLost"].Value = 0;
                    DataTable dtt = Lip.SelectRecord("SELECT  [QTYGram], [QTYOrder],SalesPriceQram    FROM  [Menu_ProductionExpensesMaster] where [Cancel]=0 and BranchID="+Comon.cInt(cmbBranchesID.EditValue) +"  and OrderID='" + txtOrderID.Text + "'");
                   if (dtt.Rows.Count > 0)
                   {
                       decimal OrderCost = Comon.cDec(Comon.cDec(dtt.Rows[0]["QTYGram"]) * Comon.cDec(dtt.Rows[0]["QTYOrder"]));
                       if (chkAllLost.Checked == true)
                           rptForm.Parameters["CostPriceOrder"].Value = OrderCost;
                       else
                           rptForm.Parameters["CostPriceOrder"].Value = 0;
                   }
                /********************** Details ****************************/
             
                rptForm.Parameters["Daimond"].Value = txtRoundQTY.Text;
                rptForm.Parameters["Zircone"].Value = txtZirCode.Text;
                rptForm.Parameters["BAGET"].Value = txtTotalBagit.Text;


                rptForm.Parameters["RoundCustomer"].Value = txtRoundQTYCustomer.Text;
                rptForm.Parameters["BAGETCustomer"].Value =txtTotalBagitCustomer.Text;

                rptForm.Parameters["TotalAllQTY"].Value = txtAllTotalQTYOrder.Text;
                rptForm.DataMember = ReportName;
                    /******************** Report Binding ************************/
                    XRSubreport subreport = (XRSubreport)rptForm.FindControl("subRptCompanyHeader", true);
                    subreport.Visible = IncludeHeader;
                    subreport.ReportSource = ReportComponent.CompanyHeader();

                    /******************** Report Before Casting Stages ************************/
                    if (chkBeforeCasting.Checked == true)
                    {
                        XRSubreport subreportBeforeCasting = (XRSubreport)rptForm.FindControl("subRptrptManu_FactoryBeforeCastingStages", true);
                        subreportBeforeCasting.Visible = IncludeHeader;
                        subreportBeforeCasting.ReportSource = Manu_BeforCastingStage();
                    }

                    /******************** Report Factory ************************/
                    XRSubreport subreportFactor = (XRSubreport)rptForm.FindControl("subRptrptManu_FactoryFactorCommendBefore", true);
                    subreportFactor.Visible = IncludeHeader;
                    subreportFactor.ReportSource = Manu_FactoryFactorBefor();

                   
                    /******************** Report Brntag Frist ************************/
                    XRSubreport subreportBrntage = (XRSubreport)rptForm.FindControl("subRptrptManu_FactoryBrntageCommendBefor", true);
                    subreportBrntage.Visible = IncludeHeader;
                    subreportBrntage.ReportSource = Manu_FactoryBrntageBefor();

 

                /******************** Report Compound ************************/
                    XRSubreport subreportCompound = (XRSubreport)rptForm.FindControl("subRptrptManu_FactoryCompoundCommendBefor", true);
                    subreportCompound.Visible = IncludeHeader;
                    subreportCompound.ReportSource = Manu_FactoryCompoundBefor();

                    //XRSubreport subreportCompoundAfter = (XRSubreport)rptForm.FindControl("subRptrptManu_FactoryCompoundCommendAfter", true);
                    //subreportCompoundAfter.Visible = IncludeHeader;
                    //subreportCompoundAfter.ReportSource = Manu_FactoryCompoundAfter();

                    /******************** Report talmee Frist  ************************/
                    XRSubreport subreportTalmee = (XRSubreport)rptForm.FindControl("subRptrptManu_FactoryTalmeeCommendBefor", true);
                    subreportTalmee.Visible = IncludeHeader;
                    subreportTalmee.ReportSource = Manu_FactoryTalmeeBefor();

             
                /******************** Report Addtional ************************/
                    if (chkAddtional.Checked == true)
                    {
                        XRSubreport subreportAddtional = (XRSubreport)rptForm.FindControl("subRptrptManu_FactoryAddtionalCommendBefore", true);
                        subreportAddtional.Visible = IncludeHeader;
                        subreportAddtional.ReportSource = Manu_FactoryAddtional(GridViewBeforAddition, "Debit");
                    }

                //XRSubreport subreportAddtionalAfter = (XRSubreport)rptForm.FindControl("subRptrptManu_FactoryAddtionalCommendAfter", true);
                //subreportAddtionalAfter.Visible = IncludeHeader;
                //subreportAddtionalAfter.ReportSource = Manu_FactoryAddtional(gridView20, "Credit");

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
                        if (dt.Rows.Count > 0)
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
        #endregion

        #region Function 
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
            public XtraReport Manu_BeforCastingStage()
            {
                string rptrptManu_FactoryFactorCommendName = "‏‏‏‏rptManu_FactoryBeforeCastingStage";
                string Path = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + @"\Reports\";
                //rptCompanyHeaderName += (UserInfo.Language == iLanguage.English ? "Eng" : "Arb");
                rptrptManu_FactoryFactorCommendName += "Arb";
                XtraReport rptFactoryFactor = XtraReport.FromFile(Path + rptrptManu_FactoryFactorCommendName + ".repx", true);


                var dataTable = new dsReports.rptManu_FactoryBeforeCastingStageDataTable();
                for (int i = 0; i <= GridCad.DataRowCount - 1; i++)
                {
                    var row = dataTable.NewRow();
                    row["#"] = i + 1;
                   
                    row["QTY"] = GridCad.GetRowCellValue(i, "QTY");

                    row["StoreName"] = GridCad.GetRowCellValue(i, "StoreName");

                    row["ItemID"] = GridCad.GetRowCellValue(i, "ItemID");
                    row["ItemName"] = GridCad.GetRowCellValue(i, ItemName);
                    row["CostPrice"] = GridCad.GetRowCellValue(i, "CostPrice");
                    row["SizeName"] = GridCad.GetRowCellValue(i, SizeName);
                    row["DateBefore"] = GridCad.GetRowCellValue(i, "DateBefore");
                    row["DateAfter"] = GridCad.GetRowCellValue(i, "DateAfter");
                    row["EmpName"] = GridCad.GetRowCellValue(i, "FactorName");

                    dataTable.Rows.Add(row);
                }
                rptFactoryFactor.DataSource = dataTable;
                rptFactoryFactor.DataMember = "rptManu_FactoryBeforeCastingStage";
                return rptFactoryFactor;
            }
            public XtraReport Manu_FactoryFactorBefor()
            {
                string rptrptManu_FactoryFactorCommendName = "rptManu_FactoryFactorCommendBefore";
                string Path = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + @"\Reports\";
                //rptCompanyHeaderName += (UserInfo.Language == iLanguage.English ? "Eng" : "Arb");
                rptrptManu_FactoryFactorCommendName += "Arb";
                XtraReport rptFactoryFactor = XtraReport.FromFile(Path + rptrptManu_FactoryFactorCommendName + ".repx", true);


                var dataTable = new dsReports.rptManu_FactoryFactorCommendBeforeDataTable();
                for (int i = 0; i <= GridViewBeforfactory.DataRowCount - 1; i++)
                {
                    var row = dataTable.NewRow();
                    row["#"] = i + 1;
                    row["MachinID"] = GridViewBeforfactory.GetRowCellValue(i, "MachinID");
                    row["MachineName"] = GridViewBeforfactory.GetRowCellValue(i, "MachineName");
                    row["QTY"] = GridViewBeforfactory.GetRowCellValue(i, "Debit");
                    row["QTYAfter"] = 0;
                    row["StoreName"] = GridViewBeforfactory.GetRowCellValue(i, "StoreName");

                    row["ItemID"] = GridViewBeforfactory.GetRowCellValue(i, "ItemID");
                    row["ItemName"] = GridViewBeforfactory.GetRowCellValue(i, ItemName);
                    row["SizeName"] = GridViewBeforfactory.GetRowCellValue(i, SizeName);
                    row["Date"] = GridViewBeforfactory.GetRowCellValue(i, "DebitDate");
                    row["Time"] = GridViewBeforfactory.GetRowCellValue(i, "DebitTime");
                    row["EmpName"] = GridViewBeforfactory.GetRowCellValue(i, "EmpName");

                    dataTable.Rows.Add(row);
                }
                for (int i = 0; i <= GridViewAfterfactory.DataRowCount - 1; i++)
                {
                    var row = dataTable.NewRow();
                    row["#"] = i + 1;
                    row["MachinID"] = GridViewAfterfactory.GetRowCellValue(i, "MachinID");
                    row["MachineName"] = GridViewAfterfactory.GetRowCellValue(i, "MachineName");
                    row["QTY"] = 0;
                    row["QTYAfter"] = GridViewAfterfactory.GetRowCellValue(i, "Credit");

                    row["StoreName"] = GridViewAfterfactory.GetRowCellValue(i, "StoreName");

                    row["ItemID"] = GridViewAfterfactory.GetRowCellValue(i, "ItemID");
                    row["ItemName"] = GridViewAfterfactory.GetRowCellValue(i, ItemName);
                    row["SizeName"] = GridViewAfterfactory.GetRowCellValue(i, SizeName);
                    row["Date"] = GridViewAfterfactory.GetRowCellValue(i, "DebitDate");
                    row["Time"] = GridViewAfterfactory.GetRowCellValue(i, "DebitTime");
                    row["EmpName"] = GridViewAfterfactory.GetRowCellValue(i, "EmpName");



                    dataTable.Rows.Add(row);
                }
                rptFactoryFactor.DataSource = dataTable;
                rptFactoryFactor.DataMember = "rptManu_FactoryFactorCommendBefore";
                return rptFactoryFactor;
            }
            public XtraReport Manu_FactoryBrntageBefor()
            {
                string rptrptManu_FactoryFactorCommendName = "rptManu_FactoryBrntageCommendBefore";
                string Path = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + @"\Reports\";
                //rptCompanyHeaderName += (UserInfo.Language == iLanguage.English ? "Eng" : "Arb");
                rptrptManu_FactoryFactorCommendName += "Arb";
                XtraReport rptFactoryFactor = XtraReport.FromFile(Path + rptrptManu_FactoryFactorCommendName + ".repx", true);


                var dataTable = new dsReports.rptManu_FactoryFactorCommendBeforeDataTable();
                for (int i = 0; i <= GridViewBeforPrentag.DataRowCount - 1; i++)
                {
                    var row = dataTable.NewRow();
                    row["#"] = i + 1;
                    row["MachinID"] = GridViewBeforPrentag.GetRowCellValue(i, "MachinID");
                    row["MachineName"] = GridViewBeforPrentag.GetRowCellValue(i, "MachineName");
                    row["QTY"] = GridViewBeforPrentag.GetRowCellValue(i, "PrentagDebit");
                    row["QTYAfter"] = 0;
                    row["StoreName"] = GridViewBeforPrentag.GetRowCellValue(i, "StoreName");
                    row["TypeName"] = UserInfo.Language == iLanguage.Arabic ? "برنتاج 1" : "Prntage 1";
                    row["ItemID"] = GridViewBeforPrentag.GetRowCellValue(i, "ItemID");
                    row["ItemName"] = GridViewBeforPrentag.GetRowCellValue(i, ItemName);
                    row["SizeName"] = GridViewBeforPrentag.GetRowCellValue(i, SizeName);
                    row["Date"] = GridViewBeforPrentag.GetRowCellValue(i, "PrentagDebitDate");
                    row["Time"] = GridViewBeforPrentag.GetRowCellValue(i, "PrentagDebitTime");
                    row["EmpName"] = GridViewBeforPrentag.GetRowCellValue(i, "EmpName");
                    dataTable.Rows.Add(row);
                }
                for (int i = 0; i <= GridViewAfterPrentag.DataRowCount - 1; i++)
                {
                    var row = dataTable.NewRow();
                    row["#"] = i + 1;
                    row["MachinID"] = GridViewAfterPrentag.GetRowCellValue(i, "MachinID");
                    row["MachineName"] = GridViewAfterPrentag.GetRowCellValue(i, "MachineName");
                    row["QTYAfter"] = GridViewAfterPrentag.GetRowCellValue(i, "PrentagCredit");
                    row["QTY"] = 0;
                    row["StoreName"] = GridViewAfterPrentag.GetRowCellValue(i, "StoreName");
                    row["TypeName"] = UserInfo.Language == iLanguage.Arabic ? "برنتاج 1" : "Prntage 1";
                    row["ItemID"] = GridViewAfterPrentag.GetRowCellValue(i, "ItemID");
                    row["ItemName"] = GridViewAfterPrentag.GetRowCellValue(i, ItemName);
                    row["SizeName"] = GridViewAfterPrentag.GetRowCellValue(i, SizeName);
                    row["Date"] = GridViewAfterPrentag.GetRowCellValue(i, "PrentagDebitDate");
                    row["Time"] = GridViewAfterPrentag.GetRowCellValue(i, "PrentagDebitTime");
                    row["EmpName"] = GridViewAfterPrentag.GetRowCellValue(i, "EmpName");
                    dataTable.Rows.Add(row);
                }
                for (int i = 0; i <= gridView12.DataRowCount - 1; i++)
                {
                    var row = dataTable.NewRow();
                    row["#"] = i + 1;
                    row["MachinID"] = gridView12.GetRowCellValue(i, "MachinID");
                    row["MachineName"] = gridView12.GetRowCellValue(i, "MachineName");
                    row["QTY"] = gridView12.GetRowCellValue(i, "PrentagDebit");
                    row["QTYAfter"] = 0;
                    row["StoreName"] = gridView12.GetRowCellValue(i, "StoreName");
                    row["TypeName"] = UserInfo.Language == iLanguage.Arabic ? "برنتاج 2" : "Prntage 2";
                    row["ItemID"] = gridView12.GetRowCellValue(i, "ItemID");
                    row["ItemName"] = gridView12.GetRowCellValue(i, ItemName);
                    row["SizeName"] = gridView12.GetRowCellValue(i, SizeName);
                    row["Date"] = gridView12.GetRowCellValue(i, "PrentagDebitDate");
                    row["Time"] = gridView12.GetRowCellValue(i, "PrentagDebitTime");
                    row["EmpName"] = gridView12.GetRowCellValue(i, "EmpName");
                    dataTable.Rows.Add(row);
                }
                for (int i = 0; i <= gridView9.DataRowCount - 1; i++)
                {
                    var row = dataTable.NewRow();
                    row["#"] = i + 1;
                    row["MachinID"] = gridView9.GetRowCellValue(i, "MachinID");
                    row["MachineName"] = gridView9.GetRowCellValue(i, "MachineName");
                    row["QTYAfter"] = gridView9.GetRowCellValue(i, "PrentagCredit");
                    row["QTY"] = 0;
                    row["TypeName"] = UserInfo.Language == iLanguage.Arabic ? "برنتاج 2" : "Prntage 2";
                    row["StoreName"] = gridView9.GetRowCellValue(i, "StoreName");
                    row["ItemID"] = gridView9.GetRowCellValue(i, "ItemID");
                    row["ItemName"] = gridView9.GetRowCellValue(i, ItemName);
                    row["SizeName"] = gridView9.GetRowCellValue(i, SizeName);
                    row["Date"] = gridView9.GetRowCellValue(i, "PrentagDebitDate");
                    row["Time"] = gridView9.GetRowCellValue(i, "PrentagDebitTime");
                    row["EmpName"] = gridView9.GetRowCellValue(i, "EmpName");
                    dataTable.Rows.Add(row);
                }
                rptFactoryFactor.DataSource = dataTable;
                rptFactoryFactor.DataMember = "rptManu_FactoryBrntageCommendBefore";
                return rptFactoryFactor;
            }

        



        //public XtraReport Manu_FactoryFactorAfter()
        //    {
        //        string rptrptManu_FactoryFactorCommendName = "rptManu_FactoryFactorCommendAfter";
        //        string Path = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + @"\Reports\";
        //        //rptCompanyHeaderName += (UserInfo.Language == iLanguage.English ? "Eng" : "Arb");
        //        rptrptManu_FactoryFactorCommendName += "Arb";
        //        XtraReport rptFactoryFactor = XtraReport.FromFile(Path + rptrptManu_FactoryFactorCommendName + ".repx", true);


        //        var dataTable = new dsReports.rptManu_FactoryFactorCommendBeforeDataTable();
        //        for (int i = 0; i <= GridViewAfterfactory.DataRowCount - 1; i++)
        //        {
        //            var row = dataTable.NewRow();
        //            row["#"] = i + 1;
        //            row["MachinID"] = GridViewAfterfactory.GetRowCellValue(i, "MachinID");
        //            row["MachineName"] = GridViewAfterfactory.GetRowCellValue(i, "MachineName");
        //            row["QTY"] = GridViewAfterfactory.GetRowCellValue(i, "Credit");

        //            row["StoreName"] = GridViewAfterfactory.GetRowCellValue(i, "StoreName");

        //            row["ItemID"] = GridViewAfterfactory.GetRowCellValue(i, "ItemID");
        //            row["ItemName"] = GridViewAfterfactory.GetRowCellValue(i, ItemName);
        //            row["SizeName"] = GridViewAfterfactory.GetRowCellValue(i, SizeName);
        //            row["Date"] = GridViewAfterfactory.GetRowCellValue(i, "DebitDate");
        //            row["Time"] = GridViewAfterfactory.GetRowCellValue(i, "DebitTime");
        //            row["EmpName"] = GridViewAfterfactory.GetRowCellValue(i, "EmpName");



        //            dataTable.Rows.Add(row);
        //        }
        //        rptFactoryFactor.DataSource = dataTable;
        //        rptFactoryFactor.DataMember = "rptManu_FactoryFactorCommendAfter";
        //        return rptFactoryFactor;
        //    }
        //    public XtraReport Manu_FactoryBrntageAfter(GridView Grid)
        //    {
        //        string rptrptManu_FactoryFactorCommendName = "rptManu_FactoryBrntageCommendAfter";
        //        string Path = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + @"\Reports\";
        //        //rptCompanyHeaderName += (UserInfo.Language == iLanguage.English ? "Eng" : "Arb");
        //        rptrptManu_FactoryFactorCommendName += "Arb";
        //        XtraReport rptFactoryFactor = XtraReport.FromFile(Path + rptrptManu_FactoryFactorCommendName + ".repx", true);


        //        var dataTable = new dsReports.rptManu_FactoryFactorCommendBeforeDataTable();
        //        for (int i = 0; i <= Grid.DataRowCount - 1; i++)
        //        {
        //            var row = dataTable.NewRow();
        //            row["#"] = i + 1;
        //            row["MachinID"] = Grid.GetRowCellValue(i, "MachinID");
        //            row["MachineName"] = Grid.GetRowCellValue(i, "MachineName");
        //            row["QTY"] = Grid.GetRowCellValue(i, "PrentagCredit");

        //            row["StoreName"] = Grid.GetRowCellValue(i, "StoreName");

        //            row["ItemID"] = Grid.GetRowCellValue(i, "ItemID");
        //            row["ItemName"] = Grid.GetRowCellValue(i, ItemName);
        //            row["SizeName"] = Grid.GetRowCellValue(i, SizeName);
        //            row["Date"] = Grid.GetRowCellValue(i, "PrentagDebitDate");
        //            row["Time"] = Grid.GetRowCellValue(i, "PrentagDebitTime");
        //            row["EmpName"] = Grid.GetRowCellValue(i, "EmpName");
        //            dataTable.Rows.Add(row);
        //        }
        //        rptFactoryFactor.DataSource = dataTable;
        //        rptFactoryFactor.DataMember = "rptManu_FactoryBrntageCommendAfter";
        //        return rptFactoryFactor;
        //    }

       



        public XtraReport Manu_FactoryAddtional( GridView Grid,string ColQTy)
            {
                string rptrptManu_FactoryFactorCommendName = "rptManu_FactoryAddtionalCommend";
                string Path = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + @"\Reports\";
                //rptCompanyHeaderName += (UserInfo.Language == iLanguage.English ? "Eng" : "Arb");
                rptrptManu_FactoryFactorCommendName += "Arb";
                XtraReport rptFactoryFactor = XtraReport.FromFile(Path + rptrptManu_FactoryFactorCommendName + ".repx", true);


                var dataTable = new dsReports.rptManu_FactoryFactorCommendBeforeDataTable();
                for (int i = 0; i <= GridViewBeforAddition.DataRowCount - 1; i++)
                {
                    var row = dataTable.NewRow();
                    row["#"] = i + 1;
                    //row["MachinID"] = Grid.GetRowCellValue(i, "MachinID");
                    //row["MachineName"] = Grid.GetRowCellValue(i, "MachineName");
                    row["QTY"] = GridViewBeforAddition.GetRowCellValue(i, "Debit");
                    row["QTYAfter"] =0;

                    row["StoreName"] = GridViewBeforAddition.GetRowCellValue(i, "StoreName");

                    row["ItemID"] = GridViewBeforAddition.GetRowCellValue(i, "ItemID");
                    row["ItemName"] = GridViewBeforAddition.GetRowCellValue(i, ItemName);
                    row["SizeName"] = GridViewBeforAddition.GetRowCellValue(i, SizeName);
                    row["Date"] = GridViewBeforAddition.GetRowCellValue(i, "DebitDate");
                    row["Time"] = GridViewBeforAddition.GetRowCellValue(i, "DebitTime");
                    row["EmpName"] = GridViewBeforAddition.GetRowCellValue(i, "EmpName");
                    dataTable.Rows.Add(row);
                }
                for (int i = 0; i <= gridView20.DataRowCount - 1; i++)
                {
                    var row = dataTable.NewRow();
                    row["#"] = i + 1;
                    //row["MachinID"] = Grid.GetRowCellValue(i, "MachinID");
                    //row["MachineName"] = Grid.GetRowCellValue(i, "MachineName");
                    row["QTY"] = 0;
                    row["QTYAfter"] = gridView20.GetRowCellValue(i, "Credit");
                    row["StoreName"] = gridView20.GetRowCellValue(i, "StoreName");

                    row["ItemID"] = gridView20.GetRowCellValue(i, "ItemID");
                    row["ItemName"] = gridView20.GetRowCellValue(i, ItemName);
                    row["SizeName"] = gridView20.GetRowCellValue(i, SizeName);
                    row["Date"] = gridView20.GetRowCellValue(i, "DebitDate");
                    row["Time"] = gridView20.GetRowCellValue(i, "DebitTime");
                    row["EmpName"] = gridView20.GetRowCellValue(i, "EmpName");
                    dataTable.Rows.Add(row);
                }
            rptFactoryFactor.DataSource = dataTable;
                rptFactoryFactor.DataMember = "rptManu_FactoryAddtionalCommend";
                return rptFactoryFactor;
            }
      
        
        public XtraReport Manu_FactoryTalmeeBefor()
            {
                string rptrptManu_FactoryFactorCommendName = "rptManu_FactoryTalmeeCommendBefore";
                string Path = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + @"\Reports\";
                //rptCompanyHeaderName += (UserInfo.Language == iLanguage.English ? "Eng" : "Arb");
                rptrptManu_FactoryFactorCommendName += "Arb";
                XtraReport rptFactoryFactor = XtraReport.FromFile(Path + rptrptManu_FactoryFactorCommendName + ".repx", true);


                var dataTable = new dsReports.rptManu_FactoryFactorCommendBeforeDataTable();
                for (int i = 0; i <= GridViewBeforPolish.DataRowCount - 1; i++)
                {
                    var row = dataTable.NewRow();
                    row["#"] = i + 1;
                    row["MachinID"] = GridViewBeforPolish.GetRowCellValue(i, "MachinID");
                    row["MachineName"] = GridViewBeforPolish.GetRowCellValue(i, "MachineName");
                    row["QTY"] = GridViewBeforPolish.GetRowCellValue(i, "Debit");
                    row["QTYAfter"] = 0;
                    row["TypeName"] = UserInfo.Language == iLanguage.Arabic ? "بوليشن  1" : "Polishn  1";

                    row["StoreName"] = GridViewBeforPolish.GetRowCellValue(i, "StoreName");

                    row["ItemID"] = GridViewBeforPolish.GetRowCellValue(i, "ItemID");
                    row["ItemName"] = GridViewBeforPolish.GetRowCellValue(i, ItemName);
                    row["SizeName"] = GridViewBeforPolish.GetRowCellValue(i, SizeName);
                    row["Date"] = GridViewBeforPolish.GetRowCellValue(i, "DebitDate");
                    row["Time"] = GridViewBeforPolish.GetRowCellValue(i, "DebitTime");
                    row["EmpName"] = GridViewBeforPolish.GetRowCellValue(i, "EmpName");
                    dataTable.Rows.Add(row);
                }

                for (int i = 0; i <= GridViewAfterPolish.DataRowCount - 1; i++)
                {
                    var row = dataTable.NewRow();
                    row["#"] = i + 1;
                    row["MachinID"] = GridViewAfterPolish.GetRowCellValue(i, "MachinID");
                    row["MachineName"] = GridViewAfterPolish.GetRowCellValue(i, "MachineName");
                    row["QTYAfter"] = GridViewAfterPolish.GetRowCellValue(i, "Credit");
                    row["QTY"] = 0;
                    row["TypeName"] = UserInfo.Language == iLanguage.Arabic ? "بوليشن  1" : "Polishn  1";

                    row["StoreName"] = GridViewAfterPolish.GetRowCellValue(i, "StoreName");

                    row["ItemID"] = GridViewAfterPolish.GetRowCellValue(i, "ItemID");
                    row["ItemName"] = GridViewAfterPolish.GetRowCellValue(i, ItemName);
                    row["SizeName"] = GridViewAfterPolish.GetRowCellValue(i, SizeName);
                    row["Date"] = GridViewAfterPolish.GetRowCellValue(i, "DebitDate");
                    row["Time"] = GridViewAfterPolish.GetRowCellValue(i, "DebitTime");
                    row["EmpName"] = GridViewAfterPolish.GetRowCellValue(i, "EmpName");
                    dataTable.Rows.Add(row);
                }

                for (int i = 0; i <= gridView18.DataRowCount - 1; i++)
                {
                    var row = dataTable.NewRow();
                    row["#"] = i + 1;
                    row["MachinID"] = gridView18.GetRowCellValue(i, "MachinID");
                    row["MachineName"] = gridView18.GetRowCellValue(i, "MachineName");
                    row["QTY"] = gridView18.GetRowCellValue(i, "Debit");
                    row["QTYAfter"] = 0;
                    row["TypeName"] = UserInfo.Language == iLanguage.Arabic ? "بوليشن  2" : "Polishn  2";

                    row["StoreName"] = gridView18.GetRowCellValue(i, "StoreName");

                    row["ItemID"] = gridView18.GetRowCellValue(i, "ItemID");
                    row["ItemName"] = gridView18.GetRowCellValue(i, ItemName);
                    row["SizeName"] = gridView18.GetRowCellValue(i, SizeName);
                    row["Date"] = gridView18.GetRowCellValue(i, "DebitDate");
                    row["Time"] = gridView18.GetRowCellValue(i, "DebitTime");
                    row["EmpName"] = gridView18.GetRowCellValue(i, "EmpName");
                    dataTable.Rows.Add(row);
                }

                for (int i = 0; i <= gridView14.DataRowCount - 1; i++)
                {
                    var row = dataTable.NewRow();
                    row["#"] = i + 1;
                    row["MachinID"] = gridView14.GetRowCellValue(i, "MachinID");
                    row["MachineName"] = gridView14.GetRowCellValue(i, "MachineName");
                    row["QTYAfter"] = gridView14.GetRowCellValue(i, "Credit");
                    row["QTY"] = 0;
                    row["StoreName"] = gridView14.GetRowCellValue(i, "StoreName");
                    row["TypeName"] = UserInfo.Language == iLanguage.Arabic ? "بوليشن  2" : "Polishn  2";
                    row["ItemID"] = gridView14.GetRowCellValue(i, "ItemID");
                    row["ItemName"] = gridView14.GetRowCellValue(i, ItemName);
                    row["SizeName"] = gridView14.GetRowCellValue(i, SizeName);
                    row["Date"] = gridView14.GetRowCellValue(i, "DebitDate");
                    row["Time"] = gridView14.GetRowCellValue(i, "DebitTime");
                    row["EmpName"] = gridView14.GetRowCellValue(i, "EmpName");
                    dataTable.Rows.Add(row);
                }

                for (int i = 0; i <= GridPollishBefore3.DataRowCount - 1; i++)
                {
                    var row = dataTable.NewRow();
                    row["#"] = i + 1;
                    row["MachinID"] = GridPollishBefore3.GetRowCellValue(i, "MachinID");
                    row["MachineName"] = GridPollishBefore3.GetRowCellValue(i, "MachineName");
                    row["QTY"] = GridPollishBefore3.GetRowCellValue(i, "Debit");
                    row["QTYAfter"] = 0;
                    row["TypeName"] = UserInfo.Language == iLanguage.Arabic ? "بوليشن  3" : "Polishn  3";

                    row["StoreName"] = GridPollishBefore3.GetRowCellValue(i, "StoreName");

                    row["ItemID"] = GridPollishBefore3.GetRowCellValue(i, "ItemID");
                    row["ItemName"] = GridPollishBefore3.GetRowCellValue(i, ItemName);
                    row["SizeName"] = GridPollishBefore3.GetRowCellValue(i, SizeName);
                    row["Date"] = GridPollishBefore3.GetRowCellValue(i, "DebitDate");
                    row["Time"] = GridPollishBefore3.GetRowCellValue(i, "DebitTime");
                    row["EmpName"] = GridPollishBefore3.GetRowCellValue(i, "EmpName");
                    dataTable.Rows.Add(row);
                }

                for (int i = 0; i <= GridPollishAfter3.DataRowCount - 1; i++)
                {
                    var row = dataTable.NewRow();
                    row["#"] = i + 1;
                    row["MachinID"] = GridPollishAfter3.GetRowCellValue(i, "MachinID");
                    row["MachineName"] = GridPollishAfter3.GetRowCellValue(i, "MachineName");
                    row["QTYAfter"] = GridPollishAfter3.GetRowCellValue(i, "Credit");
                    row["QTY"] = 0;
                    row["StoreName"] = GridPollishAfter3.GetRowCellValue(i, "StoreName");
                    row["TypeName"] = UserInfo.Language == iLanguage.Arabic ? "بوليشن  3" : "Polishn  3";
                    row["ItemID"] = GridPollishAfter3.GetRowCellValue(i, "ItemID");
                    row["ItemName"] = GridPollishAfter3.GetRowCellValue(i, ItemName);
                    row["SizeName"] = GridPollishAfter3.GetRowCellValue(i, SizeName);
                    row["Date"] = GridPollishAfter3.GetRowCellValue(i, "DebitDate");
                    row["Time"] = GridPollishAfter3.GetRowCellValue(i, "DebitTime");
                    row["EmpName"] = GridPollishAfter3.GetRowCellValue(i, "EmpName");
                    dataTable.Rows.Add(row);
                }
                rptFactoryFactor.DataSource = dataTable;
                rptFactoryFactor.DataMember = "rptManu_FactoryTalmeeCommendBefore";
                return rptFactoryFactor;
            }

            //public XtraReport Manu_FactoryTalmeeAfter(GridView Grid)
            //{
            //    string rptrptManu_FactoryFactorCommendName = "rptManu_FactoryTalmeeCommendAfter";
            //    string Path = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + @"\Reports\";
            //    //rptCompanyHeaderName += (UserInfo.Language == iLanguage.English ? "Eng" : "Arb");
            //    rptrptManu_FactoryFactorCommendName += "Arb";
            //    XtraReport rptFactoryFactor = XtraReport.FromFile(Path + rptrptManu_FactoryFactorCommendName + ".repx", true);


            //    var dataTable = new dsReports.rptManu_FactoryFactorCommendBeforeDataTable();
            //    for (int i = 0; i <= Grid.DataRowCount - 1; i++)
            //    {
            //        var row = dataTable.NewRow();
            //        row["#"] = i + 1;
            //        row["MachinID"] = Grid.GetRowCellValue(i, "MachinID");
            //        row["MachineName"] = Grid.GetRowCellValue(i, "MachineName");
            //        row["QTY"] = Grid.GetRowCellValue(i, "Credit");

            //        row["StoreName"] = Grid.GetRowCellValue(i, "StoreName");
            //        row["ItemID"] = Grid.GetRowCellValue(i, "ItemID");
            //        row["ItemName"] = Grid.GetRowCellValue(i, ItemName);
            //        row["SizeName"] = Grid.GetRowCellValue(i, SizeName);
            //        row["Date"] = Grid.GetRowCellValue(i, "DebitDate");
            //        row["Time"] = Grid.GetRowCellValue(i, "DebitTime");
            //        row["EmpName"] = Grid.GetRowCellValue(i, "EmpName");
            //        dataTable.Rows.Add(row);
            //    }
            //    rptFactoryFactor.DataSource = dataTable;
            //    rptFactoryFactor.DataMember = "rptManu_FactoryTalmeeCommendAfter";
            //    return rptFactoryFactor;
            //}

            public XtraReport Manu_FactoryCompoundBefor()
            {
                string rptrptManu_FactoryFactorCommendName = "rptManu_FactoryCompoundCommendBefore";
                string Path = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + @"\Reports\";
                //rptCompanyHeaderName += (UserInfo.Language == iLanguage.English ? "Eng" : "Arb");
                rptrptManu_FactoryFactorCommendName += "Arb";
                XtraReport rptFactoryFactor = XtraReport.FromFile(Path + rptrptManu_FactoryFactorCommendName + ".repx", true);


                var dataTable = new dsReports.rptManu_FactoryCompoundCommendDataTable();
                for (int i = 0; i <= gridViewBeforCompond.DataRowCount - 1; i++)
                {
                    var row = dataTable.NewRow();
                    row["#"] = i + 1;
                    row["BarCode"] = gridViewBeforCompond.GetRowCellValue(i, "BarcodCompond");

                    row["TypeStone"] = gridViewBeforCompond.GetRowCellValue(i, "TypeSton");
                    row["QTYAfter"] = 0;
                    row["ItemName"] = gridViewBeforCompond.GetRowCellValue(i, ItemName);
                    row["CostPrice"] = gridViewBeforCompond.GetRowCellValue(i, "CostPrice");
                    row["QTY"] = gridViewBeforCompond.GetRowCellValue(i, "GoldDebit");
                    row["SizeName"] = gridViewBeforCompond.GetRowCellValue(i, SizeName);
                    row["DebitDate"] = gridViewBeforCompond.GetRowCellValue(i, "DebitDate");
                    row["QTYStone"] = gridViewBeforCompond.GetRowCellValue(i, "ComWeightSton");
                    row["AccountName"] = gridViewBeforCompond.GetRowCellValue(i, "FromAccountName");
                    row["EmpCompundName"] = gridViewBeforCompond.GetRowCellValue(i, "EmpCompundName");
                    dataTable.Rows.Add(row);
                }
                for (int i = 0; i <= gridViewAfterCompond.DataRowCount - 1; i++)
                {
                    var row = dataTable.NewRow();
                    row["#"] = i + 1;
                    row["BarCode"] = gridViewAfterCompond.GetRowCellValue(i, "BarcodCompond");
                    row["TypeStone"] = gridViewAfterCompond.GetRowCellValue(i, "TypeSton");
                    row["ItemName"] = gridViewAfterCompond.GetRowCellValue(i, ItemName);
                    row["CostPrice"] = gridViewAfterCompond.GetRowCellValue(i, "CostPrice");
                    row["QTYStone"] = 0;
                    row["QTYAfter"] = gridViewAfterCompond.GetRowCellValue(i, "ComWeightSton");
                    row["SizeName"] = gridViewAfterCompond.GetRowCellValue(i, SizeName);
                    row["DebitDate"] = gridViewAfterCompond.GetRowCellValue(i, "DebitDate").ToString();
                    row["AccountName"] = gridViewAfterCompond.GetRowCellValue(i, "FromAccountName");
                    row["EmpCompundName"] = gridViewAfterCompond.GetRowCellValue(i, "EmpCompundName");

                    dataTable.Rows.Add(row);
                }
                rptFactoryFactor.DataSource = dataTable;
                rptFactoryFactor.DataMember = "rptManu_FactoryCompoundCommendBefore";
                return rptFactoryFactor;
            }

            //public XtraReport Manu_FactoryCompoundAfter()
            //{
            //    string rptrptManu_FactoryFactorCommendName = "rptManu_FactoryCompoundCommendAfter";
            //    string Path = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + @"\Reports\";
            //    //rptCompanyHeaderName += (UserInfo.Language == iLanguage.English ? "Eng" : "Arb");
            //    rptrptManu_FactoryFactorCommendName += "Arb";
            //    XtraReport rptFactoryFactor = XtraReport.FromFile(Path + rptrptManu_FactoryFactorCommendName + ".repx", true);
            //    var dataTable = new dsReports.rptManu_FactoryCompoundCommendDataTable();
            //    for (int i = 0; i <= gridViewAfterCompond.DataRowCount - 1; i++)
            //    {
            //        var row = dataTable.NewRow();
            //        row["#"] = i + 1;
            //        row["BarCode"] = gridViewAfterCompond.GetRowCellValue(i, "BarcodCompond");
            //        row["TypeStone"] = gridViewAfterCompond.GetRowCellValue(i, "TypeSton");
            //        row["ItemName"] = gridViewAfterCompond.GetRowCellValue(i, ItemName);
            //        row["CostPrice"] = gridViewAfterCompond.GetRowCellValue(i, "CostPrice");
            //        row["QTY"] = gridViewAfterCompond.GetRowCellValue(i, "GoldCredit");
            //        row["QTYStone"] = gridViewAfterCompond.GetRowCellValue(i, "ComWeightSton");
            //        row["SizeName"] = gridViewAfterCompond.GetRowCellValue(i, SizeName);
            //        row["DebitDate"] = gridViewAfterCompond.GetRowCellValue(i, "DebitDate").ToString();
            //        row["AccountName"] = gridViewAfterCompond.GetRowCellValue(i, "FromAccountName");
            //        row["EmpCompundName"] = gridViewAfterCompond.GetRowCellValue(i, "EmpCompundName");

            //        dataTable.Rows.Add(row);
            //    }
            //    rptFactoryFactor.DataSource = dataTable;
            //    rptFactoryFactor.DataMember = "rptManu_FactoryCompoundCommendAfter";
            //    return rptFactoryFactor;
            //}
            public void Find()
            {

                CSearch cls = new CSearch();
                int[] ColumnWidth = new int[] { 100, 300 };
                string SearchSql = "";
                string Condition = "Where 1=1";

                FocusedControl = GetIndexFocusedControl();
                if (FocusedControl == null) return;
                
                else if (FocusedControl.Trim() == txtOrderID.Name)
                {
                    //if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; };
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, txtOrderID, null, "OrderIDCad", "رقم الطلب",   Comon.cInt(cmbBranchesID.EditValue));
                    else
                        PrepareSearchQuery.Find(ref cls, txtOrderID, null, "OrderIDCad", "Order ID",   Comon.cInt(cmbBranchesID.EditValue));
                }
                else
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, txtOrderID, null, "OrderIDCad", "رقم الطلب",   Comon.cInt(cmbBranchesID.EditValue));
                    else
                        PrepareSearchQuery.Find(ref cls, txtOrderID, null, "OrderIDCad", "Order ID",   Comon.cInt(cmbBranchesID.EditValue));
                }
                GetSelectedSearchValue(cls);
            }
            string GetIndexFocusedControl()
            {
                Control c = this.ActiveControl;
                if (c == null) return null;
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
            public void GetSelectedSearchValue(CSearch cls)
            {
                if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
                {
                   if (FocusedControl == txtOrderID.Name)
                    {
                        txtOrderID.Text = cls.PrimaryKeyValue.ToString();
                        txtOrderID_Validating(null, null);
                    }
                   else
                   {
                       txtOrderID.Text = cls.PrimaryKeyValue.ToString();
                       txtOrderID_Validating(null, null);
                   }
                }
            }
            public void ReadTopInfo(string OrderID, bool flag = false)
            {
                try
                {
                    ClearFieldsTop();
                    {
                        dt = Manu_OrderRestrictionDAL.frmGetDataDetalByID(OrderID, Comon.cInt(  Comon.cInt(cmbBranchesID.EditValue)), UserInfo.FacilityID);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            txtOrderID.Text = dt.Rows[0]["OrderID"].ToString();
                            cmbTypeOrders.EditValue = Comon.cInt(dt.Rows[0]["TypeOrdersID"].ToString());
                            txtOrderDate.DateTime = DateTime.ParseExact(Comon.ConvertSerialDateTo(dt.Rows[0]["OrderDate"].ToString()), "dd/MM/yyyy", culture);
                            //Validate
                            txtCustomerID.Text = dt.Rows[0]["CustomerID"].ToString();
                            txtCustomerID_Validating(null, null);
                            txtGuidanceID.Text = dt.Rows[0]["GuidanceID"].ToString();
                            txtGuidanceID_Validating(null, null);
                            txtDelegateID.Text = dt.Rows[0]["DelegateID"].ToString();
                            txtDelegateID_Validating(null, null);
                        }
                        else
                        {
                            Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "لا يوجد طلبية تمتلك هذا الرقم .. الرجاء ادخال رقم الطلبية الصحيح" : "There is no order that has this number. Please enter the correct order number");
                            txtOrderID.Text = "";
                        }
                    }
                }
                catch
                {
                }
            }
            public void ClearFieldsTop()
            {
                try
                {
                    txtCustomerID.ReadOnly = true;
                    txtDelegateID.ReadOnly = true;
                    txtOrderDate.ReadOnly = true;
                    txtGuidanceID.ReadOnly = true;
                    cmbTypeOrders.ReadOnly = true;
                    txtDelegateID.Text = "";
                    txtDelegateID_Validating(null, null);
                    txtCustomerID.Text = "";
                    txtCustomerID_Validating(null, null);
                    txtGuidanceID.Text = "";
                    txtGuidanceID_Validating(null, null);
                }
                catch
                {

                }
            }
            private DataTable GetOrderDetail(string OrderID, int CNDTYPE)
            {

                DataTable dt = Manu_ZirconDiamondFactoryDAL.frmGetDataDetailByOrderID(OrderID, Comon.cInt(  Comon.cInt(cmbBranchesID.EditValue)), UserInfo.FacilityID, 6, CNDTYPE);

                return dt;
            }
            private void GetOrderDetail(string OrderID)
            {
            #region Stage Before Casting
                DataTable dt = Mnu_OrderRunningReportDAL.frmGetDataDetailByOrderID(OrderID,   Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID,1);
                DataTable dt2 = Mnu_OrderRunningReportDAL.frmGetDataDetailByOrderID(OrderID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID, 2);

                DataTable dt3 = Mnu_OrderRunningReportDAL.frmGetDataDetailByOrderID(OrderID,  Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID, 3);
                dt.Merge(dt2);
                dt.Merge(dt3);
                gridControl1.DataSource = lstDetail;
                    if (dt.Rows.Count > 0)
                    {
                        gridControl1.DataSource = dt;
                        CalculateTotal();
                    }                
            #endregion
            #region Factory
                //Before
                DataTable dt4 = Mnu_OrderRunningReportDAL.frmGetDataDetailByOrderID(OrderID, Comon.cInt(  Comon.cInt(cmbBranchesID.EditValue)), UserInfo.FacilityID, 4);               
                  gridControlfactroOpretion.DataSource = lstDetailfactory;
                    if (dt4.Rows.Count > 0)
                     gridControlfactroOpretion.DataSource = dt4; 
                 
                //After
                DataTable dt5 = Mnu_OrderRunningReportDAL.frmGetDataDetailByOrderID(OrderID, Comon.cInt(  Comon.cInt(cmbBranchesID.EditValue)), UserInfo.FacilityID, 5);
                 gridControlAfterFactory.DataSource = lstDetailAfterfactory;
                    if (dt5.Rows.Count > 0)
                      gridControlAfterFactory.DataSource = dt5;                     
                 
            #endregion
             #region Prntage 1
                //Before
                DataTable dt6 = Mnu_OrderRunningReportDAL.frmGetDataDetailByOrderID(OrderID, Comon.cInt(  Comon.cInt(cmbBranchesID.EditValue)), UserInfo.FacilityID, 6,7);
                gridControlBeforPrentag.DataSource = lstDetailPrentage1;
                    if (dt6.Rows.Count > 0)
                        gridControlBeforPrentag.DataSource = dt6;
                
                //After
                DataTable dt7 = Mnu_OrderRunningReportDAL.frmGetDataDetailByOrderID(OrderID, Comon.cInt(  Comon.cInt(cmbBranchesID.EditValue)), UserInfo.FacilityID, 7,7);
                 gridControlAfterPrentage.DataSource = lstDetailAfterPrentage1;
                    if (dt7.Rows.Count > 0)
                        gridControlAfterPrentage.DataSource = dt7;

            #endregion
            #region Prntage 2
            //Before
            DataTable dtPrntage21 = Mnu_OrderRunningReportDAL.frmGetDataDetailByOrderID(OrderID, Comon.cInt(  Comon.cInt(cmbBranchesID.EditValue)), UserInfo.FacilityID, 6,12);
            gridControl3.DataSource = lstDetailPrentage2;
            if (dtPrntage21.Rows.Count > 0)
                gridControl3.DataSource = dtPrntage21;

            //After
            DataTable dtPrntage2 = Mnu_OrderRunningReportDAL.frmGetDataDetailByOrderID(OrderID, Comon.cInt(  Comon.cInt(cmbBranchesID.EditValue)), UserInfo.FacilityID, 7,12);
            gridControl2.DataSource = lstDetailAfterPrentage2;
            if (dtPrntage2.Rows.Count > 0)
                gridControl2.DataSource = dtPrntage2;

            #endregion
            #region compound
            //Before
            DataTable dt8 = Mnu_OrderRunningReportDAL.frmGetDataDetailByOrderID(OrderID, Comon.cInt(  Comon.cInt(cmbBranchesID.EditValue)), UserInfo.FacilityID, 8);
                gridControlBeforCompond.DataSource = lstDetailCompund;
                    if (dt8.Rows.Count > 0)
                        gridControlBeforCompond.DataSource = dt8;                
                //After
                DataTable dt9 = Mnu_OrderRunningReportDAL.frmGetDataDetailByOrderID(OrderID, Comon.cInt(  Comon.cInt(cmbBranchesID.EditValue)), UserInfo.FacilityID, 9);
                gridControlAfterCompond.DataSource = lstDetailAfterCompund;
                    if (dt9.Rows.Count > 0)
                        gridControlAfterCompond.DataSource = dt9;
                
                #endregion
             #region Talmee 1
                    //Before
                    DataTable dt10 = Mnu_OrderRunningReportDAL.frmGetDataDetailByOrderID(OrderID, Comon.cInt(  Comon.cInt(cmbBranchesID.EditValue)), UserInfo.FacilityID, 10,8);
                    gridControlBeforePolishing.DataSource = lstDetailTalmee1;
                    if (dt10.Rows.Count > 0)
                        gridControlBeforePolishing.DataSource = dt10;
                    //After
                    DataTable dt11 = Mnu_OrderRunningReportDAL.frmGetDataDetailByOrderID(OrderID, Comon.cInt(  Comon.cInt(cmbBranchesID.EditValue)), UserInfo.FacilityID, 11,8);
                    gridControlAfterPolishing.DataSource = lstDetailAfterTalmee1;
                    if (dt11.Rows.Count > 0)
                        gridControlAfterPolishing.DataSource = dt11;

            #endregion

            #region Talmee
            //Before
                DataTable dtPolishn11 = Mnu_OrderRunningReportDAL.frmGetDataDetailByOrderID(OrderID, Comon.cInt(  Comon.cInt(cmbBranchesID.EditValue)), UserInfo.FacilityID, 10,13);
                gridControl5.DataSource = lstDetailTalmee2;
                if (dtPolishn11.Rows.Count > 0)
                    gridControl5.DataSource = dtPolishn11;
                //After
                DataTable dtPolishn12 = Mnu_OrderRunningReportDAL.frmGetDataDetailByOrderID(OrderID, Comon.cInt(  Comon.cInt(cmbBranchesID.EditValue)), UserInfo.FacilityID, 11,13);
                gridControl4.DataSource = lstDetailAfterTalmee2;
                if (dtPolishn12.Rows.Count > 0)
                    gridControl4.DataSource = dtPolishn12;

            #endregion
                #region Talmee3
                //Before
                DataTable dtPolishn14 = Mnu_OrderRunningReportDAL.frmGetDataDetailByOrderID(OrderID, Comon.cInt(  Comon.cInt(cmbBranchesID.EditValue)), UserInfo.FacilityID, 10, 14);
                gridControl7.DataSource = lstDetailTalmee3;
                if (dtPolishn14.Rows.Count > 0)
                    gridControl7.DataSource = dtPolishn14;
                //After
                DataTable dtPolishn142 = Mnu_OrderRunningReportDAL.frmGetDataDetailByOrderID(OrderID, Comon.cInt(  Comon.cInt(cmbBranchesID.EditValue)), UserInfo.FacilityID, 11, 14);
                gridControl6.DataSource = lstDetailAfterTalmee3;
                if (dtPolishn142.Rows.Count > 0)
                    gridControl6.DataSource = dtPolishn142;

                #endregion
            #region Additional
            //Before
            DataTable dtAdditional11 = Mnu_OrderRunningReportDAL.frmGetDataDetailByOrderID(OrderID, Comon.cInt(  Comon.cInt(cmbBranchesID.EditValue)), UserInfo.FacilityID,12);
            gridControlBeforeAdditional.DataSource = lstDetailAdditional;
            if (dtAdditional11.Rows.Count > 0)
                gridControlBeforeAdditional.DataSource = dtAdditional11;
            //After
            DataTable dtAdditional12 = Mnu_OrderRunningReportDAL.frmGetDataDetailByOrderID(OrderID, Comon.cInt(  Comon.cInt(cmbBranchesID.EditValue)), UserInfo.FacilityID, 13);
            gridControlAfterAdditional.DataSource = lstDetailAfterAdditional;
            if (dtAdditional12.Rows.Count > 0)
                gridControlAfterAdditional.DataSource = dtAdditional12;
            
            #endregion

            decimal QTYGold = Comon.cDec(Lip.GetValue("SELECT   Min([QTY]) as QTY  FROM  [Manu_AllOrdersDetails] INNER JOIN  dbo.Stc_Items ON dbo.Manu_AllOrdersDetails.ItemID = dbo.Stc_Items.ItemID and dbo.Manu_AllOrdersDetails.BranchID = dbo.Stc_Items.BranchID  where Stc_Items.BaseID=5 and Manu_AllOrdersDetails.[BranchID]=" +   Comon.cInt(cmbBranchesID.EditValue) + " and Manu_AllOrdersDetails.OrderID='" + txtOrderID.Text + "'"));
            txtTotalGold.Text = QTYGold.ToString();


            DataTable dtOrderDetail = GetOrderDetail(txtOrderID.Text, 8);
            var ZirconeQTY = "0";
            var BagateQTY = "0";
            var RountDaimondQTY = "0";
            var CatenaryQTY = "0";
            var BagateQTYCustomer = "0";
            var RountDaimondQTYCustomer = "0";

            DataTable dtorderDiamondZircon = Lip.SelectRecord(@"
                    SELECT 
                        mfc.ComWeightSton as QTY, 
                        mfc.SizeID, 
                        si.BaseID, 
                        si.TypeID,
                        si.IsService
                    FROM 
                        dbo.Menu_FactoryRunCommandMaster mfm
                        INNER JOIN dbo.Menu_FactoryRunCommandCompund mfc ON mfm.ComandID = mfc.ComandID and mfm.BranchID = mfc.BranchID AND mfm.TypeStageID = mfc.TypeStageID
                        INNER JOIN dbo.Stc_Items si ON mfc.ItemID = si.ItemID and mfc.BranchID = si.BranchID
                    WHERE 
                        (mfc.TypeOpration = 2) 
                        AND (mfm.TypeStageID = 9) 
                        AND (mfm.Cancel = 0) 
                        AND mfm.BranchID = " +   Comon.cInt(cmbBranchesID.EditValue) + @" 
                        AND mfm.Barcode = " + txtOrderID.Text + @"
                    UNION ALL
                    SELECT 
                        mfc.QTY, 
                        mfc.SizeID, 
                        si.BaseID, 
                        si.TypeID,
                        si.IsService
                    FROM 
                        dbo.Manu_ZirconDiamondFactoryMaster mfm
                        INNER JOIN dbo.Manu_ZirconDiamondFactoryDetails mfc ON mfm.CommandID = mfc.CommandID and mfm.BranchID = mfc.BranchID AND mfm.TypeStageID = mfc.TypeStageID
                        INNER JOIN dbo.Stc_Items si ON mfc.ItemID = si.ItemID and mfc.BranchID = si.BranchID
                    WHERE 
                        (mfm.Cancel = 0) 
                        AND mfm.BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + @" 
                        AND mfm.OrderID = " + txtOrderID.Text);

            if (dtorderDiamondZircon.Rows.Count > 0)
            {
                ZirconeQTY = dtorderDiamondZircon.Compute("SUM(QTY)", "BaseID = 4 and TypeID = 1").ToString();

                var groupedData = dtorderDiamondZircon.AsEnumerable()
                    .GroupBy(row => new { BaseID = row.Field<int>("BaseID"), IsService = row.Field<int>("IsService"), SizeID = row.Field<int>("SizeID") })
                    .Select(grp => new
                    {
                        BaseID = grp.Key.BaseID,
                        IsService = grp.Key.IsService,
                        SizeID = grp.Key.SizeID,
                        TotalQTY = grp.Sum(r => r.Field<double>("QTY"))
                    });
                BagateQTY = groupedData
              .Where(x => x.BaseID == 2 && x.IsService == 0)
              .Sum(x => x.TotalQTY)
              .ToString();


                RountDaimondQTY = groupedData
                    .Where(x => x.BaseID == 3 && x.IsService == 0)
                    .Sum(x => x.TotalQTY)
                    .ToString();

                BagateQTYCustomer = groupedData
                    .Where(x => x.BaseID == 2 && x.IsService == 1)
                    .Sum(x =>  x.TotalQTY)
                    .ToString();

                RountDaimondQTYCustomer = groupedData
                    .Where(x => x.BaseID == 3 && x.IsService == 1)
                    .Sum(x => x.TotalQTY )
                    .ToString();


                //BagateQTY = groupedData
                //  .Where(x => x.BaseID == 2 && x.IsService == 0)
                //  .Sum(x => x.SizeID == 2 ? x.TotalQTY / 5 : x.TotalQTY)
                //  .ToString();


                //RountDaimondQTY = groupedData
                //    .Where(x => x.BaseID == 3 && x.IsService == 0)
                //    .Sum(x => x.SizeID == 2 ? x.TotalQTY / 5 : x.TotalQTY)
                //    .ToString();

                //BagateQTYCustomer = groupedData
                //    .Where(x => x.BaseID == 2 && x.IsService == 1)
                //    .Sum(x => x.SizeID == 2 ? x.TotalQTY / 5 : x.TotalQTY)
                //    .ToString();

                //RountDaimondQTYCustomer = groupedData
                //    .Where(x => x.BaseID == 3 && x.IsService == 1)
                //    .Sum(x => x.SizeID == 2 ? x.TotalQTY / 5 :x.TotalQTY)
                //    .ToString();

            }

            if (dtOrderDetail.Rows.Count > 0)
                     CatenaryQTY = dtOrderDetail.Compute("SUM(QTY)", "BaseID = 1").ToString();

            txtTotalAdditional.Text = CatenaryQTY.ToString();
            txtZirCode.Text = ZirconeQTY.ToString();
            txtTotalBagit.Text = BagateQTY.ToString();
            txtRoundQTY.Text = RountDaimondQTY.ToString();
            txtTotalBagitCustomer.Text = BagateQTYCustomer.ToString();
            txtRoundQTYCustomer.Text = RountDaimondQTYCustomer.ToString();
            decimal TotalQTY = Comon.cDec(Lip.GetValue("SELECT  [TotalOrderQTY] FROM [Manu_CloseOrdersMaster] where [Cancel]=0 and BranchID="+Comon.cInt(cmbBranchesID.EditValue)+" and  [OrderID]=" + txtOrderID.Text));
            txtAllTotalQTYOrder.Text = Comon.cDec(TotalQTY).ToString();
        }

        #endregion
        private void frmOrderRunningReport_KeyDown(object sender, KeyEventArgs e)
            {
                if (e.KeyCode == Keys.F3)
                    Find();
               else  if (e.KeyCode == Keys.F9)
                    DoSave();
            }
            void CalculateTotal()
            {
                try
                {
                    decimal ToatlQty = 0;
                    decimal ToatlCostPrice = 0;
                    decimal TotalGoldQTYCloves = 0;
                    for (int i = 0; i <= GridCad.DataRowCount - 1; i++)
                    {
                        ToatlQty += Comon.cDec(GridCad.GetRowCellValue(i, "QTY").ToString());
                        ToatlCostPrice += Comon.ConvertToDecimalPrice(GridCad.GetRowCellValue(i, "TotalCost").ToString());
                        TotalGoldQTYCloves += Comon.ConvertToDecimalPrice(GridCad.GetRowCellValue(i, "GoldQTYCloves").ToString());
                    }
                    txtGoldQTYCloves.Text = TotalGoldQTYCloves.ToString();
                    lblTotalOrderGold.Text = Comon.cDec(Comon.cDec(lblEquQty.Text) * Comon.cDec(ToatlQty)).ToString();
                   
                }
                catch (Exception ex)
                { }
            }

            private void label16_Click(object sender, EventArgs e)
            {

            }

       private void simpleButton1_Click(object sender, EventArgs e)
          {
            try
            {
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);

                /******************** Report Body *************************/
                
                bool IncludeHeader = true;
                string rptFormName = "rptOrdersImage";
                XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);
                var dataTable = new dsReports.rptOrderRestrictionDataTable();
                dataTable.Rows.Clear();
                var row = dataTable.NewRow();
                row["BarCode"] = txtOrderID.Text;
                row["BarCodeImage"] =txtImageCode.Text;
                if (picItemImage.Image != null)
                {
                    // تحميل الصورة إلى التقرير
                    byte[] imageBytes;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        picItemImage.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        imageBytes = ms.ToArray();
                    }
                    row["Pic"] = imageBytes;
                }
                dataTable.Rows.Add(row);
                rptForm.DataSource = dataTable;
                rptForm.DataMember = ReportName;

                /******************** Report Binding ************************/
                XRSubreport subreport = (XRSubreport)rptForm.FindControl("subRptCompanyHeader", true);
                subreport.Visible = IncludeHeader;
                subreport.ReportSource = ReportComponent.CompanyHeader();
                rptForm.ShowPrintStatusDialog = false;
                rptForm.ShowPrintMarginsWarning = false;
                rptForm.CreateDocument();

                SplashScreenManager.CloseForm(false);
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
                    if (dt.Rows.Count > 0)
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

    }
}
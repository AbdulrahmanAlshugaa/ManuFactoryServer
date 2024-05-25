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
using Edex.Model.Language;
using Edex.Model;
using Edex.GeneralObjects.GeneralClasses;
using Edex.ModelSystem;
using Edex.DAL.Stc_itemDAL;
using DevExpress.XtraSplashScreen;

namespace Edex.StockObjects.Transactions
{
    public partial class frmTranseferItemToOnetherIem : BaseForm
    {
        private string strSQL = "";
        private string where = "";
        private string lang = "";
        private string FocusedControl = "";
        private string PrimaryName;
        public frmTranseferItemToOnetherIem()
        {
            InitializeComponent();
            //ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[2].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[3].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[4].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[5].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[7].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[8].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[9].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[11].Visible = false;
            PrimaryName = "ArbName";
            if (UserInfo.Language == iLanguage.English)
            {
                PrimaryName = "EngName";
            }
              FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
               cmbBranchesID.EditValue = MySession.GlobalBranchID;
             
            this.KeyDown += frmTranseferItemToOnetherIem_KeyDown;
            this.txtFromItemNo.Validating+=txtFromItemNo_Validating;
            this.txtItemName.Validating += txtItemName_Validating;
        }

        void txtItemName_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as ItemName FROM Stc_Items WHERE ItemID =" + Comon.cInt(txtItemName.Text) + " And Cancel =0 and BranchID=" + MySession.GlobalBranchID;
                CSearch.ControlValidating(txtItemName,lblToItemName, strSQL);//This Call  Function For Set  TypeName to txttypeName when The user Select TypeID
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }   
        }
        private void txtFromItemNo_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as ItemName FROM Stc_Items WHERE ItemID =" + Comon.cInt(txtFromItemNo.Text) + " And Cancel =0 and BranchID=" + MySession.GlobalBranchID;
                CSearch.ControlValidating(txtFromItemNo  , lblFromItemName, strSQL);//This Call  Function For Set  TypeName to txttypeName when The user Select TypeID
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        
        void frmTranseferItemToOnetherIem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3 || e.KeyCode == Keys.F4)
                Find();
            if (e.KeyCode == Keys.F5)
                DoPrint();
        }

        public bool Find()
        {

            CSearch cls = new CSearch();
            int[] ColumnWidth = new int[] { 100, 300 };
            string SearchSql = "";
            string Condition = "Where 1=1";
            FocusedControl = GetIndexFocusedControl();
            if (FocusedControl.Trim() == txtGroupID.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Search(txtGroupID, lblGroupID, "GroupID", "اسـم المجـمـوعة", "رقـم المجـمـوعة");
                else
                    PrepareSearchQuery.Search(txtGroupID, lblGroupID, "GroupID", "Group Name", "Group ID");
            }      
             
            if (FocusedControl.Trim() == txtFromItemNo.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtFromItemNo, lblFromItemName, "Items", "رقـم الـمــادة", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtFromItemNo, lblFromItemName, "Items", "Item ID", MySession.GlobalBranchID);
            }
            if (FocusedControl.Trim() == txtItemName.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtItemName, lblFromItemName, "Items", "رقـم الـمــادة", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtItemName, lblFromItemName, "Items", "Item ID", MySession.GlobalBranchID);
            }
            return GetSelectedSearchValue(cls);
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
                if (FocusedControl.Trim() == txtItemName.Name)
                {
                    txtItemName_Validating(null, null);
                }
                else if (FocusedControl.Trim() ==txtFromItemNo.Name)
                {
                    txtFromItemNo_Validating(null, null);
                }
                return true;
            }
            return false;
        }
        private void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
               

                string GroupID ="0"+ Lip.GetValue("SELECT  [GroupID]  FROM  [Stc_Items] where Cancel=0 and BranchID=" + MySession.GlobalBranchID+"  and  [ItemID]=" + Comon.cLong(txtFromItemNo.Text));
                if(GroupID==txtGroupID.Text.ToString())
                  {
                      Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "الصنف بالفعل موجود في المجموعة المحددة " : "The item already exists in the specified group");
                      return;
                  }
                if (!Validations.IsValidForm(this))
                    return;
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                if (txtFromItemNo.Text != "" && txtGroupID.Text != "")
                {
                  

                    string ItemIDAfter;
                    string groupid =  txtGroupID.Text.ToString();
                    long dtMaxBarcode = Comon.cLong(Lip.GetValue("SELECT Max(ItemID)+1 FROM Stc_Items WHERE GroupID=" + groupid + " and BranchID=" + MySession.GlobalBranchID));
                    if (dtMaxBarcode == 0)
                        ItemIDAfter = groupid + (Comon.cLong("1").ToString()).PadLeft(3, '0');
                    else
                        ItemIDAfter = "0" + (Comon.cLong(dtMaxBarcode).ToString());
                    DataTable dtfromItemUnits = Lip.SelectRecord("SELECT  [SizeID]  FROM  [Stc_ItemUnits] where UnitCancel=0 and BranchID=" + MySession.GlobalBranchID+" and [ItemID]=" + Comon.cLong(txtFromItemNo.Text) + " order by SizeID");
                    string BarCode = "";
                    for (int i = 0; i < dtfromItemUnits.Rows.Count; i++)
                    {

                        BarCode = ItemIDAfter +dtfromItemUnits.Rows[i]["SizeID"].ToString();
                        
                        //update To Table Stc_ItemUnits
                        Lip.NewFields();
                        Lip.Table = "Stc_ItemUnits";
                        Lip.AddNumericField("BarCode", BarCode.ToString());
                        Lip.AddNumericField("ItemID", ItemIDAfter.ToString());
                        Lip.sCondition = " UnitCancel =0  and SizeID =" + dtfromItemUnits.Rows[i]["SizeID"] + " and BranchID=" + MySession.GlobalBranchID+" And    ItemID='" + txtFromItemNo.Text.ToString() + "'";
                        Lip.ExecuteUpdate();
                        // Update All Table 
                        UpdateItemIDandBarCodeWhenChangToOntherGroup("Stc_ItemsMoviing", "BarCode", BarCode, txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtfromItemUnits.Rows[i]["SizeID"].ToString()), groupid);
                        UpdateItemIDandBarCodeWhenChangToOntherGroup("Manu_AllOrdersDetails", "BarCode", BarCode, txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtfromItemUnits.Rows[i]["SizeID"].ToString()), "-1");
                        UpdateItemIDandBarCodeWhenChangToOntherGroup("Sales_PurchaseInvoiceDetails", "BarCode", BarCode, txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtfromItemUnits.Rows[i]["SizeID"].ToString()), "-1");
                        UpdateItemIDandBarCodeWhenChangToOntherGroup("Sales_SalesInvoiceDetails", "BarCode", BarCode, txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtfromItemUnits.Rows[i]["SizeID"].ToString()), "-1");
                        UpdateItemIDandBarCodeWhenChangToOntherGroup("Sales_SalesInvoiceReturnDetails", "BarCode", BarCode, txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtfromItemUnits.Rows[i]["SizeID"].ToString()), "-1");
                        UpdateItemIDandBarCodeWhenChangToOntherGroup("Sales_SalesServiceInvoiceDetails", "BarCode", BarCode, txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtfromItemUnits.Rows[i]["SizeID"].ToString()), "-1");
                        UpdateItemIDandBarCodeWhenChangToOntherGroup("Sales_PurchaseInvoiceReturnDetails", "BarCode", BarCode, txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtfromItemUnits.Rows[i]["SizeID"].ToString()), "-1");
                        UpdateItemIDandBarCodeWhenChangToOntherGroup("Sales_PurchaseInvoiceDetails", "BarCode", BarCode, txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtfromItemUnits.Rows[i]["SizeID"].ToString()), "-1");
                        UpdateItemIDandBarCodeWhenChangToOntherGroup("Sales_PurchaseSaveInvoiceDetails", "BarCode", BarCode, txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtfromItemUnits.Rows[i]["SizeID"].ToString()), "-1");
                        UpdateItemIDandBarCodeWhenChangToOntherGroup("Sales_PurchaseOrderFromPurchaseDetails", "BarCode", BarCode, txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtfromItemUnits.Rows[i]["SizeID"].ToString()), "-1");
                        UpdateItemIDandBarCodeWhenChangToOntherGroup("Sales_PurchaseSaveInvoiceReturnDetails", "BarCode", BarCode, txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtfromItemUnits.Rows[i]["SizeID"].ToString()), "-1");
                        UpdateItemIDandBarCodeWhenChangToOntherGroup("Stc_GoldInonBail_Details", "BarCode", BarCode, txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtfromItemUnits.Rows[i]["SizeID"].ToString()), "-1");
                        UpdateItemIDandBarCodeWhenChangToOntherGroup("Stc_GoldOutOnBail_Details", "BarCode", BarCode, txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtfromItemUnits.Rows[i]["SizeID"].ToString()), "-1");
                        UpdateItemIDandBarCodeWhenChangToOntherGroup("Stc_GoodOpeningDetails", "BarCode", BarCode, txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtfromItemUnits.Rows[i]["SizeID"].ToString()), "-1");
                        UpdateItemIDandBarCodeWhenChangToOntherGroup("Stc_MatirialInonBail_Details", "BarCode", BarCode, txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtfromItemUnits.Rows[i]["SizeID"].ToString()), "-1");
                        UpdateItemIDandBarCodeWhenChangToOntherGroup("Stc_MatirialOutOnBail_Details", "BarCode", BarCode, txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtfromItemUnits.Rows[i]["SizeID"].ToString()), "-1");
                        UpdateItemIDandBarCodeWhenChangToOntherGroup("Stc_TransferMultipleStoresGold_Details", "BarCode", BarCode, txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtfromItemUnits.Rows[i]["SizeID"].ToString()), "-1");
                        UpdateItemIDandBarCodeWhenChangToOntherGroup("Stc_TransferMultipleStoresMatirial_Details", "BarCode", BarCode, txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtfromItemUnits.Rows[i]["SizeID"].ToString()), "-1");
                        UpdateItemIDandBarCodeWhenChangToOntherGroup("Sales_PurchaseOrderDetails", "BarCode", BarCode, txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtfromItemUnits.Rows[i]["SizeID"].ToString()), "-1");
                        UpdateItemIDandBarCodeWhenChangToOntherGroup("Sales_SalesOrderDetails", "BarCode", BarCode, txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtfromItemUnits.Rows[i]["SizeID"].ToString()), "-1");
                        UpdateItemIDandBarCodeWhenChangToOntherGroup("Manu_AfforestationFactoryDetails", "BarCode", BarCode, txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtfromItemUnits.Rows[i]["SizeID"].ToString()), "-1");
                        UpdateItemIDandBarCodeWhenChangToOntherGroup("Manu_CadWaxFactoryDetails", "BarCode", BarCode, txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtfromItemUnits.Rows[i]["SizeID"].ToString()), "-1");
                        UpdateItemIDandBarCodeWhenChangToOntherGroup("Manu_CloseOrdersDetails", "BarCode", BarCode, txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtfromItemUnits.Rows[i]["SizeID"].ToString()), "-1");
                        UpdateItemIDandBarCodeWhenChangToOntherGroup("Manu_ManufacturingCastingDetails", "BarCode", BarCode, txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtfromItemUnits.Rows[i]["SizeID"].ToString()), "-1");
                        UpdateItemIDandBarCodeWhenChangToOntherGroup("Manu_ZirconDiamondFactoryDetails", "BarCode", BarCode, txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtfromItemUnits.Rows[i]["SizeID"].ToString()), "-1");
                        UpdateItemIDandBarCodeWhenChangToOntherGroup("Menu_FactoryRunCommandCompund", "BarcodCompond", BarCode, txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtfromItemUnits.Rows[i]["SizeID"].ToString()), "-1");
                        UpdateItemIDandBarCodeWhenChangToOntherGroup("Menu_FactoryRunCommandDismant", "BarcodeTalmee", BarCode, txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtfromItemUnits.Rows[i]["SizeID"].ToString()), "-1");
                        UpdateItemIDandBarCodeWhenChangToOntherGroup("Menu_FactoryRunCommandfactory", "BarCode", BarCode, txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtfromItemUnits.Rows[i]["SizeID"].ToString()), "-1");
                        UpdateItemIDandBarCodeWhenChangToOntherGroup("Menu_FactoryRunCommandPrentagAndPulishn", "BarcodePrentag", BarCode, txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtfromItemUnits.Rows[i]["SizeID"].ToString()), "-1");
                        UpdateItemIDandBarCodeWhenChangToOntherGroup("Menu_FactoryRunCommandSelver", "BarcodeAdditional", BarCode, txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtfromItemUnits.Rows[i]["SizeID"].ToString()), "-1");
                        UpdateItemIDandBarCodeWhenChangToOntherGroup("Menu_FactoryRunCommandTalmee", "BarcodeTalmee", BarCode, txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtfromItemUnits.Rows[i]["SizeID"].ToString()), "-1");
                                            
                    }

                    //update To Table  Stc_Items
                    Lip.NewFields();
                    Lip.Table = "Stc_Items";
                    Lip.AddNumericField("GroupID", txtGroupID.Text.ToString());
                    Lip.AddNumericField("ItemID", ItemIDAfter.ToString());
                    Lip.sCondition = " Cancel =0 and BranchID=" + MySession.GlobalBranchID+" and    ItemID='" + txtFromItemNo.Text.ToString() + "'";
                    Lip.ExecuteUpdate();


                    //Save  Transaction To Table Transfer
                    Lip.NewFields();
                    Lip.Table = "stc_TransferTransactionItems";
                    Lip.AddNumericField("FromItemID", txtFromItemNo.Text.ToString());
                    Lip.AddNumericField("ToItemID","");
                    Lip.AddNumericField("FromGroupID",  GroupID.ToString());
                    Lip.AddNumericField("ToGroupID",txtGroupID.Text.ToString());
                    Lip.AddStringField(PrimaryName, lblFromItemName.Text.ToString());
                    Lip.AddNumericField("UserID", UserInfo.ID.ToString());
                    Lip.AddNumericField("RegDate", Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate())).ToString());
                    Lip.AddNumericField("RegTime", Comon.cDbl(Lip.GetServerTimeSerial()).ToString());
                    Lip.AddNumericField("EditUserID", UserInfo.ID.ToString());
                    Lip.AddNumericField("EditDate", Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate())).ToString());
                    Lip.AddNumericField("EditTime", Comon.cDbl(Lip.GetServerTimeSerial()).ToString());
                    Lip.AddStringField("ComputerInfo", UserInfo.ComputerInfo.ToString());
                    Lip.AddStringField("EditComputerInfo", UserInfo.ComputerInfo.ToString());
                    Lip.AddStringField("TypeOprationID", "2");
                    Lip.AddStringField("Reasons", txtNotes.Text.ToString());
                    Lip.AddNumericField("Cancel", 0);
                    Lip.ExecuteInsert();
                    SplashScreenManager.CloseForm(false);
                    Messages.MsgInfo(Messages.TitleInfo, UserInfo.Language == iLanguage.Arabic ? "تم نقل  الصنف  بنجاح" : "The item movement was  successfully");
                        

                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? ex.Message + "خطأ نقل الصنف" : ex.Message + "Transfer error");
                return;
            }
            
        }

        private void lblRadioEquvilanGold_CheckedChanged(object sender, EventArgs e)
        {
            btnMove.Visible =  ((RadioButton)sender).Checked;
            btnMoveItem.Visible = !((RadioButton)sender).Checked;
            //radioTransferAndDelete.Checked = radioTransferAndNotDelete.Checked = false;
           
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            btnMove.Visible = !((RadioButton)sender).Checked;
            btnMoveItem.Visible = ((RadioButton)sender).Checked;
            //lblTransferToOnetherGroupWithNewID.Checked = lblTransferToOnetherGroup.Checked = false;
         

        }

        void UpdateItemIDandBarCode(string TabelName, string BarCodeName, string BarCode, string FromItemID, string ToItemID, int SizeID)
        {   
                    Lip.NewFields();
                    Lip.Table =TabelName;
                    Lip.AddNumericField("ItemID", ToItemID.ToString());
                    Lip.AddNumericField(BarCodeName, BarCode);
                    Lip.sCondition = " SizeID =" + SizeID + " and BranchID=" + MySession.GlobalBranchID+" And   ItemID='" + FromItemID+"'";
                    Lip.ExecuteUpdate();

        }
        void UpdateItemIDandBarCodeWhenChangToOntherGroup(string TabelName, string BarCodeName, string BarCode, string FromItemID, string ToItemID, int SizeID,string GroupID)
        {
            Lip.NewFields();
            Lip.Table = TabelName;
            Lip.AddNumericField("ItemID", ToItemID.ToString());
            if (GroupID != "-1")
              Lip.AddNumericField("GroupID", GroupID.ToString());
            Lip.AddNumericField(BarCodeName, BarCode);

            Lip.sCondition = " SizeID =" + SizeID + " and BranchID=" + MySession.GlobalBranchID+" And   ItemID='" + FromItemID + "'";
            Lip.ExecuteUpdate();

        }
        private void btnMoveItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtFromItemNo.Text == txtItemName.Text)
                {
                    Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "الرجاء اختيار صنفين مختلفين لنقل الحركة " : "Please choose two different transmission types");
                    return;
                }
                if (!Validations.IsValidForm(this))
                    return;
                if (radioTransferAndNotDelete.Checked || radioTransferAndDelete.Checked)
                {
                    if (txtFromItemNo.Text == "" ||txtItemName.Text=="")
                    { 
                            Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "الرجاء اختيار صنف   للنقل " : "Please select   item for transportation");
                            return;
                         
                    }
                    DataTable dtFromItem = Lip.SelectRecord("SELECT  [GroupID] ,BaseID FROM  [Stc_Items] where Cancel=0 and BranchID=" + MySession.GlobalBranchID+" and  [ItemID]=" + Comon.cLong(txtFromItemNo.Text));
                    DataTable dtToItem = Lip.SelectRecord("SELECT  [GroupID] ,BaseID FROM  [Stc_Items] where Cancel=0 and BranchID=" + MySession.GlobalBranchID+"  and  [ItemID]=" + Comon.cLong(txtItemName.Text));
                    if ((dtFromItem != null && dtToItem != null) && (dtFromItem.Rows.Count > 0 && dtToItem.Rows.Count > 0))
                    {
                        if (Comon.cLong(dtFromItem.Rows[0]["GroupID"]) != Comon.cLong(dtToItem.Rows[0]["GroupID"]))
                        {
                            Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "لا يمكن نقل حركة صنف الى صنف اخرى لا ينتميان الى نفس المجموعة  " : "It is not possible to transfer the movement of one Item to another Item that does not belong to the same group.");
                            return;
                        }
                        if (Comon.cLong(dtFromItem.Rows[0]["BaseID"]) != Comon.cLong(dtToItem.Rows[0]["BaseID"]))
                        {
                            Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "لا يمكن نقل حركة صنف الى صنف اخرى لا ينتميان الى نفس التصنيف  " : "It is not possible to transfer the movement of one Item to another Item that does not belong to the same Base.");
                            return;
                        }
                        DataTable dtfromItemUnits = Lip.SelectRecord("SELECT  [SizeID]  FROM  [Stc_ItemUnits] where UnitCancel=0 and BranchID=" + MySession.GlobalBranchID+" and [ItemID]=" + Comon.cLong(txtFromItemNo.Text) + " order by SizeID");
                        DataTable dtToItemUnits = Lip.SelectRecord("SELECT  [SizeID]  FROM  [Stc_ItemUnits] where UnitCancel=0 and BranchID=" + MySession.GlobalBranchID+" and [ItemID]=" + Comon.cLong(txtItemName.Text) + " order by SizeID");

                        bool areEqual = dtfromItemUnits.AsEnumerable().SequenceEqual(dtToItemUnits.AsEnumerable(), DataRowComparer.Default);
                        if (!areEqual)
                        {
                            Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "لا يمكن نقل حركة صنف الى صنف اخرى لا يمتلكان نفس الوحدات  " : "It is not possible to transfer the movement of one Item to another Item that does not have the same units.");
                            return;
                        }
                        Application.DoEvents();
                        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                        DataTable dtToItemUnitsBarCode = Lip.SelectRecord("SELECT  [SizeID],[BarCode]  FROM  [Stc_ItemUnits] where UnitCancel=0 and BranchID=" + MySession.GlobalBranchID+" and [ItemID]=" + Comon.cLong(txtItemName.Text) + " order by SizeID");
                        for (int i = 0; i < dtToItemUnitsBarCode.Rows.Count; i++)
                        {

                            UpdateItemIDandBarCode("Stc_ItemsMoviing", "BarCode", dtToItemUnitsBarCode.Rows[i]["BarCode"].ToString(), txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtToItemUnitsBarCode.Rows[i]["SizeID"].ToString()));
                            UpdateItemIDandBarCode("Manu_AllOrdersDetails", "BarCode", dtToItemUnitsBarCode.Rows[i]["BarCode"].ToString(), txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtToItemUnitsBarCode.Rows[i]["SizeID"].ToString()));
                           
                            UpdateItemIDandBarCode("Sales_PurchaseInvoiceDetails", "BarCode", dtToItemUnitsBarCode.Rows[i]["BarCode"].ToString(), txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtToItemUnitsBarCode.Rows[i]["SizeID"].ToString()));
                            UpdateItemIDandBarCode("Sales_SalesInvoiceDetails", "BarCode", dtToItemUnitsBarCode.Rows[i]["BarCode"].ToString(), txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtToItemUnitsBarCode.Rows[i]["SizeID"].ToString()));
                            UpdateItemIDandBarCode("Sales_SalesInvoiceReturnDetails", "BarCode", dtToItemUnitsBarCode.Rows[i]["BarCode"].ToString(), txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtToItemUnitsBarCode.Rows[i]["SizeID"].ToString()));
                            UpdateItemIDandBarCode("Sales_SalesServiceInvoiceDetails", "BarCode", dtToItemUnitsBarCode.Rows[i]["BarCode"].ToString(), txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtToItemUnitsBarCode.Rows[i]["SizeID"].ToString()));
                       
                            UpdateItemIDandBarCode("Sales_PurchaseInvoiceReturnDetails", "BarCode", dtToItemUnitsBarCode.Rows[i]["BarCode"].ToString(), txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtToItemUnitsBarCode.Rows[i]["SizeID"].ToString()));
                            UpdateItemIDandBarCode("Sales_PurchaseInvoiceDetails", "BarCode", dtToItemUnitsBarCode.Rows[i]["BarCode"].ToString(), txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtToItemUnitsBarCode.Rows[i]["SizeID"].ToString()));
                            UpdateItemIDandBarCode("Sales_PurchaseSaveInvoiceDetails", "BarCode", dtToItemUnitsBarCode.Rows[i]["BarCode"].ToString(), txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtToItemUnitsBarCode.Rows[i]["SizeID"].ToString()));
                            UpdateItemIDandBarCode("Sales_PurchaseOrderFromPurchaseDetails", "BarCode", dtToItemUnitsBarCode.Rows[i]["BarCode"].ToString(), txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtToItemUnitsBarCode.Rows[i]["SizeID"].ToString()));
                            UpdateItemIDandBarCode("Sales_PurchaseSaveInvoiceReturnDetails", "BarCode", dtToItemUnitsBarCode.Rows[i]["BarCode"].ToString(), txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtToItemUnitsBarCode.Rows[i]["SizeID"].ToString()));
                            UpdateItemIDandBarCode("Stc_GoldInonBail_Details", "BarCode", dtToItemUnitsBarCode.Rows[i]["BarCode"].ToString(), txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtToItemUnitsBarCode.Rows[i]["SizeID"].ToString()));
                            UpdateItemIDandBarCode("Stc_GoldOutOnBail_Details", "BarCode", dtToItemUnitsBarCode.Rows[i]["BarCode"].ToString(), txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtToItemUnitsBarCode.Rows[i]["SizeID"].ToString()));
                            UpdateItemIDandBarCode("Stc_GoodOpeningDetails", "BarCode", dtToItemUnitsBarCode.Rows[i]["BarCode"].ToString(), txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtToItemUnitsBarCode.Rows[i]["SizeID"].ToString()));
                            UpdateItemIDandBarCode("Stc_MatirialInonBail_Details", "BarCode", dtToItemUnitsBarCode.Rows[i]["BarCode"].ToString(), txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtToItemUnitsBarCode.Rows[i]["SizeID"].ToString()));
                            UpdateItemIDandBarCode("Stc_MatirialOutOnBail_Details", "BarCode", dtToItemUnitsBarCode.Rows[i]["BarCode"].ToString(), txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtToItemUnitsBarCode.Rows[i]["SizeID"].ToString()));
                            UpdateItemIDandBarCode("Stc_TransferMultipleStoresGold_Details", "BarCode", dtToItemUnitsBarCode.Rows[i]["BarCode"].ToString(), txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtToItemUnitsBarCode.Rows[i]["SizeID"].ToString()));
                            UpdateItemIDandBarCode("Stc_TransferMultipleStoresMatirial_Details", "BarCode", dtToItemUnitsBarCode.Rows[i]["BarCode"].ToString(), txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtToItemUnitsBarCode.Rows[i]["SizeID"].ToString()));
                            UpdateItemIDandBarCode("Sales_PurchaseOrderDetails", "BarCode", dtToItemUnitsBarCode.Rows[i]["BarCode"].ToString(), txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtToItemUnitsBarCode.Rows[i]["SizeID"].ToString()));
                            UpdateItemIDandBarCode("Sales_SalesOrderDetails", "BarCode", dtToItemUnitsBarCode.Rows[i]["BarCode"].ToString(), txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtToItemUnitsBarCode.Rows[i]["SizeID"].ToString()));

                            UpdateItemIDandBarCode("Manu_AfforestationFactoryDetails", "BarCode", dtToItemUnitsBarCode.Rows[i]["BarCode"].ToString(), txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtToItemUnitsBarCode.Rows[i]["SizeID"].ToString()));
                            UpdateItemIDandBarCode("Manu_CadWaxFactoryDetails", "BarCode", dtToItemUnitsBarCode.Rows[i]["BarCode"].ToString(), txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtToItemUnitsBarCode.Rows[i]["SizeID"].ToString()));
                            UpdateItemIDandBarCode("Manu_CloseOrdersDetails", "BarCode", dtToItemUnitsBarCode.Rows[i]["BarCode"].ToString(), txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtToItemUnitsBarCode.Rows[i]["SizeID"].ToString()));
                            UpdateItemIDandBarCode("Manu_ManufacturingCastingDetails", "BarCode", dtToItemUnitsBarCode.Rows[i]["BarCode"].ToString(), txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtToItemUnitsBarCode.Rows[i]["SizeID"].ToString()));
                            UpdateItemIDandBarCode("Manu_ZirconDiamondFactoryDetails", "BarCode", dtToItemUnitsBarCode.Rows[i]["BarCode"].ToString(), txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtToItemUnitsBarCode.Rows[i]["SizeID"].ToString()));
                            UpdateItemIDandBarCode("Menu_FactoryRunCommandCompund", "BarcodCompond", dtToItemUnitsBarCode.Rows[i]["BarCode"].ToString(), txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtToItemUnitsBarCode.Rows[i]["SizeID"].ToString()));
                            UpdateItemIDandBarCode("Menu_FactoryRunCommandDismant", "BarcodeTalmee", dtToItemUnitsBarCode.Rows[i]["BarCode"].ToString(), txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtToItemUnitsBarCode.Rows[i]["SizeID"].ToString()));

                            UpdateItemIDandBarCode("Menu_FactoryRunCommandfactory", "BarCode", dtToItemUnitsBarCode.Rows[i]["BarCode"].ToString(), txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtToItemUnitsBarCode.Rows[i]["SizeID"].ToString()));
                            UpdateItemIDandBarCode("Menu_FactoryRunCommandPrentagAndPulishn", "BarcodePrentag", dtToItemUnitsBarCode.Rows[i]["BarCode"].ToString(), txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtToItemUnitsBarCode.Rows[i]["SizeID"].ToString()));
                            UpdateItemIDandBarCode("Menu_FactoryRunCommandSelver", "BarcodeAdditional", dtToItemUnitsBarCode.Rows[i]["BarCode"].ToString(), txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtToItemUnitsBarCode.Rows[i]["SizeID"].ToString()));
                            UpdateItemIDandBarCode("Menu_FactoryRunCommandTalmee", "BarcodeTalmee", dtToItemUnitsBarCode.Rows[i]["BarCode"].ToString(), txtFromItemNo.Text.ToString(), txtItemName.Text.ToString(), Comon.cInt(dtToItemUnitsBarCode.Rows[i]["SizeID"].ToString()));
                       
                        }

                        Lip.NewFields();
                        Lip.Table = "stc_TransferTransactionItems";
                        Lip.AddNumericField("FromItemID",txtFromItemNo.Text.ToString());
                        Lip.AddNumericField("ToItemID", txtItemName.Text.ToString());
                        Lip.AddStringField(PrimaryName, lblFromItemName.Text.ToString());
                        Lip.AddNumericField("UserID",UserInfo.ID.ToString());
                        Lip.AddNumericField("RegDate", Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate())).ToString());
                        Lip.AddNumericField("RegTime", Comon.cDbl(Lip.GetServerTimeSerial()).ToString());
                        Lip.AddNumericField("EditUserID", UserInfo.ID);
                        Lip.AddNumericField("EditDate", Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate())).ToString());
                        Lip.AddNumericField("EditTime", Comon.cDbl(Lip.GetServerTimeSerial()).ToString());
                        Lip.AddStringField("ComputerInfo", UserInfo.ComputerInfo.ToString());
                        Lip.AddStringField("EditComputerInfo", UserInfo.ComputerInfo.ToString());
                        Lip.AddStringField("Reasons", txtNotes.Text.ToString());
                        Lip.AddStringField("TypeOprationID","1");
                        Lip.AddNumericField("Cancel", 0);
                        Lip.ExecuteInsert();
                     


                        if (radioTransferAndDelete.Checked)
                        {
                            Stc_Items model = new Stc_Items();
                            model.ItemID = Comon.cLong(txtFromItemNo.Text);
                            model.EditUserID = UserInfo.ID;
                            model.BranchID = UserInfo.BRANCHID;
                            model.FacilityID = UserInfo.FacilityID;
                            model.EditComputerInfo = UserInfo.ComputerInfo;
                            model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                            model.EditTime = Comon.cLong(Lip.GetServerTimeSerial());
                            string Result = Stc_itemsDAL.Delete(model);
                        }
                        SplashScreenManager.CloseForm(false);
                        Messages.MsgInfo(Messages.TitleInfo, UserInfo.Language == iLanguage.Arabic ? "تم نقل حركة الصنف  بنجاح" : "The item movement was transferred successfully");
                        
                    }
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? ex.Message + "خطأ نقل الحركات" : ex.Message + "Transfer error");
                return;
                       
            }

            
        }
    }
}
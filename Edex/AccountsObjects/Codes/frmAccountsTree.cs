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
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using Edex.AccountsObjects.AccountsClasses;
using Edex.Model;
using Edex.DAL;
using Edex.ModelSystem;
using Edex.SalesAndPurchaseObjects.Transactions;
using Edex.GeneralObjects.GeneralClasses;
using Edex.Model.Language;
using Edex.GeneralObjects.GeneralForms;
using Edex.SalesAndPurchaseObjects.SalesClasses;
using Edex.DAL.Accounting;
using DevExpress.XtraReports.UI;
using DevExpress.XtraSplashScreen;
using Edex.HR.HRClasses;
using System.Data.OleDb;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Data.SqlClient;
using Edex.DAL.UsersManagement;

namespace Edex.AccountsObjects.Codes
{
    public partial class frmAccountsTree : Edex.GeneralObjects.GeneralForms.BaseForm
    {

        #region Declare

        private string strSQL;
        private bool IsNewRecord;

        private cAccountsTree cClass = new cAccountsTree();
        public const int xMoveFirst = 7;
        public const int xMovePrev = 8;
        public const int xMoveNext = 9;
        public const int xMoveLast = 10;
        string FocusedControl = "";
     
        #endregion
        public frmAccountsTree()
        {
            InitializeComponent();
           
            
            strSQL = "ArbName";
            if(UserInfo.Language==iLanguage.English)
            {
                strSQL = "EngName";
            }
            Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, UserInfo.Language.ToString());
            FillCombo.FillComboBox(cmbTypeAcount, "Acc_AccountType", "ID", strSQL);
            FillCombo.FillComboBoxLookUpEdit(cmbAccAccountLevel, "Acc_AccountsLevels", "LevelNumber", "LevelNumber");
            FillCombo.FillComboBox(cmbAccAccounEnd, "Acc_AccountEnd", "ID", strSQL);
            FillCombo.FillComboBox(cmbPyAccounEnd, "Acc_AccountEnd", "ID", strSQL);

            FillCombo.FillComboBoxLookUpEdit(cmbToLevel, "Acc_AccountsLevels", "LevelNumber", "LevelNumber");
            FillCombo.FillComboBoxLookUpEdit(cmbFromLevel, "Acc_AccountsLevels", "LevelNumber", "LevelNumber");
            FillCombo.FillComboBox(cmbAccountType, "Acc_AccountType", "ID", strSQL);


            //ribbonControl1.Pages[0].Groups[0].ItemLinks[4].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[5].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[7].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[8].Visible = false;
         
            FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", "ArbName", "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
            cmbBranchesID.EditValue = UserInfo.BRANCHID;
            GetAcountsTree();
            ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Visible = false;
        }
        
        public void GetAcountsTree()
        {
            List<Acc_Accounts> ListAccountsTree = new List<Acc_Accounts>();
            ListAccountsTree = Acc_AccountsDAL.GetAllData(Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID);
            List<MyRecord> list = new List<MyRecord>();
            if (ListAccountsTree != null)
            {
                for (int i = 0; i <= ListAccountsTree.Count - 1; i++)
                {
                    if(UserInfo.Language==iLanguage.Arabic)
                         list.Add(new MyRecord(ListAccountsTree[i].AccountID, ListAccountsTree[i].ParentAccountID, ListAccountsTree[i].ArbName + "," + ListAccountsTree[i].AccountID.ToString()));
                    else
                        list.Add(new MyRecord(ListAccountsTree[i].AccountID, ListAccountsTree[i].ParentAccountID, ListAccountsTree[i].EngName + "," + ListAccountsTree[i].AccountID.ToString()));

                }

                treeList1.BeginUnboundLoad();
                treeList1.DataSource = list;
                treeList1.ExpandAll();
                treeList1.EndUnboundLoad();
            }
        }


        public class MyTreeList : TreeList
        {
            public MyTreeList()
                : base()
            {
                OptionsBehavior.AutoNodeHeight = false;
            }
            protected override TreeListNode CreateNode(int nodeID, TreeListNodes owner, object tag)
            {
                return new MyTreeListNode(nodeID, owner);
            }
            protected override void InternalNodeChanged(TreeListNode node, NodeChangeTypeEnum changeType)
            {
                if (changeType == NodeChangeTypeEnum.User1)
                    LayoutChanged();
                base.InternalNodeChanged(node, changeType);
            }
            protected override void RaiseCalcNodeHeight(TreeListNode node, ref int nodeHeight)
            {
                MyTreeListNode myNode = node as MyTreeListNode;
                if (myNode != null)
                    nodeHeight = myNode.Height;
                else
                    base.RaiseCalcNodeHeight(node, ref nodeHeight);
            }
            public virtual int DefaultNodesHeight { get { return 18; } }
        }
        public class MyTreeListNode : TreeListNode
        {
            const int minHeight = 5;
            int height;
            public MyTreeListNode(int id, TreeListNodes owner)
                : base(id, owner)
            {
                this.height = (owner.TreeList as MyTreeList).DefaultNodesHeight;
            }
            public int Height
            {
                get { return height; }
                set
                {
                    if (Height == value || value < minHeight) return;
                    height = value;
                    Changed(NodeChangeTypeEnum.User1);
                }
            }
        }
        public class MyRecord
        {
            public long ID { get; set; }
            public long ParentID { get; set; }
            public string AcountName { get; set; }

            public MyRecord(long id, long parentID, string _AcountName)
            {
                ID = id;
                ParentID = parentID;
                AcountName = _AcountName;
            }
        }

        private void frmAccountsTree_Load(object sender, EventArgs e)
        {
            treeList1.ForceInitialize();
            treeList1.ExpandAll();
            GetAcountsTree();
            ribbonControl1.Pages[0].Groups[0].ItemLinks[12].Visible = true;
       
        }



        private void treeList1_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            string[] AcountNameAndID;

            if (e.Node == null) return;
            AcountNameAndID = e.Node.GetValue(0).ToString().Split(',');
            long AcountID = Comon.cLong(AcountNameAndID[1]);
            // writ function to get acount data and disbbly it in textbox

            Acc_Accounts Accounts = new Acc_Accounts();
            Accounts =   Acc_AccountsDAL.GetDataByID(AcountID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID);
            if (Accounts != null)
            {
                txtAccountID.Text = Accounts.AccountID.ToString();
                txtArbName.Text = Accounts.ArbName;
                txtEngName.Text = Accounts.EngName;
                txtMaxLimit.Text = Accounts.MaxLimit.ToString();
                txtParentID.Text = Accounts.ParentAccountID.ToString();
                if (Accounts.StopAccount == 0)
                    chkStopAccount.Checked = false;
                else
                    chkStopAccount.Checked = true;

                if (Accounts.AllowMaxLimit == 0)
                    chkAllowLimit.Checked = false;
                else
                    chkAllowLimit.Checked = true;

                cmbTypeAcount.EditValue = Accounts.AccountTypeID;
                cmbAccAccounEnd.EditValue = Accounts.EndType;
                txtNotes.Text = Accounts.Description;
                
                cmbAccAccountLevel.EditValue = Accounts.AccountLevel;
                if (Accounts.CashState <= 0)
                    chekStateCash.Checked = false;
                else
                {
                    chekStateCash.Checked = true;

                    if (Accounts.CashState == 1)
                        rdAll.Checked = true;
                    if (Accounts.CashState == 2)
                        rdDirect.Checked = true;
                    if (Accounts.CashState == 3)
                        rdUnDirect.Checked = true;
                }
                if (cmbTypeAcount.EditValue.ToString() == "1")
                {
                    ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;
                    //ribbonControl1.Pages[0].Groups[0].ItemLinks[11].Visible = true;
                }
                else
                    ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = true;
                IsNewRecord = false;

                Validations.DoReadRipon(this, ribbonControl1);
                Validations.EnabledControl(this, false);

            }

        }

        private void treeList1_GetNodeDisplayValue(object sender, GetNodeDisplayValueEventArgs e)
        {
            if (e.Node == null) return;
            string v = e.Value.ToString();
        }


        protected override void DoSave()
        {
            try
            {
                if (Comon.cInt(cmbAccAccounEnd.EditValue) <= 0)
                {
                    Messages.MsgWarning(Messages.TitleWorning, "الرجاء تحديد الحساب الختامي");
                    cmbAccAccounEnd.Focus();
                    return;
                }
                if (IsNewRecord && !FormAdd)
                {
                    Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToAddNewRecord);
                    return;

                }
                if (!IsNewRecord)
                {
                    if (!FormUpdate)
                    {
                        Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToUpdateRecord);
                        return;
                    }
                    else
                    {
                        bool Yes = Messages.MsgWarningYesNo(Messages.TitleInfo, Messages.msgConfirmUpdate);
                        if (!Yes)
                            return;
                    }
                }
                if (Validations.Important(this) == false)
                {
                    Messages.MsgExclamationk(Messages.TitleWorning, Messages.msgShouldCompleteData);
                    return;
                }
                Acc_Accounts model = new Acc_Accounts();
                model.AccountID = Comon.cLong(txtAccountID.Text);
                model.ArbName = txtArbName.Text;
                model.EngName = txtEngName.Text;
                model.ParentAccountID = Comon.cLong(txtParentID.Text);
                model.MaxLimit = Comon.cLong(txtMaxLimit.Text);
                model.MinLimit = Comon.cLong(txtMinLimit.Text);
                model.Description = txtNotes.Text;
                model.Location = txtLocation.Text;

                if ( chkAllowLimit.Checked == false)
                    model.AllowMaxLimit = 0;
                else
                    model.AllowMaxLimit = 1;

                 if (chkStopAccount.Checked == false)
                    model.StopAccount = 0;
                 else
                    model.StopAccount = 1;

                if (rdAll.Checked)
                    model.CashState = 1;

                if (rdDirect.Checked)
                    model.CashState = 2;

                if (rdUnDirect.Checked)
                    model.CashState = 3;


                model.AccountLevel = Comon.cInt(cmbAccAccountLevel.EditValue);
                model.AccountTypeID = Comon.cInt(cmbTypeAcount.EditValue);
                model.EndType = Comon.cInt(cmbAccAccounEnd.EditValue);
                model.UserID = UserInfo.ID;
                model.EditUserID = UserInfo.ID;
                model.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                model.FacilityID = UserInfo.FacilityID;
                model.ComputerInfo = UserInfo.ComputerInfo;
                model.EditComputerInfo = UserInfo.ComputerInfo;
                model.RegDate = Comon.cLong(Lip.GetServerDateSerial());
                model.RegTime = Comon.cLong(Lip.GetServerTimeSerial());
                model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                model.EditTime = Comon.cLong(Lip.GetServerDateSerial());
                model.Cancel = 0;
                strSQL = "Select * from Acc_Accounts where  BRANCHID= " + model.BranchID + " and AccountID=" + txtAccountID.Text;
                DataTable dtAcco = new DataTable();
                dtAcco = Lip.SelectRecord(strSQL);
                if (dtAcco.Rows.Count > 0)
                    Acc_AccountsDAL.UpdateAcc_Accounts(model);
                else
                    Acc_AccountsDAL.InsertAcc_Accounts(model);

                double ParentParentAccountID = 0;
                if (MySession.GlobalNoOfLevels > 4)
                    ParentParentAccountID = Comon.cDbl(Lip.GetValue("SELECT  [ParentAccountID] FROM  [Acc_Accounts] where [AccountID]=" + txtParentID.Text + " and BranchID=" + Comon.cInt(cmbBranchesID.EditValue)));
                else if (MySession.GlobalNoOfLevels == 4)
                    ParentParentAccountID = Comon.cDbl(Lip.GetValue("SELECT  [ParentAccountID] FROM  [Acc_Accounts] where [AccountID]=" + txtAccountID.Text + " and BranchID=" + Comon.cInt(cmbBranchesID.EditValue)));
               
                if (ParentParentAccountID > 0)
                {
                    // حفظ الصناديق في جدول الصناديق 
                    #region Save Boxes
                    if (ParentParentAccountID == Comon.cDbl(MySession.GlobalDefaultParentBoxesAccountID))
                    {
                        Acc_Boxes modelBox = new Acc_Boxes();
                        cBoxes cClassBox = new cBoxes();
                        if (IsNewRecord)
                            modelBox.BoxID = Comon.cInt(cClassBox.GetNewID().ToString());
                        else
                        {
                            modelBox.BoxID = Comon.cInt(Lip.GetValue("SELECT   [BoxID] FROM  [Acc_Boxes] where BranchID=" + Comon.cInt(cmbBranchesID.EditValue) + " and [AccountID]= " + Comon.cDbl(txtAccountID.Text)));
                        }
                        modelBox.AccountID = cClass.AccountID;
                        //Comon.cLong(txtAccountID.Text);

                        modelBox.ArbName = txtArbName.Text;

                        modelBox.EngName = txtEngName.Text;
                        modelBox.StopAccount = chkStopAccount.Checked == true ? 1 : 0;
                        modelBox.UserID = UserInfo.ID;
                        modelBox.EditUserID = UserInfo.ID;
                        modelBox.ComputerInfo = UserInfo.ComputerInfo;
                        modelBox.EditComputerInfo = UserInfo.ComputerInfo;
                        modelBox.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                        modelBox.FacilityID = UserInfo.FacilityID;

                        modelBox.Notes = txtNotes.Text;
                        modelBox.RegDate = Comon.cLong(Lip.GetServerDateSerial());
                        modelBox.RegTime = Comon.cLong(Lip.GetServerTimeSerial());
                        modelBox.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                        modelBox.EditTime = Comon.cLong(Lip.GetServerDateSerial());
                        modelBox.Cancel = 0;
                        modelBox.AccountID = Comon.cDbl(txtAccountID.Text);
                        modelBox.ParentAccountID = Comon.cDbl(txtParentID.EditValue);

                        int SaveID;
                        int UpdateID;
                        if (IsNewRecord == true)
                            SaveID = Acc_BoxesDAL.Insert_Acc_Boxes(modelBox);
                        else
                            UpdateID = Acc_BoxesDAL.Update_Acc_Boxes(modelBox);

                    }
                    #endregion

                    // حفظ البنوك في جدول البنوك 
                    #region Save Bank
                    if (ParentParentAccountID == Comon.cDbl(MySession.GlobalDefaultParentBanksAccountID))
                    {
                        Acc_Banks modelBank = new Acc_Banks();
                        cBanks cClassBank = new cBanks();

                        if (IsNewRecord)
                            modelBank.BankID = Comon.cInt(cClassBank.GetNewID().ToString());
                        else
                        {
                            modelBank.BankID = Comon.cInt(Lip.GetValue("SELECT   [BankID] FROM  [Acc_Banks] where BranchID=" + Comon.cInt(cmbBranchesID.EditValue) + " and [AccountID]= " + Comon.cDbl(txtAccountID.Text)));
                        }
                        model.AccountID = Comon.cLong(txtAccountID.Text);
                        model.StopAccount = chkStopAccount.Checked == true ? 1 : 0;

                        modelBank.ArbName = txtArbName.Text;
                        modelBank.EngName = txtEngName.Text;
                        modelBank.UserID = UserInfo.ID;
                        modelBank.EditUserID = UserInfo.ID;
                        modelBank.ComputerInfo = UserInfo.ComputerInfo;
                        modelBank.EditComputerInfo = UserInfo.ComputerInfo;
                        modelBank.BranchID = UserInfo.BRANCHID;
                        modelBank.FacilityID = UserInfo.FacilityID;
                        modelBank.Notes = txtNotes.Text;
                        model.RegDate = Comon.cLong(Lip.GetServerDateSerial());
                        model.RegTime = Comon.cLong(Lip.GetServerTimeSerial());
                        model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                        model.EditTime = Comon.cLong(Lip.GetServerDateSerial());
                        model.Cancel = 0;
                        model.ParentAccountID = Comon.cLong(txtParentID.Text);

                        int StoreID;
                        int UpdateID;
                        if (IsNewRecord == true)
                            StoreID = Acc_BanksDAL.Insert_Acc_Banks(modelBank);
                        else
                            UpdateID = Acc_BanksDAL.Update_Acc_Banks(modelBank);

                    }
                    #endregion

                    //حفظ العملاء في جدول العملاء 
                    #region Save Customer
                    if (ParentParentAccountID == Comon.cDbl(MySession.GlobalDefaultParentCustomerAccountID))
                    {

                        Sales_Customers modelCustomer = new Sales_Customers();
                        cCustomers cClassCustomer = new cCustomers();
                        modelCustomer.AccountID = cClass.AccountID;

                        if (IsNewRecord)
                            modelCustomer.CustomerID = Comon.cInt(cClassCustomer.GetNewID().ToString());
                        else
                        {
                            modelCustomer.CustomerID = Comon.cInt(Lip.GetValue("SELECT   [CustomerID] FROM  [Sales_Customers] where BranchID=" + Comon.cInt(cmbBranchesID.EditValue) + " and [AccountID]= " + Comon.cDbl(txtAccountID.Text)));
                        }
                        modelCustomer.AccountID = Comon.cDbl(txtAccountID.Text);
                        modelCustomer.ArbName = txtArbName.Text;

                        modelCustomer.EngName = txtEngName.Text;
                        modelCustomer.StopAccount = chkStopAccount.Checked == true ? 1 : 0;
                        modelCustomer.UserID = UserInfo.ID;
                        modelCustomer.EditUserID = UserInfo.ID;
                        modelCustomer.ComputerInfo = UserInfo.ComputerInfo;
                        modelCustomer.EditComputerInfo = UserInfo.ComputerInfo;
                        modelCustomer.BranchID = UserInfo.BRANCHID;
                        modelCustomer.FacilityID = UserInfo.FacilityID;

                        modelCustomer.Notes = txtNotes.Text;

                        modelCustomer.RegDate = Comon.cLong(Lip.GetServerDateSerial());
                        modelCustomer.RegTime = Comon.cLong(Lip.GetServerTimeSerial());
                        modelCustomer.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                        modelCustomer.EditTime = Comon.cLong(Lip.GetServerDateSerial());
                        modelCustomer.Cancel = 0;
                        modelCustomer.ContactPerson = "";
                        modelCustomer.IdentityNumber = "";
                        modelCustomer.CustomerType = "";
                        modelCustomer.BlockingReason = "";
                        modelCustomer.IsInBlackList = 0;
                        modelCustomer.Gender = 0;
                        modelCustomer.NationalityID = 0;

                        modelCustomer.ParentAccountID = Comon.cDbl(txtParentID.Text);
                        int StoreID;
                        int UpdateID;
                        if (IsNewRecord == true)
                            StoreID = Sales_CustomersDAL.InsertSales_Customers(modelCustomer);
                        else
                            UpdateID = Sales_CustomersDAL.UpdateSales_Customers(modelCustomer);

                    }
                    #endregion

                    // حفظ الموردين في جدول الموردين
                    #region Save Supplier
                    if (ParentParentAccountID == Comon.cDbl(MySession.GlobalDefaultParentSupplierAccountID))
                    {

                        Sales_Suppliers modelSupplier = new Sales_Suppliers();
                        cSuppliers cClassSuplier = new cSuppliers();
                        if (IsNewRecord)
                            modelSupplier.SupplierID = Comon.cInt(cClassSuplier.GetNewID().ToString());
                        else
                        {
                            modelSupplier.SupplierID = Comon.cInt(Lip.GetValue("SELECT   [SupplierID] FROM  [Sales_Suppliers] where BranchID=" + Comon.cInt(cmbBranchesID.EditValue) + " and [AccountID]= " + Comon.cDbl(txtAccountID.Text)));
                        }

                        //Comon.cLong(txtAccountID.Text);
                        modelSupplier.StopAccount = chkStopAccount.Checked == true ? 1 : 0;

                        modelSupplier.AccountID = Comon.cDbl(txtAccountID.Text);
                        modelSupplier.ArbName = txtArbName.Text;
                        modelSupplier.EngName = txtEngName.Text;
                        modelSupplier.UserID = UserInfo.ID;
                        modelSupplier.EditUserID = UserInfo.ID;
                        modelSupplier.ComputerInfo = UserInfo.ComputerInfo;
                        modelSupplier.EditComputerInfo = UserInfo.ComputerInfo;
                        modelSupplier.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                        modelSupplier.FacilityID = UserInfo.FacilityID;
                        modelSupplier.Notes = txtNotes.Text;
                        modelSupplier.RegDate = Comon.cLong(Lip.GetServerDateSerial());
                        modelSupplier.RegTime = Comon.cLong(Lip.GetServerTimeSerial());
                        modelSupplier.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                        modelSupplier.EditTime = Comon.cLong(Lip.GetServerDateSerial());
                        modelSupplier.Cancel = 0;
                        modelSupplier.ParentAccountID = Comon.cDbl(txtParentID.Text);
                        int StoreID;
                        int UpdateID;


                        if (IsNewRecord == true)
                            StoreID = Sales_SuppliersDAL.InsertSales_Suppliers(modelSupplier);
                        else
                            UpdateID = Sales_SuppliersDAL.UpdateSales_Suppliers(modelSupplier);

                    }
                    #endregion

                    //حفظ المخازن الى جدول المخازن 
                    #region Save Store
                    if (ParentParentAccountID == Comon.cDbl(MySession.GlobalDefaultParentStoreAccountID))
                    {
                        Stc_Stores modelStore = new Stc_Stores();

                        if (IsNewRecord == true)
                            modelStore.StoreID = Comon.cInt(STC_STORES_DAL.GetNewID().ToString());
                        else
                            modelStore.StoreID = Comon.cInt(Lip.GetValue("SELECT   [StoreID] FROM  [Stc_Stores] where BranchID=" + Comon.cInt(cmbBranchesID.EditValue) + " and [AccountID]= " + Comon.cDbl(txtAccountID.Text)));
                        modelStore.StopAccount = chkStopAccount.Checked == true ? 1 : 0;
                        modelStore.AccountID = Comon.cLong(txtAccountID.Text);
                        modelStore.ArbName = txtArbName.Text;
                        modelStore.EngName = txtEngName.Text;


                        modelStore.FacilityID = MySession.GlobalFacilityID;
                        modelStore.BranchID = MySession.GlobalBranchID;

                        modelStore.Notes = txtNotes.Text; 
                        modelStore.UserID = UserInfo.ID;
                        modelStore.EditUserID = UserInfo.ID;
                        modelStore.ComputerInfo = UserInfo.ComputerInfo;
                        modelStore.EditComputerInfo = UserInfo.ComputerInfo;

                        modelStore.RegDate = Comon.cLong(Lip.GetServerDateSerial());
                        modelStore.RegTime = Comon.cLong(Lip.GetServerTimeSerial());

                        model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                        model.EditTime = Comon.cLong(Lip.GetServerDateSerial());


                        modelStore.ParentAccountID = Comon.cDbl(txtParentID.Text);
                        modelStore.Cancel = 0;
                        int StoreID;
                        bool updateModel;
                        if (modelStore.StoreID == 0)
                            StoreID = STC_STORES_DAL.InsertStc_Stores(modelStore);
                        else
                            updateModel = STC_STORES_DAL.UpdateStc_Stores(modelStore);

                    }
                    #endregion
                    //حفظ الموظف الى جدول الموظفين   
                    #region Save Employee
                    if (ParentParentAccountID == Comon.cDbl(MySession.GlobalDefaultParentEmployeeAccountID))
                    {
                         
                       cEmployeeFiles cClass = new cEmployeeFiles();                        
                       HR_EmployeeFile modelEMP = new HR_EmployeeFile();       
                       if (IsNewRecord)
                            modelEMP.EmployeeID = Comon.cInt(cClass.GetNewID().ToString());
                       else
                           modelEMP.EmployeeID = Comon.cInt(Lip.GetValue("SELECT   [EmployeeID] FROM  [HR_EmployeeFile] where BranchID=" + Comon.cInt(cmbBranchesID.EditValue) + " and [OnAccountID]= " + Comon.cDbl(txtAccountID.Text)));

                        modelEMP.OnAccountID = Comon.cLong(txtAccountID.Text);

                        modelEMP.StopAccount = chkStopAccount.Checked == true ? 1 : 0;
                        modelEMP.ArbName = txtArbName.Text;
                     
                        modelEMP.EngName = txtEngName.Text;
                        modelEMP.WorkingHours = 0;
                        modelEMP.UserID = UserInfo.ID;
                        modelEMP.EditUserID = UserInfo.ID;
                        modelEMP.ComputerInfo = UserInfo.ComputerInfo;
                        modelEMP.EditComputerInfo = UserInfo.ComputerInfo;
                        modelEMP.BranchID = UserInfo.BRANCHID;
                        modelEMP.FacilityID = UserInfo.FacilityID;
                        modelEMP.WorkTel = "";
                        modelEMP.WorkMobile ="";
                        modelEMP.BankAccountID = "";
                        modelEMP.AddressNotes = "";
                        modelEMP.FootprintEmpID = 0;
                        modelEMP.EmpNotes = txtNotes.Text.Trim();
                        modelEMP.WorkEmail = "";
                        modelEMP.RegDate = Comon.cLong(Lip.GetServerDateSerial());
                        modelEMP.RegTime = Comon.cLong(Lip.GetServerTimeSerial());
                        modelEMP.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                        modelEMP.EditTime = Comon.cLong(Lip.GetServerDateSerial());
                        modelEMP.Cancel = 0;
                        modelEMP.Termination = 0;
                        modelEMP.BankAccountID = "";
                        modelEMP.BirthPlace = "";
                        modelEMP.ComputerInfo = "";
                        modelEMP.HomeMobil = "";
                        modelEMP.HomeTel = "";
                        modelEMP.HomeAddress = "";
                        modelEMP.HomeWorkDistance = "";
                        modelEMP.Sex = 1;
                        modelEMP.MaritalStatus = 1;
                        modelEMP.BirthDate = 0;
                        modelEMP.Nationality = 1;
                        modelEMP.Religions = 1;
                        modelEMP.WorkType = 1;
                        modelEMP.Occupation = 1;
                        modelEMP.IqamaOccupation = 1;
                        modelEMP.ContractType = 1;
                        modelEMP.Administration = 1;
                        modelEMP.Department = 1;
                        modelEMP.PaymentMethod = 1;
                        modelEMP.TerminationReason = 1;
                        modelEMP.Department = 1;
                        modelEMP.StopSalary = 0;

                        modelEMP.ClinicID = 0;
                        modelEMP.Emptype = 0;
                        modelEMP.CostCenterID = 0;
                        modelEMP.LeaveNotes = "";
                        modelEMP.WorkAddress = "";
                        modelEMP.CompanyVehicle = "";
                        modelEMP.CurrentSponsor = "";

                        modelEMP.ParentAccountID = Comon.cDbl(txtParentID.Text);
                        long StoreID;
                        bool UpdateID = false;

                        if (IsNewRecord == true)
                            StoreID =  HR_EmployeeFileDAL.InsertHR_EmployeeFile(modelEMP,IsNewRecord);
                        else
                            UpdateID = new HR_EmployeeFileDAL().UpdateHR_EmployeeFile(modelEMP);


                    }
                    #endregion
                }
                //strSQL = "SELECT  *   FROM  Branches";
                //DataTable dtcustomer = Lip.SelectRecord(strSQL);
                //if (dtcustomer.Rows.Count > 0)
                //{
                //    for (int i = 0; i <= dtcustomer.Rows.Count - 1; i++)
                //    {
                //        model.BranchID = Comon.cInt(dtcustomer.Rows[i]["BRANCHID"].ToString());

                //        strSQL = "Select * from Acc_Accounts where  BRANCHID= " + model.BranchID + " and AccountID=" + txtAccountID.Text;
                //        DataTable dtAcco = new DataTable();
                //        dtAcco = Lip.SelectRecord(strSQL);
                //        if (dtAcco.Rows.Count > 0)
                //            Acc_AccountsDAL.UpdateAcc_Accounts(model);
                //        else
                //            Acc_AccountsDAL.InsertAcc_Accounts(model);

                //    }
                //}
                Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);

                GetAcountsTree();
                if (IsNewRecord == true)
                {

                    var AccountID = txtAccountID.Text;
                    txtAccountID.Text = GetNewAccountID(UserInfo.FacilityID, Comon.cInt(cmbBranchesID.EditValue), Comon.cLong(txtParentID.Text)).ToString();
                    txtArbName.Text = "";
                    txtEngName.Text = "";
                    txtMaxLimit.Text = "";
                    DataTable dtLevel = new DataTable();
                    dtLevel = Lip.SelectRecord("Select Max(LevelNumber) AS LevelNumber  from Acc_AccountsLevels");
                    int MaxLevel = Comon.cInt(dtLevel.Rows[0]["LevelNumber"].ToString());
                    if (MaxLevel == Comon.cInt(cmbAccAccountLevel.EditValue))
                        cmbTypeAcount.ItemIndex = 1;
                    else
                        cmbTypeAcount.ItemIndex = 0;

                    txtArbName.Focus();



                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }


        //public List<Acc_Accounts> SortAccounts(List<Acc_Accounts> accounts)
        //{
        //    List<Acc_Accounts> sortedAccounts = new List<Acc_Accounts>();
        //    Dictionary<double, Acc_Accounts> accountMap = new Dictionary<double, Acc_Accounts>();

        //    foreach (var account in accounts)
        //    {
        //        accountMap[account.AccountID] = account;
        //    }
        //    accounts.Sort((a, b) => a.AccountID.CompareTo(b.AccountID));
        //    foreach (var account in accounts)
        //    {
        //        if (account.ParentAccountID == null)
        //        {
        //            sortedAccounts.Add(account);
        //        }
        //        else
        //        {
        //            Acc_Accounts parentAccount = null;
        //            if (account.ParentAccountID != 0)
        //            {
        //                parentAccount = accountMap[account.ParentAccountID];
        //            }
        //            int parentIndex = sortedAccounts.IndexOf(parentAccount);
        //            if (parentIndex == -1)
        //            {
        //                sortedAccounts.Add(account);
        //            }
        //            else
        //            {
        //                sortedAccounts.Insert(parentIndex + 1, account);
        //            }
        //        }
        //    }
        //    return sortedAccounts;
        //}

        public List<Acc_Accounts> SortAccounts(List<Acc_Accounts> accounts)
        {
            List<Acc_Accounts> sortedAccounts = new List<Acc_Accounts>();
            Dictionary<double, Acc_Accounts> accountMap = new Dictionary<double, Acc_Accounts>();

            foreach (var account in accounts)
            {
                accountMap[account.AccountID] = account;
            }

            accounts.Sort((a, b) => a.AccountID.CompareTo(b.AccountID));

            foreach (var account in accounts)
            {
                if (account.ParentAccountID != null)
                {
                    sortedAccounts.Add(account);
                    AddChildAccounts(account, accountMap, sortedAccounts);
                }
            }

            return sortedAccounts;
        }

        private void AddChildAccounts(Acc_Accounts parentAccount, Dictionary<double, Acc_Accounts> accountMap, List<Acc_Accounts> sortedAccounts)
        {
            foreach (var account in accountMap.Values)
            {
                if (account.ParentAccountID == parentAccount.AccountID)
                {
                    sortedAccounts.Add(account);
                    AddChildAccounts(account, accountMap, sortedAccounts);
                }
            }
        }

        protected override void DoPrint()
        {
            try
            {
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                /******************** Report Body *************************/
                bool IncludeHeader = true;
                ReportName = "rptAccounts";
                string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");
                if (UserInfo.Language == iLanguage.English)
                    rptFormName = ReportName + "Arb";
                XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);
                /***************** Master *****************************/

                rptForm.RequestParameters = false;
                //rptForm.Parameters["MainAccountID"].Value = txtAccountID.Text.Trim().ToString();
                //rptForm.Parameters["MainAccountName"].Value = lblAccountName.Text.Trim().ToString();
                //rptForm.Parameters["CostCenterName"].Value = lblCostCenterName.Text.Trim().ToString();
                rptForm.Parameters["TotalDebit"].Value = cmbPyAccounEnd.Text;
                rptForm.Parameters["TotalCredit"].Value = 0;
                rptForm.Parameters["TotalBalance"].Value = 0;
                ///
                rptForm.Parameters["TotalCredit"].Visible = false;
                rptForm.Parameters["FromAccountID"].Value = txtFromAccountID.Text.Trim().ToString();
                rptForm.Parameters["ToAccountID"].Value = txtToAccountID.Text.Trim().ToString();
                rptForm.Parameters["FromAccountName"].Value = lblFromAccountName.Text.Trim().ToString();
                rptForm.Parameters["ToAccountName"].Value = lblToAccountName.Text.Trim().ToString();
                rptForm.Parameters["CostCenterName"].Value = cmbBranchesID.Text.Trim().ToString();
                rptForm.Parameters["FromDate"].Value = cmbFromLevel.Text.Trim().ToString();
                rptForm.Parameters["ToDate"].Value = cmbToLevel.Text.Trim().ToString();

                /********************** Details ****************************/
                var dataTable = new dsReports.rptBalanceReviewDataTable();
                List<Acc_Accounts> ListAccountsTree = new List<Acc_Accounts>();
                ListAccountsTree = Acc_AccountsDAL.GetAllData(Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID);
                if (Comon.cLong(txtFromAccountID.Text) > 0)
                    ListAccountsTree = ListAccountsTree.FindAll(x => x.AccountID >= Comon.cLong(txtFromAccountID.Text));
                if (Comon.cLong(txtToAccountID.Text) > 0)
                    ListAccountsTree = ListAccountsTree.FindAll(x => x.AccountID <= Comon.cLong(txtToAccountID.Text));
                if (Comon.cLong(cmbFromLevel.Text) > 0)
                    ListAccountsTree = ListAccountsTree.FindAll(x => x.AccountLevel >= Comon.cLong(cmbFromLevel.Text));
                if (Comon.cLong(cmbToLevel.Text) > 0)
                    ListAccountsTree = ListAccountsTree.FindAll(x => x.AccountLevel <= Comon.cLong(cmbToLevel.Text));
                if (Comon.cLong(cmbAccountType.EditValue) > 0)
                    ListAccountsTree = ListAccountsTree.FindAll(x => x.AccountTypeID == Comon.cLong(cmbAccountType.EditValue));

                if (Comon.cLong(cmbPyAccounEnd.EditValue) > 0)
                    ListAccountsTree = ListAccountsTree.FindAll(x => x.EndType == Comon.cLong(cmbPyAccounEnd.EditValue));

                ListAccountsTree = SortAccounts(ListAccountsTree);
                for (int i = 0; i <= ListAccountsTree.Count - 1; i++)
                {
                    var row = dataTable.NewRow();
                    row["n_invoice_serial"] = i + 1;
                    row["ID"] = ListAccountsTree[i].AccountID;
                    row["OppsiteAccountName"] = ListAccountsTree[i].ArbName;
                    row["Balance"] = ListAccountsTree[i].AccountTypeID;

                    row["BalanceType"] = ListAccountsTree[i].AccountTypeID;

                    row["Debit"] = ListAccountsTree[i].ParentAccountID;
                    row["Credit"] = ListAccountsTree[i].AccountTypeID;

                    row["DebitGold"] = ListAccountsTree[i].AccountLevel;
                    row["CreditGold"] = ListAccountsTree[i].MinLimit;
                    row["DebitDiamond"] = ListAccountsTree[i].MaxLimit;
                    row["CreditDiamond"] = ListAccountsTree[i].AccountTypeID;
                    row["AmountDebitCold"] = ListAccountsTree[i].EndType;
                    row["AmountCrditGold"] = "-";
                    row["AmountDebitDiamond"] = "-";
                    row["AmountCrditDiamond"] = "-";
                    row["DebitBalance"] = ListAccountsTree[i].EngName;
                    row["CreditBalance"] = ListAccountsTree[i].AccountTypeID;
                    row["TotalDebit"] = 0;
                    row["TotalCredit"] = 0;
                    row["BalanceType"] = "";
                    dataTable.Rows.Add(row);
                }
                rptForm.DataSource = dataTable;
                rptForm.DataMember = "rptBalanceReview";
                /******************** Report Binding ************************/
                XRSubreport subreport = (XRSubreport)rptForm.FindControl("subRptCompanyHeader", true);
                subreport.Visible = IncludeHeader;
                subreport.ReportSource = ReportComponent.CompanyHeader();
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
        protected override void DoEdit()
        {
            Validations.DoEditRipon(this, ribbonControl1);
            EnabledControl(true);
            DataTable dtLevel = new DataTable();
            dtLevel = Lip.SelectRecord("Select Max(LevelNumber) AS LevelNumber  from Acc_AccountsLevels where BranchID=" + Comon.cInt(MySession.GlobalBranchID));
            int MaxLevel = Comon.cInt(dtLevel.Rows[0]["LevelNumber"].ToString());
            if (MaxLevel == Comon.cInt(cmbAccAccountLevel.EditValue))
            {
            
                chekStateCash.Enabled = true;
            }
            else
            {
               
                chekStateCash.Enabled = false;
            }
        }
        protected override void DoDelete()
        {
            int isTRansParent = Comon.cInt(Lip.GetValue(" SELECT count(*)  FROM  [dbo].[Acc_Accounts] where [ParentAccountID]='" + txtAccountID.Text + "'"));        
            if (isTRansParent > 0)
            {
                Messages.MsgExclamationk(Messages.TitleInfo, "يوجد حسابات فرعية تنتمي الى هذه الحساب الرئيسي لذلك لا يمكن حذفه ");
                return;
            }

            if (Lip.CheckAccountingTransactions(Comon.cLong(txtAccountID.Text))) 
            { 

            try
            {
                if (!FormDelete)
                {
                    Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToUpdateRecord);
                    return;
                }
                else
                {
                    bool Yes = Messages.MsgStopYesNo(Messages.TitleConfirm, Messages.msgConfirmDelete);
                    if (!Yes)
                        return;
                }
                double ParentParentAccountID =0;
                if(MySession.GlobalNoOfLevels>4)
                  ParentParentAccountID = Comon.cDbl(Lip.GetValue("SELECT  [ParentAccountID] FROM  [Acc_Accounts] where [AccountID]=" + txtParentID.Text + " and BranchID=" + Comon.cInt(cmbBranchesID.EditValue)));
                else if (MySession.GlobalNoOfLevels == 4)
                        ParentParentAccountID = Comon.cDbl(Lip.GetValue("SELECT  [ParentAccountID] FROM  [Acc_Accounts] where [AccountID]=" +txtAccountID.Text + " and BranchID=" + Comon.cInt(cmbBranchesID.EditValue)));
              
                //حذف الحساب اذا كان عميل من جدول العملاء 
                if (ParentParentAccountID ==Comon.cDbl( MySession.GlobalDefaultParentCustomerAccountID))
                {
                    cCustomers cClassCustomer = new cCustomers();
                    Sales_Customers modelCustomer = new Sales_Customers();
                    modelCustomer.AccountID = Comon.cDbl(txtAccountID.Text);
                    modelCustomer.EditUserID = UserInfo.ID;
                    modelCustomer.BranchID = UserInfo.BRANCHID;
                    modelCustomer.FacilityID = UserInfo.FacilityID;
                    modelCustomer.EditComputerInfo = UserInfo.ComputerInfo;
                    modelCustomer.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                    modelCustomer.EditTime = Comon.cLong(Lip.GetServerTimeSerial());
                    if (cClassCustomer.CheckAccountHasTransactions(Comon.cLong(txtAccountID.Text)) == true)
                    {
                        XtraMessageBox.Show("الحساب لديه حركة شراء وبيع لايمكن حذفه  ");
                        return;
                    }
                    else
                    {
                        bool ResultCustomer = Sales_CustomersDAL.DeleteSales_CustomersByAccountID(modelCustomer);                  
                    }
                 }
                // حذف الحساب اذا كان مورد من جدول الموردين 
                if (ParentParentAccountID ==Comon.cDbl( MySession.GlobalDefaultParentSupplierAccountID))
                {
                    cSuppliers cClassSupplier = new cSuppliers();
                    Sales_Suppliers modelSupplier = new Sales_Suppliers();
                    modelSupplier.AccountID = Comon.cDbl(txtAccountID.Text);
                    modelSupplier.EditUserID = UserInfo.ID;
                    modelSupplier.BranchID = UserInfo.BRANCHID;
                    modelSupplier.FacilityID = UserInfo.FacilityID;
                    modelSupplier.EditComputerInfo = UserInfo.ComputerInfo;
                    modelSupplier.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                    modelSupplier.EditTime = Comon.cLong(Lip.GetServerTimeSerial());
                    if (cClassSupplier.CheckAccountHasTransactions(Comon.cLong(txtAccountID.Text)) == true)
                    {
                        XtraMessageBox.Show("الحساب لديه حركة شراء وبيع لايمكن حذفه  ");
                        return;
                    }
                    else
                    {
                        bool ResultSupplier = Sales_SuppliersDAL.DeleteSales_SuppliersByAccountID(modelSupplier);
                    }
                }
                //حذف الحساب اذا كان صندوق من جدول الصناديق 
                if(ParentParentAccountID ==Comon.cDbl(MySession.GlobalDefaultParentBoxesAccountID))
                {
                     Acc_Boxes modelBox = new Acc_Boxes();
                     modelBox.AccountID = Comon.cDbl(txtAccountID.Text);
                     modelBox.EditUserID = UserInfo.ID;
                     modelBox.BranchID = UserInfo.BRANCHID;
                     modelBox.FacilityID = UserInfo.FacilityID;
                     modelBox.EditComputerInfo = UserInfo.ComputerInfo;
                     modelBox.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                     modelBox.EditTime = Comon.cLong(Lip.GetServerTimeSerial());
                     bool ResultBox = Acc_BoxesDAL.DeleteAcc_BoxesByAccountID(modelBox);
                }
                // حذف الحساب اذا كان بنك من جدول البنوك 
                if(ParentParentAccountID ==Comon.cDbl(MySession.GlobalDefaultParentBanksAccountID))
                {
                    Acc_Banks modelBank = new Acc_Banks();
                    modelBank.AccountID = Comon.cDbl(txtAccountID.Text);
                    modelBank.EditUserID = UserInfo.ID;
                    modelBank.BranchID = UserInfo.BRANCHID;
                    modelBank.FacilityID = UserInfo.FacilityID;
                    modelBank.EditComputerInfo = UserInfo.ComputerInfo;
                    modelBank.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                    modelBank.EditTime = Comon.cLong(Lip.GetServerTimeSerial());
                    bool ResultBank = Acc_BanksDAL.Delete_Acc_BanksByAccountID(modelBank);
                }
                // حذف الحساب اذا كان مخزن من جدول المخازن 
                if(ParentParentAccountID ==Comon.cDbl(MySession.GlobalDefaultParentStoreAccountID))
                {
                    Stc_Stores modelStore = new Stc_Stores();
                    modelStore.AccountID = Comon.cDbl(txtAccountID.Text);
                    modelStore.UserID = UserInfo.ID;
                    modelStore.BranchID = MySession.GlobalBranchID;
                    modelStore.FacilityID = MySession.GlobalFacilityID;
                    modelStore.EditUserID = UserInfo.ID;
                    modelStore.EditComputerInfo = UserInfo.ComputerInfo;
                    modelStore.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                    modelStore.EditTime = Comon.cLong(Lip.GetServerTimeSerial());

                    bool ResultStore = STC_STORES_DAL.DeleteStc_StoresByAccountID(modelStore);
                }
                Acc_Accounts model = new Acc_Accounts();
                model.AccountID = Comon.cLong(txtAccountID.Text);
                model.EditUserID = UserInfo.ID;
                model.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                model.FacilityID = UserInfo.FacilityID;
                model.EditComputerInfo = UserInfo.ComputerInfo;
                model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                model.EditTime = Comon.cLong(Lip.GetServerTimeSerial());
                model.Cancel = 1;

                bool Result = Acc_AccountsDAL.DeleteAcc_Accounts(model);
                if (Result == true)
                    Messages.MsgInfo(Messages.TitleInfo, Messages.msgDeleteComplete);

                GetAcountsTree();
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            }
            else
            {
                Messages.MsgAsterisk("لا يمكن الحذف", "لا يمكن حذف الحساب بسبب وجود حركات محاسبية على  الحساب");
            }
        }

        public void ClearFields()
        {
              
                    try
                    {
                        var AccountID = txtAccountID.Text;
                        txtAccountID.Text = GetNewAccountID(UserInfo.FacilityID, Comon.cInt(cmbBranchesID.EditValue), Comon.cLong(txtAccountID.Text)).ToString();

                        txtParentID.Text = AccountID;
                        cmbAccAccountLevel.EditValue = Comon.cInt(cmbAccAccountLevel.EditValue) + 1;
                        if (txtAccountID.Text.ToString().Substring(0, 1) == "1" || txtAccountID.Text.ToString().Substring(0, 1) == "2")
                        {
                            cmbAccAccounEnd.EditValue = 2;
                        }
                        else if (txtAccountID.Text.ToString().Substring(0, 1) == "3" || txtAccountID.Text.ToString().Substring(0, 1) == "4")
                        {
                            cmbAccAccounEnd.EditValue = 1;
                        }
                        txtArbName.Text = "";
                        txtEngName.Text = "";
                        txtMaxLimit.Text = "";
                        txtLocation.Text = "";
                        txtNotes.Text = "";
                        chkAllowLimit.Checked = false;
                        chekStateCash.Checked = false;
                        DataTable dtLevel = new DataTable();
                        dtLevel = Lip.SelectRecord("Select Max(LevelNumber) AS LevelNumber  from Acc_AccountsLevels where BranchID=" + Comon.cInt(MySession.GlobalBranchID));
                        int MaxLevel = Comon.cInt(dtLevel.Rows[0]["LevelNumber"].ToString());
                        if (MaxLevel == Comon.cInt(cmbAccAccountLevel.EditValue))
                        {
                            cmbTypeAcount.ItemIndex = 1;
                            chekStateCash.Enabled = true;
                        }
                        else
                        {
                            cmbTypeAcount.ItemIndex = 0;
                            chekStateCash.Enabled = false;
                        }

                        txtArbName.Focus();

                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                    }
                
                 
            
        }


        private long GetNewAccountID(int FACILITYID, int BranchID, long ParentAccountID)
        {
            long functionReturnValue = 0;
            string where = "FACILITYID=" + FACILITYID + " AND BRANCHID=" + BranchID ;
            int GlobalAccountsLevelDigits = MySession.GlobalAccountsLevelDigits;
            try
            {
                int code = 0;
                long AccountLevel = Comon.cLong(Lip.GetValue("SELECT AccountLevel FROM  Acc_Accounts WHERE AccountID = " + ParentAccountID + " AND  " + where + "  AND  Cancel=0"));
                int sNode = Comon.cInt(AccountLevel) + 1;
                int SumDigitsCountBeforeSelectedLevel = 0;
                int DigitsCountForSelectedLevel = 0;
                long MaxID = 0;
                string str = Lip.GetValue("SELECT  MAX(AccountID) FROM  Acc_Accounts Where ParentAccountID=" + ParentAccountID + "  And " + where + "  AND  Cancel=0");
                strSQL = "SELECT Sum(DigitsNumber) FROM  Acc_AccountsLevels WHERE  " + where + " And LevelNumber <" + sNode;
                SumDigitsCountBeforeSelectedLevel = Comon.cInt(Lip.GetValue(strSQL));
                strSQL = "SELECT  DigitsNumber FROM  Acc_AccountsLevels WHERE " + where + " And LevelNumber=" + sNode;
                DigitsCountForSelectedLevel = Comon.cInt(Lip.GetValue(strSQL));
                if (string.IsNullOrEmpty(str))
                {
                    code = 0;
                }
                else
                {
                    code = Comon.cInt(str.Substring(SumDigitsCountBeforeSelectedLevel, DigitsCountForSelectedLevel));
                }
                string strDigits = null;
                MaxID = 1;
                for (int i = 1; i <= DigitsCountForSelectedLevel; i++)
                {
                    MaxID = MaxID * 10;
                    strDigits = strDigits + "0";
                }
                // لكل مستوى عدد محدد من الحسابات
                if (code < MaxID)
                {

                    code = code + 1;
                    string strRet = ParentAccountID.ToString().Substring(0, SumDigitsCountBeforeSelectedLevel) + code.ToString(strDigits);
                    strRet = strRet.PadRight(GlobalAccountsLevelDigits, '0');
                    functionReturnValue = Comon.cLong(strRet);

                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            return functionReturnValue;
        }

        protected override void DoNew()
        {
                if(Comon.cInt(MySession.GlobalNoOfLevels)<=0)
                {
                    Messages.MsgWarning(Messages.TitleWorning, "الرجاء تعين عدد المستويات الدليل المحاسبي في الصلاحيات العامة ");
                    frmUserPermissions frm = new frmUserPermissions();
                    frm.Show();
                    return;
                }
            if (Comon.cInt(cmbAccAccountLevel.EditValue) == (MySession.GlobalNoOfLevels - 1))
                if (Lip.CheckPermionParentAccoutID() == false)
                {
                    Messages.MsgHand("خطأ إضافة حساب", "الرجاء تعريف جميع الحسابات الرئيسية الافتراضية من شاشة الصلاحيات..  ");
                    frmUserPermissions frm = new frmUserPermissions();
                    frm.ShowDialog();
                    frm.Focus();
                    return;
                }
            try
            {
                Validations.EnabledControl(this, true);
                IsNewRecord = true;
                ClearFields();
                txtArbName.Focus();
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        public void Find()
        {

            CSearch cls = new CSearch();
            int[] ColumnWidth = new int[] { 100, 300 };
            string Condition = "Where BranchID=" + cmbBranchesID.EditValue;
            FocusedControl = GetIndexFocusedControl();
            if (FocusedControl == txtAccountID.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, null, null, "AccountID", "رقم الحساب", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, null, null, "AccountID", "Account ID", MySession.GlobalBranchID);
            }

            if (FocusedControl == txtFromAccountID.Name || FocusedControl == txtToAccountID.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, null, null, "AccountID", "رقم الحساب", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, null, null, "AccountID", "Account ID", MySession.GlobalBranchID);
            }


            GetSelectedSearchValue(cls);

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


        public void GetSelectedSearchValue(CSearch cls)
        {
            if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
            {

                if (FocusedControl == txtAccountID.Name)
                {
                    txtAccountID.Text = cls.PrimaryKeyValue.ToString();
                    txtAccountID_Validating(null, null);
                }
                if (FocusedControl == txtFromAccountID.Name)
                {
                    txtFromAccountID.Text = cls.PrimaryKeyValue.ToString();
                    txtFromAccountID_Validating(null, null);
                }

                if (FocusedControl == txtToAccountID.Name)
                {
                    txtToAccountID.Text = cls.PrimaryKeyValue.ToString();
                    txtToAccountID_Validating(null, null);
                }

            }

        }

        private void txtAccountID_Validating(object sender, CancelEventArgs e)
        {
            strSQL = "SELECT * FROM Acc_Accounts WHERE BranchID = " +  cmbBranchesID.EditValue + "   AND (AccountID = " + txtAccountID.Text + ")";
            Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, UserInfo.Language.ToString());
            DataTable dt = Lip.SelectRecord(strSQL);

            if (dt.Rows.Count > 0)
            {
                txtAccountID.Text = dt.Rows[0]["AccountID"].ToString();
                txtArbName.Text = dt.Rows[0]["ArbName"].ToString();
                txtEngName.Text = dt.Rows[0]["EngName"].ToString();
                txtParentID.Text = dt.Rows[0]["ParentAccountID"].ToString();
                //txtNotes.Text = dt.Rows[0][""].ToString();
                cmbTypeAcount.ItemIndex = Comon.cInt(dt.Rows[0]["AccountTypeID"].ToString());
                cmbAccAccounEnd.EditValue = Comon.cInt(dt.Rows[0]["EndType"].ToString());

                // cmbTypeAcount.EditValue = dt.Rows[0]["AccountTypeID"].ToString();
                cmbAccAccountLevel.ItemIndex = Comon.cInt(dt.Rows[0]["AccountLevel"].ToString());

                if (Comon.cInt(dt.Rows[0]["StopAccount"].ToString()) == 1)
                    chkStopAccount.Checked = true;
                else
                    chkStopAccount.Checked = false;
                TreeListNode node = treeList1.FindNodeByFieldValue("ID", txtAccountID.Text.Trim());
                treeList1.SetFocusedNode(node);


            }

        }

        protected override void DoSearch()
        {
            try
            {
                
                txtArbName.Focus();
                txtAccountID.Focus();
                Find();
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void frmAccountsTree_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {
                txtAccountID.Focus();
                Find();
            }

            if (e.KeyCode == Keys.F9)
            {
                DoSave();
            }
            if (e.KeyCode == Keys.F11)
            {
                DoNew();
            }

        }

        private void cmbBranchesID_EditValueChanged(object sender, EventArgs e)
        {
            strSQL = "ArbName";
            Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, UserInfo.Language.ToString());
            FillCombo.FillComboBox(cmbTypeAcount, "Acc_AccountType", "ID", strSQL);
            FillCombo.FillComboBoxLookUpEdit(cmbAccAccountLevel, "Acc_AccountsLevels", "LevelNumber", "LevelNumber","","BranchID=" + cmbBranchesID.EditValue);
            FillCombo.FillComboBox(cmbAccAccounEnd, "Acc_AccountEnd", "ID", strSQL);



            GetAcountsTree();
            treeList1.ForceInitialize();
            treeList1.ExpandAll();
        }

        private void txtArbName_Validating(object sender, CancelEventArgs e)
        {
            TextEdit obj = (TextEdit)sender;
            if (UserInfo.Language == iLanguage.Arabic)
                txtEngName.Text = Translator.ConvertNameToOtherLanguage(obj.Text.Trim().ToString(), UserInfo.Language);
  
        }
        private void EnabledControl(bool Value)
        {
            foreach (Control item in this.Controls)
            {
                if (item is TextEdit && ((!(item.Name.Contains("AccountID"))) && (!(item.Name.Contains("AccountName")))))
                {
                    if (!(item.Name.Contains("lbl") && item.Name.Contains("Name")))
                    {
                        item.Enabled = Value;
                        ((TextEdit)item).Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black;
                        ((TextEdit)item).Properties.AppearanceDisabled.BackColor = System.Drawing.Color.White;
                        if (Value == true)
                            ((TextEdit)item).Properties.AppearanceDisabled.BackColor = System.Drawing.Color.White;
                    }
                }
                else if (item is TextEdit && (((item.Name.Contains("AccountID"))) || ((item.Name.Contains("AccountName")))))
                {
                    item.Enabled = Value;
                    ((TextEdit)item).Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black;
                    ((TextEdit)item).Properties.AppearanceDisabled.BackColor = System.Drawing.Color.White;
                    if (Value)
                        ((TextEdit)item).Properties.AppearanceDisabled.BackColor = System.Drawing.Color.White;
                }
                else if (item is SimpleButton && (((item.Name.Contains("btn"))) && ((item.Name.Contains("Search")))))
                {
                    ((SimpleButton)item).Enabled = Value;
                }
            }
        }

        private void txtFromAccountID_Validating(object sender, CancelEventArgs e)
        {

        }

        private void txtToAccountID_Validating(object sender, CancelEventArgs e)
        {

        }

        private void chekStateCash_CheckedChanged(object sender, EventArgs e)
        {
            if (chekStateCash.Checked == true)
            {
                rdUnDirect.Visible = true;
                rdDirect.Visible = true;
                rdAll.Visible = true;
            }
            else
            {
                rdUnDirect.Visible = false;
                rdDirect.Visible = false;
                rdAll.Visible = false;
            }
        }
        private void btnemport_Click(object sender, EventArgs e)
        {
            try
            {
                label1: 
                if(txtExcelPath.Text == string.Empty)
                    {
                        Messages.MsgError(Messages.TitleConfirm," يجب تحديد مسار ملف الأكسل ");
                        txtExcelPath.Focus();
                        btnSelectFile_Click(null, null);
                        goto label1;
                    }
                    EmportAccounts();
                    txtExcelPath.Text = "";
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm();
                Messages.MsgError(Messages.TitleError, "خطأ في الإستيراد - الرجاء مراجعة جميع حقول الملف والتأكد أنها حسب القالب المحدد" + ex.Message);
            }
        }
        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            try
            {     using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "All Files|*.*";
                    openFileDialog.FileName = "";
                    openFileDialog.CheckFileExists = false; // تعيين هذا الخيار للسماح بإغلاق النافذة دون تحديد ملف
                    if (openFileDialog.ShowDialog() == DialogResult.OK && openFileDialog.FileName!="")
                    {
                        txtExcelPath.Text = openFileDialog.FileName;
                        btnemport.Enabled = true;
                    }
                    else
                    {
                        btnemport.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
          
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }


        }
        private void EmportAccounts()
        {
            OleDbConnection oledbConn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + txtExcelPath.Text + ";Extended Properties=Excel 12.0");    
            bool Yes = Messages.MsgStopYesNo(Messages.TitleConfirm, "تأكيد الاسنيراد  ؟");
            if (!Yes)
                return;
            Application.DoEvents();
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
            oledbConn.Open();
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Sheet$]", oledbConn);
            OleDbDataAdapter oleda = new OleDbDataAdapter();
            oleda.SelectCommand = cmd;
            DataTable dt = new DataTable();
            oleda.Fill(dt);
            oledbConn.Close();
            if (dt.Rows.Count < 1)
                return;
            int Nolevel = Comon.cInt(dt.Rows[0]["LevelNumber"]);

            
            if(Nolevel<=0)
            {
                Messages.MsgError(Messages.TitleError, "لا يمكن استيراد الدليل مع عدد المستويات المحددة, الرجاء التأكد من عدد المستويات في الملف ومن ثم المتابعة");
                return;
            }        
            List<Acc_Accounts> ListAccountsTree = new List<Acc_Accounts>();         
            foreach (DataRow rows in dt.Rows)
                ListAccountsTree.Add(Acc_AccountsDAL.ConvertRowToObjFromEmport(rows));
            List<MyRecord> list = new List<MyRecord>();            
            if (ListAccountsTree != null)
            {
                SplashScreenManager.CloseForm();
                bool YesFormat = Messages.MsgWarningYesNo(Messages.TitleWorning, " هل تريد بالتاكيد الغاء دليل الحسابات السابق مع جميع بيانات الفرع ؟");
                if (!YesFormat)
                    return;
                try
                {
                    using (SqlConnection objCnn = new GlobalConnection().Conn)
                    {
                        objCnn.Open();
                        using (SqlCommand objCmd = objCnn.CreateCommand())
                        {
                            objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                            objCmd.CommandText = "[DeleteAllTable_SP]";
                            objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 1));
                            objCmd.Parameters.Add(new SqlParameter("@BranchID", Comon.cInt(MySession.GlobalBranchID)));
                            objCmd.ExecuteNonQuery();
                        }
                    }
                    string StrSql = "Delete from Acc_Accounts where BranchID=" + MySession.GlobalBranchID;
                    Lip.ExecututeSQL(StrSql);
                }
                catch
                {
                    Messages.MsgError(Messages.TitleError, "خطأ عملية حذف الدليل مع بيانات الفرع");
                }
                {
                    List<UserOtherPermissions> listUserOtherPermissions = new List<UserOtherPermissions>();
                    UserOtherPermissions UserOtherPermissions = new UserOtherPermissions();
                    UserOtherPermissions.UserID = UserInfo.ID;
                    UserOtherPermissions.BranchID = Comon.cInt(MySession.GlobalBranchID);
                    UserOtherPermissions.FacilityID = UserInfo.FacilityID;
                    UserOtherPermissions.OtherPermissionName = "NoOfLevels";
                    UserOtherPermissions.OtherPermissionValue = Nolevel.ToString();
                    UserOtherPermissions.OtherPermissionIndex = Comon.cInt(Nolevel);
                    listUserOtherPermissions.Add(UserOtherPermissions);
                    int Result = UsersManagementDAL.frmInsertUserOtherPermissions(UserInfo.ID, Comon.cInt(MySession.GlobalBranchID), listUserOtherPermissions);
                }
                for (int i = 0; i <= Nolevel; ++i)
                {
                    Lip.NewFields();
                    Lip.Table = "Acc_AccountsLevels";
                    Lip.AddNumericField("BranchID", MySession.GlobalBranchID);
                    Lip.AddNumericField("FacilityID", UserInfo.FacilityID);
                    Lip.AddNumericField("LevelNumber",  i+1);
                    Lip.AddNumericField("DigitsNumber", dt.Rows[i]["DigitsNumber"].ToString());
                    Lip.ExecuteInsert();
                }
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                
                for (int i = 0; i <= ListAccountsTree.Count - 1; i++)
                {
                    Acc_Accounts model = new Acc_Accounts();
                    model.AccountID = Comon.cLong(ListAccountsTree[i].AccountID);
                    model.ArbName = ListAccountsTree[i].ArbName;
                    model.EngName = ListAccountsTree[i].EngName;
                    model.ParentAccountID = Comon.cLong(ListAccountsTree[i].ParentAccountID);
                    model.MaxLimit = Comon.cLong(ListAccountsTree[i].MaxLimit);
                    model.MinLimit = 0;
                    model.Description = ListAccountsTree[i].Description;
                    model.Location = ListAccountsTree[i].Location;
                    model.StopAccount = ListAccountsTree[i].StopAccount;
                    model.AllowMaxLimit = ListAccountsTree[i].AllowMaxLimit;
                    model.CashState = ListAccountsTree[i].CashState;
                    model.AccountLevel = Comon.cInt(ListAccountsTree[i].AccountLevel);
                    model.AccountTypeID = Comon.cInt(ListAccountsTree[i].AccountTypeID);
                    model.EndType = Comon.cInt(ListAccountsTree[i].EndType);
                    model.UserID = ListAccountsTree[i].UserID;
                    model.EditUserID = ListAccountsTree[i].UserID;
                    model.BranchID = Comon.cInt(ListAccountsTree[i].BranchID);
                    model.FacilityID = ListAccountsTree[i].FacilityID;
                    model.ComputerInfo = ListAccountsTree[i].ComputerInfo;
                    model.EditComputerInfo = ListAccountsTree[i].ComputerInfo;
                    model.RegDate = Comon.cLong(ListAccountsTree[i].RegDate);
                    model.RegTime = Comon.cLong(ListAccountsTree[i].RegTime);
                    model.EditDate = Comon.cLong(ListAccountsTree[i].RegDate);
                    model.EditTime = Comon.cLong(ListAccountsTree[i].RegTime);
                    model.Cancel = ListAccountsTree[i].Cancel;
                    model.TypeAccount = ListAccountsTree[i].TypeAccount;
                    if (model.TypeAccount > 0)
                    {
                        // حفظ الصناديق في جدول الصناديق 
                        #region Save Boxes
                        if (model.TypeAccount == 1)
                        {
                            Acc_Boxes modelBox = new Acc_Boxes();
                            cBoxes cClassBox = new cBoxes();
                            
                            modelBox.BoxID = Comon.cInt(cClassBox.GetNewID().ToString());
                            modelBox.AccountID =model.AccountID;
                            //Comon.cLong(txtAccountID.Text);

                            modelBox.ArbName = model.ArbName;

                            modelBox.EngName = model.EngName;
                            modelBox.StopAccount = model.StopAccount;
                            modelBox.UserID = model.UserID;
                            modelBox.EditUserID = model.UserID;
                            modelBox.ComputerInfo = model.ComputerInfo;
                            modelBox.EditComputerInfo = model.ComputerInfo;
                            modelBox.BranchID = model.BranchID;
                            modelBox.FacilityID = model.FacilityID;

                            modelBox.Notes = model.Description;
                            modelBox.RegDate = Comon.cLong(Lip.GetServerDateSerial());
                            modelBox.RegTime = Comon.cLong(Lip.GetServerTimeSerial());
                            modelBox.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                            modelBox.EditTime = Comon.cLong(Lip.GetServerDateSerial());
                            modelBox.Cancel = 0;
                            modelBox.AccountID = Comon.cDbl(model.AccountID);
                            modelBox.ParentAccountID = Comon.cDbl(model.ParentAccountID);

                            int SaveID;
                            int UpdateID;
                           
                                SaveID = Acc_BoxesDAL.Insert_Acc_Boxes(modelBox);
                          

                        }
                        #endregion

                        // حفظ البنوك في جدول البنوك 
                        #region Save Bank
                        if (model.TypeAccount == 2)
                        {
                            Acc_Banks modelBank = new Acc_Banks();
                            cBanks cClassBank = new cBanks();
                             
                            modelBank.BankID = Comon.cInt(cClassBank.GetNewID().ToString());

                            modelBank.AccountID = Comon.cLong(model.AccountID);
                            modelBank.StopAccount = model.StopAccount;

                            modelBank.ArbName = model.ArbName;
                            modelBank.EngName = model.EngName;
                            modelBank.UserID = model.UserID;
                            modelBank.EditUserID = model.UserID;
                            modelBank.ComputerInfo = UserInfo.ComputerInfo;
                            modelBank.EditComputerInfo = UserInfo.ComputerInfo;
                            modelBank.BranchID = model.BranchID;
                            modelBank.FacilityID = model.FacilityID;
                            modelBank.Notes = model.Description;
                            modelBank.RegDate = Comon.cLong(Lip.GetServerDateSerial());
                            modelBank.RegTime = Comon.cLong(Lip.GetServerTimeSerial());
                            modelBank.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                            modelBank.EditTime = Comon.cLong(Lip.GetServerDateSerial());
                            modelBank.Cancel = 0;
                            modelBank.ParentAccountID = Comon.cLong(model.ParentAccountID);

                            int StoreID;
                            int UpdateID;
                            
                                StoreID = Acc_BanksDAL.Insert_Acc_Banks(modelBank);
                            

                        }
                        #endregion

                        //حفظ العملاء في جدول العملاء 
                        #region Save Customer
                        if (model.TypeAccount == 3)
                        {
                            Sales_Customers modelCustomer = new Sales_Customers();
                            cCustomers cClassCustomer = new cCustomers();
                            modelCustomer.CustomerID = Comon.cInt(cClassCustomer.GetNewID().ToString());

                            modelCustomer.AccountID = Comon.cLong(model.AccountID);
                            modelCustomer.ArbName =model.ArbName;
                            modelCustomer.EngName =model.EngName;
                            modelCustomer.StopAccount = model.StopAccount;
                            modelCustomer.UserID = UserInfo.ID;
                            modelCustomer.EditUserID = UserInfo.ID;
                            modelCustomer.ComputerInfo = UserInfo.ComputerInfo;
                            modelCustomer.EditComputerInfo = UserInfo.ComputerInfo;
                            modelCustomer.BranchID = UserInfo.BRANCHID;
                            modelCustomer.FacilityID = UserInfo.FacilityID;
                            modelCustomer.Notes = model.Description;

                            modelCustomer.RegDate = Comon.cLong(Lip.GetServerDateSerial());
                            modelCustomer.TransactionDate = Comon.cLong(Lip.GetServerDateSerial());
                            modelCustomer.RegTime = Comon.cLong(Lip.GetServerTimeSerial());
                            modelCustomer.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                            modelCustomer.EditTime = Comon.cLong(Lip.GetServerDateSerial());
                            modelCustomer.Cancel = 0;
                            modelCustomer.ContactPerson = "";
                            modelCustomer.IdentityNumber = "";
                            modelCustomer.CustomerType = "";
                            modelCustomer.BlockingReason = "";
                            modelCustomer.IsInBlackList = 0;
                            modelCustomer.Gender = 0;
                            modelCustomer.NationalityID = 0;

                            modelCustomer.ParentAccountID = Comon.cDbl(model.ParentAccountID);
                            int StoreID;
                            int UpdateID;
                            StoreID = Sales_CustomersDAL.InsertSales_Customers(modelCustomer);
                            
                        }
                        #endregion
                        // حفظ الموردين في جدول الموردين
                        #region Save Supplier
                        if (model.TypeAccount == 4)
                        {
                            Sales_Suppliers modelSupplier = new Sales_Suppliers();
                            cSuppliers cClassSuplier = new cSuppliers();
                            modelSupplier.SupplierID = Comon.cInt(cClassSuplier.GetNewID().ToString());
                             
                            //Comon.cLong(txtAccountID.Text);
                            modelSupplier.StopAccount = chkStopAccount.Checked == true ? 1 : 0;
                            modelSupplier.AccountID = Comon.cDbl(model.AccountID);
                            modelSupplier.ArbName = model.ArbName;
                            modelSupplier.EngName = model.EngName;
                            modelSupplier.UserID = UserInfo.ID;
                            modelSupplier.EditUserID = UserInfo.ID;
                            modelSupplier.ComputerInfo = UserInfo.ComputerInfo;
                            modelSupplier.EditComputerInfo = UserInfo.ComputerInfo;
                            modelSupplier.BranchID = model.BranchID;
                            modelSupplier.FacilityID = UserInfo.FacilityID;
                            modelSupplier.Notes = model.Description;
                            modelSupplier.RegDate = Comon.cLong(Lip.GetServerDateSerial());
                            modelSupplier.RegTime = Comon.cLong(Lip.GetServerTimeSerial());
                            modelSupplier.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                            modelSupplier.EditTime = Comon.cLong(Lip.GetServerDateSerial());
                            modelSupplier.Cancel = 0;
                            modelSupplier.ParentAccountID = Comon.cDbl(model.ParentAccountID);
                            int StoreID;
                            int UpdateID;
                            StoreID = Sales_SuppliersDAL.InsertSales_Suppliers(modelSupplier);
                            
                        }
                        #endregion

                        //حفظ المخازن الى جدول المخازن 
                        #region Save Store
                        if (model.TypeAccount == 5)
                        {
                            Stc_Stores modelStore = new Stc_Stores();

                            modelStore.StoreID = Comon.cInt(STC_STORES_DAL.GetNewID().ToString());
                            
                            modelStore.StopAccount = model.StopAccount;
                            modelStore.AccountID = Comon.cLong(model.AccountID);
                            modelStore.ArbName = model.ArbName;
                            modelStore.EngName = model.EngName;


                            modelStore.FacilityID = MySession.GlobalFacilityID;
                            modelStore.BranchID = model.BranchID;

                            modelStore.Notes = model.Description;
                            modelStore.UserID = UserInfo.ID;
                            modelStore.EditUserID = UserInfo.ID;
                            modelStore.ComputerInfo = UserInfo.ComputerInfo;
                            modelStore.EditComputerInfo = UserInfo.ComputerInfo;

                            modelStore.RegDate = Comon.cLong(Lip.GetServerDateSerial());
                            modelStore.RegTime = Comon.cLong(Lip.GetServerTimeSerial());

                            model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                            model.EditTime = Comon.cLong(Lip.GetServerDateSerial());


                            modelStore.ParentAccountID = Comon.cDbl(model.ParentAccountID);
                            modelStore.Cancel = 0;
                            int StoreID;
                            bool updateModel;
                   
                                StoreID = STC_STORES_DAL.InsertStc_Stores(modelStore);
                           
                        }
                        #endregion
                        //حفظ الموظف الى جدول الموظفين   
                        #region Save Employee
                        if (model.TypeAccount == 6)
                        {

                            cEmployeeFiles cClass = new cEmployeeFiles();
                            HR_EmployeeFile modelEMP = new HR_EmployeeFile();
                            modelEMP.EmployeeID = Comon.cInt(cClass.GetNewID().ToString());
                            
                            modelEMP.OnAccountID = Comon.cLong(model.AccountID);

                            modelEMP.StopAccount = model.StopAccount;
                            modelEMP.ArbName = model.ArbName;

                            modelEMP.EngName =model.EngName;
                            modelEMP.WorkingHours = 0;
                            modelEMP.UserID = UserInfo.ID;
                            modelEMP.EditUserID = UserInfo.ID;
                            modelEMP.ComputerInfo = UserInfo.ComputerInfo;
                            modelEMP.EditComputerInfo = UserInfo.ComputerInfo;
                            modelEMP.BranchID = model.BranchID;
                            modelEMP.FacilityID = UserInfo.FacilityID;
                            modelEMP.WorkTel = "";
                            modelEMP.WorkMobile = "";
                            modelEMP.BankAccountID = "";
                            modelEMP.AddressNotes = "";
                            modelEMP.FootprintEmpID = 0;
                            modelEMP.EmpNotes = model.Description;
                            modelEMP.WorkEmail = "";
                            modelEMP.RegDate = Comon.cLong(Lip.GetServerDateSerial());
                            modelEMP.RegTime = Comon.cLong(Lip.GetServerTimeSerial());
                            modelEMP.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                            modelEMP.EditTime = Comon.cLong(Lip.GetServerDateSerial());
                            modelEMP.Cancel = 0;
                            modelEMP.Termination = 0;
                            modelEMP.BankAccountID = "";
                            modelEMP.BirthPlace = "";
                            modelEMP.ComputerInfo = "";
                            modelEMP.HomeMobil = "";
                            modelEMP.HomeTel = "";
                            modelEMP.HomeAddress = "";
                            modelEMP.HomeWorkDistance = "";
                            modelEMP.Sex = 1;
                            modelEMP.MaritalStatus = 1;
                            modelEMP.BirthDate = 0;
                            modelEMP.Nationality = 1;
                            modelEMP.Religions = 1;
                            modelEMP.WorkType = 1;
                            modelEMP.Occupation = 1;
                            modelEMP.IqamaOccupation = 1;
                            modelEMP.ContractType = 1;
                            modelEMP.Administration = 1;
                            modelEMP.Department = 1;
                            modelEMP.PaymentMethod = 1;
                            modelEMP.TerminationReason = 1;
                            modelEMP.Department = 1;
                            modelEMP.StopSalary = 0;

                            modelEMP.ClinicID = 0;
                            modelEMP.Emptype = 0;
                            modelEMP.CostCenterID = 0;
                            modelEMP.LeaveNotes = "";
                            modelEMP.WorkAddress = "";
                            modelEMP.CompanyVehicle = "";
                            modelEMP.CurrentSponsor = "";
                            modelEMP.ParentAccountID = Comon.cDbl(model.ParentAccountID);
                            long StoreID;
                            bool UpdateID = false;
                            StoreID =  HR_EmployeeFileDAL.InsertHR_EmployeeFile(modelEMP, IsNewRecord);
                        

                        }
                        #endregion
                    }
                    Acc_AccountsDAL.InsertAcc_Accounts(model);
                    list.Add(new MyRecord(ListAccountsTree[i].AccountID, ListAccountsTree[i].ParentAccountID, ListAccountsTree[i].ArbName + "," + ListAccountsTree[i].AccountID.ToString()));
                }
                treeList1.DataSource = list;
                SplashScreenManager.CloseForm();
                Messages.MsgInfo(Messages.TitleConfirm, "تم الاستيراد بنجاح ");
            }
        }
        protected override void DoExport()
        {
            try
            {
                if (this.Tag == "Xlsx")
                {
                   
                    var excelApp = new Microsoft.Office.Interop.Excel.Application();
                    var workbook = excelApp.Workbooks.Add();
                    var worksheet = workbook.ActiveSheet;            
                    worksheet.Name = "Sheet";
                
                    // Set the column headers
                    worksheet.Range["A1"].Value = "AccountID";
                    worksheet.Range["B1"].Value = "ArbName";
                    worksheet.Range["C1"].Value = "EngName";
                    worksheet.Range["D1"].Value = "ParentAccountID";
                    worksheet.Range["E1"].Value = "AccountLevel";
                    worksheet.Range["F1"].Value = "AccountTypeID";
                    worksheet.Range["G1"].Value = "MaxLimit";
                    worksheet.Range["H1"].Value = "EndType";
                    worksheet.Range["I1"].Value = "CashState";
                    worksheet.Range["J1"].Value = "Location";
                    worksheet.Range["K1"].Value = "Description";
                    worksheet.Range["L1"].Value = "StopAccount";
                    worksheet.Range["M1"].Value = "UserID";
                    worksheet.Range["N1"].Value = "RegDate";
                    worksheet.Range["O1"].Value = "BranchID";
                    worksheet.Range["P1"].Value = "TypeAccount";
                    worksheet.Range["Q1"].Value = "AllowMaxLimit";
                    worksheet.Range["R1"].Value = "LevelNumber";
                    worksheet.Range["S1"].Value = "DigitsNumber";
                    // Populate the data
                    int row = 2;
                    List<Acc_Accounts> ListAccountsTree = new List<Acc_Accounts>();
                    ListAccountsTree = Acc_AccountsDAL.GetAllData(Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID);
                    foreach (var account in ListAccountsTree)
                    {
                        worksheet.Cells[row, 1] = account.AccountID;
                        worksheet.Cells[row, 2] = account.ArbName;
                        worksheet.Cells[row, 3] = account.EngName;
                        worksheet.Cells[row, 4] = account.ParentAccountID;
                        worksheet.Cells[row, 5] = account.AccountLevel;
                        worksheet.Cells[row, 6] = account.AccountTypeID;
                        worksheet.Cells[row, 7] = account.MaxLimit;
                        worksheet.Cells[row, 8] = account.EndType;
                        worksheet.Cells[row, 9] = account.CashState;
                        worksheet.Cells[row, 10] = account.Location;
                        worksheet.Cells[row, 11] = account.Description;
                        worksheet.Cells[row, 12] = account.StopAccount;
                      
                        worksheet.Cells[row, 13] = account.UserID;
                        worksheet.Cells[row, 14] = account.RegDate;
                        worksheet.Cells[row, 15] = account.BranchID;
                        double ParentParentAccountID = 0;
                        if (MySession.GlobalNoOfLevels > 4)
                            ParentParentAccountID = Comon.cDbl(Lip.GetValue("SELECT  [ParentAccountID] FROM  [Acc_Accounts] where [AccountID]=" + account.ParentAccountID + " and BranchID=" + Comon.cInt(account.BranchID)));
                        else if (MySession.GlobalNoOfLevels == 4)
                            ParentParentAccountID = Comon.cDbl(Lip.GetValue("SELECT  [ParentAccountID] FROM  [Acc_Accounts] where [AccountID]=" + account.ParentAccountID + " and BranchID=" + Comon.cInt(account.BranchID)));

                        if (ParentParentAccountID > 0)
                        {
                            if (ParentParentAccountID == Comon.cDbl(MySession.GlobalDefaultParentBoxesAccountID))
                                account.TypeAccount = 1;
                            if (ParentParentAccountID == Comon.cDbl(MySession.GlobalDefaultParentBanksAccountID))
                                account.TypeAccount = 2;
                            if (ParentParentAccountID == Comon.cDbl(MySession.GlobalDefaultParentCustomerAccountID))
                                account.TypeAccount = 3;
                            if (ParentParentAccountID == Comon.cDbl(MySession.GlobalDefaultParentSupplierAccountID))
                                account.TypeAccount = 4;
                            if (ParentParentAccountID == Comon.cDbl(MySession.GlobalDefaultParentStoreAccountID))
                                account.TypeAccount = 5;
                            if (ParentParentAccountID == Comon.cDbl(MySession.GlobalDefaultParentEmployeeAccountID))
                                account.TypeAccount = 6;
                        }
                        worksheet.Cells[row, 17] = account.AllowMaxLimit;
                        worksheet.Cells[row, 16] = account.TypeAccount;
                        row++;
                    }
                    worksheet.Cells[2, 18] = MySession.GlobalNoOfLevels;
                    int d = 2;
                    DataTable datadigits = Lip.SelectRecord("SELECT  [DigitsNumber]    FROM  [Acc_AccountsLevels] where [BranchID]=" + MySession.GlobalBranchID);
                    for (int i = 0; i <= datadigits.Rows.Count-1; i++)
                    {
                        worksheet.Cells[d, 19] = datadigits.Rows[i]["DigitsNumber"];
                        d++;
                    }
                    using (var saveFileDialog = new SaveFileDialog())
                    {
                        saveFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                        saveFileDialog.FileName = "AccountingGuide.xlsx";
                        saveFileDialog.CheckFileExists = false; // تعيين هذا الخيار للسماح بإغلاق النافذة دون تحديد ملف
                        if (saveFileDialog.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(saveFileDialog.FileName))
                        {
                          SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);

                            workbook.SaveAs(saveFileDialog.FileName);

                            workbook.Close();
                            excelApp.Quit();
                            SplashScreenManager.CloseForm();
                            Messages.MsgInfo(Messages.TitleInfo, (UserInfo.Language == iLanguage.Arabic) ? "تم التصدير بنجاح" : "Export completed successfully");
                        }
                        else
                        {
                            workbook.Close(false); // إغلاق الملف وعدم حفظه
                            excelApp.Quit();
                        }
                    }
                }
                #region Export Pdf
                else if (this.Tag == "pdf")
                {

                    List<Acc_Accounts> ListAccountsTree = new List<Acc_Accounts>();
                    ListAccountsTree = Acc_AccountsDAL.GetAllData(Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID);

                    // Create a new PDF document
                    Document document = new Document();

                    // Prompt the user to select a folder to save the PDF file
                    using (var saveFileDialog = new SaveFileDialog())
                    {
                         SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                        saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
                        saveFileDialog.Title = "Save PDF File";
                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                           
                            // Create a PdfWriter instance with the selected file path
                            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(saveFileDialog.FileName, FileMode.Create));
                            document.Open();
                            // Create a table with the column headers
                            PdfPTable table = new PdfPTable(20);
                            table.WidthPercentage = 200;
                            // Set the column headers
                            table.AddCell("AccountID");
                            table.AddCell("ArbName");
                            table.AddCell("EngName");
                            table.AddCell("ParentAccountID");
                            table.AddCell("AccountLevel");
                            table.AddCell("AccountTypeID");
                            table.AddCell("MaxLimit");
                            table.AddCell("EndType");
                            table.AddCell("CashState");
                            table.AddCell("Location");
                            table.AddCell("Description");
                            table.AddCell("StopAccount");
                            table.AddCell("UserID");
                            table.AddCell("RegDate");
                            table.AddCell("BranchID");

                            // Populate the data
                            foreach (var account in ListAccountsTree)
                            {
                                table.AddCell(account.AccountID.ToString());
                                table.AddCell(account.ArbName);
                                table.AddCell(account.EngName);
                                table.AddCell(account.ParentAccountID.ToString());
                                table.AddCell(account.AccountLevel.ToString());
                                table.AddCell(account.AccountTypeID.ToString());
                                table.AddCell(account.MaxLimit.ToString());
                                table.AddCell(account.EndType.ToString());
                                table.AddCell(account.CashState.ToString());
                                table.AddCell(account.Location);
                                table.AddCell(account.Description);
                                table.AddCell(account.StopAccount.ToString());
                                table.AddCell(account.UserID.ToString());
                                table.AddCell(account.RegDate.ToString());
                                table.AddCell(account.BranchID.ToString());
                            }
                            // Add the table to the document
                            document.Add(table);
                            // Close the document
                            document.Close();
                            writer.Close();

                            SplashScreenManager.CloseForm();
                            Messages.MsgInfo(Messages.TitleInfo, (UserInfo.Language == iLanguage.Arabic) ? "تم التصدير بنجاح" : "Export completed successfully");
                        }
                        else
                        {
                            document.Close();
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                //SplashScreenManager.CloseForm();
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void cmbTypeAcount_EditValueChanged(object sender, EventArgs e)
        {
             
            if (Comon.cInt(cmbTypeAcount.EditValue)==1)
            {
                 
                chekStateCash.Enabled = true;
            }
            else
            {
               
                chekStateCash.Enabled = false;
            }
        }
        private void txtArbName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ',')
            {
                e.Handled = true;
            }

        }

        private void chkAllowLimit_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAllowLimit.Checked && Comon.cInt(txtMaxLimit.Text) <= 0)
            {
                chkAllowLimit.Checked = false;
                Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "الرجاء ادخالالحد الاعلى في الخانة المخصصة له ومن ثم تحديد عدم التجاوز" : "Please enter the upper limit in the box designated for it and then specify not to exceed");
                txtMaxLimit.Focus();
            }
        }

             



    }
}

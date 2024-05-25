using Edex.DAL.Stc_itemDAL;
using Edex.Model;
using Edex.Model.Language;
using Edex.GeneralObjects.GeneralClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Edex.SalesAndPurchaseObjects.Transactions
{
    public partial class frmAddNewItemFromCahier : Form
    {
        public frmAddNewItemFromCahier()
        {
            InitializeComponent();

            FillCombo.FillComboBox(cmbUnits, "Stc_SizingUnits", "SizeID", "ArbName", "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
             
        }

        public string GetDateExpire()
        { 
            if (string.IsNullOrEmpty(cmbUnits.Text))
               return   "";

            SaveDB(txtBarCode.Text, Comon.cInt(cmbUnits.EditValue),1, Comon.cDec(txtSalePrice.Text), txtArbName.Text);
            return txtBarCode.Text;
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            GetDateExpire();
            this.Close();
        }

        public void SaveDB(string BarCode, int SizeID = 1, int PackingQty = 1, decimal SalePrice = 0, string ArbName = "")
        {
            
            int ItemID = 0;
            Stc_Items objRecord = new Stc_Items();

            objRecord.BranchID = 0;
            objRecord.FacilityID = UserInfo.FacilityID;

            objRecord.ItemID = Comon.cLong(Stc_itemsDAL.GetNewID().ToString());
            objRecord.ArbName = ArbName;
            objRecord.EngName = ArbName;

            objRecord.GroupID = 1;
            objRecord.TypeID = 1;


            objRecord.Notes = "";
            objRecord.IsVAT = 0;
            objRecord.Cancel = 0;
            //user Info
            objRecord.UserID = UserInfo.ID;
            objRecord.RegDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
            objRecord.RegTime = Comon.cDbl(Lip.GetServerTimeSerial()); ;
            objRecord.ComputerInfo = UserInfo.ComputerInfo;

            objRecord.EditUserID = 0;
            objRecord.EditTime = 0;
            objRecord.EditDate = 0;
            objRecord.EditComputerInfo = "";


            objRecord.picItemImage = null;

            Stc_ItemUnits returned;
            List<Stc_ItemUnits> listreturned = new List<Stc_ItemUnits>();

            returned = new Stc_ItemUnits();
            returned.ID = 1;
            returned.FacilityID = UserInfo.FacilityID;
            returned.BranchID = UserInfo.BRANCHID;
            returned.BarCode = BarCode;
            returned.ItemID = ItemID;
            returned.ItemProfit = 0;
            returned.PackingQty = PackingQty;
            returned.SizeID = SizeID;
            returned.SalePrice = SalePrice;
            returned.CostPrice = 0;
            returned.LastCostPrice = 0;
            returned.LastSalePrice = 0;
            returned.MaxLimitQty = 100;
            returned.MinLimitQty = 10;
            returned.SpecialCostPrice = 0;
            returned.SpecialSalePrice = 0;
            returned.UnitCancel = 0;
            returned.AllowedPercentDiscount = 0;
            returned.AverageCostPrice = 0;

            returned.Serials = "";
            if (returned.PackingQty > 0 && returned.SizeID > 0)

                listreturned.Add(returned);



            if (listreturned.Count > 0)
            {
                objRecord.Stc_ItemUnits = listreturned;
                string Result = Stc_itemsDAL.InsertUsingXML(objRecord, true);

            }
            

        }

        private void frmAddNewItemFromCahier_Load(object sender, EventArgs e)
        {
           
        }

        private void txtSalePrice_Validating(object sender, CancelEventArgs e)
        {
            GetDateExpire();
            this.Close();
        }

    }
}

using DevExpress.BarCodes;
using DevExpress.XtraEditors.Repository;
using Edex.Model;
using Edex.Model.Language;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Edex.ModelSystem
{
    static public class Common
    {
        /// <summary>
        /// This function is used to query the contents of tables, and LookUpEdit is filled from the output of the sent queries,
        /// as Parameter is a reference to LookUpEdit that will be sent at the time of calling the function and other parameters,
        /// which are the primary key of the table that we want to query from, the name of the table and the condition
        /// </summary>
        /// <param name="rItem"></param>
        /// <param name="primary"></param>
        /// <param name="Table"></param>
        /// <param name="Name"></param>
        /// <param name="Condition"></param>
        static public void filllookupEDit(ref RepositoryItemLookUpEdit rItem, string primary, string Table, string Name, string Condition)
        {
            // RepositoryItemLookUpEdit rItem = new RepositoryItemLookUpEdit();

            if (UserInfo.Language == iLanguage.Arabic)
            {
                rItem.DataSource = Lip.SelectRecord("SELECT distinct   " + primary + "   AS ID, " + Name + "  As Name  FROM   " + Table + "   where  " + Condition).DefaultView;
                rItem.DisplayMember = "Name";
                rItem.ValueMember = "ID";
                rItem.NullText = "";
                rItem.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Optimistic;
            }
            else {
                rItem.DataSource = Lip.SelectRecord("SELECT distinct   " + primary + "   AS ID, " + Name + "  As Name  FROM   " + Table + "   where  " + Condition).DefaultView;
                rItem.DisplayMember = "Name";
                rItem.ValueMember = "ID";
                rItem.NullText = "";
                rItem.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Optimistic;
            
                     
            }



        }
        /// <summary>
        /// This function is used to convert the names sent to it from Arabic to English using the translator in the translator class,
        /// where it receives the parameter of the word or name of type String
        /// </summary>
        /// <param name="Word"></param>
        /// <returns> Returns the word after it is also translated as string</returns>
        static public string getWordEng(string Word)
        {
            return Translator.ConvertNameToOtherLanguage(Word.ToString(), iLanguage.English);
        }
        /// <summary>
        /// This function is used to convert the names sent to it from English to Arabic  using the translator in the translator class,
        /// where it receives the parameter of the word or name of type String
        /// </summary>
        /// <param name="Word"></param>
        /// <returns></returns>
        static public string getWordArb(string Word)
        {
            return Translator.ConvertNameToOtherLanguage(Word.ToString(), iLanguage.Arabic);
        }
        /// <summary>
        /// This function is used to query all group names from the Stc_ItemsGroups table.
        /// </summary>
        /// <returns>returns an object of type RepositoryItemLookUpEdit which contains the contents of the query</returns>
        static public RepositoryItemLookUpEdit LookUpEditGroupItemID()
        {

            RepositoryItemLookUpEdit rItem = new RepositoryItemLookUpEdit();

            if (UserInfo.Language == iLanguage.Arabic)
            {
                rItem.DataSource = Lip.SelectRecord("SELECT  [ArbName]  as [إسم المجموعة] FROM Stc_ItemsGroups WHERE Cancel=0 and BranchID="+MySession.GlobalBranchID).DefaultView;
                rItem.DisplayMember = "إسم المجموعة";
                rItem.ValueMember = "إسم المجموعة";
                rItem.NullText = "";
                rItem.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Optimistic;
            }
            else
            {

                rItem.DataSource = Lip.SelectRecord("SELECT  [EngName] as [Group Name] FROM Stc_ItemsGroups WHERE Cancel=0 and BranchID=" + MySession.GlobalBranchID).DefaultView;
                rItem.DisplayMember = "Group Name";
                rItem.ValueMember = "Group Name";
                rItem.NullText = "";
                rItem.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Optimistic;
            }
            return rItem;
        }
        /// <summary>
        /// This function is used to query all Account Name  from the Acc_Accounts table.
        /// </summary>
        /// <returns>returns an object of type RepositoryItemLookUpEdit which contains the contents of the query</returns>
        static public RepositoryItemLookUpEdit LookUpEditAccountName()
        {

            RepositoryItemLookUpEdit rItem = new RepositoryItemLookUpEdit();

            if (UserInfo.Language == iLanguage.Arabic)
            {
                rItem.DataSource = Lip.SelectRecord("SELECT  ArbName as [اسم الحسـاب] FROM Acc_Accounts WHERE Cancel =0   AND AccountLevel=" + MySession.GlobalNoOfLevels + " AND BranchID = " + UserInfo.BRANCHID).DefaultView;
                rItem.DisplayMember = "اسم الحسـاب";
                rItem.ValueMember = "اسم الحسـاب";
                rItem.NullText = "";
                rItem.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Strong;
            }
            else
            {

                rItem.DataSource = Lip.SelectRecord("SELECT  EngName as [Account Name] FROM Acc_Accounts WHERE  Cancel =0   AND AccountLevel=" + MySession.GlobalNoOfLevels + " AND BranchID = " + UserInfo.BRANCHID).DefaultView;
                rItem.DisplayMember = "Account Name";
                rItem.ValueMember = "Account Name";
                rItem.NullText = "";
                rItem.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Strong;
            }
            return rItem;

        }

        static public RepositoryItemLookUpEdit LookUpEditAccountName(int NoOfLevel)
        {

            RepositoryItemLookUpEdit rItem = new RepositoryItemLookUpEdit();

            if (UserInfo.Language == iLanguage.Arabic)
            {
                rItem.DataSource = Lip.SelectRecord("SELECT  ArbName as [اسم الحسـاب] FROM Acc_Accounts WHERE Cancel =0   AND AccountLevel=" + NoOfLevel + " AND BranchID = " + MySession.GlobalBranchID).DefaultView;
                rItem.DisplayMember = "اسم الحسـاب";
                rItem.ValueMember = "اسم الحسـاب";
                rItem.NullText = "";
                rItem.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Strong;
            }
            else
            {

                rItem.DataSource = Lip.SelectRecord("SELECT  EngName as [Account Name] FROM Acc_Accounts WHERE  Cancel =0   AND AccountLevel=" + NoOfLevel + " AND BranchID = " +MySession.GlobalBranchID).DefaultView;
                rItem.DisplayMember = "Account Name";
                rItem.ValueMember = "Account Name";
                rItem.NullText = "";
                rItem.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Strong;
            }
            return rItem;

        }
        /// <summary>
        /// This function is used to query all Item ID  from the Sales_BarCodeForPurchaseInvoiceArb_Find with Arbic Langaug  or Sales_BarCodeForPurchaseInvoiceEng_Find with English  View.
        /// </summary>
        /// <returns>returns an object of type RepositoryItemLookUpEdit which contains the contents of the query</returns>
        static public RepositoryItemLookUpEdit LookUpEditItemID()
        {

            RepositoryItemLookUpEdit rItem = new RepositoryItemLookUpEdit();

            if (UserInfo.Language == iLanguage.Arabic)
            {
                rItem.DataSource = Lip.SelectRecord("SELECT distinct [رقم المادة] FROM Sales_BarCodeForPurchaseInvoiceArb_Find where BranchID=" + MySession.GlobalBranchID).DefaultView;
                rItem.DisplayMember = "رقم المادة";
                rItem.ValueMember = "رقم المادة";
                rItem.NullText = "";
                rItem.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Optimistic;
            }
            else
            {

                rItem.DataSource = Lip.SelectRecord("SELECT distinct [ItemID] FROM Sales_BarCodeForPurchaseInvoiceEng_Find where BranchID=" + MySession.GlobalBranchID).DefaultView;
                rItem.DisplayMember = "ItemID";
                rItem.ValueMember = "ItemID";
                rItem.NullText = "";
                rItem.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Optimistic;
            }
            return rItem;

        }
        /// <summary>
        /// This function is used to query all Item ID  from the Sales_BarCodeForPurchaseInvoiceArb_Find with Arbic Langaug  or Sales_BarCodeForPurchaseInvoiceEng_Find with English  View.
        /// </summary>
        /// <returns>returns an object of type RepositoryItemLookUpEdit which contains the contents of the query</returns>
        static public RepositoryItemLookUpEdit LookUpEditItemIDService()
        {

            RepositoryItemLookUpEdit rItem = new RepositoryItemLookUpEdit();

            if (UserInfo.Language == iLanguage.Arabic)
            {
                rItem.DataSource = Lip.SelectRecord("SELECT distinct [رقم المادة] FROM Sales_BarCodeForPurchaseInvoiceArb_Find where IsService=" + 1 + " and BranchID=" + MySession.GlobalBranchID).DefaultView;
                rItem.DisplayMember = "رقم المادة";
                rItem.ValueMember = "رقم المادة";
                rItem.NullText = "";
                rItem.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Optimistic;
            }
            else
            {

                rItem.DataSource = Lip.SelectRecord("SELECT distinct [ItemID] FROM Sales_BarCodeForPurchaseInvoiceEng_Find where IsService=" + 1 + " and BranchID=" + MySession.GlobalBranchID).DefaultView;
                rItem.DisplayMember = "ItemID";
                rItem.ValueMember = "ItemID";
                rItem.NullText = "";
                rItem.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Optimistic;
            }
            return rItem;

        }
        /// <summary>
        /// This function is used to query all Item Name  from the Sales_BarCodeForPurchaseInvoiceArb_Find with Arbic Langaug  or Sales_BarCodeForPurchaseInvoiceEng_Find with English  View.
        /// </summary>
        /// <returns>returns an object of type RepositoryItemLookUpEdit which contains the contents of the query</returns>
        static public RepositoryItemLookUpEdit LookUpEditItemName()
        {

            RepositoryItemLookUpEdit rItem = new RepositoryItemLookUpEdit();

            if (UserInfo.Language == iLanguage.Arabic)
            {
                rItem.DataSource = Lip.SelectRecord("SELECT distinct [اسـم الـمـادة] FROM Sales_BarCodeForPurchaseInvoiceArb_Find where BranchID=" + MySession.GlobalBranchID).DefaultView;
                rItem.DisplayMember = "اسـم الـمـادة";
                rItem.ValueMember = "اسـم الـمـادة";
                rItem.NullText = "";
                rItem.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Optimistic;
            }
            else
            {

                rItem.DataSource = Lip.SelectRecord("SELECT distinct [EngName] FROM Sales_BarCodeForPurchaseInvoiceEng_Find where BranchID=" + MySession.GlobalBranchID).DefaultView;
                rItem.DisplayMember = "EngName";
                rItem.ValueMember = "EngName";
                rItem.NullText = "";
                rItem.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Optimistic;
            }
            return rItem;

        }
        static public RepositoryItemLookUpEdit LookUpEditItemNameService()
        {

            RepositoryItemLookUpEdit rItem = new RepositoryItemLookUpEdit();

            if (UserInfo.Language == iLanguage.Arabic)
            {
                rItem.DataSource = Lip.SelectRecord("SELECT distinct [اسـم الـمـادة] FROM Sales_BarCodeForPurchaseInvoiceArb_Find where IsService=" + 1 + " and BranchID=" + MySession.GlobalBranchID).DefaultView;
                rItem.DisplayMember = "اسـم الـمـادة";
                rItem.ValueMember = "اسـم الـمـادة";
                rItem.NullText = "";
                rItem.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Optimistic;
            }
            else
            {

                rItem.DataSource = Lip.SelectRecord("SELECT distinct [ItemName] FROM Sales_BarCodeForPurchaseInvoiceEng_Find where IsService=" + 1 + " and BranchID=" + MySession.GlobalBranchID).DefaultView;
                rItem.DisplayMember = "ItemName";
                rItem.ValueMember = "ItemName";
                rItem.NullText = "";
                rItem.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Optimistic;
            }
            return rItem;

        }
        /// <summary>
        /// This function is used to query all   Size Name and Size ID from the Stc_SizingUnits  Table.
        /// </summary>
        /// <returns>returns an object of type RepositoryItemLookUpEdit which contains the contents of the query</returns>
        static public RepositoryItemLookUpEdit LookUpEditSizeForItems()
        {
            RepositoryItemLookUpEdit rSizeName = new RepositoryItemLookUpEdit();
            if (UserInfo.Language == iLanguage.Arabic)
            {
                string StrSQL = @"SELECT  dbo.Stc_SizingUnits.ArbName  as [ArbSizeName] ,dbo.Stc_SizingUnits.SizeID   from dbo.Stc_SizingUnits  WHERE  (dbo.Stc_SizingUnits.Cancel = 0) and BranchID="+MySession.GlobalBranchID;
                rSizeName.DataSource = Lip.SelectRecord(StrSQL).DefaultView;
                rSizeName.DisplayMember = "ArbSizeName";
                rSizeName.ValueMember = "SizeID";
                rSizeName.NullText = "";
                rSizeName.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Optimistic;
            }
            else
            {
                string StrSQL = @"SELECT  dbo.Stc_SizingUnits.ArbName  as [Size Name],dbo.Stc_SizingUnits.SizeID  as [Size ID] from dbo.Stc_SizingUnits  WHERE  (dbo.Stc_SizingUnits.Cancel = 0) and BranchID="+MySession.GlobalBranchID;
                rSizeName.DataSource = Lip.SelectRecord(StrSQL).DefaultView;
                rSizeName.DisplayMember = "Size Name";
                rSizeName.ValueMember = "Size ID";
                rSizeName.NullText = "";
                rSizeName.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Optimistic;
            }
            return rSizeName;
        }

        /// <summary>
        /// This function is used to query all Item Name   from   Sales_BarCodeForPurchaseInvoicePyGRoupArb_Find  View  if The Launguge is Arbic 
        /// or Sales_BarCodeForPurchaseInvoicePyGRoupArb_Find View if English
        /// </summary>
        /// <returns> This method returns a RepositoryItemLookUpEdit control with data loaded from a table</returns>
        static public RepositoryItemLookUpEdit LookUpEditItemNamePyGropbID()
        {
            RepositoryItemLookUpEdit rItem = new RepositoryItemLookUpEdit();
            if (UserInfo.Language == iLanguage.Arabic)
            {

                rItem.DataSource = Lip.SelectRecord("SELECT distinct [اسـم الـمـادة] FROM Sales_BarCodeForPurchaseInvoicePyGRoupArb_Find where BranchID=" + MySession.GlobalBranchID).DefaultView;
                rItem.DisplayMember = "اسـم الـمـادة";
                rItem.ValueMember = "اسـم الـمـادة";
                rItem.NullText = "";
                rItem.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Optimistic;
            }
            else
            {
                rItem.DataSource = Lip.SelectRecord("SELECT distinct [ItemName] FROM Sales_BarCodeForPurchaseInvoicePyGRoupArb_Find where  BranchID=" + MySession.GlobalBranchID).DefaultView;
                rItem.DisplayMember = "ItemName";
                rItem.ValueMember = "ItemName";
                rItem.NullText = "";
                rItem.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Optimistic;
            }
            return rItem;
        }

        /// <summary>
        /// This function is used to query all   Name and ID Stores    from   Stc_Stores  Table With Condtion BranchID=MySession.GlobalBranchID 
        /// </summary>
        /// <returns>This method returns a RepositoryItemLookUpEdit control with data loaded from a table</returns>
        static public RepositoryItemLookUpEdit LookUpEditStoreName()
        {

            RepositoryItemLookUpEdit rItem = new RepositoryItemLookUpEdit();

            if (UserInfo.Language == iLanguage.Arabic)
            {
                rItem.DataSource = Lip.SelectRecord("SELECT distinct ArbName,AccountID  FROM Stc_Stores where cancel =0 and BranchID="+MySession.GlobalBranchID).DefaultView;
                rItem.DisplayMember = "ArbName";
                rItem.ValueMember = "AccountID";
                rItem.NullText = "";
                rItem.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Optimistic;
            }
            else
            {

                rItem.DataSource = Lip.SelectRecord("SELECT distinct [ItemName] FROM Sales_BarCodeForPurchaseInvoiceEng_Find where BranchID=" + MySession.GlobalBranchID).DefaultView;
                rItem.DisplayMember = "ItemName";
                rItem.ValueMember = "ItemName";
                rItem.NullText = "";
                rItem.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Optimistic;
            }
            return rItem;

        }
       
        /// <summary>
        /// This function is used to query   BarCode  from   Sales_BarCodeForPurchaseInvoiceArb_Find  View  if The Launguge is Arbic 
        /// or Sales_BarCodeForPurchaseInvoiceEng_Find View if English
        /// </summary>
        /// <returns>This method returns a RepositoryItemLookUpEdit control with data loaded from a table</returns>
        static public RepositoryItemLookUpEdit LookUpEditBarCode()
        {
            /************************ Look Up Edit **************************/
            RepositoryItemLookUpEdit rBarCode = new RepositoryItemLookUpEdit();

            if (UserInfo.Language == iLanguage.Arabic)
            {
                rBarCode.DataSource = Lip.SelectRecord("SELECT  [البـاركـود] FROM Sales_BarCodeForPurchaseInvoiceArb_Find where BranchID=" + MySession.GlobalBranchID).DefaultView;
                rBarCode.DisplayMember = "البـاركـود";
                rBarCode.ValueMember = "البـاركـود";
                rBarCode.NullText = "";
                rBarCode.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Optimistic;
            }
            else
            {
                rBarCode.DataSource = Lip.SelectRecord("SELECT  [BarCode] FROM Sales_BarCodeForPurchaseInvoiceEng_Find where BranchID="+MySession.GlobalBranchID).DefaultView;
                rBarCode.DisplayMember = "BarCode";
                rBarCode.ValueMember = "BarCode";
                rBarCode.NullText = "";
                rBarCode.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Optimistic;
            }
            return rBarCode;
        }

        /// <summary>
        /// This function is used to query   BarCode  from   Sales_BarCodeForPurchaseInvoiceArb_Find  View  if The Launguge is Arbic 
        /// or Sales_BarCodeForPurchaseInvoiceEng_Find View if English
        /// </summary>
        /// <returns>This method returns a RepositoryItemLookUpEdit control with data loaded from a table</returns>
        static public RepositoryItemLookUpEdit LookUpEditBarCodeSirvice()
        {
            /************************ Look Up Edit **************************/
            RepositoryItemLookUpEdit rBarCode = new RepositoryItemLookUpEdit();

            if (UserInfo.Language == iLanguage.Arabic)
            {
                rBarCode.DataSource = Lip.SelectRecord("SELECT  [البـاركـود] FROM Sales_BarCodeForPurchaseInvoiceArb_Find where IsService=" + 1 + " and BranchID=" + MySession.GlobalBranchID).DefaultView;
                rBarCode.DisplayMember = "البـاركـود";
                rBarCode.ValueMember = "البـاركـود";
                rBarCode.NullText = "";
                rBarCode.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Optimistic;
            }
            else
            {
                rBarCode.DataSource = Lip.SelectRecord("SELECT  [BarCode] FROM Sales_BarCodeForPurchaseInvoiceEng_Find  where  IsService=" + 1 + " and BranchID=" + MySession.GlobalBranchID).DefaultView;
                rBarCode.DisplayMember = "BarCode";
                rBarCode.ValueMember = "BarCode";
                rBarCode.NullText = "";
                rBarCode.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Optimistic;
            }
            return rBarCode;
        }
        
        /// <summary>
        /// This function is used to query  Size Name  according to the terms of their existence in the two tables     Sales_PurchaseInvoiceDetails,Stc_SizingUnits  Table.
        /// </summary>
        /// <param name="ItemID"></param>
        /// <returns>This method returns a RepositoryItemLookUpEdit control with data loaded from a table</returns>
        static public RepositoryItemLookUpEdit LookUpEditSize(double ItemID)
        {

            RepositoryItemLookUpEdit rSizeName = new RepositoryItemLookUpEdit();

            if (UserInfo.Language == iLanguage.Arabic)
            {
                string StrSQL = @"SELECT  dbo.Stc_SizingUnits.ArbName  as [ArbSizeName] 
                                FROM      dbo.Stc_Items INNER JOIN
                                dbo.Sales_PurchaseInvoiceDetails ON dbo.Stc_Items.ItemID = dbo.Sales_PurchaseInvoiceDetails.ItemID LEFT OUTER JOIN
                                dbo.Stc_SizingUnits ON dbo.Sales_PurchaseInvoiceDetails.Cancel = dbo.Stc_SizingUnits.Cancel AND dbo.Sales_PurchaseInvoiceDetails.SizeID = dbo.Stc_SizingUnits.SizeID and dbo.Sales_PurchaseInvoiceDetails.BranchID = dbo.Stc_SizingUnits.BranchID
                                WHERE        (dbo.Sales_PurchaseInvoiceDetails.InvoiceID = - 1) AND (dbo.Stc_SizingUnits.Cancel = 0) AND (dbo.Sales_PurchaseInvoiceDetails.Cancel = 0) and Sales_PurchaseInvoiceDetails.BranchID=" + MySession.GlobalBranchID;
                rSizeName.DataSource = Lip.SelectRecord(StrSQL + "And dbo.Sales_PurchaseInvoiceDetails.ItemID=" + ItemID).DefaultView;
                rSizeName.DisplayMember = "ArbSizeName";
                rSizeName.ValueMember = "ArbSizeName";
                rSizeName.NullText = "";
                rSizeName.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Optimistic;
            }
            else
            {
                string StrSQL = @"SELECT   dbo.Stc_SizingUnits.EngName as  [SizeName] 
                                FROM            dbo.Stc_Items INNER JOIN
                                dbo.Sales_PurchaseInvoiceDetails ON dbo.Stc_Items.ItemID = dbo.Sales_PurchaseInvoiceDetails.ItemID LEFT OUTER JOIN
                                dbo.Stc_SizingUnits ON dbo.Sales_PurchaseInvoiceDetails.Cancel = dbo.Stc_SizingUnits.Cancel AND dbo.Sales_PurchaseInvoiceDetails.SizeID = dbo.Stc_SizingUnits.SizeID and dbo.Sales_PurchaseInvoiceDetails.BranchID = dbo.Stc_SizingUnits.BranchID
                                WHERE        (dbo.Sales_PurchaseInvoiceDetails.InvoiceID = - 1) AND (dbo.Stc_SizingUnits.Cancel = 0) AND (dbo.Sales_PurchaseInvoiceDetails.Cancel = 0) and Sales_PurchaseInvoiceDetails.BranchID=" + MySession.GlobalBranchID;

                rSizeName.DataSource = Lip.SelectRecord(StrSQL + "And dbo.Sales_PurchaseInvoiceDetails.ItemID=" + ItemID).DefaultView;
                rSizeName.DisplayMember = "Size Name";
                rSizeName.ValueMember = "Size Name";
                rSizeName.NullText = "";
                rSizeName.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Optimistic;
            }  

            return rSizeName;
        }
       /// <summary>
        /// This function is used to query  Size Name  from   Stc_SizingUnits  Table   
       /// </summary>
       /// <returns></returns>
        static public RepositoryItemLookUpEdit LookUpEditSize()
        {

            RepositoryItemLookUpEdit rSizeName = new RepositoryItemLookUpEdit();

            if (UserInfo.Language == iLanguage.Arabic)
            {
                string StrSQL = @"SELECT  dbo.Stc_SizingUnits.ArbName  as [اسـم الـوحـــده] from dbo.Stc_SizingUnits  WHERE  (dbo.Stc_SizingUnits.Cancel = 0) and BranchID=" + MySession.GlobalBranchID;
                rSizeName.DataSource = Lip.SelectRecord(StrSQL).DefaultView;
                rSizeName.DisplayMember = "اسـم الـوحـــده";
                rSizeName.ValueMember = "اسـم الـوحـــده";
                rSizeName.NullText = "";
                rSizeName.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Optimistic;
            }
            else
            {
                string StrSQL = @"SELECT  dbo.Stc_SizingUnits.EngName  as [Size Name] from dbo.Stc_SizingUnits  WHERE  (dbo.Stc_SizingUnits.Cancel = 0) and BranchID=" + MySession.GlobalBranchID;
                rSizeName.DataSource = Lip.SelectRecord(StrSQL).DefaultView;
                rSizeName.DisplayMember = "Size Name";
                rSizeName.ValueMember = "Size Name";
                rSizeName.NullText = "";
                rSizeName.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Optimistic;
            }
            return rSizeName;
        }

        /// <summary>
        /// This function is used to query  Caliber Name  from   Stc_Calibers  Table   
        /// </summary>
        /// <returns></returns>
        static public RepositoryItemLookUpEdit LookUpEditCaliber()
        {

            RepositoryItemLookUpEdit rCaliberName = new RepositoryItemLookUpEdit();

            if (UserInfo.Language == iLanguage.Arabic)
            {
                string StrSQL = @"SELECT  dbo.Stc_Calibers.ArbName  as [اسـم العيار] from dbo.Stc_Calibers  WHERE  (dbo.Stc_Calibers.Cancel = 0) ";
                rCaliberName.DataSource = Lip.SelectRecord(StrSQL).DefaultView;
                rCaliberName.DisplayMember = "اسـم العيار";
                rCaliberName.ValueMember = "اسـم العيار";
                rCaliberName.NullText = "";
                rCaliberName.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Optimistic;
            }
            else
            {
                string StrSQL = @"SELECT  dbo.Stc_Calibers.EngName  as [Caliber Name] from dbo.Stc_Calibers  WHERE  (dbo.Stc_Calibers.Cancel = 0) ";
                rCaliberName.DataSource = Lip.SelectRecord(StrSQL).DefaultView;
                rCaliberName.DisplayMember = "Caliber Name";
                rCaliberName.ValueMember = "Caliber Name";
                rCaliberName.NullText = "";
                rCaliberName.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Optimistic;
            }
            return rCaliberName;
        } 
        /// <summary>
        /// This method generates a QR code image based on the provided text.
        /// </summary>
        /// <param name="txt"></param>
        /// <returns> return the generated QR code image/returns>
        public static Image GenratCod(string txt)
        {
            BarCode barCode = new BarCode(); // create a new BarCode object
            barCode.Symbology = Symbology.QRCode; // set the symbology to QRCode
            barCode.CodeText = txt;  
            barCode.BackColor = Color.White; 
            barCode.ForeColor = Color.Black; 
            barCode.RotationAngle = 0;  
            barCode.CodeBinaryData = Encoding.Default.GetBytes(barCode.CodeText); // set the code binary data
            barCode.Options.QRCode.CompactionMode = QRCodeCompactionMode.Byte; // Set options for the QR Code
            barCode.Options.QRCode.ErrorLevel = QRCodeErrorLevel.Q;
            barCode.Options.QRCode.ShowCodeText = false;
            barCode.DpiX = 72; // set the X resolution to 72 dpi
            barCode.DpiY = 72; // set the Y resolution to 72 dpi
            barCode.Module = 2f; // set the module size to 2f
            return barCode.BarCodeImage; 
        }

    }
}

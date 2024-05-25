using Edex.GeneralObjects.GeneralClasses;
using Edex.Model;
using Edex.Model.Language;
using Edex.GeneralObjects.GeneralForms;
using System;
using System.Windows.Forms;
using Edex.ModelSystem;

namespace Edex.GeneralObjects.GeneralClasses
{
    

    public static class PrepareSearchQuery
    {

        public static void SearchForParentAccounts(Control IDCtrl, Control NameCtrl)
        {
            try
            {
                CSearch cls = new CSearch();
                int[] ColumnWidth = new int[] { 100, 450 };

                PrepareSearchScreen("ParentAccountID", ref cls, ref ColumnWidth, UserInfo.Language, 1);
                if (cls.SQLStr != "")
                {
                    frmSearch frm = new frmSearch();
                    frm.AddSearchData(cls);
                    frm.ColumnWidth = ColumnWidth;
                    cls.PrimaryKeyName = "ParentAccountID";
                    cls.strFilter = "رقم الحساب";
                    frm.ShowDialog();
                    if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
                    {
                        IDCtrl.Text = (cls.PrimaryKeyValue.Trim());
                        NameCtrl.Text = (cls.PrimaryKeyName.Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                // WT.msgError("Module1", System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        public static void PrepareSearchScreen(string SearchType, ref CSearch cls, ref int[] ColumnWidth, iLanguage Language, int GlobalBranchID, string Condition = "")
        {
            try
            {

                
                switch (SearchType)
                {

                    case "ImageIDInDesignFactory":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("ImageID", "رقم الصورة");
                                cls.SQLStr = "SELECT  ImageID as [رقم الصورة] ,ImageCode AS [كود],TheImage as [الصورة ] , Notes AS [تفاصيل],ApprovalImage as [اعتماد الصور] FROM MNG_ARCHIVINGDOCUMENTSIMAGES WHERE  1=1 ";
                            }
                            else
                            {
                                cls.AddField("ImageID",  "Image ID");
                                cls.SQLStr = "SELECT ImageID as [Image ID],ImageCode AS[Image Code],TheImage as [Image], Notes AS [Notes],ApprovalImage as[Approval Image] FROM MNG_ARCHIVINGDOCUMENTSIMAGES WHERE    and 1=1 ";
                            }
                            cls.SearchCol = 1;
                            ColumnWidth = new int[] { 100, 200, 150, 90, 90, 200 };
                            break;
                        }

                    case "ImageCode":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("ImageCode", "كود");
                                cls.SQLStr = "SELECT ImageCode AS [كود],TheImage as [الصورة ] , Notes AS [تفاصيل]FROM MNG_ARCHIVINGDOCUMENTSIMAGES WHERE  ApprovalImage=1 ";
                            }
                            else
                            {
                                cls.AddField("ImageCode", "ImageCode");
                                cls.SQLStr = "SELECT UserID AS [User ID],TheImage as [Image], Notes AS [Name] FROM MNG_ARCHIVINGDOCUMENTSIMAGES WHERE    and ApprovalImage=1 ";
                            }
                            cls.SearchCol = 1;
                            ColumnWidth = new int[] { 100, 200, 150, 90, 90, 200 };
                            break;
                        }
                  

                    case "UserID":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("UserID", "رقم المستخدم");
                                cls.SQLStr = "SELECT UserID AS [رقم المستخدم], ArbName AS [اسم المستخدم] FROM Users WHERE (Cancel = 0) and BranchID=" + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("UserID", "User ID");
                                cls.SQLStr = "SELECT UserID AS [User ID], ArbName AS [User Name] FROM Users WHERE (Cancel = 0)  and BranchID=" + GlobalBranchID;
                            }
                            cls.SearchCol = 1;
                            ColumnWidth = new int[] { 100, 450, 150, 90, 90, 200 };
                           
                            break;
                        }
                    case "CurrencyID":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("ID", "رقم العملة");
                                cls.SQLStr = "SELECT ID AS [رقم العملة], ArbName AS [اسم العملة] FROM Acc_Currency WHERE (Cancel = 0)";
                            }
                            else
                            {
                                cls.AddField("ID", "Currency ID");
                                cls.SQLStr = "SELECT ID AS [Currency ID], ArbName AS [Currency Name] FROM Acc_Currency WHERE (Cancel = 0)";
                            }
                            cls.SearchCol = 1;
                            ColumnWidth = new int[] { 100, 450, 150, 90, 90, 200 };
                            break;
                        }
                    case "Nationalities":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("ID", "الرقم");
                                cls.SQLStr = "SELECT  ID AS [الرقم], ArbName AS [الجنسية] FROM HR_Nationalities WHERE (Cancel = 0)";
                            }
                            else
                            {
                                cls.AddField("ID", "ID");
                                cls.SQLStr = "SELECT ID AS [ID], ArbName AS [Nationalities] FROM HR_Nationalities WHERE (Cancel = 0)";
                            }
                            cls.SearchCol = 1;
                            ColumnWidth = new int[] { 100, 200 };
                            break;
                        }

                    case "VariousVoucherID":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("VoucherID", "رقم السـند");
                                cls.SQLStr = " SELECT * FROM Acc_VariousVoucherArb_Find WHERE 0 <> [رقم السـند]  And  BranchID = " + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("VoucherID", "Voucher ID");
                                cls.SQLStr = " SELECT * FROM Acc_VariousVoucherEng_Find WHERE [Voucher ID] <>0 And  BranchID = " + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 0, 80, 75, 80, 200, 85, 150, 100, 100, 100, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 2;
                            break;
                        }


                    case "AccountID":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("AccountID", "رقم الحساب");
                                cls.SQLStr = "SELECT AccountID as [رقم الحساب], ArbName as [اسم الحسـاب] FROM Acc_Accounts WHERE Cancel =0 And BranchID = " + GlobalBranchID + " AND AccountLevel=" + MySession.GlobalNoOfLevels;
                            }
                            else
                            {
                                cls.AddField("AccountID", "Account ID");
                                cls.SQLStr = "SELECT AccountID as [Account ID], EngName as [Account Name] FROM Acc_Accounts WHERE  Cancel =0 And BranchID = " + GlobalBranchID + " AND AccountLevel=" + MySession.GlobalNoOfLevels;
                            }
                            ColumnWidth = new int[] { 150, 406, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 1;
                            break;
                        }

                    case "Forms":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("Forms", "اسم الشاشة");
                                cls.SQLStr = "SELECT [FormName] as [الاسم البرمجي]   ,[ArbCaption] as [اسم الشاشة] ,[FACILITYID],[BRANCHID] FROM  [Forms] WHERE  BranchID = " + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("Forms", "Form Name");
                                cls.SQLStr = "SELECT [FormName] as [Program Name]  ,[EngCaption] as [Form Name],[FACILITYID],[BRANCHID] FROM  [Forms] WHERE  BranchID = " + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 150, 406, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 1;
                            break;
                        }
                    case "AccountIDWithoutLevel":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("AccountID", "رقم الحساب");
                                cls.SQLStr = "SELECT AccountID as [رقم الحساب], ArbName as [اسم الحسـاب] FROM Acc_Accounts WHERE Cancel =0 And BranchID = " + GlobalBranchID  ;
                            }
                            else
                            {
                                cls.AddField("AccountID", "Account ID");
                                cls.SQLStr = "SELECT AccountID as [Account ID], EngName as [Account Name] FROM Acc_Accounts WHERE  Cancel =0 And BranchID = " + GlobalBranchID  ;
                            }
                            ColumnWidth = new int[] { 150, 406, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 1;
                            break;
                        }
                    case "CustomerIDAndSublierID1":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("CustomerID", "رقم الــعـــمـــيــل");
                                cls.SQLStr = "SELECT AccountID as  [رقم الــعـــمـــيــل],ArbName as [اسـم الــعـــمـــيــل]  FROM Sales_Customers  Where    BranchID =" + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("AcountID", "Customer ID");
                                cls.SQLStr = "SELECT AccountID as  [Customer ID],EngName as [Customer Name]  FROM Sales_Customers  Where   BranchID =" + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 150, 450, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 1;
                            break;
                        }
                    case "ParentAccountID":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("AccountID", "رقم الحساب");
                                cls.SQLStr = "SELECT AccountID as [رقم الحساب], ArbName as [اسم الحسـاب] FROM Acc_Accounts WHERE  Cancel =0 And BranchID = " + GlobalBranchID + " AND AccountLevel=" + (MySession.GlobalNoOfLevels - 1);
                            }
                            else
                            {
                                cls.AddField("AccountID", "Account ID");
                                cls.SQLStr = "SELECT AccountID as [Account ID], EngName as [Account Name] FROM Acc_Accounts WHERE  Cancel =0 And BranchID = " + GlobalBranchID + " AND AccountLevel=" + (MySession.GlobalNoOfLevels - 1);
                            }
                            ColumnWidth = new int[] { 150, 406, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 1;
                            break;
                        }






                    case "CostCenterID":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("CostCenterID", "رقم مركز التكلفة");
                                cls.SQLStr = "SELECT CostCenterID as [رقم مركز التكلفة], ArbName as [اسم مركز التكلفة] FROM Acc_CostCenters WHERE  Cancel =0   And BranchID = " + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("CostCenterID", "Cost Center ID");
                                cls.SQLStr = "SELECT  CostCenterID as [Cost Center ID], EngName as [Cost Center Name] FROM Acc_CostCenters WHERE   Cancel =0 And BranchID = " + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 150, 420 };
                            cls.SearchCol = 1;
                            break;
                        }



                    case "ReceiptVoucher":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("ReceiptVoucherID", "رقم السـند");
                                cls.SQLStr = " SELECT * FROM Acc_ReceiptVoucherArb_Find WHERE  BranchID = " + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("ReceiptVoucherID", "Voucher ID");
                                cls.SQLStr = " SELECT * FROM Acc_ReceiptVoucherEng_Find WHERE   BranchID = " + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 0, 80, 75, 80, 200, 85, 150, 100, 100, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 2;
                            break;
                        }

                    case "RequestForPaymentOf":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("RequestID", "رقم الطلب");
                                cls.SQLStr = " SELECT RequestID As [رقم الطلب],PersonName As [الشخص المعني],PaymentReason AS [سبب الدفعة] FROM Others_RequestForPaymentOf_Master WHERE  BranchID = " + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("RequestID", "Request ID");
                                cls.SQLStr = " SELECT RequestID As [Request ID],PersonName As [Person Name],PaymentReason AS [Payment Reason] FROM Others_RequestForPaymentOf_Master WHERE   BranchID = " + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 50, 80, 75, 80, 200, 85, 150, 100, 100, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 2;
                            break;
                        }



                    case "SpendVoucher":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("SpendVoucherID", "رقم السـند");
                                cls.SQLStr = " SELECT * FROM Acc_SpendVoucherArb_Find WHERE   BranchID = " + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("SpendVoucherID", "Voucher ID");
                                cls.SQLStr = " SELECT * FROM Acc_SpendVoucherEng_Find WHERE   BranchID = " + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 0, 80, 75, 80, 200, 85, 150, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 2;
                            break;
                        }

                    case "DriverID":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("DriverID", "رقم الموصل");
                                cls.SQLStr = " SELECT DriverID as [رقم الموصل],Arbname as [اسم الموصل], mobile as [رقم الجوال]  FROM Sales_Drivers WHERE   BranchID = " + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("DriverID", "Driver ID");
                                cls.SQLStr = " SELECT DriverID,Arbname , mobile FROM Sales_Drivers WHERE   BranchID = " + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 80, 120, 75, 80 };
                            cls.SearchCol = 1;
                            break;
                        }


                    case "CheckReceiptVoucher":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("CheckReceiptVoucherID", "رقـم الـشـيـك");
                                cls.SQLStr = "SELECT *  FROM Acc_CheckReceiptVoucherArb_Find WHERE BranchID = " + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("CheckReceiptVoucherID", "Check ID");
                                cls.SQLStr = "SELECT *  FROM Acc_CheckReceiptVoucherEng_Find WHERE BranchID = " + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 0, 80, 90, 80, 200, 150, 85, 150, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 2;
                            break;
                        }

                    case "CheckSpendVoucher":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("CheckSpendVoucherID", "رقـم الـشـيـك");
                                cls.SQLStr = "SELECT * FROM Acc_CheckSpendVoucherArb_Find WHERE BranchID = " + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("CheckSpendVoucherID", "Check ID");
                                cls.SQLStr = "SELECT * FROM Acc_CheckSpendVoucherEng_Find WHERE BranchID = " + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 0, 80, 90, 80, 200, 150, 85, 150, 100, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 2;
                            break;
                        }

                    case "ChequesUnderCollectionVoucher":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("CheckSpendVoucherID", "رقـم الـشـيـك");
                                cls.SQLStr = "SELECT  dbo.Acc_ChequesUnderCollectionMaster.CheckSpendVoucherID AS [رقـم الـشـيـك], CASE WHEN dbo.Acc_ChequesUnderCollectionMaster.CheckSpendVoucherDate = 0"
                               + " THEN '0' ELSE SUBSTRING(ltrim(str(CheckSpendVoucherDate)), 1, 4) + '/' + SUBSTRING(ltrim(str(CheckSpendVoucherDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(CheckSpendVoucherDate)), 7, 2)"
                               + " END AS [تاريخ الاستحقاق], dbo.Acc_ChequesUnderCollectionDetails.AccountID AS [رقم الحسـاب], dbo.Acc_Accounts.ArbName AS [اسم الحسـاب], dbo.Acc_ChequesUnderCollectionDetails.CostCenterID "
                               + " AS [رقم م تكلفة], dbo.Acc_CostCenters.ArbName AS [اسم م تكلفة], dbo.Acc_ChequesUnderCollectionDetails.ReceiptName AS [اسـم الـمـسـتـلـم] FROM dbo.Acc_ChequesUnderCollectionDetails"
                               + " LEFT OUTER JOIN dbo.Acc_CostCenters ON dbo.Acc_ChequesUnderCollectionDetails.BranchID = dbo.Acc_CostCenters.BranchID AND dbo.Acc_ChequesUnderCollectionDetails.CostCenterID = "
                               + " dbo.Acc_CostCenters.CostCenterID LEFT OUTER JOIN dbo.Acc_Accounts ON dbo.Acc_ChequesUnderCollectionDetails.BranchID = dbo.Acc_Accounts.BranchID AND dbo.Acc_ChequesUnderCollectionDetails.AccountID"
                               + " = dbo.Acc_Accounts.AccountID RIGHT OUTER JOIN dbo.Acc_ChequesUnderCollectionMaster ON dbo.Acc_ChequesUnderCollectionDetails.BranchID = dbo.Acc_ChequesUnderCollectionMaster.BranchID AND"
                               + " dbo.Acc_ChequesUnderCollectionDetails.CheckSpendVoucherID = dbo.Acc_ChequesUnderCollectionMaster.CheckSpendVoucherID WHERE (dbo.Acc_ChequesUnderCollectionMaster.Cancel = 0) AND "
                               + " (dbo.Acc_ChequesUnderCollectionMaster.BranchID = " + GlobalBranchID + ") ORDER BY [رقـم الـشـيـك]";
                            }
                            else
                            {
                                cls.AddField("CheckSpendVoucherID", "Check ID");
                                cls.SQLStr = "SELECT  dbo.Acc_ChequesUnderCollectionMaster.CheckSpendVoucherID AS [Check ID], CASE WHEN dbo.Acc_ChequesUnderCollectionMaster.CheckSpendVoucherDate = 0"
                                + " THEN '0' ELSE SUBSTRING(ltrim(str(CheckSpendVoucherDate)), 1, 4) + '/' + SUBSTRING(ltrim(str(CheckSpendVoucherDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(CheckSpendVoucherDate)), 7, 2)"
                                + " END AS [Check Date], dbo.Acc_ChequesUnderCollectionDetails.AccountID AS [Account ID], dbo.Acc_Accounts.EngName AS [Account Name], dbo.Acc_ChequesUnderCollectionDetails.CostCenterID "
                                + " AS [Cost Center ID], dbo.Acc_CostCenters.EngName AS [Cost Center Name], dbo.Acc_ChequesUnderCollectionDetails.ReceiptName AS [Receipt Name] FROM dbo.Acc_ChequesUnderCollectionDetails"
                                + " LEFT OUTER JOIN dbo.Acc_CostCenters ON dbo.Acc_ChequesUnderCollectionDetails.BranchID = dbo.Acc_CostCenters.BranchID AND dbo.Acc_ChequesUnderCollectionDetails.CostCenterID = "
                                + " dbo.Acc_CostCenters.CostCenterID LEFT OUTER JOIN dbo.Acc_Accounts ON dbo.Acc_ChequesUnderCollectionDetails.BranchID = dbo.Acc_Accounts.BranchID AND dbo.Acc_ChequesUnderCollectionDetails.AccountID"
                                + " = dbo.Acc_Accounts.AccountID RIGHT OUTER JOIN dbo.Acc_ChequesUnderCollectionMaster ON dbo.Acc_ChequesUnderCollectionDetails.BranchID = dbo.Acc_ChequesUnderCollectionMaster.BranchID AND"
                                + " dbo.Acc_ChequesUnderCollectionDetails.CheckSpendVoucherID = dbo.Acc_ChequesUnderCollectionMaster.CheckSpendVoucherID WHERE (dbo.Acc_ChequesUnderCollectionMaster.Cancel = 0) AND "
                                + " (dbo.Acc_ChequesUnderCollectionMaster.BranchID = " + GlobalBranchID + ") ORDER BY [Check ID]";
                            }
                            ColumnWidth = new int[] { 80, 90, 80, 150, 100, 150, 200, 100, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 2;
                            break;
                        }

                    case "DailyBox":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("DailyBoxID", "رقـم الـيـومـيـة");
                                cls.SQLStr = "SELECT * FROM    Acc_DailyBoxArb_Find WHERE BranchID = " + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("DailyBoxID", "Daily ID");
                                cls.SQLStr = "SELECT *  FROM   Acc_DailyBoxEng_Find WHERE BranchID = " + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 0, 80, 75, 80, 200, 85, 150, 100, 100, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 2;
                            break;
                        }

                    case "Items":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("ItemID", "رقـم الـمــادة");
                                cls.SQLStr = "SELECT  BarCode AS [البـاركـود], ItemID AS [رقـم الـمــادة],  ArbItemName AS [اسـم الـمــادة] , ArbSizeName AS [اسـم الـوحــده], ArbItemType AS [نـوع الـمــادة], ArbItemGroup AS [المجـمــوعة] FROM Stc_Items_Find WHERE BranchID = " + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("[Item ID]", "Item ID");
                                cls.SQLStr = "SELECT  BarCode AS [Bar Code], ItemID AS [Item ID] ,  EngItemName AS [Item Name], EngSizeName AS [Unit Name] , EngItemType AS [Item Type], EngItemGroup AS [Item Group] FROM Stc_Items_Find WHERE BranchID = " + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 80, 100, 100, 200, 120, 120, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 3;
                            break;
                        }

                    case "ItemBalanceFind":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("BarCode", "البـاركـود");
                                cls.SQLStr = "SELECT BarCode AS [البـاركـود],ItemID AS [رقـم الـمــادة] , ArbSizeName AS [اسـم الـوحــده], ArbItemName AS [اسـم الـمــادة], ArbItemType AS [نـوع الـمــادة], ArbItemGroup AS [المجـمــوعة] FROM Stc_Items_Find WHERE BranchID = " + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("[BarCode]", "BarCode");
                                cls.SQLStr = "SELECT BarCode ,ItemID AS [Item ID] , EngSizeName AS [Unit Name] , EngItemName AS [Item Name], EngItemType AS [Item Type], EngItemGroup AS [Item Group] FROM    Stc_Items_Find WHERE BranchID = " + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 100, 100, 100, 200, 120, 120, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 3;
                            break;
                        }

                    case "ItemBarcode":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("BarCode", "البـاركـود");
                                cls.SQLStr = "SELECT BarCode AS [البـاركـود],ItemID AS [رقـم الـمــادة] , ArbSizeName AS [اسـم الـوحــده], ArbItemName AS [اسـم الـمــادة],TypeID as [رقم النوع], ArbItemType AS [نـوع الـمــادة], ArbItemGroup AS [المجـمــوعة],CostPrice as [سعر التكلفة] FROM Stc_Items_Find WHERE BranchID = " + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("[BarCode]", "BarCode");
                                cls.SQLStr = "SELECT BarCode ,ItemID AS [Item ID] , EngSizeName AS [Unit Name] , EngItemName AS [Item Name], EngItemType AS [Item Type],TypeID as [Type ID], EngItemGroup AS [Item Group],CostPrice as [Cost Price] FROM    Stc_Items_Find WHERE BranchID = " + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 100, 100, 100, 200, 120, 120, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 3;
                            break;
                        }
                    case "BarCodeDimond":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("BarCodeDimond", "البـاركـود");
                                cls.SQLStr = "SELECT BarCodeDimond as [البـاركـود],ItemID AS [رقـم الصنف], ArbName as [اسم الصنف]   FROM  Stc_DiamondItemsType WHERE BranchID = " + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("[BarCodeDimond]", "BarCode");
                                cls.SQLStr = "SELECT  BarCodeDimond as [BarCode],ItemID AS [Item Id], ArbName as [Item Name] from  Stc_DiamondItemsType WHERE BranchID = " + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 100, 100, 100, 200, 120, 120, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 3;
                            break;
                        }

                    case "ItemByType1":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("BarCode", "البـاركـود");
                                cls.SQLStr = "SELECT BarCode AS [البـاركـود],ItemID AS [رقـم الـمــادة] , ArbSizeName AS [اسـم الـوحــده], ArbItemName AS [اسـم الـمــادة], ArbItemType AS [نـوع الـمــادة], ArbItemGroup AS [المجـمــوعة] FROM Stc_Items_Find where TypeID=1 and BranchID = " + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("[BarCode]", "BarCode");
                                cls.SQLStr = "SELECT BarCode ,ItemID AS [Item ID] , EngSizeName AS [Unit Name] , EngItemName AS [Item Name], EngItemType AS [Item Type], EngItemGroup AS [Item Group] FROM    Stc_Items_Find WHERE BranchID = " + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 100, 100, 100, 200, 120, 120, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 3;
                            break;
                        }


                    case "SizeID":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("SizeID", "رقـم الـوحـــده");
                                cls.SQLStr = "SELECT SizeID as [رقـم الـوحـــده], ArbName as [اسـم الـوحـــده],  Notes as [مـلاحـظــــــــات]  FROM Stc_SizingUnits WHERE BranchID = " + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("SizeID", "Size ID");
                                cls.SQLStr = "SELECT SizeID as [Size ID], EngName as [Size Name],  Notes   FROM Stc_SizingUnits WHERE BranchID = " + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 100, 250, 250, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 1;
                            break;
                        }
                    case "ItemBySize":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("SizeID", "رقـم الـوحـــده");
                                cls.SQLStr = "SELECT SizeID as [رقـم الـوحـــده], ArbName as [اسـم الـوحـــده] from Stc_ItemSizingUnitsArb_Find " + Condition;
                            }
                            else
                            {
                                cls.AddField("SizeID", "Size ID");
                                cls.SQLStr = "SELECT SizeID as [Size ID], EngName as  [Size Name] from Sales_ItemIDSizeIDForPurchaseInvoice_Find " + Condition;

                            }
                            ColumnWidth = new int[] { 100, 250, 250, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 1;
                            break;
                        }
                    case "GroupID":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("GroupID", "رقـم المجـمـوعة");
                                cls.SQLStr = "SELECT GroupID as [رقـم المجـمـوعة], ArbName as [اسـم المجـمـوعة],  Notes as [مـلاحـظــــــــات]  FROM Stc_ItemsGroups Where Cancel=0   and BranchID=" + MySession.GlobalBranchID + " and AccountTypeID= " + 1;
                            }
                            else
                            {
                                cls.AddField("GroupID", "Group ID");
                                cls.SQLStr = "SELECT GroupID as [Group ID], EngName as [Group Name],  Notes   FROM Stc_ItemsGroups Where Cancel=0  and BranchID=" + MySession.GlobalBranchID + " and AccountTypeID= " + 1;
                            }
                            ColumnWidth = new int[] { 100, 250, 250, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 1;
                            break;
                        }
                    //case "TypeIDMenuFactring":
                    //    {
                    //        if (Language == iLanguage.Arabic)
                    //        {
                    //            cls.AddField("GroupID", "رقـم النوع");
                    //            cls.SQLStr = "SELECT GroupID as [رقـم النوع], ArbName as [اسـم النوع],  Notes as [مـلاحـظــــــــات]  FROM Stc_ItemsGroups Where Cancel=0 ";
                    //        }
                    //        else
                    //        {
                    //            cls.AddField("GroupID", "Type ID");
                    //            cls.SQLStr = "SELECT GroupID as [Group ID], EngName as [Type Name],  Notes   FROM Stc_ItemsGroups Where Cancel=0 ";
                    //        }
                    //        ColumnWidth = new int[] { 100, 250, 250, 100, 100, 100, 100, 100, 100 };
                    //        cls.SearchCol = 1;
                    //        break;
                    //    }

                    case "BrandID":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("BrandID", "رقـم المودل");
                                cls.SQLStr = "SELECT BrandID as [رقـم المودل], ArbName as [اسـم المودل] FROM Stc_ItemsBrands Where Cancel=0 and BranchID = " + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("BrandID", "Brand ID");
                                cls.SQLStr = "SELECT BrandID as [Brand ID], EngName as [Brand Name] FROM Stc_ItemsBrands Where Cancel=0 and BranchID = " + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 100, 250, 250, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 1;
                            break;
                        }

                    case "BaseID":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("BaseID", "التصـــنيف");
                                cls.SQLStr = "SELECT BaseID as [التصـــنيف], ArbName as [اسـم التصنيف] FROM Stc_ItemsBases";
                            }
                            else
                            {
                                cls.AddField("BaseID", "Base ID");
                                cls.SQLStr = "SELECT BaseID as [Base ID], EngName as [Base Name] FROM Stc_ItemsBases ";
                            }
                            ColumnWidth = new int[] { 100, 250, 250, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 1;
                            break;
                        }

                    case "ColorID":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("ColorID", "رقـم اللون");
                                cls.SQLStr = "SELECT ColorID as [رقـم اللون], ArbName as [اسـم اللون] FROM Stc_ItemsColors WHERE BranchID = " + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("ColorID", "Color ID");
                                cls.SQLStr = "SELECT ColorID as [Color ID], EngName as [Color Name] FROM Stc_ItemsColors WHERE BranchID = " + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 100, 250, 250, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 1;
                            break;
                        }

                    case "BoutiqueSizes":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("SizeID", "رقـم القياس");
                                cls.SQLStr = "SELECT SizeID as [رقـم القياس], ArbName as [اسـم القياس] FROM Stc_ItemsSizes";
                            }
                            else
                            {
                                cls.AddField("SizeID", "Size ID");
                                cls.SQLStr = "SELECT SizeID as [Size ID], EngName as [Size Name] FROM Stc_ItemsSizes ";
                            }
                            ColumnWidth = new int[] { 100, 250, 250, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 1;
                            break;
                        }

                    case "TypeID":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("TypeID", "الــــنـــــــــــوع");
                                cls.SQLStr = "SELECT TypeID as الــــنـــــــــــوع, ArbName as [اسـم النوع]  FROM Stc_ItemTypes where Cancel =0   and BranchID = " + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("TypeID", "Type ID");
                                cls.SQLStr = "SELECT TypeID as [Type ID], EngName as [Type Name]   FROM Stc_ItemTypes where Cancel =0 and BranchID = " + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 150, 450, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 1;
                            break;
                        }
                   

                    case "SizingUnits":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("TypeID", "الــــنـــــــــــوع");
                                cls.SQLStr = "SELECT TypeID as الــــنـــــــــــوع, ArbName as [اسـم المجــوعة]  FROM Stc_ItemTypes where BranchID = " + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("TypeID", "Size ID");
                                cls.SQLStr = "SELECT TypeID as [Group ID], EngName as [GroupID Name]   FROM Stc_ItemTypes WHERE BranchID = " + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 150, 450, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 1;
                            break;
                        }

                    case "SupplierID":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("SupplierID", "رقم الـمــــــــــورد");
                                cls.SQLStr = "SELECT SupplierID as  [رقم الـمــــــــــورد],ArbName as [اسـم الـمــــــــــورد]  FROM Sales_Suppliers Where  Cancel =0   And BranchID =" + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("SupplierID", "Supplier ID");
                                cls.SQLStr = "SELECT SupplierID as  [Supplier ID],EngName as [Supplier Name]  FROM Sales_Suppliers Where  Cancel =0   And BranchID =" + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 150, 450, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 1;
                            break;
                        }

                    case "PurchaseDelegateID":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("DelegateID", "رقم المـندوب");
                                cls.SQLStr = "SELECT DelegateID as  [رقم المـندوب],ArbName as [اسـم المـندوب]  FROM Sales_PurchasesDelegate Where  Cancel =0   And BranchID =" + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("DelegateID", "Delegate ID");
                                cls.SQLStr = "SELECT DelegateID as  [Delegate ID],EngName as [Delegate Name]  FROM Sales_PurchasesDelegate Where  Cancel =0   And BranchID =" + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 150, 450, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 1;
                            break;
                        }

                    case "StoreID":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("AccountID", "رقم الحساب");
                                cls.SQLStr = "SELECT StoreID as  [رقم الـمـســتـودع],AccountID as [رقم الحساب],ArbName as [اسـم الـمـســتـودع]  FROM Stc_Stores Where  Cancel =0   And BranchID =" + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("AccountID", "Account ID");
                                cls.SQLStr = "SELECT StoreID as  [Store ID],AccountID as [Account ID],EngName as [Store Name]  FROM Stc_Stores Where  Cancel =0   And BranchID =" + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 80, 150, 150, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 1;
                            break;
                        }

                    case "BranchID":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("BranchID", "رقم الفـــــرع");
                                cls.SQLStr = "SELECT BranchID as  [رقم الفـــــرع],ArbName as [اسـم الفـــــرع]  FROM Branches Where  Cancel =0  ";
                            }
                            else
                            {
                                cls.AddField("BranchID", "Branch ID");
                                cls.SQLStr = "SELECT StoreID as  [Branch ID],EngName as [Branch Name]  FROM Branches Where  Cancel =0   ";
                            }
                            ColumnWidth = new int[] { 150, 450, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 1;
                            break;
                        }
                    case "ItemsDismantling":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("DismantleID", "رقـم العملية");
                                cls.SQLStr = @" SELECT        dbo.Stc_ItemsDismantlingMaster.DismantleID AS [رقـم العملية], dbo.Stc_ItemsDismantlingMaster.DismantleDate AS [تاريخ العملية], dbo.Acc_CostCenters.ArbName AS [مركز التكلفة], dbo.Stc_Stores.ArbName AS المستودع, 
                                                dbo.Stc_ItemsDismantlingMaster.Notes AS ملاحظة
                                                FROM            dbo.Stc_ItemsDismantlingMaster INNER JOIN
                                                dbo.Stc_Stores ON dbo.Stc_ItemsDismantlingMaster.StoreID = dbo.Stc_Stores.StoreID AND dbo.Stc_ItemsDismantlingMaster.BranchID = dbo.Stc_Stores.BranchID AND 
                                                dbo.Stc_ItemsDismantlingMaster.CostCenterID = dbo.Stc_Stores.FacilityID INNER JOIN
                                                dbo.Acc_CostCenters ON dbo.Stc_ItemsDismantlingMaster.CostCenterID = dbo.Acc_CostCenters.CostCenterID AND dbo.Stc_ItemsDismantlingMaster.BranchID = dbo.Acc_CostCenters.BranchID Where  dbo.Stc_ItemsDismantlingMaster.Cancel =0   And dbo.Stc_ItemsDismantlingMaster.BranchID =" + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("IDismantleID", "Dismantle ID");
                                cls.SQLStr = @"SELECT   dbo.Stc_ItemsDismantlingMaster.DismantleID AS [Dismantle ID], dbo.Stc_ItemsDismantlingMaster.DismantleDate AS [Dismantle Date], dbo.Stc_Stores.EngName AS [Store Name], 
                                               dbo.Acc_CostCenters.EngName AS [Cost Center Name], dbo.Stc_ItemsDismantlingMaster.Notes AS Notes
                                              FROM                                           dbo.Stc_ItemsDismantlingMaster INNER JOIN
                                               dbo.Stc_Stores ON dbo.Stc_ItemsDismantlingMaster.StoreID = dbo.Stc_Stores.StoreID AND dbo.Stc_ItemsDismantlingMaster.BranchID = dbo.Stc_Stores.BranchID AND 
                                               dbo.Stc_ItemsDismantlingMaster.CostCenterID = dbo.Stc_Stores.FacilityID INNER JOIN
                                               dbo.Acc_CostCenters ON dbo.Stc_ItemsDismantlingMaster.CostCenterID = dbo.Acc_CostCenters.CostCenterID AND dbo.Stc_ItemsDismantlingMaster.BranchID = dbo.Acc_CostCenters.BranchID Where  dbo.Stc_ItemsDismantlingMaster.Cancel =0   And dbo.Stc_ItemsDismantlingMaster.BranchID =" + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 80, 100, 200, 200, 200, 150, 85, 150, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 1;
                            break;
                        }
                    case "PurchaseItemID":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("ItemID", "رقم المادة");
                                cls.SQLStr = "SELECT ItemID AS [رقم المادة], ArbName AS [اسـم الـمـادة]"
                                    + " FROM dbo.Stc_Items"
                                    + " WHERE Cancel = 0 AND ItemID < 100000 And TypeID<>3 ";
                            }
                            else
                            {
                                cls.AddField("ItemID", "Item ID");
                                cls.SQLStr = "SELECT BranchID, ItemID AS [Item ID], EngName AS [Item Name]"
                                    + " FROM dbo.Stc_Items"
                                    + " WHERE Cancel = 0 AND ItemID < 100000 And TypeID<>3 ";
                            }
                            ColumnWidth = new int[] { 122, 450, 150, 80, 100, 150, 85, 150, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 1;
                            break;
                        }

                    case "ItemID":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("ItemID", "رقم المادة");
                                cls.SQLStr = "SELECT ItemID AS [رقم المادة], ArbItemName AS [اسـم الـمـادة],BarCode as [الباركود],ArbSizeName as [الوحدة],CostPrice [سعر التكلفة]"
                                    + " FROM dbo.Stc_Items_Find WHERE BranchID = " + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("ItemID", "Item ID");
                                cls.SQLStr = "SELECT   ItemID AS [Item ID], EngItemName AS [Item Name],BarCode as [BarCode],EngSizeName as [Size Name],CostPrice [Cost Price]"
                                    + " FROM dbo.Stc_Items_Find WHERE BranchID = " + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 122, 450, 150, 80, 100, 150, 85, 150, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 1;
                            break;
                        }

                    case "SupplierInvoiceID":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("SupplierInvoiceID", "رقـم فـاتـورة المورد");
                                cls.SQLStr = SupplierInvoiceSqlStr(Language, 1, GlobalBranchID);
                            }
                            else
                            {
                                cls.AddField("SupplierInvoiceID", "Supplier Invoice ID");
                                cls.SQLStr = SupplierInvoiceSqlStr(Language, 1, GlobalBranchID);
                            }
                            ColumnWidth = new int[] { 100, 120, 250, 120, 100, 100, 100, 100 };
                            cls.SearchCol = 1;
                            break;
                        }

                    case "BarCodeForPurchaseReturn":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("BarCode", "البـاركـود");
                                cls.SQLStr = "SELECT *  FROM Sales_BarCodeForPurchaseReturnArb_Find WHERE BranchID = " + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("BarCode", "BarCode");
                                cls.SQLStr = "SELECT *  FROM Sales_BarCodeForPurchaseReturnEng_Find WHERE BranchID = " + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 0, 0, 0, 0, 100, 70, 200, 70, 100, 100, 150, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 6;
                            break;
                        }

                    case "PurchaseInvoicesReturn":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("InvoiceID", "رقـم الـفـاتـورة");
                                cls.SQLStr = "SELECT *  FROM Sales_PurchaseInvoicesReturnArb WHERE BranchID = " + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("InvoiceID", "Invoice ID");
                                cls.SQLStr = "SELECT *  FROM Sales_PurchaseInvoicesReturnEng WHERE BranchID = " + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 0, 80, 80, 90, 120, 120, 120, 150, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 2;
                            break;
                        }

                    case "SellerID":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("SellerID", "رقم البائع");
                                cls.SQLStr = "SELECT SellerID AS [رقم البائع], ArbName  AS [اسـم البائع], Mobile  AS [جوال], Percentage  AS [النسبة], "
                                + " Target  AS [التارجيت], Notes AS [مـلاحـظــــــــات] FROM Sales_Sellers "
                                + " WHERE  Cancel = 0 AND  BranchID =" + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("SellerID", "Seller ID");
                                cls.SQLStr = "SELECT SellerID AS [Seller ID], EngName  AS [Seller Name], Mobile  , Percentage , "
                                + " Target , Notes FROM Sales_Sellers "
                                + " WHERE  Cancel = 0 AND  BranchID =" + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 80, 200, 100, 100, 100, 250, 100, 100 };
                            cls.SearchCol = 1;
                            break;
                        }

                    case "BarCodeForPurchaseInvoice":
                        {
                            if (MySession.GlobalUsingExpiryDate)
                            {
                                if (Language == iLanguage.Arabic)
                                {
                                    cls.AddField("BarCode", "البـاركـود");
                                    cls.SQLStr = "SELECT *  FROM Sales_BarCodeForPurchaseInvoiceArb_Find where BranchID= " + GlobalBranchID;
                                }
                                else
                                {
                                    cls.AddField("BarCode", "BarCode");
                                    cls.SQLStr = @"SELECT DISTINCT 
                                                 TOP (100) PERCENT dbo.Sales_PurchaseInvoiceDetails.BarCode AS BarCode, dbo.Sales_PurchaseInvoiceDetails.ItemID AS [Item ID], dbo.Stc_Items.EngName AS [Item Name], 
                                                 dbo.Sales_PurchaseInvoiceDetails.SizeID AS [Size ID], dbo.Stc_SizingUnits.EngName AS [Size Name], CASE WHEN dbo.Sales_PurchaseInvoiceDetails.ExpiryDate = 0 THEN '0' ELSE SUBSTRING(ltrim(str(ExpiryDate)), 1, 4) 
                                                 + '/' + SUBSTRING(ltrim(str(ExpiryDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(ExpiryDate)), 7, 2) END AS [Expire Date], dbo.Stc_SizingUnits.BranchID, dbo.Sales_PurchaseInvoiceDetails.SalePrice, dbo.Stc_Items.IsVAT, 
                                                                         dbo.Stc_Items.IsService
                                                FROM            dbo.Stc_Items RIGHT OUTER JOIN
                                                                         dbo.Sales_PurchaseInvoiceDetails LEFT OUTER JOIN
                                                                         dbo.Sales_PurchaseInvoiceMaster ON dbo.Sales_PurchaseInvoiceDetails.BranchID = dbo.Sales_PurchaseInvoiceMaster.BranchID AND 
                                                                         dbo.Sales_PurchaseInvoiceDetails.InvoiceID = dbo.Sales_PurchaseInvoiceMaster.InvoiceID LEFT OUTER JOIN
                                                                         dbo.Stc_SizingUnits ON dbo.Sales_PurchaseInvoiceDetails.SizeID = dbo.Stc_SizingUnits.SizeID ON dbo.Stc_Items.ItemID = dbo.Sales_PurchaseInvoiceDetails.ItemID
                                                WHERE        (dbo.Stc_Items.Cancel = 0) AND (dbo.Sales_PurchaseInvoiceDetails.Cancel = 0) AND (dbo.Sales_PurchaseInvoiceDetails.InvoiceID <> - 4) AND (dbo.Sales_PurchaseInvoiceDetails.InvoiceID = - 1) WHERE BranchID = " + GlobalBranchID+
                                               " ORDER BY BarCode ";
                                }

                            }


                            else
                            {
                                //if (Language == iLanguage.Arabic)
                                //{
                                //    cls.AddField("BarCode", "البـاركـود");
                                //    cls.SQLStr = "SELECT *  FROM Sales_BarCodeForPurchaseInvoiceNoUsingExpiryDateArb_Find WHERE BranchID = " + GlobalBranchID;
                                //}
                                //else
                                //{
                                //    cls.AddField("BarCode", "BarCode");
                                //    cls.SQLStr = "SELECT *  FROM Sales_BarCodeForPurchaseInvoiceNoUsingExpiryDateEng_Find WHERE BranchID = " + GlobalBranchID;
                                //}

                                if (Language == iLanguage.Arabic)
                                {
                                    cls.AddField("BarCode", "البـاركـود");
                                    cls.SQLStr = "SELECT *  FROM Sales_BarCodeForPurchaseInvoiceArb_Find WHERE BranchID = " + GlobalBranchID;
                                }
                                else
                                {
                                    cls.AddField("BarCode", "BarCode");
                                    cls.SQLStr = @"SELECT DISTINCT 
                                                 TOP (100) PERCENT dbo.Sales_PurchaseInvoiceDetails.BarCode AS BarCode, dbo.Sales_PurchaseInvoiceDetails.ItemID AS [Item ID], dbo.Stc_Items.EngName AS [Item Name], 
                                                 dbo.Sales_PurchaseInvoiceDetails.SizeID AS [Size ID], dbo.Stc_SizingUnits.EngName AS [Size Name], CASE WHEN dbo.Sales_PurchaseInvoiceDetails.ExpiryDate = 0 THEN '0' ELSE SUBSTRING(ltrim(str(ExpiryDate)), 1, 4) 
                                                 + '/' + SUBSTRING(ltrim(str(ExpiryDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(ExpiryDate)), 7, 2) END AS [Expire Date], dbo.Stc_SizingUnits.BranchID, dbo.Sales_PurchaseInvoiceDetails.SalePrice, dbo.Stc_Items.IsVAT, 
                                                                         dbo.Stc_Items.IsService
                                                FROM            dbo.Stc_Items RIGHT OUTER JOIN
                                                                         dbo.Sales_PurchaseInvoiceDetails LEFT OUTER JOIN
                                                                         dbo.Sales_PurchaseInvoiceMaster ON dbo.Sales_PurchaseInvoiceDetails.BranchID = dbo.Sales_PurchaseInvoiceMaster.BranchID AND 
                                                                         dbo.Sales_PurchaseInvoiceDetails.InvoiceID = dbo.Sales_PurchaseInvoiceMaster.InvoiceID LEFT OUTER JOIN
                                                                         dbo.Stc_SizingUnits ON dbo.Sales_PurchaseInvoiceDetails.SizeID = dbo.Stc_SizingUnits.SizeID ON dbo.Stc_Items.ItemID = dbo.Sales_PurchaseInvoiceDetails.ItemID
                                                WHERE        (dbo.Stc_Items.Cancel = 0) AND (dbo.Sales_PurchaseInvoiceDetails.Cancel = 0) AND (dbo.Sales_PurchaseInvoiceDetails.InvoiceID <> - 4) AND (dbo.Sales_PurchaseInvoiceDetails.InvoiceID = - 1) WHERE BranchID = " + GlobalBranchID +
                                                 " ORDER BY BarCode ";
                                }
                            }
                            ColumnWidth = new int[] { 100, 100, 300, 50, 90, 100, 60, 60, 100, 100, 150, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 2;
                            break;
                        }
                    case "BarCodeForPurchaseInvoiceIsService":
                        {
                            if (MySession.GlobalUsingExpiryDate)
                            {
                                if (Language == iLanguage.Arabic)
                                {
                                    cls.AddField("BarCode", "البـاركـود");
                                    cls.SQLStr = "SELECT *  FROM Sales_BarCodeForPurchaseInvoiceArb_Find where IsService="+1;
                                }
                                else
                                {
                                    cls.AddField("BarCode", "BarCode");
                                    cls.SQLStr = "SELECT *  FROM Sales_BarCodeForPurchaseInvoiceEng_Find where IsService=" + 1;
                                }

                            }


                            else
                            {
                                if (Language == iLanguage.Arabic)
                                {
                                    cls.AddField("BarCode", "البـاركـود");
                                    cls.SQLStr = "SELECT *  FROM Sales_BarCodeForPurchaseInvoiceNoUsingExpiryDateArb_Find WHERE BranchID = " + GlobalBranchID;
                                }
                                else
                                {
                                    cls.AddField("BarCode", "BarCode");
                                    cls.SQLStr = "SELECT *  FROM Sales_BarCodeForPurchaseInvoiceNoUsingExpiryDateEng_Find WHERE BranchID = " + GlobalBranchID;
                                }
                            }
                            ColumnWidth = new int[] { 100, 100, 300, 50, 90, 100, 60, 60, 100, 100, 150, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 2;
                            break;
                        }
                    //case "BarCodeDimond":
                    //    {
                    //        if (MySession.GlobalUsingExpiryDate)
                    //        {
                    //            if (Language == iLanguage.Arabic)
                    //            {
                    //                cls.AddField("BarCodeDimond", "البـاركـود");
                    //                cls.SQLStr = "SELECT *  FROM Stc_DiamondItemsType ";
                    //            }
                    //            else
                    //            {
                    //                cls.AddField("BarCodeDimond", "BarCode");
                    //                cls.SQLStr = "SELECT *  FROM Stc_DiamondItemsType ";
                    //            }

                    //        }


                    //        else
                    //        {
                    //            if (Language == iLanguage.Arabic)
                    //            {
                    //                cls.AddField("BarCodeDimond", "البـاركـود");
                    //                cls.SQLStr = "SELECT *  FROM Stc_DiamondItemsType ";
                    //            }
                    //            else
                    //            {
                    //                cls.AddField("BarCodeDimond", "BarCode");
                    //                cls.SQLStr = "SELECT *  FROM Stc_DiamondItemsType ";
                    //            }
                    //        }
                    //        ColumnWidth = new int[] { 100, 100, 300, 50, 90, 100, 60, 60, 100, 100, 150, 100, 100, 100, 100, 100, 100 };
                    //        cls.SearchCol = 2;
                    //        break;
                    //    }

                    case "BarCodeForPurchaseInvoiceByGrop5":
                        {

                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("BarCode", "البـاركـود");
                                cls.SQLStr = "SELECT *  FROM Sales_BarCodeForPurchaseInvoicepygrop5Arb_Find WHERE BranchID = " + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("BarCode", "BarCode");
                                cls.SQLStr = "SELECT *  FROM Sales_BarCodeForPurchaseInvoicepygrop5Arb_Find WHERE BranchID = " + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 100, 100, 300, 50, 90, 100, 60, 60, 100, 100, 150, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 2;
                            break;
                        }


                    case "BarCodeForPurchaseInvoicePaseCar":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("BarCode", "البـاركـود");
                                cls.SQLStr = "SELECT *  FROM Sales_BarCodeForPurchaseInvoiceArb_Find  WHERE BranchID = " + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("BarCode", "BarCode");
                                cls.SQLStr = "SELECT *  FROM Sales_BarCodeForPurchaseInvoiceEng_Find  WHERE BranchID = " + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 0, 100, 300, 70, 90, 100, 100, 70, 100, 100, 150, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 2;
                            break;
                        }

                    case "CustomerID":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("CustomerID", "رقم الــعـــمـــيــل");
                                cls.SQLStr = "SELECT CustomerID as  [رقم الــعـــمـــيــل],ArbName as [اسـم الــعـــمـــيــل] , Mobile FROM Sales_Customers Where  Cancel =0   And BranchID =" + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("CustomerID", "Customer ID");
                                cls.SQLStr = "SELECT CustomerID as  [Customer ID],EngName as [Customer Name] , Mobile  FROM Sales_Customers Where  Cancel =0   And BranchID =" + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 150, 450, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 1;
                            break;
                        }

                    case "CustomerIDAndSublierID":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("AcountID", "رقم الــعـــمـــيــل");
                                cls.SQLStr = "SELECT AcountID as  [رقم الــعـــمـــيــل],ArbName as [اسـم الــعـــمـــيــل]  , Mobile  FROM Sales_CustomerAnSublierListArb where BranchID =" + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("AcountID", "Customer ID");
                                cls.SQLStr = "SELECT AcountID as  [Customer ID],EngName as [Customer Name] , Mobile  FROM Sales_CustomerAnSublierListArb   where BranchID =" + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 150, 450, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 1;
                            break;
                        }
                    case "SublierID":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("AcountID", "رقم المـــورد");
                                cls.SQLStr = "SELECT AccountID as  [رقم المـــورد],ArbName as [اسـم المـــورد]  FROM Sales_Suppliers Where Cancel=0 And BranchID =" + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("AcountID", "Customer ID");
                                cls.SQLStr = "SELECT AccountID as  [Supplier ID],EngName as [Supplier Name]  FROM Sales_Suppliers  Where   Cancel=0 And BranchID =" + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 150, 450, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 1;
                            break;
                        }


                    case "CompanyID":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("CompanyID", "رقم الشركة");
                                cls.SQLStr = "SELECT CompanyID as  [رقم الشركة],ArbName as [اسـم الشركة]  FROM Clinic_InsuranceCompany Where  Cancel =0    And BranchID =" + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("CompanyID", "Company ID");
                                cls.SQLStr = "SELECT CustomerID as  [Company ID],EngName as [Company Name]  FROM Clinic_InsuranceCompany Where  Cancel =0   And BranchID =" + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 150, 450, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 1;
                            break;
                        }

                    case "PurchaseInvoices":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("InvoiceID", "رقـم الـفـاتـورة");
                                cls.SQLStr = "SELECT [Invoice ID] as 'رقـم الـفـاتـورة' ,[Invoice Date] as 'التاريخ',[Purchase Method] as 'طريقة الشراء',[Supplier Name] as 'المورد' ,[Supplier Invoice ID] as 'رقم فاتورة المورد',[Store Name]as 'المستودع',[Cost Center Name]as 'مركز التكلفة',[Purchase Delegate]as 'المندوب'   FROM Sales_PurchaseInvoicesArb WHERE BranchID = " + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("InvoiceID", "Invoice ID");
                                cls.SQLStr = "SELECT *  FROM Sales_PurchaseInvoicesEng WHERE BranchID = " + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 0, 80, 80, 80, 120, 120, 120, 150, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 2;
                            break;
                        }
                    case "GoodsOpening":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("InvoiceID", "رقـم الـفـاتـورة");
                                cls.SQLStr = @"SELECT       dbo.Stc_GoodOpeningMaster.InvoiceID AS [رقـم الـفـاتـورة], CASE WHEN dbo.Stc_GoodOpeningMaster.InvoiceDate = 0 THEN '0' ELSE SUBSTRING(ltrim(str(InvoiceDate)), 1, 
                                                         4) + '/' + SUBSTRING(ltrim(str(InvoiceDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(InvoiceDate)), 7, 2) END AS [التاريخ], 
                                                         dbo.Stc_Stores.ArbName AS [اسم المستودع], dbo.Acc_CostCenters.ArbName AS [مركز التكلفة], dbo.Stc_GoodOpeningMaster.BranchID [رقم الفرع]
                                                  FROM             dbo.Stc_GoodOpeningMaster LEFT OUTER JOIN
                                                         dbo.Stc_Stores ON dbo.Stc_GoodOpeningMaster.BranchID = dbo.Stc_Stores.BranchID AND dbo.Stc_GoodOpeningMaster.StoreID = dbo.Stc_Stores.StoreID LEFT OUTER JOIN
                                                         dbo.Acc_CostCenters ON dbo.Stc_GoodOpeningMaster.BranchID = dbo.Acc_CostCenters.BranchID AND dbo.Stc_GoodOpeningMaster.CostCenterID = dbo.Acc_CostCenters.CostCenterID
                                                  WHERE        (dbo.Stc_GoodOpeningMaster.InvoiceID > 0) AND (dbo.Stc_GoodOpeningMaster.Cancel = 0) And dbo.Stc_GoodOpeningMaster.BranchID = " + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("InvoiceID", "Invoice ID");
                                cls.SQLStr = @"SELECT       dbo.Stc_GoodOpeningMaster.InvoiceID AS [Invoice ID], CASE WHEN dbo.Stc_GoodOpeningMaster.InvoiceDate = 0 THEN '0' ELSE SUBSTRING(ltrim(str(InvoiceDate)), 1, 
                                                         4) + '/' + SUBSTRING(ltrim(str(InvoiceDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(InvoiceDate)), 7, 2) END AS [Invoice Date], 
                                                         dbo.Stc_Stores.ArbName AS [Store Name], dbo.Acc_CostCenters.ArbName AS [Cost Center Name], dbo.Stc_GoodOpeningMaster.BranchID [Branch No]
                                                  FROM             dbo.Stc_GoodOpeningMaster LEFT OUTER JOIN
                                                         dbo.Stc_Stores ON dbo.Stc_GoodOpeningMaster.BranchID = dbo.Stc_Stores.BranchID AND dbo.Stc_GoodOpeningMaster.StoreID = dbo.Stc_Stores.StoreID LEFT OUTER JOIN
                                                         dbo.Acc_CostCenters ON dbo.Stc_GoodOpeningMaster.BranchID = dbo.Acc_CostCenters.BranchID AND dbo.Stc_GoodOpeningMaster.CostCenterID = dbo.Acc_CostCenters.CostCenterID
                                                  WHERE        (dbo.Stc_GoodOpeningMaster.InvoiceID > 0) AND (dbo.Stc_GoodOpeningMaster.Cancel = 0) And dbo.Stc_GoodOpeningMaster.BranchID = " + GlobalBranchID;

                            }
                            ColumnWidth = new int[] { 80, 100, 100, 120, 60 };
                            cls.SearchCol = 2;
                            break;
                        }
                    case "GoldInOnBail":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("InvoiceID", "رقـم الـفـاتـورة");
                                cls.SQLStr = @"SELECT       dbo.Stc_GoldInonBail_Master.InvoiceID AS [رقـم الـفـاتـورة], CASE WHEN dbo.Stc_GoldInonBail_Master.InvoiceDate = 0 THEN '0' ELSE SUBSTRING(ltrim(str(InvoiceDate)), 1, 
                                                         4) + '/' + SUBSTRING(ltrim(str(InvoiceDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(InvoiceDate)), 7, 2) END AS [التاريخ], 
                                                         dbo.Stc_Stores.ArbName AS [اسم المستودع], dbo.Acc_CostCenters.ArbName AS [مركز التكلفة], dbo.Stc_GoldInonBail_Master.BranchID [رقم الفرع]
                                                  FROM             dbo.Stc_GoldInonBail_Master LEFT OUTER JOIN
                                                         dbo.Stc_Stores ON dbo.Stc_GoldInonBail_Master.BranchID = dbo.Stc_Stores.BranchID AND dbo.Stc_GoldInonBail_Master.StoreID = dbo.Stc_Stores.AccountID LEFT OUTER JOIN
                                                         dbo.Acc_CostCenters ON dbo.Stc_GoldInonBail_Master.BranchID = dbo.Acc_CostCenters.BranchID AND dbo.Stc_GoldInonBail_Master.CostCenterID = dbo.Acc_CostCenters.CostCenterID
                                                  WHERE        (dbo.Stc_GoldInonBail_Master.InvoiceID > 0) AND (dbo.Stc_GoldInonBail_Master.Cancel = 0) And dbo.Stc_GoldInonBail_Master.BranchID = " + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("InvoiceID", "Invoice ID");
                                cls.SQLStr = @"SELECT       dbo.Stc_GoldInonBail_Master.InvoiceID AS [Invoice ID], CASE WHEN dbo.Stc_GoldInonBail_Master.InvoiceDate = 0 THEN '0' ELSE SUBSTRING(ltrim(str(InvoiceDate)), 1, 
                                                         4) + '/' + SUBSTRING(ltrim(str(InvoiceDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(InvoiceDate)), 7, 2) END AS [Invoice Date], 
                                                         dbo.Stc_Stores.ArbName AS [Store Name], dbo.Acc_CostCenters.ArbName AS [Cost Center Name], dbo.Stc_GoldInonBail_Master.BranchID [Branch No]
                                                  FROM             dbo.Stc_GoldInonBail_Master LEFT OUTER JOIN
                                                         dbo.Stc_Stores ON dbo.Stc_GoldInonBail_Master.BranchID = dbo.Stc_Stores.BranchID AND dbo.Stc_GoldInonBail_Master.StoreID = dbo.Stc_Stores.AccountID LEFT OUTER JOIN
                                                         dbo.Acc_CostCenters ON dbo.Stc_GoldInonBail_Master.BranchID = dbo.Acc_CostCenters.BranchID AND dbo.Stc_GoldInonBail_Master.CostCenterID = dbo.Acc_CostCenters.CostCenterID
                                                  WHERE        (dbo.Stc_GoldInonBail_Master.InvoiceID > 0) AND (dbo.Stc_GoldInonBail_Master.Cancel = 0) And dbo.Stc_GoldInonBail_Master.BranchID = " + GlobalBranchID;

                            }
                            ColumnWidth = new int[] { 80, 100, 100, 120, 60 };
                            cls.SearchCol = 2;
                            break;
                        }
                    case "MatirialInOnBail":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("InvoiceID", "رقـم الـفـاتـورة");
                                cls.SQLStr = @"SELECT       dbo.Stc_MatirialInonBail_Master.InvoiceID AS [رقـم الـفـاتـورة], CASE WHEN dbo.Stc_MatirialInonBail_Master.InvoiceDate = 0 THEN '0' ELSE SUBSTRING(ltrim(str(InvoiceDate)), 1, 
                                                         4) + '/' + SUBSTRING(ltrim(str(InvoiceDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(InvoiceDate)), 7, 2) END AS [التاريخ], 
                                                         dbo.Stc_Stores.ArbName AS [اسم المستودع], dbo.Acc_CostCenters.ArbName AS [مركز التكلفة], dbo.Stc_MatirialInonBail_Master.BranchID [رقم الفرع]
                                                  FROM   dbo.Stc_MatirialInonBail_Master LEFT OUTER JOIN
                                                         dbo.Stc_Stores ON dbo.Stc_MatirialInonBail_Master.BranchID = dbo.Stc_Stores.BranchID AND dbo.Stc_MatirialInonBail_Master.StoreID = dbo.Stc_Stores.AccountID LEFT OUTER JOIN
                                                         dbo.Acc_CostCenters ON dbo.Stc_MatirialInonBail_Master.BranchID = dbo.Acc_CostCenters.BranchID AND dbo.Stc_MatirialInonBail_Master.CostCenterID = dbo.Acc_CostCenters.CostCenterID
                                                  WHERE  (dbo.Stc_MatirialInonBail_Master.InvoiceID > 0) AND (dbo.Stc_MatirialInonBail_Master.Cancel = 0) And dbo.Stc_MatirialInonBail_Master.BranchID = " + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("InvoiceID", "Invoice ID");
                                cls.SQLStr = @"SELECT       dbo.Stc_MatirialInonBail_Master.InvoiceID AS [Invoice ID], CASE WHEN dbo.Stc_MatirialInonBail_Master.InvoiceDate = 0 THEN '0' ELSE SUBSTRING(ltrim(str(InvoiceDate)), 1, 
                                                         4) + '/' + SUBSTRING(ltrim(str(InvoiceDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(InvoiceDate)), 7, 2) END AS [Invoice Date], 
                                                         dbo.Stc_Stores.ArbName AS [Store Name], dbo.Acc_CostCenters.ArbName AS [Cost Center Name], dbo.Stc_MatirialInonBail_Master.BranchID [Branch No]
                                                  FROM   dbo.Stc_MatirialInonBail_Master LEFT OUTER JOIN
                                                         dbo.Stc_Stores ON dbo.Stc_MatirialInonBail_Master.BranchID = dbo.Stc_Stores.BranchID AND dbo.Stc_MatirialInonBail_Master.StoreID = dbo.Stc_Stores.AccountID LEFT OUTER JOIN
                                                         dbo.Acc_CostCenters ON dbo.Stc_MatirialInonBail_Master.BranchID = dbo.Acc_CostCenters.BranchID AND dbo.Stc_MatirialInonBail_Master.CostCenterID = dbo.Acc_CostCenters.CostCenterID
                                                  WHERE  (dbo.Stc_MatirialInonBail_Master.InvoiceID > 0) AND (dbo.Stc_MatirialInonBail_Master.Cancel = 0) And dbo.Stc_MatirialInonBail_Master.BranchID = " + GlobalBranchID;

                            }
                            ColumnWidth = new int[] { 80, 100, 100, 120, 60 };
                            cls.SearchCol = 2;
                            break;
                        }
                    case "GoldOutOnBail":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("InvoiceID", "رقـم الـفـاتـورة");
                                cls.SQLStr = @"SELECT       dbo.Stc_GoldOutOnBail_Master.InvoiceID AS [رقـم الـفـاتـورة], CASE WHEN dbo.Stc_GoldOutOnBail_Master.InvoiceDate = 0 THEN '0' ELSE SUBSTRING(ltrim(str(InvoiceDate)), 1, 
                                                         4) + '/' + SUBSTRING(ltrim(str(InvoiceDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(InvoiceDate)), 7, 2) END AS [التاريخ], 
                                                         dbo.Stc_Stores.ArbName AS [اسم المستودع], dbo.Acc_CostCenters.ArbName AS [مركز التكلفة], dbo.Stc_GoldOutOnBail_Master.BranchID [رقم الفرع]
                                                  FROM             dbo.Stc_GoldOutOnBail_Master LEFT OUTER JOIN
                                                         dbo.Stc_Stores ON dbo.Stc_GoldOutOnBail_Master.BranchID = dbo.Stc_Stores.BranchID AND dbo.Stc_GoldOutOnBail_Master.StoreID = dbo.Stc_Stores.StoreID LEFT OUTER JOIN
                                                         dbo.Acc_CostCenters ON dbo.Stc_GoldOutOnBail_Master.BranchID = dbo.Acc_CostCenters.BranchID AND dbo.Stc_GoldOutOnBail_Master.CostCenterID = dbo.Acc_CostCenters.CostCenterID
                                                  WHERE        (dbo.Stc_GoldOutOnBail_Master.InvoiceID > 0) AND (dbo.Stc_GoldOutOnBail_Master.Cancel = 0) And dbo.Stc_GoldOutOnBail_Master.BranchID = " + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("InvoiceID", "Invoice ID");
                                cls.SQLStr = @"SELECT       dbo.Stc_GoldOutOnBail_Master.InvoiceID AS [Invoice ID], CASE WHEN dbo.Stc_GoldOutOnBail_Master.InvoiceDate = 0 THEN '0' ELSE SUBSTRING(ltrim(str(InvoiceDate)), 1, 
                                                         4) + '/' + SUBSTRING(ltrim(str(InvoiceDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(InvoiceDate)), 7, 2) END AS [Invoice Date], 
                                                         dbo.Stc_Stores.ArbName AS [Store Name], dbo.Acc_CostCenters.ArbName AS [Cost Center Name], dbo.Stc_GoldOutOnBail_Master.BranchID [Branch No]
                                                  FROM             dbo.Stc_GoldOutOnBail_Master LEFT OUTER JOIN
                                                         dbo.Stc_Stores ON dbo.Stc_GoldOutOnBail_Master.BranchID = dbo.Stc_Stores.BranchID AND dbo.Stc_GoldOutOnBail_Master.StoreID = dbo.Stc_Stores.StoreID LEFT OUTER JOIN
                                                         dbo.Acc_CostCenters ON dbo.Stc_GoldOutOnBail_Master.BranchID = dbo.Acc_CostCenters.BranchID AND dbo.Stc_GoldOutOnBail_Master.CostCenterID = dbo.Acc_CostCenters.CostCenterID
                                                  WHERE        (dbo.Stc_GoldOutOnBail_Master.InvoiceID > 0) AND (dbo.Stc_GoldOutOnBail_Master.Cancel = 0) And dbo.Stc_GoldOutOnBail_Master.BranchID = " + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 80, 100, 100, 120, 60 };
                            cls.SearchCol = 2;
                            break;
                        }
                    case "MatirialOutOnBail":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("InvoiceID", "رقـم الـفـاتـورة");
                                cls.SQLStr = @"SELECT       dbo.Stc_MatirialOutOnBail_Master.InvoiceID AS [رقـم الـفـاتـورة], CASE WHEN dbo.Stc_MatirialOutOnBail_Master.InvoiceDate = 0 THEN '0' ELSE SUBSTRING(ltrim(str(InvoiceDate)), 1, 
                                                         4) + '/' + SUBSTRING(ltrim(str(InvoiceDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(InvoiceDate)), 7, 2) END AS [التاريخ], 
                                                         dbo.Stc_Stores.ArbName AS [اسم المستودع], dbo.Acc_CostCenters.ArbName AS [مركز التكلفة], dbo.Stc_MatirialOutOnBail_Master.BranchID [رقم الفرع]
                                                  FROM   dbo.Stc_MatirialOutOnBail_Master LEFT OUTER JOIN
                                                         dbo.Stc_Stores ON dbo.Stc_MatirialOutOnBail_Master.BranchID = dbo.Stc_Stores.BranchID AND dbo.Stc_MatirialOutOnBail_Master.StoreID = dbo.Stc_Stores.AccountID LEFT OUTER JOIN
                                                         dbo.Acc_CostCenters ON dbo.Stc_MatirialOutOnBail_Master.BranchID = dbo.Acc_CostCenters.BranchID AND dbo.Stc_MatirialOutOnBail_Master.CostCenterID = dbo.Acc_CostCenters.CostCenterID
                                                  WHERE  (dbo.Stc_MatirialOutOnBail_Master.InvoiceID > 0) AND (dbo.Stc_MatirialOutOnBail_Master.Cancel = 0) And dbo.Stc_MatirialOutOnBail_Master.BranchID = " + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("InvoiceID", "Invoice ID");
                                cls.SQLStr = @"SELECT       dbo.Stc_MatirialOutOnBail_Master.InvoiceID AS [Invoice ID], CASE WHEN dbo.Stc_MatirialOutOnBail_Master.InvoiceDate = 0 THEN '0' ELSE SUBSTRING(ltrim(str(InvoiceDate)), 1, 
                                                         4) + '/' + SUBSTRING(ltrim(str(InvoiceDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(InvoiceDate)), 7, 2) END AS [Invoice Date], 
                                                         dbo.Stc_Stores.ArbName AS [Store Name], dbo.Acc_CostCenters.ArbName AS [Cost Center Name], dbo.Stc_MatirialOutOnBail_Master.BranchID [Branch No]
                                                  FROM   dbo.Stc_MatirialOutOnBail_Master LEFT OUTER JOIN
                                                         dbo.Stc_Stores ON dbo.Stc_MatirialOutOnBail_Master.BranchID = dbo.Stc_Stores.BranchID AND dbo.Stc_MatirialOutOnBail_Master.StoreID = dbo.Stc_Stores.AccountID LEFT OUTER JOIN
                                                         dbo.Acc_CostCenters ON dbo.Stc_MatirialOutOnBail_Master.BranchID = dbo.Acc_CostCenters.BranchID AND dbo.Stc_MatirialOutOnBail_Master.CostCenterID = dbo.Acc_CostCenters.CostCenterID
                                                  WHERE  (dbo.Stc_MatirialOutOnBail_Master.InvoiceID > 0) AND (dbo.Stc_MatirialOutOnBail_Master.Cancel = 0) And dbo.Stc_MatirialOutOnBail_Master.BranchID = " + GlobalBranchID;

                            }
                            ColumnWidth = new int[] { 80, 100, 100, 120, 60 };
                            cls.SearchCol = 2;
                            break;
                        }
                    case "GoldTransferMaltipleStore":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("InvoiceID", "رقـم الـفـاتـورة");
                                cls.SQLStr = @"SELECT       dbo.Stc_TransferMultipleStoresGold_Master.InvoiceID AS [رقـم الـفـاتـورة], CASE WHEN dbo.Stc_TransferMultipleStoresGold_Master.InvoiceDate = 0 THEN '0' ELSE SUBSTRING(ltrim(str(InvoiceDate)), 1, 
                                                         4) + '/' + SUBSTRING(ltrim(str(InvoiceDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(InvoiceDate)), 7, 2) END AS [التاريخ], 
                                                         dbo.Stc_Stores.ArbName AS [اسم المستودع], dbo.Acc_CostCenters.ArbName AS [مركز التكلفة], dbo.Stc_TransferMultipleStoresGold_Master.BranchID [رقم الفرع]
                                                  FROM             dbo.Stc_TransferMultipleStoresGold_Master LEFT OUTER JOIN
                                                         dbo.Stc_Stores ON dbo.Stc_TransferMultipleStoresGold_Master.BranchID = dbo.Stc_Stores.BranchID AND dbo.Stc_TransferMultipleStoresGold_Master.StoreID = dbo.Stc_Stores.AccountID LEFT OUTER JOIN
                                                         dbo.Acc_CostCenters ON dbo.Stc_TransferMultipleStoresGold_Master.BranchID = dbo.Acc_CostCenters.BranchID AND dbo.Stc_TransferMultipleStoresGold_Master.CostCenterID = dbo.Acc_CostCenters.CostCenterID
                                                  WHERE        (dbo.Stc_TransferMultipleStoresGold_Master.InvoiceID > 0) AND (dbo.Stc_TransferMultipleStoresGold_Master.Cancel = 0) And dbo.Stc_TransferMultipleStoresGold_Master.BranchID = " + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("InvoiceID", "Invoice ID");
                                cls.SQLStr = @"SELECT       dbo.Stc_TransferMultipleStoresGold_Master.InvoiceID AS [Invoice ID], CASE WHEN dbo.Stc_TransferMultipleStoresGold_Master.InvoiceDate = 0 THEN '0' ELSE SUBSTRING(ltrim(str(InvoiceDate)), 1, 
                                                         4) + '/' + SUBSTRING(ltrim(str(InvoiceDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(InvoiceDate)), 7, 2) END AS [Invoice Date], 
                                                         dbo.Stc_Stores.ArbName AS [Store Name], dbo.Acc_CostCenters.ArbName AS [Cost Center Name], dbo.Stc_TransferMultipleStoresGold_Master.BranchID [Branch No]
                                                  FROM             dbo.Stc_TransferMultipleStoresGold_Master LEFT OUTER JOIN
                                                         dbo.Stc_Stores ON dbo.Stc_TransferMultipleStoresGold_Master.BranchID = dbo.Stc_Stores.BranchID AND dbo.Stc_TransferMultipleStoresGold_Master.StoreID = dbo.Stc_Stores.AccountID LEFT OUTER JOIN
                                                         dbo.Acc_CostCenters ON dbo.Stc_TransferMultipleStoresGold_Master.BranchID = dbo.Acc_CostCenters.BranchID AND dbo.Stc_TransferMultipleStoresGold_Master.CostCenterID = dbo.Acc_CostCenters.CostCenterID
                                                  WHERE        (dbo.Stc_TransferMultipleStoresGold_Master.InvoiceID > 0) AND (dbo.Stc_TransferMultipleStoresGold_Master.Cancel = 0) And dbo.Stc_TransferMultipleStoresGold_Master.BranchID = " + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 80, 100, 100, 120, 60 };
                            cls.SearchCol = 2;
                            break;
                        }
                    case "MatirialTransferMaltipleStore":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("InvoiceID", "رقـم الـفـاتـورة");
                                cls.SQLStr = @"SELECT       dbo.Stc_TransferMultipleStoresMatirial_Master.InvoiceID AS [رقـم الـفـاتـورة], CASE WHEN dbo.Stc_TransferMultipleStoresMatirial_Master.InvoiceDate = 0 THEN '0' ELSE SUBSTRING(ltrim(str(InvoiceDate)), 1, 
                                                         4) + '/' + SUBSTRING(ltrim(str(InvoiceDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(InvoiceDate)), 7, 2) END AS [التاريخ], 
                                                         dbo.Stc_Stores.ArbName AS [اسم المستودع], dbo.Acc_CostCenters.ArbName AS [مركز التكلفة], dbo.Stc_TransferMultipleStoresMatirial_Master.BranchID [رقم الفرع]
                                                  FROM             dbo.Stc_TransferMultipleStoresMatirial_Master LEFT OUTER JOIN
                                                         dbo.Stc_Stores ON dbo.Stc_TransferMultipleStoresMatirial_Master.BranchID = dbo.Stc_Stores.BranchID AND dbo.Stc_TransferMultipleStoresMatirial_Master.StoreID = dbo.Stc_Stores.AccountID LEFT OUTER JOIN
                                                         dbo.Acc_CostCenters ON dbo.Stc_TransferMultipleStoresMatirial_Master.BranchID = dbo.Acc_CostCenters.BranchID AND dbo.Stc_TransferMultipleStoresMatirial_Master.CostCenterID = dbo.Acc_CostCenters.CostCenterID
                                                  WHERE        (dbo.Stc_TransferMultipleStoresMatirial_Master.InvoiceID > 0) AND (dbo.Stc_TransferMultipleStoresMatirial_Master.Cancel = 0) And dbo.Stc_TransferMultipleStoresMatirial_Master.BranchID = " + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("InvoiceID", "Invoice ID");
                                cls.SQLStr = @"SELECT       dbo.Stc_TransferMultipleStoresMatirial_Master.InvoiceID AS [Invoice ID], CASE WHEN dbo.Stc_TransferMultipleStoresMatirial_Master.InvoiceDate = 0 THEN '0' ELSE SUBSTRING(ltrim(str(InvoiceDate)), 1, 
                                                         4) + '/' + SUBSTRING(ltrim(str(InvoiceDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(InvoiceDate)), 7, 2) END AS [Invoice Date], 
                                                         dbo.Stc_Stores.ArbName AS [Store Name], dbo.Acc_CostCenters.ArbName AS [Cost Center Name], dbo.Stc_TransferMultipleStoresMatirial_Master.BranchID [Branch No]
                                                  FROM             dbo.Stc_TransferMultipleStoresMatirial_Master LEFT OUTER JOIN
                                                         dbo.Stc_Stores ON dbo.Stc_TransferMultipleStoresMatirial_Master.BranchID = dbo.Stc_Stores.BranchID AND dbo.Stc_TransferMultipleStoresMatirial_Master.StoreID = dbo.Stc_Stores.AccountID LEFT OUTER JOIN
                                                         dbo.Acc_CostCenters ON dbo.Stc_TransferMultipleStoresMatirial_Master.BranchID = dbo.Acc_CostCenters.BranchID AND dbo.Stc_TransferMultipleStoresMatirial_Master.CostCenterID = dbo.Acc_CostCenters.CostCenterID
                                                  WHERE        (dbo.Stc_TransferMultipleStoresMatirial_Master.InvoiceID > 0) AND (dbo.Stc_TransferMultipleStoresMatirial_Master.Cancel = 0) And dbo.Stc_TransferMultipleStoresMatirial_Master.BranchID = " + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 80, 100, 100, 120, 60 };
                            cls.SearchCol = 2;
                            break;
                        }
                    case "SalesInvoice":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("InvoiceID", "رقـم الـفـاتـورة");
                                cls.SQLStr = "SELECT * FROM    Sales_SalesInvoiceArb WHERE BranchID = " + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("InvoiceID", "Invoice ID");
                                cls.SQLStr = "SELECT *  FROM   Sales_SalesInvoiceEng WHERE BranchID = " + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 0, 80, 100, 80, 150, 150, 150, 150, 100, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 2;
                            break;
                        }
                    case "PurchaseOrder":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("OrderID", "رقم الطلب");
                                cls.SQLStr = "SELECT * FROM    Sales_PurchaseOrderArb WHERE  BranchID = " + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("InvoiceID", "Invoice ID");
                                cls.SQLStr = "SELECT *  FROM   Sales_PurchaseOrderEng WHERE BranchID = " + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 80, 80, 100, 80, 150, 150, 150, 150, 100, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 2;
                            break;
                        }
                    case "SalseOrder":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("OrderID", "رقم الطلب");
                                cls.SQLStr = "SELECT * FROM    Sales_SalseOrderArb WHERE  BranchID = " + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("InvoiceID", "Invoice ID");
                                cls.SQLStr = "SELECT *  FROM   Sales_SalseOrderArbEng WHERE BranchID = " + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 80, 80, 100, 80, 150, 150, 150, 150, 100, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 2;
                            break;
                        }
                    case "OrderIDCadWithCondtion":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("OrderID", "رقم الطلب");
                                cls.SQLStr = "SELECT OrderID as [رقم الطلب],OrderDate as [تاريخ الطلب ] ,CustomerID [رقم العميل],DelegateID as [رقم المندوب], GuidanceID as [بتوجية] FROM    Manu_OrderRestriction WHERE Cancel=0 and TypeAuxiliaryMatirialID=1 and   BranchID = " + GlobalBranchID + "  and OrderID not in(select OrderID from Manu_CadWaxFactoryMaster where Cancel=0 and TypeStageID=1)";
                                cls.SQLStr = " SELECT   dbo.Manu_OrderRestriction.OrderID AS [رقم الطلب], dbo.Manu_OrderRestriction.OrderDate AS [تاريخ الطلب ], dbo.Sales_Customers.ArbName AS [اسم العميل], dbo.Sales_SalesDelegate.ArbName AS [اسم المندوب], " +
                                      " dbo.Users.ArbName AS [بتوجيــة ], dbo.Manu_OrderRestriction.BranchID [رقم الفرع]" +
                                      " FROM  dbo.Manu_OrderRestriction INNER JOIN" +
                                     " dbo.Sales_SalesDelegate ON dbo.Manu_OrderRestriction.BranchID = dbo.Sales_SalesDelegate.BranchID AND dbo.Manu_OrderRestriction.DelegateID = dbo.Sales_SalesDelegate.DelegateID INNER JOIN " +
                                     "  dbo.Users ON dbo.Manu_OrderRestriction.GuidanceID = dbo.Users.UserID and dbo.Manu_OrderRestriction.BranchID = dbo.Users.BranchID  INNER JOIN" +
                                     "  dbo.Sales_Customers ON dbo.Manu_OrderRestriction.CustomerID = dbo.Sales_Customers.AccountID and dbo.Manu_OrderRestriction.BranchID = dbo.Sales_Customers.BranchID" +
                                     "  WHERE        (dbo.Manu_OrderRestriction.Cancel = 0) and TypeAuxiliaryMatirialID=1 AND  dbo.Manu_OrderRestriction.BranchID = " + GlobalBranchID + "  and OrderID not in(select OrderID from Manu_CadWaxFactoryMaster where Cancel=0 and TypeStageID=1)";
                         
                            }
                            else
                            {
                                cls.AddField("OrderID", "Order ID");
                                cls.SQLStr = " SELECT   dbo.Manu_OrderRestriction.OrderID AS [Order ID], dbo.Manu_OrderRestriction.OrderDate AS [Order Date], dbo.Sales_Customers.EngName AS [Customer Name], dbo.Sales_SalesDelegate.EngName AS [Delegete Name], " +
                                       " dbo.Users.EngName AS [Gaidnce ], dbo.Manu_OrderRestriction.BranchID  [Branch ID]" +
                                       " FROM  dbo.Manu_OrderRestriction INNER JOIN" +
                                      " dbo.Sales_SalesDelegate ON dbo.Manu_OrderRestriction.BranchID = dbo.Sales_SalesDelegate.BranchID AND dbo.Manu_OrderRestriction.DelegateID = dbo.Sales_SalesDelegate.DelegateID INNER JOIN " +
                                      "  dbo.Users ON dbo.Manu_OrderRestriction.GuidanceID = dbo.Users.UserID and dbo.Manu_OrderRestriction.BranchID = dbo.Users.BranchID INNER JOIN" +
                                      "  dbo.Sales_Customers ON dbo.Manu_OrderRestriction.CustomerID = dbo.Sales_Customers.AccountID and dbo.Manu_OrderRestriction.BranchID = dbo.Sales_Customers.BranchID" +
                                      "  WHERE        (dbo.Manu_OrderRestriction.Cancel = 0)   and TypeAuxiliaryMatirialID=1 and  dbo.Manu_OrderRestriction.BranchID = " + GlobalBranchID + "  and OrderID not in(select OrderID from Manu_CadWaxFactoryMaster where Cancel=0 and TypeStageID=1)";
                           
                           
                            }
                            ColumnWidth = new int[] { 50, 100, 150, 80, 150, 80};
                            cls.SearchCol = 2;
                            break;
                        }
                    case "OrderIDWaxWithCondtion":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("OrderID", "رقم الطلب");
                                cls.SQLStr = " SELECT   dbo.Manu_OrderRestriction.OrderID AS [رقم الطلب], dbo.Manu_OrderRestriction.OrderDate AS [تاريخ الطلب ], dbo.Sales_Customers.ArbName AS [اسم العميل], dbo.Sales_SalesDelegate.ArbName AS [اسم المندوب], " +
                                 " dbo.Users.ArbName AS [بتوجيــة ], dbo.Manu_OrderRestriction.BranchID [رقم الفرع]" +
                                 " FROM  dbo.Manu_OrderRestriction INNER JOIN" +
                                " dbo.Sales_SalesDelegate ON dbo.Manu_OrderRestriction.BranchID = dbo.Sales_SalesDelegate.BranchID AND dbo.Manu_OrderRestriction.DelegateID = dbo.Sales_SalesDelegate.DelegateID INNER JOIN " +
                                "  dbo.Users ON dbo.Manu_OrderRestriction.GuidanceID = dbo.Users.UserID and  dbo.Manu_OrderRestriction.BranchID = dbo.Users.BranchID INNER JOIN" +
                                "  dbo.Sales_Customers ON dbo.Manu_OrderRestriction.CustomerID = dbo.Sales_Customers.AccountID and dbo.Manu_OrderRestriction.BranchID = dbo.Sales_Customers.BranchID" +
                                "  WHERE        (dbo.Manu_OrderRestriction.Cancel = 0) and TypeAuxiliaryMatirialID=2 AND  dbo.Manu_OrderRestriction.BranchID = " + GlobalBranchID +  "  and OrderID not in(select OrderID from Manu_CadWaxFactoryMaster where Cancel=0 and TypeStageID=2)";
                         
                            }
                            else
                            {
                                cls.AddField("OrderID", "Order ID");
                                cls.SQLStr = " SELECT   dbo.Manu_OrderRestriction.OrderID AS [Order ID], dbo.Manu_OrderRestriction.OrderDate AS [Order Date], dbo.Sales_Customers.EngName AS [Customer Name], dbo.Sales_SalesDelegate.EngName AS [Delegete Name], " +
                                      " dbo.Users.EngName AS [Gaidnce ], dbo.Manu_OrderRestriction.BranchID  [Branch ID]" +
                                      " FROM  dbo.Manu_OrderRestriction INNER JOIN" +
                                     " dbo.Sales_SalesDelegate ON dbo.Manu_OrderRestriction.BranchID = dbo.Sales_SalesDelegate.BranchID AND dbo.Manu_OrderRestriction.DelegateID = dbo.Sales_SalesDelegate.DelegateID INNER JOIN " +
                                     "  dbo.Users ON dbo.Manu_OrderRestriction.GuidanceID = dbo.Users.UserID and dbo.Manu_OrderRestriction.BranchID = dbo.Users.BranchID INNER JOIN" +
                                     "  dbo.Sales_Customers ON dbo.Manu_OrderRestriction.CustomerID = dbo.Sales_Customers.AccountID and dbo.Manu_OrderRestriction.BranchID = dbo.Sales_Customers.BranchID" +
                                     "  WHERE        (dbo.Manu_OrderRestriction.Cancel = 0)   and TypeAuxiliaryMatirialID=2 and  dbo.Manu_OrderRestriction.BranchID = " + GlobalBranchID + " and OrderID not in(select OrderID from Manu_CadWaxFactoryMaster where Cancel=0 and TypeStageID=2)";
                           
                           
                            }
                            ColumnWidth = new int[] { 50, 100, 150, 80, 150, 80 };
                            cls.SearchCol = 2;
                            break;
                        }
                    case "OrderIDCad":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("OrderID", "رقم الطلب");
                                cls.SQLStr = " SELECT   dbo.Manu_OrderRestriction.OrderID AS [رقم الطلب], dbo.Manu_OrderRestriction.OrderDate AS [تاريخ الطلب ], dbo.Sales_Customers.ArbName AS [اسم العميل], dbo.Sales_SalesDelegate.ArbName AS [اسم المندوب], " +
                                 " dbo.Users.ArbName AS [بتوجيــة ], dbo.Manu_OrderRestriction.BranchID [رقم الفرع]" +
                                 " FROM  dbo.Manu_OrderRestriction INNER JOIN" +
                                 " dbo.Sales_SalesDelegate ON dbo.Manu_OrderRestriction.BranchID = dbo.Sales_SalesDelegate.BranchID AND dbo.Manu_OrderRestriction.DelegateID = dbo.Sales_SalesDelegate.DelegateID INNER JOIN " +
                                 "  dbo.Users ON dbo.Manu_OrderRestriction.GuidanceID = dbo.Users.UserID and dbo.Manu_OrderRestriction.BranchID = dbo.Users.BranchID INNER JOIN" +
                                 "  dbo.Sales_Customers ON dbo.Manu_OrderRestriction.CustomerID = dbo.Sales_Customers.AccountID and dbo.Manu_OrderRestriction.BranchID = dbo.Sales_Customers.BranchID" +
                                 "  WHERE        (dbo.Manu_OrderRestriction.Cancel = 0) and TypeAuxiliaryMatirialID=1 AND  dbo.Manu_OrderRestriction.BranchID = " + GlobalBranchID + Condition;                            
                            }
                            else
                            {
                                cls.AddField("OrderID", "Order ID");
                                cls.SQLStr = " SELECT   dbo.Manu_OrderRestriction.OrderID AS [Order ID], dbo.Manu_OrderRestriction.OrderDate AS [Order Date], dbo.Sales_Customers.EngName AS [Customer Name], dbo.Sales_SalesDelegate.EngName AS [Delegete Name], " +
                                    " dbo.Users.EngName AS [Gaidnce ], dbo.Manu_OrderRestriction.BranchID  [Branch ID]" +
                                    " FROM  dbo.Manu_OrderRestriction INNER JOIN" +
                                   " dbo.Sales_SalesDelegate ON dbo.Manu_OrderRestriction.BranchID = dbo.Sales_SalesDelegate.BranchID AND dbo.Manu_OrderRestriction.DelegateID = dbo.Sales_SalesDelegate.DelegateID INNER JOIN " +
                                   "  dbo.Users ON dbo.Manu_OrderRestriction.GuidanceID = dbo.Users.UserID and dbo.Manu_OrderRestriction.BranchID = dbo.Users.BranchID INNER JOIN" +
                                   "  dbo.Sales_Customers ON dbo.Manu_OrderRestriction.CustomerID = dbo.Sales_Customers.AccountID and dbo.Manu_OrderRestriction.BranchID = dbo.Sales_Customers.BranchID" +
                                   "  WHERE  (dbo.Manu_OrderRestriction.Cancel = 0)   and TypeAuxiliaryMatirialID=1 and  dbo.Manu_OrderRestriction.BranchID = " + GlobalBranchID + Condition;

                            }
                            ColumnWidth = new int[] { 50, 100, 150, 80, 150, 80 };
                            cls.SearchCol = 2;
                            break;
                        }
                    case "OrderIDWax":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("OrderID", "رقم الطلب");
                                cls.SQLStr = " SELECT   dbo.Manu_OrderRestriction.OrderID AS [رقم الطلب], dbo.Manu_OrderRestriction.OrderDate AS [تاريخ الطلب ], dbo.Sales_Customers.ArbName AS [اسم العميل], dbo.Sales_SalesDelegate.ArbName AS [اسم المندوب], " +
                                " dbo.Users.ArbName AS [بتوجيــة ], dbo.Manu_OrderRestriction.BranchID [رقم الفرع]" +
                                " FROM  dbo.Manu_OrderRestriction INNER JOIN" +
                               " dbo.Sales_SalesDelegate ON dbo.Manu_OrderRestriction.BranchID = dbo.Sales_SalesDelegate.BranchID AND dbo.Manu_OrderRestriction.DelegateID = dbo.Sales_SalesDelegate.DelegateID INNER JOIN " +
                               "  dbo.Users ON dbo.Manu_OrderRestriction.GuidanceID = dbo.Users.UserID and dbo.Manu_OrderRestriction.BranchID = dbo.Users.BranchID INNER JOIN" +
                               "  dbo.Sales_Customers ON dbo.Manu_OrderRestriction.CustomerID = dbo.Sales_Customers.AccountID and dbo.Manu_OrderRestriction.BranchID = dbo.Sales_Customers.BranchID " +
                               "  WHERE        (dbo.Manu_OrderRestriction.Cancel = 0) and TypeAuxiliaryMatirialID=2 AND  dbo.Manu_OrderRestriction.BranchID = " + GlobalBranchID + Condition;
                            }
                            else
                            {
                                cls.AddField("OrderID", "Order ID");
                                cls.SQLStr = "SELECT  OrderID as [Order ID],OrderDate as [Order Date] ,CustomerID [Customer ID],DelegateID as [Delegate ID], GuidanceID as [Guidance ID]  FROM   Manu_OrderRestriction WHERE Cancel=0 and TypeAuxiliaryMatirialID=2 and BranchID = " + GlobalBranchID;
                                cls.SQLStr = " SELECT   dbo.Manu_OrderRestriction.OrderID AS [Order ID], dbo.Manu_OrderRestriction.OrderDate AS [Order Date], dbo.Sales_Customers.EngName AS [Customer Name], dbo.Sales_SalesDelegate.EngName AS [Delegete Name], " +
                                    " dbo.Users.EngName AS [Gaidnce ], dbo.Manu_OrderRestriction.BranchID  [Branch ID]" +
                                    " FROM  dbo.Manu_OrderRestriction INNER JOIN" +
                                   " dbo.Sales_SalesDelegate ON dbo.Manu_OrderRestriction.BranchID = dbo.Sales_SalesDelegate.BranchID AND dbo.Manu_OrderRestriction.DelegateID = dbo.Sales_SalesDelegate.DelegateID INNER JOIN " +
                                   "  dbo.Users ON dbo.Manu_OrderRestriction.GuidanceID = dbo.Users.UserID and dbo.Manu_OrderRestriction.BranchID = dbo.Users.BranchID INNER JOIN" +
                                   "  dbo.Sales_Customers ON dbo.Manu_OrderRestriction.CustomerID = dbo.Sales_Customers.AccountID and dbo.Manu_OrderRestriction.BranchID = dbo.Sales_Customers.BranchID" +
                                   "  WHERE        (dbo.Manu_OrderRestriction.Cancel = 0)   and TypeAuxiliaryMatirialID=2 and  dbo.Manu_OrderRestriction.BranchID = " + GlobalBranchID + Condition;
                           
                            }
                            ColumnWidth = new int[] { 50, 100, 150, 80, 150, 80 };
                            cls.SearchCol = 2;
                            break;
                        }
                    case "OrderID":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("OrderID", "رقم الطلب");
                                cls.SQLStr = " SELECT   dbo.Manu_OrderRestriction.OrderID AS [رقم الطلب], dbo.Manu_OrderRestriction.OrderDate AS [تاريخ الطلب ], dbo.Sales_Customers.ArbName AS [اسم العميل], dbo.Sales_SalesDelegate.ArbName AS [اسم المندوب], " +
                               " dbo.Users.ArbName AS [بتوجيــة ], dbo.Manu_OrderRestriction.BranchID [رقم الفرع]" +
                               " FROM  dbo.Manu_OrderRestriction INNER JOIN" +
                              " dbo.Sales_SalesDelegate ON dbo.Manu_OrderRestriction.BranchID = dbo.Sales_SalesDelegate.BranchID AND dbo.Manu_OrderRestriction.DelegateID = dbo.Sales_SalesDelegate.DelegateID INNER JOIN " +
                              "  dbo.Users ON dbo.Manu_OrderRestriction.GuidanceID = dbo.Users.UserID and dbo.Manu_OrderRestriction.BranchID = dbo.Users.BranchID INNER JOIN" +
                              "  dbo.Sales_Customers ON dbo.Manu_OrderRestriction.CustomerID = dbo.Sales_Customers.AccountID and  dbo.Manu_OrderRestriction.BranchID = dbo.Sales_Customers.BranchID  " +
                              "  WHERE        (dbo.Manu_OrderRestriction.Cancel = 0) AND  dbo.Manu_OrderRestriction.BranchID = " + GlobalBranchID + Condition;
                            }
                            else
                            {
                                cls.AddField("OrderID", "Order ID");
                                cls.SQLStr = " SELECT   dbo.Manu_OrderRestriction.OrderID AS [Order ID], dbo.Manu_OrderRestriction.OrderDate AS [Order Date], dbo.Sales_Customers.EngName AS [Customer Name], dbo.Sales_SalesDelegate.EngName AS [Delegete Name], " +
                                  " dbo.Users.EngName AS [Gaidnce ], dbo.Manu_OrderRestriction.BranchID  [Branch ID]" +
                                  " FROM  dbo.Manu_OrderRestriction INNER JOIN" +
                                 " dbo.Sales_SalesDelegate ON dbo.Manu_OrderRestriction.BranchID = dbo.Sales_SalesDelegate.BranchID AND dbo.Manu_OrderRestriction.DelegateID = dbo.Sales_SalesDelegate.DelegateID INNER JOIN " +
                                 "  dbo.Users ON dbo.Manu_OrderRestriction.GuidanceID = dbo.Users.UserID INNER JOIN" +
                                 "  dbo.Sales_Customers ON dbo.Manu_OrderRestriction.CustomerID = dbo.Sales_Customers.AccountID" +
                                 "  WHERE        (dbo.Manu_OrderRestriction.Cancel = 0) AND  dbo.Manu_OrderRestriction.BranchID = " + GlobalBranchID + Condition;
                            }
                            ColumnWidth = new int[] { 50, 100, 150, 80, 150, 80 };
                            cls.SearchCol = 2;
                            break;
                        }
                    case "CastingOrderID":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("OrderID", "رقم الطلب");
                                 cls.SQLStr = " SELECT   dbo.Manu_OrderRestriction.OrderID AS [رقم الطلب], dbo.Manu_OrderRestriction.OrderDate AS [تاريخ الطلب ], dbo.Sales_Customers.ArbName AS [اسم العميل], dbo.Sales_SalesDelegate.ArbName AS [اسم المندوب], " +
                                " dbo.Users.ArbName AS [بتوجيــة ], dbo.Manu_OrderRestriction.BranchID [رقم الفرع]" +
                                " FROM  dbo.Manu_OrderRestriction left outer JOIN" +
                               " dbo.Sales_SalesDelegate ON dbo.Manu_OrderRestriction.BranchID = dbo.Sales_SalesDelegate.BranchID AND dbo.Manu_OrderRestriction.DelegateID = dbo.Sales_SalesDelegate.DelegateID INNER JOIN " +
                               "  dbo.Users ON dbo.Manu_OrderRestriction.GuidanceID = dbo.Users.UserID and dbo.Manu_OrderRestriction.BranchID = dbo.Users.BranchID left outer JOIN" +
                               "  dbo.Sales_Customers ON dbo.Manu_OrderRestriction.CustomerID = dbo.Sales_Customers.AccountID and dbo.Manu_OrderRestriction.BranchID = dbo.Sales_Customers.BranchID " +
                               "  WHERE        (dbo.Manu_OrderRestriction.Cancel = 0) AND  dbo.Manu_OrderRestriction.BranchID = " + GlobalBranchID + " and OrderID in (select OrderID from Manu_AfforestationFactoryMaster where Cancel=0 and OrderID not in(select OrderID from Manu_CastingOrders where Cancel=0))";
                           
                            }
                            else
                            {
                                cls.AddField("OrderID", "Order ID");
                                cls.SQLStr = " SELECT   dbo.Manu_OrderRestriction.OrderID AS [Order ID], dbo.Manu_OrderRestriction.OrderDate AS [Order Date], dbo.Sales_Customers.EngName AS [Customer Name], dbo.Sales_SalesDelegate.EngName AS [Delegete Name], " +
                                   " dbo.Users.EngName AS [Gaidnce ], dbo.Manu_OrderRestriction.BranchID  [Branch ID]" +
                                   " FROM  dbo.Manu_OrderRestriction INNER JOIN" +
                                   "  dbo.Sales_SalesDelegate ON dbo.Manu_OrderRestriction.BranchID = dbo.Sales_SalesDelegate.BranchID AND dbo.Manu_OrderRestriction.DelegateID = dbo.Sales_SalesDelegate.DelegateID INNER JOIN " +
                                   "  dbo.Users ON dbo.Manu_OrderRestriction.GuidanceID = dbo.Users.UserID and dbo.Manu_OrderRestriction.BranchID = dbo.Users.BranchID INNER JOIN " +
                                   "  dbo.Sales_Customers ON dbo.Manu_OrderRestriction.CustomerID = dbo.Sales_Customers.AccountID and  dbo.Manu_OrderRestriction.BranchID = dbo.Sales_Customers.BranchID" +
                                   "  WHERE        (dbo.Manu_OrderRestriction.Cancel = 0) AND  dbo.Manu_OrderRestriction.BranchID = " + GlobalBranchID + " and OrderID in (select OrderID from Manu_AfforestationFactoryMaster where Cancel=0 and OrderID not in(select OrderID from Manu_CastingOrders where Cancel=0)) ";
                           
                            }
                            ColumnWidth = new int[] { 50, 100, 150, 80, 150, 80 };
                            cls.SearchCol = 2;
                            break;
                        }
                    case "PurchaseInvoice":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("InvoiceID", "رقـم الـفـاتـورة");
                                cls.SQLStr = "SELECT * FROM    Sales_PurchaseInvoicesArb WHERE BranchID = " + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("InvoiceID", "Invoice ID");
                                cls.SQLStr = "SELECT *  FROM   Sales_PurchaseInvoicesEng WHERE BranchID = " + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 0, 80, 100, 80, 150, 150, 150, 150, 100, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 2;
                            break;
                        }
                    case "BarCodeForSalesInvoiceWithOutServicesAndWeights":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("BarCode", "البـاركـود");
                                cls.SQLStr = "SELECT DISTINCT dbo.Sales_PurchaseInvoiceDetails.BarCode AS البـاركـود, "
                                    + " dbo.Sales_PurchaseInvoiceDetails.ItemID AS [رقم المادة], dbo.Stc_Items.ArbName AS [اسـم الـمـادة], dbo.Sales_PurchaseInvoiceDetails.SizeID AS [رقم الوحدة], "
                                    + " dbo.Stc_SizingUnits.ArbName AS [اسـم الوحدة], CASE WHEN dbo.Sales_PurchaseInvoiceDetails.ExpiryDate = 0 THEN '0' ELSE SUBSTRING(ltrim(str(ExpiryDate)), 1, 4) "
                                    + " + '/' + SUBSTRING(ltrim(str(ExpiryDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(ExpiryDate)), 7, 2) END AS [تاريخ الصلاحية]"
                                    + " FROM dbo.Sales_PurchaseInvoiceDetails LEFT OUTER JOIN"
                                    + " dbo.Stc_SizingUnits ON "
                                    + " dbo.Sales_PurchaseInvoiceDetails.SizeID = dbo.Stc_SizingUnits.SizeID LEFT OUTER JOIN"
                                    + " dbo.Stc_Items ON "
                                    + " dbo.Sales_PurchaseInvoiceDetails.ItemID = dbo.Stc_Items.ItemID"
                                    + " WHERE (dbo.Stc_Items.Cancel = 0) AND (dbo.Sales_PurchaseInvoiceDetails.Cancel = 0)"
                                    + " And Stc_Items.TypeID<>2 And Stc_Items.TypeID<>3 ";
                            }
                            else
                            {
                                cls.AddField("BarCode", "BarCode");
                                cls.SQLStr = "SELECT DISTINCT dbo.Sales_PurchaseInvoiceDetails.BarCode AS BarCode, "
                                    + " dbo.Sales_PurchaseInvoiceDetails.ItemID AS [Item ID], dbo.Stc_Items.EngName AS [Item Name], dbo.Sales_PurchaseInvoiceDetails.SizeID AS [Size ID], "
                                    + " dbo.Stc_SizingUnits.EngName AS [Size Name], CASE WHEN dbo.Sales_PurchaseInvoiceDetails.ExpiryDate = 0 THEN '0' ELSE SUBSTRING(ltrim(str(ExpiryDate)), 1, 4) "
                                    + " + '/' + SUBSTRING(ltrim(str(ExpiryDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(ExpiryDate)), 7, 2) END AS [Expiry Date]"
                                    + " FROM dbo.Sales_PurchaseInvoiceDetails LEFT OUTER JOIN"
                                    + " dbo.Stc_SizingUnits ON "
                                    + " dbo.Sales_PurchaseInvoiceDetails.SizeID = dbo.Stc_SizingUnits.SizeID LEFT OUTER JOIN"
                                    + " dbo.Stc_Items ON "
                                    + " dbo.Sales_PurchaseInvoiceDetails.ItemID = dbo.Stc_Items.ItemID"
                                    + " WHERE (dbo.Stc_Items.Cancel = 0) AND (dbo.Sales_PurchaseInvoiceDetails.Cancel = 0)"
                                    + " And Stc_Items.TypeID<>2 And Stc_Items.TypeID<>3 ";
                            }
                            ColumnWidth = new int[] { 100, 100, 300, 100, 100, 100, 100, 100, 150, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 3;
                            break;
                        }

                    case "BarCodeForSalesInvoiceWithOutServicesAndWeightsAndSpecialOffers":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("BarCode", "البـاركـود");
                                cls.SQLStr = "SELECT DISTINCT dbo.Sales_PurchaseInvoiceDetails.BarCode AS البـاركـود, "
                                    + " dbo.Sales_PurchaseInvoiceDetails.ItemID AS [رقم المادة], dbo.Stc_Items.ArbName AS [اسـم الـمـادة], dbo.Sales_PurchaseInvoiceDetails.SizeID AS [رقم الوحدة], "
                                    + " dbo.Stc_SizingUnits.ArbName AS [اسـم الوحدة], CASE WHEN dbo.Sales_PurchaseInvoiceDetails.ExpiryDate = 0 THEN '0' ELSE SUBSTRING(ltrim(str(ExpiryDate)), 1, 4) "
                                    + " + '/' + SUBSTRING(ltrim(str(ExpiryDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(ExpiryDate)), 7, 2) END AS [تاريخ الصلاحية]"
                                    + " FROM dbo.Sales_PurchaseInvoiceDetails LEFT OUTER JOIN"
                                    + " dbo.Stc_SizingUnits ON "
                                    + " dbo.Sales_PurchaseInvoiceDetails.SizeID = dbo.Stc_SizingUnits.SizeID LEFT OUTER JOIN"
                                    + " dbo.Stc_Items ON "
                                    + " dbo.Sales_PurchaseInvoiceDetails.ItemID = dbo.Stc_Items.ItemID"
                                    + " WHERE (dbo.Stc_Items.Cancel = 0) AND (dbo.Sales_PurchaseInvoiceDetails.Cancel = 0)"
                                    + " And Stc_Items.TypeID <>2 And Stc_Items.TypeID <>3  And dbo.Sales_PurchaseInvoiceDetails.BarCode <> -4 ";
                            }
                            else
                            {
                                cls.AddField("BarCode", "BarCode");
                                cls.SQLStr = "SELECT DISTINCT dbo.Sales_PurchaseInvoiceDetails.BarCode AS BarCode, "
                                    + " dbo.Sales_PurchaseInvoiceDetails.ItemID AS [Item ID], dbo.Stc_Items.EngName AS [Item Name], dbo.Sales_PurchaseInvoiceDetails.SizeID AS [Size ID], "
                                    + " dbo.Stc_SizingUnits.EngName AS [Size Name], CASE WHEN dbo.Sales_PurchaseInvoiceDetails.ExpiryDate = 0 THEN '0' ELSE SUBSTRING(ltrim(str(ExpiryDate)), 1, 4) "
                                    + " + '/' + SUBSTRING(ltrim(str(ExpiryDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(ExpiryDate)), 7, 2) END AS [Expiry Date]"
                                    + " FROM dbo.Sales_PurchaseInvoiceDetails LEFT OUTER JOIN"
                                    + " dbo.Stc_SizingUnits ON "
                                    + " dbo.Sales_PurchaseInvoiceDetails.SizeID = dbo.Stc_SizingUnits.SizeID LEFT OUTER JOIN"
                                    + " dbo.Stc_Items ON "
                                    + " dbo.Sales_PurchaseInvoiceDetails.ItemID = dbo.Stc_Items.ItemID"
                                    + " WHERE (dbo.Stc_Items.Cancel = 0) AND (dbo.Sales_PurchaseInvoiceDetails.Cancel = 0)"
                                    + " And Stc_Items.TypeID <>2 And Stc_Items.TypeID <>3  And dbo.Sales_PurchaseInvoiceDetails.BarCode <> -4 ";
                            }
                            ColumnWidth = new int[] { 100, 100, 100, 200, 100, 100, 100, 100, 150, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 3;
                            break;
                        }

                    case "BarCodeForSalesInvoice":
                        {
                            var DefaulGroupID = 1;
                            if (DefaulGroupID > 0)
                            {
                                if (Language == iLanguage.Arabic)
                                {
                                    cls.AddField("BarCode", "البـاركـود");
                                    cls.SQLStr = "SELECT DISTINCT  dbo.Sales_PurchaseInvoiceDetails.BarCode AS البـاركـود, "
                                        + " dbo.Sales_PurchaseInvoiceDetails.ItemID AS [رقم المادة], dbo.Stc_Items.ArbName AS [اسـم الـمـادة], dbo.Sales_PurchaseInvoiceDetails.SizeID AS [رقم الوحدة], "
                                        + " dbo.Stc_SizingUnits.ArbName AS [اسـم الوحدة], CASE WHEN dbo.Sales_PurchaseInvoiceDetails.ExpiryDate = 0 THEN '0' ELSE SUBSTRING(ltrim(str(ExpiryDate)), 1, 4) "
                                        + " + '/' + SUBSTRING(ltrim(str(ExpiryDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(ExpiryDate)), 7, 2) END AS [تاريخ الصلاحية]"
                                        + " FROM dbo.Sales_PurchaseInvoiceDetails LEFT OUTER JOIN"
                                        + " dbo.Stc_SizingUnits ON "
                                        + " dbo.Sales_PurchaseInvoiceDetails.SizeID = dbo.Stc_SizingUnits.SizeID LEFT OUTER JOIN"
                                        + " dbo.Stc_Items ON "
                                        + " dbo.Sales_PurchaseInvoiceDetails.ItemID = dbo.Stc_Items.ItemID"
                                        + " WHERE (dbo.Stc_Items.Cancel = 0 AND dbo.Stc_Items.GroupID = " + DefaulGroupID + ") AND (dbo.Sales_PurchaseInvoiceDetails.Cancel = 0) "
                                        + " ORDER BY [اسـم الـمـادة]";
                                }
                                else
                                {
                                    cls.AddField("BarCode", "BarCode");
                                    cls.SQLStr = "SELECT DISTINCT dbo.Sales_PurchaseInvoiceDetails.BarCode AS BarCode, dbo.Sales_PurchaseInvoiceDetails.ItemID AS [Item ID], dbo.Stc_Items.EngName AS [Item Name],"
                                    + " dbo.Sales_PurchaseInvoiceDetails.SizeID AS [Size ID], dbo.Stc_SizingUnits.EngName AS [Size Name], CASE WHEN dbo.Sales_PurchaseInvoiceDetails.ExpiryDate = 0 THEN '0' "
                                    + " ELSE SUBSTRING(ltrim(str(ExpiryDate)), 1, 4)  + '/' + SUBSTRING(ltrim(str(ExpiryDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(ExpiryDate)), 7, 2) END AS [Expiry Date]"
                                    + " FROM dbo.Sales_PurchaseInvoiceDetails LEFT OUTER JOIN  dbo.Stc_SizingUnits ON dbo.Sales_PurchaseInvoiceDetails.SizeID = dbo.Stc_SizingUnits.SizeID LEFT OUTER JOIN"
                                    + " dbo.Stc_Items ON dbo.Sales_PurchaseInvoiceDetails.ItemID = dbo.Stc_Items.ItemID WHERE (dbo.Stc_Items.Cancel = 0) AND (dbo.Sales_PurchaseInvoiceDetails.Cancel = 0) ORDER BY [Item Name]";
                                }
                                ColumnWidth = new int[] { 100, 100, 300, 100, 100, 100, 100, 100, 150, 100, 100, 100, 100, 100, 100 };
                                cls.SearchCol = 2;

                                return;
                            }

                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("BarCode", "البـاركـود");
                                cls.SQLStr = "SELECT DISTINCT  dbo.Sales_PurchaseInvoiceDetails.BarCode AS البـاركـود, "
                                    + " dbo.Sales_PurchaseInvoiceDetails.ItemID AS [رقم المادة], dbo.Stc_Items.ArbName AS [اسـم الـمـادة], dbo.Sales_PurchaseInvoiceDetails.SizeID AS [رقم الوحدة], "
                                    + " dbo.Stc_SizingUnits.ArbName AS [اسـم الوحدة], CASE WHEN dbo.Sales_PurchaseInvoiceDetails.ExpiryDate = 0 THEN '0' ELSE SUBSTRING(ltrim(str(ExpiryDate)), 1, 4) "
                                    + " + '/' + SUBSTRING(ltrim(str(ExpiryDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(ExpiryDate)), 7, 2) END AS [تاريخ الصلاحية]"
                                    + " FROM dbo.Sales_PurchaseInvoiceDetails LEFT OUTER JOIN"
                                    + " dbo.Stc_SizingUnits ON "
                                    + " dbo.Sales_PurchaseInvoiceDetails.SizeID = dbo.Stc_SizingUnits.SizeID LEFT OUTER JOIN"
                                    + " dbo.Stc_Items ON "
                                    + " dbo.Sales_PurchaseInvoiceDetails.ItemID = dbo.Stc_Items.ItemID"
                                    + " WHERE (dbo.Stc_Items.Cancel = 0) AND (dbo.Sales_PurchaseInvoiceDetails.Cancel = 0) "
                                    + " ORDER BY [اسـم الـمـادة]";
                            }
                            else
                            {
                                cls.AddField("BarCode", "BarCode");
                                cls.SQLStr = "SELECT DISTINCT dbo.Sales_PurchaseInvoiceDetails.BarCode AS BarCode, dbo.Sales_PurchaseInvoiceDetails.ItemID AS [Item ID], dbo.Stc_Items.EngName AS [Item Name],"
                                + " dbo.Sales_PurchaseInvoiceDetails.SizeID AS [Size ID], dbo.Stc_SizingUnits.EngName AS [Size Name], CASE WHEN dbo.Sales_PurchaseInvoiceDetails.ExpiryDate = 0 THEN '0' "
                                + " ELSE SUBSTRING(ltrim(str(ExpiryDate)), 1, 4)  + '/' + SUBSTRING(ltrim(str(ExpiryDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(ExpiryDate)), 7, 2) END AS [Expiry Date]"
                                + " FROM dbo.Sales_PurchaseInvoiceDetails LEFT OUTER JOIN  dbo.Stc_SizingUnits ON dbo.Sales_PurchaseInvoiceDetails.SizeID = dbo.Stc_SizingUnits.SizeID LEFT OUTER JOIN"
                                + " dbo.Stc_Items ON dbo.Sales_PurchaseInvoiceDetails.ItemID = dbo.Stc_Items.ItemID WHERE (dbo.Stc_Items.Cancel = 0) AND (dbo.Sales_PurchaseInvoiceDetails.Cancel = 0) ORDER BY [Item Name]";
                            }
                            ColumnWidth = new int[] { 100, 100, 300, 100, 100, 100, 100, 100, 150, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 2;
                            break;
                        }

                    case "BarCodeForManufacturingItems":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("BarCode", "البـاركـود");
                                cls.SQLStr = "SELECT DISTINCT dbo.Manu_ItemManufacturing_Master.BarCode As البـاركـود,dbo.Sales_PurchaseInvoiceDetails.ItemID AS [رقم المادة], dbo.Stc_Items.ArbName AS [اسـم الـمـادة], "
                                    + " dbo.Sales_PurchaseInvoiceDetails.SizeID AS [رقم الوحدة], dbo.Stc_SizingUnits.ArbName AS [اسـم الوحدة], "
                                    + " CASE WHEN dbo.Sales_PurchaseInvoiceDetails.ExpiryDate = 0 THEN '0' ELSE SUBSTRING(ltrim(str(ExpiryDate)), 1, 4) + '/' + SUBSTRING(ltrim(str(ExpiryDate)), 5, 2)"
                                    + " + '/' + SUBSTRING(ltrim(str(ExpiryDate)), 7, 2) END AS [تاريخ الصلاحية]"
                                    + " FROM dbo.Sales_PurchaseInvoiceDetails INNER JOIN"
                                    + " dbo.Stc_SizingUnits ON dbo.Sales_PurchaseInvoiceDetails.SizeID = dbo.Stc_SizingUnits.SizeID INNER JOIN"
                                    + " dbo.Stc_Items ON dbo.Sales_PurchaseInvoiceDetails.ItemID = dbo.Stc_Items.ItemID RIGHT OUTER JOIN"
                                    + " dbo.Manu_ItemManufacturing_Master ON dbo.Sales_PurchaseInvoiceDetails.BarCode = dbo.Manu_ItemManufacturing_Master.BarCode"
                                    + " WHERE (dbo.Manu_ItemManufacturing_Master.Cancel = 0)"
                                    + " ORDER BY [اسـم الـمـادة]";
                            }
                            else
                            {
                                cls.AddField("BarCode", "BarCode");
                                cls.SQLStr = "SELECT DISTINCT dbo.Manu_ItemManufacturing_Master.BarCode As BarCode,dbo.Sales_PurchaseInvoiceDetails.ItemID AS [Item ID], dbo.Stc_Items.EngName AS [Item Name], "
                                    + " dbo.Sales_PurchaseInvoiceDetails.SizeID AS [Size ID], dbo.Stc_SizingUnits.EngName AS [Size Name], "
                                    + " CASE WHEN dbo.Sales_PurchaseInvoiceDetails.ExpiryDate = 0 THEN '0' ELSE SUBSTRING(ltrim(str(ExpiryDate)), 1, 4) + '/' + SUBSTRING(ltrim(str(ExpiryDate)), 5, 2)"
                                    + " + '/' + SUBSTRING(ltrim(str(ExpiryDate)), 7, 2) END AS [Expiry Date]"
                                    + " FROM dbo.Sales_PurchaseInvoiceDetails INNER JOIN"
                                    + " dbo.Stc_SizingUnits ON dbo.Sales_PurchaseInvoiceDetails.SizeID = dbo.Stc_SizingUnits.SizeID INNER JOIN"
                                    + " dbo.Stc_Items ON dbo.Sales_PurchaseInvoiceDetails.ItemID = dbo.Stc_Items.ItemID RIGHT OUTER JOIN"
                                    + " dbo.Manu_ItemManufacturing_Master ON dbo.Sales_PurchaseInvoiceDetails.BarCode = dbo.Manu_ItemManufacturing_Master.BarCode"
                                    + " WHERE (dbo.Manu_ItemManufacturing_Master.Cancel = 0)"
                                    + " ORDER BY [Item Name]";
                            }
                            ColumnWidth = new int[] { 100, 100, 300, 100, 100, 100, 100, 100, 150, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 2;
                            break;
                        }

                    case "PriceOffers":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("OfferID", "رقم العرض");
                                cls.SQLStr = "SELECT * FROM Sales_PriceOffersArb WHERE BranchID = " + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("OfferID", "Offer ID");
                                cls.SQLStr = "SELECT *  FROM Sales_PriceOffersEng WHERE BranchID = " + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 0, 100, 100, 100, 150, 150, 150, 100, 150, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 5;
                            break;
                        }

                    case "SaleDelegateID":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("DelegateID", "رقم مندوب المبيعات");
                                cls.SQLStr = "SELECT DelegateID AS [رقم مندوب المبيعات], ArbName  AS [اسـم مندوب المبيعات], Mobile  AS [جوال], Percentage  AS [النسبة], "
                                + " Target  AS [التارجيت], Notes AS [مـلاحـظــــــــات] FROM Sales_SalesDelegate "
                                + " WHERE  Cancel = 0 AND  BranchID =" + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("DelegateID", "Delegate ID");
                                cls.SQLStr = "SELECT DelegateID AS [Delegate ID], EngName  AS [Delegate Name], Mobile  , Percentage , "
                                + " Target , Notes FROM Sales_SalesDelegate "
                                + " WHERE  Cancel = 0 AND  BranchID =" + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 80, 200, 100, 100, 100, 250, 100, 100 };
                            cls.SearchCol = 1;
                            break;
                        }
                    case "ParchaseDelegateID":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("DelegateID", "رقم مندوب المشتريات");
                                cls.SQLStr = "SELECT DelegateID AS [رقم مندوب المشتريات], ArbName  AS [اسـم مندوب المشتريات], Mobile  AS [جوال], Percentage  AS [النسبة], "
                                + " Target  AS [التارجيت], Notes AS [مـلاحـظــــــــات] FROM Sales_PurchasesDelegate "
                                + " WHERE  Cancel = 0 AND  BranchID =" + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("DelegateID", "Delegate ID");
                                cls.SQLStr = "SELECT DelegateID AS [Delegate ID], EngName  AS [Delegate Name], Mobile  , Percentage , "
                                + " Target , Notes FROM Sales_PurchasesDelegate "
                                + " WHERE  Cancel = 0 AND  BranchID =" + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 80, 200, 100, 100, 100, 250, 100, 100 };
                            cls.SearchCol = 1;
                            break;
                        }
                    case "BarCodeForSalesReturn":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("BarCode", "البـاركـود");
                                cls.SQLStr = "SELECT *  FROM Sales_BarCodeForSalesReturnArb_Find where BranchID =" + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("BarCode", "BarCode");
                                cls.SQLStr = "SELECT *  FROM Sales_BarCodeForSalesReturnEng_Find  where BranchID =" + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 0, 0, 0, 0, 0, 100, 70, 200, 70, 90, 80, 180, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 7;
                            break;
                        }

                    case "SalesInvoiceReturn":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("InvoiceID", "رقـم الـفـاتـورة");
                                cls.SQLStr = "SELECT * FROM  Sales_SalesInvoiceReturnArb WHERE BranchID = " + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("InvoiceID", "Invoice ID");
                                cls.SQLStr = "SELECT *  FROM  Sales_SalesInvoiceReturnEng WHERE BranchID = " + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 0, 80, 100, 80, 150, 150, 150, 150, 100, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 2;
                            break;
                        }

                    case "BarCodeForSpcialOffers":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("BarCode", "البـاركـود");
                                cls.SQLStr = "SELECT *  FROM Sales_BarCodeForSpecialOffersArb_Find where BranchID =" + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("BarCode", "BarCode");
                                cls.SQLStr = "SELECT *  FROM Sales_BarCodeForSpecialOffersEng_Find where BranchID =" + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 0, 0, 100, 70, 150, 70, 90, 80, 120, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 4;
                            break;
                        }

                    case "AssetPosition":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("PositionID", "الرقـم");
                                cls.SQLStr = "SELECT PositionID as  الرقـم , ArbName as [اسـم المـوقع]  FROM Ast_AssetPosition Where   BranchID =" + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("PositionID", "ID");
                                cls.SQLStr = "SELECT PositionID as  ID , EngName as [Position Name]  FROM Ast_AssetPosition Where   BranchID =" + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 150, 450, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 1;
                            break;
                        }

                    case "AssetRecipient":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("RecipientID", "الرقـم");
                                cls.SQLStr = "SELECT RecipientID as  الرقـم , ArbName as [اسـم المسـتلم]  FROM Ast_AssetRecipient Where   BranchID =" + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("RecipientID", "ID");
                                cls.SQLStr = "SELECT RecipientID as  ID , EngName as [Recipient Name]  FROM Ast_AssetRecipient Where   BranchID =" + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 150, 450, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 1;
                            break;
                        }

                    case "AssetAccount":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("AssetAccountID", "رقـم الأصـل");
                                cls.SQLStr = "SELECT  AssetAccountID as [رقـم الأصـل], ArbName as [اسـم الأصـل] , AssetSerialNumber as [الرقم التسلسلي للأصل] FROM dbo.Ast_AssetsDeclaration WHERE (BranchID = " + GlobalBranchID + ")";
                            }
                            else
                            {
                                cls.AddField("AssetAccountID", "Asset ID");
                                cls.SQLStr = "SELECT  AssetAccountID as [Asset ID], EngName as [Asset Name] , AssetSerialNumber as [Asset Serial Number] FROM dbo.Ast_AssetsDeclaration WHERE (BranchID = " + GlobalBranchID + ")";
                            }
                            ColumnWidth = new int[] { 150, 250, 150, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 1;
                            break;
                        }

                    case "AssetDeclaration":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("AssetAccountID", "رقـم الأصـــل");
                                cls.SQLStr = "SELECT AssetAccountID as [رقـم الأصـــل], ArbName as [اسـم الأصـــل], AssetSerialNumber as [الرقم التسلسلي للأصل] , AssetPurchaseValue AS [قيمة شراء الأصل], "
                                + " AnnualDepreciationRate AS [نسبة الإهلاك السنوي  %]  "
                                + "From Ast_AssetsDeclaration Where Cancel =0  And BranchID =" + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("AssetAccountID", "Asset ID");
                                cls.SQLStr = "SELECT AssetAccountID as [Asset ID], EngName as [Asset Name], AssetSerialNumber as [Asset SN] , AssetPurchaseValue AS [Asset Purchase Value], "
                               + " AnnualDepreciationRate AS [Annual Depreciation Rate %] "
                               + "From Ast_AssetsDeclaration Where Cancel =0  And BranchID =" + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 100, 150, 130, 135, 150, 150, 100, 100 };
                            cls.SearchCol = 1;
                            break;
                        }

                    case "AssetTransaction":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("ProccessID", "رقـم العملية");
                                cls.SQLStr = "SELECT dbo.Ast_AssetTransaction.ProccessID AS [رقـم العملية], dbo.Ast_AssetTransaction.AssetAccountID AS [رقم الأصـل], dbo.Ast_AssetsDeclaration.ArbName AS [اسـم الأصـل], "
                                + " dbo.Ast_AssetTransaction.AssetSerialNumber AS [الرقم التسلسلي للأصل], dbo.Ast_AssetRecipient.ArbName AS [المسـتلم], dbo.Ast_AssetPosition.ArbName AS المـوقع,"
                                + " dbo.Ast_AssetTransaction.AssetStatus AS [حـــالـة الأصــل] FROM dbo.Ast_AssetRecipient RIGHT OUTER JOIN dbo.Ast_AssetTransaction ON dbo.Ast_AssetRecipient.BranchID = "
                                + " dbo.Ast_AssetTransaction.BranchID AND dbo.Ast_AssetRecipient.RecipientID = dbo.Ast_AssetTransaction.RecipientID LEFT OUTER JOIN dbo.Ast_AssetsDeclaration"
                                + " ON dbo.Ast_AssetTransaction.AssetAccountID = dbo.Ast_AssetsDeclaration.AssetAccountID AND dbo.Ast_AssetTransaction.BranchID = dbo.Ast_AssetsDeclaration.BranchID"
                                + " LEFT OUTER JOIN dbo.Ast_AssetPosition ON dbo.Ast_AssetTransaction.PositionID = dbo.Ast_AssetPosition.PositionID AND dbo.Ast_AssetTransaction.BranchID = "
                                + " dbo.Ast_AssetPosition.BranchID WHERE  (dbo.Ast_AssetTransaction.BranchID = " + GlobalBranchID + ") AND (dbo.Ast_AssetTransaction.Cancel = 0)";
                            }
                            else
                            {
                                cls.AddField("ProccessID", "Proccess ID");
                                cls.SQLStr = "SELECT dbo.Ast_AssetTransaction.ProccessID AS [Proccess ID], dbo.Ast_AssetTransaction.AssetAccountID AS [Asset Account ID] , dbo.Ast_AssetsDeclaration.EngName AS [Asset Name]"
                                + " , dbo.Ast_AssetTransaction.AssetSerialNumber AS [Asset Serial Number], dbo.Ast_AssetTransaction.AssetStatus AS [Asset Status], dbo.Ast_AssetPosition.EngName AS [Position Name], "
                                + " dbo.Ast_AssetRecipient.EngName AS [Recipient Name] FROM dbo.Ast_AssetRecipient RIGHT OUTER JOIN dbo.Ast_AssetTransaction ON dbo.Ast_AssetRecipient.BranchID = dbo.Ast_AssetTransaction.BranchID AND"
                                + " dbo.Ast_AssetRecipient.RecipientID = dbo.Ast_AssetTransaction.RecipientID LEFT OUTER JOIN dbo.Ast_AssetsDeclaration ON dbo.Ast_AssetTransaction.AssetAccountID"
                                + " = dbo.Ast_AssetsDeclaration.AssetAccountID AND dbo.Ast_AssetTransaction.BranchID = dbo.Ast_AssetsDeclaration.BranchID LEFT OUTER JOIN dbo.Ast_AssetPosition ON"
                                + " dbo.Ast_AssetTransaction.PositionID = dbo.Ast_AssetPosition.PositionID AND dbo.Ast_AssetTransaction.BranchID = dbo.Ast_AssetPosition.BranchID"
                                + " WHERE (dbo.Ast_AssetTransaction.BranchID = " + GlobalBranchID + ") AND (dbo.Ast_AssetTransaction.Cancel = 0)";
                            }
                            ColumnWidth = new int[] { 80, 100, 200, 130, 130, 130, 130, 100, 100 };
                            cls.SearchCol = 1;
                            break;
                        }

                    case "ItemManufacuring":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("ManufacturingID", "رقـم التـصـنيـع");
                                cls.SQLStr = "SELECT dbo.Manu_ItemManufacturing_Master.ManufacturingID AS [رقـم التـصـنيـع], CASE WHEN dbo.Manu_ItemManufacturing_Master.ManufacturingDate = 0 THEN '0' "
                                + " ELSE SUBSTRING(ltrim(str(ManufacturingDate)), 1, 4) + '/' + SUBSTRING(ltrim(str(ManufacturingDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(ManufacturingDate)), 7, 2) END"
                                + " AS الـــتــــــاريـخ, dbo.Manu_ItemManufacturing_Master.BarCode AS [الـبـاركــود ] , dbo.Stc_Items.ArbName AS [اسـم الـمـادة] , dbo.Manu_ItemManufacturing_Master.Notes AS مـلاحـظـــات"
                                + " FROM dbo.Manu_ItemManufacturing_Master INNER JOIN dbo.Sales_BarCodeForPurchaseInvoiceEng_Find ON dbo.Manu_ItemManufacturing_Master.BarCode = dbo.Sales_BarCodeForPurchaseInvoiceEng_Find.BarCode"
                                + " INNER JOIN dbo.Stc_Items ON dbo.Sales_BarCodeForPurchaseInvoiceEng_Find.[Item ID] = dbo.Stc_Items.ItemID WHERE (dbo.Manu_ItemManufacturing_Master.Cancel = 0) ";
                            }
                            else
                            {
                                cls.AddField("ManufacturingID", "Manufac ID");
                                cls.SQLStr = "SELECT dbo.Manu_ItemManufacturing_Master.ManufacturingID AS [Manufac ID],CASE WHEN  dbo.Manu_ItemManufacturing_Master.ManufacturingDate = 0"
                                + " THEN '0' ELSE SUBSTRING(ltrim(str(ManufacturingDate)), 1, 4) + '/' + SUBSTRING(ltrim(str(ManufacturingDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(ManufacturingDate)), 7, 2)"
                                + " END  AS [Manufac Date], dbo.Manu_ItemManufacturing_Master.BarCode , dbo.Sales_BarCodeForPurchaseInvoiceEng_Find.[Item Name] ,"
                                + " dbo.Manu_ItemManufacturing_Master.Notes  FROM dbo.Manu_ItemManufacturing_Master INNER JOIN dbo.Sales_BarCodeForPurchaseInvoiceEng_Find ON"
                                + " dbo.Manu_ItemManufacturing_Master.BarCode = dbo.Sales_BarCodeForPurchaseInvoiceEng_Find.BarCode WHERE (dbo.Manu_ItemManufacturing_Master.Cancel = 0)";
                            }
                            ColumnWidth = new int[] { 80, 100, 100, 200, 250, 100, 100, 100 };
                            cls.SearchCol = 3;
                            break;
                        }

                    case "ManufacuringOperations":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("OperationID", "رقـم العملية");
                                cls.SQLStr = "SELECT dbo.Manu_ManufacturingOperations_Master.OperationID AS [رقـم العملية], CASE WHEN dbo.Manu_ManufacturingOperations_Master.OperationDate = 0 THEN"
                                + " '0' ELSE SUBSTRING(ltrim(str(OperationDate)), 1, 4) + '/' + SUBSTRING(ltrim(str(OperationDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(OperationDate)), 7, 2) END AS الـــتــــــاريـخ, "
                                + " dbo.Manu_ManufacturingOperations_Master.InvoiceID AS [رقم الفاتورة], dbo.Stc_Stores.ArbName AS المستودع FROM dbo.Manu_ManufacturingOperations_Master LEFT OUTER JOIN"
                                + " dbo.Stc_Stores ON dbo.Manu_ManufacturingOperations_Master.StoreID = dbo.Stc_Stores.StoreID AND dbo.Manu_ManufacturingOperations_Master.BranchID = dbo.Stc_Stores.BranchID"
                                + " WHERE (dbo.Manu_ManufacturingOperations_Master.Cancel = 0) AND (dbo.Manu_ManufacturingOperations_Master.BranchID = " + GlobalBranchID + ")";
                            }
                            else
                            {
                                cls.AddField("OperationID", "Operation ID");
                                cls.SQLStr = "SELECT dbo.Manu_ManufacturingOperations_Master.OperationID AS [Operation ID], CASE WHEN dbo.Manu_ManufacturingOperations_Master.OperationDate = 0 THEN"
                                + " '0' ELSE SUBSTRING(ltrim(str(OperationDate)), 1, 4) + '/' + SUBSTRING(ltrim(str(OperationDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(OperationDate)), 7, 2) END AS [The Date],"
                                + " dbo.Manu_ManufacturingOperations_Master.InvoiceID AS [Invoice ID], dbo.Stc_Stores.EngName AS Store FROM dbo.Manu_ManufacturingOperations_Master LEFT OUTER JOIN"
                                + " dbo.Stc_Stores ON dbo.Manu_ManufacturingOperations_Master.StoreID = dbo.Stc_Stores.StoreID AND dbo.Manu_ManufacturingOperations_Master.BranchID = dbo.Stc_Stores.BranchID"
                                + " WHERE (dbo.Manu_ManufacturingOperations_Master.Cancel = 0) AND (dbo.Manu_ManufacturingOperations_Master.BranchID = " + GlobalBranchID + ")";
                            }
                            ColumnWidth = new int[] { 80, 100, 120, 280, 120, 100, 100, 100 };
                            cls.SearchCol = 3;
                            break;
                        }

                    case "ItemsOutOnBail":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("OutID", "الرقـم");
                                cls.SQLStr = "SELECT dbo.Stc_ItemsOutonBail_Master.OutID AS [الرقـم],CASE WHEN dbo.Stc_ItemsOutonBail_Master.OutDate = 0 THEN '0' ELSE SUBSTRING(ltrim(str(OutDate)), 1, 4)"
                                + " + '/' + SUBSTRING(ltrim(str(OutDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(OutDate)), 7, 2) END AS  [التـاريـخ],dbo.Sales_Customers.ArbName AS العـمــيل  , "
                                + " dbo.Stc_Stores.ArbName AS المســتودع ,dbo.Stc_ItemsOutonBail_Master.Notes AS [مـلاحـظــات] FROM dbo.Stc_ItemsOutonBail_Master LEFT OUTER JOIN "
                                + " dbo.Stc_Stores ON dbo.Stc_ItemsOutonBail_Master.StoreID = dbo.Stc_Stores.StoreID AND dbo.Stc_ItemsOutonBail_Master.BranchID = "
                                + " dbo.Stc_Stores.BranchID LEFT OUTER JOIN dbo.Sales_Customers ON dbo.Stc_ItemsOutonBail_Master.CustomerID = dbo.Sales_Customers.CustomerID AND"
                                + " dbo.Stc_ItemsOutonBail_Master.BranchID = dbo.Sales_Customers.BranchID WHERE (dbo.Stc_ItemsOutonBail_Master.BranchID = " + GlobalBranchID + ") "
                                + " AND (dbo.Stc_ItemsOutonBail_Master.Cancel = 0)";
                            }
                            else
                            {
                                cls.AddField("OutID", "Out ID");
                                cls.SQLStr = "SELECT dbo.Stc_ItemsOutonBail_Master.OutID AS [Out ID],CASE WHEN dbo.Stc_ItemsOutonBail_Master.OutDate = 0 THEN '0' ELSE SUBSTRING(ltrim(str(OutDate)), 1, 4)"
                                + " + '/' + SUBSTRING(ltrim(str(OutDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(OutDate)), 7, 2) END AS [Out Date] , dbo.Sales_Customers.EngName AS Customer"
                                + " , dbo.Stc_Stores.EngName AS Store , dbo.Stc_ItemsOutonBail_Master.Notes FROM dbo.Stc_ItemsOutonBail_Master LEFT OUTER JOIN "
                                + " dbo.Stc_Stores ON dbo.Stc_ItemsOutonBail_Master.StoreID = dbo.Stc_Stores.StoreID AND dbo.Stc_ItemsOutonBail_Master.BranchID = "
                                + " dbo.Stc_Stores.BranchID LEFT OUTER JOIN dbo.Sales_Customers ON dbo.Stc_ItemsOutonBail_Master.CustomerID = dbo.Sales_Customers.CustomerID AND"
                                + " dbo.Stc_ItemsOutonBail_Master.BranchID = dbo.Sales_Customers.BranchID WHERE (dbo.Stc_ItemsOutonBail_Master.BranchID = " + GlobalBranchID + ") "
                                + " AND (dbo.Stc_ItemsOutonBail_Master.Cancel = 0 )";
                            }
                            ColumnWidth = new int[] { 100, 100, 150, 150, 250, 100, 100, 100 };
                            cls.SearchCol = 1;
                            break;
                        }
                    case "ItemsInOnBail":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("InID", "رقـم الإستـلام");
                                cls.SQLStr = "SELECT dbo.Stc_ItemsInonBail_Master.InID AS [رقـم الإستـلام],CASE WHEN dbo.Stc_ItemsInonBail_Master.InID = 0 THEN '0' ELSE  SUBSTRING(ltrim(str(InDate)), 1, 4)"
                                + " + '/' + SUBSTRING(ltrim(str(InDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(InDate)), 7, 2) END AS  [التـاريـخ],dbo.Sales_Suppliers.ArbName AS الـمـــورد  , "
                                + " dbo.Stc_Stores.ArbName AS المســتودع ,dbo.Stc_ItemsInonBail_Master.Notes AS [مـلاحـظــات] FROM dbo.Stc_ItemsInonBail_Master LEFT OUTER JOIN "
                                + " dbo.Stc_Stores ON dbo.Stc_ItemsInonBail_Master.StoreID = dbo.Stc_Stores.StoreID AND dbo.Stc_ItemsInonBail_Master.BranchID = "
                                + " dbo.Stc_Stores.BranchID LEFT OUTER JOIN dbo.Sales_Suppliers ON dbo.Stc_ItemsInonBail_Master.SupplierID = dbo.Sales_Suppliers.SupplierID AND"
                                + " dbo.Stc_ItemsInonBail_Master.BranchID = dbo.Sales_Suppliers.BranchID WHERE (dbo.Stc_ItemsInonBail_Master.BranchID = " + GlobalBranchID + ") "
                                + " AND (dbo.Stc_ItemsInonBail_Master.Cancel = 0)";
                            }
                            else
                            {
                                cls.AddField("InID", "In ID");
                                cls.SQLStr = "SELECT dbo.Stc_ItemsInonBail_Master.InID AS [Out ID],CASE WHEN dbo.Stc_ItemsInonBail_Master.InID = 0 THEN '0' ELSE SUBSTRING(ltrim(str(InDate)), 1, 4)"
                                + " + '/' + SUBSTRING(ltrim(str(InDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(InDate)), 7, 2) END AS [Out Date] , dbo.Sales_Suppliers.EngName AS Supplier"
                                + " , dbo.Stc_Stores.EngName AS Store , dbo.Stc_ItemsInonBail_Master.Notes FROM dbo.Stc_ItemsInonBail_Master LEFT OUTER JOIN "
                                + " dbo.Stc_Stores ON dbo.Stc_ItemsInonBail_Master.StoreID = dbo.Stc_Stores.StoreID AND dbo.Stc_ItemsInonBail_Master.BranchID = "
                                + " dbo.Stc_Stores.BranchID LEFT OUTER JOIN dbo.Sales_Suppliers ON dbo.Stc_ItemsInonBail_Master.SupplierID = dbo.Sales_Suppliers.SupplierID AND"
                                + " dbo.Stc_ItemsInonBail_Master.BranchID = dbo.Sales_Suppliers.BranchID WHERE (dbo.Stc_ItemsInonBail_Master.BranchID = " + GlobalBranchID + ") "
                                + " AND (dbo.Stc_ItemsInonBail_Master.Cancel = 0 )";
                            }
                            ColumnWidth = new int[] { 100, 100, 150, 150, 250, 100, 100, 100 };
                            cls.SearchCol = 1;
                            break;
                        }
                    case "PartiesOrderID":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("OrderID", "رقـم الطـلب");
                                cls.SQLStr = "SELECT dbo.Res_Parties_Master.OrderID AS [رقـم الطـلب], CASE WHEN dbo.Res_Parties_Master.OrderDate = 0 THEN '0' ELSE SUBSTRING(ltrim(str(OrderDate)), 1, 4) "
                                + " + '/' + SUBSTRING(ltrim(str(OrderDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(OrderDate)), 7, 2) END AS [تاريخ الطـلب], dbo.Sales_Customers.ArbName AS [العـمــيل],"
                                + "  dbo.Res_Parties_Master.NetBalance AS [الـصــافي] , dbo.Res_Parties_Master.DiscountValue AS [قيمة الخصـم], dbo.Res_Parties_Master.PaidAmount AS [المـدفـوع]"
                                + " FROM dbo.Res_Parties_Master LEFT OUTER JOIN dbo.Sales_Customers ON dbo.Res_Parties_Master.BranchID = dbo.Sales_Customers.BranchID AND dbo.Res_Parties_Master.CustomerID"
                                + " = dbo.Sales_Customers.CustomerID WHERE (dbo.Res_Parties_Master.BranchID = " + GlobalBranchID + ") AND (dbo.Res_Parties_Master.Cancel = 0)";
                            }
                            else
                            {
                                cls.AddField("OrderID", "Order ID");
                                cls.SQLStr = "SELECT dbo.Res_Parties_Master.OrderID AS [Order ID], CASE WHEN dbo.Res_Parties_Master.OrderDate = 0 THEN '0' ELSE SUBSTRING(ltrim(str(OrderDate)), 1, 4) "
                                + " + '/' + SUBSTRING(ltrim(str(OrderDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(OrderDate)), 7, 2) END AS [Order Date], dbo.Sales_Customers.EngName AS [Customer Name], "
                                + " dbo.Res_Parties_Master.NetBalance AS [Net Balance], dbo.Res_Parties_Master.DiscountValue AS [Discount Value], dbo.Res_Parties_Master.PaidAmount AS [Paid Amount]"
                                + " FROM dbo.Res_Parties_Master LEFT OUTER JOIN dbo.Sales_Customers ON dbo.Res_Parties_Master.BranchID = dbo.Sales_Customers.BranchID AND dbo.Res_Parties_Master.CustomerID"
                                + " = dbo.Sales_Customers.CustomerID WHERE (dbo.Res_Parties_Master.BranchID = " + GlobalBranchID + ") AND (dbo.Res_Parties_Master.Cancel = 0)";
                            }
                            ColumnWidth = new int[] { 100, 100, 200, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 1;
                            break;
                        }

                    case "StatusID":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("StatusID", "الـرقــم");
                                cls.SQLStr = "SELECT StatusID AS [الـرقــم], ArbName AS [الاســــــم]"
                                + " FROM Sales_PriceOffersStatus WHERE BranchID = " + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("StatusID", "Status ID");
                                cls.SQLStr = "SELECT StatusID AS [Status ID], EngName AS [Status Name]"
                                + " FROM Sales_PriceOffersStatus WHERE BranchID = " + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 150, 450, 100, 100 };
                            cls.SearchCol = 1;
                            break;
                        }

                    case "PriceOffersCustomerID":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("CustomerID", "رقم الــعـــمـــيــل");
                                cls.SQLStr = "SELECT CustomerID as  [رقم الــعـــمـــيــل],ArbName as [اسـم الــعـــمـــيــل]  FROM Sales_PriceOffersCustomer Where  Cancel =0   And BranchID =" + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("CustomerID", "Customer ID");
                                cls.SQLStr = "SELECT CustomerID as  [Customer ID],EngName as [Customer Name]  FROM Sales_PriceOffersCustomer Where  Cancel =0   And BranchID =" + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 150, 450, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 1;
                            break;
                        }

                    //case "EmployeeID":
                    //    {
                    //        if (Language == iLanguage.Arabic)
                    //        {
                    //            cls.AddField("EmployeeID", "رقم الموظف");
                    //            cls.SQLStr = "SELECT dbo.HR_EmployeeFile.EmployeeID AS [رقم الموظف], dbo.HR_EmployeeFile.ArbName AS [اسـم الموظف], dbo.HR_Departments.ArbName AS [القسـم]"
                    //            + " FROM dbo.HR_EmployeeFile LEFT OUTER JOIN dbo.HR_Departments ON dbo.HR_EmployeeFile.Department = dbo.HR_Departments.ID WHERE (dbo.HR_EmployeeFile.BranchID = " + GlobalBranchID + ")"
                    //            + " AND (dbo.HR_EmployeeFile.Cancel = 0) AND (dbo.HR_EmployeeFile.ValidFromDate) >= " + Comon.ConvertSerialDateTo(Lip.GetServerDate()) + " OR (dbo.HR_EmployeeFile.ValidFromDate = 0)";
                    //        }
                    //        else
                    //        {
                    //            cls.AddField("EmployeeID", "Employee ID");
                    //            cls.SQLStr = "SELECT dbo.HR_EmployeeFile.EmployeeID AS [Employee ID], dbo.HR_EmployeeFile.EngName AS [Employee Name], dbo.HR_Departments.EngName AS [Departments Name]"
                    //            + " FROM dbo.HR_EmployeeFile LEFT OUTER JOIN dbo.HR_Departments ON dbo.HR_EmployeeFile.Department = dbo.HR_Departments.ID WHERE (dbo.HR_EmployeeFile.BranchID = " + GlobalBranchID + ")"
                    //            + " AND (dbo.HR_EmployeeFile.Cancel = 0)  AND (dbo.HR_EmployeeFile.ValidFromDate) >= " + Comon.ConvertSerialDateTo(Lip.GetServerDate()) + " OR (dbo.HR_EmployeeFile.ValidFromDate = 0)";
                    //        }
                    //        ColumnWidth = new int[] { 100, 250, 250, 90, 80, 120, 100, 100, 100, 100, 100, 100 };
                    //        cls.SearchCol = 1;
                    //        break;
                    //    }

                    case "AllowanceID":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("ID", "رقم العلاوة");
                                cls.SQLStr = "SELECT ID AS [رقم العلاوة] , ArbName AS [اسـم العلاوة] FROM dbo.HR_AllowancesTypes WHERE (Cancel = 0)";
                            }
                            else
                            {
                                cls.AddField("ID", "Allowance ID");
                                cls.SQLStr = "SELECT ID AS [Allowance ID] , EngName AS [Allowance Name] FROM dbo.HR_AllowancesTypes WHERE (Cancel = 0)";
                            }
                            ColumnWidth = new int[] { 100, 500, 250 };
                            cls.SearchCol = 1;
                            break;
                        }

                    case "QualificationID":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("ID", "رقم المؤهل");
                                cls.SQLStr = "SELECT ID AS [رقم المؤهل] , ArbName AS [اسـم المؤهل] FROM dbo.HR_Qualifications WHERE (Cancel = 0)";
                            }
                            else
                            {
                                cls.AddField("ID", "Qualification ID");
                                cls.SQLStr = "SELECT ID AS [QualificationID ID] , EngName AS [Qualification Name] FROM dbo.HR_Qualifications WHERE (Cancel = 0)";
                            }
                            ColumnWidth = new int[] { 100, 500, 250 };
                            cls.SearchCol = 1;
                            break;
                        }

                    case "DisciplineID":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("ID", "رقم التخصص");
                                cls.SQLStr = "SELECT ID AS [رقم التخصص] , ArbName AS [اسـم التخصص] FROM dbo.HR_ScientificDisciplines WHERE (Cancel = 0)";
                            }
                            else
                            {
                                cls.AddField("ID", "Discipline ID");
                                cls.SQLStr = "SELECT ID AS [Discipline ID] , EngName AS [Discipline Name] FROM dbo.HR_ScientificDisciplines WHERE (Cancel = 0)";
                            }
                            ColumnWidth = new int[] { 100, 500, 250 };
                            cls.SearchCol = 1;
                            break;
                        }

                    case "JobID":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("ID", "رقم الوظيفة");
                                cls.SQLStr = "SELECT ID AS [رقم الوظيفة] , ArbName AS [اسـم الوظيفة] FROM dbo.HR_Jobs WHERE (Cancel = 0)";
                            }
                            else
                            {
                                cls.AddField("ID", "Job ID");
                                cls.SQLStr = "SELECT ID AS [Job ID] , EngName AS [Job Name] FROM dbo.HR_Jobs WHERE (Cancel = 0)";
                            }
                            ColumnWidth = new int[] { 100, 500, 250 };
                            cls.SearchCol = 1;
                            break;
                        }

                    case "DeductionID":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("ID", "رقم الاستقطاع");
                                cls.SQLStr = "SELECT ID AS [رقم الاستقطاع] , ArbName AS [اسـم الاستقطاع] FROM dbo.HR_DeductionsTypes WHERE (Cancel = 0)";
                            }
                            else
                            {
                                cls.AddField("ID", "Deduction ID");
                                cls.SQLStr = "SELECT ID AS [Deduction ID] , EngName AS [Deduction Name] FROM dbo.HR_DeductionsTypes WHERE (Cancel = 0)";
                            }
                            ColumnWidth = new int[] { 100, 500, 250, 100, 100 };
                            cls.SearchCol = 1;
                            break;
                        }

                    case "VacationRequest":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("SN", "الرقم");
                                cls.SQLStr = "SELECT dbo.HR_VacationRequest.SN AS [الرقم], dbo.HR_VacationRequest.EmployeeID AS [رقم الموظف], dbo.HR_EmployeeFile.ArbName AS [اسم الموظف], "
                                + " CASE WHEN dbo.HR_VacationRequest.StartDate = 0 THEN '0'  ELSE SUBSTRING(ltrim(str(StartDate)), 1, 4) + '/' + SUBSTRING(ltrim(str(StartDate)), 5, 2) + '/' + "
                                + " SUBSTRING(ltrim(str(StartDate)), 7, 2) END AS [تاريـخ بداية الأجـازة], CASE WHEN dbo.HR_VacationRequest.EndDate = 0 THEN '0' ELSE SUBSTRING(ltrim(str(EndDate)), 1, 4) "
                                + " + '/' + SUBSTRING(ltrim(str(EndDate)), 5, 2) + '/' +  SUBSTRING(ltrim(str(EndDate)), 7, 2) END AS [تاريخ أنتهاء الأجازة], dbo.HR_VacationsTypes.ArbName AS [نــوع الأجـــازة]"
                                + " FROM dbo.HR_VacationRequest INNER JOIN dbo.HR_EmployeeFile ON dbo.HR_VacationRequest.EmployeeID = dbo.HR_EmployeeFile.EmployeeID AND "
                                + " dbo.HR_VacationRequest.BranchID = dbo.HR_EmployeeFile.BranchID INNER JOIN dbo.HR_VacationsTypes ON dbo.HR_VacationRequest.VacationTypeID = dbo.HR_VacationsTypes.ID"
                                + " WHERE (dbo.HR_VacationRequest.BranchID = " + GlobalBranchID + ") AND (dbo.HR_VacationRequest.Cancel = 0)";
                            }
                            else
                            {
                                cls.AddField("SN", "SN");
                                cls.SQLStr = "SELECT dbo.HR_VacationRequest.SN, dbo.HR_VacationRequest.EmployeeID AS [Employee ID], CASE WHEN dbo.HR_VacationRequest.StartDate = 0 THEN '0' "
                                + " ELSE SUBSTRING(ltrim(str(StartDate)), 1, 4) + '/' + SUBSTRING(ltrim(str(StartDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(StartDate)), 7, 2) END AS [Start Date],"
                                + " CASE WHEN dbo.HR_VacationRequest.EndDate = 0 THEN '0' ELSE SUBSTRING(ltrim(str(EndDate)), 1, 4) + '/' + SUBSTRING(ltrim(str(EndDate)), 5, 2) + '/' + "
                                + " SUBSTRING(ltrim(str(EndDate)), 7, 2) END AS [End Date], dbo.HR_EmployeeFile.EngName AS [Employee Name], dbo.HR_VacationsTypes.EngName AS [Vacation Type]"
                                + " FROM dbo.HR_VacationRequest INNER JOIN dbo.HR_EmployeeFile ON dbo.HR_VacationRequest.EmployeeID = dbo.HR_EmployeeFile.EmployeeID AND "
                                + " dbo.HR_VacationRequest.BranchID = dbo.HR_EmployeeFile.BranchID INNER JOIN dbo.HR_VacationsTypes ON dbo.HR_VacationRequest.VacationTypeID = dbo.HR_VacationsTypes.ID"
                                + " WHERE (dbo.HR_VacationRequest.BranchID = " + GlobalBranchID + ") AND (dbo.HR_VacationRequest.Cancel = 0)";
                            }
                            ColumnWidth = new int[] { 50, 100, 150, 100, 100, 160, 100, 100, 100 };
                            cls.SearchCol = 2;
                            break;
                        }

                    case "ItemInsuranceID":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("InsuranceID", "رقم التأمين");
                                cls.SQLStr = "SELECT dbo.Res_ItemsInsurance_Master.InsuranceID AS [رقم التأمين], CASE WHEN dbo.Res_ItemsInsurance_Master.OrderDate = 0 THEN '0' "
                                + " ELSE SUBSTRING(ltrim(str(OrderDate)), 1, 4) + '/' + SUBSTRING(ltrim(str(OrderDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(OrderDate)), 7, 2) END "
                                + " AS [تاريخ التأمين], dbo.Sales_Customers.ArbName AS [اسم لعميل], dbo.Res_ItemsInsurance_Master.OrderID AS [رقم الطلب] "
                                + " FROM dbo.Res_ItemsInsurance_Master LEFT OUTER JOIN dbo.Sales_Customers ON dbo.Res_ItemsInsurance_Master.CustomerID = "
                                + " dbo.Sales_Customers.CustomerID AND dbo.Res_ItemsInsurance_Master.BranchID = dbo.Sales_Customers.BranchID WHERE "
                                + " (dbo.Res_ItemsInsurance_Master.BranchID = " + GlobalBranchID + ") AND (dbo.Res_ItemsInsurance_Master.Cancel = 0)";
                            }
                            else
                            {
                                cls.AddField("InsuranceID", "Insurance ID");
                                cls.SQLStr = "SELECT dbo.Res_ItemsInsurance_Master.InsuranceID AS [Insurance ID], CASE WHEN dbo.Res_ItemsInsurance_Master.OrderDate = 0 THEN '0' "
                                + " ELSE SUBSTRING(ltrim(str(OrderDate)), 1, 4) + '/' + SUBSTRING(ltrim(str(OrderDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(OrderDate)), 7, 2) END "
                                + " AS [Order Date], dbo.Sales_Customers.EngName AS [Customer Name], dbo.Res_ItemsInsurance_Master.OrderID AS [Order ID] "
                                + " FROM dbo.Res_ItemsInsurance_Master LEFT OUTER JOIN dbo.Sales_Customers ON dbo.Res_ItemsInsurance_Master.CustomerID = "
                                + " dbo.Sales_Customers.CustomerID AND dbo.Res_ItemsInsurance_Master.BranchID = dbo.Sales_Customers.BranchID WHERE "
                                + " (dbo.Res_ItemsInsurance_Master.BranchID = " + GlobalBranchID + ") AND (dbo.Res_ItemsInsurance_Master.Cancel = 0)";
                            }
                            ColumnWidth = new int[] { 100, 100, 200, 200, 100, 100, 100, 100 };
                            cls.SearchCol = 2;
                            break;
                        }
                    case "EmployeeID":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("EmployeeID", "رقـم العامل");
                                cls.SQLStr = "SELECT  EmployeeID as [رقـم العامل], ArbName  as [اسـم الموظف] from HR_EmployeeFile where  Cancel = 0 and BranchID=" + MySession.GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("EmployeeID", "Worker ID");
                                cls.SQLStr = "SELECT  EmployeeID as [Worker ID], EngName as [Employee Name] from HR_EmployeeFile  where  Cancel = 0 and BranchID=" + MySession.GlobalBranchID;
                            }
                            cls.SearchCol = 1;
                            ColumnWidth = new int[] { 100, 200, 100, 100, 100, 100, 100, 100, 100 };
                            break;
                        }

                    case "OnAccountRequest":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("SN", "الرقم");
                                cls.SQLStr = "SELECT dbo.HR_OnAccountRequest.SN AS [الرقم] , dbo.HR_OnAccountRequest.EmployeeID AS [رقم الموظف], dbo.HR_EmployeeFile.ArbName AS [اسم الموظف], "
                                + " dbo.HR_OnAccountRequest.OnAccountValue AS [قيمة السلفة], CASE WHEN TheDate = 0 THEN '0' ELSE SUBSTRING(ltrim(str(TheDate)), 1, 4) + '/' + SUBSTRING(ltrim(str(TheDate)), 5, 2) + '/' + "
                                + " SUBSTRING(ltrim(str(TheDate)), 7, 2) END AS [التاريخ], dbo.HR_OnAccountRequest.PaymentNotes AS [معلومات السداد], dbo.HR_OnAccountRequest.OnAccountNotes AS [ملاحظات السلفة] "
                                + " FROM dbo.HR_OnAccountRequest INNER JOIN dbo.HR_EmployeeFile ON dbo.HR_OnAccountRequest.EmployeeID = dbo.HR_EmployeeFile.EmployeeID AND dbo.HR_OnAccountRequest.BranchID = dbo.HR_EmployeeFile.BranchID"
                                + " WHERE (dbo.HR_OnAccountRequest.BranchID = " + GlobalBranchID + ") AND (dbo.HR_OnAccountRequest.Cancel = 0)";
                            }
                            else
                            {
                                cls.AddField("SN", "SN");
                                cls.SQLStr = "SELECT dbo.HR_OnAccountRequest.SN, dbo.HR_OnAccountRequest.EmployeeID AS [Employee ID], dbo.HR_EmployeeFile.EngName AS [Employee Name], "
                                + " dbo.HR_OnAccountRequest.OnAccountValue AS Amount, CASE WHEN TheDate = 0 THEN '0' ELSE SUBSTRING(ltrim(str(TheDate)), 1, 4) + '/' + SUBSTRING(ltrim(str(TheDate)), 5, 2) + '/' + "
                                + " SUBSTRING(ltrim(str(TheDate)), 7, 2) END AS [The Date], dbo.HR_OnAccountRequest.PaymentNotes AS [Payment Notes], dbo.HR_OnAccountRequest.OnAccountNotes AS [On Account Notes] "
                                + " FROM dbo.HR_OnAccountRequest INNER JOIN dbo.HR_EmployeeFile ON dbo.HR_OnAccountRequest.EmployeeID = dbo.HR_EmployeeFile.EmployeeID AND dbo.HR_OnAccountRequest.BranchID = dbo.HR_EmployeeFile.BranchID"
                                + " WHERE (dbo.HR_OnAccountRequest.BranchID = " + GlobalBranchID + ") AND (dbo.HR_OnAccountRequest.Cancel = 0)";
                            }
                            cls.SearchCol = 1;
                            ColumnWidth = new int[] { 60, 100, 150, 90, 90, 200, 200, 100, 100, 100 };
                            break;
                        }

                    case "ExportType":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("ID", "الرقم");
                                cls.SQLStr = "SELECT ID AS [الرقم], ArbName AS [الاســـم] FROM dbo.Exp_ExportType WHERE (Cancel = 0)";
                            }
                            else
                            {
                                cls.AddField("ID", "ID");
                                cls.SQLStr = "SELECT ID AS [ID], EngName AS [Name] FROM dbo.Exp_ExportType WHERE (Cancel = 0)";
                            }
                            cls.SearchCol = 1;
                            ColumnWidth = new int[] { 100, 450, 150, 90, 90, 200, 200, 100, 100, 100 };
                            break;
                        }

                    case "PreparedByID":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("ID", "الرقم");
                                cls.SQLStr = "SELECT ID AS [الرقم], ArbName AS [الاســـم] FROM dbo.Exp_PreparingWayExport WHERE (Cancel = 0)";
                            }
                            else
                            {
                                cls.AddField("ID", "ID");
                                cls.SQLStr = "SELECT ID AS [ID], EngName AS [Name] FROM dbo.Exp_PreparingWayExport WHERE (Cancel = 0)";
                            }
                            cls.SearchCol = 1;
                            ColumnWidth = new int[] { 100, 450, 150, 90, 90, 200, 200, 100, 100, 100 };
                            break;
                        }

                    case "ExportToID":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("ID", "الرقم");
                                cls.SQLStr = "SELECT ID AS [الرقم], ArbName AS [الاســـم] FROM dbo.Exp_ExportedTo WHERE (Cancel = 0)";
                            }
                            else
                            {
                                cls.AddField("ID", "ID");
                                cls.SQLStr = "SELECT ID AS [ID], EngName AS [Name] FROM dbo.Exp_ExportedTo WHERE (Cancel = 0)";
                            }
                            cls.SearchCol = 1;
                            ColumnWidth = new int[] { 100, 450, 150, 90, 90, 200, 200, 100, 100, 100 };
                            break;
                        }

                    case "ImportType":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("ID", "الرقم");
                                cls.SQLStr = "SELECT ID AS [الرقم], ArbName AS [الاســـم] FROM dbo.Imp_ImportType WHERE (Cancel = 0)";
                            }
                            else
                            {
                                cls.AddField("ID", "ID");
                                cls.SQLStr = "SELECT ID AS [ID], EngName AS [Name] FROM dbo.Imp_ImportType WHERE (Cancel = 0)";
                            }
                            cls.SearchCol = 1;
                            ColumnWidth = new int[] { 100, 450, 150, 90, 90, 200, 200, 100, 100, 100 };
                            break;
                        }

                    case "ImportedFrom":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("ID", "الرقم");
                                cls.SQLStr = "SELECT ID AS [الرقم], ArbName AS [الاســـم] FROM dbo.Imp_ImportedFrom WHERE (Cancel = 0)";
                            }
                            else
                            {
                                cls.AddField("ID", "ID");
                                cls.SQLStr = "SELECT ID AS [ID], EngName AS [Name] FROM dbo.Imp_ImportedFrom WHERE (Cancel = 0)";
                            }
                            cls.SearchCol = 1;
                            ColumnWidth = new int[] { 100, 450, 150, 90, 90, 200, 200, 100, 100, 100 };
                            break;
                        }

                    case "ReferredTo":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("ID", "الرقم");
                                cls.SQLStr = "SELECT ID AS [الرقم], ArbName AS [الاســـم] FROM dbo.Imp_ReferredTo WHERE (Cancel = 0)";
                            }
                            else
                            {
                                cls.AddField("ID", "ID");
                                cls.SQLStr = "SELECT ID AS [ID], EngName AS [Name] FROM dbo.Imp_ReferredTo WHERE (Cancel = 0)";
                            }
                            cls.SearchCol = 1;
                            ColumnWidth = new int[] { 100, 450, 150, 90, 90, 200, 200, 100, 100, 100 };
                            break;
                        }

                    case "ImportTrans":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("ID", "م");
                                cls.SQLStr = "SELECT  dbo.Imp_ImportForm.ID AS [م], dbo.Imp_ImportForm.ImportID AS [رقم الوارد], CASE WHEN dbo.Imp_ImportForm.ImportDate = 0 THEN '0' "
                                + " ELSE SUBSTRING(ltrim(str(ImportDate)), 1, 4) + '/' + SUBSTRING(ltrim(str(ImportDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(ImportDate)), 7, 2) "
                                + " END AS [تاريخ الوارد], dbo.Imp_ImportForm.ExportID AS [رقم الصادر], dbo.Imp_ImportType.ArbName AS [نوع الوارد], dbo.Imp_ImportedFrom.ArbName AS [وارد من]"
                                + " FROM dbo.Imp_ImportForm INNER JOIN dbo.Imp_ImportType ON dbo.Imp_ImportForm.ImportTypeID = dbo.Imp_ImportType.ID INNER JOIN dbo.Imp_ImportedFrom ON"
                                + " dbo.Imp_ImportForm.ImportedFromID = dbo.Imp_ImportedFrom.ID WHERE (dbo.Imp_ImportForm.BranchID = " + GlobalBranchID + ") AND (dbo.Imp_ImportForm.Cancel = 0)";
                            }
                            else
                            {
                                cls.AddField("ID", "ID");
                                cls.SQLStr = "SELECT  dbo.Imp_ImportForm.ID AS [ID], dbo.Imp_ImportForm.ImportID AS [Import ID], CASE WHEN dbo.Imp_ImportForm.ImportDate = 0 THEN '0' "
                                + " ELSE SUBSTRING(ltrim(str(ImportDate)), 1, 4) + '/' + SUBSTRING(ltrim(str(ImportDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(ImportDate)), 7, 2) "
                                + " END AS [Import Date], dbo.Imp_ImportForm.ExportID AS [Export ID], dbo.Imp_ImportType.EngName AS [Import Type], dbo.Imp_ImportedFrom.EngName AS [Imported From]"
                                + " FROM dbo.Imp_ImportForm INNER JOIN dbo.Imp_ImportType ON dbo.Imp_ImportForm.ImportTypeID = dbo.Imp_ImportType.ID INNER JOIN dbo.Imp_ImportedFrom ON"
                                + " dbo.Imp_ImportForm.ImportedFromID = dbo.Imp_ImportedFrom.ID WHERE (dbo.Imp_ImportForm.BranchID = " + GlobalBranchID + ") AND (dbo.Imp_ImportForm.Cancel = 0)";
                            }
                            cls.SearchCol = 1;
                            ColumnWidth = new int[] { 70, 100, 100, 100, 120, 150, 100, 100, 100, 100 };
                            break;
                        }

                    case "ExportTrans":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("ID", "م");
                                cls.SQLStr = "SELECT dbo.Exp_ExportForm.ID AS [م], dbo.Exp_ExportForm.ExportID AS [رقم الصادر], CASE WHEN dbo.Exp_ExportForm.ExportDate = 0 THEN '0' "
                                + " ELSE SUBSTRING(ltrim(str(ExportDate)), 1, 4) + '/' + SUBSTRING(ltrim(str(ExportDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(ExportDate)), 7, 2) "
                                + " END AS [تاريخ الصادر] , dbo.Exp_ExportForm.ImportID AS [رقم الوارد], dbo.Exp_ExportType.ArbName AS [نوع الصادر], dbo.Exp_ExportedTo.ArbName AS [صادر الى]"
                                + " FROM dbo.Exp_ExportForm INNER JOIN dbo.Exp_ExportType ON dbo.Exp_ExportForm.ExportTypeID = dbo.Exp_ExportType.ID INNER JOIN dbo.Exp_ExportedTo ON"
                                + " dbo.Exp_ExportForm.ExportToID = dbo.Exp_ExportedTo.ID WHERE (dbo.Exp_ExportForm.BranchID = " + GlobalBranchID + ") AND (dbo.Exp_ExportForm.Cancel = 0)";
                            }
                            else
                            {
                                cls.AddField("ID", "ID");
                                cls.SQLStr = "SELECT dbo.Exp_ExportForm.ID AS [ID], dbo.Exp_ExportForm.ExportID AS [Export ID], CASE WHEN dbo.Exp_ExportForm.ExportDate = 0 THEN '0' "
                               + " ELSE SUBSTRING(ltrim(str(ExportDate)), 1, 4) + '/' + SUBSTRING(ltrim(str(ExportDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(ExportDate)), 7, 2) "
                               + " END AS [Export Date] , dbo.Exp_ExportForm.ImportID AS [Import ID], dbo.Exp_ExportType.EngName AS [Export Type], dbo.Exp_ExportedTo.EngName AS [Exported To]"
                               + " FROM dbo.Exp_ExportForm INNER JOIN dbo.Exp_ExportType ON dbo.Exp_ExportForm.ExportTypeID = dbo.Exp_ExportType.ID INNER JOIN dbo.Exp_ExportedTo ON"
                               + " dbo.Exp_ExportForm.ExportToID = dbo.Exp_ExportedTo.ID WHERE (dbo.Exp_ExportForm.BranchID = " + GlobalBranchID + ") AND (dbo.Exp_ExportForm.Cancel = 0)";
                            }
                            cls.SearchCol = 1;
                            ColumnWidth = new int[] { 70, 100, 100, 100, 120, 150, 100, 100, 100, 100 };
                            break;
                        }

                    case "ArchiveType":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("ID", "الرقم");
                                cls.SQLStr = "SELECT ID AS [الرقم], ArbName AS [الاســـم] FROM dbo.Arc_ArchiveType WHERE (Cancel = 0)";
                            }
                            else
                            {
                                cls.AddField("ID", "ID");
                                cls.SQLStr = "SELECT ID AS [ID], EngName AS [Name] FROM dbo.Arc_ArchiveType WHERE (Cancel = 0)";
                            }
                            cls.SearchCol = 1;
                            ColumnWidth = new int[] { 100, 450, 150, 90, 90, 200, 200, 100, 100, 100 };
                            break;
                        }

                    case "ArchiveTrans":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("ID", "الرقم");
                                cls.SQLStr = "SELECT dbo.Arc_ArchiveForm.ID AS [الرقم], dbo.Arc_ArchiveForm.DocNo AS [رقم المستند], CASE WHEN dbo.Arc_ArchiveForm.DocDate = 0 THEN '0' "
                                + " ELSE SUBSTRING(ltrim(str(DocDate)), 1, 4) + '/' + SUBSTRING(ltrim(str(DocDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(DocDate)), 7, 2) END AS [الـتـاريـخ],"
                                + " dbo.Arc_ArchiveType.ArbName AS [نــــوع الـوثيـقـة], dbo.Arc_PreparingWayArchive.ArbName AS [جـهــة الإعـــداد] FROM dbo.Arc_ArchiveForm INNER JOIN"
                                + " dbo.Arc_ArchiveType ON dbo.Arc_ArchiveForm.DocType = dbo.Arc_ArchiveType.ID INNER JOIN dbo.Arc_PreparingWayArchive ON dbo.Arc_ArchiveForm.PreparedBy"
                                + " = dbo.Arc_PreparingWayArchive.ID WHERE (dbo.Arc_ArchiveForm.BranchID = " + GlobalBranchID + ") AND (dbo.Arc_ArchiveForm.Cancel = 0)";
                            }
                            else
                            {
                                cls.AddField("ID", "ID");
                                cls.SQLStr = "SELECT dbo.Arc_ArchiveForm.ID AS [ID], dbo.Arc_ArchiveForm.DocNo AS [Doc. No], CASE WHEN dbo.Arc_ArchiveForm.DocDate = 0 THEN '0' "
                                + " ELSE SUBSTRING(ltrim(str(DocDate)), 1, 4) + '/' + SUBSTRING(ltrim(str(DocDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(DocDate)), 7, 2) END AS [Doc. Date],"
                                + " dbo.Arc_ArchiveType.EngName AS [Archive Type], dbo.Arc_PreparingWayArchive.EngName AS Prepared By FROM dbo.Arc_ArchiveForm INNER JOIN"
                                + " dbo.Arc_ArchiveType ON dbo.Arc_ArchiveForm.DocType = dbo.Arc_ArchiveType.ID INNER JOIN dbo.Arc_PreparingWayArchive ON dbo.Arc_ArchiveForm.PreparedBy"
                                + " = dbo.Arc_PreparingWayArchive.ID WHERE (dbo.Arc_ArchiveForm.BranchID = " + GlobalBranchID + ") AND (dbo.Arc_ArchiveForm.Cancel = 0)";
                            }
                            cls.SearchCol = 1;
                            ColumnWidth = new int[] { 70, 100, 100, 150, 150, 150, 100, 100, 100, 100 };
                            break;
                        }

                    case "PreparingWayArchive":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("ID", "الرقم");
                                cls.SQLStr = "SELECT ID AS [الرقم], ArbName AS [الاســـم] FROM dbo.Arc_PreparingWayArchive WHERE (Cancel = 0)";
                            }
                            else
                            {
                                cls.AddField("ID", "ID");
                                cls.SQLStr = "SELECT ID AS [ID], EngName AS [Name] FROM dbo.Arc_PreparingWayArchive WHERE (Cancel = 0)";
                            }
                            cls.SearchCol = 1;
                            ColumnWidth = new int[] { 100, 450, 150, 90, 90, 200, 200, 100, 100, 100 };
                            break;
                        }

                    case "RecipientGroups":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("GroupID", "الرقم");
                                cls.SQLStr = "SELECT GroupID AS [الرقم], ArbName AS [الاســـم] FROM dbo.SMS_RecipientGroupsMaster WHERE (Cancel = 0)";
                            }
                            else
                            {
                                cls.AddField("GroupID", "Group ID");
                                cls.SQLStr = "SELECT GroupID AS [Group ID], EngName AS [Name] FROM dbo.SMS_RecipientGroupsMaster WHERE (Cancel = 0)";
                            }
                            cls.SearchCol = 1;
                            ColumnWidth = new int[] { 100, 480, 150, 90, 90, 200, 200, 100, 100, 100 };
                            break;
                        }

                    case "TelephoneBook":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("ID", "الـــــرقم");
                                cls.SQLStr = "SELECT ID as [الـــــرقم], ArbName as [الاسم  ] "
                                + "  , FixedTel as [الـهــــــاتـف], Mobile as [مــوبــــايـل] , Notes as [مـلاحـظــــــــات] "
                                + "From TelephoneBook Where Cancel =0  And BranchID =" + GlobalBranchID;
                            }
                            else
                            {
                                cls.AddField("ID", "ID");
                                cls.SQLStr = "SELECT  ID as [ID], EngName as [ Name] , FixedTel, Mobile , Notes  "
                                 + "From TelephoneBook Where Cancel =0  And BranchID =" + GlobalBranchID;
                            }
                            cls.SearchCol = 1;
                            ColumnWidth = new int[] { 80, 150, 100, 100, 150, 100, 100, 100, 100, 100 };
                            break;
                        }

                    case "TempSalesInvoice":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("InvoiceID", "رقم الفاتورة");
                                cls.SQLStr = "SELECT dbo.Sales_TempSalesInvoiceMaster.InvoiceID AS [رقم الفاتورة], CASE WHEN dbo.Sales_TempSalesInvoiceMaster.InvoiceDate = 0 THEN '0' "
                                + " ELSE SUBSTRING(ltrim(str(InvoiceDate)), 1, 4) + '/' + SUBSTRING(ltrim(str(InvoiceDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(InvoiceDate)), 7, 2) END "
                                + " AS [تاريخ الفاتورة], dbo.Sales_TempSalesInvoiceMaster.AuthorizedPerson AS [الشخص المعني], dbo.Sales_PriceOffersCustomer.ArbName AS العميل, "
                                + " dbo.Sales_TempSalesInvoiceMaster.ApprovalNo AS [رقم التعميد], CASE WHEN dbo.Sales_TempSalesInvoiceMaster.ApprovalDate = 0 THEN '0' ELSE "
                                + " SUBSTRING(ltrim(str(ApprovalDate)), 1, 4) + '/' + SUBSTRING(ltrim(str(ApprovalDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(ApprovalDate)), 7, 2) END"
                                + " AS [تاريخ التعميد] FROM dbo.Sales_TempSalesInvoiceMaster LEFT OUTER JOIN dbo.Sales_PriceOffersCustomer ON dbo.Sales_TempSalesInvoiceMaster.BranchID"
                                + " = dbo.Sales_PriceOffersCustomer.BranchID AND dbo.Sales_TempSalesInvoiceMaster.CustomerID = dbo.Sales_PriceOffersCustomer.CustomerID"
                                + " WHERE (dbo.Sales_TempSalesInvoiceMaster.BranchID = " + GlobalBranchID + ") AND (dbo.Sales_TempSalesInvoiceMaster.Cancel = 0)";
                            }
                            else
                            {
                                cls.AddField("InvoiceID", "Invoice ID");
                                cls.SQLStr = "SELECT dbo.Sales_TempSalesInvoiceMaster.InvoiceID AS [Invoice ID], CASE WHEN dbo.Sales_TempSalesInvoiceMaster.InvoiceDate = 0 THEN '0' "
                                + " ELSE SUBSTRING(ltrim(str(InvoiceDate)), 1, 4) + '/' + SUBSTRING(ltrim(str(InvoiceDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(InvoiceDate)), 7, 2) END "
                                + " AS [Invoice Date], dbo.Sales_TempSalesInvoiceMaster.AuthorizedPerson AS [Authorized Person], dbo.Sales_PriceOffersCustomer.EngName AS [Customer], "
                                + " dbo.Sales_TempSalesInvoiceMaster.ApprovalNo AS [Approval No.], CASE WHEN dbo.Sales_TempSalesInvoiceMaster.ApprovalDate = 0 THEN '0' ELSE "
                                + " SUBSTRING(ltrim(str(ApprovalDate)), 1, 4) + '/' + SUBSTRING(ltrim(str(ApprovalDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(ApprovalDate)), 7, 2) END"
                                + " AS [Approval Date] FROM dbo.Sales_TempSalesInvoiceMaster LEFT OUTER JOIN dbo.Sales_PriceOffersCustomer ON dbo.Sales_TempSalesInvoiceMaster.BranchID"
                                + " = dbo.Sales_PriceOffersCustomer.BranchID AND dbo.Sales_TempSalesInvoiceMaster.CustomerID = dbo.Sales_PriceOffersCustomer.CustomerID"
                                + " WHERE (dbo.Sales_TempSalesInvoiceMaster.BranchID = " + GlobalBranchID + ") AND (dbo.Sales_TempSalesInvoiceMaster.Cancel = 0)";
                            }
                            cls.SearchCol = 1;
                            ColumnWidth = new int[] { 90, 100, 150, 150, 100, 100, 100, 100, 100, 100 };
                            break;
                        }

                    case "TempSalesInvoiceDoctor":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("InvoiceID", "رقم الفاتورة");
                                cls.SQLStr = " SELECT InvoiceID AS [رقم الفاتورة], CASE WHEN dbo.Sales_TempSalesInvoiceMaster.InvoiceDate = 0 THEN '0' ELSE SUBSTRING(ltrim(str(InvoiceDate)), 1, 4) "
                                + "   + '/' + SUBSTRING(ltrim(str(InvoiceDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(InvoiceDate)), 7, 2) END AS [تاريخ الفاتورة], AuthorizedPerson AS [اسم المريض ] "
                                + "  FROM Sales_TempSalesInvoiceMaster WHERE (BranchID = " + GlobalBranchID + ") AND (Cancel = 0)";
                            }
                            else
                            {
                                cls.AddField("InvoiceID", "Invoice ID");
                                cls.SQLStr = " SELECT InvoiceID AS [Invoice ID], CASE WHEN dbo.Sales_TempSalesInvoiceMaster.InvoiceDate = 0 THEN '0' ELSE SUBSTRING(ltrim(str(InvoiceDate)), 1, 4) "
                              + "   + '/' + SUBSTRING(ltrim(str(InvoiceDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(InvoiceDate)), 7, 2) END AS [Invoice Date], AuthorizedPerson AS [Patient Name] "
                              + "  FROM Sales_TempSalesInvoiceMaster WHERE (BranchID = " + GlobalBranchID + ") AND (Cancel = 0)";
                            }
                            cls.SearchCol = 1;
                            ColumnWidth = new int[] { 90, 100, 400 };
                            break;
                        }

                    case "ICD10Code":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("ID", "الرقم");
                                cls.SQLStr = "SELECT  ID as [الرقم], CODE as [الـــكود], ArbName as [الاسم]  FROM dbo.Clinc_ICD10Code Order By Code";
                            }
                            else
                            {
                                cls.AddField("ID", "ID");
                                cls.SQLStr = "SELECT  ID as [Icd10 ID] , CODE as Code], EngName as [  Name] FROM dbo.Clinc_ICD10Code Order By Code";
                            }
                            ColumnWidth = new int[] { 80, 150, 600 };
                            break;
                        }

                    case "PatientID":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("PateintID", "الرقم");
                                cls.SQLStr = "SELECT  PateintID as [الرقم],ArbName as [اسم المريض] ,NoID as[رقم الهوية] ,Mobile as[جـــوال], Tel as [هاتف] FROM dbo.CLINIC_PatientFile Order By PateintID";
                            }
                            else
                            {
                                cls.AddField("PateintID", "ID");
                                cls.SQLStr = "SELECT  PateintID as [File No], EngName as [Patient Name] ,NoID as[ID No], Mobile  , Tel FROM dbo.CLINIC_PatientFile Order By PateintID";
                            }
                            cls.SearchCol = 2;
                            ColumnWidth = new int[] { 120, 200, 100, 130, 130 };
                            break;
                        }

                    case "MedicalList":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("ID", "الرقـم");
                                cls.SQLStr = "SELECT  ID as الرقـم, ArbName as الاســـم From ClinicMedicalList Where Cancel =0  ";
                            }
                            else
                            {
                                cls.AddField("ID", "ID");
                                cls.SQLStr = "SELECT  ID , EngName as Name From ClinicMedicalList Where Cancel =0  ";
                            }
                            cls.SearchCol = 1;

                            ColumnWidth = new int[] { 150, 430, 300 };
                            break;
                        }

                    case "MachineID":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("MachineID", "رقم المكينة");
                                cls.SQLStr = "SELECT  MachineID as [رقم المكينة], ArbName as الإسم From Menu_FactoryMachine ";
                            }
                            else
                            {
                                cls.AddField("MachineID", "Machine ID");
                                cls.SQLStr = "SELECT  MachineID as [Machine ID] , EngName as Name   From Menu_FactoryMachine  ";
                            }
                            
                           
                            ColumnWidth = new int[] { 150, 430, 300 };
                            cls.SearchCol = 1;
                            break;
                        }
                    case "CommandID":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("ComandID", "رقم الأمر");
                                cls.SQLStr = "SELECT  ComandID as [رقم الأمر], Barcode as الباركود From Menu_FactoryRunCommandMaster ";
                            }
                            else
                            {
                                cls.AddField("ComandID", "Comand ID");
                                cls.SQLStr = "SELECT  ComandID as [Comand ID] , Barcode as BarCode   From Menu_FactoryRunCommandMaster ";
                            }


                            ColumnWidth = new int[] { 150, 430, 300 };
                            cls.SearchCol = 1;
                            break;

                        }
                    case "WaxCommend":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("CommandID", "رقـم الأمر");
                                cls.SQLStr = "SELECT DISTINCT dbo.Manu_CadWaxFactoryMaster.CommandID as [رقـم الأمر] ,CASE WHEN dbo.Manu_CadWaxFactoryMaster.DateBefore = 0 THEN '0' ELSE SUBSTRING(ltrim(str(DateBefore)), 1, 4) + '/' + SUBSTRING(ltrim(str(DateBefore)), 5, 2) + '/' + SUBSTRING(ltrim(str(DateBefore)), 7, 2) END as [تاريخ التسليم],CASE WHEN dbo.Manu_CadWaxFactoryMaster.DateAfter = 0 THEN '0' ELSE SUBSTRING(ltrim(str(DateAfter)), 1, 4) + '/' + SUBSTRING(ltrim(str(DateAfter)), 5, 2) + '/' + SUBSTRING(ltrim(str(DateAfter)), 7, 2) END as [تاريخ الاستلام],dbo.Manu_CadWaxFactoryMaster.OrderID [رقم الطلبية ] "
                                 + " ,Sales_Customers.ArbName as [اسم العميل],Sales_SalesDelegate.ArbName [اسم المندوب ] From dbo.Manu_CadWaxFactoryMaster "
                                 + " inner join Manu_OrderRestriction  on Manu_CadWaxFactoryMaster.OrderID= Manu_OrderRestriction.OrderID "
                                + " inner join Sales_Customers on Manu_OrderRestriction.CustomerID=Sales_Customers.AccountID "
                               + " inner join Sales_SalesDelegate on Manu_OrderRestriction.DelegateID=Sales_SalesDelegate.DelegateID "
                               + " WHERE  (dbo.Manu_CadWaxFactoryMaster.Cancel = 0) and   Manu_CadWaxFactoryMaster.BranchID =1 and Manu_CadWaxFactoryMaster.TypeStageID=1 ";
                            }
                            else
                            {
                                cls.AddField("CommandID", "Commend ID");
                                cls.SQLStr = "SELECT DISTINCT dbo.Manu_CadWaxFactoryMaster.CommandID as [Commend ID] ,CASE WHEN dbo.Manu_CadWaxFactoryMaster.DateBefore = 0 THEN '0' ELSE SUBSTRING(ltrim(str(DateBefore)), 1, 4) + '/' + SUBSTRING(ltrim(str(DateBefore)), 5, 2) + '/' + SUBSTRING(ltrim(str(DateBefore)), 7, 2) END as [Date Before],CASE WHEN dbo.Manu_CadWaxFactoryMaster.DateAfter = 0 THEN '0' ELSE SUBSTRING(ltrim(str(DateAfter)), 1, 4) + '/' + SUBSTRING(ltrim(str(DateAfter)), 5, 2) + '/' + SUBSTRING(ltrim(str(DateAfter)), 7, 2) END as [Date After],dbo.Manu_CadWaxFactoryMaster.OrderID [Order ID] "
                                 + " ,Sales_Customers.ArbName as [Customer Name],Sales_SalesDelegate.ArbName [Delegete Name] From dbo.Manu_CadWaxFactoryMaster "
                                 + " inner join Manu_OrderRestriction  on Manu_CadWaxFactoryMaster.OrderID= Manu_OrderRestriction.OrderID "
                                + " inner join Sales_Customers on Manu_OrderRestriction.CustomerID=Sales_Customers.AccountID "
                               + " inner join Sales_SalesDelegate on Manu_OrderRestriction.DelegateID=Sales_SalesDelegate.DelegateID "
                               + " WHERE  (dbo.Manu_CadWaxFactoryMaster.Cancel = 0) and   Manu_CadWaxFactoryMaster.BranchID =1 and Manu_CadWaxFactoryMaster.TypeStageID=1 ";

                            }
                            ColumnWidth = new int[] { 100, 150, 150, 80, 150, 150, 150, 150, 100, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 2;
                            break;
                        }
             
                    case "CadCommend":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("CommandID", "رقـم الأمر");
                                cls.SQLStr = "SELECT DISTINCT dbo.Manu_CadWaxFactoryMaster.CommandID as [رقـم الأمر] ,CASE WHEN dbo.Manu_CadWaxFactoryMaster.DateBefore = 0 THEN '0' ELSE SUBSTRING(ltrim(str(DateBefore)), 1, 4) + '/' + SUBSTRING(ltrim(str(DateBefore)), 5, 2) + '/' + SUBSTRING(ltrim(str(DateBefore)), 7, 2) END as [تاريخ التسليم],CASE WHEN dbo.Manu_CadWaxFactoryMaster.DateAfter = 0 THEN '0' ELSE SUBSTRING(ltrim(str(DateAfter)), 1, 4) + '/' + SUBSTRING(ltrim(str(DateAfter)), 5, 2) + '/' + SUBSTRING(ltrim(str(DateAfter)), 7, 2) END as [تاريخ الاستلام],dbo.Manu_CadWaxFactoryMaster.OrderID [رقم الطلبية ] "
                                 + " ,Sales_Customers.ArbName as [اسم العميل],Sales_SalesDelegate.ArbName [اسم المندوب ] From dbo.Manu_CadWaxFactoryMaster "
                                 + " inner join Manu_OrderRestriction  on Manu_CadWaxFactoryMaster.OrderID= Manu_OrderRestriction.OrderID "
                                + " inner join Sales_Customers on Manu_OrderRestriction.CustomerID=Sales_Customers.AccountID "
                               + " inner join Sales_SalesDelegate on Manu_OrderRestriction.DelegateID=Sales_SalesDelegate.DelegateID "
                               + " WHERE  (dbo.Manu_CadWaxFactoryMaster.Cancel = 0) and   Manu_CadWaxFactoryMaster.BranchID =1 and Manu_CadWaxFactoryMaster.TypeStageID=2 ";

                            }
                            else
                            {
                                cls.AddField("CommandID", "Commend ID");
                                cls.SQLStr = "SELECT DISTINCT dbo.Manu_CadWaxFactoryMaster.CommandID as [Commend ID] ,CASE WHEN dbo.Manu_CadWaxFactoryMaster.DateBefore = 0 THEN '0' ELSE SUBSTRING(ltrim(str(DateBefore)), 1, 4) + '/' + SUBSTRING(ltrim(str(DateBefore)), 5, 2) + '/' + SUBSTRING(ltrim(str(DateBefore)), 7, 2) END as [Date Before],CASE WHEN dbo.Manu_CadWaxFactoryMaster.DateAfter = 0 THEN '0' ELSE SUBSTRING(ltrim(str(DateAfter)), 1, 4) + '/' + SUBSTRING(ltrim(str(DateAfter)), 5, 2) + '/' + SUBSTRING(ltrim(str(DateAfter)), 7, 2) END as [Date After],dbo.Manu_CadWaxFactoryMaster.OrderID [Order ID] "
                                 + " ,Sales_Customers.ArbName as [Customer Name],Sales_SalesDelegate.ArbName [Delegete Name] From dbo.Manu_CadWaxFactoryMaster "
                                 + " inner join Manu_OrderRestriction  on Manu_CadWaxFactoryMaster.OrderID= Manu_OrderRestriction.OrderID "
                                + " inner join Sales_Customers on Manu_OrderRestriction.CustomerID=Sales_Customers.AccountID "
                               + " inner join Sales_SalesDelegate on Manu_OrderRestriction.DelegateID=Sales_SalesDelegate.DelegateID "
                               + " WHERE  (dbo.Manu_CadWaxFactoryMaster.Cancel = 0) and   Manu_CadWaxFactoryMaster.BranchID =1 and Manu_CadWaxFactoryMaster.TypeStageID=2 ";

                            }
                            ColumnWidth = new int[] { 100, 150, 150, 80, 150, 150, 150, 150, 100, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 2;
                            break;
                        }
                    case "ZirconCommendFactory":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("CommandID", "رقـم الأمر");
                                cls.SQLStr = "SELECT DISTINCT dbo.Manu_ZirconDiamondFactoryMaster.CommandID as [رقـم الأمر] ,CASE WHEN dbo.Manu_ZirconDiamondFactoryMaster.DateBefore = 0 THEN '0' ELSE SUBSTRING(ltrim(str(DateBefore)), 1, 4) + '/' + SUBSTRING(ltrim(str(DateBefore)), 5, 2) + '/' + SUBSTRING(ltrim(str(DateBefore)), 7, 2) END as [تاريخ التسليم],CASE WHEN dbo.Manu_ZirconDiamondFactoryMaster.DateAfter = 0 THEN '0' ELSE SUBSTRING(ltrim(str(DateAfter)), 1, 4) + '/' + SUBSTRING(ltrim(str(DateAfter)), 5, 2) + '/' + SUBSTRING(ltrim(str(DateAfter)), 7, 2) END as [تاريخ الاستلام],dbo.Manu_ZirconDiamondFactoryMaster.OrderID [رقم الطلبية ] "
                                 + " ,Sales_Customers.ArbName as [اسم العميل],Sales_SalesDelegate.ArbName [اسم المندوب ] From dbo.Manu_ZirconDiamondFactoryMaster "
                                 + " inner join Manu_OrderRestriction  on Manu_ZirconDiamondFactoryMaster.OrderID= Manu_OrderRestriction.OrderID "
                                + " inner join Sales_Customers on Manu_OrderRestriction.CustomerID=Sales_Customers.AccountID "
                               + " inner join Sales_SalesDelegate on Manu_OrderRestriction.DelegateID=Sales_SalesDelegate.DelegateID "
                               + " WHERE  (dbo.Manu_ZirconDiamondFactoryMaster.Cancel = 0) and   Manu_ZirconDiamondFactoryMaster.BranchID =1 and Manu_ZirconDiamondFactoryMaster.TypeStageID=3 ";
                            }
                            else
                            {
                                cls.AddField("CommandID", "Commend ID");
                                cls.SQLStr = "SELECT DISTINCT dbo.Manu_ZirconDiamondFactoryMaster.CommandID as [Commend ID] ,CASE WHEN dbo.Manu_ZirconDiamondFactoryMaster.DateBefore = 0 THEN '0' ELSE SUBSTRING(ltrim(str(DateBefore)), 1, 4) + '/' + SUBSTRING(ltrim(str(DateBefore)), 5, 2) + '/' + SUBSTRING(ltrim(str(DateBefore)), 7, 2) END as [Date Before],CASE WHEN dbo.Manu_ZirconDiamondFactoryMaster.DateAfter = 0 THEN '0' ELSE SUBSTRING(ltrim(str(DateAfter)), 1, 4) + '/' + SUBSTRING(ltrim(str(DateAfter)), 5, 2) + '/' + SUBSTRING(ltrim(str(DateAfter)), 7, 2) END as [Date After],dbo.Manu_ZirconDiamondFactoryMaster.OrderID [Order ID] "
                                 + " ,Sales_Customers.ArbName as [Customer Name],Sales_SalesDelegate.ArbName [Delegete Name] From dbo.Manu_ZirconDiamondFactoryMaster "
                                 + " inner join Manu_OrderRestriction  on Manu_ZirconDiamondFactoryMaster.OrderID= Manu_OrderRestriction.OrderID "
                                + " inner join Sales_Customers on Manu_OrderRestriction.CustomerID=Sales_Customers.AccountID "
                               + " inner join Sales_SalesDelegate on Manu_OrderRestriction.DelegateID=Sales_SalesDelegate.DelegateID "
                               + " WHERE  (dbo.Manu_ZirconDiamondFactoryMaster.Cancel = 0) and   Manu_ZirconDiamondFactoryMaster.BranchID =1 and Manu_ZirconDiamondFactoryMaster.TypeStageID=3 ";

                            }
                            ColumnWidth = new int[] { 100, 150, 150, 80, 150, 150, 150, 150, 100, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 2;
                            break;
                        }
                    case "DiamondCommendFactory":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("CommandID", "رقـم الأمر");
                                cls.SQLStr = "SELECT DISTINCT dbo.Manu_ZirconDiamondFactoryMaster.CommandID as [رقـم الأمر] ,CASE WHEN dbo.Manu_ZirconDiamondFactoryMaster.DateBefore = 0 THEN '0' ELSE SUBSTRING(ltrim(str(DateBefore)), 1, 4) + '/' + SUBSTRING(ltrim(str(DateBefore)), 5, 2) + '/' + SUBSTRING(ltrim(str(DateBefore)), 7, 2) END as [تاريخ التسليم],CASE WHEN dbo.Manu_ZirconDiamondFactoryMaster.DateAfter = 0 THEN '0' ELSE SUBSTRING(ltrim(str(DateAfter)), 1, 4) + '/' + SUBSTRING(ltrim(str(DateAfter)), 5, 2) + '/' + SUBSTRING(ltrim(str(DateAfter)), 7, 2) END as [تاريخ الاستلام],dbo.Manu_ZirconDiamondFactoryMaster.OrderID [رقم الطلبية ] "
                                 + " ,Sales_Customers.ArbName as [اسم العميل],Sales_SalesDelegate.ArbName [اسم المندوب ] From dbo.Manu_ZirconDiamondFactoryMaster "
                                 + " inner join Manu_OrderRestriction  on Manu_ZirconDiamondFactoryMaster.OrderID= Manu_OrderRestriction.OrderID "
                                + " inner join Sales_Customers on Manu_OrderRestriction.CustomerID=Sales_Customers.AccountID "
                               + " inner join Sales_SalesDelegate on Manu_OrderRestriction.DelegateID=Sales_SalesDelegate.DelegateID "
                               + " WHERE  (dbo.Manu_ZirconDiamondFactoryMaster.Cancel = 0) and   Manu_ZirconDiamondFactoryMaster.BranchID =1 and Manu_ZirconDiamondFactoryMaster.TypeStageID=4 ";
                            }
                            else
                            {
                                cls.AddField("CommandID", "Commend ID");
                                cls.SQLStr = "SELECT DISTINCT dbo.Manu_ZirconDiamondFactoryMaster.CommandID as [Commend ID] ,CASE WHEN dbo.Manu_ZirconDiamondFactoryMaster.DateBefore = 0 THEN '0' ELSE SUBSTRING(ltrim(str(DateBefore)), 1, 4) + '/' + SUBSTRING(ltrim(str(DateBefore)), 5, 2) + '/' + SUBSTRING(ltrim(str(DateBefore)), 7, 2) END as [Date Before],CASE WHEN dbo.Manu_ZirconDiamondFactoryMaster.DateAfter = 0 THEN '0' ELSE SUBSTRING(ltrim(str(DateAfter)), 1, 4) + '/' + SUBSTRING(ltrim(str(DateAfter)), 5, 2) + '/' + SUBSTRING(ltrim(str(DateAfter)), 7, 2) END as [Date After],dbo.Manu_ZirconDiamondFactoryMaster.OrderID [Order ID] "
                                 + " ,Sales_Customers.ArbName as [Customer Name],Sales_SalesDelegate.ArbName [Delegete Name] From dbo.Manu_ZirconDiamondFactoryMaster "
                                 + " inner join Manu_OrderRestriction  on Manu_ZirconDiamondFactoryMaster.OrderID= Manu_OrderRestriction.OrderID "
                                + " inner join Sales_Customers on Manu_OrderRestriction.CustomerID=Sales_Customers.AccountID "
                               + " inner join Sales_SalesDelegate on Manu_OrderRestriction.DelegateID=Sales_SalesDelegate.DelegateID "
                               + " WHERE  (dbo.Manu_ZirconDiamondFactoryMaster.Cancel = 0) and   Manu_ZirconDiamondFactoryMaster.BranchID =1 and Manu_ZirconDiamondFactoryMaster.TypeStageID=4 ";

                            }
                            ColumnWidth = new int[] { 100, 150, 150, 80, 150, 150, 150, 150, 100, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 2;
                            break;
                        }
                    case "AfforestationCommendFactory":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("CommandID", "رقـم الأمر");
                                cls.SQLStr = "SELECT DISTINCT dbo.Manu_AfforestationFactoryMaster.CommandID as [رقـم الأمر] ,CASE WHEN dbo.Manu_AfforestationFactoryMaster.DateBefore = 0 THEN '0' ELSE SUBSTRING(ltrim(str(DateBefore)), 1, 4) + '/' + SUBSTRING(ltrim(str(DateBefore)), 5, 2) + '/' + SUBSTRING(ltrim(str(DateBefore)), 7, 2) END as [تاريخ التسليم],CASE WHEN dbo.Manu_AfforestationFactoryMaster.DateAfter = 0 THEN '0' ELSE SUBSTRING(ltrim(str(DateAfter)), 1, 4) + '/' + SUBSTRING(ltrim(str(DateAfter)), 5, 2) + '/' + SUBSTRING(ltrim(str(DateAfter)), 7, 2) END as [تاريخ الاستلام],dbo.Manu_AfforestationFactoryMaster.OrderID [رقم الطلبية ] "
                                 + " ,Sales_Customers.ArbName as [اسم العميل],Sales_SalesDelegate.ArbName [اسم المندوب ] From dbo.Manu_AfforestationFactoryMaster "
                                 + " inner join Manu_OrderRestriction  on Manu_AfforestationFactoryMaster.OrderID= Manu_OrderRestriction.OrderID "
                                + " inner join Sales_Customers on Manu_OrderRestriction.CustomerID=Sales_Customers.AccountID "
                               + " inner join Sales_SalesDelegate on Manu_OrderRestriction.DelegateID=Sales_SalesDelegate.DelegateID "
                               + " WHERE  (dbo.Manu_AfforestationFactoryMaster.Cancel = 0) and   Manu_AfforestationFactoryMaster.BranchID =1 and Manu_AfforestationFactoryMaster.TypeStageID=5 ";
                            }
                            else
                            {
                                cls.AddField("CommandID", "Commend ID");
                                cls.SQLStr = "SELECT DISTINCT dbo.Manu_AfforestationFactoryMaster.CommandID as [Commend ID] ,CASE WHEN dbo.Manu_AfforestationFactoryMaster.DateBefore = 0 THEN '0' ELSE SUBSTRING(ltrim(str(DateBefore)), 1, 4) + '/' + SUBSTRING(ltrim(str(DateBefore)), 5, 2) + '/' + SUBSTRING(ltrim(str(DateBefore)), 7, 2) END as [Date Before],CASE WHEN dbo.Manu_AfforestationFactoryMaster.DateAfter = 0 THEN '0' ELSE SUBSTRING(ltrim(str(DateAfter)), 1, 4) + '/' + SUBSTRING(ltrim(str(DateAfter)), 5, 2) + '/' + SUBSTRING(ltrim(str(DateAfter)), 7, 2) END as [Date After],dbo.Manu_AfforestationFactoryMaster.OrderID [Order ID] "
                                 + " ,Sales_Customers.ArbName as [Customer Name],Sales_SalesDelegate.ArbName [Delegete Name] From dbo.Manu_AfforestationFactoryMaster "
                                 + " inner join Manu_OrderRestriction  on Manu_AfforestationFactoryMaster.OrderID= Manu_OrderRestriction.OrderID "
                                + " inner join Sales_Customers on Manu_OrderRestriction.CustomerID=Sales_Customers.AccountID "
                               + " inner join Sales_SalesDelegate on Manu_OrderRestriction.DelegateID=Sales_SalesDelegate.DelegateID "
                               + " WHERE  (dbo.Manu_AfforestationFactoryMaster.Cancel = 0) and   Manu_AfforestationFactoryMaster.BranchID =1 and Manu_AfforestationFactoryMaster.TypeStageID=5 ";

                            }
                            ColumnWidth = new int[] { 100, 150, 150, 80, 150, 150, 150, 150, 100, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 2;
                            break;
                        }
                    case "AlcadCommend":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("CommandID", "رقـم الأمر");
                                cls.SQLStr = "SELECT DISTINCT dbo.Manu_AuxiliaryMaterialsMaster.CommandID as [رقـم الأمر] ,CASE WHEN dbo.Manu_AuxiliaryMaterialsMaster.CommandDate = 0 THEN '0' ELSE SUBSTRING(ltrim(str(CommandDate)), 1, 4) + '/' + SUBSTRING(ltrim(str(CommandDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(CommandDate)), 7, 2) END as [تاريخ الأمر],dbo.Sales_Customers.ArbName  AS  [اسم العميل]  "
                              + "  FROM    dbo.Manu_AuxiliaryMaterialsMaster INNER JOIN "
                              + "  dbo.Sales_Customers ON dbo.Manu_AuxiliaryMaterialsMaster.CustomerID = dbo.Sales_Customers.AccountID   "
                              + "  WHERE  (dbo.Manu_AuxiliaryMaterialsMaster.Cancel = 0) and Manu_AuxiliaryMaterialsMaster.TypeCommand=1 and Manu_AuxiliaryMaterialsMaster.BranchID = " + GlobalBranchID;

                            }
                            else
                            {
                                cls.AddField("CommandID", "Commend ID");
                                cls.SQLStr = "SELECT DISTINCT dbo.Manu_AuxiliaryMaterialsMaster.CommandID as [Command Date] ,CASE WHEN dbo.Manu_AuxiliaryMaterialsMaster.CommandDate = 0 THEN '0' ELSE SUBSTRING(ltrim(str(CommandDate)), 1, 4) + '/' + SUBSTRING(ltrim(str(CommandDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(CommandDate)), 7, 2) END as [Command Date],dbo.Sales_Customers.EngName  AS  [Customer Name] "
                             + "  FROM    dbo.Manu_AuxiliaryMaterialsMaster INNER JOIN "
                             + "  dbo.Sales_Customers ON dbo.Manu_AuxiliaryMaterialsMaster.CustomerID = dbo.Sales_Customers.AccountID  "
                             + "  WHERE  (dbo.Manu_AuxiliaryMaterialsMaster.Cancel = 0) and Manu_AuxiliaryMaterialsMaster.TypeCommand=1 and Manu_AuxiliaryMaterialsMaster.BranchID = " + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 100, 150, 150, 80, 150, 150, 150, 150, 100, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 2;
                            break;
                        }

                    case "ZericonCommend":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("CommandID", "رقـم الأمر");
                                cls.SQLStr = "SELECT DISTINCT dbo.Manu_AuxiliaryMaterialsMaster.CommandID as [رقـم الأمر] ,CASE WHEN dbo.Manu_AuxiliaryMaterialsMaster.CommandDate = 0 THEN '0' ELSE SUBSTRING(ltrim(str(CommandDate)), 1, 4) + '/' + SUBSTRING(ltrim(str(CommandDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(CommandDate)), 7, 2) END as [تاريخ الأمر],dbo.Sales_Customers.ArbName  AS  [اسم العميل]  "
                              + "  FROM    dbo.Manu_AuxiliaryMaterialsMaster INNER JOIN "
                              + "  dbo.Sales_Customers ON dbo.Manu_AuxiliaryMaterialsMaster.CustomerID = dbo.Sales_Customers.AccountID "
                              + "  WHERE  (dbo.Manu_AuxiliaryMaterialsMaster.Cancel = 0) and Manu_AuxiliaryMaterialsMaster.TypeCommand=2 and Manu_AuxiliaryMaterialsMaster.BranchID = " + GlobalBranchID;

                            }
                            else
                            {
                                cls.AddField("CommandID", "Commend ID");
                                cls.SQLStr = "SELECT DISTINCT dbo.Manu_AuxiliaryMaterialsMaster.CommandID as [Command Date] ,CASE WHEN dbo.Manu_AuxiliaryMaterialsMaster.CommandDate = 0 THEN '0' ELSE SUBSTRING(ltrim(str(CommandDate)), 1, 4) + '/' + SUBSTRING(ltrim(str(CommandDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(CommandDate)), 7, 2) END as [Command Date],dbo.Sales_Customers.EngName  AS  [Customer Name] "
                             + "  FROM    dbo.Manu_AuxiliaryMaterialsMaster INNER JOIN "
                             + "  dbo.Sales_Customers ON dbo.Manu_AuxiliaryMaterialsMaster.CustomerID = dbo.Sales_Customers.AccountID  "
                             + "  WHERE  (dbo.Manu_AuxiliaryMaterialsMaster.Cancel = 0) and Manu_AuxiliaryMaterialsMaster.TypeCommand=2 and Manu_AuxiliaryMaterialsMaster.BranchID = " + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 100, 150, 150, 80, 150, 150, 150, 150, 100, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 2;
                            break;
                        }
                    case "CastingCommend":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("CommandID", "رقـم الأمر");
                                cls.SQLStr = "SELECT DISTINCT dbo.Manu_ManufacturingCastingMaster.CommandID as [رقـم الأمر] ,CASE WHEN dbo.Manu_ManufacturingCastingMaster.CommandDate = 0 THEN '0' ELSE SUBSTRING(ltrim(str(CommandDate)), 1, 4) + '/' + SUBSTRING(ltrim(str(CommandDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(CommandDate)), 7, 2) END as [تاريخ الأمر],dbo.Sales_Customers.ArbName  AS  [اسم العميل] "
                              + "  FROM    dbo.Manu_ManufacturingCastingMaster INNER JOIN "
                              + "  dbo.Sales_Customers ON dbo.Manu_ManufacturingCastingMaster.CustomerID = dbo.Sales_Customers.AccountID  "
                              + "  WHERE  (dbo.Manu_ManufacturingCastingMaster.Cancel = 0) and   Manu_ManufacturingCastingMaster.BranchID = " + GlobalBranchID;

                            }
                            else
                            {
                                cls.AddField("CommandID", "Commend ID");
                                cls.SQLStr = "SELECT DISTINCT CommandID as [Commend ID],CommandDate as [Command Date]  FROM   Manu_ManufacturingCastingMaster WHERE Cancel=0  and BranchID = " + GlobalBranchID;
                                cls.SQLStr = "SELECT  dbo.Manu_ManufacturingCastingMaster.CommandID as [Command Date] ,CASE WHEN dbo.Manu_ManufacturingCastingMaster.CommandDate = 0 THEN '0' ELSE SUBSTRING(ltrim(str(CommandDate)), 1, 4) + '/' + SUBSTRING(ltrim(str(CommandDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(CommandDate)), 7, 2) END as [Command Date],dbo.Sales_Customers.EngName  AS  [Customer Name]  "
                              + "  FROM    dbo.Manu_ManufacturingCastingMaster INNER JOIN "
                              + "  dbo.Sales_Customers ON dbo.Manu_ManufacturingCastingMaster.CustomerID = dbo.Sales_Customers.AccountID  "
                              + "  WHERE  (dbo.Manu_ManufacturingCastingMaster.Cancel = 0) and  Manu_ManufacturingCastingMaster.BranchID = " + GlobalBranchID;
                            }
                            ColumnWidth = new int[] { 100, 150, 150, 80, 150, 150, 150, 150, 100, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 2;
                            break;
                        }

                    case "RepetID":
                        {
                            if (Language == iLanguage.Arabic)
                            {
                                cls.AddField("RepetID", "رقم تكرار الطلبية");
                                cls.SQLStr = "SELECT    max(dbo.Manu_ArrangingClosingOrders.ID  ) as [الرقم ],  dbo.Manu_ArrangingClosingOrders.OrderID as [رقم الطلبية],min( dbo.HR_EmployeeFile.ArbName) AS [ اسم العامل ] , dbo.Manu_ArrangingClosingOrders.RepetID AS [رقم تكرار الطلبية] "
                                  +" FROM            dbo.HR_EmployeeFile INNER JOIN"
                                +" dbo.Manu_ArrangingClosingOrders ON dbo.HR_EmployeeFile.BranchID = dbo.Manu_ArrangingClosingOrders.BranchID INNER JOIN "                        
                                +" dbo.Menu_FactoryRunCommandMaster ON dbo.HR_EmployeeFile.EmployeeID = dbo.Menu_FactoryRunCommandMaster.EmpFactorID AND "
                               +"  dbo.Manu_ArrangingClosingOrders.CommandID = dbo.Menu_FactoryRunCommandMaster.ComandID AND dbo.Manu_ArrangingClosingOrders.OrderID = dbo.Menu_FactoryRunCommandMaster.Barcode AND "
                               +"  dbo.Manu_ArrangingClosingOrders.StageID = dbo.Menu_FactoryRunCommandMaster.TypeStageID AND dbo.Manu_ArrangingClosingOrders.BranchID = dbo.Menu_FactoryRunCommandMaster.BranchID AND "
                                +" dbo.HR_EmployeeFile.BranchID = dbo.Menu_FactoryRunCommandMaster.BranchID"
						        +" where "+Condition
						        +" group by dbo.Manu_ArrangingClosingOrders.OrderID ,   dbo.Manu_ArrangingClosingOrders.RepetID";

                            }
                            else
                            {
                                cls.AddField("RepetID", "Repet ID");
                                cls.SQLStr = "SELECT    max(dbo.Manu_ArrangingClosingOrders.ID  ) as [ID],  dbo.Manu_ArrangingClosingOrders.OrderID as [Order ID],min( dbo.HR_EmployeeFile.ArbName) AS [Employee ID] , dbo.Manu_ArrangingClosingOrders.RepetID AS [Repet ID] "
                                     + " FROM            dbo.HR_EmployeeFile INNER JOIN"
                                   + " dbo.Manu_ArrangingClosingOrders ON dbo.HR_EmployeeFile.BranchID = dbo.Manu_ArrangingClosingOrders.BranchID INNER JOIN "
                                   + " dbo.Menu_FactoryRunCommandMaster ON dbo.HR_EmployeeFile.EmployeeID = dbo.Menu_FactoryRunCommandMaster.EmpFactorID AND "
                                  + "  dbo.Manu_ArrangingClosingOrders.CommandID = dbo.Menu_FactoryRunCommandMaster.ComandID AND dbo.Manu_ArrangingClosingOrders.OrderID = dbo.Menu_FactoryRunCommandMaster.Barcode AND "
                                  + "  dbo.Manu_ArrangingClosingOrders.StageID = dbo.Menu_FactoryRunCommandMaster.TypeStageID AND dbo.Manu_ArrangingClosingOrders.BranchID = dbo.Menu_FactoryRunCommandMaster.BranchID AND "
                                   + " dbo.HR_EmployeeFile.BranchID = dbo.Menu_FactoryRunCommandMaster.BranchID"
                                   + " where " + Condition
                                   + " group by dbo.Manu_ArrangingClosingOrders.OrderID ,   dbo.Manu_ArrangingClosingOrders.RepetID";
                            }
                            ColumnWidth = new int[] { 100, 150, 150, 80, 150, 150, 150, 150, 100, 100, 100, 100, 100, 100, 100 };
                            cls.SearchCol = 2;
                            break;
                        }

                }
            }
            catch (Exception ex)
            {
                //WT.msgError("Module1", System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        public static string SupplierInvoiceSqlStr(iLanguage Language, long SupplierID, int GlobalBranchID)
        {
            try
            {
                string strSQL;
                if (Language == iLanguage.Arabic)
                {
                    if (SupplierID != null/* TODO Change to default(_) if this is not a reference type */ )
                        strSQL = "SELECT dbo.Sales_PurchaseInvoiceMaster.InvoiceID AS [رقـم الـفـاتـورة], dbo.Sales_PurchaseInvoiceMaster.SupplierInvoiceID AS [رقـم فـاتـورة المورد], "
                    + " dbo.Sales_Suppliers.ArbName AS [اسـم المورد] , CASE WHEN dbo.Sales_PurchaseInvoiceMaster.InvoiceDate = 0 THEN '' ELSE SUBSTRING(ltrim(str(InvoiceDate)), 1, 4) "
                    + " + '/' + SUBSTRING(ltrim(str(InvoiceDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(InvoiceDate)), 7, 2) END AS [تاريخ الفاتورة] FROM dbo.Sales_PurchaseInvoiceMaster LEFT OUTER JOIN"
                    + " dbo.Sales_Suppliers ON dbo.Sales_PurchaseInvoiceMaster.BranchID = dbo.Sales_Suppliers.BranchID AND dbo.Sales_PurchaseInvoiceMaster.SupplierID = dbo.Sales_Suppliers.SupplierID"
                    + " WHERE  Sales_PurchaseInvoiceMaster.SupplierID =" + SupplierID + "  And Sales_PurchaseInvoiceMaster.Cancel =0 And Sales_PurchaseInvoiceMaster.InvoiceID <> 0 "
                    + " And  Sales_PurchaseInvoiceMaster.BranchID =" + GlobalBranchID;
                    else
                        strSQL = "SELECT dbo.Sales_PurchaseInvoiceMaster.InvoiceID AS [رقـم الـفـاتـورة], dbo.Sales_PurchaseInvoiceMaster.SupplierInvoiceID AS [رقـم فـاتـورة المورد], "
                    + " dbo.Sales_Suppliers.ArbName AS [اسـم المورد],CASE WHEN dbo.Sales_PurchaseInvoiceMaster.InvoiceDate = 0 THEN '' ELSE SUBSTRING(ltrim(str(InvoiceDate)), 1, 4) "
                    + " + '/' + SUBSTRING(ltrim(str(InvoiceDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(InvoiceDate)), 7, 2) END AS [تاريخ الفاتورة] FROM dbo.Sales_PurchaseInvoiceMaster LEFT OUTER JOIN"
                    + " dbo.Sales_Suppliers ON dbo.Sales_PurchaseInvoiceMaster.BranchID = dbo.Sales_Suppliers.BranchID AND dbo.Sales_PurchaseInvoiceMaster.SupplierID = dbo.Sales_Suppliers.SupplierID"
                    + " WHERE  Sales_PurchaseInvoiceMaster.Cancel =0 And Sales_PurchaseInvoiceMaster.InvoiceID <> 0 And  Sales_PurchaseInvoiceMaster.BranchID =" + GlobalBranchID;
                }
                else if (SupplierID != null/* TODO Change to default(_) if this is not a reference type */ )
                    strSQL = "SELECT dbo.Sales_PurchaseInvoiceMaster.InvoiceID AS [Invoice ID], dbo.Sales_PurchaseInvoiceMaster.SupplierInvoiceID AS [Supplier Invoice ID], "
                    + " dbo.Sales_Suppliers.ArbName AS [Supplier Name] ,CASE WHEN dbo.Sales_PurchaseInvoiceMaster.InvoiceDate = 0 THEN '' ELSE SUBSTRING(ltrim(str(InvoiceDate)), 1, 4) "
                    + " + '/' + SUBSTRING(ltrim(str(InvoiceDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(InvoiceDate)), 7, 2) ENDAS [Invoice Date]  FROM dbo.Sales_PurchaseInvoiceMaster LEFT OUTER JOIN"
                    + " dbo.Sales_Suppliers ON dbo.Sales_PurchaseInvoiceMaster.BranchID = dbo.Sales_Suppliers.BranchID AND dbo.Sales_PurchaseInvoiceMaster.SupplierID = dbo.Sales_Suppliers.SupplierID"
                    + " WHERE  Sales_PurchaseInvoiceMaster.SupplierID =" + SupplierID + "  And Sales_PurchaseInvoiceMaster.Cancel =0 And Sales_PurchaseInvoiceMaster.InvoiceID <> 0 "
                    + " And  Sales_PurchaseInvoiceMaster.BranchID =" + GlobalBranchID;
                else
                    strSQL = "SELECT dbo.Sales_PurchaseInvoiceMaster.InvoiceID AS [Invoice ID], dbo.Sales_PurchaseInvoiceMaster.SupplierInvoiceID AS [Supplier Invoice ID], "
                    + " dbo.Sales_Suppliers.ArbName AS [Supplier Name] ,CASE WHEN dbo.Sales_PurchaseInvoiceMaster.InvoiceDate = 0 THEN '' ELSE SUBSTRING(ltrim(str(InvoiceDate)), 1, 4) "
                    + " + '/' + SUBSTRING(ltrim(str(InvoiceDate)), 5, 2) + '/' + SUBSTRING(ltrim(str(InvoiceDate)), 7, 2) ENDAS [Invoice Date]  FROM dbo.Sales_PurchaseInvoiceMaster LEFT OUTER JOIN"
                    + " dbo.Sales_Suppliers ON dbo.Sales_PurchaseInvoiceMaster.BranchID = dbo.Sales_Suppliers.BranchID AND dbo.Sales_PurchaseInvoiceMaster.SupplierID = dbo.Sales_Suppliers.SupplierID"
                    + " WHERE  Sales_PurchaseInvoiceMaster.Cancel =0 And Sales_PurchaseInvoiceMaster.InvoiceID <> 0 And  Sales_PurchaseInvoiceMaster.BranchID =" + GlobalBranchID;

                // WT.ConvertStrSQLToEnglishOrArabicLanguage(strSQL);
                return strSQL;

            }
            catch (Exception ex)
            {
                //WT.msgError("Module1", System.Reflection.MethodBase.GetCurrentMethod().Name);
                return "";
            }
        }
        public static void SearchForAccounts(Control IDCtrl, Control NameCtrl, int BRANCHID)
        {
            try
            {
                CSearch cls = new CSearch();
                int[] ColumnWidth = new int[] { 100, 450 };

                PrepareSearchScreen("AccountID", ref cls, ref ColumnWidth, UserInfo.Language, BRANCHID);
                if (cls.SQLStr != "")
                {
                    frmSearch frm = new frmSearch();
                    frm.AddSearchData(cls);
                    frm.ColumnWidth = ColumnWidth;
                    cls.PrimaryKeyName = "AccountID";
                    cls.strFilter = "رقم الحساب";
                    cls.strArbNameValue = "رقم الحساب";
                    frm.ShowDialog();
                    if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
                    {
                        if (Lip.CheckTheAccountIsStope(Comon.cDbl(cls.PrimaryKeyValue), Comon.cInt(MySession.GlobalBranchID)))
                        {
                            Messages.MsgWarning(Messages.TitleWorning, Messages.msgAccountIsStope);
                            cls.PrimaryKeyValue = "";

                        }
                        IDCtrl.Text = (cls.PrimaryKeyValue.Trim());
                        NameCtrl.Text = (cls.PrimaryKeyName.Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                // WT.msgError("Module1", System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }


        public static void SearchForAccounts(Control IDCtrl, Control NameCtrl)
        {
            try
            {
                CSearch cls = new CSearch();
                int[] ColumnWidth = new int[] { 100, 450 };

                PrepareSearchScreen("AccountID", ref cls, ref ColumnWidth, UserInfo.Language, UserInfo.BRANCHID);
                if (cls.SQLStr != "")
                {
                    frmSearch frm = new frmSearch();
                    frm.AddSearchData(cls);
                    frm.ColumnWidth = ColumnWidth;
                    cls.PrimaryKeyName = "AccountID";
                    cls.strFilter = "رقم الحساب";
                    cls.strArbNameValue = "اسم الحسـاب";
                    frm.ShowDialog();
                    if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
                    {
                        if (Lip.CheckTheAccountIsStope(Comon.cDbl(cls.PrimaryKeyValue), Comon.cInt(MySession.GlobalBranchID)))
                        {
                            Messages.MsgWarning(Messages.TitleWorning, Messages.msgAccountIsStope);
                            cls.PrimaryKeyValue = "";

                        }
                        IDCtrl.Text = (cls.PrimaryKeyValue.Trim());
                        NameCtrl.Text = (cls.PrimaryKeyName.Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                // WT.msgError("Module1", System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        public static void Search(Control IDCtrl, Control NameCtrl, string PrimaryKeyName, string strFilter)
        {
            try
            {
                CSearch cls = new CSearch();
                int[] ColumnWidth = new int[] { 100, 450 };

                PrepareSearchScreen("AccountID", ref cls, ref ColumnWidth, UserInfo.Language, 1);
                if (cls.SQLStr != "")
                {
                    frmSearch frm = new frmSearch();
                    frm.AddSearchData(cls);
                    frm.ColumnWidth = ColumnWidth;
                    cls.PrimaryKeyName = PrimaryKeyName;
                    cls.strFilter = strFilter;
                    frm.ShowDialog();
                    if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
                    {
                        if (Lip.CheckTheAccountIsStope(Comon.cDbl(cls.PrimaryKeyValue), Comon.cInt(MySession.GlobalBranchID)))
                        {
                            Messages.MsgWarning(Messages.TitleWorning, Messages.msgAccountIsStope);
                            cls.PrimaryKeyValue = "";

                        }
                        IDCtrl.Text = (cls.PrimaryKeyValue.Trim());
                        NameCtrl.Text = (cls.PrimaryKeyName.Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                // WT.msgError("Module1", System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        public static void Find(ref CSearch cls, Control IDCtrl, Control NameCtrl, string PrimaryKeyName, string strFilter, int BranchID, string Condition = "")
        {
            try
            {

                int[] ColumnWidth = new int[] { 100, 450 };
                PrepareSearchScreen(PrimaryKeyName, ref cls, ref ColumnWidth, UserInfo.Language, BranchID, Condition);
                
                if (cls.SQLStr != "")
                {
                    frmSearch frm = new frmSearch();
                    frm.AddSearchData(cls);
                    frm.ColumnWidth = ColumnWidth;

                    cls.PrimaryKeyName = PrimaryKeyName;
                    cls.strFilter = strFilter;
                    cls.strArbNameValue = strFilter;
                   
                    frm.ShowDialog();
                    if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
                    {
                        if (Lip.CheckTheAccountIsStope(Comon.cDbl(cls.PrimaryKeyValue), Comon.cInt(MySession.GlobalBranchID)))
                        {
                            Messages.MsgWarning(Messages.TitleWorning, Messages.msgAccountIsStope);
                            cls.PrimaryKeyValue = "";

                        }
                        if (IDCtrl != null)
                            IDCtrl.Text = (cls.PrimaryKeyValue.Trim());
                         
                    }
                    
                }
            }
            catch (Exception ex)
            {
             Messages.MsgError(Messages.TitleError,System.Reflection.MethodBase.GetCurrentMethod().Name+", "+ex.Message + "");
                 //WT.msgError("Module1", System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        public static void Search(Control IDCtrl, Control NameCtrl, string P_PrimaryKeyName, string P_PrimaryKeyField, string P_strFilter, int BranchID = 1)
        {
            try
            {
                CSearch cls = new CSearch();
                int[] ColumnWidth = new int[] { 100, 450 };
                if (BranchID == 1)
                    BranchID = UserInfo.BRANCHID;

                PrepareSearchScreen(P_PrimaryKeyName, ref cls, ref ColumnWidth, UserInfo.Language, BranchID);
                if (cls.SQLStr != "")
                {
                    frmSearch frm = new frmSearch();
                    frm.AddSearchData(cls);
                    frm.ColumnWidth = ColumnWidth;
                    cls.PrimaryKeyName = P_PrimaryKeyName;
                    cls.strFilter = P_strFilter;
                    cls.PrimaryKeyField = P_PrimaryKeyField;

                    frm.ShowDialog();
                    if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
                    {
                        if (Lip.CheckTheAccountIsStope(Comon.cDbl(cls.PrimaryKeyValue), Comon.cInt(MySession.GlobalBranchID)))
                        {
                            Messages.MsgWarning(Messages.TitleWorning, Messages.msgAccountIsStope);
                            cls.PrimaryKeyValue = "";

                        }
                        IDCtrl.Text = (cls.PrimaryKeyValue.Trim());
                        NameCtrl.Text = (cls.PrimaryKeyName.Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                // WT.msgError("Module1", System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }


    }
}

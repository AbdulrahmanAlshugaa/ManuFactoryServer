using Edex.DAL;
using Edex.DAL.Common;
using Edex.DAL.Popup;
using Edex.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Edex.DAL
{
    public class ProductModel
    {
        //Here your other model properties. There is a advantage using viewmodel instead of passing data model directly to page.
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Department { get; set; }

        //pagination
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int PagerCount { get; set; }
        public List<InboxMessages> MailSent { get; set; }
        public List<InboxMessages> MailInbox { get; set; }

        public List<UserPermissions> UserPermissions { get; set; }
        public List<UserRoles> UserRoles { get; set; }
     
        public List<Users> Users { get; set; }
        public List<Acc_SpendVoucherMaster> SpendVoucherMaster { get; set; }
        public List<Sales_PurchaseInvoiceReturnMaster> PurchaseInvoiceReturnMaster { get; set; }
        public List<Sales_SalesInvoiceReturnMaster> SaleInvoiceReturnMaster { get; set; }
        public List<Sales_SalesInvoiceMaster> SaleInvoiceMaster { get; set; }
        public List<Sales_PurchaseInvoiceMaster> PurchaseInvoiceMaster { get; set; }
        public List<StartNumbering> StartNumbering { get; set; }
        public List<Acc_DeclaringIncomeAccounts> DeclaringIncomeAccounts { get; set; }
        public List<Acc_CostCenters> AccountsCostCenters { get; set; }
        public List<GeneralSettings> GeneralSettings { get; set; }
        public List<CompanyHeader> CompanyHeader { get; set; }
        public List<Acc_Currency> Currency { get; set; }
        public List<ClassManyFields> Popup { get; set; }
        public List<Acc_DeclaringMainAccounts> DeclaringMainAccounts { get; set; }
        public List<Branches> Branches { get; set; }
        public List<Sales_Sellers> Sales_Sellers { get; set; }
        public List<Sales_PurchasesDelegate> Sales_PurchasesDelegate { get; set; }
        public List<Sales_SalesDelegate> Sales_SalesDelegate { get; set; }
        public List<Sales_PurchaseGroupSuppliers> Sales_PurchaseGroupSuppliers { get; set; }
        public List<Sales_SalesGroupCustomers> Sales_SalesGroupCustomers { get; set; }
        public List<Sales_Suppliers> Sales_Suppliers { get; set; }
        public List<Sales_Customers> Sales_Customers { get; set; }
        public List<Stc_SizingUnits> Stc_SizingUnits { get; set; }
        public List<Stc_ItemsGroups> Stc_ItemsGroups { get; set; }
        public List<Stc_ItemTypes> Stc_ItemTypes { get; set; }
        public List<Stc_Stores> Stc_Stores { get; set; }
        public List<Sales_SalesInvoiceMaster> Sales_SalesInvoiceMaster { get; set; }
        public List<Sales_PurchaseInvoiceMaster> Sales_PurchaseInvoiceMaster { get; set; }
        public List<Sales_PurchaseInvoiceReturnMaster> Sales_PurchaseInvoiceReturnMaster { get; set; }
        public List<Sales_SalesInvoiceReturnMaster> Sales_SalesInvoiceReturnMaster { get; set; }
        public List<Stc_Items> Stc_Items { get; set; }
        public List<Stc_ItemsSizes> Stc_ItemsSizes { get; set; }

        public List<SalseInvoicesReport> SalseInvoicesReport { get; set; }
        public List<Acc_CheckSpendVoucherMaster> CheckSpendVoucherMaster { get; set; }
        public List<Acc_ReceiptVoucherMaster> ReceiptVoucherMaster { get; set; }
        public List<Acc_CheckReceiptVoucherMaster> CheckReceiptVoucherMaster { get; set; }
        public List<Acc_VariousVoucherMaster> VariousVoucherMaster { get; set; }


    }


}

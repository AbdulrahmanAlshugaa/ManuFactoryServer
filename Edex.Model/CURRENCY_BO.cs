

namespace Edex.Model
{
    using System;
    using System.Collections.Generic;

    public partial class CURRENCY_BO
    {
        public readonly string TableName = "Acc_Currency";
        public readonly string PremaryKey = "ID";
        public bool FoundResult { get; set; }
        public int ID { get; set; }
        public string ARBNAME { get; set; }
        public string ENGNAME { get; set; }
        public string NOTES { get; set; }
        public string CurrncyPart { get; set; }
        public string CodeCurrency { get; set; }
        public decimal TransPricing { get; set; }
        public decimal MaxTransPricing { get; set; }
        public decimal MinTransPricing { get; set; }
        public int StoreCurrency { get; set; }
        public int TypeCurrency { get; set; }
        public int USERCREATED { get; set; }
        public int USERUPDATED { get; set; }
 
        public long DATECREATED { get; set; }
        public long DATEUPDATED { get; set; }
 
        public int CREATEDTIME { get; set; }
        public int UPDATEDTIME { get; set; }
   
        public int BranchID { get; set; }
        public int FacilityID { get; set; }
        public string ComputerInfo { get; set; }
        public string EditComputerInfo { get; set; }
        public int TIMECREATED { get; set; }
        public int TIMEUPDATED { get; set; }
        public int Cancel { get; set; }
        public int TAFQEETID { get; set; }
        public CURRENCY_BO()
        {
            ID = 0;
            ARBNAME = "";
            ENGNAME = "";
            NOTES = "";

            BranchID = 0;
            USERCREATED = 0;
            USERUPDATED = 0;
            
            DATECREATED = 0;
            DATEUPDATED = 0;
           
            CREATEDTIME = 0;
            UPDATEDTIME = 0;
            Cancel = 0;
            ComputerInfo = "";
            EditComputerInfo = "";
            CurrncyPart = "";
            CodeCurrency = "";
            TransPricing = 0;
            MaxTransPricing = 0;
            MinTransPricing = 0;
            StoreCurrency = 0;
            TypeCurrency = 0;
        }

       
    }

    public partial class UserPermatiomForTreeAccount_BO
    {
        
        public int UserID { get; set; }
        public string ARBNAME { get; set; }
        public string ENGNAME { get; set; }

        public string CurrncyID { get; set; }
        public string CurrncyCode { get; set; }
        public string CurrncyName { get; set; }
   
      
        public bool Add { get; set; }
        public bool View { get; set; }
        public UserPermatiomForTreeAccount_BO()
        {
            UserID = 0;
            ARBNAME = "";
            ENGNAME = "";
            
            Add = false;
            View = false;
            

        }
    }


}

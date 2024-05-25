﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.Model
{
     
    public partial class Sales_SalesOrderDetails
    {
        public int ID { get; set; }
        public int OrderID { get; set; }
        public string BarCode { get; set; }

        public string Serials { get; set; }
        public int ItemID { get; set; }
        public string ArbItemName { get; set; }
        public string EngItemName { get; set; }

        public string Description { get; set; }

        public int GroupID { get; set; }

        public string ArbGroupName { get; set; }
        public string EngGroupName { get; set; }

        public string Color { get; set; }
        public string CLARITY { get; set; }


        public int SizeID { get; set; }
        public string ArbSizeName { get; set; }
        public string EngSizeName { get; set; }
        public string DateFirst { get; set; }
        public string ExpiryDate { get; set; }


        public decimal QTY { get; set; }
        public int TheCount { get; set; }

        public decimal DIAMOND_W { get; set; }
        public decimal STONE_W { get; set; }

        public decimal BAGET_W { get; set; }

        public decimal Bones { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SalePrice { get; set; }

        public decimal SpendPrice { get; set; }
        public decimal CaratPrice { get; set; }



        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public decimal AdditionalValue { get; set; }
        public decimal Net { get; set; }
        public bool HavVat { get; set; }

        public int Caliber { get; set; }
        public decimal Equivalen { get; set; }

        public decimal PackingQty { get; set; }

        public double Height { get; set; }
        public double Width { get; set; }



        public int Cancel { get; set; }

        public double StoreID { get; set; }



        public int BranchID { get; set; }
        public int FacilityID { get; set; }
        public long ExpiryDateStr { get; set; }
        public long DateFirstStr { get; set; }


        public float RemainQty { get; set; }
        public byte[] ItemImage { get; set; }
        public int CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public double CurrencyPrice { get; set; }
        public double CurrencyEquivalent { get; set; }
        public Sales_SalesOrderMaster SaleMaster { get; set; }
        public Sales_SalesOrderDetails()
        {

            ID = 0;
            ItemID = 0;
            OrderID = 0;
            BarCode = "";
            EngItemName = "";
            ArbItemName = "";
            SizeID = 1;
            ArbSizeName = "";
            EngSizeName = "";
            QTY = 1;
            PackingQty = 0;
            Bones = 0;
            Height = 0;
            Width = 0;
            TheCount = 2;
            Serials = "";
            SalePrice = 0;
            CostPrice = 0;
            Equivalen = 0;
            Caliber = 0;
            Total = 0;
            Discount = 0;
            AdditionalValue = 0;
            Net = 0;
            BAGET_W = 0;
            STONE_W = 0;
            DIAMOND_W = 0;
            FacilityID = 1;
            StoreID = 1;
            Cancel = 0;
            HavVat = true;
            Description = "";
            ExpiryDate = "";
            RemainQty = 0;
            ItemImage = null;
        }
    }
}

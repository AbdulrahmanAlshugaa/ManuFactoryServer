using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.Model
{
    public class PriceItemsOffers
    {
        public int OfferID { get; set; }

        public int OrderType { get; set; }
        public int ISRepeat { get; set; }
        
        public string Description { get; set; }

        public int FromGroupID { get; set; }

        public int ToGroupID { get; set; }

        public int FromItemID { get; set; }

        public int ToItemID { get; set; }
        public int FromSizeID { get; set; }

        public int ToSizeID { get; set; }


        public int FromItemID1 { get; set; }

        public int ToItemID1 { get; set; }
        public int FromSizeID1 { get; set; }

        public int ToSizeID1 { get; set; }







        public long FromDate { get; set; }

        public long ToDate { get; set; }

        public long FromTime { get; set; }

        public long ToTime { get; set; }

        public int IsAmount { get; set; }

        public int IsPercent { get; set; }

        public int IsOffers { get; set; }

        public long PercentCost { get; set; }

        public long AmountCost { get; set; }

        public int IsTakeOne { get; set; }

        public int IsGetSame { get; set; }

        public int IsGetOnther { get; set; }

        public int QTY { get; set; }

        public string BarCode { get; set; }

        public long GetSameAmount { get; set; }

        public long SetSameAmount { get; set; }

        public long GetOntherAmount { get; set; }

        public long SetOntherAmount { get; set; }

    }
}

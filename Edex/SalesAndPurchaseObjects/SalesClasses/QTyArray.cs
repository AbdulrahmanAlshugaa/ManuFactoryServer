using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.SalesAndPurchaseObjects.SalesClasses
{
    public class QTyArray
    {

        public decimal QTYIN = 0;
        public decimal IsInOfferQty = 0;
        public decimal QtyForOffers = 0;
        public int indexrow = 0;
        public decimal QtyINRow = 0;

        public QTyArray(decimal QTYIN, decimal IsInOfferQty, decimal QtyForOffers, int indexrow, decimal QtyINRow)
        {

            this.QTYIN = QTYIN;
            this.IsInOfferQty = IsInOfferQty;
            this.QtyForOffers = QtyForOffers;
            this.indexrow = indexrow;
            this.QtyINRow = QtyINRow;
        
        }
    }
}

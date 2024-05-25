using Edex.ModelSystem;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.StockObjects.StoresClasses
{
    class cCheckIfItemWithUnitExist
    {
        string strSQL;
        DataTable dt;
        public bool PurchaseInvoiceUnit(long ItemId, long UnitID)
        {

            try
            {
                strSQL = "SELECT InvoiceID FROM Sales_PurchaseInvoiceDetails WHERE"
                    + "(ItemID = " + ItemId + ") AND (SizeID = " + UnitID + ") ";
                dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count > 0)
                    dt = null;
                return true;

            }

            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            return false;



        }
        public bool PurchaseInvoiceReturnUnit(long ItemId, long UnitID)
        {

            try
            {
                strSQL = "SELECT InvoiceID FROM Sales_PurchaseInvoiceReturnDetails WHERE "
                    + "(ItemID = " + ItemId + ") AND (SizeID = " + UnitID + ") ";
                dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count > 0)
                    dt = null;
                return true;

            }

            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            return false;



        }
        public bool SalesInvoiceUnit(long ItemId, long UnitID)
        {

            try
            {
                strSQL = "SELECT InvoiceID FROM Sales_SalesInvoiceDetails WHERE "
                    + "(ItemID = " + ItemId + ") AND (SizeID = " + UnitID + ") ";
                dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count > 0)
                    dt = null;
                return true;

            }

            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            return false;



        }
        public bool SalesInvoiceReturnUnit(long ItemId, long UnitID)
        {

            try
            {
                strSQL = "SELECT InvoiceID FROM Sales_SalesInvoiceReturnDetails WHERE "
                    + "(ItemID = " + ItemId + ") AND (SizeID = " + UnitID + ") ";
                dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count > 0)
                    dt = null;
                return true;

            }

            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            return false;



        }
        public bool SpecialOffersUnit(long ItemId, long UnitID)
        {

            try
            {
                strSQL = "SELECT OfferID FROM Sales_SpecialOffersDetails WHERE "
                    + "(ItemID = " + ItemId + ") AND (SizeID = " + UnitID + ") ";
                dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count > 0)
                    dt = null;
                return true;

            }

            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            return false;



        }
        public bool ItemsTransferUnit(long ItemId, long UnitID)
        {

            try
            {
                strSQL = "SELECT ID FROM Stc_ItemsTransferDetails WHERE "
                    + "(ItemID = " + ItemId + ") AND (SizeID = " + UnitID + ") ";
                dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count > 0)
                    dt = null;
                return true;

            }

            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            return false;



        }
        public bool ItemsDismantlingSizeIDUnit(long ItemId, long UnitID)
        {

            try
            {
                strSQL = "SELECT DismantleID FROM Stc_ItemsDismantlingDetails WHERE "
                    + "(ItemID = " + ItemId + ") AND (SizeID = " + UnitID + ") ";
                dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count > 0)
                    dt = null;
                return true;

            }

            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            return false;



        }
        public bool ItemsDismantlingAnotherSizeIDUnit(long ItemId, long UnitID)
        {

            try
            {
                strSQL = "SELECT DismantleID FROM Stc_ItemsDismantlingDetails WHERE "
                    + "(ItemID = " + ItemId + ") AND (SizeID = " + UnitID + ") ";
                dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count > 0)
                    dt = null;
                return true;

            }

            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            return false;



        }


    }
}

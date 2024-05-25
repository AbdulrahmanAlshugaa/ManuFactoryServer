using Edex.ModelSystem;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.StockObjects.StoresClasses
{
    class cCheckStores
    {
        string strSQL = "";
        public bool PurchaseInvoiceUnit(long BarCode)
        {
            try
            {
                strSQL = "SELECT InvoiceID FROM Sales_PurchaseInvoiceDetails WHERE StoreID =" + BarCode + " And InvoiceID > = 0";
                DataTable dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count > 0)
                    return true;

            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            return false;

        }
        public bool PurchaseInvoiceReturnUnit(long BarCode)
        {
            try
            {
                strSQL = "SELECT InvoiceID FROM dbo.Sales_PurchaseInvoiceReturnDetails WHERE StoreID =" + BarCode + " ";
                DataTable dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count > 0)
                    return true;

            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            return false;

        }
        public bool SalesInvoiceUnit(long BarCode)
        {
            try
            {
                strSQL = "SELECT InvoiceID FROM dbo.Sales_SalesInvoiceDetails WHERE StoreID =" + BarCode + " ";
                DataTable dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count > 0)
                    return true;

            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            return false;

        }
        public bool SalesInvoiceReturnUnit(long BarCode)
        {
            try
            {
                strSQL = "SELECT  InvoiceID FROM dbo.Sales_SalesInvoiceReturnDetails WHERE StoreID =" + BarCode + "";
                DataTable dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count > 0)
                    return true;

            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            return false;

        }
        public bool SpecialOffersUnit(long BarCode)
        {
            try
            {
                strSQL = "SELECT OfferID FROM dbo.Sales_SpecialOffersDetails WHERE StoreID =" + BarCode + "";
                DataTable dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count > 0)
                    return true;

            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            return false;

        }
        public bool ItemsTransferUnit(long BarCode)
        {
            try
            {
                strSQL = "SELECT ID FROM dbo.Stc_ItemsTransferDetails WHERE FromStoreID =" + BarCode + "";
                DataTable dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count > 0)
                    return true;

            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            return false;

        }
        public bool ItemsDismantlingSizeIDUnit(long BarCode)
        {
            try
            {
                strSQL = "SELECT DismantleID FROM dbo.Stc_ItemsDismantlingDetails WHERE StoreID =" + BarCode + "";
                DataTable dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count > 0)
                    return true;

            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            return false;

        }
        public bool ItemsDismantlingAnotherSizeIDUnit(long BarCode)
        {
            try
            {
                strSQL = "SELECT DismantleID FROM dbo.Stc_ItemsDismantlingDetails WHERE StoreID =" + BarCode + "";
                DataTable dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count > 0)
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

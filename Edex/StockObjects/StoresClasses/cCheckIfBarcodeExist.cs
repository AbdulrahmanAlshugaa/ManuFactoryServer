using DevExpress.XtraEditors;
using Edex.DAL;
using Edex.Model;
using Edex.GeneralObjects.GeneralClasses;
using Edex.GeneralObjects.GeneralForms;
using Edex.ModelSystem;
using Edex.StockObjects.StoresClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Edex.Model.Language;
using Edex.DAL.Stc_itemDAL;

namespace Edex.StockObjects.StoresClasses
{      
    class cCheckIfBarcodeExist
    {
        string strSQL = "";
        public bool PurchaseInvoiceUnit(string BarCode) {
            try
            {
                strSQL = "SELECT InvoiceID FROM Sales_PurchaseInvoiceDetails WHERE BarCode ='" + BarCode + "' And InvoiceID  = -1";
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
        public bool PurchaseInvoiceReturnUnit(string BarCode)
        {
            try
            {
                strSQL = "SELECT InvoiceID FROM dbo.Sales_PurchaseInvoiceReturnDetails WHERE BarCode ='" + BarCode + "'";
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
        public bool SalesInvoiceUnit(string BarCode)
        {
            try
            {
                strSQL = "SELECT InvoiceID FROM dbo.Sales_SalesInvoiceDetails WHERE BarCode ='" + BarCode + "'";
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
        public bool SalesInvoiceReturnUnit(string BarCode)
        {
            try
            {
                strSQL = "SELECT  InvoiceID FROM dbo.Sales_SalesInvoiceReturnDetails WHERE BarCode ='" + BarCode + "'";
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
        public bool SpecialOffersUnit(string BarCode)
        {
            try
            {
                strSQL = "SELECT OfferID FROM dbo.Sales_SpecialOffersDetails WHERE BarCode ='" + BarCode + "'";
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
        public bool ItemsTransferUnit(string BarCode)
        {
            try
            {
                strSQL = "SELECT ID FROM dbo.Stc_ItemsTransferDetails WHERE BarCode ='" + BarCode + "'";
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
        public bool ItemsDismantlingSizeIDUnit(string BarCode)
        {
            try
            {
                strSQL = "SELECT DismantleID FROM dbo.Stc_ItemsDismantlingDetails WHERE FromBarCode ='" + BarCode + "'";
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
        public bool ItemsDismantlingAnotherSizeIDUnit(string BarCode)
        {
            try
            {
                strSQL = "SELECT DismantleID FROM dbo.Stc_ItemsDismantlingDetails WHERE ToBarCode ='" + BarCode + "'";
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

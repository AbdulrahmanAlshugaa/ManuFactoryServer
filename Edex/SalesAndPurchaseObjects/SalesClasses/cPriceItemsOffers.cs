using Edex.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Edex.SalesAndPurchaseObjects.SalesClasses
{
    public class cPriceItemsOffers
    {
        #region declare & Properties
        public readonly string TableName = "PriceItemsOffers";
        public readonly string PremaryKey = "OfferID";
        public bool FoundResult;
        public bool NeedSaving;
        public bool IsNewRecord;

        private DataTable dt;
        private string strSQL;
        private object Result;
        public int OfferID { get; set; }

        public int OrderType { get; set; }
        public int ISRepeat { get; set; }
        public string Description { get; set; }

        public int FromGroupID { get; set; }

        public int ToGroupID { get; set; }

        public int FromItemID { get; set; }

        public int FromSizeID { get; set; }
        public int ToSizeID { get; set; }

        public int ToItemID { get; set; }


        public int FromItemID1 { get; set; }

        public int FromSizeID1 { get; set; }
        public int ToSizeID1 { get; set; }

        public int ToItemID1 { get; set; }


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
        #endregion
       
        /// <summary>
        /// This function is used to read record from dataTable Object to variable & Properties
        /// </summary>
        private void ReadRecord()
        {
            try
            {
                {
                    //Set Values to variable & Properties
                    var withBlock = dt;
                    OfferID = Comon.cInt(dt.Rows[0]["OfferID"].ToString());

                    ISRepeat = Comon.cInt(dt.Rows[0]["ISRepeat"].ToString());
                    OrderType = Comon.cInt(dt.Rows[0]["OrderType"].ToString());
                    
                    Description = dt.Rows[0]["Description"].ToString();

                    FromGroupID = Comon.cInt(dt.Rows[0]["FromGroupID"].ToString());

                    ToGroupID = Comon.cInt(dt.Rows[0]["ToGroupID"].ToString());

                    FromItemID = Comon.cInt(dt.Rows[0]["FromItemID"].ToString());

                    ToItemID = Comon.cInt(dt.Rows[0]["ToItemID"].ToString());


                    FromSizeID = Comon.cInt(dt.Rows[0]["FromSizeID"].ToString());

                    ToSizeID = Comon.cInt(dt.Rows[0]["ToISizeID"].ToString());


                    FromItemID1 = Comon.cInt(dt.Rows[0]["ItemIDOnther"].ToString());

                    ToItemID1 = Comon.cInt(dt.Rows[0]["ItemIDOnther"].ToString());


                    FromSizeID1 = Comon.cInt(dt.Rows[0]["FromSizeOnther"].ToString());

                    ToSizeID1 = Comon.cInt(dt.Rows[0]["ToSizeOnther"].ToString());











                    FromDate = Comon.cLong(dt.Rows[0]["FromDate"].ToString());

                    ToDate = Comon.cLong(dt.Rows[0]["ToDate"].ToString());

                    FromTime = Comon.cLong(dt.Rows[0]["FromTime"].ToString());

                    ToTime = Comon.cLong(dt.Rows[0]["ToTime"].ToString());

                    IsAmount = Comon.cInt(dt.Rows[0]["IsAmount"].ToString());

                    IsPercent = Comon.cInt(dt.Rows[0]["IsPercent"].ToString());

                    IsOffers = Comon.cInt(dt.Rows[0]["IsOffers"].ToString());

                    PercentCost = Comon.cLong(dt.Rows[0]["PercentCost"].ToString());

                    AmountCost = Comon.cLong(dt.Rows[0]["AmountCost"].ToString());

                    IsTakeOne = Comon.cInt(dt.Rows[0]["IsTakeOne"].ToString());

                    IsGetSame = Comon.cInt(dt.Rows[0]["IsGetSame"].ToString());

                    IsGetOnther = Comon.cInt(dt.Rows[0]["IsGetOnther"].ToString());

                    QTY = Comon.cInt(dt.Rows[0]["QTY"].ToString());

                    BarCode = dt.Rows[0]["BarCode"].ToString();

                    GetSameAmount = Comon.cLong(dt.Rows[0]["GetSameAmount"].ToString());

                    SetSameAmount = Comon.cLong(dt.Rows[0]["SetSameAmount"].ToString());

                    GetOntherAmount = Comon.cLong(dt.Rows[0]["GetOntherAmount"].ToString());

                    SetOntherAmount = Comon.cLong(dt.Rows[0]["SetOntherAmount"].ToString());

                }
                FoundResult = true;
                IsNewRecord = false;
            }
            catch (Exception ex)
            {
                // Lip.msgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        /// <summary>
        /// This Function to Get data Price Items Offers   by OfferID
        /// </summary>
        /// <param name="PremaryKeyValue"></param>
        public void GetRecordSet(long PremaryKeyValue)
        {
            try
            {
                FoundResult = false;
                strSQL = "SELECT Top 1 * FROM " + TableName
                    + " WHERE  " + PremaryKey + "=" + PremaryKeyValue;
                dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count > 0)
                {
                    ReadRecord();
                    FoundResult = true;
                }
                dt = null;
            }
            catch (Exception ex)
            {
                // WT.msgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        /// <summary>
        /// This function to get record which  set by sql
        /// </summary>
        /// <param name="strSQL"></param>
        public void GetRecordSetBySQL(string strSQL)
        {
            try
            {
                FoundResult = false;
                dt = Lip.SelectRecord(strSQL);//Execute Select
                if (dt.Rows.Count > 0)
                {
                    ReadRecord();
                    FoundResult = true;
                }
                dt = null;
            }
            catch (Exception ex)
            {
                //WT.msgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        /// <summary>
        /// This functoin is used to get Max ID +1 to New ID
        /// </summary>
        /// <returns>return New ID by type long</returns>
        public long GetNewID()
        {
            try
            {
                DataTable dt;
                string strSQL;
                strSQL = "SELECT Max(" + PremaryKey + ") + 1 FROM " + TableName;
                dt = Lip.SelectRecord(strSQL);
                string GetNewID = dt.Rows[0][0] == DBNull.Value ? "1" : dt.Rows[0][0].ToString();
                return Convert.ToInt32(GetNewID);

            }
            catch (Exception ex)
            {
                return 0;
                // WT.msgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

    }
}

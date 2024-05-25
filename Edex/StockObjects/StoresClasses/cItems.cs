using Edex.Model;
using Edex.ModelSystem;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Edex.StockObjects.StoresClasses
{
    class cItems
    {
        public readonly string TableName = "Stc_Items";
        public readonly string PremaryKey = "ItemID";

        // Declare Table Fields
        public int SizeID;
        public string ArbName;
        public string EngName;
        public long ItemID;

        public double GroupID;
        public int TypeID;
        public string Notes;
        public int IsVAT;

        public int ColorID;
        public int BrandID;

        public int BaseID;
        public byte[] imgByte = null;


        public bool FoundResult;
        public bool NeedSaving;
        public bool IsNewRecord;

        private DataTable dt;
        private string strSQL;
        private object Result;
        public DataTable FillDataGrid(long ItemID)
        {
            try
            {

                strSQL = "SELECT Stc_ItemUnits.SizeID, Stc_SizingUnits.ArbName AS SizeName, Stc_ItemUnits.BarCode, Stc_ItemUnits.PackingQty, "
                          + " Stc_ItemUnits.CostPrice, Stc_ItemUnits.SalePrice, Stc_ItemUnits.MinLimitQty, Stc_ItemUnits.MaxLimitQty,"
                          + " Stc_ItemUnits.LastCostPrice, Stc_ItemUnits.LastSalePrice, Stc_ItemUnits.SpecialSalePrice, "
                          + "  Stc_ItemUnits.SpecialCostPrice, Stc_ItemUnits.ItemProfit,Stc_ItemUnits.AllowedPercentDiscount,Stc_ItemUnits.AverageCostPrice, Stc_ItemUnits.UnitCancel"
                          + " FROM  Stc_Items INNER JOIN Stc_ItemUnits ON Stc_Items.ItemID = Stc_ItemUnits.ItemID  "
                          + " LEFT OUTER JOIN Stc_SizingUnits ON "
                          + " Stc_ItemUnits.SizeID = Stc_SizingUnits.SizeID"
                          + " WHERE Stc_Items.ItemID = " + ItemID
                          + " AND Stc_Items.Cancel = 0 AND Stc_SizingUnits.Cancel = 0"
                          + " ORDER BY Stc_ItemUnits.ID ";

                // Lip.la(strSQL)
                dt = Lip.SelectRecord(strSQL);



            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            return dt;


        }
      


        private void ReadRecord()
        {
             

            try 
            
            {
                {
                    var withBlock = dt;
                    // BrandID = Comon.cInt(dt.Rows[0]["BrandID"].ToString());
                    // BaseID = Comon.cInt(dt.Rows[0]["BaseID"].ToString());
                    // SizeID = Comon.cInt(dt.Rows[0]["SizeID"].ToString());
                    ItemID = Comon.cLong(dt.Rows[0]["ItemID"].ToString());
                    ArbName = dt.Rows[0]["ArbName"].ToString();
                    IsVAT = Comon.cInt(dt.Rows[0]["IsVAT"].ToString());
                    EngName = dt.Rows[0]["EngName"].ToString();
                    GroupID = Comon.cDbl(dt.Rows[0]["GroupID"].ToString());
                    TypeID = Comon.cInt(dt.Rows[0]["TypeID"].ToString());
                    // ColorID = Comon.cInt(dt.Rows[0]["ColorID"].ToString());
                    Notes = dt.Rows[0]["Notes"].ToString();
                    if (DBNull.Value != dt.Rows[0]["ItemImage"])
                    {
                        imgByte = (byte[])dt.Rows[0]["ItemImage"];
                        if (imgByte.Length <= 0)
                        {
                            imgByte = null;


                        }
                    }


                }
                FoundResult = true;
                IsNewRecord = false;
            }
            catch (Exception ex)
            {
                // Lip.msgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        public void GetRecordSet(long PremaryKeyValue)
        {
            try
            {
                FoundResult = false;
                strSQL = "SELECT Top 1 * FROM " + TableName
                    + " WHERE Cancel =0 AND " + PremaryKey + "=" + PremaryKeyValue;
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

        public void GetRecordSetBySQL(string strSQL)
        {
            try
            {
                FoundResult = false;
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
                //WT.msgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        public long GetNewID()
        {
            try
            {
                DataTable dt;
                string strSQL;
                strSQL = "SELECT Max(" + PremaryKey + ") + 1 FROM " + TableName ;
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

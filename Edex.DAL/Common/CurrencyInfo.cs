using System;
using System.Data;

namespace Edex.DAL.Common
{
    public class CurrencyInfo
    {
        public enum Currencies { Syria = 0, UAE, SaudiArabia, Bahrain, Dolar, Gold, Qatar, Yemen, Kuwait };

        #region Constructors

        public CurrencyInfo(Currencies currency)
        {


            switch (currency)
            {
                //case Currencies.Syria:

                //    CurrencyID = 0;

                //    string StrSql = "Select * From ACC_CURRENCY Where TAFQEETID=" + CurrencyID;
                //    DataTable dt = new DataTable();
                //    dt = Lip.SelectRecord(StrSql);

                //    if (dt.Rows.Count > 0)
                //    {
                //        CurrencyCode = dt.Rows[0]["CurrencyCode"].ToString();
                //        IsCurrencyNameFeminine = true;
                //        EnglishCurrencyName = dt.Rows[0]["EnglishCurrencyName"].ToString();
                //        EnglishPluralCurrencyName = dt.Rows[0]["EnglishPluralCurrencyName"].ToString();
                //        EnglishCurrencyPartName = dt.Rows[0]["EnglishCurrencyPartName"].ToString();
                //        EnglishPluralCurrencyPartName = dt.Rows[0]["EnglishPluralCurrencyPartName"].ToString();
                //        Arabic1CurrencyName = dt.Rows[0]["Arabic1CurrencyName"].ToString();
                //        Arabic2CurrencyName = dt.Rows[0]["Arabic2CurrencyName"].ToString();
                //        Arabic310CurrencyName = dt.Rows[0]["Arabic310CurrencyName"].ToString();
                //        Arabic1199CurrencyName = dt.Rows[0]["Arabic1199CurrencyName"].ToString();
                //        Arabic1CurrencyPartName = dt.Rows[0]["Arabic1CurrencyPartName"].ToString();
                //        Arabic2CurrencyPartName = dt.Rows[0]["Arabic2CurrencyPartName"].ToString();
                //        Arabic310CurrencyPartName = dt.Rows[0]["Arabic310CurrencyPartName"].ToString();
                //        Arabic1199CurrencyPartName = dt.Rows[0]["Arabic1199CurrencyPartName"].ToString();
                //        PartPrecision = 2;
                //        IsCurrencyPartNameFeminine = false;
                //    }
                //    break;
                case Currencies.Syria:
                    CurrencyID = 0;
                    CurrencyCode = "SYR";
                    IsCurrencyNameFeminine = true;
                    EnglishCurrencyName = "Syrian Pound";
                    EnglishPluralCurrencyName = "Syrian Pounds";
                    EnglishCurrencyPartName = "Piaster";
                    EnglishPluralCurrencyPartName = "Piasters";
                    Arabic1CurrencyName = "ليرة سورية";
                    Arabic2CurrencyName = "ليرتين سوريتين";
                    Arabic310CurrencyName = "ليرات سورية";
                    Arabic1199CurrencyName = "ليرة سورية";
                    Arabic1CurrencyPartName = "قرش";
                    Arabic2CurrencyPartName = "قرشين";
                    Arabic310CurrencyPartName = "قروش";
                    Arabic1199CurrencyPartName = "قرشاً";
                    PartPrecision = 2;
                    IsCurrencyPartNameFeminine = false;
                    break;

                //case Currencies.UAE:
                //    CurrencyID = 1;
                //    StrSql = "Select * From ACC_CURRENCY Where TAFQEETID=" + CurrencyID;
                //    dt = new DataTable();
                //    dt = Lip.SelectRecord(StrSql);

                //    if (dt.Rows.Count > 0)
                //    {
                //        CurrencyCode = dt.Rows[0]["CurrencyCode"].ToString();
                //        IsCurrencyNameFeminine = false;
                //        EnglishCurrencyName = dt.Rows[0]["EnglishCurrencyName"].ToString();
                //        EnglishPluralCurrencyName = dt.Rows[0]["EnglishPluralCurrencyName"].ToString();
                //        EnglishCurrencyPartName = dt.Rows[0]["EnglishCurrencyPartName"].ToString();
                //        EnglishPluralCurrencyPartName = dt.Rows[0]["EnglishPluralCurrencyPartName"].ToString();
                //        Arabic1CurrencyName = dt.Rows[0]["Arabic1CurrencyName"].ToString();
                //        Arabic2CurrencyName = dt.Rows[0]["Arabic2CurrencyName"].ToString();
                //        Arabic310CurrencyName = dt.Rows[0]["Arabic310CurrencyName"].ToString();
                //        Arabic1199CurrencyName = dt.Rows[0]["Arabic1199CurrencyName"].ToString();
                //        Arabic1CurrencyPartName = dt.Rows[0]["Arabic1CurrencyPartName"].ToString();
                //        Arabic2CurrencyPartName = dt.Rows[0]["Arabic2CurrencyPartName"].ToString();
                //        Arabic310CurrencyPartName = dt.Rows[0]["Arabic310CurrencyPartName"].ToString();
                //        Arabic1199CurrencyPartName = dt.Rows[0]["Arabic1199CurrencyPartName"].ToString();

                //        PartPrecision = 2;
                //        IsCurrencyPartNameFeminine = false;
                //    }
                //    break;
                case Currencies.UAE:
                    CurrencyID = 1;
                    CurrencyCode = "AED";
                    IsCurrencyNameFeminine = true;
                    EnglishCurrencyName = "UAE Dirham";
                    EnglishPluralCurrencyName = "UAE Dirhams";
                    EnglishCurrencyPartName = "Fils";
                    EnglishPluralCurrencyPartName = "Fils";
                    Arabic1CurrencyName = "درهم إماراتي";
                    Arabic2CurrencyName = "درهمان إماراتيان";
                    Arabic310CurrencyName = "دراهم إماراتية";
                    Arabic1199CurrencyName = "درهمًا إماراتيًا";
                    Arabic1CurrencyPartName = "فلس إماراتي";
                    Arabic2CurrencyPartName = "فلسان إماراتيان";
                    Arabic310CurrencyPartName = "فلوس إماراتية";
                    Arabic1199CurrencyPartName = "فلسًا إماراتيًا";

                    PartPrecision = 2;
                    IsCurrencyPartNameFeminine = false;
                    break;

                //case Currencies.SaudiArabia:
                //    CurrencyID = 2;

                //    StrSql = "Select * From ACC_CURRENCY Where TAFQEETID=" + CurrencyID;
                //    dt = new DataTable();
                //    dt = Lip.SelectRecord(StrSql);
                //    if (dt.Rows.Count > 0)
                //    {
                //        dt = Lip.SelectRecord(StrSql);
                //        CurrencyCode = dt.Rows[0]["CurrencyCode"].ToString();
                //        IsCurrencyNameFeminine = false;
                //        EnglishCurrencyName = dt.Rows[0]["EnglishCurrencyName"].ToString();
                //        EnglishPluralCurrencyName = dt.Rows[0]["EnglishPluralCurrencyName"].ToString();
                //        EnglishCurrencyPartName = dt.Rows[0]["EnglishCurrencyPartName"].ToString();
                //        EnglishPluralCurrencyPartName = dt.Rows[0]["EnglishPluralCurrencyPartName"].ToString();
                //        Arabic1CurrencyName = dt.Rows[0]["Arabic1CurrencyName"].ToString();
                //        Arabic2CurrencyName = dt.Rows[0]["Arabic2CurrencyName"].ToString();
                //        Arabic310CurrencyName = dt.Rows[0]["Arabic310CurrencyName"].ToString();
                //        Arabic1199CurrencyName = dt.Rows[0]["Arabic1199CurrencyName"].ToString();
                //        Arabic1CurrencyPartName = dt.Rows[0]["Arabic1CurrencyPartName"].ToString();
                //        Arabic2CurrencyPartName = dt.Rows[0]["Arabic2CurrencyPartName"].ToString();
                //        Arabic310CurrencyPartName = dt.Rows[0]["Arabic310CurrencyPartName"].ToString();
                //        Arabic1199CurrencyPartName = dt.Rows[0]["Arabic1199CurrencyPartName"].ToString();

                //        PartPrecision = 2;
                //        IsCurrencyPartNameFeminine = true;
                //    }
                //    break;
                case Currencies.SaudiArabia:
                    CurrencyID = 2;

                    // Assign the desired values directly
                    CurrencyCode = "SAR";
                    IsCurrencyNameFeminine = false;
                    EnglishCurrencyName = "Saudi Arabian Riyal";
                    EnglishPluralCurrencyName = "Saudi Arabian Riyals";
                    EnglishCurrencyPartName = "Halala";
                    EnglishPluralCurrencyPartName = "Halalas";
                    Arabic1CurrencyName = "ريال سعودي";
                    Arabic2CurrencyName = "ريالان سعوديان";
                    Arabic310CurrencyName = "ريالات سعودية";
                    Arabic1199CurrencyName = "ريالًا سعوديًا";
                    Arabic1CurrencyPartName = "هللة";
                    Arabic2CurrencyPartName = "هللتان";
                    Arabic310CurrencyPartName = "هللات";
                    Arabic1199CurrencyPartName = "هللة";
                    PartPrecision = 2;
                    IsCurrencyPartNameFeminine = true;
                    break;


                case Currencies.Bahrain:
                    CurrencyID = 3;
                    CurrencyCode = "BHD";
                    IsCurrencyNameFeminine = false;
                    EnglishCurrencyName = "Bahraini Dinar";
                    EnglishPluralCurrencyName = "Bahraini Dinars";
                    EnglishCurrencyPartName = "fils";
                    EnglishPluralCurrencyPartName = "fils";
                    Arabic1CurrencyName = "دينار بحريني";
                    Arabic2CurrencyName = "ديناران بحرينيان";
                    Arabic310CurrencyName = "دنانير بحرينية";
                    Arabic1199CurrencyName = "ديناراً بحرينيا";
                    Arabic1CurrencyPartName = "فلس";
                    Arabic2CurrencyPartName = "فلسان";
                    Arabic310CurrencyPartName = "فلوس";
                    Arabic1199CurrencyPartName = "فلساً";
                    PartPrecision = 3;
                    IsCurrencyPartNameFeminine = false;
                    break;


                case Currencies.Dolar:
                    CurrencyID = 4;
                    CurrencyCode = "$";
                    IsCurrencyNameFeminine = false;
                    EnglishCurrencyName = "Dolar Amrica";
                    EnglishPluralCurrencyName = "Dolars Amrica";
                    EnglishCurrencyPartName = "Sent";
                    EnglishPluralCurrencyPartName = "Sents";
                    Arabic1CurrencyName = "دولار أمريكي";
                    Arabic2CurrencyName = "دولاران أمريكي";
                    Arabic310CurrencyName = "دولارات أمريكية";
                    Arabic1199CurrencyName = "دولارا أمريكيا";
                    Arabic1CurrencyPartName = "سنت";
                    Arabic2CurrencyPartName = "سنتان";
                    Arabic310CurrencyPartName = "سنتات";
                    Arabic1199CurrencyPartName = "سنتا";
                    PartPrecision = 2;
                    IsCurrencyPartNameFeminine = false;
                    break;


                case Currencies.Gold:
                    CurrencyID = 5;
                    CurrencyCode = "XAU";
                    IsCurrencyNameFeminine = false;
                    EnglishCurrencyName = "Gram";
                    EnglishPluralCurrencyName = "Grams";
                    EnglishCurrencyPartName = "Milligram";
                    EnglishPluralCurrencyPartName = "Milligrams";
                    Arabic1CurrencyName = "جرام";
                    Arabic2CurrencyName = "جرامان";
                    Arabic310CurrencyName = "جرامات";
                    Arabic1199CurrencyName = "جراماً";
                    Arabic1CurrencyPartName = "ملجرام";
                    Arabic2CurrencyPartName = "ملجرامان";
                    Arabic310CurrencyPartName = "ملجرامات";
                    Arabic1199CurrencyPartName = "ملجراماً";
                    PartPrecision = 2;
                    IsCurrencyPartNameFeminine = false;
                    break;
                case Currencies.Qatar:
                    CurrencyID = 6;
                    CurrencyCode = "QAR";
                    IsCurrencyNameFeminine = false;
                    EnglishCurrencyName = "Qatari Riyal";
                    EnglishPluralCurrencyName = "Qatari Riyals";
                    EnglishCurrencyPartName = "dirham";
                    EnglishPluralCurrencyPartName = "dirhams";
                    Arabic1CurrencyName = "ريال قطري";
                    Arabic2CurrencyName = "ريالان قطريان";
                    Arabic310CurrencyName = "ريالات قطرية";
                    Arabic1199CurrencyName = "ريالاً قطريا";
                    Arabic1CurrencyPartName = "درهم";
                    Arabic2CurrencyPartName = "درهمان";
                    Arabic310CurrencyPartName = "دراهم";
                    Arabic1199CurrencyPartName = "درهماً";
                    PartPrecision = 2;
                    IsCurrencyPartNameFeminine = false;
                    break;
                case Currencies.Yemen:
                    CurrencyID = 7;
                    CurrencyCode = "YER";
                    IsCurrencyNameFeminine = true;
                    EnglishCurrencyName = "Yemeni Rial";
                    EnglishPluralCurrencyName = "Yemeni Rials";
                    EnglishCurrencyPartName = "Fils";
                    EnglishPluralCurrencyPartName = "Fils";
                    Arabic1CurrencyName = "ريال يمني";
                    Arabic2CurrencyName = "ريالان يمنيان";
                    Arabic310CurrencyName = "ريالات يمنية";
                    Arabic1199CurrencyName = "ريالًا يمنيًا";
                    Arabic1CurrencyPartName = "فلس";
                    Arabic2CurrencyPartName = "فلسان";
                    Arabic310CurrencyPartName = "فلوس";
                    Arabic1199CurrencyPartName = "فلس";
                    PartPrecision = 2;
                    IsCurrencyPartNameFeminine = true;
                    break;
                case Currencies.Kuwait:
                    CurrencyID = 8;
                    CurrencyCode = "KWD";
                    IsCurrencyNameFeminine = true;
                    EnglishCurrencyName = "Kuwaiti Dinar";
                    EnglishPluralCurrencyName = "Kuwaiti Dinars";
                    EnglishCurrencyPartName = "Fils";
                    EnglishPluralCurrencyPartName = "Fils";
                    Arabic1CurrencyName = "دينار كويتي";
                    Arabic2CurrencyName = "ديناران كويتيان";
                    Arabic310CurrencyName = "دنانير كويتية";
                    Arabic1199CurrencyName = "دينارًا كويتيًا";
                    Arabic1CurrencyPartName = "فلس";
                    Arabic2CurrencyPartName = "فلسان";
                    Arabic310CurrencyPartName = "فلوس";
                    Arabic1199CurrencyPartName = "فلس";
                    PartPrecision = 2;
                    IsCurrencyPartNameFeminine = true;
                    break;

            }
        }

        

        #endregion

        #region Properties

        /// <summary>
        /// Currency ID
        /// </summary>
        public int CurrencyID { get; set; }

        /// <summary>
        /// Standard Code
        /// Syrian Pound: SYP
        /// UAE Dirham: AED
        /// </summary>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// Is the currency name feminine ( Mua'anath مؤنث)
        /// ليرة سورية : مؤنث = true
        /// درهم : مذكر = false
        /// </summary>
        public Boolean IsCurrencyNameFeminine { get; set; }

        /// <summary>
        /// English Currency Name for single use
        /// Syrian Pound
        /// UAE Dirham
        /// </summary>
        public string EnglishCurrencyName { get; set; }

        /// <summary>
        /// English Plural Currency Name for Numbers over 1
        /// Syrian Pounds
        /// UAE Dirhams
        /// </summary>
        public string EnglishPluralCurrencyName { get; set; }

        /// <summary>
        /// Arabic Currency Name for 1 unit only
        /// ليرة سورية
        /// درهم إماراتي
        /// </summary>
        public string Arabic1CurrencyName { get; set; }

        /// <summary>
        /// Arabic Currency Name for 2 units only
        /// ليرتان سوريتان
        /// درهمان إماراتيان
        /// </summary>
        public string Arabic2CurrencyName { get; set; }

        /// <summary>
        /// Arabic Currency Name for 3 to 10 units
        /// خمس ليرات سورية
        /// خمسة دراهم إماراتية
        /// </summary>
        public string Arabic310CurrencyName { get; set; }

        /// <summary>
        /// Arabic Currency Name for 11 to 99 units
        /// خمس و سبعون ليرةً سوريةً
        /// خمسة و سبعون درهماً إماراتياً
        /// </summary>
        public string Arabic1199CurrencyName { get; set; }

        /// <summary>
        /// Decimal Part Precision
        /// for Syrian Pounds: 2 ( 1 SP = 100 parts)
        /// for Tunisian Dinars: 3 ( 1 TND = 1000 parts)
        /// </summary>
        public Byte PartPrecision { get; set; }

        /// <summary>
        /// Is the currency part name feminine ( Mua'anath مؤنث)
        /// هللة : مؤنث = true
        /// قرش : مذكر = false
        /// </summary>
        public Boolean IsCurrencyPartNameFeminine { get; set; }

        /// <summary>
        /// English Currency Part Name for single use
        /// Piaster
        /// Fils
        /// </summary>
        public string EnglishCurrencyPartName { get; set; }

        /// <summary>
        /// English Currency Part Name for Plural
        /// Piasters
        /// Fils
        /// </summary>
        public string EnglishPluralCurrencyPartName { get; set; }

        /// <summary>
        /// Arabic Currency Part Name for 1 unit only
        /// قرش
        /// هللة
        /// </summary>
        public string Arabic1CurrencyPartName { get; set; }

        /// <summary>
        /// Arabic Currency Part Name for 2 unit only
        /// قرشان
        /// هللتان
        /// </summary>
        public string Arabic2CurrencyPartName { get; set; }

        /// <summary>
        /// Arabic Currency Part Name for 3 to 10 units
        /// قروش
        /// هللات
        /// </summary>
        public string Arabic310CurrencyPartName { get; set; }

        /// <summary>
        /// Arabic Currency Part Name for 11 to 99 units
        /// قرشاً
        /// هللةً
        /// </summary>
        public string Arabic1199CurrencyPartName { get; set; }
        #endregion
    }
}

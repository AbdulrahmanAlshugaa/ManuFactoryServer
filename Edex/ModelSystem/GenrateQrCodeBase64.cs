
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace  DAL
{
    public class GenrateQrCodeBase64
    {
        public string ConverttbBase64(InvoiceViewModel x)
        {
            // Convert seller name to hexa
            string SelerName = ConvertToHexa("01", x.ArbCompanyName);
            if (x.ArbCompanyName.Length >= 14)
                // If the seller name is longer than 14 characters, truncate it
                SelerName = ConvertToHexa("01", x.ArbCompanyName.Substring(0, 15));
            // Convert VAT number to hexa
            string VatNumber = ConvertToHexa("02", x.CompanyVatCode.ToString());
            // Convert invoice date to hexa
            string TimStamp = ConvertToHexa("03", x.InvoiceDate.ToString());
            // Convert net total to hexa
            string InvoiceTotal = ConvertToHexa("04", x.NetTotal.ToString());
            // Convert VAT amount to hexa
            string VatTotal = ConvertToHexa("05", x.VatAmount.ToString());
            // Concatenate all the hexa strings
            string sHex = String.Concat(SelerName, VatNumber, TimStamp, InvoiceTotal, VatTotal);
            // Convert the hexa string to Base64
            string Base64 = convertHexToBase64String(sHex);
            // Return the Base64 string
            return Base64;
            // End of method
        }
        public static string convertHexToBase64String(String hexString)
        {
            string base64 = "";

            //--Important: remove "0x" groups from hexidecimal string--
            hexString = hexString.Replace("0x", "");

            byte[] buffer = new byte[hexString.Length / 2];

            for (int i = 0; i < hexString.Length; i++)
            {
                try
                {
                    buffer[i / 2] = Convert.ToByte(Convert.ToInt32(hexString.Substring(i, 2), 16));
                }
                catch (Exception ex) { }
                i += 1;
            }
            base64 = Convert.ToBase64String(buffer);
            return base64;
        }
        /// <summary>
        /// This method takes in a tag and a value string and returns a string in concatenated hexadecimal format.
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string ConvertToHexa(string tag, string value)
        {
            string decString = value;
            // Convert each character in the string to its hexadecimal representation using the LINQ Select method.
            var hexString = string.Join("", decString.Select(c => String.Format("{0:X2}", Convert.ToInt32(c))));
            string decStringtage = tag;
            int lenthvalue = value.Length;
            // Convert the length of the value string to its hexadecimal representation using the ToString method with format "X2".
            string hextlenthvalue = lenthvalue.ToString("X2");
            // Concatenate the tag, length of value, and hexadecimal representation of the value string.
            string ConcateString = String.Concat(tag, hextlenthvalue, hexString);
            // Return the concatenated string.
            return ConcateString;
        }

    }

 
    public class InvoiceViewModel
    {
        // These properties hold the names of the company in Arabic and English
        public string ArbCompanyName { get; set; }
        public string EngCompanyName { get; set; }

        // These properties hold the address of the company in Arabic and English
        public string ArbCompanyAdress { get; set; }
        public string EngCompanyAdress { get; set; }

        // This property holds the phone number of the company
        public string CompanyPhone { get; set; }

        // This property holds the logo of the company as a byte array
        public byte[] CompanyLogo { get; set; }

        // This property holds the VAT code of the company
        public string CompanyVatCode { get; set; }

        // This property holds the date of the invoice
        public DateTime InvoiceDate { get; set; }

        // These properties hold the net total, VAT total, and visa total of the invoice
        public decimal? NetTotal { get; set; }
        public decimal? VatTotal { get; set; }
        public decimal? VisaTotal { get; set; }

        // This property holds the amount of VAT for the invoice
        public decimal? VatAmount { get; set; }

        // This property holds the total discount for the invoice
        public decimal? DiscountTotal { get; set; }
    }

}
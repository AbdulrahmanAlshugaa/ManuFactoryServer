using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraEditors;
namespace Edex.Model.Language
{

    public enum iLanguage
    {
        Arabic = 0,
        English = 1
    }

  
    public class Translator
    {


        /// <summary>
        /// Arabic letters are translated into English and so on
        /// </summary>
        /// <param name="TheName"></param>
        /// <param name="Language"></param>
        /// <returns></returns>
        public static string ConvertNameToOtherLanguage(string TheName, iLanguage Language)
        {
            string[] ArrArabicLetters = new string[47];
            string[] ArrEnglishLetters = new string[47];
            int i;

            string Result = TheName;

            if (Language == iLanguage.Arabic)
            {
                ArrArabicLetters[1] = "أ";
                ArrArabicLetters[2] = "إ";
                ArrArabicLetters[3] = "آ";
                ArrArabicLetters[4] = "ؤ";
                ArrArabicLetters[5] = "ئ";
                ArrArabicLetters[6] = "ا";
                ArrArabicLetters[7] = "ب";
                ArrArabicLetters[8] = "ت";
                ArrArabicLetters[9] = "ث";
                ArrArabicLetters[10] = "ج";
                ArrArabicLetters[11] = "ح";
                ArrArabicLetters[12] = "خ";
                ArrArabicLetters[13] = "د";
                ArrArabicLetters[14] = "ذ";
                ArrArabicLetters[15] = "ر";
                ArrArabicLetters[16] = "ز";
                ArrArabicLetters[17] = "س";
                ArrArabicLetters[18] = "ش";
                ArrArabicLetters[19] = "ص";
                ArrArabicLetters[20] = "ض";
                ArrArabicLetters[21] = "ط";
                ArrArabicLetters[22] = "ظ";
                ArrArabicLetters[23] = "ع";
                ArrArabicLetters[24] = "غ";
                ArrArabicLetters[25] = "ف";
                ArrArabicLetters[26] = "ق";
                ArrArabicLetters[27] = "ك";
                ArrArabicLetters[28] = "ل";
                ArrArabicLetters[29] = "م";
                ArrArabicLetters[30] = "ن";
                ArrArabicLetters[31] = "ه";
                ArrArabicLetters[32] = "ى";
                ArrArabicLetters[33] = "ة";
                ArrArabicLetters[34] = "ء";
                ArrArabicLetters[35] = "ي";
                ArrArabicLetters[36] = "و";

                ArrEnglishLetters[1] = "A";
                ArrEnglishLetters[2] = "I";
                ArrEnglishLetters[3] = "A";
                ArrEnglishLetters[4] = "U";
                ArrEnglishLetters[5] = "I";
                ArrEnglishLetters[6] = "A";
                ArrEnglishLetters[7] = "B";
                ArrEnglishLetters[8] = "T";
                ArrEnglishLetters[9] = "TH";
                ArrEnglishLetters[10] = "J";
                ArrEnglishLetters[11] = "H";
                ArrEnglishLetters[12] = "KH";
                ArrEnglishLetters[13] = "D";
                ArrEnglishLetters[14] = "DH";
                ArrEnglishLetters[15] = "R";
                ArrEnglishLetters[16] = "Z";
                ArrEnglishLetters[17] = "S";
                ArrEnglishLetters[18] = "SH";
                ArrEnglishLetters[19] = "S";
                ArrEnglishLetters[20] = "D";
                ArrEnglishLetters[21] = "T";
                ArrEnglishLetters[22] = "Z";
                ArrEnglishLetters[23] = "A";
                ArrEnglishLetters[24] = "GH";
                ArrEnglishLetters[25] = "F";
                ArrEnglishLetters[26] = "Q";
                ArrEnglishLetters[27] = "K";
                ArrEnglishLetters[28] = "L";
                ArrEnglishLetters[29] = "M";
                ArrEnglishLetters[30] = "N";
                ArrEnglishLetters[31] = "H";
                ArrEnglishLetters[32] = "A";
                ArrEnglishLetters[33] = "H";
                ArrEnglishLetters[34] = "I";
                ArrEnglishLetters[35] = "Y";
                ArrEnglishLetters[36] = "O";

                for (i = 1; i <= 36; i++)
                    Result = Result.Replace(ArrArabicLetters[i], ArrEnglishLetters[i]);
            }
            else
            {
                ArrEnglishLetters[1] = "A";
                ArrEnglishLetters[2] = "B";
                ArrEnglishLetters[3] = "C";
                ArrEnglishLetters[4] = "D";
                ArrEnglishLetters[5] = "E";
                ArrEnglishLetters[6] = "F";
                ArrEnglishLetters[7] = "G";
                ArrEnglishLetters[8] = "H";
                ArrEnglishLetters[9] = "I";
                ArrEnglishLetters[10] = "J";
                ArrEnglishLetters[11] = "K";
                ArrEnglishLetters[12] = "L";
                ArrEnglishLetters[13] = "M";
                ArrEnglishLetters[14] = "N";
                ArrEnglishLetters[15] = "O";
                ArrEnglishLetters[16] = "P";
                ArrEnglishLetters[17] = "Q";
                ArrEnglishLetters[18] = "R";
                ArrEnglishLetters[19] = "S";
                ArrEnglishLetters[20] = "T";
                ArrEnglishLetters[21] = "U";
                ArrEnglishLetters[22] = "V";
                ArrEnglishLetters[23] = "W";
                ArrEnglishLetters[24] = "X";
                ArrEnglishLetters[25] = "Y";
                ArrEnglishLetters[26] = "Z";
                ArrEnglishLetters[27] = "TH";
                ArrEnglishLetters[28] = "KH";
                ArrEnglishLetters[29] = "DH";
                ArrEnglishLetters[30] = "SH";
                ArrEnglishLetters[31] = "GH";


                ArrArabicLetters[1] = "ا";
                ArrArabicLetters[2] = "ب";
                ArrArabicLetters[3] = "ك";
                ArrArabicLetters[4] = "د";
                ArrArabicLetters[5] = "ي";
                ArrArabicLetters[6] = "ف";
                ArrArabicLetters[7] = "ج";
                ArrArabicLetters[8] = "ه";
                ArrArabicLetters[9] = "ي";
                ArrArabicLetters[10] = "ج";
                ArrArabicLetters[11] = "ك";
                ArrArabicLetters[12] = "ل";
                ArrArabicLetters[13] = "م";
                ArrArabicLetters[14] = "ن";
                ArrArabicLetters[15] = "و";
                ArrArabicLetters[16] = "ب";
                ArrArabicLetters[17] = "ق";
                ArrArabicLetters[18] = "ر";
                ArrArabicLetters[19] = "س";
                ArrArabicLetters[20] = "ت";
                ArrArabicLetters[21] = "ي";
                ArrArabicLetters[22] = "ف";
                ArrArabicLetters[23] = "و";
                ArrArabicLetters[24] = "كس";
                ArrArabicLetters[25] = "ي";
                ArrArabicLetters[26] = "ز";
                ArrArabicLetters[27] = "ذ";
                ArrArabicLetters[28] = "خ";
                ArrArabicLetters[29] = "ظ";
                ArrArabicLetters[30] = "ش";
                ArrArabicLetters[31] = "غ";

                for (i = 1; i <= 31; i++)
                {
                    if (ArrEnglishLetters[i].Length == 2)
                    {
                        Result = Result.ToUpper();
                        Result = Result.Replace(ArrEnglishLetters[i], ArrArabicLetters[i]);
                    }
                }

                for (i = 1; i <= 31; i++)
                {
                    if (ArrEnglishLetters[i].Length == 1)
                    {
                        Result = Result.ToUpper();
                        Result = Result.Replace(ArrEnglishLetters[i], ArrArabicLetters[i]);
                    }
                }
            }
            return Result;
        }

    }

}

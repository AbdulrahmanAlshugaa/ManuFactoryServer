using DevExpress.XtraBars;
using DevExpress.XtraBars.Navigation;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using Edex.Model;
using Edex.Model.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Edex.ModelSystem
{
    /// <summary>
    /// This class is used to convert the contents of forms according to the calling function specified by the language used
    /// </summary>
     public class ChangeLanguage
    {
         /// <summary>
        /// This function is to change the contents and directions of the elements in a specific form that is sent to the function 
        /// and the function passes through all the contents of this form and selects the directions of the contents of this function based
        /// on the language specified in the use of the Arabic language, so the direction is from right to left
         /// </summary>
         /// <param name="form"></param>
         public static void ArabicLanguage(Form form)
        {
            try
            {
                foreach (Control item in form.Controls)
                    if (item is Label || item is LabelControl || item is SimpleButton || item is CheckEdit)
                        RTL(item);
                RTL(form);
                form.RightToLeftLayout = true;
                form.RightToLeft = RightToLeft.Yes;
                UserInfo.Language = iLanguage.Arabic;
            }
            catch (Exception)
            {

                throw;
            }
            
        }

         /// <summary>
         /// This function is to change the contents and directions of the elements in a specific form that is sent to the function 
         /// and the function passes through all the contents of this form and selects the directions of the contents of this function based
         /// on the language specified in the use of the English language, so the direction is from left to right  
         /// </summary>
        /// <param name="form"></param>
         public static void EnglishLanguage(Form form)
         {
            try
            {
                foreach (Control item in form.Controls)
                    if (item is Label || item is LabelControl || item is SimpleButton || item is CheckEdit)
                        LTR(item);

                LTR(form);

                form.RightToLeftLayout = false;
                form.RightToLeft = RightToLeft.No;
                UserInfo.Language = iLanguage.English;
            }
            catch (Exception ex)
            {

                
            }
           
         }

         public static void Invert(Form form)
         {
             try
             {
                 if (UserInfo.Language == iLanguage.English)
                 {
                     EnglishLanguage(form);
                 }
             }
             catch (Exception)
             {


             }


         }

         /// <summary>
         /// This function converts the text of an item, if it is in English, into Arbic , which is stored in Tag,
         /// and so on. The function receives a variable of type Control.
         /// </summary>
         /// <param name="item"></param>
         public static void RTL(Control item)
        {
            try
            {
                if (item.Tag!= null)
                {
                    string temp = item.Tag.ToString();
                    item.Tag = item.Text.ToString();
                    item.Text = temp;
                }
              
            }
            catch (Exception ex)
            {

               
            }
           

        }
         /// <summary>
         /// This function converts the text of an item, if it is in English, into Arbic , which is stored in Tag,
         /// and so on. The function receives a variable of type RibbonPageGroup.
         /// </summary>
         /// <param name="item"></param>
         public static void RTL(RibbonPageGroup item)
         {
            try
            {
                if (item.Tag != null)
                {
                    string temp = item.Tag.ToString();
                    item.Tag = item.Text.ToString();
                    item.Text = temp;
                }
            }
            catch (Exception ex)
            {

                
            }

         }

         /// <summary>
         /// This function converts the text of an item, if it is in English, into Arbic , which is stored in Tag,
         /// and so on. The function receives a variable of type RibbonPage.
         /// </summary>
         /// <param name="item"></param>
         public static void RTL(RibbonPage item)
         {
            try
            {
                if (item.Tag != null)
                {
                    string temp = item.Tag.ToString();
                    item.Tag = item.Text.ToString();
                    item.Text = temp;
                }
            }
            catch (Exception ex)
            {


            }
            

         }
         /// <summary>
         /// This function converts the Caption of an item, if it is in English, into Arbic , which is stored in Tag,
         /// and so on. The function receives a variable of type BarButtonItem.
         /// </summary>
         /// <param name="item"></param>
         public static void RTL(BarButtonItem item)
         {
            try
            {
                if (item.Tag != null)
                {
                    if (item is RibbonGalleryBarItem)
                        return;
                    string temp = item.Tag.ToString();
                    item.Tag = item.Caption.ToString();
                    item.Caption = temp;
                }
            }
            catch (Exception e)
            {


            }
             

         }
         /// <summary>
         /// This function converts the text of an item, if it is in Arabic, into English, which is stored in Tag,
         /// and so on. The function receives a variable of type Control.
         /// </summary>
         /// <param name="item"></param>
         public static void LTR(Control item)
         {
            try
            {
                string a = item.Name;
                if (item.Tag != null)
                {
                    string temp = item.Text.ToString();
                    item.Text = item.Tag.ToString();
                    item.Tag = temp;
                }
            }
            catch (Exception e)
            {

                
            }            
         }
         public static void LTR(TileGroup item)
         {
             try
             {
                 string a = item.Name;
                 if (item.Tag != null)
                 {
                     string temp = item.Text.ToString();
                     item.Text = item.Tag.ToString();
                     item.Tag = temp;
                 }
             }
             catch (Exception e)
             {


             }
         }
         public static void LTR(BarItem item)
         {
             try
             {
                 string a = item.Name;
                 if (item.Tag != null)
                 {
                     string temp = item.Caption.ToString();
                     item.Caption = item.Tag.ToString();
                     item.Tag = temp;
                 }
             }
             catch (Exception e)
             {


             }
         }
         public static void LTR(TileItem item)
         {
             try
             {
                 string a = item.Name;
                 if (item.Tag != null)
                 {
                     string temp = item.Text.ToString();
                     item.Text = item.Tag.ToString();
                     item.Tag = temp;
                 }
             }
             catch (Exception e)
             {


             }

         }
         public static void LTR(TileNavCategory item)
         {
             try
             {
                 string a = item.Name;
                 if (item.Tag != null)
                 {
                     string temp = item.Caption.ToString();
                     item.Caption = item.Tag.ToString();
                     item.Tag = temp;
                 }
             }
             catch (Exception e)
             {


             }

         }
        /// <summary>
         /// This function converts the text of an item, if it is in Arabic, into English, which is stored in Tag,
         /// and so on. The function receives a variable of type RibbonPage.
        /// </summary>
        /// <param name="item"></param>
         public static void LTR(RibbonPage item)
        {
            try
            {
                string temp = item.Text.ToString();
                item.Text = item.Tag.ToString();
                item.Tag = temp;
            }
            catch (Exception e)
            {

                
            }
           
        }
        /// <summary>
         /// This function converts the text of an item, if it is in Arabic, into English, which is stored in Tag,
         /// and so on. The function receives a variable of type RibbonPageGroup.
        /// </summary>
        /// <param name="item"></param>
         public static void LTR(RibbonPageGroup item)
        {
            try
            {
                string temp = item.Text.ToString();
                item.Text = item.Tag.ToString();
                item.Tag = temp;
            }
            catch (Exception e)
            {

                 
            }
           
        }

         public static void LTR(TileNavItem item)
         {
             try
             {
                 string temp = item.Caption.ToString();
                 item.Caption = item.Tag.ToString();
                 item.Tag = temp;
             }
             catch (Exception e)
             {


             }

         }
         /// <summary>
         /// This function converts the Tag of an item, if it is in Arabic, into English, which is stored in Caption,
         /// and so on. The function receives a variable of type BarButtonItem.
         /// </summary>
         /// <param name="item"></param>
         public static void LTR(BarButtonItem item)
         {
             try
             {
                 if (item is RibbonGalleryBarItem)
                     return;
                 string temp = item.Caption.ToString();
                 item.Caption = item.Tag.ToString();
                 item.Tag = temp;
             }
             catch (Exception e)
             {
                 return;
                  
             }
            
         }
    }
}

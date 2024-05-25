using DevExpress.Utils;
using DevExpress.XtraEditors;
using Edex.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Edex.ModelSystem
{
    public class Validations
    { 
        /// <summary>
        ///  This function is used to check the contents of a particular form and display a notification about the error in the control
        /// </summary>
        /// <param name="from"></param>
        /// <returns>The function returns a boolean value True if the form has been Validated and there is nothing wrong with it,
        /// otherwise false if the form has been Validated and an error has been found</returns>
        public static bool IsValidForm(Form from)
        {
            // Initialize validation flag
            bool Validated = true;
            foreach (Control item in from.Controls)
            {
                
                if (item is LookUpEdit)
                {
                    if (item.Tag != null)
                    {
                        //  Check that the field is important and required
                        if (item.Tag.ToString().ToLower() == "ImportantField".ToLower())
                        {
                            if (((LookUpEdit)item).EditValue == null || ((LookUpEdit)item).EditValue == "" || (Comon.cInt(((LookUpEdit)item).EditValue) == 0 && Comon.cLong(((LookUpEdit)item).EditValue) == 0))
                            {                              
                                LookUpEdit txt = (LookUpEdit)item;
                                string a = txt.Name;
                                txt.Focus();
                                ToolTipController toolTip = new ToolTipController();
                                txt.ToolTipController = toolTip;
                                toolTip.Appearance.BackColor = Color.AntiqueWhite;
                                toolTip.ShowBeak = true;
                                toolTip.CloseOnClick = DefaultBoolean.True;
                                toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                                toolTip.InitialDelay = 500;
                                toolTip.ShowBeak = true;
                                toolTip.Rounded = true;
                                toolTip.ShowShadow = true;
                                toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                                toolTip.ToolTipType = ToolTipType.Standard;
                                toolTip.SetTitle(txt, "Error");
                                toolTip.ShowHint(Messages.msgInputIsRequired + " " + txt.Name, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));
                                txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                Validated = false;
                            }
                        }
                    }
                }
               else if (item is TextEdit)
                {
                    if (item.Tag != null)
                    {
                        // Check if control value should be a number
                        if (item.Tag.ToString().ToLower() == "IsNumber".ToLower())
                        {
                            double num;
                            if ((string.IsNullOrEmpty(item.Text.Trim()) || string.IsNullOrWhiteSpace(item.Text.Trim())))
                            {

                            }
                            else if (!(double.TryParse(item.Text, out num)))
                            {
                                TextEdit txt = (TextEdit)item;
                           
                                txt.Focus();
                                ToolTipController toolTip = new ToolTipController();
                                txt.ToolTipController = toolTip;
                                toolTip.Appearance.BackColor = Color.AntiqueWhite;
                                toolTip.ShowBeak = true;
                                toolTip.CloseOnClick = DefaultBoolean.True;
                                toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                                toolTip.InitialDelay = 500;
                                toolTip.ShowBeak = true;
                                toolTip.Rounded = true;
                                toolTip.ShowShadow = true;
                                toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                                toolTip.ToolTipType = ToolTipType.Standard;
                                toolTip.SetTitle(txt, "Error");
                                toolTip.ShowHint(Messages.msgInputShouldBeNumber, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));
                                txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                Validated = false;
                            }


                        }

                            // Check if control value should be a Greater Than Zero
                        else if (item.Tag.ToString().ToLower() == "GreaterThanZero".ToLower())
                        {
                            double num;
                            if (!(double.TryParse(item.Text, out num)))
                            {
                                TextEdit txt = (TextEdit)item;
                                txt.Focus();
                                ToolTipController toolTip = new ToolTipController();
                                txt.ToolTipController = toolTip;
                                toolTip.Appearance.BackColor = Color.AntiqueWhite;
                                toolTip.ShowBeak = true;
                                toolTip.CloseOnClick = DefaultBoolean.True;
                                toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                                toolTip.InitialDelay = 500;
                                toolTip.ShowBeak = true;
                                toolTip.Rounded = true;
                                toolTip.ShowShadow = true;
                                toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                                toolTip.ToolTipType = ToolTipType.Standard;
                                toolTip.SetTitle(txt, "Error");

                               
                                toolTip.ShowHint(Messages.msgInputShouldBeNumber, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));
                                txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                Validated = false;
                            }
                            else if (Comon.cDbl(item.Text.Trim()) <= 0)
                            {
                                TextEdit txt = (TextEdit)item;
                                txt.Focus();
                                ToolTipController toolTip = new ToolTipController();
                                txt.ToolTipController = toolTip;
                                toolTip.Appearance.BackColor = Color.AntiqueWhite;
                                toolTip.ShowBeak = true;
                                toolTip.CloseOnClick = DefaultBoolean.True;
                                toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                                toolTip.InitialDelay = 500;
                                toolTip.ShowBeak = true;
                                toolTip.Rounded = true;
                                toolTip.ShowShadow = true;
                                toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                                toolTip.ToolTipType = ToolTipType.Standard;
                                toolTip.SetTitle(txt, "Error");
                                
                                toolTip.ShowHint(Messages.msgInputIsGreaterThanZero, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));
                                txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                Validated = false;
                            }
                        }
                        // Check that the field is important and required
                        else if (item.Tag.ToString().ToLower() == "ImportantField".ToLower())
                        {

                            if ((string.IsNullOrEmpty(item.Text.Trim()) || string.IsNullOrWhiteSpace(item.Text.Trim())))
                            {
                                TextEdit txt = (TextEdit)item;
                                txt.Focus();
                                ToolTipController toolTip = new ToolTipController();
                                txt.ToolTipController = toolTip;
                                toolTip.Appearance.BackColor = Color.AntiqueWhite;
                                toolTip.ShowBeak = true;
                                toolTip.CloseOnClick = DefaultBoolean.True;
                                toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                                toolTip.InitialDelay = 500;
                                toolTip.ShowBeak = true;
                                toolTip.Rounded = true;
                                toolTip.ShowShadow = true;
                                toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                                toolTip.ToolTipType = ToolTipType.Standard;
                                toolTip.SetTitle(txt, "Error");
                                toolTip.ShowHint(Messages.msgInputIsRequired , ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));

                                txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                Validated = false;
                            }
                        }
                        // Check that the field is important and required and Greater Than Zero
                        else if (item.Tag.ToString().ToLower() == "ImportantFieldGreaterThanZero".ToLower())
                        {
                            double num;
                             TextEdit txat = (TextEdit)item;
                                string aa = txat.Name;
                            if ((string.IsNullOrEmpty(item.Text.Trim()) || string.IsNullOrWhiteSpace(item.Text.Trim())))
                            {
                                TextEdit txt = (TextEdit)item;
                                string a = txt.Name;
                                txt.Focus();
                                ToolTipController toolTip = new ToolTipController();
                                txt.ToolTipController = toolTip;
                                toolTip.Appearance.BackColor = Color.AntiqueWhite;
                                toolTip.ShowBeak = true;
                                toolTip.CloseOnClick = DefaultBoolean.True;
                                toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                                toolTip.InitialDelay = 500;
                                toolTip.ShowBeak = true;
                                toolTip.Rounded = true;
                                toolTip.ShowShadow = true;
                                toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                                toolTip.ToolTipType = ToolTipType.Standard;
                                toolTip.SetTitle(txt, "Error");
                                toolTip.ShowHint(Messages.msgInputIsRequired  , ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));
                                txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                Validated = false;
                            }

                            else if (!(Double.TryParse(item.Text, out num)))
                            {
                                TextEdit txt = (TextEdit)item;
                                txt.Focus();
                                ToolTipController toolTip = new ToolTipController();
                                txt.ToolTipController = toolTip;
                                toolTip.Appearance.BackColor = Color.AntiqueWhite;
                                toolTip.ShowBeak = true;
                                toolTip.CloseOnClick = DefaultBoolean.True;
                                toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                                toolTip.InitialDelay = 500;
                                toolTip.ShowBeak = true;
                                toolTip.Rounded = true;
                                toolTip.ShowShadow = true;
                                toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                                toolTip.ToolTipType = ToolTipType.Standard;
                                toolTip.SetTitle(txt, "Error");
                                toolTip.ShowHint(Messages.msgInputShouldBeNumber, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));
                                txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                Validated = false;

                            }
                            else if (Comon.cDbl(item.Text.Trim()) <= 0)
                            {  
                               
                                TextEdit txt = (TextEdit)item;
                                string a = txt.Name;
                                txt.Focus();
                                ToolTipController toolTip = new ToolTipController();
                                txt.ToolTipController = toolTip;
                                toolTip.Appearance.BackColor = Color.AntiqueWhite;
                                toolTip.ShowBeak = true;
                                toolTip.CloseOnClick = DefaultBoolean.True;
                                toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                                toolTip.InitialDelay = 500;
                                toolTip.ShowBeak = true;
                                toolTip.Rounded = true;
                                toolTip.ShowShadow = true;
                                toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                                toolTip.ToolTipType = ToolTipType.Standard;
                                toolTip.SetTitle(txt, "Error");

                              
                                toolTip.ShowHint(Messages.msgInputIsGreaterThanZero, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));
                                txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                Validated = false;
                            }

                        }
                        
                    }
                }   
            }
            return Validated;
        }
        public static bool IsValidForm(Control from)
        {
            // Initialize validation flag
            bool Validated = true;
            foreach (Control item in from.Controls)
            {
                if (item is LookUpEdit)
                {
                    if (item.Tag != null)
                    {
                        //  Check that the field is important and required
                        if (item.Tag.ToString().ToLower() == "ImportantField".ToLower())
                        {
                            if (((LookUpEdit)item).EditValue == null || ((LookUpEdit)item).EditValue == "" || (Comon.cInt(((LookUpEdit)item).EditValue) == 0 && Comon.cLong(((LookUpEdit)item).EditValue) == 0))
                            {

                                LookUpEdit txt = (LookUpEdit)item;
                                string a = txt.Name;
                                txt.Focus();
                                ToolTipController toolTip = new ToolTipController();
                                txt.ToolTipController = toolTip;
                                toolTip.Appearance.BackColor = Color.AntiqueWhite;
                                toolTip.ShowBeak = true;
                                toolTip.CloseOnClick = DefaultBoolean.True;
                                toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                                toolTip.InitialDelay = 500;
                                toolTip.ShowBeak = true;
                                toolTip.Rounded = true;
                                toolTip.ShowShadow = true;
                                toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                                toolTip.ToolTipType = ToolTipType.Standard;
                                toolTip.SetTitle(txt, "Error");
                                toolTip.ShowHint(Messages.msgInputIsRequired + " " + txt.Name, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));
                                txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                Validated = false;
                            }
                        }

                    }
                }
                else if (item is TextEdit)
                {
                    if (item.Tag != null)
                    {
                        // Check if control value should be a number
                        if (item.Tag.ToString().ToLower() == "IsNumber".ToLower())
                        {
                            double num;
                            if ((string.IsNullOrEmpty(item.Text.Trim()) || string.IsNullOrWhiteSpace(item.Text.Trim())))
                            {

                            }
                            else if (!(double.TryParse(item.Text, out num)))
                            {
                                TextEdit txt = (TextEdit)item;

                                txt.Focus();
                                ToolTipController toolTip = new ToolTipController();
                                txt.ToolTipController = toolTip;
                                toolTip.Appearance.BackColor = Color.AntiqueWhite;
                                toolTip.ShowBeak = true;
                                toolTip.CloseOnClick = DefaultBoolean.True;
                                toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                                toolTip.InitialDelay = 500;
                                toolTip.ShowBeak = true;
                                toolTip.Rounded = true;
                                toolTip.ShowShadow = true;
                                toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                                toolTip.ToolTipType = ToolTipType.Standard;
                                toolTip.SetTitle(txt, "Error");
                                toolTip.ShowHint(Messages.msgInputShouldBeNumber, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));
                                txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                Validated = false;
                            }


                        }

                            // Check if control value should be a Greater Than Zero
                        else if (item.Tag.ToString().ToLower() == "GreaterThanZero".ToLower())
                        {
                            double num;
                            if (!(double.TryParse(item.Text, out num)))
                            {
                                TextEdit txt = (TextEdit)item;
                                txt.Focus();
                                ToolTipController toolTip = new ToolTipController();
                                txt.ToolTipController = toolTip;
                                toolTip.Appearance.BackColor = Color.AntiqueWhite;
                                toolTip.ShowBeak = true;
                                toolTip.CloseOnClick = DefaultBoolean.True;
                                toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                                toolTip.InitialDelay = 500;
                                toolTip.ShowBeak = true;
                                toolTip.Rounded = true;
                                toolTip.ShowShadow = true;
                                toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                                toolTip.ToolTipType = ToolTipType.Standard;
                                toolTip.SetTitle(txt, "Error");


                                toolTip.ShowHint(Messages.msgInputShouldBeNumber, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));
                                txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                Validated = false;
                            }
                            else if (Comon.cDbl(item.Text.Trim()) <= 0)
                            {
                                TextEdit txt = (TextEdit)item;
                                txt.Focus();
                                ToolTipController toolTip = new ToolTipController();
                                txt.ToolTipController = toolTip;
                                toolTip.Appearance.BackColor = Color.AntiqueWhite;
                                toolTip.ShowBeak = true;
                                toolTip.CloseOnClick = DefaultBoolean.True;
                                toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                                toolTip.InitialDelay = 500;
                                toolTip.ShowBeak = true;
                                toolTip.Rounded = true;
                                toolTip.ShowShadow = true;
                                toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                                toolTip.ToolTipType = ToolTipType.Standard;
                                toolTip.SetTitle(txt, "Error");

                                toolTip.ShowHint(Messages.msgInputIsGreaterThanZero, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));
                                txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                Validated = false;
                            }
                        }
                        // Check that the field is important and required
                        else if (item.Tag.ToString().ToLower() == "ImportantField".ToLower())
                        {

                            if ((string.IsNullOrEmpty(item.Text.Trim()) || string.IsNullOrWhiteSpace(item.Text.Trim())))
                            {
                                TextEdit txt = (TextEdit)item;
                                txt.Focus();
                                ToolTipController toolTip = new ToolTipController();
                                txt.ToolTipController = toolTip;
                                toolTip.Appearance.BackColor = Color.AntiqueWhite;
                                toolTip.ShowBeak = true;
                                toolTip.CloseOnClick = DefaultBoolean.True;
                                toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                                toolTip.InitialDelay = 500;
                                toolTip.ShowBeak = true;
                                toolTip.Rounded = true;
                                toolTip.ShowShadow = true;
                                toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                                toolTip.ToolTipType = ToolTipType.Standard;
                                toolTip.SetTitle(txt, "Error");
                                toolTip.ShowHint(Messages.msgInputIsRequired, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));

                                txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                Validated = false;
                            }
                        }
                        // Check that the field is important and required and Greater Than Zero
                        else if (item.Tag.ToString().ToLower() == "ImportantFieldGreaterThanZero".ToLower())
                        {
                            double num;
                            TextEdit txat = (TextEdit)item;
                            string aa = txat.Name;
                            if ((string.IsNullOrEmpty(item.Text.Trim()) || string.IsNullOrWhiteSpace(item.Text.Trim())))
                            {
                                TextEdit txt = (TextEdit)item;
                                string a = txt.Name;
                                txt.Focus();
                                ToolTipController toolTip = new ToolTipController();
                                txt.ToolTipController = toolTip;
                                toolTip.Appearance.BackColor = Color.AntiqueWhite;
                                toolTip.ShowBeak = true;
                                toolTip.CloseOnClick = DefaultBoolean.True;
                                toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                                toolTip.InitialDelay = 500;
                                toolTip.ShowBeak = true;
                                toolTip.Rounded = true;
                                toolTip.ShowShadow = true;
                                toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                                toolTip.ToolTipType = ToolTipType.Standard;
                                toolTip.SetTitle(txt, "Error");
                                toolTip.ShowHint(Messages.msgInputIsRequired, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));
                                txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                Validated = false;
                            }

                            else if (!(Double.TryParse(item.Text, out num)))
                            {
                                TextEdit txt = (TextEdit)item;
                                txt.Focus();
                                ToolTipController toolTip = new ToolTipController();
                                txt.ToolTipController = toolTip;
                                toolTip.Appearance.BackColor = Color.AntiqueWhite;
                                toolTip.ShowBeak = true;
                                toolTip.CloseOnClick = DefaultBoolean.True;
                                toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                                toolTip.InitialDelay = 500;
                                toolTip.ShowBeak = true;
                                toolTip.Rounded = true;
                                toolTip.ShowShadow = true;
                                toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                                toolTip.ToolTipType = ToolTipType.Standard;
                                toolTip.SetTitle(txt, "Error");
                                toolTip.ShowHint(Messages.msgInputShouldBeNumber, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));
                                txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                Validated = false;

                            }
                            else if (Comon.cDbl(item.Text.Trim()) <= 0)
                            {

                                TextEdit txt = (TextEdit)item;
                                string a = txt.Name;
                                txt.Focus();
                                ToolTipController toolTip = new ToolTipController();
                                txt.ToolTipController = toolTip;
                                toolTip.Appearance.BackColor = Color.AntiqueWhite;
                                toolTip.ShowBeak = true;
                                toolTip.CloseOnClick = DefaultBoolean.True;
                                toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                                toolTip.InitialDelay = 500;
                                toolTip.ShowBeak = true;
                                toolTip.Rounded = true;
                                toolTip.ShowShadow = true;
                                toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                                toolTip.ToolTipType = ToolTipType.Standard;
                                toolTip.SetTitle(txt, "Error");


                                toolTip.ShowHint(Messages.msgInputIsGreaterThanZero, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));
                                txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                Validated = false;
                            }

                        }
                    }
                }
            }
            return Validated;
        }
        public static bool IsValidFormCmb(Control item)
        {
            
            if (item.Tag.ToString().ToLower() == "ImportantField".ToLower())
            {
                if (((LookUpEdit)item).EditValue == null || ((LookUpEdit)item).EditValue == "" || (Comon.cInt(((LookUpEdit)item).EditValue) == 0 && Comon.cLong(((LookUpEdit)item).EditValue) == 0))
                {
                    LookUpEdit txt = (LookUpEdit)item;
                    string a = txt.Name;
                    txt.Focus();
                    ToolTipController toolTip = new ToolTipController();
                    txt.ToolTipController = toolTip;
                    toolTip.Appearance.BackColor = Color.AntiqueWhite;
                    toolTip.ShowBeak = true;
                    toolTip.CloseOnClick = DefaultBoolean.True;
                    toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                    toolTip.InitialDelay = 500;
                    toolTip.ShowBeak = true;
                    toolTip.Rounded = true;
                    toolTip.ShowShadow = true;
                    toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                    toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                    toolTip.ToolTipType = ToolTipType.Standard;
                    toolTip.SetTitle(txt, "Error");
                    toolTip.ShowHint(Messages.msgInputIsRequired + " " + txt.Name, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));
                    txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                    return false;
                }
                else
                    return true;
            }

            else
                return true;
        }
        public static bool Important(Form from)
        {
            bool Validated = true;
            foreach (Control item in from.Controls)
            {
                if (item is TextEdit)
                {
                    if (item.Tag != null)
                    {
                        if (item.Tag.ToString().ToLower() == "IsNumber".ToLower())
                        {
                            int num;
                            if (!(int.TryParse(item.Text, out num)))
                            {
                                TextEdit txt = (TextEdit)item;
                                txt.Focus();
                                ToolTipController toolTip = new ToolTipController();
                                txt.ToolTipController = toolTip;
                                toolTip.Appearance.BackColor = Color.AntiqueWhite;
                                toolTip.ShowBeak = true;
                                toolTip.CloseOnClick = DefaultBoolean.True;
                                toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                                toolTip.InitialDelay = 500;
                                toolTip.ShowBeak = true;
                                toolTip.Rounded = true;
                                toolTip.ShowShadow = true;
                                toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                                toolTip.ToolTipType = ToolTipType.Standard;
                                toolTip.SetTitle(txt, "Error");
                                toolTip.ShowHint(Messages.msgInputShouldBeNumber, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));
                                txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                Validated = false;
                            }

                        }

                        else if (item.Tag.ToString().ToLower() == "GreaterThanZero".ToLower())
                        {
                            double num;
                            if (!(double.TryParse(item.Text, out num)))
                            {
                                TextEdit txt = (TextEdit)item;
                                txt.Focus();
                                ToolTipController toolTip = new ToolTipController();
                                txt.ToolTipController = toolTip;
                                toolTip.Appearance.BackColor = Color.AntiqueWhite;
                                toolTip.ShowBeak = true;
                                toolTip.CloseOnClick = DefaultBoolean.True;
                                toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                                toolTip.InitialDelay = 500;
                                toolTip.ShowBeak = true;
                                toolTip.Rounded = true;
                                toolTip.ShowShadow = true;
                                toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                                toolTip.ToolTipType = ToolTipType.Standard;
                                toolTip.SetTitle(txt, "Error");
                                toolTip.ShowHint(Messages.msgInputShouldBeNumber, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));
                                txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                Validated = false;
                            }
                            else if (Comon.cDbl(item.Text.Trim()) <= 0)
                            {
                                TextEdit txt = (TextEdit)item;
                                txt.Focus();
                                ToolTipController toolTip = new ToolTipController();
                                txt.ToolTipController = toolTip;
                                toolTip.Appearance.BackColor = Color.AntiqueWhite;
                                toolTip.ShowBeak = true;
                                toolTip.CloseOnClick = DefaultBoolean.True;
                                toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                                toolTip.InitialDelay = 500;
                                toolTip.ShowBeak = true;
                                toolTip.Rounded = true;
                                toolTip.ShowShadow = true;
                                toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                                toolTip.ToolTipType = ToolTipType.Standard;
                                toolTip.SetTitle(txt, "Error");
                                toolTip.ShowHint(Messages.msgInputIsGreaterThanZero, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));
                                txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                Validated = false;
                            }
                        }
                        else if (item.Tag.ToString().ToLower() == "ImportantField".ToLower())
                        {

                            if ((string.IsNullOrEmpty(item.Text.Trim()) || string.IsNullOrWhiteSpace(item.Text.Trim())))
                            {
                                TextEdit txt = (TextEdit)item;
                                txt.Focus();
                                ToolTipController toolTip = new ToolTipController();
                                txt.ToolTipController = toolTip;
                                toolTip.Appearance.BackColor = Color.AntiqueWhite;
                                toolTip.ShowBeak = true;
                                toolTip.CloseOnClick = DefaultBoolean.True;
                                toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                                toolTip.InitialDelay = 500;
                                toolTip.ShowBeak = true;
                                toolTip.Rounded = true;
                                toolTip.ShowShadow = true;
                                toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                                toolTip.ToolTipType = ToolTipType.Standard;
                                toolTip.SetTitle(txt, "Error");
                                toolTip.ShowHint(Messages.msgInputIsRequired , ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));

                                txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                Validated = false;
                            }
                        }
                        else if (item.Tag.ToString().ToLower() == "ImportantFieldGreaterThanZero".ToLower())
                        {
                            double num;
                            if ((string.IsNullOrEmpty(item.Text.Trim()) || string.IsNullOrWhiteSpace(item.Text.Trim())))
                            {
                                TextEdit txt = (TextEdit)item;
                                txt.Focus();
                                ToolTipController toolTip = new ToolTipController();
                                txt.ToolTipController = toolTip;
                                toolTip.Appearance.BackColor = Color.AntiqueWhite;
                                toolTip.ShowBeak = true;
                                toolTip.CloseOnClick = DefaultBoolean.True;
                                toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                                toolTip.InitialDelay = 500;
                                toolTip.ShowBeak = true;
                                toolTip.Rounded = true;
                                toolTip.ShowShadow = true;
                                toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                                toolTip.ToolTipType = ToolTipType.Standard;
                                toolTip.SetTitle(txt, "Error");
                                toolTip.ShowHint(Messages.msgInputIsRequired, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));
                                txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                Validated = false;
                            }

                            else if (!(Double.TryParse(item.Text, out num)))
                            {
                                TextEdit txt = (TextEdit)item;
                                txt.Focus();
                                ToolTipController toolTip = new ToolTipController();
                                txt.ToolTipController = toolTip;
                                toolTip.Appearance.BackColor = Color.AntiqueWhite;
                                toolTip.ShowBeak = true;
                                toolTip.CloseOnClick = DefaultBoolean.True;
                                toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                                toolTip.InitialDelay = 500;
                                toolTip.ShowBeak = true;
                                toolTip.Rounded = true;
                                toolTip.ShowShadow = true;
                                toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                                toolTip.ToolTipType = ToolTipType.Standard;
                                toolTip.SetTitle(txt, "Error");
                                toolTip.ShowHint(Messages.msgInputShouldBeNumber, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));
                                txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                Validated = false;

                            }
                            else if (Comon.cDbl(item.Text.Trim()) <= 0)
                            {
                                TextEdit txt = (TextEdit)item;
                                txt.Focus();
                                ToolTipController toolTip = new ToolTipController();
                                txt.ToolTipController = toolTip;
                                toolTip.Appearance.BackColor = Color.AntiqueWhite;
                                toolTip.ShowBeak = true;
                                toolTip.CloseOnClick = DefaultBoolean.True;
                                toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                                toolTip.InitialDelay = 500;
                                toolTip.ShowBeak = true;
                                toolTip.Rounded = true;
                                toolTip.ShowShadow = true;
                                toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                                toolTip.ToolTipType = ToolTipType.Standard;
                                toolTip.SetTitle(txt, "Error");
                                toolTip.ShowHint(Messages.msgInputIsGreaterThanZero, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));
                                txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                Validated = false;
                            }

                        }
                    }
                }
            }

            return Validated;
        }
        public static bool GreaterThanZero(TextEdit item)
        {
            bool Validated = true;

            if (item is TextEdit)
            {

                if (item.Tag != null && item.Tag.ToString().ToLower() == "IsNumber".ToLower())
                {
                    int num;
                    if (!(int.TryParse(item.Text, out num)))
                    {
                        TextEdit txt = (TextEdit)item;
                        txt.Focus();
                        ToolTipController toolTip = new ToolTipController();
                        txt.ToolTipController = toolTip;
                        toolTip.Appearance.BackColor = Color.AntiqueWhite;
                        toolTip.ShowBeak = true;
                        toolTip.CloseOnClick = DefaultBoolean.True;
                        toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                        toolTip.InitialDelay = 500;
                        toolTip.ShowBeak = true;
                        toolTip.Rounded = true;
                        toolTip.ShowShadow = true;
                        toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                        toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                        toolTip.ToolTipType = ToolTipType.Standard;
                        toolTip.SetTitle(txt, "Error");
                        toolTip.ShowHint(Messages.msgInputShouldBeNumber, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));
                        txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                        Validated = false;
                    }

                }

                else if (item.Tag != null && item.Tag.ToString().ToLower() == "GreaterThanZero".ToLower())
                {
                    int num;
                    if (!(int.TryParse(item.Text, out num)))
                    {
                        TextEdit txt = (TextEdit)item;
                        txt.Focus();
                        ToolTipController toolTip = new ToolTipController();
                        txt.ToolTipController = toolTip;
                        toolTip.Appearance.BackColor = Color.AntiqueWhite;
                        toolTip.ShowBeak = true;
                        toolTip.CloseOnClick = DefaultBoolean.True;
                        toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                        toolTip.InitialDelay = 500;
                        toolTip.ShowBeak = true;
                        toolTip.Rounded = true;
                        toolTip.ShowShadow = true;
                        toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                        toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                        toolTip.ToolTipType = ToolTipType.Standard;
                        toolTip.SetTitle(txt, "Error");
                        toolTip.ShowHint(Messages.msgInputShouldBeNumber, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));
                        txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                        Validated = false;
                    }
                    else if (int.Parse(item.Text.Trim()) <= 0)
                    {
                        TextEdit txt = (TextEdit)item;
                        txt.Focus();
                        ToolTipController toolTip = new ToolTipController();
                        txt.ToolTipController = toolTip;
                        toolTip.Appearance.BackColor = Color.AntiqueWhite;
                        toolTip.ShowBeak = true;
                        toolTip.CloseOnClick = DefaultBoolean.True;
                        toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                        toolTip.InitialDelay = 500;
                        toolTip.ShowBeak = true;
                        toolTip.Rounded = true;
                        toolTip.ShowShadow = true;
                        toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                        toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                        toolTip.ToolTipType = ToolTipType.Standard;
                        toolTip.SetTitle(txt, "Error");
                        toolTip.ShowHint(Messages.msgInputIsGreaterThanZero, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));
                        txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                        Validated = false;
                    }
                }
            }


            return Validated;
        }
       /// <summary>
        /// This function is the same function IsValidForm, but it is different in that it receives a parameter of a type TextEdit,
       /// and this control is checked in terms of what it contains of inputs and whether it is required or a number or greater than zero and others
       /// </summary>
       /// <param name="item"></param>
        /// <returns>The function returns a boolean value True if the TextEdit has been Validated and there is nothing wrong with it,
        /// otherwise false if the TextEdit has been Validated and an error has been found</returns>
        public static bool Important(TextEdit item)
        {
            bool Validated = true;
            if (item is TextEdit)
            {
                if (item.Tag != null)
                {
                    if (item.Tag != null && item.Tag.ToString().ToLower() == "GreaterThanZero".ToLower())
                    {
                        int num;
                        if (!(int.TryParse(item.Text, out num)))
                        {
                            TextEdit txt = (TextEdit)item;
                            txt.Focus();
                            ToolTipController toolTip = new ToolTipController();
                            txt.ToolTipController = toolTip;
                            toolTip.Appearance.BackColor = Color.AntiqueWhite;
                            toolTip.ShowBeak = true;
                            toolTip.CloseOnClick = DefaultBoolean.True;
                            toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                            toolTip.InitialDelay = 500;
                            toolTip.ShowBeak = true;
                            toolTip.Rounded = true;
                            toolTip.ShowShadow = true;
                            toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                            toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                            toolTip.ToolTipType = ToolTipType.Standard;
                            toolTip.SetTitle(txt, "Error");
                            toolTip.ShowHint(Messages.msgInputShouldBeNumber, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));
                            txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                            Validated = false;
                        }
                        else if (int.Parse(item.Text.Trim()) <= 0)
                        {
                            TextEdit txt = (TextEdit)item;
                            txt.Focus();
                            ToolTipController toolTip = new ToolTipController();
                            txt.ToolTipController = toolTip;
                            toolTip.Appearance.BackColor = Color.AntiqueWhite;
                            toolTip.ShowBeak = true;
                            toolTip.CloseOnClick = DefaultBoolean.True;
                            toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                            toolTip.InitialDelay = 500;
                            toolTip.ShowBeak = true;
                            toolTip.Rounded = true;
                            toolTip.ShowShadow = true;
                            toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                            toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                            toolTip.ToolTipType = ToolTipType.Standard;
                            toolTip.SetTitle(txt, "Error");
                            toolTip.ShowHint(Messages.msgInputIsGreaterThanZero, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));
                            txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                            Validated = false;
                        }
                    }
                    else if (item.Tag.ToString().ToLower() == "ImportantField".ToLower())
                    {
                        if ((string.IsNullOrEmpty(item.Text.Trim()) || string.IsNullOrWhiteSpace(item.Text.Trim())))
                        {
                            TextEdit txt = (TextEdit)item;
                            txt.Focus();
                            ToolTipController toolTip = new ToolTipController();
                            txt.ToolTipController = toolTip;
                            toolTip.Appearance.BackColor = Color.AntiqueWhite;
                            toolTip.ShowBeak = true;
                            toolTip.CloseOnClick = DefaultBoolean.True;
                            toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                            toolTip.InitialDelay = 500;
                            toolTip.ShowBeak = true;
                            toolTip.Rounded = true;
                            toolTip.ShowShadow = true;
                            toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                            toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                            toolTip.ToolTipType = ToolTipType.Standard;
                            toolTip.SetTitle(txt, "Error");
                            toolTip.ShowHint(Messages.msgInputIsRequired , ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));

                            txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                            Validated = false;
                        }
                    }
                    else if (item.Tag.ToString().ToLower() == "ImportantFieldGreaterThanZero".ToLower())
                    {
                        int num;
                        if ((string.IsNullOrEmpty(item.Text.Trim()) || string.IsNullOrWhiteSpace(item.Text.Trim())))
                        {
                            TextEdit txt = (TextEdit)item;
                            txt.Focus();
                            ToolTipController toolTip = new ToolTipController();
                            txt.ToolTipController = toolTip;
                            toolTip.Appearance.BackColor = Color.AntiqueWhite;
                            toolTip.ShowBeak = true;
                            toolTip.CloseOnClick = DefaultBoolean.True;
                            toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                            toolTip.InitialDelay = 500;
                            toolTip.ShowBeak = true;
                            toolTip.Rounded = true;
                            toolTip.ShowShadow = true;
                            toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                            toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                            toolTip.ToolTipType = ToolTipType.Standard;
                            toolTip.SetTitle(txt, "Error");
                            toolTip.ShowHint(Messages.msgInputIsRequired, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));
                            txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                            Validated = false;
                        }

                        else if (!(int.TryParse(item.Text, out num)))
                        {
                            TextEdit txt = (TextEdit)item;
                            txt.Focus();
                            ToolTipController toolTip = new ToolTipController();
                            txt.ToolTipController = toolTip;
                            toolTip.Appearance.BackColor = Color.AntiqueWhite;
                            toolTip.ShowBeak = true;
                            toolTip.CloseOnClick = DefaultBoolean.True;
                            toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                            toolTip.InitialDelay = 500;
                            toolTip.ShowBeak = true;
                            toolTip.Rounded = true;
                            toolTip.ShowShadow = true;
                            toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                            toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                            toolTip.ToolTipType = ToolTipType.Standard;
                            toolTip.SetTitle(txt, "Error");
                            toolTip.ShowHint(Messages.msgInputShouldBeNumber, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));
                            txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                            Validated = false;

                        }
                        else if (int.Parse(item.Text.Trim()) <= 0)
                        {
                            TextEdit txt = (TextEdit)item;
                            txt.Focus();
                            ToolTipController toolTip = new ToolTipController();
                            txt.ToolTipController = toolTip;
                            toolTip.Appearance.BackColor = Color.AntiqueWhite;
                            toolTip.ShowBeak = true;
                            toolTip.CloseOnClick = DefaultBoolean.True;
                            toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                            toolTip.InitialDelay = 500;
                            toolTip.ShowBeak = true;
                            toolTip.Rounded = true;
                            toolTip.ShowShadow = true;
                            toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                            toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                            toolTip.ToolTipType = ToolTipType.Standard;
                            toolTip.SetTitle(txt, "Error");
                            toolTip.ShowHint(Messages.msgInputIsGreaterThanZero, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));
                            txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                            Validated = false;
                        }

                    }
                }
            }

            return Validated;
        }
        /// <summary>
        /// This function is used to display an error message or notify the ToolTipController.
        /// The TextEdit that is sent to the function when it is called
        /// </summary>
        /// <param name="item"></param>
        /// <param name="Msg"></param>
        // Method to show error message with tooltip
        public static void ErrorText(TextEdit item, string Msg)
        {
            if (item is TextEdit)
            {
                // Cast item to TextEdit control
                TextEdit txt = (TextEdit)item;

                // Set focus to the TextEdit control
                txt.Focus();

                // Create a new tooltip controller and set its properties
                ToolTipController toolTip = new ToolTipController();
                txt.ToolTipController = toolTip;
                toolTip.Appearance.BackColor = Color.AntiqueWhite;
                toolTip.ShowBeak = true;
                toolTip.CloseOnClick = DefaultBoolean.True;
                toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                toolTip.InitialDelay = 500;
                toolTip.Rounded = true;
                toolTip.ShowShadow = true;
                toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                toolTip.ToolTipType = ToolTipType.Standard;
                toolTip.SetTitle(txt, "Error");

                // Show the tooltip with the error message
                toolTip.ShowHint(Msg, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));

                // Set the border color of the TextEdit control
                txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
            }
        }
        /// <summary>
        /// This function is used to set value  true Or false To Proprity enable  RibbonControl Items in a specific form that is sent to the function when Click the button RolBack.
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="ribbonControl1"></param>
        public static void DoRoolBackRipon(Form frm, DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1)
        {
            ribbonControl1.Items[0].Enabled = false; //اضافة من
            ribbonControl1.Items[1].Enabled = true; //جديد
            ribbonControl1.Items[2].Enabled = false; //تعديل
            ribbonControl1.Items[3].Enabled = false;//حفظ
            ribbonControl1.Items[4].Enabled = true;//حذف
            ribbonControl1.Items[5].Enabled = true;//الاول
            ribbonControl1.Items[6].Enabled = true;//السابق
            ribbonControl1.Items[7].Enabled = true;//السجل- النقاط
            ribbonControl1.Items[8].Enabled = true;//التالي
            ribbonControl1.Items[9].Enabled = true;//الأخير
            ribbonControl1.Items[10].Enabled = true;//بحث
            ribbonControl1.Items[11].Enabled = true;//طباعة
            ribbonControl1.Items[12].Enabled = true;//تراجع
            ribbonControl1.Items[13].Enabled = true;//خروج
            ribbonControl1.Items[14].Enabled = false;//تصدير


        }
        /// <summary>
        ///  This function is used to set value  true Or false To Proprity enable  RibbonControl Items in a specific form that is sent to the function when Click the button New.
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="ribbonControl1"></param>
        public static void DoNewRipon(Form frm, DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1)
        {

            ribbonControl1.Items[1].Enabled = false; //جديد 
            ribbonControl1.Items[2].Enabled = false; //تعديل
            ribbonControl1.Items[3].Enabled = true;//حفظ
            ribbonControl1.Items[4].Enabled = false;//حذف
            ribbonControl1.Items[5].Enabled = false;//الاول
            ribbonControl1.Items[6].Enabled = false;//التالي
            ribbonControl1.Items[7].Enabled = false;// السابق
            ribbonControl1.Items[8].Enabled = false;//الأخير-
            ribbonControl1.Items[9].Enabled = false;//بحث
            ribbonControl1.Items[10].Enabled = false;//طباعة
            ribbonControl1.Items[11].Enabled = false;//تصدير الرئيسي الزر
            ribbonControl1.Items[12].Enabled = false;//خروج
            ribbonControl1.Items[13].Enabled = false;//تصدير الى ملف اكسل
            ribbonControl1.Items[14].Enabled = false;//تصدير الى ملف بي دي اف
            ribbonControl1.Items[15].Enabled = false;//تصدير الى ملف نصي
            ribbonControl1.Items[16].Enabled = false;//تصدير الى ملف وورد
            ribbonControl1.Items[17].Enabled = false;//تصديىر الى ملف اتش تي ام ال
            ribbonControl1.Items[18].Enabled = false;//......
            ribbonControl1.Items[19].Enabled = true;//اضافة من
            ribbonControl1.Items[20].Enabled = true;//تراجع
        }
        /// <summary>
        ///  This function is used to set value  true Or false To Proprity enable  RibbonControl Items in a specific form that is sent to the function when The Form is Load.
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="ribbonControl1"></param>
        public static void DoLoadRipon(Form frm, DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1)
        {
            ribbonControl1.Items[0].Enabled = true; //جديد 
            ribbonControl1.Items[1].Enabled = true; //جديد 
            ribbonControl1.Items[2].Enabled = false; //تعديل
            ribbonControl1.Items[3].Enabled = false;//حفظ
            ribbonControl1.Items[4].Enabled = false;//حذف
            ribbonControl1.Items[5].Enabled = true;//الاول
            ribbonControl1.Items[6].Enabled = true;//التالي
            ribbonControl1.Items[7].Enabled = true;// السابق
            ribbonControl1.Items[8].Enabled = true;//الأخير-
            ribbonControl1.Items[9].Enabled = true;//بحث
            ribbonControl1.Items[10].Enabled = false;//طباعة
            ribbonControl1.Items[11].Enabled = false;//تصدير الرئيسي الزر
            ribbonControl1.Items[12].Enabled = true;//خروج
            ribbonControl1.Items[13].Enabled = false;//تصدير الى ملف اكسل
            ribbonControl1.Items[14].Enabled = false;//تصدير الى ملف بي دي اف
            ribbonControl1.Items[15].Enabled = false;//تصدير الى ملف نصي
            ribbonControl1.Items[16].Enabled = false;//تصدير الى ملف وورد
            ribbonControl1.Items[17].Enabled = false;//تصديىر الى ملف اتش تي ام ال
            ribbonControl1.Items[18].Enabled = false;//......
            ribbonControl1.Items[19].Enabled = true;//اضافة من
            ribbonControl1.Items[20].Enabled = false;//تراجع

        }
        /// <summary>
        ///  //النماذج التي فيها تعديل وليس اضافة مثل الصلاحيات
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="ribbonControl1"></param>
        public static void DoLoadRiponUpdateVisibilityForm(Form frm, DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1)
        {
            ribbonControl1.Items[1].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//جديد
            ribbonControl1.Items[4].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//حذف

            ribbonControl1.Items[5].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//الاول
            ribbonControl1.Items[6].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//التالي

            ribbonControl1.Items[7].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//السابق
            ribbonControl1.Items[8].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//التارلي

            ribbonControl1.Items[9].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//بحث
            ribbonControl1.Items[10].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//طباعة 

            ribbonControl1.Items[11].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//تصدير
            ribbonControl1.Items[13].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//تصدير

            ribbonControl1.Items[14].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//تصدير
            ribbonControl1.Items[15].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//تصدير

            ribbonControl1.Items[16].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//تصدير
            ribbonControl1.Items[17].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//تصدير

            ribbonControl1.Items[18].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//....
            ribbonControl1.Items[19].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//اضافة من




            ribbonControl1.Items[2].Enabled = true; //تعديل
            ribbonControl1.Items[3].Enabled = false;//حفظ
            ribbonControl1.Items[12].Enabled = true;//خروج
            ribbonControl1.Items[20].Enabled = false;//تراجع

        }
        /// <summary>
        ///  This function is used to set value  true Or false To Proprity enable  RibbonControl Items in a specific form that is sent to the function when Click the button Save.
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="ribbonControl1"></param>
        public static void DoSaveRipon(Form frm, DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1)
        {
            ribbonControl1.Items[1].Enabled = true; //جديد 
            ribbonControl1.Items[2].Enabled = false; //تعديل
            ribbonControl1.Items[3].Enabled = false;//حفظ
            ribbonControl1.Items[4].Enabled = false;//حذف
            ribbonControl1.Items[5].Enabled = false;//الاول
            ribbonControl1.Items[6].Enabled = false;//التالي
            ribbonControl1.Items[7].Enabled = false;// السابق
            ribbonControl1.Items[8].Enabled = false;//الأخير-
            ribbonControl1.Items[9].Enabled = false;//بحث
            ribbonControl1.Items[10].Enabled = true;//طباعة
            ribbonControl1.Items[11].Enabled = false;//تصدير الرئيسي الزر
            ribbonControl1.Items[12].Enabled = true;//خروج
            ribbonControl1.Items[13].Enabled = false;//تصدير الى ملف اكسل
            ribbonControl1.Items[14].Enabled = false;//تصدير الى ملف بي دي اف
            ribbonControl1.Items[15].Enabled = false;//تصدير الى ملف نصي
            ribbonControl1.Items[16].Enabled = false;//تصدير الى ملف وورد
            ribbonControl1.Items[17].Enabled = false;//تصديىر الى ملف اتش تي ام ال
            ribbonControl1.Items[18].Enabled = false;//......
            ribbonControl1.Items[19].Enabled = false;//اضافة من
            ribbonControl1.Items[20].Enabled = true;//تراجع



        }
        /// <summary>
        ///This function is used to set value  true Or false To Proprity enable  RibbonControl Items in a specific form that is sent to the function when Click the button Edit.
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="ribbonControl1"></param>
        public static void DoEditRipon(Form frm, DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1)
        {


            ribbonControl1.Items[1].Enabled = false; //جديد 
            ribbonControl1.Items[2].Enabled = false; //تعديل
            ribbonControl1.Items[3].Enabled = true;//حفظ
            ribbonControl1.Items[4].Enabled = false;//حذف
            ribbonControl1.Items[5].Enabled = false;//الاول
            ribbonControl1.Items[6].Enabled = false;//التالي
            ribbonControl1.Items[7].Enabled = false;// السابق
            ribbonControl1.Items[8].Enabled = false;//الأخير-
            ribbonControl1.Items[9].Enabled = false;//بحث
            ribbonControl1.Items[10].Enabled = false;//طباعة
            ribbonControl1.Items[11].Enabled = false;//تصدير الرئيسي الزر
            ribbonControl1.Items[12].Enabled = false;//خروج
            ribbonControl1.Items[13].Enabled = false;//تصدير الى ملف اكسل
            ribbonControl1.Items[14].Enabled = false;//تصدير الى ملف بي دي اف
            ribbonControl1.Items[15].Enabled = false;//تصدير الى ملف نصي
            ribbonControl1.Items[16].Enabled = true;//تصدير الى ملف وورد
            ribbonControl1.Items[17].Enabled = false;//تصديىر الى ملف اتش تي ام ال
            ribbonControl1.Items[18].Enabled = true;//......
            ribbonControl1.Items[19].Enabled = false;//اضافة من
            ribbonControl1.Items[20].Enabled = true;//تراجع

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="ribbonControl1"></param>
        public static void DoReadRipon(Form frm, DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1)
        {


            ribbonControl1.Items[1].Enabled = true; //جديد 
            ribbonControl1.Items[2].Enabled = true; //تعديل
            ribbonControl1.Items[3].Enabled = false;//حفظ
            ribbonControl1.Items[4].Enabled = true;//حذف
            ribbonControl1.Items[5].Enabled = true;//الاول
            ribbonControl1.Items[6].Enabled = true;//التالي
            ribbonControl1.Items[7].Enabled = true;// السابق
            ribbonControl1.Items[8].Enabled = true;//الأخير-
            ribbonControl1.Items[9].Enabled = true;//بحث
            ribbonControl1.Items[10].Enabled = true;//طباعة
            ribbonControl1.Items[11].Enabled = true;//تصدير الرئيسي الزر
            ribbonControl1.Items[12].Enabled = true;//خروج
            ribbonControl1.Items[13].Enabled = true;//تصدير الى ملف اكسل
            ribbonControl1.Items[14].Enabled = true;//تصدير الى ملف بي دي اف
            ribbonControl1.Items[15].Enabled = true;//تصدير الى ملف نصي
            ribbonControl1.Items[16].Enabled = true;//تصدير الى ملف وورد
            ribbonControl1.Items[17].Enabled = true;//تصديىر الى ملف اتش تي ام ال
            ribbonControl1.Items[18].Enabled = true;//......
            ribbonControl1.Items[19].Enabled = false;//اضافة من
            ribbonControl1.Items[20].Enabled = false;//تراجع



        }
        //بدون التصدير     
        public static void DoVisableWithOutExport(Form frm, DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1)
        {
            ribbonControl1.Items[1].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            ribbonControl1.Items[2].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            ribbonControl1.Items[3].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            ribbonControl1.Items[4].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            ribbonControl1.Items[5].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            ribbonControl1.Items[6].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            ribbonControl1.Items[7].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            ribbonControl1.Items[8].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;

            ribbonControl1.Items[9].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            ribbonControl1.Items[10].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;

            //التصدير
            ribbonControl1.Items[11].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            ribbonControl1.Items[12].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            ribbonControl1.Items[13].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            ribbonControl1.Items[14].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            ribbonControl1.Items[15].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            ribbonControl1.Items[16].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;

            ribbonControl1.Items[17].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            ribbonControl1.Items[18].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            ribbonControl1.Items[19].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            ribbonControl1.Items[20].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            ribbonControl1.Items[21].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="ribbonControl1"></param>
        public static void DoVisableOnlyMoveBotton(Form frm, DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1)
        {


            ribbonControl1.Items[1].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            ribbonControl1.Items[2].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            ribbonControl1.Items[3].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            ribbonControl1.Items[4].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            ribbonControl1.Items[5].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            ribbonControl1.Items[6].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            ribbonControl1.Items[7].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            ribbonControl1.Items[8].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;

            ribbonControl1.Items[9].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            ribbonControl1.Items[10].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;

            //التصدير
            ribbonControl1.Items[11].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            ribbonControl1.Items[12].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            ribbonControl1.Items[13].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            ribbonControl1.Items[14].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            ribbonControl1.Items[15].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            ribbonControl1.Items[16].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

            ribbonControl1.Items[17].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            ribbonControl1.Items[18].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            ribbonControl1.Items[19].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            ribbonControl1.Items[20].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            ribbonControl1.Items[21].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;


        }
        public static void DoVisable(Form frm, DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1)
        {


            ribbonControl1.Items[1].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            ribbonControl1.Items[2].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            ribbonControl1.Items[3].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            ribbonControl1.Items[4].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            ribbonControl1.Items[5].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            ribbonControl1.Items[6].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            ribbonControl1.Items[7].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            ribbonControl1.Items[8].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

            ribbonControl1.Items[9].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            ribbonControl1.Items[10].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;

            //التصدير
            ribbonControl1.Items[11].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            ribbonControl1.Items[12].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            ribbonControl1.Items[13].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            ribbonControl1.Items[14].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            ribbonControl1.Items[15].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            ribbonControl1.Items[16].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;

            ribbonControl1.Items[17].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            ribbonControl1.Items[18].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            ribbonControl1.Items[19].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            ribbonControl1.Items[20].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            ribbonControl1.Items[21].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

            ribbonControl1.Items[10].Enabled = true;//طباعة
            ribbonControl1.Items[12].Enabled = true;
        }
        public static void DoVisableForSetting(Form frm, DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1)
        {


            ribbonControl1.Items[1].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            ribbonControl1.Items[2].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            ribbonControl1.Items[3].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;

            ribbonControl1.Items[4].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            ribbonControl1.Items[5].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            ribbonControl1.Items[6].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            ribbonControl1.Items[7].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            ribbonControl1.Items[8].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

            ribbonControl1.Items[9].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            ribbonControl1.Items[10].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            ribbonControl1.Items[11].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            ribbonControl1.Items[12].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            ribbonControl1.Items[13].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            ribbonControl1.Items[14].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            ribbonControl1.Items[15].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            ribbonControl1.Items[16].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

            ribbonControl1.Items[17].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            ribbonControl1.Items[18].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            ribbonControl1.Items[19].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            ribbonControl1.Items[20].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            ribbonControl1.Items[21].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;


        }
        public static void EnabledControl(DevExpress.XtraTab.XtraTabPage Tap, bool Value)
        {


            foreach (Control item in Tap.Controls)
            {
                if (item is TextEdit)
                {
                    if (!(item.Name.Contains("lbl") && item.Name.Contains("Name")))
                    {
                        item.Enabled = Value;
                        ((TextEdit)item).Properties.AppearanceDisabled.ForeColor = Color.Black;
                        ((TextEdit)item).Properties.AppearanceDisabled.BackColor = Color.White;
                        if (Value == true)
                            ((TextEdit)item).Properties.AppearanceDisabled.BackColor = Color.White;
                    }
                }
                //if (item is DevExpress.XtraGrid.GridControl)
                //    item.Enabled = Value;

                if (item is System.Windows.Forms.ComboBox)
                    item.Enabled = Value;


                else if (item is SimpleButton && (((item.Name.Contains("btn"))) && ((item.Name.Contains("Search")))))
                {
                    ((SimpleButton)item).Enabled = Value;
                }


            }

        }
        public static void EnabledControl(System.Windows.Forms.GroupBox grbx, bool Value)
        {


            foreach (Control item in grbx.Controls)
            {
                if (item is TextEdit)
                {
                    if (!(item.Name.Contains("lbl") && item.Name.Contains("Name")))
                    {
                        item.Enabled = Value;
                        ((TextEdit)item).Properties.AppearanceDisabled.ForeColor = Color.Black;
                        ((TextEdit)item).Properties.AppearanceDisabled.BackColor = Color.White;
                        if (Value == true)
                            ((TextEdit)item).Properties.AppearanceDisabled.BackColor = Color.White;
                    }
                }
                //if (item is DevExpress.XtraGrid.GridControl)
                //    item.Enabled = Value;


                else if (item is SimpleButton && (((item.Name.Contains("btn"))) && ((item.Name.Contains("Search")))))
                {
                    ((SimpleButton)item).Enabled = Value;
                }


            }

        }
        /// <summary>
        /// This function to enable or disable a specific control is sent to the function with the boolean value of the enable.
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="Value"></param>
        public static void EnabledControl(Form frm, bool Value)
        {


            foreach (Control item in frm.Controls)
            {
                if (item is TextEdit)
                {
                    if (!(item.Name.Contains("lbl") && item.Name.Contains("Name")))
                    {

                        item.Enabled = Value;
                        ((TextEdit)item).Properties.AppearanceDisabled.ForeColor = Color.Black;
                        ((TextEdit)item).Properties.AppearanceDisabled.BackColor = Color.White;
                        if (Value == true)
                            ((TextEdit)item).Properties.AppearanceDisabled.BackColor = Color.White;
                    }
                }
                else if (item is System.Windows.Forms.TextBox)
                {
                    if (!(item.Name.Contains("lbl") && item.Name.Contains("Name")))
                    {

                        item.Enabled = Value;
                        ((System.Windows.Forms.TextBox)item).ForeColor = Color.Black;
                        ((System.Windows.Forms.TextBox)item).BackColor = Color.White;
                        if (Value == true)
                            ((System.Windows.Forms.TextBox)item).BackColor = Color.White;
                    }
                }
                else if (item is SimpleButton && (((item.Name.Contains("btn"))) && ((item.Name.Contains("Search")))))
                {
                    ((SimpleButton)item).Enabled = Value;
                }


            }

        }
      /// <summary>
      /// This function clear  the text of a specified control that is sent to the function along with the text value
      /// </summary>
      /// <param name="frm"></param>
      /// <param name="Value"></param>
        public static void ClearControl(Form frm, string Value)
        {
            foreach (Control item in frm.Controls)
            {
                string f = item.Name.Substring(0, 3);
                if (f == "lbl" || f == "txt")
                    item.Text = Value;
                else if (item is DateEdit)
                {
                    DateEdit d = (DateEdit)item;
                    InitializeFormatDate(d);
                }

                else if (item is System.Windows.Forms.CheckBox)
                {
                    System.Windows.Forms.CheckBox d = (System.Windows.Forms.CheckBox)item;
                    d.Checked = false;
                }
                else if (item is DevExpress.XtraEditors.CheckEdit)
                {
                    DevExpress.XtraEditors.CheckEdit c = (DevExpress.XtraEditors.CheckEdit)item;
                    c.Checked = false;
                }

            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="Value"></param>
        public static void ClearControl(DevExpress.XtraTab.XtraTabPage frm, string Value)
        {
            foreach (Control item in frm.Controls)
            {
                string f = item.Name.Substring(0, 3);
                if (f == "lbl" || f == "txt")
                    item.Text = Value;
                else if (item is DateEdit)
                {
                    DateEdit d = (DateEdit)item;
                    InitializeFormatDate(d);
                }

                else if (item is System.Windows.Forms.CheckBox)
                {
                    System.Windows.Forms.CheckBox d = (System.Windows.Forms.CheckBox)item;
                    d.Checked = false;
                }
                else if (item is DevExpress.XtraEditors.CheckEdit)
                {
                    DevExpress.XtraEditors.CheckEdit c = (DevExpress.XtraEditors.CheckEdit)item;
                    c.Checked = false;
                }
            }
        }
        public static bool IsValidForm(DevExpress.XtraTab.XtraTabPage from)
        {
            bool Validated = true;
            foreach (Control item in from.Controls)
            {
                if (item is LookUpEdit)
                {
                    if (item.Tag != null)
                    {
                        if (item.Tag.ToString().ToLower() == "ImportantField".ToLower())
                        {
                            if (((LookUpEdit)item).EditValue == null || ((LookUpEdit)item).EditValue == "" || (Comon.cInt(((LookUpEdit)item).EditValue) == 0 && Comon.cLong(((LookUpEdit)item).EditValue) == 0))


                            {
                                LookUpEdit txt = (LookUpEdit)item;
                                txt.Focus();
                                ToolTipController toolTip = new ToolTipController();
                                txt.ToolTipController = toolTip;
                                toolTip.Appearance.BackColor = Color.AntiqueWhite;
                                toolTip.ShowBeak = true;
                                toolTip.CloseOnClick = DefaultBoolean.True;
                                toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                                toolTip.InitialDelay = 500;
                                toolTip.ShowBeak = true;
                                toolTip.Rounded = true;
                                toolTip.ShowShadow = true;
                                toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                                toolTip.ToolTipType = ToolTipType.Standard;
                                toolTip.SetTitle(txt, "Error");
                                toolTip.ShowHint(Messages.msgInputIsRequired, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));
                                txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                Validated = false;
                            }
                        }

                    }
                }
                else if (item is TextEdit)
                {
                    if (item.Tag != null)
                    {
                        if (item.Tag.ToString().ToLower() == "IsNumber".ToLower())
                        {
                            double num;
                            if ((string.IsNullOrEmpty(item.Text.Trim()) || string.IsNullOrWhiteSpace(item.Text.Trim())))
                            {

                            }
                            else if (!(double.TryParse(item.Text, out num)))
                            {
                                TextEdit txt = (TextEdit)item;

                                txt.Focus();
                                ToolTipController toolTip = new ToolTipController();
                                txt.ToolTipController = toolTip;
                                toolTip.Appearance.BackColor = Color.AntiqueWhite;
                                toolTip.ShowBeak = true;
                                toolTip.CloseOnClick = DefaultBoolean.True;
                                toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                                toolTip.InitialDelay = 500;
                                toolTip.ShowBeak = true;
                                toolTip.Rounded = true;
                                toolTip.ShowShadow = true;
                                toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                                toolTip.ToolTipType = ToolTipType.Standard;
                                toolTip.SetTitle(txt, "Error");
                                toolTip.ShowHint(Messages.msgInputShouldBeNumber, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));
                                txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                Validated = false;
                            }


                        }

                        else if (item.Tag.ToString().ToLower() == "GreaterThanZero".ToLower())
                        {
                            double num;
                            if (!(double.TryParse(item.Text, out num)))
                            {
                                TextEdit txt = (TextEdit)item;
                                txt.Focus();
                                ToolTipController toolTip = new ToolTipController();
                                txt.ToolTipController = toolTip;
                                toolTip.Appearance.BackColor = Color.AntiqueWhite;
                                toolTip.ShowBeak = true;
                                toolTip.CloseOnClick = DefaultBoolean.True;
                                toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                                toolTip.InitialDelay = 500;
                                toolTip.ShowBeak = true;
                                toolTip.Rounded = true;
                                toolTip.ShowShadow = true;
                                toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                                toolTip.ToolTipType = ToolTipType.Standard;
                                toolTip.SetTitle(txt, "Error");
                                toolTip.ShowHint(Messages.msgInputShouldBeNumber, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));
                                txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                Validated = false;
                            }
                            else if (Comon.cDbl(item.Text.Trim()) <= 0)
                            {
                                TextEdit txt = (TextEdit)item;
                                txt.Focus();
                                ToolTipController toolTip = new ToolTipController();
                                txt.ToolTipController = toolTip;
                                toolTip.Appearance.BackColor = Color.AntiqueWhite;
                                toolTip.ShowBeak = true;
                                toolTip.CloseOnClick = DefaultBoolean.True;
                                toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                                toolTip.InitialDelay = 500;
                                toolTip.ShowBeak = true;
                                toolTip.Rounded = true;
                                toolTip.ShowShadow = true;
                                toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                                toolTip.ToolTipType = ToolTipType.Standard;
                                toolTip.SetTitle(txt, "Error");
                                toolTip.ShowHint(Messages.msgInputIsGreaterThanZero, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));
                                txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                Validated = false;
                            }
                        }
                        else if (item.Tag.ToString().ToLower() == "ImportantField".ToLower())
                        {

                            if ((string.IsNullOrEmpty(item.Text.Trim()) || string.IsNullOrWhiteSpace(item.Text.Trim())))
                            {
                                TextEdit txt = (TextEdit)item;
                                txt.Focus();
                                ToolTipController toolTip = new ToolTipController();
                                txt.ToolTipController = toolTip;
                                toolTip.Appearance.BackColor = Color.AntiqueWhite;
                                toolTip.ShowBeak = true;
                                toolTip.CloseOnClick = DefaultBoolean.True;
                                toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                                toolTip.InitialDelay = 500;
                                toolTip.ShowBeak = true;
                                toolTip.Rounded = true;
                                toolTip.ShowShadow = true;
                                toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                                toolTip.ToolTipType = ToolTipType.Standard;
                                toolTip.SetTitle(txt, "Error");
                                toolTip.ShowHint(Messages.msgInputIsRequired, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));

                                txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                Validated = false;
                            }
                        }
                        else if (item.Tag.ToString().ToLower() == "ImportantFieldGreaterThanZero".ToLower())
                        {
                            double num;
                            TextEdit txat = (TextEdit)item;
                            string aa = txat.Name;
                            if ((string.IsNullOrEmpty(item.Text.Trim()) || string.IsNullOrWhiteSpace(item.Text.Trim())))
                            {
                                TextEdit txt = (TextEdit)item;
                                string a = txt.Name;
                                txt.Focus();
                                ToolTipController toolTip = new ToolTipController();
                                txt.ToolTipController = toolTip;
                                toolTip.Appearance.BackColor = Color.AntiqueWhite;
                                toolTip.ShowBeak = true;
                                toolTip.CloseOnClick = DefaultBoolean.True;
                                toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                                toolTip.InitialDelay = 500;
                                toolTip.ShowBeak = true;
                                toolTip.Rounded = true;
                                toolTip.ShowShadow = true;
                                toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                                toolTip.ToolTipType = ToolTipType.Standard;
                                toolTip.SetTitle(txt, "Error");
                                toolTip.ShowHint(Messages.msgInputIsRequired, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));
                                txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                Validated = false;
                            }

                            else if (!(Double.TryParse(item.Text, out num)))
                            {
                                TextEdit txt = (TextEdit)item;
                                txt.Focus();
                                ToolTipController toolTip = new ToolTipController();
                                txt.ToolTipController = toolTip;
                                toolTip.Appearance.BackColor = Color.AntiqueWhite;
                                toolTip.ShowBeak = true;
                                toolTip.CloseOnClick = DefaultBoolean.True;
                                toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                                toolTip.InitialDelay = 500;
                                toolTip.ShowBeak = true;
                                toolTip.Rounded = true;
                                toolTip.ShowShadow = true;
                                toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                                toolTip.ToolTipType = ToolTipType.Standard;
                                toolTip.SetTitle(txt, "Error");
                                toolTip.ShowHint(Messages.msgInputShouldBeNumber, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));
                                txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                Validated = false;

                            }
                            else if (Comon.cDbl(item.Text.Trim()) <= 0)
                            {
                                TextEdit txt = (TextEdit)item;
                                txt.Focus();
                                ToolTipController toolTip = new ToolTipController();
                                txt.ToolTipController = toolTip;
                                toolTip.Appearance.BackColor = Color.AntiqueWhite;
                                toolTip.ShowBeak = true;
                                toolTip.CloseOnClick = DefaultBoolean.True;
                                toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                                toolTip.InitialDelay = 500;
                                toolTip.ShowBeak = true;
                                toolTip.Rounded = true;
                                toolTip.ShowShadow = true;
                                toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                                toolTip.ToolTipType = ToolTipType.Standard;
                                toolTip.SetTitle(txt, "Error");
                                toolTip.ShowHint(Messages.msgInputIsGreaterThanZero, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));
                                txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                Validated = false;
                            }

                        }
                    }
                }

            }

            return Validated;
        }
        public static bool IsValidForm(System.Windows.Forms.TabPage from)
        {
            bool Validated = true;
            foreach (Control item in from.Controls)
            {
                if (item is LookUpEdit)
                {
                    if (item.Tag != null)
                    {
                        if (item.Tag.ToString().ToLower() == "ImportantField".ToLower())
                        {
                            if (((LookUpEdit)item).EditValue == null || ((LookUpEdit)item).EditValue == "" || (Comon.cInt(((LookUpEdit)item).EditValue) == 0 && Comon.cLong(((LookUpEdit)item).EditValue) == 0))


                            {
                                LookUpEdit txt = (LookUpEdit)item;
                                txt.Focus();
                                ToolTipController toolTip = new ToolTipController();
                                txt.ToolTipController = toolTip;
                                toolTip.Appearance.BackColor = Color.AntiqueWhite;
                                toolTip.ShowBeak = true;
                                toolTip.CloseOnClick = DefaultBoolean.True;
                                toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                                toolTip.InitialDelay = 500;
                                toolTip.ShowBeak = true;
                                toolTip.Rounded = true;
                                toolTip.ShowShadow = true;
                                toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                                toolTip.ToolTipType = ToolTipType.Standard;
                                toolTip.SetTitle(txt, "Error");
                                toolTip.ShowHint(Messages.msgInputIsRequired, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));
                                txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                Validated = false;
                            }
                        }

                    }
                }
                else if (item is TextEdit)
                {
                    if (item.Tag != null)
                    {
                        if (item.Tag.ToString().ToLower() == "IsNumber".ToLower())
                        {
                            double num;
                            if ((string.IsNullOrEmpty(item.Text.Trim()) || string.IsNullOrWhiteSpace(item.Text.Trim())))
                            {

                            }
                            else if (!(double.TryParse(item.Text, out num)))
                            {
                                TextEdit txt = (TextEdit)item;

                                txt.Focus();
                                ToolTipController toolTip = new ToolTipController();
                                txt.ToolTipController = toolTip;
                                toolTip.Appearance.BackColor = Color.AntiqueWhite;
                                toolTip.ShowBeak = true;
                                toolTip.CloseOnClick = DefaultBoolean.True;
                                toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                                toolTip.InitialDelay = 500;
                                toolTip.ShowBeak = true;
                                toolTip.Rounded = true;
                                toolTip.ShowShadow = true;
                                toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                                toolTip.ToolTipType = ToolTipType.Standard;
                                toolTip.SetTitle(txt, "Error");
                                toolTip.ShowHint(Messages.msgInputShouldBeNumber, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));
                                txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                Validated = false;
                            }


                        }

                        else if (item.Tag.ToString().ToLower() == "GreaterThanZero".ToLower())
                        {
                            double num;
                            if (!(double.TryParse(item.Text, out num)))
                            {
                                TextEdit txt = (TextEdit)item;
                                txt.Focus();
                                ToolTipController toolTip = new ToolTipController();
                                txt.ToolTipController = toolTip;
                                toolTip.Appearance.BackColor = Color.AntiqueWhite;
                                toolTip.ShowBeak = true;
                                toolTip.CloseOnClick = DefaultBoolean.True;
                                toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                                toolTip.InitialDelay = 500;
                                toolTip.ShowBeak = true;
                                toolTip.Rounded = true;
                                toolTip.ShowShadow = true;
                                toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                                toolTip.ToolTipType = ToolTipType.Standard;
                                toolTip.SetTitle(txt, "Error");
                                toolTip.ShowHint(Messages.msgInputShouldBeNumber, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));
                                txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                Validated = false;
                            }
                            else if (Comon.cDbl(item.Text.Trim()) <= 0)
                            {
                                TextEdit txt = (TextEdit)item;
                                txt.Focus();
                                ToolTipController toolTip = new ToolTipController();
                                txt.ToolTipController = toolTip;
                                toolTip.Appearance.BackColor = Color.AntiqueWhite;
                                toolTip.ShowBeak = true;
                                toolTip.CloseOnClick = DefaultBoolean.True;
                                toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                                toolTip.InitialDelay = 500;
                                toolTip.ShowBeak = true;
                                toolTip.Rounded = true;
                                toolTip.ShowShadow = true;
                                toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                                toolTip.ToolTipType = ToolTipType.Standard;
                                toolTip.SetTitle(txt, "Error");
                                toolTip.ShowHint(Messages.msgInputIsGreaterThanZero, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));
                                txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                Validated = false;
                            }
                        }
                        else if (item.Tag.ToString().ToLower() == "ImportantField".ToLower())
                        {

                            if ((string.IsNullOrEmpty(item.Text.Trim()) || string.IsNullOrWhiteSpace(item.Text.Trim())))
                            {
                                TextEdit txt = (TextEdit)item;
                                txt.Focus();
                                ToolTipController toolTip = new ToolTipController();
                                txt.ToolTipController = toolTip;
                                toolTip.Appearance.BackColor = Color.AntiqueWhite;
                                toolTip.ShowBeak = true;
                                toolTip.CloseOnClick = DefaultBoolean.True;
                                toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                                toolTip.InitialDelay = 500;
                                toolTip.ShowBeak = true;
                                toolTip.Rounded = true;
                                toolTip.ShowShadow = true;
                                toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                                toolTip.ToolTipType = ToolTipType.Standard;
                                toolTip.SetTitle(txt, "Error");
                                toolTip.ShowHint(Messages.msgInputIsRequired, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));

                                txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                Validated = false;
                            }
                        }
                        else if (item.Tag.ToString().ToLower() == "ImportantFieldGreaterThanZero".ToLower())
                        {
                            double num;
                            TextEdit txat = (TextEdit)item;
                            string aa = txat.Name;
                            if ((string.IsNullOrEmpty(item.Text.Trim()) || string.IsNullOrWhiteSpace(item.Text.Trim())))
                            {
                                TextEdit txt = (TextEdit)item;
                                string a = txt.Name;
                                txt.Focus();
                                ToolTipController toolTip = new ToolTipController();
                                txt.ToolTipController = toolTip;
                                toolTip.Appearance.BackColor = Color.AntiqueWhite;
                                toolTip.ShowBeak = true;
                                toolTip.CloseOnClick = DefaultBoolean.True;
                                toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                                toolTip.InitialDelay = 500;
                                toolTip.ShowBeak = true;
                                toolTip.Rounded = true;
                                toolTip.ShowShadow = true;
                                toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                                toolTip.ToolTipType = ToolTipType.Standard;
                                toolTip.SetTitle(txt, "Error");
                                toolTip.ShowHint(Messages.msgInputIsRequired, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));
                                txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                Validated = false;
                            }

                            else if (!(Double.TryParse(item.Text, out num)))
                            {
                                TextEdit txt = (TextEdit)item;
                                txt.Focus();
                                ToolTipController toolTip = new ToolTipController();
                                txt.ToolTipController = toolTip;
                                toolTip.Appearance.BackColor = Color.AntiqueWhite;
                                toolTip.ShowBeak = true;
                                toolTip.CloseOnClick = DefaultBoolean.True;
                                toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                                toolTip.InitialDelay = 500;
                                toolTip.ShowBeak = true;
                                toolTip.Rounded = true;
                                toolTip.ShowShadow = true;
                                toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                                toolTip.ToolTipType = ToolTipType.Standard;
                                toolTip.SetTitle(txt, "Error");
                                toolTip.ShowHint(Messages.msgInputShouldBeNumber, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));
                                txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                Validated = false;

                            }
                            else if (Comon.cDbl(item.Text.Trim()) <= 0)
                            {
                                TextEdit txt = (TextEdit)item;
                                txt.Focus();
                                ToolTipController toolTip = new ToolTipController();
                                txt.ToolTipController = toolTip;
                                toolTip.Appearance.BackColor = Color.AntiqueWhite;
                                toolTip.ShowBeak = true;
                                toolTip.CloseOnClick = DefaultBoolean.True;
                                toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                                toolTip.InitialDelay = 500;
                                toolTip.ShowBeak = true;
                                toolTip.Rounded = true;
                                toolTip.ShowShadow = true;
                                toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                                toolTip.ToolTipType = ToolTipType.Standard;
                                toolTip.SetTitle(txt, "Error");
                                toolTip.ShowHint(Messages.msgInputIsGreaterThanZero, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));
                                txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                Validated = false;
                            }

                        }
                    }
                }

            }

            return Validated;
        }
        public static bool Important(DevExpress.XtraTab.XtraTabPage from)
        {
            bool Validated = true;
            foreach (Control item in from.Controls)
            {
                if (item is TextEdit)
                {
                    if (item.Tag != null)
                    {
                        if (item.Tag.ToString().ToLower() == "IsNumber".ToLower())
                        {
                            int num;
                            if (!(int.TryParse(item.Text, out num)))
                            {
                                TextEdit txt = (TextEdit)item;
                                txt.Focus();
                                ToolTipController toolTip = new ToolTipController();
                                txt.ToolTipController = toolTip;
                                toolTip.Appearance.BackColor = Color.AntiqueWhite;
                                toolTip.ShowBeak = true;
                                toolTip.CloseOnClick = DefaultBoolean.True;
                                toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                                toolTip.InitialDelay = 500;
                                toolTip.ShowBeak = true;
                                toolTip.Rounded = true;
                                toolTip.ShowShadow = true;
                                toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                                toolTip.ToolTipType = ToolTipType.Standard;
                                toolTip.SetTitle(txt, "Error");
                                toolTip.ShowHint(Messages.msgInputShouldBeNumber, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));
                                txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                Validated = false;
                            }

                        }

                        else if (item.Tag.ToString().ToLower() == "GreaterThanZero".ToLower())
                        {
                            double num;
                            if (!(double.TryParse(item.Text, out num)))
                            {
                                TextEdit txt = (TextEdit)item;
                                txt.Focus();
                                ToolTipController toolTip = new ToolTipController();
                                txt.ToolTipController = toolTip;
                                toolTip.Appearance.BackColor = Color.AntiqueWhite;
                                toolTip.ShowBeak = true;
                                toolTip.CloseOnClick = DefaultBoolean.True;
                                toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                                toolTip.InitialDelay = 500;
                                toolTip.ShowBeak = true;
                                toolTip.Rounded = true;
                                toolTip.ShowShadow = true;
                                toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                                toolTip.ToolTipType = ToolTipType.Standard;
                                toolTip.SetTitle(txt, "Error");
                                toolTip.ShowHint(Messages.msgInputShouldBeNumber, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));
                                txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                Validated = false;
                            }
                            else if (Comon.cDbl(item.Text.Trim()) <= 0)
                            {
                                TextEdit txt = (TextEdit)item;
                                txt.Focus();
                                ToolTipController toolTip = new ToolTipController();
                                txt.ToolTipController = toolTip;
                                toolTip.Appearance.BackColor = Color.AntiqueWhite;
                                toolTip.ShowBeak = true;
                                toolTip.CloseOnClick = DefaultBoolean.True;
                                toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                                toolTip.InitialDelay = 500;
                                toolTip.ShowBeak = true;
                                toolTip.Rounded = true;
                                toolTip.ShowShadow = true;
                                toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                                toolTip.ToolTipType = ToolTipType.Standard;
                                toolTip.SetTitle(txt, "Error");
                                toolTip.ShowHint(Messages.msgInputIsGreaterThanZero, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));
                                txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                Validated = false;
                            }
                        }
                        else if (item.Tag.ToString().ToLower() == "ImportantField".ToLower())
                        {

                            if ((string.IsNullOrEmpty(item.Text.Trim()) || string.IsNullOrWhiteSpace(item.Text.Trim())))
                            {
                                TextEdit txt = (TextEdit)item;
                                txt.Focus();
                                ToolTipController toolTip = new ToolTipController();
                                txt.ToolTipController = toolTip;
                                toolTip.Appearance.BackColor = Color.AntiqueWhite;
                                toolTip.ShowBeak = true;
                                toolTip.CloseOnClick = DefaultBoolean.True;
                                toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                                toolTip.InitialDelay = 500;
                                toolTip.ShowBeak = true;
                                toolTip.Rounded = true;
                                toolTip.ShowShadow = true;
                                toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                                toolTip.ToolTipType = ToolTipType.Standard;
                                toolTip.SetTitle(txt, "Error");
                                toolTip.ShowHint(Messages.msgInputIsRequired, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));

                                txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                Validated = false;
                            }
                        }
                        else if (item.Tag.ToString().ToLower() == "ImportantFieldGreaterThanZero".ToLower())
                        {
                            double num;
                            if ((string.IsNullOrEmpty(item.Text.Trim()) || string.IsNullOrWhiteSpace(item.Text.Trim())))
                            {
                                TextEdit txt = (TextEdit)item;
                                txt.Focus();
                                ToolTipController toolTip = new ToolTipController();
                                txt.ToolTipController = toolTip;
                                toolTip.Appearance.BackColor = Color.AntiqueWhite;
                                toolTip.ShowBeak = true;
                                toolTip.CloseOnClick = DefaultBoolean.True;
                                toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                                toolTip.InitialDelay = 500;
                                toolTip.ShowBeak = true;
                                toolTip.Rounded = true;
                                toolTip.ShowShadow = true;
                                toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                                toolTip.ToolTipType = ToolTipType.Standard;
                                toolTip.SetTitle(txt, "Error");
                                toolTip.ShowHint(Messages.msgInputIsRequired, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));
                                txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                Validated = false;
                            }

                            else if (!(Double.TryParse(item.Text, out num)))
                            {
                                TextEdit txt = (TextEdit)item;
                                txt.Focus();
                                ToolTipController toolTip = new ToolTipController();
                                txt.ToolTipController = toolTip;
                                toolTip.Appearance.BackColor = Color.AntiqueWhite;
                                toolTip.ShowBeak = true;
                                toolTip.CloseOnClick = DefaultBoolean.True;
                                toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                                toolTip.InitialDelay = 500;
                                toolTip.ShowBeak = true;
                                toolTip.Rounded = true;
                                toolTip.ShowShadow = true;
                                toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                                toolTip.ToolTipType = ToolTipType.Standard;
                                toolTip.SetTitle(txt, "Error");
                                toolTip.ShowHint(Messages.msgInputShouldBeNumber, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));
                                txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                Validated = false;

                            }
                            else if (Comon.cDbl(item.Text.Trim()) <= 0)
                            {
                                TextEdit txt = (TextEdit)item;
                                txt.Focus();
                                ToolTipController toolTip = new ToolTipController();
                                txt.ToolTipController = toolTip;
                                toolTip.Appearance.BackColor = Color.AntiqueWhite;
                                toolTip.ShowBeak = true;
                                toolTip.CloseOnClick = DefaultBoolean.True;
                                toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                                toolTip.InitialDelay = 500;
                                toolTip.ShowBeak = true;
                                toolTip.Rounded = true;
                                toolTip.ShowShadow = true;
                                toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                toolTip.SetToolTipIconType(txt, ToolTipIconType.Error);
                                toolTip.ToolTipType = ToolTipType.Standard;
                                toolTip.SetTitle(txt, "Error");
                                toolTip.ShowHint(Messages.msgInputIsGreaterThanZero, ToolTipLocation.TopLeft, txt.PointToScreen(new Point(0, txt.Height)));
                                txt.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                                Validated = false;
                            }

                        }
                    }
                }
            }

            return Validated;
        }
        /// <summary>
        /// This Function To set Text error
        /// </summary>
        /// <param name="item"></param>
        /// <param name="Msg"></param>
        public static void ErrorTextClear(TextEdit item, string Msg)
        {
            if (item is TextEdit)
            {
                TextEdit txt = (TextEdit)item;
                ToolTipController toolTip = new ToolTipController();
                txt.ToolTipController = toolTip;
                txt.Properties.Appearance.BorderColor = Color.Black;
            }

        }
        /// <summary>
        /// This method initializes the DateEdit control with a specific date format and value
        /// </summary>
        /// <param name="Obj"></param>
        public static void InitializeFormatDate(DateEdit Obj)
        {
            // Set the UseMaskAsDisplayFormat property to true to make the display format use the specified mask
            Obj.Properties.Mask.UseMaskAsDisplayFormat = true;
            // Set the display format to "dd/MM/yyyy"
            Obj.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            Obj.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            // Set the edit format to "dd/MM/yyyy"
            Obj.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            Obj.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            // Set the mask to "dd/MM/yyyy" and the mask type to DateTimeAdvancingCaret
            Obj.Properties.Mask.EditMask = "dd/MM/yyyy";
            Obj.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            // Set the control's value to the current date and time
            Obj.EditValue = DateTime.Now;
        }

    }
}

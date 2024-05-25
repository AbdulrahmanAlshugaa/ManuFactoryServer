using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraWaitForm;
using DevExpress.LookAndFeel;
using Edex.DAL.Common;

namespace Edex.GeneralObjects.GeneralForms
{
    public partial class WaitForm1 : WaitForm
    {
        public WaitForm1()
        {
            GetSkinName();
            InitializeComponent();
            this.progressPanel1.AutoHeight = true;
        
        }
        void GetSkinName()
        {
            string Path = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
            UserLookAndFeel.Default.SetSkinStyle(SystemSettings.GetSkinName(Path));
        }
        #region Overrides

        public override void SetCaption(string caption)
        {
           
            base.SetCaption(caption);
            this.progressPanel1.Caption = caption;
        }
        public override void SetDescription(string description)
        {
            base.SetDescription(description);
            this.progressPanel1.Description = description;
        }
        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);
        }

        #endregion

        public enum WaitFormCommand
        {
        }
    }
}
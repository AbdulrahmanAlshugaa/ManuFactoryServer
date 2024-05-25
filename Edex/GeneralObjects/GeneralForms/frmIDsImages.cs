using DevExpress.CodeParser;
using DevExpress.XtraSplashScreen;
using Edex.GeneralObjects.GeneralClasses;
using Edex.Model;
using Edex.Model.Language;
using Edex.ModelSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Edex.GeneralObjects.GeneralForms
{
    public partial class frmIDsImages : Edex.GeneralObjects.GeneralForms.BaseForm
    {
       
        public long SCREENNO;
        public string IDNo;
        string strSQL = "";
        string FocusedControl = "";
        public const int xMoveFirst = 7;
        public const int xMovePrev = 8;
        public const int xMoveNext = 9;
        public const int xMoveLast = 10;
        public bool IsFromanotherForms = false;
        private bool IsNewRecord;
        public frmIDsImages()
        {
            InitializeComponent();
        }
        private void lnkBrowse_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Multiselect = true;
            openFileDialog1.Filter = "All Files|*.*|Bitmaps|*.bmp|GIFs|*.gif|JPEGs|*.jpg";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string[] files = openFileDialog1.FileNames;

                foreach (String file in openFileDialog1.FileNames)
                {
                    
                
                    PictureBox1.Image  = System.Drawing.Image.FromFile(file);

                  
                    //SaveDataIntoIdsImagesTable(pic);
                   
                }
            }
              

        }
        public static byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }

        public static System.Drawing.Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
            return returnImage;
        }
        protected override void DoNew()
        {
            try
            {
                txtbarcodImage.Text = "";
                txtNotes.Text = "";
                txtImageID.Text = Comon.cInt(Lip.GetValue("SELECT max([ImageID])+1  FROM  MNG_ARCHIVINGDOCUMENTSIMAGES where BranchID="+MySession.GlobalBranchID)).ToString();
                PictureBox1.Image = null;
                IsNewRecord = true;
                lnkBrowse_LinkClicked(null, null);
            }
            catch
            {
            }
        }
        protected override void DoEdit()
        {
            Validations.DoEditRipon(this, ribbonControl1);
            IsNewRecord = false;
        }

        protected override void DoSave()
        {
            if (string.IsNullOrEmpty(txtbarcodImage.Text.Trim()))
            {
                Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "الرجاء إدخال باركود للصورة !" : "Please inpute the BarCode Image !");
                return;
            }
            strSQL = "Select Top 1 ImageID From  MNG_ARCHIVINGDOCUMENTSIMAGES where BranchID =" + MySession.GlobalBranchID + "  And ImageCode ='" + txtbarcodImage.Text + "'    Order By ID";
            DataTable dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0 &&Comon.cInt( dt.Rows[0]["ImageID"])!=Comon.cInt(txtImageID.Text))
            {
                Messages.MsgError(this.GetType().Name, " الكود موجود مسبقا");
            }
            else
            {
                if(IsNewRecord)
                {
                    Lip.NewFields();
                    Lip.Table = "MNG_ARCHIVINGDOCUMENTSIMAGES";
                    Lip.AddNumericField("SCREENNO", SCREENNO.ToString());
                    Lip.AddNumericField("BranchID", UserInfo.BRANCHID);
                    Lip.AddStringField("IDNo", "0");
                    Lip.AddNumericField("ImageID", txtImageID.Text);
                    Lip.AddStringField("ImageCode", txtbarcodImage.Text);
                    Lip.AddStringField("Notes", txtNotes.Text);
                    if (checkEdit1.Checked == true)
                        Lip.AddNumericField("ApprovalImage", 1);
                    else
                        Lip.AddNumericField("ApprovalImage", 0);                       
                    Lip.ExecuteInsert();
                    Byte[] data = imageToByteArray(PictureBox1.Image);
                    SaveImage(data,Comon.cInt(txtImageID.Text));
                }
               else
                UpdateImage();
            }
        }
       

        private void UpdateImage()
        {
            Lip.NewFields();
            Lip.Table = "MNG_ARCHIVINGDOCUMENTSIMAGES";
            Lip.AddStringField("Notes", txtNotes.Text);
            Lip.AddStringField("ImageCode", txtbarcodImage.Text);
            if (checkEdit1.Checked == true)
                Lip.AddNumericField("ApprovalImage",1);
            else
                Lip.AddNumericField("ApprovalImage",0);
            if (PictureBox1.Image != null)
            {
                Lip.sCondition = "BranchID =" + MySession.GlobalBranchID + " AND SCREENNO =" + SCREENNO + "  AND ImageID =" + Comon.cInt(txtImageID.Text);
                Lip.ExecuteUpdate();
                Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
            }
            else
            {
                Messages.MsgError(this.GetType().Name, "يجب تحديد صورة");
            }
        }
        private void SaveImage(byte[] data, int ImageID)
        {
            try
            {
                string WHERE = " SCREENNO = " + SCREENNO +  "  AND ImageID = " + ImageID;
                Lip.SaveFile(data, "MNG_ARCHIVINGDOCUMENTSIMAGES", "TheImage", MySession.GlobalBranchID, WHERE);
                Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
            }
            catch
            {

            }
        }

        private void frmIDsImages_Load(object sender, EventArgs e)
        {
            txtImageID.Text = Comon.cInt(Lip.GetValue("SELECT max([ImageID])+1  FROM  MNG_ARCHIVINGDOCUMENTSIMAGES where BranchID =" + MySession.GlobalBranchID)).ToString();
        }

        private void frmIDsImages_Activated(object sender, EventArgs e)
        {

        }

        private void frmIDsImages_Shown(object sender, EventArgs e)
        {
            txtModelNo.Text = SCREENNO.ToString();
          
        }

        private void btnstripDelete_Click(object sender, EventArgs e)
        {
             if (!FormDelete)
                {
                    Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToDeleteRecord);
                    return;
                }
             else
             {
                PictureBox1.Image = null;
                Lip.NewFields();
                Lip.Table = "MNG_ARCHIVINGDOCUMENTSIMAGES";
                Lip.sCondition = "BranchID =" + MySession.GlobalBranchID + " AND SCREENNO =" + SCREENNO + " AND IDNo ='" + IDNo + "' AND ImageID =" + Comon.cInt(txtImageID.Text);
                Lip.ExecuteDelete();
                Messages.MsgInfo(Messages.TitleInfo, Messages.msgDeleteComplete);
                
            }

        }

        public void MoveRec(long PremaryKeyValue, int Direction)
        {
            try
            {
                #region If
                if (FormView == true)
                {
                    strSQL = "SELECT TOP 1 * FROM  MNG_ARCHIVINGDOCUMENTSIMAGES where    BranchID ="+ MySession.GlobalBranchID;
                    switch (Direction)
                    {
                        case xMoveFirst:
                            {
                                strSQL = strSQL + " ORDER BY ImageID ASC";
                                break;
                            }

                        case xMoveNext:
                            {
                                strSQL = strSQL + " And  ImageID   > " + PremaryKeyValue + " ORDER BY  ImageID asc";
                                break;
                            }

                        case xMovePrev:
                            {
                                strSQL = strSQL + " And  ImageID <" + PremaryKeyValue + " ORDER BY ImageID desc";
                                break;
                            }

                        case xMoveLast:
                            {
                                strSQL = strSQL + " ORDER BY  ImageID DESC";
                                break;
                            }
                    }
                    DataTable dt = Lip.SelectRecord(strSQL);
                    if (dt.Rows.Count > 0)
                    {
                        int ImageID = Comon.cInt(dt.Rows[0]["ImageID"].ToString());
                        PictureBox pic = new PictureBox();
                        Byte[] imgByte = new Byte[] { };
                        txtNotes.Text = "";
                        txtbarcodImage.Text = "";
                    
                            txtImageID.Text = dt.Rows[0]["ImageID"].ToString();
                            txtNotes.Text = dt.Rows[0]["Notes"].ToString();
                            txtbarcodImage.Text = dt.Rows[0]["ImageCode"].ToString();
                            imgByte = (Byte[])(dt.Rows[0]["TheImage"]);
                            pic.Image = byteArrayToImage(imgByte);
                            PictureBox1.Image = pic.Image;
                            checkEdit1.Checked = false;
                            if (Comon.cInt(dt.Rows[0]["ApprovalImage"].ToString()) == 1)
                                checkEdit1.Checked = true;
                             else
                                checkEdit1.Checked = false;
                            Validations.DoReadRipon(this, ribbonControl1);

                    }

                }
                #endregion
                else
                {
                    Messages.MsgStop(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord);
                    return;
                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        protected override void DoLast()
        {
            try
            {
                MoveRec(0, xMoveLast);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        protected override void DoFirst()
        {
            try
            {
                MoveRec(0, xMoveFirst);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        protected override void DoNext()
        {
            try
            {
                MoveRec(Comon.cInt(txtImageID.Text), xMoveNext);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        protected override void DoPrevious()
        {
            try
            {
                MoveRec(Comon.cInt(txtImageID.Text), xMovePrev);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        protected override void DoDelete()
        {
            try
            {

                if (!FormDelete)
                {
                    Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToDeleteRecord);
                    return;
                }
                else
                {
                    bool Yes = Messages.MsgStopYesNo(Messages.TitleConfirm, Messages.msgConfirmDelete);
                    if (!Yes)
                        return;
                }
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                int ImageID = Comon.cInt(txtImageID.Text);
                Lip.NewFields();
                Lip.Table = "MNG_ARCHIVINGDOCUMENTSIMAGES";
                Lip.sCondition = "BranchID =" + MySession.GlobalBranchID + " AND SCREENNO =" + SCREENNO + " AND IDNo ='" + IDNo + "' AND ImageID =" + ImageID;
                Lip.ExecuteDelete();
                txtbarcodImage.Text = "";
                txtNotes.Text = "";
                txtImageID.Text = Comon.cInt(Lip.GetValue("SELECT max([ImageID])+1  FROM  MNG_ARCHIVINGDOCUMENTSIMAGES  BranchID =" + MySession.GlobalBranchID)).ToString();
                PictureBox1.Image = null;
                IsNewRecord = true;
                SplashScreenManager.CloseForm(false);
               

            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }
        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            
               
        }
        public void Find()
        {

            CSearch cls = new CSearch();
            int[] ColumnWidth = new int[] { 100, 300 };
            string SearchSql = "";
            string Condition = "Where 1=1";

            FocusedControl = GetIndexFocusedControl();

            if (FocusedControl == null) return;


            else if (FocusedControl.Trim() == txtImageID.Name)
            {
                //if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; };


                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtImageID, null, "ImageIDInDesignFactory", "رقم الصورة", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtImageID, null, "ImageIDInDesignFactory", "Image ID", MySession.GlobalBranchID);
            }
           

            

            GetSelectedSearchValue(cls);
        }
        string GetIndexFocusedControl()
        {
            Control c = this.ActiveControl;
            if (c == null) return null;
            if (c is DevExpress.XtraLayout.LayoutControl)
            {
                if (!(((DevExpress.XtraLayout.LayoutControl)ActiveControl).ActiveControl == null))
                {
                    c = ((DevExpress.XtraLayout.LayoutControl)ActiveControl).ActiveControl;
                }
            }
            if (c is DevExpress.XtraEditors.TextBoxMaskBox)
            {
                c = c.Parent;
            }
            if (c.Parent is DevExpress.XtraGrid.GridControl)
            {
                return c.Parent.Name;
            }
            return c.Name;
        }
        public void GetSelectedSearchValue(CSearch cls)
        {
            if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
            {
               
                  if (FocusedControl ==txtImageID.Name)
                {
                    txtbarcodImage.Text = cls.PrimaryKeyValue.ToString();
                    txtImageID_Validating(null, null);
                }

            }
        }
        private void txtImageID_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtbarcodImage.Text) == false)
            {
                DataTable dt = Lip.SelectRecord("SELECT TOP 1 * FROM  MNG_ARCHIVINGDOCUMENTSIMAGES where BranchID =" + MySession.GlobalBranchID + "  And ImageID ='" + txtImageID.Text + "'");
                if (dt.Rows.Count > 0)
                {
                    int ImageID = Comon.cInt(dt.Rows[0]["ImageID"].ToString());
                    PictureBox pic = new PictureBox();
                    Byte[] imgByte = new Byte[] { };
                    txtNotes.Text = "";
                    txtbarcodImage.Text = "";

                    //txtImageID.Text = dt.Rows[0]["ImageID"].ToString();
                    txtNotes.Text = dt.Rows[0]["Notes"].ToString();
                    txtbarcodImage.Text = dt.Rows[0]["ImageCode"].ToString();
                    imgByte = (Byte[])(dt.Rows[0]["TheImage"]);
                    pic.Image = byteArrayToImage(imgByte);
                    PictureBox1.Image = pic.Image;
                    checkEdit1.Checked = false;
                    if (Comon.cInt(dt.Rows[0]["ApprovalImage"].ToString()) == 1)
                        checkEdit1.Checked = true;
                    else
                        checkEdit1.Checked = false;
                    Validations.DoReadRipon(this, ribbonControl1);

                }
            }
        }
        private void frmIDsImages_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F9)
            {
                falgPrint = true;
                DoSave();
            }
            if (e.KeyCode == Keys.F6)
            {
                DoSave();
            }
            if (e.KeyCode == Keys.F3)
                Find();
        }
    }
}

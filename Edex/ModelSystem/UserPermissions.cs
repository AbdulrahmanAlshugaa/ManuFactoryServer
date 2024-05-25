using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using Edex.DAL.UsersManagement;
using Edex.Model;
using Edex.Model.Language;
using Edex.GeneralObjects.GeneralForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Edex.ModelSystem
{
    public class Permissions
    {
        /// <summary>
        /// This method sets the visibility of the RibbonControl items based on user permissionshi
        /// </summary>
        /// <param name="ribbonControl"></param>
        /// <param name="UserID"></param>
        /// <param name="BranshID"></param>
        /// <param name="FacilityID"></param>
        public static void UserPermissionsMenu(RibbonControl ribbonControl, int UserID, int BranshID, int FacilityID) 
        {
            // Get user permissions for menus, forms, and reports
            var Menu = UsersManagementDAL.frmGetAllUserMenusPermissions(UserID, BranshID, FacilityID);
            var Froms = UsersManagementDAL.frmGetAllUserFormsPermissions(UserID, BranshID, FacilityID);
            var Reports = UsersManagementDAL.frmGetAllUserReportsPermissions(UserID, BranshID, FacilityID);

            // Loop through the RibbonControl items and set visibility based on permissions
            for (int i = 0; i < ribbonControl.Pages.Count; i++) {
                // Get menu permissions for this page
                var itemMenu = Menu.FirstOrDefault(o => o.MenuName == ribbonControl.Pages[i].Name.Substring(3));
                ribbonControl.Pages[i].Visible = true;

                // Change language to English if needed
                if (UserInfo.Language == iLanguage.English) {
                    ChangeLanguage.LTR(ribbonControl.Pages[i]);
                }

                // If user has no menu permissions, hide the page and continue to the next one
                if (itemMenu == null) {
                    ribbonControl.Pages[i].Visible = false;
                    continue;
                } else if (itemMenu.MenuView == 0) {
                    ribbonControl.Pages[i].Visible = false;
                    continue;
                }
        
                // Loop through the RibbonPageGroup items and set visibility based on permissions
                foreach (RibbonPageGroup group in ribbonControl.Pages[i].Groups) {
                    // Change language to English if needed
                    if (UserInfo.Language == iLanguage.English) {
                        ChangeLanguage.LTR(group);
                    }

                    // For Group3, loop through BarItemLink items and set visibility based on permissions for reports
                    if (group.Name == string.Concat(ribbonControl.Pages[i].Name, "Group3"))
                    {
                        foreach (BarItemLink link in group.ItemLinks)
                        {
                            // Change language to English if needed
                            if (UserInfo.Language == iLanguage.English)
                            {
                                ChangeLanguage.LTR((BarButtonItem)link.Item);
                            }
                            // Find the item report in the 'Reports' object for this link's item
                            var itemReports = Reports.FirstOrDefault(o => o.ReportName == link.Item.Name.Substring(3));
                            if (itemReports == null)
                            {
                                // If report is not found or link is a PopUpMenu, set link's visibility to false/true respectively
                                link.Visible = false;
                                if (link.Item.Name.Contains("PopUpMenu"))
                                {
                                    link.Visible = true;
                                }
                                continue;
                            }
                            else if (itemReports.ReportView == 0)
                            {
                                // If report view is not allowed, set link's visibility to false
                                link.Visible = false;
                                continue;
                            }
                        }
                    }
                    else
                    {
                        // Loop through each BarItemLink in the group
                        foreach (BarItemLink link in group.ItemLinks)
                        {
                            // If the user's language is English, change the direction to left-to-right
                            if (UserInfo.Language == iLanguage.English)
                            {
                                ChangeLanguage.LTR((BarButtonItem)link.Item);
                            }

                            // Try to find an entry in the Froms list that matches the item's name
                            var itemFroms = Froms.FirstOrDefault(o => o.FormName == link.Item.Name.Substring(3));

                            // If no matching entry is found, hide the link (unless it's a PopUpMenu)
                            if (itemFroms == null)
                            {
                                link.Visible = false;
                                if (link.Item.Name.Contains("PopUpMenu"))
                                {
                                    link.Visible = true;
                                }
                                continue;
                            }
                            // If the matching entry has a FormView of 0, hide the link
                            else if (itemFroms.FormView == 0)
                            {
                                link.Visible = false;
                                continue;
                            }
                        }
                    }
                }
            }
        }
        
       /// <summary>
       ///this function checks the user permissions for a given form
       /// </summary>
       /// <param name="form"></param>
       /// <param name="ribbonControl1"></param>
       /// <param name="UserID"></param>
       /// <param name="BranshID"></param>
       /// <param name="FacilityID"></param>
       /// <returns></returns>
        public static Boolean UserPermissionsFrom(BaseForm form, RibbonControl ribbonControl1, int UserID, int BranshID, int FacilityID)
        {
            if (form.IsDisposed)
                return false;
         
            // Get all user form permissions
            List<UserFormsPermissions> PermissionsFrom = UsersManagementDAL.frmGetAllUserFormsPermissions(UserID, BranshID, FacilityID);
            // Get all user report permissions
            List<UserReportsPermissions> PermissionsReport = UsersManagementDAL.frmGetAllUserReportsPermissions(UserID, BranshID, FacilityID);
            // Look for a user form permission matching the current form name
            UserFormsPermissions ResultFrom = PermissionsFrom.FirstOrDefault(o => o.FormName.ToLower() == form.Name.ToLower());
            // Look for a user report permission matching the current form name
            
            UserReportsPermissions ResultReport = PermissionsReport.FirstOrDefault(o => o.ReportName.ToLower() == ("rpt" + form.Name.Substring(3)).ToLower());
            // Initialize a variable to keep track of whether the user has any permissions for this form
            Boolean Result = false;

            // If a matching user form permission was found, set form permissions and mark the form as accessible
            if (ResultFrom != null)
            {
                form.FormAdd = Comon.cbool(ResultFrom.FormAdd);
                form.FormDelete = Comon.cbool(ResultFrom.FormDelete);
                form.FormUpdate = Comon.cbool(ResultFrom.FormUpdate);
                form.FormView = Comon.cbool(ResultFrom.FormView);

                // If the user has any form permissions, mark the form as accessible
                if (ResultFrom.FormAdd == 1 || ResultFrom.FormDelete == 1 || ResultFrom.FormUpdate == 1 || ResultFrom.FormView == 1)
                {
                    Result = true;
                }

                // If the user does not have any form permissions, mark the form as inaccessible and display a warning message
                if (ResultFrom.FormAdd == 0 && ResultFrom.FormDelete == 0 && ResultFrom.FormUpdate == 0 && ResultFrom.FormView == 0)
                {
                    Result = false;
                    Messages.MsgWarning(Messages.TitleWorning, Messages.msgNoPermissionToViewRecord);
                }
            }

            
            if (ResultReport != null)
            {
                form.ReportName = ResultReport.ReportName;
                form.ReportView = Comon.cbool(ResultReport.ReportView);
                form.ReportExport = Comon.cbool(ResultReport.ReportExport);
                form.ShowReportInReportViewer = Comon.cbool(ResultReport.ShowReportInReportViewer);
                Result = true;
                if (ResultReport.ReportView == 0 && ResultReport.ReportExport == 0 && ResultReport.ReportExport == 0 && Result == false)
                    Result = false;

            }
            if (ResultFrom != null)
            {
                // new & Update & save & delete
                ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Item.Enabled = Comon.cbool(ResultFrom.FormAdd);
                ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Item.Enabled = Comon.cbool(ResultFrom.FormUpdate);
                ribbonControl1.Pages[0].Groups[0].ItemLinks[2].Item.Enabled = Comon.cbool(1);
                ribbonControl1.Pages[0].Groups[0].ItemLinks[3].Item.Enabled = Comon.cbool(ResultFrom.FormDelete);

                // first & Prev
                ribbonControl1.Pages[0].Groups[0].ItemLinks[4].Item.Enabled = form.FormView;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[5].Item.Enabled = form.FormView;

                //pages
                ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Item.Enabled = true;


                // next & last
                ribbonControl1.Pages[0].Groups[0].ItemLinks[7].Item.Enabled = form.FormView;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[8].Item.Enabled = form.FormView;

                // search & Print &  export
                ribbonControl1.Pages[0].Groups[0].ItemLinks[9].Item.Enabled = Comon.cbool(ResultFrom.FormView);
            }
            ribbonControl1.Pages[0].Groups[0].ItemLinks[10].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[11].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[10].Item.Enabled = true;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[11].Item.Enabled = true;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[10].Visible = true;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[11].Visible = true;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[11].Visible = true;
            if (ResultFrom != null)
            {
                ResultFrom.ReportView = (ResultReport == null ? 0 : ResultReport.ReportView);
                ResultFrom.ShowReportInReportViewer = (ResultReport == null ? 0 : ResultReport.ShowReportInReportViewer);
                ResultFrom.ReportExport = (ResultReport == null ? 0 : ResultReport.ReportExport);
                 
            }
            if (ResultFrom == null&& ResultReport == null)
            {
                Result = false;
                Messages.MsgWarning(Messages.TitleWorning, Messages.msgNoPermissionToViewRecord);
            }
            return Result;
        }
    }
}

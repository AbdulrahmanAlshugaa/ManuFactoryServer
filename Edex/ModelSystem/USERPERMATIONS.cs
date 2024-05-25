using Edex.GeneralObjects.GeneralClasses;
using Edex.GeneralObjects.GeneralForms;
using Edex.DAL;
using Edex.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Edex.GeneralObjects.GeneralForms;

namespace Edex.ModelSystem
{
  public static  class USERPERMATIONS
    {
        public static bool GET_FORMPERMATION(BaseForm form)
        {

            FORMPERMATION FRM = new FORMPERMATION();//SYSUSERDAL.GET_FORMPERMATION(form.Name, UserInfo.ROLE, UserInfo.BRANCHID, UserInfo.Active);
                                                    // FORMPERMATION PermissionsReport = SYSUSERDAL.GET_FORMPERMATION(form.Name, UserInfo.ROLE, UserInfo.BRANCHID, UserInfo.Active);
            FRM.FormAdd = 1;
            FRM.FormView = 1;
            FRM.FormUpdate = 1;
            FRM.FormDelete = 1;

            if (FRM != null)
            {
                form.FormAdd = Comon.cbool(FRM.FormAdd);
                form.FormView = Comon.cbool(FRM.FormView);
                form.FormUpdate = Comon.cbool(FRM.FormUpdate);
                form.FormDelete = Comon.cbool(FRM.FormDelete);
                form.ReportName = FRM.DefaultReportName;

                bool v = form.FormAdd || form.FormView || form.FormUpdate || form.FormDelete;
                if (v == false)
                {
                    Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToUseScreen);
                    form.Close();
                    return false;
                }
                else
                return true ;
            }
            else
            {
                form.FormAdd = false;
                form.FormView = false;
                form.FormUpdate = false;
                form.FormDelete = false;
                Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToUseScreen);
                form.Close();
                return false;
            }
            

        }


       

    }
}

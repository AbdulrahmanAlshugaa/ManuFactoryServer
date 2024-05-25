using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.Model
{
   public class UserPermissions
    {

       public int PermissionID { get; set; }
       public string PermissionArbName { get; set; }
       public string PermissionEngName { get; set; }
       public int Cancel { get; set; }
       public int BranchID { get; set; }
       public int FacilityID { get; set; }
       public int VIEW { get; set; }
       public int ADD { get; set; }
       public int DELETE { get; set; }
       public int UPDATE { get; set; }
       public int DaysAllowedForEdit { get; set; }
       public string Selected { get; set; } 
   }

}

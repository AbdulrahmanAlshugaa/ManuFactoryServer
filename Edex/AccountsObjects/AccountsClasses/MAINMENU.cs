using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.AccountsObjects.AccountsClasses
{
    public partial class MAINMENU
    {
        public long ID { get; set; }
        public long MENUID { get; set; }
        public int BranchID { get; set; }
        public Nullable<int> FacilityID { get; set; }
        public string ARBNAME { get; set; }
        public string ENGNAME { get; set; }
        public string ENGCAPTION { get; set; }
        public long PARENTMENUID { get; set; }
        public int MENULEVELID { get; set; }
        public int MENUTYPEID { get; set; }

        public string MENUNAME { get; set; }
        public string FORMNAME { get; set; }




    }
}

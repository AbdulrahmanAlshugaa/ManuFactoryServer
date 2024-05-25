using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.Model
{
    public class UserRoles
    {
        public int RoleID { get; set; }
        public string RoleArbName { get; set; }
        public string RoleEngName { get; set; }
        public string RoleDescription { get; set; }
        public bool IsSystemAdmin { get; set; }
        public int AddByUserID { get; set; }
        public double RegDate { get; set; }
        public double RegTime { get; set; }
        public int EditUserID { get; set; }
        public double EditTime { get; set; }
        public double EditDate { get; set; }
        public int Cancel { get; set; }
        public int BranchID { get; set; }
        public int FacilityID { get; set; }
        public string Selected { get; set; }
        
    }
}

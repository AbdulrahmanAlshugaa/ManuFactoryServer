using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.Model
{
    public class UserLinkRolePermission
    {
        public int RoleID { get; set; }
        public List<int> PermissionID { get; set; }
    }
}

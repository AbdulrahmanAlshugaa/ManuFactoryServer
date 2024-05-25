using Edex.Model.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Edex.Model
{

    public static class UserInfo
    {

        public static int ID { get; set; }

        public static string UserName { get; set; }

        public static string Year { get; set; }


        public static string Active { get; set; }


        public static string Password { get; set; }


        public static string IpAddress { get; set; }

        public static bool RememberMe { get; set; }

        public static int BRANCHID { get; set; }
        public static string BranchName { get; set; }
        public static int FacilityID { get; set; }
        public static string FacilityName { get; set; }
        public static string imageSrc { get; set; }
        public static byte[] pic { get; set; }
        public static int SYSUSERID { get; set; }
        public static string SYSUSERENGNAME { get; set; }
        public static string SYSUSERARBNAME { get; set; }
        public static string ComputerInfo { get; set; }
        public static float RegDate { get; set; }
        public static float RegTime { get; set; }
        public static float RateVat { get; set; }
        public static iLanguage Language = iLanguage.Arabic;
        public static int MainTyepScreen { get; set; }


    }
    public class UserBO
    {

        public int ID { get; set; }

        public string UserName { get; set; }

        public string Year { get; set; }


        public string Active { get; set; }


        public string Password { get; set; }


        public string IpAddress { get; set; }

        public bool RememberMe { get; set; }

        public int BRANCHID { get; set; }
        public string BranchName { get; set; }
        public int FacilityID { get; set; }
        public string FacilityName { get; set; }

        public int SYSUSERID { get; set; }
        public string SYSUSERENGNAME { get; set; }
        public string SYSUSERARBNAME { get; set; }
        public string SYSUSERTEL { get; set; }
        public string SYSUSEREMAIL { get; set; }

        public string imageSrc { get; set; }
        public byte[] pic { get; set; }
        public string SYSUSERUSERNAME { get; set; }
        public string SYSUSERPASSWORD { get; set; }
        public int SYSUSERISACTIVE { get; set; }
        public int type { get; set; }
        public int SYSUSERROLE { get; set; }
        public long BALANCE { get; set; }

        public int IsActive { get; set; }
        public int NumberAllowedDays { get; set; }
        public string AllowedDate { get; set; }
        public int IsActiveAllowedDays { get; set; }
        public  int MainTyepScreen { get; set; }
        public string Notes { get; set; }

    }
}

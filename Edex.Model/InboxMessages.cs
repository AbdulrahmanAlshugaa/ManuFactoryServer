using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.Model
{
   public class InboxMessages
    {
        public int ID { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public int status { get; set; }
        public int SenderID { get; set; }
        public int ReceiverID { get; set; }
        public int AddByUserID { get; set; }
        public string AddDate { get; set; }
        public double RegDate { get; set; }
        public double RegTime { get; set; }
        public int EditUserID { get; set; }
        public double EditTime { get; set; }
        public double EditDate { get; set; }
        public int Cancel { get; set; }
        public int BranchID { get; set; }
        public int FacilityID { get; set; }
        public string SenderName { get; set; }
        public string ReceiverName { get; set; }   

    }
}

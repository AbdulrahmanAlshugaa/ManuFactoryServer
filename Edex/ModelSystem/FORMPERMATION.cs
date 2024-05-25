using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.ModelSystem
{
    public class FORMPERMATION
    {
        public int ID { get; set; }
        public string FORMNAME { get; set; }

        public string DefaultReportName { get; set; }
        public int FormView { get; set; }

        public int FormExport { get; set; }
        public int FormAdd { get; set; }
        public int FormDelete { get; set; }
        public int FormUpdate { get; set; }
        public int DaysAllowedForEdit { get; set; }
        public int BRANCHID { get; set; }
        public int ROLE { get; set; }

        public FORMPERMATION()
        {
            ID = 0;
            FORMNAME = "";
            FormView = 0;
            FormAdd = 0;
            FormDelete = 0;
            FormExport = 0;
            DaysAllowedForEdit = 0;
            ROLE = -1;
            BRANCHID = 0;
        }

    }
}

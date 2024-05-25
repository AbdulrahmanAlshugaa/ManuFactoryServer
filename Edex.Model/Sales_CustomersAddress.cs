using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.Model
{
    public class Sales_CustomersAddress
    {
        public int ID { get; set; }

        public long CustomerID { get; set; }

        public string ArbName { get; set; }

        public int Location { get; set; }

        public string EngName { get; set; }

        public int Cancel { get; set; }

        public int Street { get; set; }

        public string Building { get; set; }

        public string Floor { get; set; }

        public string Apartment { get; set; }

    }
}

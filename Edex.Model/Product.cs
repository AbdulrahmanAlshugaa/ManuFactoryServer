using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.Model
{
    public class Product
    {
        readonly int id;
        readonly string name;
        readonly decimal price;
        readonly string department;

        public int Id { get { return id; } }
        public string Name { get { return name; } }
        public decimal Price { get { return price; } }
        public string Department { get { return department; } }

        public Product(int id, string name, decimal price, string department)
        {
            this.id = id;
            this.name = name;
            this.price = price;
            this.department = department;
        }

        Product() { }

      
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_RESTful.entity
{
    public class Good
    {
        private int id;
        private string name;
        private int count;
        private int price;

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public int Count { get => count; set => count = value; }
        public int Price { get => price; set => price = value; }
    }
}

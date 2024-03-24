using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{
    public class Character
    {
        public int id { get; set; }
        public string name { get; set; }
        public Location location { get; set; }
        
        public Character(int id, string name, Location location)
        {
            this.id = id;
            this.name = name;
            this.location = location;
        }

        //public void leastway(Location location)
        //{

        //}
    }
}

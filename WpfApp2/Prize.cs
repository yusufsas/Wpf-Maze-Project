using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{
    public class Prize
    {
        public string filepath { get; set; }
        public string type { get; set; }

        public int size { get; set; }

        public Location location { get; set; }

        public Prize(string filepath, string type, int size, Location location)
        {
            this.filepath = filepath;
            this.type = type;
            this.size = size;
            this.location = location;   
        }



    }
}

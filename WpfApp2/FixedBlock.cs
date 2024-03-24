using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{
    public class FixedBlock:Block
    {
        public FixedBlock(string type,int size,Location location) : base(type, size, location)
        {

        }
    }

    public class DynamicBlock : Block
    {
        public DynamicBlock(string type,int size,Location location) : base(type, size, location)
        {

        }
    }
    public class Bird : DynamicBlock
    {
        public string filepath { get; set; }
        public Bird(string type,int size, Location location, string filepath) : base(type, size, location)
        {
            this.filepath = filepath;
        }
    }
    public class Bee: DynamicBlock
    {
        public string filepath { get; set; }
        public Bee(string type, int size, Location location,string filepath) : base(type, size, location)
        {
            this.filepath = filepath;
        }
    }


    public class Tree : FixedBlock
    {
        public string filepath { get; set; }
        
        public Tree(string type,int size,Location location,string filepath) : base(type, size, location)
        {
            this.filepath = filepath;
        }
    }


    public class Rock : FixedBlock
    {
        public string filepath { get; set; }

        public Rock(string type, int size, Location location, string filepath) : base(type, size, location)
        {
            this.filepath = filepath;
        }
    }
    public class Mountain : FixedBlock
    {
        public string filepath { get; set; }

        public Mountain(string type, int size, Location location, string filepath) : base(type, size, location)
        {
            this.filepath = filepath;
        }
    }
    public class Wall : FixedBlock
    {
        public string filepath { get; set; }

        public Wall(string type, int size, Location location, string filepath) : base(type, size, location)
        {
            this.filepath = filepath;
        }
    }



}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skaki
{
    public class Player
    {
        public String name;
        public int age;
        public bool won;

        public Player(string name, int age)
        {
            this.name = name;
            this.age = age;
        }
    }
}

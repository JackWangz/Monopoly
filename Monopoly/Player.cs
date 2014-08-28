using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly
{
    class Player
    {
        public string Name { get; set; }
        public string Assest { get; set; } // 應該會用泛型做
        public int Cash { get; set; }
        public string Color { get; set; }
    }
}

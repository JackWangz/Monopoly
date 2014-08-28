using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace Monopoly
{
    class Site
    {
        public int No { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Level { get; set; }
        public Point Position { get; set; }
    }
}

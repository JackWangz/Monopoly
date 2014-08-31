using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Monopoly
{
    class Player
    {
        public string Name { get; set; }
        public List<string> Assest = new List<string>();
        public int Cash { get; set; }
        public Color Color { get; set; }
        public Point Position { get; set; }
        public int PositionIndex { get; set; }
    }
}

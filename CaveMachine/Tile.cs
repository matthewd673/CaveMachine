using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaveMachine
{
    public class Tile
    {

        public bool wall;
        public int caveId;
        public bool edge;
        public bool portal;
        public bool tunnel;
        public bool extrapolated;

        public Tile(bool wall, int caveId = 0, bool edge = false, bool portal = false, bool tunnel = false, bool extrapolated = false)
        {
            this.wall = wall;
            this.caveId = caveId;
            this.edge = edge;
            this.portal = portal;
            this.tunnel = tunnel;
            this.extrapolated = extrapolated;
        }

    }
}

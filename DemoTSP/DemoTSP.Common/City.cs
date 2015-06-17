using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoTSP.Common
{
    public class City
    {
        /// <summary>
        /// X position.
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// Y position.
        /// </summary>
        public int Y { get; private set; }

        public City(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int Proximity(City other)
        {
            int xdiff = this.X - other.X;
            int ydiff = this.Y - other.Y;

            return (int)Math.Sqrt(xdiff ^ 2 + ydiff ^ 2);
        }
    }
}

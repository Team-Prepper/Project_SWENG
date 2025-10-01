using System;

namespace BKTools.Gaming.GridMap2D
{
    public struct GridCoord2D : IEquatable<GridCoord2D>
    {
        public int x { get; }
        public int y { get; }

        public GridCoord2D(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public bool Equals(GridCoord2D other)
        {
            if (x != other.x) return false;
            return y == other.y;
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", x, y);
        }
    }
}
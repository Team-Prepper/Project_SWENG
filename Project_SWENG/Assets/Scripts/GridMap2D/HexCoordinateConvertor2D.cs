using System.Collections.Generic;
using UnityEngine;

namespace BKTools.Gaming.GridMap2D
{

    public class HexCoordinateConvertor2D : ICoordinateConvertor2D
    {
        private static float xOffset;
        private static float zOffset;

        public Vector3 ConvertToVector3(GridCoord2D target)
        {
            if (target.x % 2 == 0)
                return new Vector3(target.x * xOffset,
                    0, target.y * zOffset);

            return new Vector3(target.x * xOffset,
                0, (target.y - 0.5f) * zOffset);
        }

        public GridCoord2D ConvertFromVector3(Vector3 target)
        {
            int x = Mathf.RoundToInt(target.x / xOffset);
            int z = Mathf.CeilToInt(target.z / zOffset);

            return new GridCoord2D(x, z);
        }

        public ISet<GridCoord2D> GetNeighboursFor
            (GridCoord2D Coord2Ds, int len = 1)
        {

            ISet<GridCoord2D> retval = new HashSet<GridCoord2D>();

            Vector3 origin = ConvertToVector3(Coord2Ds);

            for (int q = -len; q <= len; q++)
            {
                int r1 = Mathf.Max(-len, -q - len);
                int r2 = Mathf.Min(len, -q + len);

                for (int r = r1; r <= r2; r++)
                {
                    float xPos = xOffset * q + origin.x;
                    float zPos = zOffset * (r + q * 0.5f) + origin.z;

                    retval.Add(ConvertFromVector3(new Vector3(xPos, 0, zPos)));

                }
            }

            return retval;
        }

        public void SetCoordConstant(float x, float z)
        {
            xOffset = x;
            zOffset = z;
        }

    }
}
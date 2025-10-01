using System.Collections.Generic;
using UnityEngine;

namespace BKTools.Gaming.GridMap2D
{

    public interface ICoordinateConvertor2D
    {
        public Vector3 ConvertToVector3(GridCoord2D target);
        public GridCoord2D ConvertFromVector3(Vector3 target);
        public ISet<GridCoord2D> GetNeighboursFor(GridCoord2D target, int len=1);
        
        public void SetCoordConstant(float xOffset, float zOffset);
    }
}
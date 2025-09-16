using System;
using System.Collections.Generic;
using EasyH;
using UnityEngine;

namespace SWEng.Data
{
    public class MapUnitManager : Singleton<MapUnitManager>
    {
        public ICoordinateConvertor2D Convertor
            { get; private set; }

        private IDictionary<GridCoord2D, MapUnit> _mapUnitDict
            = new Dictionary<GridCoord2D, MapUnit>();

        public Map Map { get; set; }

        protected override void OnCreate()
        {
            Convertor = new HexCoordinateConvertor2D();
        }

        public void AddMapUnit(MapUnit hex)
        {
            _mapUnitDict.Add(hex.Pos, hex);
        }

        public MapUnit GetMapUnitAt(Vector3 coord)
        {
            GridCoord2D pos = Convertor.ConvertFromVector3(coord);

            return GetMapUnitAt(pos);
            
        }

        public MapUnit GetMapUnitAt(GridCoord2D coord)
        {
            if (!_mapUnitDict.ContainsKey(coord)) return null;
            return _mapUnitDict[coord];
        }

        public ISet<GridCoord2D> GetNeighboursFor(GridCoord2D hexCoordinates, int len = 1)
        {

            ISet<GridCoord2D> neighbours =
                Convertor.GetNeighboursFor(hexCoordinates, len);

            ISet<GridCoord2D> retval = new HashSet<GridCoord2D>();

            foreach (GridCoord2D coord in neighbours)
            {
                if (!_mapUnitDict.ContainsKey(coord)) continue;
                retval.Add(coord);
            }

            return retval;
        }
        
    }

}
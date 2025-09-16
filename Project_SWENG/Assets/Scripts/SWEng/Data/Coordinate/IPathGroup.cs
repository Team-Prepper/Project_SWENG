using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SWEng.Data
{

    public interface IPathGroup
    {

        public IList<GridCoord2D> GetPathTo(GridCoord2D destination);
        public bool IsHexCroodInRange(GridCoord2D position);

        public IEnumerable<GridCoord2D> GetRangePositions();
    }

    public struct BFSPathGroup : IPathGroup
    {
        private IDictionary<GridCoord2D, GridCoord2D?> _visitedNodesDict;

        public BFSPathGroup(IDictionary<GridCoord2D, GridCoord2D?> visitedNodesDict)
        {
            _visitedNodesDict = visitedNodesDict;
        }

        public IList<GridCoord2D> GetPathTo(GridCoord2D destination)
        {
            if (!_visitedNodesDict.ContainsKey(destination))
                return new List<GridCoord2D>();

            List<GridCoord2D> path = new List<GridCoord2D>
        { destination };

            while (_visitedNodesDict[destination] != null)
            {
                path.Add(_visitedNodesDict[destination].Value);
                destination = _visitedNodesDict[destination].Value;
            }
            path.Reverse();

            return path.Skip(1).ToList();
        }

        public bool IsHexCroodInRange(GridCoord2D position)
        {
            return _visitedNodesDict.ContainsKey(position);
        }

        public IEnumerable<GridCoord2D> GetRangePositions()
            => _visitedNodesDict?.Keys;
    }

}
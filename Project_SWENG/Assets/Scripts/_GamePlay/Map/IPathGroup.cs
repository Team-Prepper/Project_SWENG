using System.Collections.Generic;
using System.Linq;

public interface IPathGroup {

    public IList<HexCoordinate> GetPathTo(HexCoordinate destination);
    public bool IsHexCroodInRange(HexCoordinate position);

    public IEnumerable<HexCoordinate> GetRangePositions();
}

public struct BFSPathGroup : IPathGroup
{
    private IDictionary<HexCoordinate, HexCoordinate?> _visitedNodesDict;

    public BFSPathGroup(IDictionary<HexCoordinate, HexCoordinate?> visitedNodesDict) {
        _visitedNodesDict = visitedNodesDict;
    } 

    public IList<HexCoordinate> GetPathTo(HexCoordinate destination)
    {
        if (!_visitedNodesDict.ContainsKey(destination))
            return new List<HexCoordinate>();

        List<HexCoordinate> path = new List<HexCoordinate>
        { destination };

        while (_visitedNodesDict[destination] != null)
        {
            path.Add(_visitedNodesDict[destination].Value);
            destination = _visitedNodesDict[destination].Value;
        }
        path.Reverse();

        return path.Skip(1).ToList();
    }

    public bool IsHexCroodInRange(HexCoordinate position)
    {
        return _visitedNodesDict.ContainsKey(position);
    }

    public IEnumerable<HexCoordinate> GetRangePositions()
        => _visitedNodesDict?.Keys;
}
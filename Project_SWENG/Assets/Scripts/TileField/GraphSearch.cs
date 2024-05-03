using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GraphSearch
{

}

public interface IPathGroup {

    public IList<HexCoordinate> GetPathTo(HexCoordinate destination);
    public bool IsHexPositionInRange(HexCoordinate position);
    public IEnumerable<HexCoordinate> GetRangePositions();
}

public struct BFSResult : IPathGroup
{
    private IDictionary<HexCoordinate, HexCoordinate?> _visitedNodesDict;

    public BFSResult(IDictionary<HexCoordinate, HexCoordinate?> visitedNodesDict) {
        _visitedNodesDict = visitedNodesDict;
    } 

    public IList<HexCoordinate> GetPathTo(HexCoordinate destination)
    {
        if (_visitedNodesDict.ContainsKey(destination) == false)
            return new List<HexCoordinate>();

        List<HexCoordinate> path = new List<HexCoordinate>();
        path.Add(destination);

        while (_visitedNodesDict[destination] != null)
        {
            path.Add(_visitedNodesDict[destination].Value);
            destination = _visitedNodesDict[destination].Value;
        }
        path.Reverse();

        return path.Skip(1).ToList();
    }

    public bool IsHexPositionInRange(HexCoordinate position)
    {
        return _visitedNodesDict.ContainsKey(position);
    }

    public IEnumerable<HexCoordinate> GetRangePositions()
        => _visitedNodesDict?.Keys;
}
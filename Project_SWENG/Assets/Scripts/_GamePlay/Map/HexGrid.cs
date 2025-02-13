using System.Collections.Generic;
using EHTool;
using JetBrains.Annotations;
using UnityEngine;

public class HexGrid : Singleton<HexGrid> {
    //public Dictionary<Vector3Int, bool> hexTileDict = new Dictionary<Vector3Int, bool>();

    public float XOffset { get; private set; } = 5f;
    public float ZOffset { get; private set; } = 5f;

    IDictionary<HexCoordinate, MapUnit> _mapUnitDict = new Dictionary<HexCoordinate, MapUnit>();
    IDictionary<HexCoordinate, ISet<HexCoordinate>> _mapUnitNeighboursDict = new Dictionary<HexCoordinate, ISet<HexCoordinate>>();

    private IList<MapUnit> _emptyHexTiles = new List<MapUnit>();

    public Map Map { get; internal set; }

    public IPathGroup GetPathGroup(HexCoordinate startPos, int point)
    {
        IDictionary<HexCoordinate, HexCoordinate?> visitedNodes = new Dictionary<HexCoordinate, HexCoordinate?>();
        IDictionary<HexCoordinate, int> costSoFar = new Dictionary<HexCoordinate, int>();

        Queue<HexCoordinate> nodesToVisitQueue = new Queue<HexCoordinate>();

        nodesToVisitQueue.Enqueue(startPos);
        costSoFar.Add(startPos, 0);
        visitedNodes.Add(startPos, null);

        while (nodesToVisitQueue.Count > 0)
        {
            HexCoordinate currentNode = nodesToVisitQueue.Dequeue();

            foreach (HexCoordinate neighbourPosition in GetNeighboursFor(currentNode))
            {
                if (GetMapUnitAt(neighbourPosition).IsObstacle)
                    continue;

                int nodeCost = GetMapUnitAt(neighbourPosition).Cost;
                int currentCost = costSoFar[currentNode];
                int newCost = currentCost + nodeCost;

                if (newCost > point) continue;

                if (!visitedNodes.ContainsKey(neighbourPosition))
                {
                    visitedNodes[neighbourPosition] = currentNode;
                    costSoFar[neighbourPosition] = newCost;
                    nodesToVisitQueue.Enqueue(neighbourPosition);

                }
                else if (costSoFar[neighbourPosition] > newCost)
                {
                    costSoFar[neighbourPosition] = newCost;
                    visitedNodes[neighbourPosition] = currentNode;

                }
            }
        }
        return new BFSPathGroup(visitedNodes);
    }

    public IPathGroup GetPathGroupTo(HexCoordinate startPos, HexCoordinate endPos, int point)
    {
        IDictionary<HexCoordinate, HexCoordinate?> visitedNodes = new Dictionary<HexCoordinate, HexCoordinate?>();
        IDictionary<HexCoordinate, int> costSoFar = new Dictionary<HexCoordinate, int>();

        Queue<HexCoordinate> nodesToVisitQueue = new Queue<HexCoordinate>();

        nodesToVisitQueue.Enqueue(startPos);
        costSoFar.Add(startPos, 0);
        visitedNodes.Add(startPos, null);

        while (nodesToVisitQueue.Count > 0)
        {
            HexCoordinate currentNode = nodesToVisitQueue.Dequeue();
            foreach (HexCoordinate neighbourPosition in GetNeighboursFor(currentNode))
            {
                if (neighbourPosition.Equals(endPos)) {
                    visitedNodes[neighbourPosition] = currentNode;
                    return new BFSPathGroup(visitedNodes);
                }

                if (GetMapUnitAt(neighbourPosition).IsObstacle)
                    continue;

                int newCost = costSoFar[currentNode] + GetMapUnitAt(neighbourPosition).Cost;

                if (newCost > point) continue;

                if (!visitedNodes.ContainsKey(neighbourPosition))
                {
                    visitedNodes[neighbourPosition] = currentNode;
                    costSoFar[neighbourPosition] = newCost;
                    nodesToVisitQueue.Enqueue(neighbourPosition);

                }
                else if (costSoFar[neighbourPosition] > newCost)
                {
                    costSoFar[neighbourPosition] = newCost;
                    visitedNodes[neighbourPosition] = currentNode;

                }
            }
        }
        return new BFSPathGroup(new Dictionary<HexCoordinate, HexCoordinate?>());
    }

    public void AddTile(MapUnit hex) {
        _mapUnitDict[hex.HexCoords] = hex;

        if (hex.tileType == TileDataScript.TileType.normal) _emptyHexTiles.Add(hex);
    }


    public MapUnit GetMapUnitAt(Vector3 hexCoordinates)
    {
        return GetMapUnitAt(HexCoordinate.ConvertFromVector3(hexCoordinates));
    }

    public MapUnit GetMapUnitAt(HexCoordinate hexCoordinate)
    {
        _mapUnitDict.TryGetValue(hexCoordinate, out MapUnit result);
        return result;

    }

    public ISet<HexCoordinate> GetNeighboursFor(HexCoordinate hexCoordinates, int len) {
        if (len < 1) return new HashSet<HexCoordinate>();
        if (len == 1) return GetNeighboursFor(hexCoordinates);

        ISet<HexCoordinate> retval = new HashSet<HexCoordinate>();

        foreach (HexCoordinate coord in HexCoordinate.GetDirectionList(hexCoordinates))
        {
            retval.UnionWith(GetNeighboursFor(coord, len - 1));
        }

        return retval;
    }

    public ISet<HexCoordinate> GetNeighboursFor(HexCoordinate hexCoordinates)
    {
        if (_mapUnitDict.ContainsKey(hexCoordinates) == false)
            return new HashSet<HexCoordinate>();

        if (!_mapUnitNeighboursDict.ContainsKey(hexCoordinates))
        {
            _mapUnitNeighboursDict.Add(hexCoordinates, new HashSet<HexCoordinate>());

            foreach (HexCoordinate pos in HexCoordinate.GetDirectionList(hexCoordinates))
            {
                if (!_mapUnitDict.ContainsKey(pos)) continue;
                _mapUnitNeighboursDict[hexCoordinates].Add(pos);

            }
        }
        return _mapUnitNeighboursDict[hexCoordinates];
    }

    public MapUnit GetRandHexAtEmpty()
    {
        if(_emptyHexTiles.Count == 0) return null;

        int randHexIndex = Random.Range(0, _emptyHexTiles.Count);
        MapUnit randHex = _emptyHexTiles[randHexIndex];
        _emptyHexTiles.Remove(randHex);
        foreach(var hex in GetNeighboursFor(randHex.HexCoords))
        {
            _emptyHexTiles.Remove(GetMapUnitAt(hex));
        }

        return randHex;
    }

    internal void SetCoordConstant(float x, float z)
    {
        XOffset = x;
        ZOffset = z;
    }
}
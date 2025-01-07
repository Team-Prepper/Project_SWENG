using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EHTool;
using UnityEngine;
using Random = UnityEngine.Random;

public class HexGrid : Singleton<HexGrid>
{
    public static float xOffset = 4.325f, zOffset = 5.0f; //  yOffset = 0.5f,

    //public Dictionary<Vector3Int, bool> hexTileDict = new Dictionary<Vector3Int, bool>();

    public IDictionary<HexCoordinate, Hex> hexTileDict = new Dictionary<HexCoordinate, Hex>();
    IDictionary<HexCoordinate, ISet<HexCoordinate>> hexTileNeighboursDict = new Dictionary<HexCoordinate, ISet<HexCoordinate>>();

    private List<Hex> _emptyHexTiles = new List<Hex>();

    static class Direction {
        public static List<HexCoordinate> directionsOffsetOdd = new List<HexCoordinate>
    {
        new HexCoordinate( 0, 1), //N
        new HexCoordinate( 1, 0), //E1
        new HexCoordinate( 1, -1), //E2
        new HexCoordinate( 0, -1), //S
        new HexCoordinate(-1, -1), //W1
        new HexCoordinate(-1, 0), //W2
    };

        public static List<HexCoordinate> directionsOffsetEven = new List<HexCoordinate>
    {
        new HexCoordinate( 0, 1), //N
        new HexCoordinate( 1, 1), //E1
        new HexCoordinate( 1, 0), //E2
        new HexCoordinate( 0, -1), //S
        new HexCoordinate(-1, 0), //W1
        new HexCoordinate(-1, 1), //W2
    };

        public static List<HexCoordinate> GetDirectionList(int x)
            => x % 2 == 0 ? directionsOffsetEven : directionsOffsetOdd;
    }

    protected override void OnCreate()
    {

    }
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
                if (GetTileAt(neighbourPosition).IsObstacle())
                    continue;

                int nodeCost = GetTileAt(neighbourPosition).Cost;
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
        return new BFSResult(visitedNodes);
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
                    return new BFSResult(visitedNodes);
                }

                if (GetTileAt(neighbourPosition).IsObstacle())
                    continue;

                int newCost = costSoFar[currentNode] + GetTileAt(neighbourPosition).Cost;

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
        return new BFSResult(new Dictionary<HexCoordinate, HexCoordinate?>());
    }

    public void AddTile(Hex hex) {
        hexTileDict[hex.HexCoords] = hex;

        if (hex.tileType == TileDataScript.TileType.normal) _emptyHexTiles.Add(hex);
    }


    public Hex GetTileAt(Vector3 hexCoordinates)
    {
        return GetTileAt(HexCoordinate.ConvertFromVector3(hexCoordinates));
    }

    public Hex GetTileAt(HexCoordinate hexCoordinate)
    {
        hexTileDict.TryGetValue(hexCoordinate, out Hex result);
        return result;

    }

    public ISet<HexCoordinate> GetNeighboursFor(HexCoordinate hexCoordinates, uint len) {
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
        if (hexTileDict.ContainsKey(hexCoordinates) == false)
            return new HashSet<HexCoordinate>();

        if (!hexTileNeighboursDict.ContainsKey(hexCoordinates))
        {
            hexTileNeighboursDict.Add(hexCoordinates, new HashSet<HexCoordinate>());

            foreach (HexCoordinate pos in HexCoordinate.GetDirectionList(hexCoordinates))
            {
                if (!hexTileDict.ContainsKey(pos)) continue;
                hexTileNeighboursDict[hexCoordinates].Add(pos);

            }
        }
        return hexTileNeighboursDict[hexCoordinates];
    }

    public Hex GetRandHexAtEmpty()
    {
        if(_emptyHexTiles.Count == 0) return null;

        int randHexIndex = Random.Range(0, _emptyHexTiles.Count);
        Hex randHex = _emptyHexTiles[randHexIndex];
        _emptyHexTiles.Remove(randHex);
        foreach(var hex in GetNeighboursFor(randHex.HexCoords))
        {
            _emptyHexTiles.Remove(GetTileAt(hex));
        }

        return randHex;
    }

    public void RemoveAtEmeptyHexTiles(Hex setHex)
    {
        foreach(var hex in GetNeighboursFor(setHex.HexCoords))
        {
            _emptyHexTiles.Remove(GetTileAt(hex));
        }
    }
}

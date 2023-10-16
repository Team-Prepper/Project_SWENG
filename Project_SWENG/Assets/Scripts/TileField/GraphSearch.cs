using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GraphSearch
{
    public static BFSResult BFSGetRange(Vector3Int startPos, int point)
    {
        Dictionary<Vector3Int, Vector3Int?> visitedNodes = new Dictionary<Vector3Int, Vector3Int?>();
        Dictionary<Vector3Int, int> costSoFar = new Dictionary<Vector3Int, int>();
        Queue<Vector3Int> nodesToVisitQueue = new Queue<Vector3Int>();

        nodesToVisitQueue.Enqueue(startPos);
        costSoFar.Add(startPos, 0);
        visitedNodes.Add(startPos, null);

        while (nodesToVisitQueue.Count > 0)
        {
            Vector3Int currentNode = nodesToVisitQueue.Dequeue();
            foreach (Vector3Int neighbourPosition in HexGrid.Instance.GetNeighboursFor(currentNode))
            {
                if (HexGrid.Instance.GetTileAt(neighbourPosition).IsObstacle())
                    continue;

                int nodeCost = HexGrid.Instance.GetTileAt(neighbourPosition).Cost;
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
    

    public static List<Vector3Int> GeneratePathBFS(Vector3Int current, Dictionary<Vector3Int, Vector3Int?> visitedNodesDict)
    {
        List<Vector3Int> path = new List<Vector3Int>();
        path.Add(current);
        while (visitedNodesDict[current] != null)
        {
            path.Add(visitedNodesDict[current].Value);
            current = visitedNodesDict[current].Value;
        }
        path.Reverse();
        return path.Skip(1).ToList();
    }
}

public struct BFSResult
{
    private Dictionary<Vector3Int, Vector3Int?> _visitedNodesDict;

    public BFSResult(Dictionary<Vector3Int, Vector3Int?> visitedNodesDict) {
        _visitedNodesDict = visitedNodesDict;
    } 

    public List<Vector3Int> GetPathTo(Vector3Int destination)
    {
        if (_visitedNodesDict.ContainsKey(destination) == false)
            return new List<Vector3Int>();

        List<Vector3Int> path = new List<Vector3Int>();
        path.Add(destination);

        while (_visitedNodesDict[destination] != null)
        {
            path.Add(_visitedNodesDict[destination].Value);
            destination = _visitedNodesDict[destination].Value;
        }
        path.Reverse();

        return path.Skip(1).ToList();
    }

    public bool IsHexPositionInRange(Vector3Int position)
    {
        return _visitedNodesDict.ContainsKey(position);
    }

    public IEnumerable<Vector3Int> GetRangePositions()
        => _visitedNodesDict?.Keys;
}
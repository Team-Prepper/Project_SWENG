using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class HexGrid : MonoBehaviour
{
    public static HexGrid Instance;

    public Dictionary<Vector3Int, Hex> hexTileDict = new Dictionary<Vector3Int, Hex>();
    Dictionary<Vector3Int, List<Vector3Int>> hexTileNeighboursDict = new Dictionary<Vector3Int, List<Vector3Int>>();
    Dictionary<Vector3Int, List<Vector3Int>> hexTileNeighboursDoubleDict = new Dictionary<Vector3Int, List<Vector3Int>>();

    Vector3Int topOffset    = new Vector3Int(0, 1, 0);
    Vector3Int bottomOffset = new Vector3Int(0, -1, 0);

    public static event EventHandler EventSetHexTileDic;

    void Awake()
    {
        Instance = this;
        GridMaker.EventBuildComplete += SetHexTileDict;
    }

    public void SetHexTileDict(object sender, EventArgs e)
    {
        Debug.Log("Dic set Start");
        foreach (Hex hex in FindObjectsOfType<Hex>())
        {
            hexTileDict[hex.HexCoords] = hex;
        }
        Debug.Log("Dic set complete");

        EventSetHexTileDic?.Invoke(this, EventArgs.Empty);
    }

    public Hex GetTileAt(Vector3Int hexCoordinates)
    {
        Hex result = null;
        hexTileDict.TryGetValue(hexCoordinates, out result);
        return result;
    }

    public List<Vector3Int> GetNeighboursFor(Vector3Int hexCoordinates)
    {
        if (hexTileDict.ContainsKey(hexCoordinates) == false)
            return new List<Vector3Int>();

        if (hexTileNeighboursDict.ContainsKey(hexCoordinates))
            return hexTileNeighboursDict[hexCoordinates];

        hexTileNeighboursDict.Add(hexCoordinates, new List<Vector3Int>());

        foreach (Vector3Int direction in Direction.GetDirectionList(hexCoordinates.x))
        {
            if (hexTileDict.ContainsKey(hexCoordinates + direction))
            {
                hexTileNeighboursDict[hexCoordinates].Add(hexCoordinates + direction);
            }
            else if (hexTileDict.ContainsKey(hexCoordinates + direction + topOffset))
            {
                hexTileNeighboursDict[hexCoordinates].Add(hexCoordinates + direction + topOffset);
            }
            else if (hexTileDict.ContainsKey(hexCoordinates + direction + bottomOffset))
            {
                hexTileNeighboursDict[hexCoordinates].Add(hexCoordinates + direction + bottomOffset);
            }
        }
        return hexTileNeighboursDict[hexCoordinates];
    }

    public List<Vector3Int> GetNeighboursDoubleFor(Vector3Int hexCoordinates)
    {
        if (hexTileDict.ContainsKey(hexCoordinates) == false)
            return new List<Vector3Int>();

        if (hexTileNeighboursDoubleDict.ContainsKey(hexCoordinates))
            return hexTileNeighboursDoubleDict[hexCoordinates];

        hexTileNeighboursDoubleDict.Add(hexCoordinates, new List<Vector3Int>());

        foreach (var hexNeighbours in GetNeighboursFor(hexCoordinates))
        {
            foreach(var hex in GetNeighboursFor(hexNeighbours))
            {
                hexTileNeighboursDoubleDict[hexCoordinates].Add(hex);
            }
        }
        return hexTileNeighboursDoubleDict[hexCoordinates];
    }

    public Vector3Int GetClosestHex(Vector3 worldposition)
    {
        return HexCoordinates.ConvertPositionToOffset(worldposition);
    }

    public Hex GetRandHex()
    {
        if(hexTileDict.Count == 0) return null;
        Vector3Int[] keysArray = new Vector3Int[hexTileDict.Count];
        hexTileDict.Keys.CopyTo(keysArray, 0);

        return hexTileDict[keysArray[Random.Range(0,keysArray.Length)]];
    }
}

public static class Direction
{
    public static List<Vector3Int> directionsOffsetOdd = new List<Vector3Int>
    {
        new Vector3Int( 0, 0, 1), //N
        new Vector3Int( 1, 0, 0), //E1
        new Vector3Int( 1, 0,-1), //E2
        new Vector3Int( 0, 0,-1), //S
        new Vector3Int(-1, 0,-1), //W1
        new Vector3Int(-1, 0, 0), //W2
    };

    public static List<Vector3Int> directionsOffsetEven = new List<Vector3Int>
    {
        new Vector3Int( 0, 0, 1), //N
        new Vector3Int( 1, 0, 1), //E1
        new Vector3Int( 1, 0, 0), //E2
        new Vector3Int( 0, 0,-1), //S
        new Vector3Int(-1, 0, 0), //W1
        new Vector3Int(-1, 0, 1), //W2
    };

    public static List<Vector3Int> GetDirectionList(int x)
        => x % 2 == 0 ? directionsOffsetEven : directionsOffsetOdd;
}

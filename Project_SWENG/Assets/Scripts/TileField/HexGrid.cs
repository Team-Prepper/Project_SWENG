using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class HexGrid : Singleton<HexGrid>
{
    public static float xOffset = 4.325f, zOffset = 5.0f; //  yOffset = 0.5f,

    //public Dictionary<Vector3Int, bool> hexTileDict = new Dictionary<Vector3Int, bool>();

    public Dictionary<HexCoordinate, Hex> hexTileDict = new Dictionary<HexCoordinate, Hex>();
    Dictionary<HexCoordinate, List<HexCoordinate>> hexTileNeighboursDict = new Dictionary<HexCoordinate, List<HexCoordinate>>();
    Dictionary<HexCoordinate, List<HexCoordinate>> hexTileNeighboursDoubleDict = new Dictionary<HexCoordinate, List<HexCoordinate>>();

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


    public List<HexCoordinate> GetNeighboursFor(HexCoordinate hexCoordinates)
    {
        if (hexTileDict.ContainsKey(hexCoordinates) == false)
            return new List<HexCoordinate>();

        if (hexTileNeighboursDict.ContainsKey(hexCoordinates))
            return hexTileNeighboursDict[hexCoordinates];

        hexTileNeighboursDict.Add(hexCoordinates, new List<HexCoordinate>());

        foreach (HexCoordinate pos in HexCoordinate.GetDirectionList(hexCoordinates))
        {
            if (hexTileDict.ContainsKey(pos))
            {
                hexTileNeighboursDict[hexCoordinates].Add(pos);
            }
            
        }
        return hexTileNeighboursDict[hexCoordinates];
    }

    public List<HexCoordinate> GetNeighboursDoubleFor(HexCoordinate hexCoordinates)
    {
        if (hexTileDict.ContainsKey(hexCoordinates) == false)
            return new List<HexCoordinate>();

        if (hexTileNeighboursDoubleDict.ContainsKey(hexCoordinates))
            return hexTileNeighboursDoubleDict[hexCoordinates];

        hexTileNeighboursDoubleDict.Add(hexCoordinates, new List<HexCoordinate>());

        foreach (var hexNeighbours in GetNeighboursFor(hexCoordinates))
        {
            foreach(var hex in GetNeighboursFor(hexNeighbours))
            {
                
                hexTileNeighboursDoubleDict[hexCoordinates].Add(hex);
            }
        }
        HashSet<HexCoordinate> unduplicate = new HashSet<HexCoordinate>(hexTileNeighboursDoubleDict[hexCoordinates]);
        hexTileNeighboursDoubleDict[hexCoordinates] = unduplicate.ToList();

        return hexTileNeighboursDoubleDict[hexCoordinates];
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

using System;
using System.Collections.Generic;
using System.IO.Compression;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEngine.Rendering.DebugUI;
using Random = UnityEngine.Random;

public class GridMaker : MonoSingleton<GridMaker>
{
    GameObject _nowWorking;
    [Header("Ref")]
    [SerializeField] private Hex _hexPrefab;

    [SerializeField] TileDataScript _tileNormal;
    [SerializeField] TileDataScript _tileRock;
    [SerializeField] TileDataScript _tileHill;
    [SerializeField] TileDataScript _tileDungon;
    [SerializeField] TileDataScript _tileCastle;
    [SerializeField] TileDataScript _tileVillage;
    [SerializeField] TileDataScript _tileOcean;

    List<HexTileSetter> _tileSetters;

    [Space(10)]
    public GameObject hexGround;

    [Space(20)]
    public int gridSizeN = 7; // ?????? ???? N
    public int oceanSizeN = 5;

    private readonly float hexWidth = 4.325f; // horizontal
    private readonly float hexHeight = 5.0f;  // vertical

    protected override void OnCreate()
    {
        _tileSetters = new List<HexTileSetter>();
        base.OnCreate();
    }

    public TileDataScript GetTileData(TileDataScript.TileType type) {
        switch (type)
        {
            case TileDataScript.TileType.normal:
                return _tileNormal;
            case TileDataScript.TileType.rock:
                return _tileRock;
            case TileDataScript.TileType.hill:
                return _tileHill;
            case TileDataScript.TileType.dungon:
                return _tileDungon;
            case TileDataScript.TileType.castle:
                return _tileCastle;
            case TileDataScript.TileType.village:
                return _tileVillage;
        }
        return _tileOcean;
    }

    public void CreateHexGrid()
    {
        if (_nowWorking) DestroyImmediate(_nowWorking);
        _nowWorking = new GameObject();
        _nowWorking.transform.SetParent(transform);
        for (int q = -gridSizeN; q <= gridSizeN; q++)
        {
            int r1 = Mathf.Max(-gridSizeN, -q - gridSizeN);
            int r2 = Mathf.Min(gridSizeN, -q + gridSizeN);

            for (int r = r1; r <= r2; r++)
            {
                float xPos = hexWidth * (q);
                float zPos = hexHeight * (r + q * 0.5f);

                _SpawnHexTile(new Vector3(xPos, 0, zPos));

            }
        }
    }

    public void EndEdit()
    {
        foreach (HexTileSetter setter in _tileSetters) {
            DestroyImmediate(setter);
        }
        _tileSetters = new List<HexTileSetter>();
    }

    public void RemoveAll()
    {
        foreach (HexTileSetter setter in _tileSetters)
        {
            DestroyImmediate(setter.gameObject);
        }
        _tileSetters = new List<HexTileSetter>();
    }

    private Hex _SpawnHexTile(Vector3 spawnPos)
    {
        Hex hex = Instantiate(_hexPrefab, spawnPos, Quaternion.identity);
        _tileSetters.Add(hex.gameObject.AddComponent<HexTileSetter>());
        hex.transform.SetParent(_nowWorking.transform);

        GameObject iHexGround = Instantiate(hexGround, spawnPos, Quaternion.identity);
        iHexGround.layer = LayerMask.NameToLayer("HexTileGround");
        iHexGround.transform.SetParent(hex.transform);

        return hex;
    }
    #region reference
    /*
    Hex OceanSpawn(TileDataScript data, float xPos, float zPos)
    {
        Vector3 spawnPos = new Vector3(xPos, -0.5f, zPos);

        Hex hex = _SpawnHexTile(data, spawnPos);

        hex.tileType = Hex.Type.Water;
        hex.tile.transform.localPosition -= new Vector3(0f, 0.6f, 0f);

        return hex;
    }

    private Hex _HexTileSpawn(TileDataScript selectedData, float xPos, float zPos, int percentage)
    {
        //float setHigh = Random.Range(0, 2) * 0.5f;
        Vector3 spawnPos = new Vector3(xPos, 0, zPos);

        // Empty Field
        if (Random.Range(0, percentage) != 0) {
            Hex hexDefault = _SpawnHexTile(_tileNormal,  spawnPos);
            hexDefault.tileType = Hex.Type.Field;

            return hexDefault;
        }

        Hex hex = _SpawnHexTile(selectedData,  spawnPos);

        if (selectedData.cost == -1)
        {
            hex.tileType = Hex.Type.Obstacle;
            return hex;
        }

        if (selectedData.cost == 1)
        {
            hex.tileType = Hex.Type.Shop;
            return hex;
        }

        if (selectedData.cost != 3)
        {
            hex.tileType = Hex.Type.Object;
        }
        

        return hex;
    }
    */
    private Hex _SpawnHexTile(TileDataScript data, Vector3 spawnPos)
    {

        GameObject tile = Instantiate(data.tiles[Random.Range(0, data.tiles.Length)], spawnPos, Quaternion.Euler(0f, Random.Range(0, 6) * 60, 0f));
        tile.layer = LayerMask.NameToLayer("HexTile");

        Hex hex = Instantiate(_hexPrefab, spawnPos, Quaternion.identity);
        hex.WhenCreate(tile, transform, data.cost);

        GameObject iHexGround = Instantiate(hexGround, spawnPos, Quaternion.identity);
        iHexGround.layer = LayerMask.NameToLayer("HexTileGround");
        iHexGround.transform.SetParent(hex.transform);

        Transform selectFolder = hex.transform.Find("Main");
        if (selectFolder != null)
            tile.transform.SetParent(selectFolder.transform);
        else
            tile.transform.SetParent(hex.transform);

        tile.AddComponent<HexTileSetter>();
        HexGrid.Instance.AddTile(hex);

        return hex;
    }
#endregion
}


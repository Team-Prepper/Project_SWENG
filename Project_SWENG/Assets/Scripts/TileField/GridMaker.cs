using System;
using System.Collections.Generic;
using System.IO.Compression;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

[System.Serializable]
class TileData {
    public GameObject[] tiles;
    public int cost = 0;

    public TileData(GameObject[] tiles, int cost) { 
        this.tiles = tiles;
        this.cost = cost;
    }
}

public class GridMaker : MonoBehaviour
{
    [Header("Ref")]
    [SerializeField] private Hex _hexPrefab;

    [SerializeField] TileData _tileNormal = new TileData(null, 5);
    [SerializeField] TileData _tileRock = new TileData(null, 5);
    [SerializeField] TileData _tileHill = new TileData(null, 5);
    [SerializeField] TileData _tileDungon = new TileData(null, 5);
    [SerializeField] TileData _tileCastle = new TileData(null, 5);
    [SerializeField] TileData _tileVillage = new TileData(null, 5);
    [SerializeField] TileData _tileWater = new TileData(null, 5);
    [SerializeField] TileData _tileOcean = new TileData(null, 5);
    [SerializeField] TileData _tileIsland= new TileData(null, 5);

    [Space(10)]
    public GameObject hexGround;
    public GameObject hexRiver;

    public Material[] materials;
    [Space(20)]
    public int gridSizeN = 7; // ?????? ???? N
    public int oceanSizeN = 5;

    public List<GameObject> objTilesGo = new List<GameObject>();

    private float hexWidth = 4.325f; // horizontal
    private float hexHeight = 5.0f;  // vertical

    private NavMeshSurface navMeshSurface;

    public static event EventHandler EventBuildComplete;
    public static event EventHandler EventSetNavComplete;


    private void Awake()
    {
        navMeshSurface = GetComponent<NavMeshSurface>();
    }

    private void Start()
    {
        gridSizeN += oceanSizeN;
        CreateHexGrid();
    }

    public void CreateHexGrid()
    {
        for (int q = -gridSizeN; q <= gridSizeN; q++)
        {
            int r1 = Mathf.Max(-gridSizeN, -q - gridSizeN);
            int r2 = Mathf.Min(gridSizeN, -q + gridSizeN);

            for (int r = r1; r <= r2; r++)
            {
                float xPos = hexWidth * (q);
                float zPos = hexHeight * (r + q * 0.5f);

                if (MathF.Abs(q) > gridSizeN - oceanSizeN || r <= r1 + oceanSizeN - 1 || r >= r2 - oceanSizeN + 1)
                {
                    OceanSpawn(_tileOcean, xPos, zPos);
                    continue;
                }

                switch (Random.Range(0, 10))
                {
                    case 0:
                        _HexTileSpawn(_tileDungon, xPos, zPos, 10);
                        break;
                    case 1:
                        _HexTileSpawn(_tileHill, xPos, zPos, 10);
                        break;
                    case 2:
                        _HexTileSpawn(_tileCastle, xPos, zPos, 10);
                        break;
                    case 3:
                        _HexTileSpawn(_tileVillage, xPos, zPos, 10);
                        break;
                    case 4:
                    case 5:
                        OceanSpawn(_tileOcean, xPos, zPos);
                        break;
                    default:
                        _HexTileSpawn(_tileRock, xPos, zPos, 10);
                        break;
                }
            }
        }
        EventBuildComplete?.Invoke(this, EventArgs.Empty);
        SetNavMesh();
    }

    Hex OceanSpawn(TileData data, float xPos, float zPos)
    {
        Vector3 spawnPos = new Vector3(xPos, -0.5f, zPos);

        Hex hex = _SpawnHexTile(data, spawnPos);

        hex.tileType = Hex.Type.Water;
        hex.tile.transform.localPosition -= new Vector3(0f, 0.6f, 0f);

        return hex;
    }

    private Hex _HexTileSpawn(TileData selectedData, float xPos, float zPos, int percentage)
    {
        //float setHigh = Random.Range(0, 2) * 0.5f;
        Vector3 spawnPos = new Vector3(xPos, 0, zPos);

        // Empty Field
        if (Random.Range(0, percentage) != 0) {
            Hex hexDefault = _SpawnHexTile(_tileNormal,  spawnPos);
            hexDefault.tileType = Hex.Type.Field;

            HexGrid.Instance.emptyHexTiles.Add(hexDefault);

            return hexDefault;
        }

        Hex hex = _SpawnHexTile(selectedData,  spawnPos);

        if (selectedData.cost == -1)
        {
            hex.tileType = Hex.Type.Obstacle;
            return hex;
        }

        if(selectedData.cost != 3)
        {
            hex.tileType = Hex.Type.Object;
            objTilesGo.Add(hex.gameObject);
        }
        

        return hex;
    }

    private Hex _SpawnHexTile(TileData data, Vector3 spawnPos)
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

        HexGrid.Instance.AddTile(hex);

        return hex;
    }

    public void SetNavMesh()
    {
        Debug.Log("Nav set Start");
        if (navMeshSurface != null)
            navMeshSurface.BuildNavMesh();


        MaterialsConverter.ConvertMat(objTilesGo);
        EventSetNavComplete?.Invoke(this, EventArgs.Empty);
    }
}


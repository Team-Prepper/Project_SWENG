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

    #region originMember
    [Space(5)]
    public GameObject[] tileNormal;
    [SerializeField] int costNormal = 2;

    public GameObject[] tileRock;
    [SerializeField] int costRock = -1;

    public GameObject[] tileHill;
    [SerializeField] int costHill = 3;

    public GameObject[] tileDungon;
    [SerializeField] int costDungon = 4;

    public GameObject[] tileCastle;
    [SerializeField] int costCastle = 5;

    public GameObject[] tileVillage;
    [SerializeField] int costVillage = 1;

    public GameObject[] tileWater;
    //[SerializeField] int costWater = 3;

    public GameObject[] tileOcean;
    [SerializeField] int costOcean = -1;

    public GameObject[] tileIsland;
    //[SerializeField] int costIsland = 1;

    #endregion

    [Space(10)]
    public GameObject[] hexGround; // 0 Groudn 1 River
    public Material[] materials;
    [Space(20)]
    public int gridSizeN = 7; // ?????? ???? N
    public int oceanSizeN = 5;

    public List<GameObject> objTilesGo = new List<GameObject>();

    private float hexWidth = 4.325f; // horizontal
    private float hexHeight = 5.0f;  // vertical

    private NavMeshSurface navMeshSurface;
    private MaterialsConverter materialsConverter;

    public static event EventHandler EventBuildComplete;
    public static event EventHandler EventSetNavComplete;


    private void Awake()
    {
        navMeshSurface = GetComponent<NavMeshSurface>();
        materialsConverter = GetComponent<MaterialsConverter>();
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
                    OceanSpawn(tileOcean, xPos, zPos, costOcean);
                    continue;
                }

                switch (Random.Range(0, 10))
                {
                    case 0:
                        HexTileSpawn(new TileData(tileDungon, costDungon), xPos, zPos, 10);
                        //HexTileSpawn(tileDungon, tileNormal, xPos, zPos, 10, costDungon, costNormal);
                        break;
                    case 1:
                        HexTileSpawn(new TileData(tileHill, costHill), xPos, zPos, 10);
                        //HexTileSpawn(tileHill, tileNormal, xPos, zPos, 10, costHill, costNormal);
                        break;
                    case 2:
                        HexTileSpawn(new TileData(tileCastle, costCastle), xPos, zPos, 10);
                        //HexTileSpawn(tileCastle, tileNormal, xPos, zPos, 10, costCastle, costNormal);
                        break;
                    case 3:
                        HexTileSpawn(new TileData(tileVillage, costVillage), xPos, zPos, 10);
                        //HexTileSpawn(tileVillage, tileNormal, xPos, zPos, 10, costVillage, costNormal);
                        break;
                    case 4:
                    case 5:
                        OceanSpawn(new TileData(tileOcean, costOcean), xPos, zPos);
                        //OceanSpawn(tileOcean, xPos, zPos, costWater);
                        break;
                    default:
                        HexTileSpawn(new TileData(tileRock, costRock), xPos, zPos, 10);
                        //HexTileSpawn(tileRock, tileNormal, xPos, zPos, 5, costRock, costNormal);
                        break;
                }
            }
        }
        EventBuildComplete?.Invoke(this, EventArgs.Empty);
        SetNavMesh();
    }
    #region suggest

    Hex OceanSpawn(TileData data, float xPos, float zPos)
    {
        Vector3 spawnPos = new Vector3(xPos, -0.5f, zPos);

        Hex hex = _SpawnHexTile(data, spawnPos);

        hex.tileType = Hex.Type.Water;
        hex.tile.transform.localPosition -= new Vector3(0f, 0.6f, 0f);

        return hex;
    }

    Hex HexTileSpawn(TileData selectedData, float xPos, float zPos, int percentage)
    {
        //float setHigh = Random.Range(0, 2) * 0.5f;
        Vector3 spawnPos = new Vector3(xPos, 0, zPos);

        // Empty Field
        if (Random.Range(0, percentage) != 0) {
            Hex hexDefault = _SpawnHexTile(new TileData(tileNormal, costNormal),  spawnPos);
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

    Hex _SpawnHexTile(TileData data, Vector3 spawnPos)
    {

        GameObject tile = Instantiate(data.tiles[Random.Range(0, data.tiles.Length)], spawnPos, Quaternion.Euler(0f, Random.Range(0, 6) * 60, 0f));
        tile.layer = LayerMask.NameToLayer("HexTile");

        Hex hex = Instantiate(_hexPrefab, spawnPos, Quaternion.identity);
        hex.WhenCreate(tile, transform, data.cost);

        GameObject iHexGround = Instantiate(hexGround[0], spawnPos, Quaternion.identity);
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
#endregion

    #region origin
    Hex OceanSpawn(GameObject[] prefabs, float xPos, float zPos, int cost)
    {
        Vector3 spawnPos = new Vector3(xPos, -0.5f, zPos);
        Hex hex = Instantiate(_hexPrefab, spawnPos, Quaternion.identity);

        GameObject iHexGround = Instantiate(hexGround[1], spawnPos, Quaternion.identity);
        iHexGround.layer = LayerMask.NameToLayer("HexTileGround");

        GameObject tile = Instantiate(prefabs[Random.Range(0, prefabs.Length)], spawnPos, Quaternion.Euler(0f, 30f, 0f));
        hex.cost = cost;
        tile.layer = LayerMask.NameToLayer("HexTile");
        hex.tileType = Hex.Type.Water;
        hex.tile = tile;
        hex.transform.SetParent(transform);
        
        iHexGround.transform.SetParent(hex.transform);


        // just clean up hierarchy
        Transform selectFolder = hex.transform.Find("Main");
        if (selectFolder != null)
            tile.transform.SetParent(selectFolder.transform);
        else
            tile.transform.SetParent(hex.transform);

        tile.transform.localPosition -= new Vector3(0f,0.6f,0f);

        return hex;
    }

    Hex HexTileSpawn(GameObject[] prefabs, GameObject[] prefabsDefault, float xPos, float zPos, int percentage, int costSelected, int costDefalut)
    {
        bool isObjTile = Random.Range(0, percentage) == 0;
        //float setHigh = Random.Range(0, 2) * 0.5f;
        Vector3 spawnPos = new Vector3(xPos, 0, zPos);

        Hex hex = Instantiate(_hexPrefab, spawnPos, Quaternion.identity);
        GameObject iHexGround = Instantiate(hexGround[0], spawnPos, Quaternion.identity);
        iHexGround.layer = LayerMask.NameToLayer("HexTileGround"); 

        GameObject tile = null;
        float randomRotationY = Random.Range(0, 6) * 60;
        if (isObjTile)
        {
            
            tile = Instantiate(prefabs[Random.Range(0, prefabs.Length)], spawnPos, Quaternion.Euler(0f, randomRotationY, 0f));
            hex.cost = costSelected;
            hex.tileType = (costSelected == -1) ? Hex.Type.Obstacle : Hex.Type.Object;
            
        }
        else
        {
            tile = Instantiate(prefabsDefault[Random.Range(0, prefabsDefault.Length)], spawnPos, Quaternion.Euler(0f, randomRotationY, 0f));
            hex.cost = costDefalut;
            hex.tileType = Hex.Type.Field;
        }

        tile.layer = LayerMask.NameToLayer("HexTile");
        hex.tile = tile;
        hex.transform.SetParent(transform);
        iHexGround.transform.SetParent(hex.transform);

        Transform selectFolder = hex.transform.Find("Main");
        if (selectFolder != null)
            tile.transform.SetParent(selectFolder.transform);
        else
            tile.transform.SetParent(hex.transform);

        if (hex.tileType == Hex.Type.Object)
        {
            objTilesGo.Add(hex.gameObject);
        }

        return hex;
    }
    #endregion

    public void SetNavMesh()
    {
        Debug.Log("Nav set Start");
        if (navMeshSurface != null)
            navMeshSurface.BuildNavMesh();


        materialsConverter.ConvertMat(objTilesGo);
        EventSetNavComplete?.Invoke(this, EventArgs.Empty);
    }
}


using System;
using System.Collections.Generic;
using System.IO.Compression;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class GridMaker : MonoBehaviour
{
    [Header("Ref")]
    public GameObject hexPrefab; // ???????????? ?????? ??????
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
    [SerializeField] int costWater = 3;

    public GameObject[] tileOcean;
    [SerializeField] int costOcean = -1;

    public GameObject[] tileIsland;
    [SerializeField] int costIsland = 1;
    [Space(10)]
    public GameObject[] hexGround; // 0 Groudn 1 River
    public Material[] materials;
    [Space(20)]
    public int gridSizeN = 7; // ?????? ???? N
    public int oceanSizeN = 5;

    public List<GameObject> objTilesGo = new List<GameObject>();

    private float hexWidth = 4.325f; // horizontal
    private float hexHeight = 5.0f;  // vertical
    
    private bool isRiver;
    private bool isOcean;

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

                if (MathF.Abs(q) > gridSizeN - oceanSizeN || r <= r1 + oceanSizeN - 1 || r >= r2 - oceanSizeN + 1) // 
                    isOcean = true;
                else
                    isOcean= false;
                

                if (isOcean)
                {
                    OceanSpawn(tileOcean, xPos, zPos, costOcean);
                }
                else
                {
                    switch(Random.Range(0, 10))
                    {
                        case 0:
                            HexTileSpawn(tileDungon, tileNormal, xPos, zPos, 10, costDungon, costNormal);
                            break;
                        case 1:
                            HexTileSpawn(tileHill, tileNormal, xPos, zPos, 10, costHill, costNormal);
                            break;
                        case 2:
                            HexTileSpawn(tileCastle, tileNormal, xPos, zPos, 10, costCastle, costNormal);
                            break;
                        case 3:
                            HexTileSpawn(tileVillage, tileNormal, xPos, zPos, 10, costVillage, costNormal);
                            break;
                        case 4:
                        case 5:
                            OceanSpawn(tileOcean, xPos, zPos, costWater);
                            break;
                        default:
                            HexTileSpawn(tileRock, tileNormal, xPos, zPos, 5, costRock, costNormal);
                            break;
                    }  
                }
            }
        }
        EventBuildComplete?.Invoke(this, EventArgs.Empty);
        SetNavMesh();
    }

    GameObject OceanSpawn(GameObject[] prefabs, float xPos, float zPos, int cost)
    {
        Vector3 spawnPos = new Vector3(xPos, -0.5f, zPos);
        GameObject hexGO = Instantiate(hexPrefab, spawnPos, Quaternion.identity);

        GameObject iHexGround = Instantiate(hexGround[1], spawnPos, Quaternion.identity);
        iHexGround.layer = LayerMask.NameToLayer("HexTileGround");

        Hex hex = hexGO.GetComponent<Hex>();
        GameObject tile = Instantiate(prefabs[Random.Range(0, prefabs.Length)], spawnPos, Quaternion.Euler(0f, 30f, 0f));
        hex.cost = cost;
        tile.layer = LayerMask.NameToLayer("HexTile");
        hex.tile = tile;
        hexGO.transform.SetParent(transform);
        
        iHexGround.transform.SetParent(hexGO.transform);


        // just clean up hierarchy
        Transform selectFolder = hexGO.transform.Find("Main");
        if (selectFolder != null)
            tile.transform.SetParent(selectFolder.transform);
        else
            tile.transform.SetParent(hexGO.transform);

        tile.transform.localPosition -= new Vector3(0f,0.6f,0f);
        return hexGO;
    }

    GameObject HexTileSpawn(GameObject[] prefabs, GameObject[] prefabsDefault, float xPos, float zPos, int percentage, int costSelected, int costDefalut)
    {
        bool isObjTile = Random.Range(0, percentage) == 0;
        //float setHigh = Random.Range(0, 2) * 0.5f;
        Vector3 spawnPos = new Vector3(xPos, 0, zPos);

        GameObject hexGO = Instantiate(hexPrefab, spawnPos, Quaternion.identity);
        GameObject iHexGround = Instantiate(isRiver ? hexGround[1] : hexGround[0], spawnPos, Quaternion.identity);
        iHexGround.layer = LayerMask.NameToLayer("HexTileGround"); 
        Hex hex = hexGO.GetComponent<Hex>();

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
        hexGO.transform.SetParent(transform);
        iHexGround.transform.SetParent(hexGO.transform);

        Transform selectFolder = hexGO.transform.Find("Main");
        if (selectFolder != null)
            tile.transform.SetParent(selectFolder.transform);
        else
            tile.transform.SetParent(hexGO.transform);

        if (hex.tileType == Hex.Type.Object)
        {
            objTilesGo.Add(hexGO);
        }

        return hexGO;
    }

    public void SetNavMesh()
    {
        Debug.Log("Nav set Start");
        if (navMeshSurface != null)
            navMeshSurface.BuildNavMesh();


        materialsConverter.ConvertMat(objTilesGo);
        EventSetNavComplete?.Invoke(this, EventArgs.Empty);
    }
}


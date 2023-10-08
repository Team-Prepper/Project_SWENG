using Photon.Pun;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

[System.Serializable]
class TileData
{
    public GameObject[] tiles;
    public int cost = 0;
    public int tileType = 0;

    public TileData(GameObject[] tiles, int cost, int tileType)
    {
        this.tiles = tiles;
        this.cost = cost;
        this.tileType = tileType;
    }

    public int GetTileType()
    {
        return tileType;
    }
}

public class NetworkGridMaker : MonoBehaviourPun
{
    [Header("Network")]
    private PhotonView _PhotonView;
    Dictionary<int, TileData> tileDic = new Dictionary<int, TileData>();

    [Header("Ref")]
    [SerializeField] private Hex _hexPrefab;

    [SerializeField] TileData _tileNormal   = new TileData(null, 5, 0);
    [SerializeField] TileData _tileRock     = new TileData(null, 5, 1);
    [SerializeField] TileData _tileHill     = new TileData(null, 5, 2);
    [SerializeField] TileData _tileDungeon  = new TileData(null, 5, 3);
    [SerializeField] TileData _tileCastle   = new TileData(null, 5, 4);
    [SerializeField] TileData _tileVillage  = new TileData(null, 5, 5);
    [SerializeField] TileData _tileOcean    = new TileData(null, 5, 6);

    

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

    public static event EventHandler EventBuildComplete;
    public static event EventHandler EventConvertMaterials;


    private void Awake()
    {
        _PhotonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        gridSizeN += oceanSizeN;
        SetTileDict();

        if (PhotonNetwork.IsMasterClient)
            CreateHexGrid();
    }

    private void SetTileDict()
    {
        tileDic.Add(0, _tileNormal);
        tileDic.Add(1, _tileRock);
        tileDic.Add(2, _tileHill);
        tileDic.Add(3, _tileDungeon);
        tileDic.Add(4, _tileCastle);
        tileDic.Add(5, _tileVillage);
        tileDic.Add(6, _tileOcean);
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
                        _HexTileSpawn(_tileDungeon, xPos, zPos, 10);
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
        _PhotonView.RPC("ConvertMaterials", RpcTarget.All, null);
    }

    void OceanSpawn(TileData data, float xPos, float zPos)
    {
        Vector3 spawnPos = new Vector3(xPos, -0.5f, zPos);

        SpawnHexHandler(data, spawnPos, GetRandomTile(data));
    }

    private void _HexTileSpawn(TileData selectedData, float xPos, float zPos, int percentage)
    {
        Vector3 spawnPos = new Vector3(xPos, 0, zPos);

        // Empty Field
        if (Random.Range(0, percentage) != 0)
        {
            SpawnHexHandler(_tileNormal, spawnPos, GetRandomTile(_tileNormal));
        }
        else
        {
            SpawnHexHandler(selectedData, spawnPos, GetRandomTile(selectedData));
        }
    }

    private int GetRandomTile(TileData data)
    {
        return Random.Range(0, data.tiles.Length);
    }

    private void SpawnHexHandler(TileData data, Vector3 spawnPos, int hexNo)
    {

      _PhotonView.RPC("_SpawnHexTile", RpcTarget.All, data.GetTileType(), spawnPos, hexNo);
    }


    [PunRPC]
    private void _SpawnHexTile(int tileType, Vector3 spawnPos, int hexNo)
    {
        TileData data = tileDic[tileType];
        GameObject tile = Instantiate(data.tiles[hexNo], spawnPos, Quaternion.Euler(0f, Random.Range(0, 6) * 60, 0f));
        tile.layer = LayerMask.NameToLayer("HexTile");

        Hex hex = Instantiate(_hexPrefab, spawnPos, Quaternion.identity).GetComponent<Hex>();
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

        switch (data.tileType)
        {
            case 0://field
                hex.tileType = Hex.Type.Field;
                HexGrid.Instance.emptyHexTiles.Add(hex);
                break;
            case 1://rock
                hex.tileType = Hex.Type.Obstacle;
                break;
            case 2://hill
                break;
            case 3: //dungeon
                hex.tileType = Hex.Type.Object;
                objTilesGo.Add(hex.gameObject);
                break;
            case 4: //castle
                hex.tileType = Hex.Type.Object;
                objTilesGo.Add(hex.gameObject);
                break;
            case 5://village
                hex.tileType = Hex.Type.Shop;
                break;
            case 6://water
                hex.tileType = Hex.Type.Water;
                hex.tile.transform.localPosition -= new Vector3(0f, 0.6f, 0f);
                break;
        }
    }

    [PunRPC]
    public void ConvertMaterials()
    {
        MaterialsConverter.ConvertMat(objTilesGo);
        if(PhotonNetwork.IsMasterClient)
            EventConvertMaterials?.Invoke(this, EventArgs.Empty);
    }
}


using System.Collections.Generic;
using UnityEngine;

public class GridMaker : MonoBehaviour
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

    public TileDataScript GetTileData(TileDataScript.TileType type) {
        TileDataScript tileType = null;
        switch (type)
        {
            case TileDataScript.TileType.normal:
                tileType = _tileNormal;
                break;
            case TileDataScript.TileType.obstacle:
                tileType = _tileRock;
                break;
            case TileDataScript.TileType.hill:
                tileType = _tileHill;
                break;
            case TileDataScript.TileType.dungon:
                tileType = _tileDungon;
                break;
            case TileDataScript.TileType.castle:
                tileType = _tileCastle;
                break;
            case TileDataScript.TileType.village:
                tileType = _tileVillage;
                break;
            case TileDataScript.TileType.ocean:
                tileType = _tileOcean;
                break;
        }
        return tileType;
    }

    public void CreateHexGrid()
    {
        _tileSetters = new List<HexTileSetter>();
        if (_nowWorking) DestroyImmediate(_nowWorking);
        _nowWorking = new GameObject("Map");
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

        _nowWorking.transform.eulerAngles = new Vector3(-90, 0, 0);
    }

    public void EndEdit()
    {
        _nowWorking.transform.eulerAngles = new Vector3(0, 0, 0);
        foreach (HexTileSetter setter in _tileSetters) {
            DestroyImmediate(setter);
        }
        _tileSetters = new List<HexTileSetter>();
        _nowWorking = null;
    }

    public void RemoveAll()
    {
        if (_nowWorking == null) return;
        DestroyImmediate(_nowWorking);
        _tileSetters = new List<HexTileSetter>();
    }

    private Hex _SpawnHexTile(Vector3 spawnPos)
    {
        Hex hex = Instantiate(_hexPrefab, spawnPos, Quaternion.identity);
        HexTileSetter tileSetter = hex.gameObject.AddComponent<HexTileSetter>();
        tileSetter.SetInfor(this);
        _tileSetters.Add(tileSetter);
        hex.transform.SetParent(_nowWorking.transform);

        GameObject iHexGround = Instantiate(hexGround, spawnPos, Quaternion.identity);
        iHexGround.layer = LayerMask.NameToLayer("HexTileGround");
        iHexGround.transform.SetParent(hex.transform);

        return hex;
    }
    
    private Hex _SpawnHexTile(TileDataScript data, Vector3 spawnPos)
    {

        GameObject tile = Instantiate(data.tiles[Random.Range(0, data.tiles.Length)], spawnPos, Quaternion.Euler(0f, Random.Range(0, 6) * 60, 0f));
        tile.layer = LayerMask.NameToLayer("HexTile");

        Hex hex = Instantiate(_hexPrefab, spawnPos, Quaternion.identity);

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
}


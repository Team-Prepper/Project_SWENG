using System.Collections.Generic;
using BKTools.Gaming.GridMap2D;
using UnityEngine;

namespace SWEng
{

    public class MapMaker : MonoBehaviour
    {
        GameObject _nowWorking;
        [Header("Ref")]
        [SerializeField] private MapUnit _hexPrefab;
        [SerializeField] private Vector2Int _pointMove;

        [SerializeField] TileDataScript _tileNormal;
        [SerializeField] TileDataScript _tileRock;
        [SerializeField] TileDataScript _tileHill;
        [SerializeField] TileDataScript _tileDungon;
        [SerializeField] TileDataScript _tileCastle;
        [SerializeField] TileDataScript _tileVillage;
        [SerializeField] TileDataScript _tileOcean;

        List<MapUnitSetter> _tileSetters;

        [Space(10)]
        public GameObject hexGround;

        [Space(20)]
        public int gridSizeN = 7; // ?????? ???? N
        public int oceanSizeN = 5;

        public TileDataScript GetTileData(TileDataScript.TileType type)
        {
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
            _tileSetters = new List<MapUnitSetter>();
            if (_nowWorking) DestroyImmediate(_nowWorking);

            _nowWorking = new GameObject("Map");

            //_nowWorking.AddComponent<PlayerSpawner>();
            //_nowWorking.AddComponent<EnemySpawner>();

            _nowWorking.transform.SetParent(transform);

            foreach (GridCoord2D hex in Coord2DManager.Instance.
                Convertor.GetNeighboursFor(
                    new GridCoord2D(_pointMove.x, _pointMove.y), gridSizeN))
            {
                _SpawnHexTile(MapUnitManager.Instance.
                    Convertor.ConvertToVector3(hex));
            }

            _nowWorking.transform.eulerAngles = new Vector3(-90, 0, 0);

        }

        public void EndEdit()
        {
            _nowWorking.transform.eulerAngles = new Vector3(0, 0, 0);
            foreach (MapUnitSetter setter in _tileSetters)
            {
                DestroyImmediate(setter);
            }
            _tileSetters = new List<MapUnitSetter>();
            _nowWorking = null;
        }

        public void RemoveAll()
        {
            if (_nowWorking == null) return;
            DestroyImmediate(_nowWorking);
            _tileSetters = new List<MapUnitSetter>();
        }

        private MapUnit _SpawnHexTile(Vector3 spawnPos)
        {
            MapUnit hex = Instantiate(_hexPrefab, spawnPos, Quaternion.identity);
            MapUnitSetter tileSetter = hex.gameObject.AddComponent<MapUnitSetter>();
            
            tileSetter.SetInfor(this);
            _tileSetters.Add(tileSetter);
            hex.transform.SetParent(_nowWorking.transform);

            GameObject iHexGround = Instantiate(hexGround, spawnPos, Quaternion.identity);
            iHexGround.layer = LayerMask.NameToLayer("HexTileGround");
            iHexGround.transform.SetParent(hex.transform);

            return hex;
        }

        private MapUnit _SpawnHexTile(TileDataScript data, Vector3 spawnPos)
        {

            GameObject tile = Instantiate(data.tiles[Random.Range(0, data.tiles.Length)], spawnPos, Quaternion.Euler(0f, Random.Range(0, 6) * 60, 0f));
            tile.layer = LayerMask.NameToLayer("HexTile");

            MapUnit hex = Instantiate(_hexPrefab, spawnPos, Quaternion.identity);

            GameObject iHexGround = Instantiate(hexGround, spawnPos, Quaternion.identity);
            iHexGround.layer = LayerMask.NameToLayer("HexTileGround");
            iHexGround.transform.SetParent(hex.transform);

            Transform selectFolder = hex.transform.Find("Main");
            if (selectFolder != null)
                tile.transform.SetParent(selectFolder.transform);
            else
                tile.transform.SetParent(hex.transform);

            tile.AddComponent<MapUnitSetter>();

            MapUnitManager.Instance.AddMapUnit(hex);

            return hex;
        }
    }
}
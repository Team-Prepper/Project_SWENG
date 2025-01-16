using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TileDataScript;

public class Map : MonoBehaviour {

    [SerializeField] float xOffset = 4.325f, zOffset = 5.0f; //  yOffset = 0.5f,

    [SerializeField] MapUnit[] _playerSpawnPos;
    [SerializeField] MapUnit[] _enemySpawnPos;
    [SerializeField] MapUnit[] _bossSpawnPos;

    [SerializeField] MapUnit[] _allMapUnit;

    public MapUnit[] PlayerSpawnPos => _playerSpawnPos;
    public MapUnit[] EnemySpawnPos => _enemySpawnPos;
    public MapUnit[] BossSpawnPos => _bossSpawnPos;

    private void Awake()
    {
        HexGrid.Instance.Map = this;
        HexGrid.Instance.SetCoordConstant(xOffset, zOffset);

        foreach (MapUnit m in _allMapUnit)
        {
            HexGrid.Instance.AddTile(m);
            m.SetCost(_GetCost(m.tileType));
        }
    }

    private int _GetCost(TileType tileType)
    {
        switch (tileType)
        {
            case TileType.normal:
                return 2;
            case TileType.hill:
                return 3;
            case TileType.castle:
                return 4;
            case TileType.dungon:
                return 5;
            case TileType.obstacle:
                return -1;
            case TileType.ocean:
                return -1;
            case TileType.village:
                return 1;
            default:
                return 10;
        }
    }
}

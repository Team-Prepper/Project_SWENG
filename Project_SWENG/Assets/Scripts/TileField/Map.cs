using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] MapUnit[] _playerSpawnPos;
    [SerializeField] MapUnit[] _enemySpawnPos;
    [SerializeField] MapUnit[] _bossSpawnPos;

    public MapUnit[] PlayerSpawnPos => _playerSpawnPos;
    public MapUnit[] EnemySpawnPos => _enemySpawnPos;
    public MapUnit[] BossSpawnPos => _bossSpawnPos;

    private void Awake()
    {
        HexGrid.Instance.Map = this;
    }
}

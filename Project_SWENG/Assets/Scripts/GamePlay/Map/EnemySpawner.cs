using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    private IActionSelector _acitonSelector;
    private IList<MapUnit> _spawnableMapUnit;

    private void Awake() {
        _acitonSelector = gameObject.AddComponent<BasicEnemyActionSelector>();
    }

    public void SpawnEnemy()
    {
        _spawnableMapUnit = new List<MapUnit>();
        
        foreach (MapUnit mu in HexGrid.Instance.Map.EnemySpawnPos) {
            if (mu.CC != null) continue;
            _spawnableMapUnit.Add(mu);
        }

        for (int i = 0; i < GameManager.Instance.GameSetting.EnemyCnt; i++)
        {
            SpawnEnemy(GetRandHex(), GameManager.Instance.GameSetting.Enemy);
        }
    }

    public void SpawnBoss()
    {
        _spawnableMapUnit = new List<MapUnit>();
        
        foreach (MapUnit mu in HexGrid.Instance.Map.EnemySpawnPos) {
            if (mu.CC != null) continue;
            _spawnableMapUnit.Add(mu);
        }

        SpawnEnemy(GetBossPos(), GameManager.Instance.GameSetting.BossEnemy);
    }

    private void SpawnEnemy(MapUnit spawnHex, IList<string> spawnEnemyList)
    {
        if (spawnHex == null) return;
        
        Transform spawnPos = spawnHex.transform;

        GameObject enemy = GameManager.Instance.GameMaster.InstantiateCharacter(spawnPos.position, spawnPos.rotation);

        ICharacterController cc = enemy.GetComponent<ICharacterController>();
        
        cc.Initial(spawnEnemyList[Random.Range(0, spawnEnemyList.Count)], 1, true);
        cc.SetActionSelector(_acitonSelector);
        
    }

    private MapUnit GetBossPos() {

        for (int i = 0; i < HexGrid.Instance.Map.BossSpawnPos.Length; i++) {
            if (HexGrid.Instance.Map.BossSpawnPos[i].CC != null) continue;
            return HexGrid.Instance.Map.BossSpawnPos[i];
        }

        return HexGrid.Instance.Map.BossSpawnPos[0];

    }
    
    private MapUnit GetRandHex()
    {
        if(_spawnableMapUnit.Count == 0) return null;

        int randHexIdx = Random.Range(0, _spawnableMapUnit.Count);

        MapUnit randHex = _spawnableMapUnit[randHexIdx];
        _spawnableMapUnit.RemoveAt(randHexIdx);

        return randHex;
    }

}

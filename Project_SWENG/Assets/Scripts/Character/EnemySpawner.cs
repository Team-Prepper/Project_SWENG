using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemyPrefabList;
    public List<GameObject> bossEnemyPrefabList;
    public GameObject bossEnemyPrefab;
    public List<Hex> enemySpawnPos = new List<Hex>();
    public GameObject bossCam;
    public Hex stageBossPos;

    [SerializeField] private int enemyCnt = 9;
    [SerializeField] private int bossEnemyCnt = 3;

    EnemyPlayer _enemyPlayer;

    public void SpawnEnemy()
    {
        _enemyPlayer = gameObject.AddComponent<EnemyPlayer>();

        for (int i = 0; i < bossEnemyCnt; i++)
        {
            SpawnEnemy(GetRandHex(), bossEnemyPrefabList);
        }
        for (int i = 0; i < enemyCnt; i++)
        {
            SpawnEnemy(GetRandHex(), enemyPrefabList);
        }
    }

    private void SpawnEnemy(Hex spawnHex, List<GameObject> spawnEnemyList)
    {
        Transform spawnPos = spawnHex.transform;
        GameObject enemy = GameManager.Instance.GameMaster.InstantiateCharacter(spawnPos.position, spawnPos.rotation);

        ICharacterController cc = enemy.GetComponent<ICharacterController>();
        
        cc.Initial(spawnEnemyList[Random.Range(0, spawnEnemyList.Count)].name, true);
        cc.SetActionSelector(new BasicEnemyActionSelector(_enemyPlayer));
        
    }
    
    private Hex GetRandHex()
    {
        if(enemySpawnPos.Count == 0) return null;

        int randHexIndex = Random.Range(0, enemySpawnPos.Count);
        Hex randHex = enemySpawnPos[randHexIndex];
        enemySpawnPos.Remove(randHex);
        return randHex;
    }

    public GameObject SpawnBoss()
    {
        GameObject enemy = GameManager.Instance.GameMaster.InstantiateCharacter(stageBossPos.transform.position, stageBossPos.transform.rotation);

        ICharacterController cc = enemy.GetComponent<ICharacterController>();

        cc.Initial(bossEnemyPrefab.name, true);
        cc.SetActionSelector(new BasicEnemyActionSelector(_enemyPlayer));

        return enemy;
    }

}

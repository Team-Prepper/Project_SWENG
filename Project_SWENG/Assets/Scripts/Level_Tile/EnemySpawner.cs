using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemyPrefabList;
          
    public Dictionary<GameObject, Hex> enemyDic = new Dictionary<GameObject, Hex>();

    private void Awake()
    {
        GridMaker.EventSetNavComplete += SpawnEnemyHandler;
    }

    private void SpawnEnemyHandler(object sender, EventArgs e)
    {
        Debug.Log("EnemySpawn");
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {

        Hex spawnHex = HexGrid.Instance.GetRandHexAtEmpty();

        Transform spawnPos = spawnHex.transform;
        GameObject enemy = Instantiate(enemyPrefabList[Random.Range(0,enemyPrefabList.Count)], spawnPos.position, spawnPos.rotation);
        enemyDic.Add(enemy, spawnHex);
    }
}

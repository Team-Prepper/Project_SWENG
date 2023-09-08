using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemyPrefabList;
          
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

        Hex spawnHex = GetEmptyHex();

        Transform spawnPos = spawnHex.transform;
        GameObject enemy = Instantiate(enemyPrefabList[Random.Range(0,enemyPrefabList.Count)], spawnPos.position, spawnPos.rotation);
    }

    private Hex GetEmptyHex()
    {
        Hex spawnHex = HexGrid.Instance.GetRandHex();

        while (spawnHex && spawnHex.tileType != Hex.Type.Field)
        {
            spawnHex = HexGrid.Instance.GetRandHex();
        }

        spawnHex.isEnemy = true;

        return spawnHex;
    }
}

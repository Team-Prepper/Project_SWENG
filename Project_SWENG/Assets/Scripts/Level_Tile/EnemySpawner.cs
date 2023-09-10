using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemyPrefabList;
          
    public Dictionary<GameObject, Hex> enemyDic = new Dictionary<GameObject, Hex>();

    private int enemyCnt = 0;
    [SerializeField] private int diff = 10;

    private void Awake()
    {
        GridMaker.EventSetNavComplete += SpawnEnemyHandler;
        enemyCnt = diff;
    }

    private void SpawnEnemyHandler(object sender, EventArgs e)
    {
        Debug.Log("EnemySpawn");
        for(int i = 0; i < enemyCnt; i++)
        {
            Hex spawnHex = HexGrid.Instance.GetRandHexAtEmpty();
            SpawnEnemy(spawnHex);
        }
    }

    private void SpawnEnemy(Hex spawnHex)
    {
        Transform spawnPos = spawnHex.transform;
        GameObject enemy = Instantiate(enemyPrefabList[Random.Range(0,enemyPrefabList.Count)], spawnPos.position, spawnPos.rotation);
        enemy.transform.SetParent(transform);

        spawnHex.Entity = enemy;
        enemyDic.Add(enemy, spawnHex);
    }
}

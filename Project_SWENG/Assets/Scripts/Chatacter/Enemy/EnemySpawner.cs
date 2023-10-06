using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using Character;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoSingleton<EnemySpawner>
{
    public List<GameObject> enemyPrefabList;
    public List<GameObject> enemyList;
    //public Dictionary<GameObject, Hex> enemyDic = new Dictionary<GameObject, Hex>();

    [SerializeField] private int diff = 20;
    private int enemyCnt = 0;
    

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
        GameManager.Instance.enemys = enemyList;
    }

    private void SpawnEnemy(Hex spawnHex)
    {
        Transform spawnPos = spawnHex.transform;
        GameObject enemy = Instantiate(enemyPrefabList[Random.Range(0,enemyPrefabList.Count)], spawnPos.position, spawnPos.rotation);
        enemy.transform.SetParent(transform);

        spawnHex.Entity = enemy;
        EnemyController enemyController = enemy.GetComponent<EnemyController>();
        enemyList.Add(enemy);
        enemyController.curHex = spawnHex;
    }
}

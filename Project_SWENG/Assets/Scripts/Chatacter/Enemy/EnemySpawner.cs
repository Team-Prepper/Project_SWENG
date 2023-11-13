using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using Character;
using Random = UnityEngine.Random;
using Photon.Pun;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemySpawner : MonoBehaviourPun
{
    
    public List<GameObject> enemyPrefabList;
    public List<GameObject> enemyList;
    //public Dictionary<GameObject, Hex> enemyDic = new Dictionary<GameObject, Hex>();

    public List<Hex> enemySpawnPos = new List<Hex>();

    private int enemyCnt = 10;
    

    private void Start()
    {
        SpawnEnemyHandler();
    }

    private void SpawnEnemyHandler()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            Debug.Log("EnemySpawn");
            for (int i = 0; i < enemyCnt; i++)
            {
                Hex spawnHex = GetRandHex();
                Debug.Log("EnemySpawnHex : " + spawnHex.name);
                SpawnEnemy(spawnHex);
            }
            GameManager.Instance.enemys = enemyList;
        }
        
    }

    private void SpawnEnemy(Hex spawnHex)
    {
        Transform spawnPos = spawnHex.transform;
        GameObject enemy = PhotonNetwork.Instantiate(enemyPrefabList[Random.Range(0,enemyPrefabList.Count)].name, spawnPos.position, spawnPos.rotation);
        enemy.GetComponent<EnemyController>().enemySpawner = this;
        enemyList.Add(enemy);
    }
    
    private Hex GetRandHex()
    {
        if(enemySpawnPos.Count == 0) return null;

        int randHexIndex = Random.Range(0, enemySpawnPos.Count);
        Hex randHex = enemySpawnPos[randHexIndex];
        enemySpawnPos.Remove(randHex);
        return randHex;
    }
}

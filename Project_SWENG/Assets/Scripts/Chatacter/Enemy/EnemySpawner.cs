using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character;
using Random = UnityEngine.Random;
using Photon.Pun;

public class EnemySpawner : MonoBehaviourPun
{
    public List<GameObject> enemyPrefabList;
    public List<GameObject> bossEnemyPrefabList;
    public GameObject bossEnemyPrefab;
    public List<Hex> enemySpawnPos = new List<Hex>();
    public GameObject bossCam;
    public Hex stageBossPos;

    private int enemyCnt = 9;
    private int bossEnemyCnt = 3;

    private void Start()
    {
        SpawnEnemyHandler();
        bossCam.SetActive(false);
    }

    private void SpawnEnemyHandler()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            Debug.Log("EnemySpawn");
            for (int i = 0; i < bossEnemyCnt; i++)
            {
                Hex spawnHex = GetRandHex();
                Debug.Log("BossEnemySpawnHex : " + spawnHex.name);
                SpawnEnemy(spawnHex, bossEnemyPrefabList);
            }
            for (int i = 0; i < enemyCnt; i++)
            {
                Hex spawnHex = GetRandHex();
                Debug.Log("EnemySpawnHex : " + spawnHex.name);
                SpawnEnemy(spawnHex, enemyPrefabList);
            }
        }
        
    }

    private void SpawnEnemy(Hex spawnHex, List<GameObject> spawnEnemyList)
    {
        Transform spawnPos = spawnHex.transform;
        GameObject enemy = PhotonNetwork.Instantiate(spawnEnemyList[Random.Range(0,spawnEnemyList.Count)].name, spawnPos.position, spawnPos.rotation);
        EnemyController enemyController = enemy.GetComponent<EnemyController>();
        if (enemyController)
        {
            enemyController.enemySpawner = this;
            if(enemyController.enemyStat.isBoss)
                GameManager.Instance.bossEnemies.Add(enemy);
        }
            
        GameManager.Instance.enemies.Add(enemy);
        
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
        GameObject enemy = PhotonNetwork.Instantiate(bossEnemyPrefab.name, stageBossPos.transform.position, stageBossPos.transform.rotation);
        photonView.RPC("SetBossCam", RpcTarget.All, null);
        
        return enemy;
    }

    [PunRPC]
    private void SetBossCam()
    {
        bossCam.SetActive(true);
        StartCoroutine(BossCamOut());
    }

    IEnumerator BossCamOut()
    {
        yield return new WaitForSeconds(3f);
        bossCam.SetActive(false);
    }
}

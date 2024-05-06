using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterSystem;
using Random = UnityEngine.Random;

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

    public void SpawnEnemy()
    {
        for (int i = 0; i < bossEnemyCnt; i++)
        {
            //Debug.Log("BossEnemySpawnHex : " + spawnHex.name);
            SpawnEnemy(GetRandHex(), bossEnemyPrefabList);
        }
        for (int i = 0; i < enemyCnt; i++)
        {
            //Debug.Log("EnemySpawnHex : " + spawnHex.name);
            SpawnEnemy(GetRandHex(), enemyPrefabList);
        }
    }

    private void SpawnEnemy(Hex spawnHex, List<GameObject> spawnEnemyList)
    {
        Transform spawnPos = spawnHex.transform;
        GameObject enemy = GameManager.Instance.GameMaster.InstantiateCharacter(spawnPos.position, spawnPos.rotation);

        ICharacterController cc = enemy.GetComponent<ICharacterController>();
        
        cc.Initial(spawnEnemyList[Random.Range(0, spawnEnemyList.Count)].name);
        cc.SetActionSelector(enemy.AddComponent<BasicEnemyActionSelector>());
        /*
        EnemyController enemyController = enemy.GetComponent<EnemyController>();

        if (enemyController)
        {
            //if(enemyController.enemyStat.isBoss)
                //GameManager.Instance.bossEnemies.Add(enemy);
        }
            
        //GameManager.Instance.enemies.Add(enemy);
        */
        
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

        cc.Initial(bossEnemyPrefab.name);
        cc.SetActionSelector(enemy.AddComponent<BasicEnemyActionSelector>());
        enemy.GetComponent<ICharacterController>().SetActionSelector(enemy.AddComponent<BasicEnemyActionSelector>());

        return enemy;
    }

    /*
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
    }*/
}

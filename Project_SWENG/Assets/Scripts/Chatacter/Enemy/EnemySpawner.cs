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

    [SerializeField] private int diff = 20;
    private int enemyCnt = 0;
    

    private void Awake()
    {
        NetworkGridMaker.EventConvertMaterials += SpawnEnemyHandler;
        enemyCnt = diff;
    }

    private void SpawnEnemyHandler(object sender, EventArgs e)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            Debug.Log("EnemySpawn");
            for (int i = 0; i < enemyCnt; i++)
            {
                Hex spawnHex = HexGrid.Instance.GetRandHexAtEmpty();
                SpawnEnemy(spawnHex);
            }
            GameManager.Instance.enemys = enemyList;
        }
        
    }

    private void SpawnEnemy(Hex spawnHex)
    {
        Transform spawnPos = spawnHex.transform;
        GameObject enemy = PhotonNetwork.Instantiate(enemyPrefabList[Random.Range(0,enemyPrefabList.Count)].name, spawnPos.position, spawnPos.rotation);
        enemy.transform.SetParent(transform);
        enemy.GetComponent<EnemyController>().enemySpawner = this;
        enemyList.Add(enemy);
    }
}

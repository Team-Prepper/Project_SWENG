using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] string _path = "EnemyData";
    [SerializeField] string _enemyPrefabKey = "Enemy";
    [SerializeField] string _bossEnemyPrefabKey = "BossEnemy";

    public GameObject _stageBossEnemyPrefab;

    public GameObject bossCam;

    [SerializeField] private int enemyCnt = 9;
    [SerializeField] private int bossEnemyCnt = 3;

    EnemyPlayer _enemyPlayer;

    public void SpawnEnemy()
    {
        _enemyPlayer = gameObject.AddComponent<EnemyPlayer>();

        JsonDataConnector<string, string[]> connector = new JsonDataConnector<string, string[]>();

        connector.Connect(_path);

        for (int i = 0; i < bossEnemyCnt; i++)
        {
            SpawnEnemy(GetRandHex(), connector.Get(_bossEnemyPrefabKey));
        }
        for (int i = 0; i < enemyCnt; i++)
        {
            SpawnEnemy(GetRandHex(), connector.Get(_enemyPrefabKey));
        }
    }

    private void SpawnEnemy(MapUnit spawnHex, string[] spawnEnemyList)
    {
        Transform spawnPos = spawnHex.transform;
        GameObject enemy = GameManager.Instance.GameMaster.InstantiateCharacter(spawnPos.position, spawnPos.rotation);

        ICharacterController cc = enemy.GetComponent<ICharacterController>();
        
        cc.Initial(spawnEnemyList[Random.Range(0, spawnEnemyList.Length)], 1, true);
        cc.SetActionSelector(new BasicEnemyActionSelector(_enemyPlayer));
        
    }
    
    private MapUnit GetRandHex()
    {
        if(HexGrid.Instance.Map.EnemySpawnPos.Length == 0) return null;

        int randHexIndex = Random.Range(0, HexGrid.Instance.Map.EnemySpawnPos.Length);
        MapUnit randHex = HexGrid.Instance.Map.EnemySpawnPos[randHexIndex];
        //HexGrid.Instance.Map.EnemySpawnPos.Remove(randHex);
        return randHex;
    }

    public GameObject SpawnBoss()
    {
        GameObject enemy = GameManager.Instance.GameMaster.InstantiateCharacter(
            HexGrid.Instance.Map.BossSpawnPos[0].transform.position, 
            HexGrid.Instance.Map.BossSpawnPos[0].transform.rotation);

        ICharacterController cc = enemy.GetComponent<ICharacterController>();

        cc.Initial(_stageBossEnemyPrefab.name, 1, true);
        cc.SetActionSelector(new BasicEnemyActionSelector(_enemyPlayer));

        return enemy;
    }

}

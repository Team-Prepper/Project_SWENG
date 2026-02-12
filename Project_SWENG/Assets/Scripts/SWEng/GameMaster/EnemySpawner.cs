using System.Collections.Generic;
using UnityEngine;

namespace SWEng
{
    public class EnemySpawner : MonoBehaviour
    {

        private ICharacterController _enemyCC;
        private BasicEnemyCharacterController _bossCC;
        private IList<MapUnit> _spawnableMapUnit;

        private void Awake()
        {

            _enemyCC =
                gameObject.AddComponent<BasicEnemyCharacterController>();
            _bossCC =
                gameObject.AddComponent<BasicEnemyCharacterController>();

            _bossCC.SetDicePointRange(6, 7);

        }

        public void SpawnEnemy()
        {
            _spawnableMapUnit = new List<MapUnit>();

            foreach (MapUnit mu in MapUnitManager.
                Instance.Map.EnemySpawnPos)
            {
                if (EntityManager.Instance.
                    GetEntityAt(mu.Pos) != null) continue;
                _spawnableMapUnit.Add(mu);
            }

            for (int i = 0; i < GameManager.Instance.Setting.EnemyCnt; i++)
            {
                SpawnEnemy(GetRandHex(), GameManager.Instance.Setting.Enemy, _enemyCC);
            }
        }

        public void SpawnBoss()
        {
            _spawnableMapUnit = new List<MapUnit>();

            foreach (MapUnit mu in MapUnitManager.
                Instance.Map.EnemySpawnPos)
            {
                if (EntityManager.Instance.
                    GetEntityAt(mu.Pos) != null) continue;
                _spawnableMapUnit.Add(mu);
            }

            SpawnEnemy(GetBossPos(), GameManager.Instance.Setting.BossEnemy, _bossCC);
        }

        private void SpawnEnemy(MapUnit spawnHex, IList<string> spawnEnemyList, ICharacterController characterController)
        {
            if (spawnHex == null) return;

            Transform spawnPos = spawnHex.transform;

            ICharacter cc = GameManager.Instance.Master.
                InstantiateCharacter(spawnPos.position, spawnPos.rotation);
            
            cc.SetCC(characterController);
            cc.TurnMemberState.SetTeamIdx(1);
            cc.Initial(spawnEnemyList[
                Random.Range(0, spawnEnemyList.Count)]);

        }

        private MapUnit GetBossPos()
        {

            foreach (MapUnit mu in MapUnitManager.
                Instance.Map.BossSpawnPos)
            {
                if (EntityManager.Instance.
                    GetEntityAt(mu.Pos) != null) continue;
                return mu;
            }

            return MapUnitManager.Instance.Map.BossSpawnPos[0];

        }

        private MapUnit GetRandHex()
        {
            if (_spawnableMapUnit.Count == 0) return null;

            int randHexIdx = Random.Range(0, _spawnableMapUnit.Count);

            MapUnit randHex = _spawnableMapUnit[randHexIdx];
            _spawnableMapUnit.RemoveAt(randHexIdx);

            return randHex;
        }

    }
}
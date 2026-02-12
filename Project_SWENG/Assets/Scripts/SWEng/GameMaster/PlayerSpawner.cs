using UnityEngine;
using UnityEngine.Events;
using System;

namespace SWEng
{

    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private ActionSelectorSpawner _asSpawner;
        public UnityEvent<ICharacter> EventPlayerSpawn;

        public void SpawnPlayer(int posIdx, Action callback = null)
        {
            Transform spawnPos = MapUnitManager.Instance.Map.
                PlayerSpawnPos[posIdx].transform;

            ICharacter character = GameManager.Instance.Master.
                InstantiateCharacter(spawnPos.position, spawnPos.rotation);

            character.SetCC(_asSpawner.Spawn());
                
            character.TurnMemberState.SetTeamIdx(0);
            character.Initial(GameManager.Instance.Setting.
                Players[posIdx].PlayerCharacter);

            callback?.Invoke();

            EventPlayerSpawn?.Invoke(character);

        }

    }
}
using UnityEngine;
using UnityEngine.Events;
using System;
using SWEng.GamePlay;
using SWEng.Data;

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
        
        character.TurnMemberState.SetTeamIdx(0);
        character.Initial(GameManager.Instance.Setting.
            Players[posIdx].PlayerCharacter);

        character.SetCC(_asSpawner.Spawn());
        
        callback?.Invoke();

        EventPlayerSpawn?.Invoke(character);

    }

}

using UnityEngine;
using UnityEngine.Events;
using EHTool.UIKit;
using System;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private string _playerActionSelectUIKey = "PlayerActionSelect";

    public UnityEvent<GameObject> EventPlayerSpawn;
    
    public void SpawnPlayer(int posIdx, Action callback = null)
    {

        Transform spawnPos = HexGrid.Instance.Map.PlayerSpawnPos[posIdx].transform;

        GameObject player = GameManager.Instance.GameMaster.
            InstantiateCharacter(spawnPos.position, spawnPos.rotation);
        ICharacterController cc = player.GetComponent<ICharacterController>();

        GUI_PlayerActionSelect playerActionSelector =
            UIManager.Instance.OpenGUI<GUI_PlayerActionSelect>(_playerActionSelectUIKey);
        cc.SetActionSelector(playerActionSelector);
        
        cc.Initial(GameManager.Instance.GameSetting.Players[posIdx].PlayerCharacter, 0, false);

        playerActionSelector.SetPlayer(player);
        callback?.Invoke();

        EventPlayerSpawn?.Invoke(player);

    }

}

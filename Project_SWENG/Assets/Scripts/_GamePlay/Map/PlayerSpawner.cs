using UnityEngine;
using UnityEngine.Events;
using EHTool.UIKit;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private string _playerActionSelectUIKey = "PlayerActionSelect";

    public UnityEvent<GameObject> EventPlayerSpawn;
    
    public void SpawnPlayer(int posIdx)
    {

        Transform spawnPos = HexGrid.Instance.Map.PlayerSpawnPos[posIdx].transform;

        GameObject player = GameManager.Instance.GameMaster.InstantiateCharacter(spawnPos.position, spawnPos.rotation);
        ICharacterController cc = player.GetComponent<ICharacterController>();
        cc.Initial(GameManager.Instance.GameSetting.Players[0].PlayerCharacter, 0, false);

        GUI_PlayerActionSelect playerActionSelector =
            UIManager.Instance.OpenGUI<GUI_PlayerActionSelect>(_playerActionSelectUIKey);
        cc.SetActionSelector(playerActionSelector);

        playerActionSelector.SetPlayer(player);

        EventPlayerSpawn?.Invoke(player);

    }

}

using UnityEngine;
using UnityEngine.Events;
using EHTool.UIKit;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private string _path = "GameData";
    [SerializeField] private string _playerKey = "Player";
    [SerializeField] private string _playerActionSelectUIKey = "PlayerActionSelect";

    public UnityEvent<GameObject> EventPlayerSpawn;
    
    public void SpawnPlayer(int posIdx)
    {

        Transform spawnPos = HexGrid.Instance.Map.PlayerSpawnPos[posIdx].transform;

        JsonDataConnector<string, string> jsonDataConnector = new JsonDataConnector<string, string>();
        jsonDataConnector.Connect(_path);

        GameObject player = GameManager.Instance.GameMaster.InstantiateCharacter(spawnPos.position, spawnPos.rotation);
        ICharacterController cc = player.GetComponent<ICharacterController>();
        cc.Initial(jsonDataConnector.Get(_playerKey), 0, false);

        GUI_PlayerActionSelect playerActionSelector =
            UIManager.Instance.OpenGUI<GUI_PlayerActionSelect>(_playerActionSelectUIKey);
        cc.SetActionSelector(playerActionSelector);

        playerActionSelector.SetPlayer(player);

        EventPlayerSpawn?.Invoke(player);

    }

}

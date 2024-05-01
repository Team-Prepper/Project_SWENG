using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UISystem;
using CharacterSystem;

public class PlayerSpawner : MonoBehaviour
{

    [Header("SpawnPos")]
    [SerializeField] private List<Hex> spawnPosList = new List<Hex>();

    [Header("SystemManager")]
    [SerializeField] GameObject playerPrefab;
    
    public UnityEvent<GameObject> EventPlayerSpawn;
    
    public void SpawnPlayer(int posIdx)
    {

        Hex spawnHex = spawnPosList[posIdx];

        Transform spawnPos = spawnHex.transform;
        IGameMaster GM = GameManager.Instance.GameMaster;

        GameObject player = GM.InstantiateCharacter(playerPrefab, spawnPos.position, spawnPos.rotation);
        GUI_PlayerInfor playerInfo = UIManager.OpenGUI<GUI_PlayerInfor>("PlayerInfor");
        playerInfo.SetPlayer(player);

        player.GetComponent<ICharacterController>().SetActionSelector(playerInfo);
        spawnHex.Entity = player;

        EventPlayerSpawn?.Invoke(player);

    }

}

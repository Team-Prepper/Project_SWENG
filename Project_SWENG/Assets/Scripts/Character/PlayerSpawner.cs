using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using EHTool.UIKit;

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

        GameObject player = GameManager.Instance.GameMaster.InstantiateCharacter(spawnPos.position, spawnPos.rotation);
        player.tag = "Player";

        GUI_PlayerActionSelect playerInfo = UIManager.Instance.OpenGUI<GUI_PlayerActionSelect>("PlayerInfor");

        ICharacterController cc = player.GetComponent<ICharacterController>();
        cc.Initial(playerPrefab.name, false);
        cc.SetActionSelector(playerInfo);

        playerInfo.SetPlayer(player);

        EventPlayerSpawn?.Invoke(player);

    }

}

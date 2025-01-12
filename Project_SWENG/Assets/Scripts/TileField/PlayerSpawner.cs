using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using EHTool.UIKit;

public class PlayerSpawner : MonoBehaviour
{

    [Header("SystemManager")]
    [SerializeField] GameObject playerPrefab;
    
    public UnityEvent<GameObject> EventPlayerSpawn;
    
    public void SpawnPlayer(int posIdx)
    {
        MapUnit spawnHex = HexGrid.Instance.Map.PlayerSpawnPos[posIdx];

        Transform spawnPos = spawnHex.transform;

        GameObject player = GameManager.Instance.GameMaster.InstantiateCharacter(spawnPos.position, spawnPos.rotation);
        player.tag = "Player";

        GUI_PlayerActionSelect playerInfo = UIManager.Instance.OpenGUI<GUI_PlayerActionSelect>("PlayerInfor");

        ICharacterController cc = player.GetComponent<ICharacterController>();
        cc.Initial(playerPrefab.name, 0, false);
        cc.SetActionSelector(playerInfo);

        playerInfo.SetPlayer(player);

        EventPlayerSpawn?.Invoke(player);

    }

}

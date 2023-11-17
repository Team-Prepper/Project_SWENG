using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UISystem;
using Character;

public class NetworkPlayerSpawner : MonoBehaviourPun
{

    [Header("Network")]
    private PhotonView _photonView;

    [Header("SpawnPos")]
    [SerializeField] private List<Hex> spawnPosList = new List<Hex>();


    [Header("SystemManager")]

    [SerializeField] GameObject playerPrefab;
    
    public UnityEvent<GameObject> EventPlayerSpawn;

    void Awake()
    {
        _photonView = GetComponent<PhotonView>();
        SpawnPlayerAtNetwork();
    }

    private void SpawnPlayerAtNetwork()
    {
        SpawnPlayer();
    }
    
    
    void SpawnPlayer()
    {
        Debug.Log("Spawing");

        Hex spawnHex = spawnPosList[NetworkManager.PlayerID];

        Transform spawnPos = spawnHex.transform;

        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPos.position, spawnPos.rotation);
        spawnHex.Entity = player;
        player.GetComponent<NetworkUnit>().curHex = spawnHex;
        GUI_PlayerInfor playerInfo = UIManager.OpenGUI<GUI_PlayerInfor>("PlayerInfor");
        playerInfo.SetPlayer(player);
        GameManager.Instance.turnEndButton = playerInfo.turnEndButton;

        EventPlayerSpawn?.Invoke(player);

        GameManager.Instance.player = player;
        GameManager.Instance.gamePhase = GameManager.Phase.Start;

        
        Debug.Log("Spawn End");
    }

}

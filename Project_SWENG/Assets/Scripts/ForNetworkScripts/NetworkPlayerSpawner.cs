using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UISystem;

public class NetworkPlayerSpawner : MonoBehaviourPun
{

    [Header("Network")]
    private PhotonView _PhotonView;

    [Header("SpawnPos")]
    [SerializeField] private List<Hex> spawnPosList = new List<Hex>();


    [Header("SystemManager")]

    [SerializeField] GameObject playerPrefab;
    
    public UnityEvent<GameObject> EventPlayerSpawn;

    void Awake()
    {
        _PhotonView = GetComponent<PhotonView>();
        //NetworkGridMaker.EventConvertMaterials += SpawnPlayerHandler;
        SpawnPlayerAtNetwork();
    }

    private void SpawnPlayerAtNetwork()
    {
        _PhotonView.RPC("SpawnPlayer", RpcTarget.All, null);
    }

    // legacy
    void SpawnPlayerHandler(object sender, EventArgs e)
    {
        _PhotonView.RPC("SpawnPlayer", RpcTarget.All, null);
    }

    [PunRPC]
    void SpawnPlayer()
    {
        Debug.Log("Spawing");

        Hex spawnHex = spawnPosList[NetworkManager.PlayerID];

        Transform spawnPos = spawnHex.transform;

        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPos.position, spawnPos.rotation);
        spawnHex.Entity = player;

        UIManager.OpenGUI<GUI_PlayerInfor>("UnitInfor").SetPlayer(player);

        EventPlayerSpawn?.Invoke(player);

        GameManager.Instance.player = player;
        GameManager.Instance.gamePhase = GameManager.Phase.Start;

        
        Debug.Log("Spawn End");
    }

}

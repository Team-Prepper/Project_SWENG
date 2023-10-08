using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static GameManager;
using Random = UnityEngine.Random;

public class NetworkPlayerSpawner : MonoBehaviourPun
{

    [Header("Network")]
    private PhotonView _PhotonView;


    [Header("SystemManager")]

    [SerializeField] GameObject playerPrefab;
    public UnityEvent<GameObject> EventPlayerSpawn;

    void Awake()
    {
        _PhotonView = GetComponent<PhotonView>();
        NetworkGridMaker.EventSetNavComplete += SpawnPlayer;
    }

    void SpawnPlayer(object sender, EventArgs e)
    {
        Debug.Log("Spawing");

        Hex spawnHex = HexGrid.Instance.GetRandHexAtEmpty();

        Transform spawnPos = spawnHex.transform;
        Debug.Log("SpawnPos : " + spawnPos.position);

        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPos.position, spawnPos.rotation);

        UIManager.OpenGUI<GUI_PlayerInfor>("UnitInfor").SetPlayer(player);
        spawnHex.Entity = player;

        EventPlayerSpawn?.Invoke(player);

        GameManager.Instance.player = player;
        GameManager.Instance.gamePhase = GameManager.Phase.Start;

        NetworkCloudManager.Instance.CloudActiveFalse(spawnHex.HexCoords);
    }
}

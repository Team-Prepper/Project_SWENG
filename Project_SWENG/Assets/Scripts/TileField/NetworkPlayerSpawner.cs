using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Fusion;
using static GameManager;
using Random = UnityEngine.Random;

public class NetworkPlayerSpawner : NetworkBehaviour
{
    [SerializeField] NetworkObject playerPrefab;
    public UnityEvent<GameObject> EventPlayerSpawn;

    void Awake()
    {
        GridMaker.EventSetNavComplete += SpawnPlayer;
    }

    void SpawnPlayer(object sender, EventArgs e)
    {
        Debug.Log("Spawing");
       
        Hex spawnHex = HexGrid.Instance.GetRandHexAtEmpty();

        Transform spawnPos = spawnHex.transform;
        Debug.Log("SpawnPos : " + spawnPos.position);

        NetworkObject player = Runner.Spawn(playerPrefab, spawnPos.position, spawnPos.rotation); // ,spawnPos.position, spawnPos.rotation


        UIManager.OpenGUI<GUI_PlayerInfor>("UnitInfor").SetPlayer(player.gameObject);
        spawnHex.Entity = player.gameObject;

        EventPlayerSpawn?.Invoke(player.gameObject);

        GameManager.Instance.player = player.gameObject;
        GameManager.Instance.gamePhase = GameManager.Phase.Start;

        CloudBox.Instance.CloudActiveFalse(spawnHex.HexCoords);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static GameManager;

public class PlayerSpawner : MonoBehaviour
{
    [Header("SystemManager")]
    GameManager GM;
    [SerializeField] HexGrid hexGrid;
    [SerializeField] GridMaker gridMaker;
    [SerializeField] GameObject playerPrefab;
    public GameObject player;

    public UnityEvent<GameObject> EventPlayerSpawn;

    void Awake()
    {
        GM = GameManager.Instance;
        GridMaker.EventSetNavComplete += SpawnPlayer;
    }

    void SpawnPlayer(object sender, EventArgs e)
    {
        Debug.Log("Spawing");
        Hex.Type tileType;
        Hex spawnHex = hexGrid.GetRandHex();
        tileType = spawnHex.tileType;
        while (tileType != Hex.Type.Field)
        {
            spawnHex = hexGrid.GetRandHex();
            tileType = spawnHex.tileType;
        }
        Transform spawnPos = spawnHex.transform;
        Debug.Log("SpawnPos : " + spawnPos.position);
        player = Instantiate(playerPrefab, spawnPos.position, spawnPos.rotation);
        player.GetComponent<Unit>().CurPos = spawnPos.position;

        EventPlayerSpawn?.Invoke(player);
        GM.player = player;
        GM.gamePhase = GameManager.Phase.Start;
        CloudBox.Instance.CloudActiveFalse(spawnHex.HexCoords);
    }
}

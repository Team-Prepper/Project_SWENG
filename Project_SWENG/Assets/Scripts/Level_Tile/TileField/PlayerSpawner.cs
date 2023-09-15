using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static GameManager;
using Random = UnityEngine.Random;

public class PlayerSpawner : MonoBehaviour
{
    [Header("SystemManager")]

    [SerializeField] GridMaker gridMaker;
    [SerializeField] GameObject playerPrefab;

    public GameObject player;

    public UnityEvent<GameObject> EventPlayerSpawn;

    public delegate void GameObjectEventUnit(GameObject obj);
    public static GameObjectEventUnit eventUnit;

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
        player = Instantiate(playerPrefab, spawnPos.position, spawnPos.rotation);
        player.GetComponent<Unit>().CurPos = spawnPos.position;
        UIManager.OpenGUI<GUI_PlayerInfor>("UnitInfor").SetPlayer(player);
        spawnHex.Entity = player;

        EventPlayerSpawn?.Invoke(player);

        GameManager.Instance.player = player;
        GameManager.Instance.gamePhase = GameManager.Phase.Start;

        CloudBox.Instance.CloudActiveFalse(spawnHex.HexCoords);
    }
}

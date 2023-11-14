using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UISystem;

public class PlayerSpawner : MonoBehaviour
{
    [Header("SystemManager")]

    [SerializeField] GameObject playerPrefab;
    public UnityEvent<GameObject> EventPlayerSpawn;

    void Awake()
    {

    }



    public void SpawnPlayer(Hex spawnHex)
    {
        Debug.Log("Spawing");
        if (spawnHex == null) {
            Debug.Log("Spawn Hex is Null");
            return;
        }
       
        Transform spawnPos = spawnHex.transform;
        Debug.Log("SpawnPos : " + spawnPos.position);

        GameObject player = Instantiate(playerPrefab, spawnPos.position, spawnPos.rotation);

        UIManager.OpenGUI<GUI_PlayerInfor>("PlayerInfor").SetPlayer(player);
        spawnHex.Entity = player;

        EventPlayerSpawn?.Invoke(player);

        GameManager.Instance.player = player;
        GameManager.Instance.gamePhase = GameManager.Phase.Start;

        CloudBox.Instance.CloudActiveFalse(spawnHex.HexCoords);
    }
}

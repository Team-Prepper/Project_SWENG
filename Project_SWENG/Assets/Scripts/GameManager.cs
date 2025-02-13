using EHTool;
using System;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager> {

    public INetwork Network { get; private set; }
    public IGameMaster GameMaster { get; private set; }
    public IGameSetting GameSetting { get; internal set; }

    GameObject _gameMasterObject;

    public void SetGameMaster<T>() where T : Component, IGameMaster
    {
        if (_gameMasterObject != null) {
            Destroy(_gameMasterObject);
        }

        _gameMasterObject = new GameObject("GameMaster");
        _gameMasterObject.transform.SetParent(transform);

        GameMaster = _gameMasterObject.AddComponent<T>();
    }

    protected override void OnCreate()
    {
        base.OnCreate();

        SetGameMaster<LocalGameMaster>();

        Network = gameObject.AddComponent<PhotonNet>();
    }

    internal string[] GetEnemyList(string bossEnemyPrefabKey)
    {
        throw new NotImplementedException();
    }
}
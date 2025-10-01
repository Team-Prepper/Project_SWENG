using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using EasyH.Unity;
using SWEng;

public class GameManager : MonoSingleton<GameManager>
{
    public IGameMaster Master { get; set; }
    public IGameSetting Setting { get; set; }

    private Component _gameMasterComponent;

    private bool _actionRegistered = false;
    private Action _actions;

    public void SetGameMaster<T>() where T : Component, IGameMaster
    {
        if (_gameMasterComponent != null) {
            Destroy(_gameMasterComponent);
        }

        T temp = gameObject.AddComponent<T>();
        
        _gameMasterComponent = temp;
        Master = temp;

    }

    public void AddSceneLoadEvent(Action action) {
        
        if (!_actionRegistered) {
            _actionRegistered = true;
            SceneManager.sceneLoaded += (t, v) => {
                StartCoroutine(WaitAFrame());
            };

        }
        _actions += action;
    }

    IEnumerator WaitAFrame() {
        yield return null;
        _actions?.Invoke();
    }

    public void RemoveSceneLoadEvent(Action action) {
        _actions -= action;
    }

    protected override void OnCreate()
    {
        base.OnCreate();

        SetGameMaster<GameMaster>();

        Setting = new GameSetting();

    }

}
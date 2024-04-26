using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UISystem;
using UnityEngine.UI;
using TMPro;
using CharacterSystem;

public class GameManager : MonoSingleton<GameManager> {
    
    public IGameMaster GameMaster { get; private set; }

    public void SetGameMaster(IGameMaster gm) { GameMaster = gm; }

}
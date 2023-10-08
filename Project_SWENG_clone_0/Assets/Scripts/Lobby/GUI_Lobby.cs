using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GUI_Lobby : MonoSingleton<GUI_Lobby>
{
    public TMP_Text roomNameText;
    public LobbyManager lobbyManager;

    public void InitPregame(Fusion.NetworkRunner runner)
    {
        roomNameText.gameObject.SetActive(true);
        SetRoomText(runner.SessionInfo.Name);
    }

    private void SetRoomText(string roomText)
    {
        roomNameText.text = roomText;
    }
    
    public void StartGame()
    {
        lobbyManager.Server_StartGame();
    }
}

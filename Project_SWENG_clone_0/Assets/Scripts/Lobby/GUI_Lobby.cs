using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GUI_Lobby : MonoSingleton<GUI_Lobby>
{
    public TMP_Text roomNameText;

    public void InitPregame(Fusion.NetworkRunner runner)
    {
        SetRoomText(runner.SessionInfo.Name);
        roomNameText.gameObject.SetActive(true);
    }

    public void SetRoomText(string roomText)
    {
        roomNameText.text = roomText;
    }
}

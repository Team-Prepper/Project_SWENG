using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using UISystem;
using UnityEditor.XR;
using System.Runtime.InteropServices;


public partial class NetworkManager
{

    public GUI_Chat _chatting;

    public void Block(string name)
    {
        if (_blockUser.Contains(name)) return;
        _blockUser.Add(name);
    }

    List<string> _blockUser = new List<string>();

    #region chat
    public void Send(string msg)
    {
        PV.RPC("ChatRPC", RpcTarget.All, PhotonNetwork.NickName, msg);
    }

    [PunRPC]
    void ChatRPC(string sender, string msg)
    {
        if (!_chatting) return;
        if (_blockUser.Contains(sender)) return;

        _chatting.Chat(sender, msg);
    }
    #endregion
}

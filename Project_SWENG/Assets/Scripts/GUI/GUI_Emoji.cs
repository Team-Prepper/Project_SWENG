using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using UnityEngine.Windows;

public class GUI_Emoji : MonoBehaviour {

    NetworkManager _network;

    [SerializeField] GUI_Network_Room_PlayerState[] _players;
    [SerializeField] SpriteAtlas _atlas;

    private void Start()
    {
        _network = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();

        _network._emoji = this;
    }

    public void EmojiSend(string emojiKey)
    {
        _network.SendEmoji(emojiKey);

    }

    public void RecieveEmoji(string sender, string emojiKey)
    {
        Player[] memberList = _network.RoomMemberList();

        for (int i = 0; i < _players.Length; i++) {
            if (!memberList[i].NickName.Equals(sender)) continue;
            _players[i].DisplayEmoji(_atlas.GetSprite(emojiKey));
            return;
        }
    }

}

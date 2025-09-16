using System;
using UnityEngine;
using UnityEngine.U2D;
using SWEng.Chat;

public class GUIEmojiBox : MonoBehaviour {

    [SerializeField] GUIUnitRoomPlayerState[] _players;
    [SerializeField] SpriteAtlas _atlas;


    public void ReceiveEmoji(string sender, string emojiKey)
    {
        /*
        //Player[] memberList = _network.RoomMemberList();

        for (int i = 0; i < _players.Length; i++)
        {
            //if (!memberList[i].NickName.Equals(value.sender)) continue;
            _players[i].DisplayEmoji(_atlas.GetSprite(value.emojiKey));
            //return;
        }
        */
    }

    private void OnEnable()
    {
        ChatManager.Instance.System.OnReceiveChat
            += ReceiveEmoji;
    }

    private void OnDisable()
    {
        ChatManager.Instance.System.OnReceiveChat
            -= ReceiveEmoji;
    }
    private void OnDestroy()
    {
        ChatManager.Instance.System.OnReceiveChat
            -= ReceiveEmoji;
    }

    public void EmojiSend(string emojiKey)
    {
        ChatManager.Instance.System.SendEmoji(emojiKey);

    }
}

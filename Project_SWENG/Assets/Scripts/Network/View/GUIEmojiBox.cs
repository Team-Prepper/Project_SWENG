using System;
using UnityEngine;
using UnityEngine.U2D;

public class GUIEmojiBox : MonoBehaviour, IObserver<EmojiData>{

    [SerializeField] GUIUnitRoomPlayerState[] _players;
    [SerializeField] SpriteAtlas _atlas;

#nullable enable
    private IDisposable? _cancellation;

    public void OnCompleted()
    {

    }

    public void OnError(Exception error)
    {

    }

    public void OnNext(EmojiData value)
    {
        //Player[] memberList = _network.RoomMemberList();

        for (int i = 0; i < _players.Length; i++)
        {
            //if (!memberList[i].NickName.Equals(value.sender)) continue;
            _players[i].DisplayEmoji(_atlas.GetSprite(value.emojiKey));
            //return;
        }
    }

    private void OnEnable()
    {
        _cancellation =
            GameManager.Instance.Network.Chat.Subscribe(this);
    }

    private void OnDisable()
    {
        _cancellation?.Dispose();
    }
    private void OnDestroy()
    {
        _cancellation?.Dispose();
    }

    public void EmojiSend(string emojiKey)
    {
        GameManager.Instance.Network.Chat.SendEmoji(emojiKey);

    }
}

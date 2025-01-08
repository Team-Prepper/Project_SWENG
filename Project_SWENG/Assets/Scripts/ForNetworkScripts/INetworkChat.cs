using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct ChatData {
    public string sender;
    public string msg;

    public ChatData(string s, string m)
    {
        sender = s;
        msg = m;
    }
}

public struct EmojiData {
    public string sender;
    public string emojiKey;

    public EmojiData(string s, string e) {
        sender = s;
        emojiKey = e;
    }
}

public interface INetworkChat : IObservable<ChatData>, IObservable<EmojiData> {

    public void SendSystemMsg(string msg);

    public void SendChat(string msg);

    public void SendEmoji(string emojiKey);

    public void Block(string name);

}

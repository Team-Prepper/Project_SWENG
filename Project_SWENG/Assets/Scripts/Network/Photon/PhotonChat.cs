using EHTool;
using EHTool.LangKit;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PhotonChat : MonoBehaviour, INetworkChat {

    ISet<IObserver<ChatData>> _chatObservers;
    ISet<IObserver<EmojiData>> _emojiObservers;

    List<string> _blockUser = new List<string>();

    public PhotonView _pv;

    void Awake() {

        if (!TryGetComponent(out _pv)) { 
            _pv = gameObject.AddComponent<PhotonView>();
        }

        _chatObservers = new HashSet<IObserver<ChatData>>();
        _emojiObservers = new HashSet<IObserver<EmojiData>>();
    }

    public PhotonChat(PhotonView view) {
        _pv = view;
    }

    public IDisposable Subscribe(IObserver<ChatData> observer)
    {
        if (!_chatObservers.Contains(observer))
        {
            _chatObservers.Add(observer);
        }

        return new Unsubscriber<ChatData>(_chatObservers, observer);
    }

    public IDisposable Subscribe(IObserver<EmojiData> observer)
    {
        if (!_emojiObservers.Contains(observer))
        {
            _emojiObservers.Add(observer);
        }

        return new Unsubscriber<EmojiData>(_emojiObservers, observer);
    }

    public void Block(string name)
    {
        if (_blockUser.Contains(name)) return;
        _blockUser.Add(name);
    }

    public void SendSystemMsg(string msg) {

        _pv.RPC("ChatRPC", RpcTarget.All, "System", msg);
    }

    public void SendChat(string msg)
    {
        _pv.RPC("ChatRPC", RpcTarget.All, PhotonNetwork.NickName, msg);

    }
    public void SendEmoji(string emojiKey)
    {
        _pv.RPC("EmojiRPC", RpcTarget.All, PhotonNetwork.NickName, emojiKey);

    }

    [PunRPC]
    void ChatRPC(string sender, string msg)
    {
        //if (_blockUser.Contains(sender)) return;

        ChatData data = new ChatData(sender, msg);

        foreach (IObserver<ChatData> target in _chatObservers)
        {
            target.OnNext(data);
        }
    }

    [PunRPC]
    void EmojiRPC(string sender, string msg)
    {
        //if (_blockUser.Contains(sender)) return;

        EmojiData data = new EmojiData(sender, msg);

        foreach (IObserver<EmojiData> target in _emojiObservers)
        {
            target.OnNext(data);
        }
    }
}
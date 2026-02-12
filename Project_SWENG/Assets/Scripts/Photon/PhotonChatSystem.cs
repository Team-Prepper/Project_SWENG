using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;
using SWEng.Chat;


namespace MultiPlay.Photon.Chat
{
    
    public class PhotonChatSystem : MonoBehaviour, IChatSystem
    {

        private PhotonView _pv;

        private ISet<string> _blockUser = new HashSet<string>();

        public Action<string, string> OnReceiveChat { get; set; }
        public Action<string, string> OnReceiveEmoji { get; set; }

        void Awake()
        {
            if (!TryGetComponent(out _pv))
            {
                _pv = gameObject.AddComponent<PhotonView>();
            }

            ChatManager.Instance.System = this;
        }

        public void SendSystemMsg(string msg)
        {
            _pv.RPC(nameof(ChatRPC), RpcTarget.All, "System", msg);
        }

        public void SendChat(string msg)
        {
            _pv.RPC(nameof(ChatRPC), RpcTarget.All,
                PhotonNetwork.NickName, msg);
        }

        [PunRPC]
        private void ChatRPC(string sender, string msg)
        {
            if (_blockUser.Contains(sender)) return;
            OnReceiveChat?.Invoke(sender, msg);
        }

        public void SendEmoji(string emojiKey)
        {
            _pv.RPC(nameof(ChatRPC), RpcTarget.Others,
                PhotonNetwork.NickName, emojiKey);
        }

        [PunRPC]
        private void EmojiRPC(string sender, string msg)
        {
            if (_blockUser.Contains(sender)) return;
            OnReceiveEmoji?.Invoke(sender, msg);
        }

        public void Block(string name)
        {
            _blockUser.Add(name);
        }

        public void Clear()
        {
            OnReceiveChat = null;
            OnReceiveEmoji = null;
        }
    }
}
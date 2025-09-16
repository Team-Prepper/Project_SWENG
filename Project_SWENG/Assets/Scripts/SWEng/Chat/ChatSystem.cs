using System;
using System.Collections.Generic;

namespace SWEng.Chat
{
    public class ChatSystem : IChatSystem
    {
        private ISet<string> _blockUser = new HashSet<string>();

        public Action<string, string> OnReceiveChat { get; set; }
        public Action<string, string> OnReceiveEmoji { get; set; }

        public void SendSystemMsg(string msg)
        {
            OnReceiveChat?.Invoke("System", msg);
        }

        public void SendChat(string msg)
        {
            OnReceiveChat?.Invoke("", msg);
        }

        public void SendEmoji(string emojiKey)
        {
            OnReceiveEmoji?.Invoke("", emojiKey);
        }

        public void Block(string name)
        {
            _blockUser.Add(name);
        }

    }
}
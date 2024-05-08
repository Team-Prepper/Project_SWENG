using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface INetworkChat {
    public void ChatSend();

    public void Chat(string sender, string msg);

}

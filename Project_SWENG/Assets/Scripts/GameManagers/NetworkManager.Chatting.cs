using System.Collections.Generic;
using Photon.Pun;


public partial class NetworkManager {

    public GUI_Chat _chatting;
    public GUI_Emoji _emoji;

    public void Block(string name)
    {
        if (_blockUser.Contains(name)) return;
        _blockUser.Add(name);
    }

    List<string> _blockUser = new List<string>();

    public void SendChat(string msg)
    {
        PV.RPC("ChatRPC", RpcTarget.All, PhotonNetwork.NickName, msg);
    }

    public void SendEmoji(string emojiKey)
    {
        PV.RPC("EmojiRPC", RpcTarget.All, PhotonNetwork.NickName, emojiKey);

    }


    [PunRPC]
    void ChatRPC(string sender, string msg)
    {
        if (!_chatting) return;
        if (_blockUser.Contains(sender)) return;

        _chatting.Chat(sender, msg);
    }

    [PunRPC]
    void EmojiRPC(string sender, string msg)
    {
        if (!_emoji) return;
        if (_blockUser.Contains(sender)) return;

        _emoji.RecieveEmoji(sender, msg);
    } 

}

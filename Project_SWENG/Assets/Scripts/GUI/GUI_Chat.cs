using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Chat : MonoBehaviour {

    NetworkManager _network;

    public TMP_Text[] ChatText;
    public TMP_InputField ChatInput;

    public Scrollbar _scrollBar;

    int _useCount = 0;

    private void Start()
    {
        _network = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();

        _network._chatting = this;
    }

    public void ChatSend()
    {
        _network.Send(ChatInput.text);
        ChatInput.text = "";
    }

    public void Chat(string msg)
    {
        if (_useCount < ChatText.Length)
        {
            ChatText[_useCount++].text = msg;
            _scrollBar.value = 0;
            if (_useCount < ChatText.Length)
            {
                ChatText[_useCount].text = " ";
            }
            return;
        }

        for (int i = 1; i < ChatText.Length; i++) ChatText[i - 1].text = ChatText[i].text;
        ChatText[ChatText.Length - 1].text = msg;
        _scrollBar.value = 0;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class GUI_Chat : MonoBehaviour {

    NetworkManager _network;

    public TMP_Text[] ChatText;
    public InputField ChatInput;

    public Scrollbar _scrollBar;

    int _useCount = 0;

    private void Start()
    {
        _network = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();

        _network._chatting = this;
    }

    public void ChatSend()
    {
        string input = ChatInput.text;
        ChatInput.text = null;

        if (input[0] != '/') {
            _network.SendChat(input);
            return;
        }

        _CommandExecute(input.Substring(1, input.IndexOf(' ') - 1), input.Substring(input.IndexOf(' ') + 1));
        
    }

    private void _CommandExecute(string command, string input)
    {
        switch (command)
        {
            case "block":
                _network.Block(input);
                return;
            default:
                return;
        }
    }

    public void Chat(string sender, string msg) {

        if (sender.Equals("System"))
        {
            Chat(string.Format("<color=yellow>{0}</color>", msg));
            return;
        }

        Chat(sender + " : " + msg);
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

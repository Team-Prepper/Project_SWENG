using System;
using UnityEngine;
using UnityEngine.UI;
using SWEng.Chat;

public class GUIChattingBox : MonoBehaviour {

    public Text[] ChatText;
    public InputField ChatInput;

    public Scrollbar _scrollBar;

    int _useCount = 0;

    private void OnEnable()
    {
        ChatManager.Instance.System.OnReceiveChat
            += ReceiveChat;
    }

    private void OnDisable()
    {
        ChatManager.Instance.System.OnReceiveChat
            -= ReceiveChat;
    }

    private void OnDestroy()
    {
        ChatManager.Instance.System.OnReceiveChat
            -= ReceiveChat;
    }

    public void ChatSend()
    {
        string input = ChatInput.text;

        if (input.Length == 0) return;
        
        ChatInput.text = "";

        if (input[0] != '/') {
            ChatManager.Instance.System.SendChat(input);
            return;
        }

        _CommandExecute(input.Substring(1, input.IndexOf(' ') - 1),
            input.Substring(input.IndexOf(' ') + 1));
        
    }

    public void ReceiveChat(string sender, string msg)
    {
        if (sender.Equals("System"))
        {
            _Chat(string.Format("<color=yellow>{0}</color>", msg));
            return;
        }

        _Chat(sender + " : " + msg);
    }

    private void _CommandExecute(string command, string input)
    {
        switch (command)
        {
            case "block":
                ChatManager.Instance.System.Block(input);
                return;
            default:
                return;
        }
    }

    private void _Chat(string msg)
    {
        if (_useCount < ChatText.Length)
        {
            ChatText[_useCount].gameObject.SetActive(true);
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

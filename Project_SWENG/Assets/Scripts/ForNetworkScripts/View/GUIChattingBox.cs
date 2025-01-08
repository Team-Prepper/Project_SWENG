using System;
using UnityEngine;
using UnityEngine.UI;

public class GUIChattingBox : MonoBehaviour, IObserver<ChatData> {

    public Text[] ChatText;
    public InputField ChatInput;

    public Scrollbar _scrollBar;

    int _useCount = 0;

#nullable enable
    private IDisposable? _cancellation;

    public void OnCompleted()
    {

    }

    public void OnError(Exception error)
    {

    }

    public void OnNext(ChatData value)
    {
        if (value.sender.Equals("System"))
        {
            _Chat(string.Format("<color=yellow>{0}</color>", value.msg));
            return;
        }

        _Chat(value.sender + " : " + value.msg);
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

    public void ChatSend()
    {
        string input = ChatInput.text;
        if (input.Length == 0) return;
        ChatInput.text = "";

        if (input[0] != '/') {
            GameManager.Instance.Network.Chat.SendChat(input);
            return;
        }

        _CommandExecute(input.Substring(1, input.IndexOf(' ') - 1),
            input.Substring(input.IndexOf(' ') + 1));
        
    }

    private void _CommandExecute(string command, string input)
    {
        switch (command)
        {
            case "block":
                GameManager.Instance.Network.Chat.Block(input);
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

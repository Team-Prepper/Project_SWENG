using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using UISystem;
using UnityEditor.XR;
using System.Runtime.InteropServices;


public partial class NetworkManager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "3";

    string _nickName;

    public TMP_Text StatusText;
    public PhotonView PV;
    
    public static int PlayerID { get; private set; }

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = gameVersion;       
        Debug.Log(PhotonNetwork.SendRate);
    }


    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        StatusText.text = PhotonNetwork.NetworkClientState.ToString();
    }

    public void Connect(string nickName) { PhotonNetwork.ConnectUsingSettings(); _nickName = nickName; }

    public override void OnConnectedToMaster() => PhotonNetwork.JoinLobby();

}

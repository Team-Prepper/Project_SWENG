using UnityEngine;
using Photon.Pun;
using TMPro;


public partial class NetworkManager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "3";

    public int baseDiceValue = 0;
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

    public void SetBaseDicePointHandler(int value)
    {
        PV.RPC("SetBaseDicePoint",RpcTarget.All, value);
    }

    [PunRPC]
    private void SetBaseDicePoint(int value)
    {
        baseDiceValue = value;
    }

    public void Connect(string nickName) { PhotonNetwork.ConnectUsingSettings(); _nickName = nickName; }

    public override void OnConnectedToMaster() => PhotonNetwork.JoinLobby();

}

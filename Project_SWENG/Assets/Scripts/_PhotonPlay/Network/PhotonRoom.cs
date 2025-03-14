using Photon.Pun;

public class PhotonRoom : MonoBehaviourPun, INetworkRoom {

    private PhotonView _view;

    public void Awake()
    {
        _view = GetComponent<PhotonView>();

    }

    private int FindIdxByName(string name) {
        IGameSetting target = GameManager.Instance.GameSetting;
        for (int i = 0; i < target.Players.Count; i++)
        {
            if (name.Equals(target.Players[i].Name)) return i;
        }

        return 0;
    }

    public void NewPlayerEnter(RoomMember newMember)
    {
        if (!_view.IsMine) return;

        _view.RPC("PlayerOnRoom", RpcTarget.MasterClient, newMember.NickName);

    }

    [PunRPC]
    private void PlayerOnRoom(string name) {

        GameManager.Instance.Network.Chat.SendSystemMsg(
            string.Format("{0} Enter", name));
        GameManager.Instance.GameSetting.AddPlayer(name, "Human");

    }

    public void PlayerExit(RoomMember exitMember)
    {
        if (!_view.IsMine) return;
        _view.RPC("PlayerOffRoom", RpcTarget.MasterClient, exitMember.NickName);
    }

    [PunRPC]
    private void PlayerOffRoom(string name)
    {

        GameManager.Instance.Network.Chat.SendSystemMsg(
            string.Format("{0} Exit", name));
        GameManager.Instance.GameSetting.RemovePlayer(FindIdxByName(name));

    }

}
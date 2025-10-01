using Photon.Pun;
using SWEng.Chat;
using SWEng.Network;
using SWEng;
using MultiPlay.Photon.Chat;
using MultiPlay.Photon.SWEng;

namespace MultiPlay.Photon.Network
{
    [UnityEngine.RequireComponent(typeof(PhotonView))]
    public class PhotonRoom : MonoBehaviourPun, INetworkRoom
    {

        private PhotonView _view;
        
        private PhotonChatSystem _chat;
        private PhotonGameSetting _gameSettingView;

        public void Awake()
        {
            _view = GetComponent<PhotonView>();
            _chat = gameObject.AddComponent<PhotonChatSystem>();
            
        }

        public void EnterRoom()
        { 
            _gameSettingView = gameObject.
                AddComponent<PhotonGameSetting>();

            GameManager.Instance.SetGameMaster<PhotonGameMaster>();
            GameManager.Instance.Setting = _gameSettingView;
            ChatManager.Instance.System = _chat;
            
        }

        public void LeaveRoom()
        {
            GameManager.Instance.SetGameMaster<GameMaster>();
            GameManager.Instance.Setting = new GameSetting();
            ChatManager.Instance.System = new ChatSystem();
            
            Destroy(_gameSettingView);
            _view.FindObservables(true);
        }

        private int FindIdxByName(string name)
        {
            IGameSetting target = GameManager.Instance.Setting;
            for (int i = 0; i < target.Players.Count; i++)
            {
                if (name.Equals(target.Players[i].Name)) return i;
            }

            return 0;

        }

        public void NewPlayerEnter(RoomMember newMember)
        {
            if (!_view.IsMine) return;

            _view.RPC(nameof(PlayerOnRoom),
                RpcTarget.MasterClient, newMember.NickName);

        }

        [PunRPC]
        private void PlayerOnRoom(string name)
        {
            ChatManager.Instance.System.SendSystemMsg(
                string.Format("{0} Enter", name));
            GameManager.Instance.Setting.AddPlayer(name, "Human");

        }

        public void PlayerExit(RoomMember exitMember)
        {
            if (!_view.IsMine) return;
            _view.RPC(nameof(PlayerOffRoom),
                RpcTarget.MasterClient, exitMember.NickName);
        }

        [PunRPC]
        private void PlayerOffRoom(string name)
        {
            ChatManager.Instance.System.SendSystemMsg(
                string.Format("{0} Exit", name));

            GameManager.Instance.Setting.RemovePlayer(FindIdxByName(name));

        }

    }
}
using UnityEngine;
using Photon.Pun;
using SWEng.GamePlay;

namespace SWEng.MultiPlay.Photon
{
    public class PhotonItemController : ItemController
    {

        [SerializeField] private PhotonView _view;

        public override void SetInitial(string itemCode)
        {
            _view.RPC("BaseSetInitial", RpcTarget.All, itemCode);
        }

        [PunRPC]
        private void BaseSetInitial(string itemCode)
        {
            base.SetInitial(itemCode);
        }

        public override void Equip()
        {
            _view.RPC("BaseEquip", RpcTarget.All);
        }

        [PunRPC]
        void BaseEquip()
        {
            base.Equip();
        }

    }
}
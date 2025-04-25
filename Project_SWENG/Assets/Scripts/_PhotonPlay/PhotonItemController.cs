using UnityEngine;
using Photon.Pun;

public class PhotonItemController : LocalItemController {

    [SerializeField] PhotonView _view;

    public override void SetInitial(string itemCode)
    {
        _view.RPC("BaseSetInitial", RpcTarget.All, itemCode);

    }

    [PunRPC]
    private void BaseSetInitial(string itemCode) {
        base.SetInitial(itemCode);
    }

    public override void Equip()
    {
        _view.RPC("BaseEquip", RpcTarget.All);
    }

    [PunRPC]
    void BaseEquip() {
        base.Equip();
    }

}

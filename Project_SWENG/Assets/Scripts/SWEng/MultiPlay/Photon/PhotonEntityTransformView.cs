using SWEng.Data;
using SWEng.GamePlay;
using UnityEngine;
using Photon.Pun;

namespace SWEng.MultiPlay.Photon
{
    [RequireComponent(typeof(EntityTransform))]
    [RequireComponent(typeof(PhotonView))]
    public class PhotonEntityTransformView : MonoBehaviour
    {
        private EntityTransform _target;
        private PhotonView _view;

        public void Start()
        {
            _target = GetComponent<EntityTransform>();
            _target.OnPosChangedEvent += OnPosChanged;

            _view = GetComponent<PhotonView>();
        }

        private void OnPosChanged(
            GridCoord2D before, GridCoord2D after)
        {
            _view.RPC(nameof(PunOnPosChanged),
                RpcTarget.Others, after.x, after.y);
        }

        [PunRPC]
        private void PunOnPosChanged(int aX, int aY)
        {
            _target.Pos = new GridCoord2D(aX, aY);
        }
        
    }

}
using SWEng.GamePlay;
using Photon.Pun;
using UnityEngine;

namespace SWEng.MultiPlay.Photon
{
    [RequireComponent(typeof(PhotonView))]
    public class PhotonCharacterAnimation :
        MonoBehaviourPun, ICharacterAnimation
    {
        [SerializeField] private PhotonView _pv;
        private ICharacter _target;

        private void Start()
        {
            _pv = _pv != null ? _pv : GetComponent<PhotonView>();
        }


        public void SetCharacter(ICharacter character)
        {
            _target = character;
        }
        
        public void PlayAnim(string triggerType, string triggerValue)
        {
            _pv.RPC(nameof(PunPlayAnim), RpcTarget.All,
                triggerType, triggerValue);
        }

        [PunRPC]
        public void PunPlayAnim(string triggerType, string triggerValue)
        {
            _target.Actor.PlayAnim(triggerType, triggerValue);
        }

    }
}
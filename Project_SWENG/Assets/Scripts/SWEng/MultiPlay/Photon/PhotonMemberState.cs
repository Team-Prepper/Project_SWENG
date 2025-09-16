using System;
using EasyH.Gaming.TurnBased;
using Photon.Pun;
using UnityEngine;

namespace SWEng.MultiPlay.Photon
{
    [RequireComponent(typeof(PhotonView))]
    public class PhotonMemberState : MonoBehaviourPun, IMemberState
    {
        public bool TurnEnd { get; private set; }

        public int TeamIdx { get; private set; }

        public Action<bool> OnTurnEndStateChanged { get; set; }

        [SerializeField] private PhotonView _pv;

        private void Start()
        { 
            _pv = _pv != null ? _pv : GetComponent<PhotonView>();
        }

        public void SetTeamIdx(int idx)
        {
            _pv.RPC(nameof(PunSetTeamIdx), RpcTarget.All, idx);
        }

        public void PunSetTeamIdx(int idx)
        {
            if (idx == TeamIdx) return;

            if (TeamIdx > 0)
            {
                TurnManager.Instance.System.RemoveTeamMember(this);
            }

            TeamIdx = idx;

            TurnManager.Instance.System.AddTeamMember(this);
        }

        public void Remove()
        {
            _pv.RPC(nameof(PunRemove), RpcTarget.All);
        }

        [PunRPC]
        public void PunRemove()
        { 
            TurnManager.Instance.System.RemoveTeamMember(this);
        }

        public void StartTurn()
        {
            _pv.RPC(nameof(PunStartTurn), RpcTarget.All);
        }

        [PunRPC]
        public void PunStartTurn()
        {
            TurnEnd = false;
            OnTurnEndStateChanged?.Invoke(false);
        }

        public void EndTurn()
        {
            _pv.RPC(nameof(PunEndTurn), RpcTarget.All);

        }

        [PunRPC]
        public void PunEndTurn()
        {
            TurnEnd = true;
            TurnManager.Instance.System.TurnEnd();
        }
    }
}
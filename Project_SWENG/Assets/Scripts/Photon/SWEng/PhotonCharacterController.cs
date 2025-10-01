using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using SWEng;
using CameraSystem;
using BKTools.Gaming.Dice;
using EasyH.Gaming.TurnBased;
using EasyH.Gaming.Inventory;

/*
namespace SWEng.MultiPlay.Photon
{
    public class PhotonCharacterController :
        MonoBehaviour, ICharacterLogic
    {

        [SerializeField] private PhotonStatusView _status;
        [SerializeField] private Inventory _inventory;
        [SerializeField] private PhotonView _view;

        public int TeamIdx { get; private set; }

        public GridCoord2D Pos =>
            CoordManager.Instance.Convertor
                .ConvertFromVector3(transform.position);

        public Character Character { get; private set; }
        public IStatus Status => _status;
        public Inventory Inventory => _inventory;
        public IDicePoint DicePoint { get; private set; }
        public IMemberState TurnMemberState { get; private set; }
        public bool IsRollDice { get; set; }

        private IActionSelector _actionSelector;
        private bool _camSync;

        public void Initial(string characterName, int teamIdx, bool camSync)
        {
            _view.RPC(nameof(PunAllInitial), RpcTarget.All,
                characterName, teamIdx, camSync);
        }

        [PunRPC]
        private void PunAllInitial(string characterName, int teamIdx, bool camSync)
        {
            CoordManager.Instance.GetMapUnitAt(Pos).
                SetCC(gameObject, this);

            Character = Instantiate(CharacterManager.Instance.
                GetCharacterData(characterName).CharacterPrefab).GetComponent<Character>();

            Character.SetCC(this);

            Status.SetCC(this);
            Status.CharacterCode = characterName;

            DicePoint = new DicePoint();

            TeamIdx = teamIdx;
            _camSync = camSync;

            if (!PhotonNetwork.IsMasterClient) return;

            TurnMemberState.SetTeamIdx(TeamIdx);

            Character.AddRemoveAction(() =>
            {
                TurnMemberState.Remove();
                PhotonNetwork.Destroy(gameObject);
            });

        }

        public void Remove()
        {
            _view.RPC(nameof(PunMasterRemove), RpcTarget.MasterClient);
        }

        [PunRPC]
        public void PunMasterRemove()
        {
            CoordManager.Instance.GetMapUnitAt(Pos).ResetEntityState();

            Character.Die();
        }

        public void CamSetting(string key)
        {
            if (!_camSync)
            {
                CameraManager.Instance.CameraSetting(transform, key);
                return;
            }

            _view.RPC(nameof(PunAllCamSetting), RpcTarget.All, key);
        }

        [PunRPC]
        private void PunAllCamSetting(string key)
        {
            CameraManager.Instance.CameraSetting(transform, key);
        }

        public void PlayAnim(string triggerType, string triggerValue)
        {
            _view.RPC(nameof(PunAllPlayAnim), RpcTarget.All,
                triggerType, triggerValue);
        }

        [PunRPC]
        public void PunAllPlayAnim(string type, string value)
        {
            Character.PlayAnim(type, value);
        }

        public void TakeDamage(int amount)
        {
            Status.TakeDamage(amount);
        }

        public void SetPlay(bool turnEnd)
        {
            if (turnEnd) return;
            if (_actionSelector == null) return;

            IsRollDice = false;

            ActionEnd();
        }

        public void SetActionSelector(IActionSelector actionSelector)
        {
            _actionSelector = actionSelector;
        }

        public void ActionEnd(float time = 0)
        {
            if (_actionSelector == null) return;

            Invoke(nameof(_ActionEnd), time);
        }

        public void _ActionEnd()
        {
            IList<IActionSelector.Action> list
                = Character.GetActionList();

            if (IsRollDice == false)
                list.Add(IActionSelector.Action.Dice);

            _actionSelector.Ready(this, list);
        }

        public void MoveTo(GridCoord2D before, GridCoord2D after)
        {
            _view.RPC(nameof(PunAllMoveTo), RpcTarget.All,
                before.x, before.y, after.x, after.y);
        }

        [PunRPC]
        private void PunAllMoveTo(int bX, int bZ, int aX, int aZ)
        {
            CoordManager.Instance.GetMapUnitAt
                (new GridCoord2D(bX, bZ)).ResetEntityState();
            CoordManager.Instance.GetMapUnitAt
                (new GridCoord2D(aX, aZ)).SetCC(gameObject, this);
        }

        public void Move(Queue<Vector3> path)
        {
            Character.Move(path, MoveTo);
        }
    }
}*/
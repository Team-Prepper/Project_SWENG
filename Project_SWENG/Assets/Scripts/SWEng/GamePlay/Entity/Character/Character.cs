using System.Collections.Generic;
using UnityEngine;
using SWEng.Data;
using BKTools.Gaming.Dice;
using CameraSystem;
using EasyH.Gaming.TurnBased;
using EasyH.Gaming.Inventory;


namespace SWEng.GamePlay
{

    [RequireComponent(typeof(IMemberState))]
    [RequireComponent(typeof(ICharacterStat))]
    [RequireComponent(typeof(IStatus))]
    [RequireComponent(typeof(EntityTransform))]
    [RequireComponent(typeof(CharacterMoveBase))]
    [RequireComponent(typeof(CharacterAttack))]
    [RequireComponent(typeof(ICameraController))]
    [RequireComponent(typeof(ICharacterAnimation))]
    public class Character : MonoBehaviour, ICharacter
    {

        public CharacterActor Actor { get; set; }

        public IStatus Status { get; private set; }
        public ICharacterStat Stat { get; private set; }
        public ICharacterAnimation Animation { get; private set; }

        public EntityTransform EntityTransform { get; set; }
        public Inventory Inventory { get; private set; }
        public IDicePoint DicePoint { get; private set; }
        public IMemberState TurnMemberState { get; set; }
        
        public ICameraController CameraController
            { get; private set; }

        public bool IsRollDice { get; set; }

        private ICharacterController _actionSelector;

        private CharacterMoveBase _moveComp;
        private CharacterAttack _attackComp;

        [SerializeField] CharacterInteractionBase _interactionEvent;

        void Awake()
        {
            DicePoint = new DicePoint();

            Status = GetComponent<IStatus>();

            Status.OnDamageEvent += TakeDamageEvent;
            Status.OnDeathEvent += () =>
            {
                CharacterManager.Instance.
                    SetCharacterAt(EntityTransform.Pos, null);

                DeathEvent();
                Status.OnDamageEvent -= TakeDamageEvent;
                TurnMemberState.OnTurnEndStateChanged -= SetPlay;
                TurnMemberState.Remove();
            };

            Stat = GetComponent<ICharacterStat>();
            Animation = GetComponent<ICharacterAnimation>();

            Stat.SetCharacter(this);
            Animation.SetCharacter(this);

            TurnMemberState = GetComponent<IMemberState>();
            EntityTransform = GetComponent<EntityTransform>();
            CameraController = GetComponent<ICameraController>();

            _moveComp = GetComponent<CharacterMoveBase>();
            _attackComp = GetComponent<CharacterAttack>();

            TurnMemberState.OnTurnEndStateChanged += SetPlay;

        }

        public void Initial(string characterName)
        {
            Stat.SetCharacterCode(characterName);

            GridCoord2D coord = Coord2DManager.Instance.
                Convertor.ConvertFromVector3(transform.position);

            CharacterManager.Instance.
                SetCharacterAt(coord, this);

            EntityTransform.Pos = coord;
            EntityTransform.OnPosChangedEvent += MoveTo;

        }

        private void HealEvent()
        {

        }

        private void TakeDamageEvent()
        {
            Animation.PlayAnim("SetTrigger", "Hit");
        }

        private void DeathEvent()
        {
            Animation.PlayAnim("SetTrigger", "Die");
            Remove();
        }

        public void Remove()
        {
            Actor.Die();
        }

        public void CamSetting(string key)
        {
            CameraManager.Instance.CameraSetting(transform, key);
        }

        public void PlayAnim(string triggerType,
            string triggerValue)
        {
            Actor.PlayAnim(triggerType, triggerValue);
        }

        public void SetPlay(bool turnEnd)
        {
            if (turnEnd) return;
            if (_actionSelector == null) return;

            IsRollDice = false;
            ActionEnd();
        }

        public void SetCC(
            ICharacterController actionSelector)
        {
            _actionSelector = actionSelector;
        }

        public void ActionEnd(float time = 0)
        {
            if (_actionSelector == null)
            {
                return;
            }

            Invoke(nameof(_ActionEnd), time);
        }

        public void _ActionEnd()
        {
            List<ICharacterController.Action> list
                = new List<ICharacterController.Action>();

            if (DicePoint.GetPoint() > 0)
                list.Add(ICharacterController.Action.Interaction);

            _moveComp.TryAddAction(this, list);
            _attackComp.TryAddAction(this, list);

            if (IsRollDice == false)
                list.Add(ICharacterController.Action.Dice);

            _actionSelector.Ready(this, list);
        }

        public void MoveTo(GridCoord2D before, GridCoord2D after)
        {
            CharacterManager.Instance.
                SetCharacterAt(before, null);

            CharacterManager.Instance.
                SetCharacterAt(after, this);
        }

        public void Move(IList<GridCoord2D> path)
        {
            _moveComp.Move(this, path);
        }

        public EntityInteractionBase GetInteraction()
        {
            _interactionEvent.SetData(this);
            return _interactionEvent;
        }

        public void Interaction(GridCoord2D targetPos)
        {
            EntityManager.Instance.Interaction(this, targetPos);
            DicePoint.UsePoint(1);
        }

    }

}
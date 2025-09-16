using System.Collections.Generic;
using SWEng.Data;
using CameraSystem;
using BKTools.Gaming.Dice;
using EasyH.Gaming.TurnBased;
using EasyH.Gaming.Inventory;

namespace SWEng.GamePlay
{
    public interface ICharacter : IDamageable, IEntity
    {

        public CharacterActor Actor { get; set; }
        public EntityTransform EntityTransform { get; set; }
        public ICharacterStat Stat { get; }
        public ICharacterAnimation Animation { get; }

        public bool IsRollDice { get; set; }

        public IDicePoint DicePoint { get; }
        public IMemberState TurnMemberState { get; }
        public Inventory Inventory { get; }
        public ICameraController CameraController { get; }

        public void Initial(string characterName);

        public void SetCC(ICharacterController actionSelector);
        public void Remove();

        public void SetPlay(bool turnEnd);
        public void ActionEnd(float time = 0);

        public void Move(IList<GridCoord2D> path);
        public void Interaction(GridCoord2D targetPos);

    }
}
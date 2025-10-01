using System.Collections.Generic;
using System;
using BKTools.Gaming.GridMap2D;

namespace SWEng
{
    public interface ISkillTargetSelector
    {
        public void Set(Action<IList<IDamageable>> action, ICharacter cc);
    }

    public class RangeTargetSelector : ISkillTargetSelector
    {
        public void Set(Action<IList<IDamageable>> action, ICharacter cc)
        {
            List<IDamageable> targets = new List<IDamageable>();
            ISet<GridCoord2D> neighbours = MapUnitManager.
                Instance.GetNeighboursFor(cc.EntityTransform.Pos);

            neighbours.Remove(cc.EntityTransform.Pos);

            foreach (var pos in neighbours)
            {
                IDamageable target = DamageableManager.
                    Instance.GetDamageableAt(pos);

                if (target == null) continue;

                targets.Add(target);
            }

            action?.Invoke(targets);
            
        }

    }

    public class EnemyTargetSelector : ISkillTargetSelector
    {
        public void Set(Action<IList<IDamageable>> action, ICharacter cc)
        {
            IList<IDamageable> targets = new List<IDamageable>();
            ISet<GridCoord2D> neighbours = MapUnitManager.
                Instance.GetNeighboursFor(cc.EntityTransform.Pos);

            foreach (var pos in neighbours)
            {
                ICharacter target = CharacterManager.
                    Instance.GetCharacterAt(pos);

                if (target == null) continue;
                
                if (target.TurnMemberState.TeamIdx
                    == cc.TurnMemberState.TeamIdx) continue;

                targets.Add(target);

                break;
            }

            action?.Invoke(targets);

        }
    }
}
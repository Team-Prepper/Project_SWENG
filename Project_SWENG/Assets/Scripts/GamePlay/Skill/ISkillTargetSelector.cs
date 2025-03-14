using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface ISkillTargetSelector {
    public void Set(ISkill attack, ICharacterController cc);
}

public class RangeTargetSelector : ISkillTargetSelector {
    public void Set(ISkill attack, ICharacterController cc)
    {
        List<IDamagable> targets = new List<IDamagable>();
        ISet<HexCoordinate> neighbours = HexGrid.Instance.GetNeighboursFor(cc.HexPos);

        foreach (var pos in neighbours) {
            ICharacterController targetCC = HexGrid.Instance.GetMapUnitAt(pos).CC;
            if (targetCC == null) continue;
            targets.Add(targetCC);

        }
        attack.Attack(targets, neighbours.ElementAt(0).ConvertToVector3());
    }

}

public class EnemyTargetSelector : ISkillTargetSelector {
    
    public void Set(ISkill attack, ICharacterController cc)
    {
        IList<IDamagable> targets = new List<IDamagable>();
        ISet<HexCoordinate> neighbours = HexGrid.Instance.GetNeighboursFor(cc.HexPos);
        Vector3 look = neighbours.ElementAt(0).ConvertToVector3();

        foreach(var pos in neighbours) {
            ICharacterController targetCC = HexGrid.Instance.GetMapUnitAt(pos).CC;
            if (targetCC == null) continue;
            if (targetCC.TeamIdx == cc.TeamIdx) continue;
            
            look = pos.ConvertToVector3();
            targets.Add(targetCC);
            break;
        }
        
        attack.Attack(targets, look);

    }
}
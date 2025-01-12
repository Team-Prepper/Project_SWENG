using System.Collections.Generic;
using UnityEngine;

public interface ICharacterController : IDamagable, IDicePoint, IEntity {

    Transform transform { get; }

    public void Initial(string characterName, int teamIdx, bool camSync);
    public void SetActionSelector(IActionSelector actionSelector);

    public void SetPlay();
    public void ActionEnd();
    public void TurnEnd();
    public void CamSetting(string key);

    public void PlayAnim(string triggerType, string triggerValue);

    public void Move(Queue<Vector3> path);

    public void MoveTo(HexCoordinate before, HexCoordinate after);

}

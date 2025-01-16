using System.Collections.Generic;
using UnityEngine;

public interface ICharacterController : IDamagable, IDicePoint, IEntity {

    public int TeamIdx { get; }
    Transform transform { get; }
    CharacterStatus Status { get; }

    public void Initial(string characterName, int teamIdx, bool camSync);
    public void SetActionSelector(IActionSelector actionSelector);

    public void SetPlay();
    public void ActionEnd(float time);
    public void TurnEnd();
    public void CamSetting(string key);

    public void PlayAnim(string triggerType, string triggerValue);

    public void Move(Queue<Vector3> path);
    public void Interaction(HexCoordinate targetPos);

    public void MoveTo(HexCoordinate before, HexCoordinate after);
    void EquipItem(string targetItem);
}

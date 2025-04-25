using System.Collections.Generic;
using UnityEngine;

public interface ICharacterController : IDamagable, IEntity {

    public int TeamIdx { get; }
    public Character Character { get; }
    public IDicePoint DicePoint { get; }
    public bool IsRollDice { get; set; }
    public IStatus Status { get; }
    public Inventory Inventory { get; }

    public void Initial(string characterName, int teamIdx, bool camSync);
    public void SetActionSelector(IActionSelector actionSelector);
    public void Remove();

    public void SetPlay();
    public void ActionEnd(float time = 0);
    public void TurnEnd();

    public void CamSetting(string key);
    public void PlayAnim(string triggerType, string triggerValue);

    public void Move(Queue<Vector3> path);
    public void Interaction(HexCoordinate targetPos);

}

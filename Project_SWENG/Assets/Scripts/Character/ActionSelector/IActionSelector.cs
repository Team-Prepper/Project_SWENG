using System.Collections.Generic;

public interface IActionSelector
{
    public void SetCharacterController(ICharacterController cc);
    public void Ready(IList<CharacterStatus.Action> actionList);

    public void Die();

}

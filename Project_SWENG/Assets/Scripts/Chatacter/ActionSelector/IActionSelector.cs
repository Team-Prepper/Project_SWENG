using System.Collections;
using System.Collections.Generic;
using CharacterSystem;

public interface IActionSelector
{
    public void SetCharacterController(ICharacterController cc);
    public void Ready(IList<Character.Action> actionList);
}

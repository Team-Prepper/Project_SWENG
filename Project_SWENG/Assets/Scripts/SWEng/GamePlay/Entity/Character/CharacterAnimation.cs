using UnityEngine;
using SWEng.Data;

namespace SWEng.GamePlay
{
    public class CharacterAnimation :
        MonoBehaviour, ICharacterAnimation
    {
        private ICharacter _target;

        public void SetCharacter(ICharacter character)
        {
            _target = character;
        }

        public void PlayAnim(
            string triggerType, string triggerValue)
        {
            _target.Actor.PlayAnim(triggerType, triggerValue);
        }

    }
}
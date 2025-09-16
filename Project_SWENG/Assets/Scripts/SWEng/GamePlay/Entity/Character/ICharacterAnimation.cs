using SWEng.Data;

namespace SWEng.GamePlay
{ 
    public interface ICharacterAnimation : ICharacterComponent
    {
        public void PlayAnim(
            string triggerType, string triggerValue);

    }
}
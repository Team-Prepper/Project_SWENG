
namespace SWEng
{
    public interface ICharacterAnimation : ICharacterComponent
    {
        public void PlayAnim(
            string triggerType, string triggerValue);

    }
}
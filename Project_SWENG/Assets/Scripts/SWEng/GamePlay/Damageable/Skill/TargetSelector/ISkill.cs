using System.Collections.Generic;

namespace SWEng.GamePlay.Skill
{

    public interface ISkill
    {
        public void Set(ICharacterController selector, ICharacter cc);
        public void Attack(IList<IDamageable> attackPos);
    }
    
}
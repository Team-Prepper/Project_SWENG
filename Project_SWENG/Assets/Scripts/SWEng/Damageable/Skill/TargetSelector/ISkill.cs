using System.Collections.Generic;

namespace SWEng
{

    public interface ISkill
    {
        public void Set(ICharacterController selector, ICharacter cc);
        public void Attack(IList<IDamageable> attackPos);
    }
    
}
using UnityEngine;

namespace SWEng.GamePlay
{
    
    [RequireComponent(typeof(ICharacterStat))]
    public class CharacterDamageCalcer : DamageCalcer
    {
        private ICharacterStat _target;

        private void Start()
        {
            _target = GetComponent<ICharacterStat>();
        }

        public override int CalcDamage(int input)
        {
            return Mathf.Max(input - _target.Dfs, 1);
        }
    }
    
}
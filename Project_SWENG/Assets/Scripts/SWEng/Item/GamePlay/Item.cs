using System.Collections.Generic;


namespace SWEng
{
    public class Item
    {
        private IList<IItemEffect> _effects;

        public void Action(ICharacter cc)
        {
            foreach (IItemEffect effect in _effects)
            {
                if (effect == null) continue;
                effect.Action(cc);
            }
        }

        public void Initial(string value)
        {
            string[] split = value.Split(",");

            _effects = new List<IItemEffect>();

            for (int i = 0; i < split.Length; i++)
            {
                _effects.Add(ItemEffectFactory.Get(split[i]));
            }
        }

    }
}
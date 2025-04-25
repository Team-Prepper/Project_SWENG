using System.Collections.Generic;

public class Item {
    
    public enum ItemTier {
        Common,
        UnCommon,
        Rare,
        Unique,
        Legendary,
        Mythic,
    }

    public enum ItemType {
        Helmet,
        Armor,
        Weapon,
        Shield,
        Consumables
    }

    private IList<IItemEffect> _effects;

    public void Action(ICharacterController cc) {
        foreach(IItemEffect effect in _effects) {
            if (effect == null) continue;
            effect.Action(cc);
        }
    }

    public void Initial(string value) {
        string[] split = value.Split(",");

        _effects = new List<IItemEffect>();

        for (int i = 0; i < split.Length; i++) {
            _effects.Add(ItemEffectFactory.Get(split[i]));
        }
    }
    
}
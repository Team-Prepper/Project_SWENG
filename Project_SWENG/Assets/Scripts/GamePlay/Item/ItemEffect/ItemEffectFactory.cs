public class ItemEffectFactory {
    public static IItemEffect Get(string value) {
        string[] split = value.Split(":");
        
        switch(split[0]) {
            case "Heal":
                return new HealEffect(split[1]);
        }
        return null;
    }

}
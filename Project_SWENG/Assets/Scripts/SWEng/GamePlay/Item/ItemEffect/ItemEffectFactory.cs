namespace SWEng.GamePlay
{
    public class ItemEffectFactory
    {
        public static IItemEffect Get(string value)
        {
            string[] split = value.Split(":");

            switch (split[0])
            {
                case "Heal":
                    return new HealItemEffect(split[1]);
                case "Dice":
                    return new DiceItemEffect(split[1]);
            }
            return null;
        }

    }
}
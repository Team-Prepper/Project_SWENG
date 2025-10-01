namespace SWEng
{

    public abstract class ShopInteractionBase : EntityInteractionBase
    {
        protected MapUnit _mapUnit;

        public void SetData(MapUnit mapUnit)
        {
            _mapUnit = mapUnit;
        }
    }
    
 }
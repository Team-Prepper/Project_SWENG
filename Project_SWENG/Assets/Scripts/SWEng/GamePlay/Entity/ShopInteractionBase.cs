using SWEng.Data;

namespace SWEng.GamePlay
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
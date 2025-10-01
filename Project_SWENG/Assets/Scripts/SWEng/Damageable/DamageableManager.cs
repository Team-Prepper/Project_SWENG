using System.Collections;
using System.Collections.Generic;
using BKTools.Gaming.GridMap2D;
using EasyH;

namespace SWEng
{

    public class DamageableManager : Singleton<DamageableManager>
    {
        private IDictionary<GridCoord2D, IDamageable> _damageableDict
            = new Dictionary<GridCoord2D, IDamageable>();

        public void SetDamageableAt(
            GridCoord2D pos, IDamageable damageable)
        {
            if (!_damageableDict.ContainsKey(pos))
            {
                _damageableDict.Add(pos, damageable);
                return;
            }
            _damageableDict[pos] = damageable;
        }

        public IDamageable GetDamageableAt(GridCoord2D pos)
        { 
            if (!_damageableDict.ContainsKey(pos)) return null;
            return _damageableDict[pos];
            
        }
    }
}
using System.Collections.Generic;
using EasyH;
using SWEng.Data;

namespace SWEng.GamePlay
{
    public class CharacterManager :
        Singleton<CharacterManager>
    {

        private IDictionary<GridCoord2D, ICharacter>
            _characterDict = new Dictionary<GridCoord2D, ICharacter>();

        public void SetCharacterAt(
            GridCoord2D pos, ICharacter cc)
        {
            if (cc != null && cc.TurnMemberState.TeamIdx == 0)
            { 
                MapUnitManager.Instance.GetMapUnitAt(pos).
                    CloudActiveFalse();
            }
            
            DamageableManager.Instance.SetDamageableAt(pos, cc);
            EntityManager.Instance.SetEntityAt(pos, cc);

            if (!_characterDict.ContainsKey(pos))
            {
                _characterDict.Add(pos, cc);
                return;
            }
            _characterDict[pos] = cc;
        }

        public ICharacter GetCharacterAt(GridCoord2D pos)
        {
            if (!_characterDict.ContainsKey(pos)) return null;
            return _characterDict[pos];
        }

    }
}
using UnityEngine;
using System;
using SWEng.Data;

namespace SWEng.GamePlay
{
    public class EntityTransform : MonoBehaviour
    {

        public Action<GridCoord2D, GridCoord2D>
            OnPosChangedEvent
        { get; set; }

        private GridCoord2D _pos;

        public GridCoord2D Pos
        {
            get { return _pos; }
            set
            {
                if (value.Equals(Pos)) return;

                OnPosChangedEvent?.Invoke(Pos, value);
                _pos = value;
            }
        }
    }
}
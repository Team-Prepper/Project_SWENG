using UnityEngine;
using System;

namespace SWEng.GamePlay
{

    public abstract class ItemInteractionBase : EntityInteractionBase
    {
        protected string _itemCode;
        protected Action _interactionEvent;

        public void SetData(string itemCode, Action interaction)
        {
            _itemCode = itemCode;
            _interactionEvent = interaction;
        }
    }
    
 }
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DroppableUI : MonoBehaviour, IDropHandler
{
	protected	Image			icon;
	public Item.ItemType itemType;

	private void Awake()
	{
		icon	= this.transform.Find("Icon")?.GetComponent<Image>();
		
	}
    
	

	public virtual void OnDrop(PointerEventData eventData)
	{
		DraggableUI baseItem = eventData.pointerDrag.gameObject.GetComponent<DraggableUI>();
		if ( baseItem.itemData.type == this.itemType)
		{
			baseItem.EquipItem();
		}
	}
}


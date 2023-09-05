using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class DraggableUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	protected	Transform		previousParent;
	
	protected	GameObject      tooltipGUI;
	protected	GameObject      cellTooltip;
	protected	GameObject      dragTooltip;

	protected GameObject DetailPanel;

	protected  GameObject EquipGUI;
	
	
	public Item itemData;
	public bool isEquip = false;
	

	private void OnEnable()
	{
		tooltipGUI = GameObject.Find("InventoryGUI/Background/Tooltips");
		cellTooltip = tooltipGUI.transform.Find("Cell/Cell_Tooltip").gameObject;
		dragTooltip = tooltipGUI.transform.Find("Drag/Drag_Tooltip").gameObject;
		
		DetailPanel = GameObject.Find("InventoryGUI/Background/Sections/Details/Content");
		EquipGUI = transform.Find("Equipped").gameObject;
	}

	#region Tooltip
	
		// Show Tooltip
		public void OnPointerEnter(PointerEventData eventData)
		{
			cellTooltip.SetActive(true);
			cellTooltip.transform.position = this.transform.position + new Vector3(0, -30, 0);
		}

		// Hide Tooltip
		public void OnPointerExit(PointerEventData eventData)
		{
			cellTooltip.SetActive(false);
		}
    
	#endregion

	#region Drag&Drop
	
		public void OnBeginDrag(PointerEventData eventData)
		{
			dragTooltip.SetActive(true);
			previousParent = dragTooltip.transform.parent;
			dragTooltip.GetComponent<Image>().sprite = itemData.icon;
		}

		public void OnDrag(PointerEventData eventData)
		{
			dragTooltip.GetComponent<RectTransform>().position = eventData.position;
			//rect.position = eventData.position;
		}

		public virtual void OnEndDrag(PointerEventData eventData)
		{
			dragTooltip.transform.SetParent(previousParent);
			dragTooltip.GetComponent<RectTransform>().position = previousParent.GetComponent<RectTransform>().position;
			dragTooltip.SetActive(false);
		}

	#endregion

	#region Click

	public void OnPointerClick(PointerEventData eventData)
	{
		DetailPanel.SetActive(true);
		DetailPanel.transform.Find("Icon").GetComponent<Image>().sprite = itemData.icon;
		Transform itemInfo = DetailPanel.transform.Find("Scroll/Viewport/Content");
		itemInfo.Find("Name").GetComponent<Text>().text = itemData.itemName;
		Transform buttons = itemInfo.transform.Find("Buttons");
		switch (itemData.type)
		{
			case Item.ItemType.Consumables:
				buttons.transform.Find("ButtonUse").gameObject.SetActive(true);
				buttons.transform.Find("ButtonDrop").gameObject.SetActive(true);
				buttons.transform.Find("ButtonsEquip").gameObject.SetActive(true);
				break;
			default: // Equipment item
				buttons.transform.Find("ButtonUse").gameObject.SetActive(false);
				buttons.transform.Find("ButtonDrop").gameObject.SetActive(true);
				buttons.transform.Find("ButtonsEquip").gameObject.SetActive(true);
				buttons.transform.Find("ButtonsEquip/ButtonEquip").gameObject.SetActive(!isEquip);
				buttons.transform.Find("ButtonsEquip/ButtonUnequip").gameObject.SetActive(isEquip);
				Button btnEquip = buttons.transform.Find("ButtonsEquip/ButtonEquip").GetComponent<Button>();
				Button btnUnequip = buttons.transform.Find("ButtonsEquip/ButtonUnequip").GetComponent<Button>();
				btnEquip.onClick.RemoveAllListeners();
				btnEquip.onClick.AddListener(EquipItem);
				break;
		}
	}
    
	#endregion

	#region Equipment

	public void EquipItem()
	{
		isEquip = true;
		EquipGUI.SetActive(true);
		GameObject equipmentSlot = GameObject.Find("InventoryGUI/Background/Sections/Equipment/EquipSlotGrid");
		switch (itemData.type)
		{
			case Item.ItemType.Helmet:
				equipmentSlot.transform.Find("EquipSlot-Helmet/Icon").GetComponent<Image>().sprite = itemData.icon;
				int helmetCode = itemData.id % 100;
				int helmetType = itemData.id / 100;
                EquipManager.Instance.EquipHelmet(helmetCode, helmetType);
				break;
			
			case Item.ItemType.Armor:
				equipmentSlot.transform.Find("EquipSlot-Armor/Icon").GetComponent<Image>().sprite = itemData.icon;
				EquipManager.Instance.EquipArmor(itemData.id);
				break;
			
			case Item.ItemType.Weapon:
				equipmentSlot.transform.Find("EquipSlot-Weapon/Icon").GetComponent<Image>().sprite = itemData.icon;
				GameObject weaponSlot =
					GameObject.Find(
						"Player/Root/Hips/Spine_01/Spine_02/Spine_03/Clavicle_R/Shoulder_R/Elbow_R/Hand_R/WeaponSlot");
				GameObject iWeapon = Instantiate(itemData.itemObject, weaponSlot.transform);
				break;
			
			case Item.ItemType.Shield:
				equipmentSlot.transform.Find("EquipSlot-Shield/Icon").GetComponent<Image>().sprite = itemData.icon;
				GameObject shieldSlot =
					GameObject.Find(
						"Player/Root/Hips/Spine_01/Spine_02/Spine_03/Clavicle_L/Shoulder_L/Elbow_L/Hand_L/ShieldSlot");
				GameObject iShield = Instantiate(itemData.itemObject, shieldSlot.transform);
				break;
		}

	}

	#endregion
	
}


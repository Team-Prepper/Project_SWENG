using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SWEng;

public class GUIItemUI : MonoBehaviour
{
    public static GUIItemUI Instance;

    public Transform itemPickupUITransform;
    public GameObject itemPickupUI;
    public Sprite[] itemTierBackground;

    private void Awake()
    {
        Instance = this;
    }
    
    public GameObject SetPickupItemUI(ItemPickup itemPickup)
    {
        GameObject obj = Instantiate(itemPickupUI, itemPickupUITransform);
        var itemMain = obj.GetComponent<Image>();
        var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
        var itemBack = obj.transform.Find("ItemBack").GetComponent<Image>();
        var itemName = obj.transform.Find("ItemName").GetComponent<TextMeshProUGUI>();
        var itemInfo = obj.transform.Find("ItemInfo").GetComponent<TextMeshProUGUI>();
        var itemPickupBtn = obj.transform.Find("Pickup").GetComponent<Button>();
    
        itemMain.color = ItemDataManager.Instance.GetTierColor(itemPickup.item.tier);
        itemIcon.sprite = itemPickup.item.Icon;
        itemBack.sprite = itemTierBackground[(int)itemPickup.item.tier];
        itemName.text = itemPickup.item.ItemName;
        
        itemPickupBtn.onClick.AddListener(() => OnClickEvent(itemPickup));
        
        return obj;
    }

    private void OnClickEvent(ItemPickup itemPickup)
    {
        itemPickup.PickupHandler(null);
    }
}

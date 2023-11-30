using LangSystem;
using ObserberPattern;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GUI_ItemEquiped : MonoBehaviour, IObserver
{
    [SerializeField] Image slotHelmet;
    [SerializeField] Image iconHelmet;
    [SerializeField] Image slotArmor;
    [SerializeField] Image iconArmor;
    [SerializeField] Image slotHandL;
    [SerializeField] Image iconHandL;
    [SerializeField] Image slotHandR;
    [SerializeField] Image iconHandR;

    [Header("MyCharacterPopUp")]
    [SerializeField] Text helmetValue;
    [SerializeField] Text armorValue;
    [SerializeField] Text weaponValue;
    [SerializeField] TextMeshProUGUI weaponSkillValue;
    [SerializeField] Text shieldValue;

    readonly List<Color> colors = new List<Color>()
    {
        new Color(0.7169812f, 0.5083325f, 0.01690993f, 1f),
        new Color(0.2722067f, 0.5849056f, 0.13519505f, 1f),
        new Color(0.1541919f, 0.3933419f, 0.71223475f, 1f),
        new Color(0.4543215f, 0.2126654f, 0.99174132f, 1f),
        new Color(0.8971235f, 0.8946123f, 0.21643756f, 1f),
        new Color(0.9912354f, 0.3451256f, 0.61234353f, 1f)

    };

    void Start()
    {
        InventoryManager.Instance.AddObserver(this);
    }

    public void SetItemGUI(Item item)
    {
        Color tierColor = colors[(int)item.tier];
        
        switch (item.type)
        {
            case Item.ItemType.Helmet:
                slotHelmet.color = tierColor;
                if(item.icon != null)
                    iconHelmet.sprite = item.icon;
                if (helmetValue != null)
                    helmetValue.text = string.Format(StringManager.Instance.GetStringByKey("shopItem_All"), item.value);
                
                    
                break;
            case Item.ItemType.Armor:
                slotArmor.color = tierColor;
                if (item.icon != null)
                    iconArmor.sprite = item.icon;
                if (armorValue != null)
                    armorValue.text = string.Format(StringManager.Instance.GetStringByKey("shopItem_HP"), item.value);
                break;
            case Item.ItemType.Weapon:
                slotHandL.color = tierColor;
                if (item.icon != null)
                    iconHandL.sprite = item.icon;
                if (weaponValue != null)
                    weaponValue.text = string.Format(StringManager.Instance.GetStringByKey("shopItem_Attack"), item.value);
                if(weaponSkillValue != null && item.hasSkill)
                    weaponSkillValue.text = "Cost : " + item.skillCost.ToString() + "\nDMG : " + item.skillDmg.ToString();
                break;
            case Item.ItemType.Shield:
                slotHandR.color = tierColor;
                if (item.icon != null)
                    iconHandR.sprite = item.icon;
                if (shieldValue != null)
                    shieldValue.text = string.Format(StringManager.Instance.GetStringByKey("shopItem_Defense"), item.value);
                break;
        }
    }

    public void Notified()
    {
        if (!slotHelmet) return;
        List<Item> list = InventoryManager.Instance.GetItems();
        foreach (Item item in list) {
            SetItemGUI(item);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EHTool;

public class ItemManager : MonoSingleton<ItemManager>
{
    [SerializeField] List<ItemData> _items = new List<ItemData>();

    IDictionary<string, ItemData> _dict;
    IDictionary<ItemData.ItemTier, Color> _tierColorDict;

    protected override void OnCreate()
    {
        base.OnCreate();
        _dict = new Dictionary<string, ItemData>();

        foreach (ItemData data in _items) {
            Debug.Log(data.itemName);
            _dict.Add(data.itemName, data);
        }

        _tierColorDict = new Dictionary<ItemData.ItemTier, Color>() {
            {  ItemData.ItemTier.Common,    new Color(0.7169812f, 0.5083325f, 0.01690993f, 1f) },
            {  ItemData.ItemTier.UnCommon,  new Color(0.2722067f, 0.5849056f, 0.13519505f, 1f) },
            {  ItemData.ItemTier.Rare,      new Color(0.1541919f, 0.3933419f, 0.71223475f, 1f) },
            {  ItemData.ItemTier.Unique,    new Color(0.4543215f, 0.2126654f, 0.99174132f, 1f) },
            {  ItemData.ItemTier.Legendary, new Color(0.8971235f, 0.8946123f, 0.21643756f, 1f) },
            {  ItemData.ItemTier.Mythic,    new Color(0.9912354f, 0.3451256f, 0.61234353f, 1f) }
        };
    }

    public Color GetTierColor(ItemData.ItemTier tier) {
        return _tierColorDict[tier];
    }

    public ItemData GetItemData(string itemCode) {
        return _dict[itemCode];
    }

    public IList<string> GetRandomItemList(int itemCounts)
    {
        return GetRandomSelection(_dict.Keys, itemCounts);
    }

    private IList<T> GetRandomSelection<T>(ICollection<T> originalList, int count)
    {
        IList<T> selection = new List<T>();

        IList<T> tempList = new List<T>(originalList);

        while (selection.Count < count && tempList.Count > 0)
        {
            int randomIndex = Random.Range(0, tempList.Count);
            T selectedItem = tempList[randomIndex];
            selection.Add(selectedItem);
            tempList.RemoveAt(randomIndex);
        }

        return selection;
    }
}

using System.Collections.Generic;
using UnityEngine;
using EHTool;

public class ItemManager : Singleton<ItemManager>
{
    private IDictionary<string, ItemData> _dict;
    private IDictionary<Item.ItemTier, Color> _tierColorDict;

    public IList<string> ShopItems { get; private set; }

    protected override void OnCreate()
    {
        base.OnCreate();

        IDictionaryConnector<string, string> _itemDataDict
            = new JsonDictionaryConnector<string, string>();

        _dict = new Dictionary<string, ItemData>();

        foreach (var data in _itemDataDict.ReadData("ItemInfor")) {
            _dict.Add(data.Key, AssetOpener.Import<ItemData>(data.Value));
        }

        _tierColorDict = new Dictionary<Item.ItemTier, Color>() {
            {  Item.ItemTier.Common,    new Color(0.7169812f, 0.5083325f, 0.01690993f, 1f) },
            {  Item.ItemTier.UnCommon,  new Color(0.2722067f, 0.5849056f, 0.13519505f, 1f) },
            {  Item.ItemTier.Rare,      new Color(0.1541919f, 0.3933419f, 0.71223475f, 1f) },
            {  Item.ItemTier.Unique,    new Color(0.4543215f, 0.2126654f, 0.99174132f, 1f) },
            {  Item.ItemTier.Legendary, new Color(0.8971235f, 0.8946123f, 0.21643756f, 1f) },
            {  Item.ItemTier.Mythic,    new Color(0.9912354f, 0.3451256f, 0.61234353f, 1f) }
        };
        
        ShopItemInitial(5);
    }

    public void ShopItemInitial(int cnt) {
        ShopItems = GetRandomItemList(cnt);
    }

    public Color GetTierColor(Item.ItemTier tier) {
        return _tierColorDict[tier];
    }

    public ItemData GetItemData(string itemCode) {
        return _dict[itemCode];
    }

    public IList<string> GetRandomItemList(int count)
    {
        List<string> retval = new List<string>();

        int itemCount = _dict.Keys.Count;
        for (int i = 0; i * itemCount < count - itemCount; i++) {
            retval.AddRange(GetRandomSelection(_dict.Keys, itemCount));
        }
        retval.AddRange(GetRandomSelection(_dict.Keys, count - retval.Count));
        return retval;
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

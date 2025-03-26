using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Items", menuName = "Items")]
public class Items : ScriptableObject
{
    public List<Item> Itemss;

    public List<List<Item>> Denemeeeee;

    private List<List<Item>> _separatedRandomItems;
    public List<List<Item>> SeparatedRandomItems
    {
        get
        {
            if (_separatedRandomItems == null)
            {
                _separatedRandomItems = new List<List<Item>>();
                foreach (var item in Itemss)
                {
                    var itemEnum = item.ItemRareTypesEnum;
                    if (_separatedRandomItems.Count == 0)
                    {
                        _separatedRandomItems.Add(new List<Item>() { item });
                    }
                    else
                    {
                        List<Item> temp = null;
                        foreach (var rarerityLists in _separatedRandomItems)
                        {
                            if (rarerityLists[0].ItemRareTypesEnum == itemEnum)
                            {
                                //rarerityLists.Add(item);
                                temp = rarerityLists;
                                break;
                            }

                        }
                        if (temp != null)
                        {
                            temp.Add(item);
                        }
                        else
                        {
                            _separatedRandomItems.Add(new List<Item>() { item });
                        }
                    }

                }
            }
            /*
            foreach (var item in _separatedRandomItems)
            {
                Debug.Log("-----" + item[0].ItemRareTypesEnum + "-----");
                foreach (var item1 in item)
                {
                    Debug.Log(item1.Name);
                }
                Debug.Log("------------------------------");
            }*/
            Denemeeeee = new List<List<Item>>(_separatedRandomItems);
            return _separatedRandomItems;
        }
    }

    public Item GetItem(string itemName)
    {
        return Itemss.FirstOrDefault(a => a.Name == itemName);
    }
    public Item GetRandomItem()
    {
        float val = Random.Range(0, 100f);
        var rareType = ItemRareType.GetItemRareTypesEnum(val);
        Item result = GetRandomItemByPrevalence(rareType);
        Debug.Log(result.Name);
        return result;
    }
    public Item GetRandomItem(float val)
    {
        var rareType = ItemRareType.GetItemRareTypesEnum(val);
        Item result = GetRandomItemByPrevalence(rareType);
        Debug.Log(result.Name);
        return result;
    }

    public Item GetRandomItemByPrevalence(ItemRareTypesEnum itemRareTypesEnum)
    {
        foreach (var item in SeparatedRandomItems)
        {
            if (item[0].ItemRareTypesEnum == itemRareTypesEnum)
            {
                return item[Random.Range(0, item.Count)];
            }
        }
        return null;
    }

    public List<Item> GetItems(List<ItemTypeEnum> itemTypeEnums)
    {
        List<Item> result = new();
        foreach (var item in Itemss)
        {
            if (ItemTypeControl(itemTypeEnums, item.itemTypeEnums))
            {
                result.Add(item);
                continue;

            }
        }
        return result;
    }
    public bool ItemTypeControl(List<ItemTypeEnum> itemTypeEnums, List<ItemTypeEnum> itemTypeEnums1)
    {
        foreach (var item in itemTypeEnums)
        {
            var x = itemTypeEnums1.Contains(item);
            if (x)
            {
                return true;
            }
        }
        return false;
    }
}

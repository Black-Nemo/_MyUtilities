using System;
using System.Collections.Generic;
using UnityEngine;

public class Slot
{
    public int CurrentItemCount;
    public Item Item;

    public List<ItemTypeEnum> requirementsItemTypeEnums = new List<ItemTypeEnum>();

    public int Index;


    public Action ChangeValuesEvent;

    public Action<Item, Item> ChangeItemEvent;

    public Slot(int _index)
    {
        Index = _index;
        CurrentItemCount = 0;
    }
    public Slot(int _index, Item _item)
    {
        ChangeItemEvent?.Invoke(Item, _item);
        Index = _index;
        Item = _item;
        CurrentItemCount = 1;
    }
    public Slot(int _index, Item _item, int _count)
    {
        ChangeItemEvent?.Invoke(Item, _item);
        Index = _index;
        Item = _item;
        CurrentItemCount = _count;
    }


    public void AddItem()
    {
        CurrentItemCount++;
        ChangeValuesEvent?.Invoke();
    }
    public void AddItem(int _count)
    {
        CurrentItemCount += _count;
        ChangeValuesEvent?.Invoke();
    }

    public void ChangeItem(int _index, Item _item, int _count)
    {
        ChangeItemEvent?.Invoke(Item, _item);
        Index = _index;
        Item = _item;
        CurrentItemCount = _count;
        ChangeValuesEvent?.Invoke();
    }
    public void ChangeItem(Item _item, int _count)
    {
        ChangeItemEvent?.Invoke(Item, _item);
        Item = _item;
        CurrentItemCount = _count;
        ChangeValuesEvent?.Invoke();
    }
    public void Clear(int _index)
    {
        ChangeItemEvent?.Invoke(Item, null);
        Item = null;
        Index = _index;
        CurrentItemCount = 0;
        ChangeValuesEvent?.Invoke();
    }

    public void RemoveItem(int val)
    {
        CurrentItemCount -= val;
        if (CurrentItemCount <= 0)
        {
            Clear(Index);
        }
        ChangeValuesEvent?.Invoke();
    }


    //
    public Item GetItem()
    {
        return Item;

    }
    public int GetCurrentItemCount()
    {
        return CurrentItemCount;
    }

    public bool IsSlotFull()
    {
        if (Item == null)
        {
            return false;
        }
        if (Item.StackMaxCount > CurrentItemCount)
        {
            return false;
        }
        return true;
    }
    public int NeedFullItemCount()
    {
        return Item.StackMaxCount - CurrentItemCount;
    }
    public string GetItemName()
    {
        if (IsSlotUsed())
        {
            return "";
        }
        return Item.Name;
    }
    public bool IsSlotUsed()
    {
        return (Item == null) ? false : true;
    }
    public bool IsEqualItem(Item _item)
    {
        return (Item.Name == _item.Name) ? true : false;
    }
    public int GetRemaningItemCount(int _count)
    {
        return NeedFullItemCount() - _count;
    }
    public bool MeetOneOfTheRequirementsItemTypeEnums(List<ItemTypeEnum> itemTypeEnums)
    {
        if (requirementsItemTypeEnums.Count == 0) { return true; }
        foreach (var item in requirementsItemTypeEnums)
        {
            if (itemTypeEnums.Contains(item))
            {
                return true;
            }
        }
        return false;
    }
}

[System.Serializable]
public class SlotJson
{
    public int Index;
    public int CurrentItemCount;
    public ItemJson ItemJson;
    public SlotJson(int _index, ItemJson itemJson, int _currentItemCount)
    {
        Index = _index;
        ItemJson = itemJson;
        CurrentItemCount = _currentItemCount;
    }
    public SlotJson(int _index, string _itemName, int _currentItemCount)
    {
        Index = _index;
        ItemJson = new ItemJson(_itemName);
        CurrentItemCount = _currentItemCount;
    }
    public SlotJson(int _index)
    {
        Index = _index;
        ItemJson = new ItemJson();
        CurrentItemCount = 0;
    }
}
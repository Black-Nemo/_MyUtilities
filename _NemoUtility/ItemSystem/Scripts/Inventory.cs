using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<ItemTypeEnum> requirementsItemTypeEnums = new List<ItemTypeEnum>();
    public bool IsWearableInventory = false;

    [SerializeField] private Items items;

    public string Name;

    public List<Slot> Slots = new List<Slot>();
    public Vector2Int InventorySize;

    public bool autoCreateSlots = true;
    public List<ItemTypeEnums> ItemTypeEnums = new List<ItemTypeEnums>();

    public Action<Inventory> ChangeSlotsEvent;
    public Action<string> ChangeSlotsJsonEvent;
    public Action<Slot> LeftItemEvent;
    public Action<Item, int> AddItemEvent;

    private void OnEnable()
    {
        if (IsWearableInventory)
        {

        }
    }
    private void OnDisable()
    {
    }

    private void Awake()
    {
        if (autoCreateSlots)
        {
            Create();
        }
        else
        {
            for (int i = 0; i < ItemTypeEnums.Count; i++)
            {
                Slots.Add(new Slot(i) { requirementsItemTypeEnums = new List<ItemTypeEnum>(ItemTypeEnums[i].ItemTypeEnumList) });
            }
        }
    }
    private void Create()
    {
        for (int i = 0; i < InventorySize.x * InventorySize.y; i++)
        {
            Slots.Add(new Slot(i) { requirementsItemTypeEnums = new List<ItemTypeEnum>(requirementsItemTypeEnums) });
        }
        ChangeValues();
    }

    public void CreateEmptyInventory(Vector2Int _size)
    {
        for (int i = 0; i < _size.x * _size.y; i++)
        {
            Slots.Add(new Slot(i));
        }
        ChangeValues();
    }
    public void AddItem(Item _item, int _count)
    {
        foreach (var slot in Slots)
        {
            if (slot.IsSlotUsed() && slot.IsEqualItem(_item) && !slot.IsSlotFull())
            {
                if (slot.GetRemaningItemCount(_count) >= 0)
                {
                    slot.AddItem(_count);
                    ChangeValues();
                    Debug.Log("HEH1");
                    AddItemEvent?.Invoke(_item, _count);
                    return;
                }
                else
                {
                    int _remaningItemCount = -slot.GetRemaningItemCount(_count);
                    slot.AddItem(slot.NeedFullItemCount());
                    AddItem(_item, _remaningItemCount);
                    ChangeValues();
                    Debug.Log("HEH2");
                    AddItemEvent?.Invoke(_item, _count);
                    return;
                }
            }
        }
        AddItemLatestEmptySlot(_item, _count);
        AddItemEvent?.Invoke(_item, _count);
    }
    public void AddItem(int _index, Item _item, int _count)
    {
        Slots[_index].ChangeItem(_index, _item, _count);
        ChangeValues();
        AddItemEvent?.Invoke(_item, _count);
    }
    public void Change2Slot()
    {

    }
    public void ClearSlot(int _index)
    {
        Slots[_index].Clear(_index);
        ChangeValues();
    }

    public void AddItem(Item _item)
    {
        AddItem(_item, 1);
    }

    private void AddItemLatestEmptySlot(Item _item, int _count)
    {
        foreach (var slot in Slots)
        {
            if (!slot.IsSlotUsed())
            {
                if (_item.StackMaxCount >= _count)
                {
                    slot.ChangeItem(_item, _count);
                    ChangeValues();
                    return;
                }
                else
                {
                    slot.ChangeItem(_item, _item.StackMaxCount);
                    _count -= _item.StackMaxCount;
                    //AddItemLatestEmptySlot(_item, -slot.GetRemaningItemCount(_item.StackMaxCount));
                    //ChangeValues();
                    //return;
                }
            }
        }
        LeftItemEvent?.Invoke(new Slot(0, _item, _count));
        ChangeValues();
    }
    public int HowManyThisItem(Item item)
    {
        int result = 0;
        foreach (var slot in Slots)
        {
            if (slot.Item != null && slot.Item.Name == item.Name)
            {
                result += slot.CurrentItemCount;
            }
        }
        return result;
    }

    public int RemoveItem(Item item, int value)
    {
        foreach (var slot in Slots)
        {
            if (slot.Item == item)
            {
                if (slot.CurrentItemCount > value)
                {
                    slot.CurrentItemCount -= value;
                    ChangeValues();
                    return 0;
                }
                else if (slot.CurrentItemCount == value)
                {
                    slot.Clear(slot.Index);
                    ChangeValues();
                    return 0;
                }
                else
                {
                    value -= slot.CurrentItemCount;
                    slot.Clear(slot.Index);
                }
            }
        }
        ChangeValues();
        return value;
    }
    public Item RemoveItem(List<Item> items, bool isFind = false)
    {
        foreach (var slot in Slots)
        {
            if (slot.Item == null) { continue; }
            Item item = items.FirstOrDefault((a) => a.Name == slot.Item.Name);
            if (item != null)
            {
                if (isFind) { return slot.Item; }
                if (slot.CurrentItemCount > 1)
                {
                    slot.CurrentItemCount -= 1;
                    ChangeValues();
                    return slot.Item;
                }
                else if (slot.CurrentItemCount == 1)
                {
                    Item _item = slot.Item;
                    slot.Clear(slot.Index);
                    ChangeValues();
                    return _item;
                }
            }
        }
        if (!isFind) { ChangeValues(); }
        return null;
    }

    public void ChangeItems(Slot s1, Slot s2)
    {
        Item tempItem = s1.Item;
        int tempItemCount = s1.CurrentItemCount;
        Slots[s1.Index].ChangeItem(s1.Index, s2.Item, s2.CurrentItemCount);
        Slots[s2.Index].ChangeItem(s2.Index, tempItem, tempItemCount);
        ChangeValues();
    }

    public void _ChangeValues()
    {
        InventoryJson inventoryJson = new InventoryJson();
        inventoryJson.EqualInventory(this);
        ChangeSlotsJsonEvent?.Invoke(JsonUtility.ToJson(inventoryJson));
    }

    public void ChangeValues()
    {
        ChangeSlotsEvent?.Invoke(this);
    }

    public int GetEmptySlotCount()
    {
        int result = 0;
        foreach (var slot in Slots)
        {
            if (!slot.IsSlotUsed())
            {
                result++;
            }
        }
        return result;
    }

    public int GetNumberOfEmptySlotsWithTheSameItem(Item item)
    {
        int result = 0;
        foreach (var slot in Slots)
        {
            if (!slot.IsSlotUsed() && slot.Item.Name == item.Name)
            {
                result += slot.NeedFullItemCount();
            }
        }
        return result;
    }

    public Slot GetSlot(Vector2Int _location)
    {
        return Slots[GetIndex(_location)];
    }
    public Slot GetSlot(int _index)
    {
        return Slots[_index];
    }

    public int GetIndex(Vector2Int _location)
    {
        return (_location.y * InventorySize.x) + _location.x;
    }
    public int GetIndex(Slot _slot)
    {
        for (int i = 0; i < Slots.Count; i++)
        {
            if (Slots[i] == _slot)
            {
                return i;
            }
        }
        return -1;
    }
    public Vector2Int GetLocation(int _index)
    {
        int y = _index / InventorySize.x;
        int x = _index % InventorySize.x;
        return new Vector2Int(x, y);
    }
    public bool FindSlot(Slot _slot)
    {
        foreach (var slot in Slots)
        {
            if (_slot == slot) { return true; }
        }
        return false;
    }
    public void ThisSlotCollect(Slot _slot)
    {
        foreach (var slot in Slots)
        {
            Debug.Log(slot.Index);
            if (!slot.IsSlotUsed()) { Debug.Log("ItemYOK"); continue; }
            if (_slot == slot) { continue; }
            if (!slot.IsEqualItem(_slot.Item)) { continue; }
            else
            {
                SlotCollect(_slot, slot);
                if (_slot.IsSlotFull()) { return; }
            }
        }
    }
    public void SlotCollect(Slot _s1, Slot _s2)
    {
        if (_s1.GetRemaningItemCount(_s2.CurrentItemCount) >= 0)
        {
            _s1.AddItem(_s2.CurrentItemCount);
            _s2.Clear(_s2.Index);
        }
        else
        {
            _s2.CurrentItemCount -= _s1.NeedFullItemCount();
            _s1.AddItem(_s1.NeedFullItemCount());
        }
    }

    public void EqualJson(InventoryJson inventoryJson)
    {
        Name = inventoryJson.Name;
        for (int i = 0; i < Slots.Count; i++)
        {
            var slot = Slots[i];
            var slotJ = inventoryJson.SlotJsons[i];

            if (slotJ.ItemJson.Name != "null")
            {
                slot.ChangeItem(items.GetItem(slotJ.ItemJson.Name), slotJ.CurrentItemCount);
            }
            else
            {
                slot.Clear(slot.Index);
            }
        }
        ChangeValues();
    }

    public InventoryJson GetJsonInventory()
    {
        InventoryJson result = new InventoryJson();
        result.EqualInventory(this);
        return result;
    }
}
[System.Serializable]
public class InventoryJson
{
    public string Name;
    public List<SlotJson> SlotJsons;
    public Vector2Int InventorySize;
    public InventoryJson()
    {
        SlotJsons = new List<SlotJson>();
    }

    public void EqualInventory(Inventory inventory)
    {
        Name = inventory.Name;
        InventorySize = inventory.InventorySize;

        for (int i = 0; i < inventory.Slots.Count; i++)
        {
            var slot = inventory.Slots[i];

            string itemName = "";

            if (slot.Item != null)
            {
                itemName = slot.Item.Name;
            }

            SlotJsons.Add(new SlotJson(i, itemName, slot.CurrentItemCount));
        }
    }
}
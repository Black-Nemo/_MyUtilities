using System.Collections.Generic;
using NemoUtility;
using UnityEngine;

public class InventoryOfTheObject : MonoBehaviour
{

    public Inventory Inventory;


    [SerializeField] private Items items;

    [SerializeField] bool isSaveJson;
    [ConditionalField("isSaveJson")][SerializeField] string filePath;




    [SerializeField] private List<InventoryUIPanel> inventoryUIPanels = new List<InventoryUIPanel>();


    public void AddUInventoryUIPanel(InventoryUIPanel inventoryUIPanel)
    {
        inventoryUIPanels.Add(inventoryUIPanel);
        if (Inventory != null)
        {
            Inventory.ChangeSlotsEvent += inventoryUIPanel.ChangeSlots;
            Inventory.ChangeValues();
        }
    }

    public void AddItem(Item _item, int _count)
    {
        Inventory.AddItem(_item, _count);
    }

    public InventoryUISlot GetUISlot(int i)
    {
        if (inventoryUIPanels.Count > 0)
        {
            return inventoryUIPanels[0].GetInventoryUISlot(i);
        }
        return null;
    }

    private void SaveJson(Inventory inventory)
    {
        //Debug.Log(InventoryUtility.GetInventoryText(Inventory));
        if (isSaveJson)
        {
            //InventoryUtility.SaveJsonDatas(Inventory, filePath);

            //InventoryUtility.GetJsonDatas(items, filePath, Inventory);
            for (int i = 0; i < Inventory.Slots.Count; i++)
            {
                inventory.Slots[i].ChangeItem(Inventory.Slots[i].Index, Inventory.Slots[i].Item, Inventory.Slots[i].CurrentItemCount);
            }
            //Inventory = InventoryUtility.GetJsonDatas(items, filePath);
            foreach (var item in GameRememberObjectManager.Instance.CharacterInventoryUIPanels)
            {
                //Inventory.ChangeSlotsEvent += item.ChangeSlots;
                item.ChangeSlots(Inventory);
            }

            //Inventory.ChangeSlotsEvent += inventoryUIPanel.ChangeSlots;
            //Inventory.ChangeSlotsEvent += SaveJson;
        }
    }
}

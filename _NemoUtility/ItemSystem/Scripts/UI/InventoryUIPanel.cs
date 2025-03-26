using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIPanel : MonoBehaviour
{
    public Inventory Inventory;

    public Slot SeperateSlot;
    public List<InventoryUISlot> separateUISlots;

    public bool IsHotbar;
    public bool IsUseSkillPanel;
    public GameObject selectedImageObject;

    [SerializeField] private Transform InventoryPanelTransform;
    [SerializeField] private GameObject inventoryUISlotPrefab;

    [SerializeField] private List<InventoryUISlot> inventoryUISlots;

    public bool IsCreated = false;

    private void OnEnable()
    {
        if (Inventory != null)
        {
            ChangeSlots(Inventory);
        }
        else
        {
        }
    }

    public void ChangeSlots(Inventory _inventory)
    {
        Inventory = _inventory;
        if (IsCreated)
        {
            foreach (var uiSlot in inventoryUISlots)
            {
                uiSlot.ChangeValues();
            }
        }
        else
        {
            if (_inventory.InventorySize.x * _inventory.InventorySize.y != inventoryUISlots.Count)
            {
                //Envanterdeki tüm slotları silme
                ClearUISlots();

                foreach (var slot in _inventory.Slots)
                {
                    InventoryUISlot _uiSlot = Instantiate(inventoryUISlotPrefab, InventoryPanelTransform).GetComponent<InventoryUISlot>();
                    _uiSlot.ChangeSlot(this, slot);
                    inventoryUISlots.Add(_uiSlot);
                }
            }
            else
            {
                for (int i = 0; i < _inventory.Slots.Count; i++)
                {
                    inventoryUISlots[i].ChangeSlot(this, _inventory.Slots[i]);
                }
            }
            IsCreated = true;
        }
    }
    public void ClearUISlots()
    {
        IsCreated = false;
        foreach (var uiSlot in inventoryUISlots)
        {
            uiSlot.ActiveInfoPanel(false);
            Destroy(uiSlot.gameObject);
        }
        inventoryUISlots.Clear();
    }

    public InventoryUISlot GetInventoryUISlot(int i)
    {
        return inventoryUISlots[i];
    }


    public bool IsThereRememberSlot()
    {
        return (GameManager.Instance.LocalCharacter.RememberSlotData == null) ? false : true;
    }
}

using System;
using NemoUtility;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUISlot : MonoBehaviour
{
    public Image ItemImage;
    public Image BackgroundImage;
    public TextMeshProUGUI ItemCountText;
    public TextMeshProUGUI ItemNameText;
    public TextMeshProUGUI ItemText;
    public TextMeshProUGUI ItemLvlText;


    public Slider UseSlider;

    public Button SlotButton;
    public EventTrigger SlotButtonEventTrigger;

    public Slot Slot;

    [SerializeField] private GameObject InfoPanel;

    [SerializeField] private GameObject selectedImageObjectPrefab;

    private InventoryUIPanel inventoryUIPanel;
    //private Color beforeColor;

    public Action UpdateEvent;

    public bool IsSelect;
    private GameObject Image;
    private void OnEnable()
    {
    }
    private void OnDisable()
    {
        ActiveInfoPanel(false);
        MouseInfoManager.Instance.DeActiveImage();
    }
    private void Awake()
    {
        //beforeColor = BackgroundImage.color;
        SlotButton.onClick.AddListener((UnityEngine.Events.UnityAction)(() =>
        {
            if (Slot.Item is not null && !Slot.Item.Interactable) { return; }
            if (inventoryUIPanel.IsUseSkillPanel)
            {
                return;
            }
            if (inventoryUIPanel.IsHotbar)
            {
                GameManager.Instance.LocalCharacter.characterSelectedItemManager.SetSelectedSlotIndex(Slot.Index + 1);
                GameManager.Instance.LocalCharacter.characterSelectedItemManager.ChangeSelectedSlot();
                //Select();
                return;
            }
            if (inventoryUIPanel.IsThereRememberSlot())
            {
                Slot _rememberSlot = GameManager.Instance.LocalCharacter.RememberSlotData.RememberSlot.Slot;
                if (!Slot.MeetOneOfTheRequirementsItemTypeEnums(_rememberSlot.Item.itemTypeEnums))
                {
                    GameManager.Instance.LocalCharacter.RememberSlotData.RememberSlotInventory.ChangeValues();
                }
                else if (Slot.Item == null)
                {
                    if (inventoryUIPanel.Inventory.FindSlot(GameManager.Instance.LocalCharacter.RememberSlotData.RememberSlot.Slot))
                    {
                        inventoryUIPanel.Inventory.AddItem(Slot.Index, GameManager.Instance.LocalCharacter.RememberSlotData.RememberSlot.Slot.Item, GameManager.Instance.LocalCharacter.RememberSlotData.RememberSlot.Slot.CurrentItemCount);
                        inventoryUIPanel.Inventory.ClearSlot(GameManager.Instance.LocalCharacter.RememberSlotData.RememberSlot.Slot.Index);
                    }
                    else
                    {
                        Item tempItem = _rememberSlot.Item;
                        int tempItemCount = _rememberSlot.CurrentItemCount;
                        Slot.ChangeItem(Slot.Index, _rememberSlot.Item, _rememberSlot.CurrentItemCount);
                        _rememberSlot.Clear(_rememberSlot.Index);
                        GameManager.Instance.LocalCharacter.RememberSlotData.RememberSlotInventory.ChangeValues();
                    }
                    //inventoryUIPanel.RememberSlot.ClearSlot();
                }
                else
                {
                    if (_rememberSlot == Slot)
                    {
                        inventoryUIPanel.Inventory.ThisSlotCollect(Slot);
                    }
                    else if (_rememberSlot.Item == Slot.Item && !(_rememberSlot.IsSlotFull() || Slot.IsSlotFull()))
                    {
                        if (Slot.GetRemaningItemCount(_rememberSlot.CurrentItemCount) >= 0)
                        {
                            Slot.AddItem(_rememberSlot.CurrentItemCount);
                            _rememberSlot.Clear(_rememberSlot.Index);
                            if (!inventoryUIPanel.Inventory.FindSlot(GameManager.Instance.LocalCharacter.RememberSlotData.RememberSlot.Slot))
                            {
                                GameManager.Instance.LocalCharacter.RememberSlotData.RememberSlotInventory.ChangeValues();
                            }
                        }
                        else
                        {

                            _rememberSlot.CurrentItemCount -= Slot.NeedFullItemCount();
                            Slot.AddItem(Slot.NeedFullItemCount());
                            if (!inventoryUIPanel.Inventory.FindSlot(GameManager.Instance.LocalCharacter.RememberSlotData.RememberSlot.Slot))
                            {
                                GameManager.Instance.LocalCharacter.RememberSlotData.RememberSlotInventory.ChangeValues();
                            }
                        }

                    }
                    else
                    {
                        if (inventoryUIPanel.Inventory.FindSlot(GameManager.Instance.LocalCharacter.RememberSlotData.RememberSlot.Slot))
                        {
                            //Debug.Log(_slot.Item.Name + "  :  " + Slot.Item.Name);
                            inventoryUIPanel.Inventory.ChangeItems(_rememberSlot, Slot);
                        }
                        else
                        {
                            Item tempItem = _rememberSlot.Item;
                            int tempItemCount = _rememberSlot.CurrentItemCount;
                            _rememberSlot.ChangeItem(_rememberSlot.Index, Slot.Item, Slot.CurrentItemCount);
                            Slot.ChangeItem(Slot.Index, tempItem, tempItemCount);
                            GameManager.Instance.LocalCharacter.RememberSlotData.RememberSlotInventory.ChangeValues();
                        }
                    }

                    //inventoryUIPanel.Inventory.AddItem(Slot.Index, inventoryUIPanel.RememberSlot.Slot.Item, inventoryUIPanel.RememberSlot.Slot.CurrentItemCount);
                    //inventoryUIPanel.Inventory.AddItem(inventoryUIPanel.RememberSlot.Slot.Index, Slot.Item, Slot.CurrentItemCount);
                }
                GameManager.Instance.LocalCharacter.RememberSlotData = null;
                inventoryUIPanel.Inventory.ChangeValues();
                MouseInfoManager.Instance.DeActiveImage();
            }
            else
            {
                if (Slot.Item != null)
                {
                    GameManager.Instance.LocalCharacter.RememberSlotData = new RememberSlotData(this, inventoryUIPanel.Inventory);
                    //BackgroundImage.color = nr;
                    Image = MouseInfoManager.Instance.ActiveImage(Slot.Item.Sprite);
                    ItemImage.sprite = null;
                    ItemImage.color = new Color(0, 0, 0, 0);
                    ItemLvlText.text = "";
                    ItemCountText.text = "";
                }

            }
        }));
    }
    public void Select()
    {
        if(this is null || gameObject == null){return;}
        IsSelect = true;
        if (GameManager.Instance.LocalCharacter.selectedSlot != null)
        {
        }
        if (inventoryUIPanel.selectedImageObject == null)
        {
            inventoryUIPanel.selectedImageObject = Instantiate(selectedImageObjectPrefab, transform.parent.parent.parent);
            inventoryUIPanel.selectedImageObject.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
        }
        //selectedImageObject.transform.SetParent(transform.parent.parent.parent);
        inventoryUIPanel.selectedImageObject.transform.position = transform.position;
        GameManager.Instance.LocalCharacter.SetSelectedSlot(this);
        GameManager.Instance.LocalCharacter.characterSelectedItemManager.ChangeItem(Slot.Item);
    }
    public void DeSelect()
    {
        IsSelect = false;
    }

    void Update()
    {
        UpdateEvent?.Invoke();
    }

    public void ChangeSlot(InventoryUIPanel _inventoryUIPanel, Slot _slot)
    {
        if (Slot != null)
        {
            ChangeValues();
            return;
        }
        inventoryUIPanel = _inventoryUIPanel;

        var old_slot = Slot;

        Slot = _slot;


        if (old_slot == null)
        {
            _slot.ChangeValuesEvent += ChangeValues;
        }

        ChangeValues();
    }

    public void ChangeValues()
    {
        if (IsSelect)
        {
            Select();
        }
        ItemCountText.text = "";

        /*if (GameManager.Instance is not null && GameManager.Instance.LocalCharacter != null)
        {
            if (GameManager.Instance.LocalCharacter.selectedSlot == this) { Select(); }
        }*/


        //name = _slot.Index.ToString();
        if (Slot == null || inventoryUIPanel == null || inventoryUIPanel.Inventory == null || inventoryUIPanel.Inventory.GetIndex(Slot) == -1 || this == null || this.gameObject == null) { return; }
        name = inventoryUIPanel.Inventory.GetIndex(Slot).ToString();

        foreach (var item in Slot.requirementsItemTypeEnums)
        {
            var sp = ItemType.GetSprite(item);
            if (sp != null)
            {
                BackgroundImage.sprite = sp;
                break;
            }
        }

        if (Slot.Item == null)
        {
            ItemImage.sprite = null;
            ItemCountText.text = "";
            ItemNameText.text = "";
            ItemText.text = "";
            ItemImage.color = new Color(0, 0, 0, 0);
            ItemLvlText.text = "";
            //BackgroundImage.color = beforeColor;
            //BackgroundImage.sprite = null;
            return;
        }
        if (Slot.CurrentItemCount == 1)
        {
            ItemCountText.text = "";
        }
        else
        {
            ItemCountText.text = Slot.CurrentItemCount.ToString();
        }
        ItemImage.sprite = Slot.Item.Sprite;
        ItemImage.color = Slot.Item.SpriteColor;

        if (Slot.Item is Skill skill)
        {
            ItemLvlText.text = skill.Level.ToString();
        }


        ItemNameText.text = Slot.Item.Name;
        ItemText.text = Slot.Item.Text;
        UseSlider.maxValue = 100;
        UseSlider.value = Slot.Item.ItemUseTimer;
        //ItemImage.color = new Color(1, 1, 1, 1);
        //BackgroundImage.color = beforeColor;
    }

    public void ClearSlot()
    {
        //name = Slot.Index.ToString();
        name = inventoryUIPanel.Inventory.GetIndex(Slot).ToString();
        ItemImage.sprite = null;
        ItemCountText.text = "";
        ItemImage.color = new Color(0, 0, 0, 0);
        //BackgroundImage.color = beforeColor;
        ItemLvlText.text = "";
    }

    public void ActiveInfoPanel(bool active)
    {
        if (Slot.Item == null) { return; }
        if (active)
        {
            var t = ItemRareType.GetItemRareType(Slot.Item.ItemRareTypesEnum);
            string s = $"<color={t.ColorTag}>" + Slot.Item.NameLocalizedString.GetLocalizedString() + $"({ItemRareType.GetLocalizedString(t.ItemRareTypesEnum).GetLocalizedString()})" + "\n";
            if (!String.IsNullOrEmpty(Slot.Item.GetInfo()))
            {
                s += "<color=red>" + Slot.Item.GetInfo();
            }
            /*
            if (Slot.Item is Skill skill)
            {
                s += "<color=blue>" + "Lvl:" + skill.Level + "\n";
                s += "<color=green>" + "Next Lvl:" + skill.LevelCurrentNextLevelXp.ToString("F1") + "/" + skill.CurrentXp.ToString("F1") + "\n";
            }*/


            MouseInfoManager.Instance.Active(s, Color.black);
        }
        else
        {
            MouseInfoManager.Instance.DeActive();
        }

        //InfoPanel.SetActive(active);
    }

    public void FirstSerateControl()
    {

        if (inventoryUIPanel.separateUISlots.Count > 0) { return; }
        if (inventoryUIPanel.IsThereRememberSlot())
        {

            if (GameManager.Instance.LocalCharacter.RememberSlotData.RememberSlot.Slot.CurrentItemCount == 1) { return; }
            inventoryUIPanel.separateUISlots.Add(this);
        }
    }
    public void AddSeparateUISlots()
    {
        if (Slot.Item != null) { return; }
        if (inventoryUIPanel.separateUISlots.Count < 1) { return; }
        if (inventoryUIPanel.IsThereRememberSlot() || inventoryUIPanel.SeperateSlot != null)
        {
            if (!inventoryUIPanel.separateUISlots.Contains(this))
            {
                inventoryUIPanel.separateUISlots.Add(this);

                if (inventoryUIPanel.IsThereRememberSlot())
                {
                    Slot _rememberSlot = GameManager.Instance.LocalCharacter.RememberSlotData.RememberSlot.Slot;
                    inventoryUIPanel.SeperateSlot = new Slot(0, _rememberSlot.Item, _rememberSlot.CurrentItemCount);
                    _rememberSlot.Clear(_rememberSlot.Index);
                    GameManager.Instance.LocalCharacter.RememberSlotData.RememberSlotInventory.ChangeValues();
                    GameManager.Instance.LocalCharacter.RememberSlotData = null;
                    SyncSeperate(inventoryUIPanel.SeperateSlot);
                }
                else if (inventoryUIPanel.SeperateSlot != null)
                {
                    SyncSeperate(inventoryUIPanel.SeperateSlot);
                }

            }
        }
    }

    private void SyncSeperate(Slot _slot)
    {
        int _itemcount = _slot.CurrentItemCount / inventoryUIPanel.separateUISlots.Count;
        int _itemCountFinishSlot = _itemcount + (_slot.CurrentItemCount % inventoryUIPanel.separateUISlots.Count);
        if (_itemcount == 0) { FinishSeperate(); }
        for (int i = 0; i < inventoryUIPanel.separateUISlots.Count; i++)
        {
            if (i == inventoryUIPanel.separateUISlots.Count - 1)
            {
                inventoryUIPanel.separateUISlots[i].Slot.ChangeItem(_slot.Item, _itemCountFinishSlot);
            }
            else
            {
                inventoryUIPanel.separateUISlots[i].Slot.ChangeItem(_slot.Item, _itemcount);
            }
        }
        inventoryUIPanel.Inventory.ChangeValues();
    }

    public void FinishSeperate()
    {
        if (inventoryUIPanel.IsThereRememberSlot() || inventoryUIPanel.separateUISlots.Count > 1)
        {
            inventoryUIPanel.SeperateSlot = null;

            inventoryUIPanel.separateUISlots.Clear();
            MouseInfoManager.Instance.DeActiveImage();
            inventoryUIPanel.Inventory.ChangeValues();
        }

    }

    public void PointerDown()
    {
        if (inventoryUIPanel.IsUseSkillPanel && Slot != null && Slot.IsSlotUsed())
        {
            var sm = GameManager.Instance.LocalCharacter.CharacterSkillManager;
            sm.SkillDown(sm.Skills[Slot.Index], Slot.Index);
            UpdateEvent += PointerStay;
            Debug.Log("PointerDown");
        }
    }
    public void PointerStay()
    {
        if (inventoryUIPanel.IsUseSkillPanel && Slot != null && Slot.IsSlotUsed())
        {
            var sm = GameManager.Instance.LocalCharacter.CharacterSkillManager;
            sm.SkillUpdate(sm.Skills[Slot.Index], Slot.Index);
            Debug.Log("PointerStay");
        }
    }
    public void PointerUp()
    {
        if (inventoryUIPanel.IsUseSkillPanel && Slot != null && Slot.IsSlotUsed())
        {
            var sm = GameManager.Instance.LocalCharacter.CharacterSkillManager;
            sm.SkillUp(sm.Skills[Slot.Index], Slot.Index);
            UpdateEvent -= PointerStay;
            Debug.Log("PointerUp");
        }
    }
}

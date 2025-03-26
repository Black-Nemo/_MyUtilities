using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Localization;

public enum ItemShowType { Model, Sprite }

[CreateAssetMenu(fileName = "new Item", menuName = "Item/Default")]
public class Item : ScriptableObject
{
    [HideInInspector] public bool IsClone;
    public List<ItemTypeEnum> itemTypeEnums = new List<ItemTypeEnum>();
    public ItemRareTypesEnum ItemRareTypesEnum;
    public string Name;
    public string Info;
    public string Text;
    public int StackMaxCount;
    public int MaxRandomSpawnValue = 1;

    [Header("Price")]
    public int BuyPrice;
    public int SellPrice;
    [Space]
    public Sprite Sprite;
    public UnityEngine.Color SpriteColor = new UnityEngine.Color(1, 1, 1, 1);
    public GameObject ItemPrefab;
    public ItemShowType ItemShowType = ItemShowType.Sprite;

    public bool Interactable = true;
    public float ItemUseTimer = 0f;

    public List<Feature> features = new List<Feature>();

    public LocalizedString NameLocalizedString;

    public void AddAllFeatures(Character character)
    {
        features.ForEach((a) => { a.AddFeature(character); });
    }
    public void RemoveAllFeatures(Character character)
    {
        features.ForEach((a) => { a.RemoveFeature(character); });
    }

    public virtual string GetInfo()
    {
        if (String.IsNullOrEmpty(Info))
        {
            return GetFeaturesInfo();
        }

        return Info;
    }

    public string GetFeaturesInfo()
    {
        string res = "";
        foreach (var ftr in features)
        {
            res += ftr.GetInfo();
        }
        return res;
    }
}

[System.Serializable]
public class ItemJson
{
    public string Name;

    public ItemJson(string _name)
    {
        Name = _name;
    }
    public ItemJson()
    {
        Name = "";
    }
}
public enum ItemTypeEnum
{
    Default = 0,
    Helmet = 1,
    Chestplate = 2,
    Legging = 3,
    Boot = 4,
    Food = 5,
    Skill = 6,
    Weapon = 7,
    MusicKit = 8,
    Arrow = 9
}
[System.Serializable]
public class ItemTypeEnums
{
    public List<ItemTypeEnum> ItemTypeEnumList;
}
[System.Serializable]
public class ItemType
{
    public static List<ItemType> ItemTypes
    {
        get
        {
            return RememberPrefabManager.Instance.ItemTypes;
        }
    }
    public ItemTypeEnum ItemTypeEnum;
    public Sprite BackgroundSprite;
    public ItemType(ItemTypeEnum itemTypeEnum, Sprite backgroundSprite)
    {
        ItemTypeEnum = itemTypeEnum;
        BackgroundSprite = backgroundSprite;
    }

    public static Sprite GetSprite(ItemTypeEnum itemTypeEnum)
    {
        var temp = ItemTypes.Find((a) => a.ItemTypeEnum == itemTypeEnum);
        if (temp == null)
        {
            return null;
        }
        else
        {
            return temp.BackgroundSprite;
        }
    }
}
public class ItemRareType
{
    public static List<ItemRareType> RareTypes;
    public int Id;
    public string Name;
    public ItemRareTypesEnum ItemRareTypesEnum;
    public double Rarity;
    public UnityEngine.Color RareColor;
    public string ColorTag;

    public static ItemRareType Common = new ItemRareType()
    {
        Id = 0,
        Name = "Common",
        ItemRareTypesEnum = ItemRareTypesEnum.Common,
        Rarity = 100,
        RareColor = NemoUtility.ColorUtility.ConvertColor(System.Drawing.Color.DarkGray),
        ColorTag = "white",
    };


    public static ItemRareType Rare = new ItemRareType()
    {
        Id = 1,
        Name = "Rare",
        ItemRareTypesEnum = ItemRareTypesEnum.Rare,
        Rarity = 25,
        RareColor = NemoUtility.ColorUtility.ConvertColor(System.Drawing.Color.Orange),
        ColorTag = "orange",
    };

    public static ItemRareType Epic = new ItemRareType()
    {
        Id = 2,
        Name = "Epic",
        ItemRareTypesEnum = ItemRareTypesEnum.Epic,
        Rarity = 10,
        RareColor = NemoUtility.ColorUtility.ConvertColor(System.Drawing.Color.Purple),
        ColorTag = "purple",
    };

    public static ItemRareType Legendary = new ItemRareType()
    {
        Id = 3,
        Name = "Legendary",
        ItemRareTypesEnum = ItemRareTypesEnum.Legendary,
        Rarity = 1,
        RareColor = NemoUtility.ColorUtility.ConvertColor(System.Drawing.Color.Yellow),
        ColorTag = "yellow",
    };


    static ItemRareType()
    {
        RareTypes = new List<ItemRareType>
        {
            Legendary,
            Epic,
            Rare ,
            Common,
        };
    }

    public static LocalizedString GetLocalizedString(ItemRareTypesEnum val)
    {
        switch (val)
        {
            case ItemRareTypesEnum.Common:
                return RememberPrefabManager.Instance.CommonLocalizedString;
            case ItemRareTypesEnum.Rare:
                return RememberPrefabManager.Instance.RareLocalizedString;
            case ItemRareTypesEnum.Epic:
                return RememberPrefabManager.Instance.EpicLocalizedString;
            case ItemRareTypesEnum.Legendary:
                return RememberPrefabManager.Instance.LegendaryLocalizedString;
        }
        return null;
    }

    public static ItemRareType GetItemRareType(ItemRareTypesEnum val)
    {
        return RareTypes.Find((a) => a.ItemRareTypesEnum == val);
    }

    public static ItemRareTypesEnum GetItemRareTypesEnum(double value)
    {
        double val = 0;
        for (int i = 0; i < RareTypes.Count; i++)
        {
            val += RareTypes[i].Rarity;
            if (RareTypes.Count - i == 1)
            {
                return RareTypes[i].ItemRareTypesEnum;
            }
            else if (value <= val)
            {
                return RareTypes[i].ItemRareTypesEnum;
            }
        }
        return ItemRareTypesEnum.Common;
    }
}
public enum ItemRareTypesEnum
{
    Common, Rare, Epic, Legendary
}
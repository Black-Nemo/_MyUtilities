using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "BowWeaponItem", menuName = "Item/BowWeaponItem")]
public class BowWeaponItem : WeaponItem
{
    public float MaxSpeed;
    public float MaxFireForceTime;
    public float MaxAngle;

    public AudioClip BowStretchingClip;
    public AudioClip BowFireClip;

    

    public override string GetInfo()
    {
        string result = "";
        if (MaxSpeed > 0) { result += $"{RememberPrefabManager.Instance.MaxSpeedLocalizedString.GetLocalizedString()}: {MaxSpeed} \n"; }

        return base.GetInfo() + result;
    }
}

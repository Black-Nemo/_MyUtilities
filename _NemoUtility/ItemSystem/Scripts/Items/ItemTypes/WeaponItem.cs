using UnityEngine;

[CreateAssetMenu(fileName = "WeaponItem", menuName = "Item/WeaponItem")]
public class WeaponItem : Item
{
    public Damage Damage;

    [Space]
    public string ATTACK_ANIMATION_NAME;
    public float CoolDownTime;

    public bool CanEnableOutline;

    public override string GetInfo()
    {
        string result = "";
        result += Damage.GetInfo();

        return result;
    }
}
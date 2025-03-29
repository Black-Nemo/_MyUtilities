using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "BulletItem", menuName = "Item/BulletItem")]
public class BulletItem : Item
{
    public float Speed;
    public Damage Damage;


    public override string GetInfo()
    {
        string result = "";
        result += Damage.GetInfo();
        if (Speed > 0) { result += $"{RememberPrefabManager.Instance.SpeedLocalizedString.GetLocalizedString()}: +{Speed} \n"; }
        result += base.GetInfo();

        return result;
    }
}
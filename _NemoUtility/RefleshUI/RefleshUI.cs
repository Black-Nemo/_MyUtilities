using UnityEngine;
using UnityEngine.UI;

public class RefleshUI : MonoBehaviour
{
    public void RefreshUI(RectTransform targetTransform)
    {
        // Hiyerarşideki yerleşimleri anında hesapla
        LayoutRebuilder.ForceRebuildLayoutImmediate(targetTransform);
    }
}

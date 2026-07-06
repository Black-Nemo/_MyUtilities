using UnityEngine;

public class UIHierarchyProxy : MonoBehaviour
{
    public GameObject targetElement; // Kopardığımız asıl UI objesi

    // Unity hiyerarşisinde herhangi bir üst ebeveyn kapandığında bu OTOMATİK tetiklenir
    private void OnEnable()
    {
        if (targetElement != null) targetElement.SetActive(true);
    }

    private void OnDisable()
    {
        if (targetElement != null) targetElement.SetActive(false);
    }

    private void OnDestroy()
    {
        if (targetElement != null) Destroy(targetElement);
    }
}
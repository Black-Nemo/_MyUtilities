using UnityEngine;

public class MobileSetActive : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(Application.isMobilePlatform);
    }
}

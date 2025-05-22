using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VersionText : MonoBehaviour
{
    private void Start()
    {
        GetComponent<TextMeshProUGUI>().text = Application.version;
    }
}

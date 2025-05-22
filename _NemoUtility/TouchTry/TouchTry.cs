using System.Collections.Generic;
using System.Linq;
using NemoUtility;
using UnityEngine;
using UnityEngine.InputSystem;

public class TouchTry : MonoBehaviour
{
    public List<MouseInfo> mouseInfos = new List<MouseInfo>();

    public static TouchTry Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {

        if (Touchscreen.current == null) return;

        var activeTouches = Touchscreen.current.touches
            .Where(t => t.press.isPressed || t.press.wasPressedThisFrame || t.press.wasReleasedThisFrame)
            .ToList();

        // Ayarla mouseInfos sayısını
        while (mouseInfos.Count < activeTouches.Count)
        {
            mouseInfos.Add(MouseInfoManager.Instance.ActiveNotDestroy("null", Color.black));
        }
        while (mouseInfos.Count > activeTouches.Count)
        {
            var d = mouseInfos[mouseInfos.Count - 1];
            mouseInfos.RemoveAt(mouseInfos.Count - 1);
            if (d != null) Destroy(d.gameObject);
        }

        for (int i = 0; i < activeTouches.Count; i++)
        {
            try
            {
                var touch = activeTouches[i];
                mouseInfos[i].Text.text = touch.touchId.ReadValue().ToString();
                Vector2 pos = touch.position.ReadValue();
                mouseInfos[i].transform.position = new Vector3(pos.x, pos.y + 100, 0);
            }
            catch (System.Exception e)
            {
                if (mouseInfos[i] != null)
                {
                    Destroy(mouseInfos[i].gameObject);
                }
                Debug.LogError(e);
            }
        }
    }
}

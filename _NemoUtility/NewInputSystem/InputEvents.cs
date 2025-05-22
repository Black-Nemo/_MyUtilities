using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;

public class InputEvents : OnScreenButton
{

    public void OnPointerUp(PointerEventData eventData)
    {
        return;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        return;
    }

    public void KeyDown()
    {
        SendValueToControl(1.0f);
    }

    public void KeyUp()
    {
        SendValueToControl(0.0f);
    }
}

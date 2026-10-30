using UnityEngine;
using UnityEngine.EventSystems;

public class PressButton : MonoBehaviour, IPointerDownHandler
{
    private bool pressed;

    public void OnPointerDown(PointerEventData eventData)
    {
        pressed = true;
    }

    public bool ConsumePress()
    {
        bool result = pressed;
        pressed = false;
        return result;
    }

    private void OnDisable()
    {
        pressed = false;
    }
}

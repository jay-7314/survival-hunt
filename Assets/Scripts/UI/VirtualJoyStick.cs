using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoyStick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform handle;
    [SerializeField] private float radius = 100f;

    public Vector2 Direction { get; private set; }

    public void OnPointerDown(PointerEventData eventData)
    {
        Move(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Move(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ResetStick();
    }

    private void OnDisable()
    {
        ResetStick();
    }

    private void Move(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(background, eventData.position, eventData.pressEventCamera, out Vector2 local);
        Vector2 offset = Vector2.ClampMagnitude(local, radius);     //손가락이 멀리가도 100까지만 가두기
        handle.anchoredPosition = offset;
        Direction = offset / radius;
    }

    private void ResetStick()
    {
        Direction = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }
    
}

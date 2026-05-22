using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class drag_n_drop_component : EventTrigger
{
    private bool dragging = false;
    void Update()
    {
        if (dragging)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        dragging = true;
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        dragging = false;
    }
}

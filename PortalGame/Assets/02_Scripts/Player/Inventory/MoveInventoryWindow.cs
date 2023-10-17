using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveInventoryWindow : MonoBehaviour
    , IDragHandler
    /*
    , IPointerClickHandler
    , IEndDragHandler
    , IPointerEnterHandler
    , IPointerExitHandler
    */
{
    public RectTransform parentRect;


    public void OnDrag(PointerEventData eventData)
    {
        Rect rect = GetComponent<RectTransform>().rect;
        float halfWidth = rect.width / 2;
        float halfHeight = rect.height / 2;

        eventData.pointerDrag.transform.position = new Vector2(eventData.position.x, eventData.position.y - halfHeight + 50);
        float calcX = Mathf.Clamp(eventData.pointerDrag.transform.position.x, halfWidth, parentRect.rect.width - halfWidth);
        float calcY = Mathf.Clamp(eventData.pointerDrag.transform.position.y, halfHeight, parentRect.rect.height - halfHeight);

        eventData.pointerDrag.transform.position = new Vector2(calcX, calcY);
    }

}

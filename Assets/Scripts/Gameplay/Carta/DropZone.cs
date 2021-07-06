using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;
        DragDrop d = eventData.pointerDrag.GetComponent<DragDrop>();
        if (d != null)
        {
            d.placeHolderParent = this.transform;
        }

    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;
        DragDrop d = eventData.pointerDrag.GetComponent<DragDrop>();
        if (d != null &&  d.placeHolderParent == this.transform)
        {
            d.placeHolderParent = d.handReturn;
        }

    }
    public void OnDrop(PointerEventData eventData)
    {
        DragDrop d = eventData.pointerDrag.GetComponent<DragDrop>();
        if (d != null)
        {
            d.handReturn = this.transform;
        }

    }
}

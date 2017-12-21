using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler , IPointerEnterHandler,IPointerExitHandler{   
    public Transform codeBlockCase;
    public void OnPointerEnter(PointerEventData eventData) {
        if (eventData.pointerDrag == null)
            return;
        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if(d!=null)
        d.placeholderParent = this.transform;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;
        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if (d != null && d.placeholderParent == this.transform) 
            d.placeholderParent = d.ReturnParent;
    }
    public void OnDrop(PointerEventData eventData) {
        Debug.Log(eventData.pointerDrag.name + "was Drop to" + gameObject.name);
        if (eventData.pointerDrag.GetComponent<Draggable>() != null) {
            Debug.Log("Not Null");
            if (codeBlockCase == eventData.pointerDrag.transform.parent)
            {
                Debug.Log("transfom");
                Instantiate(eventData.pointerDrag.GetComponent<Draggable>().Type, this.transform, false);

            }
            else {

                Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
                d.ReturnParent = this.transform;

            }
           
        }
    }

}

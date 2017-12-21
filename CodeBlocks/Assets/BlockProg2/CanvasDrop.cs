using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CanvasDrop : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
   

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerDrag.gameObject.name + " was dropped to " + this.gameObject.name);
        CodeBlockDrag d = eventData.pointerDrag.GetComponent<CodeBlockDrag>();
        if(d!=null)
        d.returnParent = this.transform;
        //eventData.pointerDrag.transform.SetParent(this.transform);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
       // throw new NotImplementedException();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
       // throw new NotImplementedException();
    }
}

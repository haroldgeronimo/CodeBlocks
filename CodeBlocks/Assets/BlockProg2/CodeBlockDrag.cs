using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CodeBlockDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    public Transform CodeBuilder;
    public Transform CodeCanvas;
    public Transform placeholderParent;
    public Transform returnParent;
    public GameObject placeholder;
    public bool IsParentable ;
    public int SiblingI=0;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Input.touchCount > 1)
        {
            OnEndDrag(eventData);
            return;
        }
        placeholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());
        //SiblingI = this.transform.GetSiblingIndex();
        returnParent = this.transform.parent;
        placeholderParent = returnParent;
        this.transform.SetParent(CodeBuilder);
        if (returnParent.tag == "content" || returnParent.tag == "truecontent" || returnParent.tag == "falsecontent")
                if (returnParent.GetComponent<ContentDrop>().GetAllChild().Count <= 1)
                    returnParent.GetComponent<ContentDrop>().PlaceholdersetActive();//activates place holder when all content leaves
 
            this.GetComponent<CanvasGroup>().blocksRaycasts = false;
     
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (Input.touchCount > 1)
        {
            OnEndDrag(eventData);
            return;
        }

        this.transform.position = eventData.position;

            int newSbIndex = placeholderParent.childCount;
            for (int i = 0; i < placeholderParent.childCount; i++)
            {
                if (this.transform.position.y > placeholderParent.GetChild(i).position.y)
                {
                    newSbIndex = i;
                    if (placeholder.transform.GetSiblingIndex() < newSbIndex)
                        newSbIndex--;

                    break;
                }
            }
            placeholder.transform.SetSiblingIndex(newSbIndex);
      
    }
    public void OnEndDrag(PointerEventData eventData) {
      
            Debug.Log("Set to sibling " + this.transform.GetSiblingIndex());
            this.transform.SetParent(returnParent);
            //   placeholder.transform.SetAsLastSibling();
            this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());//returns a object to its placeholder

            this.GetComponent<CanvasGroup>().blocksRaycasts = true;
       
    }

 



}

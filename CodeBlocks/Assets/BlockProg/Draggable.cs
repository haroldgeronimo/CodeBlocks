using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler{
    GameObject duplicate;
    public Transform CodeCase;
    public GameObject Type;
    public Transform placeholderParent;
    public Transform ReturnParent;
    GameObject placeholder = null;
    public void OnBeginDrag(PointerEventData eventData) {
        if (this.transform.parent != CodeCase) //dragging an already created object
        {
            BeginDragExisting();
        }
        else   //duplicating object
        {
            Debug.Log("Drag a Instantiated Object");
            duplicate = Instantiate(this.gameObject, CodeCase, false);
            duplicate.transform.position = eventData.position;

            CodeCase = duplicate.transform.parent;
            duplicate.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }


    }
    public void OnDrag(PointerEventData eventData)
    {
        if (transform.parent != CodeCase)//dragging an already created object
        {
            OnDragExisting(eventData);
        }
        else
        {
            duplicate.transform.position = eventData.position;
            CodeCase = duplicate.transform.parent;
        }
    
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (this.transform.parent != CodeCase)
        {
            EndDragExisting();
        }
        else {
            CodeCase = duplicate.transform.parent;
            duplicate.GetComponent<CanvasGroup>().blocksRaycasts = true;
            Destroy(duplicate);

            
        }

    }
    
    void BeginDragExisting() {
        Debug.Log("Drag a created Object");
        placeholder = new GameObject();
        placeholder.transform.SetParent(this.transform.parent);
        //   placeholder.AddComponent<RectTransform>();
        //RectTransform rt = placeholder.GetComponent<RectTransform>();
        // rt.sizeDelta = new Vector2(0f, 27.47f);

        LayoutElement le = placeholder.AddComponent<LayoutElement>();
        le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
        le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
        le.flexibleHeight = 0;
        le.flexibleWidth = 0;
        placeholder.GetComponent<RectTransform>().sizeDelta = new Vector2(le.preferredWidth, le.preferredHeight);
        placeholder.transform.SetSiblingIndex(transform.GetSiblingIndex());

        ReturnParent = this.transform.parent;
        placeholderParent = ReturnParent;
        this.transform.SetParent(this.transform.parent.parent);
        this.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    void OnDragExisting(PointerEventData eventData) {

        this.transform.position = eventData.position;
        if (placeholder.transform != placeholderParent)
            placeholder.transform.SetParent(placeholderParent);

        int newSiblingIndex = placeholderParent.childCount;

        for (int i = 0; i < placeholderParent.childCount; i++)
        {
            if (transform.position.y < placeholderParent.GetChild(i).position.y)
            {
                newSiblingIndex = i;
                if (placeholder.transform.GetSiblingIndex() < newSiblingIndex)
                    newSiblingIndex--;

                break;
            }
        }


        placeholder.transform.SetSiblingIndex(newSiblingIndex);
    }
    void EndDragExisting()
    {
        this.transform.SetParent(ReturnParent);
        this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());

        this.GetComponent<CanvasGroup>().blocksRaycasts = true;
        Destroy(placeholder);
    }
}

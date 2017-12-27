using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ContentDrop : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler//,IPointerUpHandler
{
    public List<GameObject> contents;
    public GameObject placeholder;
    public Color phDefaultColor;
    public Vector2 phDefaultSize;
    private bool NormalState = true;

    private void Start()
    {
        placeholder = this.transform.GetChild(0).gameObject;
        phDefaultColor = placeholder.GetComponent<Image>().color;
        phDefaultSize = placeholder.GetComponent<RectTransform>().sizeDelta;
    }
    public void OnPointerEnter(PointerEventData eventData) {
        if (eventData.pointerDrag == null) { Debug.Log("null pointer drag"); return; }
        if (eventData.pointerDrag.gameObject.tag != "decision"
        && eventData.pointerDrag.gameObject.tag != "repeat"
        && eventData.pointerDrag.gameObject.tag != "codeblock") { Debug.Log("Incorrect tag: " + eventData.pointerDrag.gameObject.tag); return; }
        if ((eventData.pointerDrag != null || eventData.pointerDrag.gameObject != this.gameObject) && eventData.pointerDrag.GetComponent<CodeBlockDrag>() != null)
        {
            Debug.Log("codeblockdrag does not equal null"); 
            CodeBlockDrag cbd = eventData.pointerDrag.GetComponent<CodeBlockDrag>();
            if (cbd != null)
                cbd.placeholderParent = this.transform;

            eventData.pointerDrag.GetComponent<CodeBlockDrag>().placeholder = placeholder;
            contents.Clear();
            contents = GetAllChild();
            if (contents.Count > 1)
            {
                //Set placeholder to active
                TogglePlaceholderActive(true, eventData.pointerDrag.GetComponent<RectTransform>().sizeDelta, phDefaultColor);
                //set sibling index to nearest the mouse pointer
                //--- here---//
                //
            }
            else
            {
                //highlight and resize to fit
                TogglePlaceholderActive(false, eventData.pointerDrag.GetComponent<RectTransform>().sizeDelta, phDefaultColor);

            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;
        if ((eventData.pointerDrag != null || eventData.pointerDrag.gameObject != this.gameObject) && eventData.pointerDrag.GetComponent<CodeBlockDrag>() != null)
        {
            CodeBlockDrag cbd = eventData.pointerDrag.GetComponent<CodeBlockDrag>();
            if (cbd != null && cbd.placeholderParent ==this.transform)
                cbd.placeholderParent = cbd.returnParent;

            contents.Clear();
            contents = GetAllChild();
            if (contents.Count > 1)
            {
                //Set placeholder to active
                TogglePlaceholderDeActive(true, phDefaultSize, phDefaultColor);
                //set sibling index to nearest the mouse pointer
                //--- here---//
                //
            }
            else
            {
                //highlight and resize to fit
                TogglePlaceholderDeActive(false, phDefaultSize, phDefaultColor);

            }

        }
     
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;
        if (eventData.pointerDrag.gameObject.tag != "decision"
           && eventData.pointerDrag.gameObject.tag != "repeat"
           && eventData.pointerDrag.gameObject.tag != "codeblock") return;
        //parent here
        // int siblingIndex = placeholder.transform.GetSiblingIndex();
        CodeBlockDrag cbd = eventData.pointerDrag.GetComponent<CodeBlockDrag>();
        if (cbd != null)
            cbd.returnParent = this.transform;
        // placeholder.transform.SetAsLastSibling();
        //     eventData.pointerDrag.transform.SetSiblingIndex(siblingIndex);


       TogglePlaceholderDeActive(true, phDefaultSize, phDefaultColor);
      
    }


    public List<GameObject> GetAllChild()
    {
   
        List<GameObject> gamecon = new List<GameObject>();
       // Debug.Log("CHILD COUNT! " + this.transform.childCount);
        //int count = this.transform.childCount;

        for(int i = 0; i < transform.childCount; i++)
        {
            gamecon.Add(this.transform.GetChild(i).gameObject);
        }
        return gamecon;
    }

    public void TogglePlaceholderActive(bool withActive,Vector2 size, Color color)
    {
        //set to highlighted
        //SETS HIGHLIGHT

        // DisableAllPlaceholders();

        Color newColor =  placeholder.GetComponent<Image>().color;
            newColor.a = 255;
            placeholder.GetComponent<Image>().color = newColor;
            //SETS SIZE
            placeholder.GetComponent<RectTransform>().sizeDelta = size;
            if(withActive)
            placeholder.SetActive(true);

        DisableLineagePlaceholder();

    }

    public void TogglePlaceholderDeActive(bool withActive, Vector2 size, Color color)
    {
       //set to normal state
         //SETS HIGHLIGHT
            Color newColor = placeholder.GetComponent<Image>().color = color;
            //SETS SIZE
            placeholder.GetComponent<RectTransform>().sizeDelta = size;
            if (withActive)
                placeholder.SetActive(false);


    }

    void DisableLineagePlaceholder()
    {
        Transform t = this.transform.parent;
        if(t.parent != null || t.parent.parent.tag == "codecanvas" )
        while(t.parent.tag != "codecanvas")
        {
            if(t.tag == "content")
            {
                if(t.childCount>1)
                t.GetComponent<ContentDrop>().TogglePlaceholderDeActive(true,phDefaultSize,phDefaultColor);
                else
                    t.GetComponent<ContentDrop>().TogglePlaceholderDeActive(false, phDefaultSize, phDefaultColor);
            }

            t = t.parent.transform;
            Debug.Log("goes to parent");
        }
        Debug.Log("ends while");

    }
    private void LateUpdate()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());
        
    }

    public void PlaceholdersetActive()
    {
        placeholder.SetActive(true);
    }



    public void DisableAllPlaceholders()
    {
        GameObject[] pholders = GameObject.FindGameObjectsWithTag("placeholder");
        if (pholders.Length <= 0) return;
        foreach (GameObject ph in pholders)
        {
            if (ph.GetComponentInParent<Transform>().childCount > 1)
            {
                ph.SetActive(false);
            }
            else
            {
                ph.GetComponentInParent<ContentDrop>().TogglePlaceholderDeActive(false, phDefaultSize, phDefaultColor);
            }
        }
    }
    #region Commented
    // public bool IsOverContent;
    // public GameObject placeholder;
    // public List<GameObject> contents;
    //// public Canvas canvas;
    //  GameObject existingPlaceholder;

    // public void OnPointerEnter(PointerEventData eventData)
    // {
    //     if (eventData.pointerDrag != null && eventData.pointerDrag.GetComponent<CodeBlockDrag>() != null)
    //     {
    //         //size of the object being dragged
    //         float width = eventData.pointerDrag.gameObject.GetComponent<RectTransform>().rect.width;
    //         float height = eventData.pointerDrag.gameObject.GetComponent<RectTransform>().rect.height;

    //         //gets all child
    //         contents = GetAllChild();
    //         Debug.Log("Entered");

    //         //checks if there is content
    //         if (contents.Count > 1)
    //         {
    //             //checks if there is a placeholder
    //             existingPlaceholder = checkPlaceholder();

    //             if (existingPlaceholder != null)
    //             { //if true hightlight and resize;
    //                 Outline ot = existingPlaceholder.GetComponent<Outline>();
    //                 existingPlaceholder.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
    //                 // existingPlaceholder.transform.parent.GetComponent<VerticalLayoutGroup>().childForceExpandHeight = true;
    //                 //existingPlaceholder.transform.parent.GetComponent<VerticalLayoutGroup>().childForceExpandHeight = false;
    //                 ot.enabled = true;
    //             }
    //             else
    //             {
    //                 //if none create and highlight
    //                 Instantiate(placeholder, this.transform, false);
    //                 existingPlaceholder = checkPlaceholder();
    //                 Outline ot = existingPlaceholder.GetComponent<Outline>();
    //                 existingPlaceholder.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
    //                 ot.enabled = true;
    //             }
    //         }
    //     }
    // }

    // public void OnPointerExit(PointerEventData eventData)
    // {
    //     // if you are dragging something when you leave
    //     if (eventData.pointerDrag != null)
    //     {
    //         //check if there is a place holder
    //         existingPlaceholder = checkPlaceholder();
    //         if (existingPlaceholder != null)
    //         {
    //             //if there is a placeholder
    //             // check if it is the only item in the heirarchy
    //             contents = GetAllChild();
    //             if(contents.Count == 1)
    //             {//if true revert placeholder to old state
    //                 Outline ot = existingPlaceholder.GetComponent<Outline>();
    //                 existingPlaceholder.GetComponent<RectTransform>().sizeDelta = new Vector2(230, 50);
    //                 ot.enabled = false;
    //             }
    //             else
    //             {//if not true destroy the placeholder

    //                 destroyPlaceHolders();

    //             }



    //         }
    //     }
    //   //  Canvas.ForceUpdateCanvases();
    // }

    // public void OnPointerUp(PointerEventData eventData)
    // {
    //     //// check if there is a placeholder if true revert the placeholder
    //     //if (existingPlaceholder != null)
    //     //{
    //     //    Outline ot = existingPlaceholder.GetComponent<Outline>();
    //     //    existingPlaceholder.GetComponent<RectTransform>().sizeDelta = new Vector2(230, 50);

    //     //    //    existingPlaceholder.transform.parent.GetComponent<VerticalLayoutGroup>().childForceExpandHeight = true;
    //     //    //    existingPlaceholder.transform.parent.GetComponent<VerticalLayoutGroup>().childForceExpandHeight = false;
    //     //    ot.enabled = false;
    //     //}
    // }


    // public void OnDrop(PointerEventData eventData)
    // {
    //     Debug.Log(eventData.pointerDrag.gameObject.name + " was dropped to " + this.gameObject.name);
    //     //gets the codeblockdrag script of a dropped object
    //     CodeBlockDrag d = eventData.pointerDrag.GetComponent<CodeBlockDrag>();
    //     //sets the return parent as this parent
    //     d.returnParent = this.transform;


    //     //checks if there is a placecholder and deletes them
    //     existingPlaceholder = checkPlaceholder();
    //     if (existingPlaceholder != null)
    //     {
    //         destroyPlaceHolders();
    //     }


    // }


    // private void Update()
    // {
    //     if (this.transform.childCount <= 0)
    //     {
    //         Instantiate(placeholder, transform, false);
    //         LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());
    //         Canvas.ForceUpdateCanvases();
    //     }
    // }

    // private void LateUpdate()
    // {

    //     LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());
    // }



    // private List<GameObject> GetAllChild()
    // {
    //     Transform[] transcon;
    //     List<GameObject> gamecon = new List<GameObject>();
    //     Debug.Log("CHILD COUNT! " + this.transform.childCount);
    //     //int count = this.transform.childCount;

    //     transcon = this.gameObject.GetComponentsInChildren<Transform>();
    //     foreach(Transform con in transcon)
    //     {
    //         gamecon.Add(con.gameObject);
    //     }
    //     return gamecon;
    // }
    // private GameObject checkPlaceholder()
    // {
    //     if (contents.Count == 0)
    //         return null;
    //     foreach(GameObject content in contents)
    //     {
    //         if (content.tag == "placeholder")
    //         {
    //             return content;
    //         }
    //     }
    //     return null;
    // }
    // private void destroyPlaceHolders() {
    //     contents = GetAllChild();
    //     foreach (GameObject content in contents)
    //     {
    //         if (content.tag == "placeholder")
    //         {
    //             Destroy(content);
    //         }
    //     }
    // }
    #endregion

}

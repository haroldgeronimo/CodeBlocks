using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using CodeBlocks;
//using UnityEditor;
public class CodeCatalog : MonoBehaviour,IBeginDragHandler, IPointerDownHandler, IDragHandler, IEndDragHandler ,IDropHandler
{
    public GameObject codeprefab;
    public Transform CodeCanvas;
    public Transform BlockCodeUI;
    public GameObject placeholder;


    public GameObject droppedFunction;
    public new string tag;
    private Vector2 scale;
    public ActionStates ActStat;
    GameObject codeblk;

    public Animator conditions;
    public Animator repeats;


    public DroppedReferenceHolder DropHandlerRepeat;
    public DroppedReferenceHolder DropHandlerDecision;

    public void OnPointerDown(PointerEventData eventData)
    {

            scale = CodeCanvas.GetComponent<RectTransform>().localScale;
            codeblk = Instantiate(codeprefab, BlockCodeUI, false);

        codeblk.tag = tag;
        codeblk.GetComponent<CodeBlockDrag>().CodeCanvas = CodeCanvas;
            codeblk.GetComponent<CodeBlockDrag>().placeholderParent = CodeCanvas;
            codeblk.GetComponent<CodeBlockDrag>().CodeBuilder = BlockCodeUI;
            codeblk.GetComponent<CodeBlockDrag>().placeholder = placeholder;
            codeblk.GetComponent<CodeBlockDrag>().returnParent = CodeCanvas;
            codeblk.GetComponent<RectTransform>().localScale = scale;
            codeblk.GetComponent<Image>().color = this.GetComponent<Image>().color;
            if (tag == "codeblock")
            {
                codeblk.GetComponentInChildren<Text>().text = GetComponentInChildren<Text>().text;
                codeblk.GetComponent<CodeBlockMeta>().act = selectedAct(GetComponentInChildren<Text>().text);
            }
           
    
    }

    public void OnDrag(PointerEventData eventData)
    {

        if (Input.touchCount > 1)
        {
            OnEndDrag(eventData);
            return;
        }
        //
        //codeblk.transform.position = eventData.position;
        codeblk.GetComponent<CodeBlockDrag>().OnDrag(eventData);
   
    }

    public void OnDrop(PointerEventData eventData)
    {

    }
    private void Rules(GameObject dropped)
    {
        Debug.Log("rules");
        droppedFunction = dropped;
        if (tag == "repeat")
        {
            repeats.SetBool("IsOpen", true);
            DropHandlerRepeat.droppedContent = droppedFunction;
        }

        if (tag == "decision")
        {
            conditions.SetBool("IsOpen", true);
            DropHandlerDecision.droppedContent = droppedFunction;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        codeblk.GetComponent<CodeBlockDrag>().OnEndDrag(eventData);
        codeblk.transform.SetParent(CodeCanvas);
            Rules(codeblk);
       
    }
    public ActionStates selectedAct(string text)
    {
        ActionStates actas;
        switch (text)
        {
            case "Quick Attack": actas = ActionStates.QUICK_ATTACK; break;
            case "Block": actas = ActionStates.BLOCK; break;
            case "Spell": actas = ActionStates.SPELL; break;
             default: actas = ActionStates.IDLE; break;
        }
        return actas;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Input.touchCount > 1)
        {
            OnEndDrag(eventData);
            return;
        }

        
        codeblk.GetComponent<CodeBlockDrag>().OnBeginDrag(eventData);
        codeblk.GetComponent<CodeBlockDrag>().returnParent = CodeCanvas;
    }
}


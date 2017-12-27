using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using CodeBlocks;
public class conditionSetter : MonoBehaviour, IPointerDownHandler
{
    public bool IsRepeat;
    public int RepeatTimes;
    public ActionStates Condition;
    public GameObject dropped;
    public Animator conditions;
    public Animator repeats;
    public void OnPointerDown(PointerEventData eventData)
    {
        dropped = getDropped();
        if (dropped == null) return;

        if (IsRepeat)
            AssignRepeat(RepeatTimes);
        else
            AssignDecision(Condition);


        dropped = null;
    }
    public GameObject getDropped()
    {//improve this
        if (this.transform.parent.parent.parent.GetComponent<DroppedReferenceHolder>() != null)
        {
            return this.transform.parent.parent.parent.GetComponent<DroppedReferenceHolder>().droppedContent;
        }
        else if (this.transform.parent.parent.GetComponent<DroppedReferenceHolder>() != null)
        {
            return this.transform.parent.parent.GetComponent<DroppedReferenceHolder>().droppedContent;
        }
        else { return null; }
    }
    public void AssignRepeat(int rt) {
       
        dropped.GetComponent<CodeBlockMeta>().SetRepeatTimes(rt);
        repeats.SetBool("IsOpen", false);
    }
    public void AssignDecision(ActionStates con) {
        dropped.GetComponent<CodeBlockMeta>().SetCondition(con);
        conditions.SetBool("IsOpen", false);
    }
}

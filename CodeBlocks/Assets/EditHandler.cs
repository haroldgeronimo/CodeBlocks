using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EditHandler : MonoBehaviour, IPointerDownHandler
{
    float clicked = 0;
    float clicktime = 0;
    float clickdelay = 0.5f;

    public bool IsRepeat;
    public GameObject repeat;
    public GameObject condition;
    public Animator RepeatAnimator;
    public Animator ConditionAnimator;
    public void Start()
    {

        RepeatAnimator = GameObject.FindGameObjectWithTag("repeatcase").GetComponent<Animator>();
        ConditionAnimator = GameObject.FindGameObjectWithTag("conditioncase").GetComponent<Animator>();
    }
    public void OnPointerDown(PointerEventData data)
    {
        clicked++;
        if (clicked == 1) clicktime = Time.time;

        if (clicked > 1 && Time.time - clicktime < clickdelay)
        {
            clicked = 0;
            clicktime = 0;
            Debug.Log("Double CLick: " + this.GetComponent<RectTransform>().name);
            if (IsRepeat)
            {
                repeat.GetComponent<DroppedReferenceHolder>().droppedContent = this.gameObject;
                if (RepeatAnimator == null) return;
                RepeatAnimator.SetBool("IsOpen", true);
            }
            else
            {
                condition.GetComponent<DroppedReferenceHolder>().droppedContent = this.gameObject;
        
                if (ConditionAnimator == null) return;
                ConditionAnimator.SetBool("IsOpen", true);
            }

        }
        else if (clicked > 2 || Time.time - clicktime > 1) clicked = 0;

    }

}

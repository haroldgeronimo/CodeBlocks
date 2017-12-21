using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeleteArea : MonoBehaviour, IDropHandler {
    public Transform CodeCanvas;
    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag.gameObject.tag == "decision"
           || eventData.pointerDrag.gameObject.tag == "repeat"
           || eventData.pointerDrag.gameObject.tag == "codeblock")
        {
            Destroy(eventData.pointerDrag.gameObject);
        }
        else if (eventData.pointerDrag.gameObject.tag == "main")
        {
          //  eventData.pointerDrag.transform.SetParent(CodeCanvas);
           // eventData.pointerDrag.transform.position = new Vector3(CodeCanvas.GetComponent<RectTransform>().rect.width/2, CodeCanvas.GetComponent<RectTransform>().rect.height / 2);
        }
    }

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

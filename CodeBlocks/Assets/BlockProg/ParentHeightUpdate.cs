using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParentHeightUpdate : MonoBehaviour {
    RectTransform myParent;
	// Use this for initialization
	void Start () {
        myParent = transform.parent.GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<LayoutElement>().preferredHeight = myParent.rect.height;
	}
}

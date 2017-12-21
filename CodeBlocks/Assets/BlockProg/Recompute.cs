using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Recompute : MonoBehaviour {
	// Use this for initialization
	void Start () {
        this.GetComponent<LayoutElement>().preferredHeight = 0f;
	}
	
}

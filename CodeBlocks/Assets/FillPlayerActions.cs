using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeBlocks;
public class FillPlayerActions : MonoBehaviour {

    public CodeBlockManager ActSc;
    private List<ActionStates> Actions;
    public GameObject functionBlock;
    public void FillOutCanvas()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        Actions = ActSc.PlayerActions;
        foreach (ActionStates action in Actions)
        {
            functionBlock.GetComponentInChildren<Text>().text = action.ToString();
            if (action.ToString() == "QUICK_ATTACK")
            {
                functionBlock.GetComponent<Image>().color = Color.red;
            }
            else if (action.ToString() == "BLOCK")
            {
                functionBlock.GetComponent<Image>().color = Color.blue;
            }
            else if (action.ToString() == "SPELL")
            {
                functionBlock.GetComponent<Image>().color = Color.yellow;
            }
            else if (action.ToString() == "IDLE")
            {
                Color col = new Color();
                col.a = 0;
                functionBlock.GetComponent<Image>().color = col;
            }
            Instantiate(functionBlock, this.transform);
        }

    }
    private void Start()
    {

    }
    // Update is called once per frame
    void Update () {
        Actions = ActSc.PlayerActions;
        if (Actions!=null)
        if (Actions.Count > 0)
        {
            FillOutCanvas();
        }
	}
}

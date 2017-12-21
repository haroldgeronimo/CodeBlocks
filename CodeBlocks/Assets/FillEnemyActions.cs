using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeBlocks;
public class FillEnemyActions : MonoBehaviour {
    public ActionsScript ActSc;
    public GameObject functionBlock;
    public void FillOutCanvas()
    {
        List<ActionStates> Actions = ActSc.actions;
        foreach (ActionStates action in Actions)
        {
            functionBlock.GetComponentInChildren<Text>().text = action.ToString();
            if(action.ToString() =="QUICK_ATTACK")
            {
                functionBlock.GetComponent<Image>().color = Color.red;
            }
            else if(action.ToString() == "BLOCK")
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
            Instantiate(functionBlock,this.transform);
        }

    }
    private void Start()
    {
        FillOutCanvas();
    }
}

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
        Color color;
        foreach (ActionStates action in Actions)
        {
            functionBlock.GetComponentInChildren<Text>().text = action.ToString();
            if (action.ToString() == "QUICK_ATTACK")
            {
                ColorUtility.TryParseHtmlString("#FF7A7AFF", out color);
                functionBlock.GetComponent<Image>().color = color;
            }
            else if (action.ToString() == "BLOCK")
            {
                ColorUtility.TryParseHtmlString("#7ECFFFFF", out color);
                functionBlock.GetComponent<Image>().color = color;
            }
            else if (action.ToString() == "SPELL")
            {
                ColorUtility.TryParseHtmlString("#EC6610FF", out color);
                functionBlock.GetComponent<Image>().color = color;
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
   
    public void UpdateCanvas()
    {
        Actions = ActSc.PlayerActions;
        if (Actions != null)
            if (Actions.Count > 0)
            {
                FillOutCanvas();
            }
    }
}

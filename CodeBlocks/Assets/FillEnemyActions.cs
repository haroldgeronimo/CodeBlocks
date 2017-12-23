using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeBlocks;
public class FillEnemyActions : MonoBehaviour {
    public ActionsScript ActSc;
     public GameObject functionBlock;
    public bool IsDebug;
    public Sprite defaultSprite;
    public Sprite glitchedSprite;
    public void FillOutCanvas()
    {

        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        List<ActionHandler> Actions = new List<ActionHandler>();
        Actions.Clear();
        foreach (ActionHandler act in ActSc.actions)
        {
            Actions.Add(act);
           
        }
        Color color;


        foreach (ActionHandler actionHandle in Actions)
        {
            functionBlock.GetComponentInChildren<Text>().text = actionHandle.Action.ToString();
            bool IsSeen = actionHandle.IsSeen;
            if (!IsDebug)
            {
                IsSeen = true;
            }

            if (IsSeen)
            {

                functionBlock.GetComponent<Image>().sprite = defaultSprite;
                if (actionHandle.Action.ToString() == "QUICK_ATTACK")
                {
                    ColorUtility.TryParseHtmlString("#FF7A7AFF", out color);
                    functionBlock.GetComponent<Image>().color = color;
                }
                else if (actionHandle.Action.ToString() == "BLOCK")
                {
                ColorUtility.TryParseHtmlString("#7ECFFFFF", out color);
                    functionBlock.GetComponent<Image>().color = color;
                }
                else if (actionHandle.Action.ToString() == "SPELL")
                {
                ColorUtility.TryParseHtmlString("#EC6610FF", out color);
                    functionBlock.GetComponent<Image>().color = color;
                }

            }
            else
            {
                functionBlock.GetComponent<Image>().color = Color.white;
                functionBlock.transform.GetChild(0).GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                functionBlock.transform.GetChild(0).GetComponent<Text>().text = "?";
                functionBlock.GetComponent<Image>().sprite = glitchedSprite;
            }

            
            if (actionHandle.Action.ToString() == "IDLE")
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

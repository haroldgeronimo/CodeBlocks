using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeBlocks;
public class CodeBlockMeta : MonoBehaviour {
    public string type;
    public bool IsAction;
    public bool IsRepeat;
    public bool IsDecision;
    //for Action;
    public ActionStates act;
    //for Repeat
    public int RepeatTimes;
    private int incrementor;
    //for Decision
    public ActionStates condition;
    private Text upText;
    public string caption; 
    public GameObject topBounds;
    public GameObject trueArea;
    public GameObject falseArea;

    //
    private void Start()
    {
        if (IsRepeat || IsDecision)
        {
            upText = topBounds.GetComponentInChildren<Text>();
            caption = upText.text;
        }
    }
    public void SetCondition(ActionStates con)
    {
        condition = con;
        upText.text = "if <" + con.ToString() + ">";
    }

    public void SetRepeatTimes(int rt)
    {
        RepeatTimes = incrementor = rt;

        upText.text = "Repeat <" + rt + ">";
    }

    public bool RepeatStep()
    {
       // Debug.Log("Repeating on " + incrementor);
        upText.text = "Repeat <" + incrementor + ">";
        if (incrementor == 0)
        {

            return true;
        }
        incrementor -= 1;
        return false;

   
    }

    public void RevertRepeat()
    {
        incrementor = RepeatTimes;
            upText.text = "Repeat < " + incrementor + " >";
    }

}

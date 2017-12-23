using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeBlocks;

public partial class ActionsScript : MonoBehaviour {
    public Animator animator;
    public List<ActionHandler> actions;

    public ActionStates currentAction;


    public void ExecuteAction(ActionStates state)
    {
        Debug.Log("Execute Action:" + state.ToString());
    }
    public void PrepareActions()
    {
        //randomize Actions to Randomize anything that IsSeen = false
        foreach (ActionHandler action in actions)
        {
            if (action.ActionstoRandomize.Count <= 0)
                continue;
            action.Action = action.ActionstoRandomize[Random.Range(0, action.ActionstoRandomize.Count)];
            Debug.Log("Randomized to Action:" + action.Action.ToString());
        }

    }

}

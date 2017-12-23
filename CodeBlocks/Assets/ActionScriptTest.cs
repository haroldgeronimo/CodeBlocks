using System.Collections.Generic;
using CodeBlocks;

[System.Serializable]
public class ActionHandler  {
    public ActionStates Action;
    public bool IsSeen = true;
    public List<ActionStates> ActionstoRandomize;

} 



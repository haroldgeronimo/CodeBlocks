using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeBlocks;
public class CodeBlockManager : MonoBehaviour
{
    [Range(0.05f, 1)]
    public float Delay = 0.5f;

    public Transform pointerObj;

    public List<ActionStates> PlayerActions = new List<ActionStates>();

    public ActionStates currentEnemyAction;
  
    public Transform blockpos = null;

    public bool SimulationEnd = false;

    public FillPlayerActions fillPlayerActions;

    public void CodeContainerReader(Transform content)
    {
        //   Debug.Log("CodeContainer Reader Entered " + content.name);

        // CoroutineController controller;

        //this.StartCoroutineEx(CodeReader(block), out controller);

        //if (controller.state != CoroutineState.Finished)
        //{
        //    return;
        //}
        //{
        //}

        CodeReaders.Add(CodeReaderID);
       StartCoroutine(CodeReader(content,0));

        //Transform block = content.GetChild(0);
        //ActionStates action;
        //Transform currentBlock = block;
        //while (currentBlock != null)
        //{
        //    action = CodeBlockReader(currentBlock);
        //    if (action != ActionStates.NULL && action != ActionStates.SPECIAL)
        //    {
        //        PlayerActions.Add(action);
        //    }
        //    if (action != ActionStates.SPECIAL)
        //    {
        //        currentBlock = GetNext(currentBlock);
        //    }


        //    //yield return new WaitForSeconds(1);
        //}


    }

    //DON'T MESS AROUND THIS CURRENT CODE:  LAST MODIFIED DECEMBER 23, 2017 3:37 AM - IHG
    //THE GOLDEN RECURSIVE FUNCTION THAT READS BLOCKS
    IEnumerator CodeReader(Transform content, int depth)
    {
        Debug.Log("Starting Code Reader: " + depth);
        if (depth == 0)
        {
            pointerObj.gameObject.SetActive(true);
        }
        // int ReaderID = CodeReaderID;
        // Debug.Log("Started a Code Reader" + ReaderID);
        Transform block = content.GetChild(0);
        // pointerObj.gameObject.SetActive(true);
        blockpos = block;
        ActionStates action;
        Transform currentBlock = block;
        while (currentBlock != null)
        {

            action = ActionStates.NULL;

            if(currentBlock.childCount==0)
            blockpos = currentBlock;
            else
            {
                blockpos = currentBlock.GetChild(0);
            }

            CodeBlockMeta blockMeta = null;
            blockMeta = currentBlock.GetComponent<CodeBlockMeta>();

          //  Debug.Log("BlockMeta:" + block.name);

            if (blockMeta == null)
            {
                action = ActionStates.NULL;
             //   Debug.Log("blockMeta is null: " + currentBlock.name);

            }
            else if (blockMeta.type == "codeblock")
            {

                //Debug.Log("Executing Function:" + blockMeta.act);
                action = blockMeta.act;
            }
            else if (blockMeta.type == "decision")
            {
                if (GameObject.FindGameObjectWithTag("EM").GetComponent<ActionsScript>().actions.Count > PlayerActions.Count)
                    currentEnemyAction = GameObject.FindGameObjectWithTag("EM").GetComponent<ActionsScript>().actions[PlayerActions.Count].Action;
                else
                    currentEnemyAction = ActionStates.GMODE;
                if (GameObject.FindGameObjectWithTag("EM").GetComponent<ActionsScript>() == null)
                //    Debug.Log("Cannot find enemy manager!");

                blockpos = blockMeta.transform.GetChild(0);
                yield return new WaitForSeconds(Delay);
                //check enemy action
                //   Debug.Log("Condition if Enemy." + currentEnemyAction.ToString() + " == Player." + blockMeta.condition.ToString());
                if (currentEnemyAction == blockMeta.condition)
                {
                    //     Debug.Log("Condition Returned TRUE!");
                    //CodeReaderID++;
                    //CodeReaders.Add(CodeReaderID);
                    //CodeContainerReader(FindContentWithTag(block, "truecontent"));
                    blockpos = blockMeta.transform.GetChild(0);
                    yield return new WaitForSeconds(Delay);
                    yield return StartCoroutine(CodeReader(FindContentWithTag(currentBlock, "truecontent"), depth + 1));

                    action = ActionStates.NULL;
                }
                else
                {
                    blockpos = blockMeta.transform.GetChild(2);
                    yield return new WaitForSeconds(Delay);
                    //    Debug.Log("Condition Returned FALSE!");
                    //CodeReaderID++;
                    //CodeReaders.Add(CodeReaderID);
                    //CodeContainerReader(FindContentWithTag(block, "falsecontent"));
                    yield return StartCoroutine(CodeReader(FindContentWithTag(currentBlock, "falsecontent"), depth + 1));
                    action = ActionStates.NULL;
                }
            }
            else if (blockMeta.type == "repeat")
            {
                //    Debug.Log("Repeat Function Executed!");
                bool RepStep = blockMeta.RepeatStep();
                if (RepStep)
                {
                    blockpos = blockMeta.transform.GetChild(2);
                    yield return new WaitForSeconds(Delay);
                    //      Debug.Log("Repeat Function Ended!");
                    blockMeta.RevertRepeat();
                    action = ActionStates.NULL;
                }
                else
                {
                    //     Debug.Log("Repeat Function Repeated!");CodeReaderID++;
                    //CodeReaderID++;
                    //CodeReaders.Add(CodeReaderID);
                    //  CodeContainerReader(FindContentWithTag(block, "content"));
                    blockpos = blockMeta.transform.GetChild(0);
                    yield return new WaitForSeconds(Delay);
                    yield return StartCoroutine(CodeReader(FindContentWithTag(currentBlock, "content"), depth + 1));
                    action = ActionStates.SPECIAL;
                }
            }


            if (action != ActionStates.NULL && action != ActionStates.SPECIAL)
            {
                PlayerActions.Add(action);
            }
            if (action != ActionStates.SPECIAL)
            {
                currentBlock = GetNext(currentBlock);
            }


            fillPlayerActions.UpdateCanvas();
            yield return new WaitForSeconds(Delay);
        }

        //  yield return null;
        //SimulationEnd = true;
        if (depth == 0)
        {
            pointerObj.gameObject.SetActive(false);
            SimulationEnd = true;
            fillPlayerActions.UpdateCanvas();   
        }


        Debug.Log("Ending Code Reader: " + depth);

      
        yield return null;

        //Debug.Log("Ended a Code Reader" + ReaderID);
        // CodeReaderID++;
    }

    ActionStates CodeBlockReader(Transform block)
    {

        blockpos = block;
        CodeBlockMeta blockMeta = block.GetComponent<CodeBlockMeta>();
        if (blockMeta == null)
            return ActionStates.NULL;
        if (blockMeta.type == "codeblock")
        {

            //Debug.Log("Executing Function:" + blockMeta.act);
            return blockMeta.act;
        }
        if (blockMeta.type == "decision")
        {
            if (GameObject.FindGameObjectWithTag("EM").GetComponent<ActionsScript>().actions.Count > PlayerActions.Count)
                currentEnemyAction = GameObject.FindGameObjectWithTag("EM").GetComponent<ActionsScript>().actions[PlayerActions.Count].Action;
            else
                currentEnemyAction = ActionStates.GMODE;
            if (GameObject.FindGameObjectWithTag("EM").GetComponent<ActionsScript>() == null)
                Debug.Log("Cannot find enemy manager!");
            //check enemy action
         //   Debug.Log("Condition if Enemy." + currentEnemyAction.ToString() + " == Player." + blockMeta.condition.ToString());
            if (currentEnemyAction == blockMeta.condition)
            {
                //     Debug.Log("Condition Returned TRUE!");
                // CodeReaderID++;
                // CodeReaders.Add(CodeReaderID);
              // GetPlayerMoveCount(FindContentWithTag(block, "truecontent"));
                // StartCoroutine(ContainerReader(FindContentWithTag(block, "truecontent")));

                return ActionStates.NULL;
            }
            else
            {
                //    Debug.Log("Condition Returned FALSE!");
                //CodeReaderID++;
                //CodeReaders.Add(CodeReaderID);
              // GetPlayerMoveCount(FindContentWithTag(block, "falsecontent"));
                //StartCoroutine(ContainerReader(FindContentWithTag(block, "falsecontent")));
                return ActionStates.NULL;
            }
        }
        if (blockMeta.type == "repeat")
        {
        //    Debug.Log("Repeat Function Executed!");
            bool RepStep = blockMeta.RepeatStep();
            if (RepStep)
            {
          //      Debug.Log("Repeat Function Ended!");
                blockMeta.RevertRepeat();
                return ActionStates.NULL;
            }
            else
            {
                //     Debug.Log("Repeat Function Repeated!");CodeReaderID++;
                //  CodeReaderID++;
                // CodeReaders.Add(CodeReaderID);
                //GetPlayerMoveCount(FindContentWithTag(block, "content"));
               // StartCoroutine(ContainerReader(FindContentWithTag(block, "content")));
                return ActionStates.SPECIAL;
            }
        }
        return ActionStates.NULL;
    }
    
    #region OPTIONAL 
    int CodeReaderID = 0;
    List<int> CodeReaders = new List<int>();

    IEnumerator ContainerReader(Transform content)
    {
        Transform block = content.GetChild(0);
        //StartCoroutine(CodeReader(block));
        ActionStates action;
        Transform currentBlock = block;
        while (currentBlock != null)
        {

            yield return new WaitForSeconds(1);
            action = CodeBlockReader(currentBlock);
            if (action != ActionStates.NULL && action != ActionStates.SPECIAL)
            {
                PlayerActions.Add(action);
            }
            if (action != ActionStates.SPECIAL)
            {
                currentBlock = GetNext(currentBlock);
            }



          
      }

    }
    #endregion
    
    private void Update()
    {
        if(blockpos!=null)
        pointerObj.position = Vector3.Lerp(pointerObj.position, blockpos.position - new Vector3(blockpos.GetComponent<RectTransform>().rect.width / 2, 0), .4f);
        if (SimulationEnd)
        {
            SimulationEnd = false;
        }
    }

    #region FUNCTIONS
    List<ActionStates> pa = new List<ActionStates>();
    public int GetPlayerMoveCount(Transform content, int depth)
    {
      
        if(depth == 0)
        {
            pa.Clear();
        }
        ActionStates action = ActionStates.NULL;
        Transform currentBlock = content.GetChild(0);

        while (currentBlock != null)
        {
           CodeBlockMeta blockMeta = currentBlock.GetComponent<CodeBlockMeta>();

            if (blockMeta == null) { 
                action = ActionStates.NULL;
                Debug.Log("NO ACTION FOUND!");
               
                }
            else if (blockMeta.type == "codeblock")
            {


                action =  blockMeta.act;
            }
            else if (blockMeta.type == "decision")
            {
                if (GameObject.FindGameObjectWithTag("EM").GetComponent<ActionsScript>().actions.Count > PlayerActions.Count)
                    currentEnemyAction = GameObject.FindGameObjectWithTag("EM").GetComponent<ActionsScript>().actions[PlayerActions.Count].Action;
                else
                    currentEnemyAction = ActionStates.GMODE;
                if (GameObject.FindGameObjectWithTag("EM").GetComponent<ActionsScript>() == null)
                    Debug.Log("Cannot find enemy manager!");

                if (currentEnemyAction == blockMeta.condition)
                {
                    GetPlayerMoveCount(FindContentWithTag(currentBlock, "truecontent"),depth + 1);

                    action = ActionStates.NULL;
                }
                else
                {
                    GetPlayerMoveCount(FindContentWithTag(currentBlock, "falsecontent"), depth + 1);
                    action =  ActionStates.NULL;
                }
            }
            else if (blockMeta.type == "repeat")
            {
              bool RepStep = blockMeta.RepeatStep();
                if (RepStep)
                {
                   blockMeta.RevertRepeat();
                    action =  ActionStates.NULL;
                }
                else
                {
                    GetPlayerMoveCount(FindContentWithTag(currentBlock, "content"), depth + 1);
                    action = ActionStates.SPECIAL;
                }
            }

           
            if (action != ActionStates.NULL && action != ActionStates.SPECIAL)
            {
                pa.Add(action);
            }
            if (action != ActionStates.SPECIAL)
            {
                currentBlock = GetNext(currentBlock);
            }


         
        }


        if(depth == 0)
        return pa.Count;

        return 0;
    }
    Transform GetNext(Transform transform)
    {
        var myself = transform;
        var parent = transform.parent;
        var childCount = parent.childCount;
        for (int i = 0; i < childCount - 1; i++)
        { // skip the last, as it doesn't have a successor
            if (parent.GetChild(i) == myself)
                return parent.GetChild(i + 1);
        }
        return null;
    }
    Transform FindContentWithTag(Transform parentTransform, string Tag)
    {
        foreach (Transform child in parentTransform) if (child.CompareTag(Tag))
            {
                return child;
            }
        return null;
    }
    public void ChangeDelay(float d)
    {
        Delay = d;
    }
#endregion
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeBlocks;
public class CodeSimulator : MonoBehaviour
{
    public Text ConsoleText;
    public Transform MainFuncContent;
    public ActionsScript EAS;
    public ActionStates enemyCurrentAction;
    public ActionStates playerCurrentAction;
    public Transform CodeCanvas;


    private List<ActionStates> EnemyActions;
    public int OverheadActionTreshold = 5;
    private int PlayerPoints;
    private int EnemyPoints;
    private int EnemyPointer = 0;
    public static CodeSimulator CS;
    public CodeBlockManager CBM;
    public Countdown Timer;

    //States
    bool IsCompiling = false;
    bool IsSimulating = false;

    //Animators
    public Animator CodeblockUIAnimator;
    public Animator StopAnimator;
    public Animator PlayerScriptSim;
    public Animator EnemyScriptSim;

    //Pointers
    public Transform PlayerPointerObject;
    public Transform EnemyPointerObject;
    private Transform PlayerBlockPos;
    private Transform EnemyBlockPos;
    public Transform EnemyScriptCase;
    public Transform PlayerScriptCase;

    //Constraints
    public CanvasGroup CodeCanvasCG;

    //Simulation
    public FillPlayerActions fillPlayerActions;

    void Awake()
    {
        CS = this;
    }

    private void Start()
    {
        GameObject cbm = GameObject.FindGameObjectWithTag("CBM");
        CBM = cbm.GetComponent<CodeBlockManager>();
    }

    public void LateUpdate()
    {
        if (CBM.SimulationEnd)
        {
            EndCompilation();
        }
    }

    public void StartCompilation()
    {
        if (IsCompiling)
            return;

        
        //  cbm = new CodeBlockManager();
        PlayerPoints = 0;
        EnemyPoints = 0;
        EnemyActions = EAS.actions;
        //int i = 0;
        ClearConsole();
        AddtoConsole("Start of Simulation");
        Debug.Log("Starting Simulation");
        if (MainHasContent()) Debug.Log("MainFunction has content"); else { Debug.Log("MainFunction has NO content"); return; }

        // CodeblockUIAnimator.SetBool("IsOpen", false);


        int playerActs = CBM.GetPlayerMoveCount(MainFuncContent, 0);
        Debug.Log("Player actions:" + playerActs);
        if(playerActs <= 0)
        {
            Debug.Log("Warning: Player yields no actions! Revisit your codeBlocks!");
            return;
        }

        if ((playerActs - EnemyActions.Count) >= OverheadActionTreshold)
        {
            Debug.Log("Warning: Player have too much Overhead Actions! Please revisit your codeBlocks!");
            return;
        }

        if (playerActs > EnemyActions.Count)
        {
            //todo when  players action is more than enemies action
            Debug.Log("Warning: Player have MORE actions than the enemy, Overhead actions have no effect on enemies!");
        }

        else if (playerActs < EnemyActions.Count)
        {
            //todo when enemys action is less than players   action
            Debug.Log("Warning: Player have LESS actions than the enemy, Proceeding actions will be considered as IDLE!");
        }

        IsCompiling = true;
        Timer.IsPaused = true;
        PlayerScriptSim.SetBool("IsOpen", true);
        CodeCanvasCG.blocksRaycasts = false;
        CBM.PlayerActions.Clear();
        CBM.CodeContainerReader(MainFuncContent);

        //Debug.Log("LOL!");
    }

    public void EndCompilation()
    {
        CodeCanvasCG.blocksRaycasts = true;
        CBM.SimulationEnd = false;
        PrepareCodeBlocks();
    }

    public void PrepareCodeBlocks()
    {
        Debug.Log("_____________________________________________");
        Debug.Log("Action Summary:");
        foreach (ActionStates act in CBM.PlayerActions)
        {
            Debug.Log(act);
        }
        //preparing for combat
        int playerActCount = CBM.PlayerActions.Count;
        int enemyActCount = EnemyActions.Count;
        int totalCount = enemyActCount;
        if (enemyActCount > playerActCount)
        {

            int count = enemyActCount - playerActCount;
            for (int i = 0; i < count; i++)
            {
                CBM.PlayerActions.Add(ActionStates.IDLE);
            }
            totalCount = enemyActCount;
        }

        if (playerActCount > enemyActCount)
        {

            int count = playerActCount - enemyActCount;
            for (int i = 0; i < count; i++)
            {
                EnemyActions.Add(ActionStates.GMODE);
            }
            totalCount = playerActCount;
        }

        //Start Simulation
        SimulateMode();


        fillPlayerActions.UpdateCanvas();
        StartSimulation(totalCount);




    }

    public void StartSimulation(int totalCount)
    {
        if (IsSimulating)
            return;

        IsSimulating = true;
        StartCoroutine(simulation(totalCount));

    }

    IEnumerator simulation(int totalCount)
    {
        //fillPlayerActions.UpdateCanvas();
        yield return new WaitForSeconds(.5f);
        PlayerPointerObject.gameObject.SetActive(true);
        EnemyPointerObject.gameObject.SetActive(true);
        for (int i = 0; i < totalCount; i++)
        {
            playerCurrentAction = CBM.PlayerActions[i];
            enemyCurrentAction = EnemyActions[i];


            if (EnemyScriptCase.childCount >= i + 1)
            {
                EnemyBlockPos = EnemyScriptCase.GetChild(i);
            }

            if (PlayerScriptCase.childCount >= i + 1)
            {
                PlayerBlockPos = PlayerScriptCase.GetChild(i);
            }


            yield return new WaitForSeconds(2f);
            Rules();
            yield return new WaitForSeconds(.5f);

            //check here if someone is dead
        
        }
      
        //check here if player and enemy life is equal if yes declare a tie 
        //if life is not equal repeat sequence
        AddtoConsole("Player:" + PlayerPoints + " points");
        AddtoConsole("Enemy:" + EnemyPoints + " points");
        Debug.Log("Player:" + PlayerPoints + " points");
        Debug.Log("Enemy:" + EnemyPoints + " points");
        if (PlayerPoints > EnemyPoints)
        {
            Debug.Log("PLAYER WINS!");
            AddtoConsole("Player wins!");
        }
        else if (PlayerPoints < EnemyPoints)
        {
            DebugMode();
            Debug.Log("ENEMY WINS!");
            AddtoConsole("Enemy Wins!");
        }
        else
        {
            DebugMode();
            Debug.Log("ITS A TIE");
            AddtoConsole("Its a tie");
            }
        Debug.Log("End of Simulation");
        AddtoConsole("End of Simulation");

        PlayerPointerObject.gameObject.SetActive(false);
        EnemyPointerObject.gameObject.SetActive(false);
        IsCompiling = false;
        IsSimulating = false;

        Debug.Log("Comencing Script Battle!");
        yield return new WaitForSeconds(2f);

    }

    void AddtoConsole(string consoleText)
    {
        ConsoleText.text += "\n" + consoleText;
    }

    void ClearConsole()
    {
        ConsoleText.text = "";
    }

    void Rules()
    {
        Debug.Log("Player uses " + playerCurrentAction.ToString() + " and Enemy uses " + enemyCurrentAction.ToString());
        if (enemyCurrentAction == ActionStates.IDLE && playerCurrentAction == ActionStates.IDLE)
        {

            Debug.Log(PointsManager(false, false, 0));
        }
        if ((enemyCurrentAction == ActionStates.QUICK_ATTACK ||
            enemyCurrentAction == ActionStates.SPELL
            ) && playerCurrentAction == ActionStates.IDLE)
        {

            Debug.Log(PointsManager(false, true, 1));
        }


        if (enemyCurrentAction == ActionStates.QUICK_ATTACK && playerCurrentAction == ActionStates.BLOCK)
        {

            Debug.Log(PointsManager(true, false, 1));
        }
        if (enemyCurrentAction == ActionStates.SPELL && playerCurrentAction == ActionStates.BLOCK)
        {
            Debug.Log(PointsManager(false, true, 1));
        }
        if (enemyCurrentAction == ActionStates.BLOCK && playerCurrentAction == ActionStates.BLOCK)
        {
            Debug.Log(PointsManager(false, false, 0));
        }

        if (enemyCurrentAction == ActionStates.QUICK_ATTACK && playerCurrentAction == ActionStates.QUICK_ATTACK)
        {
            Debug.Log(PointsManager(false, false, 0));
        }
        if (enemyCurrentAction == ActionStates.SPELL && playerCurrentAction == ActionStates.QUICK_ATTACK)
        {
            Debug.Log(PointsManager(true, false, 1));
        }
        if (enemyCurrentAction == ActionStates.BLOCK && playerCurrentAction == ActionStates.QUICK_ATTACK)
        {
            Debug.Log(PointsManager(false, true, 1));
        }

        if (enemyCurrentAction == ActionStates.QUICK_ATTACK && playerCurrentAction == ActionStates.SPELL)
        {
            Debug.Log(PointsManager(false, true, 1));
        }
        if (enemyCurrentAction == ActionStates.SPELL && playerCurrentAction == ActionStates.SPELL)
        {
            Debug.Log(PointsManager(false, false, 0));
        }
        if (enemyCurrentAction == ActionStates.BLOCK && playerCurrentAction == ActionStates.SPELL)
        {
            Debug.Log(PointsManager(true, false, 1));
        }
    }

    string PointsManager(bool Player, bool Enemy, int points)
    {
        string ReturnString = "";
        if (Player)
        {
            ReturnString += "Player Receives " + points + " points!";
            PlayerPoints += points;
        }
        if (Enemy)
        {
            ReturnString += " Enemy Receives " + points + " points!";
            EnemyPoints += points;

        }
        if (!Player && !Enemy)
        {
            ReturnString += " No one received any points";
        }


        return ReturnString;
    }

    public bool MainHasContent()
    {
        if (MainFuncContent.childCount > 1)
            return true;
        else return false;
    }

    public ActionStates ReadCodeBlocks(int pointer)
    {
        ActionStates ActState;
        Transform currentContent = MainFuncContent;
        GameObject pointerObject = null;

        pointerObject = currentContent.GetChild(pointer).gameObject;
        if (pointerObject.tag == "placeholder")
        {
            pointer++;
            pointerObject = currentContent.GetChild(pointer).gameObject;
        }


        ActState = pointerObject.GetComponent<CodeBlockMeta>().act;
        if (pointerObject == null)
            ActState = ActionStates.IDLE;
        return ActState;

    }

    private void Update()
    {
        if (PlayerBlockPos != null)
            PlayerPointerObject.position = Vector3.Lerp(PlayerPointerObject.position, PlayerBlockPos.position + new Vector3(PlayerBlockPos.GetComponent<RectTransform>().rect.width - (PlayerBlockPos.GetComponent<RectTransform>().rect.width / 2), 0), .2f);


        if (EnemyBlockPos != null)
            EnemyPointerObject.position = Vector3.Lerp(EnemyPointerObject.position, EnemyBlockPos.position - new Vector3(EnemyBlockPos.GetComponent<RectTransform>().rect.width / 2, 0), .2f);
    
    }

    public void StopAll()
    {
        if(!IsSimulating && IsCompiling)
        {


            StopAllCoroutines();
            CBM.StopAllCoroutines();
            CBM.PlayerActions.Clear();
            CBM.pointerObj.gameObject.SetActive(false);
            fillPlayerActions.UpdateCanvas();


            PlayerPointerObject.gameObject.SetActive(false);
            EnemyPointerObject.gameObject.SetActive(false);
            IsCompiling = false;
            IsSimulating = false;
            Timer.IsPaused = false;

            CodeCanvasCG.blocksRaycasts = true;
            CBM.GetPlayerMoveCount(MainFuncContent, 0);
            DebugMode();
        }
    }

    public void ResetAll()
    {
        ////



        //StopAllCoroutines();
        //CBM.StopAllCoroutines();
        //CBM.PlayerActions.Clear();
        //CBM.pointerObj.gameObject.SetActive(false);
        //fillPlayerActions.UpdateCanvas();


        //PlayerPointerObject.gameObject.SetActive(false);
        //EnemyPointerObject.gameObject.SetActive(false);
        //IsCompiling = false;
        //IsSimulating = false;
        //Timer.InitializeTimer();
        //Timer.IsPaused = false;
        ////clear all main content
        //Transform parent = MainFuncContent;

        //foreach (Transform child in parent)
        //{
        //    Destroy(child);
        //}

        //parent = CodeCanvas;

        //foreach (Transform child in parent)
        //{
        //    if (child.tag != "main")
        //        Destroy(child);
        //}

        //CodeCanvasCG.blocksRaycasts = true;
        //DebugMode();

        //reset health of player here

       // Reload Scene
    }

    #region "Animations"
    public void DebugMode()
    {
        CodeblockUIAnimator.SetBool("IsOpen", true);
        PlayerScriptSim.SetBool("IsOpen", false);
        EnemyScriptSim.SetBool("IsOpen", false);
    }
    public void SimulateMode()
    {
        CodeblockUIAnimator.SetBool("IsOpen", false);
        PlayerScriptSim.SetBool("IsOpen", true);
        EnemyScriptSim.SetBool("IsOpen", true);
    }
    #endregion
}
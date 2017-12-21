using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScriptUIAnimate : MonoBehaviour {
    public Animator animator;
    public void OpenEnemyScriptCase()
    {
        animator.SetBool("IsOpen",true);
    }
    public void CloseEnemyScriptCase()
    {
        animator.SetBool("IsOpen", false);
    }
}

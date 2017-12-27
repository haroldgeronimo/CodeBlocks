using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ModalDialogueSystem : MonoBehaviour {
   private static ModalDialogueSystem modalDialogueSystem;
    public Button yesButton;
    public Button noButton;
    public Button okButton;
    public Text modalTitle;
    public Text modalMessage;

    public Animator animator;

   
    private void Start()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
    public static ModalDialogueSystem instance()
    {
     
        if (!modalDialogueSystem)
        {
            modalDialogueSystem = FindObjectOfType(typeof(ModalDialogueSystem)) as ModalDialogueSystem;
            if (!modalDialogueSystem)
            {
                Debug.LogError("Modal instance cannot be found!");
            }
        }

        return modalDialogueSystem;
    }

    public void ShowYesNo(string message, UnityAction yesAction, UnityAction noAction, string title = "Warning")
    {
        yesButton.gameObject.SetActive(true) ;
        noButton.gameObject.SetActive(true);
        modalTitle.text = title;
        modalMessage.text = message;
        Focus();
        yesButton.onClick.RemoveAllListeners();
        yesButton.onClick.AddListener(yesAction);
        yesButton.onClick.AddListener(() => Close());
        noButton.onClick.RemoveAllListeners();
        noButton.onClick.AddListener(noAction);
        noButton.onClick.AddListener(() => Close());

    }

    public void ShowOk(string message, UnityAction okAction, string title = "Warning")
    {
        okButton.gameObject.SetActive(true);
        modalTitle.text = title;
        modalMessage.text = message;
        Focus();
        okButton.onClick.RemoveAllListeners();
       okButton.onClick.AddListener(okAction);
       okButton.onClick.AddListener(() => Close());

    }


    void Focus() {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.SetAsLastSibling();
        animator.SetBool("IsOpen", true);
        Time.timeScale = 0;
    }
    void Close()
    {


        animator.SetBool("IsOpen", false);

        Time.timeScale = 1;
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
        okButton.gameObject.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(false);
    }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ToggleCC : MonoBehaviour, IPointerClickHandler
{
    public Animator CodeCase;
    public void OnPointerClick(PointerEventData eventData)
    {
        bool IsOpen = CodeCase.GetBool("IsOpen");
        CodeCase.SetBool("IsOpen", !IsOpen);
    }
}

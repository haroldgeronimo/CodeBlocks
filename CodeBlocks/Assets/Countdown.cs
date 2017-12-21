﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour {
    public int CountdownInSeconds;
    public Text displayText;
    public CodeBlockManager cbm;
    bool IsCountingDown = true;
    public bool IsPaused = false;
    private int minutes = 0;
    private int seconds = 0;
	void Start () {
        if (CountdownInSeconds > 59)
        {
            minutes = CountdownInSeconds / 60;
            seconds = CountdownInSeconds - (minutes * 60);
        }
        StartCountdown();   

	}
	public void StartCountdown()
    {
        StartCoroutine(CountDownTimer());
    }
	// Update is called once per frame
	IEnumerator CountDownTimer() {

        while (IsPaused)
        {
            yield return null;
        }
        while (IsCountingDown)
        {
            seconds --;
            if (seconds <= 0 && minutes > 0)
            {
                seconds = 59;
                minutes--;
            }else if(seconds <= 0 && minutes <= 0)
            {
                seconds = 0;
                IsCountingDown = false;
   
            }
            displayText.text = minutes.ToString("D2") + ":" + seconds.ToString("D2");
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(0.2f);
        if (!IsCountingDown)
        {
            //to do when timer finished!

            Debug.Log("Timer Finished!");

        }

        yield return null;
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour 
{
    #region Vars
    public int secondsToCountdown;
    private float timeCounter;
    private int timeInt;
    private Text timer;
    public const float timeDecreaseRate = 1.25f;

    public static Countdown instance;
    #endregion

    #region Props

    public int TimeInt
    {
        get { return timeInt; }
    }

    #endregion

    void Awake()
    {
        if(instance == null)
            instance = this;
        else if(instance != this)
            Destroy(this.gameObject);
    }

    void OnEnable()
	{
        timeCounter = 0f;
        timer = GetComponent<Text>();
        timeInt = secondsToCountdown;
        timer.text = timeInt.ToString();
    }

	void FixedUpdate()
	{
        timeCounter += Time.fixedDeltaTime;    
        if(timeCounter >= timeDecreaseRate)
        {
            timeCounter = 0f;
            timeInt--;
            timer.text = timeInt.ToString();
        }

		if(timeInt == 0) timer.text = "Go!";  		                 
		else if(timeInt < 0)
		{
            EnemySpawner.instance.enabled = true;			
            GameUI.instance.ToggleInGameMenu(true);
            gameObject.SetActive(false);
		}
    }
}
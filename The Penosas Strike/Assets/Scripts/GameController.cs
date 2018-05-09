using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour 
{
    #region Variables    
    public int losses;
    public int nextLevelPontuation;
    public int nextEnemyRaiseScore;
    public int level;
    public float timer;
    public float eggSpeed;
    public float pigeonSpeed;
    public float pigeonMaxSpeed;

    private int levelChangeOffset = 5; 
    private int _score;   

    public static GameController instance;     
    #endregion

    #region Props
    public int Score 
    {
        get { return _score; } 
        set 
        {
            _score = value;
            GameUI.instance.UpdateScore(_score); 
        }
    }

    public bool IsGameOver
    {
        get; private set;
    }
    #endregion

    void Awake()
	{
		if(instance == null)
            instance = this;
		else
            Destroy(this.gameObject);
    }

    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;               
        nextLevelPontuation = 5;
        level = 1;      

        if(EnemySpawner.instance.maxEnemiesCurrent > 
            EnemySpawner.instance.maxEnemiesTotal)
                EnemySpawner.instance.maxEnemiesCurrent = 1;
    }

    void Update()
	{
		if(Score == nextLevelPontuation)
        {            
            ChangeLevel();
        }        

        // if(!IsGameOver && losses == 5)
        // {
        //     IsGameOver = true;
        //     GameUI.instance.ToggleGameOverMenu(true);            
        // }
    }

	void FixedUpdate()
	{
        timer += Time.fixedDeltaTime;
    }

    private void RaiseEnemyAmount()
    {
        if (Score == nextEnemyRaiseScore)
        {
            int current = EnemySpawner.instance.maxEnemiesCurrent;
            int total = EnemySpawner.instance.maxEnemiesTotal;
            if(current < total)            
            {
                EnemySpawner.instance.maxEnemiesCurrent++;
                if(Score % 10 == 0) 
                {
                    nextEnemyRaiseScore += nextEnemyRaiseScore * 2;
                }
            }                    
        }
    }

    private void ChangeLevel()
    {
        level++;
        RaiseEnemyAmount();

        if(pigeonSpeed < pigeonMaxSpeed)
            pigeonSpeed += 0.075f;             

        if(level % 3 == 0)
        {
            if(EnemySpawner.instance.minTimeSpawn > 0.2f)
                EnemySpawner.instance.minTimeSpawn -= 0.04f;
            
            if(EnemySpawner.instance.maxTimeSpawn > 0.5f)
                EnemySpawner.instance.maxTimeSpawn -= 0.065f;
        }
        
        if(levelChangeOffset < 10 && EnemySpawner.instance.maxEnemiesCurrent >= 3)                             
            levelChangeOffset = 10;

        nextLevelPontuation += levelChangeOffset;  
    }
}

/*
    Level 1: 1 pombo, velocidade 1,5, spawn rate 0,5 ~ 1,5
    Level impossible: 5 pombos, velocidade 2,5 spawn rate 0,2 ~ 0,5
*/

/*

Valores de f(x) que eu quero alcançar pra nextEnemyRaiseScore
10 (2) 30 (3) 50 (4) 80 (5) 120



f(x) = 10 + x

11 ~ 49 += 20 => x = 10

51 ~ 79 += 30 => x = 20

81 ~ 119 += 40 => x = 30
 */
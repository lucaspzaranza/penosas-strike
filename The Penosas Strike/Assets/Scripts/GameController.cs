using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour 
{
    #region Variables
    public bool canGameOver;
    public int losses;
    public int nextLevel;
    public int enemyRaise;
    public int level;
    public float timer;
    public float eggSpeed;
    public float pigeonSpeed;        

    private const int maxScore = 999;
    private const int gameOverScore = 5;
    private const int levelUpIncreaseRate = 5; 
    private const float minTimeSpawnLimit = 0.4f;
    private const float minTimeSpawnDecreaseRate = 0.025f;
    private const float maxTimeSpawnLimit = 0.6f;
    private const float maxTimeSpawnDecreaseRate = 0.15f;    
    [SerializeField] private int _score;   

    public static GameController instance;     
    #endregion

    #region Props
    public int Score
    {
        get { return _score; }
        set 
        {    
            if(value <= maxScore)                   
            {
                _score = value;
                GameUI.instance.UpdateScore(_score);  
                if(value == maxScore)            
                    print("Dude, get a life!"); // Fazer algo pro jogo acabar       
            }
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
        //Time.timeScale = 0.3f;
        Screen.orientation = ScreenOrientation.Portrait;               
        nextLevel = 5;
        level = 1;      

        if(EnemySpawner.instance.maxEnemiesCurrent > 
            EnemySpawner.instance.maxEnemiesTotal)
                EnemySpawner.instance.maxEnemiesCurrent = 1;
    }

    void Update()
	{
		if(Score == nextLevel)
        {            
            ChangeLevel();
        }       

        if(canGameOver && !IsGameOver && losses == gameOverScore)
        {
            IsGameOver = true;
            GameUI.instance.ToggleGameOverMenu(true);            
        }
    }

	void FixedUpdate()
	{
        timer += Time.fixedDeltaTime;
    }

    private void IncreaseEnemyAmount()
    {        
        int current = EnemySpawner.instance.maxEnemiesCurrent;
        int total = EnemySpawner.instance.maxEnemiesTotal;
        if(current < total)            
        {
            EnemySpawner.instance.maxEnemiesCurrent++;
            enemyRaise += 10;
            #region Versão Antiga
            /*
            if(Score < 30)
                enemyRaise += 10;
            else if(Score < 50)
                enemyRaise += 20;
            else if(Score < 70)
                enemyRaise += 30;
            else if(Score < 120)
                enemyRaise += 40;
            */
            #endregion
        }                            
    }

    private void ChangeLevel()
    {
        level++;
        if(Score == enemyRaise) IncreaseEnemyAmount(); 
        
        if(level % 2 == 0)
        {
            if(EnemySpawner.instance.minTimeSpawn  - minTimeSpawnDecreaseRate >= 
                                                              minTimeSpawnLimit)                                                             
                EnemySpawner.instance.minTimeSpawn -= minTimeSpawnDecreaseRate;
            
            if(EnemySpawner.instance.maxTimeSpawn - maxTimeSpawnDecreaseRate >= 
                                                              maxTimeSpawnLimit)                                                            
                EnemySpawner.instance.maxTimeSpawn -= maxTimeSpawnDecreaseRate;            
        }                

        nextLevel += levelUpIncreaseRate;  
    }
}
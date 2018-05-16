using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour 
{
    #region Variables
    public bool canGameOver;        
    public int nextLevel;
    public int enemyRaise;
    public int level;
    public float timer;
    public float eggSpeed;
    public float pigeonSpeed;
    public Animator sunglasses;

    private const int maxScore = 999;    
    private const int insaneLevelScore = 150;
    private const int levelUpIncreaseRate = 5; 
    private const float minTimeSpawnLimit = 0.4f;
    private const float minTimeSpawnDecreaseRate = 0.025f;
    private const float maxTimeSpawnLimit = 0.6f;
    private const float maxTimeSpawnDecreaseRate = 0.15f;    
    [SerializeField] private int _score;
    [SerializeField] [Range(0,5)] private int _life;
    [SerializeField] private bool _insaneLevel;
    private bool restartedGame;

    public static GameController instance;     
    #endregion

    #region Props
    public int MaxLife
    {
        get { return 5; }
    }

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
                    GameUI.instance.CallGetALifeMenu();                           
                else if(_score % 100 == 0)                
                    Life++;                    
            }
        }
    }

    public int Life
    {
        get { return _life; }
        set 
        {
            bool isBigger = value > _life;        
            if(value >= 0 && value <= MaxLife) _life = value;            
            GameUI.instance.UpdateLifeHUD(isBigger);
        }
    }

    public bool IsGameOver
    {
        get; private set;
    }

    public bool InsaneLevel
    {
        get
        {
            return _insaneLevel;
        }
        private set
        {
            _insaneLevel = value;
        }
    }

    public bool LostGameRestarted
    {
        get
        {
            return restartedGame;
        }
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
        nextLevel = 5;
        level = 1;      

        if(EnemySpawner.instance.maxEnemiesCurrent > 
            EnemySpawner.instance.maxEnemiesTotal)
                EnemySpawner.instance.maxEnemiesCurrent = 1;
    }

    void Update()
	{        
        if(Score == nextLevel && !InsaneLevel)
        {            
            ChangeLevel();
        }               

        if(canGameOver && !IsGameOver && Life == 0)
        {
            IsGameOver = true;
            GameUI.instance.ToggleGameOverMenu(true);            
        }             		   
    }

	void FixedUpdate()
	{
        if(!IsGameOver)
            timer += Time.fixedDeltaTime;    
    }

    private void ActivateInsaneMode()
    {
        InsaneLevel = true;        
        EnemySpawner.instance.minTimeSpawn = minTimeSpawnLimit;
        EnemySpawner.instance.maxTimeSpawn = 0.5f;
        EnemySpawner.instance.maxEnemiesCurrent = 
            EnemySpawner.instance.maxEnemiesTotal;

        sunglasses.SetBool("Sunglasses", true);
    }

    private void IncreaseEnemyAmount()
    {        
        int current = EnemySpawner.instance.maxEnemiesCurrent;
        int total = EnemySpawner.instance.maxEnemiesTotal;
        if(current < total - 1)            
        {                     
            EnemySpawner.instance.maxEnemiesCurrent++;                             
            enemyRaise += 10;            
        }                            
    }

    public void ResumeLostGameplay(int numOfLives)
    {        
        Life = numOfLives;
        GameUI.instance.RestartLifeHUD();
        IsGameOver = false;
        restartedGame = true;
        ReturnEggsToPool();
        DestroyAllPigeons();
    }

    private void ReturnEggsToPool()
    {
        var eggs = GameObject.FindGameObjectsWithTag("Egg");

        for (int i = 0; i < eggs.Length; i++)
        {
            var eggScript = eggs[i].GetComponent<Egg>();
            if(eggScript.target != null)            
                ObjectPooler.Instance.ReturnToPool(ref eggs[i]);                            
        }        
    }

    private void DestroyAllPigeons()
    {
        var pigeons = GameObject.FindGameObjectsWithTag("Pigeon");        
        for (int i = 0; i < pigeons.Length; i++)
        {
            Destroy(pigeons[i]);            
        }
    }

    private void ChangeLevel()
    {
        level++;
        
        if(Score == enemyRaise) IncreaseEnemyAmount(); 
        else if(Score >= insaneLevelScore) ActivateInsaneMode();        

        if(level % 2 == 0)
        {
            if(EnemySpawner.instance.minTimeSpawn - minTimeSpawnDecreaseRate >= 
                                                            minTimeSpawnLimit)                                                             
                EnemySpawner.instance.minTimeSpawn -= minTimeSpawnDecreaseRate;
            
            if(EnemySpawner.instance.maxTimeSpawn - maxTimeSpawnDecreaseRate >= 
                                                            maxTimeSpawnLimit)                                                            
                EnemySpawner.instance.maxTimeSpawn -= maxTimeSpawnDecreaseRate;            
        }                        

        nextLevel += levelUpIncreaseRate;  
    }
}
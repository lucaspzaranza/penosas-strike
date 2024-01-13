using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using UnityEngine.SceneManagement;

public enum Difficulty
{
    Easy = 0,
    Medium = 1,
    Hard = 2
}

public class GameController : MonoBehaviour
{
    #region Variables
    public bool canGameOver;
    [HideInInspector] public bool lostLife;
    public int nextLevel;
    public int enemyRaise;
    public int level;
    public float timer;
    public float eggSpeed;
    public float pigeonSpeed;     
    public float timeToInvokeGameOver;     
    public GameObject bomb; 
    public Difficulty _gameDifficulty;

    public const int maxScore = 999;
    private const int insaneLevelScore = 150;
    private const int levelUpIncreaseRate = 5;
    private const float minTimeSpawnLimit = 0.4f;
    private const float minTimeSpawnDecreaseRate = 0.025f;
    private const float maxTimeSpawnLimit = 0.6f;
    private const float maxTimeSpawnDecreaseRate = 0.15f;
    public const float easySpeed = 1f;
    public const float mediumSpeed = 1.5f;
    public const float hardSpeed = 2f;

    private float initialMinTimeSpawn;
    private float initialMaxTimeSpawn;
    int initialMaxEnemiesCurrent;
    [SerializeField] private int _score;
    [SerializeField] [Range(0, 5)] private int _life;
    [SerializeField] private bool _showTouchFinger;
    private bool _insaneLevel;
    private bool restartedGame;
    private bool _isPaused;
    private bool newRecordTriggered;
    private Dictionary<string, string> achievements;

    public static GameController instance;
    #endregion

    #region Props
    
    public int MaxLife
    {
        get { return 5; }
    }

    public Difficulty GameDifficulty
    {
        get { return _gameDifficulty;}
        set 
        { 
            _gameDifficulty = value;

            if(value == Difficulty.Easy)
			{
				GameController.instance.pigeonSpeed = GameController.easySpeed;				
				PlayerPrefs.SetInt("Difficulty", 0);
			}
			else if(value == Difficulty.Medium)
			{
				GameController.instance.pigeonSpeed = GameController.mediumSpeed;				
				PlayerPrefs.SetInt("Difficulty", 1);
			}
			else if(value == Difficulty.Hard)
			{
				GameController.instance.pigeonSpeed = GameController.hardSpeed;				
				PlayerPrefs.SetInt("Difficulty", 2);
			}
        }
    }

    public int Score
    {
        get { return _score; }
        set
        {
            if (value <= maxScore)
            {
                _score = value;
                GameUI.instance.UpdateScore(_score);
                if (value == maxScore)
                    GameUI.instance.CallGetALifeMenu();
                else if (_score > 0 && _score % 100 == 0)
                {
                    GameUI.instance.DeleteUIMessages(x => x);
                    StartCoroutine(GameUI.instance.InstantiateUIMessage(GameUI.instance.lifePlus));                                     
                    Life++;
                    SoundManager.instance.PlayAudio(ref SoundManager.instance.lifePlus);
                }              

                if(value > Record)
                {
                    Record = Score;
                    if(!newRecordTriggered)
                    {                                                
                        GameUI.instance.DeleteUIMessages(x => !x.name.Contains("Life +1") && !x.name.Contains("Record"));
                        StartCoroutine(GameUI.instance.InstantiateUIMessage(GameUI.instance.newRecord));                                                                                            
                        newRecordTriggered = true;
                        SoundManager.instance.PlayAudio(ref SoundManager.instance.newRecord);
                    }
                }    

                ScoreAchievements(value);   
                ScoreLeaderboard(value);                
            }
        }
    }

    public int Life
    {
        get { return _life; }
        set
        {
            if (value >= 0 && value <= MaxLife) _life = value;
            GameUI.instance.UpdateLifeHUD(_life);
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

    public bool IsPaused
    {
        get { return _isPaused; }
        set { _isPaused = value; }
    }

    [SerializeField] private bool _christmas;
    public bool IsChristmas
    {
        get { return _christmas; }
        private set { _christmas = value; }
    }

    public int Record
    {
        get
        {         
            return PlayerPrefs.GetInt(GameDifficulty.ToString() + " Record");
        }
        set
        {
            PlayerPrefs.SetInt(GameDifficulty.ToString() + " Record", value);
        }        
    }

    public bool FirstPlay
    {
        get{ return PlayerPrefs.GetInt("FirstPlay") == 0;}
    }

    public bool Authenticated { get; private set; }

    public bool ShowTouchFinger { get { return _showTouchFinger;}}

    #endregion

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    void Start()
    {           
        Screen.orientation = ScreenOrientation.Portrait;                     
        level = 1;

        if (EnemySpawner.instance.maxEnemiesCurrent >
            EnemySpawner.instance.maxEnemiesTotal)
            EnemySpawner.instance.maxEnemiesCurrent = 1;

        initialMinTimeSpawn = EnemySpawner.instance.minTimeSpawn;
        initialMaxTimeSpawn = EnemySpawner.instance.maxTimeSpawn;
        initialMaxEnemiesCurrent = EnemySpawner.instance.maxEnemiesCurrent;

        if(FirstPlay)        
            GameDifficulty = Difficulty.Medium;                    
        else
            LoadGameDifficulty();

        UpdateAchievementsID();

        //pigeonSpeed = 0.75f;    
        GooglePlayAuthentication();   
    }

    private void GooglePlayAuthentication()
    {                           
        PlayGamesPlatform.Activate();     
        Social.localUser.Authenticate((bool success) => 
        {          
              
            Authenticated = success;
            if(success) Debug.Log("Autenticado!");
            else Debug.Log("Erro na autenticação.");
        });        
    }

    void Update()
    {
        if (Score >= nextLevel && !InsaneLevel)        
            ChangeLevel();

        if (canGameOver && !IsGameOver && Life == 0)        
            GameOver();
    }

    void FixedUpdate()
    {
        if (EnemySpawner.instance.enabled && !IsGameOver)
            timer += Time.fixedDeltaTime;
    }
    
    private void ScoreLeaderboard(int value)
    {
        string leaderboard = PenosasStrikeGPS.leaderboard_medium;
        if(GameDifficulty == Difficulty.Easy) leaderboard = PenosasStrikeGPS.leaderboard_easy;
        else if(GameDifficulty == Difficulty.Hard) leaderboard = PenosasStrikeGPS.leaderboard_hard;
        ReportScore(value, leaderboard);
    }

    private void ScoreAchievements(int value)
    {
        if(value == 25) UnlockAchievement(PenosasStrikeGPS.achievement_got_25_points);

        if(newRecordTriggered) UnlockAchievement(PenosasStrikeGPS.achievement_broke_a_record);
           
        bool isMediumOrHard = GameDifficulty == Difficulty.Medium || GameDifficulty == Difficulty.Hard;

        if(isMediumOrHard)
        {            
            if(value == 50)        
                UnlockAchievement(PenosasStrikeGPS.achievement_got_50_points);                                               
            else if(value == 100)
            {            
                UnlockAchievement(PenosasStrikeGPS.achievement_got_100_points);
                UnlockAchievement(PenosasStrikeGPS.achievement_earned_a_life);                                                          
            }
            else if(value == 150)
            {                        
                if(GameDifficulty == Difficulty.Medium)
                    UnlockAchievement(PenosasStrikeGPS.achievement_insane_mode_activated);
                else if(GameDifficulty == Difficulty.Hard)
                    UnlockAchievement(PenosasStrikeGPS.achievement_insane_mode_activated_hard);                                                                                 

                if(!lostLife)
                    UnlockAchievement(PenosasStrikeGPS.achievement_flawless_insane_mode);            
            }
            else if(value == 300)            
                UnlockAchievement(PenosasStrikeGPS.achievement_got_300_points);                                             
            else if(value == 500)            
                UnlockAchievement(PenosasStrikeGPS.achievement_got_500_points);                                                 
            else if(value == maxScore)            
                UnlockAchievement(PenosasStrikeGPS.achievement_got_999_points);                                                                              
        }
    }

    private void GameOver()
    {
        IsGameOver = true;     
        SoundManager.instance.PlayAudio(ref SoundManager.instance.gameOver);
        DestroyAllEggs();		
        ExplodeAllPigeons();
        
        GameUI.instance.ToggleInGameMenu(false);
        Instantiate(bomb);
        SoundManager.instance.PlayAudio(ref SoundManager.instance.bombFall);
        PlayerPrefs.SetInt("FirstPlay", 1);      
        Invoke("CallGameOverMenu", timeToInvokeGameOver);
    }

    public void CallGameOverMenu()
    {
        GameUI.instance.ToggleGameOverScoreMenu(true);

        if(!LostGameRestarted) GameUI.instance.ToggleAdvertisingMenu(true);
        else GameUI.instance.ToggleGameOverMenu(true);
            
        if(newRecordTriggered)
        {
            GameUI.instance.recordAnimation.SetActive(true);
            AlignRecordAnimation();
        }
    }

    private void AlignRecordAnimation()
    {
        float YCoord = GameUI.instance.recordAnimation.transform.position.y;
        Transform record = GameUI.instance.recordAnimation.transform;

        if(GameUI.instance.gameOverScore.text.Length == 1)            
            record.position = new Vector2(1.015625f, YCoord);

        else if(GameUI.instance.gameOverScore.text.Length == 2)
            record.position = new Vector2(1.171875f, YCoord);

        else if(GameUI.instance.gameOverScore.text.Length == 3)
            record.position = new Vector2(1.367188f, YCoord);            
    }

    private void ExplodeAllPigeons()
    {
        var crosshairs = GameObject.FindGameObjectsWithTag("Crosshair");
        var pigeons = Object.FindObjectsOfType<SplineWalker>().Select(x => x.GetComponent<Animator>()).ToArray();        
        int explodeTrigger = Animator.StringToHash("Explode");        

        foreach (var pigeon in pigeons)
        {
            pigeon.SetTrigger(explodeTrigger);
            var script = pigeon.GetComponent<SplineWalker>();
            script.forceStop = true;
        }

        foreach (var crosshair in crosshairs)
        {
            Destroy(crosshair.gameObject);
        }
    }

    private void ActivateInsaneMode()
    {
        InsaneLevel = true;
        EnemySpawner.instance.minTimeSpawn = minTimeSpawnLimit;
        EnemySpawner.instance.maxTimeSpawn = 0.5f;
        EnemySpawner.instance.maxEnemiesCurrent =
            EnemySpawner.instance.maxEnemiesTotal;
        
        GameUI.instance.potty.SetActive(false);
        GameUI.instance.dastardlyHat.SetActive(true);
        GameUI.instance.DeleteUIMessages(x => !x.name.Contains("Record"));        
        StartCoroutine(GameUI.instance.InstantiateUIMessage(GameUI.instance.insaneMode));      

        SoundManager.instance.PlayAudio(ref SoundManager.instance.insaneMode);
    }

    private void IncreaseEnemyAmount()
    {
        int current = EnemySpawner.instance.maxEnemiesCurrent;
        int total = EnemySpawner.instance.maxEnemiesTotal;
        if (current < total - 1)
        {
            EnemySpawner.instance.maxEnemiesCurrent++;
            enemyRaise += 10;
        }
    }

    public IEnumerator ContinueLostGameplay(int numOfLives)
    {
        Machineggun.instance.cannon.transform.rotation = new Quaternion(0f, 0f, 180f, 1f);
        Penosa.instance.gameObject.SetActive(true);
        Penosa.instance.roastedChicken.SetActive(false);
        Penosa.instance.ghostChicken.SetActive(false);

        while (GameCountdown.instance.TimeInt > -1)
        {
            yield return new WaitForEndOfFrame();
        }

        Life = numOfLives;
        IsGameOver = false;
        restartedGame = true;
    }

    public void ResetGame()
    {
        if (IsPaused)
        {
            IsPaused = false;
            Time.timeScale = 1f;
        }
        EnemySpawner.instance.minTimeSpawn = initialMinTimeSpawn;
        EnemySpawner.instance.maxTimeSpawn = initialMaxTimeSpawn;
        EnemySpawner.instance.maxEnemiesCurrent = initialMaxEnemiesCurrent;
        EnemySpawner.instance.enabled = false;          
        DestroyAllEggs();
        ExplodeAllPigeons();
        timer = 0f;
        Score = 0;
        IsGameOver = false;
        restartedGame = false;
        level = 1;
        nextLevel = 5;
        enemyRaise = 10;
        Life = MaxLife;
        Machineggun.instance.cannon.transform.rotation = new Quaternion(0f, 0f, 180f, 1f);
        GameUI.instance.TogglePenosa(true);  
        if(GameUI.instance.getALifeMenu.activeSelf)                         
            GameUI.instance.getALifeMenu.SetActive(false);

        if(GameUI.instance.dastardlyHat.activeInHierarchy)
        {
            GameUI.instance.dastardlyHat.SetActive(false);
            GameUI.instance.potty.SetActive(true);
        }
    }

    public void ReturnEggsToPool()
    {
        print("Returning eggs to Pool...");
        var eggs = GameObject.FindGameObjectsWithTag("Egg");

        for (int i = 0; i < eggs.Length; i++)
        {
            var eggScript = eggs[i].GetComponent<Egg>();
            if (eggScript.target != null)
                ObjectPooler.Instance.ReturnToPool(ref eggs[i]);
        }
    }

    public void DestroyAllEggs()
    {        
        var eggs = GameObject.FindGameObjectsWithTag("Egg");

        foreach(var egg in eggs)
        {
            var eggScript = egg.GetComponent<Egg>();
            var crackedEgg = Instantiate(eggScript.eggParts) as GameObject;
        
            crackedEgg.transform.position = egg.transform.position;
            Destroy(egg.gameObject);
        }
    }

    public void DestroyAllPigeons()
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

        if (Score >= enemyRaise && Score < insaneLevelScore) IncreaseEnemyAmount();
        if (Score >= insaneLevelScore) ActivateInsaneMode();

        if (level % 2 == 0)
        {
            if (EnemySpawner.instance.minTimeSpawn - minTimeSpawnDecreaseRate >=
                                                            minTimeSpawnLimit)
                EnemySpawner.instance.minTimeSpawn -= minTimeSpawnDecreaseRate;

            if (EnemySpawner.instance.maxTimeSpawn - maxTimeSpawnDecreaseRate >=
                                                            maxTimeSpawnLimit)
                EnemySpawner.instance.maxTimeSpawn -= maxTimeSpawnDecreaseRate;
        }

        nextLevel += levelUpIncreaseRate;
    }   

    private void LoadGameDifficulty() 
    {
        if(PlayerPrefs.GetInt("Difficulty") == 0)                
            GameController.instance.GameDifficulty = Difficulty.Easy;    

        else if(PlayerPrefs.GetInt("Difficulty") == 1)        
            GameController.instance.GameDifficulty = Difficulty.Medium;    
        
        else if(PlayerPrefs.GetInt("Difficulty") == 2)            
            GameController.instance.GameDifficulty = Difficulty.Hard;                            
    }   

    public void UnlockAchievement(string achievementID)
    {
        Social.ReportProgress(achievementID, 100f, (bool success) => 
        {
            Debug.Log("Achievement unlocked! "  + achievements[achievementID]);
        });    
    }

    public void ReportScore(int score, string leaderboardID)
    {            
        Social.ReportScore(score, leaderboardID, (bool success) =>
        {
            Debug.Log("Score reported to leaderboard. " + success.ToString());
        });
    }


    private void OnApplicationQuit() 
    {             
        PlayerPrefs.SetInt("Difficulty", 1);
        PlayerPrefs.SetFloat("SFXVol", 1f);
    }

    // Função para fins de Debug
    private void UpdateAchievementsID()
    {
        achievements = new Dictionary<string, string>()
        {
            { PenosasStrikeGPS.achievement_broke_a_record, "Broke a Record"},
            { PenosasStrikeGPS.achievement_earned_a_life, "Earned a Life"},
            { PenosasStrikeGPS.achievement_got_25_points, "Got 25 points"},
            { PenosasStrikeGPS.achievement_got_50_points, "Got 50 points"},
            { PenosasStrikeGPS.achievement_got_100_points, "Got 100 points"},
            { PenosasStrikeGPS.achievement_insane_mode_activated, "Insane Mode Activated"},
            { PenosasStrikeGPS.achievement_insane_mode_activated_hard, "Hard Insane Mode"},
            { PenosasStrikeGPS.achievement_flawless_insane_mode, "Flawless Insane Mode"},
            { PenosasStrikeGPS.achievement_got_300_points, "Got 300 points"},
            { PenosasStrikeGPS.achievement_got_500_points, "Got 500 points"},
            { PenosasStrikeGPS.achievement_got_999_points, "Got 999 points"}
        };                                
    }
}
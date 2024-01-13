using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Linq;

public class GameUI : MonoBehaviour
{
    #region Vars
    public static GameUI instance;
    public GameObject penosa;
    public GameObject initialMenu;
    public GameObject howToPlayMenu;
    public GameObject inGameMenu;
    public GameObject gameOverMenu;
    public GameObject recordMenu;
    public GameObject resetConfirmationMenu;
    public GameObject getALifeMenu;
    public GameObject adsMenu;
    public GameObject optionsMenu;
    public GameObject adsButton;
    public GameObject countdown;
    public GameObject resetButton;
    public GameObject pauseText;
    public GameObject miss;
    public GameObject insaneMode;
    public GameObject newRecord;
    public GameObject potty;
    public GameObject dastardlyHat;
    public GameObject plusOne;
    public GameObject plusTwo;
    public GameObject lifePlus;
    public GameObject noNetMenu;
    public GameObject restartBigButton;
    public GameObject restartSmallButton;
    public GameObject recordAnimation;
    public GameObject personalRecordMenu;
    public GameObject recordMainMenu;
    public GameObject gameOverScoreMenu;
    public GameObject recordBackBtn;
    public GameObject personalRecordBackBtn;
    public GameObject resetRecordBtn;
    public Button resumeMatchButton;    
    public Text gameScore;
    public Text gameOverScore;    
    public Text gameOverText;
    public ToggleGroup adsToggleGroup;  
    [SerializeField] private Image soundWaves;
    [SerializeField] private GameObject howToPlayBtn;
    [SerializeField] private GameObject optionsBtn;
    [SerializeField] private GameObject quitBtn;
    [SerializeField] private GameObject[] lives;   
    [SerializeField] private Slider SFXSlider;
    [SerializeField] private GameObject mainMenuButton;
    private const float timeToInvokeInitMenu = 0.025f;
    private int numOfLives;
    private bool isInGame;
    private Vector2 quitBtnInitPos;
    private Vector2 optionsBtnInitPos;
    #endregion

    private GameController GCtrl { get { return GameController.instance; }}

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    void Start()
    {
        UpdateLifeHUD(GameController.instance.Life);
        UpdateScore(GameController.instance.Score);     

        resumeMatchButton.interactable = false;	

        AddOnClickListener();  

        if(SoundManager.instance != null)
            SoundManager.instance.volume = PlayerPrefs.GetFloat("SFXVol");
    }

    void Update() 
    {
        if(GameController.instance.FirstPlay && howToPlayBtn.activeSelf) 
        {                       
            quitBtnInitPos = quitBtn.transform.position;
            optionsBtnInitPos = optionsBtn.transform.position;

            quitBtn.transform.position = optionsBtn.transform.position;
            optionsBtn.transform.position = howToPlayBtn.transform.position;
            howToPlayBtn.SetActive(false);                     
        }    
        else if(!GameController.instance.FirstPlay && !howToPlayBtn.activeSelf)
        {            
            optionsBtn.transform.position = optionsBtnInitPos;
            quitBtn.transform.position = quitBtnInitPos;
            howToPlayBtn.SetActive(true);
        }    
    }

    private void AddOnClickListener()
    {
        var buttons = Resources.FindObjectsOfTypeAll<Button>();
        foreach (var btn in buttons)
        {
            btn.onClick.AddListener(PlayClickSound);
        }
    }


    public void ChangeGameSkin(int scene)
    {  
        SceneManager.LoadScene(scene);
    }

    private void PlayClickSound()
    {
        SoundManager.instance.PlayAudio(ref SoundManager.instance.click);
    }

    public void ToggleOptionsMenu(bool value)
    {
        ToggleInitialMenu(!value);
        optionsMenu.SetActive(value);        
    }

    public void SetGameSFXVolume()
    {
        if(SoundManager.instance != null)
            SoundManager.instance.volume = SFXSlider.value;

        PlayerPrefs.SetFloat("SFXVol", SFXSlider.value);

        soundWaves.fillAmount = SFXSlider.value;
    }

    public void ToggleResetConfirmationMenu(bool value)
    {
        personalRecordBackBtn.SetActive(!value);
        resetConfirmationMenu.SetActive(value);
        if(recordBackBtn.activeSelf) recordBackBtn.SetActive(false);
    }

    public void ResetRecord()
    {
        PlayerPrefs.SetInt("Easy Record", 0);
        PlayerPrefs.SetInt("Medium Record", 0);
        PlayerPrefs.SetInt("Hard Record", 0);
    }

    public void SetInGameButtons(bool value)
    {
        resetButton.SetActive(value);
    }

    public void UpdateScore(int value)
    {
        gameScore.text = value.ToString();
    }

    public void TogglePenosa(bool value)
    {
        penosa.SetActive(value);
    }

    public void ToggleInitialMenu(bool value)
    {
        initialMenu.SetActive(value);
    }
 
    public void CallGetALifeMenu()
    {
        getALifeMenu.SetActive(true);
    }

    public void ToggleGameOverMenu(bool value)
    {
        if(value)
        {
            if(!gameOverScoreMenu.activeInHierarchy)
                gameOverScoreMenu.SetActive(true);
            gameOverScore.text = gameScore.text;
            gameOverText.text = LanguageController.instance.TryAGain;
        }
        gameOverMenu.SetActive(value);                
    }

    public void ToggleAdvertisingMenu(bool value)
    {       
        adsMenu.SetActive(value); 

        if (value)
        {                                
            gameOverScore.text = gameScore.text;            
            if (getALifeMenu.activeInHierarchy)
                getALifeMenu.SetActive(false);            
        }
        else                               
            EnemySpawner.instance.enabled = false;                       
    }

    public void ToggleGameOverScoreMenu(bool value)
    {
        gameOverScoreMenu.SetActive(value);
    }

    public void ToggleInGameMenu(bool value)
    {
        inGameMenu.SetActive(value);
    }

    public void StartGame()
    {        
        isInGame = true;
        ToggleInitialMenu(false);        
        if(GameController.instance.FirstPlay)
        {
            ToggleHowToPlayMenu(true);                                                  
            PlayerPrefs.SetInt("FirstPlay", 1);
        }            
        else        
            StartCoroutine(SetCountdownAndPenosa());
    }

    private IEnumerator SetCountdownAndPenosa()
    {
        TogglePenosa(true); 
        yield return new WaitForSeconds(timeToInvokeInitMenu);
        countdown.SetActive(true);                       
    }

    public void ToggleHowToPlayMenu(bool value)
    {
        if(!value)
        {                    
            if(isInGame)                                       
                StartCoroutine(SetCountdownAndPenosa());
            else ToggleInitialMenu(true);
        }
        howToPlayMenu.SetActive(value);
    }

    public void RestartGame()
    {        
        int scene = 0;
        if(SceneManager.sceneCountInBuildSettings == 4 && GCtrl.IsChristmas) scene = 2;
        SceneManager.LoadScene(scene);
    }

    public void BackToMainMenu()
    {        
        Time.timeScale = 1f;
        if(!GameController.instance.IsChristmas)
            SceneManager.LoadScene(0);
        else SceneManager.LoadScene(2);
    }

    public void CallTogglePauseMenu()
    {
        StartCoroutine(TogglePauseGame());
    }

    public void ToggleRecordMenu(bool value)
    {
        recordBackBtn.SetActive(value);
        recordMenu.SetActive(value);
    }

    public void ToggleRecordMainMenu(bool value)
    {
        recordMainMenu.SetActive(value);
    }

    public void TogglePersonalRecordMenu(bool value)
    {        
        recordBackBtn.SetActive(!value);
        personalRecordBackBtn.SetActive(value);
        resetRecordBtn.SetActive(value);
        personalRecordMenu.SetActive(value);        
    }

    public void CallAchievementsUI()
    {
        if(Social.localUser.authenticated)
            Social.ShowAchievementsUI();
    }

    public void CallLeaderboardUI()
    {
        if(Social.localUser.authenticated)        
            Social.ShowLeaderboardUI();        
    }

    public IEnumerator TogglePauseGame()
    {        
        if (!GameController.instance.IsPaused)
        {
            if(getALifeMenu.activeInHierarchy) getALifeMenu.SetActive(false);

            resetButton.SetActive(true);
            mainMenuButton.SetActive(true);
            GameController.instance.IsPaused = true;
            pauseText.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            if(GameController.instance.Score == GameController.maxScore && !getALifeMenu.activeInHierarchy)
                getALifeMenu.SetActive(true); 
            
            resetButton.SetActive(false);
            mainMenuButton.SetActive(false);
            pauseText.SetActive(false);
            Time.timeScale = 1f;
            countdown.SetActive(true);
            while (countdown.activeInHierarchy)
            {
                yield return new WaitForEndOfFrame();
            }           
            GameController.instance.IsPaused = false;
        }        
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetNumberOfLives(int newNumOfLives)
    {
        if(!resumeMatchButton.interactable)
            resumeMatchButton.interactable = true;        

        numOfLives = newNumOfLives;
        if(AdManager.instance != null)
            AdManager.instance.ChooseAd(numOfLives);            
    }

    public void ContinueGame()
    {                
        resumeMatchButton.interactable = false;
        countdown.SetActive(true);
        if(noNetMenu.activeInHierarchy) noNetMenu.SetActive(false);
        StartCoroutine(GameController.instance.ContinueLostGameplay(numOfLives));
    }

    public void CallResetGame()
    {       
        resetButton.SetActive(false);
        mainMenuButton.SetActive(false);
        if(pauseText.activeInHierarchy) pauseText.SetActive(false);
        countdown.SetActive(true);
        GameController.instance.ResetGame();
    }

    public void UpdateLifeHUD(int value)
    {
        for (int i = 0; i < lives.Length; i++)
        {
            bool active = i < value;
            lives[i].SetActive(active);
        }
    }   

    public void SetNoNetworkMenu(bool value)
    {
        noNetMenu.SetActive(value);
    }

    public void DeleteUIMessages(Func<GameObject, bool> predicate)
    {
        var messages = GameObject.FindGameObjectsWithTag("Game Messages").Where(predicate);

        if(messages != null && messages.Count() > 0)
        {                       
            foreach (var msg in messages)
            {
                Destroy(msg.gameObject);
            }
        }        
    }

    public IEnumerator InstantiateUIMessage(GameObject newUIMsg)
    {        
        var UIMsgsInScene = GameObject.FindGameObjectsWithTag("Game Messages");
        var UIGameOBjs = UIMsgsInScene.Where(x => !x.name.Contains("Miss")).ToArray();   
    
        if(!newUIMsg.name.Contains("Miss"))
        {
            while(UIGameOBjs.Count(x => x != null) > 0)
            {                                
                yield return new WaitForEndOfFrame();
            }        
        
            Instantiate(newUIMsg);            
        }
        else
        {                        
            var newMiss = Instantiate(miss) as GameObject;
            SoundManager.instance.PlayAudio(ref SoundManager.instance.miss);
            if(UIGameOBjs.Count(x => x != null) > 0)                        
                newMiss.transform.position = new Vector2(0, 0);                        
        }
    }   

    public void OnAchievementsClick()
    {
        if(Social.localUser.authenticated)
        {
            Social.ShowAchievementsUI();
        }
    }

    public void OnLeaderboardClick()
    {
        if(Social.localUser.authenticated)
        {
            Social.ShowLeaderboardUI();
        }
    }            
}
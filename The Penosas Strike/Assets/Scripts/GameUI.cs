using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour 
{
    #region Vars
    public static GameUI instance;
    public GameObject inGameMenu;
    public GameObject gameOverMenu;
    public GameObject getALifeMenu;
    public GameObject adsMenu;
    public GameObject adsButton;
    public GameObject countdown;
    public GameObject resetButton;
    public Text gameScore;
    public Text pauseButtonTxt;
    public GameObject initialMenu;
    [SerializeField] private GameObject[] lives;
    private int counter = 0;
    private int numOfLives;
    private bool isPaused = false;
    #endregion

    void Awake()
	{
		if(instance == null)
            instance = this;
		else if(instance != this)
            Destroy(gameObject);
    }
        
    void Start()
    {                
        StartLifeHUD();       
    }

	public void ToggleGameOverMenu(bool value)
	{
        if(!value)
            EnemySpawner.instance.enabled = false;

        if(value && getALifeMenu.activeInHierarchy) 
            getALifeMenu.SetActive(false);

        gameOverMenu.gameObject.SetActive(value);
        DeactivateInGameButtons();

        if(GameController.instance.LostGameRestarted)
            adsButton.SetActive(false);
    }

    private void DeactivateInGameButtons()
    {
        if(resetButton.activeSelf)
        {
            pauseButtonTxt.transform.parent.gameObject.SetActive(false);
            resetButton.SetActive(false);
        }       
    }

    public void UpdateScore(int value)
    {
        gameScore.text = value.ToString();
    }

    public void ToggleInitialMenu(bool value)
    {
        initialMenu.SetActive(value);
    }

    public void CallGetALifeMenu()
    {
        getALifeMenu.SetActive(true);   
    }

    public void ToggleAdvertisingMenu(bool value)
    {
        adsMenu.SetActive(value);
    }

    public void StartLifeHUD()
    {
        counter = GameController.instance.Life;        
        for (int i = lives.Length - 1; i >= counter; i--)
        {
            lives[i].SetActive(false);
        }        
    }

    public void RestartLifeHUD()
    {
        counter = GameController.instance.Life;
        for (int i = 0; i < counter; i++)
        {
            lives[i].SetActive(true);
        }
    }

    public void ToggleInGameMenu(bool value)
    {
        inGameMenu.SetActive(value);
    }

    public void StartGame()
    {                
        ToggleInitialMenu(false);
        countdown.SetActive(true);
    }

    public void RestartGame()
    {        
        SceneManager.LoadScene(0);
    }

    public void TogglePauseGame()
    {
        if(!isPaused)
        {
            isPaused = true;
            Time.timeScale = 0f;
            pauseButtonTxt.text = "Resume";
        }
        else
        {
            isPaused = false;
            Time.timeScale = 1f;
            pauseButtonTxt.text = "Pause";
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetNumberOfLives(int newNumOfLives)
    {
        numOfLives = newNumOfLives;
    }

    public void ResumeGame()
    {
        countdown.SetActive(true);        
        StartCoroutine(GameController.instance.ResumeLostGameplay(numOfLives));

        if(!resetButton.activeSelf)
        {
            pauseButtonTxt.transform.parent.gameObject.SetActive(true);
            resetButton.SetActive(true);
        }
    }

    public void CallResetGame()
    {
        countdown.SetActive(true);
        StartCoroutine(GameController.instance.ResetGame());
    }

    public void UpdateLifeHUD(bool value)
    {                      
        if(value)
        {                        
            if(counter < lives.Length) lives[counter].SetActive(value);                 
            counter = GameController.instance.Life;        
        }
        else
        {
            counter = GameController.instance.Life;            
            if(counter >= 0 && counter < lives.Length) 
                lives[counter].SetActive(value);                
        }                                         
    }          
}
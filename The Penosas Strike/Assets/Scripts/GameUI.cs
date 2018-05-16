using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour 
{
    #region Vars
    public static GameUI instance;
    public GameObject gameOverMenu;
    public GameObject getALifeMenu;
    public GameObject adsMenu;
    public GameObject adsButton;
    public Text gameScore;    
    [SerializeField] private GameObject[] lives;
    private int counter = 0;
    private int numOfLives;
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
        if(value && getALifeMenu.activeInHierarchy) 
            getALifeMenu.SetActive(false);

        gameOverMenu.gameObject.SetActive(value);

        if(GameController.instance.LostGameRestarted)
            adsButton.SetActive(false);
    }

    public void UpdateScore(int value)
    {
        gameScore.text = value.ToString();
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

    public void RestartGame()
    {        
        SceneManager.LoadScene(0);
    }

    public void SetNumberOfLives(int newNumOfLives)
    {
        numOfLives = newNumOfLives;
    }

    public void ResumeGame()
    {
        GameController.instance.ResumeLostGameplay(numOfLives);
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
            if(counter >= 0) lives[counter].SetActive(value);                
        }                                         
    }          
}
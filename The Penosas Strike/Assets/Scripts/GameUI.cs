using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameUI : MonoBehaviour 
{
    public static GameUI instance;
    public GameObject gameOverMenu;
    public GameObject getALifeMenu;
    public Text gameScore;    
    [SerializeField] private GameObject[] lives;
    private int counter = 0; 

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
    }

    public void RestartGame()
    {        
        SceneManager.LoadScene(0);
    }

    public void UpdateScore(int value)
    {                    
        gameScore.text = value.ToString();
    }

    public void CallGetALifeMenu()
    {
        getALifeMenu.SetActive(true);   
    }

    public void StartLifeHUD()
    {              
        counter = GameController.instance.MaxLife - GameController.instance.Life;        
        for (int i = 0; i < counter; i++)
        {
            lives[i].SetActive(false);
        }
    }

    public void UpdateLifeHUD(bool value)
    {
        if(value && counter - 1 >= 0) counter--;            
        else if(value && counter + 1 < lives.Length) counter++;

        lives[counter].SetActive(value);
    }
}
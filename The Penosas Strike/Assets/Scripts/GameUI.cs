using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour 
{
    public static GameUI instance;
    public GameObject gameOverMenu;
    public Text gameScore;

    void Awake()
	{
		if(instance == null)
            instance = this;
		else if(instance != this)
            Destroy(gameObject);
    }

	public void ToggleGameOverMenu(bool value)
	{
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
}
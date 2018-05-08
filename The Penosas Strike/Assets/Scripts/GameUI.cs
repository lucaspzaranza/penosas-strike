using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour 
{
    public static GameUI instance;
    public GameObject gameOverMenu;

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
}

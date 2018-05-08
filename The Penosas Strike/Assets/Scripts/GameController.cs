using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour 
{
	#region Variables
	public int score;
    public int losses;
    public int nextLevelPontuation;
    public float timer;
    public float eggSpeed;
    public float pigeonSpeed;

    private bool changePigeonAmount = true;
    private bool changeTimeSpawn = false;

    public static GameController instance;
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
        EnemySpawner.instance.maxEnemies = 1;
        EnemySpawner.instance.timeToSpawnEnemy = 0.5f;
        pigeonSpeed = 0.7f;
        nextLevelPontuation = 5;
    }

    void Update()
	{
		if(score == nextLevelPontuation)
        {
            print("Aloha!");
            ChangeLevel();
        }

        if(losses == 5)
        {
            GameUI.instance.ToggleGameOverMenu(true);
            Time.timeScale = 0f;
        }
	}

	void FixedUpdate()
	{
        timer += Time.fixedDeltaTime;
    }

    private void ChangeLevel()
    {
        if(changePigeonAmount && EnemySpawner.instance.maxEnemies < 10)
            EnemySpawner.instance.maxEnemies++;
        
        if(nextLevelPontuation < 100)
            nextLevelPontuation *= 2;
        else
            nextLevelPontuation += 50;

        if(pigeonSpeed < 1f)
            pigeonSpeed += 0.02f;

        if(changeTimeSpawn && EnemySpawner.instance.timeToSpawnEnemy > 0.1f)
            EnemySpawner.instance.timeToSpawnEnemy -= 0.05f;

        changePigeonAmount = !changePigeonAmount;
        changeTimeSpawn = !changeTimeSpawn;
    }
}

/*
    Level 1: 1 pombo, velocidade 0.7, spawn rate 0.5
    Level impossible: 10 pombos, velocidade 1 spawn rate 0.1
*/
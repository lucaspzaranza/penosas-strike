using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour 
{
	#region Variables
	public int score;
    public int losses;
    public float timer;
    public float eggSpeed;
    public float pigeonSpeed;

    public static GameController instance;
    #endregion

    void Awake()
	{
		if(instance == null)
            instance = this;
		else
            Destroy(this.gameObject);
    }

    void Update()
	{
		// Fazer os bagulho sinistro pra aumentar a dificuldade do game...
	}

	void FixedUpdate()
	{
        timer += Time.fixedDeltaTime;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPlayController : MonoBehaviour 
{
	public static FirstPlayController instance;

	void Awake()
	{	
		if(instance == null)
			instance = this;
		else
			Destroy(this.gameObject);

		DontDestroyOnLoad(this.gameObject);		
	}

	void Start()
	{
		PlayerPrefs.SetInt("FirstPlay", 0);
	}
}

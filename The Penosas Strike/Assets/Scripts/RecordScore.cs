using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordScore : MonoBehaviour 
{
	[SerializeField] private Text score;
	[SerializeField] private Difficulty difficulty;	

	private void OnEnable() 
	{		
		score.text = PlayerPrefs.GetInt(difficulty.ToString() + " Record").ToString();
	}
}
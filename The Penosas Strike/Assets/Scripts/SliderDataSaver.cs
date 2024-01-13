using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderDataSaver : MonoBehaviour 
{
	private Slider slider;
	void Start() 
	{
		slider = GetComponent<Slider>();

		if(GameController.instance.FirstPlay)	
			PlayerPrefs.SetFloat("SFXVol", 1f);
		
		slider.value = PlayerPrefs.GetFloat("SFXVol");
	}	
}

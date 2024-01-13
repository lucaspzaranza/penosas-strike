using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionTogglePosition : MonoBehaviour 
{

	[SerializeField] private float XCoord;
	private float YCoord;

	void Start()
	{
		YCoord = transform.position.y;
	}	

	void Update () 
	{				
		if(LanguageController.instance.IsEnglish)		
			transform.position = new Vector2(XCoord, YCoord);						
	}
}

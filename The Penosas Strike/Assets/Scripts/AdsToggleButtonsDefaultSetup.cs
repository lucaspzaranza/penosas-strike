using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsToggleButtonsDefaultSetup : MonoBehaviour 
{
	public void ToggleButtonsDefaultSetup()
	{
		GameUI.instance.adsToggleGroup.allowSwitchOff = true;
		GameUI.instance.adsToggleGroup.SetAllTogglesOff();
		GameUI.instance.adsToggleGroup.allowSwitchOff = false;        

		if(GameUI.instance.resumeMatchButton.interactable)
			GameUI.instance.resumeMatchButton.interactable = false;	
	}
}
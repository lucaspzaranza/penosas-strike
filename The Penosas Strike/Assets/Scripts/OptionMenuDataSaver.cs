using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionMenuDataSaver : MonoBehaviour 
{
	public static OptionMenuDataSaver instance;
	public Button[] difficultyToggles;		
	public Button[] gameSkinToggles;
	public Difficulty savedDifficulty;
	public bool isDefaultSkin;
	[HideInInspector] public bool gameSkinChanged;
	private int buttonIndex { get { return PlayerPrefs.GetInt("Difficulty");}}
	private int GameSkinIndex { get { if(isDefaultSkin) return 0; else return 1;}}

	private void Awake() 
	{
		if(instance == null)	
			instance = this;
		else
			Destroy(this.gameObject);
	}	

	private void Start()
	{
		isDefaultSkin = !GameController.instance.IsChristmas;
	}

	public void SetAndSaveGameDifficulty(string difficulty)
	{
		if(GameController.instance != null)
		{
			var pointer = new PointerEventData(EventSystem.current);			
			ExecuteEvents.Execute(difficultyToggles[buttonIndex].gameObject, pointer, ExecuteEvents.pointerUpHandler);

			if(difficulty == "Easy")
				GameController.instance.GameDifficulty = Difficulty.Easy;

			else if(difficulty == "Medium")
				GameController.instance.GameDifficulty = Difficulty.Medium;

			else if( difficulty == "Hard")
				GameController.instance.GameDifficulty = Difficulty.Hard;

			savedDifficulty = GameController.instance.GameDifficulty;
		}	
	}

	public void SetAndSaveGameSkin(bool defaultSkin)
	{
		var pointer = new PointerEventData(EventSystem.current);			
		ExecuteEvents.Execute(gameSkinToggles[GameSkinIndex].gameObject, pointer, ExecuteEvents.pointerUpHandler);
		if(isDefaultSkin != defaultSkin)
		{
			isDefaultSkin = defaultSkin;
			gameSkinChanged = true;
		} 
	}
	
	void Update() 
	{		
		SelectDifficultyToggleButton();
		SelectGameSkinToggle();
	}

	private void SelectDifficultyToggleButton()
	{		
		var pointer = new PointerEventData(EventSystem.current);								

		if(GameController.instance != null)
		{			
			ExecuteEvents.Execute(difficultyToggles[buttonIndex].gameObject, pointer, ExecuteEvents.pointerEnterHandler);
			ExecuteEvents.Execute(difficultyToggles[buttonIndex].gameObject, pointer, ExecuteEvents.pointerDownHandler);													
		}
	}

	private void SelectGameSkinToggle()
	{
		var pointer = new PointerEventData(EventSystem.current);	

		if(GameController.instance != null)
		{			
			ExecuteEvents.Execute(gameSkinToggles[GameSkinIndex].gameObject, pointer, ExecuteEvents.pointerEnterHandler);
			ExecuteEvents.Execute(gameSkinToggles[GameSkinIndex].gameObject, pointer, ExecuteEvents.pointerDownHandler);													
		}
	}
}
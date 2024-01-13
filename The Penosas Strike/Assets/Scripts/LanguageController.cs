using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageController : MonoBehaviour 
{
	public static LanguageController instance;
	public bool forceEnglish;
	[SerializeField] private Language Portuguese;
	[SerializeField] private Language English;
	
	#region Game Texts
	[SerializeField] private Text startBtn;
	[SerializeField] private Text recordBtn;
	[SerializeField] private Text howToPlayBtn;
	[SerializeField] private Text quitBtn;	
	[SerializeField] private Text gameOverRestart;
	[SerializeField] private Text resumeLostMatchBtn;
	[SerializeField] private Text restartMatchBtn;
	[SerializeField] private Text chooseYourChances;
	[SerializeField] private Text oneLife;
	[SerializeField] private Text twoLives;
	[SerializeField] private Text threeLives;
	[SerializeField] private Text continueMatchBtn;	
	[SerializeField] private Text backBtn;
	[SerializeField] private Text resetBtn;
	[SerializeField] private Text areYouSure;
	[SerializeField] private Text yes;
	[SerializeField] private Text no;
	[SerializeField] private Text tapThePigeons;
	[SerializeField] private Text beFast;
	[SerializeField] private Text insaneMode;
	[SerializeField] private Text gotItBtn;	
	[SerializeField] private Text getALife;
	[SerializeField] private Text neverMind;
	[SerializeField] private Text noNetwork;
	[SerializeField] private Text noNetPlayAnywayBtn;
	[SerializeField] private Text difficulty;
	[SerializeField] private Text easy;
	[SerializeField] private Text medium;
	[SerializeField] private Text hard;	
	[SerializeField] private Text sfxVolume;
	[SerializeField] private Text optionsBackBtn;
	[SerializeField] private Text optionsBtn;
	[SerializeField] private Text easyRecordMenuTxt;
	[SerializeField] private Text mediumRecordMenuTxt;
	[SerializeField] private Text hardRecordMenuTxt; 
	[SerializeField] private Text livesMenuTxt;
	[SerializeField] private Text restartBigButtonTxt;
	[SerializeField] private Text mainMenuBtnTxt;
	[SerializeField] private Text personalRecordMenuBackBtn;
	[SerializeField] private Text personalRecordBtn;
	[SerializeField] private Text leaderboardBtn;
	[SerializeField] private Text achievementsBtn;
	[SerializeField] private Text gameSkinBtn;
	[SerializeField] private Text defaultSkinBtn;
	[SerializeField] private Text christmasSkinBtn;

	#endregion

	#region Props

	private string _resumeGame;
	public string ResumeGame
	{
		get { return _resumeGame;}
		private set { _resumeGame = value;}
	}

	private string _tryAgain;
	public string TryAGain
	{
		get { return _tryAgain;}
		private set { _tryAgain = value;}
	}

	public string PauseGameBtn{ get; private set; }	

	public bool IsEnglish 
	{ 
		get 
		{ 
			bool result = Application.systemLanguage != SystemLanguage.Portuguese;
			result |= forceEnglish;
			return result;
		}
	}

	#endregion

	private Language currentLang;
	private void Awake() 
	{
		if(instance == null) instance = this;
		else if(instance !=  null) Destroy(this.gameObject);		
		DontDestroyOnLoad(this);
			
		if(IsEnglish)	
		{
			currentLang = English;
			SetGameLanguage(currentLang);
			SetPropsLang(currentLang);
		}	
		else		
			SetPropsLang(Portuguese);
	}

	private void SetGameLanguage(Language newLang)
	{		
		print("Aloha!");
		startBtn.text = newLang.StartBtn;		
		recordBtn.text = newLang.RecordBtn;
		howToPlayBtn.text = newLang.HowToPlayBtn;
		quitBtn.text = newLang.QuitBtn;		
		gameOverRestart.text = newLang.GameOverRestart;
		resumeLostMatchBtn.text = newLang.ResumeLostMatchBtn;
		restartMatchBtn.text = newLang.RestartMatchBtn;
		chooseYourChances.text = newLang.ChooseYourChances;
		oneLife.text = newLang.OneLife;
		twoLives.text = newLang.TwoLives;
		threeLives.text = newLang.ThreeLives;
		continueMatchBtn.text = newLang.ContinueMatchBtn;		
		backBtn.text = newLang.BackBtn;
		resetBtn.text = newLang.ResetBtn;
		areYouSure.text = newLang.AreYouSure;
		yes.text = newLang.Yes;
		no.text = newLang.No;
		tapThePigeons.text = newLang.TapThePigeons;
		beFast.text = newLang.BeFast;
		insaneMode.text = newLang.InsaneMode;
		gotItBtn.text = newLang.GotItBtn;	
		getALife.text = newLang.GetALife;	
		neverMind.text = newLang.NeverMind;
		noNetwork.text = newLang.NoNetwork;
		noNetPlayAnywayBtn.text = newLang.NoNetPlayAnyway;
		difficulty.text = newLang.Difficulty;
		easy.text = newLang.Easy;
		medium.text = newLang.Medium;
		hard.text = newLang.Hard;
		sfxVolume.text = newLang.SFXVolume;	
		optionsBackBtn.text = newLang.BackBtn;
		optionsBtn.text = newLang.Options;	
		easyRecordMenuTxt.text = newLang.Easy;
		mediumRecordMenuTxt.text = newLang.Medium;
		hardRecordMenuTxt.text = newLang.Hard;	
		livesMenuTxt.text = newLang.LivesMenu;
		restartBigButtonTxt.text = newLang.RestartMatchBtn;
		mainMenuBtnTxt.text = newLang.MainMenuBtn;
		personalRecordMenuBackBtn.text = newLang.BackBtn;
		personalRecordBtn.text = newLang.PersonalRecordBtn;
		leaderboardBtn.text = newLang.LeaderboardBtn;
		achievementsBtn.text = newLang.AchievementsBtn;
		gameSkinBtn.text = newLang.GameSkin;
		defaultSkinBtn.text = newLang.DefaultSkin;
		christmasSkinBtn.text = newLang.ChristmasSkin;
	} 

	private void SetPropsLang(Language lang)
	{
		ResumeGame = lang.ResumeGame;
		TryAGain = lang.TryAgain;
	}
}

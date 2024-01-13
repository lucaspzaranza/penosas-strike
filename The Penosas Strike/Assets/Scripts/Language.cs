using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Language", menuName = "Language")]
public class Language : ScriptableObject 
{
	#region Private Vars
	[SerializeField] private string _startBtn;
	[SerializeField] private string _recordBtn;
	[SerializeField] private string _howToPlayBtn;
	[SerializeField] private string _quitBtn;	
	[SerializeField] private string _gameOverRestart;
	[SerializeField] private string _resumeLostMatchBtn;
	[SerializeField] private string _restartMatchBtn;
	[SerializeField] private string _chooseYourChances;
	[SerializeField] private string _oneLife;
	[SerializeField] private string _twoLives;
	[SerializeField] private string _threeLives;
	[SerializeField] private string _continueMatchBtn;	
	[SerializeField] private string _backBtn;
	[SerializeField] private string _resetBtn;
	[SerializeField] private string _areYouSure;
	[SerializeField] private string _yes;
	[SerializeField] private string _no;
	[SerializeField] private string _tapThePigeons;
	[SerializeField] private string _beFast;
	[SerializeField] private string _insaneMode;
	[SerializeField] private string _gotItBtn;	
	[SerializeField] private string _resumeGame;
	[SerializeField] private string _tryAgain;
	[SerializeField] private string _getALife;
	[SerializeField] private string _neverMind;
	[SerializeField] private string _noNetwork;
	[SerializeField] private string _noNetPlayAnyway;
	[SerializeField] private string _difficulty;
	[SerializeField] private string _easy;
	[SerializeField] private string _medium;
	[SerializeField] private string _hard;
	[SerializeField] private string _sfxVolume;
	[SerializeField] private string _options;
	[SerializeField] private string _livesMenu;
	[SerializeField] private string _mainMenuBtn;
	[SerializeField] private string _personalRecordBtn;
	[SerializeField] private string _leaderboardBtn;
	[SerializeField] private string _achievementsBtn;
	[SerializeField] private string _gameSkin;
	[SerializeField] private string _defaultSkin;
	[SerializeField] private string _christmasSkin;


	#endregion

	#region Public Props
	public string StartBtn { get { return _startBtn; }}
	public string RecordBtn { get { return _recordBtn; }}
	public string HowToPlayBtn { get { return _howToPlayBtn; }}
	public string QuitBtn { get { return _quitBtn; }}	
	public string GameOverRestart { get { return _gameOverRestart; }}
	public string ResumeLostMatchBtn { get { return _resumeLostMatchBtn; }}
	public string RestartMatchBtn { get { return _restartMatchBtn; }}
	public string ChooseYourChances { get { return _chooseYourChances; }}
	public string OneLife { get { return _oneLife; }}
	public string TwoLives { get { return _twoLives; }}
	public string ThreeLives { get { return _threeLives; }}
	public string ContinueMatchBtn { get { return _continueMatchBtn; }}	
	public string BackBtn { get { return _backBtn; }}
	public string ResetBtn { get { return _resetBtn; }}
	public string AreYouSure { get { return _areYouSure; }}
	public string Yes { get { return _yes; }}
	public string No { get { return _no; }}
	public string TapThePigeons { get { return _tapThePigeons; }}
	public string BeFast { get { return _beFast; }}
	public string InsaneMode { get { return _insaneMode; }}
	public string GotItBtn { get { return _gotItBtn; }}
	public string ResumeGame { get {return _resumeGame; }}
	public string TryAgain { get {return _tryAgain; }}
	public string GetALife { get { return _getALife; }}
	public string NeverMind { get {return _neverMind;}}
	public string NoNetwork { get { return _noNetwork;}}
	public string NoNetPlayAnyway { get { return _noNetPlayAnyway;}}
	public string Difficulty { get { return _difficulty;}}
	public string Easy { get { return _easy;}}
	public string Medium { get { return _medium;}}
	public string Hard { get {return _hard ;}}	
	public string SFXVolume { get { return _sfxVolume;}}
	public string Options { get { return _options;}}
	public string LivesMenu { get { return _livesMenu;}}
	public string MainMenuBtn { get { return _mainMenuBtn; }}
	public string PersonalRecordBtn { get { return _personalRecordBtn; }}
	public string LeaderboardBtn { get { return _leaderboardBtn; }}
	public string AchievementsBtn { get { return _achievementsBtn; }}
	public string GameSkin { get { return _gameSkin; }}
	public string DefaultSkin { get { return _defaultSkin; }}
	public string ChristmasSkin { get { return _christmasSkin; }}
	
	#endregion
}
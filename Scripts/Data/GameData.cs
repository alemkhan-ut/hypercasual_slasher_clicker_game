using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "new GameData", menuName = "Game Data", order = 51)]
public class GameData : ScriptableObject
{
    [Header("Debuging")]
    [SerializeField] private bool _isDebug = false;
    [SerializeField] private bool _isOSOKDebug = false;

    [Header("Project Values")]
    [SerializeField] private int _mainSceneIndex = 0;
    [SerializeField] private string _mainSceneName = "Main Menu";
    [SerializeField] private int _gameSceneIndex = 1;
    [SerializeField] private string _gameSceneName = "Game";

    [Header("PlayerPrefs Values")]
    [SerializeField] private string _currentGlobalLevel = "CurrentGlobalLevel";
    [SerializeField] private string _stage = "Stage";
    [SerializeField] private string _bestLevel = "BestLevel";
    [SerializeField] private string _currentHits = "CurrentHits";
    [SerializeField] private string _bestHits = "BestHits";
    [SerializeField] private string _totalStars = "TotalStars";
    [SerializeField] private string _starCollectInSession = "StarCollectInSession";
    [SerializeField] private string _soundState = "SoundState";
    [SerializeField] private string _musicState = "MusicState";
    [SerializeField] private string _selectedWeaponID = "SelectedWeaponID";
    [SerializeField] [HideInInspector] private string _zero = "0";
    //

    [Header("POWER UPS")]
    [SerializeField] private string _secondChance = "SecondChance";
    [SerializeField] private string _defaultSecondChance = "DefaultSecondChance";
    [SerializeField] private float _defaultSecondChanceValue = 0.15f;

    [Header("Weapons")]
    [SerializeField] private int TotalWeaponsInGame;


    [Header("Product Values")]
    [SerializeField] private string _gameName;
    [SerializeField] private string _gameVersion;

    [Header("Music & Sound Value")]
    [SerializeField] private AudioClip _mainMenuMusic;
    [SerializeField] private string _mainMenuMusicName;
    [SerializeField] private AudioClip _inGameMusic;
    [SerializeField] private string _inGameMusicName;
    [SerializeField] private bool _isMusicOn;
    [SerializeField] private bool _isSoundOn;

    [Header("Tasks")]
    [SerializeField] private string taskStatus_ = "TaskStatus_";
    [SerializeField] private string taskValue_ = "TaskValue_";

    [SerializeField] private string taskStatus01_ = "TaskStatus_number01";
    [SerializeField] private string taskValue01_ = "TaskValue_number01";

    [SerializeField] private string taskStatus02_ = "TaskStatus_number02";
    [SerializeField] private string taskValue02_ = "TaskValue_number02";

    [SerializeField] private string taskStatus03_ = "TaskStatus_number03";
    [SerializeField] private string taskValue03_ = "TaskValue_number03";

    [Header("Achievments")]
    [SerializeField] private string achievmentValue = "AchievmentValue_";
    [SerializeField] private string achievmentValue01 = "AchievmentValue_number01";
    [SerializeField] private string achievmentValue02 = "AchievmentValue_number02";
    [SerializeField] private string achievmentValue03 = "AchievmentValue_number03";
    [SerializeField] private string achievmentValue04 = "AchievmentValue_number04";
    [SerializeField] private string achievmentValue05 = "AchievmentValue_number05";
    [SerializeField] private string achievmentValue06 = "AchievmentValue_number06";
    [SerializeField] private string achievmentValue07 = "AchievmentValue_number07";
    [SerializeField] private string achievmentValue08 = "AchievmentValue_number08";
    [SerializeField] private string achievmentValue09 = "AchievmentValue_number09";
    [SerializeField] private string achievmentValue10 = "AchievmentValue_number10";

    [SerializeField] private string hitsCounter_ = "HitsCounter";

    [Header("URL's")]
    [SerializeField] private string _ourVK = "https://vk.com/oinast";
    [SerializeField] private string _ourInstagram = "https://telegram.com/oinast_games";
    [SerializeField] private string _ourTelegram = "https://vk.com/oinast";
    [SerializeField] private string _ourFB = "https://facebook.com";
    [SerializeField] private string _ourTwitter = "https://twitter.com/oinast";
    [SerializeField] private string _ourYouTube = "https://youtube.com";

    public string GameName { get => _gameName; }
    public string GameVersion { get => _gameVersion; }

    public string GetBestLevel()
    {
        return _bestLevel;
    }

    public string GetBestHits()
    {
        return _bestHits;
    }

    public string GetTotalStars()
    {
        return _totalStars;
    }

    public string Zero { get => _zero; }
    public AudioClip MainMenuMusic { get => _mainMenuMusic; }
    public string MainMenuMusicName { get => _mainMenuMusic.name; }
    public AudioClip InGameMusic { get => _inGameMusic; }
    public string InGameMusicName { get => _inGameMusic.name; }
    public string CurrentGlobalLevel { get => _currentGlobalLevel; }

    public string GetCurrentHits()
    {
        return _currentHits;
    }

    public string OurVK { get => _ourVK; }
    public string OurInstagram { get => _ourInstagram; }
    public string OurTelegram { get => _ourTelegram; }
    public string OurFB { get => _ourFB; }
    public string OurTwitter { get => _ourTwitter; }
    public string OurYouTube { get => _ourYouTube; }
    public int MainSceneIndex { get => _mainSceneIndex; }
    public string MainSceneName { get => _mainSceneName; }
    public int GameSceneIndex { get => _gameSceneIndex; }
    public string GameSceneName { get => _gameSceneName; }
    public bool IsDebug { get => _isDebug; set => _isDebug = value; }
    public bool IsMusicOn { get => _isMusicOn; set => _isMusicOn = value; }
    public bool IsSoundOn { get => _isSoundOn; set => _isSoundOn = value; }
    public string SoundState { get => _soundState; set => _soundState = value; }
    public string MusicState { get => _musicState; set => _musicState = value; }
    public string Stage { get => _stage; set => _stage = value; }
    public bool IsOSOKDebug { get => _isOSOKDebug; set => _isOSOKDebug = value; }
    public string SelectedWeaponID { get => _selectedWeaponID; set => _selectedWeaponID = value; }
    public int TotalWeaponsInGame1 { get => TotalWeaponsInGame; set => TotalWeaponsInGame = value; }
    public string StarCollectInSession { get => _starCollectInSession; set => _starCollectInSession = value; }
    public string SecondChance { get => _secondChance; set => _secondChance = value; }
    public string DefaultSecondChance { get => _defaultSecondChance; set => _defaultSecondChance = value; }
    public float DefaultSecondChanceValue { get => _defaultSecondChanceValue; set => _defaultSecondChanceValue = value; }
    public string TaskStatus01_ { get => taskStatus01_; set => taskStatus01_ = value; }
    public string TaskValue01_ { get => taskValue01_; set => taskValue01_ = value; }
    public string TaskStatus02_ { get => taskStatus02_; set => taskStatus02_ = value; }
    public string TaskValue02_ { get => taskValue02_; set => taskValue02_ = value; }
    public string TaskStatus03_ { get => taskStatus03_; set => taskStatus03_ = value; }
    public string TaskValue03_ { get => taskValue03_; set => taskValue03_ = value; }
    public string AchievmentValue01 { get => achievmentValue01; set => achievmentValue01 = value; }
    public string AchievmentValue02 { get => achievmentValue02; set => achievmentValue02 = value; }
    public string AchievmentValue03 { get => achievmentValue03; set => achievmentValue03 = value; }
    public string AchievmentValue04 { get => achievmentValue04; set => achievmentValue04 = value; }
    public string AchievmentValue05 { get => achievmentValue05; set => achievmentValue05 = value; }
    public string AchievmentValue06 { get => achievmentValue06; set => achievmentValue06 = value; }
    public string AchievmentValue07 { get => achievmentValue07; set => achievmentValue07 = value; }
    public string AchievmentValue08 { get => achievmentValue08; set => achievmentValue08 = value; }
    public string AchievmentValue09 { get => achievmentValue09; set => achievmentValue09 = value; }
    public string AchievmentValue10 { get => achievmentValue10; set => achievmentValue10 = value; }
    public string AchievmentValue { get => achievmentValue; set => achievmentValue = value; }
    public string TaskStatus_ { get => taskStatus_; set => taskStatus_ = value; }
    public string TaskValue_ { get => taskValue_; set => taskValue_ = value; }
    public string HitsCounter_ { get => hitsCounter_; set => hitsCounter_ = value; }

    public bool IsMainScene()
    {
        if (SceneManager.GetActiveScene().name == MainSceneName &&
            SceneManager.GetActiveScene().buildIndex == MainSceneIndex)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsGameScene()
    {
        if (SceneManager.GetActiveScene().name == GameSceneName &&
            SceneManager.GetActiveScene().buildIndex == GameSceneIndex)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnEnable()
    {
        _gameName = Application.productName;
        _gameVersion = Application.version;
    }
}

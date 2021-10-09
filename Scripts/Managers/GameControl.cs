using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Purchasing;
using Firebase;
using Firebase.Analytics;

public class GameControl : MonoBehaviour
{
    [Range(0, 1)] [SerializeField] private float timeScaleValue_;
    [SerializeField] private bool isDemo_;
    [SerializeField] private bool isOnlyDemoBoss_;
    [SerializeField] private bool isPresentationDemoMode_;
    [SerializeField] private bool isLoseVariant_;
    [SerializeField] private GameObject playNowWindow_;
    [SerializeField] private AudioClip failedClip_;
    [SerializeField] private AudioClip playNowClip_;

    [SerializeField] private bool no_ads_;
    [SerializeField] private GameObject no_adsButton_;
    private bool weapons1pack_;
    [SerializeField] private GameObject weapons1packUnlockImage_;
    private bool weapons2pack_;
    [SerializeField] private GameObject weapons2packUnlockImage_;
    [SerializeField] private GameObject noInternetImage_;
    [SerializeField] private GameObject loadingWindow_;

    [Space]
    public GameData game_Data;
    public GameObject shakeTarget_;
    public AdsManager adsManager_;
    public TapjoyUnity.Internal.TapjoyUnityInit tapjoyUnityInit_;
    public PurchaseManager purchaseManager_;
    [SerializeField] private Canvas canvas_;
    [SerializeField] private GameObject comingSoonPanel_;

    [Header("Scene Objects")]
    [SerializeField] private Text _currentLevelValueText; // Обьект с значением текущего уровня
    [SerializeField] private int currentLevelValue_; // Значение текущего уровня
    [SerializeField] private int levelTarget_; // Значение максимального кол-во очков для уровня
    [SerializeField] private int finishTarget_; // Значение последнего количества очков

    [SerializeField] private Text _hitAmountsText; // Обьект с значениям текущего количества ударов

    [SerializeField] private BoxCollider2D enemyScneCleanerTriggerCollider_;

    [SerializeField] private Text bestScoreValueText_;
    [SerializeField] private Text currentScoreValueText;
    [SerializeField] private GameObject _newRecordObject;

    [SerializeField] private Shop shop_;
    [SerializeField] private Text _starCountText;
    [SerializeField] private Text _totalStarCountText;
    [SerializeField] private int _totalStarCount;

    [SerializeField] private int _loseCounter;

    [SerializeField] private Text _totalKeyCounterText;
    [SerializeField] private Text _totalKeysCounterInGame;
    [SerializeField] private int _totalKeyCountValue;

    [SerializeField] private GameObject _continueRewardButton;
    [SerializeField] private GameObject _inGameStoreButton;
    [SerializeField] private GameObject purchaseWindow_;

    [SerializeField] private GameObject _losePanel;
    [SerializeField] private GameObject _demoLosePanel;
    [SerializeField] private GameObject _demoWinPanel;
    [SerializeField] private GameObject demoShop_;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private Text _resultValue;
    [SerializeField] private SoundManager _soundManager;
    [SerializeField] private GameObject _secondChanceObject;
    [SerializeField] private WeaponManager _weaponManager;
    [SerializeField] private NiobiumStudios.DailyRewards dailyRewards_;

    [SerializeField] private GameObject _mainMenuElements;
    [SerializeField] private GameObject _gamePlayElements;

    [SerializeField] private bool _canPlay;
    [SerializeField] private bool isRewardedAdShown_;
    [SerializeField] private bool isLose_;
    [SerializeField] private bool gameLose_;
    [SerializeField] private GameObject _tapToPlayObject;
    [SerializeField] private GameObject bossTextObject_;

    [SerializeField] public bool secondChanceActive;
    private float secondChanceValue;


    [SerializeField] private GameObject afterBossScoreWindow_;
    [SerializeField] private Text starCollectScoreText_;
    [SerializeField] private Text doubleStarCollectScoreText_;

    private AudioSource audioSource_;
    private EnemySpawner enemySpawner_;
    private TaskAchievmentManager taskAchievmentManager_;
    public SoundManager SoundManager { get => _soundManager; set => _soundManager = value; }
    public bool CanPlay { get => _canPlay; set => _canPlay = value; }
    public bool GameLose_ { get => gameLose_; set => gameLose_ = value; }
    public int CurrentLevelValue_ { get => currentLevelValue_; set => currentLevelValue_ = value; }
    public int TotalStarCount_ { get => _totalStarCount; set => _totalStarCount = value; }
    public int CheckPointHits_ { get => checkPointHits_; set => checkPointHits_ = value; }
    public int HitsCollect { get => _hitsCollect; set => _hitsCollect = value; }
    public int CheckPointLevel_ { get => checkPointLevel_; set => checkPointLevel_ = value; }
    public int EnemyDestroyedCount_ { get => enemyDestroyedCount_; set => enemyDestroyedCount_ = value; }
    public Canvas Canvas_ { get => canvas_; set => canvas_ = value; }
    public bool IsLose_ { get => isLose_; set => isLose_ = value; }
    public int LevelTarget_ { get => levelTarget_; set => levelTarget_ = value; }
    public bool No_ads_ { get => no_ads_; set => no_ads_ = value; }
    public bool Weapons1pack_ { get => weapons1pack_; set => weapons1pack_ = value; }
    public bool Weapons2pack_ { get => weapons2pack_; set => weapons2pack_ = value; }
    public bool IsDemo_ { get => isDemo_; set => isDemo_ = value; }
    public GameObject DemoLosePanel { get => DemoLosePanel1; set => DemoLosePanel1 = value; }
    public bool IsOnlyDemoBoss_ { get => isOnlyDemoBoss_; set => isOnlyDemoBoss_ = value; }
    public bool IsPresentationDemoMode_ { get => isPresentationDemoMode_; set => isPresentationDemoMode_ = value; }
    public GameObject LoadingWindow_ { get => loadingWindow_; set => loadingWindow_ = value; }
    public GameObject LosePanel { get => _losePanel; set => _losePanel = value; }
    public GameObject DemoLosePanel1 { get => _demoLosePanel; set => _demoLosePanel = value; }
    public GameObject PausePanel { get => _pausePanel; set => _pausePanel = value; }
    public GameObject AfterBossScoreWindow_ { get => afterBossScoreWindow_; set => afterBossScoreWindow_ = value; }
    public bool IsRewardedAdShown_ { get => isRewardedAdShown_; set => isRewardedAdShown_ = value; }
    public bool IsLoseVariant_ { get => isLoseVariant_; set => isLoseVariant_ = value; }

    private int _hitsCollect = 0; // Собранные удары за уровень
    [SerializeField] private int checkPointHits_; // Посл. сохранение после босса
    [SerializeField] private int checkPointLevel_; // Посл. сохранение после босса
    private int _starCollect = 0; // Собранные монеты за уровень
    private int enemyDestroyedCount_ = 0; // Уничтожено врагов за уровень


    string m_ReachabilityText; // DEBUG

    private void Awake()
    {
        _weaponManager = FindObjectOfType<WeaponManager>();
        _soundManager = FindObjectOfType<SoundManager>();
        enemySpawner_ = FindObjectOfType<EnemySpawner>();
        adsManager_ = FindObjectOfType<AdsManager>();
        taskAchievmentManager_ = FindObjectOfType<TaskAchievmentManager>();
        dailyRewards_ = FindObjectOfType<NiobiumStudios.DailyRewards>();
        purchaseManager_ = FindObjectOfType<PurchaseManager>();
        tapjoyUnityInit_ = FindObjectOfType<TapjoyUnity.Internal.TapjoyUnityInit>();
    }

    private void Start()
    {
        Application.targetFrameRate = 30;
        QualitySettings.vSyncCount = 0;
        StartCoroutine(CheckInternet());

        UpdateUI();

        if (isDemo_)
        {
            _mainMenuElements.SetActive(false);
            GetStart();

            CurrentLevelValue_ = 0;
        }
    }

    void Update()
    {
#if UNITY_EDITOR
        Time.timeScale = timeScaleValue_;
#endif
        if (!CanPlay)
        {
            if (!LosePanel.activeSelf &&
                !PausePanel.activeSelf &&
                !AfterBossScoreWindow_.activeSelf &&
                !IsRewardedAdShown_)
            {
                CanPlay = true;
            }
        }
    }

    private IEnumerator CheckInternet()
    {
        yield return new WaitForSeconds(20.0f);

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            CanPlay = false;
            noInternetImage_.SetActive(true);
        }
        else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
        {
            if (!LosePanel.activeSelf && !PausePanel.activeSelf)
            {
                CanPlay = true;
            }
            noInternetImage_.SetActive(false);
        }
        else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            if (!LosePanel.activeSelf && !PausePanel.activeSelf)
            {
                CanPlay = true;
            }
            noInternetImage_.SetActive(false);
        }

        StartCoroutine(CheckInternet());
    }

    public void HideUI()
    {
        _mainMenuElements.SetActive(false);
        _gamePlayElements.SetActive(false);
    }
    public void UpdateUI()
    {
        if (bestScoreValueText_ != null)
        {
            bestScoreValueText_.text = PlayerPrefs.GetInt(game_Data.GetBestHits()).ToString();
        }
        if (currentScoreValueText != null)
        {
            currentScoreValueText.text = PlayerPrefs.GetInt("CheckPointHits").ToString();
        }

        UpdateСurrency();
    }

    public void UpdateСurrency()
    {
        _totalStarCount = PlayerPrefs.GetInt("TotalStars");

        if (_totalStarCountText != null)
        {
            _totalStarCountText.text = _totalStarCount.ToString();
        }

        _totalKeyCountValue = PlayerPrefs.GetInt("TotalKeys");

        if (_totalKeyCounterText != null)
        {
            _totalKeyCounterText.text = _totalKeyCountValue.ToString();
        }

        if (_totalKeysCounterInGame != null)
        {
            _totalKeysCounterInGame.text = _totalKeyCountValue.ToString();
        }
    }

    public void Hit()
    {
        if (!enemySpawner_.IsBossTime_ && CanPlay)
        {
            HitsCollect += 1; // Увеличиваем собранные удары
            PlayerPrefs.SetInt(game_Data.GetCurrentHits(), HitsCollect); // Сохраняем собранные удары в базе
            PlayerPrefs.SetInt(game_Data.HitsCounter_, HitsCollect); // Сохраняем собранные удары в базе

            if (!isDemo_ || IsPresentationDemoMode_)
            {
                if (EnemyDestroyedCount_ % LevelTarget_ == 0 && EnemyDestroyedCount_ != 0)
                {
                    FirebaseAnalytics.LogEvent("completed_to_" + PlayerPrefs.GetInt(game_Data.CurrentGlobalLevel) + "_boss");

                    enemySpawner_.IsBossTime_ = true;
                    GetBoss();
                }

                if (EnemyDestroyedCount_ % finishTarget_ == 0 && EnemyDestroyedCount_ != 0)
                {
                    comingSoonPanel_.SetActive(true);
                }

                if (HitsCollect % 52 == 0)
                {
                    //GetEnemyRain();
                }
            }
        }

        taskAchievmentManager_.ProgressCheck();

        StartCoroutine(ShakeCamera());
    }

    public void LevelTargetCorrector()
    {
        if (PlayerPrefs.GetInt(game_Data.CurrentGlobalLevel) < 2)
        {
            LevelTarget_ = 50;
        }
        else if (PlayerPrefs.GetInt(game_Data.CurrentGlobalLevel) < 6)
        {
            LevelTarget_ = 20;
        }
        else if (PlayerPrefs.GetInt(game_Data.CurrentGlobalLevel) < 7)
        {
            LevelTarget_ = 25;
        }
        else if (PlayerPrefs.GetInt(game_Data.CurrentGlobalLevel) < 11)
        {
            LevelTarget_ = 35;
        }
        else if (PlayerPrefs.GetInt(game_Data.CurrentGlobalLevel) < 15)
        {
            LevelTarget_ = 45;
        }
        else if (PlayerPrefs.GetInt(game_Data.CurrentGlobalLevel) > 15)
        {
            LevelTarget_ = 45;
        }
    }
    public void LevelUP(int levelAmount = 1)
    {
        CurrentLevelValue_ += levelAmount;
        PlayerPrefs.SetInt(game_Data.CurrentGlobalLevel, CurrentLevelValue_);
        enemySpawner_.SpawnTime_ += 0.1f;

        LevelTargetCorrector();
    }
    public void EnemyDestroyed()
    {
        EnemyDestroyedCount_ += 1;
        PlayerPrefs.SetInt("TotalEnemyDestroyed", PlayerPrefs.GetInt("TotalEnemyDestroyed") + 1);
        _hitAmountsText.text = EnemyDestroyedCount_.ToString();
    }
    public void GetBoss()
    {

        StartCoroutine(BossEnterAnimation());

        if (isDemo_ && IsPresentationDemoMode_)
        {
            SoundManager.PlayDemoEvilLaught();
        }
    }

    private IEnumerator BossEnterAnimation()
    {
        enemySpawner_.StopAllCoroutines();
        enemySpawner_.DestroyAllSimpleEnemy();

        PlayBossPresentationSound();

        for (int i = 0; i < 4; i++)
        {
            bossTextObject_.SetActive(true);
            bossTextObject_.transform.DOScale(2, 0.5f);
            yield return new WaitForSeconds(.25f);
            bossTextObject_.transform.DOScale(1, 0.5f);
            bossTextObject_.SetActive(false);
        }

        StartCoroutine(enemySpawner_.BossEnemySpawn());
    }

    public void AddStarInSession(int amount = 1)
    {
        _starCollect += amount;
        PlayerPrefs.SetInt(game_Data.StarCollectInSession, _starCollect);

        if (SoundManager != null)
        {
            SoundManager.gameSound.clip = SoundManager.pickupClip;
            SoundManager.gameSound.Play();
        }

        _starCountText.text = _starCollect.ToString();
    }

    public void AddStar(int amount = 1)
    {
        _totalStarCount = PlayerPrefs.GetInt("TotalStars");

        int addValue = amount + _totalStarCount;

        PlayerPrefs.SetInt("TotalStars", addValue);
        Debug.Log("Вы купили: " + amount + " звёзд. Всего теперь: " + PlayerPrefs.GetInt("TotalStars"));

        UpdateUI();
    }

    public void AddKey(int amount = 1)
    {
        _totalKeyCountValue = PlayerPrefs.GetInt("TotalKeys");

        int addValue = amount + _totalKeyCountValue;

        PlayerPrefs.SetInt("TotalKeys", addValue);
        Debug.Log("Вы купили: " + amount + " Ключей. Всего теперь: " + PlayerPrefs.GetInt("TotalKeys"));

        UpdateUI();
    }

    public void UseKeys()
    {
        if (PlayerPrefs.GetInt("TotalKeys") >= 1)
        {
            PlayerPrefs.SetInt("TotalKeys", PlayerPrefs.GetInt("TotalKeys") - 1);

            CalmContinueGame();
        }
        else
        {
            OpenInGameStore();
        }
    }

    public void OpenInGameStore()
    {
        purchaseWindow_.SetActive(true);
    }

    public void WatchContinueRewardVideo()
    {
        FirebaseAnalytics.LogEvent("continue_reward_video");

        adsManager_.ShowContinueRewarded();
    }

    public void CalmContinueGame()
    {
        _weaponManager.target_ = null;

        enemySpawner_.StartEnemySpawn();

        if (enemySpawner_.IsBossTime_)
        {
            enemySpawner_.DestroyBoss();
            GetBoss();
            CanPlay = true;
            PlayBossPresentationSound();
        }
        else
        {
            enemyScneCleanerTriggerCollider_.enabled = true;
            enemyScneCleanerTriggerCollider_.gameObject.SetActive(true);
            PlayGamePlayMusic();
        }

        UnPauseGame();
    }

    public void DoubleReward()
    {
        adsManager_.ShowDoubleCollecteRewarded();
    }

    public void GetDoubleReward()
    {
        FirebaseAnalytics.LogEvent("double_reward_after_boss");

        _starCollect *= 2;
        PlayerPrefs.SetInt(game_Data.StarCollectInSession, _starCollect);

        _starCountText.text = _starCollect.ToString();
        _soundManager.PlayButtonSound();

        CalmContinueGame();
    }

    public void CollectReward()
    {
        PlayerPrefs.SetInt("TotalStars", _starCollect);

        _starCountText.text = _starCollect.ToString();
        _soundManager.PlayButtonSound();
        CalmContinueGame();
    }

    public void OpenAfterBossScore()
    {
        if (!isDemo_)
        {
            AfterBossScoreWindow_.SetActive(true);

            starCollectScoreText_.text = _starCollect.ToString();
            doubleStarCollectScoreText_.text = (_starCollect * 2).ToString();
        }
        else
        {

        }

    }

    public void PlayHitSound()
    {
        SoundManager.PlayHitSound();
    }
    public void PlayLoseSound()
    {
        if (isDemo_)
        {
            SoundManager.musicSound.clip = failedClip_;
            SoundManager.musicSound.Play();
        }
    }
    public void PlayNowMusic()
    {
        if (isDemo_)
        {
            SoundManager.musicSound.clip = playNowClip_;
            SoundManager.musicSound.Play();
        }
    }

    public void PlayBossPresentationSound()
    {
        SoundManager.PlayBossPresentationSound();
    }
    public void PlayMisstakeHitSound()
    {

    }
    public void PlayThrowWeaponSound()
    {
        SoundManager.PlayThrowWeaponSound();
    }
    public void PlayGamePlayMusic()
    {
        SoundManager.PlayGamePlayMusic();
    }
    public void PlayMainMenuMusic()
    {
        SoundManager.PlayMainMenuMusic();
    }
    public void MusicPause()
    {
        SoundManager.MusicPause();
    }
    public void PauseGame()
    {
        CanPlay = false;
        ShowPausePanel();
    }
    public void UnPauseGame()
    {
        CanPlay = true;
        PausePanel.SetActive(false);
        LosePanel.SetActive(false);
        afterBossScoreWindow_.SetActive(false);
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Сцена с игрой
    }
    public void GetStart(bool isBegin = true)
    {
        _gamePlayElements.SetActive(true);
        _mainMenuElements.SetActive(false);

        PlayGamePlayMusic();

        if (isBegin)
        {
            _weaponManager.SetSelectedWeapon();
            StartCoroutine(enemySpawner_.EnemySpawn());
        }
        else
        {
            if (true)
            {
                if (enemySpawner_.IsBossTime_)
                {
                    enemySpawner_.DestroyBoss();
                    enemySpawner_.StartEnemySpawn();
                }
                else
                {
                    enemyScneCleanerTriggerCollider_.enabled = true;
                    enemyScneCleanerTriggerCollider_.gameObject.SetActive(true);
                }
            }
        }

        UnPauseGame();
        enemyDestroyedCount_ = PlayerPrefs.GetInt("CheckPointHits");
        HitsCollect = CheckPointHits_;


        LevelTargetCorrector();

        UpdateUI();

        _loseCounter = 0;

        CurrentLevelValue_ = PlayerPrefs.GetInt(game_Data.CurrentGlobalLevel);

        if (_currentLevelValueText != null)
        {
            _currentLevelValueText.text = CurrentLevelValue_.ToString();
        }

        if (PlayerPrefs.HasKey(game_Data.CurrentGlobalLevel))
        {
            _soundManager.UpdateAudio();
        }

        if (_hitAmountsText != null)
        {
            _hitAmountsText.text = EnemyDestroyedCount_.ToString(); // Заполняем им UI
        }

    }
    public void GetDemoStart()
    {
        PlayGamePlayMusic();

        UnPauseGame();

        _weaponManager.SetSelectedWeapon();
    }

    public void ShowWinPanel()
    {
        if (isDemo_ || IsPresentationDemoMode_)
        {
            enemySpawner_.DestroyAllSimpleEnemy();

            _demoWinPanel.SetActive(true);
            StartCoroutine(OpenPlayNowWindow());
            SoundManager.musicSound.loop = false;
            CanPlay = false;
        }
        else
        {

        }
    }

    public void ShowLosePanel()
    {
        if (isDemo_ || IsPresentationDemoMode_)
        {
            if (isLoseVariant_)
            {
                enemySpawner_.DestroyAllSimpleEnemy();

                DemoLosePanel1.SetActive(true); // Показать панель проигрыша
                StartCoroutine(OpenPlayNowWindow());
                SoundManager.musicSound.loop = false;
                PlayLoseSound();
                CanPlay = false;
            }
            else
            {
                SoundManager.musicSound.loop = false;
                PlayLoseSound();
                CanPlay = false;
                StartCoroutine(OpenDemoShop());
            }
        }
        else
        {
            //tapjoyUnityInit_.ShowStageFailedPlacement();

            _loseCounter += 1;
#if UNITY_EDITOR
            Debug.Log("ВЫ ПРОИГРАЛИ. ЧИСЛО ПОРАЖЕНИЙ : " + _loseCounter);
#endif

            if (_loseCounter >= 3)
            {
                purchaseManager_.CheckAllProducts();

                _loseCounter = 0;

                if (!no_ads_)
                {
                    adsManager_.ShowInterstitial();
                }
            }

            MusicPause();

            CanPlay = false;

#if UNITY_EDITOR
            Debug.Log("Окно с поражением. Игра остановлена");
#endif
            _resultValue.text = enemyDestroyedCount_.ToString();
            UpdateUI();

            if (currentLevelValue_ > PlayerPrefs.GetInt(game_Data.GetBestLevel())) // Сравниваем текущий уровень и лучший
            {
                PlayerPrefs.SetInt(game_Data.GetBestLevel(), currentLevelValue_); // назначаем новое значение

                _newRecordObject.SetActive(true); // Показываем обьект на сцене с отображением нового рекорда
            }

            LosePanel.SetActive(true); // Показать панель проигрыша

#if UNITY_EDITOR

            _continueRewardButton.SetActive(true);

#elif UNITY_ANDROID
                _continueRewardButton.SetActive(true);
#endif
        }
    }

    public void ShowLevelComplete()
    {
        if (!isDemo_)
        {
            //tapjoyUnityInit_.ShowLevelCompletePlacement();
        }
    }


    private IEnumerator OpenPlayNowWindow()
    {
        yield return new WaitForSeconds(3f);

        PlayNowMusic();
        playNowWindow_.SetActive(true);
    }
    private IEnumerator OpenDemoShop()
    {
        CanPlay = false;
        enemySpawner_.DestroyAllSimpleEnemy();

        DemoLosePanel1.SetActive(true); // Показать панель проигрыша

        yield return new WaitForSeconds(1f);

        demoShop_.SetActive(true);
    }

    public void ShowPausePanel()
    {
        MusicPause();
        _resultValue.text = enemyDestroyedCount_.ToString();
        UpdateUI();

        PausePanel.SetActive(true); // Показать панель проигрыша
    }

    public void GameLose()
    {
        if (enemyDestroyedCount_ > PlayerPrefs.GetInt(game_Data.GetBestHits())) // Сравниваем текущий количество ударов и лучшее
        {
            PlayerPrefs.SetInt(game_Data.GetBestHits(), enemyDestroyedCount_); // Назначаем новое значение
        }

        _totalStarCount += _starCollect;
        PlayerPrefs.SetInt("TotalStars", _totalStarCount);

        UpdateUI();

        PlayerPrefs.GetInt(game_Data.GetCurrentHits(), 0);
        PlayerPrefs.GetInt(game_Data.StarCollectInSession, 0);

        _starCollect = 0;
        EnemyDestroyedCount_ = 0;
    }

    public void GrandClearProgress()
    {
        PlayerPrefs.DeleteKey(game_Data.CurrentGlobalLevel); // Обнуление текущего уровня
        PlayerPrefs.SetInt(game_Data.StarCollectInSession, 0); // Обнуление текущего уровня
        PlayerPrefs.SetInt(game_Data.GetBestLevel(), 0); // Обнуление текущего рекорда по уровню
        PlayerPrefs.SetInt(game_Data.GetCurrentHits(), 0); // Обнуление текущего количества ударов
        PlayerPrefs.SetInt(game_Data.GetBestHits(), 0); // Обнуление текущего рекорда по количеству ударов
        PlayerPrefs.DeleteKey("TotalStars"); // Обнуление текущего рекорда по количеству ударов
        PlayerPrefs.SetInt("TotalBossDestroyed", 0); // Обнуление текущего рекорда по количеству ударов
        PlayerPrefs.SetInt("TotalEnemyDestroyed", 0); // Обнуление текущего рекорда по количеству ударов
        PlayerPrefs.DeleteKey("TotalKeys"); // Обнуление текущего рекорда по количеству ударов
        PlayerPrefs.SetInt(game_Data.HitsCounter_, 0); // Обнуление текущего рекорда по количеству ударов
        PlayerPrefs.SetInt("CheckPointHits", 0);
        PlayerPrefs.SetString(game_Data.MusicState, "ON");
        PlayerPrefs.SetString(game_Data.SoundState, "ON");

        for (int i = 1; i <= 10; i++)
        {
            if (i >= 10)
            {
                PlayerPrefs.SetInt("AchievmentValue_number" + i, 0);
                PlayerPrefs.SetInt("TaskStatus_number" + i, 0);
            }

            PlayerPrefs.SetInt("AchievmentValue_number0" + i, 0);
            PlayerPrefs.SetInt("TaskStatus_number0" + i, 0);
        }

        shop_.DeleteWeaponData();
        dailyRewards_.Reset();

        PlayerPrefs.SetInt(game_Data.SelectedWeaponID, 0);

        Restart();
    }
    public void ClearProgress() // очистка игрового прогресса
    {
        PlayerPrefs.DeleteKey(game_Data.CurrentGlobalLevel); // Обнуление текущего уровня
        PlayerPrefs.SetInt(game_Data.StarCollectInSession, 0); // Обнуление текущего уровня
        PlayerPrefs.SetInt(game_Data.GetBestLevel(), 0); // Обнуление текущего рекорда по уровню
        PlayerPrefs.SetInt(game_Data.GetCurrentHits(), 0); // Обнуление текущего количества ударов
        PlayerPrefs.SetInt(game_Data.GetBestHits(), 0); // Обнуление текущего рекорда по количеству ударов
                                                        //PlayerPrefs.SetInt("TotalStars", 0); // Обнуление текущего рекорда по количеству ударов
        PlayerPrefs.SetInt("TotalBossDestroyed", 0); // Обнуление текущего рекорда по количеству ударов
        PlayerPrefs.SetInt("TotalEnemyDestroyed", 0); // Обнуление текущего рекорда по количеству ударов
                                                      //PlayerPrefs.SetInt("TotalKeys", 0); // Обнуление текущего рекорда по количеству ударов
        PlayerPrefs.SetInt(game_Data.HitsCounter_, 0); // Обнуление текущего рекорда по количеству ударов
        PlayerPrefs.SetInt("CheckPointHits", 0);
        //PlayerPrefs.SetString(game_Data.MusicState, "ON");
        //PlayerPrefs.SetString(game_Data.SoundState, "ON");

        for (int i = 1; i <= 10; i++)
        {
            if (i >= 10)
            {
                PlayerPrefs.SetInt("AchievmentValue_number" + i, 0);
                PlayerPrefs.SetInt("TaskStatus_number" + i, 0);
            }

            PlayerPrefs.SetInt("AchievmentValue_number0" + i, 0);
            PlayerPrefs.SetInt("TaskStatus_number0" + i, 0);
        }

        PlayerPrefs.SetInt(game_Data.SelectedWeaponID, 0);

        Restart();
    }
    public IEnumerator ShakeCamera()
    {
        yield return shakeTarget_.transform.DOShakePosition(.2f, 20, 100);
    }
}

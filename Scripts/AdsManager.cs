using System.Collections;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.Advertisements;
using TapjoyUnity.Internal;
using TapjoyUnity;
using UnityEngine.UI;

public class AdsManager : MonoBehaviour, IUnityAdsListener
{
    [SerializeField] private string adMobKey_;
    [SerializeField] private bool noAds_;

    [SerializeField] private bool isTest_;
    [Range(0, 100)] [SerializeField] private int bannerAdMobPriority_;

    [SerializeField] private bool isAdMob_;
    [SerializeField] private bool isUnityAds_;
    [SerializeField] private bool isTapJoyAds_;

    private bool isUnityAdsContinueRewardStarted_;
    private bool isUnityAdsContinueRewardComleted_;

    private bool isUnityAdsDoubleRewardStarted_;
    private bool isUnityAdsDoubleRewardComleted_;

    private TapjoyUnityInit tapjoyUnityInit_;

    // TapJoy

    private TJPlacement tapJoyAppLaunchAd_ = TJPlacement.CreatePlacement("AppLaunch");
    private TJPlacement tapJoyAppLaunch2Ad_ = TJPlacement.CreatePlacement("APP_LAUNCH");
    private TJPlacement tapJoyDoubleCollectRewardAd_ = TJPlacement.CreatePlacement("DoubleCollectReward");
    private TJPlacement tapJoyContinueRewardAd_ = TJPlacement.CreatePlacement("ContinueReward");
    private TJPlacement tapJoyInterstitialAd_ = TJPlacement.CreatePlacement("Interstitial");

    static bool initialized = false;

    // UNITY ADS
#if UNITY_EDITOR
    private string unityGameID_ = "4167299";
#elif UNITY_ANDROID
    private string unityGameID_ = "4167299";
#elif UNITY_IOS
    private string unityGameID_ = "4167298";
#endif

    private string continueRewardID = "rewardedVideo";
    private string doubleRewardID = "doubleReward";
    private string bannerID = "banner";

    // AdMob
    private BannerView bannerView_;

    private const string testBannerUnitID_ = "ca-app-pub-3940256099942544/6300978111";

#if UNITY_EDITOR
    private const string bannerUnitID_ = "ca-app-pub-3940256099942544/6300978111"; // test
#elif UNITY_ANDROID
    private const string bannerUnitID_ = "ca-app-pub-9588434950397584/5122040293";
#elif UNITY_IPHONE
    private const string bannerUnitID_ = "";
#else
    private const string bannerUnitID_ = "unexpected_platform";
#endif

    private InterstitialAd interstitialAd_;

    private const string testInterstitialUnitID_ = "ca-app-pub-3940256099942544/1033173712"; // test

#if UNITY_EDITOR
    private const string interstitialUnitID_ = "ca-app-pub-3940256099942544/1033173712"; // test
#elif UNITY_ANDROID
    private const string interstitialUnitID_ = "ca-app-pub-9588434950397584/6656621427";
#elif UNITY_IPHONE
    private const string interstitialUnitID_ = "";
#else
    private const string interstitialUnitID_ = "unexpected_platform";
#endif

    private bool isContinueRewardedAdShown_;
    private RewardedAd continueRewardedAd_;

    private const string testContinueRewardedUnitID_ = "ca-app-pub-3940256099942544/5224354917"; // test

#if UNITY_EDITOR
    private const string continueRewardedUnitID_ = "ca-app-pub-3940256099942544/5224354917"; // test
#elif UNITY_ANDROID
    private const string continueRewardedUnitID_ = "ca-app-pub-9588434950397584/8544418165";
#elif UNITY_IPHONE
    private const string continueRewardedUnitID_ = "";
#else
    private const string continueRewardedUnitID_ = "unexpected_platform";
#endif

    private bool isDoubleCollectRewardedAdShown_;
    private RewardedAd doubleCollectRewardedAd_;

    private const string testsDoubleCollectRewardedUnitID_ = "ca-app-pub-3940256099942544/5224354917"; // test

#if UNITY_EDITOR
    private const string doubleCollectRewardedUnitID_ = "ca-app-pub-3940256099942544/5224354917"; // test
#elif UNITY_ANDROID
    private const string doubleCollectRewardedUnitID_ = "ca-app-pub-9588434950397584/9735841218";
#elif UNITY_IPHONE
    private const string doubleCollectRewardedUnitID_ = "";
#else
    private const string doubleCollectRewardedUnitID_ = "unexpected_platform";
#endif

    private GameControl gameControl_;

    bool isBannerShown = false;

    public bool NoAds_ { get => noAds_; set => noAds_ = value; }

    void Awake()
    {
        gameControl_ = FindObjectOfType<GameControl>();
        tapjoyUnityInit_ = GetComponent<TapjoyUnityInit>();

        if (isTapJoyAds_)
        {
            if (!initialized)
            {
                initialized = true;

#if UNITY_EDITOR
#elif UNITY_ANDROID
        ApiBindingAndroid.Install();
#elif UNITY_IOS
        ApiBindingIos.Install();
#endif

#if UNITY_5_4_OR_NEWER
                UnityEngine.SceneManagement.SceneManager.activeSceneChanged += (oldScene, newScene) =>
                {
                    UnityDependency.OnActiveSceneChanged(Wrap(oldScene), Wrap(newScene));
                };
                UnityEngine.SceneManagement.SceneManager.sceneLoaded += (scene, mode) =>
                {
                    UnityDependency.OnSceneLoaded(Wrap(scene), (int)mode);
                };
                UnityEngine.SceneManagement.SceneManager.sceneUnloaded += (scene) =>
                {
                    UnityDependency.OnSceneUnloaded(Wrap(scene));
                };
                TapjoyUnity.Internal.UnityDependency.sceneCount = () =>
                {
                    return UnityEngine.SceneManagement.SceneManager.sceneCount;
                };
                TapjoyUnity.Internal.UnityDependency.GetActiveScene = () =>
                {
                    return Wrap(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
                };
                TapjoyUnity.Internal.UnityDependency.GetSceneAt = (index) =>
                {
                    return Wrap(UnityEngine.SceneManagement.SceneManager.GetSceneAt(index));
                };

                TapjoyUnity.Internal.UnityDependency.ToJson = JsonUtility.ToJson;
#endif
            }

            if (!Tapjoy.IsConnected)
            {
                Tapjoy.Connect();
#if UNITY_EDITOR
                Debug.Log("TAPJOY CONNECT!");
#endif
            }

            RequestAll();

            TJPlacement.OnRequestSuccess += HandlePlacementRequestSuccess;
            TJPlacement.OnRequestFailure += HandlePlacementRequestFailure;
            TJPlacement.OnContentReady += HandlePlacementContentReady;
            TJPlacement.OnContentShow += HandlePlacementContentShow;
            TJPlacement.OnContentDismiss += HandlePlacementContentDismiss;
            TJPlacement.OnPurchaseRequest -= HandleOnPurchaseRequest;
            TJPlacement.OnRewardRequest -= HandleOnRewardRequest;
        }
    }

    void Start()
    {
        StartCoroutine(BannerCoroutine());
        if (!noAds_)
        {

            if (isUnityAds_)
            {
                Advertisement.AddListener(this);
                Advertisement.Initialize(unityGameID_, isTest_);
            }
            if (isAdMob_)
            {
                MobileAds.Initialize(initStatus => { });
                //EnableAdMobBanner();
                EnableAdMobInterstitialAd();
                EnableAdMobContinueRewardedAd();
                EnableAdMobDoubleCollectRewardedAd();
            }
            if (isTapJoyAds_)
            {
                TJPlacement.OnPurchaseRequest += HandleOnPurchaseRequest;
                TJPlacement.OnRewardRequest += HandleOnRewardRequest;
            }

        }
    }

    private void OnDisable()
    {
        if (isAdMob_)
        {
            if (continueRewardedAd_ != null)
            {
                continueRewardedAd_.OnUserEarnedReward -= HandleUserEarnedContinueReward;
            }
            if (doubleCollectRewardedAd_ != null)
            {
                doubleCollectRewardedAd_.OnUserEarnedReward -= HandleUserEarnedDoubleCollectReward;
            }
        }

        if (isTapJoyAds_)
        {
            TJPlacement.OnPurchaseRequest -= HandleOnPurchaseRequest;
            TJPlacement.OnRewardRequest -= HandleOnRewardRequest;
        }
    }

    private void EnableAdMobBanner()
    {
        if (bannerView_ != null)
        {
            HideBanner();
#if UNITY_EDITOR
            Debug.Log("BANNER HIDE");
#endif
        }

        if (!NoAds_)
        {
            ShowAdMobBanner();
        }
    }
    private void EnableAdMobInterstitialAd()
    {
        interstitialAd_ = isTest_ ? new InterstitialAd(testInterstitialUnitID_) : new InterstitialAd(interstitialUnitID_);
        AdRequest adRequest = new AdRequest.Builder().Build();
        interstitialAd_.LoadAd(adRequest);
    }
    private void EnableAdMobContinueRewardedAd()
    {
        continueRewardedAd_ = isTest_ ? new RewardedAd(testContinueRewardedUnitID_) : new RewardedAd(continueRewardedUnitID_);
        AdRequest adRequest = new AdRequest.Builder().Build();
        continueRewardedAd_.LoadAd(adRequest);

        continueRewardedAd_.OnUserEarnedReward += HandleUserEarnedContinueReward;
    }
    private void EnableAdMobDoubleCollectRewardedAd()
    {
        doubleCollectRewardedAd_ = isTest_ ? new RewardedAd(testsDoubleCollectRewardedUnitID_) : new RewardedAd(doubleCollectRewardedUnitID_);
        AdRequest adRequest = new AdRequest.Builder().Build();
        doubleCollectRewardedAd_.LoadAd(adRequest);

        doubleCollectRewardedAd_.OnUserEarnedReward += HandleUserEarnedDoubleCollectReward;
    }


    public void ShowAdMobBanner()
    {
        if (!NoAds_ && !isBannerShown)
        {
            StartCoroutine(BannerCoroutine());
        }
    }

    public void HideBanner()
    {
        if (isAdMob_)
        {
            if (bannerView_ != null)
            {
                bannerView_.Hide();
            }
        }

        if (isUnityAds_)
        {
            if (Advertisement.isInitialized && Advertisement.Banner.isLoaded)
            {
                Advertisement.Banner.Hide();
            }
        }
    }

    public void ShowInterstitial()
    {
        if (!NoAds_)
        {
            if (isTapJoyAds_)
            {
                if (tapJoyInterstitialAd_.IsContentReady())
                {
                    tapJoyInterstitialAd_.ShowContent();
                }
                else
                {
                    if (isAdMob_)
                    {
                        if (interstitialAd_.IsLoaded())
                        {
                            interstitialAd_.Show();
                        }
                        else
                        {
                            if (isUnityAds_)
                            {
                                if (Advertisement.isInitialized && Advertisement.IsReady())
                                {
                                    Advertisement.Show();
                                }
                            }
                        }
                    }
                    else
                    {
                        if (isUnityAds_)
                        {
                            if (Advertisement.isInitialized && Advertisement.IsReady())
                            {
                                Advertisement.Show();
                            }
                        }
                    }
                }
            }
            else if (isAdMob_)
            {
                if (interstitialAd_.IsLoaded())
                {
                    interstitialAd_.Show();
                }
                else
                {
                    if (isUnityAds_)
                    {
                        if (Advertisement.isInitialized && Advertisement.IsReady())
                        {
                            Advertisement.Show();
                        }
                    }
                }
            }
            else
            {
                if (isUnityAds_)
                {
                    if (Advertisement.isInitialized && Advertisement.IsReady())
                    {
                        Advertisement.Show();
                    }
                }
            }
        }
    }

    public void ShowContinueRewarded()
    {
        if (isTapJoyAds_)
        {
            if (tapJoyContinueRewardAd_.IsContentReady())
            {
                gameControl_.IsRewardedAdShown_ = true;
                gameControl_.CanPlay = false;
                tapJoyContinueRewardAd_.ShowContent();
            }
            else
            {
                if (isAdMob_)
                {
                    if (continueRewardedAd_.IsLoaded())
                    {
                        gameControl_.IsRewardedAdShown_ = true;
                        gameControl_.CanPlay = false;
                        continueRewardedAd_.Show();
                    }
                    else
                    {
                        if (isUnityAds_)
                        {
                            if (Advertisement.isInitialized && Advertisement.IsReady(continueRewardID))
                            {
                                Advertisement.Show(continueRewardID);
                                isUnityAdsContinueRewardStarted_ = true;
                                gameControl_.IsRewardedAdShown_ = true;
                                gameControl_.CanPlay = false;
                            }
                        }
                    }
                }
                else
                {
                    if (isUnityAds_)
                    {
                        if (Advertisement.isInitialized && Advertisement.IsReady(continueRewardID))
                        {
                            Advertisement.Show(continueRewardID);
                            isUnityAdsContinueRewardStarted_ = true;
                            gameControl_.IsRewardedAdShown_ = true;
                            gameControl_.CanPlay = false;
                        }
                    }
                }
            }
        }
        else if (isAdMob_)
        {
            if (continueRewardedAd_.IsLoaded())
            {
                gameControl_.IsRewardedAdShown_ = true;
                gameControl_.CanPlay = false;
                continueRewardedAd_.Show();
            }
            else
            {
                if (isUnityAds_)
                {
                    if (Advertisement.isInitialized && Advertisement.IsReady(continueRewardID))
                    {
                        Advertisement.Show(continueRewardID);
                        isUnityAdsContinueRewardStarted_ = true;
                        gameControl_.IsRewardedAdShown_ = true;
                        gameControl_.CanPlay = false;
                    }
                }
            }
        }
        else
        {
            if (isUnityAds_)
            {
                if (Advertisement.isInitialized && Advertisement.IsReady(continueRewardID))
                {
                    Advertisement.Show(continueRewardID);
                    isUnityAdsContinueRewardStarted_ = true;
                    gameControl_.IsRewardedAdShown_ = true;
                    gameControl_.CanPlay = false;
                }
            }
        }
    }

    public void ShowDoubleCollecteRewarded()
    {
        if (isTapJoyAds_)
        {
            if (tapJoyDoubleCollectRewardAd_.IsContentReady())
            {
                gameControl_.IsRewardedAdShown_ = true;
                gameControl_.CanPlay = false;
                tapJoyDoubleCollectRewardAd_.ShowContent();
            }
            else
            {
                if (isAdMob_)
                {
                    if (doubleCollectRewardedAd_.IsLoaded())
                    {
                        gameControl_.IsRewardedAdShown_ = true;
                        gameControl_.CanPlay = false;
                        doubleCollectRewardedAd_.Show();
                    }
                    else
                    {
                        if (isUnityAds_)
                        {
                            if (Advertisement.isInitialized && Advertisement.IsReady(doubleRewardID))
                            {
                                Advertisement.Show(doubleRewardID);
                                isUnityAdsContinueRewardStarted_ = true;
                                gameControl_.IsRewardedAdShown_ = true;
                                gameControl_.CanPlay = false;
                            }
                        }
                    }
                }
                else
                {
                    if (isUnityAds_)
                    {
                        if (Advertisement.isInitialized && Advertisement.IsReady(doubleRewardID))
                        {
                            Advertisement.Show(doubleRewardID);
                            isUnityAdsContinueRewardStarted_ = true;
                            gameControl_.IsRewardedAdShown_ = true;
                            gameControl_.CanPlay = false;
                        }
                    }
                }
            }
        }
        else if (isAdMob_)
        {
            if (doubleCollectRewardedAd_.IsLoaded())
            {
                gameControl_.IsRewardedAdShown_ = true;
                gameControl_.CanPlay = false;
                doubleCollectRewardedAd_.Show();
            }
            else
            {
                if (isUnityAds_)
                {
                    if (Advertisement.isInitialized && Advertisement.IsReady(doubleRewardID))
                    {
                        Advertisement.Show(doubleRewardID);
                        isUnityAdsContinueRewardStarted_ = true;
                        gameControl_.IsRewardedAdShown_ = true;
                        gameControl_.CanPlay = false;
                    }
                }
            }
        }
        else
        {
            if (isUnityAds_)
            {
                if (Advertisement.isInitialized && Advertisement.IsReady(doubleRewardID))
                {
                    Advertisement.Show(doubleRewardID);
                    isUnityAdsContinueRewardStarted_ = true;
                    gameControl_.IsRewardedAdShown_ = true;
                    gameControl_.CanPlay = false;
                }
            }
        }
    }

    public void HandleUserEarnedContinueReward(object sender, Reward args)
    {
        gameControl_.CanPlay = true;

#if UNITY_EDITOR
        Debug.Log("CONTINUE REWARD EARNED");
#endif
        gameControl_.CalmContinueGame();
        EnableAdMobContinueRewardedAd();
    }
    public void HandleUserEarnedDoubleCollectReward(object sender, Reward args)
    {
        gameControl_.CanPlay = true;

#if UNITY_EDITOR
        Debug.Log("DOUBLE COLLECT REWARD EARNED");
#endif
        gameControl_.GetDoubleReward();
        EnableAdMobDoubleCollectRewardedAd();
    }

    IEnumerator BannerCoroutine()
    {
        yield return new WaitForSecondsRealtime(1.0f);

        if (!NoAds_)
        {
            HideBanner();

            int randomNetwork = UnityEngine.Random.Range(0, 100);

            if (randomNetwork < bannerAdMobPriority_)
            {
                if (isAdMob_)
                {
                    bannerView_ = isTest_ ? new BannerView(testBannerUnitID_, AdSize.Banner, AdPosition.Bottom) : new BannerView(bannerUnitID_, AdSize.Banner, AdPosition.Bottom);
                    AdRequest adRequest = new AdRequest.Builder().Build();
                    bannerView_.LoadAd(adRequest);
                    isBannerShown = true;
                }
            }
            else
            {
                if (isUnityAds_)
                {
                    while (!Advertisement.isInitialized)
                    {
                        yield return new WaitForSeconds(0.5f);
                    }

                    Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
                    Advertisement.Banner.Show(bannerID);
                }
            }

            yield return new WaitForSecondsRealtime(25.0f);
            StartCoroutine(BannerCoroutine());
        }
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        StartCoroutine(ShowResultCoroutine(placementId, showResult));
    }

    private IEnumerator ShowResultCoroutine(string placementId, ShowResult showResult)
    {
        yield return new WaitForSecondsRealtime(.25f);

        if (showResult == ShowResult.Finished)
        {
            if (placementId == continueRewardID)
            {
                gameControl_.CanPlay = true;

#if UNITY_EDITOR
                Debug.Log("CONTINUE REWARD EARNED");
#endif
                gameControl_.CalmContinueGame();
                Advertisement.Load(continueRewardID);
            }

            if (placementId == doubleRewardID)
            {
                gameControl_.CanPlay = true;

#if UNITY_EDITOR
                Debug.Log("DOUBLE COLLECT REWARD EARNED");
#endif
                gameControl_.GetDoubleReward();
                Advertisement.Load(doubleRewardID);
            }
        }

        else if (showResult == ShowResult.Skipped)
        {

        }
        else if (showResult == ShowResult.Failed)
        {

        }
    }

    public void OnUnityAdsReady(string placementId)
    {
    }

    public void OnUnityAdsDidError(string message)
    {
    }

    public void OnUnityAdsDidStart(string placementId)
    {
    }


#if UNITY_5_4_OR_NEWER
    static TapjoyUnity.Internal.UnityCompat.SceneCompat Wrap(UnityEngine.SceneManagement.Scene scene)
    {
        return new TapjoyUnity.Internal.UnityCompat.SceneCompat(scene, scene.IsValid(), scene.buildIndex, scene.name, scene.path);
    }
#endif

    public void HandlePlacementRequestSuccess(TJPlacement placement)
    {
        // Called when the SDK has made contact with Tapjoy's servers. It does not necessarily mean that any content is available.
    }


    public void HandlePlacementRequestFailure(TJPlacement placement, string error)
    {
        // Called when there was a problem during connecting Tapjoy servers.
    }


    public void HandlePlacementContentReady(TJPlacement placement)
    {
        // Called when the content is actually available to display.
    }


    public void HandlePlacementContentShow(TJPlacement placement)
    {
        tapJoyInterstitialAd_.RequestContent();
        if (placement == tapJoyAppLaunchAd_)
        {
            gameControl_.AddStar(25);
        }

#if UNITY_EDITOR
        Debug.Log("НОВЫЙ ЗАПРОС НА МЕЖСТРАНИЧКУ TAPJOY");
#endif
    }


    public void HandlePlacementContentDismiss(TJPlacement placement)
    {
        // Called when the content is dismissed.
    }
    public void HandleOnPurchaseRequest(TJPlacement placement, TJActionRequest request, string productId)
    {
        if (placement == tapJoyInterstitialAd_)
        {

        }
        if (placement == tapJoyContinueRewardAd_)
        {

        }
        if (placement == tapJoyDoubleCollectRewardAd_)
        {

        }
    }

    public void HandleOnRewardRequest(TJPlacement placement, TJActionRequest request, string itemId, int quantity)
    {
        if (placement == tapJoyAppLaunchAd_)
        {
            gameControl_.AddStar(25);
        }
        if (placement == tapJoyInterstitialAd_)
        {
        }
        if (placement == tapJoyContinueRewardAd_)
        {
            gameControl_.CanPlay = true;

#if UNITY_EDITOR
            Debug.Log("CONTINUE REWARD EARNED");
#endif
            gameControl_.CalmContinueGame();
            tapJoyContinueRewardAd_.RequestContent();

        }
        if (placement == tapJoyDoubleCollectRewardAd_)
        {
            gameControl_.CanPlay = true;

#if UNITY_EDITOR
            Debug.Log("DOUBLE COLLECT REWARD EARNED");
#endif
            gameControl_.GetDoubleReward();
            tapJoyDoubleCollectRewardAd_.RequestContent();
        }
    }
    public void RequestAll()
    {
        if (Tapjoy.IsConnected)
        {
            tapJoyInterstitialAd_.RequestContent();
            tapJoyContinueRewardAd_.RequestContent();
            tapJoyDoubleCollectRewardAd_.RequestContent();
        }
        else
        {
            Debug.LogWarning("Tapjoy SDK must be connected before you can request content.");
        }
    }
}

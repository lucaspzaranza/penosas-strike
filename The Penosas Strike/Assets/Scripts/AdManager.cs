using UnityEngine;
using GoogleMobileAds.Api;
using System;

public enum AdType
{
	None = 0,
	SimpleInterstitial = 1,
	Interstitial = 2,
	Rewarded = 3
}

public class AdManager : MonoBehaviour
{
	public static AdManager instance;
	private AdType _adType = 0;
	private InterstitialAd _interstitialAd;
	private RewardedAd _rewardedAd;

	private string _interstitialGoogleTestID = "ca-app-pub-3940256099942544/1033173712";
    private string _simpleInterstitialID = "ca-app-pub-8413154693668249/4044118377";
	private string _interstitialID = "ca-app-pub-8413154693668249/5255722724";
	private string _rewardedID = "ca-app-pub-8413154693668249/1293793962";

	private string gameID = "1802509";

    void Awake()
	{
		if(instance == null)
			instance = this;
		else if(instance != null)
			Destroy(this.gameObject);		
	}

    private void Start()
    {
		MobileAds.RaiseAdEventsOnUnityMainThread = true;
		MobileAds.Initialize(initStatuts =>
		{
			print("Mobile Ads initialized successfully.");
		});
    }

    public void ChooseAd(int numOfLives)
	{
		_adType = (AdType)numOfLives;
	}

	public void ShowRewardedAd()
	{
		if(_adType == AdType.SimpleInterstitial || _adType == AdType.Interstitial)
		{
			LoadInterstitialAd();
			ShowInterstitialAd();
        }
		else if (_adType == AdType.Rewarded)
		{
			LoadRewardedAdMob();
			ShowRewardedAdMob();
		}
    }

	public void LoadInterstitialAd()
	{
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

        AdRequest adRequest = new AdRequest();
		adRequest.Keywords.Add("unity-admob-sample");
		print(_adType);

		string adID = _adType == AdType.SimpleInterstitial ? _simpleInterstitialID : _interstitialID;
		InterstitialAd.Load(adID, adRequest, (InterstitialAd ad, LoadAdError error) =>
		{
			if (error != null || ad == null)
			{
				Debug.LogWarning("Interstitial Ad failed to load.");
				return;
			}

			print("Interstitial Ad loaded! " + ad.GetResponseInfo());

            _interstitialAd = ad;
            InterstitialAdEventsCallbacks(_interstitialAd);
        });
	}

    public void ShowInterstitialAd()
    {
		if(_interstitialAd != null && _interstitialAd.CanShowAd())
			_interstitialAd.Show();
		else
			Debug.Log("Ad could not be shown.");
    }

    public void InterstitialAdEventsCallbacks(InterstitialAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        _interstitialAd.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(string.Format("Interstitial ad paid {0} {1}.", adValue.Value, adValue.CurrencyCode));
        };

        // Raised when an impression is recorded for an ad.
        _interstitialAd.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };

        // Raised when a click is recorded for an ad.
        _interstitialAd.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };

        // Raised when an ad opened full screen content.
        _interstitialAd.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };

        // Raised when the ad closed full screen content.
        _interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");
            GameUI.instance.ContinueGame();
        };

        // Raised when the ad failed to open full screen content.
        _interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
        };
    }

	public void LoadRewardedAdMob()
	{
		if(_rewardedAd != null)
		{
			_rewardedAd.Destroy();
			_rewardedAd = null;
		}

		AdRequest adRequest = new AdRequest();
		adRequest.Keywords.Add("unity-admob-sample");

		RewardedAd.Load(_rewardedID, adRequest, (RewardedAd ad, LoadAdError error) =>
		{
			if(error != null || ad == null)
			{
				Debug.LogWarning("Rewarded Ad failed to load. " + error);
				return;
			}

            print("Rewarded Ad loaded! " + ad.GetResponseInfo());
			_rewardedAd = ad;
			RewardedAdMobEventsCallbacks(_rewardedAd);
        });
	}

	public void ShowRewardedAdMob()
	{
		if (_rewardedAd != null && _rewardedAd.CanShowAd())
		{
			_rewardedAd.Show((Reward reward) =>
			{
				print("Give Reward to the player!");
			});
		}
	}

	public void RewardedAdMobEventsCallbacks(RewardedAd ad)
	{
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded ad paid {0} {1}.", adValue.Value, adValue.CurrencyCode));
        };

        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };

        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };

        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };

        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
            GameUI.instance.ContinueGame();
        };

        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " + "with error : " + error);
        };
    }
}
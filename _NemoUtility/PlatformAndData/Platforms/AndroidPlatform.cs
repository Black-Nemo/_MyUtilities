using System;
using System.IO;
#if GOOGLE_MOBILE_ADS_SDK
using GoogleMobileAds.Api;
#endif
using UnityEngine;

namespace NemoUtility
{
        /// <summary>
        /// Android platformu için Google Mobile Ads (AdMob) SDK v11 ile çalışan platform implementasyonu.
        /// </summary>
        public class AndroidPlatform : Platform
        {
                // ──────────────────────────────────────────────────────────
                // Yüklü reklamlar
                // ──────────────────────────────────────────────────────────
#if GOOGLE_MOBILE_ADS_SDK
                private RewardedAd _rewardedAd;
                private InterstitialAd _interstitialAd;
                private BannerView _bannerView;
#endif

                // ──────────────────────────────────────────────────────────
                // Bekleyen callback'ler
                // ──────────────────────────────────────────────────────────
                private Action _rewardedCompleteAction;
                private Action _fullScreenCloseAction;

                // ──────────────────────────────────────────────────────────
                // Lokal veri (LocalPlatform ile aynı pattern)
                // ──────────────────────────────────────────────────────────
                private string _filePath = "";
                private Data _data;

                // ══════════════════════════════════════════════════════════
                // Platform lifecycle
                // ══════════════════════════════════════════════════════════

                public override void OnEnable()
                {
                        // Veri dosyasını yükle
                        _filePath = Path.Combine(Application.persistentDataPath, "Datas", "datas.json");
                        _data = MyJsonUtility<Data>.Load(_filePath);
                        if (_data == null)
                        {
                                _data = new Data();
                                MyJsonUtility<Data>.SaveData(_filePath, _data);
                        }

#if GOOGLE_MOBILE_ADS_SDK
                        // AdMob'u başlat ve ilk reklamları yükle
                        MobileAds.Initialize(_ =>
                        {
                                LoadRewardedAd();
                                LoadInterstitialAd();
                        });
#endif
                }

                public override void OnDisable()
                {
#if GOOGLE_MOBILE_ADS_SDK
                        _rewardedAd?.Destroy();
                        _interstitialAd?.Destroy();
                        _bannerView?.Destroy();

                        _rewardedAd = null;
                        _interstitialAd = null;
                        _bannerView = null;
#endif
                }

                // ══════════════════════════════════════════════════════════
                // Rewarded Ad
                // ══════════════════════════════════════════════════════════

                private void LoadRewardedAd()
                {
#if GOOGLE_MOBILE_ADS_SDK
                        _rewardedAd?.Destroy();
                        _rewardedAd = null;

                        RewardedAd.Load(NemoUtility.AdConfig.REWARDED_AD_UNIT_ID, new AdRequest(), (ad, error) =>
                        {
                                if (error != null)
                                {
                                        Debug.LogWarning($"[AndroidPlatform] Rewarded yüklenemedi: {error}");
                                        return;
                                }
                                _rewardedAd = ad;
                                _rewardedAd.OnAdFullScreenContentClosed += OnRewardedClosed;
                                _rewardedAd.OnAdFullScreenContentFailed += OnRewardedFailed;
                        });
#endif
                }

                public override void Rewarded(Action rewardComplateAction)
                {
                        _rewardedCompleteAction = rewardComplateAction;

#if GOOGLE_MOBILE_ADS_SDK
                        if (_rewardedAd != null && _rewardedAd.CanShowAd())
                        {
                                _rewardedAd.Show(reward =>
                                {
                                        PlatformManager.Instance.EnqueueMainThreadAction(() =>
                                        {
                                                InvokeAndClearRewarded();
                                        });
                                });
                        }
                        else
                        {
                                Debug.LogWarning("[AndroidPlatform] Rewarded reklam hazır değil, yüklenmeye çalışılıyor...");
                                LoadRewardedAd();
                        }
#else
                        PlatformManager.Instance.EnqueueMainThreadAction(() =>
                        {
                                InvokeAndClearRewarded();
                        });
#endif
                }

                private void OnRewardedClosed()
                {
#if GOOGLE_MOBILE_ADS_SDK
                        PlatformManager.Instance.EnqueueMainThreadAction(() =>
                        {
                                InvokeAndClearRewarded();
                                LoadRewardedAd(); // bir sonraki gösterim için yükle
                        });
#endif
                }

#if GOOGLE_MOBILE_ADS_SDK
                private void OnRewardedFailed(AdError error)
                {
                        Debug.LogWarning($"[AndroidPlatform] Rewarded gösterilemedi: {error}");
                        PlatformManager.Instance.EnqueueMainThreadAction(() =>
                        {
                                _rewardedCompleteAction = null;
                                LoadRewardedAd();
                        });
                }
#endif

                private void InvokeAndClearRewarded()
                {
                        if (_rewardedCompleteAction != null)
                        {
                                _rewardedCompleteAction.Invoke();
                                _rewardedCompleteAction = null;
                        }
                }

                // ══════════════════════════════════════════════════════════
                // Interstitial (FullScreenShow)
                // ══════════════════════════════════════════════════════════

                private void LoadInterstitialAd()
                {
#if GOOGLE_MOBILE_ADS_SDK
                        _interstitialAd?.Destroy();
                        _interstitialAd = null;

                        InterstitialAd.Load(NemoUtility.AdConfig.INTERSTITIAL_AD_UNIT_ID, new AdRequest(), (ad, error) =>
                        {
                                if (error != null)
                                {
                                        Debug.LogWarning($"[AndroidPlatform] Interstitial yüklenemedi: {error}");
                                        return;
                                }
                                _interstitialAd = ad;
                                _interstitialAd.OnAdFullScreenContentClosed += OnInterstitialClosed;
                                _interstitialAd.OnAdFullScreenContentFailed += OnInterstitialFailed;
                        });
#endif
                }

                // ══════════════════════════════════════════════════════════
                // Banner Ad
                // ══════════════════════════════════════════════════════════

                public override void ShowBanner()
                {
#if GOOGLE_MOBILE_ADS_SDK
                        if (_bannerView != null) return; // Zaten gösteriliyor

                        // Klasik ve sabit 320x50 banner boyutu kullan
                        AdSize bannerSize = AdSize.Banner;

                        // Banner oluştur (Bottom)
                        _bannerView = new BannerView(NemoUtility.AdConfig.BANNER_AD_UNIT_ID, bannerSize, AdPosition.Bottom);

                        // Banner'ı yükle
                        _bannerView.LoadAd(new AdRequest());
#endif
                }

                public override void HideBanner()
                {
#if GOOGLE_MOBILE_ADS_SDK
                        if (_bannerView != null)
                        {
                                _bannerView.Destroy();
                                _bannerView = null;
                        }
#endif
                }

                public override void FullScreenShow(Action finishAction)
                {
                        _fullScreenCloseAction = finishAction;

#if GOOGLE_MOBILE_ADS_SDK
                        if (_interstitialAd != null && _interstitialAd.CanShowAd())
                        {
                                _interstitialAd.Show();
                        }
                        else
                        {
                                Debug.LogWarning("[AndroidPlatform] Interstitial hazır değil, yüklenmeye çalışılıyor...");
                                LoadInterstitialAd();
                                // Reklam hazır olmadığı için callback'i hemen çağır
                                finishAction?.Invoke();
                                _fullScreenCloseAction = null;
                        }
#else
                        finishAction?.Invoke();
                        _fullScreenCloseAction = null;
#endif
                }

                private void OnInterstitialClosed()
                {
#if GOOGLE_MOBILE_ADS_SDK
                        PlatformManager.Instance.EnqueueMainThreadAction(() =>
                        {
                                _fullScreenCloseAction?.Invoke();
                                _fullScreenCloseAction = null;
                                LoadInterstitialAd(); // bir sonraki gösterim için yükle
                        });
#endif
                }

#if GOOGLE_MOBILE_ADS_SDK
                private void OnInterstitialFailed(AdError error)
                {
                        Debug.LogWarning($"[AndroidPlatform] Interstitial gösterilemedi: {error}");
                        PlatformManager.Instance.EnqueueMainThreadAction(() =>
                        {
                                _fullScreenCloseAction?.Invoke();
                                _fullScreenCloseAction = null;
                                LoadInterstitialAd();
                        });
                }
#endif

                // ══════════════════════════════════════════════════════════
                // Data (LocalPlatform ile aynı — JSON dosyası)
                // ══════════════════════════════════════════════════════════

                public override Data GetAllData()
                {
                        return MyJsonUtility<Data>.Load(_filePath);
                }

                public override object GetData(string id)
                {
                        if (_data.Datas.TryGetValue(id, out object value))
                        {
                                return value;
                        }
                        return null;
                }

                public override void SetData(string id, object value)
                {
                        if (FindId(_data, id))
                        {
                                _data.Datas[id] = value;
                        }
                        else
                        {
                                _data.Datas.Add(id, value);
                        }
                        MyJsonUtility<Data>.SaveData(_filePath, _data);
                }

                public override void ResetData()
                {
                        _data = new Data();
                        MyJsonUtility<Data>.SaveData(_filePath, _data);
                }

                // ══════════════════════════════════════════════════════════
                // Leaderboard — Android'de Google Play Games kullanılabilir,
                // şimdilik stub bırakıldı.
                // ══════════════════════════════════════════════════════════

                public override void SetLeaderBoardValue(string id, double value)
                {
                        // Google Play Games SDK eklendiğinde buraya implement edilecek
                        Debug.Log($"[AndroidPlatform] SetLeaderBoardValue: {id} = {value}");
                }

                // ══════════════════════════════════════════════════════════
                // Dil
                // ══════════════════════════════════════════════════════════

                public override string GetLanguage()
                {
                        return Application.systemLanguage.ToString().ToLower();
                }

                public override void SetLanguage(string lang)
                {
                        SwitchLangEvent?.Invoke(lang);
                }

                // ══════════════════════════════════════════════════════════
                // Display Name — Play Games'ten isim al
                // ══════════════════════════════════════════════════════════

#if UNITY_ANDROID
                public override string GetDisplayName()
                {
                        // Önce Play Games'ten isim almaya çalış
#if GOOGLE_PLAY_GAMES_SDK
                        if (GooglePlayGames.PlayGamesPlatform.Instance != null &&
                            GooglePlayGames.PlayGamesPlatform.Instance.IsAuthenticated())
                        {
                                string playName = GooglePlayGames.PlayGamesPlatform.Instance.GetUserDisplayName();
                                if (!string.IsNullOrEmpty(playName))
                                {
                                        return playName;
                                }
                        }
#endif

                        // Play Games ismi yoksa PlayerPrefs'ten al
                        string name = DataManager.Instance.GetString("D_NAME");
                        return string.IsNullOrEmpty(name) ? "Player" : name;
                }
#endif
        }
}
using System;
using System.IO;
using UnityEngine;
#if CRAZY_GAMES_SDK
using CrazyGames;
#endif
using Newtonsoft.Json;

namespace NemoUtility
{
#if CRAZY_GAMES_SDK
    public class CrazyGamesPlatform : Platform
    {
        private string _filePath = "";
        private Data _data = new Data();
        private Action _fullScreenCloseAction;
        private Action _rewardedCompleteAction;

        public override void OnEnable()
        {
            Debug.Log("<color=cyan>[CrazyGamesPlatform] OnEnable called. Initializing SDK...</color>");
            CrazySDK.Init(() =>
            {
                TriggerInitialized();
                Debug.Log("<color=green>[CrazyGamesPlatform] SDK Initialized Successfully!</color>");
                LoadDataFromCloud();
                SetupInviteLinkListener();
                CheckInitialInvite();
            });
        }

        public override void CheckInitialInvite()
        {
            if (!IsInitialized) 
            {
                Debug.LogWarning("[CrazyGamesPlatform] CheckInitialInvite called before SDK initialization. Skipping.");
                return;
            }

            // Tüm OnEnable/Start aboneliklerinin tamamlanması için bir kare bekletiyoruz
            PlatformManager.Instance.EnqueueMainThreadAction(() =>
            {
                var initialParams = CrazySDK.Game.InviteParams;
                if (initialParams != null && initialParams.Count > 0)
                {
                    Debug.Log("[CrazyGamesPlatform] <color=cyan>Initial Invite Params Found and Dispatched!</color>");
                    InviteLinkReceivedEvent?.Invoke(initialParams);
                }
                else
                {
                    Debug.Log("[CrazyGamesPlatform] No initial invite params found.");
                }
            });
        }

        private void SetupInviteLinkListener()
        {
            Debug.Log("[CrazyGamesPlatform] Setting up JoinRoomListener...");
            CrazySDK.Game.AddJoinRoomListener((parameters) =>
            {
                Debug.Log("[CrazyGamesPlatform] <color=cyan>Invite Link Received from SDK!</color>");
                InviteLinkReceivedEvent?.Invoke(parameters);
            });
        }

        private void LoadDataFromCloud()
        {
            string json = CrazySDK.Data.GetString("user_data", "");
            if (!string.IsNullOrEmpty(json))
            {
                try
                {
                    Data cloudData = JsonConvert.DeserializeObject<Data>(json);
                    if (cloudData != null && cloudData.Datas != null)
                    {
                        if (_data == null) _data = new Data();
                        if (_data.Datas == null) _data.Datas = new System.Collections.Generic.Dictionary<string, object>();

                        foreach (var kvp in cloudData.Datas)
                        {
                            _data.Datas[kvp.Key] = kvp.Value;
                        }
                    }
                }
                catch
                {
                }
            }

            if (_data == null)
            {
                _data = new Data();
            }
        }

        public override void OnDisable()
        {
            // Banners and ads handle themselves in CrazySDK, no explicit destroy needed
        }

        public override void FullScreenShow(Action finishAction)
        {
            _fullScreenCloseAction = finishAction;

            if (!IsInitialized)
            {
                InvokeFullScreenClose();
                return;
            }

            CrazySDK.Ad.RequestAd(CrazyAdType.Midgame,
                () =>
                {
                    Debug.Log("[CrazyGamesPlatform] Interstitial Started");
                    AudioListener.pause = true;
                    Time.timeScale = 0f;
                },
                (error) =>
                {
                    Debug.LogWarning($"[CrazyGamesPlatform] Interstitial Error: {error}");
                    AudioListener.pause = false;
                    Time.timeScale = 1f;
                    InvokeFullScreenClose();
                },
                () =>
                {
                    Debug.Log("[CrazyGamesPlatform] Interstitial Finished");
                    AudioListener.pause = false;
                    Time.timeScale = 1f;
                    InvokeFullScreenClose();
                }
            );
        }

        private void InvokeFullScreenClose()
        {
            PlatformManager.Instance.EnqueueMainThreadAction(() =>
            {
                _fullScreenCloseAction?.Invoke();
                _fullScreenCloseAction = null;
            });
        }

        public override void Rewarded(Action rewardComplateAction)
        {
            _rewardedCompleteAction = rewardComplateAction;

            if (!IsInitialized)
            {
                InvokeRewardedComplete();
                return;
            }

            CrazySDK.Ad.RequestAd(CrazyAdType.Rewarded,
                () =>
                {
                    Debug.Log("[CrazyGamesPlatform] Rewarded Started");
                    AudioListener.pause = true;
                    Time.timeScale = 0f;
                },
                (error) =>
                {
                    Debug.LogWarning($"[CrazyGamesPlatform] Rewarded Error: {error}");
                    AudioListener.pause = false;
                    Time.timeScale = 1f;
                    
                    PlatformManager.Instance.EnqueueMainThreadAction(() =>
                    {
                        // Eğer CrazyGames paneli Basic Launch modundaysa, testi engellememesi için ödülü ver.
                        if (error != null && error.code == "adsDisabledBasicLaunch")
                        {
                            InvokeRewardedComplete();
                        }
                        else
                        {
                            _rewardedCompleteAction = null;
                            // CrazyGames requirement: non-popup notice for adblocker/unavailable ads
                            if (NotificationManager.Instance != null)
                            {
                                NotificationManager.Instance.ShowNotification(
                                    "Ad unavailable. Disable adblocker for rewards.",
                                    Color.white, Color.yellow);
                            }
                        }
                    });
                },
                () =>
                {
                    Debug.Log("[CrazyGamesPlatform] Rewarded Finished");
                    AudioListener.pause = false;
                    Time.timeScale = 1f;
                    InvokeRewardedComplete();
                }
            );
        }

        private void InvokeRewardedComplete()
        {
            PlatformManager.Instance.EnqueueMainThreadAction(() =>
            {
                _rewardedCompleteAction?.Invoke();
                _rewardedCompleteAction = null;
            });
        }

        private CrazyBanner _banner;

        public override void ShowBanner()
        {
            // Banner geçici olarak devre dışı bırakıldı
        }

        public override void HideBanner()
        {
            // Banner geçici olarak devre dışı bırakıldı
        }

        public override Data GetAllData()
        {
            return _data;
        }

        public override object GetData(string id)
        {
            if (_data != null && _data.Datas != null && _data.Datas.TryGetValue(id, out object value))
            {
                return value;
            }
            return null;
        }

        public override void SetData(string id, object value)
        {
            if (_data == null) _data = new Data();
            if (_data.Datas == null) _data.Datas = new System.Collections.Generic.Dictionary<string, object>();

            if (FindId(_data, id))
            {
                _data.Datas[id] = value;
            }
            else
            {
                _data.Datas.Add(id, value);
            }

            if (IsInitialized)
            {
                string json = JsonConvert.SerializeObject(_data);
                CrazySDK.Data.SetString("user_data", json);
            }
        }

        public override void ResetData()
        {
            _data = new Data();
            if (IsInitialized)
            {
                CrazySDK.Data.DeleteKey("user_data");
            }
        }

        public override void SetLeaderBoardValue(string id, double value)
        {
            Debug.Log($"[CrazyGamesPlatform] SetLeaderBoardValue: {id} = {value}");
            // Optional: Implement CrazyGames Leaderboard logic here if needed
        }

        public override string GetLanguage()
        {
            return Application.systemLanguage.ToString().ToLower();
        }

        public override void SetLanguage(string lang)
        {
            SwitchLangEvent?.Invoke(lang);
        }

        public override void GameplayStart()
        {
            if (IsInitialized)
            {
                CrazySDK.Game.GameplayStart();
                Debug.Log("[CrazyGamesPlatform] GameplayStart called.");
            }
        }

        public override void GameplayStop()
        {
            if (IsInitialized)
            {
                CrazySDK.Game.GameplayStop();
                Debug.Log("[CrazyGamesPlatform] GameplayStop called.");
            }
        }

        public override void HappyTime()
        {
            if (IsInitialized)
            {
                CrazySDK.Game.HappyTime();
                Debug.Log("[CrazyGamesPlatform] HappyTime called.");
            }
        }

        public override void LoadingStart()
        {
            /*
            if (IsInitialized)
            {
                CrazySDK.Game.LoadingStart();
                Debug.Log("[CrazyGamesPlatform] LoadingStart called.");
            }
            */
        }

        public override void LoadingStop()
        {
            /*
            if (IsInitialized)
            {
                CrazySDK.Game.LoadingStop();
                Debug.Log("[CrazyGamesPlatform] LoadingStop called.");
            }
            */
        }

        public override string GetInviteLink(System.Collections.Generic.Dictionary<string, string> parameters)
        {
            if (IsInitialized)
            {
                string inviteLink = CrazySDK.Game.InviteLink(parameters);
                Debug.Log($"[CrazyGamesPlatform] Invite Link Generated: {inviteLink}");
                return inviteLink;
            }
            return string.Empty;
        }

        // --- Account Integration ---

        public override void GetCurrentUser(System.Action<string, string, string> callback)
        {
            if (!IsInitialized) { callback?.Invoke(null, null, null); return; }
            CrazySDK.User.GetUser((portalUser) =>
            {
                PlatformManager.Instance.EnqueueMainThreadAction(() =>
                {
                    if (portalUser != null)
                        callback?.Invoke(portalUser.__dangerousUserId, portalUser.username, portalUser.profilePictureUrl);
                    else
                        callback?.Invoke(null, null, null);
                });
            });
        }

        public override void GetUserToken(System.Action<string, string> callback)
        {
            if (!IsInitialized) { callback?.Invoke("notInitialized", null); return; }
            CrazySDK.User.GetUserToken((error, token) =>
            {
                PlatformManager.Instance.EnqueueMainThreadAction(() =>
                {
                    callback?.Invoke(error?.code, token);
                });
            });
        }

        public override void ShowAuthPrompt(System.Action<string, string, string, string> callback)
        {
            if (!IsInitialized) { callback?.Invoke("notInitialized", null, null, null); return; }
            CrazySDK.User.ShowAuthPrompt((error, user) =>
            {
                PlatformManager.Instance.EnqueueMainThreadAction(() =>
                {
                    if (error != null)
                        callback?.Invoke(error.code, null, null, null);
                    else
                        callback?.Invoke(null, user?.__dangerousUserId, user?.username, user?.profilePictureUrl);
                });
            });
        }

        public override void AddAuthListener(System.Action<string, string, string> listener)
        {
            if (!IsInitialized) return;
            CrazySDK.User.AddAuthListener((user) =>
            {
                PlatformManager.Instance.EnqueueMainThreadAction(() =>
                {
                    listener?.Invoke(user?.__dangerousUserId, user?.username, user?.profilePictureUrl);
                });
            });
        }

        public override bool IsUserAccountAvailable()
        {
            return IsInitialized && CrazySDK.User.IsUserAccountAvailable;
        }

        // --- Multiplayer Compliance ---

        public override void ShowInviteButton(System.Collections.Generic.Dictionary<string, string> parameters)
        {
            if (IsInitialized)
            {
                CrazySDK.Game.ShowInviteButton(parameters);
                Debug.Log("[CrazyGamesPlatform] ShowInviteButton called.");
            }
        }

        public override void HideInviteButton()
        {
            if (IsInitialized)
            {
                CrazySDK.Game.HideInviteButton();
                Debug.Log("[CrazyGamesPlatform] HideInviteButton called.");
            }
        }

        public override void UpdateRoom(string roomId, bool isJoinable, System.Collections.Generic.Dictionary<string, string> inviteParams)
        {
            if (!IsInitialized) return;
            CrazySDK.Game.UpdateRoom(new UpdateRoomInput
            {
                RoomId = roomId,
                IsJoinable = isJoinable,
                InviteParams = inviteParams
            });
            Debug.Log($"[CrazyGamesPlatform] UpdateRoom: {roomId}, joinable={isJoinable}");
        }

        public override void LeftRoom()
        {
            if (IsInitialized)
            {
                CrazySDK.Game.LeftRoom();
                Debug.Log("[CrazyGamesPlatform] LeftRoom called.");
            }
        }

        public override void ListFriends(int page, int size, System.Action<string, System.Collections.Generic.List<FriendData>> callback)
        {
            if (!IsInitialized) { callback?.Invoke("notInitialized", null); return; }
            CrazySDK.User.ListFriends(page, size, (error, friendsPage) =>
            {
                PlatformManager.Instance.EnqueueMainThreadAction(() =>
                {
                    if (error != null)
                    {
                        callback?.Invoke(error.code, null);
                        return;
                    }
                    var list = new System.Collections.Generic.List<FriendData>();
                    if (friendsPage?.friends != null)
                    {
                        foreach (var f in friendsPage.friends)
                            list.Add(new FriendData { id = f.id, username = f.username, profilePictureUrl = f.profilePictureUrl });
                    }
                    callback?.Invoke(null, list);
                });
            });
        }

        public override bool IsInstantMultiplayer()
        {
            return IsInitialized && CrazySDK.Game.IsInstantMultiplayer;
        }

        public override void SetGameContext(System.Collections.Generic.Dictionary<string, string> context)
        {
            if (IsInitialized)
            {
                CrazySDK.Game.SetGameContext(context);
                Debug.Log("[CrazyGamesPlatform] SetGameContext called.");
            }
        }

        public override string GetDisplayName()
        {
#if CRAZY_GAMES_SDK
            if (CrazyGamesAccountManager.Instance != null)
            {
                return CrazyGamesAccountManager.Instance.GetDisplayName();
            }
#endif
            return base.GetDisplayName();
        }
    }
#endif
}


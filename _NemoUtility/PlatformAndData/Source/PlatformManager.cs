using System;
using System.Collections.Generic;
using UnityEngine;

namespace NemoUtility
{
    [DefaultExecutionOrder(-10000)]
    public class PlatformManager : MonoBehaviour
    {
        public PlatformTypes PlatformTypes;

        private Platform _currentPlatform;


        private void OnEnable()
        {
            if (_currentPlatform != null)
            {
                _currentPlatform.OnEnable();
            }
        }
        private void OnDisable()
        {
            if (_currentPlatform != null)
            {
                _currentPlatform.OnDisable();
            }
        }


        public static PlatformManager Instance;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);

                // Otomatik Platform Seçimi
#if CRAZY_GAMES_SDK && UNITY_WEBGL
                PlatformTypes = PlatformTypes.CrazyGames;
#elif UNITY_ANDROID || UNITY_IOS
                PlatformTypes = PlatformTypes.Android;
#else
                PlatformTypes = PlatformTypes.Local;
#endif

                SetPlatform(PlatformTypes);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        private static readonly System.Collections.Concurrent.ConcurrentQueue<Action> _executionQueue = new System.Collections.Concurrent.ConcurrentQueue<Action>();

        public void EnqueueMainThreadAction(Action action)
        {
            if (action == null) return;
            _executionQueue.Enqueue(action);
        }

        private void Update()
        {
            while (_executionQueue.TryDequeue(out var action))
            {
                action?.Invoke();
            }
        }

        public void SetPlatform(PlatformTypes platformTypes)
        {
            switch (platformTypes)
            {
                case PlatformTypes.Local:
                    _currentPlatform = new LocalPlatform();
                    break;
                case PlatformTypes.YandexGames:
                    _currentPlatform = new YandexGamesPlatform();
                    break;
                case PlatformTypes.CrazyGames:
#if CRAZY_GAMES_SDK
                    _currentPlatform = new CrazyGamesPlatform();
#else
                    Debug.LogWarning("[PlatformManager] CrazyGames SDK is not defined (CRAZY_GAMES_SDK). Falling back to LocalPlatform.");
                    _currentPlatform = new LocalPlatform();
#endif
                    break;
                case PlatformTypes.Android:
                    // AndroidPlatform handles missing SDKs internally (AdMob, PlayGames)
                    _currentPlatform = new AndroidPlatform();
                    break;
                default:
                    _currentPlatform = new LocalPlatform();
                    break;
            }
        }

        public void FullScreenShow(Action finishAction)
        {
            _currentPlatform.FullScreenShow(finishAction);
        }
        public void Rewarded(Action rewardComplateAction)
        {
            _currentPlatform.Rewarded(rewardComplateAction);
        }
        public void ShowBanner()
        {
            _currentPlatform?.ShowBanner();
        }
        public void HideBanner()
        {
            _currentPlatform?.HideBanner();
        }

        public void GameplayStart()
        {
            _currentPlatform?.GameplayStart();
        }

        public void GameplayStop()
        {
            _currentPlatform?.GameplayStop();
        }

        public void HappyTime()
        {
            _currentPlatform?.HappyTime();
        }

        public void LoadingStart()
        {
            _currentPlatform?.LoadingStart();
        }

        public void LoadingStop()
        {
            _currentPlatform?.LoadingStop();
        }

        public Data GetAllData()
        {
            return _currentPlatform.GetAllData();
        }
        public object GetData(string id)
        {
            return _currentPlatform.GetData(id);
        }
        public void SetData(string id, object value)
        {
            _currentPlatform.SetData(id, value);
        }

        public void SetLeaderBoardValue(string id, double value)
        {
            _currentPlatform.SetLeaderBoardValue(id, value);
        }

        public string GetInviteLink(System.Collections.Generic.Dictionary<string, string> parameters)
        {
            return _currentPlatform?.GetInviteLink(parameters) ?? string.Empty;
        }

        public event Action<Dictionary<string, string>> OnInviteLinkReceived
        {
            add { if (_currentPlatform != null) _currentPlatform.InviteLinkReceivedEvent += value; }
            remove { if (_currentPlatform != null) _currentPlatform.InviteLinkReceivedEvent -= value; }
        }

        public bool IsPlatformInitialized => _currentPlatform?.IsInitialized ?? false;

        public event Action OnPlatformInitialized
        {
            add { if (_currentPlatform != null) _currentPlatform.OnInitialized += value; }
            remove { if (_currentPlatform != null) _currentPlatform.OnInitialized -= value; }
        }

        public string GetLanguage()
        {
            return _currentPlatform.GetLanguage();
        }
        public void SetLanguage(string lang)
        {
            _currentPlatform.SetLanguage(lang);
        }

        public void ResetData()
        {
            _currentPlatform.ResetData();
        }

        public Action<string> GetSwitchLangEvent()
        {
            return _currentPlatform.SwitchLangEvent;
        }

        // --- Account Integration ---
        public void GetCurrentUser(Action<string, string, string> callback)
        {
            _currentPlatform?.GetCurrentUser(callback);
        }

        public void GetUserToken(Action<string, string> callback)
        {
            _currentPlatform?.GetUserToken(callback);
        }

        public void ShowAuthPrompt(Action<string, string, string, string> callback)
        {
            _currentPlatform?.ShowAuthPrompt(callback);
        }

        public void AddAuthListener(Action<string, string, string> listener)
        {
            _currentPlatform?.AddAuthListener(listener);
        }

        public bool IsUserAccountAvailable()
        {
            return _currentPlatform?.IsUserAccountAvailable() ?? false;
        }

        // --- Multiplayer Compliance ---
        public void ShowInviteButton(Dictionary<string, string> parameters)
        {
            _currentPlatform?.ShowInviteButton(parameters);
        }

        public void HideInviteButton()
        {
            _currentPlatform?.HideInviteButton();
        }

        public void UpdateRoom(string roomId, bool isJoinable, Dictionary<string, string> inviteParams)
        {
            _currentPlatform?.UpdateRoom(roomId, isJoinable, inviteParams);
        }

        public void LeftRoom()
        {
            _currentPlatform?.LeftRoom();
        }

        public void ListFriends(int page, int size, Action<string, System.Collections.Generic.List<FriendData>> callback)
        {
            _currentPlatform?.ListFriends(page, size, callback);
        }

        public bool IsInstantMultiplayer()
        {
            return _currentPlatform?.IsInstantMultiplayer() ?? false;
        }

        public void SetGameContext(Dictionary<string, string> context)
        {
            _currentPlatform?.SetGameContext(context);
        }

        public void CheckInviteParams()
        {
            _currentPlatform?.CheckInitialInvite();
        }

        public string GetDisplayName()
        {
            string name = _currentPlatform?.GetDisplayName();
            
            if (string.IsNullOrEmpty(name))
            {
                name = DataManager.Instance.GetString("D_NAME");
                if (string.IsNullOrEmpty(name))
                {
                    name = "Player";
                }
            }

            // Eger platformdan veya lokalden isim gelmediyse bos birakiyoruz. 
            // CloudDataService bu boslugu sunucuya iletecek ve sunucu Player_0, Player_1 gibi sirali isim atayacak.
            return name;
        }
    }
}
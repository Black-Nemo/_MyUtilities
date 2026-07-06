using System;
using System.Collections.Generic;

namespace NemoUtility
{
    [System.Serializable]
    public class FriendData
    {
        public string id;
        public string username;
        public string profilePictureUrl;
    }

    public abstract class Platform
    {
        public Action<string> SwitchLangEvent;
        public Action<Dictionary<string, string>> InviteLinkReceivedEvent;

        public bool IsInitialized { get; protected set; }
        public event Action OnInitialized;

        protected void TriggerInitialized()
        {
            IsInitialized = true;
            OnInitialized?.Invoke();
        }

        public abstract void OnEnable();
        public abstract void OnDisable();

        public abstract void FullScreenShow(Action finishAction);
        public abstract void Rewarded(Action rewardComplateAction);

        public abstract void ShowBanner();
        public abstract void HideBanner();

        public abstract Data GetAllData();
        public abstract object GetData(string id);
        public abstract void SetData(string id, object value);
        public abstract void SetLeaderBoardValue(string id, double value);
        public abstract string GetLanguage();
        public abstract void SetLanguage(string lang);
        public abstract void ResetData();

        public virtual void GameplayStart() { }
        public virtual void GameplayStop() { }
        public virtual void HappyTime() { }
        public virtual void LoadingStart() { }
        public virtual void LoadingStop() { }

        public virtual string GetInviteLink(Dictionary<string, string> parameters) { return string.Empty; }

        // --- Account Integration ---
        public virtual void GetCurrentUser(Action<string, string, string> callback) { callback?.Invoke(null, null, null); }
        public virtual void GetUserToken(Action<string, string> callback) { callback?.Invoke("unsupported", null); }
        public virtual void ShowAuthPrompt(Action<string, string, string, string> callback) { callback?.Invoke("unsupported", null, null, null); }
        public virtual void AddAuthListener(Action<string, string, string> listener) { }
        public virtual bool IsUserAccountAvailable() { return false; }

        // --- Multiplayer Compliance ---
        public virtual void ShowInviteButton(Dictionary<string, string> parameters) { }
        public virtual void HideInviteButton() { }
        public virtual void UpdateRoom(string roomId, bool isJoinable, Dictionary<string, string> inviteParams) { }
        public virtual void LeftRoom() { }
        public virtual void ListFriends(int page, int size, Action<string, List<FriendData>> callback) { callback?.Invoke("unsupported", null); }
        public virtual bool IsInstantMultiplayer() { return false; }
        public virtual void SetGameContext(Dictionary<string, string> context) { }
        public virtual void CheckInitialInvite() { }
        public virtual string GetDisplayName()
        {
            string name = DataManager.Instance.GetString("D_NAME");
            return string.IsNullOrEmpty(name) ? "Player" : name;
        }

        protected bool FindId(Data data, string id)
        {
            return data.Datas.ContainsKey(id);
        }
    }
}
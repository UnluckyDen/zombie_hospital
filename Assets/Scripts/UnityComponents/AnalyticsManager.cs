using Extension;
using Facebook.Unity;
using GameAnalyticsSDK;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityComponents
{
    public class AnalyticsManager : Singleton<AnalyticsManager>
    {
        [SerializeField] private bool debug = true;

        private void InitCallback()
        {
            if (FB.IsInitialized)
            {
                FB.ActivateApp();
                GameStart();
            }
            else Debug.Log("Failed to Initialize the Facebook SDK");
        }

        private static void OnHideUnity(bool isGameShown)
        {
            Time.timeScale = isGameShown ? 1 : 0;
        }

        private void Start()
        {
            GameAnalytics.Initialize();

            if (!FB.IsInitialized) FB.Init(InitCallback, OnHideUnity);
            else FB.ActivateApp();
        }

        private void GameStart()
        {
            SendMessageEvent("game start");
            _ = LevelManager.Instance;
            SceneManager.LoadScene(1);
        }

        public void LevelStart(int levelNumber)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "level", levelNumber);
            FB.LogAppEvent("level start", levelNumber);
        
            if (debug) Debug.Log($"SEND EVENT: {GAProgressionStatus.Start} level {levelNumber}");
        }
        
        public void LevelEnd(int levelNumber)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "level", levelNumber);
            FB.LogAppEvent("level_end", levelNumber);
        
            if (debug) Debug.Log($"SEND EVENT: {GAProgressionStatus.Complete} level {levelNumber}");
        }
        
        private void SendMessageEvent(string message)
        {
            GameAnalytics.NewDesignEvent(message);
            FB.LogAppEvent(message);
        
            if (debug) Debug.Log($"SEND EVENT: {message}");
        }
    }
}
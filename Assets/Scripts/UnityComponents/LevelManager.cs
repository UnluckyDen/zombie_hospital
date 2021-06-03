using Extension;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityComponents
{
    public class LevelManager : Singleton<LevelManager>
    {
        public bool teethComplete;
        public bool limbComplete;
        public bool trepanationComplete;
        
        private const int MinLevelNumber = 1;
        private const int MaxLevelNumber = 3;
        
        private int _startLevelIndex = 1;
        private int _nextLevelIndex;
        private int _levelsCompleted;

        private void Start()
        {
            Reset();
        }

        public void SetTeethComplete()
        {
            teethComplete = true;
        }

        public void SetLimbComplete()
        {
            limbComplete = true;
        }

        public void SetTrepanationComplete()
        {
            trepanationComplete = true;
        }

        public void LoadNextLevel()
        {
            _nextLevelIndex++;
            _levelsCompleted++;
            if (_nextLevelIndex > MaxLevelNumber)
            {
                _nextLevelIndex = MinLevelNumber;
            }
            if (_levelsCompleted == MaxLevelNumber)
            {
                _startLevelIndex++;
                if (_startLevelIndex > MaxLevelNumber)
                    _startLevelIndex = MinLevelNumber;
                Reset();
                ZombieManager.Instance.IncrementZombieIndex();
                ZombieManager.Instance.ResetData();
                MoneyManager.Instance.AddMoney(300);
                SceneManager.LoadScene(4);
                return;
            }
            SceneManager.LoadScene(_nextLevelIndex);
        }

        public void ReturnToGameplay()
        {
            SceneManager.LoadScene(_nextLevelIndex);
        }

        private void Reset()
        {
            _levelsCompleted = 0;
            _nextLevelIndex = _startLevelIndex;
            teethComplete = false;
            limbComplete = false;
            trepanationComplete = false;
        }
    }
}
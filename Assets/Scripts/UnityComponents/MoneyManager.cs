using System;
using Extension;
using UnityEngine;
using UnityEngine.Events;

namespace UnityComponents
{
    public class MoneyManager : Singleton<MoneyManager>
    {
        public UnityEvent onMoneyChanged = new UnityEvent();

        private void Awake()
        {
            if (PlayerPrefs.GetInt("StartedMoney") != 0) return;
            PlayerPrefs.SetInt("StartedMoney", 1);
            AddMoney(200);
        }

        public void Reset()
        {
            PlayerPrefs.SetInt("StartedMoney", 1);
            PlayerPrefs.SetInt("Money", 200);
        }

        private static int Money
        {
            get => PlayerPrefs.GetInt("Money");
            set => PlayerPrefs.SetInt("Money", value);
        }

        public void InvokeOnMoneyChanged()
        {
            onMoneyChanged.Invoke();
        }

        public static int GetMoney()
        {
            return Money;
        }

        public void AddMoney(int value)
        {
            Money += value;
            onMoneyChanged.Invoke();
        }

        public bool SubMoney(int value)
        {
            if (value > Money) return false;
            Money -= value;
            onMoneyChanged.Invoke();
            return true;
        }
    }
}
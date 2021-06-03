using System;
using UnityEngine;

namespace Extension
{
    public class Singleton<T> : MonoBehaviour 
        where T : MonoBehaviour
    {
        private static T _instance;
        private static readonly object _lock = new object();

        public static T Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance != null || (_instance = FindObjectOfType<T>()) != null)
                    {
                        DontDestroyOnLoad(_instance);
                        return _instance;
                    }

                    _instance = new GameObject("SINGLETON " + typeof(T)).AddComponent<T>();
                    DontDestroyOnLoad(_instance);
                    return _instance;
                }
            }
        }
    }
}
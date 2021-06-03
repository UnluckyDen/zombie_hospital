using System.Collections.Generic;
using GameAnalyticsSDK.Setup;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "ZombieData", menuName = "Zombie Hospital/Zombie Data", order = 0)]
    public class ZombieData : ScriptableObject
    {
        [System.Serializable]
        public struct ZombieSettingsStruct
        {
            public string name;
            
            public GameObject zombieTeethPrefab;
            public GameObject zombieLimbPrefab;
            public GameObject zombieTrepanationPrefab;

            public List<GameObject> trayHandPrefabs;
            public List<GameObject> handPrefab;

            public Sprite zombieAvatar;

            public GameObject springHand;
            public GameObject hand;
        }
        
        public List<ZombieSettingsStruct> zombieSettings;
    }
}
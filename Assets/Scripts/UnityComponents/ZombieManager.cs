using Data;
using Extension;
using UnityEngine;

namespace UnityComponents
{
    public class ZombieManager : Singleton<ZombieManager>
    {
        [HideInInspector] public ZombieData zombieData;
        [HideInInspector] public TeethIformation.TeethStruct zombieTooth;
        [HideInInspector] public int zombieIndex;

        private const int MaxZombieIndex = 4;
            
        private int? _handIndex = null;

        public bool tutorial = true;
        
        public GameObject GetHand()
        {
            return _handIndex == null 
                ?  zombieData.zombieSettings[zombieIndex].hand
                : zombieData.zombieSettings[zombieIndex].handPrefab[_handIndex.Value];
        }
        
        public GameObject GetSpringHand()
        {
            return _handIndex == null 
                ?  zombieData.zombieSettings[zombieIndex].springHand
                : zombieData.zombieSettings[zombieIndex].handPrefab[_handIndex.Value];
        }

        public Sprite GetAvatar()
        {
            return zombieData.zombieSettings[zombieIndex].zombieAvatar;
        }

        public void SetHandIndex(int index)
        {
            _handIndex = index;
        }

        public void IncrementZombieIndex()
        {
            zombieIndex++;
            if (zombieIndex > MaxZombieIndex)
                zombieIndex = 0;
            if (zombieIndex == 2)
                tutorial = false;
        }

        public void ResetData()
        {
            zombieTooth = new TeethIformation.TeethStruct();
            _handIndex = null;
        }
    }
}

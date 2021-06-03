using System;
using Extension;
using UnityEngine;

namespace UnityComponents
{
    public class GameManager : Singleton<GameManager>
    {
        public bool mute;

        public static int FurnitureIndex
        {
            get => PlayerPrefs.GetInt("FurnitureIndex");
            set => PlayerPrefs.SetInt("FurnitureIndex", value);
        }

        public static int DecorIndex
        {
            get => PlayerPrefs.GetInt("DecorIndex");
            set => PlayerPrefs.SetInt("DecorIndex", value);
        }

        public static int RoomIndex
        {
            get
            {
                var roomIndex = PlayerPrefs.GetInt("RoomIndex");
                return roomIndex == 0 ? 1 : roomIndex;
            }
            set => PlayerPrefs.SetInt("RoomIndex", value);
        }

        [ContextMenu("Reset values")]
        private void ResetValues()
        {
            FurnitureIndex = 0;
            DecorIndex = 0;
            RoomIndex = 1;
            MoneyManager.Instance.Reset();
        }
    }
}
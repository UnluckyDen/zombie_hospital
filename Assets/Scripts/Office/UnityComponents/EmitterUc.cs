using UnityEngine;
using UnityEngine.UI;

namespace Office.UnityComponents
{
    public class EmitterUc : MonoBehaviour
    {
        [System.Serializable]
        public struct UpgradeButtonStruct
        {
            public Button button;
            public Text costText;
            public Text levelText;
        }
        
        public Text moneyText;
        public Button nextDayButton;
        public UpgradeButtonStruct furnitureUpgrade;
        public UpgradeButtonStruct decorUpgrade;
        public UpgradeButtonStruct interiorUpgrade;
    }
}

using Leopotam.Ecs;
using Office.Components;
using Office.UnityComponents;
using UnityComponents;

namespace Office.Systems
{
    public class EmitterSystem : IEcsInitSystem
    {
        private readonly EcsWorld _world = null;
        private readonly EmitterUc _emitterUc = null;

        public void Init()
        {
            MoneyManager.Instance.onMoneyChanged.RemoveAllListeners();
            MoneyManager.Instance.onMoneyChanged.AddListener(RefreshMoney);
            MoneyManager.Instance.InvokeOnMoneyChanged();

            _emitterUc.nextDayButton.onClick.AddListener(() =>
            {
                MoneyManager.Instance.onMoneyChanged.RemoveAllListeners();
                LevelManager.Instance.ReturnToGameplay();
            });

            RefreshButtons();
        }

        private void RefreshButtons()
        {
            //furniture
            LocationManager.Instance.GetFurnitureInfo(out var furnitureCost, out var furnitureLevel,
                out var furnitureMaxLevel, out var cameraPosition, out var cameraRotation);
            var furnitureUpgrade = _emitterUc.furnitureUpgrade;
            if (!furnitureMaxLevel)
            {
                furnitureUpgrade.button.interactable = true;
                furnitureUpgrade.costText.text = $"{furnitureCost}$";
                furnitureUpgrade.levelText.text = (furnitureLevel + 1).ToString();
                furnitureUpgrade.button.interactable = furnitureCost < MoneyManager.GetMoney();
            }
            else
            {
                furnitureUpgrade.button.interactable = false;
                furnitureUpgrade.costText.text = string.Empty;
                furnitureUpgrade.levelText.text = "MAX";
            }
            _emitterUc.furnitureUpgrade.button.onClick.RemoveAllListeners();
            _emitterUc.furnitureUpgrade.button.onClick.AddListener(() =>
            {
                if(!MoneyManager.Instance.SubMoney(furnitureCost)) return;
                GameManager.FurnitureIndex++;
                _world.NewEntity().Replace(new CameraPositionEvent
                {
                    CameraPosition = cameraPosition,
                    CameraRotation = cameraRotation
                });
                RefreshButtons();
                LocationManager.Instance.RefreshRoom();
            });

            //decor
            LocationManager.Instance.GetDecorInfo(out var decorCost, out var decorLevel, out var decorMaxLevel,
                out var decorCameraPosition, out var decorCameraRotation);
            var decorUpgrade = _emitterUc.decorUpgrade;
            if (furnitureLevel < 3)
            {
                decorUpgrade.button.interactable = false;
                decorUpgrade.costText.text = "locked";
                decorUpgrade.levelText.text = "-";
            }
            else if (!decorMaxLevel)
            {
                decorUpgrade.button.interactable = true;
                decorUpgrade.costText.text = $"{decorCost}$";
                decorUpgrade.levelText.text = (decorLevel + 1).ToString();
                decorUpgrade.button.interactable = decorCost < MoneyManager.GetMoney();
            }
            else
            {
                decorUpgrade.button.interactable = false;
                decorUpgrade.costText.text = string.Empty;
                decorUpgrade.levelText.text = "MAX";
            }
            _emitterUc.decorUpgrade.button.onClick.RemoveAllListeners();
            _emitterUc.decorUpgrade.button.onClick.AddListener(() =>
            {
                if(!MoneyManager.Instance.SubMoney(decorCost)) return;
                GameManager.DecorIndex++;
                _world.NewEntity().Replace(new CameraPositionEvent
                {
                    CameraPosition = decorCameraPosition,
                    CameraRotation = decorCameraRotation
                });
                RefreshButtons();
                LocationManager.Instance.RefreshRoom();
            });

            //room
            LocationManager.Instance.GetRoomInfo(out var roomCost, out var roomLevel, out var roomMaxLevel);
            var roomUpgrade = _emitterUc.interiorUpgrade;
            if (!roomMaxLevel)
            {
                roomUpgrade.button.interactable = true;
                roomUpgrade.costText.text = $"{roomCost}$";
                roomUpgrade.levelText.text = roomLevel.ToString();
                roomUpgrade.button.interactable = roomCost < MoneyManager.GetMoney();
            }
            else
            {
                roomUpgrade.button.interactable = false;
                roomUpgrade.costText.text = string.Empty;
                roomUpgrade.levelText.text = "MAX";
            }
            _emitterUc.interiorUpgrade.button.onClick.RemoveAllListeners();
            _emitterUc.interiorUpgrade.button.onClick.AddListener(() =>
            {
                if(!MoneyManager.Instance.SubMoney(roomCost)) return;
                GameManager.RoomIndex++;
                RefreshButtons();
                LocationManager.Instance.RefreshRoom();
            });
        }

        private void RefreshMoney()
        {
            _emitterUc.moneyText.text = $"{MoneyManager.GetMoney()}$";
        }
    }
}
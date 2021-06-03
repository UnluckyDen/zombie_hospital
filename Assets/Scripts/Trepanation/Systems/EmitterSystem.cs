using Leopotam.Ecs;
using Trepanation.Components;
using Trepanation.UnityComponents;
using UnityComponents;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Trepanation.Systems
{
    public class EmitterSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsWorld _world = null;
        private readonly EmitterUc _emitterUc = null;
        private readonly EcsFilter<ItemComponent> _itemFilter = null;
        private readonly EcsFilter<FastenEvent> _fastenFilter = null;
        private readonly EcsFilter<CompletedEvent> _completedFilter = null;

        private bool _prevConnected;
        private bool _playWin;
        private bool _showInterface;
        private bool _gamePause;
        
        private const float Tolerance = 0.01f;

        public void Init()
        {
            _emitterUc.pauseButton.onClick.AddListener(() =>
            {
                _gamePause = !_gamePause;
                Time.timeScale = _gamePause ? 0 : 1;
                _emitterUc.pauseButton.GetComponent<Image>().sprite =
                    _gamePause ? _emitterUc.playSprite : _emitterUc.pauseSprite;
            });
            _emitterUc.soundButton.GetComponent<Image>().sprite = SoundManager.Instance.GetMuteSprite();
            _emitterUc.soundButton.onClick.AddListener(() =>
            {
                SoundManager.Instance.MuteButton();
                _emitterUc.soundButton.GetComponent<Image>().sprite = SoundManager.Instance.GetMuteSprite();
            });
            _emitterUc.chooseThisButton.onClick.AddListener(() =>
            {
                if (_fastenFilter.IsEmpty())
                    _world.NewEntity().Get<FastenEvent>();
                //_emitterUc.finishScreenParent.SetActive(true);
                _emitterUc.chooseThisButton.gameObject.SetActive(false);
            });
            
            _emitterUc.finishButton.onClick.AddListener(() =>
            {
                // ZombieManager.Instance.IncrementZombieIndex();
                AnalyticsManager.Instance.LevelEnd(3);
                // SceneManager.LoadScene(LevelManager.Instance.LoadNextLevel());
                LevelManager.Instance.LoadNextLevel();
            });
        }

        public void Run()
        {
            if (!_completedFilter.IsEmpty())
            {
                if (!_showInterface)
                {
                    LevelManager.Instance.trepanationComplete = true;

                    _emitterUc.avatar.sprite = ZombieManager.Instance.GetAvatar();

                    var brain = LevelManager.Instance.trepanationComplete;
                    var hand = LevelManager.Instance.limbComplete;
                    var teeth = LevelManager.Instance.teethComplete;

                    _emitterUc.brainCheckMark.enabled = brain;
                    _emitterUc.handCheckMark.enabled = hand;
                    _emitterUc.teethCheckMark.enabled = teeth;

                    var progressBarValue = 0;
                    if (brain) progressBarValue++;
                    if (hand) progressBarValue++;
                    if (teeth) progressBarValue++;

                    _emitterUc.progressBar.fillAmount = progressBarValue / 3f;
                    if (progressBarValue == 3)
                    {
                        _emitterUc.stamp.enabled = true;
                        _emitterUc.moneyText.enabled = true;
                    }

                    _emitterUc.finishScreenParent.SetActive(true);
                    _showInterface = true;
                }

                if (!_playWin)
                {
                    SoundManager.Instance.PlayWin();
                    _playWin = true;
                }
            }
            if(!_fastenFilter.IsEmpty()) return;
            
            // Debug.Log("Emitter System");
            
            var connected = false;
            foreach (var idx in _itemFilter)
            {
                ref var itemComponent = ref _itemFilter.Get1(idx);
                if (itemComponent.Attached && 
                    Mathf.Abs(itemComponent.NewPosition.x - itemComponent.Transform.position.x) < Tolerance &&
                    Mathf.Abs(itemComponent.NewPosition.y - itemComponent.Transform.position.y) < Tolerance &&
                    Mathf.Abs(itemComponent.NewPosition.z - itemComponent.Transform.position.z) < Tolerance)
                    connected = true;
            }
            _emitterUc.chooseThisButton.gameObject.SetActive(connected);

            if (connected && !_prevConnected)
            {
                SoundManager.Instance.PlayZombieReaction();
            }

            _prevConnected = connected;
        }
    }
}
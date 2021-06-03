using Leopotam.Ecs;
using Limb.Components;
using Limb.UnityComponents;
using UnityComponents;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Limb.Systems
{
    public class EmitterSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsWorld _world = null;
        private readonly EcsFilter<HandSelectedEvent> _handSelectedFilter = null;
        private readonly EcsFilter<HandComponent> _handFilter = null;
        private readonly EcsFilter<CompletedEvent> _completedFilter = null;
        private readonly EmitterUc _emitterUc = null;

        private bool _prevConnected;
        private bool _buttonClick;
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
            _emitterUc.attachButton.onClick.AddListener(() =>
            {
                if (_handSelectedFilter.IsEmpty())
                    _world.NewEntity().Get<HandSelectedEvent>();
                var handIndex = 0;
                foreach (var idx in _handFilter)
                {
                    ref var handComponent = ref _handFilter.Get1(idx);
                    if (!handComponent.Attached) continue;
                    handIndex = handComponent.HandIndex;
                    break;
                }
                ZombieManager.Instance.SetHandIndex(handIndex);
                _emitterUc.attachButton.gameObject.SetActive(false);
            });
        }

        public void Run()
        {
            if (!_completedFilter.IsEmpty())
            {
                if (!_showInterface)
                {
                    LevelManager.Instance.limbComplete = true;

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

                    if (progressBarValue == 3)
                    {
                        _emitterUc.stamp.enabled = true;
                        _emitterUc.moneyText.enabled = true;
                    }

                    _emitterUc.progressBar.fillAmount = progressBarValue / 3f;

                    _emitterUc.finishScreenParent.SetActive(true);
                    _showInterface = true;
                }

                _emitterUc.nextButton.onClick.AddListener(() =>
                {
                    if (!_buttonClick)
                    {
                        AnalyticsManager.Instance.LevelEnd(2);
                        // SceneManager.LoadScene(LevelManager.Instance.LoadNextLevel());
                        LevelManager.Instance.LoadNextLevel();
                        _buttonClick = true;
                    }
                });
            }
            if (!_handSelectedFilter.IsEmpty()) return;
            var connected = false;
            foreach (var idx in _handFilter)
            {
                ref var handComponent = ref _handFilter.Get1(idx);
                if (handComponent.Attached && 
                    Mathf.Abs(handComponent.NewPosition.x - handComponent.Transform.position.x) < Tolerance &&
                    Mathf.Abs(handComponent.NewPosition.y - handComponent.Transform.position.y) < Tolerance &&
                    Mathf.Abs(handComponent.NewPosition.z - handComponent.Transform.position.z) < Tolerance)
                    connected = true;
            }
            _emitterUc.attachButton.gameObject.SetActive(connected);
            if (connected && !_prevConnected)
            {
                SoundManager.Instance.PlayZombieReaction();
            }
            _prevConnected = connected;
        }
    }
}
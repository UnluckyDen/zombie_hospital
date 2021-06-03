using Leopotam.Ecs;
using Teeth.Components;
using Teeth.UnityComponents;
using UnityComponents;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Teeth.Systems
{
    public class EmitterSystem : IEcsInitSystem
    {
        private readonly EcsWorld _world = null;
        private readonly EmitterUc _emitterUc = null;
        private readonly EcsFilter<ToothComponent> _toothFilter = null;
        
        private bool _gamePause;
        
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
                GameObject toothGameObject = null;
                foreach (var idx in _toothFilter)
                {
                    ref var toothComponent = ref _toothFilter.Get1(idx);
                    if (!toothComponent.InMouth) continue;
                    toothGameObject = toothComponent.ToothPrefab;
                    break;
                }

                ZombieManager.Instance.zombieTooth = toothGameObject.GetComponent<TeethIformation>().teethStruct;

                _world.NewEntity().Get<CompletedEvent>();
                SoundManager.Instance.PlayWin();
                LevelManager.Instance.SetTeethComplete();
                _emitterUc.chooseThisButton.gameObject.SetActive(false);
            });
            
            _emitterUc.nextButton.onClick.AddListener(() =>
            {
                AnalyticsManager.Instance.LevelEnd(1);
                // SceneManager.LoadScene(LevelManager.Instance.LoadNextLevel());
                LevelManager.Instance.LoadNextLevel();
            });
        }
    }
}
using Leopotam.Ecs;
using Limb.Components;
using Limb.Data;
using Limb.UnityComponents;
using MoreMountains.NiceVibrations;
using UnityEngine;

namespace Limb.Systems
{
    public class SpinSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world = null;
        private readonly EcsFilter<CompletedEvent> _completedFilter = null;
        private readonly EmitterUc _emitterUc = null;
        private readonly LimbData _limbData = null;
        private readonly EcsFilter<ScrewdriverComponent> _screwdriverFilter = null;
        private readonly EcsFilter<ScrewComponent> _screwFilter = null;
        private readonly EcsFilter<HandSelectedEvent> _handSelectedFilter = null;
        private readonly EcsFilter<ZombieComponent> _zombieFilter = null;

        private static readonly int End = Animator.StringToHash("End");
        
        private float _spinScrewProgress = 0;
        private float _spinScrewdriverProgress = 0;
        private bool _rotateScrew;
        private bool _screwdriverReady;

        public void Run()
        {
            if (_handSelectedFilter.IsEmpty()) return;
            if (!_completedFilter.IsEmpty())
            {
                SoundManager.Instance.ScrewDriverPlay(false);
                _emitterUc.barParent.SetActive(false);
                return;
            }

            if (!_screwdriverReady)
            {
                foreach (var idx in _screwdriverFilter)
                {
                    ref var screwdriverComponent = ref _screwdriverFilter.Get1(idx);
                    _screwdriverReady = screwdriverComponent.Ready;
                }
                return;
            }

            if (!_emitterUc.barParent.activeInHierarchy)
                _emitterUc.barParent.SetActive(true);
            
            if (!Input.GetMouseButton(0))
            {
                if (_spinScrewdriverProgress > 0)
                    _spinScrewdriverProgress -= Time.deltaTime;

                foreach (var idx in _screwdriverFilter)
                {
                    ref var screwdriverComponent = ref _screwdriverFilter.Get1(idx);

                    if (screwdriverComponent.StartPosition == null)
                        return;

                    screwdriverComponent.Transform.position = Vector3.Lerp(
                        screwdriverComponent.StartPosition.Value,
                        screwdriverComponent.StartPosition.Value -
                        screwdriverComponent.Transform.forward * _limbData.spinDistance,
                        _spinScrewdriverProgress);
                }
                SoundManager.Instance.ScrewDriverPlay(false);
                MMVibrationManager.StopContinuousHaptic();

                return;
            }
            
            MMVibrationManager.ContinuousHaptic(0.9f, 0.07f, 0.1f, HapticTypes.LightImpact, null, true, -1, true);
            SoundManager.Instance.ScrewDriverPlay(true);

            if (_rotateScrew = _spinScrewdriverProgress > _spinScrewProgress)
            {
                _spinScrewdriverProgress += Time.deltaTime / 3;
                _spinScrewProgress += Time.deltaTime / 3;
                _emitterUc.barForeground.fillAmount = _spinScrewProgress;

                if (_spinScrewProgress >= 1)
                {
                    _world.NewEntity().Get<CompletedEvent>();
                    SoundManager.Instance.PlayWin();
                    foreach (var idz in _zombieFilter)
                    {
                        ref var zombieComponent = ref _zombieFilter.Get1(idz);
                        zombieComponent.Animator.SetBool(End, true);
                    }
                }
            }
            else
            {
                _spinScrewdriverProgress += Time.deltaTime * 2;
            }

            foreach (var idx in _screwdriverFilter)
            {
                ref var screwdriverComponent = ref _screwdriverFilter.Get1(idx);

                if (screwdriverComponent.StartPosition == null)
                    screwdriverComponent.StartPosition = screwdriverComponent.Transform.position;

                if (_rotateScrew)
                {
                    screwdriverComponent.ScrewdriverUc.spinTransform.Rotate(
                        Time.deltaTime * -_limbData.rotationSpeed,
                        0,
                        0);
                }

                screwdriverComponent.Transform.position = Vector3.Lerp(
                    screwdriverComponent.StartPosition.Value,
                    screwdriverComponent.StartPosition.Value -
                    screwdriverComponent.Transform.forward * _limbData.spinDistance,
                    _spinScrewdriverProgress);
            }

            foreach (var idx in _screwFilter)
            {
                ref var screwComponent = ref _screwFilter.Get1(idx);

                if (screwComponent.StartPosition == null)
                {
                    screwComponent.ReturnToOriginal = false;
                    screwComponent.StartPosition = screwComponent.Transform.position;
                }

                screwComponent.Transform.position = Vector3.Lerp(
                    screwComponent.StartPosition.Value,
                    screwComponent.StartPosition.Value - screwComponent.Transform.forward * _limbData.spinDistance,
                    _spinScrewProgress);

                if (!_rotateScrew) continue;
                screwComponent.ScrewUc.spinTransform.Rotate(
                    0,
                    0,
                    Time.deltaTime * _limbData.rotationSpeed);
            }
        }
    }
}
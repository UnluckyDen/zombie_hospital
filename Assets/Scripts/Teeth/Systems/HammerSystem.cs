using Leopotam.Ecs;
using MoreMountains.NiceVibrations;
using Teeth.Components;
using Teeth.Data;
using Teeth.UnityComponents;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Teeth.Systems
{
    public class HammerSystem : IEcsInitSystem, IEcsRunSystem
    {
        private static readonly int KickState = Animator.StringToHash("Kick");

        private readonly Camera _camera = null;
        private readonly EcsWorld _world = null;
        private readonly TeethData _teethData = null;
        private readonly EcsFilter<HammerComponent> _hammerFilter = null;
        private readonly EcsFilter<ZombieComponent> _zombieFilter = null;
        private readonly EcsFilter<ToothlessEvent> _toothlessFilter = null;

        private Vector3 _hammerPosition;
        private bool _needKick;

        public void Init()
        {
            _hammerPosition = _teethData.hammerPosition;
            var gameObject = Object.Instantiate(_teethData.hammerPrefab, _hammerPosition, Quaternion.identity);

            var entity = _world.NewEntity();
            entity.Replace(new HammerComponent
            {
                GameObject = gameObject,
                Transform = gameObject.transform,
                Animator = gameObject.GetComponent<Animator>()
            });
        }

        public void Run()
        {
            if (!_toothlessFilter.IsEmpty()) return;

            foreach (var idx in _hammerFilter)
            {
                ref var hammerComponent = ref _hammerFilter.Get1(idx);

                foreach (var idy in _zombieFilter)
                {
                    ref var zombieComponent = ref _zombieFilter.Get1(idy);

                    if (zombieComponent.ZombiePointsUc.teethProgress.completed)
                    {
                        _world.NewEntity().Get<ToothlessEvent>();
                        zombieComponent.Animator.SetBool(KickState, false);
                        hammerComponent.GameObject.SetActive(false);
                    }

                    #region HammerPosition

                    var currentPosition = hammerComponent.Transform.position;
                    currentPosition = Vector3.Lerp(currentPosition, _hammerPosition,
                        Time.deltaTime * _teethData.hammerSpeed);
                    hammerComponent.Transform.position = currentPosition;

                    if (_needKick && Vector3.Distance(currentPosition, _hammerPosition) < 0.02f)
                    {
                        hammerComponent.Animator.SetBool(KickState, true);
                        zombieComponent.Animator.SetBool(KickState, true);
                        SoundManager.Instance.PlayHammer(0.1f);
                        SoundManager.Instance.PlayBrokenTeeth(0.5f);
                        
                        MMVibrationManager.Haptic(HapticTypes.HeavyImpact, false, true);
                        
                        _needKick = false;
                    }
                    else
                    {
                        hammerComponent.Animator.SetBool(KickState, false);
                        zombieComponent.Animator.SetBool(KickState, false);
                    }

                    if (!Input.GetMouseButtonDown(0) || hammerComponent.Animator.GetBool(KickState)) return;
                    if (!Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out var hit, 100)) return;
                    if (hit.collider.name != "teethCollider") return;
                    _needKick = true;

                    var xPosition = Mathf.Clamp(hit.point.x, -_teethData.xRange, _teethData.xRange);
                    var yPosition = hit.point.y;
                    _hammerPosition = new Vector3(xPosition, yPosition - 0.29f, _hammerPosition.z);

                    #endregion
                }
            }
        }
    }
}
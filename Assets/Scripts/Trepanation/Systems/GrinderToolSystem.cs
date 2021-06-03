using Leopotam.Ecs;
using MoreMountains.NiceVibrations;
using Trepanation.Components;
using Trepanation.Data;
using Trepanation.UnityComponents;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Trepanation.Systems
{
    public class GrinderToolSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsWorld _world = null;
        private readonly Camera _camera = null;
        private readonly TrepanationData _trepanationData = null;
        private readonly EcsFilter<ZombieComponent> _zombieFilter = null;
        private readonly EcsFilter<ZombieRotateEvent> _zombieRotateFilter = null;
        private readonly EcsFilter<ExtractionEvent> _extractionFilter = null;

        private Transform _grinderToolTransform;
        private Vector3 _newPosition;
        private Vector3 _newRotation;

        public void Init()
        {
            _grinderToolTransform = Object.Instantiate(
                _trepanationData.grinderGameObject,
                _trepanationData.grinderStartPosition, 
                Quaternion.Euler(_trepanationData.grinderRotation)).transform;
        }

        public void Run()
        {
            if (!_extractionFilter.IsEmpty())
            {
                _grinderToolTransform.position = Vector3.Lerp(
                    _grinderToolTransform.position, 
                    _trepanationData.grinderEndPosition, 
                    10 * Time.deltaTime);
                SoundManager.Instance.GrinderToolPlay(false);
                return;
            }
            
            // Debug.Log("GrinderTool System");
            
            foreach (var idx in _zombieFilter)
            {
                ref var zombieComponent = ref _zombieFilter.Get1(idx);
                
                var ray = _camera.ScreenPointToRay(
                    _camera.WorldToScreenPoint(zombieComponent.ZombiePointsUc.headPoint.position));
                if(!Physics.Raycast(ray, out var hit, 5)) return;

                if (Input.GetMouseButton(0))
                {
                    SoundManager.Instance.GrinderToolPlay(true);
                    MMVibrationManager.ContinuousHaptic(0.9f, 0.07f, 0.1f, HapticTypes.LightImpact, null, true, -1, true);

                    _newPosition = hit.point + _trepanationData.grinderUseShiftPosition;
                    _newRotation = _trepanationData.grinderUseRotation;
                    
                    var grinderRotation = _grinderToolTransform.localEulerAngles;
                    if (grinderRotation.x > 350 || grinderRotation.x < 1)
                    {
                        if (_zombieRotateFilter.IsEmpty())
                        {
                            _world.NewEntity().Get<ZombieRotateEvent>();
                        }
                    }
                }
                else
                {
                    SoundManager.Instance.GrinderToolPlay(false);
                    MMVibrationManager.StopContinuousHaptic();
                    
                    _newPosition = hit.point + _trepanationData.grinderShiftPosition;
                    _newRotation = _trepanationData.grinderRotation;

                    if (!_zombieRotateFilter.IsEmpty())
                    {
                        foreach (var idy in _zombieRotateFilter)
                        {
                            _zombieRotateFilter.GetEntity(idy).Destroy();
                        }
                    }
                }

                _grinderToolTransform.position = Vector3.Lerp(
                    _grinderToolTransform.position, 
                    _newPosition, 
                    10 * Time.deltaTime);
                _grinderToolTransform.rotation = Quaternion.Lerp(
                    _grinderToolTransform.rotation,
                    Quaternion.Euler(_newRotation),
                    10 * Time.deltaTime);
            }
        }
    }
}
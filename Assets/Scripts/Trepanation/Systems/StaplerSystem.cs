using System;
using Leopotam.Ecs;
using MoreMountains.NiceVibrations;
using Trepanation.Components;
using Trepanation.Data;
using Trepanation.UnityComponents;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Trepanation.Systems
{
    public class StaplerSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsWorld _world = null;
        private readonly Camera _camera = null;
        private readonly TrepanationData _trepanationData = null;
        private readonly EcsFilter<ZombieComponent> _zombieFilter = null;
        private readonly EcsFilter<FastenEvent> _fastenFilter = null;
        private readonly EcsFilter<CompletedEvent> _completedFilter = null;
        private readonly EcsFilter<ZombieRotateAngleEvent> _zombieRotateAngleFilter = null;

        private Transform _staplerTransform;
        private Transform _headPoint;
        private Vector3 _newPosition;
        private Vector3 _newRotation;
        private bool _animateStapler;

        public void Init()
        {
            _staplerTransform = Object.Instantiate(
                _trepanationData.staplerGameObject,
                _trepanationData.staplerStartPosition,
                Quaternion.Euler(_newRotation)).transform;
        }

        public void Run()
        {
            if (_fastenFilter.IsEmpty()) return;
            
            if (!_completedFilter.IsEmpty())
            {
                _newPosition = _trepanationData.staplerEndPosition;
            }
            
            _staplerTransform.position = Vector3.Lerp(
                _staplerTransform.position,
                _newPosition,
                10 * Time.deltaTime);
            _staplerTransform.rotation = Quaternion.Lerp(
                _staplerTransform.rotation,
                Quaternion.Euler(_newRotation),
                10 * Time.deltaTime);

            if (_animateStapler)
            {
                if (!CompletedPosition()) return;
                var staplerRotation = _staplerTransform.localEulerAngles;
                var stapleGameObject = Object.Instantiate(
                    _trepanationData.stapleGameObject, 
                    _staplerTransform.position + _trepanationData.stapleShiftPosition,
                    Quaternion.Euler(new Vector3(
                        staplerRotation.x + _trepanationData.stapleShiftRotation.x, 
                        staplerRotation.y + _trepanationData.stapleShiftRotation.y, 
                        staplerRotation.z + Random.Range(-_trepanationData.stapleShiftRotation.z,_trepanationData.stapleShiftRotation.z))));
                stapleGameObject.transform.SetParent(_headPoint);
                _world.NewEntity().Get<ZombieRotateAngleEvent>();
                SoundManager.Instance.PlayStapler();
                MMVibrationManager.Haptic(HapticTypes.HeavyImpact, false, true);
                _animateStapler = false;
                return;
            }

            foreach (var idx in _zombieFilter)
            {
                ref var zombieComponent = ref _zombieFilter.Get1(idx);
                if (_headPoint == null)
                    _headPoint = zombieComponent.ZombiePointsUc.headPoint;
                
                var ray = _camera.ScreenPointToRay(
                    _camera.WorldToScreenPoint(_headPoint.position));
                if (!Physics.Raycast(ray, out var hit, 5)) return;
                _newPosition = hit.point + _trepanationData.staplerShiftPosition;
                _newRotation = _trepanationData.staplerRotation;
                
                if(!CompletedPosition()) return; 
                if(!_zombieRotateAngleFilter.IsEmpty()) return;

                if (!Input.GetMouseButton(0)) return;
                _newPosition = hit.point + _trepanationData.staplerUseShiftPosition;
                _newRotation = _trepanationData.staplerUseRotation;
                _animateStapler = true;
            }
        }

        private bool CompletedPosition()
        {
            var position = _staplerTransform.position;
            return Math.Abs(position.x - _newPosition.x) < 0.01f &&
                   Math.Abs(position.y - _newPosition.y) < 0.01f &&
                   Math.Abs(position.z - _newPosition.z) < 0.01f;
        }
    }
}
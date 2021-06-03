using Leopotam.Ecs;
using Limb.Components;
using Limb.Data;
using UnityEngine;

namespace Limb.Systems
{
    public class MoveCameraSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly LimbData _limbData = null;
        private readonly Camera _camera = null;

        private readonly EcsFilter<HandSelectedEvent> _handSelectFilter = null;
        private readonly EcsFilter<CompletedEvent> _completedFilter = null;
        private readonly EcsFilter<DetachedEvent> _detachedFilter = null;

        // private Vector3 _startPosition;
        // private Vector3 _startRotation;
        private Vector3 _newPosition;
        private Vector3 _newRotation;

        private float _delay = 0.5f;

        public void Init()
        {
            var cameraTransform = _camera.transform;
            // _startPosition = cameraTransform.position;
            // _startRotation = cameraTransform.rotation.eulerAngles;
        }

        public void Run()
        {
            _delay -= Time.deltaTime;
            if(_delay > 0) return;
            
            var cameraTransform = _camera.transform;

            if (_detachedFilter.IsEmpty())
            {
                _newPosition = _limbData.cameraDetachPosition;
                _newRotation = _limbData.cameraDetachRotation;
            }
            else if (_handSelectFilter.IsEmpty())
            {                
                _newPosition = _limbData.cameraSelectPosition;
                _newRotation = _limbData.cameraSelectRotation;
            }
            else if (_completedFilter.IsEmpty())
            {
                _newPosition = _limbData.cameraScrewPosition;
                _newRotation = _limbData.cameraScrewRotation;
            }
            else
            {
                _newPosition = _limbData.cameraStartPosition;
                _newRotation = _limbData.cameraStartRotation;
            }

            cameraTransform.position = Vector3.Lerp(
                cameraTransform.position,
                _newPosition,
                _limbData.cameraSpeed * Time.deltaTime);
            cameraTransform.rotation = Quaternion.Lerp(
                cameraTransform.rotation,
                Quaternion.Euler(_newRotation),
                _limbData.cameraSpeed * Time.deltaTime);
        }
    }
}
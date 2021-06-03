using Leopotam.Ecs;
using Teeth.Components;
using Teeth.Data;
using UnityEngine;

namespace Teeth.Systems
{
    public class CameraSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly TeethData _teethData = null;
        private readonly Camera _camera = null;
        private readonly EcsFilter<ToothlessEvent> _toothlessFilter = null;
        private readonly EcsFilter<CompletedEvent> _completedFilter = null;

        private Transform _cameraTransform;

        private float _delay = 0.5f;

        public void Init()
        {
            _cameraTransform = _camera.transform;
        }

        public void Run()
        {
            _delay -= Time.deltaTime;
            if(_delay > 0) return;

            Vector3 newPosition;
            Vector3 newRotation;

            if (_toothlessFilter.IsEmpty())
            {
                newPosition = _teethData.cameraTeethPosition;
                newRotation = _teethData.cameraTeethRotation;
            }
            else if (_completedFilter.IsEmpty())
            {
                newPosition = _teethData.cameraInsertPosition;
                newRotation = _teethData.cameraInsertRotation;
            }
            else
            {
                newPosition = _teethData.cameraStartPosition;
                newRotation = _teethData.cameraStartRotation;
            }
            
            _cameraTransform.position = Vector3.Lerp(
                _cameraTransform.position,
                newPosition,
                _teethData.cameraSpeed * Time.deltaTime);
            _cameraTransform.localRotation = Quaternion.Lerp(
                _cameraTransform.localRotation,
                Quaternion.Euler(newRotation),
                _teethData.cameraSpeed * Time.deltaTime);
        }
    }
}
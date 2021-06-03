using Leopotam.Ecs;
using Trepanation.Components;
using Trepanation.Data;
using UnityEngine;

namespace Trepanation.Systems
{
    public class CameraMoveSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly Camera _camera = null;
        private readonly TrepanationData _trepanationData = null;
        private readonly EcsFilter<ExtractionEvent> _extractionFilter = null;
        private readonly EcsFilter<InsertEvent> _insertFilter = null;
        private readonly EcsFilter<FastenEvent> _fastenFilter = null;
        private readonly EcsFilter<CompletedEvent> _completedFilter = null;

        private Transform _cameraTransform;
        private Vector3 _newRotation;
        private Vector3 _newPosition;
        
        public void Init()
        {
            _cameraTransform = _camera.transform;

            _newPosition = _trepanationData.cameraStartPosition;
            _newRotation = _trepanationData.cameraStartRotation;
            
            _cameraTransform.position = _newPosition;
            _cameraTransform.rotation = Quaternion.Euler(_newRotation);
        }

        public void Run()
        {
            // Debug.Log("Camera System");

            if (_extractionFilter.IsEmpty())
            {
                _newPosition = _trepanationData.cameraTrepanationPosition;
                _newRotation = _trepanationData.cameraTrepanationRotation;
            }
            else if(_insertFilter.IsEmpty())
            {
                _newPosition = _trepanationData.cameraExtractionPosition;
                _newRotation = _trepanationData.cameraExtractionRotation;
            }
            else if(_fastenFilter.IsEmpty())
            {
                _newPosition = _trepanationData.cameraInsertPosition;
                _newRotation = _trepanationData.cameraInsertRotation;
            }
            else if(_completedFilter.IsEmpty())
            {
                _newPosition = _trepanationData.cameraTrepanationPosition;
                _newRotation = _trepanationData.cameraTrepanationRotation;
            }
            else
            {
                _newPosition = _trepanationData.cameraStartPosition;
                _newRotation = _trepanationData.cameraStartRotation;
            }

            _cameraTransform.position = Vector3.Lerp(
                _cameraTransform.position, 
                _newPosition, 
                _trepanationData.cameraSpeed * Time.deltaTime);
            _cameraTransform.rotation = Quaternion.Lerp(_cameraTransform.rotation,
                Quaternion.Euler(_newRotation), 
                _trepanationData.cameraSpeed * Time.deltaTime);
        }
    }
}
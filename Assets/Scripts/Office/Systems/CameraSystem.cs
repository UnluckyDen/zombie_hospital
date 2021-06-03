using System;
using Leopotam.Ecs;
using Office.Components;
using Office.Data;
using UnityEngine;

namespace Office.Systems
{
    public class CameraSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly Camera _camera = null;
        private readonly OfficeData _officeData = null;
        private readonly EcsFilter<CameraPositionEvent> _cameraPositionFilter = null;
        private readonly EcsFilter<ReadyToAnimateEvent> _readyToAnimateFilter = null;

        private Transform _cameraTransform;
        private bool _turnRight = true;
        private float _localEulerAngleY;
        
        public void Init()
        {
            _cameraTransform = _camera.transform;
            _cameraTransform.position = _officeData.cameraStartPosition;
            var localEulerAngles = _cameraTransform.localEulerAngles;
            _cameraTransform.rotation = Quaternion.Euler(new Vector3(_officeData.cameraStartAngle, localEulerAngles.y,
                localEulerAngles.z));
            _localEulerAngleY = localEulerAngles.y;
        }

        public void Run()
        {
            if (!_cameraPositionFilter.IsEmpty())
            {
                foreach (var idx in _cameraPositionFilter)
                {
                    ref var cameraPositionComponent = ref _cameraPositionFilter.Get1(idx);
                    MoveCameraTo(cameraPositionComponent.CameraPosition, cameraPositionComponent.CameraRotation);
                    break;
                }
                return;
            }
            
            if(_readyToAnimateFilter.IsEmpty()) CameraDefaultState();
        }

        private void CameraDefaultState()
        {
            _cameraTransform.position = Vector3.Lerp(
                _cameraTransform.position, 
                _officeData.cameraStartPosition, 
                _officeData.cameraSpeed * Time.deltaTime);

            if (_turnRight)
            {
                _localEulerAngleY += _officeData.cameraSpeed * Time.deltaTime;
                if (_localEulerAngleY > _officeData.maxAngle)
                    _turnRight = false;
            }
            else
            {
                _localEulerAngleY -= _officeData.cameraSpeed * Time.deltaTime;
                if (_localEulerAngleY < _officeData.minAngle)
                    _turnRight = true;
            }
            
            var localEulerAngles = _cameraTransform.localEulerAngles;
            var newRotation = Quaternion.Euler(new Vector3(_officeData.cameraStartAngle, _localEulerAngleY,
                localEulerAngles.z));
            _cameraTransform.rotation = Quaternion.Lerp(_cameraTransform.rotation, newRotation, _officeData.cameraSpeed * Time.deltaTime);
        }

        private void MoveCameraTo(Vector3 position, Vector3 rotation)
        {
            var cameraPosition = _cameraTransform.position;
            
            _cameraTransform.position = Vector3.Lerp(
                cameraPosition, 
                position,
                _officeData.cameraSpeed * Time.deltaTime);
            _cameraTransform.rotation = Quaternion.Lerp(
                _cameraTransform.rotation, 
                Quaternion.Euler(rotation),
                _officeData.cameraSpeed * Time.deltaTime);

            if (_cameraTransform.localEulerAngles.y > 300)
                _localEulerAngleY = 0;
            else _localEulerAngleY = Mathf.Clamp(_cameraTransform.localEulerAngles.y, _officeData.minAngle, _officeData.maxAngle);

            if (Math.Abs(cameraPosition.x - position.x) > 0.01f ||
                Math.Abs(cameraPosition.y - position.y) > 0.01f ||
                Math.Abs(cameraPosition.z - position.z) > 0.01f)
                return;
            foreach (var idx in _cameraPositionFilter) _cameraPositionFilter.GetEntity(idx).Destroy();
        }
    }
}
using Leopotam.Ecs;
using Limb.Components;
using Limb.Data;
using Limb.UnityComponents;
using UnityEngine;

namespace Limb.Systems
{
    public class DetachSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly LimbData _limbData = null;
        private readonly Camera _camera = null;
        private readonly EcsFilter<DetachedEvent> _detachEvent = null;
        private readonly EcsWorld _world = null;

        private const float HandSeparateTime = 0.5f;

        private Collider _collider;
        private SpringHandUc _springHandUc;
        private float _yPosition;
        private float _currentHandSeparateTime;

        public void Init()
        {
            _currentHandSeparateTime = HandSeparateTime;
        }

        public void Run()
        {
            if(!_detachEvent.IsEmpty()) return;
            
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit, 10)) return;
            
            if (Input.GetMouseButtonDown(0))
            {
                _springHandUc = hit.collider.GetComponent<SpringHandUc>();
                if(_springHandUc != null)
                {
                    _collider = hit.collider;
                    _yPosition = hit.point.y;
                }
            }
            
            if(_collider == null) return;

            if (Input.GetMouseButton(0))
            {
                var normalized = (new Vector3(hit.point.x, _yPosition, hit.point.z) - _collider.transform.position).normalized;
                _springHandUc.AddForce(normalized * 3000);
                // Debug.Log(_springHandUc.GetSpringDistance());
                if (_springHandUc.GetSpringDistance() > 1.4f)
                {
                    _currentHandSeparateTime -= Time.deltaTime; 
                    if (_currentHandSeparateTime < 0)
                    {
                        SoundManager.Instance.PlayDetach();
                        SoundManager.Instance.PlayZombieScream();
                        _springHandUc.SeparateHand();
                        SeparateHand();
                    }
                }
                else
                {
                    _currentHandSeparateTime = HandSeparateTime;
                }
            }

            if (!Input.GetMouseButtonUp(0)) return;
            _collider = null;
            _springHandUc = null;
        }

        private void SeparateHand()
        {
            _world.NewEntity().Get<DetachedEvent>();
        }
    }
}
using Leopotam.Ecs;
using Trepanation.Components;
using UnityEngine;

namespace Trepanation.Systems
{
    public class BrainSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world = null;
        private readonly Camera _camera = null;
        private readonly EcsFilter<ExtractionEvent> _extractionFilter = null;
        private readonly EcsFilter<ZombieComponent> _zombieFilter = null;
        private readonly EcsFilter<InsertEvent> _insertFilter = null;

        private Rigidbody _hitRigidbody;

        public void Run()
        {
            if (_extractionFilter.IsEmpty()) return;
            if(!_insertFilter.IsEmpty()) return;
            
            // Debug.Log("Brain System");
            
            foreach (var idx in _zombieFilter)
            {
                ref var zombieComponent = ref _zombieFilter.Get1(idx);
                if (zombieComponent.ZombiePointsUc.brainTriggerUc.hasBrain) continue;
                _world.NewEntity().Get<InsertEvent>();
                return;
            }
            
            var ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (_hitRigidbody != null) _hitRigidbody.velocity = 10 * (ray.origin + ray.direction - _hitRigidbody.position);

            if (Input.GetMouseButtonUp(0) && _hitRigidbody != null)
            {
                _hitRigidbody.freezeRotation = false;
                _hitRigidbody = null;
            }

            if (!Input.GetMouseButtonDown(0)) return;
            if (!Physics.Raycast(ray, out var hit, 3f)) return;
            if (!hit.collider.CompareTag("Brain")) return;
            _hitRigidbody = hit.collider.GetComponent<Rigidbody>();
            _hitRigidbody.freezeRotation = true;
        }
    }
}
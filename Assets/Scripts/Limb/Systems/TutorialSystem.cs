using Leopotam.Ecs;
using Limb.Components;
using Limb.UnityComponents;
using UnityEngine;

namespace Limb.Systems
{
    public class TutorialSystem : IEcsRunSystem
    {
        private readonly Camera _camera = null;
        private readonly EmitterUc _emitterUc = null;

        private readonly EcsFilter<ZombieComponent> _zombieFilter = null;
        private readonly EcsFilter<HandSelectedEvent> _handSelectFilter = null;
        private readonly EcsFilter<CompletedEvent> _completedFilter = null;
        private readonly EcsFilter<DetachedEvent> _detachedFilter = null;
        private readonly EcsFilter<HandComponent> _handComponent = null;

        public void Run()
        {
            if (_detachedFilter.IsEmpty())
            {
                foreach (var idx in _zombieFilter)
                {
                    ref var zombieComponent = ref _zombieFilter.Get1(idx);
                    var handPointPosition = zombieComponent.ZombiePointsUc.handPoint.position;
                    Vector2 startPosition = _camera.WorldToScreenPoint(handPointPosition);
                    Vector2 endPosition = _camera.WorldToScreenPoint(handPointPosition + Vector3.right);
                    
                    _emitterUc.SetCursorSwipePositions(startPosition, endPosition);
                }
            }
            else if (_handSelectFilter.IsEmpty())
            {
                foreach (var idx in _zombieFilter)
                {
                    ref var zombieComponent = ref _zombieFilter.Get1(idx);
                    Vector2 startPosition = _camera.WorldToScreenPoint(zombieComponent.ZombiePointsUc.handPoint.position);

                    foreach (var idy in _handComponent)
                    {
                        ref var handComponent = ref _handComponent.Get1(idy);
                        if (!handComponent.Attached) continue;
                        _emitterUc.Hide();
                        return;
                    }

                    foreach (var idy in _handComponent)
                    {
                        ref var handComponent = ref _handComponent.Get1(idy);
                        Vector2 endPosition = _camera.WorldToScreenPoint(handComponent.Transform.position);
                        
                        _emitterUc.SetCursorSwipePositions(endPosition, startPosition);
                        return;
                    }
                }
            }
            else if (_completedFilter.IsEmpty())
            {
                _emitterUc.Show();
                _emitterUc.SetCursorPosition(new Vector2(500, 500), EmitterUc.AnimationType.Hold);
            }
            else
            {
                _emitterUc.Hide();
            }
        }
    }
}
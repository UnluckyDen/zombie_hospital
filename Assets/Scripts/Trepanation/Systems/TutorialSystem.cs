using Leopotam.Ecs;
using Trepanation.Components;
using Trepanation.UnityComponents;
using UnityEngine;

namespace Trepanation.Systems
{
    public class TutorialSystem : IEcsRunSystem
    {
        private readonly Camera _camera = null;
        private readonly EmitterUc _emitterUc = null;
        private readonly EcsFilter<ExtractionEvent> _extractionFilter = null;
        private readonly EcsFilter<InsertEvent> _insertFilter = null;
        private readonly EcsFilter<FastenEvent> _fastenFilter = null;
        private readonly EcsFilter<CompletedEvent> _completedFilter = null;
        private readonly EcsFilter<ZombieComponent> _zombieFilter = null;
        private readonly EcsFilter<ItemComponent> _itemFilter = null;
        
        public void Run()
        {
            if (_extractionFilter.IsEmpty())
            {
                _emitterUc.SetCursorPosition(new Vector2(500, 500), EmitterUc.AnimationType.Hold);
            }
            else if(_insertFilter.IsEmpty())
            {
                foreach (var idx in _zombieFilter)
                {
                    ref var zombieComponent = ref _zombieFilter.Get1(idx);
                    
                    _emitterUc.SetCursorPosition(_camera.WorldToScreenPoint(zombieComponent.ZombiePointsUc.headInsertPoint.position),
                        EmitterUc.AnimationType.Click);
                }
            }
            else if(_fastenFilter.IsEmpty())
            {
                foreach (var idx in _itemFilter)
                {
                    ref var itemComponent = ref _itemFilter.Get1(idx);
                    if (!itemComponent.Attached) continue;
                    _emitterUc.Hide();
                    return;
                }
                
                foreach (var idx in _itemFilter)
                {
                    ref var itemComponent = ref _itemFilter.Get1(idx);

                    foreach (var idy in _zombieFilter)
                    {
                        ref var zombieComponent = ref _zombieFilter.Get1(idy);

                        _emitterUc.Show();
                        _emitterUc.SetCursorSwipePositions(_camera.WorldToScreenPoint(itemComponent.Transform.position), 
                            _camera.WorldToScreenPoint(zombieComponent.ZombiePointsUc.headInsertPoint.position));
                    }
                    return;
                }
                
            }
            else if(_completedFilter.IsEmpty())
            {
                _emitterUc.Show();
                _emitterUc.SetCursorPosition(new Vector2(500, 500), EmitterUc.AnimationType.Click);
            }
            else
            {
                _emitterUc.Hide();
            }
        }
    }
}
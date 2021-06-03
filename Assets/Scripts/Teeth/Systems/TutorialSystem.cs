using Leopotam.Ecs;
 using Teeth.Components;
 using Teeth.UnityComponents;
 using UnityEngine;

namespace Teeth.Systems
 {
     public class TutorialSystem : IEcsRunSystem
     {
         private readonly Camera _camera = null;
         private readonly EmitterUc _emitterUc = null;
         private readonly EcsFilter<ToothlessEvent> _toothlessFilter = null;
         // private readonly EcsFilter<CompletedEvent> _completedFilter = null;
         private readonly EcsFilter<ZombieComponent> _zombieFilter = null;
         private readonly EcsFilter<ToothComponent> _toothFilter = null;
         
         public void Run()
         {
             if (_toothlessFilter.IsEmpty())
             {
                 var position = Vector2.zero;
                 foreach (var idx in _zombieFilter)
                 {
                     ref var zombieComponent = ref _zombieFilter.Get1(idx);
                     position = _camera.WorldToScreenPoint(zombieComponent.ZombiePointsUc.teethPoint.position);
                     break;
                 }
                 _emitterUc.SetCursorPosition(position);
             }
             else
             {
                 var startPosition = Vector2.zero;
                 var endPosition = Vector2.zero;
                 foreach (var idx in _toothFilter)
                 {
                     ref var toothComponent = ref _toothFilter.Get1(idx);
                     if (!toothComponent.InMouth) continue;
                     _emitterUc.Hide();
                     return;
                 }
                 foreach (var idx in _toothFilter)
                 {
                     ref var toothComponent = ref _toothFilter.Get1(idx);
                     startPosition = _camera.WorldToScreenPoint(toothComponent.Transform.position);
                     break;
                 }
                 foreach (var idx in _zombieFilter)
                 {
                     ref var zombieComponent = ref _zombieFilter.Get1(idx);
                     endPosition = _camera.WorldToScreenPoint(zombieComponent.ZombiePointsUc.teethPoint.position);
                     break;
                 }
                 _emitterUc.SetCursorSwipePositions(startPosition, endPosition);
             }
         }
     }
 }
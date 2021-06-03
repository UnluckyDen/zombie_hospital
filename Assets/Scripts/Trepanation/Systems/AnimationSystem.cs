using Leopotam.Ecs;
using Trepanation.Components;
using UnityEngine;

namespace Trepanation.Systems
{
    public class AnimationSystem : IEcsRunSystem
    {
        private readonly EcsFilter<ZombieComponent> _zombieFilter = null;

        private readonly EcsFilter<ExtractionEvent> _extractionFilter = null;
        private readonly EcsFilter<FastenEvent> _fastenFilter = null;
        private readonly EcsFilter<InsertEvent> _insertFilter = null;
        private readonly EcsFilter<ZombieRotateEvent> _rotateFilter = null;
        private readonly EcsFilter<ZombieRotateAngleEvent> _zombieRotateAngleFilter = null;
        private readonly EcsFilter<CompletedEvent> _complitedFilter = null;

        private bool firstRotate;

        public void Run()
        {
            Debug.Log(_extractionFilter.IsEmpty());//срезали верхушку
            Debug.Log(_fastenFilter.IsEmpty());//объект выбран
            Debug.Log(_insertFilter.IsEmpty());//мозг выпал
            Debug.Log(_rotateFilter.IsEmpty());//первый поворот
            Debug.Log(_zombieRotateAngleFilter.IsEmpty());//второй повотот
            
            foreach (var idx in _zombieFilter)
            {
                ref var zombieComponent = ref _zombieFilter.Get1(idx);
                var animator = zombieComponent.ZombiePointsUc.animator;
                
                
                if (!_rotateFilter.IsEmpty() && !firstRotate)
                {
                    animator.SetTrigger("Auch");
                    firstRotate = true;
                }
                
                if (!_extractionFilter.IsEmpty())
                {
                        animator.SetTrigger("Surprised");
                }

                if (!_zombieRotateAngleFilter.IsEmpty())
                {
                    animator.SetTrigger("Auch");
                    animator.ResetTrigger("Surprised");
                }

                if (!_complitedFilter.IsEmpty())
                {
                    animator.SetTrigger("Happy");
                }
            }
        }
    }
}
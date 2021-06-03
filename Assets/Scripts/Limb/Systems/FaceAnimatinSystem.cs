using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.Ecs;
using Limb.Components;
using Limb.UnityComponents;

namespace Limb.Systems
{
    public class FaceAnimatinSystem : IEcsRunSystem
    {
        private readonly EcsFilter<ZombieComponent> _zombieFilter = null;
        
        private readonly EcsFilter<DetachedEvent> _detachedFilter = null;
        private readonly EcsFilter<HandSelectedEvent> _handSelectedFilter = null;

        private bool _animationState;
        private bool _isHandDetouched;
        private bool _isHandSelected;

        public void Run()
        {
            _animationState = !_detachedFilter.IsEmpty();

            foreach (var idx in _zombieFilter)
            {
                ref var zombieComponent = ref _zombieFilter.Get1(idx);
                var animator = zombieComponent.ZombiePointsUc.animator;

                if (_animationState && !_isHandDetouched)
                {
                    animator.SetTrigger("Screeam");
                    _isHandDetouched = true;
                }
                if (!_handSelectedFilter.IsEmpty()) 
                {
                    animator.SetTrigger("Auch");
                    //skinnedMeshRenderer.SetBlendShapeWeight(2, 100 - _mouthBlend); 
                }
                

               
            }
            
            

        }
    }
}
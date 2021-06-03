using Leopotam.Ecs;
using Teeth.Components;
using UnityEngine;

namespace Teeth.Systems
{
    public class ZombieFaceAnimationSystem : IEcsRunSystem
    {
        private readonly EcsFilter<ZombieComponent> _zombieFilter = null;
        private readonly EcsFilter<ToothlessEvent> _toothlessFilter = null;

        private const float AnimationSpeed = 150;
        private float _mouthBlend = 0;

        private bool _animationState;

        public void Run()
        {
            
        }
    }
}
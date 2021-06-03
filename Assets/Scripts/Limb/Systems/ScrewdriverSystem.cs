using System;
using Leopotam.Ecs;
using Limb.Components;
using Limb.Data;
using Limb.UnityComponents;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Limb.Systems
{
    public class ScrewdriverSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsWorld _world = null;
        private readonly LimbData _limbData = null;
        private readonly EcsFilter<ScrewdriverComponent> _screwdriverFilter = null;
        private readonly EcsFilter<ZombieComponent> _zombieFilter = null;
        private readonly EcsFilter<HandSelectedEvent> _handSelectedFilter = null;
        private readonly EcsFilter<CompletedEvent> _completedFilter = null;
        
        private const float Tolerance = 0.01f;

        public void Init()
        {
            var gameObject = Object.Instantiate(_limbData.screwdriverPrefab, _limbData.screwdriverPosition,
                Quaternion.identity);

            var entity = _world.NewEntity();
            entity.Replace(new ScrewdriverComponent
            {
                Transform = gameObject.transform,
                ScrewdriverUc = gameObject.GetComponent<ScrewdriverUc>()
            });
        }

        public void Run()
        {
            if (_handSelectedFilter.IsEmpty()) return;

            foreach (var idx in _screwdriverFilter)
            {
                ref var screwdriverComponent = ref _screwdriverFilter.Get1(idx);
                var screwdriverTransform = screwdriverComponent.Transform;

                if (!_completedFilter.IsEmpty())
                {
                    if(screwdriverComponent.StartPosition == null)
                        throw new Exception("Start position = null");
                    screwdriverTransform.position = Vector3.Lerp(
                        screwdriverComponent.Transform.position,
                        _limbData.screwdriverPosition,
                        10f * Time.deltaTime);
                    return;
                }
                
                foreach (var idy in _zombieFilter)
                {
                    ref var zombieComponent = ref _zombieFilter.Get1(idy);
                    var zombiePointUc = zombieComponent.ZombiePointsUc;
                    
                    var screwdriverPosition = screwdriverComponent.Transform.position;
                    var zombiePointPosition = zombiePointUc.screwdriverPoint.position;
                    
                    screwdriverTransform.position = Vector3.Lerp(
                        screwdriverPosition,
                        zombiePointPosition,
                        10f * Time.deltaTime);
                    screwdriverTransform.rotation = zombiePointUc.screwdriverPoint.rotation;

                    if (!screwdriverComponent.Ready && 
                        Math.Abs(screwdriverPosition.x - zombiePointPosition.x) < Tolerance &&
                        Math.Abs(screwdriverPosition.y - zombiePointPosition.y) < Tolerance &&
                        Math.Abs(screwdriverPosition.z - zombiePointPosition.z) < Tolerance)
                    {
                        screwdriverComponent.Ready = true;
                    }
                }
            }
        }
    }
}
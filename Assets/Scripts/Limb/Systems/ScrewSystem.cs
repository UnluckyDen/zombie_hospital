using Leopotam.Ecs;
using Limb.Components;
using Limb.Data;
using Limb.UnityComponents;
using UnityEngine;
using ZombieComponent = Limb.Components.ZombieComponent;

namespace Limb.Systems
{
    public class ScrewSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsWorld _world = null;
        private readonly LimbData _limbData = null;
        private readonly EcsFilter<ScrewComponent> _screwFilter = null;
        private readonly EcsFilter<ZombieComponent> _zombieFilter = null;
        private readonly EcsFilter<HandSelectedEvent> _handSelectedFilter = null;

        public void Init()
        {
            var gameObject = Object.Instantiate(_limbData.screwPrefab, _limbData.screwPosition, Quaternion.identity);

            var entity = _world.NewEntity();
            entity.Replace(new ScrewComponent
            {
                Transform = gameObject.transform,
                ScrewUc = gameObject.GetComponent<ScrewUc>(),
                ReturnToOriginal = true
            });
        }

        public void Run()
        {
            if (_handSelectedFilter.IsEmpty()) return;

            foreach (var idx in _screwFilter)
            {
                ref var screwComponent = ref _screwFilter.Get1(idx);
                if (!screwComponent.ReturnToOriginal) continue;
                var screwTransform = screwComponent.Transform;

                foreach (var idy in _zombieFilter)
                {
                    ref var zombieComponent = ref _zombieFilter.Get1(idy);
                    var zombiePointUc = zombieComponent.ZombiePointsUc;
                    screwTransform.position = Vector3.Lerp(
                        screwComponent.Transform.position,
                        zombiePointUc.screwPoint.position,
                        10 * Time.deltaTime);
                    screwTransform.rotation = zombiePointUc.screwPoint.rotation;
                }
            }
        }
    }
}
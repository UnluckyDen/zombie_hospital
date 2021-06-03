using Leopotam.Ecs;
using Limb.Components;
using Limb.UnityComponents;
using UnityEngine;

namespace Limb.Systems
{
    public class VolumeSystem : IEcsRunSystem
    {
        private readonly Camera _camera = null;
        private readonly EcsFilter<ZombieComponent> _zombieFilter = null;

        public void Run()
        {
            foreach (var idy in _zombieFilter)
            {
                ref var zombieComponent = ref _zombieFilter.Get1(idy);
                var distance = Vector3.Distance(_camera.transform.position,
                    zombieComponent.ZombiePointsUc.transform.position) * 1.5f;
                SoundManager.Instance.SetBreathVolume(1 / distance);
            }
        }
    }
}
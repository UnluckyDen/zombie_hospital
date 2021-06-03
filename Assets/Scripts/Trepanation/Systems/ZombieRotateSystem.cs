using Leopotam.Ecs;
using Trepanation.Components;
using Trepanation.Data;
using UnityEngine;

namespace Trepanation.Systems
{
    public class ZombieRotateSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world = null;
        private readonly TrepanationData _trepanationData = null;
        private readonly EcsFilter<ZombieComponent> _zombieFilter = null;
        private readonly EcsFilter<ZombieRotateEvent> _zombieRotateFilter = null;
        private readonly EcsFilter<ExtractionEvent> _extractionFilter = null;

        private float _rotateAngle;
        private float _progress;

        public void Run()
        {
            if(_zombieRotateFilter.IsEmpty()) return;
            if(!_extractionFilter.IsEmpty()) return;
            
            // Debug.Log("Zombie Rotate System");
            
            foreach (var idx in _zombieFilter)
            {
                ref var zombieComponent = ref _zombieFilter.Get1(idx);
                
                zombieComponent.Transform.Rotate(Vector3.up * _trepanationData.zombieRotateSpeed * Time.deltaTime);

                _rotateAngle += _trepanationData.zombieRotateSpeed * Time.deltaTime;
                _progress = _rotateAngle / 360;

                if (_progress > 1) _world.NewEntity().Get<ExtractionEvent>();
            }
        }
    }
}
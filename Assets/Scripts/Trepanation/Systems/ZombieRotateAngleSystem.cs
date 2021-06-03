using Leopotam.Ecs;
using Trepanation.Components;
using Trepanation.Data;
using UnityEngine;

namespace Trepanation.Systems
{
    public class ZombieRotateAngleSystem : IEcsRunSystem
    {
        private readonly TrepanationData _trepanationData = null;
        private readonly EcsWorld _world = null;
        private readonly EcsFilter<ZombieComponent> _zombieFilter = null;
        private readonly EcsFilter<ZombieRotateAngleEvent> _zombieRotateAngleFilter = null;
        private readonly EcsFilter<CompletedEvent> _completedFilter = null;
        
        private float _rotateAngle;
        // private float _progress;
        private int _staplesCount = 1;
        
        public void Run()
        {
            if(_zombieRotateAngleFilter.IsEmpty()) return;
            if(!_completedFilter.IsEmpty()) return;
            
            foreach (var idx in _zombieFilter)
            {
                ref var zombieComponent = ref _zombieFilter.Get1(idx);
                
                zombieComponent.Transform.Rotate(Vector3.up * _trepanationData.zombieRotateSpeed * Time.deltaTime);

                _rotateAngle += _trepanationData.zombieRotateSpeed * Time.deltaTime;
                if (_rotateAngle / 360 > 1)
                {
                    _world.NewEntity().Get<CompletedEvent>();
                }
                if (!(_rotateAngle > 360 / _trepanationData.staplesCount * _staplesCount)) continue;
                foreach (var idy in _zombieRotateAngleFilter)
                {
                    _zombieRotateAngleFilter.GetEntity(idy).Destroy();
                }
                _staplesCount++;
            }
        }
    }
}
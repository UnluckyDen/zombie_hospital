using Leopotam.Ecs;
using Limb.Components;
using Limb.Data;
using UnityEngine;

namespace Limb.Systems
{
    public class MoveTraySystem : IEcsRunSystem
    {
        private readonly LimbData _limbData = null;
        private readonly EcsFilter<TrayComponent> _trayFilter = null;
        private readonly EcsFilter<HandSelectedEvent> _handSelectedFilter = null;
        private readonly EcsFilter<DetachedEvent> _detachedEvent = null;
        
        public void Run()
        {
            if(_detachedEvent.IsEmpty()) return;
            
            foreach (var idx in _trayFilter)
            {
                ref var trayComponent = ref _trayFilter.Get1(idx);
                var trayPosition = trayComponent.Transform.position;

                trayComponent.Transform.position = Vector3.Lerp(
                    trayPosition,
                    _handSelectedFilter.IsEmpty()
                        ? _limbData.trayPosition
                        : _limbData.trayStartPosition,
                    _limbData.traySpeed * Time.deltaTime);
            }
        }
    }
}
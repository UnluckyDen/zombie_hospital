using Leopotam.Ecs;
using Trepanation.Components;
using Trepanation.Data;
using Trepanation.UnityComponents;
using UnityComponents;
using UnityEngine;

namespace Trepanation.Systems
{
    public class TraySystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly TrepanationData _trepanationData = null;
        private readonly EcsWorld _world = null;
        private readonly EcsFilter<InsertEvent> _insertFilter = null;
        private readonly EcsFilter<TrayComponent> _trayFilter = null;
        private readonly EcsFilter<FastenEvent> _fastenFilter = null;
        
        public void Init()
        {
            var gameObject = Object.Instantiate(
                _trepanationData.trayGameObject,
                _trepanationData.trayStartPosition,
                Quaternion.Euler(_trepanationData.trayRotation));
            var trayPoints = gameObject.GetComponent<TrayPointsUc>();

            var entity = _world.NewEntity();
            entity.Replace(new TrayComponent
            {
                Transform = gameObject.transform,
                TrayPointsUc = trayPoints
            });

            var rndIndexes = Extension.Random.GetUniqueIntArray(
                0,
                _trepanationData.trayItems.Count,
                trayPoints.spawnPoints.Count);
            var rndIndex = 0;
            trayPoints.spawnPoints.ForEach(spawnPoint =>
            {
                var item = Object.Instantiate(_trepanationData.trayItems[rndIndexes[rndIndex++]],
                    spawnPoint.position, spawnPoint.rotation);
                item.AddComponent<ItemUc>();
                
                var itemEntity = _world.NewEntity();
                itemEntity.Replace(new ItemComponent
                {
                    Transform = item.transform,
                    Collider = item.GetComponent<BoxCollider>(),
                    CurrentParentTransform = spawnPoint,
                    StartParentTransform = spawnPoint,
                    ToParentTransform = true,
                    Speed = 1000
                });
            });
        }

        public void Run()
        {
            if (_insertFilter.IsEmpty()) return;
            
            // Debug.Log("Tray system");

            foreach (var idx in _trayFilter)
            {
                ref var trayComponent = ref _trayFilter.Get1(idx);

                trayComponent.Transform.position = Vector3.Lerp(
                    trayComponent.Transform.position, 
                    _fastenFilter.IsEmpty() 
                        ? _trepanationData.trayInsertPosition 
                        : _trepanationData.trayStartPosition, 
                    5 * Time.deltaTime);
            }
        }
    }
}
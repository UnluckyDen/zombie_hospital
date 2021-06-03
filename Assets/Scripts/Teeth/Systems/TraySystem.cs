using Leopotam.Ecs;
using Teeth.Components;
using Teeth.Data;
using Teeth.UnityComponents;
using UnityComponents;
using UnityEngine;

namespace Teeth.Systems
{
    public class TraySystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly TeethData _teethData = null;
        private readonly EcsWorld _world = null;
        private readonly EcsFilter<ToothlessEvent> _toothlessFilter = null;
        private readonly EcsFilter<CompletedEvent> _completedFilter = null;

        private readonly EcsFilter<TrayComponent> _trayFilter = null;

        public void Init()
        {
            var gameObject =
                Object.Instantiate(_teethData.trayPrefab, _teethData.trayStartPosition, Quaternion.identity);
            var trayPoints = gameObject.GetComponent<TrayPointsUc>();

            var entity = _world.NewEntity();
            entity.Replace(new TrayComponent
            {
                Transform = gameObject.transform,
                TrayPointsUc = trayPoints
            });

            var randomTeethIndex = Extension.Random.GetUniqueIntArray(0, _teethData.teeth.Count, 3);
            var toothIndex = 0;

            trayPoints.spawnPoints.ForEach(trayPoint =>
            {
                var toothTransform = Object.Instantiate(_teethData.teeth[randomTeethIndex[toothIndex]],
                    trayPoint.position, trayPoint.rotation);
                toothTransform.gameObject.AddComponent<ToothUc>();

                var toothEntity = _world.NewEntity();
                toothEntity.Replace(new ToothComponent
                {
                    ToothPrefab = _teethData.teeth[randomTeethIndex[toothIndex]].gameObject,
                    Transform = toothTransform,
                    Collider = toothTransform.GetComponent<BoxCollider>(),
                    CurrentParentTransform = trayPoint,
                    StartParentTransform = trayPoint,
                    ToParentTransform = true,
                    Speed = 1000
                });
                toothIndex++;
            });
        }

        public void Run()
        {
            if (_toothlessFilter.IsEmpty()) return;

            foreach (var idx in _trayFilter)
            {
                ref var trayComponent = ref _trayFilter.Get1(idx);
                var trayTransform = trayComponent.Transform;

                var newPosition = _completedFilter.IsEmpty() 
                    ? _teethData.trayPosition 
                    : _teethData.trayStartPosition;
                
                trayTransform.position = Vector3.Lerp(
                    trayTransform.position,
                    newPosition,
                    Time.deltaTime * 10);
            }
        }
    }
}
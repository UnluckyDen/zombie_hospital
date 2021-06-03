using Data;
using Leopotam.Ecs;
using Limb.Components;
using Limb.Data;
using Limb.UnityComponents;
using UnityComponents;
using UnityEngine;

namespace Limb.Systems
{
    public class CreateTraySystem : IEcsInitSystem
    {
        private readonly ZombieData _zombieData = null;
        private readonly EcsWorld _world = null;
        private readonly LimbData _limbData = null;

        public void Init()
        {
            var trayGameObject = Object.Instantiate(_limbData.tray, _limbData.trayStartPosition, Quaternion.identity);
            var trayTransform = trayGameObject.transform;
            var trayPointsUc = trayGameObject.GetComponent<TrayPointsUc>();

            var entity = _world.NewEntity();
            entity.Replace(new TrayComponent
            {
                Transform = trayTransform
                //TrayPointsUc = trayPointsUc
            });

            var randomIndexes = Extension.Random.GetUniqueIntArray(
                0,
                //_limbData.handPrefabs.Count,
                _zombieData.zombieSettings[ZombieManager.Instance.zombieIndex].trayHandPrefabs.Count,
                trayPointsUc.spawnPoints.Count);
            var index = 0;

            trayPointsUc.spawnPoints.ForEach(spawnPoint =>
            {
                var hand = Object.Instantiate(
                    _zombieData.zombieSettings[ZombieManager.Instance.zombieIndex].trayHandPrefabs[randomIndexes[index++]],
                    spawnPoint.position,
                    spawnPoint.rotation);
                hand.AddComponent<HandUc>();

                var handEntity = _world.NewEntity();
                handEntity.Replace(new HandComponent
                {
                    Transform = hand.transform,
                    Collider = hand.GetComponent<BoxCollider>(),
                    CurrentParentTransform = spawnPoint,
                    StartParentTransform = spawnPoint,
                    ToParentTransform = true,
                    Speed = 1000,
                    HandIndex = hand.GetComponent<HandIndexUc>().handId
                });
            });
        }
    }
}
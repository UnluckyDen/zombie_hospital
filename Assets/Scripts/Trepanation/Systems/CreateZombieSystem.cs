using Data;
using Leopotam.Ecs;
using Trepanation.Components;
using Trepanation.Data;
using Trepanation.UnityComponents;
using UnityComponents;
using UnityEngine;

namespace Trepanation.Systems
{
    public class CreateZombieSystem : IEcsInitSystem
    {
        private readonly ZombieData _zombieData = null;
        private readonly TrepanationData _trepanationData = null;
        private readonly EcsWorld _world = null;
        
        private GameObject _toothTop;
        private Transform _teethPointTop;
        private GameObject _toothBottom;
        private Transform _teethPointBottom;


        public void Init()
        {
            AnalyticsManager.Instance.LevelStart(3);
            
            var zombiePrefab = _zombieData.zombieSettings[ZombieManager.Instance.zombieIndex].zombieTrepanationPrefab;
            var zombieGameObject = Object.Instantiate(
                zombiePrefab, 
                _trepanationData.zombiePosition,
                Quaternion.Euler(_trepanationData.zombieRotation));
            var zombiePoints = zombieGameObject.GetComponent<ZombiePointsUc>();
            var toothGameObjectTop = ZombieManager.Instance.zombieTooth.IsEmpty() ? ZombieManager.Instance.zombieTooth.teethTop : _trepanationData.teethPrefabTop;
            _toothTop = Object.Instantiate(toothGameObjectTop,
                zombiePoints.teethPointTop.position,
                zombiePoints.teethPointTop.rotation);
            _teethPointTop = zombiePoints.teethPointTop;
            _toothTop.transform.SetParent(_teethPointTop);
            
            var toothGameObjectBottom = ZombieManager.Instance.zombieTooth.IsEmpty() ? ZombieManager.Instance.zombieTooth.teethBottom : _trepanationData.teethPrefabBottom;
            _toothBottom = Object.Instantiate(toothGameObjectBottom,
                zombiePoints.teethPointBottom.position,
                zombiePoints.teethPointBottom.rotation);
            _teethPointBottom = zombiePoints.teethPointBottom;
            _toothBottom.transform.SetParent(_teethPointBottom);

            var hand = ZombieManager.Instance.GetHand();
            var handTransform =
                Object.Instantiate(hand, zombiePoints.handPoint.position, zombiePoints.handPoint.rotation).transform;
            handTransform.SetParent(zombiePoints.handPoint);
            handTransform.localPosition = Vector3.zero;
            handTransform.localScale = Vector3.one;

            var entity = _world.NewEntity();
            entity.Replace(new ZombieComponent
            {
                Transform = zombieGameObject.transform,
                ZombiePointsUc = zombiePoints
            });
        }
    }
}
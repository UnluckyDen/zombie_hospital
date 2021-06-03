using Data;
using Leopotam.Ecs;
using Limb.Components;
using Limb.Data;
using Limb.UnityComponents;
using UnityComponents;
using UnityEngine;
using ZombiePointsUc = Limb.UnityComponents.ZombiePointsUc;

namespace Limb.Systems
{
    public class CreateZombieSystem : IEcsInitSystem
    {
        private readonly ZombieData _zombieData = null;
        private readonly EcsWorld _world = null;
        private readonly LimbData _limbData = null;

        private GameObject _toothTop;
        private GameObject _toothBottom;
        private Transform _teethPointTop;
        private Transform _teethPointBottom;

        public void Init()
        {
            AnalyticsManager.Instance.LevelStart(2);
            
            var zombiePrefab = _zombieData.zombieSettings[ZombieManager.Instance.zombieIndex].zombieLimbPrefab; 
            var gameObject = Object.Instantiate(
                zombiePrefab,
                _limbData.zombiePosition,
                Quaternion.Euler(_limbData.zombieRotation));
            var zombiePoints = gameObject.GetComponent<ZombiePointsUc>();
            
            var toothGameObjectTop = ZombieManager.Instance.zombieTooth.IsEmpty() ? ZombieManager.Instance.zombieTooth.teethTop : _limbData.teethTop;
            _toothTop = Object.Instantiate(toothGameObjectTop,
                zombiePoints.teethPointTop.position,
                zombiePoints.teethPointTop.rotation);
            _teethPointTop = zombiePoints.teethPointTop;
            _toothTop.transform.SetParent(_teethPointTop);
            
            var toothGameObjectBottom = ZombieManager.Instance.zombieTooth.IsEmpty() ? ZombieManager.Instance.zombieTooth.teethBottom : _limbData.teethBottom;
            _toothBottom = Object.Instantiate(toothGameObjectBottom,
                zombiePoints.teethPointBottom.position,
                zombiePoints.teethPointBottom.rotation);
            _teethPointBottom = zombiePoints.teethPointBottom;
            _toothBottom.transform.SetParent(_teethPointBottom);
            
            var hand = Object.Instantiate(
                ZombieManager.Instance.GetSpringHand(), 
                zombiePoints.startHandPoint, 
                zombiePoints.startHandPoint);
            hand.transform.localPosition = Vector3.zero;
            hand.transform.localEulerAngles = Vector3.zero;
            if(hand.gameObject.GetComponentInChildren<FixedJoint>()!=null)
            hand.gameObject.GetComponentInChildren<FixedJoint>().connectedBody = zombiePoints.rigidbodyBodyBone;
//            hand.GetComponent<RagdollHandUc>().SetRootRigidbody(zombiePoints.startHandPoint.GetComponent<Rigidbody>());

            var entity = _world.NewEntity();
            entity.Replace(new ZombieComponent
            {
                Animator = gameObject.GetComponent<Animator>(),
                ZombiePointsUc = zombiePoints
            });
        }
    }
}
using Data;
using Leopotam.Ecs;
using Teeth.Components;
using Teeth.Data;
using Teeth.UnityComponents;
using UnityComponents;
using UnityEngine;

namespace Teeth.Systems
{
    public class CreateZombieSystem : IEcsInitSystem
    {
        private readonly ZombieData _zombieData = null;
        private readonly TeethData _teethData = null;

        private readonly EcsWorld _world = null;

        public void Init()
        {
            AnalyticsManager.Instance.LevelStart(1);
            
            var zombiePrefab = _zombieData.zombieSettings[ZombieManager.Instance.zombieIndex].zombieTeethPrefab;
            var gameObject =
                Object.Instantiate(zombiePrefab, _teethData.zombiePosition, Quaternion.identity);
            var zombiePoints = gameObject.GetComponent<ZombiePointsUc>();
            var hand = ZombieManager.Instance.GetHand();
            var handTransform =
                Object.Instantiate(hand, zombiePoints.handPoint.position, zombiePoints.handPoint.rotation).transform;
            handTransform.SetParent(zombiePoints.handPoint);
            handTransform.localPosition = Vector3.zero;
            handTransform.localScale = Vector3.one;

            var entity = _world.NewEntity();
            entity.Replace(new ZombieComponent
            {
                Animator = gameObject.GetComponent<Animator>(),
                ZombiePointsUc = zombiePoints
            });
        }
    }
}
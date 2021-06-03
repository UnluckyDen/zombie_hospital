using Leopotam.Ecs;
using Limb.Components;
using UnityEngine;
using UnityEngine.UI;

namespace Limb.Systems
{
    public class MoveHandSystem : IEcsRunSystem
    {
        private readonly EcsFilter<HandComponent> _handFilter = null;

        public void Run()
        {
            foreach (var idx in _handFilter)
            {
                ref var handComponent = ref _handFilter.Get1(idx);

                var position = handComponent.ToParentTransform
                    ? handComponent.CurrentParentTransform.position
                    : handComponent.NewPosition;
                var rotation = handComponent.CurrentParentTransform.rotation;
                var scale = handComponent.CurrentParentTransform.localScale;

                // if(position.y < handComponent.StartParentTransform.position.y)
                //     position = new Vector3(position.x, handComponent.StartParentTransform.position.y, position.z);

                handComponent.Transform.position = Vector3.Lerp(
                    handComponent.Transform.position,
                    position,
                    handComponent.Speed * Time.deltaTime);
                handComponent.Transform.rotation = Quaternion.Lerp(
                    handComponent.Transform.rotation,
                    rotation,
                    handComponent.Speed * Time.deltaTime);
                handComponent.Transform.localScale = Vector3.Lerp(
                    handComponent.Transform.localScale,
                    scale,
                    handComponent.Speed * Time.deltaTime);
            }
        }
    }
}
using Leopotam.Ecs;
using Trepanation.Components;
using UnityEngine;

namespace Trepanation.Systems
{
    public class MoveItemSystem : IEcsRunSystem
    {
        private readonly EcsFilter<ItemComponent> _itemFilter = null;
        
        public void Run()
        {
            // Debug.Log("MoveItem System");
            foreach (var idx in _itemFilter)
            {
                ref var itemComponent = ref _itemFilter.Get1(idx);

                var position = itemComponent.ToParentTransform
                    ? itemComponent.CurrentParentTransform.position
                    : itemComponent.NewPosition;
                var rotation = itemComponent.CurrentParentTransform.rotation;

                itemComponent.Transform.position = Vector3.Lerp(
                    itemComponent.Transform.position,
                    position,
                    itemComponent.Speed * Time.deltaTime);
                itemComponent.Transform.rotation = Quaternion.Lerp(
                    itemComponent.Transform.rotation,
                    rotation,
                    itemComponent.Speed * Time.deltaTime);
            }
        }
    }
}
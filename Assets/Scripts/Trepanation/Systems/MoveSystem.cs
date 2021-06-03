using Leopotam.Ecs;
using Trepanation.Components;
using Trepanation.UnityComponents;
using UnityEngine;

namespace Trepanation.Systems
{
    public class MoveSystem : IEcsRunSystem
    {
        private readonly Camera _camera = null;
        private readonly EcsFilter<ItemComponent> _itemFilter = null;
        private readonly EcsFilter<ZombieComponent> _zombieFilter = null;
        private readonly EcsFilter<ItemSelectedEvent> _itemSelectedFilter = null;

        private Collider _hitCollider;
        private Vector3 _startHitPosition;

        private float? _headPointX;

        public void Run()
        {
            if (!_itemSelectedFilter.IsEmpty()) return;
            
            // Debug.Log("Move System");

            foreach (var idx in _itemFilter)
            {
                ref var itemComponent = ref _itemFilter.Get1(idx);

                var ray = _camera.ScreenPointToRay(Input.mousePosition);
                if (!Physics.Raycast(ray, out var hit, 10f)) continue;
                if (AnyOtherAttached(itemComponent)) continue;

                if (Input.GetMouseButtonDown(0))
                {
                    if (hit.transform != itemComponent.Transform) continue;
                    if (hit.collider.GetComponent<ItemUc>())
                    {
                        itemComponent.Speed = 10;
                        itemComponent.NewPosition = itemComponent.CurrentParentTransform.position;
                        itemComponent.StartMovePosition = hit.transform.position;
                        itemComponent.ToParentTransform = false;

                        _hitCollider = hit.collider;
                        SetItemColliders(false);
                        Physics.Raycast(ray, out hit, 10f);

                        _startHitPosition = hit.point;
                        continue;
                    }
                }

                if (_hitCollider == null || itemComponent.ToParentTransform) continue;

                if (Input.GetMouseButton(0))
                {
                    var addPosition = hit.point - _startHitPosition;

                    foreach (var idy in _zombieFilter)
                    {
                        ref var zombieComponent = ref _zombieFilter.Get1(idy);
                        var headPoint = zombieComponent.ZombiePointsUc.headInsertPoint;

                        if (_headPointX == null)
                            _headPointX = headPoint.position.x;

                        if (itemComponent.StartMovePosition.x + addPosition.x < _headPointX.Value - 0.1f)
                        {
                            itemComponent.CurrentParentTransform = itemComponent.StartParentTransform;
                            itemComponent.Attached = false;

                            itemComponent.NewPosition = new Vector3(
                                itemComponent.StartMovePosition.x + addPosition.x,
                                itemComponent.StartParentTransform.position.y + 0.3f,
                                itemComponent.StartMovePosition.z + addPosition.z);

                            continue;
                        }

                        itemComponent.CurrentParentTransform = headPoint;
                        itemComponent.NewPosition = headPoint.position;
                        itemComponent.Attached = true;

                        return;
                    }
                }

                if (!Input.GetMouseButtonUp(0)) continue;
                itemComponent.NewPosition = itemComponent.CurrentParentTransform.position;
                itemComponent.ToParentTransform = true;
                _hitCollider = null;
                SetItemColliders(true);
            }
        }

        private void SetItemColliders(bool enabled)
        {
            foreach (var idx in _itemFilter)
            {
                ref var itemComponent = ref _itemFilter.Get1(idx);
                itemComponent.Collider.enabled = enabled;
            }
        }

        private bool AnyOtherAttached(ItemComponent currentItemComponent)
        {
            foreach (var idx in _itemFilter)
            {
                ref var itemComponent = ref _itemFilter.Get1(idx);
                if (!itemComponent.Attached) continue;
                if (itemComponent.Collider == currentItemComponent.Collider) continue;
                return true;
            }

            return false;
        }
    }
}
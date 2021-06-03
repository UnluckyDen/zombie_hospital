using Leopotam.Ecs;
using Teeth.Components;
using Teeth.UnityComponents;
using UnityEngine;

namespace Teeth.Systems
{
    public class ToothSystem : IEcsRunSystem
    {
        private static readonly int Insert = Animator.StringToHash("Insert");

        private readonly Camera _camera = null;
        private readonly EcsFilter<ToothComponent> _toothFilter = null;
        private readonly EcsFilter<ZombieComponent> _zombieFilter = null;
        private readonly EcsFilter<CompletedEvent> _completedEvent = null;

        private Collider _hitCollider;
        private Vector3 _startHitPosition;

        private float? _teethPointY;

        public void Run()
        {
            foreach (var idx in _toothFilter)
            {
                ref var toothComponent = ref _toothFilter.Get1(idx);

                if (!_completedEvent.IsEmpty())
                {
                    toothComponent.Speed = 1000;
                    foreach (var idy in _zombieFilter)
                    {
                        ref var zombieComponent = ref _zombieFilter.Get1(idy);
                        zombieComponent.Animator.SetBool(Insert, false);
                    }
                    return;
                }
                
                var ray = _camera.ScreenPointToRay(Input.mousePosition);
                if (!Physics.Raycast(ray, out var hit, 10f)) continue;
                if (AnyOtherInMouth(toothComponent)) continue;

                if (Input.GetMouseButtonDown(0))
                {
                    if (hit.transform != toothComponent.Transform) continue;
                    if (hit.collider.GetComponent<ToothUc>())
                    {
                        toothComponent.Speed = 10;
                        toothComponent.CurrentParentTransform = toothComponent.StartParentTransform;
                        toothComponent.NewPosition = toothComponent.StartParentTransform.position;
                        toothComponent.ToParentTransform = false;
                        toothComponent.StartMovePosition = hit.transform.position;

                        _hitCollider = hit.collider;
                        SetToothColliders(false);
                        Physics.Raycast(ray, out hit, 10f);
                        _startHitPosition = hit.point;
                        continue;
                    }
                }

                if (_hitCollider == null || toothComponent.ToParentTransform) continue;

                if (Input.GetMouseButton(0))
                {
                    var addPosition = hit.point - _startHitPosition;

                    foreach (var idy in _zombieFilter)
                    {
                        ref var zombieComponent = ref _zombieFilter.Get1(idy);
                        var teethPoint = zombieComponent.ZombiePointsUc.teethPoint;

                        if (_teethPointY == null)
                            _teethPointY = teethPoint.position.y;

                        if (toothComponent.StartMovePosition.y + addPosition.y < _teethPointY.Value + 0.2f)
                        {
                            toothComponent.CurrentParentTransform = toothComponent.StartParentTransform;
                            zombieComponent.Animator.SetBool(Insert, false);
                            toothComponent.InMouth = false;

                            toothComponent.NewPosition = new Vector3(
                                toothComponent.StartMovePosition.x + addPosition.x,
                                toothComponent.StartMovePosition.y + addPosition.y,
                                toothComponent.StartParentTransform.position.z);

                            continue;
                        }

                        zombieComponent.Animator.SetBool(Insert, true);
                        toothComponent.CurrentParentTransform = teethPoint;
                        toothComponent.NewPosition = teethPoint.position;
                        toothComponent.InMouth = true;
                        //newPositionSet = true;
                        return;
                    }
                }

                if (!Input.GetMouseButtonUp(0)) continue;
                toothComponent.NewPosition = toothComponent.CurrentParentTransform.position;
                toothComponent.ToParentTransform = true;
                _hitCollider = null;
                SetToothColliders(true);
            }
        }

        private void SetToothColliders(bool enabled)
        {
            foreach (var idx in _toothFilter)
            {
                ref var toothComponent = ref _toothFilter.Get1(idx);
                toothComponent.Collider.enabled = enabled;
            }
        }

        private bool AnyOtherInMouth(ToothComponent currentToothComponent)
        {
            foreach (var idx in _toothFilter)
            {
                ref var toothComponent = ref _toothFilter.Get1(idx);
                if (!toothComponent.InMouth) continue;
                if (toothComponent.Collider == currentToothComponent.Collider) continue;
                return true;
            }

            return false;
        }
    }
}
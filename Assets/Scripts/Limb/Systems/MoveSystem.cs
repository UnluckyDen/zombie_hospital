using Leopotam.Ecs;
using Limb.Components;
using Limb.UnityComponents;
using UnityEngine;

namespace Limb.Systems
{
    public class MoveSystem : IEcsRunSystem
    {
        private readonly Camera _camera = null;
        private readonly EcsFilter<HandComponent> _handFilter = null;
        private readonly EcsFilter<ZombieComponent> _zombieFilter = null;
        private readonly EcsFilter<HandSelectedEvent> _handSelectedFilter = null;

        private Collider _hitCollider;
        private Vector3 _startHitPosition;

        private float? _handPointZ;

        public void Run()
        {
            if (!_handSelectedFilter.IsEmpty()) return;

            foreach (var idx in _handFilter)
            {
                ref var handComponent = ref _handFilter.Get1(idx);

                var ray = _camera.ScreenPointToRay(Input.mousePosition);
                if (!Physics.Raycast(ray, out var hit, 10f)) continue;
                if (AnyOtherInMouth(handComponent)) continue;

                if (Input.GetMouseButtonDown(0))
                {
                    if (hit.transform != handComponent.Transform) continue;
                    if (hit.collider.GetComponent<HandUc>())
                    {
                        handComponent.Speed = 10;
                        //handComponent.CurrentParentTransform = handComponent.StartParentTransform;
                        handComponent.NewPosition = handComponent.CurrentParentTransform.position;
                        handComponent.StartMovePosition = hit.transform.position;
                        handComponent.ToParentTransform = false;

                        _hitCollider = hit.collider;
                        SetHandColliders(false);
                        Physics.Raycast(ray, out hit, 10f);

                        _startHitPosition = hit.point;
                        continue;
                    }
                }

                if (_hitCollider == null || handComponent.ToParentTransform) continue;

                if (Input.GetMouseButton(0))
                {
                    var addPosition = hit.point - _startHitPosition;

                    foreach (var idy in _zombieFilter)
                    {
                        ref var zombieComponent = ref _zombieFilter.Get1(idy);
                        var handPoint = zombieComponent.ZombiePointsUc.handPoint;

                        if (_handPointZ == null)
                            _handPointZ = handPoint.position.z;

                        if (handComponent.StartMovePosition.z + addPosition.z < _handPointZ.Value - 0.05f)
                        {
                            handComponent.CurrentParentTransform = handComponent.StartParentTransform;
                            // zombieComponent.Animator.SetBool(Insert, false);
                            handComponent.Attached = false;

                            handComponent.NewPosition = new Vector3(
                                handComponent.StartMovePosition.x + addPosition.x,
                                handComponent.StartParentTransform.position.y + 0.1f,
                                handComponent.StartMovePosition.z + addPosition.z);

                            continue;
                        }

                        // zombieComponent.Animator.SetBool(Insert, true);
                        handComponent.CurrentParentTransform = handPoint;
                        handComponent.NewPosition = handPoint.position;
                        handComponent.Attached = true;

                        return;
                    }
                }

                if (!Input.GetMouseButtonUp(0)) continue;
                handComponent.NewPosition = handComponent.CurrentParentTransform.position; //1
                handComponent.ToParentTransform = true;
                _hitCollider = null;
                SetHandColliders(true);
            }
        }

        private void SetHandColliders(bool enabled)
        {
            foreach (var idx in _handFilter)
            {
                ref var handComponent = ref _handFilter.Get1(idx);
                handComponent.Collider.enabled = enabled;
            }
        }

        private bool AnyOtherInMouth(HandComponent currentToothComponent)
        {
            foreach (var idx in _handFilter)
            {
                ref var toothComponent = ref _handFilter.Get1(idx);
                if (!toothComponent.Attached) continue;
                if (toothComponent.Collider == currentToothComponent.Collider) continue;
                return true;
            }

            return false;
        }
    }
}
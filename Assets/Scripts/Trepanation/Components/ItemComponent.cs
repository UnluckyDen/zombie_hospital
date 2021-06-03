using UnityEngine;

namespace Trepanation.Components
{
    public struct ItemComponent
    {
        public Transform Transform;
        public Collider Collider;
        public Transform CurrentParentTransform;

        public Transform StartParentTransform;
        public bool ToParentTransform;
        public bool Attached;
        public Vector3 StartMovePosition;
        public Vector3 NewPosition;
        public float Speed;
    }
}
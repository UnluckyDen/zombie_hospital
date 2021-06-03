using UnityEngine;

namespace Limb.Components
{
    public struct HandComponent
    {
        public Transform Transform;
        public Collider Collider;
        public Transform CurrentParentTransform;
        public int HandIndex;

        public Transform StartParentTransform;
        public bool ToParentTransform;
        public bool Attached;
        public Vector3 StartMovePosition;
        public Vector3 NewPosition;
        public float Speed;
    }
}
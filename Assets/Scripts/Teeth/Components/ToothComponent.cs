using UnityEngine;

namespace Teeth.Components
{
    public struct ToothComponent
    {
        public GameObject ToothPrefab;
        public Transform Transform;
        public Collider Collider;
        public Transform CurrentParentTransform;

        public Transform StartParentTransform;
        public bool ToParentTransform;
        public bool InMouth;
        public Vector3 StartMovePosition;
        public Vector3 NewPosition;
        public float Speed;
    }
}
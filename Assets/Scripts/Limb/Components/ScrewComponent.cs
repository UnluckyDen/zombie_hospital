using Limb.UnityComponents;
using UnityEngine;

namespace Limb.Components
{
    public struct ScrewComponent
    {
        public Transform Transform;
        public ScrewUc ScrewUc;
        public Vector3? StartPosition;
        public bool ReturnToOriginal;
    }
}
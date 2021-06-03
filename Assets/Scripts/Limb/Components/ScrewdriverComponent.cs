using Limb.UnityComponents;
using UnityEngine;

namespace Limb.Components
{
    public struct ScrewdriverComponent
    {
        public Transform Transform;
        public ScrewdriverUc ScrewdriverUc;
        public Vector3? StartPosition;
        public bool Ready;
    }
}
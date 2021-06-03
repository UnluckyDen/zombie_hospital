using UnityEngine;

namespace Teeth.UnityComponents
{
    public class RagdollHandUc : MonoBehaviour
    {
        [SerializeField] private SpringJoint joint;
        
        public void SetRootRigidbody(Rigidbody connectBody)
        {
            joint.connectedBody = connectBody;
        }
    }
}

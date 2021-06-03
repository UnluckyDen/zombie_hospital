using UnityEngine;

namespace Teeth.UnityComponents
{
    public class HammerUc : MonoBehaviour
    {
        private void OnTriggerEnter(Collider enterCollider)
        {
            if (enterCollider.CompareTag("Teeth")) enterCollider.GetComponent<TeethUc>().Kick();
        }
    }
}
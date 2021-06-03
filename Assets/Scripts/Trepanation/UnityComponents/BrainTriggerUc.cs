using System;
using UnityEngine;

namespace Trepanation.UnityComponents
{
    public class BrainTriggerUc : MonoBehaviour
    {
        [HideInInspector] public bool hasBrain;
        
        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Brain"))
                hasBrain = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Brain"))
                hasBrain = false;
            other.transform.SetParent(null);
        }
    }
}

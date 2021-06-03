using System.Collections;
using UnityEngine;

namespace Teeth.UnityComponents
{
    public class TeethUc : MonoBehaviour
    {
        [SerializeField] private int toothStrength;
        [SerializeField] private float lifetime = 1f;

        private Rigidbody _rigidbody;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Kick()
        {
            toothStrength--;
            if (toothStrength != 0) return;
            _rigidbody.isKinematic = false;
            StartCoroutine(nameof(LifeTime));
        }

        private IEnumerator LifeTime()
        {
            yield return new WaitForSeconds(lifetime);
            Destroy(gameObject);
        }
    }
}
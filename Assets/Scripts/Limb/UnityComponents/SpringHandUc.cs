using System.Collections;
using UnityEngine;

namespace Limb.UnityComponents
{
    public class SpringHandUc : MonoBehaviour
    {
        public Transform particlePoint;
        
        [SerializeField] private Transform mainTransform;
        [SerializeField] private Rigidbody springRigidbody;
        [SerializeField] private FixedJoint fixedJoint;
        
        private Rigidbody _currentRigidbody;
        private Vector3 _forceDirection;
        private bool _separate;
        //private GameObject _armchair;

        private SpringJoint _springJoint;

        private void Start()
        {
            //_armchair = GameObject.Find("Armchair");
            _springJoint = GetComponent<SpringJoint>();
            _currentRigidbody = GetComponent<Rigidbody>();
            _forceDirection = Vector3.zero;
        }

        private void Update()
        {
            if (_separate) return;
            if(Input.GetMouseButtonUp(0))
                _forceDirection = Vector3.zero;
        }

        private IEnumerator HideHand()
        {
            yield return new WaitForSeconds(0.25f);
            var time = 1f;
            while (time > 0)
            {
                transform.Translate(Vector3.left * (5 * Time.deltaTime));
                time -= Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);
            }
            Destroy(mainTransform.gameObject);
        }

        private void FixedUpdate()
        {
            if(!_separate)
                _currentRigidbody.AddForce(_forceDirection);
        }

        public void AddForce(Vector3 forceDirection)
        {
            _forceDirection = forceDirection;
        }

        public float GetSpringDistance()
        {
            return Vector3.Distance(transform.position, mainTransform.position);
        }

        public void SeparateHand()
        {
            _currentRigidbody.isKinematic = true;
            springRigidbody.isKinematic = false;
            _separate = true;
            StartCoroutine(nameof(HideHand));
            StartCoroutine(nameof(SetPhysics));
        }

        private IEnumerator SetPhysics()
        {
            yield return new WaitForSeconds(0.1f);
            //_armchair.GetComponent<BoxCollider>().isTrigger = true;
            Destroy(_springJoint);
            gameObject.AddComponent<CharacterJoint>().connectedBody = springRigidbody;
            gameObject.GetComponent<CapsuleCollider>().isTrigger = false;
            Destroy(fixedJoint);

            _currentRigidbody.velocity = Vector3.zero;
            _currentRigidbody.isKinematic = false;
            _currentRigidbody.mass = 1;
            _currentRigidbody.drag = 0;
            _currentRigidbody.useGravity = true;
            
            springRigidbody.velocity = Vector3.zero;
            springRigidbody.mass = 1;
            springRigidbody.drag = 0;
            springRigidbody.isKinematic = false;
            springRigidbody.useGravity = true;
            springRigidbody.gameObject.GetComponent<CapsuleCollider>().isTrigger = false;
            yield return true;
        }
    }
}

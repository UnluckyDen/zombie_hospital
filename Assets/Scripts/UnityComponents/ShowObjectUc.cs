using UnityEngine;

namespace UnityComponents
{
    public class ShowObjectUc : MonoBehaviour
    {
        private float _scale = -1f;

        private void Update()
        {
            _scale += Time.deltaTime * 5;
            if (_scale > 0) transform.localScale = Vector3.one * _scale;
            if (_scale < 1) return;
            transform.localScale = Vector3.one;
            Destroy(this);
        }
    }
}
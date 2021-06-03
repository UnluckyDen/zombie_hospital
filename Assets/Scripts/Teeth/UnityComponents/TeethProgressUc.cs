using UnityEngine;
using UnityEngine.UI;

namespace Teeth.UnityComponents
{
    public class TeethProgressUc : MonoBehaviour
    {
        [HideInInspector] public bool completed;

        private Image _barForeground;
        private int _startCount;
        private float _progress;

        private const int TeethMayStay = 20;

        private void Start()
        {
            _barForeground = GameObject.Find("Foreground").GetComponent<Image>();
            _startCount = transform.childCount - TeethMayStay;
            _barForeground.fillAmount = 0;
        }

        private void Update()
        {
            if (completed) return;
            _progress = Mathf.Clamp(1 - (transform.childCount - TeethMayStay) / (float) _startCount, 0, 1);
            if (_progress >= 1)
            {
                _barForeground.transform.parent.gameObject.SetActive(false);
                completed = true;
                for (var i = 0; i < transform.childCount; i++) Destroy(transform.GetChild(i).gameObject);
            }

            _barForeground.fillAmount = Mathf.Lerp(_barForeground.fillAmount, _progress, Time.deltaTime * 10);
        }
    }
}
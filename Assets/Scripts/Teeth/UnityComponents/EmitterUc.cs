using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Teeth.UnityComponents
{
    public class EmitterUc : MonoBehaviour
    {
        public enum AnimationType
        {
            Click
        }
        private const float ShowTime = 1f;

        [Header("Main Buttons")]
        public Button soundButton;
        public Button pauseButton;
        public Sprite pauseSprite;
        public Sprite playSprite;
        
        public Button chooseThisButton;
        public Button nextButton;
        
        [Header("Finish Screen General")]
        public GameObject finishScreenParent;
        public Image avatar;
        public Image stamp;
        public Image progressBar;
        public Text moneyText;

        [Header("Check Marks")]
        public Image teethCheckMark;
        public Image handCheckMark;
        public Image brainCheckMark;

        [Header("Tutorial")]
        [SerializeField] private RectTransform parent;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private RectTransform cursor;
        [SerializeField] private Image hand;
        [SerializeField] private Image circle;
        
        private Animator _cursorAnimator;
        private float _time;
        private bool _show;

        private void Start()
        {
            _time = ShowTime;
            _cursorAnimator = cursor.GetComponentInChildren<Animator>();
            
            circle.enabled = false;
            hand.enabled = false;
        }

        public void SetCursorPosition(Vector2 position)
        {
            _show = true;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, position, mainCamera, out var localPoint);
            cursor.anchoredPosition = localPoint;
            _cursorAnimator.Play("Click");
        }

        public void Hide()
        {
            enabled = false;
            StopCoroutine(nameof(Swipe));
            circle.enabled = false;
            hand.enabled = false;
        }

        public void SetCursorSwipePositions(Vector2 startPosition, Vector2 endPosition)
        {
            _show = true;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, startPosition, mainCamera, out var startPoint);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, endPosition, mainCamera, out var endPoint);

            _startPoint = startPoint;
            _endPoint = endPoint;
            
            if(!_swipe) StartCoroutine(nameof(Swipe));
        }

        private Vector2 _startPoint;
        private Vector2 _endPoint;
        private bool _swipe;

        private IEnumerator Swipe()
        {
            _swipe = true;
            cursor.anchoredPosition = _startPoint;
            var position = 0f;
            _cursorAnimator.Play("Swipe");
            yield return new WaitForSeconds(0.5f);
            while (Math.Abs(cursor.anchoredPosition.x - _endPoint.x) > 0.1f && 
                   Math.Abs(cursor.anchoredPosition.y - _endPoint.y) > 0.1f)
            {
                position += Time.deltaTime;
                cursor.anchoredPosition = Vector2.Lerp(_startPoint, _endPoint, position);
                yield return new WaitForSeconds(Time.deltaTime);
            }
            yield return new WaitForSeconds(0.5f);
            _swipe = false;
        }

        private void Update()
        {
            if (!_show) return;
            
            if (Input.GetMouseButtonDown(0))
            {
                circle.enabled = false;
                hand.enabled = false;
                _time = ShowTime;
            }

            _time -= Time.deltaTime;
            
            if (!(_time < 0)) return;
            
            circle.enabled = true;
            hand.enabled = true;
        }
    }
}

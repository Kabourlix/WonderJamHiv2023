using System.Collections;
using UnityEngine;

namespace Game.Scripts.UI
{
    public class ApparitionController : MonoBehaviour
    {
        private RectTransform _rectTransform;
        private Vector2 _initPos;
        [SerializeField] private Vector2 hiddenPos;
        [SerializeField] private float smoothTime = 5;
        private Vector2 velocity;
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _initPos = _rectTransform.anchoredPosition;
        }
        
        [ContextMenu("Apparition")]
        public void Apparition()
        {
            StopAllCoroutines();
            StartCoroutine(ApparitionCoroutine());
        }
        
        [ContextMenu("Disapparition")]
        public void Disapparition()
        {
            StopAllCoroutines();
            StartCoroutine(DisapparitionCoroutine());
        }

        public IEnumerator ApparitionCoroutine()
        {
            while ((_rectTransform.anchoredPosition - _initPos).sqrMagnitude > 0.1f )
            {
                _rectTransform.anchoredPosition = Vector2.SmoothDamp(_rectTransform.anchoredPosition, _initPos, ref velocity, smoothTime);
                yield return typeof(WaitForEndOfFrame);
            }
            _rectTransform.anchoredPosition = _initPos;
        }
        
        public IEnumerator DisapparitionCoroutine()
        {
            while ((_rectTransform.anchoredPosition - hiddenPos).sqrMagnitude > 0.1f )
            {
                _rectTransform.anchoredPosition = Vector2.SmoothDamp(_rectTransform.anchoredPosition, hiddenPos, ref velocity, smoothTime);
                yield return typeof(WaitForEndOfFrame);
            }
            _rectTransform.anchoredPosition = hiddenPos;
        }
        
        
    }
}
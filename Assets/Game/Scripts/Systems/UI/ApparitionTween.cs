using System;
using UnityEngine;

namespace Game.Scripts.Systems.UI
{
    public class ApparitionTween : MonoBehaviour
    {
        private Vector3 _initPos;
        
        [SerializeField] private Vector2 hideDirection;
        [SerializeField] private float smoothTime = 5;
        [SerializeField] private LeanTweenType easeType = LeanTweenType.easeOutQuad;

        private void Start()
        {
            _initPos = transform.position;
        }

        private bool _isActive = false;
        public bool IsActive => _isActive;
        public bool IsShown { get; private set; }

        [ContextMenu("Disappear")]
        public void Disappear()
        {
            if (_isActive) return;
            _isActive = true;
            LeanTween.move(gameObject,
                    transform.position + hideDirection.x * Vector3.right + hideDirection.y * Vector3.up, smoothTime)
                .setEase(easeType)
                .setOnComplete(() =>
                {
                    _isActive = false;
                    IsShown = false;
                });
            
        }
        
        [ContextMenu("Appear")]
        public void Appear()
        {
            if (_isActive) return;
            _isActive = true;
            LeanTween.move(gameObject, transform.position - hideDirection.x*Vector3.right - hideDirection.y*Vector3.up, smoothTime)
                .setEase(easeType)
                .setOnComplete(() =>
                {
                    transform.position = _initPos;
                    _isActive = false;
                    IsShown = true;
                });
        }
        
        public void Appear(Action callback)
        {
            if(_isActive) return;
            _isActive = true;
            LeanTween.move(gameObject, transform.position - hideDirection.x*Vector3.right - hideDirection.y*Vector3.up, smoothTime)
                .setEase(easeType)
                .setOnComplete(() =>
                {
                    callback?.Invoke(); 
                    transform.position = _initPos;
                    _isActive = false;
                    IsShown = true;
                });
        }
    }
}
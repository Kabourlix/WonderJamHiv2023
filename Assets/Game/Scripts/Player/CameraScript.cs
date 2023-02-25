using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _smoothTime = 0.3f;
    [SerializeField] private bool _fixedY;
    private Vector3 _offset;
    private float _initialY;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        _offset = transform.position - _target.position;
        _initialY = transform.position.y;
        PlayerController.OnEvolve += () => _isEvolving = true;
        PlayerController.OnEvolveEnd += () => _isEvolving = false;
    }

    private bool _isEvolving = false;
    private void LateUpdate()
    {
        if (_isEvolving)
        {
            Debug.Log("Evolving");
            return;
        }
        Vector3 desiredPosition = _target.position + _offset;
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position,desiredPosition, ref velocity, _smoothTime);

        if(_fixedY)
        {
            smoothedPosition.y = _initialY;
        }

        transform.position = smoothedPosition;
    }
}

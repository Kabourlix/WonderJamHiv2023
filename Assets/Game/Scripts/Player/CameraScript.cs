using Game.Inputs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _smoothTime = 0.3f;
    [SerializeField] private bool _fixedY;
    [Tooltip("Degree per second")]
    [SerializeField] private float _rotationSpeed=50;
    private Vector3 _offset;
    private Vector3 _lookToward;
    private float _yAngleFromTarget = -90;
    private float _initialY;
    private float _deltaAngle=0;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        _offset = transform.position - _target.position;
        _initialY = transform.position.y;
        PlayerController.OnEvolve += () => _isEvolving = true;
        PlayerController.OnEvolveEnd += () => _isEvolving = false;
        InputManager.OnMoveCameraEvent += (float newValue) => { _deltaAngle = newValue; };
        _lookToward = Vector3.Normalize(_target.position - transform.position);
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

        Vector3 desiredLookDirection=Vector3.Normalize(_target.position - smoothedPosition);
        _lookToward = desiredLookDirection;

        if (_fixedY)
        {
            smoothedPosition.y = _initialY;
        }

        Rotate(_deltaAngle);
        transform.position = smoothedPosition;
        transform.LookAt(_target.position);
    }

    private void Rotate(float deltaAngle)
    {
        _yAngleFromTarget += deltaAngle*Time.deltaTime*_rotationSpeed;
        float angleInRadians = Mathf.Deg2Rad * _yAngleFromTarget;

        float distanceOffset = Vector3.Magnitude(new Vector3(_offset.x,0,_offset.z));
        Vector3 newOffset = new Vector3(Mathf.Cos(angleInRadians) * distanceOffset, _offset.y, Mathf.Sin(angleInRadians) * distanceOffset);
        _offset = newOffset;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Is created when you pickup an item, and is destroyed when you drop it. Will 
/// </summary>
public class LeashFollowScript : MonoBehaviour
{
    public GameObject _target;
    private Rigidbody _selfRigidbody;
    public Rigidbody _attachedRigidbody;
    public float _speed = 10;
    public float _acceleration = 10;
    public Vector3 _currentVelocity=Vector3.zero;
    private int _defaultLayerIndex;
    [SerializeField] private string _pickedObjectLayer;
    private bool _used=false;
    private float _timeElapsedSinceUse=0;
    private Vector3 _positionWhenUsed;
    private Vector3 _scaleWhenUsed;
    [Tooltip("The time it takes for the object to reach the target when used")]
    public float _timeToReachTarget=0.5f;

    public void Init(GameObject target, Rigidbody attachedRigidbody)
    {
        _attachedRigidbody = attachedRigidbody;
        _target = target;
        GetComponent<Joint>().connectedBody = _attachedRigidbody;
        _selfRigidbody = gameObject.GetComponent<Rigidbody>();
        attachedRigidbody.gameObject.layer=LayerMask.NameToLayer(_pickedObjectLayer);
    }

    private void FixedUpdate()
    {
        if (_target != null && !_used)
        {
            Vector3 targetPosition = _target.transform.position;
            Vector3 deltaPosition = targetPosition - transform.position;
            _currentVelocity= Vector3.MoveTowards(_currentVelocity, deltaPosition, Time.fixedDeltaTime*_speed);
            _selfRigidbody.position += _currentVelocity*Time.fixedDeltaTime * _acceleration;
            return;
        }
    }

    private void Update()
    {
        if (!_used) return;

        _timeElapsedSinceUse += Time.deltaTime/_timeToReachTarget;

        Vector3 newScale=Vector3.Lerp(_scaleWhenUsed, Vector3.zero, _timeElapsedSinceUse);
        Vector3 newPosition=Vector3.Lerp(_positionWhenUsed, _target.transform.position, _timeElapsedSinceUse);

        _attachedRigidbody.gameObject.transform.position = newPosition;
        _attachedRigidbody.gameObject.transform.localScale = newScale;
        
        if(_timeElapsedSinceUse>=1)
        {
            Destroy(_attachedRigidbody);
            Destroy(gameObject);
        }
    }

    public void OnUse(GameObject target)
    {
        _used = true;
        _target = target;

        _positionWhenUsed = _attachedRigidbody.gameObject.transform.position;
        _scaleWhenUsed = _attachedRigidbody.gameObject.transform.localScale;
        _attachedRigidbody.isKinematic= true;
        _attachedRigidbody.GetComponent<Collider>().enabled=false;
    }
}

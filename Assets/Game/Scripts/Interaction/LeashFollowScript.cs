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
    [Tooltip("If we get further than that from the attached object, we teleport it")]
    [SerializeField] private float _maxDistanceBeforeTeleport = 10;

    public GameObject _grabVfx;
    public GameObject _teleportVfx; 

    public void Init(GameObject target, Rigidbody attachedRigidbody)
    {
        _attachedRigidbody = attachedRigidbody;
        _target = target;
        GetComponent<Joint>().connectedBody = _attachedRigidbody;
        _selfRigidbody = gameObject.GetComponent<Rigidbody>();
        attachedRigidbody.gameObject.layer=LayerMask.NameToLayer(_pickedObjectLayer);
        GameObject newGo=Instantiate(_grabVfx, attachedRigidbody.transform);
        newGo.transform.localPosition = Vector3.zero;
    }

    private void FixedUpdate()
    {
        if (_target == null || _used) return;
       
            Vector3 targetPosition = _target.transform.position;
            Vector3 deltaPosition = targetPosition - transform.position;
            _currentVelocity= Vector3.MoveTowards(_currentVelocity, deltaPosition, Time.fixedDeltaTime*_speed);
            _selfRigidbody.position += _currentVelocity*Time.fixedDeltaTime * _acceleration;
            if (Vector3.SqrMagnitude(_attachedRigidbody.position - _selfRigidbody.position) > Mathf.Pow(_maxDistanceBeforeTeleport, 2))
            {
                Teleport();
            }

    }

    private void Update()
    {
        if (!_used) return;

        _timeElapsedSinceUse += Time.deltaTime/_timeToReachTarget;

        Vector3 newScale=Vector3.Lerp(_scaleWhenUsed, Vector3.zero, _timeElapsedSinceUse);

        _attachedRigidbody.gameObject.transform.localScale = newScale;
        
        if(_timeElapsedSinceUse>=1)
        {
            Destroy(_attachedRigidbody);
            Destroy(gameObject);
        }

        
    }

    public void OnUse()
    {
        _used = true;

        _positionWhenUsed = _attachedRigidbody.gameObject.transform.position;
        _scaleWhenUsed = _attachedRigidbody.gameObject.transform.localScale;
        _attachedRigidbody.isKinematic= true;
        _attachedRigidbody.GetComponent<Collider>().enabled=false;
    }

    /// <summary>
    /// If the item is too far away from the leash, we teleport it
    /// </summary>
    public void Teleport()
    {
        _attachedRigidbody.position = transform.position;
        GameObject newGo = Instantiate(_teleportVfx);
        newGo.transform.position = transform.position;
        Destroy(newGo, 3);
    }

}

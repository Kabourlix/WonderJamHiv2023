using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Structs
    [System.Serializable]
    public struct FormStat
    {
        public Forms form;
        public float accelerationRate;
        public float speed;
    }
    #endregion
    public enum Forms { bat, spider, humanoid}
    [SerializeField] Forms _baseForm = Forms.bat;
    [SerializeField] List<FormStat> _formStats = new List<FormStat>();
    [Tooltip("If the player is standing on collider in this mask, he falls. Must not contain the layer of the player, else it won't work")]
    [SerializeField] LayerMask _layersForGravity;
    private Vector3 _movementInput;
    private Rigidbody _rigidbody;
    private Collider _collider;
    private Forms _currentForm;
    private float _accelerationRate;
    private Vector3 _movementVelocity;
    private Vector3 _targetMovementVelocity;
    float _speed=10;

    public Forms CurrentForm { get => _currentForm; set {
            _currentForm = value;
            OnSwitchForm(_currentForm);
        }
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();  
        _collider = GetComponent<Collider>();
        CurrentForm= _baseForm;
    }

    void Start()
    {
        InputManager.OnMoveEvent += (Vector2 value) => { _movementInput = new Vector3(value.x,0,value.y); };
        InputManager.OnInteractEvent += OnInteract;
        InputManager.OnTest1Event += () => { CurrentForm = Forms.bat; };
        InputManager.OnTest2Event += () => { CurrentForm = Forms.spider; };
    }

    void FixedUpdate()
    {
        ApplyMovement();
        _rigidbody.velocity = new Vector3(_movementVelocity.x,_rigidbody.velocity.y,_movementVelocity.z);      
    }

    void ApplyMovement()
    {
        _targetMovementVelocity = _movementInput * _speed;
        _movementVelocity = Vector3.Lerp(_movementVelocity, _targetMovementVelocity, Time.fixedDeltaTime * _accelerationRate);
        AvoidFlying();
    }

    //Avoid player getting launched in the air after taking a ramp
    void AvoidFlying()
    {
        //If we're going upwards and there's no ground bellow us, we shouldn't
        if (_rigidbody.velocity.y > 0)
        {
            if(!Physics.BoxCast(_collider.bounds.center,_collider.bounds.size,Vector3.down,Quaternion.identity,0.5f,_layersForGravity)){
                Debug.Log(_collider.bounds.center.ToString());
                _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);
            }
        }
    }

    void OnInteract()
    {

    }

    void OnSwitchForm(Forms newForm)
    {
        FormStat formStat = _formStats.Find(x => x.form == newForm);
        _accelerationRate = formStat.accelerationRate;
        _speed= formStat.speed;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        _collider=GetComponent<Collider>();
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_collider.bounds.center + Vector3.down * 0.5f, _collider.bounds.size);
    }
#endif
}

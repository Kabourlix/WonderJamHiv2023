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
        [Tooltip("The maximum speed the player can fall at, along the y axis, positive if gravity is downward (like on earth)")]
        public float gravityTerminalVelocity;
        [Tooltip("1: human, 0:ghost, low:chicken in minecraft")]
        [Range(0f, 1f)]
        public float gravityMultiplier;
    }
    #endregion
    public enum Forms { bat, spider, humanoid}
    [SerializeField] Forms _baseForm = Forms.bat;
    [SerializeField] List<FormStat> _formStats = new List<FormStat>();
    [Tooltip("If the player is standing on collider in this mask, he falls. Must not contain the layer of the player, else it won't work")]
    [SerializeField] LayerMask _layersForGravity;
    private Vector3 _movementInput;
    private Rigidbody _rigidbody;
    private Forms _currentForm;
    private float _accelerationRate;
    private float _height;
    private Vector3 _movementVelocity;
    private Vector3 _targetMovementVelocity;
    private float _gravityVelocity;
    private Vector3 _gravityVelocityVector;
    private float _gravityTerminalVelocity;
    private float _gravityMultiplier;
    float _speed=10;

    public Forms CurrentForm { get => _currentForm; set {
            _currentForm = value;
            OnSwitchForm(_currentForm);
        }
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();    
        CurrentForm= _baseForm;
    }

    void Start()
    {
        InputManager.OnMoveEvent += (Vector2 value) => { _movementInput = new Vector3(value.x,0,value.y); };
        InputManager.OnInteractEvent += OnInteract;
        InputManager.OnTest1Event += () => { CurrentForm = Forms.bat; };
        InputManager.OnTest2Event += () => { CurrentForm = Forms.spider; };
        _height = GetComponent<Collider>().bounds.size.y;
    }

    void FixedUpdate()
    {
        ApplyMovement();
        ApplyGravity();
        _rigidbody.velocity = _movementVelocity + _gravityVelocityVector;      
    }

    void ApplyMovement()
    {
        _targetMovementVelocity = _movementInput * _speed;
        _movementVelocity = Vector3.Lerp(_movementVelocity, _targetMovementVelocity, Time.fixedDeltaTime * _accelerationRate);
    }

    void ApplyGravity()
    {
        if (Physics.Raycast(transform.position, Vector3.down, 0.5f * _height + 0.01f, _layersForGravity)) return;
        Debug.Log("PIPI");
        _gravityVelocity = Mathf.Clamp(_gravityVelocity+_gravityMultiplier*10*Time.fixedDeltaTime, -_gravityTerminalVelocity, _gravityTerminalVelocity);
        _gravityVelocityVector=new Vector3(0,-_gravityVelocity,0);
    }

    void OnInteract()
    {

    }

    void OnSwitchForm(Forms newForm)
    {
        FormStat formStat = _formStats.Find(x => x.form == newForm);
        _accelerationRate = formStat.accelerationRate;
        _speed= formStat.speed;
        _gravityMultiplier = formStat.gravityMultiplier;
        _gravityTerminalVelocity = formStat.gravityTerminalVelocity;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * (0.5f * _height + 0.01f));
    }
#endif
}

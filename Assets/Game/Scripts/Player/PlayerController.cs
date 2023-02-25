using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Interaction;
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
    private CharacterController _characterController;
    private Collider _collider;
    private Forms _currentForm;
    private float _accelerationRate;
    private Vector3 _movementVelocity;
    private Vector3 _targetMovementVelocity;
    float _speed=10;

    private float _gravityVelocity;
    private Vector3 _gravityVelocityVector;
    private float _gravityTerminalVelocity;
    private float _gravityMultiplier;

    [Header("Interaction")]
    [SerializeField] private CapsuleCollider interactCollider;
    [SerializeField] private LayerMask interactLayerMask;
    
    public Forms CurrentForm { get => _currentForm; set {
            _currentForm = value;
            OnSwitchForm(_currentForm);
        }
    }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();  
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
        ApplyGravity();
        _characterController.Move((_movementVelocity+_gravityVelocityVector)*Time.fixedDeltaTime);
    }

    void ApplyMovement()
    {
        _targetMovementVelocity = _movementInput * _speed;
        _movementVelocity = Vector3.Lerp(_movementVelocity, _targetMovementVelocity, Time.fixedDeltaTime * _accelerationRate);
    }

    void ApplyGravity()
    {
        if (_characterController.isGrounded) return;
        _gravityVelocity = Mathf.Clamp(_gravityVelocity + _gravityMultiplier * 10 * Time.fixedDeltaTime, -_gravityTerminalVelocity, _gravityTerminalVelocity);
        _gravityVelocityVector = new Vector3(0, -_gravityVelocity, 0);
    }


    void OnInteract()
    {
        Debug.Log("Interact");
        var bounds = interactCollider.bounds;
        var colliders = Physics.OverlapCapsule(bounds.center, bounds.center + new Vector3(0, interactCollider.height, 0), interactCollider.radius, interactLayerMask);
        foreach (var c in colliders)
        {
            Debug.Log($"{name} interacted with {c.name}");
            c.GetComponent<IInteractable>()?.Interact();
        }
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
    }
#endif
}

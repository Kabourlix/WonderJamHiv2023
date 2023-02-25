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
        [Tooltip("The animator must come from the prefab/scene and be attached to the visual representation of this form. Do not slide it from the assets")]
        public Animator animator;
    }

    #endregion
    public enum Forms { bat, spider, humanoid }

    [SerializeField] Forms _baseForm = Forms.bat;
    [SerializeField] List<FormStat> _formStats = new List<FormStat>();
    [Tooltip("If the player is standing on collider in this mask, he falls. Must not contain the layer of the player, else it won't work")]
    [SerializeField] LayerMask _layersForGravity;
    private Vector3 _movementInput;
    private CharacterController _characterController;
    private Collider _collider;
    private Animator _animator;
    private Forms _currentForm;
    private float _accelerationRate;
    private Vector3 _movementVelocity;
    private Vector3 _targetMovementVelocity;
    private float _orientation;
    float _speed = 10;


    private float _gravityVelocity;
    private Vector3 _gravityVelocityVector;
    private float _gravityTerminalVelocity;
    private float _gravityMultiplier;

    #region AnimatorHashes
    private int _isWalkingHash;
    private int _walkingSpeedHash;
    #endregion


    [Header("Interaction")]
    [SerializeField] private CapsuleCollider interactCollider;
    [SerializeField] private LayerMask interactLayerMask;

    public Forms CurrentForm
    {
        get => _currentForm;
        set
        {
            _currentForm = value;
            OnSwitchForm(_currentForm);
        }
    }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _collider = GetComponent<Collider>();

        CurrentForm = _baseForm;

        _isWalkingHash = Animator.StringToHash("IsWalking");
        _walkingSpeedHash = Animator.StringToHash("WalkingSpeed");
    }

    void Start()
    {

        InputManager.OnMoveEvent += (Vector2 value) => { _movementInput = new Vector3(value.x, 0, value.y); };
        InputManager.OnInteractEvent += OnInteract;
        InputManager.OnTest1Event += () => { CurrentForm = Forms.bat; };
        InputManager.OnTest2Event += () => { CurrentForm = Forms.spider; };
        InputManager.OnTest3Event += () => { CurrentForm = Forms.humanoid; };
        CurrentForm = _baseForm;
    }

    void Update()
    {
        ApplyMovement();
        ApplyGravity();
        AnimateMovement();
        _characterController.Move((_movementVelocity + _gravityVelocityVector) * Time.deltaTime);
    }

    void ApplyMovement()
    {
        _targetMovementVelocity = _movementInput * _speed;
        _movementVelocity = Vector3.Lerp(_movementVelocity, _targetMovementVelocity, Time.deltaTime * _accelerationRate);
    }

    void AnimateMovement()
    {
        _animator.SetBool(_isWalkingHash, _characterController.isGrounded && _movementInput.sqrMagnitude > 0.001f);
        _animator.SetFloat(_walkingSpeedHash, _movementInput.magnitude);
        if (_movementInput.sqrMagnitude <= 0.01f) return;
        _orientation = Mathf.MoveTowardsAngle(_orientation, Vector2.SignedAngle(new Vector2(_movementInput.x, _movementInput.z), Vector2.up), 1000 * Time.deltaTime);
        gameObject.transform.rotation = Quaternion.Euler(0, _orientation, 0);
    }

    void ApplyGravity()
    {
        if (_characterController.isGrounded) return;
        _gravityVelocity = Mathf.Clamp(_gravityVelocity + _gravityMultiplier * 10 * Time.deltaTime, -_gravityTerminalVelocity, _gravityTerminalVelocity);
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
        ClearPreviousForm();

        FormStat formStat = _formStats.Find(x => x.form == newForm);
        _accelerationRate = formStat.accelerationRate;
        _speed = formStat.speed;

        _gravityMultiplier = formStat.gravityMultiplier;
        _gravityTerminalVelocity = formStat.gravityTerminalVelocity;

        _animator = formStat.animator;
        _animator.gameObject.SetActive(true);
    }

    void ClearPreviousForm()
    {
        foreach (FormStat formStat in _formStats)
        {
            formStat.animator.gameObject.SetActive(false);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
    }
#endif
}

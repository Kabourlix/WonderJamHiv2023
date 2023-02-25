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
        [Tooltip("The animator must come from the prefab/scene and be attached to the visual representation of this form. Do not slide it from the assets")]
        public Animator animator;
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
    private Animator _animator;
    private Forms _currentForm;
    private float _accelerationRate;
    private Vector3 _movementVelocity;
    private Vector3 _targetMovementVelocity;
    float _speed=10;

    private float _gravityVelocity;
    private Vector3 _gravityVelocityVector;
    private float _gravityTerminalVelocity;
    private float _gravityMultiplier;

    public Forms CurrentForm { get => _currentForm; set {
            _currentForm = value;
            OnSwitchForm(_currentForm);
        }
    }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();  
        _collider = GetComponent<Collider>();
        
        foreach (FormStat formStat in _formStats)
        {
            formStat.animator.gameObject.SetActive(false);
        }

        CurrentForm = _baseForm;
    }

    void Start()
    {
        
        InputManager.OnMoveEvent += (Vector2 value) => { _movementInput = new Vector3(value.x,0,value.y); };
        InputManager.OnInteractEvent += OnInteract;
        InputManager.OnTest1Event += () => { CurrentForm = Forms.bat; };
        InputManager.OnTest2Event += () => { CurrentForm = Forms.spider; };
        CurrentForm = _baseForm;
    }

    void FixedUpdate()
    {
        ApplyMovement();
        ApplyGravity();
        AnimateMovement();
        _characterController.Move((_movementVelocity+_gravityVelocityVector)*Time.fixedDeltaTime);
    }

    void ApplyMovement()
    {
        _targetMovementVelocity = _movementInput * _speed;
        _movementVelocity = Vector3.Lerp(_movementVelocity, _targetMovementVelocity, Time.fixedDeltaTime * _accelerationRate);
    }

    void AnimateMovement()
    {
        _animator.SetBool("IsWalking", _movementInput.sqrMagnitude > 0.01f);
        if (_movementInput.sqrMagnitude <= 0.01f) return;
        float orientation = Vector2.SignedAngle(Vector2.right, new Vector2(_movementInput.x,_movementInput.z));
        _animator.gameObject.transform.rotation= Quaternion.Euler(0,orientation, 0);
    }

    void ApplyGravity()
    {
        if (_characterController.isGrounded) return;
        _gravityVelocity = Mathf.Clamp(_gravityVelocity + _gravityMultiplier * 10 * Time.fixedDeltaTime, -_gravityTerminalVelocity, _gravityTerminalVelocity);
        _gravityVelocityVector = new Vector3(0, -_gravityVelocity, 0);
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

        _animator = formStat.animator;
        formStat.animator.gameObject.SetActive(true);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
    }
#endif
}

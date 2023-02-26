using System;
using System.Collections;
using System.Collections.Generic;
using Game.Inputs;
using Game.Scripts.Interaction;
using Game.Scripts.Systems.Interaction;
using UnityEngine;
using UnityEngine.Serialization;
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

    [System.Serializable]
    public struct PushResolution
    {
        //Constructor
        public PushResolution(GameObject target, Vector3 originalPosition, Vector3 finalPosition)
        {
            this.target = target;
            this.originalPosition = originalPosition;
            this.finalPosition = finalPosition;
            this.progression = 0;
        }
        public GameObject target;
        public Vector3 originalPosition;
        public Vector3 finalPosition;
        public float progression;
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

    [Header("Push")]
    [SerializeField] private LayerMask pushLayerMask;
    [SerializeField] private CapsuleCollider pushCollider;
    private List<PushResolution> _pushResolutions;
    [SerializeField] private float _pushDistance = 5f;
    [SerializeField] private float _pushDuration = 0.5f;
    [SerializeField] private Animator _pushAnimator;

    #region AnimatorHashes
    private int _isWalkingHash;
    private int _walkingSpeedHash;
    private int _pushHash;
    #endregion


    [Header("Interaction")]
    [SerializeField] private CapsuleCollider interactCollider;
    [SerializeField] private LayerMask interactLayerMask;
    
    [Header("External")]
    [SerializeField] Transform _movementReferential;

    private bool _isSuspect;
    public bool IsSuspect 
    { 
        get => _isSuspect;
        set => _isSuspect = IsHoldingObject || value;
    }
    
    public bool IsHoldingObject { get; set; }
    
    public Forms CurrentForm
    {
        get => _currentForm;
        set
        {
            _currentForm = value;
            OnSwitchForm(_currentForm);
        }
    }

    public static PlayerController Instance { get; private set; }
    private void Awake()
    {
        if(Instance != null)
            Destroy(gameObject);
        Instance = this;
        _characterController = GetComponent<CharacterController>();
        _collider = GetComponent<Collider>();

        CurrentForm = _baseForm;

        _isWalkingHash = Animator.StringToHash("IsWalking");
        _walkingSpeedHash = Animator.StringToHash("WalkingSpeed");
        _pushHash = Animator.StringToHash("Push");

        _pushResolutions = new List<PushResolution>();
    }

    void Start()
    {

        InputManager.OnMoveEvent += (Vector2 value) => { _movementInput = new Vector3(value.x, 0, value.y); };
        InputManager.OnInteractEvent += OnInteract;
        InputManager.OnPushEvent += OnPush;
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
        //First we rotate the movement input
        Vector3 movementInput=_movementInput.x*_movementReferential.right+_movementInput.z*_movementReferential.forward;
        _targetMovementVelocity = movementInput * _speed;
        _movementVelocity = Vector3.Lerp(_movementVelocity, _targetMovementVelocity, Time.deltaTime * _accelerationRate);
    }

    void AnimateMovement()
    {
        _animator.SetBool(_isWalkingHash, _characterController.isGrounded && _movementInput.sqrMagnitude > 0.001f);
        _animator.SetFloat(_walkingSpeedHash, _movementInput.magnitude);
        if (_movementInput.sqrMagnitude <= 0.01f) return;
        _orientation = Mathf.MoveTowardsAngle(_orientation, Vector2.SignedAngle(new Vector2(_movementInput.x, _movementInput.z), Vector2.up), 1000 * Time.deltaTime);
        gameObject.transform.rotation = Quaternion.Euler(0, _movementReferential.eulerAngles.y+_orientation, 0);
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
            c.GetComponent<Interactable>()?.Interact();
        }
    }
    

    public static event Action OnEvolve; 
    public static event Action OnEvolveEnd;
    public void EvolveBegin()
    {
        OnEvolve?.Invoke();
    }
    
    public void EvolveEnd()
    {
        OnEvolveEnd?.Invoke();
        //Controls are enabled back by EvolveViewSwitch
    }
    
    void OnPush()
    {
        Debug.Log("Interact");
        var bounds = pushCollider.bounds;
        var colliders = Physics.OverlapCapsule(bounds.center, bounds.center + new Vector3(0, pushCollider.height, 0), pushCollider.radius, pushLayerMask);
        bool pushedOnce = false;
        foreach (var c in colliders)
        {
            if (c.GetComponent<Pushable>() == null) continue;
            
            Vector3 targetPosition = c.transform.position + transform.forward * _pushDistance;
            c.GetComponent<Pushable>().Push(targetPosition,_pushDuration);
            _pushAnimator.SetTrigger(_pushHash);
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


    [SerializeField] private ParticleSystem deathParticle;
    [FormerlySerializedAs("renderer")] [SerializeField] private Renderer playerRenderer;
    [ContextMenu("Die")]
    public void HandleDeath()
    {
        StartCoroutine(DeathCoroutine());
    }
    //!BIG DEBUG DEGUEULASSE A ENLEVER PLUS TARD
    public Vector3 spawnPoint;
    private IEnumerator DeathCoroutine()
    {
        deathParticle.Play();
        Debug.Log(deathParticle.main.duration);
        playerRenderer.enabled = false;
        //Disable Controls TODO
        yield return new WaitForSeconds(deathParticle.main.duration);
        //Enable controls TODO
        transform.position = spawnPoint;
        playerRenderer.enabled = true; 
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
    }
#endif
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class AIChild : MonoBehaviour
{
    CharacterController _characterController;
    public Vector3 TargetPosition; //The child wants to touch the fire
    public float Speed = 1f;
    public float StunTime = 0f;
    private Vector3 _movementVelocity;
    private Vector3 _gravityVelocityVector;
    private float _gravityVelocity;
    private float _gravityTerminalVelocity = 10;
    private float _gravityMultiplier = 1;
    [SerializeField] private float _accelerationRate = 50;
    private Vector3 _targetMovementVelocity;

    [SerializeField] private UnityEvent _onDie;

    [Header("Externals components")]
    [SerializeField] private Animator _animatorWithFire;
    [SerializeField] private Animator _animatorOfWizard;

    [Header("Animation")]
    [SerializeField] private string _burnTriggerName = "Burn";
    [SerializeField] private string _walkingSpeedName = "WalkingSpeed";
    [SerializeField] private string _isStunName = "IsStun";
    [SerializeField] private float _timeFromBurnToDestroy = 5;
    private int _burnTriggerHash;
    private int _walkingSpeedHash;
    private int _isStunNameHash;
    private float _orientation;

    private GameManager _gameManager;
    
    private bool IsDead=false;

    [Header("Dialogues")]
    [SerializeField] private List<string> _possibleDialoguesOnDeath;

    void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _burnTriggerHash = Animator.StringToHash(_burnTriggerName);
        _walkingSpeedHash = Animator.StringToHash(_walkingSpeedName);
        _isStunNameHash = Animator.StringToHash(_isStunName);
    }

    private void Start()
    {
        _gameManager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (TargetPosition == null) return;

        ApplyMovement();
        ApplyGravity();
        _characterController.Move((_movementVelocity + _gravityVelocityVector) * Time.deltaTime);
        AnimateMovement();
    }

    void ApplyMovement()
    {
        if (StunTime > 0)
        {
            StunTime -= Time.deltaTime;
            _movementVelocity= Vector3.zero;
            return;
        }

        _animatorOfWizard.SetBool(_isStunNameHash, false);
        _animatorWithFire.SetBool(_isStunNameHash, false);

        TargetPosition = new Vector3(TargetPosition.x, 0, TargetPosition.z);
        Vector3 currentPosition = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 direction = TargetPosition - currentPosition;

        if (direction.sqrMagnitude > 1)
        {
            direction.Normalize();
        }

        _targetMovementVelocity = direction * Speed;
        _movementVelocity = Vector3.MoveTowards(_movementVelocity, _targetMovementVelocity, Time.deltaTime * _accelerationRate);

    }

    void AnimateMovement()
    {
        float magnitude = _targetMovementVelocity.magnitude;
        _animatorOfWizard.SetFloat(_walkingSpeedHash, magnitude);
        if (magnitude <= 0.01f) return;
        _orientation = Mathf.MoveTowardsAngle(_orientation, Vector2.SignedAngle(new Vector2(_targetMovementVelocity.x, _targetMovementVelocity.z), Vector2.up), 1000 * Time.deltaTime);
        gameObject.transform.rotation = Quaternion.Euler(0, _orientation, 0);
    }


    void ApplyGravity()
    {
        if (_characterController.isGrounded) return;
        _gravityVelocity = Mathf.Clamp(_gravityVelocity + _gravityMultiplier * 10 * Time.deltaTime, -_gravityTerminalVelocity, _gravityTerminalVelocity);
        _gravityVelocityVector = new Vector3(0, -_gravityVelocity, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Fire")
        {
            Die();
        }
    }

    private void Die()
    {
        if(IsDead) return;
        IsDead = true;
        GetComponent<Pushable>().CanBePushed = false;
        _gameManager.LostAChild(); //Loose child hp
        _onDie.Invoke(); //Useless for now
        _animatorOfWizard.SetTrigger(_burnTriggerHash);
        _animatorWithFire.SetTrigger(_burnTriggerHash);

        StunTime= _timeFromBurnToDestroy+1;

        DialogueSystem.AddMessage("Villager: " + _possibleDialoguesOnDeath[Random.Range(0,_possibleDialoguesOnDeath.Count)],10);

        StartCoroutine(TeleportBeforeDying(_timeFromBurnToDestroy));    
    }

    //If I destroy the game object before it leaves the trigger it doesn't call "OnTriggerLeave", so instead, I send it in the sky before deleting it
    public IEnumerator TeleportBeforeDying(float time)
    {
        yield return new WaitForSeconds(time);
        _characterController.Move(Vector3.up * 100);
        _gravityMultiplier = 0;
        Destroy(gameObject, 1);
    }

    public void Stun(float time)
    {
        StunTime = time;
        _animatorOfWizard.SetBool(_isStunNameHash, true);
        _animatorWithFire.SetBool(_isStunNameHash, true);
    }
}

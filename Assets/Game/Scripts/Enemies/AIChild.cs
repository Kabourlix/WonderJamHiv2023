using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIChild : MonoBehaviour
{
    CharacterController _characterController;
    public Vector3 TargetPosition; //The child wants to touch the fire
    public float Speed = 1f;
    public float StunTime=0f;
    private Vector3 _movementVelocity;
    private Vector3 _gravityVelocityVector;
   private float _gravityVelocity;
    private float _gravityTerminalVelocity = 10;
    private float _gravityMultiplier = 1;
    [SerializeField] private float _accelerationRate = 50;
    private Vector3 _targetMovementVelocity;

    [SerializeField] private UnityEvent _onDie;

    void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (TargetPosition == null) return;

            ApplyMovement();
            ApplyGravity();
            _characterController.Move((_movementVelocity + _gravityVelocityVector) * Time.deltaTime);
    }

    void ApplyMovement()
    {
        if (StunTime > 0)
        {
            StunTime-= Time.deltaTime;
            return;
        }

        TargetPosition=new Vector3(TargetPosition.x,0,TargetPosition.z);
        Vector3 currentPosition = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 direction = TargetPosition - currentPosition;

        if(direction.sqrMagnitude>1)
        {
            direction.Normalize();
        }

        _targetMovementVelocity = direction * Speed;
        _movementVelocity = Vector3.MoveTowards(_movementVelocity, _targetMovementVelocity, Time.deltaTime * _accelerationRate);
    }

    void ApplyGravity()
    {
        if (_characterController.isGrounded) return;
        _gravityVelocity = Mathf.Clamp(_gravityVelocity + _gravityMultiplier * 10 * Time.deltaTime, -_gravityTerminalVelocity, _gravityTerminalVelocity);
        _gravityVelocityVector = new Vector3(0, -_gravityVelocity, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Fire")
        {
            Die();
        }
    }

    private void Die()
    {
        _onDie.Invoke();
    }

    public void Stun(float time) 
    { 
        StunTime = time;
    }
}

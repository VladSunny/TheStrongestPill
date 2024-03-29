using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed;
    public float sprintSpeed;
    public float groundDrag;
    public float crouchSpeed;
    
    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    
    // public Animator animator;
    protected Animator _animator;
    protected static readonly int IsWalking = Animator.StringToHash("isWalking");
    protected static readonly int Velocity = Animator.StringToHash("Velocity");
    protected static readonly int IsAir = Animator.StringToHash("isAir");
    [SerializeField] private Transform _hipsBone;
    
    protected bool _readyToJump;
    
    protected float _currentMoveSpeed;
    
    [Header("GroundCheck")]
    public LayerMask whatIsGround;
    
    protected bool _grounded;
    
    [Header("Slope Handling")]
    public float playerHeight = 2f;
    public float maxSlopeAngle;
    public float slopeSpeed = 20f;
    public float slopeForce = 80f;
    
    protected RaycastHit _slopeHit;
    protected bool _exitingSlope;
    
    protected Rigidbody _rb;
    
    [SerializeField] protected MovementState state = MovementState.walking;
    public enum MovementState
    {
        crouching,
        walking,
        sprinting,
        air
    }

    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        _rb.freezeRotation = true;
        _exitingSlope = false;
    }

    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        
        Debug.Log(_hipsBone);
    }


    protected virtual void MovementStateHandler()
    {
        if (_grounded)
        {
            state = MovementState.walking;
            _currentMoveSpeed = walkSpeed;
            _animator.SetBool(IsAir, false);
        }
        else
        {
            state = MovementState.air;
            _animator.SetBool(IsAir, true);
        }
    }
    
    protected virtual void Update()
    {
        SpeedControl();
        MovementStateHandler();

        if (_grounded)
            _rb.drag = groundDrag;
        else
            _rb.drag = 0;
    }

    protected virtual void FixedUpdate()
    {
        _rb.useGravity = !OnSlope();
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if ((whatIsGround.value & (1 << other.gameObject.layer)) != 0)
        {
            _grounded = true;
        }
    }
    
    protected virtual void OnTriggerExit(Collider other)
    {
        if ((whatIsGround.value & (1 << other.gameObject.layer)) != 0)
        {
            _grounded = false;
        }
    }
    
    private void SpeedControl()
    {
        if (OnSlope() && !_exitingSlope)
        {
            if (_rb.velocity.magnitude > _currentMoveSpeed)
                _rb.velocity = _rb.velocity.normalized * _currentMoveSpeed;
        }
        else
        {
            Vector3 flatVel = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);

            if (flatVel.magnitude > _currentMoveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * _currentMoveSpeed;
                _rb.velocity = new Vector3(limitedVel.x, _rb.velocity.y, limitedVel.z);
            }
        }
    }
    
    protected bool OnSlope()
    {
        // Debug.DrawRay(transform.position, Vector3.down * (playerHeight * 0.5f + 0.6f), Color.red);
        if (Physics.Raycast(transform.position, Vector3.down, out _slopeHit, playerHeight * 0.5f + 0.3f, whatIsGround))
        {
            float angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private void AdjustParentPositionToHipsBone()
    {
        Vector3 initHipsPosition = _hipsBone.position;
        transform.position = initHipsPosition;

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 5, whatIsGround))
            transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);

        _hipsBone.position = initHipsPosition;
    }

    private void AdjustParentRotationToHipsBone()
    {
        Vector3 initHipsPosition = _hipsBone.position;
        Quaternion initHipsRotation = _hipsBone.rotation;

        Vector3 directionForRotate = -_hipsBone.up;

        directionForRotate.y = 0;

        Quaternion correctionRotation = Quaternion.FromToRotation(transform.forward, directionForRotate);
    }
}

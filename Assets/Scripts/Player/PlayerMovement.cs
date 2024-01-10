using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : CharacterMovement
{
    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;
    
    public Transform orientation;

    private float _horizontalInput;
    private float _verticalInput;

    private Vector3 _moveDirection;

    protected override void Start()
    {
        base.Start();
        ResetJump();
    }
    protected override void MovementStateHandler()
    {
        if (_grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            _currentMoveSpeed = sprintSpeed;
        }
        else if (_grounded && Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            _currentMoveSpeed = crouchSpeed;
        }
        else if (_grounded)
        {
            state = MovementState.walking;
            _currentMoveSpeed = walkSpeed;
        }
        else
        {
            state = MovementState.air;
        }
    }

    protected override void Update()
    {
        base.Update();
        MyInput();
    }

    protected override void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");
        
        if (_horizontalInput != 0f || _verticalInput != 0f)
        {
            _animator.SetFloat(Velocity, _rb.velocity.magnitude);
            _animator.SetBool(IsWalking, true);
        }
        else
        {
            _animator.SetFloat(Velocity, 0f);
            _animator.SetBool(IsWalking, false);
        }

        if (Input.GetKey(jumpKey) && _readyToJump && _grounded)
        {
            _readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        _moveDirection = orientation.forward * _verticalInput + orientation.right * _horizontalInput;
        
        if (OnSlope() && !_exitingSlope)
        {
            _rb.AddForce(GetSlopeMoveDirection() * _currentMoveSpeed * slopeSpeed, ForceMode.Force);
            _rb.AddForce(-_slopeHit.normal * slopeForce, ForceMode.Force);
        }
        
        if (_grounded)
            _rb.AddForce(_moveDirection.normalized * _currentMoveSpeed * 10f, ForceMode.Force);
        
        else if (!_grounded)
            _rb.AddForce(_moveDirection.normalized * airMultiplier * 10f, ForceMode.Force);

        _rb.useGravity = !OnSlope();
    }

    private void Jump()
    {
        _exitingSlope = true;
            
        _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
        _rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        _readyToJump = true;

        _exitingSlope = false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(_moveDirection, _slopeHit.normal).normalized;
    }
}

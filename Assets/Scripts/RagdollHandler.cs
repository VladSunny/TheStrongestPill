using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RagdollHandler : MonoBehaviour
{
    public bool DebugRagdoll = false;
    
    private List<Rigidbody> _rigidbodies;
    private Rigidbody _playerRigidbody;
    private List<Collider> _colliders;
    private CapsuleCollider _myCollider;
    private Animator _animator;
    
    public bool IsRagdoll = false;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        _rigidbodies = new List<Rigidbody>(GetComponentsInChildren<Rigidbody>());
        _playerRigidbody = GetComponentInParent<Rigidbody>();
        _colliders = new List<Collider>(GetComponentsInChildren<Collider>());
        _myCollider = GetComponent<CapsuleCollider>();
        _animator = GetComponentInChildren<Animator>();
        Disable();
    }

    public void Enable()
    {
        _playerRigidbody.isKinematic = true;
        _myCollider.isTrigger = true;
        _animator.enabled = false;
        IsRagdoll = true;
        
        foreach (Rigidbody rigidbody in _rigidbodies)
            rigidbody.isKinematic = false;

        foreach (Collider collider in _colliders)
            collider.isTrigger = false;

    }

    public void Disable()
    {
        _playerRigidbody.isKinematic = false;
        _animator.enabled = true;
        IsRagdoll = false;
        
        foreach (Rigidbody rigidbody in _rigidbodies)
            rigidbody.isKinematic = true;
        
        foreach (Collider collider in _colliders)
            collider.isTrigger = true;

        _myCollider.isTrigger = false;
    }

    private void Update()
    {
        if (DebugRagdoll && Input.GetKeyDown(KeyCode.H))
        {
            if (!IsRagdoll)
                Enable();
            else
                Disable();
        }
    }
}

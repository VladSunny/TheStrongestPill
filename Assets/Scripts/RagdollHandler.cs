using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    [SerializeField] private CameraSwitcher _cameraSwitcher;
    
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
        _animator.enabled = false;
        
        _playerRigidbody.isKinematic = true;
        IsRagdoll = true;
        
        foreach (Rigidbody rigidbody in _rigidbodies)
            rigidbody.isKinematic = false;

        foreach (Collider collider in _colliders)
        {
            if (collider != _myCollider)
                collider.isTrigger = false;
        }

        _myCollider.enabled = false;
    }

    public void Disable()
    {
        _animator.enabled = true;
        _animator.Rebind();
        _animator.Update(0);
        
        _playerRigidbody.isKinematic = false;
        IsRagdoll = false;
        
        foreach (Rigidbody rigidbody in _rigidbodies)
            rigidbody.isKinematic = true;

        foreach (Collider collider in _colliders)
            if (collider != _myCollider)
                collider.isTrigger = true;
        
        _myCollider.enabled = true;
    }

    private void Update()
    {
        if (DebugRagdoll && Input.GetKeyDown(KeyCode.H))
        {
            if (!IsRagdoll)
            {
                _cameraSwitcher.SwitchCamera(CameraSwitcher.CameraType.DeathCamera);
                Enable();
            }
            else
            {
                _cameraSwitcher.SwitchCamera(CameraSwitcher.CameraType.BaseCamera);
                Disable();
            }
        }
    }
}

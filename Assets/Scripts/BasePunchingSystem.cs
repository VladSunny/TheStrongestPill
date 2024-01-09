using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.InputSystem;

public class BasePunchingSystem : MonoBehaviour
{
    public Animator animator;
    
    [Header("M1 punches info")]
    public List<M1PunchesInfo> m1Punches = new List<M1PunchesInfo>();
    [SerializeField, ReadOnly] private int _currentBasePunch = 0;
    public float comboTimer = 2f;
    private float _currentComboTimer;
    
    private DynamicHitBox _dynamicHitBox;
    private Rigidbody _playerRigidBody;
    private float _currentStan = 0f;

    [System.Serializable]
    public struct M1PunchesInfo
    {
        public float damage;
        public string attackTrigger;
        public float cooldown;
    }
    
    private void Awake()
    {
        _dynamicHitBox = GetComponent<DynamicHitBox>();
        _playerRigidBody = GetComponent<Rigidbody>();
        
        PlayerInputActions playerInputActions = new PlayerInputActions();
        playerInputActions.BaseAttacks.Enable();
        playerInputActions.BaseAttacks.Punch.performed += Punch;
    }

    private void Update()
    {
        if (_currentStan >= 0f)
            _currentStan -= Time.deltaTime;
        if (_currentComboTimer >= 0f)
            _currentComboTimer -= Time.deltaTime;
        else
            _currentBasePunch = 0;
    }

    public void Punch(InputAction.CallbackContext context)
    {
        if (_currentStan > 0f) return;
        
        _currentComboTimer = comboTimer;
        
        _currentBasePunch++;
        _currentBasePunch %= m1Punches.Count;
        _currentStan = m1Punches[_currentBasePunch].cooldown;
        
        animator.SetTrigger(m1Punches[_currentBasePunch].attackTrigger);
        _dynamicHitBox.CreateHitBox(
            relativeBoxPosition: Vector3.forward * 1f,
            boxSize: new Vector3(1.5f, 2f, 1.5f),
            debugDraw: true,
            actionOnHit: (collider) =>
            {
                Health healthComponent = collider.GetComponentInParent<Health>();
                Rigidbody rb = collider.GetComponentInParent<Rigidbody>();

                if (!healthComponent || !rb)
                    return;
                
                healthComponent.TakeDamage(m1Punches[_currentBasePunch].damage);
                rb.AddForce(_dynamicHitBox.characterTransform.forward * 20f, ForceMode.Impulse);
                _playerRigidBody.AddForce(_dynamicHitBox.characterTransform.forward * 20f, ForceMode.Impulse);
            });
    }
}

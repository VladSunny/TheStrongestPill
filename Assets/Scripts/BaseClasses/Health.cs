using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField, ReadOnly]
    protected float _health;
    
    [Header("Health Bar")]
    public float maxHealth = 100f;

    public float debugDamage = 5f;
    public float debugRestore = 5f;

    [Header("Keybinds")]
    [SerializeField] private bool debugInput = false;
    public KeyCode getDamageKey = KeyCode.Alpha1;
    public KeyCode restoreHealthKey = KeyCode.Alpha2;

    [Header("Animation")] 
    public string damagedTrigger = "Damaged";
    protected Animator _animator;
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        _health = maxHealth;
    }

    protected virtual void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (debugInput)
            MyInput();
        
        _health = Mathf.Clamp(_health, 0, maxHealth); 
    }

    protected virtual void MyInput()
    {
        if (Input.GetKeyDown(getDamageKey))
            TakeDamage(debugDamage);
        if (Input.GetKeyDown(restoreHealthKey))
            RestoreHealth(debugRestore);
    }

    public virtual void TakeDamage(float damage)
    {
        _health -= damage;
        if (_animator)
            _animator.SetTrigger(damagedTrigger);
    }

    public virtual void RestoreHealth(float healAmount)
    {
        _health += healAmount;
    }
}

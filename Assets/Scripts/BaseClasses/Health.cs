using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    protected static readonly int DamagedTrigger = Animator.StringToHash("Damaged");
    protected static readonly int XDamaged = Animator.StringToHash("XDamaged");
    protected static readonly int ZDamaged = Animator.StringToHash("ZDamaged");
    protected Animator _animator;

    [Header("Effects")] 
    public ParticleSystem damageParticleSystem;
    [SerializeField] private GameObject damagePopupPrefab;
    [SerializeField] private Transform damagePopupPosition;
    
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
    
    private void ShowDamage(float damage)
    {
        GameObject popup = Instantiate(damagePopupPrefab, damagePopupPosition.localPosition, Quaternion.identity);
        popup.GetComponent<DamagePopup>().Setup(damage);
        popup.transform.SetParent(transform, false);
    }

    public virtual void TakeDamage(float damage, Vector3? fromTransform = null)
    {
        
        _health -= damage;
        
        if (damageParticleSystem)
        {
            if (fromTransform.HasValue)
                damageParticleSystem.transform.LookAt((Vector3)fromTransform);
            else
                damageParticleSystem.transform.forward = damageParticleSystem.transform.parent.forward;
            
            damageParticleSystem.Play();
        }
        
        if (damagePopupPrefab && damagePopupPosition != null)
            ShowDamage(damage);

        if (fromTransform.HasValue)
        {
            Vector3 direction = (Vector3) fromTransform - transform.position;
            Vector2 directionXZ = new Vector2(direction.x, direction.z);
            Vector2 normalizedDirection = directionXZ.normalized;
            
            _animator.SetFloat(XDamaged, normalizedDirection.x);
            _animator.SetFloat(ZDamaged, normalizedDirection.y);
            
            // Debug.DrawLine(transform.position, (Vector3)fromTransform, Color.green, 5f);
        }
        
        if (_animator)
            _animator.SetTrigger(DamagedTrigger);
    }

    public virtual void RestoreHealth(float healAmount)
    {
        _health += healAmount;
    }
}

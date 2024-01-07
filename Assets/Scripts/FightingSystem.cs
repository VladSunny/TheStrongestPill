using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FightingSystem : MonoBehaviour
{
    [Header("M1 punches")]
    public float m1damage = 5f;
    
    private DynamicHitBox _dynamicHitBox;

    private void Awake()
    {
        _dynamicHitBox = GetComponent<DynamicHitBox>();
        
    }

    public void Punch()
    {
        _dynamicHitBox.CreateHitBox(
            relativeBoxPosition: Vector3.forward * 1f,
            boxSize: new Vector3(1f, 2f, 1f),
            debugDraw: true,
            actionOnHit: (collider) =>
            {
                Health healthComponent = collider.GetComponentInParent<Health>();
                Rigidbody rb = collider.GetComponentInParent<Rigidbody>();

                if (!healthComponent || !rb)
                    return;
                
                healthComponent.TakeDamage(m1damage);
                rb.AddForce(_dynamicHitBox.characterTransform.forward * 20f, ForceMode.Impulse);
            });
    }
}

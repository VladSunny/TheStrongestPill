using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Serialization;

public class DummyHealth : Health
{
    [Header("Health Bar")]
    public float chipSpeed = 2f;
    private float _lerpTimer;

    public float displayDistance = 20f;
    public Image frontHealthBar;
    public Image backHealthBar;
    public Canvas dummyCanvas;
    
    private Transform cameraTransform;
    
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        
        float distanceToCamera = Vector3.Distance(transform.position, cameraTransform.position);
        
        dummyCanvas.gameObject.SetActive(distanceToCamera <= displayDistance);
        
        if (dummyCanvas.gameObject.activeSelf)
            UpdateHealthUI();
    }

    private void LateUpdate()
    {
        if (dummyCanvas.gameObject.activeSelf)
            dummyCanvas.transform.LookAt(dummyCanvas.transform.position + cameraTransform.forward);
    }

    private void UpdateHealthUI()
    {
        // Debug.Log(_health);
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFraction = _health / maxHealth;

        if (fillB > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            _lerpTimer += Time.deltaTime;
            
            float percentComplete = _lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }

        if (fillF < hFraction)
        {
            backHealthBar.color = Color.green;
            backHealthBar.fillAmount = hFraction;
            _lerpTimer += Time.deltaTime;
            
            float percentComplete = _lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;

            frontHealthBar.fillAmount = Mathf.Lerp(fillF, backHealthBar.fillAmount, percentComplete);
        }
    }
    

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        
        _lerpTimer = 0f;
    }

    public override void RestoreHealth(float healAmount)
    {
        base.RestoreHealth(healAmount);
        
        _health += healAmount;
        _lerpTimer = 0f;
    }
}

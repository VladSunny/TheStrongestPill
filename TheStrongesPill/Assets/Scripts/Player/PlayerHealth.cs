using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerHealth : Health
{
    [Header("Health Bar")]
    public float chipSpeed = 2f;
    private float _lerpTimer;
    
    public Image frontHealthBar;
    public Image backHealthBar;
    
    [Header("Damage Overlay")] 
    public Image overlay = null;
    public float duration;
    public float fadeSpeed;
    public float maxAlpha = 0.5f;
    
    
    private float _durationTimer;
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        
        if (overlay != null)
            overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        
        UpdateHealthUI();
        UpdateDamageOverlayUI();
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

    private void UpdateDamageOverlayUI()
    {
        if (overlay == null)
            return;
        if (overlay.color.a > 0)
        {
            if (_health < 30)
                return;
            
            _durationTimer += Time.deltaTime;
            if (_durationTimer > duration)
            {
                float tempAlpha = overlay.color.a;
                tempAlpha -= Time.deltaTime * fadeSpeed;
                overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, tempAlpha);
            }
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        
        _lerpTimer = 0f;
        _durationTimer = 0f;
        
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, maxAlpha);
    }

    public override void RestoreHealth(float healAmount)
    {
        base.RestoreHealth(healAmount);
        
        _health += healAmount;
        _lerpTimer = 0f;
    }
}

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random; // Используйте UnityEngine.UI для работы с Text
// Или, если используете TextMeshPro
// using TMPro;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] private float lifeTime = 1f;
    [SerializeField] private Vector3 randomizePositionFrom = new Vector3(1f, 1f, 0);
    [SerializeField] private Vector3 randomizePositionTo = new Vector3(1f, 1f, 0);
    
    private TextMeshProUGUI damageText;
    private float disappearTimer;

    private void OnEnable()
    {
        if (Camera.main != null)
        {
            Vector3 cameraRight = Camera.main.transform.right;
            Vector3 cameraUp = Camera.main.transform.up;
            Vector3 cameraForward = Camera.main.transform.forward;
            
            cameraForward.y = 0;
            
            transform.position += cameraRight * Random.Range(randomizePositionFrom.x, randomizePositionTo.x) 
                                  + cameraUp * Random.Range(randomizePositionFrom.y, randomizePositionTo.y)
                                  + cameraForward * Random.Range(randomizePositionFrom.z, randomizePositionTo.z);
        }

        disappearTimer = lifeTime;
    }

    public void Setup(float damageAmount)
    {
        damageText = GetComponentInChildren<TextMeshProUGUI>();
        damageText.text = "-" + damageAmount.ToString();
    }

    private void Update()
    {
        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            Destroy(gameObject);
        }
    }
}

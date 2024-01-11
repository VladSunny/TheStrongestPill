using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random; // Используйте UnityEngine.UI для работы с Text
// Или, если используете TextMeshPro
// using TMPro;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] private float disappearTimer = 1f;
    [SerializeField] private Vector3 randomizePositionFrom = new Vector3(1f, 1f, 0);
    [SerializeField] private Vector3 randomizePositionTo = new Vector3(1f, 1f, 0);
    private TextMeshProUGUI damageText;


    private void Start()
    {
        // Setup(100);
    }

    private void OnEnable()
    {
        // transform.localPosition += new Vector3(
        //     Random.Range(randomizePositionFrom.x, randomizePositionTo.x),
        //     Random.Range(randomizePositionFrom.y, randomizePositionTo.y),
        //     Random.Range(randomizePositionFrom.z, randomizePositionTo.z)
        // );
        
        if (Camera.main != null)
        {
            // Получаем направления камеры
            Vector3 cameraRight = Camera.main.transform.right;
            Vector3 cameraUp = Camera.main.transform.up;
            Vector3 cameraForward = Camera.main.transform.forward;

            // Удаляем компонент y, если не хотим, чтобы текст перемещался вверх и вниз вдоль направления взгляда камеры
            cameraForward.y = 0;

            // Рандомизируем смещение вдоль векторов право, вверх и вперед камеры
            transform.position += cameraRight * Random.Range(randomizePositionFrom.x, randomizePositionTo.x) 
                                  + cameraUp * Random.Range(randomizePositionFrom.y, randomizePositionTo.y)
                                  + cameraForward * Random.Range(randomizePositionFrom.z, randomizePositionTo.z);
        }
    }

    public void Setup(float damageAmount)
    {
        damageText = GetComponentInChildren<TextMeshProUGUI>();
        damageText.text = damageAmount.ToString();
    }

    private void Update()
    {
        if (Camera.main != null)
        {
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha6))
            Setup(Random.Range(0, 999));
        
        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            Destroy(gameObject);
        }
        else
        {
            
        }
    }
}

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FallDamage : MonoBehaviour
{
    [Header("GroundCheck")]
    public LayerMask whatIsGround;
    
    public float safeFallDistance = 5f; // безопасное расстояние падения, не причиняет урона
    public float maxFallDistance = 20f; // максимальное расстояние падения, после которого урон будет максимален
    public int maxDamage = 50; // максимальный урон от падения

    private Vector3 lastGroundedPosition;
    private bool isFalling = false;
    private Health healthComponent; // ссылка на скрипт здоровья

    void Start()
    {
        healthComponent = GetComponent<Health>();
        lastGroundedPosition = transform.position;
    }

    void Update()
    {
        Debug.Log(isGrounded());
        // Проверяем, касаемся ли мы земли
        if (isGrounded())
        {
            if (isFalling)
            {
                // Вычисляем урон от падения на основе разницы в высоте
                float fallDistance = lastGroundedPosition.y - transform.position.y;
                if (fallDistance > safeFallDistance)
                {
                    Debug.Log("Fall Damage");
                    float damagePercent = Mathf.Clamp01((fallDistance - safeFallDistance) / (maxFallDistance - safeFallDistance));
                    int damage = Mathf.RoundToInt(damagePercent * maxDamage);
                    healthComponent.TakeDamage(damage);
                }

                isFalling = false;
            }

            // Обновляем последнюю позицию, когда были на земле
            lastGroundedPosition = transform.position;
        }
        else
        {
            isFalling = true;
        }
    }

    private bool isGrounded()
    {
        // Метод для проверки, находится ли персонаж на земле
        // Реализация может быть простой проверкой коллизии или более сложным образом, например, с использованием raycast
        Debug.DrawRay(transform.position, -Vector3.up * 1.3f, Color.green);
        return Physics.Raycast(transform.position, -Vector3.up, 1.3f, whatIsGround);
    }
}
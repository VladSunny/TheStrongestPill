using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FallDamage : MonoBehaviour
{
    [Header("GroundCheck")]
    public LayerMask whatIsGround;
    
    public float safeFallDistance = 5f;
    public float maxFallDistance = 20f;
    public int maxDamage = 50;

    private Vector3 lastGroundedPosition;
    private bool isFalling = false;
    private Health healthComponent;

    void Start()
    {
        healthComponent = GetComponent<Health>();
        lastGroundedPosition = transform.position;
    }

    void Update()
    {
        
        if (isGrounded())
        {
            if (isFalling)
            {
                
                float fallDistance = lastGroundedPosition.y - transform.position.y;
                if (fallDistance > safeFallDistance)
                {
                    float damagePercent = Mathf.Clamp01((fallDistance - safeFallDistance) / (maxFallDistance - safeFallDistance));
                    int damage = Mathf.RoundToInt(damagePercent * maxDamage);
                    healthComponent.TakeDamage(damage);
                }

                isFalling = false;
            }

            
            lastGroundedPosition = transform.position;
        }
        else
        {
            isFalling = true;
        }
    }

    private bool isGrounded()
    {
        // Debug.DrawRay(transform.position, -Vector3.up * 1.3f, Color.green);
        return Physics.Raycast(transform.position, -Vector3.up, 1.3f, whatIsGround);
    }
}
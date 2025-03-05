using UnityEngine;

public class Gravity : MonoBehaviour
{
    [SerializeField] private float gravityScale = 2f;
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody rb;
    private bool isGrounded;

    public bool OnGround => isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        isGrounded = CheckGrounded();
        
    }
    void FixedUpdate()
    {
        ApplyGravity();
    }

    void ApplyGravity()
    {
        if (!isGrounded)
        {
            rb.AddForce(Physics.gravity * gravityScale, ForceMode.Acceleration);
        }
    }

    bool CheckGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
    }
}

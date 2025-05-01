using UnityEngine;

public class Gravity : MonoBehaviour
{
    [SerializeField] private float gravityScale = 2f;
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody rb;
    private bool isGrounded;
    private Vector3 gravity = new Vector3(0, -9.8f, 0);
    public bool OnGround => isGrounded;
    private void Awake()
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
            rb?.AddForce(gravity * gravityScale, ForceMode.Acceleration);
        }
    }

    bool CheckGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
    }

    public void CreateRigidBody()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
        }
    }
}

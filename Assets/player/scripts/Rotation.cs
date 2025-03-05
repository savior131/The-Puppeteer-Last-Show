using UnityEngine;

public class Rotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float threshold = 0.1f;

    private Rigidbody rb;
    private Vector3 lastDirection;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        RotatePlayer();
    }

    void RotatePlayer()
    {
        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

        if (horizontalVelocity.magnitude > threshold)
        {
            lastDirection = horizontalVelocity.normalized;
        }

        if (lastDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lastDirection);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}

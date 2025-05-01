using UnityEngine;

public class Rotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float threshold = 0.1f;

    private Rigidbody rb;
    private Vector3 lastDirection;
    private AnimationLock animationLock;
    private TargetDetector detector;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animationLock = GetComponent<AnimationLock>();
        detector = GetComponent<TargetDetector>();
    }

    void Update()
    {
        UpdateLastDirection();
        RotatePlayer();
    }

    void UpdateLastDirection()
    {
        if (!animationLock.canRotate && detector.ClosestTarget != null)
        {
            Vector3 directionToTarget = detector.ClosestTarget.position - transform.position;
            directionToTarget.y = 0;
            if (directionToTarget.sqrMagnitude > threshold)
            {
                lastDirection = directionToTarget.normalized;
            }
        }
        else
        {
            Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            if (horizontalVelocity.magnitude > threshold)
            {
                lastDirection = horizontalVelocity.normalized;
            }
        }
       
    }

    void RotatePlayer()
    {
        if (lastDirection.sqrMagnitude > threshold)
        {
            RotateTowardsTarget(transform.position + lastDirection);
        }
    }

    void RotateTowardsTarget(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0;
        if (direction.sqrMagnitude > threshold)
        {
            direction.Normalize();
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}

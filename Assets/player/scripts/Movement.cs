using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float moveForce = 25f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float sprintMultiplier = 1.5f;
    [SerializeField] private float jumpForce = 20f;
    [SerializeField] private float decelerationRate = 10f;
    [SerializeField] private float movingDecelerationRate = 2f;
    [SerializeField] private float sprintLerpSpeed = 5f;
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem speedUpParticles;
    [SerializeField] private Health health;
    [SerializeField] private bool flipMovement= false;

    private Rigidbody rb;
    private Gravity gravity;
    private Stamina stamina;
    private AnimationLock animationLock;
    private Vector3 moveInput;
    private bool isSprinting;
    private float moveX, moveZ;
    private float currentSprintFactor = 1f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        gravity = GetComponent<Gravity>();
        stamina = GetComponent<Stamina>();
        animationLock = GetComponent<AnimationLock>();
    }

    void Update()
    {
        if (health.isDead) {
            rb.linearVelocity = new Vector3(0,rb.linearVelocity.y,0);
            return;
                }
        moveX = Input.GetAxisRaw("Horizontal");
        moveZ = Input.GetAxisRaw("Vertical");
        if (flipMovement)
        {
            moveX = -moveX;
            moveZ = -moveZ;
        }
        moveInput = new Vector3(moveX, 0, moveZ).normalized;

        isSprinting = Input.GetKey(KeyCode.LeftShift) && !stamina.IsDrained;
        float targetSprintFactor = isSprinting ? sprintMultiplier : 1f;
        currentSprintFactor = Mathf.Lerp(currentSprintFactor, targetSprintFactor, Time.deltaTime * sprintLerpSpeed);

        if (isSprinting && moveInput.sqrMagnitude > 0.1f)
        {
            if (!speedUpParticles.isPlaying)
                speedUpParticles.Play();
        }
        else if (speedUpParticles.isPlaying)
        {
            speedUpParticles.Stop();
        }
        if (Input.GetKeyDown(KeyCode.Space) && gravity.OnGround && (animationLock == null || animationLock.canMove))
        {
            Jump();
            stamina.DrainChunk(5);
        }


        animator.SetBool("isGrounded", gravity.OnGround);
        float speed = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).magnitude;
        float normalizedSpeed = Mathf.Clamp01(speed / (maxSpeed * sprintMultiplier));
        animator.SetFloat("horizontalVelocity", normalizedSpeed);
    }

    void FixedUpdate()
    {
        HandleMovement();
        ApplyDeceleration();
    }

    void HandleMovement()
    {
        if (animationLock != null && !animationLock.canMove)
        {
            stamina.gradualDraining(false, Time.deltaTime);
            return;
        }
        stamina.gradualDraining(isSprinting && moveInput.sqrMagnitude > 0.1f, Time.deltaTime);

        if (moveInput.sqrMagnitude > 0)
        {
            Vector3 force = moveInput * (moveForce * currentSprintFactor);
            rb.AddForce(force, ForceMode.Force);
        }

        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        float currentMaxSpeed = maxSpeed * currentSprintFactor;

        if (horizontalVelocity.magnitude > currentMaxSpeed)
        {
            Vector3 limitedVelocity = horizontalVelocity.normalized * currentMaxSpeed;
            Vector3 velocityChange = limitedVelocity - horizontalVelocity;
            rb.AddForce(velocityChange, ForceMode.VelocityChange);
        }
    }

    void ApplyDeceleration()
    {
        if (!gravity.OnGround) return;

        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

        bool isTryingToMove = moveInput.sqrMagnitude > 0.1f;
        bool canMove = animationLock == null || animationLock.canMove;

        float deceleration = (!canMove || !isTryingToMove) ? decelerationRate : movingDecelerationRate;

        Vector3 decelerationForce = -horizontalVelocity.normalized * deceleration;
        rb.AddForce(decelerationForce, ForceMode.Force);

        if (horizontalVelocity.magnitude < 0.05f)
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        }
    }


    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}

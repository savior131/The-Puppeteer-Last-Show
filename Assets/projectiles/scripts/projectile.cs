using UnityEngine;

public class Projectile : MonoBehaviour, IParriable
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float lifeTime = 10f;
    [SerializeField] private bool enableUpwardForce = false;
    [SerializeField] private float upwardForce = 5f;
    [SerializeField] private bool enableWave = false;
    [SerializeField] private float waveFrequency = 5f;
    [SerializeField] private float waveAmplitude = 1f;
    [SerializeField] private bool freezeRotationXZ = true;
    [SerializeField] private float waveDelay = 0.5f;
    [SerializeField] private float bulletDamage = 5.0f;
    private Transform returnTarget;

    private float phaseOffset;
    private bool waveActive;
    private Rigidbody rb;
    private float startTime;
    private Vector3 initialDirection;
    public float damage => bulletDamage;
    
   

    void Awake()
    {
        startTime = Time.time;
        phaseOffset = Random.Range(0f, Mathf.PI * 2);

        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.useGravity = false;
        if (freezeRotationXZ)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
        }
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    void Start()
    {
        initialDirection = transform.forward;
        Vector3 initialVelocity = initialDirection * speed;

        if (enableUpwardForce)
        {
            initialVelocity += Vector3.up * upwardForce;
        }

        rb.linearVelocity = initialVelocity;
        Destroy(gameObject, lifeTime);
    }

    void FixedUpdate()
    {
        if (enableWave && Time.time >= startTime + waveDelay)
        {
            waveActive = true;
        }

        if (enableWave && waveActive)
        {
            float timeSinceStart = Time.time - startTime - waveDelay;
            float waveOffset = Mathf.Sin(timeSinceStart * waveFrequency + phaseOffset) * waveAmplitude;
            rb.linearVelocity = (initialDirection * speed) + (transform.right * waveOffset);
        }
    }
    public void OnParried()
    {
        if (returnTarget != null)
        {
            Vector3 directionToTarget = (returnTarget.position - transform.position).normalized;
            rb.linearVelocity = directionToTarget * speed;
            transform.forward = directionToTarget;
        }
        else
        {
            Vector3 oppositeDirection = -rb.linearVelocity.normalized;
            rb.linearVelocity = oppositeDirection * speed;
            transform.forward = oppositeDirection;
        }
    }
    public void setReturnTarget(Transform target)
    {
        returnTarget = target;
    } 
}

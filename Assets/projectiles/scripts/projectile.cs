using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour, IParriable
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 20f;
    [SerializeField] private float afterParrySpeed = 25f;
    [SerializeField] private float lifeTime = 10f;
    [SerializeField] private bool freezeRotationXZ = true;

    [Header("Upward Force")]
    [SerializeField] private bool enableUpwardForce = false;
    [SerializeField] private float upwardForce = 5f;

    [Header("Wave Settings")]
    [SerializeField] private bool enableWave = false;
    [SerializeField] private float waveFrequency = 5f;
    [SerializeField] private float waveAmplitude = 1f;
    [SerializeField] private float waveDelay = 0.5f;

    [Header("Tracking Settings")]
    [SerializeField] private bool enableTracking = false;
    [SerializeField] private bool followPlayer = false;
    [SerializeField] private float trackingDelay = 1f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float trackingRange = 100f;

    [Header("Combat Settings")]
    [SerializeField] private float bulletDamage = 5.0f;
    [SerializeField] private float maxParryRandomAngle = 4f;

    private Transform returnTarget;
    private Gravity gravity;
    private Rigidbody rb;
    private Transform player;
    private Vector3 initialDirection;
    private float startTime;
    private float phaseOffset;
    private bool waveActive;
    private bool isParried = false;
    private bool hasTracked = false;

    public float damage => bulletDamage;

    void Awake()
    {
        gravity = GetComponent<Gravity>();
        rb = GetComponent<Rigidbody>();
        if(rb == null){
            rb=gameObject.AddComponent<Rigidbody>();
        }
        rb.useGravity = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        if (freezeRotationXZ)
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        startTime = Time.time;
        phaseOffset = Random.Range(0f, Mathf.PI * 2);
        if (enableTracking || followPlayer)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, trackingRange, playerLayer);
            if (hits.Length > 0)
                player = hits[0].transform;
        }
    }

    void Start()
    {
        initialDirection = transform.forward;
        Vector3 initialVelocity = initialDirection * speed;

        if (enableUpwardForce)
            initialVelocity += Vector3.up * upwardForce;

        rb.AddForce(initialVelocity, ForceMode.VelocityChange);
        Destroy(gameObject, lifeTime);
    }

    void FixedUpdate()
    {
        if (enableWave && Time.time >= startTime + waveDelay && !isParried)
            waveActive = true;

        if (waveActive && !isParried)
        {
            float timeSinceWaveStart = Time.time - startTime - waveDelay;
            float waveOffset = Mathf.Sin(timeSinceWaveStart * waveFrequency + phaseOffset) * waveAmplitude;
            Vector3 waveForce = transform.right * waveOffset;
            rb.AddForce(waveForce, ForceMode.Acceleration);
        }

        if (enableTracking && !hasTracked && !followPlayer && !isParried && Time.time >= startTime + trackingDelay && player != null)
        {
            if (gravity != null) gravity.enabled = false;
            Vector3 offset = new Vector3(
    Random.Range(-2f, 2f),
    0,
    Random.Range(-2f, 2f)
);
            Vector3 dir = ((player.position + offset) - transform.position).normalized;
            rb.linearVelocity = dir * speed;
            transform.forward = dir;
            hasTracked = true;
        }

        if (followPlayer && !isParried && player != null)
        {
            if (gravity != null) gravity.enabled = false;

            Vector3 direction = (player.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 5f);
            rb.linearVelocity = transform.forward * speed;
        }
    }

    public void OnParried()
    {
        isParried = true;

        if (gravity != null) gravity.enabled = false;

        Vector3 targetDir = returnTarget != null
            ? (returnTarget.position - transform.position).normalized
            : -transform.forward;

        float randomAngle = Random.Range(-maxParryRandomAngle, maxParryRandomAngle);
        Quaternion randomRot = Quaternion.AngleAxis(randomAngle, Vector3.up);
        Vector3 randomizedDir = randomRot * targetDir;

        rb.linearVelocity = randomizedDir * Mathf.Max(speed, afterParrySpeed);
        transform.forward = randomizedDir;
    }

    public void setReturnTarget(Transform target)
    {
        returnTarget = target;
    }
}

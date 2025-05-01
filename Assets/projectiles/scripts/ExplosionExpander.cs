using UnityEngine;

public class ExplosionExpander : MonoBehaviour
{
    private float maxRadius;
    private float duration;
    private float explotionDamage;

    private float timer = 0f;
    private SphereCollider sphereCollider;

    public float damage => explotionDamage;

    public void Init(float maxRadius, float duration, float damage)
    {
        this.maxRadius = maxRadius;
        this.duration = duration;
        this.explotionDamage = damage;
    }

    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        transform.localScale = Vector3.zero;
    }

    void Update()
    {
        timer += Time.deltaTime;
        float progress = Mathf.Clamp01(timer / duration);
        float currentScale = maxRadius * 2f * progress;
        transform.localScale = new Vector3(currentScale, currentScale, currentScale);

        if (timer >= duration)
        {
            Destroy(gameObject);
        }
    }
}

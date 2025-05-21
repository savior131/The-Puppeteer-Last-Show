using UnityEngine;

public class TargetDetector : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private LayerMask detectionLayer;
    [SerializeField] private Health HealthScript;

    public Transform ClosestTarget { get; private set; }

    private void Update()
    {
        if(HealthScript.isDead) return;
        UpdateClosestTarget();
    }
    public void UpdateClosestTarget()
    {
        ClosestTarget = null;
        float minDistance = detectionRadius;
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, detectionLayer);
        foreach (Collider col in colliders)
        {

                float distance = Vector3.Distance(transform.position, col.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    ClosestTarget = col.transform;
                }
            
        }
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}

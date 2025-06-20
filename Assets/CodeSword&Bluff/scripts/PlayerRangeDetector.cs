using UnityEngine;

public class PlayerRangeDetector : MonoBehaviour
{
    public bool isPlayerInChaseRange { get; set; }
    public bool isPlayerInAttackRange { get; set; }

    [Header("Ranges")]
    public float chaseRange = 8f;
    public float attackRange = 2f;

    public void UpdateRangeStatus(float distanceToPlayer)
    {
        //Debug.Log(distanceToPlayer);
        isPlayerInChaseRange = (distanceToPlayer <= chaseRange) && (distanceToPlayer > attackRange);
        isPlayerInAttackRange = distanceToPlayer <= attackRange;
    }

}

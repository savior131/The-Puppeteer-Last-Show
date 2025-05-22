//using UnityEngine;

//public class RangeBehavior : MonoBehaviour
//{

//    public enum RangeType {CHASE, ATTACK}
//    [SerializeField] private RangeType rangeType;

//    [SerializeField] float chaseRange = 10f;
//    [SerializeField] float attackRange = 2f;

//    PlayerRangeDetector detector;

//    private void Start()
//    {
//        detector = GetComponentInParent<PlayerRangeDetector>();
//        if(rangeType == RangeType.CHASE)
//        {
//            gameObject.GetComponent<SphereCollider>().radius = chaseRange;
//        }
//        else
//        {
//            gameObject.GetComponent<SphereCollider>().radius = attackRange;

//        }
//        gameObject.GetComponent<SphereCollider>().isTrigger = true;

//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        Debug.Log("trigger");
//        if (other.CompareTag("Player"))
//        {
//            Debug.Log("IS PLAYER");
//            if (rangeType == RangeType.CHASE)
//            {
//                detector.SetChaseRangeStatus(true);
//                detector.SetAttackRangeStatus(false);
//            }
//            else
//            {
//                detector.SetChaseRangeStatus(false);
//                detector.SetAttackRangeStatus(true);
//            }

//        }
//    }


//    private void OnTriggerExit(Collider other)
//    {
//        if (other.CompareTag("Player"))
//        {
//            if (rangeType == RangeType.CHASE)
//                detector.SetChaseRangeStatus(false);
//            else
//            {
//                detector.SetAttackRangeStatus(false);
//                detector.SetChaseRangeStatus(true);
//            }
//        }
        
//    }
//}

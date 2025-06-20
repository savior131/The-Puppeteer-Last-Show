using UnityEngine;

public class SwordDamage : MonoBehaviour, IDamagingObject
{
    float damage = 5f;

    public float Damage => damage;
}

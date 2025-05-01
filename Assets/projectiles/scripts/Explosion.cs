using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float maxRadius = 2f;
    [SerializeField] private float expandDuration = 0.1f;
    [SerializeField] private float explosionDamage = 10f;


    public void TriggerExplosion()
    {
        GameObject explosion = new GameObject("Explosion");

        explosion.tag = "Explosion";
        explosion.layer = LayerMask.NameToLayer("Attacks");

        explosion.transform.position = transform.position;
        explosion.transform.localScale = Vector3.zero;

        SphereCollider collider = explosion.AddComponent<SphereCollider>();
        collider.isTrigger = true;

        ExplosionExpander expander = explosion.AddComponent<ExplosionExpander>();
        expander.Init(maxRadius, expandDuration, explosionDamage);
    }
}

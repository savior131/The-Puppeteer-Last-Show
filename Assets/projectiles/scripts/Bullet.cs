using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosionEffect;
    [SerializeField] private float destroyDelay = 1f;
    
    private Explosion explosionScript;
    private MeshRenderer meshRenderer;
    private Collider bulletCollider;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        bulletCollider = GetComponent<Collider>();
        explosionScript = GetComponent<Explosion>();
    }

    void OnTriggerEnter(Collider other)
    {
        DestroyProjectile();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (explosionScript != null)
        {
            explosionScript.TriggerExplosion();
            DestroyProjectile();
        }
        else
        {
            DestroyProjectile();
        }
    }

    public void DestroyProjectile()
    {
        meshRenderer.enabled = false;
        bulletCollider.enabled = false;
        if (explosionEffect != null)
        {
            
            explosionEffect.transform.SetParent(null); 
            explosionEffect.Play();
        }
        Destroy(gameObject, destroyDelay);

    }
}
